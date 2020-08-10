<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmCostos_View.aspx.vb" Inherits="AgrisoftWeb.UI.frmCostos_View" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     
    <!-- Colocar esto como titulo -->
    <div class="form-row">
        <div class="col-12">
            <div id="Div1" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label2" runat="server" Text="Costos"></asp:Label></b>
            </div>
        </div>
    </div>
    <div style="padding-top: 20px">
        <div class="form-row">
            <div class="col-0">
                <asp:Label ID="lblFind" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-0">
                <asp:DropDownList ID="cboFields" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboFields_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="col-3">
                <asp:TextBox ID="find" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                <asp:TextBox ID="fecha_0" runat="server" CssClass="date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="fecha_1" runat="server" CssClass="dateTo" Width="100px"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="Label1" runat="server" Text="Zona de Trabajo"></asp:Label>
            </div>
            <div class="col-2">
                <asp:TextBox ID="find2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <div class="col-1">
                <asp:Button ID="btnBuscar" runat="server" Text="" OnClick="btnBuscar_Click" OnClientClick="return validateSearch();" CssClass="btn btn-primary btn-sm" />
            </div>
        </div>
        &nbsp;
        <div id="dvMessage" runat="server" class="alert alert-danger" visible="false">
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
        <div class="form-row">
            <div id="Frame3" style="height: 380px; overflow: scroll; float: left;" class="col-sm-10">
                <asp:GridView ID="grilla" runat="server" OnRowDataBound="grilla_RowDataBound" CssClass="table table-hover table-striped" GridLines="None"
                    OnRowCreated="grilla_RowCreated" OnSelectedIndexChanged="grilla_SelectedIndexChanged" AutoGenerateColumns="false">
                    <HeaderStyle BackColor="#337ab7" Font-Bold="false" ForeColor="White" Font-Size="Smaller" HorizontalAlign="Center" Wrap="false" />
                    <RowStyle Font-Size="X-Small" Wrap="false" />
                    <Columns>
                        <asp:BoundField DataField="id_costo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="tipocosto" />
                        <asp:BoundField DataField="numero_parte" />
                        <asp:BoundField DataField="presup" />
                        <asp:BoundField DataField="fecha" />
                        <asp:BoundField DataField="cultivo" />
                        <asp:BoundField DataField="zt" />
                        <asp:BoundField DataField="etapa" />
                        <asp:BoundField DataField="actividad" />
                        <asp:BoundField DataField="producto" />
                        <asp:BoundField DataField="trabajador" />
                        <asp:BoundField DataField="maquina" />
                        <asp:BoundField DataField="nombreprov" />
                        <asp:BoundField DataField="cantidad" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="costo_unitario_standar" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="tot" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="campana" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="moneda" />
                        <asp:BoundField DataField="tipo_cambio" />
                        <asp:BoundField DataField="precio_contable" />
                        <asp:BoundField DataField="Avance" />
                        <asp:BoundField DataField="id_zonatrabajo" />
                        <asp:BoundField DataField="observaciones" />
                        <asp:BoundField DataField="id_usuario" />
                        <asp:BoundField DataField="fechaauditoria" />
                        <asp:BoundField DataField="tipo_costo" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-sm-2" style="padding-left: 25px">
                <div style="padding-bottom: 10px;">
                    <asp:Button ID="btnNuevo" runat="server" Text="" OnClick="btnNuevo_Click" OnClientClick="return showNewForm();" CssClass="btn btn-primary btn-sm" Width="105px" />
                </div>
                <div style="padding-bottom: 10px">
                    <asp:Button ID="btnModificar" runat="server" Text="" OnClick="btnModificar_Click" OnClientClick="return showEditForm();" CssClass="btn btn-primary btn-sm" Width="105px" />
                </div>
                <div style="padding-bottom: 10px">
                    <asp:Button ID="btnEliminar" runat="server" Text="" OnClick="btnEliminar_Click" OnClientClick="if(!ConfirmDelete()) return false;" CssClass="btn btn-primary btn-sm" Width="105px" />
                </div>
                <div style="padding-bottom: 10px">
                    <asp:Button ID="btnExportarExcel" runat="server" Text="" OnClick="btnExportarExcel_Click" CssClass="btn btn-primary btn-sm" Width="105px" />
                </div>
                <div style="padding-bottom: 10px">
                    <asp:Button ID="btnRefrescar" runat="server" Text="" OnClick="btnRefrescar_Click" CssClass="btn btn-primary btn-sm" Width="105px" />
                </div>
                <div style="padding-bottom: 10px">
                    <asp:Button ID="btnBorradoTotal" runat="server" Text="" OnClick="btnBorradoTotal_Click" OnClientClick="if(!ConfirmDeleteTotal()) return false;" CssClass="btn btn-primary btn-sm" Width="105px" />
                </div>
            </div>
            <br />
        </div>
        <div class="row">
            <asp:Label ID="lblReg" runat="server" Text="N Registros"></asp:Label>
        </div>
        <br />
        <asp:HiddenField ID="hdnStr1005" runat="server" />
        <asp:HiddenField ID="hdnStr615" runat="server" />
        <asp:HiddenField ID="hdnStr12104" runat="server" />
        <asp:HiddenField ID="hdnStr89" runat="server" />
        <asp:HiddenField ID="hdnDelete" runat="server" />
        <asp:HiddenField ID="hdnstr99999980" runat="server" />
    </div>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=find.ClientID %>').on("keypress", function (event) {
                var character = String.fromCharCode(event.keyCode);
                var invalidChars = ".,·()º'¡`'+´´|!ª$%&/=?¿Ç*^¨Ññ;:_\@#~€¬]}'[{¨¨°-\\";
                if (invalidChars.indexOf(character) != -1) {
                    event.preventDefault();
                }

                if (character.search(/'|"/g) != -1) {
                    event.preventDefault();
                }
            });
        });

        function showEditForm() {
            //Reset hdn value to avoid the code-behind execution when page is reloaded
            document.getElementById('<%=hdnDelete.ClientID%>').value = "";
            HideErrorMessage();

            var result = verifySelection();
            if (result == false)
                return false;

            var w = $(window).width();
            var modalPage = "frmCostos.aspx?Action=EDIT";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 600,
                width: w,
                title: document.getElementById('<%=hdnStr89.ClientID%>').value,
                position: ['center', 20],
                close: function (event, ui) {
                    location.reload();
                }
            });
            $dialog.dialog('open');

            return false;
        }

        function showNewForm() {
            //Reset hdn value to avoid the code-behind execution when page is reloaded
            document.getElementById('<%=hdnDelete.ClientID%>').value = "";
            HideErrorMessage();

            var w = $(window).width();
            var modalPage = "frmCostos.aspx?Action=NEW";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 600,
                width: w,
                title: document.getElementById('<%=hdnStr89.ClientID%>').value,
                position: ['center', 20],
                close: function (event, ui) {
                    location.reload();
                }
            });
            $dialog.dialog('open');

            return false;
        }

        function ConfirmDeleteTotal() {
            HideErrorMessage();

            var result = verifySelection();
            if (result == false)
                return false;

            //if (!confirm('<%=hdnstr99999980.ClientID%>')) {
         //   if (!confirm(<%=hdnstr99999980.ClientID%>)) {
                
            if (!confirm(document.getElementById('<%=hdnstr99999980.ClientID%>').value)) {
                return false;
            }
            //'  alert(document.getElementById('<%=hdnStr12104.ClientID%>').value);'

            document.getElementById('<%=hdnDelete.ClientID%>').value = "Delete";
            return true;
        }

        function ConfirmDelete() {
            HideErrorMessage();

            var result = verifySelection();
            if (result == false)
                return false;

            if (!confirm(document.getElementById('<%=hdnStr1005.ClientID%>').value)) {
                return false;
            }

            document.getElementById('<%=hdnDelete.ClientID%>').value = "Delete";
            return true;
        }

        function verifySelection() {
            var selectionId = '<%= Session("frmCostosView_IdCosto") %>';
            if (selectionId.valueOf() == null || selectionId.valueOf() == '') {
                alert(document.getElementById('<%=hdnStr12104.ClientID%>').value);
                return false;
            }
            return true;
        }

        function HideErrorMessage() {
            var x = document.getElementById('<%=dvMessage.ClientID%>');

            if (x != null) {
                if (x.style.display === "none") {
                    x.style.display = "block";
                }
                else {
                    x.style.display = "none";
                }
            }
        }

        $(function () {
            setDatePickerControls();
            //document.getElementById('<%=fecha_0.ClientID%>').value = $(".date").val();
            //document.getElementById('<%=fecha_1.ClientID%>').value = $(".dateTo").val();
        });

        function setDatePickerControls() {
            var dateCurrent = new Date();
            $(".date").datepicker({ dateFormat: 'dd/mm/yy' });
            $(".dateTo").datepicker({ dateFormat: 'dd/mm/yy' });

            if ($(".date").val() == "") {
                $(".date").datepicker('setDate', dateCurrent);
            }

            if ($(".dateTo").val() == "") {
                $(".dateTo").datepicker('setDate', dateCurrent);
            }
        }

        function validateSearch() {
            //validate date inputs
            var datevalueFrom = $(".date").val();
            var datevalueTo = $(".dateTo").val();

            var isValidFrom = isValidDate(datevalueFrom);
            var isValidTo = isValidDate(datevalueTo);

            if (isValidFrom == false || isValidTo == false) {
                alert(Resource1.str99992);
                return false;
            }

            if (datevalueFrom > datevalueTo) {
                alert(document.getElementById('<%=hdnStr615.ClientID%>').value);
                return false;
            }
        }
    </script>
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
