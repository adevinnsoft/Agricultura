<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmPrestamoAsociados.aspx.cs" Inherits="frmPrestamoAsociados" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script src="../Scripts/jComparation.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <link href="../comun/css/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../comun/CSS/ui-lightness/jquery-ui-1.8.21.custom.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script type="text/javascript">
        var preview = false;
        var nivelP = 0;

        $(function () {
            //triggers();
            gvAsociadosLibres();

            $('#ctl00_ddlPlanta').live('change', function () {
                gvAsociadosLibres();
            });

            $("input.estados").change(function () {
                var box = $(this);
                var group = $("input.estados");
                $(group).prop("checked", false);
                $(box).prop("checked", true);
                
                //$('input.estados:checked').each(function () {
                var idEstado = $('input.estados:checked').attr('idEstado');
                if (idEstado == 0) {
                    $('#tablaAsociadosLider tbody tr').removeClass('invisible').show();
                }
                else {
                    $('#tablaAsociadosLider tbody tr').addClass('invisible');
                    $('#tablaAsociadosLider tbody tr[estado="' + idEstado + '"]').removeClass('invisible').show();
                }
                //});
            });
        });


        function gvAsociadosLibres() {
            $("input.estados").prop("checked", false);
            $($("input.estados")[0]).prop("checked", true);

            try {
                $.blockUI();

                PageMethods.tablaAsociadosLibres(function (result) {
                    $("#<%=divAsociadosLibres.ClientID %>").html(result);
                    $("#<%=divAsociadosLibres.ClientID %>").show();
                    if ($("#tablaAsociadosLibres").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pagerAsociadosLibres"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaAsociadosLibres").tablesorter({
                            widthFixed: true, widgets: ['zebra', 'filter'],
                            headers: { /*0: { filter: false} */
                            },
                            widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                    }
                    else {
                        $("#pagerAsociadosLibres").hide();
                        $("#tablaAsociadosLibres").html("<table><tr><td colspan='2'><h3 style='float:none !important;'>No hay Asociados libres por el momento </h3></td></tr></table>");
                    }
                    $.unblockUI(); gvAsociadosLider();
                }, function (e) {
                    $.unblockUI();
                    console.log(e);
                });
            } catch (err) {
                console.log(err); $.unblockUI();
            }
        }


        function gvAsociadosLider() {
            try {
                $.blockUI();

                PageMethods.tablaAsociadosLider(function (result) {
                    $("#<%=divAsociadosLider.ClientID %>").html(result);
                    $("#<%=divAsociadosLider.ClientID %>").show();
                    if ($("#tablaAsociadosLider").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pagerAsociadosLider"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaAsociadosLider").tablesorter({
                            widthFixed: true, widgets: ['zebra', 'filter'],
                            headers: { /*0: { filter: false} */
                            },
                            widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                    }
                    else {
                        $("#pagerAsociadosLider").hide();
                    }
                    $.unblockUI();
                    tooltip();
                }, function (e) {
                    $.unblockUI();
                    console.log(e);
                });
            } catch (err) {
                console.log(err); $.unblockUI();
            }
        }

        function btnClean() {


            if ($('.selGuardaNuevo').length) {
                popUpAlertConfirm('<h4>¿Existen relaciones sin guardar, desea limpiar?</h4>',
               '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();"><input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="cleanCallBack();">  ', 'warning');
            }
            else { cleanCallBack(); }
        }

        function cleanCallBack() {
            closeJsPopUpAux();
            $('.selGuardaNuevo').each(
                                function () {
                                    $("#" + $(this).val().split('|')[2]).attr('disabled', null);
                                    $("#label" + $(this).val().split('|')[2]).text('---');
                                }
                            );

            $('input[type="checkbox"]:checked').attr('checked', false);
            $("input.estados").prop("checked", false);
            $($("input.estados")[0]).prop("checked", true);
            $($("input.estados")[0]).change();
        }

        function prestarAsociados(accion) {
            if ($("input:checked.asociados" + accion).length) {
                $.blockUI();
                var idasociados = "";
                var familias = "";
                var niveles = "";
                var nombres = "";
                var grupo = "";

                $("input:checked.asociados" + accion).each(
                        function () {
                            idasociados += $(this).prop("checked") ? "|" + $(this).attr("id") : "";
                            nombres += "|" + $($(this).parent().children()[1]).text(); //.split('-')[1];
                            grupo += "|" + $(this).parent().next().text();
                        });

                        if (grupo.indexOf("---") >= 0) {
                            var arrgrupo = grupo.split('|');
                            var arrnombres = nombres.split('|');

                            var radios =
                        "<table>" +
                            "<tr style='text-align: left;'>" +
                                "<td style='width: 50px; vertical-align:top;'>" +
                                    "<label id='lbRFamilia'>*Familia:</label>" +
                                "</td>" +
                                "<td class='checkboxes'>" +
                                    "<div id='divRadiosFamilias'></div>" +
                                "</td>" +
                            "</tr>" +
                            "<tr style='text-align: left;'>" +
                                "<td style='width: 50px; vertical-align:top;'>" +
                                    "<label id='lbRNivel'>*Nivel:</label>" +
                                "</td>" +
                                "<td class='checkboxes'>" +
                                    "<div id='divRadiosNiveles'></div>" +
                                "</td>" +
                            "</tr>" +
                        "</table>";

                            var tabla = "<div style='margin:10px'><table id='singrupo'>";
                            $(arrgrupo).each(function (index, value) {
                                if (value == "---") {
                                    tabla += "<tr><td>" + arrnombres[index] + "</td></tr>";
                                }

                            }); tabla += "</table></div>";

                            //popUpAlert("uno o varios asociados no estan en algun grupo " + tabla, "warning");
                            popUpAlertButtons("<h4>Antes de seguir debe seleccionar Familia/Nivel para los siguientes asociados:</h4><br/>" + radios + tabla,
                                [
                                    ["Guardar", "guardarPrestamoAsociados('" + idasociados +"', '" + accion +"', false);"],
                                    ["Cancelar", "closeJsPopUp(); $.unblockUI();"]
                                ],
                                "warning",
                                500,
                                parseInt($(window).height() / 1.1),
                                true);

                            PageMethods.dibujaRadiosFamilias(function (result) {
                                try {
                                    $("#divRadiosFamilias").html(result);

                                    $("#divRadiosFamilias input[type='radio']").change(function () {
                                        if ($(this).attr("name") == "familias") {
                                            PageMethods.dibujaRadiosNiveles($(this).val().split('|')[0], function (result) {
                                                try {
                                                    $("#divRadiosNiveles").html(result);
                                                } catch (err) {
                                                    console.log(err); $.unblockUI();
                                                }
                                            });
                                        }
                                    });

                                } catch (err) {
                                    console.log(err); $.unblockUI();
                                }
                            });

                        } else {
                            guardarPrestamoAsociados(idasociados, accion, true);
                        }




            } else {
                        popUpAlert("<h4>Seleccione a los asociados que desea " + accion + "</h4>", "info");
            }
        }


        function guardarPrestamoAsociados(idasociados, accion, ban) {
            var idasociadosgrupo = "";
            var nivel = 0;

            if ($("#divRadiosNiveles").find("input:checked").length) {
                nivel = $("#divRadiosNiveles").find("input:checked").val();
                $("#singrupo tr").each(
                        function () {
                            idasociadosgrupo += "|" + $.trim(($(this).text().split('-')[0]));
                        });
                        ban = true;
            }

            if (ban) {
                PageMethods.guardaRelacion(idasociados, accion == 'Prestar' ? true : false, idasociadosgrupo, nivel, function (result) {
                    try {
                        $.unblockUI();
                        gvAsociadosLibres();
                        popUpAlert(result[1], result[0]);
                    } catch (err) {
                        console.log(err); $.unblockUI();
                    }
                });
            }
        }

        function tooltip() {
            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: true,
                trigger: 'hover',
                position: 'right'
            });
        }


    </script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        input[type="checkbox"], input[type="radio"]
        {
            display: none;
        }
        input[type="checkbox"] + label span
        {
            display: inline-block;
            width: 19px;
            height: 19px;
            margin: -1px 4px 0 0;
            vertical-align: middle;
            background: url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
            cursor: pointer;
        }
        
        input[type="radio"] + label span
        {
            display: inline-block;
            width: 19px;
            height: 19px;
            margin: -1px 4px 0 0;
            vertical-align: middle;
            background: url(../comun/img/check_radio_sheet.png) left -39px top no-repeat;
            cursor: pointer;
        }
        
        input[type="checkbox"]:checked + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
        }
        
        input[type="radio"]:checked + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
        }
        
        input[type="checkbox"]:disabled + label span
        {
            /*background:none;*/
            background: url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
        }
        
        input[type="radio"]:disabled + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
        }
        
        .check-with-label:checked + .label-for-check
        {
            font-weight: bold;
            color: #C12929;
        }
        
        .check-with-label:disabled + .label-for-check
        {
            color: gray;
        }
        
        .left
        {
            text-align: left !important;
        }
        
        #ctl00_ContentPlaceHolder1_divPreview h3
        {
            float: none !important;
        }
        #ctl00_ContentPlaceHolder1_divPreview .ui-accordion-content
        {
            background: #E5EED2;
            width: 89%;
        }
        #ctl00_ContentPlaceHolder1_divPreview .ui-accordion
        {
            min-width: 95%;
            margin: 10px;
        }
        
        .container
        {
            /*display: block;*/
        }
        
        table.index tr td table.gridView
        {
            min-width: inherit;
            max-width: inherit;
        }
        
        .pagedisplay
        {
            background: transparent !important;
        }
        
        .imgSinguardar
        {
            background-image: url('../comun/img/smallinfo.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 18px;
            height: 18px;
        }
        
        .imgGuardado
        {
            background: url("../comun/img/smallcheck.png") no-repeat;
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 18px;
            height: 18px;
        }
        
        .grid
        {
            width: 100%;
            max-width: 500px !important;
            float: left; /*min-width: 800px !important;*/
        }
        
        .grid table
        {
            width: 100%;
        }
        
        table.index
        {
            width: 100%;
            max-width: 1000px !important;
            min-width: 1000px !important;
            padding-top: 0px;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <asp:validationsummary id="validaciones" runat="server" validationgroup="valida"
            meta:resourcekey="validacionesResource1" />
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Prestamo de Asociados"></asp:label></h1>
        <table class="index">
            <tr>
                <td align="left" colspan="2">
                    <h2>
                        <asp:literal id="ltSubTitulo" text="Lista de Asociados Libres" runat="server"></asp:literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 50%;">
                    <div id="gvAsociadosLibres" class="grid">
                        <input id="libres" name="grupo" type="button" value="Tomar Asociados" onclick="prestarAsociados('Tomar');" />
                        <div id="pagerAsociadosLibres" class="pager" style="width: 100%; min-width: 100%;
                            display: none;">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                            <span class="pagedisplay"></span>
                            <img alt="next" src="../comun/img/next.png" class="next" />
                            <img alt="last" src="../comun/img/last.png" class="last" />
                            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                                <option value="30">30</option>
                                <option value="40">40</option>
                                <option value="50">50</option>
                                <option value="60">60</option>
                                <option value="70">70</option>
                            </select>
                            <select class="gotoPage" title="Select page number">
                            </select>
                        </div>
                        <div id="divAsociadosLibres" runat="server" />
                    </div>
                </td>
                <td style="vertical-align: top; width: 50%;">

                    <div>
                        <input type="checkbox" class="check-with-label estados" id="1" idEstado="0" /><label class='label-for-check' for="1"><span></span>Todos</label>
                        <input type="checkbox" class="check-with-label estados" id="2" idEstado="1" /><label class='label-for-check' for="2"><span></span>Propios</label>
                        <input type="checkbox" class="check-with-label estados" id="3" idEstado="2" /><label class='label-for-check' for="3"><span></span>Prestados</label>
                    </div>

                    <div id="gvAsociadosLider" class="grid">
                        <input id="ocupados" name="grupo" type="button" value="Soltar Asociados" onclick="prestarAsociados('Prestar');" />
                        <div id="pagerAsociadosLider" class="pager" style="width: 100%; min-width: 100%;
                            display: none;">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                            <span class="pagedisplay"></span>
                            <img alt="next" src="../comun/img/next.png" class="next" />
                            <img alt="last" src="../comun/img/last.png" class="last" />
                            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                                <option value="30">30</option>
                                <option value="40">40</option>
                                <option value="50">50</option>
                                <option value="60">60</option>
                                <option value="70">70</option>
                            </select>
                            <select class="gotoPage" title="Select page number">
                            </select>
                        </div>
                        <div id="divAsociadosLider" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6">
                    <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();" />
                </td>
            </tr>
        </table>
    </div>
</asp:content>