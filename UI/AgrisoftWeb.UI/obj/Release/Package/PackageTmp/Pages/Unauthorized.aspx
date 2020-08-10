<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="Unauthorized.aspx.vb" Inherits="AgrisoftWeb.UI.Unauthorized" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
    <br />
    <br />
    <div id="notfound">
		<div class="notfound">
			<h2>401 - No tienes acceso a esta página</h2>
			<p>Por favor contacta con tu administrador acerca de tus permisos.</p>
			<a href="Default.aspx">Ir a la página de inicio</a>
		</div>
	</div>

    <link type="text/css" rel="stylesheet" href="../css/401.css" />
</asp:Content>
