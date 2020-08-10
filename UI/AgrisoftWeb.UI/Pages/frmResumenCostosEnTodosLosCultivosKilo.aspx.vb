Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class frmResumenCostosEnTodosLosCultivosKilo
    Inherits BasePage

    Dim Report As New Crcostosxhaxkgs
    Dim MatrizZ() As Object
    Dim rsReporte As New ADODB.Recordset

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

            cboMoneda.Items.Clear()
            cboMoneda.Items.Add(Resource1.str10004) 'Nacional
            cboMoneda.Items.Add(Resource1.str10005) 'Extranjera
            cboMoneda.SelectedIndex = 0

            ' Load Report only first load
            Dim dtData As New DataTable
            'Dim sFilter As String = " WHERE (((ResumenDeCostosPorCultivopORkILO.fecha) Between convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) And convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')))) "
            Dim sFilter As String = " WHERE ResumenDeCostosPorCultivopORkILO.campana=2017 "

            'rsReporte = objBL.Refresh_Query(" WHERE r.campana=2017 ", 0, gblstrIdUsuario)
            Dim ssql As String = Refresh_Query(sFilter)
            dtData = cargarDataTable(ssql)

            If dtData.Rows.Count() = 0 Then
                crvCostosEnTodosLosCultivos.ReportSource = Nothing
                Exit Sub
            End If

            Report.Load(Report.ResourceName) : Report.SetDataSource(dtData)
            Report.SetParameterValue("DomainURL", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Pages/")
            crvCostosEnTodosLosCultivos.ReportSource = Report
            crvCostosEnTodosLosCultivos.ToolPanelView = ToolPanelViewType.None
            setResourcesToReport()

            Session.Add("frmReporteCostosEnTodosLosCultivosPorKilo_Report", Report)

        Else

            If Session("frmReporteCostosEnTodosLosCultivosPorKilo_Report") IsNot Nothing Then
                Report = Session("frmReporteCostosEnTodosLosCultivosPorKilo_Report")
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

        Dim CampoCampana As String
        If Val(edtNumeroCampana.Text) = Int(IIf(edtNumeroCampana.Text = "", 0, edtNumeroCampana.Text)) Then
            If Me.cboMoneda.SelectedIndex = 0 Then
                CampoCampana = "int(ResumenDeCostosPorCultivoPorKilo.campana)"
            Else
                CampoCampana = "int(ResumenDeCostosPorCultivoPorKiloME.campana)"
            End If
        Else
            If Me.cboMoneda.SelectedIndex = 0 Then
                CampoCampana = "(ResumenDeCostosPorCultivoPorKilo.campana)"
            Else
                CampoCampana = "(ResumenDeCostosPorCultivoPorKiloME.campana)"
            End If
        End If

        If Me.cboMoneda.SelectedIndex = 0 Then
            ''Dim edtmoneda As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("edtmoneda") : edtmoneda.Text = My.Resources.Resource1.Resource1.str10004 'Nacional
        Else
            ''Dim edtmoneda2 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("edtmoneda") : edtmoneda2.Text = My.Resources.Resource1.Resource1.str10005
        End If

        If rbtnCampañaFilter.Checked = True Then
            If Trim(edtNumeroCampana.Text) = "" Then
                edtNumeroCampana.Focus()
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
                'str99992

                crvCostosEnTodosLosCultivos.ReportSource = Nothing
                Exit Sub
            End If

            If Me.cboMoneda.SelectedIndex = 0 Then
                sFilter = " WHERE (((ResumenDeCostosPorCultivoPorKilo.fecha) Between convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) And convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')))) "
            Else
                sFilter = " WHERE (((ResumenDeCostosPorCultivoPorKiloME.fecha) Between convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) And convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')))) "
            End If
        End If

        Dim ssql As String = Refresh_Query(sFilter)
        Dim dtData As New DataTable
        dtData = cargarDataTable(ssql)

        If dtData.Rows.Count() = 0 Then
            crvCostosEnTodosLosCultivos.ReportSource = Nothing
            Exit Sub
        End If

        Report.Load(Report.ResourceName)
        Report.SetDataSource(dtData)
        Report.SetParameterValue("DomainURL", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Pages/")
        crvCostosEnTodosLosCultivos.ToolPanelView = ToolPanelViewType.None
        crvCostosEnTodosLosCultivos.ReportSource = Report
        setResourcesToReport()

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

        Session.Add("frmReporteCostosEnTodosLosCultivosPorKilo_Report", Report)

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

    Protected Sub btnConfigurar_Click(sender As Object, e As EventArgs)

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
    End Sub

    Private Function Refresh_Query(ByRef pFilter As String) As String
        Dim rsReporte As New ADODB.Recordset
        Dim strSQL As String

        If rsReporte.State = True Then
            rsReporte.Close()
        End If

        Dim objGenericBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"
        Dim gblstrIdUsuario As String = businessUser    '"Fundo0"

        If Me.cboMoneda.SelectedIndex = 0 Then
            strSQL = "  sELECT x.orden  ,View_cosecha.jabas as jabas,View_cosecha.cantidad as cosecha ,p.* FROM ( SELECT r.id_zonatrabajo ,fundo,r.tipo_costo, r.cultivosdesc, x.zonatrabajodesc, r.id_cultivo, x.totalmontostandar2,SUM(r.monto_standar) AS totalmontostandar, AVG(CASE WHEN r.id_cultivo = 'COSTIN' OR substring(r.id_cultivo, 1, 4) = 'INVE' THEN 0 ELSE r.hectareas END) AS MáxDehectareas,X.costohectarea  FROM ResumenDeCostosPorCultivo r INNER JOIN ( SELECT zonatrabajodesc, SUM(CASE WHEN r.[hectareas] <> 0 THEN [monto_standar] / r.[hectareas] ELSE 0 END) AS costohectarea , " & " SUM(monto_standar) AS totalmontostandar2, FUNDOS.descripcion as fundo From ResumenDeCostosPorCultivo as r INNER JOIN ZONA_TRABAJO ON r.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo INNER JOIN USUARIOCCOSTOS ON ZONA_TRABAJO.id_fundo = USUARIOCCOSTOS.id_fundo  INNER JOIN FUNDOS ON ZONA_TRABAJO.id_fundo = FUNDOS.Id_fundo  WHERE ((cast(r.campana as int)=" & edtNumeroCampana.Text & " )) " & " AND (tipo_costo IN ('I', 'M', 'R', 'H', 'O')) and USUARIOCCOSTOS.Id_usuario = '" & gblstrIdUsuario & "' GROUP BY zonatrabajodesc , FUNDOS.descripcion )x on x.zonatrabajodesc=r.zonatrabajodesc   WHERE ((cast(r.campana as int)=" & edtNumeroCampana.Text & "))  AND (tipo_costo IN ('I', 'M', 'R', 'H', 'O'))  GROUP BY r.hectareas, r.tipo_costo, r.orden, r.cultivosdesc, x.zonatrabajodesc, r.id_cultivo,x.costohectarea,x.totalmontostandar2, " & " x.fundo,r.id_zonatrabajo   ) t   PIVOT (SUM(totalmontostandar) FOR [TIPO_COSTO] IN ([I],[M],[H],[R],[O])) as p  LEFT JOIN CULTIVOS x ON x.id_cultivo=p.id_cultivo LEFT JOIN ( SELECT   sum(id_enlace) as jabas,   SUM(cantidad) AS cantidad, id_zonatrabajo FROM         dbo.COSTOS WHERE     (observaciones LIKE '%cosecha%') AND (tipo_costo = 'c') AND (campana = " & edtNumeroCampana.Text & ")  GROUP BY id_zonatrabajo ) as View_cosecha  ON View_cosecha.id_zonatrabajo =p.id_zonatrabajo   ORDER BY zonatrabajodesc asc "
        Else
            strSQL = "TRANSFORM IIf((Sum([ms])) is null,0,Sum([ms])) AS monto " & "SELECT ResumenDeCostosPorCultivoPorKiloME.orden, ResumenDeCostosPorCultivoPorKiloME.cultivosdesc, ResumenDeCostosPorCultivoPorKiloME.zonatrabajodesc, ResumenDeCostosPorCultivoPorKiloME.id_cultivo, Sum(ResumenDeCostosPorCultivoPorKiloME.ms) AS totalmontostandar, Sum(ResumenDeCostosPorCultivoPorKiloME.kilos) AS SumaDekilos, (IIf(Sum([kilos])<>0,Sum([ms])/Sum([kilos]),0)) AS costokilo " & "From ResumenDeCostosPorCultivoPorKiloME "
            strSQL = strSQL & pFilter
            strSQL = strSQL & "GROUP BY ResumenDeCostosPorCultivoPorKiloME.orden, ResumenDeCostosPorCultivoPorKiloME.cultivosdesc, ResumenDeCostosPorCultivoPorKiloME.zonatrabajodesc, ResumenDeCostosPorCultivoPorKiloME.id_cultivo " & "ORDER BY ResumenDeCostosPorCultivoPorKiloME.orden " & "PIVOT ResumenDeCostosPorCultivoPorKiloME.tipo_costo In ('I', 'M', 'H', 'R', 'O')"
        End If

        Return strSQL
    End Function

    Private Sub setResourcesToReport()
        Dim Campo As TextObject
        Campo = Report.ReportDefinition.ReportObjects("text1") : Campo.Text = Resource1.str315 ' cultivo
        Campo = Report.ReportDefinition.ReportObjects("text2") : Campo.Text = Resource1.str3014 ' ZW
        Campo = Report.ReportDefinition.ReportObjects("text3") : Campo.Text = Resource1.str114 ' Cosecha
        'Campo = Report.ReportDefinition.ReportObjects("text5") : Campo.Text = Resource1.str18 ' Personal
        'Campo = Report.ReportDefinition.ReportObjects("text6") : Campo.Text = Resource1.str3012 ' Riego
        'Campo = Report.ReportDefinition.ReportObjects("text7") : Campo.Text = Resource1.str540 ' Otros
        Campo = Report.ReportDefinition.ReportObjects("text18") : Campo.Text = Resource1.str569 ' Otros
        'Campo = Report.ReportDefinition.ReportObjects("text8") : Campo.Text = Resource1.str3014 ' Total
        'Campo = Report.ReportDefinition.ReportObjects("text9") : Campo.Text = Resource1.str609 ' kilos
        Campo = Report.ReportDefinition.ReportObjects("text10") : Campo.Text = Resource1.str311 ' hectareas
        Campo = Report.ReportDefinition.ReportObjects("text12") : Campo.Text = Resource1.str10003 ' Costo / Kilo
    End Sub

    Protected Function GetConnectionInfo() As CrystalDecisions.Shared.ConnectionInfo
        Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
        Dim objBL As New GenericMethods("Fundo0")
        Dim dctInfo As New Dictionary(Of String, String)
        dctInfo = objBL.GetConnectionInfo()

        With crCon
            .ServerName = dctInfo("BDServidor")
            .UserID = dctInfo("BDUser")
            .Password = dctInfo("BDPassword")
            .DatabaseName = dctInfo("BDName")
            .Type = ConnectionInfoType.SQL
            .IntegratedSecurity = False
        End With

        Return crCon
    End Function

End Class