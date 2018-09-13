<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Zonificacion.aspx.cs" Inherits="configuracion_Zonificacion" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script src="../comun/scripts/jquery.mask.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script type="text/javascript" id="variableGlobal">
        var objZonificacion = null;
        //var transaccionTerminada = false;
    </script>
    <script type="text/javascript" id="zonificacion">

        $(function () {
            generarMatriz();
            $('#btnGuardarConfiguracion').click(function () {
                //validarFormatoCampos();
                if ($("input.Error").length >= 0) {
                    popUpAlert('Algunas cadenas no tienen el formato correcto.', 'warning');
                } else {
                    guardarConfiguracion();
                }
            });
        });


        function obtenerIdPlanta() {
            return $('[id*="ddlPlanta"] option:selected').val();
        }

        function recargarPagina() {
            setTimeout(function () {
                location.reload();
            }, 1000);
        }

        function quitarSorterColumnasInvernaderos() {
            $('.Encabezado').each(function () {
                var Encabezado = $(this);
                $(Encabezado).attr('class', 'sorter-false');
            });

            $('.gridView thead .tablesorter-header-inner').hover(function () {
                var Encabezado = $(this);
                $(Encabezado).css('text-decoration', 'none');
            });
        }

        //        function validarRangoSurcosChange() {
        //            $('#tblZonificacion tr.Asociado').find('td.surco input#txtSurcos').change(function () {
        //                var inputSurcos = $(this);
        //                var Invernadero = $(inputSurcos).parent().prev().find('input#txtHidden').attr('invernadero');
        //                var nombreAsociado = $(inputSurcos).parent().prev().find('input#txtHidden').attr('nombreAsociado');
        //                var noSurcosInvernadero = parseInt($(inputSurcos).parent().prev().find('input#txtHidden').attr('numeroSurcos'));
        //                var rangoSurcos = $(inputSurcos).val();
        //                rangoSurcos = rangoSurcos.split('-');
        //                var surcoInicial = parseInt(rangoSurcos[0]);
        //                var surcoFinal = parseInt(rangoSurcos[1]);
        //                if ((surcoInicial > noSurcosInvernadero || surcoInicial <= 0) || (surcoFinal > noSurcosInvernadero || surcoFinal <= 0)) {
        //                    popUpAlert('Del asociado ' + nombreAsociado + ' , en el invernadero ' + Invernadero + ' , el número de surco debe estar entre 1 y ' + noSurcosInvernadero + '', 'error');
        //                } else {

        //                }
        //            });
        //        }

        function validarRegex() {
            $(".surco input").mask('ZZZ-ZZZ', { translation: { 'Z': { pattern: /[0-9]/, optional: true}} });

            $(".surco input").change(function () {
                var surcos = parseInt($(this).parent().prev().find('input#txtHidden').attr('numeroSurcos'));
                var v1 = parseInt($(this).val().split("-")[0]);
                var v2 = parseInt($(this).val().split("-")[1]);

                if (v1 <= surcos && v2 <= surcos && v1 <= v2 && v1 > 0) {
                    $(this).removeClass("Error");
                    tooltip();
                    $(this).attr("title", null);
                } else {
                    $(this).removeClass("Error");
                    tooltip();
                    if (isNaN(v1) || isNaN(v2)) { $(this).attr("title", "Rango de surco faltante"); }
                    else {
                        if (v1 <= 0) { $(this).attr("title", "El surcos inicial no puede ser 0 "); }
                        else {
                            if (v1 > v2) { $(this).attr("title", "surco final no puede ser menor a surco inicial"); }
                            else {
                                $(this).attr("title", "El número de surcos debe estar entre 1 y " + surcos);
                            }
                        }
                    }
                    $(this).addClass("Error");
                    tooltip();
                }
            });
        }

        //        function validarFormatoCampos() {
        //            var bandera = 1;
        //            if (bandera == 1) {
        //                $('#tblZonificacion').find('tr').find('td').find('input[vaciosurco="0"]').each(function () {//validamos el formato de los inputs que contengan datos
        //                    var valor = $(this).val();
        //                    if (/^(([0-9]*|[0-9]{2,3})+[-]+([0-9]{2,3}|[0-9]))*$/.test(valor) == true) {
        //                        bandera = 1;
        //                    } else {
        //                        bandera = 0;
        //                    }
        //                });
        //            }

        //            if (bandera == 0) {
        //                popUpAlert('La cadena de entrada no tiene el formato correcto.', 'warning');
        //            } else {
        //                validarRangoSurcosClick()
        //                //guardarConfiguracion();
        //            }
        //        }

        //        function validarRangoSurcosClick() {
        //            var listaInvAsociado = [];
        //            var tabla = '';
        //            var bandera = 0;
        //            $('#tblZonificacion tr.Asociado').find('td.surco input#txtSurcos').each(function () {
        //                var inputSurcos = $(this);
        //                var Invernadero = $(inputSurcos).parent().prev().find('input#txtHidden').attr('invernadero');
        //                var nombreAsociado = $(inputSurcos).parent().prev().find('input#txtHidden').attr('nombreAsociado');
        //                var noSurcosInvernadero = parseInt($(inputSurcos).parent().prev().find('input#txtHidden').attr('numeroSurcos'));
        //                var rangoSurcos = $(inputSurcos).val();
        //                var rangoSurcosSplit = rangoSurcos.split('-');
        //                var surcoInicial = parseInt(rangoSurcosSplit[0]);
        //                var surcoFinal = parseInt(rangoSurcosSplit[1]);
        //                if ((surcoInicial > noSurcosInvernadero || surcoInicial <= 0) || (surcoFinal > noSurcosInvernadero || surcoFinal <= 0)) {
        //                    listaInvAsociado.push(nombreAsociado + ',' + Invernadero + ',' + rangoSurcos + ',' + noSurcosInvernadero);
        //                    bandera = 1;
        //                    //popUpAlert('Del asociado ' + nombreAsociado + ' , en el invernadero ' + Invernadero + ' , el número de surco debe estar entre 1 y ' + noSurcosInvernadero + '', 'error');
        //                } else {

        //                }
        //            });

        //            if (bandera == 1) {
        //                tabla = generarTablaHTML(listaInvAsociado);
        //                popUpAlert(tabla, 'error');
        //            }
        //            else {
        //                guardarConfiguracion();
        //            }
        //        }

        //        function generarTablaHTML(lista) {
        //            var html = '<span class="LeyendaTabla">La siguiente lista de Asociados exceden el número de surcos de sus invernaderos</span>';
        //            html += '<table class="gridView " id="tblSurcosRevasados">';
        //            html += '<thead><tr>';
        //            html += '<th>Asociado</th>';
        //            html += '<th>Invernadero</th>';
        //            html += '<th>Rango de surcos asignados</th>';
        //            html += '<th>Alerta</th>';
        //            html += '</tr></thead>';
        //            html += '<tbody>';
        //            for (var i = 0; i < lista.length; i++) {
        //                var Contenido = lista[i];
        //                Contenido = Contenido.split(',');
        //                var nombreAsociado = Contenido[0];
        //                var Invernadero = Contenido[1];
        //                var rangoDeSurcos = Contenido[2];
        //                var limiteSurcos = Contenido[3];
        //                html += '<tr class="Excepcion"><td>' + nombreAsociado + '</td><td>' + Invernadero + '</td><td>' + rangoDeSurcos + '</td><td>El número de surcos debe estar entre 1 y ' + limiteSurcos + '</td></tr>';
        //            }
        //            html += '</tbody></table>';

        //            return html;
        //        }

        function generarEncabezadoFijoDinamico() {
            var primerHeader = $('table.gridView thead tr th').first();
            var widthPrimerHeader = $('table.gridView thead tr th:first-child').width();
            var divWidth = $('.index').width();
            var nHeaders = $('table.gridView tr th:not(:eq(0))').length;
            divWidth = divWidth - widthPrimerHeader;
            var nPixeles = divWidth / nHeaders;
            $(primerHeader).css('width', widthPrimerHeader);
            $('table.gridView tr th:not(:eq(0))').css('width', nPixeles);
            $('table.gridView tbody tr td:not(:eq(0))').css('width', nPixeles);
            $('table.gridView tbody tr').find('td.nombreAsociado:eq(0)').each(function () {
                var td = $(this);
                $(td).css('width', widthPrimerHeader);
            });
            $('table.gridView tbody tr td:first-child').css('width', nPixeles);
            $('div.index table.gridView thead').css('display', 'block');
            $('div.index table.gridView tbody').css('display', 'block');
            $('div.index table.gridView tbody').css('height', '350px');
            $('div.index table.gridView tbody').css('overflow-y', 'scroll');
        }



        function registerControls3() {
            if ($("#tblZonificacion").find("tbody").find("tr").size() >= 1) {
                var pagerOptions = { // Opciones para el  paginador
                    container: $("#pager"),
                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                };

                $("#tblZonificacion")
				 .tablesorter({
				     widthFixed: true,
				     widgets: ['zebra', 'filter'],
				     headers: { 1: { filter: false }
				     },
				     widgetOptions: {
				         zebra: ["gridView", "gridViewAlt"],
				         filter_hideFilters: true // Autohide
				     }
				 })
                validarRegex();
                $(".tablesorter-filter.disabled").hide(); // hide disabled filters
            }
            else {

            }
        }

        function ordenarSurcos(arreglo) {//algoritmo de ordenamiento burbuja para ordenar los surcos
            var auxiliar;
            var cambio = false;

            while (true) {
                cambio = false;
                for (var i = 1; i < arreglo.length; i++) {
                    if (arreglo[i] < arreglo[i - 1]) {
                        auxiliar = arreglo[i];
                        arreglo[i] = arreglo[i - 1];
                        arreglo[i - 1] = auxiliar;
                        cambio = true;
                    }
                }

                if (cambio == false) {
                    break;
                }
            }

            return arreglo;
        }

        function cambiarAttrModificado() {
            $('#tblZonificacion').find('tr').find('td').find('input[cargado="1"]').each(function () {
                $(this).change(function () {
                    if ($(this).val() != '' || $(this).val() == '') {
                        $(this).removeAttr('cargado');
                        $(this).attr('modificado', 1);
                        $(this).parent().prev().find('input[id="txtHidden"]').removeAttr('cargado');
                        $(this).parent().prev().find('input[id="txtHidden"]').attr('modificado', 1);
                        $(this).parent().prev().find('input[id="txtHidden"]').removeAttr('nuevo');
                        $(this).parent().prev().find('input[id="txtHidden"]').attr('accesible', 1);

                    }
                });

            });
        }

        function crearObjZonificacion() {
            try {
                return objZonificacion = $('#tblZonificacion').find('tr').find('td[class="invisible"]').find('input[accesible="1"]:not([cargado="1"])').map(function () {
                    return {
                        idAsociado: $(this).attr('idAsociado'),
                        nombreAsociado: $(this).attr('nombreAsociado'),
                        idInvernadero: $(this).attr('idInvernadero'),
                        invernadero: $(this).attr('invernadero'),
                        surcos: ($(this).parent().next().find('input[vaciosurco="0"]').val() == '' ? null : $(this).parent().next().find('input[vaciosurco="0"]').val()) || ($(this).parent().next().find('input[vaciosurco="1"]').val() == '' ? null : $(this).parent().next().find('input[vaciosurco="1"]').val())
                    }
                }).get();
            } catch (e) {
                console.log(e);
            }
        }


        function guardarConfiguracion() {
            PageMethods.guardarConfiguracion(crearObjZonificacion(), function (response) {
                if (response[0] == '1') {
                    popUpAlert(response[1], response[2]);
                    obtenerConfiguracion();
                } else {
                    popUpAlert(response[1], response[2]);
                }
            });
        }


        function obtenerConfiguracion() {
            bloqueoDePantalla.bloquearPantalla();
            try {
                PageMethods.obtenerMatrizConfiguracion(function (response) {
                    if (response[0] == '1') {
                        $('.divOculto').html(response[2]);
                        $('.divOculto').find('input').each(function () {
                            var idAsociado = $(this).attr('idAsociado');
                            var idInvernadero = $(this).attr('idInvernadero');
                            var surcos = $(this).attr('value');
                            var arreglo = surcos.split('-');
                            surcos = surcos.toString();

                            $('#tblZonificacion').find('tr').find('td[class="invisible"]').find('input[id="txtHidden"]').each(function () {
                                if (idAsociado == $(this).attr('idAsociado') && idInvernadero == $(this).attr('idInvernadero')) {
                                    $(this).parent().next().find('input[id="txtSurcos"]').val(surcos.toString());
                                    $(this).parent().next().find('input[id="txtSurcos"]').attr('vaciosurco', 0);
                                    $(this).parent().next().find('input[id="txtSurcos"]').removeAttr('nuevo');
                                    $(this).parent().next().find('input[id="txtSurcos"]').attr('cargado', 1);
                                    $(this).attr('cargado', 1);
                                }
                            });

                            cambiarAttrModificado();
                        });
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                    } else {
                        popUpAlert(response[1], response[2]);
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                    }
                });
            } catch (e) {
                console.log(e);
                bloqueoDePantalla.indicarTerminoDeTransaccion();
            }
            bloqueoDePantalla.desbloquearPantalla();
        }


        function generarMatriz() {
            bloqueoDePantalla.bloquearPantalla();
            try {
                PageMethods.generarMatriz(function (response) {
                    if (response[0] == '1') {
                        $('#tblZonificacion').html(response[2]);
                        $('#tblZonificacion').find('tr').find('td[class="surco"]').find('input[id="txtSurcos"]').each(function () {
                            $(this).change(function () {
                                if ($(this).val() != '') {
                                    $(this).attr('vacioSurco', 0);
                                    $(this).parent().prev().find('input[id="txtHidden"]').attr('accesible', 1);
                                    $(this).parent().prev().find('input[id="txtHidden"]').attr('activo', 1);
                                } else {
                                    $(this).attr('vacioSurco', 1);
                                    $(this).parent().prev().find('input[id="txtHidden"]').attr('accesible', 0);
                                    $(this).parent().prev().find('input[id="txtHidden"]').attr('activo', 0);
                                }
                            });
                        });
                        registerControls3();
                        quitarSorterColumnasInvernaderos();
                        generarEncabezadoFijoDinamico();
                        /*Quitamos los filtros de los invernaderos*/
                        $('input[type="search"]').eq(0).attr('filtroAsociados', '1');
                        $('input[type="search"]:not([filtroAsociados="1"])').css('display', 'none');
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                    } else {
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                        popUpAlert(response[1], response[2]);
                    }
                });
            } catch (e) {
                console.log(e);
                bloqueoDePantalla.indicarTerminoDeTransaccion();
            }
            bloqueoDePantalla.desbloquearPantalla();
            obtenerConfiguracion();
        }

        function tooltip() {
            if ($('.tooltipstered').length >= 1) {
                $('.tooltipstered').tooltipster('destroy');
            }
            $('.Error').tooltipster({
                animation: 'fade',
                contentAsHTML: true,
                interactive: false,
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: true,
                trigger: 'hover',
                position: 'right'
            });
        }
    </script>
    <style type="text/css">
        input.Error
        {
            border: 1px solid red;
            background: rgba(255,0,0,0.2);
        }
        .divOculto
        {
            display: none;
        }
        .divContainer
        {
            margin-left: 25px;
            margin-bottom: 4px;
            margin-top: 4px;
        }
        
        div#divAsociado
        {
            margin-bottom: 10px;
        }
        
        
        table#tblZonificacion input[type="text"]
        {
            width: 50px;
            margin-right: 5px;
        }
        
        table#tblZonificacion td[class="Asociado"]
        {
            text-align: left;
            padding-left: 10px;
        }
        
        div#divZonificacion
        {
            width: 100%;
            max-width: 1024px;
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
        }
        
        div#divtblAsociados
        {
            width: 25%;
        }
        
        div#divtblZonificacion
        {
            width: 70.85%;
            overflow-x: auto;
        }
        
        .gridView
        {
            min-width: 300px !important;
            margin-bottom: 0 !important;
        }
        
        input.invisible2
        {
            visibility: hidden;
            width: 0px;
        }
        
        input#btnGuardarConfiguracion
        {
            width: 175px;
            margin: 20px 0 0 0;
            margin-right: 110px;
        }
        
        span.spanInvisible
        {
            display: block;
            font-size: 10px;
        }
        
        span.numeroSurcos
        {
            display: block;
            font-size: 10px;
            color: #F60;
            text-transform: lowercase;
        }
        
        .container
        {
            min-width: 1024px;
            max-width: 1024px;
            display: inherit !important;
            max-height: 520px;
            overflow-y: auto;
        }
        table#tblAsociados
        {
            width: 100%;
        }
        
        table#tblZonificacion
        {
            width: 100%;
            max-width: 800px;
        }
        
        span#ctl00_ContentPlaceHolder1_lblSubtitulo
        {
            font-size: 15px;
        }
        
        span#ctl00_ContentPlaceHolder1_lblSubtitulo2
        {
            font-size: 15px;
        }
        
        span#ctl00_ContentPlaceHolder1_lblSubtitulo3
        {
            font-size: 15px;
        }
        
        /*table#tblZonificacion input[type="search"] {
    visibility: hidden;
}*/
        
        table#tblZonificacion input[type="text"]
        {
            text-align: center;
        }
        
        /*.index {
    height: 300px;
    overflow-y: auto;
    max-width: 800px;
    margin: auto;
    overflow-x: auto;
}*/
    </style>
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Zonificación"></asp:label></h1>
        <h2>
            <asp:label id="lblSubtitulo" runat="server" text="Por favor ingrese los rangos de surcos que cada asociado tendrá asignado mediante el formato {A-B} donde A representa el surco inicial y B el surco final.">
            </asp:label></h2>
        <h2>
            <asp:label id="lblSubtitulo2" runat="server" text="Por ejemplo: 1-5 (El asociado tendrá asignados los surcos 1,2,3,4 y 5).">
            </asp:label></h2>
        <br />
        <div class="index">
            <table id="tblZonificacion" class="gridView" cellspacing="0">
            </table>
        </div>
        <div id="divButton">
            <input type="button" id="btnGuardarConfiguracion" value="Guardar" />
        </div>
        <div class="divOculto">
        </div>
    </div>
</asp:content>
