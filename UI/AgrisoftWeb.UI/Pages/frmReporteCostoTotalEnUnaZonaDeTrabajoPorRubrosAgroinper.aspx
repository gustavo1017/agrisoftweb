<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper.aspx.vb" Inherits="AgrisoftWeb.UI.frmReporteCostoTotalEnUnaZonaDeTrabajoPorRubrosAgroinper" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Reporte de Costos por Recursos Resumido"></asp:Label></b>
            </div>
        </div>
    </div>

    <div>
        <asp:Panel ID="Frame1" runat="server" GroupingText="Parametros">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row" style="padding-bottom: 8px">
                        <div style="padding-left: 10px; width: 150px;">
                            <asp:Label ID="lblZonaTrabajo" runat="server" CssClass="col-form-label-sm"></asp:Label>
                        </div>
                        <div class="col-3">
                            <asp:DropDownList ID="cboZonatrabajo" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-row" style="padding-bottom: 8px">
                        <div style="padding-left: 30px; width: 100px;">
                            <asp:RadioButton ID="rbtnCampañaFilter" runat="server" Text="Campaña" GroupName="SearchFilter" AutoPostBack="true" OnCheckedChanged="rbtnCampañaFilter_CheckedChanged" CssClass="form-check-input form-control-sm align-top" />
                        </div>
                        <div class="col-1">
                            <asp:TextBox ID="edtNumeroCampana" runat="server" CssClass="form-control form-control-sm text-right" Width="90px"></asp:TextBox>
                        </div>
                        <div class="col-0"></div>
                        <div class="col-sm-1" style="text-align: center">
                            <asp:Label ID="lblMoneda" runat="server" Text="Moneda" CssClass="col-form-label-sm"></asp:Label>
                        </div>
                        <div class="col-1">
                            <asp:DropDownList ID="cboMoneda" runat="server" CssClass="form-control form-control-sm" Width="105px"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-row" style="padding-bottom: 8px">
                        <div style="padding-left: 30px; width: 100px;">
                            <asp:RadioButton ID="rbtnFechaDesdeFilter" runat="server" Text="Desde" GroupName="SearchFilter" AutoPostBack="true" OnCheckedChanged="rbtnFechaDesdeFilter_CheckedChanged" CssClass="form-check-input form-control-sm" />
                        </div>
                        <div class="col-1">
                            <asp:TextBox ID="dtfechamin" runat="server" CssClass="date form-control form-control-sm" Width="95px"></asp:TextBox>
                        </div>
                        <div class="col-0"></div>
                        <div class="col-sm-1" style="text-align: center">
                            <asp:Label ID="lblHasta" runat="server" Text="Hasta" CssClass="col-form-label-sm"></asp:Label>
                        </div>
                        <div class="col-2">
                            <asp:TextBox ID="dtfechamax" runat="server" CssClass="dateTo form-control form-control-sm" Width="100px"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rbtnCampañaFilter" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rbtnFechaDesdeFilter" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <div class="form-row" style="padding-top: 8px">
            <div class="col-sm-2"></div>
            <div class="col-sm-auto" style="padding-left: 25px">
                <asp:Button ID="btnVer" runat="server" Text="" OnClientClick="return validateSearch();" CssClass="btn btn-primary btn-sm" Width="95px" />
            </div>
            <div style="display: none">
                <asp:Button ID="btnConfigurar" runat="server" Text="Configurar" />
                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
                <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar" />
            </div>
            <div class="col-sm-2">
                <input id="btnOrdenar" type="button" value="Actividades" class="btn btn-primary btn-sm" style="width: 95px" />
            </div>
        </div>
        <div id="containerReport" style="position: relative; z-index: 0;">
            <CR:CrystalReportViewer ID="crvCostoTotalEnUnaZonaDeTrabajoPorRubros" runat="server" AutoDataBind="true" />
        </div>
        <asp:HiddenField ID="hdnStr6018" runat="server" />
        <asp:HiddenField ID="hdnStr6013" runat="server" />
    </div>

    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>

    <script type="text/javascript">
        // On update panel refresh
        $(document).ready(function () {
            $('#btnOrdenar').click(function () {
                showOrder();
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                setDatePickerControls();
            }
        });

        // On page load
        $(function () {
            setDatePickerControls();
        });

        function setDatePickerControls() {
            var dateCurrent = new Date();
            var dateFrom = new Date();
            dateFrom.setDate(dateCurrent.getDate() - 30);

            $(".date").datepicker({ dateFormat: 'dd/mm/yy' }).attr({ 'readonly': true, 'background-color': 'white' });
            $(".dateTo").datepicker({ dateFormat: 'dd/mm/yy' }).attr({ 'readonly': true, 'background-color': 'white' });

            if ($(".date").val() == "") {
                $(".date").datepicker('setDate', dateFrom);
            }
            if ($(".dateTo").val() == "") {
                $(".dateTo").datepicker('setDate', new Date());
            }
        }

        function showOrder() {
            var modalPage = "frmActividades_ordenar.aspx";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 400,
                width: 400,
                title: "Titulo",
                position: ['center', 20]
            });
            $dialog.dialog('open');
        }

        function validateSearch() {
            if ($('input[id*=rbtnCampañaFilter]').is(":checked")) {
                var campanaValor = $("#<%=edtNumeroCampana.ClientID%>").val()
                    if (campanaValor == "") {
                        alert(document.getElementById('<%=hdnStr6018.ClientID%>').value);
                    return false;
                }
                else {
                    if ($.isNumeric(campanaValor) == false) {
                        alert(document.getElementById('<%=hdnStr6013.ClientID%>').value);
                        return false;
                    }
                    return true;
                }
            }
        }
    </script>
</asp:Content>
