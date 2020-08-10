Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmMaquinas_view
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
            If Session("frmMaquinas_Action") = "NEW" Or Session("frmMaquinas_Action") = "EDIT" Then
                Refresh()
            End If
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        Refresh()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs)
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmMaquinasView_IdMaquina") Is Nothing Then
            Exit Sub
        End If

        If hdnDelete.Value <> "Delete" Then
            Exit Sub
        End If

        Dim sParamSistema As String = ""
        If Not String.IsNullOrEmpty(Session("frmMaquinasView_ParametroSistema")) Then
            sParamSistema = Session("frmMaquinasView_ParametroSistema")
        End If
        If sParamSistema = "0" Then

            Dim nRecords As Long = 0
            If Session("frmMaquinasView_nRecords") IsNot Nothing Then
                nRecords = Convert.ToInt64(Session("frmMaquinasView_nRecords"))
            End If

            If nRecords > 0 Then
                If Not Delete() Then
                    dvMessage.Visible = True
                    lblResults.Text = Resource1.str408
                End If
                Refresh()
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
        Dim sField As String = ""
        Dim sText As String = ""

        If Not String.IsNullOrEmpty(find.Text) Then
            sText = find.Text
            sField = GetFieldFilter()
        End If

        Dim ssql As String = GetSqlQuery(sField, sText)

        Session.Add("frmExportParametros_ParamExport", "MAQUINAS")
        Session.Add("frmExportParametros_ReportQuery", ssql)
        Response.Redirect("frmExport_Parametros.aspx")
    End Sub

    Protected Sub grilla_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(0).Text = Resource1.str402
                e.Row.Cells(1).Text = Resource1.str403
                e.Row.Cells(2).Text = Resource1.str404
                e.Row.Cells(3).Text = Resource1.str514

                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right

                e.Row.Cells(2).Width = New Unit(80, UnitType.Pixel)
                e.Row.Cells(3).Width = New Unit(46, UnitType.Pixel)
                e.Row.Cells(4).Width = New Unit(46, UnitType.Pixel)
                e.Row.Cells(5).Width = New Unit(46, UnitType.Pixel)
                e.Row.Cells(6).Width = New Unit(46, UnitType.Pixel)

            Case DataControlRowType.DataRow
                e.Row.Cells(3).Text = Convert.ToDecimal(e.Row.Cells(3).Text).ToString("###0.00")

                'Dim dateConvert As DateTime
                'If DateTime.TryParse(e.Row.Cells(14).Text, dateConvert) Then
                '    e.Row.Cells(14).Text = Convert.ToDateTime(e.Row.Cells(14).Text).ToString("dd/MM/yyyy")
                'End If
        End Select


        e.Row.Cells(4).Visible = False
        e.Row.Cells(5).Visible = False
        e.Row.Cells(6).Visible = False
        e.Row.Cells(7).Visible = False
    End Sub

    Protected Sub grilla_SelectedIndexChanged(sender As Object, e As EventArgs)
        For Each row As GridViewRow In grilla.Rows
            If row.RowIndex = grilla.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = ""

                Session.Add("frmMaquinasView_IdMaquina", row.Cells(0).Text)
                Session.Add("frmMaquinas_Codigo", row.Cells(0).Text)
                Session.Add("frmMaquinasView_ParametroSistema", row.Cells(4).Text)
            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Seleccionar"
            End If
        Next
    End Sub

    Protected Sub grilla_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.cursor='pointer';this.style.textDecoration='underline';"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';"
            e.Row.ToolTip = "Seleccionar"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(grilla, "Select$" & e.Row.RowIndex)
        End If
    End Sub

    Public Function Delete() As Boolean
        Dim idMaquina As String = Session("frmMaquinasView_IdMaquina").ToString()
        Dim bolResult As Boolean = False
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection
        Dim ssql As String = ""
        Dim RS As New ADODB.Recordset

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "SELECT COSTOS.id_maquinaria From COSTOS WHERE (((COSTOS.id_maquinaria)='" & idMaquina & "'));"
            RS = New ADODB.Recordset
            RS.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

            If RS.EOF Then
                ssql = "delete from MAQUINAS where id_maquinaria='" & idMaquina & "';"
                DBconn.Execute(ssql)
                bolResult = True
            Else
                bolResult = False
            End If
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
            bolResult = False
        End Try

        Return bolResult
    End Function

    Private Sub setCaptionsLabels()
        Title = Resource1.str401
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
        cboFields.Items.Add(Resource1.str402)
        cboFields.Items.Add(Resource1.str403)
        cboFields.Items.Add(Resource1.str111)

        cboFields.SelectedIndex = 0
    End Sub

    Private Sub Refresh()
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            field = GetFieldFilter()
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If

        dvMessage.Visible = False
    End Sub

    Private Sub cargarDatosGrilla(ByVal sField As String, ByVal sText As String)
        Dim sRecords As String
        Dim ssql As String = GetSqlQuery(sField, sText)

        If sField <> "" Then
            sRecords = Resource1.str1003
        Else
            sRecords = Resource1.str1002
        End If

        Dim dtData As New DataTable
        dtData = cargarDataTable(ssql)
        Session.Add("frmMaquinasView_nRecords", 0)

        Dim nRecords As Long
        nRecords = dtData.Rows.Count

        'hdnDelete verifica si el page_load viene desde el botón Delete, sino se limpia la variable de sesión 
        'If String.IsNullOrEmpty(hdnDelete.Value) Then
        Session.Add("frmMaquinasView_IdMaquina", Nothing)
        'End If

        Session.Add("frmMaquinas_Codigo", Nothing)
        Session.Add("frmMaquinas_Action", "")
        Session.Add("frmMaquinasView_nRecords", nRecords)
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

    Private Function GetFieldFilter() As String
        Dim field As String = ""

        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "id_maquinaria"
                Case 2
                    field = "descripcion"
                Case 3
                    field = "tipo"
            End Select
        End If

        Return field
    End Function

    Private Function GetSqlQuery(ByVal sField As String, ByVal sText As String) As String
        Dim ssql As String = "select * from MAQUINAS "

        If sField <> "" Then
            ssql = ssql & " where " & sField & " like '%" & sText & "%';"
        Else
            ssql = ssql & ";"
        End If

        Return ssql
    End Function

    Private Sub LoadPageResources()
        hdnStr1005.Value = Resource1.str1005
        hdnStr12104.Value = Resource1.str12104
        hdnStr12124.Value = Resource1.str12124
    End Sub

End Class