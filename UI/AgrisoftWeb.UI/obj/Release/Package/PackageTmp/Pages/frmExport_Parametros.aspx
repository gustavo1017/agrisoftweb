<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmExport_Parametros.aspx.vb" Inherits="AgrisoftWeb.UI.frmExport_Parametros" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
    <div id="containerReport" style="position:relative; z-index:0;">
        <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" Width="100%" EnableDatabaseLogonPrompt="false" />
    </div>
</asp:Content>
