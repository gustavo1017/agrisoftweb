<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmActividades.aspx.vb" Inherits="AgrisoftWeb.UI.frmActividades" %>

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
                var invalidChars = ".,·()º'¡`'+´´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}'[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });

            $('#txtId').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = ".,·()º'¡`'+´´|!ª$%&/=?¿Ç*^¨'Ññ;:_\@#~€¬]}[{¨¨°-\\";
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
            if (idText.length == 6) {
                return true;
            }
            else {
                alert(document.getElementById('<%=hdnStr208.ClientID%>').value);
                return false;
            }

            var descripcionText = $.trim($('#descripcion').val());
            if (descripcionText.length == 0) {
                alert(document.getElementById('<%=hdnStr209.ClientID%>').value);
                return false;
            }

           <%-- var hectareasValor = $("#<%=hectareas.ClientID%>").val();
            if ($.isNumeric(campanaValor) == false) {
                alert(document.getElementById('<%=hdnStr312.ClientID%>').value);
                return false;
            }--%>

            return true;
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
                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control form-control-sm" MaxLength="6"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblDescripcion" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="descripcion" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboTipo" runat="server" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblEtapa" runat="server" Text="Etapa"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboEtapa" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <br />
            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick="return Validar();" CssClass="btn btn-primary btn-sm" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" Visible="false" />
            <br />
            <div id="dvMessage" runat="server" visible="false">
                <asp:Label ID="lblResult" runat="server" />
            </div>
            <asp:HiddenField ID="hdnStr208" runat="server" />
            <asp:HiddenField ID="hdnStr209" runat="server" />
            <asp:HiddenField ID="hdnStr312" runat="server" />
        </div>
    </form>
</body>
</html>
