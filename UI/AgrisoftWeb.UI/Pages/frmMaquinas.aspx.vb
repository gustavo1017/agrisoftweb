Imports AgrisoftWeb.UI.Resources
Imports AgrisoftWeb.BL
Imports System.Globalization

Public Class frmMaquinas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()

        setCaptionsLabels()
        setCaptionsButtons()

        Dim strAction As String = Request.QueryString("Action")
        Dim Openform As String = Session("FrmMaquinas_OpenForm")

        If Not Page.IsPostBack() Then
            Select Case UCase(Openform)
                Case "FRMCOSTOSMAQUINARIA"
                    rbtnTipo0.Checked = True
                    rbtnTipo1.Visible = False
                Case "FRMCOSTOSRIEGO"
                    rbtnTipo1.Checked = True
                    rbtnTipo0.Visible = False
                Case Else
                    rbtnTipo0.Visible = True
                    rbtnTipo1.Visible = True
            End Select

            Select Case UCase(strAction)
                Case "NEW"
                    clearTextBox()
                    Session.Add("frmMaquinas_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str409
                    End If

                    Session.Add("frmMaquinas_Action", "EDIT")
            End Select
        End If

        hdnStr410.Value = Resource1.str410
        hdnStr411.Value = Resource1.str411
        hdnStr528.Value = Resource1.str528
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        Dim STRID As String = ""
        Dim strAction As String = Request.QueryString("Action")
        Dim Openform As String = Session("frmMaquinas_Openform")

        If Validar() Then
            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed() Then
                        Select Case UCase(Openform)
                            Case "FRMCOSTOSPERSONALAGROINCA", "FRMCOSTOSMAQUINARIA", "FRMCOSTOSRIEGO", "FRMTESORERIACAJA", "FRMTESORERIACAJA2"
                                STRID = txtId.Text
                            Case Else
                                'frmMaquinas_view.Refrescar()
                        End Select
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5004
                    End If
                Case "EDIT"
                    If Edit() Then
                        'frmMaquinas_view.Refrescar()
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5004
                    End If
            End Select

            Openform = ""
            Session.Add("FrmMaquinas_STRID", STRID)
            Session.Add("FrmMaquinass_OpenForm", Openform)
        End If
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
    End Sub

    Private Sub setCaptionsLabels()
        Title = Resource1.str401
        lblCodigo.Text = Resource1.str402
        lblDescripcion.Text = Resource1.str403
        rbtnTipo0.Text = Resource1.str404
        rbtnTipo1.Text = Resource1.str405
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        'btnCancelar.Text = My.Resources.Resource1.Resource1.str5
    End Sub

    Function getRecord() As Boolean
        Dim bResult As Boolean = False
        Dim rs As New ADODB.Recordset
        Dim codigo As String = Session("frmMaquinas_Codigo")
        Dim ssql As String = " SELECT * FROM MAQUINAS  where id_maquinaria='" & codigo & "'"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        rs = DBconn.Execute(ssql)

        If Not rs.EOF Then
            txtId.Text = rs.Fields("id_maquinaria").Value
            descripcion.Text = rs.Fields("descripcion").Value
            costo_unitario_standar.Text = IIf(rs.Fields("costo").Value > 0, Convert.ToDecimal(rs.Fields("costo").Value).ToString("0.00", CultureInfo.InvariantCulture), "")

            If UCase(rs.Fields("tipo").Value) = "M" Then 'MAQUINA
                rbtnTipo0.Checked = True
            Else
                rbtnTipo1.Checked = True 'BOMBA
            End If

            txtId.Enabled = False
            bResult = True
        Else
            bResult = False
        End If

        rs.Close()
        rs = Nothing

        Return bResult
    End Function

    Function Add_Renamed() As Boolean
        Dim boolResult As Boolean = False
        Dim t As String

        If rbtnTipo0.Checked = True Then
            t = "M" 'MAQUINA
        Else
            t = "B" 'BOMBA
        End If

        Dim ssql As String = "insert into MAQUINAS (id_maquinaria,descripcion,costo,tipo,energia,caudaL) "
        ssql = ssql & "values('" & txtId.Text & "',ltrim(rtrim('" & descripcion.Text & "'))," & costo_unitario_standar.Text & ",'" & t & "',1,1);"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-success")
            lblResult.Text = Resource1.str543
            boolResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(dctException)
            boolResult = False
        End Try

        Return boolResult
    End Function

    Function Edit() As Boolean
        Dim t As String
        Dim ssql As String = ""
        Dim bResult As Boolean = False
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        If rbtnTipo0.Checked = True Then
            t = "M" 'MAQUINA
        Else
            t = "B" 'BOMBA
        End If

        ssql = "update MAQUINAS set "
        ssql = ssql & "descripcion=ltrim(rtrim('" & descripcion.Text.Trim() & "')), "
        ssql = ssql & "costo=" & costo_unitario_standar.Text & ", "
        ssql = ssql & "tipo='" & t & "' "
        ssql = ssql & "where id_maquinaria='" & txtId.Text.Trim() & "';"

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-success")
            lblResult.Text = Resource1.str543

            bResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(dctException)
            bResult = False
        End Try

        Return bResult
    End Function

    Function Validar() As Boolean
        Dim ssql As String = ""
        Dim RS As New ADODB.Recordset
        Dim bResult As Boolean = True
        Dim strAction As String = Request.QueryString("Action")
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        txtId.Text = Trim(Replace(txtId.Text, " ", ""))

        'If strAction = "NEW" Then
        'ssql = " SELECT descripcion From maquinas WHERE descripcion=ltrim(rtrim('" & descripcion.Text & "')); "
        ssql = String.Format("SELECT DESCRIPCION From dbo.maquinas (NOLOCK) WHERE DESCRIPCION = '{0}' AND id_maquinaria <> '{1}'", descripcion.Text.Trim(), txtId.Text.Trim())
        RS.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        If Not RS.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str99999972
            bResult = False
        End If

        RS.Close()
        RS = Nothing
        'End If

        txtId.Text = UCase(txtId.Text)
        descripcion.Text = UCase(descripcion.Text)
        Return bResult
    End Function

    Private Sub Cargarcombo(ByRef ssql As String, ByRef id As String, ByRef Desc As String, ByRef Cbo As DropDownList)

    End Sub

End Class