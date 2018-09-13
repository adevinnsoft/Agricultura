<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmDensidadesInvernadero.aspx.cs" Inherits="frmDensidadesInvernadero" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script type="text/javascript">
        $(function () {
            triggers();

            $('#ctl00_ddlPlanta').live('change', function () {
                dibujaTabla();
            });
        });

        function dibujaTabla() {
            $.blockUI();
            PageMethods.obtieneInvernaderosDensidad(function (result) {
                $("#<%=divtablaInvernadero.ClientID %>").html(result);
                if ($("#tablaInvernadero").find("tbody").find("tr").size() >= 1) {
                    $(".pager, .forma").show();
                    var pagerOptions = { // Opciones para el  paginador
                        container: $("#pager2"),
                        output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                    };

                    $("#tablaInvernadero").tablesorter({
                        widthFixed: true,
                        widgets: ['zebra', 'filter'],
                        headers: { 8: { filter: false }, 9: { filter: false }, 10: { filter: false} },
                        widgetOptions: {
                            zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */
                        }
                    }).tablesorterPager(pagerOptions);
                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                }
                else {
                    $(".pager, .forma").hide();
                }
                $.unblockUI();
                validar();
            });
        }

        function validar() {
            //$(".densidad input").inputmask('Regex', { regex: "^[0-9]{1,6}(\\.\\d{1,2})?$" });

            $(".densidad").change(function () {
                var fila = $(this).parent().parent(); //.split('-')[1];
                var id = fila.attr('id'); //.split('-')[1];


                if ($("#densidad-" + id).val() != $("#densidad-" + id).attr("vprev")) {
                    $("#densidad-" + id).removeClass("error");
                    $("#densidad-" + id).addClass("change");
                } else {
                    $("#densidad-" + id).removeClass("change");
                }

                var cal = (parseFloat($("#densidad-" + id).val()) / parseFloat($("#surco-" + id).text())).toFixed(2);
                $("#planta-" + id).text(isNaN(cal) ? 0 : cal);

                if ($("#densidad-" + id).hasClass("change")) {
                    if (!$("#densidad-" + id).hasClass("error")) {
                        fila.addClass("save");
                    }
                }
                else {
                    fila.removeClass("save");
                }
            });


            var posi;
            $('#<%=divtablaInvernadero.ClientID %> td').on('click', function () {
                var tdSelected = $(this);
                if (posi != (tdSelected.parent().index() + '' + tdSelected.index())) {
                    posi = tdSelected.parent().index() + '' + tdSelected.index();

                    var inputCreated = $(tdSelected).find('input');
                    $(inputCreated).focus();

                    $(inputCreated).on('keydown', function (e) {

                        switch (e.which) {
                            case 37:
                                var tdPadre = $(this).parent();
                                tdPadre.prev().click();
                                //tdPadre.prev().children().select();
                                break; //Izquierda
                            case 38:
                                var tdPadre = $(this).parent();
                                if (tdPadre.parent().prev().children().length) {
                                    //tdPadre.parent().prev().children()[tdPadre.index()].click();
                                    $(tdPadre.parent().prev().children()[tdPadre.index()]).children()[1].click();
                                }
                                break; //Arriba
                            case 39:
                                var tdPadre = $(this).parent();
                                tdPadre.next().click();
                                break; //Derecha
                            case 40:
                                var tdPadre = $(this).parent();
                                if (tdPadre.parent().next().children().length) {
                                    //tdPadre.parent().next().children()[tdPadre.index()].click();
                                    $(tdPadre.parent().next().children()[tdPadre.index()]).children()[1].click();
                                }
                                break; //Abajo
                            default:

                                break;
                        }
                    });
                    ultimaCelda = $(inputCreated);
                }
            });
        }

        function btnSave() {
            var registrar = true;
            $('.required').each(function () {
                if ($(this).val() == '') {
                    registrar = false;
                    $(this).addClass('error');
                }
                else {
                    $(this).removeClass('error');
                }
            });

            if (registrar) {
                if ($('.save').length) {
                    $.blockUI();

                    var matriz = '';
                    $('#<%=divtablaInvernadero.ClientID %> tbody tr.save').each(function (index) {
                        $(this).find('td').each(function (indexCol) {
                            if (indexCol == 0) {
                                matriz += $($(this)).attr('idinvernadero') + ',';
                            }
                            if (indexCol == 3) {
                                matriz += $($(this)).text() + ',';
                            }
                        });

                        matriz = matriz.substr(0, matriz.length - 1);
                        matriz += '|';
                    });

                    matriz = matriz.substr(0, matriz.length - 1);

                    PageMethods.guardaDensidad(matriz.split('|'), function (result) {
                        try {
                            $.unblockUI();
                            if (result[0] != "error") {
                                $(".change").removeClass('change');
                            }
                            popUpAlert(result[1], result[1]);
                            dibujaTabla();
                        } catch (err) { }
                    });
                } else {
                    if ($('.error').length) {
                        popUpAlert("No puede haber densidades negativas", "info");
                    }
                    else {
                        popUpAlert("No hay modificaciones para guardar", "info");
                    }
                }
            }
            else {
                popUpAlert('No se permiten valores vacios en el campo Densidad.', 'error');
            }
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function triggers() {
            dibujaTabla();

            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: false,
                trigger: 'hover',
                position: 'right'
            });
        }

    </script>
    <style type="text/css">
        .gridView {
            min-width: 100% !important;
        }
        
        input.error
        {
            border: 1px solid red !important;
            background: rgba(255,0,0,0.2) !important;
        }
        
        input.change, textarea.change, .change + .chosen-container .chosen-single
        {
            border: 1px solid #65AB1B;
            color: #FF8400;
            font-weight: bold;
            background: white;
        }
        
        .left
        {
            text-align: left !important;
        }
        
        table.index
        {
            width: 100%;
            max-width: 1000px !important;
            min-width: 1000px !important;
            padding-top: 0px;
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
        
        .invisible
        {
            display: none !important;
        }
        
        .grid table tr td
        {
            white-space: normal;
            padding-left: 1px;
            padding-right: 1px;
        }
        
        .tablesorter-header-inner
        {
            white-space: normal;
        }
        
        h1
        {
            width: 100%;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Densidad por Invernadero"></asp:label>
        </h1>
        <h2>Captura y modificación de Densidades por Invernaderos</h2>
<%--        <table class="index forma">
            <tr>
                <td align="right" colspan="2">
                    <input id="save" name="grupo" type="button" value="Guardar" onclick="btnSave();" />
                </td>
            </tr>
        </table>--%>
        <div id="pager2" class="pager" style="width: 100%; min-width: 100%;">
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
        <div id="divtablaInvernadero" runat="server">
        </div>
        <div>
            <input id="save" name="grupo" type="button" value="Guardar" onclick="btnSave();" />
        </div>
    </div>
</asp:content>
