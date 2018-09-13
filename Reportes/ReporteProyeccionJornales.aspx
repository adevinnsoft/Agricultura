<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReporteProyeccionJornales.aspx.cs" Inherits="Reportes_ReporteProyeccionJornales" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
<script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
<script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
<script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
<script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
<script type="text/javascript">

    var hora = 0,
        ausentismo = 0,
        capacitacion = 0,
        curva = 0;
    var STR_HORA = "Hora",
        STR_AUSENTISMO = "Ausentismo",
        STR_CURVA = "Curva",
        STR_CAPACITACION = "Capacitacion";

    $(function () {
        var semanaActual = $(".semana span").text();
        $('#txtsemanaPartida').val(semanaActual);
        $('#txtanioPartida').val((new Date).getFullYear());
        $("#subPronostico").text(" " + parseInt(semanaActual));
        $('.txtConfiguracion').change(function () {
            var nSemanas = $('#txtSemanas').val();
            var nHistoricos = $('#txtNumHistoricos').val();
            var semanaPartida = $('#txtsemanaPartida').val();
            var anioPartida = $('#txtanioPartida').val();

            if (nSemanas != null && parseInt(nSemanas) > 0
               && nHistoricos != null && parseInt(nHistoricos) > 0
               && semanaPartida != null && parseInt(semanaPartida) > 0
               && anioPartida != null && parseInt(anioPartida) > 0) {
                genTablaConfiguraciones(nSemanas, nHistoricos, semanaPartida, anioPartida);
            }
        });

        $("#txtNumHistoricos").val("5").change();
        $('#txtSemanas').val("6").change();

        $('#btnObtenerReporte').click(function () {
            if (validarCampos()) {
                obtenerReporteJornales();
            }
        });
        
    });


    function genTablaConfiguraciones(nSemanas, nHistoricos, semanaPartida, anioPartida) {

        PageMethods.generarConfiguracion(nSemanas, nHistoricos, semanaPartida, anioPartida, function (response) {
            //window.console && console.log("JSON " + response);
            var json = JSON.parse(response);
            var code = " <table id=\"tblConfiguraciones\">";
            code += genHeaderSemanas(json);
            code += genTextRow(nSemanas, STR_HORA, json);
            code += genTextRow(nSemanas, STR_AUSENTISMO, json);
            code += genTextRow(nSemanas, STR_CAPACITACION, json);
            code += genTextRow(1, STR_CURVA, json);
            code += "</table>";
            $("#divOpcionesDinamicas").html(code);
        });

    }

    function genHeaderSemanas(json) {
        //  var nSemana = $("#txtSemanaPartida").val();
        var code = "<tr>";
        var cont = 0;
        var semana = "Semana ";
        while (cont < json.length) {
            //   nSemana = (nSemana == 52 ? 0 : nSemana);
            code += "<td class=\"semanaConfig\" semana=\"" + json[cont].semanaNS + "\" anio=\"" + json[cont].anioNS + "\">" + semana + " " + json[cont].semanaNS + "</td>";
            cont++;

        }
        code += "</tr>"
        return code;
    }

    function genTextRow(cols, name, json) {
        var nSemana = $("#txtSemanaPartida").val();
        var code = "<tr id=\"row" + name + "\">";
        var cont = 0;
        while (cont < json.length) {
            code += "<td><input type=\"text\" class=\"txtConfiguracion floatValidate\" " /*/id=\"txtNumSemanas\"*/ + " value=\""
                        + (name == STR_HORA ? json[cont].hora : (name == STR_AUSENTISMO ? json[cont].ausentismo : (name == STR_CAPACITACION ? json[cont].capacitacion : (name == STR_CURVA ? json[cont].curva : getConstante(name)))))
                        + "\" semana=\"" + json[cont].semanaNS + "\" anio=\"" + json[cont].anioNS + "\"/></td>";
            cont++;
            //                if (name == STR_CURVA)
            //                    break;
        }
        code += "</tr>"
        return code;
    }


    function getConstante(name) {
        switch (name) {
            case STR_HORA:
                return 40;
                break;
            case STR_AUSENTISMO:
                return 2.5;
                break;
            case STR_CURVA:
                return 103;
                break;
            case STR_CAPACITACION:
                return 0.0;
                break;
            default:
                return null;
                break;
        }
    }


    function convertirHorasJornales() {
        var listSemanas = new Array();
        $('th.semana').each(function () {
            listSemanas.push($(this).attr('semana'));

        });

        var listFamilias = new Array();
        $('th.thfamilia').each(function () {
            listFamilias.push($(this).attr('id')); //crear un array con las familias
        });

        Array.prototype.unique = function (a) {
            return function () { return this.filter(a) }
        } (function (a, b, c) {
            return c.indexOf(a, b + 1) < 0
        });

        listFamilias.unique();
       

        var i = 1;
        $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').each(function () {
            for (var semana in listSemanas) {
                var week = listSemanas[semana];
                for (var familia in listFamilias.unique()) {
                    var family = listFamilias[familia];
                    week = listSemanas[semana];
                    $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdhoras' + i + '[semana="' + week + '"][familia="' + family + '"]').each(function () {
                        var categoria = $(this).attr('categoria');
                        week = listSemanas[semana];
                        week = week.substr(5, week.length - 1);
                        if (categoria != 'Cosecha') {
                            var horasHombre = parseInt($(this).text());
                            var horasLaborales = parseInt($('#tblConfiguraciones tbody tr#rowHora td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            var porcentajeAusentismo = parseFloat($('#tblConfiguraciones tbody tr#rowAusentismo td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            var porcentajeCapacitacion = parseFloat($('#tblConfiguraciones tbody tr#rowCapacitacion td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            var totalJornales = (horasHombre + ((horasHombre / 100) * porcentajeAusentismo) + ((horasHombre / 100) * porcentajeCapacitacion)) / horasLaborales;
                            totalJornales = totalJornales.toFixed(2);
                            $(this).text(Math.ceil(totalJornales));
                        } else {
                            var horasHombre = parseInt($(this).text());
                            var horasLaborales = parseInt($('#tblConfiguraciones tbody tr#rowHora td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            var porcentajeCurva = parseFloat($('#tblConfiguraciones tbody tr#rowCurva td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            var porcentajeAusentismo = parseFloat($('#tblConfiguraciones tbody tr#rowAusentismo td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            var porcentajeCapacitacion = parseFloat($('#tblConfiguraciones tbody tr#rowCapacitacion td').find('input.txtConfiguracion[semana="' + week + '"]').val());
                            horasHombre = ((horasHombre / 100) * porcentajeCurva);
                            var totalJornales = (horasHombre + ((horasHombre / 100) * porcentajeAusentismo) + ((horasHombre / 100) * porcentajeCapacitacion)) / horasLaborales;
                            totalJornales = totalJornales.toFixed(2);
                            $(this).text(Math.ceil(totalJornales));
                        }
                    });
                }
            }
            i++;
        });
        actualizarTotalesP();
        actualizarTotalesGenerales();
    }


    function JSONConfiguraciones() {
        try {
            return JSONConfiguraciones = $('#tblConfiguraciones tbody td.semanaConfig').map(function () {
                var semana = $(this).attr('semana');
                var anio = $(this).attr('anio');
                return Configuraciones = {
                    anio: anio,
                    semana: semana,
                    horas: $('#tblConfiguraciones tbody tr#rowHora td').find('input.txtConfiguracion[semana="' + semana + '"][anio="' + anio + '"]').val(),
                    ausentismo: $('#tblConfiguraciones tbody tr#rowAusentismo td').find('input.txtConfiguracion[semana="' + semana + '"][anio="' + anio + '"]').val(),
                    capacitacion: $('#tblConfiguraciones tbody tr#rowCapacitacion td').find('input.txtConfiguracion[semana="' + semana + '"][anio="' + anio + '"]').val(),
                    curva: $('#tblConfiguraciones tbody tr#rowCurva td').find('input.txtConfiguracion[semana="' + semana + '"][anio="' + anio + '"]').val(),
                    pronosticoDetalle: $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td[tieneJornales="1"][semana="' + anio + '_' + semana + '"]').map(function () {
                        var idLider = $(this).attr('idLider');
                        var idUsuario = $(this).attr('idUsuario');
                        var producto = $('.gridView thead tr.trProducto').attr('producto');
                        return {
                            idLider: idLider,
                            idUsuario: idUsuario,
                            semana: semana,
                            anio: anio,
                            producto: producto,
                            idFamilia: $(this).attr('idFamilia'),
                            idCategoria: $(this).attr('idCategoria') == 'C' ? 0 : $(this).attr('idCategoria'),
                            esCosecha: $(this).attr('idCategoria') == 'C' ? 1 : 0,
                            jornales: $(this).text()
                        }

                    }).get(),
                    pronosticoSemana: $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td[tieneActivosGenerales="1"][semana="' + anio + '_' + semana + '"]').map(function () {
                        var idLider = $(this).attr('idLider');
                        var idUsuario = $(this).attr('idUsuario');
                        var producto = $('.gridView thead tr.trProducto').attr('producto');
                        return {
                            idLider: idLider,
                            idUsuario: idUsuario,
                            semana: semana,
                            anio: anio,
                            producto: producto,
                            totalGeneral: $(this).text()
                        }
                    }).get()
                }
            }).get();
        } catch (e) {
        console.log(e);
        }
    }


    function actualizarTotalesP() {
        var listSemanas = new Array();
        $('th.semana').each(function () {
            listSemanas.push($(this).attr('semana'));

        });

        var listFamilias = new Array();
        $('th.thfamilia').each(function () {
            listFamilias.push($(this).attr('id')); //crear un array con las familias
        });

        Array.prototype.unique = function (a) {
            return function () { return this.filter(a) }
        } (function (a, b, c) {
            return c.indexOf(a, b + 1) < 0
        });

        listFamilias.unique();


        var i = 1;
        $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').each(function () {

            for (var semana in listSemanas) {
                var week = listSemanas[semana];
                for (var familia in listFamilias.unique()) {
                    var totalActivos = 0;
                    var family = listFamilias[familia];
                    $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdhoras' + i + '[semana="' + week + '"][familia="' + family + '"]').each(function () {
                        totalActivos = totalActivos + parseFloat($(this).text());

                    });
                    $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdactivos' + i + '[semana="' + week + '"][familia="' + family + '"]').each(function () {
                        $(this).text(Math.ceil(totalActivos.toFixed(2)));
                    });
                }
            }
            i++;
        });
    }


    function actualizarTotalesGenerales() {
        var listSemanas = new Array();
        $('th.semana').each(function () {
            listSemanas.push($(this).attr('semana'));

        });

        var listFamilias = new Array();
        $('th.thfamilia').each(function () {
            listFamilias.push($(this).attr('id')); //crear un array con las familias
        });

        Array.prototype.unique = function (a) {
            return function () { return this.filter(a) }
        } (function (a, b, c) {
            return c.indexOf(a, b + 1) < 0
        });

        listFamilias.unique();

        var i = 1;
        $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').each(function () {

            for (var semana in listSemanas) {

                var totalActivosGenerales = 0;
                var week = listSemanas[semana];
                $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdactivos' + i + '[semana="' + week + '"]').each(function () {
                    totalActivosGenerales = totalActivosGenerales + parseFloat($(this).text());

                });
                $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdactivosgenerales' + i + '[semana="' + week + '"]').each(function () {
                    $(this).text(Math.ceil(totalActivosGenerales.toFixed(2)));
                });
            }
            i++;
        });
    }



    function obtenerTotalesGenerales() {
        var listSemanas = new Array();
        $('th.semana').each(function () {
            listSemanas.push($(this).attr('semana'));

        });

        var listFamilias = new Array();
        $('th.thfamilia').each(function () {
            listFamilias.push($(this).attr('id')); //crear un array con las familias
        });

        Array.prototype.unique = function (a) {
            return function () { return this.filter(a) }
        } (function (a, b, c) {
            return c.indexOf(a, b + 1) < 0
        });

        listFamilias.unique();


        var i = 1;
        $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').each(function () {

            for (var semana in listSemanas) {

                var totalActivosGenerales = 0;
                var week = listSemanas[semana];

                $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdactivos' + i + '[semana="' + week + '"]').each(function () {
                    totalActivosGenerales = totalActivosGenerales + parseInt($(this).text());

                });
                $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdactivosgenerales' + i + '[semana="' + week + '"]').each(function () {
                    $(this).text(totalActivosGenerales);
                });
            }
            i++;
        });
    }

    function obtenerTotalesP() {
        var listSemanas = new Array();
        $('th.semana').each(function () {
            listSemanas.push($(this).attr('semana'));

        });

        var listFamilias = new Array();
        $('th.thfamilia').each(function () {
            listFamilias.push($(this).attr('id')); //crear un array con las familias
        });

        Array.prototype.unique = function (a) {
            return function () { return this.filter(a) }
        } (function (a, b, c) {
            return c.indexOf(a, b + 1) < 0
        });

        listFamilias.unique();


        var i = 1;
        $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').each(function () {

            for (var semana in listSemanas) {
                var week = listSemanas[semana];
                for (var familia in listFamilias.unique()) {
                    var totalActivos = 0;
                    var family = listFamilias[familia];
                    $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdhoras' + i + '[semana="' + week + '"][familia="' + family + '"]').each(function () {
                        totalActivos = totalActivos + parseInt($(this).text());

                    });
                    $('.gridView').find('tbody').find('tr.trLider[cuentaconactivos="1"]').find('td.tdactivos' + i + '[semana="' + week + '"][familia="' + family + '"]').each(function () {
                        $(this).text(totalActivos);
                    });
                }
            }
            i++;
        });
    }

    function generarEncabezados() {
        $('th.thfamilia').each(function () {
            var colspan = parseInt($(this).attr('colspan')) + 1;
            $(this).attr('colspan', colspan);
        });

        var listSemanas = new Array();
        var totaltds = 0;
        $('th.semana').each(function () {
            listSemanas.push($(this).attr('semana'));

        });

        var listFamilias = new Array();
        $('th.thfamilia').each(function () {
            listFamilias.push($(this).attr('id')); //crear un array con las familias
        });

        Array.prototype.unique = function (a) {
            return function () { return this.filter(a) } 
        } (function (a, b, c) {
            return c.indexOf(a, b + 1) < 0
        });

        listFamilias.unique();

        for (var semana in listSemanas) {//agregamos el encabezado de total
            var week = listSemanas[semana];
            for (var familia in listFamilias.unique()) {
                var family = listFamilias[familia];
                var ultimoCategoria = $('.encabezadoCategorias th[semana="' + week + '"][familia="' + family + '"]').last().attr('categoria');
                $('.encabezadoCategorias th[semana="' + week + '"][familia="' + family + '"]').each(function () {
                    var categoria = $(this).attr('categoria');
                    if (categoria == ultimoCategoria) {
                        $(this).after('<th class="th" color="black" semana="' + week + '" familia="' + family + '">TA</th>');
                    }

                });

            }
        }

        for (var semana in listSemanas) {
            var week = listSemanas[semana];
            var ultimaFamilia = $('th.thfamilia[semana="' + week + '"]').last();
            ultimaFamilia.after('<th></th>');
        }


   
        for (var semana in listSemanas) {
            var week = listSemanas[semana];
            var ultimotd = $('.encabezadoCategorias th[semana="' + week + '"]').last();
            ultimotd.after('<th class="th" color="red">Total General</th>');
        }

        for (var semana in listSemanas) {
            var tamano = $('.encabezadoCategorias th.th').length
        }


        var colspan = tamano / listSemanas.length
        var i = 1;
        $('th.semana').each(function () {
            $(this).attr('colspan', colspan);
        });

        //funciones extras para agregar atributos a los tr's y td's
        $('.gridView tbody tr.trLider').each(function () {
            if ($(this).find('td.tdhoras').length > 0) {
                $(this).attr('cuentaconactivos', '1');
            }
        });

        $('.gridView tbody tr.trLider[cuentaconactivos="1"]').each(function () {
            $(this).find('td.tdhoras').attr('class', 'tdhoras' + i + '');
            $(this).find('td.tdactivos').attr('class', 'tdactivos' + i + '');
            $(this).find('td.tdactivosgenerales').attr('class', 'tdactivosgenerales' + i + '');
            i++;
        });
    }


    function validarCampos() {
        var anioPartida = $('#txtanioPartida').val().trim();
        var semanaPartida = $('#txtsemanaPartida').val().trim();
        var semanas = $('#txtSemanas').val().trim();

        if (anioPartida.lenght == 0) {
            popUpAlert('Ingrese un año de partida inicial');
            return false;
        } else if (semanaPartida.length == 0) {
            popUpAlert('Ingrese una semana de partida inicial');
            return false;
        }else if(semanas.length == 0){
            popUpAlert('Ingrese las semanas que se desean pronosticar');
            return false;
        }

        return true;
    }

    function obtenerIdPlanta() {
        return $('[id*="ddlPlanta"] option:selected').val();
    }

    function obtenerReporteJornales() {

        var anioPartida = $('#txtanioPartida').val().trim();
        var semanaPartida = $('#txtsemanaPartida').val().trim();
        var semanas = $('#txtSemanas').val().trim();

        try {
            $.blockUI();
            PageMethods.obtenerReporteJornales(obtenerIdPlanta(), anioPartida, semanaPartida, semanas, function (response) {
                if (response[0] == '1') {

                    $('#divJornales').html(response[2]);
                    $('#Asociados').html(response[3]);
                    //$('#divAuxiliar').html(response[3]);
                    generarEncabezados();
                    obtenerTotalesP();
                    obtenerTotalesGenerales();
                    convertirHorasJornales();
                    $('#btnGuardarReporte').click(function () {
                        guardarReporte();
                    });
                } else {
                    popUpAlert(response[1], response[2]);
                }
            });
        } catch (e) {
          console.log();
      } finally {
          $.unblockUI();
     }
  }

  function guardarReporte() {
      var anioPartida = $('#txtanioPartida').val();
      var semanaPartida = $('#txtsemanaPartida').val();
      var semanas = $('#txtSemanas').val();
      var eficienciaHistorica = $('#txtNumHistoricos').val();

      PageMethods.guardarReporte(anioPartida, semanaPartida, semanas, eficienciaHistorica, JSONConfiguraciones(), function (response) {
          if (response[0] == '1') {
              popUpAlert(response[1], response[2]);
          } else {
              popUpAlert(response[1],response[2]);
          }

      });
  }


</script>
<style type="text/css">
    #tblConfiguraciones tr:nth-child(odd)
    {
        background-color: #fff;
    }
    
    #tblConfiguraciones tr:nth-child(even)
    {
        background-color: #d6dfd0;
    }
    
    table#tblConfiguraciones
    {
        min-width: 650px;
    }
    table#tblConfiguraciones td
    {
        height: 25px;
        text-align: center;
        text-transform: uppercase;
        font-weight: bold;
        color: #F60;
    }
    #tblConfiguraciones input[type="text"]
    {
        min-width: 80px;
        text-align: center;
        margin-left: 10px;
        margin-right: 10px;
    }
    
    input.txtConfiguracion
    {
        width: 60px;
        text-align: center;
    }
    
    div#divJornales
    {
        width: 1090px;
        overflow-x: auto;
        overflow-y: hidden;
        margin-left: -2px;
    }
    
    div#tblReporteJornales .gridView
    {
        margin-bottom: 0 !important;
    }
    
    table.gridView td[color="red"]
    {
        color: red;
        font-weight: bold;
    }
    
    table.gridView td[color="black"]
    {
        color: black;
        font-weight: bold;
    }
    
    table.gridView td[color="green"]
    {
        color: green;
        font-weight: bold;
    }
    input#btnGuardarReporte
    {
        margin-top: 15px;
    }
    
    div#tblReporteJornales
    {
        width: 100%;
        max-width: 1070px;
        display: flex;
        justify-content: space-between;
        margin-top: 25px;
    }
    
    table#tblAsociados
    {
        min-width: 320px;
    }
    
    tr#trDatos td:nth-child(odd)
    {
        text-align: right;
    }
    
    tr#trDatos td:nth-child(even)
    {
        text-align: left;
    }
    
    td.tdLider
    {
        color: black;
        font-weight: bold;
    }
    
    td.tdidLider
    {
        font-weight: bold;
    }
    
    tr.encabezadoCategorias th[color="green"]
    {
        color: green;
    }
    
    tr.encabezadoCategorias th[color="red"]
    {
        color: red;
    }
</style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Proyección de Jornales OL</asp:Label>
        </h1>
        <table class="index2">
            <tr>
                <td colspan="5">
                    <h2>
                        <label>Pronóstico de Jornales OL - Semana</label>
                        <span id="subPronostico"></span>
                    </h2>
                </td>
            </tr>
            <tr id="trDatos">
               <td><label>Año de partida:</label></td>
               <td><input type="text" class="txtConfiguracion intValidate required" id="txtanioPartida" /></td>
               <td><label>Semana de Partida:</label></td>
               <td><input type="text" class="txtConfiguracion intValidate required" id="txtsemanaPartida" /></td>
               <td><label>Semanas a pronosticar:</label></td>
               <td> <input type="text" class="txtConfiguracion intValidate" id="txtSemanas" /></td>
               <td><label>Eficiencia Histórica</label></td>
               <td><input type="text" class="txtConfiguracion intValidate" id="txtNumHistoricos" value="5"/></td>
            </tr>
            <tr>
                <td></td>
                <td rowspan="5" colspan="4"><div id="divOpcionesDinamicas"></div></td>
            </tr>
            <tr>
                <td><label>Hora:</label></td>
            </tr>
            <tr>
                <td><label>Ausentismo(%):</label></td>
            </tr>
            <tr>
                <td><label>Capacitacitación(%):</label></td>
            </tr>
            <tr>
                <td><label>Curva(%):</label></td>
            </tr>
            <tr>
              <td colspan="8">
                <input type="button" class="button" id="btnObtenerReporte" value="Obtener Reporte" />
              </td>
            </tr>
        </table>
        <div id="tblReporteJornales">
            <div id="Asociados">
            </div>
            <div id="divJornales">
         
            </div>
        </div>
        <input type="button" class="button" id="btnGuardarReporte" value="Guardar Reporte" />
        <div id="divAuxiliar">
        </div>
    </div>
</asp:Content>