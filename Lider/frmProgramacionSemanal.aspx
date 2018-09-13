<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmProgramacionSemanal.aspx.cs"
    Inherits="Lider_frmProgramacionSemanal" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" 
    meta:resourcekey="PageResource1" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/slider/slick.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/dates.js" type="text/javascript"></script>
    <link href="../comun/scripts/fullcalendar.min.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/fullcalendar.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fullcalendar_es.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script id="Globales" type="text/javascript">
        var ctrlFechaActual = null;
        var Contador = 0;
        var jornadaInicio = "<%=horaInicio%>:<%=minutoInicio%>";
        var jornadaFin = "<%=horaFin%>:<%=minutoFin%>";
        var diaInicioDeSemana = '<%=diaInicioDeSemana%>';
        var idiomaCalendario = '<%=idiomaCalendario%>';
        var calendarioInicio = '<%=int.Parse(horaInicio)-2 %>:00';
        var calendarioFin = '<%=int.Parse(horaFin)+2 %>:00';
        var semanaActual = null;
        var JsonAsociados;
        var periodos = [];
        var Eliminadas = [];
        var Cosecha = "<%=cosecha %>";
        var fumigacion = "<%=fumigacion %>";
        var preparacionSuelos = "<%=preparacionSuelos %>";
        var oficiales=[];
        var Actividades = [];
        var tecnologias =[];
        var dividedEvents = [];
        var rangosemana;
        var Familias=[];
        var DirectrizNoP=[];
        var Infestaciones=[];
        var Ausencias=[];
        var EnviarP = true;
        var limpiezas= "<%=limpieza%>";
        var podayvuelta = "<%=podayvuelta%>";
        var deshojes = "<%=deshoje%>";

        $(function () {

            JsonAsociados = JSON.parse('<%=AsociadosLider%>');
            Familias = JSON.parse('<%=Familias %>');
            tecnologias = JSON.parse('<%=tecnologias %>');

            PageMethods.cargaInfestacionNivelInvernadero(function(response){
                Infestaciones = JSON.parse(response);
            });
            

                       $(window).scroll(function () {
                           if (window.scrollY >= 228) {
                               $('div#roller.invernaderos').addClass('fixed');
                               $('div#rollerInvernaderosProgramados.invernaderosProgramados').addClass('fixed');
                           } else {
                               $('div#roller.invernaderos').removeClass('fixed');
                               $('div#rollerInvernaderosProgramados.invernaderosProgramados').removeClass('fixed');
                           }
                           $('div#popUpCalendario').css({ 'top': window.scrollY, 'position': 'fixed' });
                       });


            document.onmousedown=noclick;
            function noclick(event){
                if(event.button==2){
                    return false;
                }
            }


        });
       


        function rgb2hex(orig) {
            var rgb = orig.replace(/\s/g, '').match(/^rgba?\((\d+),(\d+),(\d+)/i);
            return (rgb && rgb.length === 4) ?
                  ("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
                  ("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
                  ("0" + parseInt(rgb[3], 10).toString(16)).slice(-2) : orig;
        }
    </script>
    <script src="../comun/scripts/ProgramaciónSemanal.js" type="text/javascript"></script>
    <link href="../comun/css/ProgramaciónSemanal.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
 div#popUpContenido  {
     display:flex;
     margin-top:40px;
     }
     
 .ajusteHora {
	width: 35px;
	float:left;
}


input.txtHoraInicio  {
	width: 35px;
}

input.txtHoraFin  {
	width: 35px;
}

div.ajusteHora input[type="button"] {
	max-width: 35px;
	float:left;
}
    div#contHoraFin 
    {
        position:relative;
        margin-top:650%;
        }
        
   /*span.nombreasociado 
   {
       width: 175px;
       }*/
   ul#Asociados 
   {
       display: flex;
       flex-direction: column;
       }
      
       
   span.removeJornal 
   {
       float: right;
    
       }
       
   td#tdTitleActivity
   {
       text-align: center;
       font-weight: bold;
       text-transform: uppercase;
       background: #f4d101;
       padding: 5px 0;
       }
       
   #ActivityData 
   {
       width: 460px;}
   table
   {
       padding: 10px !important;
       }
       
   img#Existe {
    max-width: 25px;
    margin-left: 15px;
}
       
        #rollslider div
        {
            width: 800px;
        }
        #rollerSliderInvernaderosProgramados div
        {
            width: 900px;
        }
        #rollslider2 div
        {
            width: 800px;
        }
        .auto-style1 {
            width: 882px;
        }
    </style>
   


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Programación Semanal</asp:Label>
            <span id="product"></span>
        </h1>
        <table class="index">
            <tr>
                <td>
                    <h2>Semana:</h2>
                </td>
            
                <td  style="text-align: left;">
                    <img src="../comun/img/prev.png" style="float: none;" onclick="semanaAnterior();" />
                    <input id="txtSemanaCalendario"  type="text" style="float: none; width: 60px; text-align: center;"
                        readonly />
                    <input id="txtAnioCalendario"  type="text" style="float: none; width: 60px; text-align: center;"
                        readonly />
                    <img src="../comun/img/next.png" style="float: none;" onclick="semanaSiguiente();" />
                </td>
            </tr>
        </table>
        <table class="index">
            <tr>
                <td>
                    <h3 style="float:right;"><span  id="nombreLider"></span></h3></td>
                    <td style="text-align: center;"><h4 style="align:left;"><span  id="jornalesAprobados"></span></h4>
                </td>
            </tr>
            <tr>
                <td><h2> Invernaderos</h2></td>
                <td>
                    <input type="button" value="Eliminar Selección" onclick="EliminarSeleccionDeInvernaderos();" />
                    <input type="button" value="Seleccionar Todos" onclick="ClickACadaInvenradero();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="roller" class="invernaderos">
                        <div id="rollslider">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="avisoAletraConfiguracion" class="aviso">
                        <p>
                            <span>*Existen Invernaderos sin configuración al 100%, por lo que puede afectar tu programación.<br />Para mas información dar click en el botón <b>"Ver Detalle"</b> Por favor, reportalo a tu gerente.</span>
                        </p>
                        <input type="button" id="btnDetalleConfiguracionInvernadero" value="Ver Detalle"/>
                    </div>
                </td>
            </tr>
        </table>
        <table class="index">
            <tr>
                <td>
                    <h2>
                        Actividades</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>

                                <input type="button" value="Contraer" onclick="$('.accordionBody').slideUp();" />
                            </td>
                            <td>
                                <input type="button" value="Extender" onclick="$('.accordionBody').slideDown();" />
                            </td>
                            <td>
                                <input type="button" value="Guardar Semana" onclick="securityVal(false);" />
                            </td>
                            <td>
                                <input type="button"  id="btnCopiarProgramaConfiguracion"  value ="Copiar programa" />
                            </td>
                            <td>
                                <input type="button" id="btnEnviar" value="Enviar" onclick="securityVal(true);" />
                            </td>
                        </tr>

                    </table>
                   

                </td>
            </tr>
            <tr>
                <td>
                    <div id="divTareasProgramadas">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <div id="roller" class="habilidades">
                        <div id="rollslider2">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div id="ActividadesProgramadas">
        </div>
        <div id="popUpHabilidades">
            <div class="popUpHeader moveHandle">
                <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popUpHabilidades').hide();"
                    style="float: right; margin: 10px; cursor: pointer;" />
            </div>
            <div id="divHabilidades">
            </div>
            <div class="popUpBotones moveHandle">
                <input type="button" value="Aplicar" onclick="AgregarHabilidadesAInvernaderos();" />
                <input type="button" value="Cancelar" onclick="$('#popUpHabilidades').hide();" />
                
            </div>
        </div>
        <div id="popUpFechaHora" class="popUp">
            <div id="SelectorInterior">
                <input type="text" id="DateTimeDemo" />
                <br />
                <input type="button" id="Button1" value="Cancelar" onclick="$('#popUpFechaHora').hide();"
                    style="float: none;" />
                <input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarFecha();"
                    style="float: none;" />
            </div>
            <script type="text/javascript">
                $("#DateTimeDemo").AnyTime_picker({
                    format: "%Y-%m-%d %H:%i",
                    hideInput: true,
                    placement: "inline",
                    labelTitle: "Fecha y hora",
                    labelYear: "Año",
                    labelMonth: "Mes",
                    labelDayOfMonth: "Día del Mes",
                    labelSecond: "Segundo",
                    labelHour: "Hora",
                    labelMinute: "Minuto"
                });
            </script>
        </div>
        <div id="popUpAsociados" idtr="" idjson="">
            
            <div id="asociadosFam">
            <input type="text" class="buscador" id="BuscaAsociados" placeholder="Ingresa tu búsqueda"/>
                <input id="asociadosActividadAplicar" type="checkbox"  /><label for="asociadosActividadAplicar">Aplicar a toda la Actividad</label>
                <input id="asociadosInvernaderoAplicar" type="checkbox"  /><label for="asociadosInvernaderoAplicar">Aplicar a Invernadero</label>
                <div id="EquiposHead">
                    <div class="tab">
                        <span class="tabEquipos">Equipos</span>
                    </div>
                    <div class="tab">
                        <span class="tabFamilias">Asociados</span>
                    </div>
                </div>
                <div id="asociadosHead">
                </div>
                <div id="EquiposData">
                
                </div>
                <div id="asociadosData">
                </div>
                
                <input type="button" value="Cerrar" onclick="$('div#popUpAsociados').hide();" />
            </div>
        </div>
        <div id="popUpActividad">
            <div id="ActivityData" idJson="" idTr="">
                <table>
                <tr>
                    <td colspan="4" id="tdTitleActivity">
                        
                    </td>
                </tr>
                    <tr>
                        <th>
                            Inicio
                        </th>
                        <th>
                            Fin
                        </th>
                        <th>
                            Surcos
                        </th>
                        <th>
                            J. Estimados
                        </th>
                    </tr>
                        
                   
                    <tr>
                        <td>
                            <input type="text" id="ActividadInicio" class="fechaInicio" />
                        </td>
                        <td>
                            <input type="text" id="ActividadFin" class="fechaFin" />
                        </td>
                        <td>
                            <input type="text" id="ActividadSurcos" class="required intValidate"/>
                            <span id="ActividadSurcosEstimados"></span> Estimados
                        </td>
                        <td>
                            <label id="ActividadJornales"></label>
                        </td>
                        
                    </tr>
                    <tr>
                        <th colspan="4">
                            Asociados <span class="addJornal" idetapa="" idjson="" idtr="">+</span>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <ul id="Asociados">
                                
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <input type="button" class="cajaChica" value="Cerrar" onclick="$('#popUpActividad').hide();" />
                        </td>
                        <td>
                            <input id="modificaActividad" type="button" class="cajaChica" value="Modificar"  />
                        </td>
                        
                    </tr>
                </table>
            </div>
        </div>
        <div id="popUpCalendario">
            <div class="popUpHeader moveHandle">
                <div class="filtroPlantas" style="width: auto; max-width: 80%; top: 15px; position: relative;
                    right: -17px;">
                </div>
                <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popUpCalendario').hide(); $('#calendar').fullCalendar( 'destroy' );"
                    style="float: right; margin: 10px; cursor: pointer; top: -17px; position: relative;" />
            </div>
            <div id="popUpContenido" style="max-height: 90%;">
            <div class="ajusteHora">
                <div id="contHoraInicio">
                    <table class="inicioInv">
                        <tr><td><input type="button" class="btnAddHora" value="+"/></td></tr>
                        <tr><td><input type="text" class="txtHoraInicio" /></td></tr>
                        <tr><td><input type="button" class="btnRemHora" value="-" /></td></tr>
                    </table>
                </div>

                <div id="contHoraFin">
                    <table class="finInv">
                        <tr><td><input type="button" class="btnAddHora" value="+"/></td></tr>
                        <tr><td><input type="text" class="txtHoraFin" /></td></tr>
                        <tr><td><input type="button" class="btnRemHora" value="-" /></td></tr>
                    </table>
                </div>
            </div>
                <div id="calendar" class="programacionSemanal">
                </div>
            </div>
            <div class="popUpBotones moveHandle">
                <input type="checkbox" id="mostrarfechas" class="fechasoficiales filtroPlantas" checked />
                <label for="mostrarfechas">Mostrar fechas oficiales</label>
            </div>
        </div>
        <div id="popupDetalleConfiguracionInvernadero">
             <div class="popUpHeader moveHandle">
                <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popupDetalleConfiguracionInvernadero').hide();"
                    style="float: right; margin: 10px; cursor: pointer;" />
            </div>
            <div style="max-height: 90%; display:flex; margin-top:40px;">
            <table>
                    <tbody>
                        <tr>
                            <td>
                                <div id="contenidoDetalleConfiguracionInvernadero">
                         
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>*NOTA: Si estas configuraciones no son correctas reportelas a su gerente.</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
                
             <div class="popUpBotones moveHandle">
                <input type="button" value="Cerrar" onclick="$('#popupDetalleConfiguracionInvernadero').hide();" />
            </div>
        </div>

         <div id="popupConfiguraCopiaProgramacion">
             <div class="popUpHeader moveHandle">
                <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popupConfiguraCopiaProgramacion').hide();"
                    style="float: right; margin: 10px; cursor: pointer;" />
            </div>
             <table>
                 <tr><td>  <h1>Copiar Programación de un invernadero</h1></td></tr>
                 <tr><td>  <h1><span  id="semanaAnio"></span></h1></td></tr>
                  <tr><td>  <h1><span  id="anioActual"></span></h1></td></tr>
             </table>
             <table>               
                 <tr>
                     <td>
                                  <h2>Invernaderos programados</h2>

                     </td>
                         <td>   
                               <h3 style="float:right;"><span  id="InvernaderosProgramados"></span></h3>

                     </td>
                 </tr>

             </table>
         
            <table>
                    <tbody>                      
                        <tr>
                            <td>
                                <h2>Invernaderos a programar</h2>
                            </td>
                            
                        </tr>
                        <tr>

                             <td>
                               <h3 style="float:right;"><span  id="InvernaderosAprogramar"></span></h3>
                            </td>
                        </tr>
                      
                       
                    </tbody>
                </table>
             <table>
                 <tr>
                     <td></td>
                     <td>
                         <input type="button"  id="btnGuardar"  value ="Copiar Programación"  onclick="AgregaInvernaderosACopiar();"/>
                         
                     </td>
                 </tr>
             </table>
                
         
                
             <div class="popUpBotones moveHandle">
                <input type="button" value="Cerrar" onclick="$('#popupConfiguraCopiaProgramacion').hide();" />
            </div>
        </div>
    </div>
</asp:Content>
