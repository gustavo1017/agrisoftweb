<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImportarMapeo.aspx.vb" Inherits="AgrisoftWeb.UI.ImportarMapeo" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PagesContent" runat="server">
    <div>
        <br />
        <div class="form-row">
            <div class="col-6">
                <div id="Div1" runat="server" class="alert alert-info">
                    <asp:RadioButton ID="rbtnPlantacion" runat="server" Text="" GroupName="SearchFilter" AutoPostBack="true" />
                    <b><asp:label id="Label1" runat="server" text="Inventario de Plantacion"></asp:label></b>
                </div>
            </div>
            <div class="col-6">
                <div id="Div2" runat="server" class="alert alert-info">
                    <asp:RadioButton ID="rbtnCosechas" runat="server" Text="" GroupName="SearchFilter" AutoPostBack="true" />
                    <b><asp:label id="Label2" runat="server" text="Proyeccion de Cosechas"></asp:label></b>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-6">
                <asp:fileupload id="fuImport" runat="server" accept=".txt" onchange="HideErrorMessage();" onclick="HideErrorMessage();" />
                <asp:button id="btnUpload" runat="server" onclick="btnUpload_Click" text="Cargar" />
            </div>
            <div class="col-6" style="text-align: right">
                <asp:fileupload id="fuImportPlan" runat="server" accept=".txt" onchange="HideErrorMessage();" onclick="HideErrorMessage();" />
                <asp:button id="btnUploadPlan" runat="server" onclick="btnUploadPlan_Click" text="Cargar" />
            </div>
        </div>
        <br />
        <div id="dvMessage" runat="server" visible="false" class="alert alert-success">
            <asp:label id="lblResults" runat="server" text=""></asp:label>
        </div>
    </div>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        function HideErrorMessage() {
            var x = document.getElementById('<%=dvMessage.ClientID%>');

            if (x != null) {
                if (x.style.display === "none") {
                    //x.style.display = "block";
                }
                else {
                    x.style.display = "none";
                }
            }
        }
    </script>
</asp:Content>
