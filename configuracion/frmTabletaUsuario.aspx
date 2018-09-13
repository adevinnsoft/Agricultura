<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmTabletaUsuario.aspx.cs"  EnableEventValidation="false" Inherits="configuracion_frmTabletaUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli" Text="Asignar Código de Tableta a Usuario"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltRancho" runat="server" meta:resourceKey="ltRanchoResource" Text="Usuario"></asp:Literal>
                            </td>
                            <td class="auto-style5" colspan="1" dir="ltr">
                                <asp:DropDownList ID="ddlUsuario" runat="server" ></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="1">
                                <asp:Literal ID="ltComplete0" runat="server" meta:resourcekey="ltCompleteResource1" Text="Activo"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkEstatus" runat="server" meta:resourcekey="chkActivoResource1" />
                            </td>
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
                DataKeyNames="idEmpleado"  >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>

                        <asp:BoundField DataField="idEmpleado" SortExpression="idEmpleado"
                            HeaderText="No. Empleado">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="vNombre" SortExpression="vNombre"
                            HeaderText="Nombre">
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha" SortExpression="fecha"
                             HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                          
                        </asp:BoundField>
    
                        <asp:BoundField DataField="Estatus" SortExpression="Estatus"
                            HeaderText="Estatus" />
                   
                      
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



