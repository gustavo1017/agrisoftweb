<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmCostos.aspx.vb" Inherits="AgrisoftWeb.UI.frmCostos" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtTC').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#txtCantidad').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#txtCostoUnitarioStandar').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#txtCampana').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $('#txtAvance').keypress(function () {
                if (event.which && (event.which && event.which < 46 || event.which > 57 || event.which == 47) && event.keyCode != 8) {
                    event.preventDefault();
                }
                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                }
            });

            $("#txtCantidad,#txtCostoUnitarioStandar").keyup(function () {
                var calculo = $('#txtCantidad').val() * $('#txtCostoUnitarioStandar').val();
                $('#txtMontoStandar').val(calculo.toFixed(2));
                document.getElementById('<%=txtMontoStandar.ClientID%>').value = calculo.toFixed(2);
            });

            $("#txtMontoStandar").attr('readonly', 'readonly');

        });

        $(function () {
            setDatePickerControls();
            document.getElementById('<%=dtPicker3.ClientID%>').value = $(".date").val();
        });

        function setDatePickerControls() {
            var dateCurrent = new Date();
            $(".date").datepicker({ dateFormat: 'dd/mm/yy' }).attr({'readonly': true, 'background-color': 'white'});

            if ($(".date").val() == "") {
                $(".date").datepicker('setDate', dateCurrent).attr('readonly', true);
            }

            $(".date").on("change", function () {
                var selectedDate = $('.date').datepicker('getDate');
                var year = selectedDate.getFullYear();
                var month = selectedDate.getMonth() + 1;
                var day = selectedDate.getDate();

                $.ajax({
                    url: "frmCostos.aspx/GetTipoCambio",
                    method: "POST",
                    //data: '{"selectedDate":"' + selectedDate + '"}',
                    data: '{"year":"' + year + '", "month":"' + month + '", "day":"' + day + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var message = response.d;
                        $('#<%=txtTC.ClientID %>').val(message);
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            });
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

        function verifySelection() {
            var selectionId = '<%= Session("frmCostos_PreviousSelectedIndex") %>';
            if (selectionId.valueOf() == null || selectionId.valueOf() == '' || selectionId.valueOf() == '-1') {
                alert(document.getElementById('<%=hdnStr12104.ClientID%>').value);
                return false;
            }

            var txtCampanaText = $.trim($("#<%=txtCampana.ClientID%>").val());
            if (txtCampanaText.length == 0) {
                alert(document.getElementById('<%=hdnStr9019.ClientID%>').value);
                return false;
            }

            return true;
        }

        function Validar() {
            HideErrorMessage();

            var cantidadText = $.trim($("#<%=txtCantidad.ClientID%>").val());
            if (cantidadText.length == 0) {
                alert(document.getElementById('<%=hdnStr527.ClientID%>').value);
                return false;
            }

            var cUnitarioText = $.trim($("#<%=txtCostoUnitarioStandar.ClientID%>").val());
            if (cUnitarioText.length == 0) {
                alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                return false;
            }

            var txtTCText = $.trim($("#<%=txtTC.ClientID%>").val());
            if (txtTCText.length == 0) {
                alert(document.getElementById('<%=hdnStr548.ClientID%>').value);
                return false;
            }

            var txtCampanaText = $.trim($("#<%=txtCampana.ClientID%>").val());
            var buscarCriterio = document.getElementById('<%=hdnBuscarProductos.ClientID%>').value
            //if (txtCampanaText.length == 0 && buscarCriterio != 'Productos') {
            if (txtCampanaText.length == 0) {
                alert(document.getElementById('<%=hdnStr9019.ClientID%>').value);
                return false;
            }

            var cantidadValue = $.trim($("#<%=txtCantidad.ClientID%>").val());
            if (cantidadValue == 0) {
                alert(document.getElementById('<%=hdnStr527.ClientID%>').value);
                return false;
            }

            var cUnitarioValue = $.trim($("#<%=txtCostoUnitarioStandar.ClientID%>").val());
            if (cUnitarioValue == 0) {
                alert(document.getElementById('<%=hdnStr528.ClientID%>').value);
                return false;
            }

            var txtTCValue = $.trim($("#<%=txtTC.ClientID%>").val());
            if (txtTCValue == 0) {
                alert(document.getElementById('<%=hdnStr548.ClientID%>').value);
                return false;
            }

            var calculo = $('#txtCantidad').val() * $('#txtCostoUnitarioStandar').val();
            $('#txtMontoStandar').val(calculo.toFixed(2));
            document.getElementById('<%=txtMontoStandar.ClientID%>').value = calculo.toFixed(2);
        }

        function ValidarGrabar() {
            var txtCicloPeriodoText = $.trim($("#<%=txtCicloPeriodo.ClientID%>").val());
            if (txtCicloPeriodoText.length == 0) {
                alert(document.getElementById('<%=hdnStr12125.ClientID%>').value);
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding-top: 10px">
            <asp:HiddenField ID="hdnStr12104" runat="server" />
            <asp:HiddenField ID="hdnStr4016" runat="server" />
            <asp:HiddenField ID="hdnStr548" runat="server" />
            <asp:HiddenField ID="hdnStr9019" runat="server" />
            <asp:HiddenField ID="hdnStr527" runat="server" />
            <asp:HiddenField ID="hdnStr528" runat="server" />
            <asp:HiddenField ID="hdnStr12125" runat="server" />
            <asp:HiddenField ID="hdnStr6018" runat="server" />
            <asp:HiddenField ID="hdnBuscarProductos" runat="server" />
            <div class="form-row">
                <div class="col-0">
                </div>
                <div class="col-0">
                </div>
                <div class="col-0">
                    <asp:Label ID="lblCiclo" runat="server" Text="Ciclo" CssClass="col-form-label-sm"></asp:Label>
                </div>
                <div class="col-0">
                    <asp:TextBox ID="txtCicloPeriodo" runat="server" CssClass="form-control form-control-sm" Width="100px"></asp:TextBox>
                </div>
                <div class="col-0">
                    <asp:Label ID="lblMoneda" runat="server" Text="Moneda" CssClass="col-form-label-sm"></asp:Label>
                </div>
                <div class="col-0">
                    <asp:DropDownList ID="cboMoneda" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                </div>
                <div class="col-3" style="text-align: right">
                </div>
                <div class="col-0">
                </div>
            </div>
            <div id="dvMessage" runat="server" class="alert alert-danger" visible="false">
                <asp:Label ID="lblResults" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-row" style="padding-top: 5px">
                <div id="dvGrilla" style="width: 80%; height: 200px; overflow: scroll; float: left" class="col-sm-12">
                    <asp:GridView ID="lvwAsientos" runat="server" OnRowDataBound="lvwAsientos_RowDataBound" CssClass="table table-hover table-striped" GridLines="None"
                        OnRowCreated="lvwAsientos_RowCreated" OnSelectedIndexChanged="lvwAsientos_SelectedIndexChanged" Width="100%"
                        OnPreRender="lvwAsientos_PreRender">
                        <HeaderStyle BackColor="#337ab7" Font-Bold="false" ForeColor="White" Font-Size="Smaller" HorizontalAlign="Center" Wrap="false" />
                        <RowStyle Font-Size="X-Small" Wrap="false" />
                    </asp:GridView>
                </div>
            </div>
            <div class="form-row" style="padding-bottom: 5px">
                <div class="col-6">
                </div>
                <div class="col-0">
                    <asp:Label ID="LblTotCantidad" runat="server" Text="Total Cantidad" Visible="false"></asp:Label>
                </div>
                <div class="col-0">
                    <asp:TextBox ID="TotCantidad" runat="server" CssClass="form-control form-control-sm text-right" Visible="false"></asp:TextBox>
                </div>
                <div class="col-0">
                    <asp:Label ID="LblTotMonto" runat="server" Text="Total Monto" CssClass="col-form-label-sm"></asp:Label>
                </div>
                <div class="col-0">
                    <asp:TextBox ID="TotMonto" runat="server" CssClass="form-control form-control-sm text-right" Enabled="false" Width="150px"></asp:TextBox>
                </div>
            </div>
            <div class="form-row" style="padding-bottom: 5px">
                <div class="col-0">
                    <asp:Label ID="Label1" runat="server" Text="Fecha" CssClass="col-form-label-sm"></asp:Label>
                    <asp:TextBox ID="dtPicker3" runat="server" CssClass="date form-control form-control-sm" Width="90px"></asp:TextBox>
                </div>
                <div class="col-0">
                    <asp:Label ID="lbltipoCambio" runat="server" Text="Tipo Cambio:" CssClass="col-form-label-sm"></asp:Label>
                    <asp:TextBox ID="txtTC" runat="server" CssClass="form-control form-control-sm text-right" Width="100px"></asp:TextBox>
                </div>
                <div class="col-0">
                    <asp:Label ID="lblTipoRecurso" runat="server" Text="Tipo Recurso" CssClass="col-form-label-sm"></asp:Label>
                    <asp:DropDownList ID="cboTipoCosto" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboTipoCosto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="col-3">
                    <asp:Label ID="lblRecurso" runat="server" Text=Resource1.str3005 CssClass="col-form-label-sm"></asp:Label>
                    <asp:DropDownList ID="cboCodanx" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboCodanx_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="col-0"></div>
                <div class="col-0">
                    <asp:Label ID="lblCantidad" runat="server" Text="Cantidad" CssClass="col-form-label-sm"></asp:Label>
                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control form-control-sm text-right" Width="90px"></asp:TextBox>
                </div>
                <div class="col-0">
                    <asp:Label ID="lblPrecioUnitario" runat="server" Text="Precio U." CssClass="col-form-label-sm"></asp:Label>
                    <asp:TextBox ID="txtCostoUnitarioStandar" runat="server" CssClass="form-control form-control-sm text-right" Width="100px"></asp:TextBox>
                </div>
                <div class="col-0">
                    <asp:Label ID="lblMonto" runat="server" Text="Monto" CssClass="col-form-label-sm"></asp:Label>
                    <asp:TextBox ID="txtMontoStandar" runat="server" CssClass="form-control form-control-sm text-right" Width="120px"></asp:TextBox>
                </div>
            </div>
            <div class="form-row" style="padding-bottom: 5px">
                <div class="col-0">

                </div>
            </div>
            <div class="form-row" style="padding-bottom: 5px">
                <div class="col-0">
                    <asp:Label ID="lblCultivo" runat="server" Text="Cultivo" CssClass="col-form-label-sm" Visible="False" Width="0px"></asp:Label>
                    <asp:Label ID="lblZonadeTrabajo" runat="server" Text="Zona de Trabajo" CssClass="col-form-label-sm"></asp:Label>
                </div>
                <div class="col-3">
                    <asp:DropDownList ID="cboCultivo" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboCultivo_SelectedIndexChanged" AutoPostBack="true" Visible="False" Width="0px"></asp:DropDownList>
                    <asp:DropDownList ID="cboZonaTrabajo" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboZonaTrabajo_SelectedIndexChanged" AutoPostBack="true" Width="300px"></asp:DropDownList>
                </div>
                <div class="col-0" style="width: 130px; text-align: right">
                    <asp:Label ID="lblActividad" runat="server" Text="Actividad" CssClass="col-form-label-sm"></asp:Label>
                </div>
                <div class="col-3">
                    <asp:DropDownList ID="cboActividad" runat="server" CssClass="form-control form-control-sm" Width="350px"></asp:DropDownList>
                </div>
                <div class="col-1" style="width: 80px; height: 49px;">
                    <asp:Label ID="lblCampana" runat="server" Text="Etapa" CssClass="col-form-label-sm" Width="0px"></asp:Label>
                </div>
                <div class="col-1">
                    <asp:TextBox ID="txtCampana" runat="server" CssClass="form-control form-control-sm text-right" Width="110px"></asp:TextBox>
                </div>
                <div class="col-0" style="width: 70px; text-align: right">
                    <asp:Label ID="lblAvance" runat="server" Text="Avance" CssClass="col-form-label-sm"></asp:Label>
                </div>
                <div class="col-1">
                    <asp:TextBox ID="txtAvance" runat="server" CssClass="form-control form-control-sm text-right" Width="110px"></asp:TextBox>
                </div>
            </div>
            <div id="dvInfo" runat="server">
                <div class="form-row">
                    <asp:DropDownList ID="cboEtapa" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboEtapa_SelectedIndexChanged" AutoPostBack="true" Visible="False" Width="0px"></asp:DropDownList>
                </div>
                <div class="form-row">
                    <div class="col-0" style="width: 70px; text-align: right">
                        <asp:Label ID="lblEtapa" runat="server" Text="Etapa" CssClass="col-form-label-sm" Visible="False"></asp:Label>
                    </div>
                    <div class="col-2">
                        <%--<asp:DropDownList ID="cboEtapa" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="cboEtapa_SelectedIndexChanged" AutoPostBack="true" Visible="False"></asp:DropDownList>--%>
                    </div>
                    <div class="col-0" style="width: 200px; text-align: right">
                    </div>
                    <div class="col-1">
                    </div>
                    <div class="col-0" style="width: 105px; text-align: right">
                    </div>
                    <div class="col-2">
                    </div>
                </div>
            </div>
            <div id="dvProveedor" runat="server">
                <div class="form-row">
                    <div class="col-0" style="width: 70px; text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="Proveedor" CssClass="col-form-label-sm"></asp:Label>
                    </div>
                    <div class="col-0" style="width: 315px;">
                        <asp:DropDownList ID="cboProveedor" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-row border-top">
                <div class="col-6">
                </div>
                <div class="col-3" style="text-align: right; padding-top: 10px">
                    <asp:Button ID="cmdAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" OnClick="cmdAgregar_Click" OnClientClick="return Validar();" Width="95px" />
                </div>
                <div class="col-1" style="text-align: right; padding-top: 10px">
                    <asp:Button ID="cmdModificar" runat="server" Text="Modificar" CssClass="btn btn-primary btn-sm" OnClick="cmdModificar_Click" OnClientClick="return verifySelection();" Width="95px" />
                </div>
            </div>
            <div class="form-row">
                <div class="col-6">
                </div>
                <div class="col-3" style="text-align: right; padding-top: 10px">
                    <asp:Button ID="cmdEliminar" runat="server" Text="Eliminar" CssClass="btn btn-primary btn-sm" OnClick="cmdEliminar_Click" OnClientClick="return verifySelection();" Width="95px" />
                </div>
                <div class="col-1" style="text-align: right; padding-top: 10px">
                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" CssClass="btn btn-primary btn-sm" Width="95px" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
