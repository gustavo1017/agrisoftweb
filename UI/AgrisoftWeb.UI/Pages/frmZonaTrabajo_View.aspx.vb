Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmZonaTrabajo_View
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            'Verify permissions
            currentModule = "Agricultura.EstructuraCostos"

            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            LoadPageResources()
            setCboFind()
            Refresh()
        Else
            'Verificar si es que proviene de editar para que refresque la grilla 
            If Session("frmZonaTrabajo_Action") = "NEW" Or Session("frmZonaTrabajo_Action") = "EDIT" Then
                Refresh()
            End If
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "z.id_zonatrabajo"
                Case 2
                    field = "z.descripcion"
                Case 3
                    field = "c.descripcion"
                Case 4
                    field = "z.campana"
                Case 5
                    field = "z.id_ccc"
                Case 6
                    field = "fundos.descripcion"
                Case 7
                    field = "z.estado"
            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        Session.Add("frmZonaTrabajo_Action", "NEW")
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs)
        If Session("frmZonaTrabajoView_IdZonaTrabajo") Is Nothing Then
            Exit Sub
        End If

        Dim idZonaTrabajo As String = Session("frmZonaTrabajoView_IdZonaTrabajo")
        Dim objZonaTrabajoViewBL As New ZonaTrabajoView("Fundo0")
        Dim blnDistrib As Boolean = objZonaTrabajoViewBL.VerifyZonaTrabajoDistribucion(idZonaTrabajo)
        Dim blnVentas As Boolean = objZonaTrabajoViewBL.VerifyZonaTrabajoVentas(idZonaTrabajo)

        Session.Add("frmZonaTrabajo_blnDistrib", blnDistrib)
        Session.Add("frmZonaTrabajo_blnVentas", blnVentas)

        Dim nRecords As Long = 0
        If Session("frmZonaTrabajoView_nRecords") IsNot Nothing Then
            nRecords = Convert.ToInt64(Session("frmZonaTrabajoView_nRecords"))
        End If

        Session.Add("frmZonaTrabajo_Codigo", idZonaTrabajo)
        Session.Add("frmZonaTrabajo_Action", "EDIT")
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmZonaTrabajoView_IdZonaTrabajo") Is Nothing Then
            Exit Sub
        End If

        If hdnDelete.Value <> "Delete" Then
            Exit Sub
        End If

        Dim sParamSistema As String = ""
        If Not String.IsNullOrEmpty(Session("frmZonaTrabajoView_ParametroSistema")) Then
            sParamSistema = Session("frmZonaTrabajoView_ParametroSistema")
        End If

        If sParamSistema = "0" Then
            Dim nRecords As Long = 0
            If Session("frmZonaTrabajoView_nRecords") IsNot Nothing Then
                nRecords = Convert.ToInt64(Session("frmZonaTrabajoView_nRecords"))
            End If

            If nRecords > 0 Then
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
        Dim Report As New crCultivos
        Dim rsReporte As New ADODB.Recordset
        Dim dtData As New DataTable
        Dim sField As String = ""
        Dim sText As String = ""

        If Not String.IsNullOrEmpty(find.Text) Then
            sText = find.Text

            If cboFields.SelectedIndex <> -1 Then
                Select Case cboFields.SelectedIndex + 1
                    Case 1
                        sField = "z.id_zonatrabajo"
                    Case 2
                        sField = "z.descripcion"
                    Case 3
                        sField = "c.descripcion"
                    Case 4
                        sField = "z.campana"
                    Case 5
                        sField = "z.id_ccc"
                    Case 6
                        sField = "fundos.descripcion"
                    Case 7
                        sField = "z.estado"
                End Select
            End If
        End If

        Dim objZonaTrabajoViewBL As New ZonaTrabajoView()
        Dim sQuery As String = objZonaTrabajoViewBL.GetSQLQuery(sField, sText)

        Session.Add("frmExportParametros_ParamExport", "ZONATRABAJO")
        Session.Add("frmExportParametros_ReportQuery", sQuery)

        Response.Redirect("frmExport_Parametros.aspx")

        'dtData = objZonaTrabajoViewBL.cargarDatosGrilla(sField, sText)

        'Report.Load(Report.ResourceName)
        'Report.SetDataSource(dtData)
        'Response.Buffer = False
        'Response.ClearContent()
        'Response.ClearHeaders()
        'Report.ExportToHttpResponse(ExportFormatType.Excel, Response, True, "Archivo")
        'Response.End()
    End Sub

    Protected Sub grilla_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(0).Text = Resource1.str302
                e.Row.Cells(1).Text = Resource1.str303
                e.Row.Cells(2).Text = Resource1.str311
                e.Row.Cells(3).Text = Resource1.str5003
                e.Row.Cells(4).Text = Resource1.str541
                e.Row.Cells(5).Text = Resource1.str86
                e.Row.Cells(6).Text = Resource1.str11032
                e.Row.Cells(7).Text = Resource1.str136
                e.Row.Cells(8).Text = Resource1.str164
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right

                e.Row.Cells(1).Width = New Unit(150, UnitType.Pixel)
                e.Row.Cells(2).Width = New Unit(55, UnitType.Pixel)
                e.Row.Cells(3).Width = New Unit(145, UnitType.Pixel)
                e.Row.Cells(4).Width = New Unit(66, UnitType.Pixel)
                e.Row.Cells(5).Width = New Unit(40, UnitType.Pixel)

            Case DataControlRowType.DataRow
                e.Row.Cells(2).Text = Convert.ToDecimal(e.Row.Cells(2).Text).ToString("###0.00")
                e.Row.Cells(4).Text = Convert.ToDecimal(e.Row.Cells(4).Text).ToString("###0")

                Dim dateConvert As DateTime
                If DateTime.TryParse(e.Row.Cells(14).Text, dateConvert) Then
                    e.Row.Cells(14).Text = Convert.ToDateTime(e.Row.Cells(14).Text).ToString("dd/MM/yyyy")
                End If

        End Select
        e.Row.Cells(5).Visible = False
        e.Row.Cells(6).Visible = False
        e.Row.Cells(7).Visible = False
        e.Row.Cells(8).Visible = False
        e.Row.Cells(9).Visible = False
        e.Row.Cells(10).Visible = False
        e.Row.Cells(11).Visible = False
        e.Row.Cells(12).Visible = False
        e.Row.Cells(13).Visible = False
        e.Row.Cells(15).Visible = False
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

    Protected Sub grilla_SelectedIndexChanged(sender As Object, e As EventArgs)
        For Each row As GridViewRow In grilla.Rows
            If row.RowIndex = grilla.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = ""

                Session.Add("frmZonaTrabajoView_IdZonaTrabajo", row.Cells(0).Text)
                Session.Add("frmZonaTrabajo_Codigo", row.Cells(0).Text)
                Session.Add("frmZonaTrabajoView_ZonaTrabajoDescripcion", row.Cells(1).Text)
                Session.Add("frmZonaTrabajoView_ParametroSistema", row.Cells(5).Text)
                Session.Add("frmZonaTrabajoView_Estado", row.Cells(15).Text)
            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Seleccionar"
            End If
        Next
    End Sub

    Private Sub setCaptionsLabels()
        Title = Resource1.str301
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
        cboFields.Items.Add((Resource1.str313)) ' Codigo Zona de trabajo
        cboFields.Items.Add((Resource1.str905)) ' Descripcion Zona de trabajo
        cboFields.Items.Add((Resource1.str318)) ' cultivo descripcion
        cboFields.Items.Add((Resource1.str541))
        cboFields.Items.Add((Resource1.str10176)) 'ccc
        cboFields.Items.Add((Resource1.str11032)) ' fundos
        cboFields.Items.Add(("Estado")) ' fundos

        cboFields.SelectedIndex = 0
    End Sub

    Private Sub cargarDatosGrilla(ByVal sField As String, ByVal sText As String)
        Dim sRecords As String
        If sField <> "" Then
            sRecords = Resource1.str1003
        Else
            sRecords = Resource1.str1002
        End If

        Dim dtData As New DataTable
        Session.Add("frmZonaTrabajoView_nRecords", 0)
        Dim objZonaTrabajoViewBL As New ZonaTrabajoView("Fundo0")
        dtData = objZonaTrabajoViewBL.cargarDatosGrilla(sField, sText)

        'strParamExport = "CULTIVOS"
        'SQLExport = ssql

        Dim nRecords As Long
        nRecords = dtData.Rows.Count

        'hdnDelete verifica si el page_load viene desde el botón Delete, sino se limpia la variable de sesión 
        'If String.IsNullOrEmpty(hdnDelete.Value) Then
        Session.Add("frmZonaTrabajoView_IdZonaTrabajo", Nothing)
        'End If

        Session.Add("frmZonaTrabajo_Codigo", Nothing)
        Session.Add("frmZonaTrabajo_Action", "")
        Session.Add("frmZonaTrabajoView_nRecords", nRecords)
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

    Private Function Delete() As Boolean
        Dim sMen As String = Resource1.str1005
        Dim idZonaTrabajo As String = Session("frmZonaTrabajoView_IdZonaTrabajo")
        Dim bParametroSistema As String = Session("frmZonaTrabajoView_ParametroSistema")
        Dim objZonaTrabajoViewBL As New ZonaTrabajoView("Fundo0")
        Dim nCantidadRegistros As Long = Session("frmZonaTrabajoView_nRecords")
        Dim bolResult As Boolean = False

        If bParametroSistema = "0" Then
            If nCantidadRegistros > 0 Then
                Dim strSQL As String = "SELECT CTASCTES.Fecha_pago, CTASCTES.id_activo, CTASCTES.origen_asiento FROM CTASCTES INNER JOIN ACTIVOS ON CTASCTES.id_activo = ACTIVOS.id_activocontable WHERE (CTASCTES.id_activo = '" & idZonaTrabajo & "' ) "
                Dim rs As New ADODB.Recordset

                If rs.State = 1 Then
                    rs.Close()
                End If

                Dim objBL As New GenericMethods("Fundo0")
                Dim DBconn As New ADODB.Connection
                DBconn.Open(objBL.GetSQLConnection())
                rs.let_ActiveConnection(DBconn)
                rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
                rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
                rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
                rs.let_Source(strSQL)
                rs.Open()

                If Not rs.EOF() Then
                    dvMessage.Visible = True
                    lblResults.Text = Resources.Resource1.str999997 ' "Esta zona de trabajo ya fue uitilizada como activo en contabilidad. Para eliminar esta zona de trabajo, debe primero borrar tambien el activo y los asientos contables asociados"
                End If

                bolResult = objZonaTrabajoViewBL.Delete(idZonaTrabajo)
                If Not bolResult Then
                    dvMessage.Visible = True
                    lblResults.Text = Resource1.str306
                End If
                Refresh()
            End If
        Else
            dvMessage.Visible = True
            lblResults.Text = Resource1.str100
        End If

        Return bolResult
    End Function

    Private Sub Refresh()
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "z.id_zonatrabajo"
                Case 2
                    field = "z.descripcion"
                Case 3
                    field = "c.descripcion"
                Case 4
                    field = "z.campana"
                Case 5
                    field = "z.id_ccc"
                Case 6
                    field = "fundos.descripcion"
                Case 7
                    field = "z.estado"
            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If
    End Sub

    Private Sub LoadPageResources()
        hdnStr1005.Value = Resource1.str1005
        hdnStr12104.Value = Resource1.str12104
        hdnStr301.Value = Resource1.str301
    End Sub
End Class