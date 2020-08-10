Imports System.Globalization
Imports System.Web.Services
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmCostos
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




        If Not Page.IsPostBack() Then
            setCultureDecimalSeparator()
            Call pInicializaControles()
            Call setCaptionsLabels()

            Dim strAction As String = Request.QueryString("Action")

            Select Case UCase(strAction)
                Case "NEW"
                    CargarFecha()
                    Session("frmCostos_Action") = "NEW"
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
                    Session("frmCostos_Action") = "EDIT"
            End Select

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

        e.Row.Cells(11).Visible = False
        e.Row.Cells(12).Visible = False
        e.Row.Cells(13).Visible = False
        e.Row.Cells(15).Visible = False
        e.Row.Cells(16).Visible = False
        e.Row.Cells(17).Visible = False
        e.Row.Cells(18).Visible = False
        e.Row.Cells(20).Visible = False
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
        Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
        Dim MatrizAx() As Object = Session("FrmCostos_MatrizAx")
        Dim MatrizCm() As Object = Session("FrmCostos_MatrizCm")
        Dim MatrizMq() As Object = Session("FrmCostos_MatrizMq")
        Dim MatrizCc() As Object = Session("FrmCostos_MatrizCc")
        Dim MatrizPr() As Object = Session("FrmCostos_MatrizPr")
        Dim MatrizTd() As Object = Session("FrmCostos_MatrizTd")
        Dim MatrizCt() As Object = Session("FrmCostos_MatrizCt")
        Dim MatrizCp() As Object = Session("FrmCostos_MatrizCp")

        Dim currentIndex As Integer = lvwAsientos.SelectedIndex
        Dim PreviousIndex As Integer = Convert.ToInt32(Session("frmCostos_PreviousSelectedIndex"))

        'Verify if a different row was selected
        If currentIndex = PreviousIndex Then
            Session("frmCostos_PreviousSelectedIndex") = currentIndex
            Exit Sub
        End If

        For Each row As GridViewRow In lvwAsientos.Rows
            row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
            row.ToolTip = "Seleccionar"
        Next

        Session.Add("frmCostos_PreviousSelectedIndex", currentIndex)
        Dim index As Short = 0

        Dim gRow As GridViewRow = lvwAsientos.Rows(currentIndex)
        gRow.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
        gRow.ToolTip = ""

        Dim dtPeriodo As DateTime = DateTime.Parse(gRow.Cells(0).Text)
        dtPicker3.Text = dtPeriodo.ToShortDateString
        txtTC.Text = gRow.Cells(14).Text

        index = fBuscarPosCodEnMatriz(MatrizCt, gRow.Cells(17).Text)
        If cboCultivo.SelectedIndex <> index Then
            cboCultivo.SelectedIndex = index
            cboCultivo_SelectedIndexChanged(Nothing, Nothing)
        End If

        MatrizAx = Session("FrmCostos_MatrizAx")
        index = fBuscarPosCodEnMatriz(MatrizAx, gRow.Cells(11).Text)
        If cboZonaTrabajo.SelectedIndex <> index Then
            cboZonaTrabajo.SelectedIndex = index
            cboZonaTrabajo_SelectedIndexChanged(Nothing, Nothing)
        End If

        MatrizTd = Session("FrmCostos_MatrizTd")
        index = fBuscarPosCodEnMatriz(MatrizTd, gRow.Cells(16).Text)
        If cboEtapa.SelectedIndex <> index Then
            cboEtapa.SelectedIndex = index
            cboEtapa_SelectedIndexChanged(Nothing, Nothing)
        End If

        MatrizCm = Session("FrmCostos_MatrizCm")
        cboActividad.SelectedIndex = fBuscarPosCodEnMatriz(MatrizCm, gRow.Cells(12).Text)

        MatrizAC = Session("FrmCostos_MatrizAC")

        index = fBuscarPosCodEnMatriz(MatrizAC, gRow.Cells(20).Text)
        If cboTipoCosto.SelectedIndex <> index Then
            cboTipoCosto.SelectedIndex = index
            cboTipoCosto_SelectedIndexChanged(Nothing, Nothing)
        End If

        Dim indexRecursos As Integer = 0
        MatrizMq = Session("FrmCostos_MatrizMq")
        MatrizCc = Session("FrmCostos_MatrizCc")
        MatrizPr = Session("FrmCostos_MatrizPr")
        MatrizCp = Session("FrmCostos_MatrizCp")
        Select Case UCase(MatrizAC(cboTipoCosto.SelectedIndex))
            Case "I"
                indexRecursos = fBuscarPosCodEnMatriz(MatrizCc, gRow.Cells(13).Text)
            Case "M"
                indexRecursos = fBuscarPosCodEnMatriz(MatrizMq, gRow.Cells(13).Text)
            Case "R"
                indexRecursos = fBuscarPosCodEnMatriz(MatrizCp, gRow.Cells(13).Text)
            Case "H"
                indexRecursos = fBuscarPosCodEnMatriz(MatrizPr, gRow.Cells(13).Text)
            Case "O"
                'cboCodAnx.ListIndex = -1
        End Select

        If cboCodanx.SelectedIndex <> indexRecursos Then
            cboCodanx.SelectedIndex = indexRecursos
            cboCodanx_SelectedIndexChanged(Nothing, Nothing)
        End If

        txtCampana.Text = gRow.Cells(9).Text
        txtCantidad.Text = gRow.Cells(6).Text
        txtCostoUnitarioStandar.Text = gRow.Cells(7).Text
        txtMontoStandar.Text = gRow.Cells(8).Text
        txtAvance.Text = gRow.Cells(19).Text
        Session("FrmCostos_IdCosto") = gRow.Cells(18).Text
    End Sub

    Protected Sub cboCodanx_SelectedIndexChanged(sender As Object, e As EventArgs)


        setCultureDecimalSeparator()

        Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
        Dim MatrizPC() As Object = Session("FrmCostos_MatrizPC")
        Dim MatrizPTC() As Object = Session("FrmCostos_MatrizPTC")

        Select Case UCase(MatrizAC(cboTipoCosto.SelectedIndex))
            Case "I"
                txtCostoUnitarioStandar.Text = MatrizPC(cboCodanx.SelectedIndex)

                If MatrizPTC(cboCodanx.SelectedIndex) = 0 Then
                    txtCostoUnitarioStandar.Text = IIf(cboMoneda.SelectedIndex = 0, MatrizPC(cboCodanx.SelectedIndex), MatrizPC(cboCodanx.SelectedIndex) / Val(txtTC.Text))
                Else
                    txtCostoUnitarioStandar.Text = IIf(cboMoneda.SelectedIndex = 1, MatrizPC(cboCodanx.SelectedIndex), MatrizPC(cboCodanx.SelectedIndex) * Val(txtTC.Text))
                End If

            Case "O"
                txtCostoUnitarioStandar.Text = CStr(0)

            Case Else
                If cboMoneda.SelectedIndex = 0 Then
                    txtCostoUnitarioStandar.Text = MatrizPC(cboCodanx.SelectedIndex)
                Else
                    Dim amount As Double = MatrizPC(cboCodanx.SelectedIndex) / CDbl(txtTC.Text)
                    txtCostoUnitarioStandar.Text = IIf(CDbl(txtTC.Text) > 0, amount.ToString("0.00"), MatrizPC(cboCodanx.SelectedIndex))
                End If
        End Select

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
        Dim MatrizCt() As Object = Session("FrmCostos_MatrizCt")
        CargarCombo("SELECT ID_zonatrabajo, Descripcion, campana FROM ZONA_TRABAJO  where tipo='C' ORDER BY DESCRIPCION", "ID_ZONATRABAJO", "DESCRIPCION", cboZonaTrabajo)
    End Sub

    Protected Sub cboEtapa_SelectedIndexChanged(sender As Object, e As EventArgs)
        cboActividad.Items.Clear()
        Dim MatrizTd() As Object = Session("FrmCostos_MatrizTd")
        CargarCombo("SELECT ID_actividad, Descripcion FROM Actividades  ORDER BY DESCRIPCION", "ID_actividad", "DESCRIPCION", cboActividad)
    End Sub

    Protected Sub cboTipoCosto_SelectedIndexChanged(sender As Object, e As EventArgs)
        cboCodanx.Items.Clear()
        Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
        dvInfo.Visible = True
        dvProveedor.Visible = False
        lblRecurso.Text = Resource1.str14
        hdnBuscarProductos.Value = ""

        'lblCultivo.Visible = True
        'cboCultivo.Visible = True
        'lblEtapa.Visible = True
        'cboEtapa.Visible = True
        lblActividad.Visible = True
        cboActividad.Visible = True
        lblAvance.Text = "" '"Avance"

        Select Case UCase(MatrizAC(cboTipoCosto.SelectedIndex))
            Case "I"
                CargarCombo("SELECT ID_Producto,   Descripcion, costo, tc FROM Productos Where Tipo = 'I' ORDER BY DESCRIPCION", "ID_Producto", "DESCRIPCION", cboCodanx) 'INSUMOS
                lblAvance.Text = Resource1.str99999986
            Case "M" : CargarCombo("SELECT ID_Maquinaria as ID_Maquinaria1, Descripcion, costo FROM Maquinas Where Tipo = 'M' ORDER BY DESCRIPCION", "ID_Maquinaria1", "DESCRIPCION", cboCodanx) 'MAQUINAS
                lblAvance.Text = Resource1.str9264
            Case "R"
                CargarCombo("SELECT ID_Maquinaria as ID_Maquinaria2, Descripcion,COSTO FROM Maquinas Where Tipo = 'B' ORDER BY DESCRIPCION", "ID_Maquinaria2", "DESCRIPCION", cboCodanx) 'BOMBAS DE AGUA
                lblAvance.Text = Resource1.str10590
            Case "H" : CargarCombo("SELECT PERSONAL.* FROM PERSONAL WHERE (PERSONAL.estado <> 0) ORDER BY PERSONAL.nombre", "ID_Personal", "Nombre", cboCodanx) 'PERSONAL
                lblAvance.Text = Resource1.str9264
            Case "O" : cboCodanx.Items.Clear()
                lblAvance.Text = ""
            Case "C" : dvInfo.Visible = False
                dvProveedor.Visible = True
                lblRecurso.Text = Resource1.str544
                hdnBuscarProductos.Value = "Productos"
                CargarCombo("SELECT ID_Producto,   Descripcion, costo, tc FROM Productos Where Tipo = 'I' ORDER BY DESCRIPCION", "ID_Producto", "DESCRIPCION", cboCodanx)
                'CargarCombo("SELECT ID_Proveedor, Descripcion FROM Proveedores ORDER BY DESCRIPCION", "ID_Proveedor", "DESCRIPCION", cboProveedor)
            Case "F"
                lblCultivo.Visible = False
                cboCultivo.Visible = False
                lblEtapa.Visible = False
                cboEtapa.Visible = False
                lblActividad.Visible = False
                cboActividad.Visible = False
                lblRecurso.Text = Resource1.str544
                hdnBuscarProductos.Value = "Productos"
                CargarCombo("SELECT ID_Producto,   Descripcion, costo, tc FROM Productos Where Tipo = 'I' ORDER BY DESCRIPCION", "ID_Producto", "DESCRIPCION", cboCodanx)
        End Select
    End Sub

    Protected Sub cboZonaTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim MatrizCa As Object = Session("FrmCostos_MatrizCa")

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
        dvMessage.Visible = False

        If Not fValidaDatos() Then Exit Sub

        If Session("frmCostos_IdCosto") = Nothing Then
            Exit Sub
        End If

        Dim idCosto As String = Session("frmCostos_IdCosto")
        Dim dtContent As New DataTable()
        dtContent = Session("frmCostos_Datatable")

        Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
        Dim MatrizAx() As Object = Session("FrmCostos_MatrizAx")
        Dim MatrizCm() As Object = Session("FrmCostos_MatrizCm")
        Dim MatrizMq() As Object = Session("FrmCostos_MatrizMq")
        Dim MatrizCc() As Object = Session("FrmCostos_MatrizCc")
        Dim MatrizPr() As Object = Session("FrmCostos_MatrizPr")
        Dim MatrizTd() As Object = Session("FrmCostos_MatrizTd")
        Dim MatrizCt() As Object = Session("FrmCostos_MatrizCt")
        Dim MatrizCp() As Object = Session("FrmCostos_MatrizCp")
        Dim strIdCosto As String = Session("FrmCostos_IdCosto").ToString()

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
        row.Item(10) = MatrizAC(cboTipoCosto.SelectedIndex)

        Select Case Mid(row.Item(10), 1, 1)
            Case "O"
                row.Item(10) = Resource1.str538
            Case "I"
                row.Item(10) = Resource1.str2012
            Case "F"
                row.Item(10) = "F" & Resource1.str62
            Case "C"
                row.Item(10) = "C" & Resource1.str11010
            Case "M"
                row.Item(10) = Resource1.str401
            Case "R"
                row.Item(10) = Resource1.str3012
            Case "H"
                row.Item(10) = "H" & Resource1.str4001
        End Select

        row.Item(11) = MatrizAx(cboZonaTrabajo.SelectedIndex)
        row.Item(12) = MatrizCm(cboActividad.SelectedIndex)

        Select Case Mid(row.Item(10), 1, 1)
            Case "I"
                row.Item(13) = MatrizCc(cboCodanx.SelectedIndex)
            Case "M"
                row.Item(13) = MatrizMq(cboCodanx.SelectedIndex)
            Case "R"
                row.Item(13) = MatrizCp(cboCodanx.SelectedIndex)
            Case "H"
                row.Item(13) = MatrizPr(cboCodanx.SelectedIndex)
        End Select

        row.Item(14) = txtTC.Text
        row.Item(15) = CStr(cboMoneda.SelectedIndex)
        row.Item(16) = MatrizTd(cboEtapa.SelectedIndex)
        row.Item(17) = MatrizCt(cboCultivo.SelectedIndex)
        row.Item(18) = strIdCosto
        row.Item(19) = IIf(String.IsNullOrEmpty(txtAvance.Text.Trim()), "0", txtAvance.Text.Trim())
        row.Item("TipoCostoID") = MatrizAC(cboTipoCosto.SelectedIndex)

        Session.Add("frmCostos_Datatable", dtContent)
        lvwAsientos.DataSource = dtContent
        lvwAsientos.DataBind()
        pSumaMontos()
        'Session("frmCostos_PreviousSelectedIndex") = -1
    End Sub

    Protected Sub cmdEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmCostos_IdCosto") = Nothing Then
            Exit Sub
        End If

        Dim idCosto As String = Session("frmCostos_IdCosto")
        Dim dtContent As New DataTable()
        dtContent = Session("frmCostos_Datatable")

        Dim currentIndex As Integer = lvwAsientos.SelectedIndex
        dtContent.Rows(currentIndex).Delete()

        Session("frmCostos_PreviousSelectedIndex") = -1
        Session("frmCostos_Datatable") = dtContent
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

        'If Val(txtCampana.Text) <= 0 And hdnBuscarProductos.Value <> "Productos" Then
        If Val(txtCampana.Text) <= 0 Then
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

    End Sub

    Private Sub pInsertarRegistro(ByVal IdZonaTrabajo As String, ByVal IdActividad As String, ByVal IdProducto As String, ByVal IdMaquina As String, ByVal IdPersonal As String, ByVal cantidad As Double, ByVal CostoUnitario As Double, ByVal MontoStandar As Double, ByVal campana As Double, ByVal tc As Short, ByVal TipoCambio As Double, ByVal numeroparte As String, ByVal tipocosto As String, ByVal fecha As Date, ByVal IdCosto As String, ByVal IdProveedor As String, ByVal sObservaciones As String, ByVal Avance As String)
        Dim ssql As String
        Dim StrMoned As String

        'Get BusinessUser
        Dim objBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"

        Dim strAction As String = Request.QueryString("Action")
        If strAction = "NEW" Then
            Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
            Select Case UCase(MatrizAC(cboTipoCosto.SelectedIndex))
                Case "F" : sObservaciones = "Cosecha"
            End Select


        Else
            IdCosto = Session("frmCostos_Codigo").ToString()

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


        Return sResult
    End Function

    Private Sub Limpia()
        lvwAsientos.DataSource = Nothing
        lvwAsientos.DataBind()
        TotCantidad.Text = ""
        TotMonto.Text = ""
        Session("frmCostos_Datatable") = Nothing
    End Sub

    Private Sub CargarCombo(ByRef ssql As String, ByRef idField As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim i, intOrden As Short
        Dim MatrizPC() As Object = Session("FrmCostos_MatrizPC")
        Dim MatrizPTC() As Object = Session("FrmCostos_MatrizPTC")
        Dim MatrizCt() As Object = Session("FrmCostos_MatrizCt")
        Dim MatrizAx() As Object = Session("FrmCostos_MatrizAx")
        Dim MatrizCa() As Object = Session("FrmCostos_MatrizCa")
        Dim MatrizTd() As Object = Session("FrmCostos_MatrizTd")
        Dim MatrizCc() As Object = Session("FrmCostos_MatrizCc")
        Dim MatrizMq() As Object = Session("FrmCostos_MatrizMq")
        Dim MatrizCp() As Object = Session("FrmCostos_MatrizCp")
        Dim MatrizPr() As Object = Session("FrmCostos_MatrizPr")
        Dim MatrizCm() As Object = Session("FrmCostos_MatrizCm")
        Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
        Dim MatrizProv() As Object = Session("FrmCostos_MatrizProv")

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

            Dim STRID As String = Session("FrmCostos_STRID")
            If adoTabla.Fields(idField).Value = STRID Then intOrden = i
            i = i + 1
            adoTabla.MoveNext()
        End While

        Session("FrmCostos_MatrizPC") = MatrizPC
        Session("FrmCostos_MatrizPTC") = MatrizPTC
        Session("FrmCostos_MatrizCt") = MatrizCt
        Session("FrmCostos_MatrizAx") = MatrizAx
        Session("FrmCostos_MatrizCa") = MatrizCa
        Session("FrmCostos_MatrizTd") = MatrizTd
        Session("FrmCostos_MatrizCc") = MatrizCc
        Session("FrmCostos_MatrizMq") = MatrizMq
        Session("FrmCostos_MatrizCp") = MatrizCp
        Session("FrmCostos_MatrizPr") = MatrizPr
        Session("FrmCostos_MatrizAC") = MatrizAC
        Session("FrmCostos_MatrizCm") = MatrizCm
        Session("FrmCostos_MatrizProv") = MatrizProv

        If Cbo.Items.Count > 0 Then Cbo.SelectedIndex = intOrden
    End Sub

    Private Sub pSumaMontos()
        Dim dblSuma As Double
        Dim dt As New DataTable()
        dt = Session("frmCostos_Datatable")

        For Each row As DataRow In dt.Rows
            dblSuma = dblSuma + Convert.ToDouble(IIf(row.Item(8) = "", 0, row.Item(8)), CultureInfo.InvariantCulture)
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
        STRID = "" : CargarCombo("SELECT ID_TipoCosto, Descripcion FROM Tipo_Costo Where Id_TipoCosto in('I', 'R', 'M', 'H', 'O') ORDER BY ORDEN", "ID_tipocosto", "DESCRIPCION", cboTipoCosto) : cboTipoCosto_SelectedIndexChanged(Nothing, Nothing)
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

        Session("frmCostos_Datatable") = Nothing
        Session("frmCostos_PreviousSelectedIndex") = -1
    End Sub

    Private Function fValidaDatos() As Boolean
        fValidaDatos = True

        If cboZonaTrabajo.SelectedIndex = -1 Then
            fValidaDatos = False
            ShowErrorMessage(Resource1.str10538)
        End If

        If cboActividad.SelectedIndex = -1 Then
            fValidaDatos = False
            ShowErrorMessage(Resource1.str10539)
        End If

    End Function

    Private Sub AddRecord()
        'Dim strKey As String
        setCultureDecimalSeparator()

        If Not fValidaDatos() Then Exit Sub

        Dim MatrizAC() As Object = Session("FrmCostos_MatrizAC")
        Dim MatrizAx() As Object = Session("FrmCostos_MatrizAx")
        Dim MatrizCm() As Object = Session("FrmCostos_MatrizCm")
        Dim MatrizMq() As Object = Session("FrmCostos_MatrizMq")
        Dim MatrizCc() As Object = Session("FrmCostos_MatrizCc")
        Dim MatrizPr() As Object = Session("FrmCostos_MatrizPr")
        Dim MatrizTd() As Object = Session("FrmCostos_MatrizTd")
        Dim MatrizCt() As Object = Session("FrmCostos_MatrizCt")
        Dim MatrizCp() As Object = Session("FrmCostos_MatrizCp")
        Dim strIdCosto As String = Session("FrmCostos_IdCosto")

        Dim dr As DataRow
        Dim dt As New DataTable()
        dt = Session("frmCostos_Datatable")

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
            dt.Columns.Add("TipoCostoID")
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
        dr.Item(10) = MatrizAC(cboTipoCosto.SelectedIndex)

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
        dr.Item(12) = MatrizCm(cboActividad.SelectedIndex)

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
        dr.Item(16) = MatrizTd(cboEtapa.SelectedIndex)
        dr.Item(17) = MatrizCt(cboCultivo.SelectedIndex)
        dr.Item(18) = strIdCosto
        dr.Item(19) = IIf(String.IsNullOrEmpty(txtAvance.Text.Trim()), "0", txtAvance.Text.Trim())
        dr.Item("TipoCostoID") = MatrizAC(cboTipoCosto.SelectedIndex)
        dt.Rows.Add(dr)

        Session("frmCostos_PreviousSelectedIndex") = -1
        Session("frmCostos_Datatable") = dt
        lvwAsientos.DataSource = dt
        lvwAsientos.DataBind()

        pSumaMontos()
    End Sub

    Function getRecord() As Boolean
        Dim StrIdenlace As String = Session("frmCostos_Codigo")

        Dim bResult As Boolean = False
        Dim RS As New ADODB.Recordset
        Dim ssql As String = ""
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
            dt = Session("frmCostos_Datatable")

            If dt Is Nothing Then

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
                    Case "I"
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
                dr.Item("TipoCostoID") = tipoCosto
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

            Dim MatrizProv() As Object = Session("frmCostos_MatrizProv")
            'Session.Add("frmCostos_provDesc") = provDesc
            Dim Index As Short = fBuscarPosCodEnMatriz(MatrizProv, provID)
            If cboProveedor.SelectedIndex <> Index Then
                cboProveedor.SelectedIndex = Index
            End If

            If tipoCosto = "C" Then
                cboTipoCosto.SelectedIndex = 5
                cboTipoCosto_SelectedIndexChanged(Nothing, Nothing)
            End If

            Session("frmCostos_Datatable") = dt
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

    Private Sub GetCorrelativo()
        Dim bResult As Boolean = False

        Dim RSUpdate As New ADODB.Recordset
        Dim sqlUpdate As String = "UPDATE dbo.Contador SET Valor = Valor + 1 WHERE Campo LIKE 'CostosDiversos'"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        DBconn.Execute(sqlUpdate)
        DBconn.Close()

        Dim RS As New ADODB.Recordset
        Dim ssql As String = "SELECT Valor FROM dbo.Contador WHERE Campo LIKE 'CostosDiversos'"
        RS = New ADODB.Recordset

        DBconn.Open()
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(ssql)
        RS.Open()

        Dim numCorrelativo As Long = 0
        If Not RS.EOF Then
            numCorrelativo = RS.Fields(0).Value
        End If

        txtCicloPeriodo.Text = numCorrelativo.ToString()
    End Sub

End Class