<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Pages/MasterAgrisoftWeb.Master" CodeBehind="frmCultivos_ordenarJS.aspx.vb" Inherits="AgrisoftWeb.UI.frmCultivos_ordenarJS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PagesContent" runat="server">

    <div>
        <asp:Label ID="Label1" runat="server" Text="Hello world!!"></asp:Label>

        <select id="Select1" multiple="multiple" name="Select1">
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
        </select>

        <input id="btnUp" type="button" value="UP" />
        <input id="btnDown" type="button" value="DOWN" />
        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" />
        <div style="display: none">
            <asp:Button ID="btnProcess" runat="server" OnClick="btnProcess_Click" />
        </div>
        <asp:HiddenField ID="hdnList" runat="server" />
    </div>


    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnUp').click(function(){
                var $op =$('#Select1 option:selected');
                if($op.length){
                    if($op.index()==1){
                        $op.first().prevAll().before($op);
                    }
                    else{
                        $op.first().prev().before($op);
                    }
                }
            });

            $('#btnDown').click(function(){
                var $op =$('#Select1 option:selected');
                if($op.length){
                    $op.last().next().after($op);
                }
            });
        });

        function test() {
            alert("Probando desde CodeBehind");

            // read all elements
            var ddlArray = new Array();
            var ddl = document.getElementById('Select1');
            for (i = 0; i < ddl.options.length; i++) {
                ddlArray[i] = ddl.options[i].value;
            }

            document.getElementById('<%=hdnList.ClientID%>').value = ddlArray;
            document.getElementById('<%=btnProcess.ClientID%>').click();
        }

        function fillList() {
            var index = 0;
            var ArrayDB = <%= ListItemsJS%>;
            var ddl = document.getElementById('Select1');
            for (i = 0; i < ArrayDB.length; i++) {
                var opt = document.createElement("option");
                opt.value = index;
                opt.innerHTML = ArrayDB[i]

                ddl.appendChild(opt);
                index++;
            }
        }
    </script>
</asp:Content>
