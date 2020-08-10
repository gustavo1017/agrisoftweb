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
            <h1 class="display-4"><%= Me.btnstr12106%></h1>
        </div>
    </div>
    
    <div>
        <div class="alert-success jumbotron jumbotron-fluid" style="padding-left: 30px;">
    
                    <h1 class="display-4"><%= Me.btnUsuariostr12000%></h1>
                    <h1 class="display-4"><%= Me.btnUsuario%></h1>
        </div>
    </div>

    <div style="padding-bottom: 50px; padding-left: 50px; padding-right: 50px; padding-top: 20px">
        <div class="card mb-3">
            <div class="card-body text-info" style="text-align: right;">
                <form id="form1" runat="server">
                    <button id="btnHelp" type="button" class="btn btn-info menuButton"><%= Me.btnhelp%></button>
                    <asp:LoginStatus ID="LoginStatus1" runat="server" CssClass="btn btn-danger menuButton" /><%= Me.btnLogout%> &nbsp;</form>
            </div>
        </div>
        <div class="card border-info mb-3">
            <div class="card-header"><%= Me.btnIntroducirDatos%></div>
            <div class="card-body text-info">
                <h5 class="card-title"><%= Me.btnImportarArchivodeTexto%></h5>
                <button id="btnImport" type="button" class="btn btn-primary menuButton"><%= Me.btnImportar%></button>
                <button id="btnExportExcelMobile" type="button" class="btn btn-primary menuButton"><%= Me.btnExportExcelMobile%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.btnIngresarRegistros%></h5>
                <button id="btnIngresoCostos" type="button" class="btn btn-primary menuButton"><%= Me.btnIngresoCostos%></button>
                <button id="btnIngresoAlmacen" type="button" class="btn btn-primary menuButton"><%= Me.btnIngresoAlmacen%></button>
                <button id="btnIngresoCosechas" type="button" class="btn btn-primary menuButton"><%= Me.btnIngresoCosechas%></button>
                <button id="btnPlaneamiento" type="button" class="btn btn-primary menuButton"><%= Me.btnPlaneamiento%></button>
            </div>
        </div>
        <div class="card border-info mb-3">
            <div class="card-header"><%= Me.btnEstructuradeCostos%></div>
            <div class="card-body text-info">
                <h5 class="card-title"><%= Me.btnCentrodeCostos%></h5>
                <button id="btnCultivos" type="button" class="btn menuButton menuCostosButton"><%= Me.btnCultivos%></button>
                <button id="btnZonaTrabajo" type="button" class="btn menuButton menuCostosButton"><%= Me.btnZonaTrabajo%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.btnProcesos%></h5>
                <button id="btnEtapas" type="button" class="btn menuButton menuCostosButton"><%= Me.btnEtapas%></button>
                <button id="btnActividades" type="button" class="btn menuButton menuCostosButton"><%= Me.btnActividades%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.btnRecursos%></h5>
                <button id="btnPersonal" type="button" class="btn menuButton menuCostosButton"><%= Me.btnPersonal%></button>
                <button id="btnInsumos" type="button" class="btn menuButton menuCostosButton"><%= Me.btnInsumos%></button>
                <button id="btnMaquinaria" type="button" class="btn menuButton menuCostosButton"><%= Me.btnMaquinaria%></button>
                <button id="btnProveedores" type="button" class="btn menuButton menuCostosButton"><%= Me.btnProveedores%></button>
                <button id="btnLineaProductos" type="button" class="btn menuButton menuCostosButton"><%= Me.btnLineaProductos%></button>
            </div>
        </div>
        <div class="card border-info mb-3">
            <div class="card-header"><%= Me.btnReportes%></div>
            <div class="card-body text-info">
                <h5 class="card-title"><%= Me.btnReportedeCostos%></h5>
                <button id="btnReporteRecursosHa" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteRecursosHa%></button>
                <button id="btnReporteZonaTrabajoRecursosResumido" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteZonaTrabajoRecursosResumido%></button>
                <button id="btnReporteZonaTrabajoRecursosDetallado" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteZonaTrabajoRecursosDetallado%></button>
                <button id="btnReporteZonaTrabajoActividades" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteZonaTrabajoActividades%></button>
                <button id="btnReporteCostosKilo" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteCostosKilo%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.btnReportedeCosechas%></h5>
                <button id="btnReporteCampanas" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteCampanas%></button>
                <h5 class="card-title" style="padding-top: 20px"><%= Me.btnReportedeAlmacen%></h5>
                <button id="btnReporteSaldosAlmacen" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteSaldosAlmacen%></button>
                <button id="btnReporteKardexStandar" type="button" class="btn menuButton menuReportesButton"><%= Me.btnReporteKardexStandar%></button>
            </div>
        </div>
    </div>
</body>
</html>
