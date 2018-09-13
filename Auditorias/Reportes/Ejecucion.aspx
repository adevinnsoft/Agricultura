<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Ejecucion.aspx.cs" Inherits="Auditorias_Reportes_Ejecucion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
   
        <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.0/knockout-min.js"></script>
        <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
        <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.chartjs.js"></script>
        <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.webappjs.js"></script>
        <script type="text/javascript" src="../scripts/jszip.js"></script>
        <script type="text/javascript" src="../scripts/jszip.min.js"></script>

        <link rel="stylesheet" type="text/css" href="http://cdn3.devexpress.com/jslib/15.2.10/css/dx.common.css" />
        <link rel="stylesheet" type="text/css" href="../CSS/dx.light.css" />
        <title></title>
     
        <script type="text/javascript">
            $(function () {
                $('#<%= lblDatax.ClientID %>').hide();
                $('#<%= lblSemanas.ClientID %>').hide();
                $('#<%= lblSemaforos.ClientID %>').hide();

                var arregloSemanas = $('#<%= lblSemanas.ClientID %>').text().split(",");
                var arregloSemaforos = jQuery.parseJSON($('#<%= lblSemaforos.ClientID %>').text());

                var columnas = '[{ "dataField":"nomPlanta", "caption":"Planta", "Datatype":"string", "allowFiltering":"true", "allowHeaderFiltering":"true", "filterType":"include", "groupIndex":"0" }, ' +
                               '{ "dataField":"nomLider", "caption":"Lider", "Datatype":"string", "allowFiltering":"true", "allowHeaderFiltering":"true", "filterType":"include", "groupIndex":"1" }, ' +
                               '{ "dataField":"vGreenHouse", "caption":"Invernadero", "Datatype":"string", "allowFiltering":"true", "allowHeaderFiltering":"true", "filterType":"include" }, ';

                for (var i = 0; i < arregloSemanas.length; i++) {
                    columnas += '{ "dataField":"' + arregloSemanas[i] + '", "caption":"S' + arregloSemanas[i] + '", "Datatype":"string", "alignment":"center", "allowFiltering":"true", "allowHeaderFiltering":"true", "filterType":"include" },';
                }

                columnas = columnas.substring(0, columnas.length - 1);

                columnas += ']';


                var promedios = '[';

                for (var i = 0; i < arregloSemanas.length; i++) {
                    promedios += '{ "column": "S' + arregloSemanas[i] + '", "skipEmptyValues":"true", "showInGroupFooter":"true", "summaryType":"avg", "valueFormat":"decimal", "precision":"2", "displayFormat":"{0}" },';
                }

                promedios = promedios.substring(0, promedios.length - 1);

                promedios += ']';

                $("#gridContainer").dxDataGrid({
                    dataSource: jQuery.parseJSON($('#<%= lblDatax.ClientID %>').text()),
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    allowColumnReordering: true,
                    hoverStateEnabled: true,

                    selection: {
                        mode: "multiple",
                        showCheckBoxesMode: "onLongTap"
                    },

                    pager: {
                        infoText: "Página {0} de {1}",
                        showInfo: true,
                        showNavigationButtons: true,
                        visible: true
                    },

                    paging: {
                        pageSize: 100
                    },

                    filterRow: {
                        applyFilter: "auto",
                        visible: true,
                        showOperationChooser: true
                    },

                    headerFilter: {
                        visible: true
                    },

                    "export": {
                        enabled: true,
                        fileName: "ReporteAnualAuditorias",
                        allowExportSelectedData: true,
                        excelFilterTextEnabled: true,
                        excelWrapTextEnabled: true
                    },

                    groupPanel: {
                        visible: true,
                        emptyPanelText: "Arrastre un encabezado de columna aquí para agrupar por esa columna",
                        allowColumnDragging: true
                    },

                    columnAutoWidth: true,

                    grouping: {
                        autoExpandAll: false
                    },

                    onCellPrepared: function (info) {
                        for (var i = 0; i < arregloSemanas.length; i++) {
                            if (info.column.dataField === (arregloSemanas[i] + '')) {
                                if (info.value == null) {

                                } else {
                                    for (var j = 0; j < arregloSemaforos.length; j++) {
                                        if (info.value > (parseInt(arregloSemaforos[j]["iInicial"]) - 1) && info.value <= (parseInt(arregloSemaforos[j]["iFinal"]))) {
                                            info.cellElement.css('background', arregloSemaforos[j]["vColorHex"]);
                                        }
                                    }
                                }
                            }
                        }
                    },

                    columns: jQuery.parseJSON(columnas),

                    summary: {
                        groupItems: jQuery.parseJSON(promedios)
                    }
                });

            });
        </script>

        <style>
            @media print {
                body {
                    -webkit-print-color-adjust: exact;
                }
            }

            .pivotgrid-demo .plantacss {
                color: #bfae6a;
                font-weight: bold;
            }

            #pivotgrid-demo > .dx-button {
                margin: 10px 0;
            }

            #pivotgrid-demo .desc-container a {
                color: #f05b41;
                text-decoration: underline;
                cursor: pointer;
            }

            #pivotgrid-demo .desc-container a:hover {
                text-decoration: none;
            }

            .dx-datagrid .dx-row-alt>td{
                background-color: none;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
  <asp:label ID="lblDatax" runat="server" Visible="true"></asp:label>
        <asp:label ID="lblSemanas" runat="server" Visible="true"></asp:label>
        <asp:label ID="lblSemaforos" runat="server" Visible="true"></asp:label>
        <br/>
        <div id="applyCustomFilter"></div>
        <div id="gridContainer"></div>
    
        <br/>
        </div>
</asp:Content>

