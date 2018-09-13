<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteCumplimientoInvCosechados.aspx.cs" Inherits="frmReporteCumplimientoInvCosechados"
    meta:resourcekey="PageResource1" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <%--<script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>--%>
    <!--[if lt IE 9]>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <![endif]-->
    <!--[if gte IE 9]><!-->
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <!--<![endif]-->
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../comun/CSS/ui-lightness/jquery-ui-1.8.21.custom.css" />
    <script src="../comun/scripts/jsZip/jszip.js" type="text/javascript"></script>
    <link href="../comun/css/dx.common.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/dx.light.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/dx.all.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            triggers();
            //$('#ctl00_ddlPlanta').change(function () {
            //    //triggers();
            //    gvReporte();
            //});
        });

        function gvReporte() {
            try {
                $.blockUI();
                var dia = $(".datepicker").val();
                var variety = $("#ddlVariedades").val();
                var idPlanta = $('#ctl00_ddlPlanta').val();

                //if (variety != 0) {
                PageMethods.ObtenerReporte(dia, variety, idPlanta, function (result) {
                    if (result[0] == 'ok') {

                       <%-- /*
                        $(".gridView").trigger("destroy");
                        $('#tblReporte').html(result[1]);

                        if ($("#tblReporte").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                        container: $("#pagerReporte"),
                        output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tblReporte").tablesorter({
                        widthFixed: true, widgets: ['zebra', 'filter'],
                        headers: {},
                        widgetOptions: { zebra: ["gridView", "gridViewAlt"] }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                        }
                        else {
                        $("#pagerReporte").hide();
                        }
                        */--%>

                        $("#gridContainer").dxDataGrid({
                            dataSource: JSON.parse(result[1]),
                            allowColumnResizing: false,
                            columnAutoWidth: true,
                            showColumnLines: false,
                            showRowLines: false,
                            rowAlternationEnabled: true,
                            showBorders: false,
                            rowTemplate: function (container, item) {

                                if (null == item.data.FechaActividad) {
                                    item.data.FechaActividad = "N/D";
                                }
                                if (null == item.data.FechaProgramo) {
                                    item.data.FechaProgramo = "N/D";
                                }
                                if (null == item.data.FechaEjecucion) {
                                    item.data.FechaEjecucion = "N/D";
                                }
                                var data = item.data, markup = "<tbody onmouseover='tooltip();' class='tblInv " + ((item.rowIndex % 2) ? 'dx-row-alt' : '') + "'>" +
                                            "<tr class='dx-row main-row'>" +
                                            "<td>" + data.Invernadero + "</td>" +
                                            "<td>" + data.Variedad + "</td>" +
					                        "<td>" + data.FechaActividad.replace("T00:00:00", "") + "</td>" +
                                            "<td>" + data.FechaProgramo.replace("T00:00:00", "") + "</td>" +
					                        "<td>" + data.FechaEjecucion.replace("T00:00:00", "") + "</td>" +
                                            "<td " + (data.Folios != 0 ? " class='help' title='" + data.toolTipCosecha + "' " : "") + " >" + data.Folios + "</td>" +
                                            "<td " + (data.FullPreHarvest != 0 ? " class='help' title='" + data.toolTipPreharvest + "' " : "") + " >" + data.FullPreHarvest + "</td>" +
                                            "<td>" + (data.FechaProgramo < data.FechaActividad ? "Si" : "NO") + "</td>" +
                                        "</tr>" +
                                    "</tbody>";
                                container.append(markup);
                                //$(".tblInv>.dx-row").css("text-align", "center");
                                //tooltip();
                            },
                            export: {
                                enabled: true,
                                fileName: "Reporte"
                            },
                            columnWidth: "auto",
                            columns: [
                                    { dataField: "Invernadero", caption: "Invernadero", alignment: "center" },
                                    { dataField: "Variedad", caption: "Variedad", alignment: "center" },
                                    { dataField: "FechaActividad", caption: "Fecha Planeada", alignment: "center", dataType: "date" },
                                    { dataField: "FechaProgramo", caption: "Usuario Programó", alignment: "center", dataType: "date" },
                                    { dataField: "FechaEjecucion", caption: "Fecha Cosechada", alignment: "center", dataType: "date" },
                                    { dataField: "Folios", caption: "Folios", alignment: "center" },
                                    { dataField: "FullPreHarvest", caption: "Full Preharvest", alignment: "center" },
                                    { dataField: "Programado", caption: "Programado", alignment: "center" }
                                ],
                            //filterRow: {
                            //    visible: true,
                            //    applyFilter: "auto"
                            //},
                            paging: {
                                pageSize: 20
                            },
                            pager: {
                                showPageSizeSelector: true,
                                allowedPageSizes: [0, 20, 40, 60, 100],
                                showInfo: true
                            }
                            //,searchPanel: {
                            //    visible: true,
                            //    width: 240,
                            //    placeholder: "Search..."
                            //},
                            //headerFilter: {
                            //    visible: true
                            //}
                        });

                        $("#gCharts").css("display", "flex");
                        $("#gridContainer").css("display", "flex");

                        var serie = "";

                        //GRAFICA A
                        serie = "[" +
                                "{ \"color\": \"#af8a53\", \"valueField\": \"CosechasProgramadas\", \"name\": \"Inv Cosechas Programadas\" }," +
                                "{ \"color\": \"#55A208\", \"valueField\": \"CosechadasATiempo\", \"name\": \"Inv Cosechadas A Tiempo\" }," +
                                "{ \"color\": \"#4108A2\", \"valueField\": \"CosechadasFueraDeTiempo\", \"name\": \"Inv Cosechadas Fuera De Tiempo\" }," +
                                "{ \"color\": \"#a20808\", \"valueField\": \"ProgramadosNoCosechados\", \"name\":\"No Cosechados\"}" +
                            "]";
                        dibujaGrafica($("#graficaA"), result[2], "Cosecha", serie);

                        //GRAFICA B
                        serie = "[" +
                                "{ \"color\": \"#55A208\", \"valueField\": \"LibrasCosechadasATiempo\", \"name\": \"Lbs Cosechadas A Tiempo\" }," +
                                "{ \"color\": \"#4108A2\", \"valueField\": \"LibrasCosechadasFueraDeTiempo\", \"name\": \"Lbs Cosechadas Fuera De Tiempo\" }," +
                                "{ \"color\": \"#f4d101\", \"valueField\": \"LibrasPreharvest\", \"name\": \"Lbs Preharvest\" }" +
                            "]";
                        dibujaGrafica($("#graficaB"), result[3], "Libras", serie);

                        //GRAFICA C
                        serie = "[" +
                                "{ \"color\": \"#55A208\", \"valueField\": \"CosechadasATiempo\", \"name\": \"Inv Cosechadas A Tiempo\" }," +
                                "{ \"color\": \"#4108A2\", \"valueField\": \"CosechadasFueraDeTiempo\", \"name\": \"Inv Cosechadas Fuera De Tiempo\" }" +
                            "]";
                        dibujaGrafica($("#graficaC"), result[4], "Full PreHarvest", serie);

                    } else {
                        popUpAlert(result[1], result[0]);
                        setTimeout(function () { closeJsPopUp(); }, 3000);
                        $(".gridView").trigger("destroy");
                        $("#tblReporte").html("");
                        $("#gCharts").css("display", "none");
                        $("#gridContainer").css("display", "none");
                    }
                    $.unblockUI();
                    //$("td.help").on({ mouseover: function () { tooltip(); } });
                });
                //}
                //else {
                    //popUpAlert("Necesita especificar una variedad", "info");
                    //setTimeout(function () { closeJsPopUp(); }, 3000);
                    //$.unblockUI();
                //}
            } catch (err) {
                console.log(err);
                $.unblockUI();
            }
        }

        function dibujaGrafica(div, data, titulo, vserie) {
            div.dxChart({
                dataSource: JSON.parse(data),
                commonSeriesSettings: {
                    argumentField: "Name",
                    //tagField: "Farm",
                    type: "bar",
                    // hoverMode: "allArgumentPoints",
                    // selectionMode: "allArgumentPoints",
                    label: {
                        visible: true,
                        format: {
                            type: "fixedPoint",
                            precision: 0
                        }
                    }
                },
                series: JSON.parse(vserie),
                title: titulo,
                legend: {
                    verticalAlignment: "bottom",
                    horizontalAlignment: "center"
                },
                export: {
                    enabled: true
                },
                onLegendClick: function (e) {
                    var series = e.target;
                    //alert(e.name);
                    series.isVisible() ? series.hide() : series.show();
                }
            });
        }

        function tooltip() {
            if (!$(".help").hasClass("tooltipstered")) {
                $(".help").tooltipster({
                    animation: 'fade',
                    contentAsHTML: true,
                    interactive: true,
                    delay: 100,
                    theme: 'tooltipster-shadow',
                    touchDevices: true,
                    trigger: 'hover',
                    position: 'left'
                });
            }
        }

        function triggers() {
            PageMethods.comboVariedades(function (result) {
                $("#divCombo").html(result);
                $("#ddlVariedades").chosen({ no_results_text: "No se encontró ningun lider", width: '200px', placeholder_text_single: "--Seleccione un Lider--" });

                //$("#ddlVariedades").chosen().change(function () {
                //    gvReporte();
                //});
            });

            $("#btnObtenerReporte").click(function () {
                gvReporte();
            });

            $(".datepicker").val(new Date().format("yyyy-MM-dd"));
            $(".datepicker").datepicker("destroy");
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                buttonImage: "../comun/img/calendario.png",
                showOn: "both",
                dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
                dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                dayNamesShort: ["Dom", "Lun", "Mar", "Mier", "Jue", "Vie", "Sab"],
                monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                changeYear: true,
                changeMonth: true//,
                //minDate: new Date((new Date).getFullYear(), 00, 01),
                //maxDate: new Date($('ddlAño').val(), 11, 31)
                //maxDate: new Date(date),
                //minDate: new Date(year,minda,1)
            });

            //$(".datepicker").change(function () {
            //    gvReporte();
            //});


        }
       
    </script>
    <style type="text/css">
        #tblDatos tbody tr[id="filtros"]
        {
            display: none;
        }
        
        .dx-row-alt
        {
            background: #D6DFD0;
        }
        .dx-header-row
        {
            border-top: 1px solid #adc995;
            border-bottom: 1px solid #adc995;
            border-left: none;
            border-right: none;
            background: #f0f5e5;
            color: #000000;
            font-weight: bold !important;
            font-family: ProximaNovaRgBold, ProximaNovaLtBold,ProximaNovaThBold, Arial, Helvetica,Sans serif !important;
            font-size: 12px !important;
            text-transform: uppercase;
            white-space: nowrap;
            text-align: center !important;
            color: #000000;
        }
        
        .tblInv tr:hover
        {
            background-color: #F1FFA3;
            cursor: pointer;
        }
        
        tr.dx-row.main-row
        {
            text-align: center !important;
        }
        
        .container
        {
            width: 1024px;
        }
        
        div#gridContainer
        {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
        }
        
        table.dx-datagrid-table.dx-datagrid-table-fixed tr td
        {
            padding: 5px;
        }
        .ui-datepicker-trigger img
        {
            top: 5px;
            cursor: pointer;
            }
    </style>
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Reporte Cumplimiento De Invernaderos Cosechados">
            </asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index">
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblTitulo2" runat="server" text="*Día:"></asp:label>
                        </span>
                    </td>
                    <td style="display: block; text-align: left;">
                        <span>
                            <input id="txtSemanaDesde" class="datepicker" type="text" style="width: 75px; float: none !important;"
                                readonly placeholder="YYYY-MM-DD" />
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:label id="lbLider" runat="server" text="*Variedad:"></asp:label>
                        </span>
                    </td>
                    <td style="display: block; text-align: left;">
                        <div id="divCombo">
                        </div>
                    </td>
                    <td>
                        <input id="btnObtenerReporte" type="button" value="Obtener Reporte" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="gCharts" style="width: 100%; max-width: 1218px; overflow-x: auto; display: none;">
            <div id="graficaA" style="width: 30%; min-width:380px; display: inline-table; border: 1px black solid;
                margin: 6px; padding: 6px;">
            </div>
            <div id="graficaB" style="width: 30%; min-width:380px; display: inline-table; border: 1px black solid;
                margin: 6px; padding: 6px;">
            </div>
            <div id="graficaC" style="width: 30%; min-width:380px; display: inline-table; border: 1px black solid;
                margin: 6px; padding: 6px;">
            </div>
        </div>
        <div id="gridContainer">
        </div>
        <%--<div id="pagerReporte" class="pager" style="width: 100%; min-width: 100%; display: none;">
            <img alt="first" src="../comun/img/first.png" class="first" />
            <img alt="prev" src="../comun/img/prev.png" class="prev" />
            <span class="pagedisplay"></span>
            <img alt="next" src="../comun/img/next.png" class="next" />
            <img alt="last" src="../comun/img/last.png" class="last" />
            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                <option value="20">20</option>
                <option value="40">40</option>
                <option value="60">60</option>
                <option value="80">80</option>
                <option value="100">100</option>
            </select>
        </div>
        <table class="gridView" id="tblReporte">
        </table>--%>
    </div>
</asp:content>