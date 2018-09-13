<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmImportacionDeEjecucion.aspx.cs" Inherits="admin_ImportacionDeEjecucion" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="popUp" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script type="text/javascript">
            var todas = false;
            var seleccionando = false;
            var unCheck = false;

        function pageLoad() {
            var real = /^\d+\.\d+$/;
            var entero = /^\d+$/;
            var year = 2012;

           
            $(".valSemana").focusout(function () {
                if ($(this).val() < 0 || $(this).val() > 53 || $(this).val() == "" || !(entero.test($(this).val()))) {
                    popUpAlert("Debes anotar el número de la semana en el intervalo de 1 a 53", 'error');
                    $(this).attr('value', '');
                    var txtBox = $(this);
                    return false;
                }
            });

            $(".valFloat").focusout(function () {
                if (!validar($(this).val())) {
                    popUpAlert('El valor insertado no tiene el formato correcto.', 'error');
                    $(this).attr('value', '');
                    var txtBox = $(this);
                    return false;
                }
            });

            $('#<%=chksPlantas.ClientID%>').find('input[type="checkbox"]').change(function () {
                var isAllCk = $(this).parent().has('.todos').length > 0;

                if (isAllCk) {
                
                
                }
                //console.log("check " + $('.Plantas').find('span.invisible.todos').parent().parent().find('input[type="checkbox"]').prop("checked"));
                if (!seleccionando && !unCheck) {
                    if (todas && $('.Plantas').find('span.invisible.todos').parent().parent().find('input[type="checkbox"]').prop("checked")
                            && $('.Plantas input[type="checkbox"]:not(:checked)').length > 0) {
                        unCheck = true;
                        $('.Plantas').find('span.invisible.todos').parent().parent().find('input[type="checkbox"]').prop("checked", false);
                        todas = false;
                        unCheck = false;
                    } else if (
                            !isAllCk && (
                                $('.Plantas input[type="checkbox"]:checked').length
                                + $('.Plantas span.invisible.todos').parent().parent().find('input[type="checkbox"]:not(":checked")').length
                            ) == $('.Plantas input[type="checkbox"]').length) {
                        unCheck = true;
                        $('.Plantas').find('span.invisible.todos').parent().parent().find('input[type="checkbox"]').prop("checked", true);
                        todas = true;
                        unCheck = false;

                    }

                    if (todas != $('.Plantas').find('span.invisible.todos').parent().parent().find('input[type="checkbox"]').prop("checked")) {

                        todas = !todas;
                        seleccionando = true;
                        $('#<%=chksPlantas.ClientID%>').find('input[type="checkbox"]').prop('checked', todas);
                        //console.log("ahora check " + todas);
                        seleccionando = false;
                        $($('.Plantas input[type="checkbox"]:checked')[0]).change();
                        return;
                    }

                    var idsPlantas = "";
                    $('.Plantas input[type="checkbox"]:checked').each(function () {
                        if (!$(this).next().find('span.invisible').hasClass('todos')) {
                            var idPlanta = $(this).next().find('span.invisible').text();
                            idsPlantas += idPlanta + ",";
                        }
                    });
                    $('#<%=idsPlantas.ClientID%>').val(idsPlantas.substr(0, idsPlantas.length - 1));
                    //console.log(idsPlantas);


                } else {
                    //console.log("seleccionado:" + seleccionando);
                }
            });

            //Cargar la semana y año actuales
            var semanaActual = parseInt($('span.semana').find('span').text());
            var anioActual = new Date();
            anioActual = anioActual.getFullYear();
            //Seleccionamos por default el año y semana actuales
            $('.ddlSemanas > option[value="' + semanaActual + '"]').attr('selected', 'selected');
            $('.ddlAnios > option[value="' + anioActual + '"]').attr('selected', 'selected');
            $('.Plantas span.invisible.todos').parent().parent().find('input[type="checkbox"]').prop("checked", true).change();

        }

        function validar(valor) {
            var real = /^\d+\.\d+$/;
            var entero = /^\d+$/;
            if (real.test(valor) || entero.test(valor))
                return true;
            else
                return false;
        }


        function ValidaSeleccion() {
            if ($('#<%=idsPlantas.ClientID%>').val() != "") {
                return true;
            }
            else {
                popUpAlert("Falta seleccionar almenos una planta para importación", "info");
                return false;
            }
        }

        //        function cargarCombos() {
        //            bloqueoDePantalla.bloquearPantalla();
        //            try {
        //                PageMethods.obtenerPlantasYSemanas(function (response) {
        //                    if (response[0] == '1') {
        //                        //$('#Plantas').append(response[2]);
        //                        $('.ddlAnios').append(response[2]);
        //                        $('.ddlSemanas').append(response[3]);
        //                        cargarSemanaAnioActuales();
        //                        concatenarIdsPlantas();
        //                        bloqueoDePantalla.indicarTerminoDeTransaccion();
        //                    } else {
        //                        popUpAlert(response[0], response[1]);
        //                        bloqueoDePantalla.indicarTerminoDeTransaccion();
        //                    }
        //                }, function (e) {
        //                    console.log(e);
        //                    bloqueoDePantalla.indicarTerminoDeTransaccion();
        //                });
        //            } catch (e) {
        //                console.log(e);
        //                bloqueoDePantalla.indicarTerminoDeTransaccion();
        //            }
        //            bloqueoDePantalla.desbloquearPantalla();
        //        }
        
        
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            Importación de la Ejecución</h1>
        <table class="index">
            <tr>
                <td colspan="7">
                    <h2>
                        Importación</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="lblPlantas" runat="server" text="Plantas:"></asp:label>
                </td>
                <td style="width: 300px;">
                    <asp:checkboxlist runat="server" id="chksPlantas" repeatdirection="Horizontal" repeatcolumns="3"
                        cssclass="Plantas">
                    </asp:checkboxlist>
                    <asp:hiddenfield id="idsPlantas" runat="server" />
                </td>
                <td>
                    <asp:label id="Label1" runat="server" text="Año:"></asp:label>
                </td>
                <td>
                    <asp:dropdownlist runat="server" id="ddl_Anio" cssclass="ddlAnios">
                    </asp:dropdownlist>
                </td>
                <td>
                    <asp:label id="Label2" runat="server" text="Semana:"></asp:label>
                </td>
                <td>
                    <asp:dropdownlist runat="server" id="ddl_Semana" cssclass="ddlSemanas">
                    </asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:literal id="ltImportar" runat="server">Importar Archivo (XLS)</asp:literal>
                </td>
                <td>
                    <asp:fileupload id="File1" runat="server" style="float: left" />
                </td>
                <td style="width: 95px; text-align: left !important;" colspan="4">
                    <asp:checkbox runat="server" id="CheckSobreescribirCiclos" text="Sobreescribir Ciclos" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:button id="btnImportar" runat="server" text="Importar" onclick="btnImportar_Click"
                        onclientclick="return ValidaSeleccion();"></asp:button>
                </td>
            </tr>
        </table>
        <div class="grid" id="divGrid" runat="server">
            <table cellspacing="0" rules="all" border="1" id="gv_Ciclos" style="border-collapse: collapse;">
            </table>
        </div>
        <popUp:popUpMessageControl ID="popUpMessageControl1" runat="server"></popUp:popUpMessageControl>
    </div>
</asp:content>
