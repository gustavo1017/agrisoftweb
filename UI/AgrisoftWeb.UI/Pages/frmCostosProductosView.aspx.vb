Imports System.Globalization
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmCostosProductosView
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
            If Session("frmCostosProductos_Action") = "NEW" Or Session("frmCostosProductos_Action") = "EDIT" Then
                Refresh()
            End If
        End If
    End Sub

    Protected Sub btnBorradoTotal_Click(sender As Object, e As EventArgs)
        If Session("frmCostosProductosView_NumeroParte") Is Nothing Then
            Exit Sub
        End If

        Dim nRecords As Long = 0
        If Session("frmCostosProductosView_nRecords") IsNot Nothing Then
            nRecords = Convert.ToInt64(Session("frmCostosProductosView_nRecords"))
        End If

        If nRecords > 0 Then
            If Not DeleteTotal() Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
                lblResult.Text = Resource1.str106
            End If
            Refresh()
        End If
    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        If Session("frmCostosProductosView_IdCosto") Is Nothing Then
            Exit Sub
        End If

        If hdnDelete.Value <> "Delete" Then
            Exit Sub
        End If

        Dim nRecords As Long = 0
        If Session("frmCostosProductosView_nRecords") IsNot Nothing Then
            nRecords = Convert.ToInt64(Session("frmCostosProductosView_nRecords"))
        End If

        If nRecords > 0 Then
            If Not Delete() Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-success")
                lblResult.Text = Resource1.str106
            End If
            Refresh()
        End If

    End Sub

    Protected Sub btnExportarExcel_Click(sender As Object, e As EventArgs)
        Dim sField As String = ""
        Dim sText As String = ""

        If Not String.IsNullOrEmpty(find.Text) Then
            sText = find.Text
            sField = GetFieldFilter()
        End If

        Dim ssql As String = GetSqlQuery(sField, sText, "", "")

        Session.Add("frmExportParametros_ParamExport", "INGRESOSALMACEN")
        Session.Add("frmExportParametros_ReportQuery", ssql)
        Response.Redirect("frmExport_Parametros.aspx")
    End Sub

    Protected Sub btnRefrescar_Click(sender As Object, e As EventArgs)
        find.Text = ""
        cargarDatosGrilla("", "")
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        Refresh()
    End Sub

    Protected Sub grilla_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(1).Text = Resource1.str516
                e.Row.Cells(3).Text = Resource1.str503
                e.Row.Cells(4).Text = Resource1.str504
                e.Row.Cells(5).Text = Resource1.str7001
                e.Row.Cells(6).Text = Resource1.str902
                e.Row.Cells(7).Text = Resource1.str6001
                e.Row.Cells(8).Text = Resource1.str546
                e.Row.Cells(9).Text = Resource1.str2012
                e.Row.Cells(10).Text = Resource1.str3011
                e.Row.Cells(11).Text = Resource1.str2003
                e.Row.Cells(12).Text = Resource1.str7001
                e.Row.Cells(13).Text = Resource1.str3007
                e.Row.Cells(14).Text = Resource1.str3008
                e.Row.Cells(15).Text = Resource1.str3009
                e.Row.Cells(16).Text = Resource1.str541
                e.Row.Cells(17).Text = Resource1.str10003
                e.Row.Cells(20).Text = Resource1.str9264
                e.Row.Cells(23).Text = Resource1.str12000
                e.Row.Cells(24).Text = Resource1.str10203

                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(13).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(14).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(15).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(17).HorizontalAlign = HorizontalAlign.Center

            Case DataControlRowType.DataRow
                'e.Row.Cells(11).Text = Convert.ToDecimal(e.Row.Cells(11).Text).ToString("###0.00")
                e.Row.Cells(13).Text = Convert.ToDouble(e.Row.Cells(13).Text).ToString(CultureInfo.InvariantCulture)
                e.Row.Cells(14).Text = Convert.ToDouble(e.Row.Cells(14).Text).ToString("###0.00", CultureInfo.InvariantCulture)
                e.Row.Cells(15).Text = Convert.ToDouble(e.Row.Cells(15).Text).ToString("###0.00", CultureInfo.InvariantCulture)
                Dim dateConvert As DateTime
                If DateTime.TryParse(e.Row.Cells(4).Text, dateConvert) Then
                    e.Row.Cells(4).Text = Convert.ToDateTime(e.Row.Cells(4).Text).ToString("dd/MM/yyyy")
                End If
        End Select

        'e.Row.Cells(0).Visible = False
        e.Row.Cells(2).Visible = False
        e.Row.Cells(6).Visible = False
        e.Row.Cells(7).Visible = False
        e.Row.Cells(8).Visible = False
        e.Row.Cells(10).Visible = False
        e.Row.Cells(11).Visible = False
        e.Row.Cells(12).Visible = False
        e.Row.Cells(16).Visible = False
        e.Row.Cells(18).Visible = False
        e.Row.Cells(19).Visible = False
        e.Row.Cells(20).Visible = False
        e.Row.Cells(21).Visible = False
        e.Row.Cells(22).Visible = False
        e.Row.Cells(25).Visible = False
    End Sub

    Protected Sub grilla_SelectedIndexChanged(sender As Object, e As EventArgs)
        For Each row As GridViewRow In grilla.Rows
            If row.RowIndex = grilla.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = ""

                Session.Add("frmCostosProductosView_IdCosto", row.Cells(0).Text)
                Session.Add("frmCostosProductos_Codigo", row.Cells(0).Text)
                Session.Add("frmCostosProductosView_NumeroParte", row.Cells(2).Text)
                'Session.Add("frmCostosProductos_StrIdenlace", row.Cells(0).Text)
                'Session.Add("frmCostosProductosView_ParametroSistema", row.Cells(4).Text)
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

    Protected Sub cboFields_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cboFields.SelectedIndex = 1 Then
            'fmeFecha.Visible = True
            find.Visible = False
            fecha_0.Visible = True
            fecha_1.Visible = True
            Label1.Visible = True
            find2.Visible = True
        Else
            'fmeFecha.Visible = False
            find.Visible = True
            fecha_0.Visible = False
            fecha_1.Visible = False
            Label1.Visible = False
            find2.Visible = False
            find2.Text = ""
        End If
    End Sub

    Public Function Delete() As Boolean
        Dim idCosto As String = Session("frmCostosProductosView_IdCosto").ToString()
        Dim bolResult As Boolean = False
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection
        Dim ssql As String = ""
        Dim RS As New ADODB.Recordset

        Try
            DBconn.Open(objBL.GetSQLConnection())
            ssql = "delete from COSTOS where id_costo=" & idCosto & ";"
            DBconn.Execute(ssql)
            bolResult = True
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

    Private Function DeleteTotal() As Boolean
        'Dim sMen As String
        'Dim i As Short
        'Dim intCount As Short
        Dim bolResult As Boolean = False
        Dim nroParte As String = Session("frmCostosProductosView_NumeroParte").ToString()
        Dim objBL As New GenericMethods("Fundo0")
        Dim DBconn As New ADODB.Connection
        Dim ssql As String = "delete from COSTOS where numero_parte='" & nroParte & "' and precio_contable>=0;"

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)

            btnRefrescar_Click(btnRefrescar, New System.EventArgs())
            bolResult = True
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
        Title = Resource1.str11010
        lblFind.Text = Resource1.str1004
    End Sub

    Private Sub setCaptionsButtons()
        btnNuevo.Text = Resource1.str1
        btnModificar.Text = Resource1.str2
        btnEliminar.Text = Resource1.str3
        btnBuscar.Text = Resource1.str7
        btnRefrescar.Text = Resource1.str8
        btnExportarExcel.Text = Resource1.str12
        btnBorradoTotal.Text = Resource1.str163
    End Sub

    Private Sub setCaptionGrilla()
        'Las columnas ocultas se pusieron en el evento grilla_RowDataBound
    End Sub

    Private Sub setCboFind()
        cboFields.Items.Clear()

        cboFields.Items.Add(Resource1.str503)
        cboFields.Items.Add(Resource1.str3047)
        'cboFields.Items.Add(Resource1.str511)
        'cboFields.Items.Add(Resource1.str211)
        'cboFields.Items.Add(Resource1.str3001)
        cboFields.Items.Add(Resource1.str544)
        'cboFields.Items.Add(Resource1.str516)
        'cboFields.Items.Add(Resource1.str21)   ' CULTIVO
        cboFields.Items.Add(Resource1.str7001) ' PROVEEDOR
        'cboFields.Items.Add(Resource1.str3011) ' TRABAJADOR
        'cboFields.Items.Add(Resource1.str401) ' MAQUINARIA

        cboFields.SelectedIndex = 1
        cboFields_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Private Sub Refresh()
        Dim field As String = ""

        If cboFields.SelectedIndex <> -1 Then
            field = GetFieldFilter()

            If field = "costos.fecha" Then
                Dim fechaValida As Boolean = True
                Dim fechaFrom As DateTime = DateTime.Now
                Dim fechaTo As DateTime = DateTime.Now

                If Not String.IsNullOrEmpty(fecha_0.Text) Then
                    fechaValida = DateTime.TryParseExact(fecha_0.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechaFrom)
                End If

                If Not String.IsNullOrEmpty(fecha_1.Text) Then
                    fechaValida = (fechaValida And DateTime.TryParseExact(fecha_1.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, fechaTo))
                End If

                If Not fechaValida Then
                    dvMessage.Visible = True
                    dvMessage.Attributes.Add("class", "alert alert-danger")
                    lblResult.Text = Resource1.str99991
                    Return
                End If

                cargarDatosGrilla(field, fechaFrom.ToString("yyyy/MM/dd"), fechaTo.ToString("yyyy/MM/dd"))
            Else
                cargarDatosGrilla(field, UCase(Replace(find.Text, " ", "")))
            End If
        End If

        dvMessage.Visible = False
    End Sub

    Private Sub cargarDatosGrilla(ByVal sField As String, ByVal sText As String, Optional ByVal sText2 As String = "", Optional ByVal sOrder As String = "")
        Dim sRecords As String
        Dim ssql As String = GetSqlQuery(sField, sText, sText2, sOrder)

        If sField <> "" Then
            sRecords = Resource1.str1003
        Else
            sRecords = Resource1.str1002
        End If

        Dim dtData As New DataTable
        dtData = cargarDataTable(ssql)
        Session.Add("frmCostosProductosView_nRecords", 0)

        Dim nRecords As Long
        nRecords = dtData.Rows.Count

        'hdnDelete verifica si el page_load viene desde el botón Delete, sino se limpia la variable de sesión 
        'If String.IsNullOrEmpty(hdnDelete.Value) Then
        Session.Add("frmCostosProductosView_IdCosto", Nothing)
        'End If

        Session.Add("frmCostosProductos_Codigo", Nothing)
        Session.Add("frmCostosProductos_StrIdenlace", Nothing)
        Session.Add("frmCostosProductos_Action", "")
        Session.Add("frmCostosProductosView_nRecords", nRecords)
        Session.Add("frmCostosProductosView_NumeroParte", Nothing)

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
        Dim field As String = "costos.fecha"

        If cboFields.SelectedIndex <> -1 Then
            Select Case cboFields.SelectedIndex + 1
                Case 1
                    field = "costos.numero_parte"
                Case 2
                    field = "costos.fecha"
                'Case 3
                '    field = "ZONA_TRABAJO.descripcion"
                'Case 3
                '    field = "ETAPAS.descripcion"
                'Case 4
                '    field = "ACTIVIDADES.descripcion"
                Case 3
                    field = "PRODUCTOS.descripcion"
                    'Case 6
                    '    field = "TIPO_COSTO.descripcion"
                    'Case 8
                    '    field = "CULTIVOS.descripcion"
                Case 4
                    field = "case when tipo_costo='c' and substring(costos.observaciones,1,7) <>'cosecha' then proveedores.descripcion else '' end"

                    'Case 10
                    '    field = "PERSONAL.NOMBRE"
                    'Case 11
                    '    field = "MAQUINAS.descripcion"

            End Select
        End If

        Return field
    End Function

    Private Function GetSqlQuery(ByVal sField As String, ByVal sText As String, ByVal sText2 As String, ByVal sOrder As String) As String
        Dim strUserLogged As String = Session("UserLogged")
        Dim sCantidadRows As String = GetEmpresaParametroValorByUserName(strUserLogged, "CantidadRegistros")

        If String.IsNullOrEmpty(sCantidadRows) Then
            sCantidadRows = ConfigurationManager.AppSettings("Default_CantRegistros")
        End If

        Dim ssql As String = "SELECT top " & sCantidadRows & " COSTOS.id_costo, CASE WHEN SUBSTRING(costos.OBSERVACIONES,1,7)='Cosecha' then 'Cosecha' else TIPO_COSTO.descripcion end as tipocosto, COSTOS.numero_parte, COSTOS.numero_parte as presup, COSTOS.fecha, CULTIVOS.descripcion as cultivo, ZONA_TRABAJO.descripcion as zt, ETAPAS.descripcion as etapa, ACTIVIDADES.descripcion as actividad, PRODUCTOS.descripcion as producto, PERSONAL.nombre as trabajador, MAQUINAS.descripcion as maquina, case when tipo_costo='c' and substring(costos.observaciones,1,7) <>'cosecha' then proveedores.descripcion else '' end as nombreprov, COSTOS.cantidad, COSTOS.costo_unitario_standar, COSTOS.monto_standar as tot, COSTOS.campana, case when cOSTOS.TC=0 then '" & Resource1.str10004 & "' else '" & Resource1.str10005 & "' end AS moneda, COSTOS.Tipo_Cambio, COSTOS.Precio_contable,case when COSTOS.tipo_costo in ('H','I','R','M') then costos.id_enlace else 0 end AS Avance , COSTOS.id_zonatrabajo, COSTOS.observaciones, COSTOS.id_usuario, COSTOS.fechaauditoria, COSTOS.tipo_costo FROM ((CULTIVOS INNER JOIN ZONA_TRABAJO ON CULTIVOS.id_cultivo = ZONA_TRABAJO.id_cultivo) INNER JOIN (PRODUCTOS INNER JOIN (PERSONAL INNER JOIN ((ETAPAS INNER JOIN ACTIVIDADES ON ETAPAS.id_etapa = ACTIVIDADES.id_etapa) INNER JOIN (TIPO_COSTO INNER JOIN (MAQUINAS INNER JOIN COSTOS ON MAQUINAS.id_maquinaria = COSTOS.id_maquinaria) ON TIPO_COSTO.id_tipocosto = COSTOS.tipo_costo) ON ACTIVIDADES.id_actividad = COSTOS.id_actividad) ON PERSONAL.id_personal = COSTOS.id_personal) ON PRODUCTOS.id_producto = COSTOS.id_producto) ON ZONA_TRABAJO.id_zonatrabajo = COSTOS.id_zonatrabajo) inner join proveedores on costos.id_proveedor=proveedores.id_proveedor and tipo_costo = 'C' and costos.observaciones not like '%Cosecha%'"

        If sField <> "" Then
            If sField = "costos.fecha" Then
                ssql = ssql & " and ZONA_TRABAJO.descripcion like '%" & find2.Text & "%'  and " & sField & " BETWEEN  convert(datetime,replace('" & DateTime.Parse(sText).ToString("yyyyMMdd") & "',',','/')) AND convert(datetime,replace('" & DateTime.Parse(sText2).ToString("yyyyMMdd") & "',',','/'))  "
            Else
                If sField = "costos.numero_parte" Then
                    ssql = ssql & " and ZONA_TRABAJO.descripcion like '%" & find2.Text & "%'  and " & sField & " = '" & sText & "' "
                Else
                    ssql = ssql & " and ZONA_TRABAJO.descripcion like '%" & find2.Text & "%'  and " & sField & " like '%" & sText & "%' "
                End If
            End If
        Else
        End If

        If sOrder <> "" And sOrder <> "costos.fecha" Then
            ssql = ssql & " ORDER BY " & sOrder & ", costos.fecha;"
        Else
            ssql = ssql & " ORDER BY costos.id_costo DESC "
        End If

        Return ssql
    End Function

    Private Sub LoadPageResources()
        hdnStr1005.Value = Resource1.str1005
        hdnStr12104.Value = Resource1.str12104
        hdnStr89.Value = Resource1.str11010
        hdnstr99999980.Value = Resource1.str99999980

    End Sub

End Class