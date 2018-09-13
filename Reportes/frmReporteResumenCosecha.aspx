<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteResumenCosecha.aspx.cs" Inherits="frmReporteResumenCosecha" meta:resourcekey="PageResource1" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <link href="../comun/css/fixed_table_rc.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/fixedtables/widgets/widget-scroller.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
   <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
   <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />


    <link href="../comun/css/jquery-ui-1.8.21.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../comun/scripts/jquery-ui-1.8.21.custom.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../comun/scripts/jsPopUp.js"></script>
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>


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
    <script type="text/javascript" id="reporteEficiencia">
        var idLider = 0;

        $(function () {
            $('.datepicker').val(getDateString())
            $('.datepicker').datepicker({ 'dateFormat': 'yy/mm/dd' }).on('click', function () { $(this).attr('readonly', 'readonly') });


            triggers();

        });

        function getDateString() {
            var date = new Date();
            var dia = date.getDate();
            var mes = date.getMonth() + 1;
            var anio = date.getFullYear();
            var fecha = anio + '/' + (mes < 10 ? '0' : '') + mes + '/' + (dia < 10 ? '0' : '') + dia;

            return fecha;
        }

        function tooltip() {
            $('.help').tooltipster({
                animation: 'fade',
                contentAsHTML: true,
                interactive: true,
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: true,
                trigger: 'hover',
                position: 'left'
            });
        }

        function triggers() {
            bloqueoDePantalla.bloquearPantalla();
            PageMethods.obtenerPlantasDdl( function (result) {
                if(parseInt(result[0]) == 1) {
                    console.log('cargando plantas');
                    $('#ddlPlanta').html('').html(result[2] );
                    //$('#ddlPlanta').val('0');
                    $('#ddlPlanta').val(<%= Session["idPlanta"] != null ? Session["idPlanta"] : '0' %>);
                    
                    dateOnChange();
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                    bloqueoDePantalla.desbloquearPantalla();

                }else {
                    console.log('no se cargaron plantas');
                }
            });
            ;
        }

        function dateOnChange() {
            bloqueoDePantalla.bloquearPantalla();
            console.log('change');
            
            var diaInicio = $('#txtDiaInicio').val();
            var diaFin = $('#txtDiaFin').val();
            var planta = $('#ddlPlanta').val();
            planta = planta != undefined ? planta : 0;
            if (diaInicio.length ==10
                && diaFin.length == 10
                && planta != undefined) {
                console.log('date ok');

                
                        $(".gridView").trigger("destroy");
                        $('#tblEficiencia').html("");
                        $('#imgdescarga').hide();
                PageMethods.obtenerReporte( diaInicio, diaFin,  planta, function (result) {
                

                    if (parseInt(result[0]) == 1) {
                        $('#imgdescarga').show();
                        $('#tblEficiencia').html(result[2].split('@')[0]);
                        $(".gridView").trigger("destroy");

                        if ($("#tblEficiencia").find("tbody").find("tr").size() >= 1) {
                            var pagerOptions = { // Opciones para el  paginador
                                container: $("#pagerReporte"),
                                output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                            };
                           
                            $(".gridView").tablesorter({
                                widthFixed: true, widgets: ['zebra', 'filter'],
                                headers: { /*1: { filter: false }, 2: { filter: false }, 3: { filter: false }, 4: { filter: false }*/
                                },
                                widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                            }).tablesorterPager(pagerOptions);
                           
                            
                            tooltip();
                        } else {
                            $("#pagerReporte").hide();
                            $('#imgdescarga').hide();
                        }

                    }
                    else {
                        popUpAlert(result[2], result[1]);
                        $(".gridView").trigger("destroy");
                        $('#tblEficiencia').html("");
                        $('#imgdescarga').hide();
                    }
                    ﻿
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                });

            } else {
              //  console.log('no actions');
                bloqueoDePantalla.indicarTerminoDeTransaccion();
            }
            bloqueoDePantalla.desbloquearPantalla();
        }

//        function descargaExcel() {
//            $('#tblEficiencia').each(function () {
//                var clone = $(this).clone();
//                clone.find('.invisible').remove();
//                clone.find('.tablesorter-ignoreRow').remove();
//                clone.find('.remove-me').remove();
//                clone.find('td').css({ 'border': '1px solid black' });
//                clone.find('tr[role="row"]').show();

//                window.open('data:application/vnd.ms-excel,' + encodeURIComponent('<table style="border:1 px solid black;">' + clone.html() + '</table>'));
//            });
//        }
       


        function descargaExcel() {
            $('#tblEficiencia').each(function () {
                var clone = $(this).clone();
                clone.find('.invisible').remove();
                clone.find('.tablesorter-ignoreRow').remove();
                clone.find('.remove-me').remove();
                clone.find('td').css({ 'border': '1px solid black' });
                clone.find('tr[role="row"]').show();
				clone.find('tbody tr').each(function(){$(this).find('td').eq(1).addClass('text'); });
                window.open('data:application/vnd.ms-excel,' + encodeURIComponent('<style> .num {mso-number-format:General;} .text {mso-number-format:"\@";/*force text*/} </style><table style="border:1 px solid black;">' + clone.html() + '</table>'));
            });
        }
    </script>
    <style type="text/css">
        #tblDatos tbody tr[id="filtros"]
        {
            display: none;
        }
        
        table.index2 input[type="text"]
        {
            text-align: center;
        }
        
        table#tblEficiencia
        {
            max-width: 900px;
        }
        table#tblDatos
        {
            text-align: right;
        }
    </style>
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Reporte de Cosechas"></asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index2">
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblDiaInicio" runat="server" text="Desde:">
                            </asp:label></span>
                    </td>
                    <td>
                        <span>
                            <input id="txtDiaInicio" class="Texto datepicker" type="text" value="" placeholder="<%=Session["Locale"].ToString() == "es-MX" ? "DD/MM/AAAA" : "DD/MM/YYYY"%>" <%--onchange="javascript:dateOnChange();"--%>/>
                        </span>
                    </td>
                   
                    <td>
                        <span>
                            <asp:label id="lblDiaFin" runat="server" text="Hasta:"></asp:label>
                        </span>
                    </td>
                    <td>
                        <span>
                            <input id="txtDiaFin" class="Texto datepicker" type="text" value="" placeholder="<%=Session["Locale"].ToString() == "es-MX" ? "DD/MM/AAAA" : "DD/MM/YYYY"%>" <%--onchange="javascript:dateOnChange();"--%> />
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:label id="lblPlanta" runat="server" text="Planta:"></asp:label>
                        </span>
                    </td>
                    <td>
                        <span>
                            <select id="ddlPlanta" class="Texto datepicker"  <%--onchange="javascript:dateOnChange();"--%> ></select>
                        </span>
                    </td>
                   
                <td>
                        <span><img alt="Descarga" id="imgdescarga" src="../comun/img/download_xls.png" onclick="descargaExcel($(this));" style="cursor:pointer;"/></span>
                    </td>
                    <td>
                        <input id="btnObtenerReporteEficiencia" type="button" value="Obtener Reporte" onclick="dateOnChange()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="pagerReporte" class="pager" style="width: 100%; min-width: 100%; display: none;">
            <img alt="first" src="../comun/img/first.png" class="first" />
            <img alt="prev" src="../comun/img/prev.png" class="prev" />
            <span class="pagedisplay"></span>
            <img alt="next" src="../comun/img/next.png" class="next" />
            <img alt="last" src="../comun/img/last.png" class="last" />
            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                <option value="20">20</option>
                <option value="40">40</option>
                <option value="60">60</option>
                <option value="80">80</option>
                <option value="100">100</option>
            </select>
            <select class="gotoPage" title="Select page number">
            </select>
        </div>
        <table class="gridView" id="tblEficiencia">
        </table>
    </div>
</asp:content>