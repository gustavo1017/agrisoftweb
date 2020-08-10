<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmLineaProducto.aspx.vb" Inherits="AgrisoftWeb.UI.frmLineaProducto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtId').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = ".,·()º'¡`'+´´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });
            $('#descripcion').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = ".,·()º'¡`'+´´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });
        });

        function Validar() {
            var idText = $.trim($('#txtId').val());
            if (idText.length != 3) {
                alert(document.getElementById('<%=hdnStr5008.ClientID%>').value);
                return false;
            }

            var descripcionText = $.trim($('#descripcion').val());
            if (descripcionText.length == 0) {
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: small">
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblCodigo" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control form-control-sm" MaxLength="3"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblDescripcion" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="descripcion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <asp:Label ID="Label1" runat="server" Text="Componente Cuenta" Visible="false"></asp:Label>
                <asp:TextBox ID="txtComponente" runat="server" Visible="false" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <br />
            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick="return Validar();" CssClass="btn btn-primary btn-sm" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" Visible="false" />
            <br />
            <div id="dvMessage" runat="server" visible="false" class="alert alert-success">
                <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
            </div>
            <asp:HiddenField ID="hdnStr5008" runat="server" />
        </div>
    </form>
</body>
</html>
