Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmEtapas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            Dim STRID As String = ""
            Session.Add("FrmEtapas_STRID", STRID)

            Dim strAction As String = Request.QueryString("Action")
            Select Case UCase(strAction)
                Case "NEW"
                    clearTextBox()
                    Session.Add("frmEtapas_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str107
                    End If

                    Session.Add("frmEtapas_Action", "EDIT")
            End Select
        End If

        hdnStr109.Value = Resource1.str109
        hdnStr108.Value = String.Format(Resource1.str12127, "3")
        hdnStr528.Value = Resource1.str528
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        Dim STRID As String = ""
        Dim strAction As String = Request.QueryString("Action")
        Dim Openform As String = Session("frmEtapas_Openform")

        If Validar() Then
            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed() Then
                        Select Case UCase(Openform)
                            Case "FRMAPLICACIONINSUMOS", "FRMCOSECHAS", "FRMPLANIFICACION", "FRMREQUERIMIENTOS"
                                STRID = txtId.Text
                            Case Else
                                'frmEtapas_View.Refrescar()
                        End Select

                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str104
                    End If
                Case "EDIT"
                    If Edit() Then
                        'frmEtapas_View.Refrescar()
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str105
                    End If
            End Select

            Session.Add("frmEtapas_STRID", STRID)
        End If
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
    End Sub

    Private Sub setCaptionsLabels()
        Title = "Etapas"
        lblCodigo.Text = Resource1.str102
        lblDescripcion.Text = Resource1.str103
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
    End Sub

    Function getRecord() As Boolean
        Dim bResult As Boolean = False
        Dim codigo As String = Session("frmEtapas_Codigo")
        Dim RS As New ADODB.Recordset
        Dim ssql As String = "SELECT id_etapa, descripcion, ubicacion_cc, id_componentecuenta9, parametrodelsistema FROM dbo.Etapas WHERE id_etapa = '" & codigo & "' ;"
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        DBconn.Open(objBL.GetSQLConnection())
        RS = DBconn.Execute(ssql)

        If Not RS.EOF Then
            txtId.Text = RS.Fields("id_etapa").Value
            descripcion.Text = RS.Fields("descripcion").Value

            txtId.Enabled = False
            bResult = True
        Else
            bResult = False
        End If

        RS.Close()
        Return bResult
    End Function

    Function Add_Renamed() As Boolean
        Dim boolResult As Boolean = False

        'ubicacion_cc = 0 De acuerdo con Juan
        Dim ssql As String = "insert into Etapas (id_etapa, descripcion, ubicacion_cc, id_componentecuenta9, parametrodelsistema) "
        ssql = ssql & "values('" & txtId.Text.Trim() & "','" & descripcion.Text.Trim() & "', 0, '', 0)"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            'Get BusinessUser
            Dim businessUser = "DEMO01"

            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-success")
            lblResult.Text = Resource1.str543
            boolResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(ex, dctException)
            boolResult = False
        End Try

        Return boolResult
    End Function

    Function Edit() As Boolean
        Dim ssql As String = ""
        Dim MatrizZ() As Object = Session("frmEtapas_MatrizZ")
        Dim MatrizU() As Object = Session("frmEtapas_MatrizU")
        Dim bResult As Boolean = True
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "update dbo.Etapas set "
            ssql = ssql & "descripcion = '" & descripcion.Text.Trim() + "' "
            ssql = ssql & "where id_etapa='" & txtId.Text & "'"

            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-success")
            lblResult.Text = Resource1.str543
            bResult = True

        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(ex, dctException)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str305
            bResult = False
        End Try

        Return bResult
    End Function

    Private Sub Cargarcombo(ByRef ssql As String, ByRef id As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim adotabla As New ADODB.Recordset
        Dim I, intOrden As Short
        If Cbo.Items.Count > 0 Then intOrden = Cbo.SelectedIndex
        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        adotabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        Do While Not adotabla.EOF
            Cbo.Items.Add(adotabla.Fields(Desc).Value)
            If adotabla.Fields(id).Value = Session("frmEtapas_STRID").ToString() Then
                intOrden = I
            End If
            I = I + 1
            adotabla.MoveNext()
        Loop

        Cbo.SelectedIndex = intOrden
    End Sub

    Function Validar() As Boolean
        Dim ssql As String = ""
        Dim RS As New ADODB.Recordset
        Dim bResult As Boolean = False
        Dim strAction As String = Request.QueryString("Action")
        Dim MatrizU() As Object = Session("frmEtapas_MatrizU")

        'Remove not allowed characters
        descripcion.Text = descripcion.Text.Replace(vbTab, "")
        txtId.Text = txtId.Text.Replace(vbTab, "")

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        If strAction = "NEW" Then
            'Validate by ID & Description
            ssql = String.Format("SELECT TOP 1 id_etapa FROM dbo.Etapas (NOLOCK) WHERE id_etapa = '{0}'", txtId.Text.Trim())
            RS.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If Not RS.EOF Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str99999988

                bResult = False
                RS.Close()
                Return bResult
            End If

            RS.Close()
        End If

        ssql = String.Format("SELECT DESCRIPCION From Etapas WHERE DESCRIPCION = '{0}' AND id_etapa <> '{1}'", descripcion.Text.Trim(), txtId.Text.Trim())
        RS.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        If Not RS.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str99999989

            bResult = False
            RS.Close()
            Return bResult
        End If

        RS.Close()
        RS = Nothing
        txtId.Text = UCase(txtId.Text)
        descripcion.Text = UCase(descripcion.Text)
        bResult = True

        Return bResult
    End Function

End Class