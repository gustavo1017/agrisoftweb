Imports AgrisoftWeb.UI.Resources
Imports System.Globalization
Imports System.Web.Services
Imports AgrisoftWeb.BL


Public Class frmMainMenu
    Inherits BasePage


    Protected Friend sbtnIntroducirDatos As String
    Protected Friend sbtnImportarArchivodeTexto As String
    Protected Friend sbtnImport As String
    Protected Friend sbtnExportExcelMobile As String
    Protected Friend sbtnIngresarRegistros As String
    Protected Friend sbtnIngresoCostos As String
    Protected Friend sbtnIngresoAlmacen As String
    Protected Friend sbtnIngresoCosechas As String
    Protected Friend sbtnPlaneamiento As String
    Protected Friend sbtnEstructuradeCostos As String
    Protected Friend sbtnCentrodeCostos As String
    Protected Friend sbtnCultivos As String
    Protected Friend sbtnZonaTrabajo As String
    Protected Friend sbtnProcesos As String
    Protected Friend sbtnEtapas As String
    Protected Friend sbtnActividades As String
    Protected Friend sbtnRecursos As String
    Protected Friend sbtnPersonal As String
    Protected Friend sbtnInsumos As String
    Protected Friend sbtnMaquinaria As String
    Protected Friend sbtnProveedores As String
    Protected Friend sbtnLineaProductos As String
    Protected Friend sbtnReportes As String
    Protected Friend sbtnReportedeCostos As String
    Protected Friend sbtnReporteRecursosHa As String
    Protected Friend sbtnReporteZonaTrabajoRecursosResumido As String
    Protected Friend sbtnReporteZonaTrabajoRecursosDetallado As String
    Protected Friend sbtnReporteZonaTrabajoActividades As String
    Protected Friend sbtnReporteCostosKilo As String
    Protected Friend sbtnReportedeCosechas As String
    Protected Friend sbtnReporteCampanas As String
    Protected Friend sbtnReportedeAlmacen As String
    Protected Friend sbtnReporteSaldosAlmacen As String
    Protected Friend sbtnReporteKardexStandar As String
    Protected Friend sbtnhelp As String
    Protected Friend sbtnImportar As String
    Protected Friend stitleABCosts As String
    Protected Friend sbtnLogout As String
    Protected Friend sbtnstr12106 As String
    Protected Friend sbtnstr99999940 As String
    Protected Friend sbtnUsuario As String
    Protected Friend sbtnUsuariostr12000 As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        Dim businessUser = "DEMO01"

        sbtnImportar = Resource1.str9277

        sbtnImportarArchivodeTexto = Resource1.str99999960
        sbtnImport = Resource1.str9277
        sbtnExportExcelMobile = Resource1.str9194
        sbtnIngresarRegistros = Resource1.str99999973
        sbtnIngresoCostos = Resource1.str99999974
        sbtnIngresoAlmacen = Resource1.str11010
        sbtnIngresoCosechas = Resource1.str601
        sbtnPlaneamiento = Resource1.str99999975
        sbtnEstructuradeCostos = Resource1.str123
        sbtnCentrodeCostos = Resource1.str124
        sbtnCultivos = Resource1.str5001
        sbtnZonaTrabajo = Resource1.str547
        sbtnProcesos = Resource1.str170
        sbtnEtapas = Resource1.str6001
        sbtnActividades = Resource1.str23
        sbtnRecursos = Resource1.str2011
        sbtnPersonal = Resource1.str4001
        sbtnInsumos = Resource1.str16
        sbtnMaquinaria = Resource1.str17
        sbtnProveedores = Resource1.str38
        sbtnLineaProductos = Resource1.str7007
        sbtnReportes = Resource1.str2010
        sbtnhelp = Resource1.str131
        sbtnReportedeCostos = Resource1.str99999976
        sbtnReporteRecursosHa = Resource1.str9242
        sbtnReporteZonaTrabajoRecursosResumido = Resource1.str99999977
        sbtnReporteZonaTrabajoRecursosDetallado = Resource1.str99999978
        sbtnReporteZonaTrabajoActividades = Resource1.str9212
        sbtnReporteCostosKilo = Resource1.str9275
        sbtnReportedeCosechas = Resource1.str99999951
        sbtnReporteCampanas = Resource1.str99999979
        sbtnReportedeAlmacen = Resource1.str99999950
        sbtnReporteSaldosAlmacen = Resource1.str9205
        sbtnReporteKardexStandar = Resource1.str9207
        stitleABCosts = Resource1.str9207
        sbtnLogout = Resource1.str99999940
        sbtnstr12106 = Resource1.str12106
        sbtnstr99999940 = Resource1.str99999940
        sbtnUsuario = businessUser
        sbtnUsuariostr12000 = Resource1.str12000


    End Sub



End Class