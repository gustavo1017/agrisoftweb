<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmSignUp.aspx.vb" Inherits="AgrisoftWeb.UI.frmSignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap.min.css" media="screen" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.8/css/all.css" />
    <style type="text/css">
        .divider-text {
            position: relative;
            text-align: center;
            margin-top: 15px;
            margin-bottom: 15px;
        }
        .divider-text span {
            padding: 7px;
            font-size: 12px;
            position: relative;
            z-index: 2;
        }
        .divider-text:after {
            content: "";
            position: absolute;
            width: 100%;
            border-bottom: 1px solid #ddd;
            top: 55%;
            left: 0;
            z-index: 1;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnSignUp').click(function (e) {
                var isValid = true;
                $('#txtUsername,#txtPassword,#txtPasswordOK,#txtAccessCode,#txtEmail').each(function () {
                    if ($.trim($(this).val()) == '') {
                        isValid = false;
                        $(this).css({ "border": "1px solid red", "background": "#FFCECE" });
                    }
                    else {
                        $(this).css({ "border": "", "background": "" });
                    }
                });

                var userName = $.trim($('#txtUsername').val());
                if (userName.length != 6)
                {
                    alert('El ID de Usuario debe tener 6 caracteres');
                    isValid = false;
                }

                if (isValid == false) {
                    e.preventDefault();
                }
                else {
                    var email = $.trim($('#txtEmail').val());
                    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                    var isValidEmail = regex.test(email);

                    if (!isValidEmail) {
                        alert("Email no válido");
                        return false;
                    }

                    var strPassword = $.trim($('#txtPassword').val());
                    var strPasswordOK = $.trim($('#txtPasswordOK').val());
                    if (strPassword != strPasswordOK) {
                        alert("Las contraseñas no coinciden");
                        return false;
                    }
                }
            });

            $('#txtUsername').keypress(function () {
                var keyCode = event.keyCode || event.which;

                //if (keyCode == 8 || (keyCode >= 35 && keyCode <= 40)) {
                //    return;
                //}
            
                return ((keyCode > 47 && keyCode < 58) || (keyCode > 64 && keyCode < 91) || (keyCode > 96 && keyCode < 123) || keyCode == 0);
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <br />
        <div class="container">
            <div class="card bg-light">
                <article class="card-body mx-auto" style="max-width: 400px; width: 400px">
                    <h4 class="card-title mt-3 text-center"><%=Resources.Resource1.str12122 %></h4>
                    <asp:ImageButton ID="btnFacebook" runat="server" Height="30px" ImageUrl="~/img/facebuttonES.png" Width="176px" Visible="false" /><br />
                    <p class="divider-text">
                        <span class="bg-light">O</span>
                    </p>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-user"></i></span>
                        </div>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12134 %>" MaxLength="6" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-envelope"></i></span>
                        </div>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12115 %>" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-lock"></i></span>
                        </div>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12111 %>" TextMode="Password" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-lock"></i></span>
                        </div>
                        <asp:TextBox ID="txtPasswordOK" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12117 %>" TextMode="Password" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-user"></i></span>
                        </div>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str10403 %>" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-user"></i></span>
                        </div>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12120 %>" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-building"></i></span>
                        </div>
                        <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12121 %>" />
                    </div>
                    <div class="form-group input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-lock"></i></span>
                        </div>
                        <asp:TextBox ID="txtAccessCode" runat="server" CssClass="form-control" placeholder="<%$Resources:Resource1, str12130 %>" />
                    </div>
                    <div id="dvMessage" runat="server" visible="false" class="alert alert-danger">
                        <strong><%=Resources.Resource1.str12107 %></strong>
                        <asp:Label ID="lblMessage" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnSignUp" Text="<%$Resources:Resource1, str12123 %>" runat="server" Class="btn btn-primary btn-block" OnClick="btnSignUp_Click" />
                    </div>
                </article>
            </div>
        </div>
    </form>
</body>
</html>
