<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SessionExpired.aspx.vb" Inherits="AgrisoftWeb.UI.SessionExpired" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Resources.Resource1.str12128 %></title>
    <link type="text/css" rel="stylesheet" href="../css/401.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="notfound">
    <h1><%=Resources.Resource1.str12128 %></h1>
        <p>
            Su sesión ha expirado por tiempo de espera (Tiempo máximo de espera: 3 minutos).<br />
            Por favor retorne a la página de <a href="frmLoginServer.aspx">login</a> para acceder a su cuenta nuevamente. 
        </p>
    </div>
    </form>
</body>
</html>
