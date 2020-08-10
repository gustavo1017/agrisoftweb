<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResetPassword.aspx.vb" Inherits="AgrisoftWeb.UI.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap.min.css" media="screen" />
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnReset").click(function (e) {
                var content = $("#inputResetPasswordEmail").val();

                if (content == '') {
                    alert('Debe rellenar este campo');
                    return;
                }

                var email = $.trim($('#inputResetPasswordEmail').val());
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                var isValidEmail = regex.test(email);

                if (!isValidEmail) {
                    alert("Email no válido");
                    return false;
                }

                var DTO = { 'email': content };
                $.ajax({
                    type: "POST",
                    url: 'ResetPassword.aspx/RequestResetPassword',
                    data: JSON.stringify(DTO),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //async: true,
                    success: function (resultado) {
                        //alert(resultado.d);
                        if (resultado.d == 'EmailNotExists')
                        {
                            alert('No existe usuario con ese email');
                            $("#btnReset").attr("disabled", false);
                        }
                        else {
                            alert('Se envió las instrucciones a su correo electrónico');
                            $("#btnReset").attr("disabled", true);
                        }
                    },
                    error: function (resultado) {
                        alert('Error');
                    }
                });
            });
        });
    </script>
</head>
<body>
    <br />
    <br />
    <br />
    <div class="row">
        <div class="col-md-6 offset-md-3">
            <div class="card card-outline-secondary">
                <div class="card-header">
                    <h3 class="mb-0">Password Reset</h3>
                </div>
                <div class="card-body">
                    <form class="form" role="form" autocomplete="off">
                        <div class="form-group">
                            <label for="inputResetPasswordEmail">Email</label>
                            <input type="email" class="form-control" id="inputResetPasswordEmail" required="" />
                            <br />
                            <span id="helpResetPasswordEmail" class="form-text small text-muted">
                                Este correo debe ser el mismo que al momento de la creación del usuario.<br />
                                Le llegará un email con el link para restablecer su contraseña.<br />
                                Revisar en su bandeja de correo No deseado.
                            </span>
                        </div>
                        <div class="form-group">
                            <button id="btnReset" type="button" class="btn btn-success btn-lg float-right">Reset</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
