<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grafica5.aspx.cs" Inherits="Grafica5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <%-- <!--[if lt IE 9]>
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
          /////---------------------------------------------------------------------------------------------------------------
          var data = <%=topTenData%>;
            $(function () { 
                $("#container").dxChart({
                    dataSource: data, 
                    rotated: true,
                    legend: {
                        visible: false,     
                    },
                    size: {
                        height: 400
                    },
                    commonSeriesSettings: {
                        //tagField: "Name",
                        cornerRadius: 0,
                        label: {
                    visible: true,
                    format: "fixedPoint",
                    precision: 0
                }
                    },
                    series: {
                        argumentField: "ClaveInvernadero",
                        valueField: "Cajas",
                        name: "Clave Invernadero",
                        type: "bar",
                        color: '#ffa500'
                    },
                    tooltip: {
                        enabled: true,
                        customizeTooltip: function (args) {
                            return {
                                html: "<div class='state-tooltip'><h4>" + 
                               // args.point.tag + " - " +
                                args.value + " </h4></div>"
                            };
                        }
                    },
                    title: {
                        text: "Top 10 Invernadero"
                    },
                    "export": {
                        enabled: true
                    },
                    argumentAxis: {
                        label: {
                            overlappingBehavior: {
                                mode: "rotate",
                                rotationAngle: 0
                            }
                        }
                    },
                });
            });

          
        </script>
    <style>

        #container {
    height: 440px;
    width: 100%;
}
    </style>
</head>
<body>
     <div id="container"></div>
 
</div>
</body>
</html>
