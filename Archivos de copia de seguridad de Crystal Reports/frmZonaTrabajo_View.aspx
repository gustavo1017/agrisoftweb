<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmZonaTrabajo_View.aspx.vb" Inherits="AgrisoftWeb.UI.frmZonaTrabajo_View" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Zonas de Trabajo"></asp:Label></b>
            </div>
        </div>
    </div>
    <div style="padding-top: 20px">
        <div class="form-row">
            <div class="col-0">
                <asp:label id="lblFind" runat="server" text=""></asp:label>
            </div>
            <div class="col-0">
                <asp:dropdownlist id="cboFields" runat="server" cssclass="form-control form-control-sm"></asp:dropdownlist>
            </div>
            <div class="col-3">
                <asp:textbox id="find" runat="server" cssclass="form-control form-control-sm"></asp:textbox>
            </div>
            <div class="col-4">
                <asp:button id="btnBuscar" runat="server" text="" onclientclick="return validateSearch();" onclick="btnBuscar_Click" cssclass="btn btn-primary btn-sm" />
            </div>
        </div>
        <div id="dvMessage" runat="server" class="alert alert-danger" visible="false">
            <asp:label id="lblResults" runat="server" text=""></asp:label>
        </div>
        <div class="row" style="padding-top:10px">
            <div id="Frame3" style="height: 250px; overflow: scroll; float: left; position: relative; left: 0;" class="col-sm-8">
                <asp:gridview id="grilla" runat="server" onrowdatabound="grilla_RowDataBound" cssclass="table-bordered"
                    onrowcreated="grilla_RowCreated" onselectedindexchanged="grilla_SelectedIndexChanged">
                    <RowStyle Font-Size="X-Small" />
                </asp:gridview>
            </div>
            <br />
            <div id="Frame4" class="col-sm-2">
                <div style="padding-bottom: 8px; padding-top: 20px">
                    <asp:button id="btnNuevo" runat="server" text="" onclick="btnNuevo_Click" onclientclick="return showNewForm();" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:button id="btnModificar" runat="server" text="" onclick="btnModificar_Click" onclientclick="return showEditForm();" CssClass="btn btn-primary btn-sm" Width="95px"  />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:button id="btnEliminar" runat="server" text="" onclick="btnEliminar_Click" onclientclick="if(!ConfirmDelete()) return false;" CssClass="btn btn-primary btn-sm" Width="95px"  />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:button id="btnRefrescar" runat="server" text="" onclick="btnRefrescar_Click" CssClass="btn btn-primary btn-sm" Width="95px"  />
                </div>
                <div style="padding-bottom: 8px">
                    <asp:button id="btnExportarExcel" runat="server" text="" onclick="btnExportarExcel_Click" CssClass="btn btn-primary btn-sm" Width="95px"  />
                </div>
            </div>
        </div>
        <div class="row">
            <asp:label id="lblReg" runat="server" text="N Registros"></asp:label>
        </div>
        <asp:hiddenfield id="hdnStr1005" runat="server" />
        <asp:HiddenField ID="hdnStr12104" runat="server" />
        <asp:HiddenField ID="hdnStr301" runat="server" />
        <asp:hiddenfield id="hdnDelete" runat="server" />
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
            var selectionId = '<%= Session("frmZonaTrabajoView_IdZonaTrabajo") %>';
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
            var modalPage = "frmZonaTrabajo.aspx?Action=EDIT";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 390,
                width: 500,
                title: document.getElementById('<%=hdnStr301.ClientID%>').value,
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

            var modalPage = "frmZonaTrabajo.aspx?Action=NEW";
            var urlBase = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var url = urlBase + modalPage;
            var $dialog = $('<div></div>').html('<iframe style="border: 0px; " src="' + modalPage + '" width="100%" height="100%"></iframe>').dialog({
                autoOpen: false,
                modal: true,
                height: 400,
                width: 500,
                title: document.getElementById('<%=hdnStr301.ClientID%>').value,
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
