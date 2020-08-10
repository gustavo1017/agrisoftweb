Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmLineaProductoView
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
            If Session("frmLineaProducto_Action") = "NEW" Or Session("frmLineaProducto_Action") = "EDIT" Then
                Refresh()
            End If
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "id_linea"
                Case 2
                    field = "descripcion"
            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        Session.Add("frmLineaProducto_Action", "NEW")
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs)
        If Session("frmLineaProductoView_IdLinea") Is Nothing Then
            Exit Sub
        End If

        Dim nRecords As Long = 0
        If Session("frmLineaProductoView_nRecords") IsNot Nothing Then
            nRecords = Convert.ToInt64(Session("frmLineaProductoView_nRecords"))
        End If

        Dim idLineaProducto As String = Session("frmLineaProductoView_IdLinea")
        Session.Add("frmLineaProducto_Codigo", idLineaProducto)
        Session.Add("frmLineaProducto_Action", "EDIT")
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmLineaProductoView_IdLinea") Is Nothing Then
            Exit Sub
        End If

        If hdnDelete.Value <> "Delete" Then
            Exit Sub
        End If

        Dim sParamSistema As String = ""
        If Not String.IsNullOrEmpty(Session("frmLineaProductoView_ParametroSistema")) Then
            sParamSistema = Session("frmLineaProductoView_ParametroSistema")
        End If

        'If sParamSistema = "0" Then
        Dim nRecords As Long = 0
            If Session("frmLineaProductoView_nRecords") IsNot Nothing Then
                nRecords = Convert.ToInt64(Session("frmLineaProductoView_nRecords"))
            End If

            If nRecords > 0 Then
                If Not Delete() Then
                    dvMessage.Visible = True
                    lblResults.Text = Resource1.str5006
                End If
            End If
        'Else
        '    dvMessage.Visible = True
        '    lblResults.Text = Resource1.str100
        'End If
    End Sub

    Protected Sub btnRefrescar_Click(sender As Object, e As EventArgs)
        find.Text = ""
        cargarDatosGrilla("", "")
    End Sub

    Protected Sub btnExportarExcel_Click(sender As Object, e As EventArgs)
        Dim Report As New CrLineasP
        Dim rsReporte As New ADODB.Recordset
        Dim dtData As New DataTable
        Dim sField As String = ""
        Dim sText As String = ""

        If Not String.IsNullOrEmpty(find.Text) Then
            sText = find.Text

            If cboFields.SelectedIndex <> -1 Then
                Select Case cboFields.SelectedIndex + 1
                    Case 1
                        sField = "id_linea"
                    Case 2
                        sField = "descripcion"
                End Select
            End If
        End If

        Dim sQuery As String = GetSQLQuery(sField, sText)

        Session.Add("frmExportParametros_ParamExport", "LINEAPRODUCTO")
        Session.Add("frmExportParametros_ReportQuery", sQuery)

        Response.Redirect("frmExport_Parametros.aspx")
    End Sub

    Protected Sub grilla_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = Resource1.str5002
            e.Row.Cells(1).Text = Resource1.str5003

            Dim columnWidth As New Unit(200, UnitType.Pixel)
            e.Row.Cells(1).Width = columnWidth
        End If
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

                Session.Add("frmLineaProductoView_IdLinea", row.Cells(0).Text)
                Session.Add("frmLineaProducto_Codigo", row.Cells(0).Text)
                Session.Add("frmLineaProductoView_LineaDescripcion", row.Cells(1).Text)
            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Seleccionar"
            End If
        Next
    End Sub

    Private Sub setCaptionsLabels()
        Title = Resource1.str39
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
        cboFields.Items.Add((Resource1.str5002))
        cboFields.Items.Add((Resource1.str5003))
        cboFields.SelectedIndex = 0
    End Sub

    Private Sub cargarDatosGrilla(ByVal sField As String, ByVal sText As String)
        Dim sRecords As String
        If sField <> "" Then
            sRecords = Resource1.str1003
        Else
            sRecords = Resource1.str1002
        End If

        Dim dtData As New DataTable
        Session.Add("frmLineaProductoView_nRecords", 0)
        Dim ssql As String = GetSQLQuery(sField, sText)
        dtData = cargarDataTable(ssql)

        'strParamExport = "CULTIVOS"
        'SQLExport = ssql

        Dim nRecords As Long
        nRecords = dtData.Rows.Count

        'hdnDelete verifica si el page_load viene desde el botón Delete, sino se limpia la variable de sesión 
        'If String.IsNullOrEmpty(hdnDelete.Value) Then
        Session.Add("frmLineaProductoView_IdLinea", Nothing)
        'End If

        Session.Add("frmLineaProducto_Codigo", Nothing)
        Session.Add("frmLineaProducto_Action", "")
        Session.Add("frmLineaProductoView_nRecords", nRecords)
        hdnDelete.Value = ""
        grilla.DataSource = dtData
        grilla.DataBind()
        setCaptionGrilla()

        lblReg.Text = nRecords & " " & sRecords
        dvMessage.Visible = False
    End Sub

    Private Function Delete() As Boolean
        Dim sMen As String = Resource1.str1005
        Dim idlinea As String = Session("frmLineaProductoView_IdLinea")
        Dim ssql As String = ""
        Dim DBconn As New ADODB.Connection
        Dim boolResult As Boolean = False
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "delete from LineasProductos where id_linea='" & idlinea & "';"
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

        Refresh()
        Return boolResult
    End Function

    Private Sub Refresh()
        Dim field As String = ""
        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "id_linea"
                Case 2
                    field = "descripcion"
            End Select
            cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
        End If

        dvMessage.Visible = False
    End Sub

    Private Sub LoadPageResources()
        hdnStr1005.Value = Resource1.str1005
        hdnStr12104.Value = Resource1.str12104
        hdnStr39.Value = Resource1.str39
    End Sub

    Public Function GetSQLQuery(ByVal sField As String, ByVal sText As String) As String
        Dim ssql As String = "select id_linea, descripcion from LINEASPRODUCTOS "

        If sField <> "" Then
            ssql = ssql & " where " & sField & " like '%" & sText & "%';"
        Else
            ssql = ssql & ";"
        End If

        Return ssql
    End Function
End Class