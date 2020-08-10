<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmKardexDeProductoStandar.aspx.vb" Inherits="AgrisoftWeb.UI.frmKardexDeProductoStandar" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Kardex de Almacen"></asp:Label></b>
            </div>
        </div>
    </div>

    <div class="form-row" id="FrameSearch" title="Parametros" runat="server" style="padding-top:10px">
        <div class="col-0">
            <asp:Label ID="lblcultivo" runat="server" Text="Insumos: " CssClass="col-form-label-sm"></asp:Label>
        </div>
        <div class="col-3">
            <asp:DropDownList ID="cboCultivo" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
        </div>
        <div class="col-sm-3">
            <asp:Button ID="btnVer" runat="server" Text="" OnClick="btnVer_Click" OnClientClick="return validateSearch();" CssClass="btn btn-primary btn-sm" />
        </div>
    </div>
    <div class="form-row" id="Div1" title="Parametros" runat="server" style="padding-top:8px">
        <div class="col-0">
            <asp:Label ID="lblFecha" runat="server" Text="Fec. Ini: " CssClass="col-form-label-sm"></asp:Label>
        </div>
        <div class="col-0">
            <asp:TextBox ID="dtfechamin" runat="server" CssClass="date form-control form-control-sm" Width="90px"></asp:TextBox>
        </div>
        <div class="col-0">
            <asp:Label ID="lblHasta" runat="server" Text="Fec. Fin: " CssClass="col-form-label-sm"></asp:Label>
        </div>
        <div class="col-0">
            <asp:TextBox ID="dtfechamax" runat="server" CssClass="dateTo form-control form-control-sm" Width="90px"></asp:TextBox>
        </div>
    </div>
    <div id="containerReport" class="form-row">
        <CR:CrystalReportViewer ID="crvKardexProductoStandar" runat="server" AutoDataBind="true" Width="100%" EnableDatabaseLogonPrompt="false" />
    </div>
    <asp:HiddenField ID="hdnStr6018" runat="server" />
    <asp:HiddenField ID="hdnStr6013" runat="server" />
    <asp:HiddenField ID="hdnDateFrom" runat="server" />
    <asp:HiddenField ID="hdnDateTo" runat="server" />

    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        // On page load
        $(function () {
            setDatePickerControls();
            document.getElementById('<%=hdnDateFrom.ClientID%>').value = $(".date").val();
            document.getElementById('<%=hdnDateTo.ClientID%>').value = $(".dateTo").val();
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
     
        function validateSearch() {
            //validate date inputs
            var datevalueFrom = $(".date").val();
            var datevalueTo = $(".todate").val();

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
