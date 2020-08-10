<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmPersonalB.aspx.vb" Inherits="AgrisoftWeb.UI.frmPersonalB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <%--<script src="../Scripts/jquery-ui.min.js"></script>--%>
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

            $('#txtCosto_Conta').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,·()º'¡`+´´|'!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });

            $('#costo_unitario_standar').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,·()º'¡`+´'´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }

                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#txtHoraES').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,·()º'¡`+´´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });

            $('#txtHoraED').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,·()º'¡`+´´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });

            $('#nombre').keypress(function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = ".,·()º'¡`'+´´|!ª$%&/=?¿'Ç*^¨Ññ;:_\@#~€¬]}[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });

            //$('#txtCosto_Conta').mask("9.9999");
            //$.each($('#txtCosto_Conta'), function (iEmt, emt) {
            //    $(emt).mask("##0.0000")
            //});
        });

        function Validar() {
            var idText = $.trim($('#txtId').val());
            if (idText.length == 0) {
                alert('Falta ingresar el documento del trabajador');
                return false;
            }

            var nombreText = $.trim($('#nombre').val());
            if (nombreText.length == 0) {
                alert(document.getElementById('<%=hdnstr4009.ClientID%>').value);
                return false;
            }

            var costoUnitarioValor = $("#<%=costo_unitario_standar.ClientID%>").val();
            if ($.isNumeric(costoUnitarioValor) == false) {
                alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                return false;
            }

            var HasERP = document.getElementById('<%=hdnIncludeERP.ClientID%>').value;

            if (HasERP == "True") {
                var costoContaValor = $("#<%=txtCosto_Conta.ClientID%>").val();
                if ($.isNumeric(costoContaValor) == false) {
                    alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                    return false;
                }

                var horaESText = $.trim($('#txtHoraES').val());
                if ($.isNumeric(horaESText) == false) {
                    alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                    return false;
                }

                var horaEDText = $.trim($('#txtHoraED').val());
                if ($.isNumeric(horaEDText) == false) {
                    alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                    return false;
                }
            }

            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnIncludeERP" runat="server" />
        <div style="font-size: small">
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="Label2" runat="server" Text="TIPO DOCUMENTO:"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="CboTipoDoc" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblCodigo" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                    <asp:TextBox ID="fecha2" runat="server" CssClass="date" Visible="false"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblNombre" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="nombre" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblCategoria" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboCategoria" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboCategoria_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboCiclo" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblCosto_conta" runat="server" Text="COSTO HR PAGO"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtCosto_Conta" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:HiddenField ID="Text1" runat="server" />
                    <asp:Label ID="lblHoraES" runat="server" Text="Hora 25%"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtHoraES" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:HiddenField ID="Text2" runat="server" />
                    <asp:Label ID="lblHoraED" runat="server" Text="Hora 35%"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtHoraED" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:HiddenField ID="Text3" runat="server" />
                    <asp:Label ID="lblCostoUnitarioStandar" runat="server" Text="Costo Hr Std"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="costo_unitario_standar" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row" style="display: none">
                <div class="col-4">
                    <asp:Label ID="lblEstado" runat="server" Text="ESTADO"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboEstado" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                    <asp:HiddenField ID="fecha" runat="server" />
                </div>
            </div>
            <br />
            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick="return Validar();" CssClass="btn btn-primary btn-sm" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" Visible="false" />
            <br />
            <div id="dvMessage" runat="server" visible="false" class="ui-state-error-text">
                <asp:Label ID="lblResult" runat="server" />
            </div>
            <asp:HiddenField ID="hdnStr208" runat="server" />
            <asp:HiddenField ID="hdnStr312" runat="server" />
            <asp:HiddenField ID="hdnstr4009" runat="server" />
            <asp:HiddenField ID="hdnStr528" runat="server" />
        </div>
    </form>
</body>
</html>
