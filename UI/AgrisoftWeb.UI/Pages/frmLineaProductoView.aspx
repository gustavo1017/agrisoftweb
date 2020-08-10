<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmLineaProductoView.aspx.vb" Inherits="AgrisoftWeb.UI.frmLineaProductoView" EnableEventValidation="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Lineas de Productos"></asp:Label></b>
            </div>
        </div>
    </div>
    <div style="padding-top: 20px">
        <div class="form-row">
            <div class="col-0">
                <asp:Label ID="lblFind" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-0">
                <asp:DropDownList ID="cboFields" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
            </div>
            <div class="col-3">
                <asp:TextBox ID="find" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <div class="col-4">
                <asp:Button ID="btnBuscar" runat="server" Text="" OnClientClick="return validateSearch();" OnClick="btnBuscar_Click" CssClass="btn btn-primary btn-sm" />
            </div>
        </div>
        <div id="dvMessage" runat="server" class="alert alert-danger" visible="false">
            <asp:Label ID="lblResults" runat="server" Text=""></asp:Label>
        </div>
        <div class="row" style="padding-top:10px">
            <div id="Frame3" style="height: 250px; overflow: scroll; float: left; position: relative; left: 0;" class="col-sm-4">
                <asp:GridView ID="grilla" runat="server" OnRowDataBound="grilla_RowDataBound" CssClass="table-bordered"
                    OnRowCreated="grilla_RowCreated" OnSelectedIndexChanged="grilla_SelectedIndexChanged">
                    <RowStyle Font-Size="X-Small" />
                </asp:GridView>
            </div>
            <br />
            <div id="Frame4" class="col-sm-2">
                <div style="padding-bottom: 8px; padding-top:20px">
                    <asp:Button ID="btnNuevo" runat="server" Text="" OnClick="btnNuevo_Click" OnClientClick="return showNewForm();" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:Button ID="btnModificar" runat="server" Text="" OnClick="btnModificar_Click" OnClientClick="return showEditForm();" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:Button ID="btnEliminar" runat="server" Text="" OnClick="btnEliminar_Click" OnClientClick="if(!ConfirmDelete()) return false;" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:Button ID="btnRefrescar" runat="server" Text="" OnClick="btnRefrescar_Click" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:Button ID="btnExportarExcel" runat="server" Text="" OnClick="btnExportarExcel_Click" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
            </div>
        </div>
        <div class="row">
            <asp:Label ID="lblReg" runat="server" Text="N Registros"></asp:Label>
        </div>
        <asp:HiddenField ID="hdnStr1005" runat="server" />
        <asp:HiddenField ID="hdnStr12104" runat="server" />
        <asp:HiddenField ID="hdnStr39" runat="server" />
        <asp:HiddenField ID="hdnDelete" runat="server" />
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

        function validateSearch() {
            return true;
        }

        function verifySelection() {
            var selectionId = '<%= Session("frmLineaProductoView_IdLinea") %>';
            if (selectionId.valueOf() == null || selectionId.valueOf() == '') {
                alert(document.getElementById('<%=hdnStr12104.ClientID%>').value);
                return false;
            }
            return true;
        }

        function showEditForm() {
            //Reset hdn value to avoid the code-behind execution when page is reloaded
            document.getElementById('<%=hdnDelete.ClientID%>').value = "";
            HideErrorMessage();

            var result = verifySelection();
            if (result == false)
                return false;

            //var btnName ="Cerrando";
            var modalPage = "frmLineaProducto.aspx?Action=EDIT";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 290,
                width: 420,
                title: document.getElementById('<%=hdnStr39.ClientID%>').value,
                position: ['center', 20],
                //buttons:{
                //    btnName: function () {
                //        $(this).dialog("close");
                //    }
                //},
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

            var modalPage = "frmLineaProducto.aspx?Action=NEW";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 290,
                width: 420,
                title: document.getElementById('<%=hdnStr39.ClientID%>').value,
                position: ['center', 20],
                close: function (event, ui) {
                    location.reload();
                }
            });
            $dialog.dialog('open');

            return false;
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
    </script>
</asp:Content>
