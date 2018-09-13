<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Graficacion de cajas</title>
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
    <script type="text/javascript">
       

        
        /////---------------------------------------------------------------------------------------------------------------
        
        $(function () {
            var fechaInicio = new Date();
            var fechaFin = new Date();
            var parametroInicio = fechaInicio.getFullYear().toString() + "/" + (fechaInicio.getMonth() + 1).toString() + "/" + fechaInicio.getDate().toString();
            var parametroFin = fechaFin.getFullYear().toString() + "/" + (fechaFin.getMonth() + 1).toString() + "/" + fechaFin.getDate().toString();

            $("#dateInicio").dxDateBox({
                format: "date",
                onValueChanged: function (data) {
                    fechaInicio = new Date(data.value);
                    parametroInicio = fechaInicio.getFullYear().toString() + "/" + (fechaInicio.getMonth() + 1).toString() + "/" + fechaInicio.getDate().toString();
                    parent.document.getElementById('Iframe3').setAttribute('src', "Grafica3.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin);
                    parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin);
                }
            });

            $("#dateFin").dxDateBox({
                format: "date",
                onValueChanged: function (data) {
                    fechaFin = new Date(data.value);
                    parametroFin = fechaFin.getFullYear().toString() + "/" + (fechaFin.getMonth() + 1).toString() + "/" + fechaFin.getDate().toString();
                    parent.document.getElementById('Iframe3').setAttribute('src', "Grafica3.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin);
                    parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin);

                }
            });

        });
    </script>
    <style>
        #containerPeriod {
            margin-top:0;
            padding-top:0;
            top:0;
            height: 150px;
            width: 100%;
            background:#ffffff;
            z-index:3;
            position:fixed;
        }
        .graficas {
            width: 1300px;
            margin:150px auto;
            margin-bottom:0px;
            height: 620px;
            z-index:1;
        }
        
        .ajuste {
            float: left;
            top:150px;
            margin: 10px;
            padding: 0px;
            width: 100%;
            border: 1px solid black;
        }

        .calendar{
            width:300px;
            float:left;
        }

        .label{
            width:auto;
            float:left;
            margin-left:30px;
        }
                 

        #dx-field {
    display: inline-block;
    margin-left: 40px;
}
     
    </style>
</head>
<body>
    <div style="width:100%; height:100%" >
        <div style="witdh:800px; ">
          <div id="containerPeriod">
              <div style="margin-left: 25%; display: inline-block;">
              <label class="label dx-field-label" ><h1>COSECHA - ARRIBO DE CAJAS</h1></label>
           
              </div>         
              <div  class="dx-field" style="margin-left:30%;">
                    <div class="label dx-field-label">Inicio:</div>
                    <div class = "calendar" id="dateInicio"></div>
                    <div class="dx-field-value"> </div>

                    <div class="label dx-field-label">Fin:</div>
                    <div class="calendar" id="dateFin"></div>
                    <div class="dx-field-value"></div>

                </div>
            </div>
        </div>
       
         
        <div class="graficas">
            
          <%--<iframe class="ajuste" src="" name="myIframe" id="myIframe" height="600px" runat="server" frameborder="0"></iframe>--%>
           <iframe class="ajuste" src="Grafica5.aspx" name="myIframe" id="topten"  height="600px" runat="server" frameborder="0"></iframe>
            <iframe  class="ajuste"  src="Grafica3.aspx" name="myIframe" id="Iframe3"  height="600px" runat="server"></iframe>
        </div>
        <br />


    </div>
  

    
</body>
</html>
