<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmReporteSaldosDeAlmacen.aspx.vb" Inherits="AgrisoftWeb.UI.frmReporteSaldosDeAlmacen" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Saldos de Almacen"></asp:Label></b>
            </div>
        </div>
    </div>

    <div class="form-row" id="FrameSearch" title="Parametros" runat="server">
        <div class="col-0 border-primary">
            <asp:Label ID="lblFecha" runat="server" Text="Fecha: "></asp:Label>
        </div>
        <div class="col-2">
            <asp:TextBox ID="dtfechamin" runat="server" CssClass="date form-control form-control-sm" Width="90px"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:Button ID="btnVer" runat="server" Text="" OnClick="btnVer_Click" OnClientClick="return validateSearch();" CssClass="btn btn-primary btn-sm" />
        </div>
        <div style="display: none">
            <asp:Button ID="btnConfigurar" runat="server" Text="Configurar" />
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar" />
        </div>
    </div>
    <div id="containerReport" class="form-row" style="position:relative; z-index:0;">
        <CR:CrystalReportViewer ID="crvCostosEnTodosLosCultivos" runat="server" AutoDataBind="true" Width="100%" EnableDatabaseLogonPrompt="false" />
    </div>
    <asp:HiddenField ID="hdnStr6018" runat="server" />
    <asp:HiddenField ID="hdnStr6013" runat="server" />
    <asp:HiddenField ID="hdnDateFrom" runat="server" />

    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        // On page load
        $(function () {
            setDatePickerControls();
            document.getElementById('<%=hdnDateFrom.ClientID%>').value = $(".date").val();
        });



        function setDatePickerControls() {
            var dateCurrent = new Date();
            var dateFrom = new Date();
            //dateFrom.setDate(dateCurrent.getDate() - 30);

            // $(".date").datepicker({ dateFormat: 'dd/mm/yy' }).attr({'readonly': true, 'background-color': 'white'});

            $(".date").datepicker({ dateFormat: 'dd/mm/yy' }).attr({ 'readonly': true, 'background-color': 'white' });
            //$(".dateTo").datepicker({ dateFormat: 'dd/mm/yy' }).attr({ 'readonly': true, 'background-color': 'white' });

            if ($(".date").val() == "") {
                $(".date").datepicker('setDate', dateFrom);
            }
            //if ($(".dateTo").val() == "") {
            //    $(".dateTo").datepicker('setDate', new Date());
            }
        }
        //function setDatePickerControls() {
        //    var dateCurrent = new Date();
        //    var dateFrom = new Date();
        //    dateFrom.setDate(dateCurrent.getDate() - 30);

        //    $(".date").datepicker({ dateFormat: 'dd/mm/yy' });

        //    if ($(".date").val() == "") {
        //        $(".date").datepicker('setDate', dateFrom);
        //    }
        //}

        function validateSearch() {
            //validate date inputs
            var datevalueFrom = $(".date").val();
            var isValidFrom = isValidDate(datevalueFrom);

            if (isValidFrom == false) {
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
