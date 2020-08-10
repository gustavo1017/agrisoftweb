Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinperDetail
    Inherits BasePage

    Dim Report2 As New crCostoTotalEnUnaZonaDeTrabajoPorRubrosDetAgroinper
    Dim STRID As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        setEtiquetas()

        If Not Page.IsPostBack() Then
            currentModule = "Agricultura.Reportes"
            Dim intAcceso As Integer = HabilitaFrame()

            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            Report2 = New crCostoTotalEnUnaZonaDeTrabajoPorRubrosDetAgroinper

            STRID = "" : CargarCombo("id_zonatrabajo")
            rbtnCampañaFilter.Checked = True
            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.RefreshReport()
            cboMoneda.Items.Add(Resource1.str10004) 'Nacional
            cboMoneda.Items.Add(Resource1.str10005) 'Extranjera
            edtNumeroCampana.Text = CStr(2017)

            If rbtnCampañaFilter.Checked Then
                rbtnCampañaFilter_CheckedChanged(Me, Nothing)
            End If

            If rbtnFechaDesdeFilter.Checked Then
                rbtnFechaDesdeFilter_CheckedChanged(Me, Nothing)
            End If

            hdnStr6013.Value = Resource1.str6013
            hdnStr6018.Value = Resource1.str6018
            btnVer_Click(btnVer, New System.EventArgs())
        Else
            If Session("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_Report2") IsNot Nothing Then
                Report2 = Session("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_Report2")
            End If

            Dim CrTables As Tables
            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Report2
            CrTables = Report2.Database.Tables

            'Refresh Report connection
            Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
            Dim crtableLogoninfo As New TableLogOnInfo
            Dim CrTable As Table

            crCon = GetConnectionInfo()

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

    Sub setEtiquetas()
        btnVer.Text = Resource1.str9197 'Ver
        btnConfigurar.Text = Resource1.str9196 'Configurar
        btnImprimir.Text = Resource1.str9195 'Imprimir
        btnExportarExcel.Text = Resource1.str9194 'Exportar
        Me.lblMoneda.Text = Resource1.str10003 'Moneda

        rbtnFechaDesdeFilter.Text = Resource1.str9199 'Desde
        rbtnCampañaFilter.Text = Resource1.str541 'campaña
        lblHasta.Text = Resource1.str3004 'Hasta
        lblZonaTrabajo.Text = Resource1.str301 'ZT
        Frame1.GroupingText = Resource1.str13 'Parametros
    End Sub

    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim Campo2 As TextObject

        If rbtnCampañaFilter.Checked Then
            If Trim(edtNumeroCampana.Text) = "" Then
                edtNumeroCampana.Focus()
                Exit Sub
            End If

            If Not IsNumeric(Trim(edtNumeroCampana.Text)) Then
                Exit Sub
            End If
        End If

        CargarReporte()

        Report2.SummaryInfo.ReportTitle = Resource1.str9241
        Campo2 = Report2.ReportDefinition.ReportObjects("txtmoneda") : Campo2.Text = Resource1.str10003 ' Moneda

        If Me.cboMoneda.SelectedIndex = 0 Then
            Campo2 = Report2.ReportDefinition.ReportObjects("edtmoneda") : Campo2.Text = Resource1.str10004 'Nacional
        Else
            Campo2 = Report2.ReportDefinition.ReportObjects("edtmoneda") : Campo2.Text = Resource1.str10005 'Extranjera
        End If

        If rbtnCampañaFilter.Checked Then
            If Not IsNumeric(Trim(edtNumeroCampana.Text)) Then
                Exit Sub
            End If

            Dim edtlabel As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtlabel") : edtlabel.Text = Resource1.str541
            Dim edtvalue As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtvalue") : edtvalue.Text = edtNumeroCampana.Text

            If Me.cboMoneda.SelectedIndex = 0 Then
                Dim edtmoneda As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtmoneda") : edtmoneda.Text = Resource1.str10004 'Nacional
            Else
                Dim edtmoneda As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtmoneda") : edtmoneda.Text = Resource1.str10005 'Extranjera
            End If
        Else
            Dim edtlabel As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtlabel") : edtlabel.Text = Resource1.str3003
            Dim edtvalue2 As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtvalue") : edtvalue2.Text = dtfechamin.Text & Resource1.str3004 & dtfechamax.Text

            If Me.cboMoneda.SelectedIndex = 0 Then
                Dim edtmoneda As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtmoneda") : edtmoneda.Text = Resource1.str10004 'Nacional
            Else
                Dim edtmoneda As CrystalDecisions.CrystalReports.Engine.TextObject = Report2.Sección2.ReportObjects("edtmoneda") : edtmoneda.Text = Resource1.str10005 'Extranjera
            End If
        End If

        btnConfigurar.Enabled = True
        btnImprimir.Enabled = True
        btnExportarExcel.Enabled = True
        Session.Add("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_Report2", Report2)
    End Sub

    Private Sub CargarCombo(ByVal id As String)
        Dim MatrizZ() As Object = Session("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_MatrizZ")
        Dim intOrden, i As Short
        Dim adoTabla As ADODB.Recordset

        If cboZonatrabajo.Items.Count > 0 Then intOrden = cboZonatrabajo.SelectedIndex

        Dim objBL As New ReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper("Fundo0")

        Dim objGenericBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"

        Dim gblstrIdUsuario As String = businessUser '"Fundo0"
        adoTabla = objBL.getDataZonaTrabajo(gblstrIdUsuario, id, "descripcion")

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
        Session.Add("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_MatrizZ", MatrizZ)
    End Sub

    Private Sub CargarReporte()
        Dim objBL As New ReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper("Fundo0")
        Dim MatrizZ() As Object = Session("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_MatrizZ")

        Dim searchFilter As String
        Dim MatrizZT, strCampoCampaña As String

        Dim fechaValida As Boolean = True
        Dim fechaFrom As DateTime = DateTime.Now
        Dim fechaTo As DateTime = DateTime.Now
        Dim rsReporte As New ADODB.Recordset
        Dim DetailLevelSelected As String = "Detalle"

        If rbtnCampañaFilter.Checked Then
            searchFilter = "Campaña"
            strCampoCampaña = CampoCampana((edtNumeroCampana.Text), cboMoneda.SelectedIndex)
        Else
            searchFilter = "Fecha"
        End If

        MatrizZT = MatrizZ(cboZonatrabajo.SelectedIndex)
        rsReporte = objBL.Refresh_Query(searchFilter, DetailLevelSelected, cboMoneda.SelectedIndex, MatrizZT, strCampoCampaña, dtfechamin.Text, dtfechamax.Text, edtNumeroCampana.Text)

        If rsReporte.RecordCount = 0 Then crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Nothing : Exit Sub
        Report2.Load(Report2.ResourceName) : Report2.SetDataSource(rsReporte)
        crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Report2
        Session.Add("frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper_Report2", Report2)

        crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ToolPanelView = ToolPanelViewType.None

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