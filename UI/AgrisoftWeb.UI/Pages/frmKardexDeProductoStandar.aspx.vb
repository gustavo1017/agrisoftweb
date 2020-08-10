Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class frmKardexDeProductoStandar
    Inherits BasePage

    Dim Report As New crKardexDeProductoContablebeta4b
    'Dim rsReport As New ADODB.Recordset
    'Dim dtData As New DataTable

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim STRID As String = Session("frmKardexDeProductoStandar_STRID")
        CargarCombo("select id_producto, descripcion from productos order by descripcion", "id_producto", "descripcion", cboCultivo)
        setcaptionlabels()

        If Not Page.IsPostBack() Then
            'Dim gblstrIdUsuario As String = "Fundo0"
            currentModule = "Agricultura.Reportes"
            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            dtfechamin.Text = DateTime.Now.AddMonths(-1).ToShortDateString
            dtfechamax.Text = DateTime.Now.ToShortDateString
            hdnStr6013.Value = Resource1.str6013
            hdnStr6018.Value = Resource1.str6018

            Dim dtData As DataTable = CrearCursor()
            Refresh_Query(dtData)
        Else
            If Session("frmKardexDeProductoStandar_Report") IsNot Nothing Then
                Report = Session("frmKardexDeProductoStandar_Report")
                crvKardexProductoStandar.ReportSource = Report
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

            For i As Integer = 0 To crvKardexProductoStandar.LogOnInfo.Count - 1
                crvKardexProductoStandar.LogOnInfo(i).ConnectionInfo = crCon
            Next i

            crvKardexProductoStandar.ReuseParameterValuesOnRefresh = True
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
    End Sub

    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
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

        For i As Integer = 0 To crvKardexProductoStandar.LogOnInfo.Count - 1
            crvKardexProductoStandar.LogOnInfo(i).ConnectionInfo = crCon
        Next i

        Dim fechaFrom As DateTime ' = DateTime.ParseExact(dtfechamin.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None)
        DateTime.TryParseExact(dtfechamin.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechaFrom)

        If fechaFrom = DateTime.MinValue Then
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "text", "ShowErrorMessage(Resource1.str99992)", True)
            'lblMessage.Text = "El formato de la fecha debe ser dd/MM/yyyy"
            crvKardexProductoStandar.ReportSource = Nothing
            Exit Sub
        End If

        Dim dtData As DataTable = LlenarKardex()
        Refresh_Query(dtData)

        'Save on Session the values needed for report 
        If String.IsNullOrEmpty(dtfechamin.Text) Then
            Session.Add("frmKardexDeProductoStandar_dtFechaMin", hdnDateFrom.Value)
        Else
            Dim datefromConverted As DateTime
            If DateTime.TryParse(dtfechamin.Text, datefromConverted) Then
                Session.Add("frmKardexDeProductoStandar_dtFechaMin", datefromConverted)
            Else
                Session.Add("frmKardexDeProductoStandar_dtFechaMin", Nothing)
            End If
        End If
    End Sub

    Private Sub Refresh_Query(ByVal dtData As DataTable)
        If dtData.Rows.Count() = 0 Then
            crvKardexProductoStandar.ReportSource = Nothing
            Exit Sub
        End If

        Report.Load(Report.ResourceName) : Report.SetDataSource(dtData)
        crvKardexProductoStandar.ReportSource = Report
        crvKardexProductoStandar.ToolPanelView = ToolPanelViewType.None
        setResourcesToReport()

        Session.Add("frmKardexDeProductoStandar_Report", Report)
    End Sub

    Private Function LlenarKardex() As DataTable
        'Dim rsFactura As Object
        'Dim strFecha As Object
        Dim StrEnlace As Object
        Dim strProveedor As Object
        Dim StrTipoDoc As Object
        Dim StrFact As Object
        Dim StrFechaDoc As Object
        'Dim rsSum As ADODB.Recordset
        Dim RS As ADODB.Recordset
        Dim strSQL As String
        Dim SalAcum, cTotSal, cTotAct, cCosAct, cSalIni, cCosIni, cCtsAnt, cTotIng, cfecver, EntAcum As Object
        Dim StrNumParte As Object
        Dim drItem As DataRow
        Dim dtdata As DataTable = CrearCursor()

        cTotAct = 0
        cCosAct = 0
        'CrearCursor()
        cfecver = DateTime.Parse(dtfechamin.Text).ToString("yyyyMMdd")
        Dim Matrizp As Object = Session("frmKardexDeProductoStandar_Matrizp")

        '// Saldo Anterior //
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        strSQL = " SELECT KardexDeProductos.id_producto, KardexDeProductos.id_almacen, Sum(KardexDeProductos.[Entradap]) AS SumaDeEntradap, Sum(KardexDeProductos.[salidaP]) AS SumaDesalidaP, Sum([entradap]-[salidap]) AS SaldoP, Sum([entradap]-[salidap]) AS Expr1, Sum([entradaV]-[salidaV]) AS SaldoV  From KardexDeProductos Where (((KardexDeProductos.fecha) < convert(datetime,replace('" & cfecver & "',',','/')))) " & " GROUP BY KardexDeProductos.id_producto, KardexDeProductos.id_almacen " & " HAVING (((KardexDeProductos.id_producto)='" & Matrizp(cboCultivo.SelectedIndex) & "') AND (KardexDeProductos.id_almacen='01'));"
        RS = DBconn.Execute(strSQL, , ADODB.CommandTypeEnum.adCmdText)
        cSalIni = 0
        cCosIni = 0

        Dim SumaAcumEnt As Decimal = 0
        Dim SumaAcumSal As Decimal = 0
        If Not RS.EOF Then
            cSalIni = RS.Fields("SaldoP").Value
            cCosIni = RS.Fields("SaldoV").Value

            If cSalIni = 0 Then
                cCtsAnt = 0
            Else
                cCtsAnt = cCosIni / cSalIni
            End If

            SumaAcumEnt = RS.Fields("SumaDeEntradap").Value
            SumaAcumSal = RS.Fields("SumaDeSalidaP").Value
            cTotAct = cCtsAnt * cSalIni

            drItem = dtdata.NewRow()
            drItem("Numero_Parte") = Resource1.str10320
            drItem("CostoPd") = cCtsAnt
            drItem("SaldosP") = cSalIni
            drItem("SaldosV") = cCtsAnt * cSalIni
            drItem("EntradaP") = RS.Fields("SumaDeEntradap").Value
            drItem("EntradaV") = RS.Fields("SumaDeEntradap").Value * cCtsAnt
            drItem("SalidaP") = RS.Fields("SumaDesalidaP").Value 'SumaDesalidaP
            drItem("SalidaV") = RS.Fields("SumaDesalidaP").Value * cCtsAnt
            dtdata.Rows.Add(drItem)
        Else
            SumaAcumEnt = CStr(0)
            SumaAcumSal = CStr(0)
        End If

        strSQL = " SELECT KardexDeProductos.Periodo, KardexDeProductos.Numero_Parte, KardexDeProductos.Fecha, KardexDeProductos.EntradaP, KardexDeProductos.SalidaP, KardexDeProductos.EntradaV, KardexDeProductos.SalidaV, KardexDeProductos.CostoC, KardexDeProductos.observaciones, KardexDeProductos.id_enlace, KardexDeProductos.tipo_costo AS tipo, KardexDeProductos.id_almacen " & " From KardexDeProductos " & " WHERE (((KardexDeProductos.Fecha)>=convert(datetime,replace('" & cfecver & "',',','/')) And (KardexDeProductos.Fecha)<=convert(datetime,replace('" & DateTime.Parse(dtfechamax.Text).ToString("yyyyMMdd") & "',',','/'))) AND ((KardexDeProductos.tipo_costo)='C' Or (KardexDeProductos.tipo_costo)='I' Or (KardexDeProductos.tipo_costo)='E' Or (KardexDeProductos.tipo_costo)='S' Or (KardexDeProductos.tipo_costo)='T' Or (KardexDeProductos.tipo_costo)='V') AND ((KardexDeProductos.id_producto)='" & Matrizp(cboCultivo.SelectedIndex) & "')) " & " ORDER BY KardexDeProductos.Fecha, KardexDeProductos.tipo_costo, KardexDeProductos.numero_parte;"
        RS = DBconn.Execute(strSQL, , ADODB.CommandTypeEnum.adCmdText)
        SalAcum = 0
        EntAcum = 0

        While Not RS.EOF
            '// Proceso //
            StrNumParte = RS.Fields("Numero_parte").Value
            cTotIng = 0
            cTotSal = 0
            StrFechaDoc = DateTime.Now.ToString()
            StrFact = ""
            StrTipoDoc = ""
            strProveedor = ""
            StrEnlace = RS.Fields("id_enlace").Value
            ' strFecha = Year(RS.Fields("Fecha").Value) & "," & Month(RS.Fields("Fecha").Value) & "," & VB.Day(RS.Fields("Fecha").Value)

            If RS.Fields("SalidaP").Value <> 0 Or RS.Fields("observaciones").Value = Resource1.str9244 Then
                If cSalIni <> 0 Then cCosIni = cTotAct / cSalIni

                If RS.Fields("observaciones").Value = Resource1.str9244 Then '
                    If RS.Fields("SalidaP").Value = 0 Then
                        cCosIni = 0
                    Else
                        cCosIni = RS.Fields("CostoC").Value
                    End If

                    cTotAct = cTotAct + RS.Fields("SalidaV").Value
                    cTotSal = -RS.Fields("SalidaV").Value
                Else
                    cTotAct = cTotAct - (RS.Fields("SalidaP").Value * cCosIni)
                    cTotSal = RS.Fields("SalidaP").Value * cCosIni
                End If
            Else
                If RS.Fields("observaciones").Value = Resource1.str9245 Then
                    If RS.Fields("EntradaP").Value = 0 Then
                        cCosIni = 0
                    Else
                        cCosIni = RS.Fields("CostoC").Value
                    End If

                    cTotIng = RS.Fields("EntradaV").Value
                    cTotAct = cTotAct + RS.Fields("EntradaV").Value
                Else
                    cTotIng = RS.Fields("EntradaP").Value * RS.Fields("CostoC").Value
                    cTotAct = cTotAct + (RS.Fields("EntradaP").Value * RS.Fields("CostoC").Value)
                    cCosIni = RS.Fields("CostoC").Value
                End If
            End If

            cSalIni = (cSalIni + RS.Fields("EntradaP").Value) - RS.Fields("SalidaP").Value

            drItem = dtdata.NewRow()
            drItem("Numero_Parte") = StrNumParte
            drItem("Fecha") = RS.Fields("Fecha").Value

            If Len(StrFact) <> 0 Then
                drItem("Tipo_doc") = StrTipoDoc
                drItem("Numero_doc") = StrFact
                drItem("Fecha_doc") = StrFechaDoc
            Else


                drItem("Tipo_doc") = ""
                drItem("Numero_doc") = ""
            End If


            drItem("EntradaP") = RS.Fields("EntradaP").Value
            drItem("SalidaP") = RS.Fields("SalidaP").Value
            drItem("SaldosP") = cSalIni
            drItem("EntradaV") = cTotIng
            drItem("SalidaV") = cTotSal
            drItem("CostoPd") = cCosIni
            drItem("SaldosV") = cTotAct

            If RS.Fields("tipo").Value = "C" Then
                'rsReport.Fields("Observaciones").Value = strProveedor
                drItem("Observaciones") = strProveedor
            Else
                ' rsReport.Fields("Observaciones").Value = RS.Fields("Observaciones").Value
                drItem("Observaciones") = RS.Fields("Observaciones").Value
            End If

            EntAcum = RS.Fields("EntradaP").Value + Val(EntAcum)
            SalAcum = RS.Fields("SalidaP").Value + Val(SalAcum)

            dtdata.Rows.Add(drItem)
            RS.MoveNext()
        End While

        'rsReport.AddNew()
        'rsReport.Fields("EntradaP").Value = EntAcum
        'rsReport.Fields("SalidaP").Value = SalAcum
        'rsReport.AddNew()
        'rsReport.Fields("EntradaP").Value = Val(SumaAcumEnt) + EntAcum
        'rsReport.Fields("SalidaP").Value = Val(SumaAcumSal) + SalAcum

        drItem = dtdata.NewRow()
        drItem("EntradaP") = EntAcum
        drItem("SalidaP") = SalAcum
        dtdata.Rows.Add(drItem)

        drItem = dtdata.NewRow()
        drItem("EntradaP") = Val(SumaAcumEnt) + EntAcum
        drItem("SalidaP") = Val(SumaAcumSal) + SalAcum
        dtdata.Rows.Add(drItem)

        Return dtdata
    End Function

    Private Sub setcaptionlabels()
        lblFecha.Text = Resource1.str10129
        lblHasta.Text = Resource1.str10130
        Title = Resource1.str9205
        btnVer.Text = Resource1.str9197 'Ver
        'btnConfigurar.Text = Resource1.str9196 'Configurar
        'btnImprimir.Text = Resource1.str9195 'Imprimir
        'btnExportarExcel.Text = Resource1.str9194 'Exportar
    End Sub

    Private Sub setResourcesToReport()
        Dim Campo As TextObject
        Campo = Report.ReportDefinition.ReportObjects("texto11") : Campo.Text = Resource1.str3014  ' Totales
        Campo = Report.ReportDefinition.ReportObjects("text1") : Campo.Text = Resource1.str517 ' observaciones
        'Campo = Report.ReportDefinition.ReportObjects("texto2") : Campo.Text = Resource1.str9205 ' descripcion
        'Campo = Report.ReportDefinition.ReportObjects("texto4") : Campo.Text = Resource1.str9205 ' cantidad
        'Campo = Report.ReportDefinition.ReportObjects("edtRangoFecha") : Campo.Text = Resource1.str3004 + DateTime.Parse(dtfechamin.Text).ToString("dd/MM/yyyy")
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

    Private Sub CargarCombo(ByRef ssql As String, ByRef idField As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim adoTabla As ADODB.Recordset
        Dim i, intOrden As Short

        If Cbo.Items.Count > 0 Then intOrden = Cbo.SelectedIndex

        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest
        adoTabla = New ADODB.Recordset

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        adoTabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
        Dim Matrizp() As Object = Session("frmKardexDeProductoStandar_Matrizp")
        Dim STRID As String = Session("frmKardexDeProductoStandar_STRID")

        While Not adoTabla.EOF
            Select Case idField
                Case "id_producto" : ReDim Preserve Matrizp(i)
                    Matrizp(i) = adoTabla.Fields(idField).Value
            End Select

            Cbo.Items.Add(adoTabla.Fields(Desc).Value)

            If adoTabla.Fields(idField).Value = STRID Then
                intOrden = i
            End If

            i = i + 1
            adoTabla.MoveNext()
        End While

        Session("frmKardexDeProductoStandar_Matrizp") = Matrizp
        Cbo.SelectedIndex = intOrden
    End Sub

    Private Function CrearCursor() As DataTable


        Dim dtData As New DataTable()
        dtData.Columns.Add("Numero_Parte", System.Type.GetType("System.String"))
        dtData.Columns.Add("Fecha", System.Type.GetType("System.DateTime"))
        dtData.Columns.Add("Tipo_doc", System.Type.GetType("System.String"))
        dtData.Columns.Add("Numero_doc", System.Type.GetType("System.String"))
        dtData.Columns.Add("Fecha_doc", System.Type.GetType("System.DateTime"))
        dtData.Columns.Add("EntradaP", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("SalidaP", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("SaldosP", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("CostoPd", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("EntradaV", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("SalidaV", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("SaldosV", System.Type.GetType("System.Decimal"))
        dtData.Columns.Add("Observaciones", System.Type.GetType("System.String"))

        Return dtData
    End Function
End Class