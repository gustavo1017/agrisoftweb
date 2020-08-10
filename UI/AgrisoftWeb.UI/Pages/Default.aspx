<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="Default.aspx.vb" Inherits="AgrisoftWeb.UI._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">

    <div>
        <br />
        <h2><%=Resources.Resource1.str12126 %></h2>
        <hr />
        <h4>
            <asp:LoginName ID="LoginName1" runat="server" Font-Bold="false" />
        </h4>
        <br />
        <asp:LoginStatus ID="LoginStatus1" runat="server" CssClass="btn btn-warning" />
    </div>

    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../css/bootstrap.min.css" media="screen" />
</asp:Content>
