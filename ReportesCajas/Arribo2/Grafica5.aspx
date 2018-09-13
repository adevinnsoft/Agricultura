<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grafica5.aspx.cs" Inherits="Grafica5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
       <!--[if lt IE 9]>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <![endif]-->
        <!--[if gte IE 9]><!-->
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
        <!--<![endif]-->
        <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.0/knockout-min.js"></script>
        <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
        <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.chartjs.js"></script>
    <script>setTimeout('document.location.reload()',60000); </script>
    <script type="text/javascript">
          /////---------------------------------------------------------------------------------------------------------------
          var data = <%=topTenData%>;
            $(function () { 
               
                $("#container").dxChart({
                    dataSource: data,
                    commonSeriesSettings: {
                        argumentField: "Name",
                        type: "bar",
                        hoverMode: "allArgumentPoints",
                        selectionMode: "allArgumentPoints",
                        label: {
                            visible: true,
                            format: {
                                type: "fixedPoint",
                                precision: 0
                            }
                        }
                    },
                    series: [
                        { valueField: "CajasCosecha", name: "Cosecha" },
                        { valueField: "CajasArribo", name: "Arribo" },
                        { valueField: "CajasEmpacadas", name: "Empacado" },
                        { valueField: "Merma", name: "Merma" }
                    ],
                    title: "Cosecha en Arribo por Planta",
                    legend: {
                        verticalAlignment: "bottom",
                        horizontalAlignment: "center"
                    },
                    "export": {
                        enabled: true
                    },
                    onPointClick: function (e) {
                        e.target.select();
                    }
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
