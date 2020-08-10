<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImportarCostos.aspx.vb" Inherits="AgrisoftWeb.UI.ImportarCostos" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PagesContent" runat="server">
    <div>
        <br />
        <div class="form-row">
            <div class="col-6">
                <div id="Div1" runat="server" class="alert alert-info">
                    <b><asp:label id="Label1" runat="server" text="Costos Compras y Cosechas"></asp:label></b>
                </div>
            </div>
            <div class="col-6">
                <div id="Div2" runat="server" class="alert alert-info">
                    <b><asp:label id="Label2" runat="server" text="Planeamiento"></asp:label></b>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-6">
                <asp:fileupload id="fuImport" runat="server" accept=".txt" />
                <asp:button id="btnUpload" runat="server" onclick="btnUpload_Click" text="Cargar" />
            </div>
            <div class="col-6" style="text-align: right">
                <asp:fileupload id="fuImportPlan" runat="server" accept=".txt" />
                <asp:button id="btnUploadPlan" runat="server" onclick="btnUploadPlan_Click" text="Cargar" />
            </div>
        </div>
        <br />
        <div id="dvMessage" runat="server" visible="false" class="alert alert-success">
            <asp:label id="lblResults" runat="server" text=""></asp:label>
        </div>
    </div>
</asp:Content>
