﻿Imports System.Globalization
Imports System.Web.Services
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmCostosCosechas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()

        Dim blnPasa1 As Boolean = False
        If Not Session("FrmCostosCosechas_blnPasa1") = Nothing Then
            blnPasa1 = Convert.ToBoolean(Session("blnPasa1"))
        End If

        If Not Page.IsPostBack() Then
            setCultureDecimalSeparator()
            Call pInicializaControles()
            Call setCaptionsLabels()

            Dim strAction As String = Request.QueryString("Action")
            LoadData()

            Select Case UCase(strAction)
                Case "NEW"
                    CargarFecha()
                    Session("FrmCostosCosechas_Action") = "NEW"
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResults.Text = Resource1.str409
                    Else
                        ' Select first and single row
                        lvwAsientos.SelectedIndex = 0
                        lvwAsientos_SelectedIndexChanged(Nothing, Nothing)
                    End If
                    cmdAgregar.Enabled = False
                    Session("FrmCostosCosechas_Action") = "EDIT"
            End Select

            cboTipoCosto.SelectedIndex = 0
            LoadPageResources()

        End If
    End Sub

    Protected Sub lvwAsientos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(0).Text = Resource1.str504
                e.Row.Cells(1).Text = Resource1.str315
                e.Row.Cells(2).Text = Resource1.str511
                e.Row.Cells(3).Text = Resource1.str211
                e.Row.Cells(4).Text = Resource1.str546
                e.Row.Cells(5).Text = Resource1.str516
                e.Row.Cells(6).Text = Resource1.str3007
                e.Row.Cells(7).Text = Resource1.str3008
                e.Row.Cells(8).Text = Resource1.str3009
                e.Row.Cells(9).Text = Resource1.str541
                e.Row.Cells(10).Text = Resource1.str3005
                e.Row.Cells(11).Text = Resource1.str99999982
                e.Row.Cells(12).Text = Resource1.str99999983
                e.Row.Cells(13).Text = Resource1.str99999984
                e.Row.Cells(14).Text = Resource1.str60
                e.Row.Cells(15).Text = Resource1.str10003
                e.Row.Cells(16).Text = Resource1.str211
                e.Row.Cells(17).Text = Resource1.str315
                e.Row.Cells(18).Text = Resource1.str315

                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right

            Case DataControlRowType.DataRow
                e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(6).Text = Convert.ToDecimal(e.Row.Cells(6).Text, CultureInfo.InvariantCulture).ToString("###0.00", CultureInfo.InvariantCulture)
                e.Row.Cells(7).Text = Convert.ToDecimal(e.Row.Cells(7).Text, CultureInfo.InvariantCulture).ToString("###0.00", CultureInfo.InvariantCulture)
                e.Row.Cells(8).Text = Convert.ToDecimal(e.Row.Cells(8).Text, CultureInfo.InvariantCulture).ToString("###0.00", CultureInfo.InvariantCulture)

                Dim dateConvert As DateTime
                If DateTime.TryParse(e.Row.Cells(0).Text, dateConvert) Then
                    e.Row.Cells(0).Text = Convert.ToDateTime(e.Row.Cells(0).Text).ToString("dd/MM/yyyy")
                End If
        End Select

        e.Row.Cells(3).Visible = False
        e.Row.Cells(4).Visible = False
        e.Row.Cells(11).Visible = False
        e.Row.Cells(12).Visible = False
        e.Row.Cells(13).Visible = False
        e.Row.Cells(15).Visible = False
        e.Row.Cells(16).Visible = False
        e.Row.Cells(17).Visible = False
        e.Row.Cells(18).Visible = False
    End Sub

    Protected Sub lvwAsientos_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.cursor='pointer';this.style.textDecoration='underline';"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';"
            e.Row.ToolTip = "Seleccionar"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(lvwAsientos, "Select$" & e.Row.RowIndex)
        End If
    End Sub

    Protected Sub lvwAsientos_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")
        Dim MatrizAx() As Object = Session("FrmCostosCosechas_MatrizAx")
        Dim MatrizCm() As Object = Session("FrmCostosCosechas_MatrizCm")
        Dim MatrizMq() As Object = Session("FrmCostosCosechas_MatrizMq")
        Dim MatrizCc() As Object = Session("FrmCostosCosechas_MatrizCc")
        Dim MatrizPr() As Object = Session("FrmCostosCosechas_MatrizPr")
        Dim MatrizTd() As Object = Session("FrmCostosCosechas_MatrizTd")
        Dim MatrizCt() As Object = Session("FrmCostosCosechas_MatrizCt")
        Dim MatrizCp() As Object = Session("FrmCostosCosechas_MatrizCp")

        Dim currentIndex As Integer = lvwAsientos.SelectedIndex
        Dim PreviousIndex As Integer = Convert.ToInt32(Session("FrmCostosCosechas_PreviousSelectedIndex"))

        'Verify if a different row was selected
        If currentIndex = PreviousIndex Then
            Session("FrmCostosCosechas_PreviousSelectedIndex") = currentIndex
            Exit Sub
        End If

        For Each row As GridViewRow In lvwAsientos.Rows
            row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
            row.ToolTip = "Seleccionar"
        Next

        Session.Add("FrmCostosCosechas_PreviousSelectedIndex", currentIndex)
        Dim index As Short = 0

        Dim gRow As GridViewRow = lvwAsientos.Rows(currentIndex)
        gRow.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
        gRow.ToolTip = ""

        Dim dtPeriodo As DateTime = DateTime.Parse(gRow.Cells(0).Text)
        dtPicker3.Text = dtPeriodo.ToShortDateString
        txtTC.Text = gRow.Cells(14).Text

        'index = fBuscarPosCodEnMatriz(MatrizCt, gRow.Cells(17).Text)
        'If cboCultivo.SelectedIndex <> index Then
        '    cboCultivo.SelectedIndex = index
        '    cboCultivo_SelectedIndexChanged(Nothing, Nothing)
        'End If

        MatrizAx = Session("FrmCostosCosechas_MatrizAx")
        index = fBuscarPosCodEnMatriz(MatrizAx, gRow.Cells(11).Text)
        If cboZonaTrabajo.SelectedIndex <> index Then
            cboZonaTrabajo.SelectedIndex = index
            cboZonaTrabajo_SelectedIndexChanged(Nothing, Nothing)
        End If

        'MatrizTd = Session("FrmCostosCosechas_MatrizTd")
        'index = fBuscarPosCodEnMatriz(MatrizTd, gRow.Cells(16).Text)
        'If cboEtapa.SelectedIndex <> index Then
        '    cboEtapa.SelectedIndex = index
        '    cboEtapa_SelectedIndexChanged(Nothing, Nothing)
        'End If

        'MatrizCm = Session("FrmCostosCosechas_MatrizCm")
        'cboActividad.SelectedIndex = fBuscarPosCodEnMatriz(MatrizCm, gRow.Cells(12).Text)

        'MatrizAC = Session("FrmCostosCosechas_MatrizAC")
        'index = fBuscarPosCodEnMatriz(MatrizAC, Mid(gRow.Cells(10).Text, 1, 1))
        'If cboTipoCosto.SelectedIndex <> index Then
        '    cboTipoCosto.SelectedIndex = index
        '    cboTipoCosto_SelectedIndexChanged(Nothing, Nothing)
        'End If

        MatrizMq = Session("FrmCostosCosechas_MatrizMq")
        MatrizCc = Session("FrmCostosCosechas_MatrizCc")
        MatrizPr = Session("FrmCostosCosechas_MatrizPr")
        MatrizCp = Session("FrmCostosCosechas_MatrizCp")

        ' No vale
        'Select Case UCase(MatrizAC(cboTipoCosto.SelectedIndex))
        '    Case "I"
        '        index = fBuscarPosCodEnMatriz(MatrizCc, gRow.Cells(13).Text)
        '    Case "M"
        '        index = fBuscarPosCodEnMatriz(MatrizMq, gRow.Cells(13).Text)
        '    Case "R"
        '        index = fBuscarPosCodEnMatriz(MatrizCp, gRow.Cells(13).Text)
        '    Case "H"
        '        index = fBuscarPosCodEnMatriz(MatrizPr, gRow.Cells(13).Text)
        '    Case "O"
        '        'cboCodAnx.ListIndex = -1
        'End Select

        Dim indexRecursos As Integer = fBuscarPosCodEnMatriz(MatrizCc, gRow.Cells(13).Text)

        If cboCodanx.SelectedIndex <> indexRecursos Then
            cboCodanx.SelectedIndex = indexRecursos
            cboCodanx_SelectedIndexChanged(Nothing, Nothing)
        End If

        txtCampana.Text = gRow.Cells(9).Text
        txtCantidad.Text = gRow.Cells(6).Text
        txtCostoUnitarioStandar.Text = gRow.Cells(7).Text
        txtMontoStandar.Text = gRow.Cells(8).Text
        txtAvance.Text = gRow.Cells(19).Text
        Session("FrmCostosCosechas_IdCosto") = gRow.Cells(18).Text
    End Sub

    Protected Sub cboCodanx_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim blnPasa1 As Boolean = False
        'If Not Session("FrmCostosCosechas_blnPasa1") = Nothing Then
        '    blnPasa1 = Convert.ToBoolean(Session("blnPasa1"))
        'End If

        'If blnPasa1 Then
        '    If Val(txtTC.Text) <= 0 Then
        '        lblResults.Text = Resource1.str548
        '        Exit Sub
        '    End If
        'Else
        '    blnPasa1 = True
        '    Session("blnPasa1") = blnPasa1
        '    Exit Sub
        'End If

        setCultureDecimalSeparator()

        Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")
        Dim MatrizPC() As Object = Session("FrmCostosCosechas_MatrizPC")
        Dim MatrizPTC() As Object = Session("FrmCostosCosechas_MatrizPTC")

        'Select Case UCase(MatrizAC(cboTipoCosto.SelectedIndex))
        '    Case "I"
        '        txtCostoUnitarioStandar.Text = MatrizPC(cboCodanx.SelectedIndex)

        '        If MatrizPTC(cboCodanx.SelectedIndex) = 0 Then
        '            txtCostoUnitarioStandar.Text = IIf(cboMoneda.SelectedIndex = 0, MatrizPC(cboCodanx.SelectedIndex), MatrizPC(cboCodanx.SelectedIndex) / Val(txtTC.Text))
        '        Else
        '            txtCostoUnitarioStandar.Text = IIf(cboMoneda.SelectedIndex = 1, MatrizPC(cboCodanx.SelectedIndex), MatrizPC(cboCodanx.SelectedIndex) * Val(txtTC.Text))
        '        End If

        '    Case "O"
        '        txtCostoUnitarioStandar.Text = CStr(0)

        '    Case Else
        '        If cboMoneda.SelectedIndex = 0 Then
        '            txtCostoUnitarioStandar.Text = MatrizPC(cboCodanx.SelectedIndex)
        '        Else
        '            Dim amount As Double = MatrizPC(cboCodanx.SelectedIndex) / CDbl(txtTC.Text)
        '            txtCostoUnitarioStandar.Text = IIf(CDbl(txtTC.Text) > 0, amount.ToString("0.00"), MatrizPC(cboCodanx.SelectedIndex))
        '        End If
        'End Select

        If cboMoneda.SelectedIndex = 0 Then
            txtCostoUnitarioStandar.Text = MatrizPC(cboCodanx.SelectedIndex)
        Else
            Dim amount As Double = MatrizPC(cboCodanx.SelectedIndex) / CDbl(txtTC.Text)
            txtCostoUnitarioStandar.Text = IIf(CDbl(txtTC.Text) > 0, amount.ToString("0.00"), MatrizPC(cboCodanx.SelectedIndex))
        End If

        getMontoStandar()
    End Sub

    Protected Sub cboPerPla_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim rstmp As New ADODB.Recordset
        'Dim ssql As String = "Select * from PERPLA where id_perpla='" & cboPerPla.Text & "';"
        'Dim DBconn As New ADODB.Connection
        'Dim objBL As New GenericMethods("Fundo0")
        'DBconn.Open(objBL.GetSQLConnection())
        'rstmp.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
    End Sub

    Protected Sub cboCultivo_SelectedIndexChanged(sender As Object, e As EventArgs)
        cboZonaTrabajo.Items.Clear()
        Dim MatrizCt() As Object = Session("FrmCostosCosechas_MatrizCt")
        CargarCombo("SELECT ID_zonatrabajo, Descripcion, campana FROM ZONA_TRABAJO  where tipo='C' ORDER BY DESCRIPCION", "ID_ZONATRABAJO", "DESCRIPCION", cboZonaTrabajo)
    End Sub

    Protected Sub cboEtapa_SelectedIndexChanged(sender As Object, e As EventArgs)
        cboActividad.Items.Clear()
        Dim MatrizTd() As Object = Session("FrmCostosCosechas_MatrizTd")
        CargarCombo("SELECT ID_actividad, Descripcion FROM Actividades ORDER BY DESCRIPCION", "ID_actividad", "DESCRIPCION", cboActividad)
    End Sub

    Protected Sub cboTipoCosto_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub LoadData()
        cboCodanx.Items.Clear()
        Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")

        dvInfo.Visible = True
        dvProveedor.Visible = False
        'lblCultivo.Visible = True
        'cboCultivo.Visible = True
        'lblEtapa.Visible = False
        'cboEtapa.Visible = False
        'lblActividad.Visible = False
        'cboActividad.Visible = False
        lblRecurso.Text = Resource1.str544
        hdnBuscarProductos.Value = "Productos"
        Label2.Text = Resource1.str99999961
        CargarCombo("SELECT ID_Producto,   Descripcion, costo, tc FROM Productos Where Tipo = 'P' ORDER BY DESCRIPCION", "ID_Producto", "DESCRIPCION", cboCodanx)
    End Sub

    Protected Sub cboZonaTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim MatrizCa As Object = Session("FrmCostosCosechas_MatrizCa")

        Dim campañaValue As Decimal = Decimal.Zero
        Try
            campañaValue = Convert.ToDecimal(MatrizCa(cboZonaTrabajo.SelectedIndex))
        Catch ex As Exception
            campañaValue = Decimal.Zero
        End Try

        txtCampana.Text = campañaValue.ToString(CultureInfo.InvariantCulture)
    End Sub

    Protected Sub cmdModificar_Click(sender As Object, e As EventArgs)
        'Dim strKey As String = strIdCosto

        If Not fValidaDatos() Then Exit Sub

        If Session("FrmCostosCosechas_IdCosto") = Nothing Then
            Exit Sub
        End If

        Dim idCosto As String = Session("FrmCostosCosechas_IdCosto")
        Dim dtContent As New DataTable()
        dtContent = Session("FrmCostosCosechas_Datatable")

        Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")
        Dim MatrizAx() As Object = Session("FrmCostosCosechas_MatrizAx")
        Dim MatrizCm() As Object = Session("FrmCostosCosechas_MatrizCm")
        Dim MatrizMq() As Object = Session("FrmCostosCosechas_MatrizMq")
        Dim MatrizCc() As Object = Session("FrmCostosCosechas_MatrizCc")
        Dim MatrizPr() As Object = Session("FrmCostosCosechas_MatrizPr")
        Dim MatrizTd() As Object = Session("FrmCostosCosechas_MatrizTd")
        Dim MatrizCt() As Object = Session("FrmCostosCosechas_MatrizCt")
        Dim strIdCosto As String = Session("FrmCostosCosechas_IdCosto").ToString()

        Dim currentIndex As Integer = lvwAsientos.SelectedIndex
        Dim montoCantidad As Decimal = Decimal.Parse(txtCantidad.Text.Trim(), CultureInfo.InvariantCulture)
        Dim montoCUnitario As Decimal = Decimal.Parse(txtCostoUnitarioStandar.Text.Trim(), CultureInfo.InvariantCulture)

        Dim row As DataRow = dtContent.Rows(currentIndex)
        row.Item(0) = dtPicker3.Text
        row.Item(1) = cboCultivo.Text
        row.Item(2) = cboZonaTrabajo.Text
        row.Item(3) = cboEtapa.Text
        row.Item(4) = cboActividad.Text
        row.Item(5) = cboCodanx.Text
        row.Item(6) = montoCantidad.ToString("###0.00", CultureInfo.InvariantCulture)
        row.Item(7) = montoCUnitario.ToString("###0.00", CultureInfo.InvariantCulture)
        row.Item(8) = (montoCantidad * montoCUnitario).ToString("###0.00", CultureInfo.InvariantCulture)
        row.Item(9) = txtCampana.Text
        row.Item(10) = "C" & Resource1.str11010

        row.Item(11) = MatrizAx(cboZonaTrabajo.SelectedIndex)
        row.Item(12) = "000001"

        Select Case Mid(row.Item(10), 1, 1)
            Case "I"
                row.Item(13) = MatrizCc(cboCodanx.SelectedIndex)
            Case "M", "R"
                row.Item(13) = MatrizMq(cboCodanx.SelectedIndex)
            Case "H"
                row.Item(13) = MatrizPr(cboCodanx.SelectedIndex)
        End Select

        row.Item(14) = txtTC.Text
        row.Item(15) = CStr(cboMoneda.SelectedIndex)
        row.Item(16) = "NA0"
        row.Item(17) = "COSTIN"
        row.Item(18) = "C"
        row.Item(19) = IIf(String.IsNullOrEmpty(txtAvance.Text.Trim()), "0", txtAvance.Text.Trim())

        Session.Add("FrmCostosCosechas_Datatable", dtContent)
        lvwAsientos.DataSource = dtContent
        lvwAsientos.DataBind()
        pSumaMontos()
        'Session("FrmCostosCosechas_PreviousSelectedIndex") = -1
    End Sub

    Protected Sub cmdEliminar_Click(sender As Object, e As EventArgs)
        If Session("FrmCostosCosechas_IdCosto") = Nothing Then
            Exit Sub
        End If

        Dim idCosto As String = Session("FrmCostosCosechas_IdCosto")
        Dim dtContent As New DataTable()
        dtContent = Session("FrmCostosCosechas_Datatable")

        Dim currentIndex As Integer = lvwAsientos.SelectedIndex
        dtContent.Rows(currentIndex).Delete()

        Session("FrmCostosCosechas_PreviousSelectedIndex") = -1
        Session("FrmCostosCosechas_Datatable") = dtContent
        lvwAsientos.DataSource = dtContent
        lvwAsientos.DataBind()
        pSumaMontos()
    End Sub

    Protected Sub cmdAgregar_Click(sender As Object, e As EventArgs)
        dvMessage.Visible = False

        If Val(txtTC.Text) <= 0 Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResults.Text = Resource1.str548
            Exit Sub
        End If

        If Val(txtCampana.Text) <= 0 And hdnBuscarProductos.Value <> "Productos" Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResults.Text = Resource1.str9019
            Exit Sub
        End If

        If Val(txtCantidad.Text) <= 0 Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResults.Text = Resource1.str527
            Exit Sub
        End If

        If Val(txtCostoUnitarioStandar.Text) <= 0 Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResults.Text = Resource1.str528
            Exit Sub
        End If

        'If chkReplicar.CheckState Then


        '    If MsgBox(My.Resources.Resource1.Resource1.str4016, MsgBoxStyle.YesNo, My.Application.Info.ProductName) = MsgBoxResult.Yes Then
        '        ndias = DateDiff(DateInterval.Day, CDate(dtpicker5.Value), CDate(dtpicker4.Value))
        '        ndias = Int(CDbl(ndias))
        '        dtPicker3.Value = dtpicker5.Value

        '        Select Case nFactorCP
        '            Case 30
        '                nvuelta = Int(ndias / nFactorCP)
        '                cDia = VB6.Format(dtPicker3.Value, "DD")

        '                For i = 1 To nvuelta
        '                    AddRecord()
        '                    cFecha = VB6.Format(dtPicker3.Value, "YYYYMM")
        '                    dtPicker3.Value = CDate(cDia + "/" + IIf(Mid(cFecha, 5, 2) = "12", "01/" & Str(Val(Mid(cFecha, 1, 4)) + 1), Str(Val(Mid(cFecha, 5, 2)) + 1) & "/" & Mid(cFecha, 1, 4)))
        '                Next i
        '            Case 7
        '                nvuelta = Int(ndias / nFactorCP)

        '                For i = 1 To nvuelta
        '                    AddRecord()
        '                    dtPicker3.Value = VB6.Format(System.DateTime.FromOADate(CDate(dtPicker3.Value).ToOADate + 7), "dd/mm/yyyy")
        '                Next i
        '        End Select
        '    Else
        '        chkReplicar.CheckState = False
        '    End If
        'Else
        AddRecord()
        'End If
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        'Dim intReg As Short
        'Dim strSQL As String
        Dim enlace As String

        If lvwAsientos.Rows.Count = 0 Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResults.Text = Resource1.str99999981
            Exit Sub
        End If

        Dim MatrizProv() As Object = Session("FrmCostosCosechas_MatrizProv")
        Dim IdProveedor As String = MatrizProv(cboProveedor.SelectedIndex)
        Dim sObservaciones As String = IIf(hdnBuscarProductos.Value = "Productos", cboProveedor.Text, cboZonaTrabajo.Text)

        Dim cantidad, costoUnitario, montoItem, campanaItem, tipoCambioItem, avance As Double
        For Each row As GridViewRow In lvwAsientos.Rows
            enlace = NumeroEnlace()

            cantidad = Double.Parse(row.Cells(6).Text, CultureInfo.InvariantCulture)
            costoUnitario = Double.Parse(row.Cells(7).Text, CultureInfo.InvariantCulture)
            montoItem = Double.Parse(row.Cells(8).Text, CultureInfo.InvariantCulture)
            campanaItem = Double.Parse(row.Cells(9).Text, CultureInfo.InvariantCulture)
            tipoCambioItem = Double.Parse(row.Cells(14).Text, CultureInfo.InvariantCulture)
            avance = Double.Parse(row.Cells(19).Text, CultureInfo.InvariantCulture)

            pInsertarRegistro(row.Cells(11).Text, row.Cells(12).Text, IIf(Mid(row.Cells(10).Text, 1, 1) = "I" Or Mid(row.Cells(10).Text, 1, 1) = "F" Or Mid(row.Cells(10).Text, 1, 1) = "C", row.Cells(13).Text, "000001"), IIf(Mid(row.Cells(10).Text, 1, 1) = "M" Or Mid(row.Cells(10).Text, 1, 1) = "R", row.Cells(13).Text, "000001"), IIf(Mid(row.Cells(10).Text, 1, 1) = "H", row.Cells(13).Text, "00000001"), cantidad, costoUnitario, montoItem, campanaItem, IIf(cboMoneda.SelectedIndex = 0, False, True), tipoCambioItem, txtCicloPeriodo.Text, Mid(row.Cells(10).Text, 1, 1), CDate(row.Cells(0).Text), row.Cells(18).Text, IdProveedor, sObservaciones, avance)
        Next

        dvMessage.Visible = True
        dvMessage.Attributes.Add("class", "alert alert-success")
        lblResults.Text = Resource1.str543
        Limpia()
    End Sub

    Private Sub pInsertarRegistro(ByVal IdZonaTrabajo As String, ByVal IdActividad As String, ByVal IdProducto As String, ByVal IdMaquina As String, ByVal IdPersonal As String, ByVal cantidad As Double, ByVal CostoUnitario As Double, ByVal MontoStandar As Double, ByVal campana As Double, ByVal tc As Short, ByVal TipoCambio As Double, ByVal numeroparte As String, ByVal tipocosto As String, ByVal fecha As Date, ByVal IdCosto As String, ByVal IdProveedor As String, ByVal sObservaciones As String, ByVal Avance As String)
        Dim ssql As String
        Dim StrMoned As String

        Dim objBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"

        Dim strAction As String = Request.QueryString("Action")
        If strAction = "NEW" Then
            Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")

            sObservaciones = "Cosecha"

        Else
            IdCosto = Session("FrmCostosCosechas_Codigo").ToString()

            If cboMoneda.SelectedIndex = 0 Then
                StrMoned = CStr(0)
            Else
                StrMoned = CStr(-1)
            End If


        End If

        Dim DBconn As New ADODB.Connection

        DBconn.Open(objBL.GetSQLConnection())
        DBconn.Execute(ssql)
    End Sub

    Private Function NumeroEnlace() As String
        Dim sResult As String = ""

        Randomize()
        sResult = CStr(Int((1234567890 * Rnd()) + 1)).ToString()

        Return sResult
    End Function

    Private Sub Limpia()
        lvwAsientos.DataSource = Nothing
        lvwAsientos.DataBind()
        TotCantidad.Text = ""
        TotMonto.Text = ""
        Session("FrmCostosCosechas_Datatable") = Nothing
    End Sub

    Private Sub CargarCombo(ByRef ssql As String, ByRef idField As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim i, intOrden As Short
        Dim MatrizPC() As Object = Session("FrmCostosCosechas_MatrizPC")
        Dim MatrizPTC() As Object = Session("FrmCostosCosechas_MatrizPTC")
        Dim MatrizCt() As Object = Session("FrmCostosCosechas_MatrizCt")
        Dim MatrizAx() As Object = Session("FrmCostosCosechas_MatrizAx")
        Dim MatrizCa() As Object = Session("FrmCostosCosechas_MatrizCa")
        Dim MatrizTd() As Object = Session("FrmCostosCosechas_MatrizTd")
        Dim MatrizCc() As Object = Session("FrmCostosCosechas_MatrizCc")
        Dim MatrizMq() As Object = Session("FrmCostosCosechas_MatrizMq")
        Dim MatrizCp() As Object = Session("FrmCostosCosechas_MatrizCp")
        Dim MatrizPr() As Object = Session("FrmCostosCosechas_MatrizPr")
        Dim MatrizCm() As Object = Session("FrmCostosCosechas_MatrizCm")
        Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")
        Dim MatrizProv() As Object = Session("FrmCostosCosechas_MatrizProv")

        If Cbo.Items.Count > 0 Then intOrden = Cbo.SelectedIndex
        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest
        Dim adoTabla = New ADODB.Recordset
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        adoTabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        While Not adoTabla.EOF
            Cbo.Items.Add(IIf(String.IsNullOrEmpty(adoTabla.Fields(Desc).Value), "", Trim(adoTabla.Fields(Desc).Value)))

            Select Case UCase(idField)
                Case UCase("ID_Cultivo") : ReDim Preserve MatrizCt(i)
                    MatrizCt(i) = adoTabla.Fields(idField).Value
                Case UCase("ID_ZonaTrabajo") : ReDim Preserve MatrizAx(i) : ReDim Preserve MatrizCa(i)
                    MatrizAx(i) = adoTabla.Fields(idField).Value
                    MatrizCa(i) = adoTabla.Fields("Campana").Value
                Case UCase("ID_Etapa") : ReDim Preserve MatrizTd(i)
                    MatrizTd(i) = adoTabla.Fields(idField).Value
                Case UCase("ID_Actividad") : ReDim Preserve MatrizCm(i)
                    MatrizCm(i) = adoTabla.Fields(idField).Value
                Case UCase("ID_TipoCosto") : ReDim Preserve MatrizAC(i)
                    MatrizAC(i) = adoTabla.Fields(idField).Value
                Case UCase("ID_Producto") : ReDim Preserve MatrizCc(i)
                    ReDim Preserve MatrizPC(i)
                    ReDim Preserve MatrizPTC(i)
                    MatrizCc(i) = adoTabla.Fields(idField).Value
                    MatrizPC(i) = adoTabla.Fields("costo").Value
                    MatrizPTC(i) = adoTabla.Fields("tc").Value
                Case UCase("ID_Maquinaria1") : ReDim Preserve MatrizMq(i)
                    ReDim Preserve MatrizPC(i)
                    MatrizMq(i) = adoTabla.Fields(idField).Value
                    MatrizPC(i) = adoTabla.Fields("costo").Value
                Case UCase("ID_Maquinaria2") : ReDim Preserve MatrizCp(i)
                    ReDim Preserve MatrizPC(i)
                    MatrizCp(i) = adoTabla.Fields(idField).Value
                    MatrizPC(i) = adoTabla.Fields("costo").Value
                Case UCase("ID_Personal") : ReDim Preserve MatrizPr(i)
                    ReDim Preserve MatrizPC(i)
                    MatrizPr(i) = adoTabla.Fields(idField).Value
                    MatrizPC(i) = adoTabla.Fields("costo").Value
                Case UCase("ID_Proveedor") : ReDim Preserve MatrizProv(i)
                    MatrizProv(i) = adoTabla.Fields(idField).Value
            End Select

            Dim STRID As String = Session("FrmCostosCosechas_STRID")
            If adoTabla.Fields(idField).Value = STRID Then intOrden = i
            i = i + 1
            adoTabla.MoveNext()
        End While

        Session("FrmCostosCosechas_MatrizPC") = MatrizPC
        Session("FrmCostosCosechas_MatrizPTC") = MatrizPTC
        Session("FrmCostosCosechas_MatrizCt") = MatrizCt
        Session("FrmCostosCosechas_MatrizAx") = MatrizAx
        Session("FrmCostosCosechas_MatrizCa") = MatrizCa
        Session("FrmCostosCosechas_MatrizTd") = MatrizTd
        Session("FrmCostosCosechas_MatrizCc") = MatrizCc
        Session("FrmCostosCosechas_MatrizMq") = MatrizMq
        Session("FrmCostosCosechas_MatrizCp") = MatrizCp
        Session("FrmCostosCosechas_MatrizPr") = MatrizPr
        Session("FrmCostosCosechas_MatrizAC") = MatrizAC
        Session("FrmCostosCosechas_MatrizCm") = MatrizCm
        Session("FrmCostosCosechas_MatrizProv") = MatrizProv

        If Cbo.Items.Count > 0 Then Cbo.SelectedIndex = intOrden
    End Sub

    Private Sub pSumaMontos()
        Dim dblSuma As Decimal
        Dim dt As New DataTable()
        dt = Session("FrmCostosCosechas_Datatable")

        For Each row As DataRow In dt.Rows
            dblSuma = dblSuma + Convert.ToDecimal(IIf(row.Item(8) = "", 0, row.Item(8)), CultureInfo.InvariantCulture)
        Next

        TotMonto.Text = dblSuma.ToString("#####0.00", CultureInfo.InvariantCulture)
    End Sub

    Private Sub setCaptionsLabels()

        Title = Resource1.str33
        'lblPeriodo.Text = Resource1.str96
        lblMoneda.Text = Resource1.str10003
        lbltipoCambio.Text = Resource1.str60
        lblTipoRecurso.Text = Resource1.str516
        lblCultivo.Text = Resource1.str315
        lblZonadeTrabajo.Text = Resource1.str511
        lblRecurso.Text = Resource1.str14
        lblCantidad.Text = Resource1.str3007
        lblPrecioUnitario.Text = Resource1.str3008
        lblMonto.Text = Resource1.str3009
        lblCampana.Text = Resource1.str541
        LblTotCantidad.Text = Resource1.str415
        LblTotMonto.Text = Resource1.str909
        lblEtapa.Text = Resource1.str211
        lblActividad.Text = Resource1.str546
        lblCiclo.Text = Resource1.str503 ' "Nro Documento" 'Resource1.str10114
        'lbldesde.Text = Resource1.str9199
        'lblHasta.Text = Resource1.str3004
        Label1.Text = Resource1.str504
        'chkReplicar.Text = Resource1.str416
        cmdAgregar.Text = Resource1.str10131
        cmdModificar.Text = Resource1.str2
        cmdEliminar.Text = Resource1.str3
        btnGrabar.Text = Resource1.str4
        'CmdSalir.Text = Resource1.str6
    End Sub

    Private Sub pInicializaControles()
        Dim STRID As String = Session("")

        STRID = "" : CargarCombo("SELECT ID_Cultivo, Descripcion FROM cultivos ORDER BY DESCRIPCION", "ID_cultivo", "DESCRIPCION", cboCultivo) : cboCultivo_SelectedIndexChanged(Nothing, Nothing)
        CargarCombo("SELECT ID_etapa, Descripcion FROM etapas  ORDER BY DESCRIPCION", "ID_etapa", "DESCRIPCION", cboEtapa) : cboEtapa_SelectedIndexChanged(Nothing, Nothing)
        'STRID = "" : CargarCombo("SELECT ID_TipoCosto, Descripcion FROM Tipo_Costo Where Id_TipoCosto in('I', 'R', 'M', 'H', 'O', 'C', 'F') ORDER BY ORDEN", "ID_tipocosto", "DESCRIPCION", cboTipoCosto) : cboTipoCosto_SelectedIndexChanged(Nothing, Nothing)
        CargarCombo("SELECT ID_Proveedor, Descripcion FROM Proveedores ORDER BY DESCRIPCION", "ID_Proveedor", "DESCRIPCION", cboProveedor)
        'STRID = "" : Call CargarCombo("Select Id_PerPla, Descripcion From PerPla Where Tipo_Rep = 'E' ORDER BY DESCRIPCION ", "Id_PerPla", "Id_PerPla", cboPerPla)

        'txtTC.Text = (Decimal.Parse(txtTC.Text)).ToString("0.0000")
        cboMoneda.Items.Clear()
        cboMoneda.Enabled = True
        cboMoneda.Items.Add(Resource1.str10004) 'Nacional str10004
        cboMoneda.Items.Add(Resource1.str10005) 'Extranjera str10005
        cboMoneda.SelectedIndex = 0

        'chkReplicar.CheckState = False
        cboTipoCosto.SelectedIndex = 0

        txtCantidad.Text = CStr(0)
        txtCostoUnitarioStandar.Text = CStr(0)
        TotMonto.Text = CStr(0)
        TotCantidad.Text = CStr(0)
        dtPicker3.Text = DateTime.Now.ToShortDateString() 'IIf(t_fechaIni.Text = "", Today, t_fechaIni.Text)

        Session("FrmCostosCosechas_Datatable") = Nothing
        Session("FrmCostosCosechas_PreviousSelectedIndex") = -1
    End Sub

    Private Function fValidaDatos() As Boolean
        fValidaDatos = True
        'If CDate(dtpicker3.Value) >= CDate(t_fechaIni.Text) And CDate(dtpicker3.Value) <= CDate(t_fechaFin.Text) Then fValidaDatos = True
        'If CDate(dtpicker5.Value) >= CDate(t_fechaIni.Text) And CDate(dtpicker5.Value) <= CDate(t_fechaFin.Text) Then fValidaDatos = True
        'If CDate(dtpicker4.Value) >= CDate(t_fechaIni.Text) And CDate(dtpicker4.Value) <= CDate(t_fechaFin.Text) Then fValidaDatos = True
        'If CDate(dtpicker4.Value) > CDate(dtpicker5.Value) Then fValidaDatos = True
        If cboZonaTrabajo.SelectedIndex = -1 Then
            fValidaDatos = False
            ShowErrorMessage(Resource1.str10538)
        End If

        'If cboActividad.SelectedIndex = -1 Then
        '    fValidaDatos = False
        '    ShowErrorMessage(Resource1.str10539)
        'End If

    End Function

    Private Sub AddRecord()
        'Dim strKey As String
        setCultureDecimalSeparator()

        If Not fValidaDatos() Then Exit Sub

        Dim MatrizAC() As Object = Session("FrmCostosCosechas_MatrizAC")
        Dim MatrizAx() As Object = Session("FrmCostosCosechas_MatrizAx")
        Dim MatrizCm() As Object = Session("FrmCostosCosechas_MatrizCm")
        Dim MatrizMq() As Object = Session("FrmCostosCosechas_MatrizMq")
        Dim MatrizCc() As Object = Session("FrmCostosCosechas_MatrizCc")
        Dim MatrizPr() As Object = Session("FrmCostosCosechas_MatrizPr")
        Dim MatrizTd() As Object = Session("FrmCostosCosechas_MatrizTd")
        Dim MatrizCt() As Object = Session("FrmCostosCosechas_MatrizCt")
        Dim MatrizCp() As Object = Session("FrmCostosCosechas_MatrizCp")
        Dim strIdCosto As String = Session("FrmCostosCosechas_IdCosto")

        Dim dr As DataRow
        Dim dt As New DataTable()
        dt = Session("FrmCostosCosechas_Datatable")

        If dt Is Nothing Then
            dt = New DataTable()
            dt.Columns.Add("Fecha")
            dt.Columns.Add("Cultivo")
            dt.Columns.Add("ZonaTrabajo")
            dt.Columns.Add("Etapa")
            dt.Columns.Add("Actividad")
            dt.Columns.Add(Resource1.str3005)
            dt.Columns.Add("Cantidad")
            dt.Columns.Add("CostoUnitario")
            dt.Columns.Add("MontoStandar")
            dt.Columns.Add("Campana")
            dt.Columns.Add("TipoCosto")
            dt.Columns.Add("IdZonaTrabajo")
            dt.Columns.Add("IdActividad")
            dt.Columns.Add("CodRecurso")
            dt.Columns.Add("TipoCambio")
            dt.Columns.Add("IdMoneda")
            dt.Columns.Add("IdEtapa")
            dt.Columns.Add("IdCultivo")
            dt.Columns.Add("IdCosto")
            dt.Columns.Add("Avance")
        End If

        Dim montoCantidad As Decimal = Decimal.Parse(txtCantidad.Text.Trim())
        Dim montoCUnitario As Decimal = Decimal.Parse(txtCostoUnitarioStandar.Text.Trim())

        dr = dt.NewRow()
        dr.Item(0) = dtPicker3.Text
        dr.Item(1) = cboCultivo.Text
        dr.Item(2) = cboZonaTrabajo.Text
        dr.Item(3) = cboEtapa.Text
        dr.Item(4) = cboActividad.Text
        dr.Item(5) = cboCodanx.Text
        dr.Item(6) = montoCantidad.ToString("###0.00")
        dr.Item(7) = montoCUnitario.ToString("###0.00")
        dr.Item(8) = (montoCantidad * montoCUnitario).ToString("###0.00")
        dr.Item(9) = IIf(String.IsNullOrEmpty(txtCampana.Text), "0", txtCampana.Text)
        dr.Item(10) = "C"

        Select Case Mid(dr.Item(10), 1, 1)
            Case "O"
                dr.Item(10) = Resource1.str538
            Case "I"
                dr.Item(10) = Resource1.str2012
            Case "F"
                dr.Item(10) = "F" & Resource1.str62
            Case "C"
                dr.Item(10) = "C" & Resource1.str11010
            Case "M"
                dr.Item(10) = Resource1.str401
            Case "R"
                dr.Item(10) = Resource1.str3012
            Case "H"
                dr.Item(10) = "H" & Resource1.str4001
        End Select

        dr.Item(11) = MatrizAx(cboZonaTrabajo.SelectedIndex)
        '   dr.Item(12) = MatrizCm(cboActividad.SelectedIndex)

        Select Case Mid(dr.Item(10), 1, 1)
            Case "I"
                dr.Item(13) = MatrizCc(cboCodanx.SelectedIndex)
            Case "F"
                dr.Item(13) = MatrizCc(cboCodanx.SelectedIndex)
            Case "C"
                dr.Item(13) = MatrizCc(cboCodanx.SelectedIndex)
            Case "M"
                dr.Item(13) = MatrizMq(cboCodanx.SelectedIndex)
            Case "R"
                dr.Item(13) = MatrizCp(cboCodanx.SelectedIndex)
            Case "H"
                dr.Item(13) = MatrizPr(cboCodanx.SelectedIndex)
        End Select

        dr.Item(14) = txtTC.Text
        dr.Item(15) = CStr(cboMoneda.SelectedIndex)
        dr.Item(16) = "NA0"
        dr.Item(17) = "COSTIN"



        dr.Item(18) = strIdCosto
        dr.Item(19) = IIf(String.IsNullOrEmpty(txtAvance.Text.Trim()), "0", txtAvance.Text.Trim())
        dt.Rows.Add(dr)

        Session("FrmCostosCosechas_PreviousSelectedIndex") = -1
        Session("FrmCostosCosechas_Datatable") = dt
        lvwAsientos.DataSource = dt
        lvwAsientos.DataBind()

        pSumaMontos()
    End Sub

    Function getRecord() As Boolean
        Dim StrIdenlace As String = Session("FrmCostosCosechas_Codigo")

        Dim bResult As Boolean = False
        Dim RS As New ADODB.Recordset
        Dim ssql As String = "roveedor = PROVEEDORES.id_proveedor  ;"
        RS = New ADODB.Recordset

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(ssql)
        RS.Open()

        setCultureDecimalSeparator()

        If Not RS.EOF Then
            'cboPerPla.Text = Buscar((RS.Fields("numero_parte").Value), cboPerPla)
            If RS.Fields("tc").Value = 0 Then
                cboMoneda.SelectedIndex = 0 'Moneda nacional
            Else
                cboMoneda.SelectedIndex = 1 'Moneda extranjera
            End If
            txtTC.Text = (Decimal.Parse(RS.Fields("Tipo_Cambio").Value)).ToString("##0.0000")
            txtCicloPeriodo.Text = RS.Fields("numero_parte").Value

            '======================
            Dim dr As DataRow
            Dim dt As New DataTable()
            dt = Session("FrmCostosCosechas_Datatable")

            If dt Is Nothing Then
                dt = New DataTable()
                dt.Columns.Add("Fecha")
                dt.Columns.Add("Cultivo")
                dt.Columns.Add("ZonaTrabajo")
                dt.Columns.Add("Etapa")
                dt.Columns.Add("Actividad")
                dt.Columns.Add(Resource1.str3005)
                dt.Columns.Add("Cantidad")
                dt.Columns.Add("CostoUnitario")
                dt.Columns.Add("MontoStandar")
                dt.Columns.Add("Campana")
                dt.Columns.Add("TipoCosto")
                dt.Columns.Add("IdZonaTrabajo")
                dt.Columns.Add("IdActividad")
                dt.Columns.Add("CodRecurso")
                dt.Columns.Add("TipoCambio")
                dt.Columns.Add("IdMoneda")
                dt.Columns.Add("IdEtapa")
                dt.Columns.Add("IdCultivo")
                dt.Columns.Add("IdCosto")
                dt.Columns.Add("Avance")
            End If

            Dim provID As String = ""
            Dim tipoCosto As String = ""
            While Not RS.EOF
                dr = dt.NewRow()
                dr.Item(0) = RS.Fields("costosfecha").Value
                dr.Item(1) = RS.Fields("culdes").Value
                dr.Item(2) = RS.Fields("zondes").Value
                dr.Item(3) = RS.Fields("etades").Value
                dr.Item(4) = RS.Fields("actdes").Value

                Select Case RS.Fields("tipo_costo").Value
                    Case "M", "R"
                        dr.Item(5) = RS.Fields("maqdes").Value
                    Case "H"
                        dr.Item(5) = RS.Fields("perdes").Value
                    Case "C"
                        dr.Item(5) = RS.Fields("prodes").Value
                    Case "O"
                        dr.Item(5) = ""
                End Select

                dr.Item(6) = Convert.ToDecimal(RS.Fields("cantidad").Value).ToString("###0.00")

                Dim amount As Decimal = IIf(RS.Fields("tc").Value = 0, RS.Fields("costo_unitario_standar").Value, RS.Fields("costo_unitario_standar").Value / IIf(String.IsNullOrEmpty(RS.Fields("Tipo_Cambio").Value), 0, RS.Fields("Tipo_Cambio").Value))
                dr.Item(7) = amount.ToString("###0.00")

                Dim amount2 As Decimal = IIf(RS.Fields("tc").Value = 0, RS.Fields("monto_standar").Value, RS.Fields("monto_standar").Value / IIf(String.IsNullOrEmpty(RS.Fields("Tipo_Cambio").Value), 0, RS.Fields("Tipo_Cambio").Value))
                dr.Item(8) = amount2.ToString("###0.00")

                dr.Item(9) = RS.Fields("campana").Value
                tipoCosto = RS.Fields("tipo_costo").Value
                dr.Item(10) = RS.Fields("tipo_costo").Value

                Select Case Mid(dr.Item(10), 1, 1)
                    Case "O"
                        dr.Item(10) = Resource1.str538
                    Case "I"
                        dr.Item(10) = Resource1.str2012
                    Case "F"
                        dr.Item(10) = "F" & Resource1.str62
                    Case "C"
                        dr.Item(10) = "C" & Resource1.str11010
                    Case "M"
                        dr.Item(10) = Resource1.str401
                    Case "R"
                        dr.Item(10) = Resource1.str3012
                    Case "H"
                        dr.Item(10) = "H" & Resource1.str4001
                End Select

                dr.Item(11) = RS.Fields("id_zonatrabajo").Value
                dr.Item(12) = RS.Fields("id_actividad").Value

                Select Case Mid(dr.Item(10), 1, 1)
                    Case "I"
                        dr.Item(13) = RS.Fields("id_producto").Value
                    Case "M", "R"
                        dr.Item(13) = RS.Fields("id_maquinaria").Value
                    Case "H"
                        dr.Item(13) = RS.Fields("id_personal").Value
                    Case Else
                        dr.Item(13) = RS.Fields("id_producto").Value
                End Select

                dr.Item(14) = RS.Fields("Tipo_Cambio").Value
                dr.Item(15) = IIf(RS.Fields("tc").Value = 0, 0, 1)
                dr.Item(16) = RS.Fields("id_etapa").Value
                dr.Item(17) = RS.Fields("id_cultivo").Value
                dr.Item(18) = RS.Fields("id_costo").Value
                dr.Item(19) = RS.Fields("id_enlace").Value.ToString().Trim()
                provID = RS.Fields("id_Proveedor").Value

                dt.Rows.Add(dr)
                RS.MoveNext()
            End While

            Dim MatrizProv() As Object = Session("FrmCostosCosechas_MatrizProv")
            'Session.Add("FrmCostosCosechas_provDesc") = provDesc
            Dim Index As Short = fBuscarPosCodEnMatriz(MatrizProv, provID)
            'If cboProveedor.SelectedIndex <> Index Then
            '    cboProveedor.SelectedIndex = Index
            'End If

            If tipoCosto = "C" Then
                cboTipoCosto.SelectedIndex = 5
                cboTipoCosto_SelectedIndexChanged(Nothing, Nothing)
            End If

            Session("FrmCostosCosechas_Datatable") = dt
            lvwAsientos.DataSource = dt
            lvwAsientos.DataBind()
            pSumaMontos()
            '======================

            bResult = True
        Else
            bResult = False
        End If

        Return bResult
    End Function

    Private Sub CargarFecha()
        Dim fec As String
        Dim rsTC As ADODB.Recordset

        Dim dfecha As DateTime = DateTime.Now 'IIf(t_fechaIni.Text = "", Today, t_fechaIni.Text)
        fec = dfecha.ToString("yyyy/MM/dd")

        Dim ssql As String = "select * from TIPOCAMBIO where fecha = convert(datetime,replace('" & (DateTime.Parse(fec)).ToString("yyyyMMdd") & "',',','/'))"
        rsTC = New ADODB.Recordset

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        rsTC.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        If Not rsTC.EOF Then
            txtTC.Text = rsTC.Fields(1).Value
        Else
            txtTC.Text = "0"

        End If

        txtTC.Text = (Decimal.Parse(txtTC.Text)).ToString("0.0000")
    End Sub

    Private Sub ShowErrorMessage(ByVal strMessage As String)
        dvMessage.Visible = True
        dvMessage.Attributes.Add("class", "alert alert-danger")
        lblResults.Text = strMessage
    End Sub

    Private Sub ShowOKMessage(ByVal strMessage As String)
        dvMessage.Visible = True
        dvMessage.Attributes.Add("class", "alert alert-success")
        lblResults.Text = strMessage
    End Sub

    Protected Sub txtCantidad_TextChanged(sender As Object, e As EventArgs)
        'getMontoStandar()
    End Sub

    Protected Sub txtCostoUnitarioStandar_TextChanged(sender As Object, e As EventArgs)
        'getMontoStandar()
    End Sub

    Private Sub getMontoStandar()
        Dim q, X As Object
        Dim t As Double

        q = 0
        X = 0
        t = 0

        'If IsNumeric(txtCantidad.Text) Then q = CDbl(FormatNumber(txtCantidad.Text, 2))
        If IsNumeric(txtCantidad.Text) Then q = Convert.ToDouble(txtCantidad.Text.Trim(), CultureInfo.InvariantCulture)
        'If IsNumeric(txtCostoUnitarioStandar.Text) Then X = CDbl(FormatNumber(txtCostoUnitarioStandar.Text, 2))
        If IsNumeric(txtCostoUnitarioStandar.Text) Then X = Convert.ToDouble(txtCostoUnitarioStandar.Text.Trim(), CultureInfo.InvariantCulture)

        t = q * X
        'txtMontoStandar.Text = Replace(FormatNumber(t, 2), ",", "")
        txtMontoStandar.Text = t.ToString("###0.00")
    End Sub

    Private Sub LoadPageResources()
        hdnStr4016.Value = Resource1.str4016
        hdnStr548.Value = Resource1.str548
        hdnStr9019.Value = Resource1.str9019
        hdnStr527.Value = Resource1.str527
        hdnStr528.Value = Resource1.str528
        hdnStr12104.Value = Resource1.str12104
        hdnStr12125.Value = Resource1.str12125
    End Sub

    Private Sub setCultureDecimalSeparator()
        Dim cultureName As String = System.Threading.Thread.CurrentThread.CurrentCulture.Name
        Dim ci As New CultureInfo(cultureName)
        If ci.NumberFormat.NumberDecimalSeparator <> "." Then
            ci.NumberFormat.NumberDecimalSeparator = "."
            Threading.Thread.CurrentThread.CurrentCulture = ci
        End If
    End Sub

    Protected Sub lvwAsientos_PreRender(sender As Object, e As EventArgs)
        If lvwAsientos.Rows.Count > 0 Then
            If Not lvwAsientos.HeaderRow Is Nothing Then
                lvwAsientos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        End If
    End Sub

    <WebMethod>
    Public Shared Function GetTipoCambioA(ByVal selectedDate As String) As String
        Dim exchangeDate As DateTime = DateTime.Parse(selectedDate, CultureInfo.InvariantCulture)
        Dim ssql As String = String.Format("select * from TIPOCAMBIO where fecha = '{0}'", exchangeDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture))
        Dim rsTC As New ADODB.Recordset
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection

        DBconn.Open(objBL.GetSQLConnection())
        rsTC.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        Dim tipoCambio As Decimal = Decimal.Zero

        If Not rsTC.EOF Then
            tipoCambio = rsTC.Fields(1).Value
        End If

        Return tipoCambio.ToString("0.0000", CultureInfo.InvariantCulture)
    End Function

    <WebMethod>
    Public Shared Function GetTipoCambio(ByVal year As String, ByVal month As String, ByVal day As String) As String
        Dim exchangeDate As DateTime = New DateTime(year, month, day)
        Dim ssql As String = String.Format("select * from TIPOCAMBIO where fecha = '{0}'", exchangeDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture))
        Dim rsTC As New ADODB.Recordset
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection

        DBconn.Open(objBL.GetSQLConnection())
        rsTC.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        Dim tipoCambio As Decimal = Decimal.Zero

        If Not rsTC.EOF Then
            tipoCambio = rsTC.Fields(1).Value
        End If

        Return tipoCambio.ToString("0.0000", CultureInfo.InvariantCulture)
    End Function

End Class