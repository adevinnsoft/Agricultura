<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteRecuperacionCalidad.aspx.cs" Inherits="frmReporteResumenCosecha" meta:resourcekey="PageResource1" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">


    <script src="../comun/scripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>--%>
    <link href="../comun/css/fixed_table_rc.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>--%>
    <%--<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>--%>
    <%--<script src="../comun/scripts/fixedtables/widgets/widget-scroller.min.js" type="text/javascript"></script>--%>
    <%--<script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.blockUI.2.70.0.js" type="text/javascript"></script>
   <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />

    <script src="../comun/scripts/jquery-ui.1.11.3.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.1.11.3.js" type="text/javascript"></script>
    <link href="../comun/css/jquery-ui-1.8.21.custom.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="../comun/scripts/jquery-ui-1.8.21.custom.min.js"></script>--%>
    <script type="text/javascript" src="../comun/scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../comun/scripts/jsPopUp.js"></script>
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>

    <%--<link href="../comun/css/dx.light.css" rel="stylesheet" type="text/css" />--%>

    <link href="../comun/scripts/devextreme/dx.16.2.5.common.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/devextreme/dx.16.2.5.light.css" rel="stylesheet" type="text/css" />

    <script src="../comun/scripts/jsZip/jszip.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsZip/jszip.min.js" type="text/javascript"></script>

    <script src="../comun/scripts/devextreme/dx.16.2.5.all.js" type="text/javascript"></script>

    <style type="text/css">
     
    </style>
    <script type="text/javascript" id="reporteEficiencia">
        var idLider = 0;

        $(function () {
            $('.datepicker').val(getDateString())
            $('.datepicker').datepicker({ 'dateFormat': 'yy/mm/dd' }).on('click', function () { $(this).attr('readonly', 'readonly') });


            triggers();

            $('body').on('mouseover','img.img_recov', function () {
                if ($(this).attr('done') == undefined) {
                    $('img.img_recov').tooltipster({
                        animation: 'fade',
                        contentAsHTML: true,
                        interactive: true,
                        delay: 100,
                        theme: 'tooltipster-shadow',
                        touchDevices: true,
                        trigger: 'click',
                        position: 'left'
                    });
                }
            });

            $('body').on('change', 'select#ctl00_ddlPlanta', function () {
                $('#resultados_').html('');
                triggers();
            });
            

        });

        function getDateString() {
            var date = new Date();
            var dia = date.getDate();
            var mes = date.getMonth() + 1;
            var anio = date.getFullYear();
            var fecha = anio + '/' + (mes < 10 ? '0' : '') + mes + '/' + (dia < 10 ? '0' : '') + dia;

            return fecha;
        }


        function getDetails(folio) {
            var vfolio = $(folio).attr('folio');
            if ($(folio).attr('done') == undefined) {


                PageMethods.getDetail(vfolio, function (response) {

                    $('.tooltipster-content').html(response);

                    
                });
            }
           
                
            
            

        }

        function triggers() {
            bloqueoDePantalla.bloquearPantalla();
            PageMethods.cargar(($('select#ctl00_ddlPlanta').val() == undefined ? 0 : $('select#ctl00_ddlPlanta').val()), function (result) {

                if (parseInt(result[0]) == 1) {

                    console.log('cargando plantas');
                    $('#ddlVariedad').html('').html(result[2] );
                    //$('#ddlPlanta').val('0');
                    $('#ddlVariedad').chosen({ no_results_text: "No se encontró", width: '150px', placeholder_text_single: "--Todos--" });
                    // dateOnChange();
                    $('#ddlVariedad').trigger("chosen:updated");
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                    bloqueoDePantalla.desbloquearPantalla();

                }else {
                    console.log('no se cargaron plantas');
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                    bloqueoDePantalla.desbloquearPantalla();
                }
            });
            ;
        }

        function generar() {
        
            bloqueoDePantalla.bloquearPantalla();
            
            var inicio = $('#txtDiaInicio').val();
            var fin = $('#txtDiaFin').val();
            var variedad = parseInt($('#ddlVariedad').val()) == 0 ? "0" : $('#ddlVariedad option[value="' + $('#ddlVariedad').val()+'"]').text();
            $('#resultados_').html('');

            if ( inicio.toString().length == 10) {
                PageMethods.obtenerReporte(inicio, fin, variedad, function(result) {
                    
                    if (parseInt(result[0]) == 1) {
                        $('#resultados_').html('<div id="pieRecuperacion" style="width: 45%; min-height:420px; min-width:380px; display: inline-table; border: 1px black solid; margin: 6px; padding: 6px;"></div>' +
                            ' <div id="chartTipos" style="width: 45%; min-height:420px; min-width:380px; display: inline-table; border: 1px black solid; margin: 6px; padding: 6px;"></div> <div id="gridFolios"></div>');

                        var jsonConfFolios = getJSONConfigdxDataGrid( 
                             $.parseJSON(result[3]), [ "invernadero", "folio", "secciones", "calidad","lbsTotal", "fechaFullPreHarvest", "fechaLlegadaArribo", "variedad","recuperado"]);
                        $("#gridFolios").dxDataGrid(jsonConfFolios);
                        $("#pieRecuperacion").dxPieChart(getJSONConfigxPie($.parseJSON(result[4])));
                        $("#chartTipos").dxChart(getchartdata($.parseJSON(result[5])));

                    } else {
                    
                    
                        popUpAlert(result[1], result[2]);
                    }
            
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                   
                });
            } else {
            popUpAlert('Antes de continuar, seleccione por lo menos fecha.', 'warning');
            bloqueoDePantalla.indicarTerminoDeTransaccion(); 
            }

            bloqueoDePantalla.desbloquearPantalla();

            
        }

        function getchartdata(data) {

            serie = "[" +
                               "{ \"color\": \"#55A208\", \"valueField\": \"CMANS\", \"name\": \"CM A NS\" }," +
                               "{ \"color\": \"#4108A2\", \"valueField\": \"MRACM\", \"name\": \"MR A CM\" }," +
                               "{ \"color\": \"#f4d101\", \"valueField\": \"MRANS\", \"name\": \"MR A NS\" }" +
                           "]";

            return {
                dataSource: data,
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
                series: JSON.parse(serie),
                height: 420,
                width: 460,
                title:"Libras Recuperadas",
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
            }
        }

       function getJSONConfigdxDataGrid(datasource, cols) {
                return {dataSource: datasource
                        , "export": {
                            enabled: true,
                            fileName: "recuperacion",
                            allowExportSelectedData: true
                        }
                        , columns: cols,
                        rowTemplate: function (container, item) {
                            var data = item.data, markup = "<tbody class='tblInv " + ((item.rowIndex % 2) ? 'dx-row-alt' : '') + "'>" +
                                 "<tr class='dx-row main-row'>" +
                                 "<td>" + data.invernadero + "</td>" +
                                 "<td>" + data.folio + "</td>" +
                                 "<td>" + data.secciones + "</td>" +
                                 "<td>" + data.calidad + "</td>" +
                                 "<td>" + data.lbsTotal + "</td>" +
                                 "<td>" + (data.fechaFullPreHarvest == null ? "" : data.fechaFullPreHarvest) + "</td>" +
                                 "<td>" + (data.fechaLlegadaArribo == null ? "" : data.fechaLlegadaArribo) + "</td>" +
                                 "<td>" + data.variedad + "</td>" +
                                 "<td>" + (data.fechaFullPreHarvest == null ? " <img height='23' title='<img src=\"../comun/img/ajax-loader.gif\"/>' folio='" + data.folio + "' width='23' onclick='getDetails(this);' src='../comun/img/" + (data.recuperado == false ? "norecuperado.png" : "recuperado.png") + "' class='img_recov'/>" : "") + "</td>" +
                                 "</tr>";
                            container.append(markup);
                        }
                        
                        ,filterRow: {
                            visible: true,
                            applyFilter: "auto"
                        }
                        , paging: {
                                pageSize:20 
                            }
                        , pager: {showPageSizeSelector: true
                                , allowedPageSizes: [20, 40, 60,100,0]
                                , showInfo: true
                            }
//                        ,searchPanel: {
//                            visible: true,
//                            width: 240,
//                            placeholder: "Search..."
//                        }
                       , headerFilter: {
                            visible: true
                        }
                   };
                
       
       }

       function getJSONConfigxPie(datasource) {
        return {
                            title: "Recuperación de calidad",
                                                
                            height: 420,
                            width: 460,
                            format: {
                                type: 'thousands'
                            },
                            palette: "bright",
                            dataSource: datasource,
                            legend: {
                                orientation: "horizontal",
                                itemTextPosition: "right",
                                horizontalAlignment: "right",
                                verticalAlignment: "bottom",
                                columnCount: 4
                            },
                            "export": {
                                enabled: true
                            },
                            series: [
                                {
                                    argumentField: "calidad",
                                    valueField: "lbs",
                                    label: {
                                        visible: true,
                                        font: {
                                            size: 12
                                        },
                                        connector: {
                                            visible: true,
                                            width: 0.5
                                        },
                                        format: {
                                            type: 'fixedPoint'
                                        },
                                        position: "columns",
                                        customizeText: function(arg) {
                                            return arg.valueText + "lbs (" + arg.percentText + ")";
                                        }
                                    }
                                }
                            ],
                            
                            onPointClick: function (e) {
                                var point = e.target;
    
                                toggleVisibility(point);
                            },
                            onLegendClick: function (e) {
                                var arg = e.target;
    
                                toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                            }
                        }
       
       
       
       }


       function toggleVisibility(item) {
            if(item.isVisible()) {
                item.hide();
            } else { 
                item.show();
            }
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
        
        table.index2 input[type="text"]
        {
            text-align: center;
        }
        
        table#tblEficiencia
        {
            max-width: 900px;
        }
        table#tblDatos
        {
            text-align: right;
        }
        
        .container
        {
            width: 1024px;
        }
        
        .dx-datagrid-content table tr td
        {
            text-align: center !important;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            padding: 5px 0 !important;
        }
        
        .dx-datagrid-content table tr:hover
        {
            background: #f1ffa3 !important;
            cursor: pointer;
        }
        
      
        
        svg.dxc.dxc-chart
        {
            margin: auto;
            margin-top: 20px;
        }
        
        .dx-datagrid-content .dx-datagrid-table .dx-row > td:nth-child(4)
        {
            width: 18%;
        }
        
        .dx-datagrid-content .dx-datagrid-table .dx-row > td:nth-child(6)
        {
            width: 17%;
        }
        
        .dx-datagrid-content .dx-datagrid-table .dx-row > td:last-child
        {
            width: 12%;
        }
        
        .dx-datagrid-headers.dx-datagrid-nowrap
        {
            background: #f0f5e5;
            color: black;
            text-transform: uppercase;
            font-weight: bold;
        }
        tr.dx-row.dx-column-lines.dx-header-row
        {
            background: #f0f5e5;
            border-top: 1px solid #adc995;
        }
        
        tr.dx-row.dx-column-lines.dx-header-row:hover
        {
            background: none !important;
        }
    </style>
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Recuperación de Calidad"></asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index2">
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblDia" runat="server" text="Inicio:">
                            </asp:label></span>
                    </td>
                    <td>
                        <span>
                            <input id="txtDiaInicio" class="Texto datepicker" type="text" value="" placeholder="<%=Session["Locale"].ToString() == "es-MX" ? "DD/MM/AAAA" : "DD/MM/YYYY"%>" <%--onchange="javascript:dateOnChange();"--%>/>
                        </span>
                    </td>
                   
                    <td>
                        <span>
                            <asp:label id="lblPlanta" runat="server" text="Fin:"></asp:label>
                        </span>
                    </td>
                    <td>
                        <span>
                            <input id="txtDiaFin" class="Texto datepicker" type="text" value="" placeholder="<%=Session["Locale"].ToString() == "es-MX" ? "DD/MM/AAAA" : "DD/MM/YYYY"%>" <%--onchange="javascript:dateOnChange();"--%>/>
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:label id="lblVariedad" runat="server" text="Variedad:"></asp:label>
                        </span>
                    </td>
                    <td>
                        <span>
                            <select id="ddlVariedad" class="Texto datepicker" ></select>
                        </span>
                    </td>
                   
                    <td>
                        <input id="btnObtenerReporteEficiencia" type="button" value="Obtener Reporte" onclick="generar();" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="resultados_">
           
        </div>


       
        <div id="pipeRecuperacion">
        </div>
  
    <div>
    </div>
</div>
</asp:content>
