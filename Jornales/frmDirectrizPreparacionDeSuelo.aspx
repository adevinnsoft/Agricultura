<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmDirectrizPreparacionDeSuelo.aspx.cs" Inherits="Jornales_frmDirectrizPreparacionDeSuelo" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
     <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
     <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
     	<script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
 <script type="text/javascript">
     $(function () {
         var javacriptVariedadC = '<%= VariedadC%>';
         var javacriptVariableC = '<%= VariableC%>';
         var javacriptTemporalC = '<%= TemporalC%>';


         triggers();





         $('#btn_ConsultarTabla').click(function () {
             $.blockUI();
             var idVariedad = '';
             var idVariable = '';
             var idTemporal = '';
             var idHabilidades = '';
             $('#tdvariedad tbody td input:checked').each(function () {
                 idVariedad += $(this).attr('idVariedad') + ',';
                 $('#<%=Variedad.ClientID%>').val(idVariedad.substring(0, idVariedad.length - 1));
             });
             $('#tdvariable tbody td input:checked').each(function () {
                 idVariable += $(this).attr('idVariable') + ',';
                 $('#<%=Variable.ClientID%>').val(idVariable.substring(0, idVariable.length - 1));
             });
             $('#tdtemporal tbody td input:checked').each(function () {
                 idTemporal += $(this).attr('idtemporal') + ',';
                 $('#<%=Temporal.ClientID%>').val(idTemporal.substring(0, idTemporal.length - 1));
             });
             PageMethods.ConsultarDirectriz(idVariedad.substring(0, idVariedad.length - 1), idVariable.substring(0, idVariable.length - 1), idTemporal.substring(0, idTemporal.length - 1), $('#ctl00_ddlPlanta').val(), function (resultado) {
                 $('#tddirectriz').html(resultado);
                 var contdirseleccionadas = 0;
                 $('#tddirectriz input[type="checkbox"]').change(function () {
                     var a;
                     contdirseleccionadas = 0; // = contdirseleccionadas + 1;

                     $('#tddirectriz input:checked').each(function () {
                         contdirseleccionadas = contdirseleccionadas + 1;
                         //alert("nada");
                     });
                                          if (contdirseleccionadas == 1) {
                                              //WebMethod
                                              //alert("WebMethod" + $(this));
                                              PageMethods.DatosHabilidadDirectriz($(this).attr('iddirectriz'), function (response) {
//                                                  $.unblockUI();
//                                                  if (response[1] == "ok") {
//                                                      popUpAlert(response[0], response[1]);
//                                                  } else {
//                                                      popUpAlert(response[0], response[1]);
//                                                  }
                                              });
                                          }
                                          else {
                                              //nada
                                              //alert("nada" + $(this));
                                          }
                     //popUpMostrar($(this));

                 });


                 var var1;
                 var1 = var1 + 1;
                 $.unblockUI();
             });
         });

         $('#idGuardar').click(function () {
             var contadorElementos = 0;
             $('#tddirectriz input:checked').each(function () {
                 contadorElementos = contadorElementos + 1;
             });
             if (contadorElementos != 0) {
                 $.blockUI();
                 var idDirectrices = '';
                 $('#tddirectriz tbody td input:checked').each(function () {
                     idDirectrices += $(this).attr('iddirectriz') + ',';

                 });
                 var HabilidadesPrepSuelo = $('#tdHabilidades input[value!=""]').map(function () {
                     return habilidades = {
                         idHabilidad: $(this).parent().prev().children().attr('idhabilidad'),
                         idEtapa: $(this).parent().prev().children().attr('idetapa'),
                         idNivel: $(this).parent().prev().children().attr('idnivel'),
                         Repeticiones: parseFloat($(this).val())
                     }
                 }).get();
                 PageMethods.Guardar(HabilidadesPrepSuelo, idDirectrices.substring(0, idDirectrices.length - 1), function (response) {
                     $.unblockUI();
                     if (response[1] == "ok") {
                         popUpAlert(response[0], response[1]);
                     } else {
                         popUpAlert(response[0], response[1]);
                     }
                 });
             }
             else {
                 popUpAlert("Necesita al menos tener una directriz seleccionada", "warning");
             }
         });
     });
         var idVariedad = '';
         var idVariable = '';
         var idTemporal = '';



      function triggers() {
          PageMethods.WMVariedad(function (response) {
              var formularioArticulo = response;
              $('#tdvariedad').html(response);
          });
          PageMethods.WMVariable(function (response) {
              var formularioArticulo = response;
              $('#tdvariable').html(response);
          });
          PageMethods.WMTemporal(function (response) {
              var formularioArticulo = response;
              $('#tdtemporal').html(response);
          });

          PageMethods.WMHabiidades(($('#ctl00_ddlPlanta').val()),function (response) {
              var formularioArticulo = response;
              $('#tdHabilidades').html(response);
          });

          $('#ctl00_ddlPlanta').live('change', function () {
              var tree = 0;
              $('#btn_ConsultarTabla').click();
              PageMethods.HabilidadesAcomodo(($('#ctl00_ddlPlanta').val()), function (response) {
                  $('#tdHabilidades').html(response);
              });

              $('#ctl00_ContentPlaceHolder1_chkl_Habilidades .invisible').each(function () {
                  if ($(this).attr('idplanta') == $('#ctl00_ddlPlanta').val()) {
                      tree = tree + 1;
                      $(this).parent().parent().addClass('invisible');
                  }
              });
          });



      }
      $(function () {
          function check() {
              
          }
      });

      function popUpMostrar(btn) {
          btnMaterialesPresionado = btn;
          
          PageMethods.ConsultarDirectrizUni(($(btnMaterialesPresionado).parent().find('input[type="checkbox"]').attr('iddirectriz')), function (response) {
              var directriz = $.parseJSON(response);
              $('#idNomDirectriz').html(directriz["nombre"]);
              
              $('.grid').html(directriz["tabla"]);
          });
        


          $('#popUpHerrmaientasYMateriales').css({
              top: '50%',
              left: '50%',
              'margin-left': ($('#popUpHerrmaientasYMateriales').width() * -0.5) + 'px',
              'margin-top': ($('#popUpHerrmaientasYMateriales').height() * -0.5 + $(window).scrollTop()) + 'px'
          }).show();
      }
      function cerrarPopUpHerrmaientasYMateriales() {
          $('#popUpHerrmaientasYMateriales').hide();
          $('#popUpHerrmaientasYMateriales input:not([type=button])').val('');
          $('#popUpHerrmaientasYMateriales input[type="search"]').change();
      }
    </script>
    <style type="text/css">
    table#tdHabilidades input[type="text"] {
        max-width: 40px;
    }
    .divPadreGrid {
    min-width: 100%;
    overflow-x: auto;
    height: 476px;
}

table#gv_Directriz {
    text-align: center;
}
</style>
<style type="text/css" id="popUp">
    div.popUpWMP
    {
        position: absolute;
        width: 65%; /*height: 65%;*/
        height: 600px;
        background: white;
        z-index: 9999;
        overflow: hidden;
        border: 1px solid #cccccc;
        -moz-box-shadow: 0 0 9px #999999;
        -webkit-box-shadow: 0 0 9px #999999;
        box-shadow: 0 0 9px #999999;
        display: none;
    }
    div.popUpHeader
    {
        background: #F4D101;
        height: 42px;
        width: 100%;
        color:yellow;
    }
    div.popUpContenido
    {
        padding: 5px;
        max-height: 80%;
        overflow: auto;
        width: 98%;
    }
    div.popUpBotones
    {
        width: 100%;
        height: 40px;
        bottom: 0px;
        position: absolute;
        background: #F4D101;
    }
    .accordionHeader
    {
        width: 100%;
        background: #ADC995;
        padding-top: 5px;
        padding-bottom: 5px;
        margin-top: 3px;
    }
    div.etapa
    {
        text-align: center;
        font-size: 12px;
        margin-top: 0px;
        margin-left: auto;
        margin-right: auto;
        margin-bottom: 10px;
        width: 800px;
        max-width: 800px;
        min-width: 800px;
    }
    .configuracionAdicionalDeEtapa
    {
        border-left: 1px solid #ccc;
        border-right: 1px solid #ccc;
        border-bottom: 1px solid #ccc;
        height: auto;
    }
    .etapaGenerales
    {
        width: 100%;
        background: #ADC995;
        border: none;
        border-collapse: collapse;
    }
    #divEtapas input
    {
        max-width: 100px;
        margin: 3px 2px;
    }
    #divEtapas select
    {
        max-width: 100px;
        margin: 3px 2px;
    }
    .configuacionPorProducto div
    {
        display: table-cell;
        border: 1px dashed #D2CFCF;
        width: 50%;
    }
    .porcenajesDeIncremento
    {
        display: inline-block;
    }
    .configuacionPorProducto div.target
    {
        padding: 0;
        margin: 5px;
    }
    .configuacionPorProducto div.materiales
    {
        padding: 0;
        margin: 5px;
    }
    .configuacionPorProducto div.materiales table.index5
    {
        margin: 5px;
        width: 97%;
    }
    .configuacionPorProducto div.targetPorProducto
    {
        padding: 0;
        background: #f1f1f1;
        display: block;
    }
    .configuacionPorProducto div.targetPorProducto h5
    {
        margin: 0px;
        padding: 0px 5px;
        width: 343px;
        font-size: 11px;
        text-align: right;
        float: left;
    }
    .configuacionPorProducto div.targetPorProducto h3
    {
        margin: 5px;
    }
    .configuacionPorProducto div.materiales h3
    {
        margin: 5px;
    }
    .configuacionPorProducto div
    {
        border: 1px dashed #fff;
        width: 388px;
        display: table-cell;
        text-align: left;
        min-height: 84px;
    }
    .index5
    {
        text-align: center;
        border: 1px solid #adc995;
        background: #f0f5e5;
        font-size: 12px;
        margin-top: 0px;
        -moz-border-radius: 10px;
        -webkit-border-radius: 10px;
        border-radius: 10px;
        margin-left: auto;
        margin-right: auto;
        margin-bottom: 10px;
        width: 100%;
    }
    .invisible
    {
        display: none;
    }
    .porcentaje input
    {
        width: 30px;
    }
    span.porcentaje
    {
        display: inline-block;
        width: 72px;
    }
    span.porcentaje label
    {
        display: inline-block;
        margin: 10px 3px;
        width: 20px;
        text-align: right;
    }
    
    table#tddirectriz input[type="checkbox"]
    {
        margin-top: 5px;
    }
    
    table#tddirectriz img.consultarDirectriz
    {
        position: relative;
        top: 3px;
        cursor: pointer;
    }
    
    table#gv_Directriz tr td
    {
        border: dotted 1px #999;
        min-width: 15px;
        padding: 5px;
    }
    
    div#popUpHerrmaientasYMateriales
    {
        width: 90%;
        transform: translateX(-50%) translateY(-50%);
        margin-left: auto !important;
        margin-top: auto !important;
    }
    
    h2#idNomDirectriz
    {
        display: inherit !important;
        margin: 20px 0 0;
        text-align: center;
        width: auto;
    }
    
    table#configvariables table.index
    {
        padding: 10px;
    }
    
    p.small
    {
        font-size: 11px;
        text-align: left;
        margin: 0;
        padding-left: 7px;
        font-style: italic;
    }
    
    .celdasGrandes
    {
        width: 100%;
        border: 1px solid #adc995;
        padding: 10px;
        border-radius: 10px;
    }
    
    input[type="checkbox"]
    {
        margin-right: 5px;
    }
    
    .popUpBotones input[type="button"] {
        display: none;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="container">
    <div class="column">
    <h1>Directriz Preparación de Suelo</h1>
      
     
    <div class="column" id="divCargadas" style=" min-width:800px;"></div>

                <table class="index" id="configvariables">
                   <tr>
                        <td colspan="4">
                            <h2>Selección de Configuración de Directrices</h2>
                        </td>
                   </tr>
                    <tr>
                        <td >
                        <asp:Label ID="lblVariedad" runat="server" Text="Variedades:"></asp:Label></td>
                        <td colspan="3" >
                            <table class="celdasGrandes"  id="tdvariedad">

                            </table>
                        </td>
                    </tr>        
                    <tr><td><asp:Label ID="lblVariable" runat="server" Text="Variables:"></asp:Label></td>
                        <td colspan="3">
                         <table class="celdasGrandes"  id="tdvariable">

                            </table>
                            
                    
                        </td>
                    </tr>
                    <tr><td><asp:Label ID="lblTemporales" runat="server" Text="Temporales:"></asp:Label></td>
                        <td colspan="3">
                        <table class="celdasGrandes"  id="tdtemporal">

                            </table>
                            
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td><asp:Label ID="lblCiclo" runat="server" Text="Ciclo:"></asp:Label></td><td class="checkboxes"><asp:CheckBox runat="server" ID="chkNormal" RepeatDirection="Horizontal"  RepeatColumns="8"  Text="Normal" Checked="true"></asp:CheckBox></td>
                        <td class="checkboxes"><asp:CheckBox runat="server" ID="chkInterplanting" RepeatDirection="Horizontal"  RepeatColumns="8" Text="Interplanting"></asp:CheckBox></td>
                    </tr>
                    <tr style="display:none;"><td ><asp:Label ID="lblPlanta" runat="server" Text="Planta:"></asp:Label></td><td><asp:TextBox type="text" ID="txtPlantaImportada" runat="server"  CssClass="txtPlantaImportada"/> <asp:Label runat="server" ID="lblPlantaImportada"></asp:Label></td></tr>        
                    <tr><td colspan="4"><input type="button" id="btn_ConsultarTabla" value="Consultar" /></td></tr> 
                   
                    <tr> 
                    
                        <td><asp:HiddenField runat="server" Value="" ID="Variedad" /></td>
                        <td><asp:HiddenField runat="server" Value="" ID="Variable" /></td>
                        <td><asp:HiddenField runat="server" Value="" ID="Temporal" /></td>
                    </tr>
                </table>
     <h1>Directrices</h1>
     <table class="index">
     <tr>
        <td colspan="3">
            <h2>Selección de Directrices Consultadas</h2>
        </td>
     </tr>
     <tr><td><asp:Label ID="lblDirectriz" runat="server" Text=""></asp:Label></td>
         <td colspan="3">
            <table class="index celdasGrandes"  id="tddirectriz">

                    </table>
            
         </td>
     </tr>        
      </table>

         <h1>Configuración de Habilidades</h1>
        

     <div id="divHabilidades">
        <table class="index">
        <tr>
            <td colspan="3"><h2>Capture y edite el número de repeticiones por actividad</h2></td>
        </tr>
            <tr><td><asp:Label ID="lblHabilidades" runat="server" Text=""></asp:Label></td>
                <td colspan="3">
                    <table class="index celdasGrandes"  id="tdHabilidades">
                    <tbody>
                    
                    </tbody>
                    </table>
                   
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <p class="small">*Nota: La captura de repeticiones por actividad para la semana de <b>Preparación de Suelo</b> se asigna a las directrices seleccionadas en el formulario anterior.</p>
                </td>
            </tr>      
        </table>
        <input type="button" id="idGuardar" value="Guardar" />
     </div>
    </div>


    <div id="popUpHerrmaientasYMateriales" class="popUpWMP">
            <div class="popUpHeader">
                <img src="../comun/img/remove-icon.png" alt="X" onclick="cerrarPopUpHerrmaientasYMateriales();" style=" margin-left: -536.5px;   margin-top: 247px;  float: right; margin: 10px; cursor: pointer;" />
            </div>
            <h2 id="idNomDirectriz">Directriz</h2>
            <div class="divPadreGrid" id="iddivPadreGrid">
            <div class="grid" id="divGrid" runat="server">
                <table cellspacing="0" rules="all" border="1" id="gv_Directriz" style="border-collapse:collapse;"></table>

                </div> 
                </div>  
            <div class="popUpContenido" style="max-height: 80%;overflow: auto;">
            
            </div>
            <div class="popUpBotones">
                <input type="button" value="Aplicar" onclick="AgregarMateriales(); cerrarPopUpHerrmaientasYMateriales();"/>
                <input type="button" value="Cancelar" onclick="cerrarPopUpHerrmaientasYMateriales();" />
            </div>
        </div>
</div>
</div>
<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>

