<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmHelp.aspx.vb" Inherits="AgrisoftWeb.UI.frmHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">
     <div class="form-row">
        <div class="col-12">
            <div id="Div2" runat="server" class="alert alert-info">
                <b><asp:Label ID="Label3" runat="server" Text="Ayuda"></asp:Label></b>
            </div>
        </div>
    </div>
    <div style="font-size: small">
        <br />
        <div class="form-row">
            <div class="col-8"><h2>Página de Ayuda</h2></div>
        </div>
        <div class="form-row">
            <div class="col-8">En esta página podras encontrar los manuales de ayuda del Sistema para facilitar su uso y absolver preguntas.</div>
        </div>
        <br />
        <div class="form-row">
            <div class="col-8">
                <asp:linkbutton ID="lbtnBuenasPracticas" OnClick="lbtnBuenasPracticas_Click" runat="server">Manual de Buenas Practicas para Control de Costos(Teorico)</asp:linkbutton>
            </div>
        </div>
        <br />
        <div class="form-row">
            <div class="col-8">
                <asp:linkbutton ID="lbtnManualUsuario" OnClick="lbtnManualUsuario_Click" runat="server">Manual de Usuario (Practico)</asp:linkbutton>
            </div>
        </div>
     <%--   <br />
        <div class="form-row">
            <div class="col-8">
                <asp:linkbutton ID="lbtnFAQ" OnClick="lbtnFAQ_Click" runat="server">Preguntas Frecuentes</asp:linkbutton>
            </div>
        </div>--%>
         <br />
        <div class="form-row">
            <div class="col-8">
                <asp:linkbutton ID="lbtnFormatos" OnClick="lbtnFormatos_Click" runat="server">Formatos de Campo</asp:linkbutton>
            </div>
        </div>
        <br />
        <div class="form-row">
             <div class="col-8">
                <asp:linkbutton ID="lblPlantillaExcel"  runat="server">Plantilla de Excel</asp:linkbutton>
            </div>
        </div>
        <br />
         <div class="form-row">
             <div class="col-8">
                <asp:linkbutton href="https://convertio.co/es/xls-txt/" target="_blank"  runat="server">Convertidor de archivo de Excel a TXT</asp:linkbutton>
            </div>
        </div>
        <br />
        <div class="form-row">
             <div class="col-8">
                <asp:linkbutton href="https://www.agrisoftweb.com/apk" target="_blank"  runat="server">Descargar app</asp:linkbutton>
            </div>
        </div>
    </div>

    

    
</asp:Content>
