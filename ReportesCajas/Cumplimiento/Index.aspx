<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <meta charset="utf-8" />
    <title>Cumplimiento</title>
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
        var idLider = 0;
        var idPlanta = 0;
        /////---------------------------------------------------------------------------------------------------------------

        function moreFields() {
            var SemanaI = document.getElementById("SemanaNumI").value;
            var SemanaF = document.getElementById("SemanaNumF").value;
            if(SemanaI==''){
                SemanaI = 0;
            }
            if(SemanaF==''){
                SemanaF = 0;
            }
            
            parent.document.getElementById('grid').setAttribute('src', "Ejecucion.aspx?SemanaInicio="+SemanaI+"&SemanaFin="+SemanaF);
        }
        $(function () {
            $("#SemanaNumI").dxNumberBox({
                value: 1,
                min: 1,
                max: 54,
                width: "80px",
                showSpinButtons: true
            });

            $("#SemanaNumF").dxNumberBox({
                value: 1,
                min: 1,
                max: 54,
                width: "80px",
                showSpinButtons: true
            });

            $("#Aceptable").dxNumberBox({
                value: 1,
                min: 1,
                max: 100,
                width: "80px",
                showSpinButtons: true
            });
            $("#default").dxButton({
                text: "Actualizar!!",
                type: "default",
                onClick: function (data) {
                    var SemanaI = document.getElementById("SemanaNumI").value;
                    var SemanaF = document.getElementById("SemanaNumF").value;
                    var Anio = document.getElementById("ddlAnio").value;
                    if (SemanaI == '') {
                        SemanaI = 0;
                    }
                    if (SemanaF == '') {
                        SemanaF = 0;
                    }
                    parent.document.getElementById('grid').setAttribute('src', "Ejecucion.aspx?SemanaInicio=" + SemanaI + "&SemanaFin=" + SemanaF + "&Anio="+Anio);
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
            width: 30%;
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
        .dx-field-label {
     padding: 0px 0px 0px 0;
}
        HTML, BODY {
            display: block;
            margin: 0;
            padding: 0;
            height: 100%;
            overflow: hidden;
            font-family: sans-serif;
        }
        #pan_superior {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 140px;
            overflow: hidden;
            background-color: #E0E0E0;
            text-align: center;
        }
        #pan_inferior {
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            height: 32px;
            overflow: hidden;
            background-color: #FF8A01;
            text-align: center;
            font-size: 13px;
        }
        #pan_central {
            position: absolute;
            top: 140px;
            bottom: 32px;
            left: 0;
            right: 0;
            background-color: #E0E0E0;
        }
        #pan_izquierdo {
            height: 100%;
            overflow-y: scroll;
            width: 350px;
            float: left;
            background-color: #A0C0F0;
            text-align: center;
        }
        #pan_derecho {
            height: 100%;
            overflow-y: scroll;
            width: 100px;
            float: right;
            background-color: #A0C0E0;
            text-align: center;
        }
        #pan_centro {
            overflow: scroll;
            height: 100%;
            background-color: #F0F0F0;
        }
 .margen { /* Estilo */
  padding:16px;
  }

 #gridContainer {
    height: auto;
    width: 100%;
}
 .custom-height-slider {
    height: 75px;
}
    </style>
</head>
<body>

       
 
        <div id="pan_superior">
                <div style="display: inline-block;">
              <label class="label dx-field-label" ><h1>EJECUCION Y CUMPLIMIENTO</h1></label></div>
            <div  class="dx-field" style="margin-left:0%;">
           <%--   </div>         
            <div  class="dx-field" style="margin-left:32%;">
                    <div class="label dx-field-label">SEMANA INICIO:  <div id="SemanaNumI"></div></div>
         
                    <div class="dx-field-value"> </div>

                    <div class="label dx-field-label">SEMANA FIN:<div id="SemanaNumF"></div></div>
                    <div class="label dx-field-label">(+/-) % Aceptable:<div id="Aceptable"></div></div>
                   <%-- <div class="dx-field-value"></div>--%>
                
             <%--   
               <div class="dx-field">
                <div class="dx-field-label">Actualizar!!</div>
                <div class="dx-field-value">
            <div id="default"></div>
        </div>
    </div>--%>
                      SEMANA INICIO: <input type="number" min="0" max="53" step="1" id="SemanaNumI" value="0" />
            SEMANA FIN: <input type="number" min="0" max="53" step="1" id="SemanaNumF" value="0" />
                AÑO:  <input type="number" id="ddlAnio" runat="server" name="ddlAnio"/>
                (+/-) % Aceptable:<input type="number" min="0" max="100" step="1" id="Aceptable" value="5" />
             <%--<input type="button" onclick="moreFields();" id="enviar" value="Actualizar!!" />--%>
                 <div id="default"></div>
           
            </div>
           
                </div><br />
            
             
           
        </div>
           </div>

        <div id="pan_central">
           <%--  <div id="pan_izquierdo">
                 <form id="form" runat="server">
                     <dx:ASPxComboBox ID="Combo_Pais" runat="server" OnSelectedIndexChanged="ComboPais_Changed" AutoPostBack="true">
                     </dx:ASPxComboBox><br />
                     <dx:ASPxComboBox ID="Combo_Planta" runat="server" OnSelectedIndexChanged="ComboPlanta_Changed" AutoPostBack="true">
                     </dx:ASPxComboBox><br />
                     <dx:ASPxComboBox ID="Combo_Inv" runat="server"  AutoPostBack="true">
                     </dx:ASPxComboBox><br />
                     </form>
                 
            </div>--%>
          <div id="pan_centro">
              <%--<div id="gridContainer"></div>--%>
                      <iframe src="" name="grid" id="grid"  height="5000px" width="100%" runat="server" frameborder="0"></iframe>
                </div>
          </div>
 
  

    
</body>
</html>
</form>

