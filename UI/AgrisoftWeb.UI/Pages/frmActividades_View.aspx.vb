Imports System.Data.OleDb
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmActividades_View
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            'Verify permissions
            currentModule = "Agricultura.EstructuraCostos"

            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If

            LoadPageResources()
            setCboFind()
            Refresh()
        Else
            'Verificar si es que proviene de editar para que refresque la grilla 
            If Session("frmActividad_Action") = "NEW" Or Session("frmActividad_Action") = "EDIT" Then
                Refresh()
            End If
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "a.id_actividad"
                Case 2
                    field = "a.descripcion"
                Case 3
                    field = "e.descripcion"
                Case 4
                    field = "a.id_afectacion"
            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        Session.Add("frmActividad_Action", "NEW")
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs)
        If Session("frmActividadesView_IdActividad") Is Nothing Then
            Exit Sub
        End If

        Dim idActividad As String = Session("frmActividadesView_IdActividad")
        Dim nRecords As Long = 0
        If Session("frmActividadesView_nRecords") IsNot Nothing Then
            nRecords = Convert.ToInt64(Session("frmActividadesView_nRecords"))
        End If

        Session.Add("frmActividad_Codigo", idActividad)
        Session.Add("frmActividad_Action", "EDIT")
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmActividadesView_IdActividad") Is Nothing Then
            Exit Sub
        End If

        If hdnDelete.Value <> "Delete" Then
            Exit Sub
        End If

        Dim sParamSistema As String = ""
        If Not String.IsNullOrEmpty(Session("frmActividadesView_ParametroSistema")) Then
            sParamSistema = Session("frmActividadesView_ParametroSistema")
        End If

        If sParamSistema = "0" Then
            Dim nRecords As Long = 0
            If Session("frmActividadesView_nRecords") IsNot Nothing Then
                nRecords = Convert.ToInt64(Session("frmActividadesView_nRecords"))
            End If

            If nRecords > 0 Then
                If Not Delete() Then
                    dvMessage.Visible = True
                    lblResults.Text = Resource1.str206
                End If
            End If
        Else
            dvMessage.Visible = True
            lblResults.Text = Resource1.str100
        End If
    End Sub

    Protected Sub btnRefrescar_Click(sender As Object, e As EventArgs)
        find.Text = ""
        cargarDatosGrilla("", "")
    End Sub

    Protected Sub btnExportarExcel_Click(sender As Object, e As EventArgs)
        Dim rsReporte As New ADODB.Recordset
        Dim sField As String = ""
        Dim sText As String = ""

        If Not String.IsNullOrEmpty(find.Text) Then
            sText = find.Text

            If cboFields.SelectedIndex <> -1 Then
                Select Case cboFields.SelectedIndex + 1
                    Case 1
                        sField = "a.id_actividad"
                    Case 2
                        sField = "a.descripcion"
                    Case 3
                        sField = "e.descripcion"
                    Case 4
                        sField = "a.id_afectacion"
                End Select
            End If
        End If

        Dim ssql As String = " SELECT a.id_actividad, a.descripcion as actividad, a.ubicacion_cc, e.descripcion AS etapa, a.ID_Afectacion, a.Parametrodelsistema, a.Presupuesto, PLANCONTABLE.Descripcion AS cuenta FROM  PLANCONTABLE RIGHT OUTER JOIN CUENTAACTIVIDAD ON PLANCONTABLE.Id_cuentacontable = CUENTAACTIVIDAD.id_CuentaContable RIGHT OUTER JOIN ETAPAS AS e INNER JOIN ACTIVIDADES AS a ON e.id_etapa = a.id_etapa ON CUENTAACTIVIDAD.id_Actividad = a.id_actividad WHERE ((a.id_etapa)=[e].[id_etapa] ) "

        If sField <> "" Then
            ssql = ssql & " and " & sField & " like '%" & sText & "%' "
        Else
            ssql = ssql & " "
        End If

        ssql = ssql & " ORDER BY a.descripcion "
        Session.Add("frmExportParametros_ParamExport", "ACTIVIDADES")
        Session.Add("frmExportParametros_ReportQuery", ssql)

        Response.Redirect("frmExport_Parametros.aspx")
    End Sub

    Protected Sub grilla_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(0).Text = Resource1.str202
                e.Row.Cells(1).Text = Resource1.str203
                e.Row.Cells(2).Text = Resource1.str212
                e.Row.Cells(3).Text = Resource1.str211
                e.Row.Cells(4).Text = Resource1.str10147
                'e.Row.Cells(5).Text = Resource1.str86
                e.Row.Cells(6).Text = Resource1.str10301

                e.Row.Cells(0).Width = New Unit(50, UnitType.Pixel)
                e.Row.Cells(1).Width = New Unit(330, UnitType.Pixel)

                e.Row.Cells(3).Width = New Unit(200, UnitType.Pixel)


            Case DataControlRowType.DataRow

        End Select

        e.Row.Cells(2).Visible = False
        e.Row.Cells(6).Visible = False
        e.Row.Cells(7).Visible = False
        e.Row.Cells(4).Visible = False
        e.Row.Cells(5).Visible = False
    End Sub

    Protected Sub grilla_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.cursor='pointer';this.style.textDecoration='underline';"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';"
            e.Row.ToolTip = "Seleccionar"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(grilla, "Select$" & e.Row.RowIndex)
            'e.Row.Attributes("onclick") = Page.ClientScript.RegisterForEventValidation((grilla, "Select$" & e.Row.RowIndex)
        End If
    End Sub

    Protected Sub grilla_SelectedIndexChanged(sender As Object, e As EventArgs)
        For Each row As GridViewRow In grilla.Rows
            If row.RowIndex = grilla.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = ""

                Session.Add("frmActividadesView_IdActividad", row.Cells(0).Text)
                Session.Add("frmActividad_Codigo", row.Cells(0).Text)
                'Session.Add("frmActividadesView_ZonaTrabajoDescripcion", row.Cells(1).Text)
                Session.Add("frmActividadesView_ParametroSistema", row.Cells(5).Text)
            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Seleccionar"
            End If
        Next
    End Sub

    Private Sub setCaptionsLabels()

        Title = Resource1.str201
        lblFind.Text = Resource1.str1004
    End Sub

    Private Sub setCaptionsButtons()
        btnNuevo.Text = Resource1.str1
        btnModificar.Text = Resource1.str2
        btnEliminar.Text = Resource1.str3
        btnBuscar.Text = Resource1.str7
        btnRefrescar.Text = Resource1.str8
        btnExportarExcel.Text = Resource1.str12
    End Sub

    Private Sub setCaptionGrilla()
        'Las columnas ocultas se pusieron en el evento grilla_RowDataBound
    End Sub

    Private Sub setCboFind()
        cboFields.Items.Clear()
        cboFields.Items.Add(Resource1.str200) ' codigo de actividad
        cboFields.Items.Add(Resource1.str510) ' descripcion de actividad
        cboFields.Items.Add(Resource1.str6003) ' nombre de etapa


        cboFields.SelectedIndex = 0
    End Sub

    Private Sub cargarDatosGrilla(ByVal sField As String, ByVal sText As String)
        Dim sRecords As String
        Dim ssql As String = " SELECT a.id_actividad, a.descripcion as actividad, a.ubicacion_cc, e.descripcion AS etapa, a.ID_Afectacion, a.Parametrodelsistema, a.Presupuesto, PLANCONTABLE.Descripcion AS cuenta FROM  PLANCONTABLE RIGHT OUTER JOIN CUENTAACTIVIDAD ON PLANCONTABLE.Id_cuentacontable = CUENTAACTIVIDAD.id_CuentaContable RIGHT OUTER JOIN ETAPAS AS e INNER JOIN ACTIVIDADES AS a ON e.id_etapa = a.id_etapa ON CUENTAACTIVIDAD.id_Actividad = a.id_actividad WHERE ((a.id_etapa)=[e].[id_etapa] ) "

        If sField <> "" Then
            sRecords = Resource1.str1003
            ssql = ssql & " and " & sField & " like '%" & sText & "%' "
        Else
            sRecords = Resource1.str1002
            ssql = ssql & " "
        End If

        ssql = ssql & " ORDER BY a.descripcion "

        Dim dtData As New DataTable
        dtData = cargarDatosGrilla(ssql)
        Session.Add("frmActividadesView_nRecords", 0)

        Dim nRecords As Long
        nRecords = dtData.Rows.Count

        'hdnDelete verifica si el page_load viene desde el botón Delete, sino se limpia la variable de sesión 
        'If String.IsNullOrEmpty(hdnDelete.Value) Then
        Session.Add("frmActividadesView_IdActividad", Nothing)
        'End If

        Session.Add("frmActividad_Codigo", Nothing)
        Session.Add("frmActividad_Action", "")
        Session.Add("frmActividadesView_nRecords", nRecords)
        hdnDelete.Value = ""
        grilla.DataSource = dtData
        grilla.DataBind()
        setCaptionGrilla()

        lblReg.Text = nRecords & " " & sRecords

        If dtData.Rows.Count > 0 Then
            grilla.HeaderRow.TableSection = TableRowSection.TableHeader
            grilla.UseAccessibleHeader = True
        End If

        dvMessage.Visible = False
    End Sub

    Public Function cargarDatosGrilla(ByVal sQuery As String) As DataTable
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

    Private Function Delete() As Boolean
        Dim idActividad As String = Session("frmActividadesView_IdActividad")
        Dim bolResult As Boolean = False
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection
        Dim ssql As String = ""

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "delete from ACTIVIDADES where id_actividad='" & idActividad & "'"
            DBconn.Execute(ssql)

            ssql = "delete from cuentaACTIVIDAD where id_actividad='" & idActividad & "';"
            DBconn.Execute(ssql)

            bolResult = True
            Refresh()
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
        End Try

        Return bolResult
    End Function

    Private Sub Refresh()
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "a.id_actividad"
                Case 2
                    field = "a.descripcion"
                Case 3
                    field = "e.descripcion"

            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If

        dvMessage.Visible = False
    End Sub

    Private Sub LoadPageResources()
        hdnStr1005.Value = Resource1.str1005
        hdnStr12104.Value = Resource1.str12104
        hdnStr23.Value = Resource1.str23
    End Sub

End Class