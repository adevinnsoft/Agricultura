<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invernaderos.aspx.cs" Inherits="Grafica5" %>

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

     <script src="../../Scripts/js/jquery-3.1.0.min.js"></script>

    <script src="../../Scripts/js/cldr.min.js"></script>
    <script src="../../Scripts/js/cldr/event.min.js"></script>
    <script src="../../Scripts/js/cldr/supplemental.min.js"></script>

    <script src="../../Scripts/js/globalize.min.js"></script>
    <script src="../../Scripts/js/globalize/message.min.js"></script>
    <script src="../../Scripts/js/globalize/number.min.js"></script>
    <script src="../../Scripts/js/globalize/currency.min.js"></script>

    <script src="../../Scripts/js/dx.all.js"></script>
    <link href="../../Scripts/css/dx.common.css" rel="stylesheet" />
    <link href="../../Scripts/css/dx.light.css" rel="stylesheet" />

    <%--<script>setTimeout('document.location.reload()',60000); </script>--%>
    <script type="text/javascript">
          /////---------------------------------------------------------------------------------------------------------------
          var data = <%=topTenData%>;
            $(function () { 
               
                var chart = $("#container").dxChart({
                    dataSource: data,
                    commonSeriesSettings: {
                        argumentField: "inv",
                        tagField: "inv",
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
                    onLegendClick: function(e) {
                        var series = e.target;
                        series.isVisible() ? series.hide() : series.show(); 
                    },
                    series: [ <%=tipo%>
                        //{ valueField: "Estimado", name: "Estimado", color:"#af8a53"},
                        //{ valueField: "Cosecha", name: "Cosecha", color:"#55A208"},
                        //{ valueField: "Arribo", name: "Arribo",color:"#A20808" },
                        //{ valueField: "Empacado", name: "Empacado" },
                        //{ valueField: "Merma", name: "Merma" }
                    ],
                    title: "Invernaderos de <%=FarmName%>",
                    legend: {
                        verticalAlignment: "bottom",
                        horizontalAlignment: "center"
                    },
                    
                    export: {
                        enabled: true,
                        fileName: "Invernaderos_<%=FarmName%>",
                        printingEnabled: false
                    },
                    scrollBar: {
                        visible: true
                    },
                    scrollingMode: "all", 
                    zoomingMode: "all",
                    onPointClick: function (e) {
                        e.target.select();
                        // alert(e.target.tag);

                        $('#topten', window.parent.document).animate({ height: '450px' },1000);
                      //  $("#topten").animate({ height: '500px' },1000);
                        //parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?idPlanta="+e.target.tag+"&fechaI=" + fi + "&fechaF=" + ff);
                    }
                });
                chart.zoomArgument(300, 500);

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
