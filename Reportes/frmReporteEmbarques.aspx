<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteEmbarques.aspx.cs" Inherits="frmReporteEmbarques" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
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
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var spanish = '<%= Session["Locale"].ToString() %>' == 'es-MX' ? true : false;
        var lastFila = null;
        //var partidaTem = "";

        $(function () {
            gvEmbarques();
            triggers();
        });

        function gvEmbarques() {
            try {
                $.blockUI();

                $('#btnRegresar').hide();
                $("#gvPartidas").hide();
                $("#gvEmbarques").show();
                $(".index").hide();

                PageMethods.obtineEmbarques(function (result) {
                    $("#<%=divEmbarques.ClientID %>").html(result);
                    $("#<%=divEmbarques.ClientID %>").show();
                    if ($("#tablaEmbarques").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pagerEmbarques"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaEmbarques").tablesorter({
                            widthFixed: true, widgets: ['zebra', 'filter'],
                            headers: { /*0: { filter: false} */
                            },
                            widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                    }
                    else {
                        $("#pagerEmbarques").hide();
                    }
                    $.unblockUI();
                });
            } catch (err) { $.unblockUI(); }
        }

        function gvPartidas(fila) {
            try {
                //if (fila != lastFila) {
                lastFila = fila;
                $.blockUI();

                $('#btnRegresar').show();
                $("#<%=lblCaptura.ClientID %>").text($(fila).attr("idEmbarque"));
                $("#gvEmbarques").hide();
                $("#gvPartidas").show();
                $(".index").show();

                PageMethods.obtienePartidas($(fila).attr("idEmbarque"), $(fila).attr("comentario"), function (result) {
                    //partidaTem = result;
                    $("#<%=divPartidas.ClientID %>").html(result);
                    $("#<%=divPartidas.ClientID %>").show();
                    if ($("#tablaPartidas").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pagerPartidas"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaPartidas").tablesorter({
                            widthFixed: true,
                            widgets: ['zebra', 'filter'],
                            headers: { /*6: { filter: false }, 7: { filter: false}*/ },
                            widgetOptions: { zebra: ["gridView", "gridViewAlt"]/*filter_hideFilters: true // Autohide*/ }
                        }).tablesorterPager(pagerOptions);

                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                    }
                    else {
                        $("#pagerPartidas").hide();
                    }

                    triggers();
                    $.unblockUI();
                });
                //} else {
                //   btnCancel();
                //}
            } catch (err) { $.unblockUI(); }
        }

        function btnCancel() {
            if (lastFila != null || $(".save").length) {
                gvPartidas(lastFila);

                //                $('#btnRegresar').show();
                //                $("#<%=lblCaptura.ClientID %>").text($(lastFila).attr("idEmbarque"));
                //                $("#gvEmbarques").hide();
                //                $("#gvPartidas").show();

                //                $.blockUI();
                //                $("#<%=divPartidas.ClientID %>").html(partidaTem);
                //                $("#<%=divPartidas.ClientID %>").show();
                //                if ($("#tablaPartidas").find("tbody").find("tr").size() >= 1) {
                //                    var pagerOptions = { // Opciones para el  paginador
                //                        container: $("#pagerPartidas"),
                //                        output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                //                    };

                //                    $("#tablaPartidas").tablesorter({
                //                        widthFixed: true,
                //                        widgets: ['zebra', 'filter'],
                //                        headers: { 6: { filter: false }, 7: { filter: false} },
                //                        widgetOptions: { zebra: ["gridView", "gridViewAlt"]/*filter_hideFilters: true // Autohide*/ }
                //                    }).tablesorterPager(pagerOptions);

                //                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                //                }
                //                else {
                //                    $("#pagerPartidas").hide();
                //                }

                //                triggers();
                //                $.unblockUI();
            }
        }

        function btnClean() {
            //lastFila = null;
            $("#<%=divPartidas.ClientID %>").hide();
            $('#btnRegresar').hide();
            $("#<%=lblCaptura.ClientID %>").text("");
            $("#gvEmbarques").show();
            $("#gvPartidas").hide();
            $(".index").hide();
        }

        function btnSave() {
            try {
                if ($('.save').length) {
                    $.blockUI();
                    var idEmbarque = "";
                    var comentario = "";
                    var ids = "";
                    var fechas = "";
                    var destinos = "";
                    var productos = "";
                    var cajas = "";
                    var ordenes = "";
                    var comentarios = "";

                    comentario = $("#comentario").val();

                    $('.save').each(
                        function () {
                            idEmbarque = $(this).attr('idembarque');
                            ids += '|' + $(this).attr('idpartida');
                            fechas += '|' + $(this).find(".datePicker").val();
                            destinos += '|' + $(this).find(".destino").val();
                            productos += '|' + $(this).find(".producto").val();
                            cajas += '|' + $(this).find(".cajas").val();
                            ordenes += '|' + $(this).find(".orden").val();
                            comentarios += '|' + $(this).find(".comentarios").val();
                        }
                    );

                        PageMethods.guardaPartidasCambios(idEmbarque, comentario, ids, fechas, destinos, productos, cajas, ordenes, comentarios, function (result) {

                            //                        PageMethods.obtineEmbarques(function (result2) {
                            //                            $("#<%=divEmbarques.ClientID %>").html(result2);
                            //                            $("#<%=divEmbarques.ClientID %>").show();
                            //                            if ($("#tablaEmbarques").find("tbody").find("tr").size() >= 1) {
                            //                                var pagerOptions = { // Opciones para el  paginador
                            //                                    container: $("#pagerEmbarques"),
                            //                                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                            //                                };
                            //                                $("#tablaEmbarques").tablesorter({
                            //                                    widthFixed: true, widgets: ['zebra', 'filter'],
                            //                                    headers: { /*0: { filter: false} */
                            //                                    },
                            //                                    widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                            //                                }).tablesorterPager(pagerOptions);
                            //                                $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                            //                            }
                            //                            else {
                            //                                $("#pagerEmbarques").hide();
                            //                            }
                            //                            $.unblockUI();
                            //                        });

                            var sum = 0;
                            $('.cajas').each(function () {
                                sum += parseInt($(this).val());
                            });
                            $($("#tablaEmbarques tr[idembarque=" + idEmbarque + "]").children()[4]).text(sum);
                            $("#tablaEmbarques tr[idembarque=" + idEmbarque + "]").attr("comentario", comentario);

                            $.unblockUI();

                            if (result[0] == "ok") {
                                $(".change").removeClass('change');
                                $(".save").removeClass('save');
                            }
                            popUpAlert(result[1], result[0]);

                        });
                } else {
                    popUpAlert("No hay cambios en las partidas", "info");
                }
            } catch (err) { $.unblockUI(); }
        }


        function gvCallBack(result) {
            $("#<%=divPartidas.ClientID %>").html(result);
            gvTablesorter();
        }

        function triggers() {

            $(".datePicker").prop('readonly', true).datepicker(
                        {
                            dateFormat: "yy-mm-dd",
                            buttonImage: "../comun/img/calendario.png",
                            showOn: "both",
                            dayNames: spanish ? ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"] : ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
                            dayNamesMin: spanish ? ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"] : ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
                            dayNamesShort: spanish ? ["Dom", "Lun", "Mar", "Mier", "Jue", "Vie", "Sab"] : ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                            monthNames: spanish ? ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"] : ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
                            monthNamesShort: spanish ? ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"] : ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                            changeYear: false,
                            changeMonth: true//,
                            //minDate: '-15D',
                            //maxDate: 0
                        }
                    );

            $(".chosen").chosen({ width: "100%" });

            $('.chosen').each(function () {
                var idproducto = $(this).parent().parent().attr('idProducto');
                var iddestino = $(this).parent().parent().attr('iddestino');
                var orden = $(this).parent().parent().attr('orden');
                if ($(this).hasClass("producto")) { $(this).val(idproducto).trigger("chosen:updated"); }
                if ($(this).hasClass("destino")) { $(this).val(iddestino).trigger("chosen:updated"); }
                if ($(this).hasClass("orden")) { $(this).val(orden).trigger("chosen:updated"); }
            });

            $(".destino").chosen().change(function (event) {
                console.log($(this).val());
                var max = 0;
                var add = false;
                var change = this;
                $('.chosen').each(function () {
                    if ($(change).val() == $(this).val() && this != change) {
                        $($(change).parent().next().next().next().children()[1]).val($($(this).parent().next().next().next().children()[1]).val()).trigger("chosen:updated");
                        add = true;
                        console.log("si");
                        //return true;
                    } else {
                        console.log("no");
                    }
                    if (max <= $($(this).parent().next().next().next().children()[1]).val()) { max = parseInt($($(this).parent().next().next().next().children()[1]).val()) + 1; }
                });


                if (!add) {
                    $('.orden').each(function () {
                        if (($(this).parent().prev().prev().prev().children()[1]) != change) {
                            if ($(this).find("option[value=" + max + "]").length == 0) {
                                $(this).append("<option value='" + max + "'>" + max + "</option>");
                            }
                        }
                        else {
                            if ($(this).find("option[value=" + max + "]").length > 0) {
                                $(this).find("option[value=" + max + "]").prop('selected', true);
                            }
                            else {
                                $(this).append("<option value='" + max + "' selected='selected'>" + max + "</option>");
                            }

                            $(this).attr("previo", max);
                        }

                    });

                    $('.orden').chosen().trigger("chosen:updated");
                }

                $('.orden').each(function () {
                    if ($(this).val() != $(this).parent().parent().attr("orden")) {
                        $(this).next().children().addClass("change");
                        $(this).parent().parent().addClass("save");
                    } else {
                        $(this).next().children().removeClass("change");
                        $(this).parent().parent().removeClass("save");
                    }
                });
            });

            $(".orden").chosen().change(function (event) {
                var orden = $(this).val();
                var destino = $($(this).parent().prev().prev().prev().children()[1]).val();
                var prev = $(this).attr("previo");

                $('.orden').each(function () {
                    if ($($(this).parent().prev().prev().prev().children()[1]).val() == destino) {
                        $(this).val(orden);
                        $(this).attr("previo", orden);
                    } else {
                        if ($(this).val() == orden) {
                            $(this).val(prev);
                            $(this).attr("previo", prev);
                        }
                    }

                    if ($(this).val() != $(this).parent().parent().attr("orden")) {
                        $(this).next().children().addClass("change");
                        $(this).parent().parent().addClass("save");
                    } else {
                        $(this).next().children().removeClass("change");
                    }
                });

                $('.orden').chosen().trigger("chosen:updated");
                $(this).parent().parent().removeClass("save");
            });


            $(".destino, .orden, .producto").chosen().change(function (event) {
                if ($(this).hasClass("destino")) {
                    if ($(this).val() != $(this).parent().parent().attr("iddestino")) {
                        $(this).next().children().addClass("change");
                    } else {
                        $(this).next().children().removeClass("change");
                    }
                }

                if ($(this).hasClass("orden")) {
                    if ($(this).val() != $(this).parent().parent().attr("orden")) {
                        $(this).next().children().addClass("change");
                    } else {
                        $(this).next().children().removeClass("change");
                    }
                }

                if ($(this).hasClass("producto")) {
                    if ($(this).val() != $(this).parent().parent().attr("idProducto")) {
                        $(this).next().children().addClass("change");
                    } else {
                        $(this).next().children().removeClass("change");
                    }
                }

                if ($(this).parent().parent().html().indexOf("change") != -1) {
                    $(this).parent().parent().addClass("save");
                } else {
                    $(this).parent().parent().removeClass("save");
                }
            });

            $(".datePicker, .cajas").change(function (event) {
                if ($(this).hasClass("datePicker")) {
                    if ($(this).val() != $(this).parent().parent().attr("fecha")) {
                        $(this).addClass("change");
                    } else {
                        $(this).removeClass("change");
                    }
                }

                if ($(this).hasClass("cajas")) {
                    if ($(this).val() != $(this).parent().parent().attr("cajas")) {
                        $(this).addClass("change");

                        if ($(this).val() == "" || parseInt($(this).val()) == 0) {
                            $(this).val($(this).prev().text());
                            $(this).removeClass("change");
                        }
                    } else {
                        $(this).removeClass("change");

                    }
                }

                if ($(this).parent().parent().html().indexOf("change") != -1) {
                    $(this).parent().parent().addClass("save");
                } else {
                    $(this).parent().parent().removeClass("save");
                }
            });
        }

        function setTooltips() {
            if ($(".help.tooltipstered").length) {
                $('.help.tooltipstered').tooltipster("destroy");
            }

            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: true,
                trigger: 'hover',
                position: 'right',
                contentAsHTML: true
            });
        }

    </script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        input.Error
        {
            border: 1px solid red !important;
            background: rgba(255,0,0,0.2);
        }
        
        .change
        {
            border: 1px solid #65AB1B !important;
            color: #FF8400 !important;
            font-weight: bold !important;
            background: white;
        }
        
        .focus
        {
            border: transparent 1px solid !important;
            background: none;
            border-style: none;
            box-shadow: none !important;
        }
        
        .focus:focus
        {
            border: 1px black solid !important;
            background: white;
            border-style: none;
        }
        
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
        
        .grid
        {
            padding-top: 0px;
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
        
        img#btnRegresar
        {
            vertical-align: middle;
            margin-right: 15px;
            display: none;
        }
        
        .datePicker
        {
            width: 80px;
        }
        
        .producto
        {
            width: 200px;
        }
        
        .destino
        {
            width: 180px;
        }
        
        .cajas
        {
            width: 80px;
        }
        
        .grid table
        {
            width: 1000px !important;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Reporte de Embarques"></asp:label>
        </h1>
        <h1>
            <img src="../comun/img/regresar.png" alt="Regresar" id="btnRegresar" onclick="btnClean();" />
            <asp:label runat="server" id="lblCaptura" text=""></asp:label>
        </h1>

        <div id="gvEmbarques" class="grid" style="display: none;">
            <%--<input id="grupo" name="grupo" type="button" value="Agregar Grupo" onclick="addGrupo();"/>--%>
            <div id="pagerEmbarques" class="pager" style="width: 100%; min-width: 100%;">
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
            <div id="divEmbarques" runat="server" class="grid" />
        </div>

        <div id="gvPartidas" class="grid">
            <div id="pagerPartidas" class="pager" style="width: 100%; min-width: 100%;">
                <img alt="first" src="../comun/img/first.png" class="first" />
                <img alt="prev" src="../comun/img/prev.png" class="prev" />
                <span class="pagedisplay"></span>
                <img alt="next" src="../comun/img/next.png" class="next" />
                <img alt="last" src="../comun/img/last.png" class="last" />
                <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                    <option value="10">10</option>
                    <option value="20">20</option>
                    <option value="30">30</option>
                    <option value="40">40</option>
                    <option value="50">50</option>
                </select>
                <select class="gotoPage" title="Select page number">
                </select>
            </div>
            <div id="divPartidas" runat="server" class="GridViewContainer gridView" />
        </div>

                <table class="index" style="display:none;">
            <%--<tr>
                <td align="left" colspan="10">
                    <h2>
                        <asp:literal id="ltSubTitulo" text="Embarque" runat="server"></asp:literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="lblEmbarque" runat="server" text="Embarque:"></asp:label>
                </td>
                <td class="left">
                    <asp:label id="txtEmbarque" runat="server" text=""></asp:label>
                </td>
                <td>
                    <asp:label id="lblFechaOrigen" runat="server" text="Fecha Origen:"></asp:label>
                </td>
                <td class="left">
                    <asp:label id="txtFechaOrigen" runat="server" text=""></asp:label>
                </td>
                <td>
                    <asp:label id="lblOrigen" runat="server" text="Origen:"></asp:label>
                </td>
                <td class="left">
                    <asp:label id="txtOrigen" runat="server" text=""></asp:label>
                </td>
                <td>
                    <asp:label id="lblSemana" runat="server" text="Semana:"></asp:label>
                </td>
                <td class="left">
                    <asp:label id="txtSemana" runat="server" text=""></asp:label>
                </td>
                <td>
                    <asp:label id="lblTotalCajas" runat="server" text="Total Cajas:"></asp:label>
                </td>
                <td class="left">
                    <asp:label id="txtTotalCajas" runat="server" text=""></asp:label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="lblPartida" runat="server" text="Partida:"></asp:label>
                </td>
                <td class="left">
                    <asp:label id="txtPartida" runat="server" text=""></asp:label>
                </td>
                <td>
                    <asp:label id="lblFechaDestino" runat="server" text="*Fecha Destino:"></asp:label>
                </td>
                <td class="left">
                    <input type="text" id="txtFechaDestino" maxlength="10" />
                </td>
                <td>
                    <asp:label id="lblDestino" runat="server" text="*Destino:"></asp:label>
                </td>
                <td id="comboDestinos" class="left"></td>
                <td>
                    <asp:label id="lblProductos" runat="server" text="*Producto:"></asp:label>
                </td>
                <td id="comboProductos" class="left"></td>
                <td>
                    <asp:label id="lblCajas" runat="server" text="*Cajas:"></asp:label>
                </td>
                <td class="left">
                    <input type="text" id="txtCajas" maxlength="6" />
                </td>
            </tr>

            <tr align="right" colspan="10">
                <td>
                    <asp:label id="lblEstado" runat="server" text="*Estado:"></asp:label>
                </td>
                <td id="comboEstados" class="left"></td>
            </tr>--%>
            <tr>
                <td align="right" colspan="10">
                    <input id="save" class="btnSave" name="grupo" type="button" value="Guardar" onclick="btnSave();" />
                    <input id="cancel" class="btnCancel" name="grupo" type="button" value="Cancelar"
                        onclick="btnClean();" />
                    <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnCancel();" />
                </td>
            </tr>
        </table>

    </div>
    

    
</asp:content>