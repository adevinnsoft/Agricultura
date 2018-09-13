<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmCiclos.aspx.cs" Inherits="frmCiclos" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script src="../comun/scripts/jquery.mask.js" type="text/javascript"></script>
    <style type="text/css">
        .change
        {
            border: 1px solid #65AB1B;
            color: #FF8400;
            font-weight: bold;
            background: white;
        }
        
        table.index tr td {
            text-align: center !important;
        }
        
        h1 {
            width: 100% !important;
        }
        
        div.nav {
            box-shadow: 0 0 0px #999999 !important;
        }
        div#ctl00_ContentPlaceHolder1_divCiclos {
            width: 1024px;
            overflow: auto;
        }
        
        table.index {
            width: 100%;
            min-width: 100%;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            triggers();

            $('#ctl00_ddlPlanta').live('change', function () {
                dibujaTabla();
            });
        });

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

        function dibujaTabla() {
            try {
                $.blockUI();
                $("#gvCiclos").show();

                PageMethods.obtineCiclos(function (result) {
                    $("#<%= divCiclos.ClientID%>").html(result);
                    $("#<%= divCiclos.ClientID%>").show();
                    if ($("#tablaCiclos").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pagerCiclos"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaCiclos").tablesorter({
                            widthFixed: true, widgets: ['zebra', 'filter'],
                            headers: { /*0: { filter: false} */
                            },
                            widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                    }
                    else {
                        $("#pagerCiclos").hide();
                    }
                    funcionesDelGrid();
                    $.unblockUI();
                });
            } catch (err) { $.unblockUI(); }
        }

        function funcionesDelGrid() {
            $(".edit").change(function () {
                var fila = $(this).parent();
                //var id = fila.attr('idRealCiclo');

                if ($(this).hasClass("Boolean")) {
                    if ($(this).find("input").prop("checked") != parseBoolean($(this).attr("vprev"))) {
                        //$(this).removeClass("error");
                        $(this).addClass("change");
                    } else {
                        $(this).removeClass("change");
                    }
                }
                else {
                    if ($(this).find("input").val() != $(this).attr("vprev")) {
                        //$(this).removeClass("error");
                        $(this).addClass("change");
                    } else {
                        $(this).removeClass("change");
                    }
                }

                if ($(fila).find("td").hasClass("change")) {
                    fila.addClass("save");
                }
                else {
                    fila.removeClass("save");
                }
            });

            $('#tablaCiclos td').click(function () {
                var tdSelected = $(this);
                var tdValue = $(this).html();

                //if (tdValue.indexOf('-') > 0)
                //    return;

                if (tdValue.indexOf('input') > 0) {
                    $(tdSelected).find('input').focus();
                }
                else {
                    $(this).html('<input type="' + ($(this).hasClass('Boolean') ? 'checkbox' : 'text') + '" ' + ($(this).hasClass("readonly") ? "readonly" : "") + ' value="' + tdValue + '" maxlength="' + $(this).attr("longitud") + '" style="width:' + ($(this).attr("Longitud") * 10) + 'px; text-align:center; padding: 0px 0px; border:none;" class="' + $(this).attr("class") + '" />');
                    if ($(this).hasClass("Boolean")) {
                        $(this).children().prop("checked", parseBoolean(tdValue));
                        if ($(this).hasClass("readonly")) {
                            $(this).children().attr("onclick", "return false;");
                        }
                    }
                }

                $('.Int32 input').mask('0#');

                var inputCreated = $(tdSelected).find('input');
                $(inputCreated).focus();
                $(inputCreated).focusout(function () {
                    if ($(this).hasClass("Boolean")) {
                        $(tdSelected).html($(this).prop("checked") ? "True" : "False");
                        if ($(tdSelected).attr("vprev") == "") {
                            $(tdSelected).addClass("change");
                            $(tdSelected).parent().addClass("save");
                        }
                    }
                    else {
                        $(tdSelected).html($(this).val());
                    }
                });

                $(inputCreated).on('keydown', function (e) {
                    switch (e.which) {
                        case 37: //Izquierda
                            var tdPadre = $(this).parent();
                            tdPadre.prev().click();
                            break;
                        case 38: //Arriba
                            var tdPadre = $(this).parent();
                            if (tdPadre.parent().prev().children().length) {
                                tdPadre.parent().prev().children()[tdPadre.index()].click();
                            }
                            break;
                        case 9: //Derecha
                            var tdPadre = $(this).parent();
                            tdPadre.next().click();
                            break;
                        case 39: //Derecha
                            var tdPadre = $(this).parent();
                            tdPadre.next().click();
                            break;
                        case 40: //Abajo
                            var tdPadre = $(this).parent();
                            if (tdPadre.parent().next().children().length) {
                                tdPadre.parent().next().children()[tdPadre.index()].click();
                            }
                            break;
                        default:
                            break;
                    }
                });
                ultimaCelda = $(inputCreated);
            });
        }

        $(function () {
            $('#btn_GuardarTabla').click(function () {
                $.blockUI();
                if ($('.save').length > 0) {
                    var matriz = '';

                    $('#tablaCiclos tbody tr.save').each(function () {
                        matriz += $($(this)).attr('idrealciclo') + ',';
                        $(this).find('td.edit').each(function () {
                            matriz += $(this).text().trim() + ',';
                        });
                        matriz = matriz.substr(0, matriz.length - 1);
                        matriz += '|';
                    });
                    matriz = matriz.substr(0, matriz.length - 1);

                    PageMethods.GuardarCiclos(matriz.split('|'), function (resultado) {
                        popUpAlert(resultado[1], resultado[0]);
                        dibujaTabla();
                    });
                }
                else {
                    popUpAlert('No hay datos para guardar', 'info');
                }
                $.unblockUI();
            });
        });


        function parseBoolean(string) {
            var bool;
            bool = (function () {
                switch (false) {
                    case string.toLowerCase() !== 'true':
                        return true;
                    case string.toLowerCase() !== 'false':
                        return false;
                }
            })();
            if (typeof bool === "boolean") {
                return bool;
            }
            return void 0;
        };
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <%--<div class="grid" id="divGrid" runat="server" />--%>
        <h1>Ciclos
        </h1>
        <asp:hiddenfield id="hddTabla" runat="server" />

        <div id="pagerCiclos" class="pager" style="width: 100%; min-width: 100%;">
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
        <div id="divCiclos" runat="server" class="gridView" />
        <uc1:popUpMessageControl runat="server" ID="popUpMessage" />
        <input type="hidden" id="idCiclos" value="" />
        <table class="index">
            <tr>
                <td colspan="4">
                    <input type="button" id="btn_GuardarTabla" value="Guardar" />
                </td>
            </tr>
        </table>
        </div>
</asp:content>
