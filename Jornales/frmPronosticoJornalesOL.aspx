<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  CodeFile="frmPronosticoJornalesOL.aspx.cs" Inherits="Jornales_frmPronosticoJornalesOL" %>
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
        $('#txtsemanaP').val(semanaActual);
        $('#txtanioP').val((new Date).getFullYear());
        $("#subPronostico").text(" " + parseInt(semanaActual));

        $('.configuraciones').hide()
        $('#txtsemanaP').change( function(){obtenerListaPronostico()});
        $('#txtanioP').change(function () { obtenerListaPronostico() });
        obtenerListaPronostico();

        //        obtenerReporteJornales();
        //        genTablaConfiguraciones();
        //        $('input').attr('readonly', true);
    });


    function obtenerReporteInicial() {
        obtenerReporteJornales();
    }

    function genTablaConfiguraciones() {

        PageMethods.generarConfiguracion(function (response) {
            //window.console && console.log("JSON " + response);

            var json = JSON.parse(response);
            var nSemanas = json.length;
            var code = " <table id=\"tblConfiguraciones\">";

            code += genHeaderSemanas(json);
            code += genTextRow(nSemanas, STR_HORA, json);
            code += genTextRow(nSemanas, STR_AUSENTISMO, json);
            code += genTextRow(nSemanas, STR_CAPACITACION, json);
            code += genTextRow(1, STR_CURVA, json);
            code += "</table>";
            $("#divOpcionesDinamicas").html(code);

            $('input#txtanioPartida').val(json[0].anioNS)
            $('input#txtsemanaPartida').val(json[0].semanaNS)
            $('input#txtSemanas').val(json.length)
        });

    }

    function genHeaderSemanas(json) {
        var code = "<tr>";
        var cont = 0;
        var semana = "Semana ";
        while (cont < json.length) {
            code += "<td nSemana=\"" + json[cont].cont + "\" semana=\"" + json[cont].semanaNS + "\" anio=\"" + json[cont].anioNS + "\">" + json[cont].week + "</td>";
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
                        + "\" semana=\"" + json[cont].semanaNS + "\" anio=\"" + json[cont].anioNS + "\" readonly/></td>";
            cont++;
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

    function JSONActividades() { return jsonEtapas; }

    function conversionHorasJornales() {
        var semanaActual = parseInt($('#txtsemanaPartida').val());
        $('.tblDatosOL').find('.Horas'/*:not(.Horas[semana="' + semanaActual + '"])'*/).each(function () {
            var totalJornales = parseFloat($(this).text());


             totalJornales = totalJornales.toFixed(2);
             $(this).text(totalJornales);

        });
    }


    function agregarColspanSemanas() {
        $('.tblDatosOL thead th.Semana').each(function () {
            var semana = $(this).attr('semana');
            var anio = $(this).attr('anio');
            var numCategorias = $(this).parent().parent().parent().find('.Categoria[semana="' + semana + '"][anio="' + anio + '"]').length + 1;
            $(this).attr('colspan', numCategorias);
        });
    }

    function calculoActivos() {
        $('.tblDatosOL').find('td.TotalActivos').each(function () {
            var totalActivos = 0;
            var semana = $(this).attr('semana');
            var anio = $(this).attr('anio');
            $(this).parent().find('.Horas[semana="' + semana + '"][anio="' + anio + '"]').each(function () {
                totalActivos += parseFloat($(this).text());
            });
            $(this).text(totalActivos.toFixed(2));
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
        } else if (semanas.length == 0) {
            popUpAlert('Ingrese las semanas que se desean pronosticar');
            return false;
        }

        return true;
    }

    function obtenerReporteJornales() {
        bloqueoDePantalla.bloquearPantalla();
        try {
            PageMethods.obtenerReporteJornales( function (response) {
                if (response[0] == '1') {
                    $('#PanelA').html(response[2]);
                    $('#PanelB').html(response[3]);
                    agregarColspanSemanas();
                    conversionHorasJornales();
                    calculoActivos();
                    calculaFooter();

                    bloqueoDePantalla.indicarTerminoDeTransaccion();

                } else {
                    popUpAlert(response[1], response[2]);
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

    function guardarReporte() {
        var anioPartida = $('#txtanioPartida').val();
        var semanaPartida = $('#txtsemanaPartida').val();
        var semanas = $('#txtSemanas').val();
        var eficienciaHistorica = $('#txtNumHistoricos').val();

        PageMethods.guardarReporte(anioPartida, semanaPartida, semanas, eficienciaHistorica, JSONConfiguraciones(), function (response) {
            if (response[0] == '1') {
                popUpAlert(response[1], response[2]);
            } else {
                popUpAlert(response[1], response[2]);
            }

        });
    }


    function calculaFooter() {
        $('table.tblDatosOL[producto] th.Total').each(function () {
            var hTotal = 0.00;
          //  if ($(this).attr('familia') != 'T') {
            $(this).parent().parent().find(
                'tr.trRow> td' + ($(this).attr('familia') != 'T' ? '.Horas' : '.TotalActivos')
                + '[semana="' + $(this).attr("semana") + '"][anio="' + $(this).attr("anio") + '"][familia="' + $(this).attr("familia") + '"][categoria="' + $(this).attr("categoria") + '"]'
             ).each(function () {
                hTotal += parseFloat($(this).text()) 
                })
          //  } else {
          //      $(this).parent().parent().find('tr.trRow> td.TotalActivos[semana="' + $(this).attr("semana") + '"][anio="' + $(this).attr("anio") + '"][familia="' + $(this).attr("familia") + '"][categoria="' + $(this).attr("categoria") + '"]').each(function () { hTotal += parseFloat($(this).text()) })
          //  }
            $(this).find('span').text(hTotal.toFixed(2))
        })
    }

    function obtenerListaPronostico() {
        bloqueoDePantalla.bloquearPantalla();
        var anioPartida = 0//$('#txtanioP').val()
        var semanaPartida = 0//$('#txtsemanaP').val()

        $('.table').html('');
        PageMethods.obtenerListaPronosticos(anioPartida, semanaPartida, function (response) {
            if (response[0] == '1') {
                $('.table').html(response[1]);
            } else {
                popUpAlert(response[1],response[2]);
            }
            bloqueoDePantalla.indicarTerminoDeTransaccion();







            $(".table").each(function () {

                if ($(this).find("tbody").find("tr").size() >= 1) {

                    window.console && console.log('aplicaFormato');
                    $(this).trigger('destroy');
                    $(this).tablesorter({
                        widthFixed: true
                        , widgets: ['zebra', 'filter']
                        //, headers: { 1: { filter: true} }
                        ,widgetOptions: {
                            zebra: ["table", "gridViewAlt"]
                            , filter_hideFilters: true // Autohide
                        }
                    }); //.tablesorterPager(pagerOptions);
                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                }


            });

            $('.table thead .tablesorter-filter-row td input').each(function () {
                if ($(this).attr('data-column') > 1) {
                    $(this).parent().addClass('oculto');
                }
            });





        });
        bloqueoDePantalla.desbloquearPantalla();
    }

    function cargarPronostico(idPronosticoJornales) {

        bloqueoDePantalla.bloquearPantalla();
        $('#PanelA').html();
        $('#PanelB').html();
        $('.configuraciones').hide();
        PageMethods.obtenerPronostico(idPronosticoJornales, function (response) {
            if (response[0] == '1') {
                $('#PanelA').html(response[2]);
                $('#PanelB').html(response[3]);
                agregarColspanSemanas();
                conversionHorasJornales();
                calculoActivos();
                calculaFooter();



                var json = JSON.parse(response[4]);
                var nSemanas = json.length;
                var code = " <table id=\"tblConfiguraciones\">";

                code += genHeaderSemanas(json);
                code += genTextRow(nSemanas, STR_HORA, json);
                code += genTextRow(nSemanas, STR_AUSENTISMO, json);
                code += genTextRow(nSemanas, STR_CAPACITACION, json);
                code += genTextRow(1, STR_CURVA, json);
                code += "</table>";
                $("#divOpcionesDinamicas").html(code);

                $('input#txtanioPartida').val(json[0].anioNS)
                $('input#txtsemanaPartida').val(json[0].semanaNS)
                $('input#txtSemanas').val(json.length)

                $('.configuraciones').show();



                bloqueoDePantalla.indicarTerminoDeTransaccion();

            } else {
                popUpAlert(response[1]);
                bloqueoDePantalla.indicarTerminoDeTransaccion();
            }
        });
        bloqueoDePantalla.desbloquearPantalla();
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
    
    /*table#tblAsociados
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
    }*/
    
    
    div#contenderodReporte
    {
        display: flex;
        justify-content: space-between;
    }
    
    div#PanelB
    {
        max-width: 800px;
        overflow-x: auto;
        margin: 0px;
    }
    
    div#contenderodReporte table th
    {
        white-space: nowrap;
        padding: 4px 8px;
        background: #76933C;
        color: white;
    }
    
    div#contenderodReporte table, div#contenderodReporte table tr
    {
        border: 1px solid #adc995;
        text-align: center;
    }
    
    div#contenderodReporte table th:last-child
    {
        border-right: none;
    }
    div#contenderodReporte table th
    {
        /*border-left: 1px solid black;*/
        border-right: 1px solid #adc995;
        background: #f0f5e5;
        text-transform: uppercase;
        color: Black;
    }
    div#contenderodReporte table td
    {
        white-space: nowrap;
        padding: 5px 8px;
    }
    
    div#contenderodReporte table tr:nth-child(odd)
    {
        background: #d6dfd0;
    }
    
    div#contenderodReporte table tr:nth-child(even)
    {
        background: #FFFFFF;
    }
    
    div#PanelB table
    {
        border-left: none;
    }
    
    div#contenderodReporte table thead tr th
    {
        border-bottom: 1px solid #adc995;
    }
    
    td.Horas
    {
        color: green;
        font-weight: bold;
    }
    
    td.TotalActivos
    {
        color: black;
        font-weight: bold;
    }
    
    table.tblProducto td
    {
        font-weight: bold;
    }
    table.index
    {
        margin-top: 10px;
        }
    table.index tr td
    {
        text-align: center;
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
                        <label>Consulta de Pronóstico de Jornales OL </label>
                        <%--<span id="subPronostico"></span>--%>
                    </h2>
                </td>
            </tr>
          <%--  <tr id="trDatos">
               <td><label>Año de partida:</label></td>
               <td><input type="text" class="txtConfiguracion intValidate required" id="txtanioP" /></td>
               <td><label>Semana de Partida:</label></td>
               <td><input type="text" class="txtConfiguracion intValidate required" id="txtsemanaP" /></td>

            </tr>--%>
        </table>

        <table class="table index">
        </table>


        <table  class="index2 configuraciones">
        <tr>
                <td colspan="5">
                    <h2>
                        <label>Configuraciones almacenadas</label>
                        <%--<span id="subPronostico"></span>--%>
                    </h2>
                </td>
            </tr>
            <tr>




               
               <td><label>Año de partida:</label></td>
               <td><input type="text" class="txtConfiguracion intValidate required" id="txtanioPartida" /></td>
               <td><label>Semana de Partida:</label></td>
               <td><input type="text" class="txtConfiguracion intValidate required" id="txtsemanaPartida" /></td>
               <td><label>Semanas pronosticadas:</label></td>
               <td> <input type="text" class="txtConfiguracion intValidate" id="txtSemanas" /></td>
              






               <%--<td><label>Eficiencia Histórica</label></td>--%>
               <%--<td><input type="text" class="txtConfiguracion intValidate" id="txtNumHistoricos" value="5"/></td>--%>
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
                <td><label>Capacitación(%):</label></td>
            </tr>
            <tr>
                <td><label>Curva(%):</label></td>
            </tr>
            <%--<tr>
              <td colspan="8">
                <input type="button" class="button" id="btnObtenerReporte" value="Obtener Reporte" />
              </td>
            </tr>--%>
        </table>
        <div id="contenderodReporte">
            <div id="PanelA"></div>
            <div id="PanelB"></div>
        </div>
        <%--<input type="button" class="button" id="btnGuardarReporte" value="Guardar Reporte" />--%>
    </div>
</asp:Content>