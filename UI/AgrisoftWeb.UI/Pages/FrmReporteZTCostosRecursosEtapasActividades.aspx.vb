Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class FrmReporteZTCostosRecursosEtapasActividades
    Inherits BasePage

    Dim Report As New CrCultivoZwRecEtaAct
    Dim MatrizZ() As Object

    Dim STRID As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        setEtiquetas()

        If Not Page.IsPostBack() Then
            Me.currentModule = "Agricultura.Reportes"
            Dim intAcceso As Integer = HabilitaFrame()

            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            Report = New CrCultivoZwRecEtaAct

            STRID = "" : CargarCombo("id_zonatrabajo")
            rbtnCampañaFilter.Checked = True
            edtNumeroCampana.Text = CStr(2017)

            crvCostoTotalEnUnaZonaDeTrabajoPorRubros.RefreshReport()

            'Select ZonaTrabajo from the previous report
            Dim strZonaTrabajo As String = ""
            If Not String.IsNullOrEmpty(Request.QueryString("ZonaTrabajo")) Then
                strZonaTrabajo = Request.QueryString("ZonaTrabajo").ToString()
                If cboZonatrabajo.Items.FindByText(strZonaTrabajo) IsNot Nothing Then
                    cboZonatrabajo.ClearSelection()
                    cboZonatrabajo.Items.FindByText(strZonaTrabajo).Selected = True
                End If
            End If

            cboMoneda.Items.Add(Resource1.str10004) 'Nacional
            cboMoneda.Items.Add(Resource1.str10005) 'Extrangera

            'Get the session values
            If Not String.IsNullOrEmpty(Session("FrmReporteZTCostosRecursosEtapasActividades_NumeroCampana")) Then
                edtNumeroCampana.Text = Session("FrmReporteZTCostosRecursosEtapasActividades_NumeroCampana").ToString()
            End If

            If Not String.IsNullOrEmpty(Session("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMin")) Then
                dtfechamin.Text = Convert.ToDateTime(Session("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMin")).ToString("dd/MM/yyyy")
            End If

            If Not String.IsNullOrEmpty(Session("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMax")) Then
                dtfechamax.Text = Convert.ToDateTime(Session("FrmReporteZTCostosRecursosEtapasActividades_dtFechaMax")).ToString("dd/MM/yyyy")
            End If

            If Not String.IsNullOrEmpty(Session("FrmReporteZTCostosRecursosEtapasActividades_cboMonedaSelectedIndex")) Then
                cboMoneda.SelectedIndex = Convert.ToInt32(Session("FrmReporteZTCostosRecursosEtapasActividades_cboMonedaSelectedIndex"))
            End If

            If Not String.IsNullOrEmpty(Session("FrmReporteZTCostosRecursosEtapasActividades_rbtnCampañaSelected")) Then
                rbtnCampañaFilter.Checked = Convert.ToBoolean(Session("FrmReporteZTCostosRecursosEtapasActividades_rbtnCampañaSelected"))
            End If

            If Not String.IsNullOrEmpty(Session("FrmReporteZTCostosRecursosEtapasActividades_rbtnDesdeSelected")) Then
                rbtnFechaDesdeFilter.Checked = Convert.ToBoolean(Session("FrmReporteZTCostosRecursosEtapasActividades_rbtnDesdeSelected"))
            End If

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
            If Session("FrmReporteZTCostosRecursosEtapasActividades_Report") IsNot Nothing Then
                Report = Session("FrmReporteZTCostosRecursosEtapasActividades_Report")
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

    Sub setEtiquetas()
        btnVer.Text = Resource1.str9197 'Ver
        'Me.Text = Resource1.str9241
        btnConfigurar.Text = Resource1.str9196 'Configurar
        btnImprimir.Text = Resource1.str9195 'Imprimir
        btnExportarExcel.Text = Resource1.str9194 'Exportar
        Me.lblMoneda.Text = Resource1.str10003 'Moneda

        rbtnFechaDesdeFilter.Text = Resource1.str9199 'Desde
        rbtnCampañaFilter.Text = Resource1.str541 'campaña
        lblHasta.Text = Resource1.str3004 'Hasta
        lblZonaTrabajo.Text = Resource1.str301 'ZT
        Frame1.GroupingText = Resource1.str13 'Parametros
        'btnOrdenar.Text = Resource1.str23 'Ordenar actividades

    End Sub

    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Dim crCon As New CrystalDecisions.Shared.ConnectionInfo
        Dim crtableLogoninfo As New TableLogOnInfo
        'Dim CrTables As Tables
        'Dim CrTable As Table
        Dim Campo As TextObject

        'With crCon
        '    .ServerName = "AGRISOFT-PC"
        '    .UserID = "sa"
        '    .Password = "123456"
        '    .DatabaseName = "romex"
        '    '.Type = ConnectionInfoType.SQL
        'End With

        'Report = New CrCultivoZwRecEtaAct()

        'CrTables = Report.Database.Tables
        'For Each CrTable In CrTables
        '    crtableLogoninfo = CrTable.LogOnInfo
        '    crtableLogoninfo.ConnectionInfo = crCon
        '    CrTable.ApplyLogOnInfo(crtableLogoninfo)
        'Next

        If rbtnCampañaFilter.Checked Then
            If Trim(edtNumeroCampana.Text) = "" Then
                'MsgBox(My.Resources.Resource1.Resource1.str6018)
                edtNumeroCampana.Focus()
                Exit Sub
            End If

            If Not IsNumeric(Trim(edtNumeroCampana.Text)) Then
                'MsgBox(My.Resources.Resource1.Resource1.str6013)
                Exit Sub
            End If
        End If

        CargarReporte()

        Report.SummaryInfo.ReportTitle = Resource1.str9241
        Campo = Report.ReportDefinition.ReportObjects("txtmoneda") : Campo.Text = Resource1.str10003 ' Moneda

        If Me.cboMoneda.SelectedIndex = 0 Then
            Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10004 'Nacional
        Else
            Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10005 'Extranjera
        End If

        If rbtnCampañaFilter.Checked Then
            If Not IsNumeric(Trim(edtNumeroCampana.Text)) Then
                'MsgBox(Resource1.str6013)
                Exit Sub
            End If

            Campo = Report.ReportDefinition.ReportObjects("edtvalue") : Campo.Text = edtNumeroCampana.Text
            Campo = Report.ReportDefinition.ReportObjects("txtmoneda") : Campo.Text = Resource1.str10003 'Moneda

            If Me.cboMoneda.SelectedIndex = 0 Then
                Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10004 'Nacional
            Else
                Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10005 'Extranjera
            End If

            If Me.cboMoneda.SelectedIndex = 0 Then
                Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10004 'Nacional
            Else
                Campo = Report.ReportDefinition.ReportObjects("edtmoneda") : Campo.Text = Resource1.str10005 'Extranjera
            End If
        End If

        btnConfigurar.Enabled = True
        btnImprimir.Enabled = True
        btnExportarExcel.Enabled = True

        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_Report", Report)
    End Sub

    Private Sub CargarCombo(ByVal id As String)
        Dim intOrden, i As Short
        Dim adoTabla As ADODB.Recordset

        If cboZonatrabajo.Items.Count > 0 Then intOrden = cboZonatrabajo.SelectedIndex
        'cboZonatrabajo.Items.Clear() : cboZonatrabajo.AutoCompleteSource = AutoCompleteSource.ListItems : cboZonatrabajo.AutoCompleteMode = AutoCompleteMode.Suggest

        Dim objBL As New ReporteZTCostosRecursosEtapasActividades("Fundo0")

        Dim objGenericBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"

        Dim gblstrIdUsuario As String = businessUser '"Fundo0"
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
        Session.Add("FrmReporteZTCostosRecursosEtapasActividades_MatrizZ", MatrizZ)
    End Sub

    Private Sub CargarReporte()
        Dim objBL As New ReporteZTCostosRecursosEtapasActividades("Fundo0")
        Dim fechaFrom As DateTime
        Dim fechaTo As DateTime
        Dim rsReporte As New ADODB.Recordset

        MatrizZ = Session("FrmReporteZTCostosRecursosEtapasActividades_MatrizZ")

        If rbtnCampañaFilter.Checked Then
            If cboMoneda.SelectedIndex = 0 Then
                rsReporte = objBL.Refresh_Query("SELECT ZONA_TRABAJO.id_zonatrabajo AS ZONA_TRABAJO_id_zonatrabajo, ZONA_TRABAJO.descripcion AS ZONA_TRABAJO_descripcion, ZONA_TRABAJO.hectareas AS ZONA_TRABAJO_hectareas, CULTIVOS.id_cultivo AS CULTIVOS_id_cultivo, CULTIVOS.descripcion AS CULTIVOS_descripcion, ETAPAS.id_etapa AS ETAPAS_id_etapa, ETAPAS.descripcion AS ETAPAS_descripcion, ETAPAS.ubicacion_cc AS ETAPAS_ubicacion_cc, ACTIVIDADES.id_actividad AS ACTIVIDADES_id_actividad, ACTIVIDADES.descripcion AS ACTIVIDADES_descripcion, ACTIVIDADES.ubicacion_cc AS ACTIVIDADES_ubicacion_cc, TIPO_COSTO.orden AS TIPO_COSTO_orden, upper(TIPO_COSTO.descripcion) AS TIPO_COSTO_descripcion, COSTOS.tipo_costo AS COSTOS_tipo_costo, " & "case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end AS INSUMOS_descripcion, Sum(COSTOS.cantidad) AS COSTOS_cantidad, Sum(COSTOS.monto_standar) AS COSTOS_monto_standar " & "FROM (CULTIVOS INNER JOIN ((((((COSTOS INNER JOIN ACTIVIDADES ON COSTOS.id_actividad = ACTIVIDADES.id_actividad) INNER JOIN ETAPAS ON ACTIVIDADES.id_etapa = ETAPAS.id_etapa) INNER JOIN MAQUINAS ON COSTOS.id_maquinaria = MAQUINAS.id_maquinaria) INNER JOIN PERSONAL ON COSTOS.id_personal = PERSONAL.id_personal) INNER JOIN PRODUCTOS ON COSTOS.id_producto = PRODUCTOS.id_producto) INNER JOIN ZONA_TRABAJO ON COSTOS.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo) ON CULTIVOS.id_cultivo = ZONA_TRABAJO.id_cultivo) INNER JOIN TIPO_COSTO ON COSTOS.tipo_costo = TIPO_COSTO.id_tipocosto " & "Where COSTOS.PRECIO_CONTABLE>=0 AND COSTOS.Tipo_Costo <> 'C' And COSTOS.Tipo_Costo <> 'V' And COSTOS.Tipo_Costo <> 'T' And COSTOS.Tipo_Costo <> 'E' And COSTOS.Tipo_Costo <> 'S' And COSTOS.Tipo_Costo <> 'D'  And ((" & CampoCampana((edtNumeroCampana.Text), cboMoneda.SelectedIndex) & "= " & edtNumeroCampana.Text & ")) " & "GROUP BY ZONA_TRABAJO.id_zonatrabajo, ZONA_TRABAJO.descripcion, ZONA_TRABAJO.hectareas, CULTIVOS.id_cultivo, CULTIVOS.descripcion, ETAPAS.id_etapa, ETAPAS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.id_actividad, ACTIVIDADES.descripcion, ACTIVIDADES.ubicacion_cc, TIPO_COSTO.orden, upper(TIPO_COSTO.descripcion), COSTOS.tipo_costo, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end " & "Having (((zona_trabajo.id_zonatrabajo) = '" & MatrizZ(cboZonatrabajo.SelectedIndex) & "')) " & "ORDER BY ZONA_TRABAJO.descripcion, CULTIVOS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.ubicacion_cc, Sum(COSTOS.monto_standar) DESC, TIPO_COSTO.orden, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end")
            Else
                rsReporte = objBL.Refresh_Query("SELECT ZONA_TRABAJO.id_zonatrabajo AS ZONA_TRABAJO_id_zonatrabajo, ZONA_TRABAJO.descripcion AS ZONA_TRABAJO_descripcion, ZONA_TRABAJO.hectareas AS ZONA_TRABAJO_hectareas, CULTIVOS.id_cultivo AS CULTIVOS_id_cultivo, CULTIVOS.descripcion AS CULTIVOS_descripcion, ETAPAS.id_etapa AS ETAPAS_id_etapa, ETAPAS.descripcion AS ETAPAS_descripcion, ETAPAS.ubicacion_cc AS ETAPAS_ubicacion_cc, ACTIVIDADES.id_actividad AS ACTIVIDADES_id_actividad, ACTIVIDADES.descripcion AS ACTIVIDADES_descripcion, ACTIVIDADES.ubicacion_cc AS ACTIVIDADES_ubicacion_cc, TIPO_COSTO.orden AS TIPO_COSTO_orden, upper(TIPO_COSTO.descripcion) AS TIPO_COSTO_descripcion, COSTOSME.tipo_costo AS COSTOS_tipo_costo, " & "case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end AS INSUMOS_descripcion, Sum(COSTOSME.cantidad) AS COSTOS_cantidad, Sum(COSTOSME.MS) AS COSTOS_monto_standar " & "FROM (CULTIVOS INNER JOIN ((((((COSTOSME INNER JOIN ACTIVIDADES ON COSTOSME.id_actividad = ACTIVIDADES.id_actividad) INNER JOIN ETAPAS ON ACTIVIDADES.id_etapa = ETAPAS.id_etapa) INNER JOIN MAQUINAS ON COSTOSME.id_maquinaria = MAQUINAS.id_maquinaria) INNER JOIN PERSONAL ON COSTOSME.id_personal = PERSONAL.id_personal) INNER JOIN PRODUCTOS ON COSTOSME.id_producto = PRODUCTOS.id_producto) INNER JOIN ZONA_TRABAJO ON COSTOSME.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo) ON CULTIVOS.id_cultivo = ZONA_TRABAJO.id_cultivo) INNER JOIN TIPO_COSTO ON COSTOSME.tipo_costo = TIPO_COSTO.id_tipocosto " & "Where COSTOSME.PC>=0 AND COSTOSME.Tipo_Costo <> 'C' And COSTOSME.Tipo_Costo <> 'V' And COSTOSME.Tipo_Costo <> 'T' And COSTOSME.Tipo_Costo <> 'E' And COSTOSME.Tipo_Costo <> 'S' And COSTOSme.Tipo_Costo <> 'D' And ((" & CampoCampana((edtNumeroCampana.Text), cboMoneda.SelectedIndex) & "= " & edtNumeroCampana.Text & ")) " & "GROUP BY ZONA_TRABAJO.id_zonatrabajo, ZONA_TRABAJO.descripcion, ZONA_TRABAJO.hectareas, CULTIVOS.id_cultivo, CULTIVOS.descripcion, ETAPAS.id_etapa, ETAPAS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.id_actividad, ACTIVIDADES.descripcion, ACTIVIDADES.ubicacion_cc, TIPO_COSTO.orden, upper(TIPO_COSTO.descripcion), COSTOSME.tipo_costo, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end " & "Having (((zona_trabajo.id_zonatrabajo) = '" & MatrizZ(cboZonatrabajo.SelectedIndex) & "')) " & "ORDER BY ZONA_TRABAJO.descripcion, CULTIVOS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.ubicacion_cc, Sum(COSTOSME.MS) DESC, TIPO_COSTO.orden, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end")
            End If
        Else
            fechaFrom = DateTime.Parse(dtfechamin.Text)
            fechaTo = DateTime.Parse(dtfechamax.Text)

            If cboMoneda.SelectedIndex = 0 Then
                rsReporte = objBL.Refresh_Query("SELECT ZONA_TRABAJO.id_zonatrabajo AS ZONA_TRABAJO_id_zonatrabajo, ZONA_TRABAJO.descripcion AS ZONA_TRABAJO_descripcion, ZONA_TRABAJO.hectareas AS ZONA_TRABAJO_hectareas, CULTIVOS.id_cultivo AS CULTIVOS_id_cultivo, CULTIVOS.descripcion AS CULTIVOS_descripcion, ETAPAS.id_etapa AS ETAPAS_id_etapa, ETAPAS.descripcion AS ETAPAS_descripcion, ETAPAS.ubicacion_cc AS ETAPAS_ubicacion_cc, ACTIVIDADES.id_actividad AS ACTIVIDADES_id_actividad, ACTIVIDADES.descripcion AS ACTIVIDADES_descripcion, ACTIVIDADES.ubicacion_cc AS ACTIVIDADES_ubicacion_cc, TIPO_COSTO.orden AS TIPO_COSTO_orden, upper(TIPO_COSTO.descripcion) AS TIPO_COSTO_descripcion, COSTOS.tipo_costo AS COSTOS_tipo_costo, " & "case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end AS INSUMOS_descripcion, Sum(COSTOS.cantidad) AS COSTOS_cantidad, Sum(COSTOS.monto_standar) AS COSTOS_monto_standar " & "FROM (CULTIVOS INNER JOIN ((((((COSTOS INNER JOIN ACTIVIDADES ON COSTOS.id_actividad = ACTIVIDADES.id_actividad) INNER JOIN ETAPAS ON ACTIVIDADES.id_etapa = ETAPAS.id_etapa) INNER JOIN MAQUINAS ON COSTOS.id_maquinaria = MAQUINAS.id_maquinaria) INNER JOIN PERSONAL ON COSTOS.id_personal = PERSONAL.id_personal) INNER JOIN PRODUCTOS ON COSTOS.id_producto = PRODUCTOS.id_producto) INNER JOIN ZONA_TRABAJO ON COSTOS.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo) ON CULTIVOS.id_cultivo = ZONA_TRABAJO.id_cultivo) INNER JOIN TIPO_COSTO ON COSTOS.tipo_costo = TIPO_COSTO.id_tipocosto " & "Where COSTOS.PRECIO_CONTABLE>=0 AND COSTOS.Tipo_Costo <>'C' And COSTOS.Tipo_Costo <> 'V' And COSTOS.Tipo_Costo <> 'E' And COSTOS.Tipo_Costo <> 'S' And COSTOS.Tipo_Costo <> 'T'  And COSTOS.Tipo_Costo <> 'D' And (((COSTOS.fecha) >= convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) " & "And (COSTOS.fecha) <= convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')))) " & "GROUP BY ZONA_TRABAJO.id_zonatrabajo, ZONA_TRABAJO.descripcion, ZONA_TRABAJO.hectareas, CULTIVOS.id_cultivo, CULTIVOS.descripcion, ETAPAS.id_etapa, ETAPAS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.id_actividad, ACTIVIDADES.descripcion, ACTIVIDADES.ubicacion_cc, TIPO_COSTO.orden, upper(TIPO_COSTO.descripcion), COSTOS.tipo_costo, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end " & "Having (((zona_trabajo.id_zonatrabajo) = '" & MatrizZ(cboZonatrabajo.SelectedIndex) & "')) " & "ORDER BY ZONA_TRABAJO.descripcion, CULTIVOS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.ubicacion_cc, Sum(COSTOS.monto_standar) DESC, TIPO_COSTO.orden, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end")
            Else
                rsReporte = objBL.Refresh_Query("SELECT ZONA_TRABAJO.id_zonatrabajo AS ZONA_TRABAJO_id_zonatrabajo, ZONA_TRABAJO.descripcion AS ZONA_TRABAJO_descripcion, ZONA_TRABAJO.hectareas AS ZONA_TRABAJO_hectareas, CULTIVOS.id_cultivo AS CULTIVOS_id_cultivo, CULTIVOS.descripcion AS CULTIVOS_descripcion, ETAPAS.id_etapa AS ETAPAS_id_etapa, ETAPAS.descripcion AS ETAPAS_descripcion, ETAPAS.ubicacion_cc AS ETAPAS_ubicacion_cc, ACTIVIDADES.id_actividad AS ACTIVIDADES_id_actividad, ACTIVIDADES.descripcion AS ACTIVIDADES_descripcion, ACTIVIDADES.ubicacion_cc AS ACTIVIDADES_ubicacion_cc, TIPO_COSTO.orden AS TIPO_COSTO_orden, upper(TIPO_COSTO.descripcion) AS TIPO_COSTO_descripcion, COSTOSME.tipo_costo AS COSTOS_tipo_costo, " & "case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end AS INSUMOS_descripcion, Sum(COSTOSME.cantidad) AS COSTOS_cantidad, Sum(COSTOSME.MS) AS COSTOS_monto_standar " & "FROM (CULTIVOS INNER JOIN ((((((COSTOSME INNER JOIN ACTIVIDADES ON COSTOSME.id_actividad = ACTIVIDADES.id_actividad) INNER JOIN ETAPAS ON ACTIVIDADES.id_etapa = ETAPAS.id_etapa) INNER JOIN MAQUINAS ON COSTOSME.id_maquinaria = MAQUINAS.id_maquinaria) INNER JOIN PERSONAL ON COSTOSME.id_personal = PERSONAL.id_personal) INNER JOIN PRODUCTOS ON COSTOSME.id_producto = PRODUCTOS.id_producto) INNER JOIN ZONA_TRABAJO ON COSTOSME.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo) ON CULTIVOS.id_cultivo = ZONA_TRABAJO.id_cultivo) INNER JOIN TIPO_COSTO ON COSTOSME.tipo_costo = TIPO_COSTO.id_tipocosto " & "Where COSTOSME.PC>=0 AND COSTOSME.Tipo_Costo <>'C'  And COSTOSME.Tipo_Costo <> 'V' And COSTOSME.Tipo_Costo <> 'E' And COSTOSME.Tipo_Costo <> 'S' And COSTOSME.Tipo_Costo <> 'T'  And COSTOSme.Tipo_Costo <> 'D' And (((COSTOSME.fecha) >= convert(datetime,replace('" & fechaFrom.ToString("yyyyMMdd") & "',',','/')) " & "And (COSTOSME.fecha) <= convert(datetime,replace('" & fechaTo.ToString("yyyyMMdd") & "',',','/')))) " & "GROUP BY ZONA_TRABAJO.id_zonatrabajo, ZONA_TRABAJO.descripcion, ZONA_TRABAJO.hectareas, CULTIVOS.id_cultivo, CULTIVOS.descripcion, ETAPAS.id_etapa, ETAPAS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.id_actividad, ACTIVIDADES.descripcion, ACTIVIDADES.ubicacion_cc, TIPO_COSTO.orden, upper(TIPO_COSTO.descripcion), COSTOSME.tipo_costo, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end " & "Having (((zona_trabajo.id_zonatrabajo) = '" & MatrizZ(cboZonatrabajo.SelectedIndex) & "')) " & "ORDER BY ZONA_TRABAJO.descripcion, CULTIVOS.descripcion, ETAPAS.ubicacion_cc, ACTIVIDADES.ubicacion_cc, Sum(COSTOSME.MS) DESC, TIPO_COSTO.orden, case when [tipo_costo]='I' then [PRODUCTOS].[descripcion] else '' end")
            End If
        End If

        If rsReporte.RecordCount = 0 Then Exit Sub
        Report.Load(Report.ResourceName) : Report.SetDataSource(rsReporte) ': MsgBox(Resource1.str11, MsgBoxStyle.Information) ' Reporte Generado ' Reporte Generado
        crvCostoTotalEnUnaZonaDeTrabajoPorRubros.ReportSource = Report : crvCostoTotalEnUnaZonaDeTrabajoPorRubros.RefreshReport()
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