<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewPassword.aspx.vb" Inherits="AgrisoftWeb.UI.NewPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap.min.css" media="screen" />
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnReset").click(function (e) {
                var newPWdValue = $("#NewPwd").val();
                var confirmPwdValue = $("#ConfirmNewPwd").val();

                if (newPWdValue == '' || confirmPwdValue == '') {
                    alert('Los campos son obligatorios');
                    return;
                }

                if (newPWdValue != confirmPwdValue) {
                    alert('Los valores no coinciden');
                    return;
                }

                if ($("#hdnToken").val() == '')
                {
                    alert('Debe ingresar desde la url que recibió en su email.');
                    return;
                }

                var DTO = {
                    'newPassword': newPWdValue,
                    'token': $("#hdnToken").val()
                };

                $.ajax({
                    type: "POST",
                    url: 'NewPassword.aspx/UpdatePassword',
                    data: JSON.stringify(DTO),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //async: true,
                    success: function (resultado) {
                        $("#btnReset").attr("disabled", true);
                        alert('Su Password ha sido actualizado correctamente');
                        window.location.href = "/Pages/frmLoginServer.aspx";
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
                    <h3 class="mb-0">Resetear Password</h3>
                </div>
                <div class="card-body">
                    <form class="form" role="form" autocomplete="off">
                        <div class="form-group row">
                            <label class="col-lg-3 col-form-label form-control-label">Nuevo Password</label>
                            <div class="col-lg-9">
                                <input class="form-control" type="password" id="NewPwd" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-lg-3 col-form-label form-control-label">Confirmar Password</label>
                            <div class="col-lg-9">
                                <input class="form-control" type="password" id="ConfirmNewPwd" />
                            </div>
                        </div>
                        <div class="form-group ">
                            <button id="btnReset" type="button" class="btn btn-success btn-lg float-right">Reset</button>
                        </div>
                    </form>
                </div>
                <form runat="server">
                    <asp:HiddenField ID="hdnToken" runat="server" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
