Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmZonaTrabajo
    Inherits BasePage

    Dim Openform As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            Dim STRID As String = ""
            STRID = "" : Call Cargarcombo("select * from cultivos where id_cultivo<>'COSTIN' order by descripcion", "id_cultivo", "descripcion", cboCultivo)
            Session.Add("FrmZonaTrabajo_STRID", "")

            cboTipo.Items.Add(Resource1.str12100)
            cboTipo.Items.Add(Resource1.str12101)
            cboTipo.SelectedIndex = 0
            hdnTipocosto.Value = "COSTDIR"

            hdnStr308.Value = Resource1.str308
            hdnStr309.Value = Resource1.str309
            hdnStr312.Value = Resource1.str312
            lblCultivo.Visible = True
            cboCultivo.Visible = True
            lblHectareas.Visible = True
            hectareas.Visible = True
            campana.Enabled = True
            rbtnActivo.Checked = True

            Dim strAction As String = Request.QueryString("Action")
            Select Case UCase(strAction)
                Case "NEW"
                    Call clearTextBox()
                    Session.Add("frmZonaTrabajo_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str307
                    End If

                    Session.Add("frmZonaTrabajo_Action", "EDIT")
            End Select
        End If
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        If Validar() Then
            'Dim objBL As New ZonaTrabajo("Fundo0")
            Dim strAction As String = Request.QueryString("Action")
            Dim strModulo As String = Resource1.str5004
            Dim Openform As String = String.Empty

            dvMessage.Visible = True
            Select Case UCase(strAction)
                Case "NEW"
                    If Add_Renamed() Then
                        Select Case UCase(Openform)
                            Case "FRMAPLICACIONINSUMOS", "FRMCOSTOSMAQUINARIA", "FRMCOSTOSRIEGO", "FRMCOSTOSPERSONAL", "FRMCOSECHAS", "FRMCOSTOSDIVERSOS2", "FRMCOSTOSDIVERSOS", "FRMCOMPLETARACCESOSPERSONAL", "FRMPRESUPUESTOS", "FRMTESORERIACAJA", "FRMTESORERIACAJA2", "FRMTESORERIACAJA3", "FRMCOSTOSDIVERSOS3", "FRMCOSTOSPERSONALAGROINCA"
                                'STRID = id.Text
                            Case Else
                                'El formulario padre se refrescará en el lado del cliente.
                                'frmZonaTrabajo_view.Refrescar()
                        End Select
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                        btnGrabar.Enabled = False 'str543

                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str304
                    End If
                Case "EDIT"
                    If Edit() Then
                        'El formulario padre se refrescará en el lado del cliente.
                        'frmZonaTrabajo_view.Refrescar()
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str305
                    End If
            End Select
        Else
            'dvMessage.Attributes.Add("class", "alert alert-success")
            'lblResult.Text = "Error en Validacion."
        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)
        'El formulario padre se refrescará en el lado del cliente.
        'If Openform = "" Then frmCultivos_view.Refrescar()

        'El proceso 
        'Openform = "" : Me.Close()
    End Sub

    Private Sub setCaptionsLabels()
        Title = Resource1.str301
        lblCodigo.Text = Resource1.str302
        lblDescripcion.Text = Resource1.str303
        lblHectareas.Text = Resource1.str311
        lblCampana.Text = Resource1.str541
        lblTipo.Text = Resource1.str111
        lblCultivo.Text = Resource1.str315
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        btnCancelar.Text = Resource1.str5
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
        hectareas.Text = ""
        cboCultivo.SelectedIndex = 0
        campana.Text = ""
    End Sub

    Private Function getRecord() As Boolean
        Dim Codigo As String = ""
        Dim blGetRecord As Boolean = False

        If Session("frmZonaTrabajo_Codigo") IsNot Nothing Then
            Codigo = Session("frmZonaTrabajo_Codigo")
        Else
            blGetRecord = False
        End If

        Dim MatrizC As Object = Session("FrmZonaTrabajo_MatrizC")

        Dim rs As New ADODB.Recordset
        Dim objBL As New ZonaTrabajo("Fundo0")
        rs = objBL.getRecord(Codigo)

        If Not rs.EOF Then
            Dim fec As String

            If IsDBNull(rs.Fields("fecS").Value) Then
            Else
                'fec = Year(rs.Fields("fecS").Value) & "/" & VB.Right("0" & Month(rs.Fields("fecS").Value), 2) & "/" & VB.Right("0" & VB.Day(rs.Fields("fecS").Value), 2)
                fec = Convert.ToDateTime(rs.Fields("fecS").Value).ToString("yyyy/MM/dd")
                fecsiembra.Text = rs.Fields("fecS").Value
            End If

            txtId.Text = rs.Fields("id_zonatrabajo").Value
            descripcion.Text = rs.Fields("descripcion").Value

            Dim cultureName As String = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Dim ci As New CultureInfo(cultureName)
            If ci.NumberFormat.NumberDecimalSeparator <> "." Then
                ci.NumberFormat.NumberDecimalSeparator = "."
                Threading.Thread.CurrentThread.CurrentCulture = ci
            End If

            hectareas.Text = rs.Fields("hectareas").Value 'Replace(FormatNumber(rs!hectareas, 2), ",", "")
            Dim campanaValor As Decimal = Convert.ToDecimal(rs.Fields("campana").Value)
            campana.Text = campanaValor.ToString("###0.00")

            If rs.Fields("id_cultivo").Value = "COSTIN" Then
                cboTipo.SelectedIndex = 1
                cboTipo_SelectedIndexChanged(Nothing, Nothing)
            Else
                cboCultivo.Text = Buscar((rs.Fields("des_cultivo").Value), cboCultivo)
            End If

            If MatrizC(cboCultivo.SelectedIndex) = "COSTIN" Then cboTipo.SelectedIndex = 1
            'CboCCC.Text = Buscar((rs.Fields("CCC_des").Value), CboCCC)
            'Cbofundo.Text = Buscar((rs.Fields("FUNDO_des").Value), Cbofundo)
            If rs.Fields("Estado").Value = 0 Then
                rbtnInactivo.Checked = True
            Else
                rbtnActivo.Checked = True
            End If

            txtId.Enabled = False
            blGetRecord = True
        Else
            blGetRecord = False
        End If

        rs.Close()
        rs = Nothing

        Return blGetRecord
    End Function

    Function Validar() As Boolean
        txtId.Text = Trim(Replace(txtId.Text, " ", ""))
        Dim MatrizC As Object = Session("FrmZonaTrabajo_MatrizC")
        Dim objGenericMethods As New GenericMethods("Fundo0")
        Dim Validademo As Boolean = objGenericMethods.ValidaPersonal()

        If Not Validademo Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resources.Resource1.str999995 '"Excedió la cantidad de empleados permitidos para esta versión"
            Return False
        End If

        Dim rs As New ADODB.Recordset
        Dim strAction As String = Request.QueryString("Action")
        Dim objBL As New ZonaTrabajo("Fundo0")

        If strAction = "NEW" Then
            rs = objBL.getZonaTrabajoByCodigo(txtId.Text.Trim())

            If Not rs.EOF Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resources.Resource1.str999996 '"Existe una zona de trabajo con el mismo codigo"
                rs.Close()
                Return False
            End If

            rs.Close()
        End If

        rs = objBL.getZonaTrabajoByDescripcion(txtId.Text.Trim(), descripcion.Text.Trim())

        If Not rs.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str10532
            rs.Close()
            Return False
        End If

        If Me.cboTipo.SelectedIndex = 1 Then hectareas.Text = CStr(1)

        If Not IsNumeric(campana.Text) Then
            Me.campana.Text = "0"
        End If

        If Not IsNumeric(hectareas.Text) Then
            Me.hectareas.Text = "0"
        End If

        If MatrizC(cboCultivo.SelectedIndex) = "COSTIN" Then
            hectareas.Text = 1

        End If

        txtId.Text = txtId.Text.ToUpper()
        descripcion.Text = descripcion.Text.ToUpper()
        hectareas.Text = hectareas.Text.ToUpper()

        Return Validademo
    End Function

    Private Sub Cargarcombo(ByRef ssql As String, ByRef id As String, ByRef Desc As String, ByRef Cbo As DropDownList)
        Dim i, intOrden As Short
        If Cbo.Items.Count > 0 Then intOrden = Cbo.SelectedIndex
        Cbo.Items.Clear() ': Cbo.AutoCompleteSource = AutoCompleteSource.ListItems : Cbo.AutoCompleteMode = AutoCompleteMode.Suggest

        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection()
        DBconn.Open(objBL.GetSQLConnection())

        Dim adoTabla = New ADODB.Recordset
        adoTabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        Dim MatrizC() As Object = Session("FrmZonaTrabajo_MatrizC")
        Dim STRID As String = Session("FrmZonaTrabajo_STRID")

        Do While Not adoTabla.EOF
            Select Case id
                Case "id_cultivo" : ReDim Preserve MatrizC(i)
                    MatrizC(i) = adoTabla.Fields(id).Value
                    Session.Add("FrmZonaTrabajo_MatrizC", MatrizC)
                Case "id_fundo"  'ReDim Preserve Matrizf(i)
                    'Matrizf(i) = adoTabla.Fields(id).Value
                Case "id_ccc"  'ReDim Preserve MatrizCTA9(i)
                    'ReDim Preserve MatrizCCC(i)
                    'MatrizCCC(i) = adoTabla.Fields(id).Value
                    'MatrizCTA9(i) = adoTabla.Fields("id_cuentacontable3").Value
                Case "id_sector"  'ReDim Preserve MatrizSec(i)
                    ' MatrizSec(i) = adoTabla.Fields(id).Value
            End Select

            Cbo.Items.Add(adoTabla.Fields(Desc).Value)
            If adoTabla.Fields(id).Value = STRID Then
                intOrden = i
            End If
            i = i + 1
            adoTabla.MoveNext()
        Loop

        Cbo.SelectedIndex = intOrden
    End Sub

    Protected Sub cboTipo_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.cboTipo.SelectedIndex = 0 Then
            Me.lblCultivo.Visible = True
            Me.cboCultivo.Visible = True
            Me.lblHectareas.Visible = True
            Me.hectareas.Visible = True
            If cboCultivo.SelectedIndex <> -1 Then cboCultivo.SelectedIndex = 0
            lblfecsiembra.Visible = True
            fecsiembra.Visible = True
            hdnTipocosto.Value = "COSTDIR"
        Else
            Me.lblCultivo.Visible = False
            Me.cboCultivo.Visible = False
            Me.lblHectareas.Visible = False
            Me.hectareas.Visible = False
            lblfecsiembra.Visible = False
            fecsiembra.Visible = False
            hdnTipocosto.Value = "COSTIND"
        End If
    End Sub

    Private Function Add_Renamed() As Boolean
        Dim boolResult As Boolean = False
        Dim MatrizC() As Object = Session("FrmZonaTrabajo_MatrizC")
        Dim cantHectareas As String = hectareas.Text

        Dim fechaValida As Boolean = True
        Dim fechas As DateTime = DateTime.Now


        If Not String.IsNullOrEmpty(fecsiembra.Text) Then
            fechaValida = DateTime.TryParseExact(fecsiembra.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechas)
        End If


        If Not fechaValida Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str99991
            'str99991

            Exit Function
        End If

        If Me.cboTipo.SelectedIndex = 1 Then
            Me.cboCultivo.Items.Clear() : Me.cboCultivo.Items.Add("") : Me.cboCultivo.SelectedIndex = 0
            ReDim MatrizC(1)

            MatrizC(0) = "COSTIN"
            Session.Add("FrmZonaTrabajo_MatrizC", MatrizC)
            cantHectareas = "1"
        End If

        Dim fec As String = Convert.ToDateTime(fecsiembra.Text).ToString("yyyy/MM/dd")
        Dim ssql As String

        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection()

        Try
            DBconn.Open(objBL.GetSQLConnection())
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

    Private Function Edit() As Boolean
        Dim MatrizC() As Object = Session("FrmZonaTrabajo_MatrizC")
        Dim cantHectareas As String = hectareas.Text

        If Me.cboTipo.SelectedIndex = 1 Then
            Me.cboCultivo.Items.Clear() : Me.cboCultivo.Items.Add("") : Me.cboCultivo.SelectedIndex = 0
            ReDim MatrizC(1)

            MatrizC(0) = "COSTIN"
            cantHectareas = "1"
        End If

        Dim objBL As New ZonaTrabajo("Fundo0")
        Dim sEstado As String = "1"
        If rbtnActivo.Checked Then
            sEstado = "1"
        Else
            sEstado = "0"
        End If

        Dim bResult As Boolean = objBL.Edit(txtId.Text, MatrizC(cboCultivo.SelectedIndex), fecsiembra.Text, descripcion.Text, cantHectareas, campana.Text, sEstado)

        'Get BusinessUser
        Dim objGenericBL As New GenericMethods("Fundo0")
        Dim businessUser = "DEMO01"

        'TODO: Falta llamar a Auditoria
        AuditoriaBackup(Title, businessUser, DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"), "Regularizacion ", txtId.Text.Trim, cboCultivo.Text, "000000", descripcion.Text, hectareas.Text, "91", campana.Text)
        objBL.InsertAuditoriaGeneral(Title)

        Return bResult
    End Function
End Class