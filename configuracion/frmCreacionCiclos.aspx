<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCreacionCiclos.aspx.cs" Inherits="configuracion_frmCreacionCiclos"  EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
        
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript">
        $(function () {
            cargar();

            $('.initialTime').live('click', function () {
                //     $('.initialTime').attr('readonly', false);
                $('#popUpHora').show();
                ctrlHoraActual = $(this);
            });
            $('.finalTime').live('click', function () {
                //   $('.finalTime').attr('readonly', false);
                $('#popUpHora').show();
                ctrlHoraActual = $(this);
                //    $('.finalTime').attr('readonly', true);
            });
            $('.fecha').live('click', function () {
                //    $('.fecha').attr('readonly', false);
                $('#popUpFecha').show();
                ctrlFechaActual = $(this);
                //   $('.fecha').attr('readonly', true);
            });

            $("#DateDemo").AnyTime_picker({
                format: "%Y-%m-%d",
                hideInput: true,
                placement: "inline",
                labelTitle: "Fecha y hora",
                labelYear: "Año",
                labelMonth: "Mes",
                labelDayOfMonth: "Día del Mes",
                labelSecond: "Segundo",
                labelHour: "Hora",
                labelMinute: "Minuto"
            });

            $("#TimeDemo").AnyTime_picker({
                format: "%H:%i",
                hideInput: true,
                placement: "inline",
                labelTitle: "Hora",
                labelYear: "Año",
                labelMonth: "Mes",
                labelDayOfMonth: "Día del Mes",
                labelSecond: "Segundo",
                labelHour: "Hora",
                labelMinute: "Minuto"
            });


            $(".timeValidate").live('change', function (e) {
                var este = $(this);
                var errorMsg = checkTime(este);
                if (null != errorMsg && errorMsg != "") {
                    popUpAlert(errorMsg, 'error');
                    este.addClass('Error');
                    este.val('00:00').focus();
                }
                else if ((este.is('.initialTime') || este.is('.finalTime')) && $('.initialTime').length && $.trim($('.initialTime').val()).length > 0 && $('.finalTime').length && $.trim($('.finalTime').val()).length > 0) {
                    var errorMsg = sonHorasValidas($('.initialTime').val(), $('.finalTime').val())
                    if (errorMsg != null && errorMsg != "") {
                        popUpAlert(errorMsg, 'error');
                        este.addClass('Error');
                        este.val('00:00').focus();
                    } else {
                        este.removeClass('Error');
                    }
                }
                else {
                    este.removeClass('Error');
                }
            });


        });


        $(".rblRepetir").live('change', function (e) {

            validaRepeticion();
        });

        function validaRepeticion() {
            if ($('.rblRepetir input:checked').val() == 2) {
                window.console && console.log("es " + $('.rblRepetir input:checked').val());
                $('.ckDias').show();
            } else {
                $('#ckDias').hide();
                window.console && console.log('disable when  ' + $('.rblRepetir input:checked').val());
                $('.ckDias').hide();
            }

        }

        function asignarHora() {
            //           window.console && console.log("asigna hora");
            $(ctrlHoraActual).attr('readonly', false);
            $(ctrlHoraActual).val($("#TimeDemo").val());
            $('#popUpHora').hide();
            $(ctrlHoraActual).change();
            $(ctrlHoraActual).attr('readonly', true);
        }

        function asignarFecha() {
            //            window.console && console.log("asigna fecha");
            $(ctrlFechaActual).attr('readonly', false);
            $(ctrlFechaActual).val($("#DateDemo").val());
            $('#popUpFecha').hide();
            $(ctrlFechaActual).change();
            $(ctrlFechaActual).attr('readonly', true);
        }


        function cargar() {
            registerControls();
            validaRepeticion();
        }

    </script>
    


    <script type = "text/javascript">
        function functionx(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                alert("Allow Only Numbers");
                return false;
            }
        }
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                else
                    return false;
            }

            if (txt.value.indexOf(".") > 0) {
                var txtlen = txt.value.length;
                var dotpos = txt.value.indexOf(".");
                //Change the number here to allow more decimal points than 2
                if ((txtlen - dotpos) > 2)
                    return false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode <= 90 && charCode >= 65) || (charCode <= 122 && charCode >= 97) || charCode == 8)
                return true;

            return false;

        }
        </script>
    
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 153px;
        }
        .auto-style5 {
            width: 93px;
        }
        .auto-style6 {
            width: 79px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdnIdPlanta" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="6" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltRancho" runat="server" meta:resourceKey="ltRanchoResource"></asp:Literal>
                            </td>
                            <td class="auto-style5">
                                <asp:DropDownList ID="ddlRancho" runat="server" meta:resourceKey="ddlRanchoResource" AutoPostBack="True" OnSelectedIndexChanged="ddlRancho_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltInvernadero" runat="server" meta:resourceKey="ltInvernaderoResource"></asp:Literal>
                            </td>
                            <td class="auto-style1" colspan="1">
                                <asp:DropDownList ID="ddlInvernadero" runat="server" meta:resourceKey="ddlRanchoResource">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="ltProducto" runat="server" meta:resourceKey="ltProductoResource"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProducto" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltVariedad" runat="server" meta:resourceKey="ltVariedadResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                                <asp:DropDownList ID="ddlVariedad" runat="server" >
                                </asp:DropDownList>
                            </td>
                             <td align="right">
                                 <asp:Literal ID="ltVariables" runat="server" meta:resourceKey="ltVariablesResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:DropDownList ID="ddlVariables" runat="server"  >
                                </asp:DropDownList>
                            </td>
                             <td align="right">
                                 <asp:Literal ID="ltClaveCiclo" runat="server" meta:resourceKey="ltClaveCicloResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblClaveCiclo" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                           
                        </tr>
                        <tr>
                            <td align="right">*<asp:Literal ID="ltFechaPlantacion" runat="server" meta:resourceKey="ltFechaPlantacionResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                                <asp:TextBox ID="txtPlantDate" runat="server" CssClass="required fecha"  />
                            </td>
                            <td align="right">*<asp:Literal ID="ltHarvestDate" runat="server" meta:resourceKey="ltHarvestDateResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:TextBox ID="txtHarvestDate" runat="server" CssClass="required fecha" meta:resourceKey="txtFecha" />
                            </td>
                            <td align="right">*<asp:Literal ID="ltLastHarvest" runat="server" meta:resourceKey="ltLastHarvestResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtLastHarvestDate" runat="server" CssClass="required fecha" meta:resourceKey="txtFecha" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                *<asp:Literal ID="ltNoCabezas" runat="server" meta:resourceKey="ltNoCabezasResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                               
                                <asp:TextBox ID="txtNumeroCabezas" runat="server"  Width="39px" onkeypress="return onlyDotsAndNumbers(this,event);" ></asp:TextBox>
                           
                            </td>

                            <td>
                                <asp:Literal ID="ltAbejorros0" runat="server" meta:resourcekey="ltAbejorrosResource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAbojorros" runat="server" meta:resourcekey="chkZonificadoResource1" />
                            </td>
                            <td>
                                <asp:Literal ID="ltInjertado" runat="server" meta:resourcekey="ltInjertadoResource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkInjertado" runat="server" meta:resourcekey="chkZonificadoResource1" />
                            </td>

                        </tr>
                        
                        <tr>
                            <td colspan="1">
                                <asp:Literal ID="ltComplete0" runat="server" meta:resourcekey="ltCompleteResource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkComplete0" runat="server" meta:resourcekey="chkActivoResource1" />
                            </td>
                            <td></td>
                            <td class="auto-style6">&nbsp;</td>
                            <td> <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" /></td>
                          <td>

                              <asp:Button ID="btnGuardar" runat="server"  Text="Guardar" OnClick="btnGuardar_Click" />
                          </td>
           
                        </tr>
                    </table>
              </asp:Panel>
           

            <div class="grid">
                 <div id="pager" class="pager">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <input type="text" class="pagedisplay" />
                    <img alt="next" src="../comun/img/next.png" class="next" />
                    <img alt="last" src="../comun/img/last.png" class="last" />
                    <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="30">30</option>
                        <option value="40">40</option>
                        <option value="50">50</option>
                    </select>
                </div>
                <asp:GridView ID="GvPlantas" runat="server" AutoGenerateColumns="False" 
                     CssClass="gridView" Width="75%" EmptyDataText="No existen registros" 
                DataKeyNames="idCycle" meta:resourcekey="GvPlantasResource1"  
                     onprerender="GvPlantas_PreRender" onrowdatabound="GvPlantas_RowDataBound" 
                     onselectedindexchanged="GvPlantas_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>

                        <asp:BoundField DataField="Cycle" SortExpression="Cycle"
                            meta:resourcekey="BoundFieldResource1">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Greenhouse" SortExpression="Greenhouse"
                            meta:resourcekey="BoundFieldResource2">
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="NombrePlanta" SortExpression="NombrePlanta"
                            meta:resourcekey="BoundFieldResource3">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Product" SortExpression="Product"
                            meta:resourcekey="BoundFieldResource4">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="PlantDate" meta:resourcekey="BoundFieldResource5"
                            SortExpression="PlantDate" DataFormatString="{0:dd/MM/yyyy}">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Week" meta:resourcekey="BoundFieldResource7"
                            SortExpression="Week">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Year" meta:resourcekey="BoundFieldResource8"
                            SortExpression="Year">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="FirstHarvest" meta:resourcekey="BoundFieldResource9"
                            SortExpression="FirstHarvest" DataFormatString="{0:dd/MM/yyyy}" >
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="FirstHarvestWeek" meta:resourcekey="BoundFieldResource10"
                            SortExpression="FirstHarvestWeek">
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="LastHarvest" meta:resourcekey="BoundFieldResource11"
                            SortExpression="LastHarvest" DataFormatString="{0:dd/MM/yyyy}">
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="Variety" meta:resourcekey="BoundFieldResource12"
                            SortExpression="Variety">
                        
                        </asp:BoundField>
                       <%-- <asp:BoundField DataField="Variable" meta:resourcekey="BoundFieldResource13"
                            SortExpression="Variable">
                         
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="Complete" SortExpression="Complete"
                            meta:resourcekey="BoundFieldResource6">
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
                
            </div>
    </div>
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    <div id="popUpHora" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="TimeDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarHora" value="OK" onclick="asignarHora();" style="float:none;" /></td></tr>
       </table>
    </div>
    <div id="popUpFecha" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="DateDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarFecha();" style="float:none;" /></td></tr>
       </table>
    </div>
</asp:Content>



