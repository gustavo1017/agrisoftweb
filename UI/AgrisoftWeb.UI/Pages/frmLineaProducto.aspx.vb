Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmLineaProducto
    Inherits BasePage

    Dim Openform As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            Dim strAction As String = Request.QueryString("Action")

            Select Case UCase(strAction)
                Case "NEW"
                    Call clearTextBox()
                    Session.Add("frmLineaProducto_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5007
                    End If

                    Session.Add("frmLineaProducto_Action", "EDIT")
            End Select

            hdnStr5008.Value = Resource1.str5008
        End If
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        If Validar() Then
            Dim strAction As String = Request.QueryString("Action")
            Dim strModulo As String = Resource1.str5004

            dvMessage.Visible = True

            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed(txtId.Text.Trim, descripcion.Text.Trim, txtComponente.Text.Trim, strModulo) Then
                        Select Case UCase(Openform)
                            Case "FRMZONATRABAJO", "FRMPRESUPUESTOS"
                                'STRID = id.Text
                            Case Else
                                'El formulario padre se refrescará en el lado del cliente.
                                'frmLineaProducto_view.Refrescar()
                        End Select
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5004
                    End If
                Case "EDIT"
                    If Edit(txtId.Text.Trim, descripcion.Text.Trim, txtComponente.Text.Trim) Then
                        'El formulario padre se refrescará en el lado del cliente.
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5005
                    End If
            End Select
        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub setCaptionsLabels()
        lblCodigo.Text = Resource1.str5002
        lblDescripcion.Text = Resource1.str5003
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        btnCancelar.Text = Resource1.str5
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
    End Sub

    Private Function getRecord() As Boolean
        Dim Codigo As String = ""
        Dim blGetRecord As Boolean = False

        If Session("frmLineaProducto_Codigo") IsNot Nothing Then
            Codigo = Session("frmLineaProducto_Codigo")
        Else
            blGetRecord = False
        End If

        Dim rs As New ADODB.Recordset
        Dim ssql As String = " SELECT * FROM LINEASPRODUCTOS  where id_linea='" & Codigo & "'"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        rs = DBconn.Execute(ssql)

        If Not rs.EOF Then
            txtId.Text = rs.Fields("id_linea").Value
            descripcion.Text = rs.Fields("descripcion").Value
            txtId.Enabled = False
            blGetRecord = True
        Else
            blGetRecord = False
        End If

        rs.Close()
        rs = Nothing

        Return blGetRecord
    End Function

    Function Validar() As Boolean
        Dim rs As New ADODB.Recordset
        Dim strAction As String = Request.QueryString("Action")
        Dim DBconn As New ADODB.Connection
        Dim ssql As String = String.Format("SELECT DESCRIPCION From dbo.LINEASPRODUCTOS (NOLOCK) WHERE DESCRIPCION = '{0}' AND id_linea <> '{1}'", descripcion.Text.Trim(), txtId.Text.Trim())
        Dim objBL As New GenericMethods("Fundo0")

        DBconn.Open(objBL.GetSQLConnection())
        rs.let_ActiveConnection(DBConn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        If Not rs.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str99999971
            rs.Close()
            Return False
        End If

        rs.Close()
        'End If

        txtId.Text = txtId.Text.ToUpper()
        descripcion.Text = descripcion.Text.ToUpper()
        Return True
    End Function

    Public Function Add_Renamed(ByVal strId As String, ByVal strDescripcion As String, ByVal strComponente As String, ByVal strModulo As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "insert into LINEASPRODUCTOS (id_linea,descripcion) "
            ssql = ssql & "values('" & strId & "',ltrim(rtrim('" & strDescripcion & "')));"
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"
            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & strModulo & "',getdate())"
            DBconn.Execute(ssql)

            boolResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBConn.ConnectionString)

            objBL.RegisterEvent(dctException)
            boolResult = False
        End Try

        Return boolResult
    End Function

    Public Function Edit(ByVal strId As String, ByVal strDescripcion As String, ByVal strComponente As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "update LINEASPRODUCTOS set "
            ssql = ssql & "descripcion=ltrim(rtrim('" & strDescripcion & "')) "
            ssql = ssql & "where id_linea='" & strId & "';"
            DBconn.Execute(ssql)
            boolResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBConn.ConnectionString)

            objBL.RegisterEvent(dctException)
            boolResult = False
        End Try

        Return boolResult
    End Function
End Class