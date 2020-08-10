Imports System.Data.OleDb
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmPersonalb_View

    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify permissions
        CheckCurrentSession()
        currentModule = "Agricultura.EstructuraCostos"

        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            Dim productsEnabled As List(Of String) = GetProductsEnabled("Fundo0")

            If productsEnabled.Count > 0 Then
                If productsEnabled.Exists(Function(x) x = "ERP") Then
                    hdnIncludeERP.Value = True
                Else
                    hdnIncludeERP.Value = False
                End If
            Else
                hdnIncludeERP.Value = False
            End If

            LoadPageResources()
            setCboFind()
            Refresh()
        Else
            'Verificar si es que proviene de editar para que refresque la grilla 
            If Session("frmPersonalB_Action") = "NEW" Or Session("frmPersonalB_Action") = "EDIT" Then
                Refresh()
            End If
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        Dim field As String = ""

        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "nombre"
                Case 2
                    field = "id_personal"
                Case 3
                    field = "estado"
                Case 4
                    field = "CATEGORIA.descripcion"
                Case 5
                    field = "personal.id_CicloPago"
            End Select

            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        Session.Add("frmPersonalB_Action", "NEW")
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs)
        If Session("frmPersonalbView_IdPersonal") Is Nothing Then
            Exit Sub
        End If

        Dim idPersonal As String = Session("frmPersonalbView_IdPersonal")
        Dim nRecords As Long = 0
        If Session("frmPersonalbView_nRecords") IsNot Nothing Then
            nRecords = Convert.ToInt64(Session("frmPersonalbView_nRecords"))
        End If

        Session.Add("frmPersonalB_Codigo", idPersonal)
        Session.Add("frmPersonalB_Action", "EDIT")
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmPersonalbView_IdPersonal") Is Nothing Then
            Exit Sub
        End If

        If hdnDelete.Value <> "Delete" Then
            Exit Sub
        End If

        Dim sParamSistema As String = ""
        If Not String.IsNullOrEmpty(Session("frmPersonalbView_ParametroSistema")) Then
            sParamSistema = Session("frmPersonalbView_ParametroSistema")
        End If

        If sParamSistema = "0" Then
            Dim nRecords As Long = 0
            If Session("frmPersonalbView_nRecords") IsNot Nothing Then
                nRecords = Convert.ToInt64(Session("frmPersonalbView_nRecords"))
            End If

            If nRecords > 0 Then
                Dim idPersonal As String = Session("frmPersonalbView_IdPersonal").ToString()

                If Not ValidaIntegridad("CTASCTES", "PERSONAL", idPersonal) Then
                    dvMessage.Visible = True
                    lblResults.Text = Resource1.str5006
                    Exit Sub
                End If

                If Not Delete() Then
                    dvMessage.Visible = True
                    lblResults.Text = Resource1.str5006
                End If

            End If
        Else
            dvMessage.Visible = True
            lblResults.Text = Resource1.str100
        End If
    End Sub

    Protected Sub btnRefrescar_Click(sender As Object, e As EventArgs)
        find.Text = ""
        cargarDatosGrilla("", "")
    End Sub

    Protected Sub btnExportarExcel_Click(sender As Object, e As EventArgs)
        Dim sField As String = ""
        Dim sText As String = ""

        If Not String.IsNullOrEmpty(find.Text) Then
            sText = find.Text

            If cboFields.SelectedIndex <> -1 Then
                Select Case cboFields.SelectedIndex + 1
                    Case 1
                        sField = "nombre"
                    Case 2
                        sField = "id_personal"
                    Case 3
                        sField = "estado"
                    Case 4
                        sField = "CATEGORIA.descripcion"
                    Case 5
                        sField = "personal.id_CicloPago"
                End Select
            End If
        End If

        Dim ssql As String = "SELECT PERSONAL.id_personal, CASE WHEN [PERSONAL].[estado] <> 0 THEN 'Activo' ELSE 'Inactivo' END AS Estado, PERSONAL.nombre, PERSONAL.costo_conta, PERSONAL.costo, PERSONAL.horaes, PERSONAL.horaed, CATEGORIA.descripcion AS Categoria, PERSONAL.orden, PERSONAL.Parametrodelsistema, PERSONAL.id_tipodocumento, CICLODEPAGO.Descripcion FROM         PERSONAL LEFT OUTER JOIN CICLODEPAGO ON PERSONAL.id_CicloPago = CICLODEPAGO.Id_ciclopago LEFT OUTER JOIN CATEGORIA ON PERSONAL.id_categoria = CATEGORIA.id_categoria WHERE PERSONAL.ID_CATEGORIA<>'EMPL' "

        If sField <> "" Then
            ssql = ssql & " and " & sField & " like '%" & sText & "%';"
        Else
            ssql = ssql & ";"
        End If

        Session.Add("frmExportParametros_ParamExport", "PERSONAL")
        Session.Add("frmExportParametros_ReportQuery", ssql)
        Response.Redirect("frmExport_Parametros.aspx")
    End Sub

    Protected Sub grilla_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(0).Text = Resource1.str4002 'Estado
                e.Row.Cells(1).Text = Resource1.str3053 'Codigo
                e.Row.Cells(2).Text = Resource1.str4003 'Nombre
                e.Row.Cells(3).Text = Resource1.str10138 'costo_conta
                e.Row.Cells(4).Text = Resource1.str514 'costo standar
                e.Row.Cells(5).Text = Resource1.str10111 'Hora E Simple
                e.Row.Cells(6).Text = Resource1.str10112 'HOra Ex Doble
                e.Row.Cells(7).Text = Resource1.str10113 'Categoria

                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right

                e.Row.Cells(0).Width = New Unit(60, UnitType.Pixel)
                e.Row.Cells(1).Width = New Unit(45, UnitType.Pixel)
                e.Row.Cells(2).Width = New Unit(165, UnitType.Pixel)
                e.Row.Cells(3).Width = New Unit(55, UnitType.Pixel)
                e.Row.Cells(4).Width = New Unit(55, UnitType.Pixel)
                e.Row.Cells(5).Width = New Unit(55, UnitType.Pixel)
                e.Row.Cells(6).Width = New Unit(55, UnitType.Pixel)
                e.Row.Cells(7).Width = New Unit(130, UnitType.Pixel)
                e.Row.Cells(11).Width = New Unit(230, UnitType.Pixel)

            Case DataControlRowType.DataRow
                'e.Row.Cells(2).Text = Convert.ToDecimal(e.Row.Cells(2).Text).ToString("###0.00")
                'e.Row.Cells(4).Text = Convert.ToDecimal(e.Row.Cells(4).Text).ToString("###0")

                'Dim dateConvert As DateTime
                'If DateTime.TryParse(e.Row.Cells(14).Text, dateConvert) Then
                '    e.Row.Cells(14).Text = Convert.ToDateTime(e.Row.Cells(14).Text).ToString("dd/MM/yyyy")
                'End If
        End Select

        e.Row.Cells(8).Visible = False
        e.Row.Cells(9).Visible = False
        e.Row.Cells(10).Visible = False

        If hdnIncludeERP.Value = "False" Then
            e.Row.Cells(3).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
        End If
    End Sub

    Protected Sub grilla_SelectedIndexChanged(sender As Object, e As EventArgs)
        For Each row As GridViewRow In grilla.Rows
            If row.RowIndex = grilla.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = ""

                Session.Add("frmPersonalbView_IdPersonal", row.Cells(0).Text)
                Session.Add("frmPersonalB_Codigo", row.Cells(0).Text)
                Session.Add("frmPersonalbView_ParametroSistema", row.Cells(9).Text)
            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Seleccionar"
            End If
        Next
    End Sub

    Protected Sub grilla_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.cursor='pointer';this.style.textDecoration='underline';"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';"
            e.Row.ToolTip = "Seleccionar"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(grilla, "Select$" & e.Row.RowIndex)
            'e.Row.Attributes("onclick") = Page.ClientScript.RegisterForEventValidation((grilla, "Select$" & e.Row.RowIndex)
        End If
    End Sub

    Public Function ValidaIntegridad(ByVal pNomTablaPadre As String, ByVal pNomTablaHijo As String, ByVal pRecord As String) As Boolean
        Dim RS As New ADODB.Recordset
        Dim ssql As String = "SELECT * FROM CTASCTES WHERE id_anexo = '" & pRecord & "';"
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        RS = New ADODB.Recordset
        If RS.State = 1 Then
            RS.Close()
        End If
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(ssql)
        RS.Open()

        If RS.RecordCount >= 1 Then Return False

        Return True
    End Function

    Public Function Delete() As Boolean
        Dim idPersonal As String = Session("frmPersonalbView_IdPersonal").ToString()
        Dim bolResult As Boolean = False
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection
        Dim ssql As String = ""

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "delete from PERSONAL where id_personal='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from AFECTAPERSONAL where id_personal='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA17B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA11B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA4B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA5B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA25B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA26B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            ssql = "delete from PDTESTRUCTURA12B where NUMERO_DOCUMENTO='" & idPersonal & "';"
            DBconn.Execute(ssql)

            bolResult = True
            Refresh()
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
            bolResult = False
        End Try

        Return bolResult
    End Function

    Private Sub setCaptionsLabels()
        Title = Resource1.str4001
        lblFind.Text = Resource1.str1004
    End Sub

    Private Sub setCaptionsButtons()
        btnNuevo.Text = Resource1.str1
        btnModificar.Text = Resource1.str2
        btnEliminar.Text = Resource1.str3
        btnBuscar.Text = Resource1.str7
        btnRefrescar.Text = Resource1.str8
        btnExportarExcel.Text = Resource1.str12
    End Sub

    Private Sub setCaptionGrilla()
        'Las columnas ocultas se pusieron en el evento grilla_RowDataBound
    End Sub

    Private Sub setCboFind()
        cboFields.Items.Clear()
        cboFields.Items.Add(Resource1.str4003) ' Nombre de personal
        cboFields.Items.Add(Resource1.str4002) ' Código de personal
        cboFields.Items.Add(Resource1.str3053) ' Estado
        cboFields.Items.Add(Resource1.str10113) ' Categoria
        cboFields.Items.Add(Resource1.str10148) ' Ciclo de Pago

        cboFields.SelectedIndex = 0
    End Sub

    Private Sub Refresh()
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "nombre"
                Case 2
                    field = "id_personal"
                Case 3
                    field = "estado"
                Case 4
                    field = "CATEGORIA.descripcion"
                Case 5
                    field = "personal.id_CicloPago"
            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If
    End Sub

    Private Sub cargarDatosGrilla(ByVal sField As String, ByVal sText As String)
        Dim sRecords As String
        Dim ssql As String = "SELECT     PERSONAL.id_personal, CASE WHEN [PERSONAL].[estado] <> 0 THEN 'Activo' ELSE 'Inactivo' END AS Estado, PERSONAL.nombre, PERSONAL.costo_conta, PERSONAL.costo, PERSONAL.horaes, PERSONAL.horaed, CATEGORIA.descripcion AS Categoria, PERSONAL.orden, PERSONAL.Parametrodelsistema, PERSONAL.id_tipodocumento, CICLODEPAGO.Descripcion as ciclo FROM         PERSONAL LEFT OUTER JOIN CICLODEPAGO ON PERSONAL.id_CicloPago = CICLODEPAGO.Id_ciclopago LEFT OUTER JOIN CATEGORIA ON PERSONAL.id_categoria = CATEGORIA.id_categoria WHERE PERSONAL.ID_CATEGORIA<>'EMPL' "

        If sField <> "" Then
            sRecords = Resource1.str1003
            ssql = ssql & " and " & sField & " like '%" & sText & "%';"
        Else
            sRecords = Resource1.str1002
            ssql = ssql & ";"
        End If

        Dim dtData As New DataTable
        dtData = cargarDataTable(ssql)
        Session.Add("frmPersonalbView_nRecords", 0)

        Dim nRecords As Long
        nRecords = dtData.Rows.Count

        'hdnDelete verifica si el page_load viene desde el botón Delete, sino se limpia la variable de sesión 
        'If String.IsNullOrEmpty(hdnDelete.Value) Then
        Session.Add("frmPersonalbView_IdPersonal", Nothing)
        'End If

        Session.Add("frmPersonalB_Codigo", Nothing)
        Session.Add("frmPersonalB_Action", "")
        Session.Add("frmPersonalbView_nRecords", nRecords)
        hdnDelete.Value = ""
        grilla.DataSource = dtData
        grilla.DataBind()
        setCaptionGrilla()

        lblReg.Text = nRecords & " " & sRecords

        If dtData.Rows.Count > 0 Then
            grilla.HeaderRow.TableSection = TableRowSection.TableHeader
            grilla.UseAccessibleHeader = True
        End If

        dvMessage.Visible = False
    End Sub

    Private Sub LoadPageResources()
        hdnStr1005.Value = Resource1.str1005
        hdnStr12104.Value = Resource1.str12104
        hdnStr4001.Value = Resource1.str4001
    End Sub
End Class