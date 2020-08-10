Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class frmResumenCosechaporcampanas
    Inherits BasePage

    Dim Report As New CrCosechasxCampanasDif2

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        setEtiquetas()

        If Not Page.IsPostBack() Then
            currentModule = "Agricultura.Reportes"
            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            hdnStr6013.Value = Resource1.str6013
            hdnStr6018.Value = Resource1.str6018

            ' Load Report only first load
            Dim dtData As New DataTable
            Dim ssql As String = Refresh_Query(" WHERE ZONA_TRABAJO.id_cultivo='' ")
            dtData = cargarDataTable(ssql)

            If dtData.Rows.Count() = 0 Then
                crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Nothing
                Exit Sub
            End If

            Report.Load(Report.ResourceName) : Report.SetDataSource(dtData)
            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Report
            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ToolPanelView = ToolPanelViewType.None
            setResourcesToReport()

            Session.Add("frmResumenCosechaporcampanas_Report", Report)
        Else
            If Session("frmResumenCosechaporcampanas_Report") IsNot Nothing Then
                Report = Session("frmResumenCosechaporcampanas_Report")
                crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Report
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

            For i As Integer = 0 To crvCostoTotalEnUnaZonaDeTrabajoPorRubros.LogOnInfo.Count - 1
                crvCostoTotalEnUnaZonaDeTrabajoPorRubros.LogOnInfo(i).ConnectionInfo = crCon
            Next i

            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReuseParameterValuesOnRefresh = True
        End If
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

        For i As Integer = 0 To crvCostoTotalEnUnaZonaDeTrabajoPorRubros.LogOnInfo.Count - 1
            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.LogOnInfo(i).ConnectionInfo = crCon
        Next i

        sFilter = "  WHERE SUBSTRING(COSTOS.observaciones, 1, 7) = 'Cosecha' AND COSTOS.tipo_costo = 'C' "
        Dim ssql As String = Refresh_Query(sFilter)

        Dim dtData As New DataTable
        dtData = cargarDataTable(ssql)

        If dtData.Rows.Count() = 0 Then
            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Nothing
            Exit Sub
        End If

        Report.Load(Report.ResourceName)
        Report.SetDataSource(dtData)
        crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ToolPanelView = ToolPanelViewType.None
        crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Report
        setResourcesToReport()

        Session.Add("frmResumenCosechaporcampanas_Report", Report)
    End Sub

    Private Sub setEtiquetas()
        Title = Resource1.str3032
        btnVer.Text = Resource1.str9197 'Ver
    End Sub

    Private Function Refresh_Query(ByRef pFilter As String) As String
        Dim Strc1 As String
        Dim Strc2 As String
        Dim Strc3 As String

        Strc1 = IIf(Len(Trim(text1.Text)) = 0, "0", Trim(text1.Text))
        Strc2 = IIf(Len(Trim(text2.Text)) = 0, "-1", Trim(text2.Text))
        Strc3 = IIf(Len(Trim(text3.Text)) = 0, "-2", Trim(text3.Text))

        Dim objBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"

        Dim gblstrIdUsuario As String = businessUser    '"Fundo0"
        Dim strSQL As String = " select q.fundo,q.fechasiembra,q.orden,q.cultivosdesc ,q.zonatrabajodesc , q.hectareas , sum(q.c1) as c1 ,sum(q.c2) as c2,sum(q.c3) as c3,case when sum(q.c1) =0 then 0 else sum(q.dif1)/sum(q.c1) end * 100 as difp1 , case when sum(q.c2) =0 then 0 else sum(q.dif2)/sum(q.c2) end * 100 as difp2 , sum(q.dif1) as dif1 , sum(q.dif2) as dif2 from (  SELECT x.orden ,p.fundo,p.fechasiembra,p.cultivosdesc ,p.zonatrabajodesc , p.hectareas , case when p.[" & Strc1 & "] is null then 0 else p.[" & Strc1 & "] end as c1 ," & " case when p.[" & Strc2 & "] is null then 0 else p.[" & Strc2 & "] end  as c2,case when p.[" & Strc3 & "] is null then 0 else p.[" & Strc3 & "] end  as c3, case when p.[" & Strc2 & "] is null then 0 else p.[" & Strc2 & "] end - case when p.[" & Strc1 & "] is null then 0 else p.[" & Strc1 & "] end  as dif1 ,  case when p.[" & Strc3 & "] is null then 0 else p.[" & Strc3 & "] end - case when p.[" & Strc2 & "] is null then 0 else p.[" & Strc2 & "] end  as dif2 FROM ( SELECT r.fundo,r.fechasiembra,r.campana, r.zonatrabajodesc, SUM(CASE WHEN r.[hectareas] <> 0 THEN [cantidad] / r.[hectareas] ELSE 0 END) " & " AS costohectarea, SUM(r.cantidad) AS totalmontostandar2, MAX(ZONA_TRABAJO_1.hectareas) AS hectareas, r.cultivosdesc,  r.id_cultivo, CULTIVOS_1.orden   FROM ( SELECT     CULTIVOS.orden, ZONA_TRABAJO.descripcion AS zonatrabajodesc, CASE WHEN TIPO_COSTO.id_tipocosto = 'O' THEN 'Otros' ELSE TIPO_COSTO.descripcion END AS tipocostodesc, COSTOS.cantidad, ZONA_TRABAJO.hectareas, CASE WHEN [hectareas] <> 0 THEN [cantidad] / [hectareas] ELSE 0 END AS costohectaria, COSTOS.fecha, " & " CULTIVOS.id_cultivo, COSTOS.campana, CULTIVOS.descripcion AS cultivosdesc, COSTOS.tipo_costo, COSTOS.id_costo, COSTOS.id_actividad, COSTOS.id_zonatrabajo, COSTOS.nro_ordenprod, COSTOS.observaciones, FUNDOS.descripcion as fundo, ZONA_TRABAJO.fechasiembra FROM CULTIVOS INNER JOIN ZONA_TRABAJO ON CULTIVOS.id_cultivo = ZONA_TRABAJO.id_cultivo INNER JOIN TIPO_COSTO INNER JOIN COSTOS ON TIPO_COSTO.id_tipocosto = COSTOS.tipo_costo ON ZONA_TRABAJO.id_zonatrabajo = COSTOS.id_zonatrabajo INNER JOIN " & " FUNDOS ON ZONA_TRABAJO.id_fundo = FUNDOS.Id_fundo WHERE (COSTOS.tipo_costo = 'C') AND (COSTOS.observaciones LIKE '%cosecha%') ) AS r INNER JOIN ZONA_TRABAJO AS ZONA_TRABAJO_1 ON r.id_zonatrabajo = ZONA_TRABAJO_1.id_zonatrabajo   INNER JOIN USUARIOCCOSTOS ON ZONA_TRABAJO_1.id_fundo = USUARIOCCOSTOS.id_fundo INNER JOIN CULTIVOS AS CULTIVOS_1 ON ZONA_TRABAJO_1.id_cultivo = CULTIVOS_1.id_cultivo  WHERE     (r.observaciones LIKE '%cosecha%') AND (USUARIOCCOSTOS.Id_usuario = '" & gblstrIdUsuario & "') " & " GROUP BY r.zonatrabajodesc, r.fundo,r.fechasiembra,r.campana, r.cultivosdesc, r.id_cultivo, CULTIVOS_1.orden  HAVING (r.campana IN (" & Strc1 & ", " & Strc2 & ", " & Strc3 & ")) )x PIVOT (SUM(totalmontostandar2) FOR x.campana IN ([" & Strc1 & "],[" & Strc2 & "],[" & Strc3 & "]))  as p  LEFT JOIN CULTIVOS x ON x.id_cultivo=p.id_cultivo ) as q group by q.fundo,q.fechasiembra,q.orden,q.cultivosdesc ,q.zonatrabajodesc , q.hectareas   "
        Return strSQL
    End Function

    Private Sub setResourcesToReport()
        Dim Campo As TextObject
        Campo = Report.ReportDefinition.ReportObjects("text2") : Campo.Text = Resource1.str3034 ' Kilos
        Campo = Report.ReportDefinition.ReportObjects("text1") : Campo.Text = Resource1.str9024 ' Monto / ha
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