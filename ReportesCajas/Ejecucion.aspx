<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ejecucion.aspx.cs" Inherits="TablaPivot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <!--[if lt IE 9]>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <![endif]-->
    <!--[if gte IE 9]><!-->
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <!--<![endif]-->
    <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.0/knockout-min.js"></script>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
    <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.chartjs.js"></script>
    <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.webappjs.js"></script>
    <script type="text/javascript" src="Scripts/jszip.js"></script>
    <script type="text/javascript" src="Scripts/jszip.min.js"></script>

    <link rel="stylesheet" type="text/css" href="http://cdn3.devexpress.com/jslib/15.2.10/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="http://cdn3.devexpress.com/jslib/15.2.10/css/dx.light.css" />
    <title></title>
    <script type="text/javascript">

        /////---------------------------------------------------------------------------------------------------------------
        var sales = <%=pivotData%>;
            
        $(function () {
            var pivotgrid = $("#pivotgrid").dxPivotGrid({
                allowSortingBySummary: true,
                allowSorting: true,
                allowFiltering: true,
                allowExpandAll: true,
                showBorders: true,
                height: 600,
                fieldChooser: {enabled: true},
                export: { enabled: true },
               
                dataSource: {
                    fields: [{
                        caption: "Planta",
                        cssClass: "plantacss",
                        width: 120,
                        dataField: "Planta",
                        area: "row"
                        //sortBySummaryField: "Total"
                        }, 
                       
                        {
                        dataField: "Invernadero",
                        //dataType: "number",
                        area: "row"
                    }, 
                    {
                        dataField: "Habilidad",
                        //dataType: "number",
                        area: "row"
                    }, 
                        {
                            dataField: "ActPeriodo",
                            //dataType: "number",
                            area: "row"
                        }, 
                    {
                        dataField: "Semana",
                        dataType: "number",
                        area: "column",
                        allowSorting: true
                    }, 
                     {
                         dataField: "ActPeriodo",
                         dataType: "number",
                         area: "column",
                         allowSorting: true
                     }, 
                      {
                          dataField: "SurcosProgramados",
                          dataType: "number",
                          area: "column",
                          allowSorting: true
                      }, 
                     {
                         caption: "Total",
                         dataField: "SurcoFinal",
                         dataType: "number",
                         summaryType: "max",
                         format: "number",
                         area: "data"
                     }],
                    store: sales
                }
            }).dxPivotGrid("instance");

            $("#reset").dxButton({
                text: "Reset Tabla",
                onClick: function() {
                    pivotgrid.getDataSource().state({});
                }
            });

        });
 
        $(function(){
            $("#gridContainer").dxDataGrid({
                dataSource: sales,
                showRowLines: true,
                rowAlternationEnabled: true,
                selection: {
                    mode: "multiple"
                },
                "export": {
                    enabled: true,
                    fileName: "Ejecucion",
                    allowExportSelectedData: true
                },
                groupPanel: {
                    visible: true
                },
                columnAutoWidth:true,
                onCellPrepared: function (info) {
                    if (info.value == 'NO REALIZADO'||info.value == 'SIN REGISTROS')
                        info.cellElement.addClass('noData');
                },
                filterRow: {
                    visible: true,
                    applyFilter: "auto"
                },
                columns: [
                    { dataField: "Invernadero", groupIndex: 0 },
                    "Habilidad",
                    {
                        dataField: "Inicio",
                        dataType: "date",
                        width: 100
                    }, 
                    {
                        dataField: "Fin",
                        dataType: "date",
                        width: 100
                    },{
                        dataField: "SurcosProgramados",
                        width: 100
                    } ,
                    {
                        dataField: "InicioReal",
                        dataType: "date",
                        width: 100
                    },
                    {
                        dataField: "FinReal",
                        dataType: "date",
                        width: 100
                    }, "TotalSurcoActividad",
                        "Cumplimiento"
                        
                ],
                //sortByGroupSummaryInfo: [{
                //    summaryItem: "Cumplimiento"
                //}],
                summary: {
                    groupItems: [{
                        column: "Cumplimiento",
                        summaryType: "avg",
                        valueFormat: "decimal",
                        displayFormat: "{0}% Cumplimiento"
                    }]}
            });
        });
        </script>
    <style>

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
.noData {
    color: #FF4500;
}
    </style>
</head>
<body>
   <%--<div id="reset"></div>
        <div id="pivotgrid-demo">
            <div id="pivotgrid"></div>
        </div>--%>
    <br />
    <div id="gridContainer"></div>
</body>
</html>
