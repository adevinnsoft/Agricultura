<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReporteCumplimientoEjecucion.aspx.cs" Inherits="Reportes_ReporteCumplimientoEjecucion" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>


    <script type ="text/javascript">

        $(function () {
            $('#btnObtenerReporte').click(function () {
                validarCampos();
            });
        });

        function desplazarInicioPagina() {
            $("html, body").animate({ scrollTop: "0px" });
        }


        function validarCampos() {
            if (($('#txtSemana').val().trim() === '' || $('#txtAnio').val().trim() === '') || ($('#txtSemana').val().trim() === '' && $('#txtAnio').val().trim() === '')) {
                popUpAlert('Favor de completar los campos', 'warning');
                return false;
            }

            obtenerCumplimientoEjecucion();
        }

        function desplazarPagina() {
            var posBoton = $('#imgDESC2').offset().top;
            $("html, body").animate({ scrollTop: posBoton + "px" });
        }

        function desplazarFinalPagina() {
            var altura = $(document).height();
            $("html, body").animate({ scrollTop: altura + "px" });
        }

        function expandirDivEjecucion() {
            if ($('#divEjecutado').is(':hidden')) {
                $('#divEjecutado').show();
                $('#imgASC').show();
                $('#imgDESC').hide();
               
            }
            else {
                $('#divEjecutado').hide();
                $('#imgDESC').show();
                $('#imgASC').hide();
            }

            desplazarPagina();

        }


        function expandirDivPlaneacion() {
            if ($('#divPlaneado').is(':hidden')) {
                $('#divPlaneado').show();
                $('#imgASC2').show();
                $('#imgDESC2').hide();

            }
            else {
                $('#divPlaneado').hide();
                $('#imgDESC2').show();
                $('#imgASC2').hide();
            }

            desplazarFinalPagina();
        }

        
        function tablesorter(table) {
            if ($(table).find("tbody").find("tr").size() >= 1) {
                var pagerOptions = { // Opciones para el  paginador
                    container: $("#pager"),
                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                };

                $(table)
				 .tablesorter({
				     widthFixed: true,
				     widgets: ['zebra', 'filter'],
				     headers: { /*0: { filter: false} */
				 },
				 widgetOptions: {
				     zebra: ["gridView", "gridViewAlt"]
				     //filter_hideFilters: true // Autohide
				 }
	          }).tablesorterPager(pagerOptions);
                
                $(".tablesorter-filter.disabled").hide(); // hide disabled filters
            }
            else {
                $("#pager").hide();
            }
        } 


        function obtenerCumplimientoEjecucion() {
            var semana = $('#txtSemana').val();
            var anio = $('#txtAnio').val();


            PageMethods.ObtenerCumplimientoEjecucion(semana, anio, function (response) {
                try {
                    $.blockUI();
                    if (response[0] == '1') {
                        $("#tblDesgloseInvernaderoCultivo").trigger("destroy");
                        $("#tblDesgloseInvernaderoCosecha").trigger("destroy");
                        $('#divDetalleGeneral').show();
                        $('#divDetallePorInvernadero').show();
                        $('#divDetalleSemanal').show();
                        $('#divDetallesHabilidades').show();
                        $('#tblGenerales').html(response[2]);
                        $('#tblDesgloseInvernaderoCultivo').html(response[3]);
                        $('#tblDesgloseInvernaderoCosecha').html(response[4]);
                        $('#divTablasDetalleSemanalCultivo').html(response[5]);
                        $('#divTablasDetalleSemanalCosecha').html(response[6]);
                        tablesorter($('#tblDesgloseInvernaderoCultivo'));
                        tablesorter($('#tblDesgloseInvernaderoCosecha'));
                        $('#divEjecutado').html(response[8]);
                        $('#divPlaneado').html(response[7]);
                        $('#imgDESC').show();
                        $('#imgDESC2').show();
                        $('#divBtnInicio').show();

                    } else {
                        $('#divDetalleGeneral').hide();
                        $('#divDetallePorInvernadero').hide();
                        $('#divDetalleSemanal').hide();
                        $('#divDetallesHabilidades').hide();
                        $('#divEjecutado').hide();
                        $('#divPlaneado').hide();
                        popUpAlert(response[1], response[2]);
                    }
                } catch (e) {
                    console.log(e);
                } finally {
                    $.unblockUI();
                }

            });
          
          
        }

    </script>
    <style type="text/css">
          
          div#divDetalleGeneral
          {
              display:none;
          }
          
          div#divDetallePorInvernadero
          {
              display:none;
          }
          
          div#divDetalleSemanal
          {
              display:none;
          }
          
          div#divDetallesHabilidades
          {
              display:none;
          }
          
          #tblDetalleHabilidadesPlaneacion
          {
              display:none;
          }
          
          div#Ejecutado
          {
              display:none;
          }
          
          div#Planeado
          {
              display:none;
          }
          #imgASC
          {
              display:none;
              cursor:pointer;
              float: right;
          }
          
          #imgDESC
          {
              display:none;
              cursor:pointer;
             float : right;
          }

        
        img#imgASC2
        {
            display:none;
             cursor:pointer;
            float: right;
        }
        
        img#imgDESC2
        {
            display:none;
            cursor:pointer;
            float: right;
           
        }
        
        #btnDetallePlan
        {
            display:none;
        }
        
        div#divTablasDetalleSemanalCultivo{
	        height: 192px;
	        overflow-y: auto;
	        overflow-x: hidden;
       }

        div#divTablasDetalleSemanalCosecha{
	        height: 100px;
	        overflow-y: auto;
	        overflow-x: hidden;
        }

        h2{
	        display: block;
	        border-bottom: 1px solid #ADCA98;
	        margin-bottom: 6px;
	        padding-bottom: 3px;
        }

        div#divDetalleSemanal h3{
	        text-align: center;
	        text-transform: uppercase;
	        margin-bottom: 6px;
        }
        
        div#divTitle
        {
             margin-bottom: 10px;
        }
        
        div#divTitle h1
        {
            display: inline;
        }
        
        div#divDesgloseInvernaderoCultivo 
        {
            height: 210px;
            overflow-y: auto;
	        overflow-x: hidden;
        }
            
       div#divDesgloseInvernaderoCosecha 
       {
           height: 145px;
           overflow-y: auto;
	       overflow-x: hidden;
       }
       div#divEjecutado 
       {
           height: 230px;
           overflow-y: auto;
	       overflow-x: hidden;
	       display:none;
       }
           
      div#divPlaneado 
      {
          height: 370px;
          overflow-y: auto;
	      overflow-x: hidden;
	      display:none;
      }
      
      div#divTitle2 h1 {
          display: inline;
      }

      div#divTitle2 {
         margin-bottom: 10px;
      }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
         <h1><asp:Label ID="lblTitulo" runat="server" Text="Reporte Cumplimiento-Ejecucion" ></asp:Label></h1>
         <div id="divTablaPorcentajesGenerales">
         <table class="index2">
            <tr>
               <td>
                  <span><asp:Label ID="Label1" runat="server" Text="Semana:"></asp:Label></span>
                  <input id="txtSemana" type="text" style="float: none; width: 60px; text-align: center;" />
                  <label>del año</label>
                  <input id="txtAnio" type="text" style="float: none; width: 60px; text-align: center;" />
                  <input type="button" id="btnObtenerReporte" class="btn" value="Obtener Reporte" />
               </td>
            </tr>
            <tr>
              <td>
                 <select id="ddlLider">
                 </select>
              </td>
            </tr>
         </table>
         </div>
         <br />
          <h1><asp:Label ID="lblCumplimiento" runat="server" Text="Cumplimiento" ></asp:Label></h1>
         <div id="divDetalleGeneral">
          <h2><asp:Label ID="lblTitulo2" runat="server" Text="Detalle General" ></asp:Label></h2>
          <table id="tblGenerales" class="gridView">
          </table>
         </div>
         <br />
         <div id="divDetallePorInvernadero">
             <h2><asp:Label ID="lblTitulo3" runat="server" Text="Detalle por Invernadero" ></asp:Label></h2>
             <div id="divDesgloseInvernaderoCultivo">
                <table id="tblDesgloseInvernaderoCultivo" class="gridView"> 
                </table>
             </div>
             <div id="divDesgloseInvernaderoCosecha">
               <table id="tblDesgloseInvernaderoCosecha" class="gridView">
               </table>
             </div>
         </div>
         <div id="divDetalleSemanal">
             <h2><asp:Label ID="lblTitulo4" runat="server" Text="Detalle Semanal" ></asp:Label></h2>
             <h3><asp:Label ID="lblTitulo5" runat="server" Text="Cultivo" ></asp:Label></h3>
             <div id="divTablasDetalleSemanalCultivo">
             </div>
             <h3><asp:Label ID="lblTitulo6" runat="server" Text="Cosecha" ></asp:Label></h3>
             <div id="divTablasDetalleSemanalCosecha">
             </div>
         </div>
         <div id="divTitle">
             <h1><asp:Label ID="lblEjecucion" runat="server" Text="Ejecución-Detalle por Invernadero" ></asp:Label></h1>
             <img src="../comun/img/sort_desc.png" id="imgDESC" onclick="expandirDivEjecucion()">
             <img src="../comun/img/sort_asc.png" id="imgASC" onclick="expandirDivEjecucion()">
         </div>
         <div id="divEjecutado">
         </div>
         <div id="divTitle2">
             <h1><asp:Label ID="lblPlaneacion" runat="server" Text="Planeación-Detalle por Invernadero" ></asp:Label></h1>
             <img src="../comun/img/sort_desc.png" id="imgDESC2" onclick="expandirDivPlaneacion()">
             <img src="../comun/img/sort_asc.png" id="imgASC2" onclick="expandirDivPlaneacion()">
         </div>
         <div id="divPlaneado">
         </div>
    </div>
</asp:Content>