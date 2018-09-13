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
                    //parent.document.getElementById('Iframe3').setAttribute('src', "Grafica3.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin);
                    var kgsSelected = $("#kilogramos").dxCheckBox("instance");
                    var newValue = kgsSelected.option('value');

                    parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin + "&kgs=" + newValue);
                    parent.document.getElementById('myIframe').setAttribute('src', "GraficaRanchos.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin + "&kgs=" + newValue);
                 
                    $("#topten").animate({ height: '500px' });
                    $("#myIframe").animate({ height: '520px' });
                  
                }
            });

            $("#dateFin").dxDateBox({
                format: "date",
                onValueChanged: function (data) {
                    fechaFin = new Date(data.value);
                    parametroFin = fechaFin.getFullYear().toString() + "/" + (fechaFin.getMonth() + 1).toString() + "/" + fechaFin.getDate().toString();
                    //parent.document.getElementById('Iframe3').setAttribute('src', "Grafica3.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin);
                    var kgsSelected = $("#kilogramos").dxCheckBox("instance");
                    var newValue = kgsSelected.option('value');

                    parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin + "&kgs=" + newValue);
                    parent.document.getElementById('myIframe').setAttribute('src', "GraficaRanchos.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin + "&kgs=" + newValue);
                 
                    $("#topten").animate({ height: '500px' });
                    $("#myIframe").animate({ height: '520px' });
                 
                }
            });

        });
        $(document).ready(function(){
            $("#topten").animate({ height: '500px' },1000);
        });
     
        $(function () {

            

           

            $("#kilogramos").dxCheckBox({
                "text": "kgs",
                onValueChanged: function (e) {
                    var previousValue = e.previousValue;
                    var newValue = e.value;
                   
                    var dateInicioBox = $("#dateInicio").dxDateBox("instance");
                    var fechaInicio = dateInicioBox.option('value');
                    var parametroInicio = fechaInicio.getFullYear().toString() + "/" + (fechaInicio.getMonth() + 1).toString() + "/" + fechaInicio.getDate().toString();

                    var dateFinBox = $("#dateFin").dxDateBox("instance");
                    var fechaFin = dateFinBox.option('value');


                    var parametroFin = fechaFin.getFullYear().toString() + "/" + (fechaFin.getMonth() + 1).toString() + "/" + fechaFin.getDate().toString();

                    parent.document.getElementById('topten').setAttribute('src', "Grafica5.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin + "&kgs=" + newValue);
                    parent.document.getElementById('myIframe').setAttribute('src', "GraficaRanchos.aspx?fechaI=" + parametroInicio + "&fechaF=" + parametroFin + "&kgs=" + newValue);
                    $("#topten").animate({ height: '500px' });
                    $("#myIframe").animate({ height: '500px' });
                }
            });
        });

    </script>
    <style>
        .button-0 {
    position: relative;
    padding: 10px 40px;
    margin: 0px 10px 10px 0px;
    float: left;
    border-radius: 10px;
    font-family: 'Helvetica', cursive;
    font-size: 25px;
    color: #FFF;
    text-decoration: none;  
    background-color: #3498DB;
    border-bottom: 5px solid #2980B9;
    text-shadow: 0px -2px #2980B9;
    /* Animation */
    transition: all 0.1s;
    -webkit-transition: all 0.1s;
}

       .button-0:hover {
       color: #fff;
       background-color: #2980B9;
       cursor:pointer;
       } 
       
.button-0:focus {
    text-decoration: none;
    color: #fff;
}

.button-0:active {
    transform: translate(0px,5px);
    -webkit-transform: translate(0px,5px);
    border-bottom: 1px solid;
}
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
            width: auto;
            margin:180px auto;
            margin-bottom:0px;
            height: 620px;
            z-index:1;
        }
        
        .ajuste {
            float: left;
            top:180px;
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
            width:957px;
            float:left;
            margin-left:30px;
        }
                 

        #dx-field {
    display: inline-block;
    margin-left: 40px;
}
     
    </style>
</head>
<body style="width:1310px;   margin:0px auto; >
       <form runat="server" >
    <div style="width:100%; height:100%" >
        <div style="witdh:800px; ">
          <div id="containerPeriod">
              <div style="margin-left: 25%; display: inline-block;">
              
           
              </div>      
              <div style="width:1300px;  position:relative;  float:left; text-align:center; top: 0px; left: 0px;">
                  <div style="width:80%;  position:relative;  float:none; text-align:center;">
                         <div style="width:20%; position:relative; float:left;">
                      <div style="width:100%; position:relative; float:left;">
                        <img alt="" src="../../comun/img/bonanza.png" width="200px"  style="align-self:flex-start; height: 100px;" align="left" />
                          </div>
                      <%--<div style="width:100%; position:relative; float:left;">
                           <asp:Button style="position:relative;  float:none; top: 0px; left: 0px;" ID="Generar" Text="Generar" runat="server" autpostback="true" CssClass="button-0" OnClick="Generar_Click" Enabled="true"/>
                    
                      </div>--%>
                </div>
                         <div style="width:80%; position:relative; float:left; text-align:center;">
                      <div style="width:100%; position:relative; float:left; text-align:center;">
                         <label class="label dx-field-label" ><h1>CAJAS / KGS EN CADA PROCESO</h1></label>
                      </div>

                    <div  class="dx-field" style="width:100%;  position:relative;  float:none; top: 0px; left: 0px;">
                        <div  class="dx-field" style="margin-left:30%;">
                    <div class="label dx-field-label" style="width:40px;">Inicio:</div>
                    <div class = "calendar" id="dateInicio"></div>
                    <div class="dx-field-value"> </div>

                    <div class="label dx-field-label" style="width:30px;">Fin:</div>
                    <div class="calendar" id="dateFin"></div>
                    <div class="dx-field-value"></div>
                    <div style="padding: 10px;">
                      <div id="kilogramos"></div>
                    </div>
                </div>
                        
             
                </div>
                               
                    </div>
                  </div>
              </div>   
           
            </div>
        </div>
   
        <div id="Reporte" class="graficas">
           <iframe class="ajuste" src="GraficaRanchos.aspx" name="myIframe" id="myIframe" height="520px" runat="server" frameborder="0" width="100px" ></iframe>
           <iframe class="ajuste" src="Grafica5.aspx" name="myIframe" id="topten"  height="100px" runat="server" frameborder="0"></iframe>
            <iframe class="ajuste" src="#" name="frameINV" id="frameINV"  height="0px" runat="server" frameborder="0"></iframe>
        </div>
         
        <br />


    </div>
  

     </form>
</body>
</html>
