<%@ Page Title="Ordenar las actividades" Language="vb" AutoEventWireup="false" CodeBehind="frmActividades_ordenar.aspx.vb" Inherits="AgrisoftWeb.UI.frmActividades_ordenar" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width:300px">
                <div style="float: left">
                    <asp:ListBox ID="lstActividades" runat="server" Width="200px" Height="200px" OnSelectedIndexChanged="lstActividades_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                </div>
                <div style="float: right">
                    <asp:Button ID="btnUp" runat="server" Text="UP" OnClick="btnUp_Click" />
                    <asp:Button ID="btnDown" runat="server" Text="DOWN" OnClick="btnDown_Click" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUp" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDown" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lstActividades" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
    <br />
    <asp:Label ID="lblResults" runat="server" Text=""></asp:Label>
        </div>
        </form>
</body>
</html>
