Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class frmResumenCostosEnTodosLosCultivosHA
    Inherits BasePage

    Dim Report As New crCostosEnTodosLosCultivosHa2
    Dim MatrizZ() As Object
    Dim rsReporte As New ADODB.Recordset
    'Dim gblstrIdUsuario As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim fechaFrom As DateTime = DateTime.Now.AddMonths(-1)
        Dim fechaTo As DateTime = DateTime.Now

        setEtiquetas()

        If Not Page.IsPostBack() Then
            currentModule = "Agricultura.Reportes"
            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            edtNumeroCampana.Enabled = True
            dtfechamin.Enabled = False
            dtfechamax.Enabled = False
            edtNumeroCampana.Text = CStr(2017)


            btnConfigurar.Enabled = False
            btnImprimir.Enabled = False
            btnExportarExcel.Enabled = False
            rbtnCampañaFilter.Checked = True

            hdnStr6013.Value = Resource1.str6013
            hdnStr6018.Value = Resource1.str6018

            CargarCombo("id_zonatrabajo")

            cboMoneda.Items.Clear()
            cboMoneda.Items.Add(Resource1.str10004) 'Nacional
            cboMoneda.Items.Add(Resource1.str10005) 'Extranjera
            cboMoneda.SelectedIndex = 0

            ' Load Report only first load
            Dim objGenericBL As New GenericMethods("Fundo0")
            Dim businessUser = "DEMO01"

            Dim gblstrIdUsuario As String = businessUser    '"Fundo0"
            Dim objBL As New ResumenCostosEnTodosLosCultivosHABL("Fundo0")
            'rsReporte = objBL.Refresh_Query(" WHERE fecha Between convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) And convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')) ", 0, gblstrIdUsuario)

            rsReporte = objBL.Refresh_Query(" WHERE r.campana=2017 ", 0, gblstrIdUsuario)


            If rsReporte.RecordCount = 0 Then
                crvCostosEnTodosLosCultivos.ReportSource = Nothing
                Exit Sub
            End If

            Report.Load(Report.ResourceName) : Report.SetDataSource(rsReporte)
            Report.SetParameterValue("DomainURL", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Pages/")
            'MsgBox(Resource1.str11, MsgBoxStyle.Information) ' Reporte Generado ' Reporte Generado
            crvCostosEnTodosLosCultivos.ReportSource = Report
            crvCostosEnTodosLosCultivos.ToolPanelView = ToolPanelViewType.None
            setResourcesToReport()

            Session.Add("FrmResumenCostosEnTodosLosCultivosHA_Report", Report)
        Else
            If Session("FrmResumenCostosEnTodosLosCultivosHA_Report") IsNot Nothing Then
                Report = Session("FrmResumenCostosEnTodosLosCultivosHA_Report")
                crvCostosEnTodosLosCultivos.ReportSource = Report
            End If

            Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
            Dim crtableLogoninfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table

            crCon = GetConnectionInfo()

            CrTables = Report.Database.Tables
            For Each CrTable In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            For i As Integer = 0 To crvCostosEnTodosLosCultivos.LogOnInfo.Count - 1
                crvCostosEnTodosLosCultivos.LogOnInfo(i).ConnectionInfo = crCon
            Next i

            crvCostosEnTodosLosCultivos.ReuseParameterValuesOnRefresh = True
        End If

        'Save on Session the values needed for report FrmReporteZTCostosRecursosEtapasActividades
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_NumeroCampana", edtNumeroCampana.Text)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMin", fechaFrom)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMax", fechaTo)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_cboMonedaSelectedIndex", cboMoneda.SelectedIndex)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_rbtnCampañaSelected", rbtnCampañaFilter.Checked)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_rbtnDesdeSelected", rbtnFechaDesdeFilter.Checked)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()

    End Sub

    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Dim sFilter As String
        Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim CrTables As Tables
        Dim CrTable As Table

        crCon = GetConnectionInfo()

        CrTables = Report.Database.Tables
        For Each CrTable In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crCon
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        For i As Integer = 0 To crvCostosEnTodosLosCultivos.LogOnInfo.Count - 1
            crvCostosEnTodosLosCultivos.LogOnInfo(i).ConnectionInfo = crCon
        Next i

        'lblMessage.Text = ""
        Dim CampoCampana As String
        If Val(edtNumeroCampana.Text) = Int(IIf(edtNumeroCampana.Text = "", 0, edtNumeroCampana.Text)) Then
            If Me.cboMoneda.SelectedIndex = 0 Then
                CampoCampana = "cast(r.campana as int)"
            Else
                CampoCampana = "cast(r.campana as int)"
            End If
        Else
            If Me.cboMoneda.SelectedIndex = 0 Then
                CampoCampana = "(r.campana)"
            Else
                CampoCampana = "(r.campana)"
            End If
        End If

        If rbtnCampañaFilter.Checked = True Then
            If Trim(edtNumeroCampana.Text) = "" Then
                'MsgBox(Resource1.str6018)
                edtNumeroCampana.Focus()
                Exit Sub
            End If

            If Not IsNumeric(Trim(edtNumeroCampana.Text)) Then
                'MsgBox(Resource1.str6013)
                Exit Sub
            End If

            sFilter = " WHERE ((" & CampoCampana & "=" & edtNumeroCampana.Text & ")) "

        Else
            Dim fechaFrom As DateTime ' = DateTime.ParseExact(dtfechamin.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None)
            Dim fechaTo As DateTime '= DateTime.ParseExact(dtfechamax.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None)

            DateTime.TryParseExact(dtfechamin.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechaFrom)
            DateTime.TryParseExact(dtfechamax.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechaTo)

            If fechaFrom = DateTime.MinValue Then
                ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "text", "ShowErrorMessage(Resource1.str99992)", True)
                'lblMessage.Text = "El formato de la fecha debe ser dd/MM/yyyy"
                crvCostosEnTodosLosCultivos.ReportSource = Nothing
                Exit Sub
            End If

            If fechaTo = DateTime.MinValue Then
                ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "text", "ShowErrorMessage(Resource1.str99992)", True)
                'lblMessage.Text = "El formato de la fecha debe ser dd/MM/yyyy"
                crvCostosEnTodosLosCultivos.ReportSource = Nothing
                Exit Sub
            End If

            If Me.cboMoneda.SelectedIndex = 0 Then
                sFilter = " WHERE fecha Between convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) And convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')) "
            Else
                sFilter = " WHERE fecha Between convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) And convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')) "
            End If
        End If

        Dim objBL As New ResumenCostosEnTodosLosCultivosHABL("Fundo0")
        Dim objGenericBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"
        Dim gblstrIdUsuario As String = businessUser    '"Fundo0"
        rsReporte = objBL.Refresh_Query(sFilter, cboMoneda.SelectedIndex, gblstrIdUsuario)

        If rsReporte.RecordCount = 0 Then
            crvCostosEnTodosLosCultivos.ReportSource = Nothing
            Exit Sub
        End If
        Report.Load(Report.ResourceName)
        Report.SetDataSource(rsReporte)
        Report.SetParameterValue("DomainURL", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Pages/")

        'MsgBox(Resource1.str11, MsgBoxStyle.Information) ' Reporte Generado ' Reporte Generado
        crvCostosEnTodosLosCultivos.ToolPanelView = ToolPanelViewType.None
        crvCostosEnTodosLosCultivos.ReportSource = Report
        setResourcesToReport()

        btnConfigurar.Enabled = True
        btnImprimir.Enabled = True
        btnExportarExcel.Enabled = True

        'Save on Session the values needed for report FrmReporteZTCostosRecursosEtapasActividades
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_NumeroCampana", edtNumeroCampana.Text)

        If String.IsNullOrEmpty(dtfechamin.Text) Then
            Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMin", hdnDateFrom.Value)
        Else
            Dim datefromConverted As DateTime
            If DateTime.TryParse(dtfechamin.Text, datefromConverted) Then
                Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMin", datefromConverted)
            Else
                Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMin", Nothing)
            End If
        End If

        If String.IsNullOrEmpty(dtfechamax.Text) Then
            Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMax", hdnDateTo.Value)
        Else
            Dim datetoConverted As DateTime
            If DateTime.TryParse(dtfechamax.Text, datetoConverted) Then
                Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMax", datetoConverted)
            Else
                Session.Add("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMax", Nothing)
            End If
        End If

        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_cboMonedaSelectedIndex", cboMoneda.SelectedIndex)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_rbtnCampañaSelected", rbtnCampañaFilter.Checked)
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_rbtnDesdeSelected", rbtnFechaDesdeFilter.Checked)
        'Session.Add("FrmReporteZTCostosRecursosEtapasActividades_cboZonaTrabajoSelectedIndex", cboZonatrabajo.SelectedIndex)

        Session.Add("FrmResumenCostosEnTodosLosCultivosHA_Report", Report)
    End Sub

    Private Sub setEtiquetas()
        btnVer.Text = Resource1.str9197 'Ver
        btnConfigurar.Text = Resource1.str9196 'Configurar
        btnImprimir.Text = Resource1.str9195 'Imprimir
        btnExportarExcel.Text = Resource1.str9194 'Exportar
        Me.lblMoneda.Text = Resource1.str10003 'Moneda

        rbtnFechaDesdeFilter.Text = Resource1.str9199 'Desde
        rbtnCampañaFilter.Text = Resource1.str541 'Campaña
        lblHasta.Text = Resource1.str3004 'Hasta
        Frame1.GroupingText = Resource1.str13 'Parametros
        'btnOrdenar.Text = Resource1.str21 'Ordenar cultivos
        'Me.Text = Resource1.str9242


    End Sub

    Private Sub setResourcesToReport()
        Dim Campo As TextObject
        Campo = Report.ReportDefinition.ReportObjects("txtMoneda") : Campo.Text = Resource1.str10003 ' Moneda
        Campo = Report.ReportDefinition.ReportObjects("text1") : Campo.Text = Resource1.str315 ' cultivo
        Campo = Report.ReportDefinition.ReportObjects("text3") : Campo.Text = Resource1.str2012 ' Insumo
        Campo = Report.ReportDefinition.ReportObjects("text4") : Campo.Text = Resource1.str2003 ' Maquinaria
        Campo = Report.ReportDefinition.ReportObjects("text6") : Campo.Text = Resource1.str3012 ' Riego
        Campo = Report.ReportDefinition.ReportObjects("text5") : Campo.Text = Resource1.str18 ' Trabajadores


        If Me.cboMoneda.SelectedIndex = 0 Then
            Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10004 'Nacional
        Else
            Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10005 'Extranjera
        End If

        If rbtnCampañaFilter.Checked = True Then
            Campo = Report.ReportDefinition.ReportObjects("edtlabel") : Campo.Text = Resource1.str541
            Campo = Report.ReportDefinition.ReportObjects("edtvalue") : Campo.Text = edtNumeroCampana.Text
        Else
            Dim fechaFrom As DateTime = DateTime.Parse(dtfechamin.Text)
            Dim fechaTo As DateTime = DateTime.Parse(dtfechamax.Text)
            Campo = Report.ReportDefinition.ReportObjects("edtlabel") : Campo.Text = Resource1.str3003
            Campo = Report.ReportDefinition.ReportObjects("edtvalue") : Campo.Text = fechaFrom.ToString("dd/MM/yyyy") & Resource1.str3004 & fechaTo.ToString("dd/MM/yyyy")
        End If
    End Sub

    Private Sub CargarCombo(ByVal id As String)
        Dim intOrden, i As Short
        Dim adoTabla As ADODB.Recordset
        Dim STRID As String = ""

        If cboZonatrabajo.Items.Count > 0 Then intOrden = cboZonatrabajo.SelectedIndex
        'cboZonatrabajo.Items.Clear() : cboZonatrabajo.AutoCompleteSource = AutoCompleteSource.ListItems : cboZonatrabajo.AutoCompleteMode = AutoCompleteMode.Suggest

        Dim objBL As New ReporteZTCostosRecursosEtapasActividades("Fundo0")
        Dim objGenericBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"
        Dim gblstrIdUsuario As String = businessUser    '"Fundo0"
        adoTabla = objBL.getDataZonaTrabajo(gblstrIdUsuario, MatrizZ, id, "descripcion")

        Do While Not adoTabla.EOF
            Select Case id
                Case "id_zonatrabajo" : ReDim Preserve MatrizZ(i)

                    MatrizZ(i) = adoTabla.Fields(id).Value
            End Select
            cboZonatrabajo.Items.Add(adoTabla.Fields("descripcion").Value)
            If adoTabla.Fields(id).Value = STRID Then
                intOrden = i
            End If
            i = i + 1
            adoTabla.MoveNext()
        Loop

        cboZonatrabajo.SelectedIndex = intOrden

    End Sub

    Protected Sub btnConfigurar_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub rbtnCampañaFilter_CheckedChanged(sender As Object, e As EventArgs)
        edtNumeroCampana.Enabled = True
        dtfechamin.Enabled = False
        dtfechamax.Enabled = False
    End Sub

    Protected Sub rbtnFechaDesdeFilter_CheckedChanged(sender As Object, e As EventArgs)
        edtNumeroCampana.Enabled = False
        dtfechamin.Enabled = True
        dtfechamax.Enabled = True
    End Sub

    Protected Function GetConnectionInfo() As CrystalDecisions.Shared.ConnectionInfo
        Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
        Dim objBL As New GenericMethods("Fundo0")
        Dim dctInfo As New Dictionary(Of String, String)
        '  dctInfo = objBL.GetConnectionInfo()

        With crCon
            .ServerName = ConfigurationManager.AppSettings("Default_DataSource")
            .UserID = ConfigurationManager.AppSettings("Default_DBUser")
            .Password = ConfigurationManager.AppSettings("Default_Password")
            .DatabaseName = ConfigurationManager.AppSettings("Default_DBName")
            .Type = ConnectionInfoType.SQL
            .IntegratedSecurity = False
        End With

        Return crCon
    End Function
End Class