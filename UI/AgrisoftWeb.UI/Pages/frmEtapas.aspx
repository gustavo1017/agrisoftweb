<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEtapas.aspx.vb" Inherits="AgrisoftWeb.UI.frmEtapas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
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
        });

        function Validar() {
            var idText = $.trim($('#txtId').val());
            idText = idText.replace('', '');

            if (idText.length != 3) {
                alert(document.getElementById('<%=hdnStr108.ClientID%>').value);
                return false;
            }

            var descripcionText = $.trim($('#descripcion').val());
            if (descripcionText.length == 0) {
                alert(document.getElementById('<%=hdnStr109.ClientID%>').value);
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
                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control form-control-sm" MaxLength="3" Width="50px"></asp:TextBox>
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
            <br />
            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick="return Validar();" CssClass="btn btn-primary btn-sm" />
            <br />&nbsp;
            <div id="dvMessage" runat="server" visible="false" style="padding-top:10px">
                <asp:Label ID="lblResult" runat="server" />
            </div>
            <asp:HiddenField ID="hdnStr108" runat="server" />
            <asp:HiddenField ID="hdnStr109" runat="server" />
            <asp:HiddenField ID="hdnStr528" runat="server" />
        </div>
    </form>
</body>
</html>
