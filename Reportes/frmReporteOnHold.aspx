<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteOnHold.aspx.cs" Inherits="frmReporteCumplimientoInvCosechados"
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
            //gvReporte().val());
            $('#ctl00_ddlPlanta').change(function () {
                triggers();
                gvReporte($(this).val());
            });
        });

        function gvReporte() {
            try {
                $.blockUI();
                var inicio = $(".datepicker.inicio").val();
                var fin = $(".datepicker.fin").val();
                var idPlanta = $('#ctl00_ddlPlanta').val();

                //if (variety != 0) {
                PageMethods.ObtenerReporte(inicio, fin, idPlanta, function (result) {
                    if (result[0] == 'ok') {

                     

                        $("#gridContainer").dxDataGrid({
                            dataSource: JSON.parse(result[1]),
                            allowColumnResizing: false,
                            columnAutoWidth: true,
                            showColumnLines: false,
                            showRowLines: false,
                            rowAlternationEnabled: true,
                            showBorders: false,
                            rowTemplate: function (container, item) {

                                if (null == item.data.calidad) {
                                    item.data.calidad = 'N/D';
                                }
                                if (null == item.data.fechaInbound) {
                                    item.data.fechaInbound = 'N/D';
                                }
                                if (null == item.data.fechaRetrabajo) {
                                    item.data.fechaRetrabajo = 'N/D';
                                }
                                

                                console.log(item.data.calidad);
                                console.log(item.data.fechaInbound);
                                console.log(item.data.fechaRetrabajo);
                                var data = item.data, markup = "<tbody onmouseover='tooltip();' class='tblInv " + ((item.rowIndex % 2) ? 'dx-row-alt' : '') + "'>" +
                                            "<tr class='dx-row main-row'>" +
                                            "<td>" + data.claveinvernadero + "</td>" +
                                            "<td>" + data.folio + "</td>" +
					                        "<td>" + data.variedad + "</td>" +
                                            "<td>" + data.fechaCalidad.replace("T", " ") + "</td>" +
					                        "<td>" + data.cajas + "</td>" +
                                            "<td>" + data.calidad + "</td>" +
                                            "<td>" + data.fechaInbound.replace("T", " ") + "</td>" +
                                            "<td>" + data.merma + "</td>" +
                                            "<td>" + data.fechaRetrabajo.replace("T", " ") + "</td>" +
                                            "<td>" + data.calidadNueva + "</td>" +
                                            "<td>" + (data.recuperado == 1 ? 'Si' :'No') + "</td>" +
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
                                    { dataField: "Invernadero", alignment: "center" },
                                    { dataField: "Folio", alignment: "center" },
                                    { dataField: "Variedad", alignment: "center", dataType: "date" },
                                    { dataField: "FechaCalidad", alignment: "center", dataType: "date" },
                                    { dataField: "cajas", alignment: "center", dataType: "date" },
                                    { dataField: "calidad", alignment: "center" },
                                    { dataField: "fechaInbound", alignment: "center" },
                                    { dataField: "merma", alignment: "center" },
                                    { dataField: "fechaRetrabajo", alignment: "center" },
                                    { dataField: "calidadNueva", alignment: "center" },
                                    { dataField: "recuperado", alignment: "center" }
                                ]
                        });

                        $("#gCosecha").css("display", "flex");
                        $("#gPreharvest").css("display", "flex");

                        var serie = "";

                        //GRAFICA A
                        serie = "[" +
//                                "{ \"argumentField\": \"calidad\", \"valueField\": \"val\" }," +
//                                "{ \"argumentField\": \"calidad\", \"valueField\": \"val\" }," +
//                                "{ \"argumentField\": \"calidad\", \"valueField\": \"val\" }," +
                                "{ \"argumentField\": \"calidad\", \"valueField\": \"val\" }" +
                            "]";
                        dibujaGrafica2($("#graficaA"), result[2], "Calidad OnHold", serie);

                        //GRAFICA B
                        serie = "[" +
                                "{\"color\": \"#4108a2\", \"valueField\": \"total\", \"name\": \"Folios OnHold\" }," +
                                "{\"color\": \"#af8a53\", \"valueField\": \"Recuperados\", \"name\": \"Recuperados\" }" +
                            "]";
                        dibujaGrafica($("#graficaB"), result[3], "OnHold", serie);

                        //                        //GRAFICA C
                        //                        serie = "[" +
                        //                                "{ \"color\": \"#af8a53\", \"valueField\": \"CosechadasATiempo\", \"name\": \"Cosechadas A Tiempo\" }," +
                        //                                "{ \"color\": \"#4108A2\", \"valueField\": \"CosechadasFueraDeTiempo\", \"name\": \"Cosechadas Fuera De Tiempo\" }" +
                        //                            "]";
                        //                        dibujaGrafica($("#graficaC"), result[4], "PreHarvest", serie);

                    } else {
                        popUpAlert(result[1], result[0]);
                        setTimeout(function () { closeJsPopUp(); }, 3000);
                        $(".gridView").trigger("destroy");
                        $("#tblReporte").html("");
                        $("#gCosecha").css("display", "none");
                        $("#gPreharvest").css("display", "none");
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
                    argumentField: "name",
                    //tagField: "Farm",
                    type: "stackedBar",
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
        function dibujaGrafica2(div, data, titulo, vserie) {
           
            div.dxPieChart({
                type: "doughnut",
                palette: ['#a20808', '#f4d101', '#55a208', '#af8a53'],
                dataSource: JSON.parse(data),
                title:titulo,
                tooltip: {
                    enabled: true,
                    format: "Lbs",
                    customizeTooltip: function (arg) {
                        
                        return {
                            text: arg.valueText + "Lbs."
                        };
                    }
                },
                legend: {
                    horizontalAlignment: "right",
                    verticalAlignment: "top",
                    margin: 0
                },
                "export": {
                    enabled: true
                },
                 series: [
                                {
                                    argumentField: "calidad",
                                    valueField: "val",
                                    label: {
                                        visible: true,
                                        font: {
                                            size: 16
                                        },
                                        connector: {
                                            visible: true,
                                            width: 0.5
                                        },
                                        position: "columns",
                                        customizeText: function(arg) {
                                            return   arg.percentText ;
                                        }
                                    }
                                }
                            ]
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

            $(".datepicker.fin").change(function () {
                gvReporte();
            });


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
            <asp:label id="lblTitulo" runat="server" text="Folios Recuperados OnHold">
            </asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index">
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblTitulo2" runat="server" text="*Inicio:"></asp:label>
                        </span>
                    </td>
                    <td style="display: block; text-align: left;">
                        <span>
                            <input id="txtSemanaDesde" class="datepicker inicio" type="text" style="width: 75px; float: none !important;"
                                readonly placeholder="YYYY-MM-DD" />
                        </span>
                        
                    </td>
                    <td>
                        <span>
                            <asp:label id="lblTitulo3" runat="server" text="*Fin:"></asp:label>
                        </span>
                    </td>
                    <td style="display: block; text-align: left;">
                        <span>
                             <input id="txtSemanaFin" class="datepicker fin" type="text" style="width: 75px; float: none !important;"
                                readonly placeholder="YYYY-MM-DD" />
                        </span>
                    </td>
                    <td>
                        <input id="btnObtenerReporte" type="button" value="Obtener Reporte" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="gCosecha" style="width: 100%; display: none; justify-content: center;">
            <div id="graficaA" style="width: 450px; display: inline-table; border: 1px black solid;
                margin: 20px; padding: 20px;">
            </div>
            <div id="graficaB" style="width: 450px; display: inline-table; border: 1px black solid;
                margin: 20px; padding: 20px;">
            </div>
            <%--<div id="graficaC" style="width: 450px; display: inline-table; border: 1px black solid;
                margin: 20px; padding: 20px;">
            </div>--%>
        </div>
        <div id="gridContainer">
        </div>
        
    </div>
</asp:content>