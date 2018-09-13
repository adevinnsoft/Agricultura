<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TablaPivot.aspx.cs" Inherits="TablaPivot" %>

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
                        dataField: "Farm",
                        area: "row",
                        sortBySummaryField: "Total"
                    }, {
                        caption: "Invernadero",
                        width: 120,
                        dataField: "claveInvernadero",
                        area: "row",
                        allowSorting: true
                    },  {
                        dataField: "fechaInicio",
                        dataType: "date",
                        area: "column"
                    }, 
                     {
                         groupName: "fechaInicio",
                         groupInterval: "year"
                     },  {
                         groupName: "fechaInicio",
                         groupInterval: "quarter"
                     },
                    {
                        dataField: "producto",
                        area: "column",
                    }, 
                    {
                        dataField: "Seccion",
                        dataType: "number",
                        area: "column",
                        allowSorting: true
                    }, 
                     {
                         caption: "Total",
                         dataField: "cajas",
                         dataType: "number",
                         summaryType: "sum",
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
    </style>
</head>
<body>
   <div id="reset"></div>
        <div id="pivotgrid-demo">
            <div id="pivotgrid"></div>
        </div>
</body>
</html>
