<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmADMEmpleados.aspx.cs" EnableEventValidation="false" Inherits="RH_frmADMEmpleados" %>



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
            registerControls();
        });
    </script>
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

        function limpiaFecha() {
            //            window.console && console.log("asigna fecha");
            $(ctrlFechaActual).attr('readonly', false);
            $(ctrlFechaActual).val("");
            $('#popUpFecha').hide();
            $(ctrlFechaActual).change();
            $(ctrlFechaActual).attr('readonly', true);
        }

        function cargar() {
            registerControls();
            validaRepeticion();
        }

    </script>
    
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdinIdAsociado" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltRancho" runat="server" Text="Rancho"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlRanchos" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlRanchos_SelectedIndexChanged"  >
                                </asp:DropDownList>
                            </td>
                            <td><asp:Literal ID="ltLider" runat="server" Text="Jefe Inmediato" ></asp:Literal></td>
                            <td>
                                <asp:DropDownList ID="ddlLider" runat="server"  style="width:auto;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltLider0" runat="server" Text="Fecha Alta"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFechaAlta" runat="server" CssClass="required fecha" />
                            </td>
                            <td>
                                <asp:Literal ID="ltLider2" runat="server" Text="Fecha Baja"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaBaja0" runat="server" CssClass="required fecha" meta:resourceKey="txtFecha"></asp:TextBox>
                            </td>
                            <td>Activo</td>
                            <td>
                                <asp:CheckBox ID="cbxActivo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltInvernadero" runat="server" Text="No Empleado"></asp:Literal>
                            </td>
                            <td align="right" colspan="5">
                                <asp:TextBox ID="txtNoEmpleado" runat="server"></asp:TextBox>
                            </td>
                           
                        </tr>
                        <tr>
                            <td align="right"><asp:Literal ID="ltNombreEmpleado" runat="server" Text="Nombre"></asp:Literal></td>
                            <td align="right" colspan="5">
                                <asp:TextBox ID="txtNombreEmpleado" runat="server" Width="600px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>   
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                            </td>
                          <td>

                              <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar"  Width="150px" OnClick="btnCancelar_Click"  />
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
                     CssClass="gridView" Width="90%" EmptyDataText="No existen registros" 
                DataKeyNames="ID_EMPLEADO" meta:resourcekey="GvPlantasResource1"  onprerender="GvPlantas_PreRender" onrowdatabound="GvPlantas_RowDataBound" 
                     onselectedindexchanged="GvPlantas_SelectedIndexChanged"  >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="ID_EMPLEADO" SortExpression="ID_EMPLEADO" 
                             HeaderText="NO. EMPLEADO" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" SortExpression="Nombre" 
                             HeaderText="EMPLEADO" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField DataField="NombrePlanta" SortExpression="NombrePlanta" HeaderText="RANCHO" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField  DataField="id_Lider" SortExpression="id_Lider" 
                             HeaderText="NO. EMPLEADO JEFE" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre_Lider"  HeaderText="JEFE INMEDIATO"
                            SortExpression="Nombre_Lider" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                        <asp:BoundField DataField="fechaAlta"  HeaderText="Fecha Alta" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false"
                            SortExpression="fechaAlta" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                        <asp:BoundField DataField="fechaBaja"  HeaderText="Fecha Baja" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false"
                            SortExpression="fechaBaja" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
            
        </div> 
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
     <div id="popUpFecha" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="DateDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarFecha();" style="float:none;" /></td></tr>
            <tr><td><input type="button" id="btnCancelarFecha" value="Cancelar" onclick="limpiaFecha();" style="float:none;" /></td> </tr>
       </table>
    </div>
</asp:Content>



