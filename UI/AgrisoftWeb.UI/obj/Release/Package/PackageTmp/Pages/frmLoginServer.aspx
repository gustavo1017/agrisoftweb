<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmLoginServer.aspx.vb" Inherits="AgrisoftWeb.UI.frmLoginServer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../css/bootstrap.min.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="max-width: 400px; margin: auto; padding: 30px; padding-top: 50px">
            <div class="form-row" style="display:none" >
                <div class="col"></div>
                <div class="col" style="text-align:right">
                    <a href="https://tawk.to/chat/5e7fec3c35bcbb0c9aab7670/default">Chat</a>
                    <img src="../img/supportimage.png" />
                </div>                
            </div>
            <div class="form-row" style="padding-bottom: 20px">
                <div class="col-sm-4">
                    <img src="../img/ctmlogo1.png" />
                </div>
                <div class="col-sm-8">
                    <div class="float-right">
                        <asp:DropDownList ID="ddlLanguage" runat="server" Visible="true" CssClass="form-control btn-sm" Width="105px" AutoPostBack="true" Height="35px">
                            <asp:ListItem Text="Español" Value="es-ES"></asp:ListItem>
                            <asp:ListItem Text="Ingles" Value="en-US"></asp:ListItem>
                            <asp:ListItem Text="Portugues" Value="pt-BR"></asp:ListItem>
                            <asp:ListItem Text="Quechua" Value="quz-PE"></asp:ListItem>
                            <asp:ListItem Text="Frances" Value="fr-FR"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div style="padding-bottom: 15px">
                <h2 class="form-signin-heading"><%=Resources.Resource1.str12106 %></h2>
                <h6 class="form-signin-heading"><%=Resources.Resource1.str12132 %></h6>
                <h7 class="form-signin-heading"><%=Resources.Resource1.str99999930 %></h7>
            </div>
            <label for="txtUsername"><%=Resources.Resource1.str12000 %></label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12110 %>" />
            <br />
            <label for="txtPassword"><%=Resources.Resource1.str11007 %></label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="<%$Resources:Resource1, str12111 %>" />
            <div class="checkbox" style="padding-top: 10px">
                <asp:CheckBox ID="chkRememberMe" Text="<%$Resources:Resource1, str12112 %>" runat="server" />
            </div>
            <asp:Button ID="btnLogin" Text="<%$Resources:Resource1, str12113 %>" runat="server" OnClick="btnLogin_Click" Class="btn btn-primary btn-sm" />
            <br />
            <br />
            <div id="dvMessage" runat="server" visible="false" class="alert alert-danger">
                <strong><%=Resources.Resource1.str12107 %></strong>
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <asp:ImageButton ID="btnFacebook" visible="false"  runat="server" Height="30px" ImageUrl="~/img/facebuttonES.png" Width="176px" />
            <br />
            <label for="txtNoAccount"><%=Resources.Resource1.str12108 %></label> <a href="frmSignUp.aspx"><%=Resources.Resource1.str12109 %></a>
            <br />
            <a href="ResetPassword.aspx">Olvidaste tu contraseña?</a>
        </div>
    </form>
    <!--Start of Tawk.to Script-->
<script type="text/javascript">
var Tawk_API=Tawk_API||{}, Tawk_LoadStart=new Date();
(function(){
var s1=document.createElement("script"),s0=document.getElementsByTagName("script")[0];
s1.async=true;
s1.src='https://embed.tawk.to/5e800ce269e9320caabe12c7/default';
s1.charset='UTF-8';
s1.setAttribute('crossorigin','*');
s0.parentNode.insertBefore(s1,s0);
})();
</script>
<!--End of Tawk.to Script-->
</body>
</html>
