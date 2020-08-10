<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="AgrisoftWeb.UI.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnUp').click(function(){
                var $op =$('#Select1 option:selected');
                if($op.length){
                    
                        $op.first().prev().before($op);
                    
                }
            });

            $('#btnDown').click(function(){
                var $op =$('#Select1 option:selected');
                if($op.length){
                    $op.last().next().after($op);
                }
            });

            var msg = <%=GetGlobalResourceObject("Resource", "str6018") %>;
            alert(msg);
        });
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <select id="Select1" multiple="multiple" name="Select1">
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
        </select>

        <input id="btnUp" type="button" value="UP" />
        <input id="btnDown" type="button" value="DOWN" />
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:ListBox ID="lstActividades" runat="server" Height="120px">
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
        </asp:ListBox>
        <asp:Button ID="btnArriba" runat="server" Text="up" OnClick="btnArriba_Click" />
                </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnArriba" />
            </Triggers>
            </asp:UpdatePanel>
    </form>
    
</body>
</html>
