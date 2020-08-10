Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmActividades
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            cboTipo.Items.Add(Resource1.str12102)
            cboTipo.Items.Add(Resource1.str12103)
            cboTipo.SelectedIndex = 0
            lblEtapa.Visible = True
            cboEtapa.Visible = True
            hdnStr208.Value = Resource1.str208
            hdnStr209.Value = Resource1.str209

            Dim STRID As String = "" : Call Cargarcombo("select * from etapas where id_etapa<>'NA0' order by descripcion", "id_etapa", "descripcion", cboEtapa)

            Dim rsMax As New ADODB.Recordset
            Dim strAction As String = Request.QueryString("Action")
            Select Case UCase(strAction)
                Case "NEW"
                    clearTextBox()
                    Dim ssql As String = "SELECT MAX(ubicacion_cc) as maxorder FROM ACTIVIDADES"

                    Dim DBconn As New ADODB.Connection()
                    Dim objBL As New GenericMethods("Fundo0")
                    DBconn.Open(objBL.GetSQLConnection())
                    rsMax.let_ActiveConnection(DBconn)
                    rsMax.CursorLocation = ADODB.CursorLocationEnum.adUseClient
                    rsMax.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
                    rsMax.LockType = ADODB.LockTypeEnum.adLockReadOnly
                    rsMax.let_Source(ssql)
                    rsMax.Open()

                    If Not rsMax.EOF And Not IsDBNull(rsMax.Fields("maxorder").Value) Then
                        'ubicacion_cc.Text = rsMax!maxorder + 1
                    Else
                        'ubicacion_cc.Text = "0"
                    End If

                    rsMax.Close()
                    rsMax = Nothing
                    Session.Add("frmActividad_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        MsgBox(Resource1.str207, MsgBoxStyle.Exclamation, Application)
                    End If

                    Session.Add("frmActividad_Action", "EDIT")
            End Select
        End If
    End Sub

    Protected Sub cboTipo_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.cboTipo.SelectedIndex = 0 Then
            Me.lblEtapa.Visible = True
            Me.cboEtapa.Visible = True
            'If cboEtapa.SelectedIndex <> -1 Then cboEtapa.SelectedIndex = 0
        Else
            Me.lblEtapa.Visible = False
            Me.cboEtapa.Visible = False
            Me.cboEtapa.SelectedIndex = 0
            'If cboLinea.SelectedIndex <> -1 Then cboLinea.SelectedIndex = 0
        End If
    End Sub

    Private Sub Cargarcombo(ByRef ssql As String, ByRef id As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim i, txtCombo As Short
        If Cbo.Items.Count > 0 Then txtCombo = Cbo.SelectedIndex
        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest
        Dim adoTabla = New ADODB.Recordset
        Dim DBconn As New ADODB.Connection()
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        adoTabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        Dim MatrizE() As Object = Session("frmActividad_MatrizE")

        Do While Not adoTabla.EOF
            Select Case id
                Case "ID_cuentacontable"
                    'ReDim Preserve MatrizLinea(i)
                    'MatrizLinea(i) = adoTabla.Fields(id).Value
                Case "id_etapa" : ReDim Preserve MatrizE(i)
                    MatrizE(i) = adoTabla.Fields(id).Value
            End Select
            i = i + 1
            Cbo.Items.Add(adoTabla.Fields(Desc).Value)
            adoTabla.MoveNext()
        Loop

        Cbo.SelectedIndex = txtCombo
        Session.Add("frmActividad_MatrizE", MatrizE)
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
        cboEtapa.SelectedIndex = 0
    End Sub

    Sub setCaptionsLabels()
        Title = Resource1.str201
        lblCodigo.Text = Resource1.str202
        lblDescripcion.Text = Resource1.str203
        lblTipo.Text = Resource1.str111
        lblEtapa.Text = Resource1.str211
        'Lblctacont.Text = Resource1.str10301
    End Sub

    Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        btnCancelar.Text = Resource1.str5
    End Sub

    Function getRecord() As Boolean
        Dim rs As New ADODB.Recordset
        Dim RS2 As New ADODB.Recordset
        Dim Codigo As String = ""
        Dim blGetRecord As Boolean = False

        If Session("frmActividad_Codigo") IsNot Nothing Then
            Codigo = Session("frmActividad_Codigo")
        Else
            blGetRecord = False
        End If

        Dim MatrizE As Object = Session("FrmActividad_MatrizE")

        Dim ssql As String = "select a.id_actividad,a.descripcion,a.ubicacion_cc,e.id_etapa,e.descripcion as des_etapa "
        ssql = ssql & "from ACTIVIDADES as a,ETAPAS as e where a.id_etapa = e.id_etapa and "
        ssql = ssql & "a.id_actividad='" & Codigo & "';"

        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection()
        DBconn.Open(objBL.GetSQLConnection())

        rs.let_ActiveConnection(DBconn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        If Not rs.EOF Then
            txtId.Text = rs.Fields("id_actividad").Value
            descripcion.Text = rs.Fields("descripcion").Value

            ssql = " SELECT CUENTAACTIVIDAD.id_CuentaContable, CUENTAACTIVIDAD.id_Actividad, PLANCONTABLE.Descripcion as plandes" & " FROM CUENTAACTIVIDAD INNER JOIN PLANCONTABLE ON CUENTAACTIVIDAD.id_CuentaContable = PLANCONTABLE.Id_cuentacontable " & " WHERE (((CUENTAACTIVIDAD.id_Actividad)='" & Codigo & "'));"

            If RS2.State = 1 Then
                RS2.Close()
            End If

            RS2.let_ActiveConnection(DBconn)
            RS2.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            RS2.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
            RS2.LockType = ADODB.LockTypeEnum.adLockReadOnly
            RS2.let_Source(ssql)
            RS2.Open()

            If Not RS2.EOF Then
                'cboLinea.Text = Buscar((RS2.Fields("plandes").Value), cboLinea)
            End If

            If rs.Fields("id_etapa").Value = "NA0" Then
                cboTipo.SelectedIndex = 1
            Else
                cboEtapa.Text = Buscar((rs.Fields("des_etapa").Value), cboEtapa)
            End If

            If MatrizE(cboEtapa.SelectedIndex) = "NA0" Then cboTipo.SelectedIndex = 1
            cboTipo_SelectedIndexChanged(Nothing, Nothing)
            txtId.Enabled = False
            blGetRecord = True
        Else
            blGetRecord = False
        End If

        rs.Close()
        rs = Nothing

        Return blGetRecord
    End Function

    Function Add_Renamed() As Boolean
        Dim boolResult As Boolean = False
        Dim MatrizE() As Object = Session("FrmActividad_MatrizE")

        If Me.cboTipo.SelectedIndex = 1 Then
            Me.cboEtapa.Items.Clear() : Me.cboEtapa.Items.Add("") : Me.cboEtapa.SelectedIndex = 0
            ReDim MatrizE(1)
            MatrizE(0) = "NA0"
        End If

        Dim ssql As String
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection()

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            ssql = "insert into CUENTAACTIVIDAD (ID_CUENTACONTABLE,ID_ACTIVIDAD) "
            'UPGRADE_WARNING: No se puede resolver la propiedad predeterminada del objeto MatrizLinea(). Haga clic aquí para obtener más información: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ssql = ssql & "values('630000','" & txtId.Text & "');"
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"

            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
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
        Dim bResult As Boolean = False
        Dim MatrizE() As Object = Session("FrmActividad_MatrizE")
        Dim ssql As String = ""

        Try
            If Me.cboTipo.SelectedIndex = 1 Then
                Me.cboEtapa.Items.Clear() : Me.cboEtapa.Items.Add("") : Me.cboEtapa.SelectedIndex = 0
                ReDim MatrizE(1)
                MatrizE(0) = "NA0"
            End If

            ssql = "update ACTIVIDADES set "
            ssql = ssql & "descripcion=ltrim(rtrim('" & descripcion.Text & "')), "
            'sSQL = sSQL & "ubicacion_cc=" & ubicacion_cc.Text & ", "
            ssql = ssql & "id_etapa='" & MatrizE(cboEtapa.SelectedIndex) & "' "
            ssql = ssql & "where id_actividad='" & txtId.Text & "';"

            Dim objBL As New GenericMethods("Fundo0")
            Dim DBconn As New ADODB.Connection()
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            ssql = "update cuentAACTIVIDAD set "
            ssql = ssql & "id_cuentacontable='630000' "
            ssql = ssql & "where ID_ACTIVIDAD='" & txtId.Text & "';"
            DBconn.Execute(ssql)

            'Get BusinessUser
            Dim businessUser = "DEMO01"
            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & businessUser & "','" & Title & "', getdate())"
            DBconn.Execute(ssql)

            bResult = True
        Catch ex As Exception
            bResult = False
        End Try

        Return bResult
    End Function

    Function Validar() As Boolean
        Dim bValidar As Boolean = True
        Dim RS As New ADODB.Recordset
        Dim ssql As String = ""

        txtId.Text = Trim(Replace(txtId.Text, " ", ""))
        If RS.State = 1 Then
            RS.Close()
        End If

        Dim strAction As String = Request.QueryString("Action")
        If strAction = "NEW" Then
            If Mid(txtId.Text, 1, 4) = "SUBS" Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str99999985 ' Resource1.str99999985
                Return False
            ElseIf Mid(txtId.Text, 1, 4) = "DESM" Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str99999985
                Return False
            ElseIf Mid(txtId.Text, 1, 4) = "DIAF" Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str99999985
                Return False
            ElseIf Mid(txtId.Text, 1, 4) = "VACA" Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str99999985
                Return False
            Else
                bValidar = True
            End If
        End If

        ssql = String.Format("SELECT DESCRIPCION From dbo.actividades (NOLOCK) WHERE DESCRIPCION = '{0}' AND id_actividad <> '{1}'", descripcion.Text.Trim(), txtId.Text.Trim())

        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection()
        DBconn.Open(objBL.GetSQLConnection())
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        RS.LockType = ADODB.LockTypeEnum.adLockReadOnly
        RS.let_Source(ssql)
        RS.Open()

        If Not RS.EOF Then
            '        MsgBox "Existe una actividad con la misma descripcion"
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str10531
            Return False
        End If

        RS.Close()
        RS = Nothing

        Return bValidar
    End Function

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        If Validar() Then
            Dim Openform As String = ""
            Dim strAction As String = Request.QueryString("Action")
            dvMessage.Visible = True
            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed() Then
                        Select Case UCase(Openform)
                            Case "FRMAPLICACIONINSUMOS", "FRMCOSTOSMAQUINARIA", "FRMCOSTOSRIEGO", "FRMCOSTOSPERSONAL", "FRMCOMPLETARACCESOSPERSONAL", "FRMPRESUPUESTOS", "FRMTESORERIACAJA", "FRMCOSTOSDIVERSOS", "FRMTESORERIACAJA3", "FRMCOSTOSDIVERSOS2"
                                'STRID = ID.Text
                            Case Else
                                'El formulario padre se refrescará en el lado del cliente.
                                'frmActividades_View.Refrescar()
                        End Select
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str204
                    End If
                Case "EDIT"
                    If Edit() Then
                        'frmActividades_View.Refrescar()
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str205
                    End If
            End Select
            'Openform = "" : Me.Close()
        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)
    End Sub
End Class