<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmPreharvest.aspx.cs" Inherits="Reportes_frmPreHarvest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/moment.min.js" type="text/javascript"></script>
    <link href="../comun/scripts/fullcalendar.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/fullcalendar.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fullcalendar_es.js" type="text/javascript"></script>
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/fixed_table_rc.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fixedtables/jquery.tablesorter.combined.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fixedtables/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fixedtables/jquery.tablesorter.widgets.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fixedtables/widgets/widget-scroller.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
        /*----- Filtros de la tabla ------*/
        #imgdescarga
        {
            display:none;
        }
        .container
        {
            width:1000px;
        }
        .tablesorter .filtered
        {
            display: none;
        }
        /* Ajax error row */
        .tablesorter .tablesorter-errorRow td
        {
            text-align: center;
            cursor: pointer;
            background-color: #e6bf99;
        }
        #SelectorInterior{
            background: #fff;
            border: 2px solid black;
            width: 221px;
            margin: 0 auto;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
        }
        /* Fixed column scroll bar spacer styling */
.tablesorter-scroller-bar-spacer {
  background: #eee;
}
/* add border to right side (LTR pages) of fixed column */
.tablesorter-scroller-fixed:after {
  content: '';
  border-right: 1px solid #444;
  width: 1px;
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 2;
  /* set to zero for non-jquery ui themes; use "left" here for RTL pages */
  right: 0;
  /* match the margins set to the table to keep the border the same height as the table */
  margin: 10px 0 15px;
}

/* using-x-theme added by the demo code */
.using-jui-theme .tablesorter-scroller-fixed:after {
  /* set to -2px for jquery ui themes; use "left" here for RTL pages */
  right: -2px;
}
.using-green-theme .tablesorter-scroller-fixed:after,
.using-black-ice-theme .tablesorter-scroller-fixed:after,
.using-dark-theme .tablesorter-scroller-fixed:after,
.using-dropbox-theme .tablesorter-scroller-fixed:after {
  /* match the margins set to the table to keep the border the same height as the table */
  margin: 0;
}

/* OPTIONAL CSS! */
#fixed-columns-table tbody td {
  /* force "Notes" column to not wrap, so we get a horizontal scrolling demo! */
  white-space: nowrap;
  /* Add min column width, or "Index" column filter gets too narrow to use */
  min-width: 60px;
}
    </style>
    <script type="text/javascript">
        $(function () {
            $('#ctl00_ddlPlanta').live('change', function () {
                ObtenerReportePreharvest();
            });
        });

        function funcionesTabla() {

            var startFixedColumns = 2;

            $(".gridView")
				 .tablesorter({
				     widthFixed: true,
				     widgets: ['zebra', 'filter','scroller'],
				     headers: {4 : { filter: false}, 
                     5 : { filter: false},
                     6 : { filter: false},
                     7 : { filter: false},
                     8 : { filter: false},
                     9 : { filter: false}
				     },
				     widgetOptions: {
				         zebra: ["gridView", "gridViewAlt"]
				         //filter_hideFilters: true // Autohide
                         ,
				         scroller_upAfterSort: true,
				         // pop table header into view while scrolling up the page
				         scroller_jumpToHeader: true,

				         scroller_height: 400,
				         // set number of columns to fix
				         scroller_fixedColumns: startFixedColumns,
				         // add a fixed column overlay for styling
				         scroller_addFixedOverlay: true,
				         // add hover highlighting to the fixed column (disable if it causes slowing)
				         scroller_rowHighlight: 'hover',

				         // bar width is now calculated; set a value to override
				         scroller_barWidth: null
				     }
				 });

				 $('#slider').slider({
				     value: startFixedColumns,
				     min: 0,
				     max: 4,
				     step: 1,
				     slide: function (event, ui) {
				         // page indicator
				         $('.fixed-columns').text(ui.value);
				         // method to update the fixed column size
				         $('.gridView').trigger('setFixedColumnSize', ui.value);
				     }
				 });

				 // update column value display
				 $('.fixed-columns').text(startFixedColumns);

				 $(".tablesorter-filter.disabled").hide();
        }

        function ObtenerReportePreharvest() {
            $.blockUI();
            var inicio = $('#finicio').val();
            var fin = $('#ffin').val();

            PageMethods.ObtenerPreharvest(inicio, fin, function (response) {
                $('#preharvest').html(response);
                $('.gridView').trigger('destroy')

                funcionesTabla();

                $('#imgdescarga').show();
                $.unblockUI();
            });
        }
        function asignarFecha() {
            $(ctrlFechaActual).val($("#DateDemo").val());
            $('#popUpFecha').hide();
            $(ctrlFechaActual).change();
        }
        function descargaExcel() {
        $('#tblPreharvest').each(function () {
              var clone = $(this).clone();
              clone.find('.invisible').remove();
              clone.find('td').css({ 'border': '1px solid black' });

              window.open('data:application/vnd.ms-excel,' + encodeURIComponent('<table style="border:1 px solid black;">' + clone.html() + '</table>'));
          });
        }

        $(function () {
            var fechaInicio = new Date();
            var fechaFin = new Date();
            var parametroInicio = fechaInicio.getFullYear().toString() + "/" + (fechaInicio.getMonth() + 1).toString() + "/" + fechaInicio.getDate().toString();
            var parametroFin = fechaFin.getFullYear().toString() + "/" + (fechaFin.getMonth() + 1).toString() + "/" + fechaFin.getDate().toString();

            
            $('input.fechaCaptura').val(function () {
                var fecha = new Date();
                var yyyy = fecha.getFullYear().toString();
                var mm = (fecha.getMonth() + 1).toString(); // getMonth() is zero-based
                var dd = fecha.getDate().toString();
                return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]); // padding
            });
            
            $('.fechaCaptura').live('click', function () {
                $('#DateDemo').val($(this).val()).click();
                $('#popUpFecha').show();
                ctrlFechaActual = $(this);
            });

            

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            Reporte Preharvest</h1>
        <table id="Fechas" class="index">
       
            <tr>
            
                <td align="center" style="text-align: center;">
                <img alt="Descarga" id="imgdescarga" src="../comun/img/download_xls.png" onclick="descargaExcel($(this));" style="cursor:pointer;"/>
                        Inicio:
                    <input id="finicio" class="dateInicio fechaCaptura"  style="float: none; width: 100px; text-align: center;"
                        readonly />
                        Fin:
                    <input id="ffin"  class="dateFin fechaCaptura" style="float: none; width: 100px; text-align: center;"
                        readonly />
                        <input type="button" class="cajachica" onclick="ObtenerReportePreharvest()" value="Obtener Reporte"/>
                </td>
            </tr>
        </table>
        <br />
        <table id="tblPreharvest" class="gridView">
            <thead>
                <tr>
                    <th>
                        Planta
                    </th>
                    <th>
                        Invernadero
                    </th>
                    <th>
                        Zona
                    </th>
                    
                    <th>
                        cosecha
                    </th>
                    <th>
                        FullPreharvest
                    </th>
                    <th>
                        hora
                    </th>
                    <th>
                        Seccion
                    </th>
                    <th>
                        Brix
                    </th>
                    <th>
                        Calidad
                    </th>
                    <th>
                        Folio
                    </th>
                    <th>
                        FolioEmpaque
                    </th>
                </tr>
            </thead>
            <tbody id="preharvest">
            </tbody>
        </table>
    </div>
      <div id="popUpFecha" class="popUp">
            <div id="SelectorInterior">
                <input type="text" id="DateDemo" />
                <br />
                <input type="button" id="Button1" value="Cancelar" class="cajaChica" onclick="$('#popUpFecha').hide();"
                    style="float: none;" />
                <input type="button" id="btnSeleccionarFecha" class="cajaChica" value="OK" onclick="asignarFecha();"
                    style="float: none;" />
            </div>
            <script type="text/javascript">
                $("#DateDemo").AnyTime_picker({
                    format: "%Y-%m-%d",
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
</asp:Content>
