<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmZonaTrabajo.aspx.vb" Inherits="AgrisoftWeb.UI.frmZonaTrabajo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#hectareas').keypress(function () {
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

            $('#campana').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
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

        // On page load
        $(function () {
            setDatePickerControls();
        });

          function setDatePickerControls() {
            var dateCurrent = new Date();
            $(".date").datepicker({ dateFormat: 'dd/mm/yy' }).attr({'readonly': true, 'background-color': 'white'});

            if ($(".date").val() == "") {
                $(".date").datepicker('setDate', dateCurrent).attr('readonly', true);
            }
          }

        //function setDatePickerControls() {
        //    var dateCurrent = new Date();
        //    var dateFrom = new Date();
        //    dateFrom.setDate(dateCurrent.getDate());

        //    $(".date").datepicker({ dateFormat: 'dd/mm/yy' });

        //    if ($(".date").val() == "") {
        //        $(".date").datepicker('setDate', dateFrom).attr('readonly', 'readonly');
        //    }
        //}

        function Validar() {
            var idText = $.trim($('#txtId').val());
            if (idText.length != 7) {
                alert(document.getElementById('<%=hdnStr308.ClientID%>').value);
                return false;
            }

            var descripcionText = $.trim($('#descripcion').val());
            if (descripcionText.length == 0) {
                alert(document.getElementById('<%=hdnStr309.ClientID%>').value);
                return false;
            }

            var campañaValor = $("#<%=campana.ClientID%>").val();
            if ($.isNumeric(campañaValor) == false) {
                alert('Falta ingresar Campaña');
                return false;
            }

            var tipoCosto = document.getElementById('<%=hdnTipocosto.ClientID%>').value;
            if (tipoCosto == "COSTDIR") {
                var hectareasValor = $("#<%=hectareas.ClientID%>").val();
                if ($.isNumeric(hectareasValor) == false) {
                    alert(document.getElementById('<%=hdnStr312.ClientID%>').value);
                    return false;
                }
            }            

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
                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control form-control-sm" MaxLength="7"></asp:TextBox>
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
                    <asp:Label ID="lblCampana" runat="server" Text="N° CAMPAÑA"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="campana" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblTipo" runat="server" Text="TIPO"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboTipo" runat="server" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblCultivo" runat="server" Text="CULTIVO"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="cboCultivo" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblHectareas" runat="server" Text="HECTAREAS"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="hectareas" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="form-row">
                <div class="col-4">
                    <asp:Label ID="lblfecsiembra" runat="server" Text="Fecha de Siembra"></asp:Label>
                </div>
                <div class="col">
                    <asp:TextBox ID="fecsiembra" runat="server" CssClass="date"></asp:TextBox>
                </div>
            </div>
            <asp:RadioButton ID="rbtnActivo" runat="server" Visible="false" GroupName="ZonaTrabajoEstado" />
            <asp:RadioButton ID="rbtnInactivo" runat="server" Visible="false" GroupName="ZonaTrabajoEstado" />
            <br />
            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick="return Validar();" CssClass="btn btn-primary btn-sm" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" Visible="false" />
            <br />
            <div id="dvMessage" runat="server" visible="false">
                <asp:Label ID="lblResult" runat="server" />
            </div>
            <asp:HiddenField ID="hdnStr308" runat="server" />
            <asp:HiddenField ID="hdnStr309" runat="server" />
            <asp:HiddenField ID="hdnStr312" runat="server" />
            <asp:HiddenField ID="hdnTipocosto" runat="server" />
        </div>
    </form>
</body>
</html>
