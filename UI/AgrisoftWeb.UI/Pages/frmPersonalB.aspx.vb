Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmPersonalB
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()
        CboTipoDoc.Visible = False

        If Not Page.IsPostBack() Then

            Dim STRID As String = ""
            Dim Openform As String = ""

            cboEstado.Items.Add("A - Activo")
            cboEstado.Items.Add("I - Inactivo")
            cboEstado.SelectedIndex = 0

            'Get BusinessUser
            Dim objBL As New GenericMethods("Fundo0")
            Dim businessUser = "DEMO01"

            Session.Add("FrmPersonalB_STRID", STRID)
            Cargarcombo("SELECT CICLODEPAGO.Id_ciclopago, CICLODEPAGO.Descripcion, CICLODEPAGO.Parametrodelsistema, CICLODEPAGO.factor, CICLODEPAGO.ComentarioBoleta FROM CICLODEPAGO INNER JOIN USUARIOCCOSTOS ON CICLODEPAGO.id_fundo = USUARIOCCOSTOS.id_fundo WHERE  ID_CICLOPAGO<>'MENS' AND  USUARIOCCOSTOS.Id_usuario = '" & businessUser & "' order by CICLODEPAGO.Descripcion", "id_ciclopago", "descripcion", cboCiclo)

            Dim strAction As String = Request.QueryString("Action")
            If strAction <> "NEW" Then
                STRID = "" : Call Cargarcombo("SELECT CATEGORIA.* FROM CATEGORIA WHERE CATEGORIA.id_categoria<>'MENS' ORDER BY CATEGORIA.descripcion", "id_categoria", "descripcion", cboCategoria)
            Else
                STRID = "" : Call Cargarcombo("SELECT CATEGORIA.* FROM CATEGORIA INNER JOIN USUARIOCATEGORIAS ON CATEGORIA.id_categoria = USUARIOCATEGORIAS.id_categoria WHERE (USUARIOCATEGORIAS.id_usuario = '" & businessUser & "') AND CATEGORIA.id_categoria<>'MENS' ORDER BY CATEGORIA.descripcion", "id_categoria", "descripcion", cboCategoria)
            End If

            STRID = "" : Call Cargarcombo("select * from tipodocumentoPERSONAL order by descripcion", "id_tipodocumentopersonal", "descripcion", CboTipoDoc)
            cboCategoria.SelectedIndex = 0


            If Openform = "FRMCOMPLETARACCESOSPERSONAL" Then

            End If

            ' If strWeb = "S" Then



            'End If
            'Comentado en acuerdo con Juan
            'If strCostosPersonal = "N" Then
            '    FraCostosporhora.Enabled = False
            '    cboCategoria.Enabled = False
            'End If

            Select Case UCase(strAction)
                Case "NEW"
                    clearTextBox()
                    Session.Add("frmPersonalB_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str4007
                    End If

                    Session.Add("frmPersonalB_Action", "EDIT")
            End Select

            Session.Add("FrmPersonalB_STRID", STRID)
            hdnstr4009.Value = Resource1.str4009
            hdnStr528.Value = Resource1.str528

            Dim productsEnabled As List(Of String) = GetProductsEnabled("Fundo0")
            If Not productsEnabled.Exists(Function(x) x = "ERP") Then
                lblHoraED.Visible = False
                lblHoraES.Visible = False
                lblCosto_conta.Visible = False
                txtHoraED.Visible = False
                txtHoraES.Visible = False
                txtCosto_Conta.Visible = False
                hdnIncludeERP.Value = False
            Else
                hdnIncludeERP.Value = True
            End If
        End If
    End Sub

    Protected Sub cboCategoria_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        Dim STRID As String = ""
        Dim strAction As String = Request.QueryString("Action")
        Dim Openform As String = ""

        If Validar() Then
            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed() Then
                        Select Case UCase(Openform)
                            Case "FRMCOSTOSPERSONALAGROINCA", "FRMCOSTOSPERSONAL", "FRMCOMPLETARACCESOSPERSONAL", "FRMTESORERIACAJA", "FRMTESORERIACAJA3"
                                STRID = txtId.Text
                            Case Else
                                'frmPersonalb_View.Refrescar()
                        End Select

                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5004
                    End If

                Case "EDIT"
                    If Edit() Then
                        Select Case UCase(Openform)
                            Case "FRMCOSTOSPERSONALAGROINCA"
                                STRID = txtId.Text
                            Case Else

                        End Select


                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                    Else
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str4005
                    End If
            End Select

            Session.Add("FrmPersonalB_STRID", STRID)

        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub clearTextBox()
        fecha.Value = Today
        txtId.Text = ""
        nombre.Text = ""
    End Sub

    Private Sub setCaptionsLabels()
        Title = Resource1.str4001
        lblCodigo.Text = Resource1.str102
        lblNombre.Text = Resource1.str4003
        lblCostoUnitarioStandar.Text = Resource1.str10104
        lblCosto_conta.Text = Resource1.str10138
        lblHoraES.Text = Resource1.str10111
        lblHoraED.Text = Resource1.str10112
        lblCategoria.Text = Resource1.str6020
        lblEstado.Text = Resource1.str3053
        Label1.Text = Resource1.str10148
        Label2.Visible = False

    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        btnCancelar.Text = Resource1.str5
        'btnAdicional.Text = Resource1.str157
    End Sub

    Function getRecord() As Boolean
        Dim RS2 As ADODB.Recordset
        Dim rs As ADODB.Recordset
        Dim bResult As Boolean = False
        Dim StrPersonal As String = Session("frmPersonalB_StrPersonal")
        Dim codigo As String = Session("frmPersonalB_Codigo")

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        If Not String.IsNullOrEmpty(StrPersonal) Then
            Dim Sql As String = " SELECT personal.id_personal,PERSONAL.NOMBRE From PERSONAL WHERE (((PERSONAL.NOMBRE)='" & StrPersonal & "')); "
            DBconn.Execute(Sql)
            RS2 = DBconn.Execute(Sql)

            If RS2.EOF Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str11029

                Return False
                Exit Function
            Else
                Session.Add("frmPersonalB_Codigo", RS2.Fields("id_personal").Value)
                codigo = RS2.Fields("id_personal").Value
            End If

        End If

        Dim ssql As String = " SELECT     ca.descripcion AS cadesc, pe.id_personal, pe.nombre, pe.costo, pe.costo_conta, pe.horaes, pe.horaed, pe.orden, pe.estado, ci.Descripcion AS cidesc, TIPODOCUMENTOPERSONAL.descripcion AS tipodoc FROM         CATEGORIA AS ca INNER JOIN CICLODEPAGO AS ci INNER JOIN           PERSONAL AS pe ON ci.Id_ciclopago = pe.id_CicloPago ON ca.id_categoria = pe.id_categoria INNER JOIN TIPODOCUMENTOPERSONAL ON pe.id_tipodocumento = TIPODOCUMENTOPERSONAL.id_tipodocumentopersonal WHERE     (pe.id_personal = '" & codigo & "') "
        rs = DBconn.Execute(ssql)
        If rs.State = 1 Then
            rs.Close()
        End If

        rs.let_ActiveConnection(DBconn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        If Not rs.EOF Then
            txtId.Text = rs.Fields("ID_PERSONAL").Value
            nombre.Text = rs.Fields("nombre").Value
            costo_unitario_standar.Text = IIf(rs.Fields("costo").Value > 0, Convert.ToDecimal(rs.Fields("costo").Value).ToString("0.0000", CultureInfo.InvariantCulture), "")
            txtCosto_Conta.Text = IIf(rs.Fields("costo_conta").Value > 0, Convert.ToDecimal(rs.Fields("costo_conta").Value).ToString("0.0000", CultureInfo.InvariantCulture), "")
            txtHoraES.Text = IIf(rs.Fields("horaes").Value > 0, Convert.ToDecimal(rs.Fields("horaes").Value).ToString("0.0000", CultureInfo.InvariantCulture), "")
            txtHoraED.Text = IIf(rs.Fields("horaed").Value > 0, Convert.ToDecimal(rs.Fields("horaed").Value).ToString("0.0000", CultureInfo.InvariantCulture), "")
            cboCiclo.Text = Buscar(rs.Fields("cidesc").Value, cboCiclo) 'rsCiclo(1).Value
            cboEstado.Text = IIf(rs.Fields("Estado").Value, "A - Activo", "I - Inactivo")
            cboCategoria.Text = Buscar(rs.Fields("CAdesc").Value, cboCategoria)
            CboTipoDoc.Text = Buscar(rs.Fields("TipoDoc").Value, CboTipoDoc)

            txtId.Enabled = False
            bResult = True
        Else
            bResult = False
        End If
        rs.Close()

        Return bResult
    End Function

    Function Add_Renamed() As Boolean
        Dim boolResult As Boolean = False
        Dim MatrizTd() As Object = Session("frmPersonalB_MatrizTd")
        Dim MatrizZ() As Object = Session("frmPersonalB_MatrizZ")
        Dim MatrizA() As Object = Session("frmPersonalB_MatrizA")

        Dim horaEDAmount As String = txtHoraED.Text
        Dim horaESAmount As String = txtHoraES.Text
        Dim costoContaAmount As String = txtCosto_Conta.Text

        If hdnIncludeERP.Value = "False" Then
            horaEDAmount = costo_unitario_standar.Text
            horaESAmount = costo_unitario_standar.Text
            costoContaAmount = costo_unitario_standar.Text
        End If

        Dim ssql As String = "insert into PERSONAL (id_tipodocumento,id_personal,nombre,costo, costo_conta, horaes, horaed, id_categoria, id_ciclopago, estado) "
        ssql = ssql & "values('" & MatrizTd(CboTipoDoc.SelectedIndex) & "','" & txtId.Text & "',ltrim(rtrim('" & nombre.Text & "'))," & costo_unitario_standar.Text & ", " & costoContaAmount & ", " & horaESAmount & ", " & horaEDAmount & ", '" & MatrizZ(cboCategoria.SelectedIndex) & "', '" & MatrizA(cboCiclo.SelectedIndex) & "'," & IIf(cboEstado.Text.Substring(0, 1) = "A", 1, 0) & " )"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "',getdate())"
            DBconn.Execute(ssql)

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
        Dim MatrizTd() As Object = Session("frmPersonalB_MatrizTd")
        Dim MatrizZ() As Object = Session("frmPersonalB_MatrizZ")
        Dim MatrizA() As Object = Session("frmPersonalB_MatrizA")
        Dim bResult As Boolean = True
        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())

            If Mid(MatrizZ(cboCategoria.SelectedIndex), 1, 2) = "PR" Then
                ssql = " update costos set monto=0 where id_personal='" & txtId.Text & "' "
                DBconn.Execute(ssql)
            Else
                ssql = " update costos set monto=monto_standar where id_personal='" & txtId.Text & "' "
                DBconn.Execute(ssql)
            End If

            ssql = "update PERSONAL set "
            ssql = ssql & "id_tipodocumento='" & MatrizTd(CboTipoDoc.SelectedIndex) & "', "
            ssql = ssql & "nombre=ltrim(rtrim('" & nombre.Text & "')), "
            ssql = ssql & "costo=" & costo_unitario_standar.Text & ", "
            ssql = ssql & "costo_conta=" & txtCosto_Conta.Text & ", "
            ssql = ssql & "horaes=" & txtHoraES.Text & ", "
            ssql = ssql & "horaed=" & txtHoraED.Text & ", "
            ssql = ssql & "id_categoria='" & MatrizZ(cboCategoria.SelectedIndex) & "', "
            ssql = ssql & "id_ciclopago='" & MatrizA(cboCiclo.SelectedIndex) & "' "
            ssql = ssql & "where id_personal='" & txtId.Text.Trim() & "';"
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "',getdate())"
            DBconn.Execute(ssql)

            AuditoriaBackup(Title, businessUser, DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"), "Modifico lo nuevo es ", txtId.Text, nombre.Text, txtCosto_Conta.Text, cboCategoria.Text, cboCiclo.Text, txtHoraES.Text, txtHoraED.Text, txtCosto_Conta.Text)
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
            bResult = False
        End Try

        Return bResult

    End Function

    Function Validar() As Boolean
        Dim ssql As String = ""
        Dim RS As ADODB.Recordset
        Dim bResult As Boolean = False
        Dim strAction As String = Request.QueryString("Action")

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        ssql = String.Format("SELECT Nombre From dbo.PERSONAL (NOLOCK) WHERE nombre = '{0}' AND id_personal <> '{1}'", nombre.Text.Trim(), txtId.Text.Trim())
        RS = New ADODB.Recordset

        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        RS.LockType = ADODB.LockTypeEnum.adLockReadOnly
        RS.let_Source(ssql)
        RS.Open()

        If Not RS.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str10018
            bResult = False
            RS.Close()
            Return bResult
        End If

        RS.Close()
        RS = Nothing
        'End If

        txtId.Text = UCase(txtId.Text)
        nombre.Text = UCase(nombre.Text)
        bResult = True

        Dim objGenericMethods As New GenericMethods("Fundo0")
        Dim Validademo As Boolean = objGenericMethods.ValidaPersonal()

        If Not Validademo Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str999995
            Return False
        End If


        Return bResult
    End Function

    Private Sub Cargarcombo(ByRef ssql As String, ByRef id As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim adoTabla As ADODB.Recordset
        Dim i, intOrden As Short
        If Cbo.Items.Count > 0 Then intOrden = Cbo.SelectedIndex
        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest
        adoTabla = New ADODB.Recordset

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        adoTabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        Dim Matrizz() As Object = Session("frmPersonalB_Matrizz")
        Dim MatrizZC() As Object = Session("frmPersonalB_MatrizZC")
        Dim MatrizA() As Object = Session("frmPersonalB_MatrizA")
        Dim MatrizAC() As Object = Session("frmPersonalB_MatrizAC")
        Dim MatrizTd() As Object = Session("frmPersonalB_MatrizTd")
        Dim MatriztdD() As Object = Session("frmPersonalB_MatrizTdD")

        Do While Not adoTabla.EOF
            Select Case id
                Case "id_categoria" 'Periodicidad
                    ReDim Preserve Matrizz(i)
                    ReDim Preserve MatrizZC(i)

                    Matrizz(i) = adoTabla.Fields(id).Value
                    MatrizZC(i) = RTrim(adoTabla.Fields(Desc).Value)
                Case "id_ciclopago" 'Tipo
                    ReDim Preserve MatrizA(i)
                    ReDim Preserve MatrizAC(i)

                    MatrizA(i) = adoTabla.Fields(id).Value
                    MatrizAC(i) = Trim(adoTabla.Fields(Desc).Value)

                Case "id_tipodocumentopersonal" 'Tipo
                    ReDim Preserve MatrizTd(i)
                    ReDim Preserve MatriztdD(i)

                    MatrizTd(i) = adoTabla.Fields(id).Value
                    MatriztdD(i) = Trim(adoTabla.Fields(Desc).Value)
            End Select

            Session.Add("frmPersonalB_MatrizTd", MatrizTd)
            Session.Add("frmPersonalB_MatrizTdD", MatriztdD)
            Session.Add("frmPersonalB_Matrizz", Matrizz)
            Session.Add("frmPersonalB_MatrizZC", MatrizZC)
            Session.Add("frmPersonalB_MatrizA", MatrizA)
            Session.Add("frmPersonalB_MatrizAC", MatrizAC)

            Cbo.Items.Add(adoTabla.Fields(Desc).Value)
            If adoTabla.Fields(id).Value = Session("FrmPersonalB_STRID").ToString() Then
                intOrden = i
            End If
            i = i + 1
            adoTabla.MoveNext()
        Loop

        Cbo.SelectedIndex = intOrden
    End Sub
End Class