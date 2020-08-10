<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmResumenCostosEnTodosLosCultivosHA.aspx.vb" Inherits="AgrisoftWeb.UI.frmResumenCostosEnTodosLosCultivosHA" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PagesContent" runat="server">
    <div>
         <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Resumen de Costos por Recursos"></asp:Label></b>
            </div>
        </div>
    </div>


        <asp:Panel ID="Frame1" runat="server" GroupingText="Parametros">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row" style="padding-bottom: 8px">
                        <div style="padding-left: 30px; width: 100px;">
                            <asp:RadioButton ID="rbtnCampañaFilter" runat="server" Text="Campaña" GroupName="SearchFilter" AutoPostBack="true" OnCheckedChanged="rbtnCampañaFilter_CheckedChanged" CssClass="form-check-input form-control-sm align-top" />
                        </div>
                        <div class="col-1">
                            <asp:TextBox ID="edtNumeroCampana" runat="server" CssClass="form-control form-control-sm text-right" Width="95px"></asp:TextBox>
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
                            <asp:TextBox ID="dtfechamax" runat="server" CssClass="dateTo form-control form-control-sm" Width="95px"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rbtnCampañaFilter" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rbtnFechaDesdeFilter" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <div class="form-row">
            <div class="col-sm-2"></div>
            <div class="col-sm-auto" style="padding-left:25px">
                <asp:Button ID="btnVer" runat="server" Text="" OnClick="btnVer_Click" OnClientClick="return validateSearch();" CssClass="btn btn-primary btn-sm" Width="95px" />
            </div>
            <div style="display: none">
                <asp:Button ID="btnConfigurar" runat="server" Text="Configurar" OnClick="btnConfigurar_Click" />
                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
                <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar" />
            </div>
            <div class="col-sm-2">
                <input id="btnOrdenar" type="button" value="Cultivos" class="btn btn-primary btn-sm" style="width: 95px" />
            </div>
        </div>
        <br />
        <asp:DropDownList ID="cboZonatrabajo" runat="server" Visible="false"></asp:DropDownList>
        <div id="containerReport" style="position: relative; z-index: 0;">
            <CR:CrystalReportViewer ID="crvCostosEnTodosLosCultivos" runat="server" AutoDataBind="true" Width="100%" EnableDatabaseLogonPrompt="false" />
        </div>
        <asp:HiddenField ID="hdnStr6018" runat="server" />
        <asp:HiddenField ID="hdnStr6013" runat="server" />
        <asp:HiddenField ID="hdnDateFrom" runat="server" />
        <asp:HiddenField ID="hdnDateTo" runat="server" />
    </div>

    <%--<script type="text/javascript" src="Scripts/jquery-3.3.1.min.js"></script>--%>
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
                document.getElementById('<%=hdnDateFrom.ClientID%>').value = $(".date").val();
                document.getElementById('<%=hdnDateTo.ClientID%>').value = $(".dateTo").val();
            }
        });

        // On page load
        $(function () {
            setDatePickerControls();
            document.getElementById('<%=hdnDateFrom.ClientID%>').value = $(".date").val();
            document.getElementById('<%=hdnDateTo.ClientID%>').value = $(".dateTo").val();
        });

        //function setDatePickerControls() {
        //    var dateCurrent = new Date();
        //    var dateFrom = new Date();
        //    dateFrom.setDate(dateCurrent.getDate() - 30);

        //    $(".date").datepicker({ dateFormat: 'dd/mm/yy' });
        //    $(".dateTo").datepicker({ dateFormat: 'dd/mm/yy' });

        //    if ($(".date").val() == "") {
        //        $(".date").datepicker('setDate', dateFrom);
        //    }
        //    if ($(".dateTo").val() == "") {
        //        $(".dateTo").datepicker('setDate', new Date());
        //    }
        //}
        function setDatePickerControls() {
            var dateCurrent = new Date();
            var dateFrom = new Date();
            dateFrom.setDate(dateCurrent.getDate() - 30);

            // $(".date").datepicker({ dateFormat: 'dd/mm/yy' }).attr({'readonly': true, 'background-color': 'white'});

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
            var modalPage = "frmCultivos_ordenar.aspx";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 400,
                width: 400,
                position: ['center', 20],
                title: "Titulo"
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

            //validate date inputs
            var datevalueFrom = $(".date").val();
            var datevalueTo = $(".dateTo").val();

            var isValidFrom = isValidDate(datevalueFrom);
            var isValidTo = isValidDate(datevalueTo);

            if (isValidFrom == false || isValidTo == false) {
                alert(Resource1.str99992);
                document.getElementById('containerReport').innerHTML = "";
                $("#containerReport").hide();
                return false;
            }
        }

        function isValidDate(dateString) {
            //var regEx = /^\d{2}-\d{2}-\d{4}$/;
            var regEx = /^\d\d?\/\d\d?\/\d\d\d\d$/;
            if (!dateString.match(regEx)) return false;  // Invalid format

            var arrayDate = dateString.split("/");
            var d = new Date(arrayDate[2], arrayDate[1] - 1, arrayDate[0]);
            if (Number.isNaN(d.getTime())) return false; // Invalid date
            var strDate = arrayDate[2] + "-" + arrayDate[1] + "-" + arrayDate[0];
            return d.toISOString().slice(0, 10) === strDate;
        }

        function ShowErrorMessage(messageText) {
            alert(messageText);
        }
    </script>
</asp:Content>

