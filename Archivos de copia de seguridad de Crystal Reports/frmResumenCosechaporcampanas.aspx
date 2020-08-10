<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmResumenCosechaporcampanas.aspx.vb" Inherits="AgrisoftWeb.UI.frmResumenCosechaporcampanas" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PagesContent" runat="server">
    <div>
  <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Resumen Comparativo de Cosechas"></asp:Label></b>
            </div>
        </div>
    </div>

        <div class="form-row" style="padding-bottom: 15px; padding-top:10px">
            <div class="col-sm-1" style="text-align: center">
                <span class="col-form-label-sm"><%=Resources.Resource1.str541 %> 1</span>
            </div>
            <div class="col-1">
                <asp:TextBox ID="text1" runat="server" CssClass="form-control form-control-sm text-right" Width="95px"></asp:TextBox>
            </div>
            <div class="col-0"></div>
            <div class="col-sm-1" style="text-align: center">
                <span class="col-form-label-sm"><%=Resources.Resource1.str541 %> 2</span>
            </div>
            <div class="col-1">
                <asp:TextBox ID="text2" runat="server" CssClass="form-control form-control-sm text-right" Width="95px"></asp:TextBox>
            </div>
            <div class="col-0"></div>
            <div class="col-sm-1" style="text-align: center">
                <span class="col-form-label-sm"><%=Resources.Resource1.str541 %> 3</span>
            </div>
            <div class="col-1">
                <asp:TextBox ID="text3" runat="server" CssClass="form-control form-control-sm text-right" Width="95px"></asp:TextBox>
            </div>
            <div class="col-2"></div>
            <div class="col-sm-auto" style="padding-left: 25px">
                <asp:Button ID="btnVer" runat="server" Text="" OnClick="btnVer_Click" OnClientClick="return validateSearch();" CssClass="btn btn-primary btn-sm" Width="95px" />
            </div>
        </div>
        <div id="containerReport" style="position: relative; z-index: 0;">
            <CR:CrystalReportViewer ID="crvCostoTotalEnUnaZonaDeTrabajoPorRubros" runat="server" AutoDataBind="true" Width="100%" EnableDatabaseLogonPrompt="false" />
        </div>
        <asp:HiddenField ID="hdnStr6018" runat="server" />
        <asp:HiddenField ID="hdnStr6013" runat="server" />
        <asp:HiddenField ID="hdnDateFrom" runat="server" />
        <asp:HiddenField ID="hdnDateTo" runat="server" />
    </div>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=text1.ClientID %>').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#<%=text2.ClientID %>').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#<%=text3.ClientID %>').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });
        });
    </script>
</asp:Content>
