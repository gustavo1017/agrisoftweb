Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmProductos
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            Dim STRID As String = ""
            Session.Add("FrmProductos_STRID", STRID)

            STRID = "" : Call Cargarcombo("select * from LINEASPRODUCTOS order by descripcion", "id_linea", "descripcion", (cboLinea))
            STRID = "" : Call Cargarcombo("select * from unidades order by descripcion", "id_unidad", "descripcion", cboUnidad)

            Dim strAction As String = Request.QueryString("Action")
            Select Case UCase(strAction)
                Case "NEW"
                    clearTextBox()
                    Session.Add("frmProductos_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str107
                    End If

                    Session.Add("frmProductos_Action", "EDIT")
            End Select
        End If

        hdnStr109.Value = Resource1.str109
        hdnStr108.Value = Resource1.str108
        hdnStr528.Value = Resource1.str528

    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        Dim STRID As String = ""
        Dim strAction As String = Request.QueryString("Action")
        Dim Openform As String = Session("frmProductos_Openform")

        If Validar() Then
            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed() Then
                        Select Case UCase(Openform)
                            Case "FRMAPLICACIONINSUMOS", "FRMCOSECHAS", "FRMPLANIFICACION", "FRMREQUERIMIENTOS"
                                STRID = txtId.Text
                            Case Else
                                'frmProductos_View.Refrescar()
                        End Select
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str104
                    End If
                Case "EDIT"
                    If Edit() Then
                        'frmProductos_View.Refrescar()
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str105
                    End If
            End Select

            Session.Add("FrmProductos_STRID", STRID)
        End If
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
    End Sub

    Private Sub setCaptionsLabels()
        Title = Resource1.str16
        'lblMoneda.Text = Resource1.str10003
        lblCodigo.Text = Resource1.str102
        lblDescripcion.Text = Resource1.str103
        lblCostoUnitarioStandar.Text = Resource1.str514
        'rbtnTipo(1).Text = Resource1.str2012
        'rbtnTipo(0).Text = Resource1.str544
        lblLinea.Text = Resource1.str113
        Label1.Text = Resource1.str702
        'Label2.Text = Resource1.str111
        rbtnInsumos.Text = Resource1.str2012
        rbtnProductos.Text = Resource1.str101
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        'btnCancelar.Text = My.Resources.Resource1.Resource1.str5
    End Sub

    Function getRecord() As Boolean
        Dim bResult As Boolean = False
        Dim codigo As String = Session("frmProductos_Codigo")
        Dim RS As New ADODB.Recordset
        Dim ssql As String = "SELECT productos.id_codbarras,productos.ChkIGV,productos.vanaquel,precio_venta,productos.pbruto,productos.pneto,PRODUCTOS.id_producto, stock_min,PRODUCTOS.descripcion as pdes, PRODUCTOS.costo ,PRODUCTOS.TC, case when tipo='P' then 'Producto' else 'Insumo' end AS tipos, LINEASPRODUCTOS.descripcion as lides, UNIDADES.Descripcion AS UNIDES " & "FROM LINEASPRODUCTOS INNER JOIN (PRODUCTOS INNER JOIN UNIDADES ON PRODUCTOS.id_unidad = UNIDADES.Id_unidad) ON LINEASPRODUCTOS.id_linea = PRODUCTOS.id_linea " & "WHERE (id_producto='" & codigo & "') ;"
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        Dim tipoProducto As String

        DBconn.Open(objBL.GetSQLConnection())
        RS = DBconn.Execute(ssql)

        If Not RS.EOF Then
            txtId.Text = RS.Fields("id_producto").Value
            tipoProducto = RS.Fields("tipos").Value

            If tipoProducto = "Insumo" Then
                rbtnInsumos.Checked = True
            Else
                rbtnProductos.Checked = True
            End If

            If Len(RS.Fields("pdes").Value) >= 4 Then
                descripcion.Text = Mid(RS.Fields("pdes").Value, 1, Len(RS.Fields("pdes").Value) - 4)
            Else
                descripcion.Text = RS.Fields("pdes").Value
            End If

            costo_unitario_standar.Text = IIf(RS.Fields("costo").Value > 0, Convert.ToDecimal(RS.Fields("costo").Value).ToString("0.00", CultureInfo.InvariantCulture), "")
            cboLinea.Text = Buscar((RS.Fields("lides").Value), cboLinea)
            cboUnidad.Text = Buscar((RS.Fields("unides").Value), cboUnidad)
            txtId.Enabled = False
            bResult = True
        Else
            bResult = False
        End If

        RS.Close()
        Return bResult
    End Function

    Function Add_Renamed() As Boolean
        Dim boolResult As Boolean = False
        Dim MatrizZ() As Object = Session("frmProductos_MatrizZ")
        Dim MatrizU() As Object = Session("frmProductos_MatrizU")
        Dim tipoItem As String

        If rbtnInsumos.Checked Then
            tipoItem = "I"
        Else
            tipoItem = "P"
        End If

        Dim ssql As String = "insert into PRODUCTOS (id_CodBarras,precio_venta,chkigv,vanaquel,id_producto,stock_min,id_linea,descripcion,costo,TC,id_unidad, tipo,pbruto,pneto) "
        'ssql = ssql & "values('" & TXTCODBARRAS.Text & "'," & txt_precioventa.Text & ",0," & txtvidaanaquel.Text & ",'" & ID.Text & "'," & TXTStockMin.Text & ", '" & Matrizz(cboLinea.SelectedIndex) & "' ,'" & descripcion.Text & " ' + '" & MatrizU(cboUnidad.SelectedIndex) & "'," & costo_unitario_standar.Text & ", 0, '" & MatrizU(cboUnidad.SelectedIndex) & "' ,"
        ' De acuerdo con Juan los Textbox faltantes, los valores se completarán con "1"
        ssql = ssql & "values('" & "1" & "'," & "1" & ",0," & "1" & ",'" & txtId.Text & "'," & "1" & ", '" & MatrizZ(cboLinea.SelectedIndex) & "' ,'" & descripcion.Text & " ' + '" & MatrizU(cboUnidad.SelectedIndex) & "'," & costo_unitario_standar.Text & ", 0, '" & MatrizU(cboUnidad.SelectedIndex) & "' ,"
        ssql = ssql & "'" & tipoItem & "',0,0)"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-success")
            lblResult.Text = Resource1.str543
            boolResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(dctException)
            boolResult = False
        End Try

        Return boolResult
    End Function

    Function Edit() As Boolean
        Dim ssql As String = ""
        Dim MatrizZ() As Object = Session("frmProductos_MatrizZ")
        Dim MatrizU() As Object = Session("frmProductos_MatrizU")
        Dim bResult As Boolean = True
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        Dim tipoItem As String

        Try
            'Get BusinessUser
            Dim businessUser = "DEMO01"

            'AuditoriaBackup(Title, "Fundo0", DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"), "Modifico lo nuevo es ", txtId.Text, descripcion.Text, cboUnidad.Text, costo_unitario_standar.Text, txt_precioventa.Text, cboLinea.Text, cboUnidad.Text, TXTStockMin.Text, Me.txtpbruto.Text, Me.txtpesoneto.Text, TXTCODBARRAS.Text, txtvidaanaquel.Text)
            AuditoriaBackup(Title, businessUser, DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"), "Modifico lo nuevo es ", txtId.Text, descripcion.Text, cboUnidad.Text, costo_unitario_standar.Text, "1", cboLinea.Text, cboUnidad.Text, "1", "1", "1", "1", "1")

            If rbtnInsumos.Checked Then
                tipoItem = "I"
            Else
                tipoItem = "P"
            End If

            ssql = "update PRODUCTOS set "
            ssql = ssql & "descripcion=ltrim(rtrim('" & descripcion.Text & " ' + '" & MatrizU(cboUnidad.SelectedIndex) & "')), "
            ssql = ssql & "costo=" & costo_unitario_standar.Text & ", "
            ssql = ssql & "id_linea='" & MatrizZ(cboLinea.SelectedIndex) & "', "
            ssql = ssql & "id_unidad='" & MatrizU(cboUnidad.SelectedIndex) & "', "
            ssql = ssql & "tipo = '" & tipoItem & "' "
            ssql = ssql & "where id_producto='" & txtId.Text & "'"

            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-success")
            lblResult.Text = Resource1.str543
            bResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(dctException)

            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str305
            bResult = False
        End Try

        Return bResult
    End Function

    Function Validar() As Boolean
        Dim ssql As String = ""
        Dim RS As New ADODB.Recordset
        Dim bResult As Boolean = False
        Dim strAction As String = Request.QueryString("Action")
        Dim MatrizU() As Object = Session("frmProductos_MatrizU")

        'Remove not allowed characters
        descripcion.Text = descripcion.Text.Replace(vbTab, "")
        txtId.Text = txtId.Text.Replace(vbTab, "")
        costo_unitario_standar.Text = costo_unitario_standar.Text.Replace(vbTab, "")

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        'If strAction = "NEW" Then
        'ssql = " SELECT DESCRIPCION From PRODUCTOS WHERE PRODUCTOS.DESCRIPCION='" & descripcion.Text & " ' + '" & MatrizU(cboUnidad.SelectedIndex) & "'; "
        ssql = String.Format("SELECT DESCRIPCION From dbo.PRODUCTOS (NOLOCK) WHERE DESCRIPCION = '{0} {1}' AND id_producto <> '{2}'", descripcion.Text.Trim(), MatrizU(cboUnidad.SelectedIndex), txtId.Text.Trim())
        RS.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        If Not RS.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resources.Resource1.str999999 '"Existe un producto con la misma descripcion y unidad. Modifique la descripcion o la unidad "

            bResult = False
            RS.Close()
            Return bResult
        End If

        RS.Close()
        RS = Nothing
        'End If

        txtId.Text = UCase(txtId.Text)
        descripcion.Text = UCase(descripcion.Text)
        bResult = True

        Return bResult
    End Function

    Private Sub Cargarcombo(ByRef ssql As String, ByRef id As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim adotabla As New ADODB.Recordset
        Dim I, intOrden As Short
        If Cbo.Items.Count > 0 Then intOrden = Cbo.SelectedIndex
        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        adotabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        Dim MatrizZ() As Object = Session("frmProductos_MatrizZ")
        Dim MatrizZC() As Object = Session("frmProductos_MatrizZC")
        Dim MatrizU() As Object = Session("frmProductos_MatrizU")
        Dim MatrizUD() As Object = Session("frmProductos_MatrizUD")

        Do While Not adotabla.EOF
            Select Case id
                Case "id_linea" 'Linea
                    ReDim Preserve MatrizZ(I)
                    ReDim Preserve MatrizZC(I)

                    MatrizZ(I) = adotabla.Fields(id).Value
                    MatrizZC(I) = RTrim(adotabla.Fields(Desc).Value)

                Case "id_unidad" '
                    ReDim Preserve MatrizU(I)
                    ReDim Preserve MatrizUD(I)
                    MatrizU(I) = adotabla.Fields(id).Value
                    MatrizUD(I) = RTrim(adotabla.Fields(Desc).Value)

            End Select

            Cbo.Items.Add(adotabla.Fields(Desc).Value)
            If adotabla.Fields(id).Value = Session("FrmProductos_STRID").ToString() Then
                intOrden = I
            End If
            I = I + 1
            adotabla.MoveNext()
        Loop

        Session.Add("frmProductos_MatrizZ", MatrizZ)
        Session.Add("frmProductos_MatrizZC", MatrizZC)
        Session.Add("frmProductos_MatrizU", MatrizU)
        Session.Add("frmProductos_MatrizUD", MatrizUD)
        Cbo.SelectedIndex = intOrden
    End Sub
End Class