Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class frmReporteSaldosDeAlmacen
    Inherits BasePage

    Dim Report As New crsaldosalmacen
    'Dim MatrizAL() As Object
    'Dim rsReporte As New ADODB.Recordset

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim fechaFrom As DateTime = DateTime.Now
        SetEtiquetas()

        If Not Page.IsPostBack() Then
            'Dim gblstrIdUsuario As String = "Fundo0"
            currentModule = "Agricultura.Reportes"
            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            dtfechamin.Text = DateTime.Now.ToShortDateString
            hdnStr6013.Value = Resource1.str6013
            hdnStr6018.Value = Resource1.str6018

            ' Load Report only first load 
            Dim dtData As New DataTable
            Dim ssql As String = " SELECT SaldosAlmacenProductosCompras.id_almacen, SaldosAlmacenProductosCompras.expr1, SaldosAlmacenProductosCompras.id_producto, SaldosAlmacenProductosCompras.descripcion, Sum(Cantidad*case when Tipo_costo='I' Or Tipo_costo='S' Or Tipo_costo='T' then -1 else 1 end) AS Cantidad2, SaldosAlmacenProductosCompras.costo, PRODUCTOS.Stock_min " & " FROM SaldosAlmacenProductosCompras INNER JOIN PRODUCTOS ON SaldosAlmacenProductosCompras.id_producto = PRODUCTOS.id_producto "
            'Id Almacen y fecha tienen valores por defecto para esta versión Web, no se ha considerado los filtros como en la version Windows
            ssql &= "WHERE fecha <=convert(datetime,replace('" & DateTime.Parse(dtfechamin.Text).ToString("yyyyMMdd") & "',',','/')) And id_almacen='01'  and SaldosAlmacenProductosCompras.id_producto in (  select SaldosAlmacenProductosCompras.id_producto from SaldosAlmacenProductosCompras where id_almacen='01'  group by id_producto having max(fecha) >= '20000101') "
            ssql &= "GROUP BY SaldosAlmacenProductosCompras.id_almacen, SaldosAlmacenProductosCompras.expr1, SaldosAlmacenProductosCompras.id_producto, SaldosAlmacenProductosCompras.descripcion, SaldosAlmacenProductosCompras.costo, PRODUCTOS.Stock_min ORDER BY SaldosAlmacenProductosCompras.expr1, SaldosAlmacenProductosCompras.descripcion "
            dtData = cargarDataTable(ssql)

            If dtData.Rows.Count() = 0 Then
                crvCostosEnTodosLosCultivos.ReportSource = Nothing
                Exit Sub
            End If

            Report.Load(Report.ResourceName) : Report.SetDataSource(dtData)
            crvCostosEnTodosLosCultivos.ReportSource = Report
            crvCostosEnTodosLosCultivos.ToolPanelView = ToolPanelViewType.None
            setResourcesToReport()

            Session.Add("frmReporteSaldosDeAlmacen_Report", Report)
        Else
            If Session("frmReporteSaldosDeAlmacen_Report") IsNot Nothing Then
                Report = Session("frmReporteSaldosDeAlmacen_Report")
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
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
    End Sub

    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Dim sFilter As String
        sFilter = "WHERE productos.tipo='I' and fecha <=convert(datetime,replace('" & DateTime.Parse(dtfechamin.Text).ToString("yyyyMMdd") & "',',','/')) And id_almacen='01'  and SaldosAlmacenProductosCompras.id_producto in (  select SaldosAlmacenProductosCompras.id_producto from SaldosAlmacenProductosCompras where id_almacen='01'  group by id_producto having max(fecha) >= '20000101')  "
        Dim ssql As String = Refresh_Query(sFilter)

        Dim dtData As New DataTable
        dtData = cargarDataTable(ssql)

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

        Dim fechaFrom As DateTime ' = DateTime.ParseExact(dtfechamin.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None)
        DateTime.TryParseExact(dtfechamin.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechaFrom)

        If fechaFrom = DateTime.MinValue Then
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "text", "ShowErrorMessage(Resource1.str99992)", True)
            'lblMessage.Text = "El formato de la fecha debe ser dd/MM/yyyy"
            crvCostosEnTodosLosCultivos.ReportSource = Nothing
            Exit Sub
        End If

        If dtData.Rows.Count() = 0 Then
            crvCostosEnTodosLosCultivos.ReportSource = Nothing
            Exit Sub
        End If

        Report.Load(Report.ResourceName)
        Report.SetDataSource(dtData)
        crvCostosEnTodosLosCultivos.ToolPanelView = ToolPanelViewType.None
        crvCostosEnTodosLosCultivos.ReportSource = Report
        setResourcesToReport()

        'Save on Session the values needed for report 
        If String.IsNullOrEmpty(dtfechamin.Text) Then
            Session.Add("frmReporteSaldosDeAlmacen_dtFechaMin", hdnDateFrom.Value)
        Else
            Dim datefromConverted As DateTime
            If DateTime.TryParse(dtfechamin.Text, datefromConverted) Then
                Session.Add("frmReporteSaldosDeAlmacen_dtFechaMin", datefromConverted)
            Else
                Session.Add("frmReporteSaldosDeAlmacen_dtFechaMin", Nothing)
            End If
        End If

        Session.Add("frmReporteSaldosDeAlmacen_Report", Report)
    End Sub

    Private Function Refresh_Query(ByVal pFilter As String) As String
        Dim ssql As String = " SELECT SaldosAlmacenProductosCompras.id_almacen, SaldosAlmacenProductosCompras.expr1, SaldosAlmacenProductosCompras.id_producto, SaldosAlmacenProductosCompras.descripcion, Sum(Cantidad*case when Tipo_costo='I' Or Tipo_costo='S' Or Tipo_costo='T' then -1 else 1 end) AS Cantidad2, SaldosAlmacenProductosCompras.costo, PRODUCTOS.Stock_min " & " FROM SaldosAlmacenProductosCompras INNER JOIN PRODUCTOS ON SaldosAlmacenProductosCompras.id_producto = PRODUCTOS.id_producto "
        'Id Almacen y fecha tienen valores por defecto para esta versión Web, no se ha considerado los filtros como en la version Windows
        ssql &= pFilter
        ssql &= "GROUP BY SaldosAlmacenProductosCompras.id_almacen, SaldosAlmacenProductosCompras.expr1, SaldosAlmacenProductosCompras.id_producto, SaldosAlmacenProductosCompras.descripcion, SaldosAlmacenProductosCompras.costo, PRODUCTOS.Stock_min ORDER BY SaldosAlmacenProductosCompras.expr1, SaldosAlmacenProductosCompras.descripcion "

        Return ssql
    End Function

    Private Sub SetEtiquetas()
        lblFecha.Text = Resource1.str504
        Title = Resource1.str9205
        btnVer.Text = Resource1.str9197 'Ver
        btnConfigurar.Text = Resource1.str9196 'Configurar
        btnImprimir.Text = Resource1.str9195 'Imprimir
        btnExportarExcel.Text = Resource1.str9194 'Exportar
    End Sub

    Private Sub setResourcesToReport()
        Dim Campo As TextObject
        Campo = Report.ReportDefinition.ReportObjects("texto6") : Campo.Text = Resource1.str9205 ' Titulo
        Campo = Report.ReportDefinition.ReportObjects("texto1") : Campo.Text = Resource1.str9205 ' codigo
        Campo = Report.ReportDefinition.ReportObjects("texto2") : Campo.Text = Resource1.str9205 ' descripcion
        Campo = Report.ReportDefinition.ReportObjects("texto4") : Campo.Text = Resource1.str9205 ' cantidad
        Campo = Report.ReportDefinition.ReportObjects("edtRangoFecha") : Campo.Text = Resource1.str3004 + DateTime.Parse(dtfechamin.Text).ToString("dd/MM/yyyy")
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