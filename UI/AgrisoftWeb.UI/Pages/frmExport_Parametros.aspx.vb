Imports System.Data.OleDb
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web

Public Class frmExport_Parametros
    Inherits BasePage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strParamExport As String = ""
        Dim strFileName As String = ""
        Dim strSQL As String = ""
        Dim Report As New Object()

        If Session("frmExportParametros_ParamExport") IsNot Nothing Then
            strParamExport = Session("frmExportParametros_ParamExport")
        Else
            Exit Sub
        End If

        If Session("frmExportParametros_ReportQuery") IsNot Nothing Then
            strSQL = Session("frmExportParametros_ReportQuery")
        Else
            Exit Sub
        End If

        Dim dtData As New DataTable
        Dim Campo As TextObject

        Select Case UCase(strParamExport)
            Case "CULTIVOS"
                Report = New crCultivos()
                Dim objCultivosViewBL As New CultivosView("Fundo0")
                dtData = objCultivosViewBL.cargarDatosGrilla(strSQL)
                SetReport(Report, dtData)
                Campo = Report.ReportDefinition.ReportObjects("text5")
                Report.SummaryInfo.ReportTitle = "ArchivoTest"

            Case "ZONATRABAJO"
                Dim objZonaTrabajoBL As New ZonaTrabajoView("Fundo0")
                dtData = objZonaTrabajoBL.cargarDatosGrilla(strSQL)
                Report = New crZonaTrabajo3()
                SetReport(Report, dtData)
                Dim text1 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text1") : text1.Text = Resource1.str102 ' codigo
                Dim text2 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text2") : text2.Text = Resource1.str103 ' desc
                Dim text3 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text3") : text3.Text = Resource1.str311 'hectareas
                Dim text4 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text4") : text4.Text = Resource1.str315 'cultivo
                Dim text5 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text5") : text5.Text = Resource1.str541 'campaña
                Dim text6 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text6") : text6.Text = Resource1.str86 'parametro del sistema
                Dim text7 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text7") : text7.Text = Resource1.str11032 'fundo

            Case "ACTIVIDADES"
                Report = New crActividades()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)
                Dim text1 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text1") : text1.Text = Resource1.str102 ' codigo
                Dim text2 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text2") : text2.Text = Resource1.str103 ' DESCRIPCION
                Dim text3 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text3") : text3.Text = Resource1.str6003 ' UBICACION CC

            Case "PERSONAL"
                Report = New CrPersonal()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)
                Dim text13 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section6.ReportObjects("text13") : text13.Text = Resource1.str3053 ' estado
                Dim text15 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section6.ReportObjects("text15") : text15.Text = Resource1.str10138 ' costo hora planilla
                Dim text16 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section6.ReportObjects("text16") : text16.Text = Resource1.str514 ' costo hora standar
                Dim text17 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section6.ReportObjects("text17") : text17.Text = Resource1.str10102 '25%
                Dim text1 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section6.ReportObjects("text1") : text1.Text = Resource1.str10103 '35%
                Dim text19 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section6.ReportObjects("text19") : text19.Text = Resource1.str10148 'ciclo de pago

            Case "PRODUCTOS"
                Report = New CrProductos()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)
                Dim text5 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section1.ReportObjects("text5") : text5.Text = Resource1.str16 ' titulo
                Dim text2 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section1.ReportObjects("text2") : text2.Text = Resource1.str111 ' tipo
                Dim text3 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section1.ReportObjects("text3") : text3.Text = Resource1.str16 ' desc
                Dim text4 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section1.ReportObjects("text4") : text4.Text = Resource1.str3008 ' costou
                Dim text20 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.section1.ReportObjects("text20") : text20.Text = Resource1.str10003 ' moneda

            Case "MAQUINAS"
                Report = New crMaquinarias()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)
                Dim text7 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text7") : text7.Text = Resource1.str17 ' TITULO
                Dim text2 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text2") : text2.Text = Resource1.str102 ' CODIGO
                Dim text3 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text3") : text3.Text = Resource1.str103 ' DESCRIPCION
                Dim text4 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text4") : text4.Text = Resource1.str111 ' TIPO
                Dim text5 As CrystalDecisions.CrystalReports.Engine.TextObject = Report.Section2.ReportObjects("text5") : text5.Text = Resource1.str514 ' costo hora

            Case "COSTOSWEB"
                Report = New CrCostosWeb()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)

            Case "COSECHAS"
                Report = New CrCosechas()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)

            Case "INGRESOSALMACEN"
                Report = New CrIngresosAlmacen()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)


            Case "ETAPAS"
                'Report = New CrCostosWeb()
                'dtData = cargarDataReporte(strSQL)
                'SetReport(Report, dtData)

            Case "LINEAPRODUCTO"
                Report = New CrLineasP()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)

            Case "PROVEEDORES"
                Report = New CrProveedores()
                dtData = cargarDataReporte(strSQL)
                SetReport(Report, dtData)
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
    End Sub

    Public Sub SetReport(ByVal Report As Object, dtData As DataTable)
        Report.Load(Report.ResourceName)
        Report.SetDataSource(dtData)
        crvReport.ReportSource = Report
        crvReport.ToolPanelView = ToolPanelViewType.None
    End Sub

    Public Function cargarDataReporte(ByVal sQuery As String) As DataTable
        Dim RS As New ADODB.Recordset
        Dim ssql As String = sQuery
        Dim DBconn As New ADODB.Connection

        If RS.State = 1 Then
            RS.Close()
        End If

        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        RS.LockType = ADODB.LockTypeEnum.adLockReadOnly
        RS.let_Source(ssql)
        RS.Open()

        Dim sqlAdapter As New OleDbDataAdapter()
        Dim dtData As New DataTable()
        sqlAdapter.Fill(dtData, RS)

        If RS.State = 1 Then
            RS.Close()
        End If

        Return dtData
    End Function

End Class