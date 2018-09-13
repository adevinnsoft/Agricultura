<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmJornalesPorHaActiva.aspx.cs" Inherits="Jornales_Default" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <%-- <script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>  --%>
 <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
  <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
  <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
  <script type="text/javascript" src="../comun/scripts/inputValidations.js"></script>
  <script type="text/javascript">



      $(function () {
          asignarValoresACampos();
      });


//      function obtenerReporteInicial() {
//          var semana = 1,
//              anio = 1;
//          var idPlantaActual = $('[id*="ddlPlanta"] option:selected').val();
//          obtenerReporte(parseInt(idPlantaActual), semana, anio);
//      }
//      function seleccionarPlanta() {
//          $('[id*=ddlPlanta]').change(function () {
//              var idPlanta = $(this).val();
//              obtenerReporte(parseInt(idPlanta),semana,alert);
//          });
      //      }
      function asignarValoresACampos() {
          $('#txtSemana').val($('[id*="ltSemana"]').text());
          $('#txtAnio').val(new Date().getFullYear());
          $('#txtPronostico').val(6);
      }
      function ObtenerReporteClick() {
          validarTodosLosCamposContenidosEn('tblFormulario');
          
          if ($('.error').length == 0) {
              var semana = $('#txtSemana').val();
              var anio = $('#txtAnio').val();
              var pronostico = $('#txtPronostico').val();
              var idPlanta = $('[id*=ddlPlanta]').val();
              obtenerReporte(idPlanta, semana, anio, pronostico);
          } else { 
            //los errores se marcaron.
          }
      }

      function obtenerReporte(idPlanta, semana, anio, pronostico) {
          bloqueoDePantalla.bloquearPantalla();
          try {
              PageMethods.obtenerReporte(idPlanta, semana, anio, pronostico, function (response) {
                  if (response[0] == 'ok') {
                      $('#PanelA').html(response[1]);
                      $('#PanelB').html(response[2]);
                      calcularHectareasActivas();
                      calcularJornalesPorHectareaActiva();
                      acortarTextos();
                      formatoDecimal();
                      bloqueoDePantalla.indicarTerminoDeTransaccion();
                  } else {
                      popUpAlert(response[1], response[0]);
                      console.log(response[2]);
                  }
              }, function (e) {
                  bloqueoDePantalla.indicarTerminoDeTransaccion();
                  console.log(e);
              });
          } catch (e) {
              console.log(e);
              bloqueoDePantalla.indicarTerminoDeTransaccion();
          }
          bloqueoDePantalla.desbloquearPantalla();
     }

      function calcularHectareasActivas() {
          $('.Activas').each(function () {
              var Activas = 0;
              var semana = $(this).attr('Semana');
              $(this).parent().find('.Hectareas[Semana="' + semana + '"]').each(function () {
                  Activas += parseFloat($(this).text());
              });
              $(this).text(Activas);
          });
      }

      function calcularJornalesPorHectareaActiva() {
          $('.tblDatosJornales tr').each(function () {
              var Jornales = 0;
              $(this).find('.Familia,.Fijos').each(function () {
                  Jornales += parseFloat($(this).text());
              });
              $(this).find('.JHA').each(function () {
                  var semana = $(this).attr('Semana');
                  var Activas = parseFloat($(this).parent().find('.Activas[Semana="' + semana + '"]').text());
                  var JHA = Jornales / Activas;
                  $(this).text(isNaN(JHA) || Activas==0 ? 0 : JHA);
              });
          });
      }

      function acortarTextos() {
          $('.container td').each(function () {
              if ($(this).text().length > 40) {
                  $(this).addClass('largeText').click(function (e) {
                      popUpAlert($(this).text(), '');
                      $('.jsPopUp img').remove();
                      $('.jsPopUp, .screenBlocker').click(function () {
                          closeJsPopUp();
                      });
                  });
              }
          });
      }

      function formatoDecimal() {
          $('.Hectareas,.Activas').each(function () {
              $(this).text(parseFloat($(this).text()).toFixed(2));
          });
      }

  </script>


  <style type="text/css">
    div#contenderodReporte {
        display: flex;
        justify-content: space-between;
    }

    div#PanelB {
        max-width: 800px;
        overflow-x: auto;
        margin:0px;
        border-right: 1px dotted black;
    }

    div#contenderodReporte table th {
        white-space: nowrap;
        padding: 4px 8px;
        background: #76933C;
        color: white;
    }
    
    div#contenderodReporte table, div#contenderodReporte table tr
    {
        text-align: center; 
    }
    
    div#contenderodReporte table th {
        background: #76933c;
        color: White;
        font-weight: normal;
    }
    div#contenderodReporte table td {
        white-space: nowrap;
        padding: 5px 8px;
    }
    
    td.largeText {
        max-width: 251px;
        color: rgb(70, 46, 197);
        overflow: hidden;
        cursor:pointer;

    }
    
    table.tblJornalesAsociados, table.tblDatosJornales
    {
        border-collapse: collapse;
    }
    
    table.tblJornalesAsociados tr td, table.tblDatosJornales tr td
    {
        border: 1px dotted black;
    }
    
   table.tblJornalesAsociados th, table.tblDatosJornales th
    {
        border: 1px dotted white;
    }
    
    table.tblDatosJornales tr th:first-child, table.tblDatosJornales tr td:first-child
    {
        border-left: none;    
    }
    table.tblDatosJornales tr th:last-child, table.tblDatosJornales tr td:last-child
    {
        border-right: none;    
    }
    table.tblDatosJornales td.JHA
    {
        background: red;
        color: White;    
    }
    table.tblDatosJornales td.Impar
    {
        background: #c4d79b;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="container">
        <h1><asp:Label runat="server" Text="Jornales Por Hectárea Activa" 
                meta:resourcekey="LabelResource1"></asp:Label></h1>
        <table class="index" id="tblFormulario">
            <tr>
                <td><asp:Label ID="Label1" runat="server" Text="Semana" 
                        meta:resourcekey="Label1Resource1"></asp:Label></td>
                <td><input type="text" id="txtSemana"  class="intValidate requerido" maxlength="2" /></td>
                <td><asp:Label ID="Label2" runat="server" Text="Año" 
                        meta:resourcekey="Label2Resource1"></asp:Label></td>
                <td><input type="text" id="txtAnio" class="intValidate requerido" maxlength="4" /></td>
                <td><asp:Label ID="Label3" runat="server" Text="Pronóstico" 
                        meta:resourcekey="Label3Resource1"></asp:Label></td>
                <td><input type="text" id="txtPronostico" class="intValidate requerido" maxlength="2" /></td>
            </tr>
            <tr>
                <td colspan="6">
                     <input type="button" value='<%=GetLocalResourceObject("ObtenerRep")%>' onclick="ObtenerReporteClick();"/>
                </td>
            </tr>
        </table>
        <div id="contenderodReporte">
            <div id="PanelA"></div>
            <div id="PanelB"></div>
        </div>
   </div>
</asp:Content>

