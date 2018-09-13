﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grafica2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <%--<!--[if lt IE 9]>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <![endif]-->
        <!--[if gte IE 9]><!-->
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
        <!--<![endif]-->
        <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.0/knockout-min.js"></script>
        <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
        <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.chartjs.js"></script>--%>

     <script src="../Scripts/js/jquery-3.1.0.min.js"></script>
  
    <script src="../Scripts/js/cldr.min.js"></script>
    <script src="../Scripts/js/cldr/event.min.js"></script>
    <script src="../Scripts/js/cldr/supplemental.min.js"></script>

    <script src="../Scripts/js/globalize.min.js"></script>
    <script src="../Scripts/js/globalize/message.min.js"></script>
    <script src="../Scripts/js/globalize/number.min.js"></script>
    <script src="../Scripts/js/globalize/currency.min.js"></script>

    <script src="../Scripts/js/dx.all.js"></script>
    <link href="../Scripts/css/dx.common.css" rel="stylesheet" />
    <link href="../Scripts/css/dx.light.css" rel="stylesheet" />

    <script type="text/javascript">
        
        $(function () {
            $("#detalles").dxPieChart({
                dataSource: <%=dos%>,
                commonSeriesSettings: {
                    tagField: "id"
                },
            series: [
                {
                    argumentField: "Producto",
                    valueField: "totales",
                    label: {
                        format: 'fixedPoint',
                        precision: 0,
                        visible: true,
                        connector: {
                            visible: true,
                            width: 1
                        },
                        position: "columns",
                        customizeText: function(arg) {
                            return arg.valueText;
                        }
                    }
                }
            ],

            title: <%=NombrePlanta%>,
                tooltip: {
                    enabled: true,
                    customizeTooltip: function (args) {
                        return {
                            html: "<div class='state-tooltip'><img src='imgs/" + 
                            args.point.tag + ".png' style='width:50px;'/><h4>" +
                            args.percentText + "</h4></div>"
                        };
                    }
                },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "right",
                verticalAlignment: "bottom",
                columnCount: 3
            },
            export: {
                enabled: true,
                printingEnabled: false
            },
            onPointClick: function (e) {
                var point = e.target;
                toggleVisibility(point);
            },
            onLegendClick: function (e) {
                var arg = e.target;

                toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
            }
        });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
       <div id="detalles"  style="height:500px; width:100%; max-width:800px; align-content:center; align-self:center;"></div>
</body>
</html>