Imports System.IO
Imports AgrisoftWeb.BL
Imports ClosedXML.Excel

Public Class frmExportarExcelMobile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GenerateExcel()
    End Sub

    Protected Sub GenerateExcel()
        Dim dtPlagas As New DataTable()
        dtPlagas.Columns.Add("id_plaga")
        dtPlagas.Columns.Add("descripcion")

        Dim dtClones As New DataTable()
        dtClones.Columns.Add("id_clon")
        dtClones.Columns.Add("descripcion")

        Dim dtPersonal As New DataTable()
        dtPersonal.Columns.Add("id_personal")
        dtPersonal.Columns.Add("nombre")

        Dim dtZonaTrabajo As New DataTable()
        dtZonaTrabajo.Columns.Add("id_zonatrabajo")
        dtZonaTrabajo.Columns.Add("descripcion")

        Dim dtActividad As New DataTable()
        dtActividad.Columns.Add("id_actividad")
        dtActividad.Columns.Add("descripcion")

        Dim dtPatron As New DataTable()
        dtPatron.Columns.Add("id_patron")
        dtPatron.Columns.Add("descripcion")

        Dim dtProductos As New DataTable()
        dtProductos.Columns.Add("id_producto")
        dtProductos.Columns.Add("descripcion")

        Dim dtMaquinas As New DataTable()
        dtMaquinas.Columns.Add("id_maquinaria")
        dtMaquinas.Columns.Add("descripcion")


        Dim dtESTADOFISICO As New DataTable()
        dtESTADOFISICO.Columns.Add("id_estadofisico")
        dtESTADOFISICO.Columns.Add("descripcion")

        Dim dtESTADOSANITARIO As New DataTable()
        dtESTADOSANITARIO.Columns.Add("id_estadosanitario")
        dtESTADOSANITARIO.Columns.Add("descripcion")

        Dim dtESTADOSITIO As New DataTable()
        dtESTADOSITIO.Columns.Add("id_estadositio")
        dtESTADOSITIO.Columns.Add("descripcion")

        Dim dtCONDICION As New DataTable()
        dtCONDICION.Columns.Add("id_condicion")
        dtCONDICION.Columns.Add("descripcion")

        Dim dtINDICEMAZORCA As New DataTable()
        dtINDICEMAZORCA.Columns.Add("id_IM")
        dtINDICEMAZORCA.Columns.Add("descripcion")

        Dim dtSECTORES As New DataTable()
        dtSECTORES.Columns.Add("id_sector")
        dtSECTORES.Columns.Add("descripcion")


        Dim dtVersionWeb As New DataTable()
        dtVersionWeb.Columns.Add("id_versionweb")
        dtVersionWeb.Columns.Add("descripcion")
        dtVersionWeb.Columns.Add("code")

        Dim dtVersionWeb2 As New DataTable()
        dtVersionWeb2.Columns.Add("id_versionweb")
        dtVersionWeb2.Columns.Add("fechaVersion")
        dtVersionWeb2.Columns.Add("descripcion")
        dtVersionWeb2.Columns.Add("code")
        dtVersionWeb2.Columns.Add("nombreAbrev")
        dtVersionWeb2.Columns.Add("nombre1")
        dtVersionWeb2.Columns.Add("nombre2")

        Dim sqlPlagas As String = "select id_plaga,descripcion from plagas order by descripcion"
        Dim sqlClones As String = "select id_clon,descripcion from clones  order by id_clon"
        Dim sqlPersonal As String = "select id_personal,nombre from personal  order by nombre"
        Dim sqlZonatrabajo As String = "select id_zonatrabajo,descripcion from zona_trabajo  order by descripcion"
        Dim sqlActividades As String = "select id_actividad,descripcion from actividades  order by descripcion"
        Dim sqlPatrones As String = "select '0000' AS id_patron, '0000' as descripcion "
        Dim sqlProductos As String = "SELECT ID_PRODUCTO,DESCRIPCION FROM PRODUCTOS  order by descripcion"
        Dim sqlMaquinas As String = "SELECT ID_MAQUINARIA,DESCRIPCION FROM MAQUINAS  order by descripcion"
        Dim sqlVersionWeb As String = "SELECT '00000001' as id_versionweb,'CUADRILLA WEB' as descripcion,'VersionWeb' as code"
        Dim sqlVersionWeb2 As String = "select '00000001' as id_versionweb,'2020/01/01' as fechaVersion,'Cuadrilla web' as descripcion,'000000' as code,'GASTGEN' as nombreAbrev,'VersionWeb' as nombre1,'Versionweb' as nombre2"
        Dim sqlEstadositio As String = "SELECT ID_estadositio,DESCRIPCION FROm estadositio  order by descripcion"
        Dim sqlEstadofisico As String = "SELECT ID_estadofisico,DESCRIPCION FROm estadofisico  order by descripcion"
        Dim sqlEstadosanitario As String = "SELECT ID_estadosanitario,DESCRIPCION FROm estadosanitario  order by descripcion"
        Dim sqlcondicion As String = "SELECT ID_condicion,DESCRIPCION FROm condicion  order by descripcion"
        Dim sqlIndiceMazorca As String = "SELECT ID_im,DESCRIPCION FROm indicemazorca  order by id_clon"
        Dim sqlSectores As String = "SELECT ID_sector,DESCRIPCION FROm sectores  order by descripcion"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())

        Dim RS As New ADODB.Recordset
        'DBconn.Open()
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(sqlPlagas)
        RS.Open()

        Dim dr As DataRow


        While Not RS.EOF
            dr = dtPlagas.NewRow()
            dr.Item(0) = RS.Fields("id_plaga").Value
            dr.Item(1) = RS.Fields("descripcion").Value
            dtPlagas.Rows.Add(dr)
            RS.MoveNext()
        End While

        RS.Close()

        RS.let_Source(sqlClones)
        RS.Open()
        Dim drClones As DataRow
        While Not RS.EOF
            drClones = dtClones.NewRow()
            drClones.Item(0) = RS.Fields("id_clon").Value
            drClones.Item(1) = RS.Fields("descripcion").Value
            dtClones.Rows.Add(drClones)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlPersonal)
        RS.Open()
        Dim drPersonal As DataRow
        While Not RS.EOF
            drPersonal = dtPersonal.NewRow()
            drPersonal.Item(0) = RS.Fields("id_personal").Value
            drPersonal.Item(1) = RS.Fields("nombre").Value
            dtPersonal.Rows.Add(drPersonal)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlZonatrabajo)
        RS.Open()
        Dim drZonaTrabajo As DataRow
        While Not RS.EOF
            drZonaTrabajo = dtZonaTrabajo.NewRow()
            drZonaTrabajo.Item(0) = RS.Fields("id_zonatrabajo").Value
            drZonaTrabajo.Item(1) = RS.Fields("descripcion").Value
            dtZonaTrabajo.Rows.Add(drZonaTrabajo)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlActividades)
        RS.Open()
        Dim drActividades As DataRow
        While Not RS.EOF
            drActividades = dtActividad.NewRow()
            drActividades.Item(0) = RS.Fields("id_actividad").Value
            drActividades.Item(1) = RS.Fields("descripcion").Value
            dtActividad.Rows.Add(drActividades)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlPatrones)
        RS.Open()
        Dim drPatrones As DataRow
        While Not RS.EOF
            drPatrones = dtPatron.NewRow()
            drPatrones.Item(0) = RS.Fields("id_patron").Value
            drPatrones.Item(1) = RS.Fields("descripcion").Value
            dtPatron.Rows.Add(drPatrones)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlProductos)
        RS.Open()
        Dim drProductos As DataRow
        While Not RS.EOF
            drProductos = dtProductos.NewRow()
            drProductos.Item(0) = RS.Fields("id_producto").Value
            drProductos.Item(1) = RS.Fields("descripcion").Value
            dtProductos.Rows.Add(drProductos)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlEstadositio)
        RS.Open()
        Dim drES As DataRow
        While Not RS.EOF
            drES = dtESTADOSITIO.NewRow()
            drES.Item(0) = RS.Fields("ID_estadositio").Value
            drES.Item(1) = RS.Fields("descripcion").Value
            dtESTADOSITIO.Rows.Add(drES)
            RS.MoveNext()
        End While
        RS.Close()



        RS.let_Source(sqlEstadosanitario)
        RS.Open()
        Dim drESa As DataRow
        While Not RS.EOF
            drESa = dtESTADOSANITARIO.NewRow()
            drESa.Item(0) = RS.Fields("id_estadosanitario").Value
            drESa.Item(1) = RS.Fields("descripcion").Value
            dtESTADOSANITARIO.Rows.Add(drESa)
            RS.MoveNext()
        End While
        RS.Close()


        RS.let_Source(sqlEstadofisico)
        RS.Open()
        Dim dref As DataRow
        While Not RS.EOF
            dref = dtESTADOFISICO.NewRow()
            dref.Item(0) = RS.Fields("id_estadofisico").Value
            dref.Item(1) = RS.Fields("descripcion").Value
            dtESTADOFISICO.Rows.Add(dref)
            RS.MoveNext()
        End While
        RS.Close()


        RS.let_Source(sqlIndiceMazorca)
        RS.Open()
        Dim drIM As DataRow
        While Not RS.EOF
            drIM = dtINDICEMAZORCA.NewRow()
            drIM.Item(0) = RS.Fields("id_im").Value
            drIM.Item(1) = RS.Fields("descripcion").Value
            dtINDICEMAZORCA.Rows.Add(drIM)
            RS.MoveNext()
        End While
        RS.Close()


        RS.let_Source(sqlSectores)
        RS.Open()
        Dim drsec As DataRow
        While Not RS.EOF
            drsec = dtSECTORES.NewRow()
            drsec.Item(0) = RS.Fields("id_sector").Value
            drsec.Item(1) = RS.Fields("descripcion").Value
            dtSECTORES.Rows.Add(drsec)
            RS.MoveNext()
        End While
        RS.Close()


        RS.let_Source(sqlcondicion)
        RS.Open()
        Dim drcondicion As DataRow
        While Not RS.EOF
            drcondicion = dtCONDICION.NewRow()
            drcondicion.Item(0) = RS.Fields("id_condicion").Value
            drcondicion.Item(1) = RS.Fields("descripcion").Value
            dtCONDICION.Rows.Add(drcondicion)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlVersionWeb)
        RS.Open()
        Dim drVersionWeb As DataRow
        While Not RS.EOF
            drVersionWeb = dtVersionWeb.NewRow()
            drVersionWeb.Item(0) = RS.Fields("id_versionweb").Value
            drVersionWeb.Item(1) = RS.Fields("descripcion").Value
            drVersionWeb.Item(2) = RS.Fields("code").Value
            dtVersionWeb.Rows.Add(drVersionWeb)
            RS.MoveNext()
        End While
        RS.Close()

        RS.let_Source(sqlVersionWeb2)
        RS.Open()
        Dim drVersionWeb2 As DataRow
        While Not RS.EOF
            drVersionWeb2 = dtVersionWeb2.NewRow()
            drVersionWeb2.Item(0) = RS.Fields("id_versionweb").Value
            drVersionWeb2.Item(1) = RS.Fields("fechaVersion").Value
            drVersionWeb2.Item(2) = RS.Fields("descripcion").Value
            drVersionWeb2.Item(3) = RS.Fields("code").Value
            drVersionWeb2.Item(4) = RS.Fields("nombreAbrev").Value
            drVersionWeb2.Item(5) = RS.Fields("nombre1").Value
            drVersionWeb2.Item(6) = RS.Fields("nombre2").Value
            dtVersionWeb2.Rows.Add(drVersionWeb2)
            RS.MoveNext()
        End While
        RS.Close()

        Dim wb As New XLWorkbook()
        wb.Worksheets.Add(dtPlagas, "PLAGAS")
        wb.Worksheets.Add(dtClones, "CLON")
        wb.Worksheets.Add(dtPatron, "PATRON")
        wb.Worksheets.Add(dtZonaTrabajo, "ZONA_TRABAJO")
        wb.Worksheets.Add(dtMaquinas, "MAQUINARIA")
        wb.Worksheets.Add(dtActividad, "ACTIVIDADES")
        wb.Worksheets.Add(dtProductos, "INSUMOS")
        wb.Worksheets.Add(dtPersonal, "TRABAJADORES")
        wb.Worksheets.Add(dtVersionWeb, "CUADRILLA")
        wb.Worksheets.Add(dtVersionWeb2, "PLANIFICACION")
        wb.Worksheets.Add(dtCONDICION, "CONDICION")
        wb.Worksheets.Add(dtESTADOFISICO, "ESTADOFISICO")
        wb.Worksheets.Add(dtESTADOSANITARIO, "ESTADOSANITARIO")
        wb.Worksheets.Add(dtESTADOSITIO, "ESTADOSITIO")
        wb.Worksheets.Add(dtINDICEMAZORCA, "INDICEMAZORCA")
        wb.Worksheets.Add(dtSECTORES, "SECTORES")


        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=analisis1.xls")

        Using memoStream As New MemoryStream
            wb.SaveAs(memoStream)
            memoStream.WriteTo(Response.OutputStream)
            Response.Flush()
            Response.End()
        End Using


    End Sub


End Class