<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmMainMenu.aspx.vb" Inherits="AgrisoftWeb.UI.frmMainMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .menuButton {
            height: 90px;
            width: 155px;
            white-space: normal;
            border-radius: 18px;
        }

        .menuCostosButton {
            background-color: lightgreen;
            border-color: lightgreen;
        }

        .menuReportesButton {
            background-color: darkorchid;
            border-color: darkorchid;
            color: white;
        }
    </style>

    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnImport").click(function () {
                window.location.href = "/Pages/ImportarCostos.aspx";
            });
            $("#btnImport2").click(function () {
                window.location.href = "/Pages/ImportarMapeo.aspx";
            });
            $("#btnHelp").click(function () {
                window.location.href = "/Pages/frmHelp.aspx";
            });
            $("#btnExportExcelMobile").click(function () {
                window.location.href = "/Pages/frmExportarExcelMobile.aspx";
            });
            $("#btnIngresoCostos").click(function () {
                window.location.href = "/Pages/frmCostos_View.aspx";
            });
            $("#btnIngresoAlmacen").click(function () {
                window.location.href = "/Pages/frmCostosProductosView.aspx";
            });
            $("#btnIngresoCosechas").click(function () {
                window.location.href = "/Pages/frmCostosCosechasView.aspx";
            });
            $("#btnPlaneamiento").click(function () {
                window.location.href = "/Pages/frmPlaneamiento_View.aspx";
            });
            $("#btnCultivos").click(function () {
                window.location.href = "/Pages/frmCultivosView.aspx";
            });
            $("#btnZonaTrabajo").click(function () {
                window.location.href = "/Pages/frmZonaTrabajo_View.aspx";
            });
            $("#btnEtapas").click(function () {
                window.location.href = "/Pages/frmEtapasView.aspx";
            });
            $("#btnActividades").click(function () {
                window.location.href = "/Pages/frmActividades_View.aspx";
            });
            $("#btnPersonal").click(function () {
                window.location.href = "/Pages/frmPersonalB_View.aspx";
            });
            $("#btnInsumos").click(function () {
                window.location.href = "/Pages/frmProductos_View.aspx";
            });
            $("#btnMaquinaria").click(function () {
                window.location.href = "/Pages/frmMaquinas_View.aspx";
            });
            $("#btnProveedores").click(function () {
                window.location.href = "/Pages/frmProveedoresView.aspx";
            });
            $("#btnLineaProductos").click(function () {
                window.location.href = "/Pages/frmLineaProductoView.aspx";
            });

            $("#btnReporteRecursosHa").click(function () {
                window.location.href = "/Pages/frmResumenCostosEnTodosLosCultivosHA.aspx";
            });
            $("#btnReporteZonaTrabajoRecursosResumido").click(function () {
                window.location.href = "/Pages/frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper.aspx";
            });
            $("#btnReporteZonaTrabajoRecursosDetallado").click(function () {
                window.location.href = "/Pages/frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinperDetail.aspx";
            });
            $("#btnReporteZonaTrabajoActividades").click(function () {
                window.location.href = "/Pages/frmReporteZTCostosRecursosEtapasActividades.aspx";
            });
            $("#btnReporteCostosKilo").click(function () {
                window.location.href = "/Pages/frmResumenCostosEnTodosLosCultivosKilo.aspx";
            });
            $("#btnReporteCampanas").click(function () {
                window.location.href = "/Pages/frmResumenCosechaporcampanas.aspx";
            });
            $("#btnReporteSaldosAlmacen").click(function () {
                window.location.href = "/Pages/frmReporteSaldosDeAlmacen.aspx";
            });
            $("#btnReporteKardexStandar").click(function () {
                window.location.href = "/Pages/frmKardexDeProductoStandar.aspx";
            });
        });
    </script>
</head>
<body>
    <div>
        <div class="alert-success jumbotron jumbotron-fluid" style="padding-left: 40px;">
            <h1 class="display-4"><%= Me.sbtnstr12106%></h1>
        </div>
    </div>
    
    <div>
        <div class="alert-success jumbotron jumbotron-fluid" style="padding-left: 30px;">
    
                    <h1 class="display-4"><%= Me.sbtnUsuariostr12000%></h1>
                    <h1 class="display-4"><%= Me.sbtnUsuario%></h1>
        </div>
    </div>

    <div style="padding-bottom: 50px; padding-left: 50px; padding-right: 50px; padding-top: 20px">
        <div class="card mb-3">
            <div class="card-body text-info" style="text-align: right;">
                <form id="form1" runat="server">
                    <button id="btnHelp" type="button" class="btn btn-info menuButton"><%= Me.sbtnhelp%></button>
                    
                        <asp:LoginStatus ID="LoginStatus1" runat="server" CssClass="btn btn-danger menuButton" /></form>
                    
            </div>
        </div>
        <div class="card border-info mb-3">
            <div class="card-header"><%= Me.sbtnIntroducirDatos%></div>
            <div class="card-body text-info">
                <h5 class="card-title"><%= Me.sbtnImportarArchivodeTexto%></h5>
                <button id="btnImport" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnImportar%></button>
                <button id="btnImport2" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnImportar%></button>
                <button id="btnExportExcelMobile" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnExportExcelMobile%></button>
                <h5 class="card-title" style="padding-top: 20px" id="lblInsertarRegistros" runat="server"><%= Me.sbtnIngresarRegistros%></h5>
                <button id="btnIngresoCostos" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnIngresoCostos%></button>
                <button id="btnIngresoAlmacen" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnIngresoAlmacen%></button>
                <button id="btnIngresoCosechas" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnIngresoCosechas%></button>
                <button id="btnPlaneamiento" type="button" runat="server" class="btn btn-primary menuButton"><%= Me.sbtnPlaneamiento%></button>
            </div>
        </div>
        <div runat="server"  class="card border-info mb-3" id="divEstructuraCostos">
            <div class="card-header"><%= Me.sbtnEstructuradeCostos%></div>
            <div class="card-body text-info">
                <h5 class="card-title"><%= Me.sbtnCentrodeCostos%></h5>
                <button id="btnCultivos" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnCultivos%></button>
                <button id="btnZonaTrabajo" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnZonaTrabajo%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.sbtnProcesos%></h5>
                <button id="btnEtapas" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnEtapas%></button>
                <button id="btnActividades" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnActividades%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.sbtnRecursos%></h5>
                <button id="btnPersonal" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnPersonal%></button>
                <button id="btnInsumos" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnInsumos%></button>
                <button id="btnMaquinaria" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnMaquinaria%></button>
                <button id="btnProveedores" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnProveedores%></button>
                <button id="btnLineaProductos" type="button" runat="server" class="btn menuButton menuCostosButton"><%= Me.sbtnLineaProductos%></button>
            </div>
        </div>
        <div runat="server" class="card border-info mb-3" id="divReports">
            <div class="card-header"><%= Me.sbtnReportes%></div>
            <div class="card-body text-info">
                <h5 class="card-title"><%= Me.sbtnReportedeCostos%></h5>
                <button id="btnReporteRecursosHa" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteRecursosHa%></button>
                <button id="btnReporteZonaTrabajoRecursosResumido" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteZonaTrabajoRecursosResumido%></button>
                <button id="btnReporteZonaTrabajoRecursosDetallado" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteZonaTrabajoRecursosDetallado%></button>
                <button id="btnReporteZonaTrabajoActividades" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteZonaTrabajoActividades%></button>
                <button id="btnReporteCostosKilo" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteCostosKilo%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.sbtnReportedeCosechas%></h5>
                <button id="btnReporteCampanas" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteCampanas%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.sbtnReportedeAlmacen%></h5>
                <button id="btnReporteSaldosAlmacen" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteSaldosAlmacen%></button>
                <button id="btnReporteKardexStandar" type="button" runat="server" class="btn menuButton menuReportesButton"><%= Me.sbtnReporteKardexStandar%></button>
            </div>
        </div>
    </div>
</body>
</html>
