﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmProductos.aspx.vb" Inherits="AgrisoftWeb.UI.frmProductos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#costo_unitario_standar').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
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

            if (idText.length != 6) {
                alert(document.getElementById('<%=hdnStr108.ClientID%>').value);
                return false;
            }

            var descripcionText = $.trim($('#descripcion').val());
            if (descripcionText.length == 0) {
                alert(document.getElementById('<%=hdnStr109.ClientID%>').value);
                return false;
            }

            var CostoUnitarioValor = $("#<%=costo_unitario_standar.ClientID%>").val();
            if ($.isNumeric(CostoUnitarioValor) == false) {
                alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                return false;
            }

            if (($('#rbtnInsumos').is(':checked') || $('#rbtnProductos').is(':checked')) == false) {
                alert('Debe Seleccionar el tipo de Producto');
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
                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control form-control-sm" MaxLength="6"></asp:TextBox>
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
                <div class="col-4">
                    <asp:Label ID="lblCostoUnitarioStandar" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="costo_unitario_standar" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblLinea" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboLinea" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboUnidad" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                </div>
                <div class="col">
                    <asp:RadioButton ID="rbtnInsumos" runat="server" GroupName="GroupTipo"  />
                    <asp:RadioButton ID="rbtnProductos" runat="server" GroupName="GroupTipo" />
                </div>
            </div>
            <br />
            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick="return Validar();" CssClass="btn btn-primary btn-sm" />
            <br />
            <div id="dvMessage" runat="server" visible="false">
                <asp:Label ID="lblResult" runat="server" />
            </div>
            <asp:HiddenField ID="hdnStr108" runat="server" />
            <asp:HiddenField ID="hdnStr109" runat="server" />
            <asp:HiddenField ID="hdnStr528" runat="server" />
        </div>
    </form>
</body>
</html>
