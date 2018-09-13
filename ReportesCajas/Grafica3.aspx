<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grafica3.aspx.cs" Inherits="Grafica3" %>

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

    <script>setTimeout('document.location.reload()',60000); </script>
    <script type="text/javascript">
        /////---------------------------------------------------------------------------------------------------------------
        var dataSource = <%=convertido%>;
        var dir ="Grafica2.aspx?idPlanta=8";
        $(function () {//////////////////////////////////////////////////
             
            $("#container").dxChart({
                dataSource: dataSource, 
                animation: {
                    duration: 2000
                }, 
                size: {
                    height: 500
                },
                commonSeriesSettings: {
                    cornerRadius: 0,
                    argumentField: "state",
                    tagField: "Farm",
                    type: "bar",
                    hoverMode: "allArgumentPoints",
                    selectionMode: "allArgumentPoints",
                    label: {
                        visible: true,
                        format: "fixedPoint",
                        precision: 0
                    }
                },
                series: {
                    argumentField: "Name",
                    valueField: "totales",
                    name: "RANCHOS",
                    type: "bar",
                    color: '#5EB2E6'
                },
                title: {
                    text: "RANCHOS"
                },
                //tooltip: {
                //    enabled: true,
                //    customizeTooltip: function (args) {
                //        return {
                //            html: "<div class='state-tooltip'><img src='imgs/" + 
                //            args.point.tag + ".png' style='width:50px;'/><h4>" +
                //            args.percentText + "</h4></div>"
                //        };
                //    }
                //},
                legend: {
                    visible: false,     
                },
                "export": {
                    enabled: true
                },
                argumentAxis: {
                    label: {
                        overlappingBehavior: {
                            mode: "rotate",
                            rotationAngle: 35
                        }
                    }
                },
                onPointClick: function (e) {
                    e.target.select(); 
                    var yourID = e.target.tag;
                    var fi = <%=fechaI%>.toString();
                    var ff = <%=fechaF%>.toString();
                    var kgs= "<%=kgs%>".toString();
                   // window.open("Asociado/index.aspx?idFarm="+e.target.tag);
                    parent.document.getElementById('myIframe').setAttribute('src', "Grafica2.aspx?idPlanta="+yourID+"&Name="+e.target.originalArgument+"&fechaI="+fi+"&fechaFin="+ff+ "&kgs=" + kgs);
                    parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?idPlanta="+e.target.tag+"&fechaI=" + fi + "&fechaF=" + ff+ "&kgs=" + kgs);
                        
                }
            });
            ///////////////////////////////////////////////////////
        });
    </script>
      <style>
        
        .ajuste {
            float: left;
            margin: 10px;
            padding: 10px;
            width: 500px;
            height: 600px;
            border: 1px solid black;
        }</style>
</head>
<body>
     <div id="container"></div>
</body>
</html>
