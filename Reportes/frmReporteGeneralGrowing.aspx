<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmReporteGeneralGrowing.aspx.cs" Inherits="Reportes_frmReporteGeneralGrowing" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <%--<script type="text/javascript" src="../comun/scripts/highcharts.js"></script>--%>
    <script type="text/javascript" src="https://code.highcharts.com/highcharts.js"></script>
    <%--<script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />--%>
<style type="text/css">
    .calificacion
    {
        font-weight: bold;
    }
    
    
    .container table
    {
        width: 100%; /*background: #f6f6f6;*/
        border: 1px solid black;
        border-collapse: collapse;
        text-transform: uppercase;
        text-align: center;
        margin-bottom: 10px;
    }
    
    table.Semanas tr td
    {
        border: #FFF solid 1px;
    }
    
    .container table td, th
    {
        border: 1px solid black;
    }
    h3
    {
        display: block;
        background: #558ed5;
        color: black;
        text-align: center;
        border: 1px solid black;
        clear: left;
        padding: 2px 0;
        margin: 10px 0;
    }
    
    .ResumenPlantacion, .ResumenNoPlantacion, .ResumenGlobal
    {
        width: 100%;
        display: flex;
    }
    .ResumenPlantacion > div, .ResumenNoPlantacion > div
    {
        box-sizing: border-box;
        padding: 0 5px;
    }
    
    .ResumenPlantacion > div:first-child, .ResumenNoPlantacion > div:first-child
    {
        width: 50%;
    }
    .ResumenPlantacion div > img, .ResumenNoPlantacion div > img
    {
        max-width: 100%;
    }
    .ResumenPlantacion > div:nth-child(2), .ResumenNoPlantacion > div:nth-child(2)
    {
        border: 1px solid black;
        width: 50%;
    }
    
    
    .ResumenGlobal > div
    {
        box-sizing: border-box;
        padding: 0 5px;
    }
    .ResumenGlobal > div:first-child
    {
        width: 50%;
    }
    .ResumenGlobal div > img
    {
        max-width: 100%;
    }
    .ResumenGlobal > div:nth-child(2)
    {
        border: 1px solid black;
    }
    
    .header
    {
        background: #558ed5;
    }
    .green
    {
        background: #00b050;
    }
    .orange
    {
        background: #FFC000;
    }
    .redg
    {
        background: #FF0000;
    }
    .blue1
    {
        background: #4f81bd;
    }
    .blue2
    {
        background: #8eb4e3;
    }
    .blue3
    {
        background: #c6d9f1;
    }
    
    
    
    
    .PlantacionXActividad_plantacion
    {
        display: flex;
        justify-content: space-between;
        flex-wrap: wrap;
    }
    .PlantacionXActividad_plantacion > div
    {
        width: 49.8%;
        box-sizing: border-box;
        padding: 5px;
        border: 1px solid black;
        margin-bottom: 5px;
    }
    .PlantacionXActividad_No_Plantacion
    {
        display: flex;
        justify-content: space-between;
        flex-wrap: wrap;
    }
    .PlantacionXActividad_No_Plantacion > div
    {
        width: 49.8%;
        box-sizing: border-box;
        padding: 5px;
        border: 1px solid black;
        margin-bottom: 5px;
    }
    /*div#containerResumenGlobal, div#containerPlantacion, div#containerNoPlantacion
        {
            width: 50%;
            }*/
    table tr
    {
        height: 25px;
    }
    table#idProblemasPlantacion tr td
    {
        width: 50%;
    }
    table#idProblemasNoPlantacion tr td
    {
        width: 50%;
    }
    .Bloquear
    {
        display: initial;
    }
    .DesBloquear
    {
        display: none;
    }
    
    .headerRepGralGrowing
    {
        width: 1090px;
        margin: auto;
        font-weight: bold;
        padding: 10px 0;
    }
    
    .headerRepGralGrowing table, .headerRepGralGrowing table tr td
    {
        border: none !important;
    }
    
    /*.headerRepGralGrowing table
    {
        background: #d9d9d9;
    }*/
    
    .white
    {
        background: white;
        color: #000;
    }
    
    .none
    {
        display: none;
    }
    
    .right
    {
        text-align: right;
        padding-right: 10px;
    }
    
    
</style>

    <script type="text/javascript" id="Datos Iniciales">
        $(function () {
            $('#lblPlanta').text($('#ctl00_ddlPlanta option:selected').text());
            PageMethods.ObtenerSemanasNS(function (response) {

                $('#ddlSemana').html(response);
                //$('#ddlSemana').chosen();
            });

            //if ($('#ddlGerente').val() != null) {
            $('#lblPlanta').text($('#ctl00_ddlPlanta option:selected').text());


            PageMethods.ObtenerGrower(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), ($('#ddlGerente').val() == null ? 0 : $('#ddlGerente').val()), ($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {
                $('#ddlGrower').html(response);
                //$('#ddlLider').chosen();

            });
            PageMethods.ObtenerLideresPlanta(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), ($('#ddlGerente').val() == null ? 0 : $('#ddlGerente').val()), ($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {

                $('#ddlLider').html(response);
                //$('#ddlLider').chosen();

            });
            //}
            PageMethods.ObtenerGerentePlanta(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()),($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {

                $('#ddlGerente').html(response);
               // $('#ddlGerente').chosen();
            });
            $('#ctl00_ddlPlanta').live('change', function () {
                var tree = 0;
                $('#btn_ConsultarTabla').click();
                $('#lblPlanta').text($('#ctl00_ddlPlanta option:selected').text());
                PageMethods.ObtenerLideresPlanta(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), ($('#ddlGerente').val() == null ? 0 : $('#ddlGerente').val()), ($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {
                    $('#ddlLider').html(response);
                   //$('#ddlLider').chosen();
                });
                PageMethods.ObtenerGerentePlanta(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), ($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {
                    $('#ddlGerente').html(response);
                    //$('#ddlGerente').chosen();
                });
                PageMethods.ObtenerGrower(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), ($('#ddlGerente').val() == null ? 0 : $('#ddlGerente').val()), ($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {
                    $('#ddlGrower').html(response);
                    //$('#ddlLider').chosen();

                });
                ObtenerDatosIniciales();
            });

            ObtenerDatosIniciales();

            $('#ddlSemana').live('change', function () {
                var tree = 0;
                ObtenerDatosIniciales();
            });
            $('#ddlGrower').live('change', function () {
                ObtenerDatosIniciales();
            });
            $('#ddlLider').live('change', function () {
                var tree = 0;
                ObtenerDatosIniciales();
            });
            $('#ddlGerente').live('change', function () {
                var tree = 0;
                //if ($('#ddlGerente').val() != null) {
                PageMethods.ObtenerLideresPlanta(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), ($('#ddlGerente').val() == null ? 0 : $('#ddlGerente').val()), ($('#ddlSemana').val() == null ? 0 : $('#ddlSemana').val()), function (response) {
                    $('#ddlLider').html(response);
                    ObtenerDatosIniciales();
                    //$('#ddlLider').chosen();
                });
                //}
                //ObtenerDatosIniciales();
            });
        });
//        $(function () {

//            $('select').chosen();
//        });
        function ObtenerDatosIniciales() {
            $.blockUI();
            PageMethods.ObtenerDatosIniciales(($('#ctl00_ddlPlanta').val() == null ? 0 : parseInt($('#ctl00_ddlPlanta').val())), $('#ddlSemana').val(), ($('#ddlGrower').val() == null ? 0 : parseInt($('#ddlGrower').val())), ($('#ddlLider').val() == null ? 0 : parseInt($('#ddlLider').val())), ($('#ddlGerente').val() == null ? 0 : parseInt($('#ddlGerente').val())), function (response) {
                $.unblockUI();

                $('#idCalifTotalPlantacion').text('0%');
                $('#idCalifTotalNoPlantacion').text('0%');
                $('#idCalifTotal').text('0%');
                if (response[19] == 'false') {
                    $('.ResumenPlantacion').removeClass('DesBloquear');
                    $('.ResumenPlantacion').removeClass('Bloquear');
                    //$('.ResumenPlantacion').addClass('Bloquear')
                    $('#idResumenPlantacion tbody').html(response[1]);
                    $('#idResumenPlantacion tbody tr').find('td:eq(1)').each(function () {
                        semaforizacion($(this));
                        $(this).next().addClass($(this).attr('class'));
                    });
                    semaforizacion($('#lblSumaTotalCalificacionPlantacion').parent());
                    
                    

                    var RR = response[4].toString().substring(0, response[4].toString().length - 1).split(',');
                    var RR2 = response[5].toString().substring(0, response[5].toString().length - 1).split('|');
                    var RR21 = RR2[0].toString().substring(0, RR2[0].toString().length - 1).split(',');
                    var myArray = RR21;
                    for (var i = 0; i < myArray.length; i++) { myArray[i] = parseFloat(myArray[i], 10); }
                    var RR22 = RR2[1].toString().substring(0, RR2[1].toString().length - 1).split(',');
                    var myArray2 = RR22;
                    for (var i = 0; i < myArray.length; i++) { myArray2[i] = parseFloat(myArray2[i], 10); }
                    var RR23 = RR2[2].toString().substring(0, RR2[2].toString().length - 1).split(',');
                    var myArray3 = RR23;
                    for (var i = 0; i < myArray.length; i++) { myArray3[i] = parseFloat(myArray3[i], 10); }
                    //Armar el objeto para agregar la calificación Total de la plantación
                    var calif_grafica = [];
                    for (var i = 0; i <= myArray.length; i++) {
                        calif_grafica[i] = 0;
                        //myArray3[i] = parseFloat(myArray3[i], 10); 
                    }
                    calif_grafica.push(parseFloat($('#lblSumaTotalCalificacionPlantacion').text()));
                    generarGrafica('containerPlantacion', 'Resumen Plantación', RR, myArray, myArray2, myArray3, calif_grafica, parseFloat($('#lblSumaTotalCalificacionPlantacion').text()));
                    $('#idProblemasPlantacion tbody').html(response[14]);
                }
                else {
                    $('.ResumenPlantacion').removeClass('DesBloquear');
                    $('.ResumenPlantacion').removeClass('Bloquear');
                    $('.ResumenPlantacion').addClass('DesBloquear');
                    popUpAlert('No existe configuracion para esta planta', 'error');
                }

                if (response[20] == 'false') {
                    $('#idResumenNoPlantacion tbody').html(response[6]);
                    $('.ResumenNoPlantacion').removeClass('DesBloquear');
                    $('.ResumenNoPlantacion').removeClass('Bloquear');
                    $('#idResumenNoPlantacion tbody tr').find('td:eq(1)').each(function () {
                        semaforizacion($(this));
                        $(this).next().addClass($(this).attr('class'));

                    });
                    semaforizacion($('#lblSumaTotalCalificacionNoPlantacion').parent());
                    var RR = response[9].toString().substring(0, response[9].toString().length - 1).split(',');
                    var RR2 = response[10].toString().substring(0, response[10].toString().length - 1).split('|');
                    var RR21 = RR2[0].toString().substring(0, RR2[0].toString().length - 1).split(',');
                    var myArray = RR21;
                    for (var i = 0; i < myArray.length; i++) { myArray[i] = parseFloat(myArray[i], 10); }

                    var RR22 = RR2[1].toString().substring(0, RR2[1].toString().length - 1).split(',');
                    var myArray2 = RR22;
                    for (var i = 0; i < myArray.length; i++) { myArray2[i] = parseFloat(myArray2[i], 10); }

                    var RR23 = RR2[2].toString().substring(0, RR2[2].toString().length - 1).split(',');
                    var myArray3 = RR23;
                    for (var i = 0; i < myArray.length; i++) { myArray3[i] = parseFloat(myArray3[i], 10); }
                    var calif_grafica2 = [];
                    for (var i = 0; i <= myArray.length; i++) {
                        calif_grafica2[i] = null;
                    }
                    calif_grafica2.push(parseFloat($('#lblSumaTotalCalificacionNoPlantacion').text()));
                    generarGrafica('containerNoPlantacion', 'Resumen No Plantación', RR, myArray, myArray2, myArray3, calif_grafica2, parseFloat($('#lblSumaTotalCalificacionNoPlantacion').text()));
                }
                else {
                    $('.ResumenNoPlantacion').removeClass('DesBloquear');
                    $('.ResumenNoPlantacion').removeClass('Bloquear');
                    $('.ResumenNoPlantacion').addClass('DesBloquear');
                    popUpAlert('No existe configuracion para esta planta', 'error');
                }
                $('#idProblemasNoPlantacion tbody').html(response[15]);
                //


                //var Calif_Total_Plantacion = parseFloat($('#lblSumaTotalCalificacionPlantacion').text().substring(0, $('#lblSumaTotalCalificacionPlantacion').text().length - 1), 10);
                $('#idCalifTotalPlantacion').text(parseFloat($('#lblSumaTotalCalificacionPlantacion').text().substring(0, $('#lblSumaTotalCalificacionPlantacion').text().length - 1), 10) + '%');
                //var Calif_Total_NoPlantacion = parseFloat($('#lblSumaTotalCalificacionNoPlantacion').text().substring(0, $('#lblSumaTotalCalificacionNoPlantacion').text().length - 1), 10);
                $('#idCalifTotalNoPlantacion').text(parseFloat($('#lblSumaTotalCalificacionNoPlantacion').text().substring(0, $('#lblSumaTotalCalificacionNoPlantacion').text().length - 1), 10) + '%');

                $('#idCalifTotal').text(response[23].toString() + '%');
//                $('#idCalifTotal').text((parseFloat($('#idCalifTotalPlantacion').text()) + parseFloat($('#idCalifTotalNoPlantacion').text())) / 2 + '%');
//                if ((response[21] == 'true') && (response[22] == 'false')) {
//                    $('#idCalifTotal').text((parseFloat($('#idCalifTotalPlantacion').text())) + '%');
//                }
//                if ((response[21] == 'false') && (response[22] == 'true')) {
//                    $('#idCalifTotal').text((parseFloat($('#idCalifTotalNoPlantacion').text())) + '%');
//                }
                

                semaforizacion($('#idCalifTotalPlantacion').parent());
                semaforizacion($('#idCalifTotalNoPlantacion').parent());
                semaforizacion($('#idCalifTotal').parent());


                generarGraficaGeneral('containerResumenGlobal', 'Resumen Global', RR, myArray, myArray2, myArray3);
                //
                //se crea la informacion para las gráficas por actividad de la plantacion
                if (response[19] == 'false') {
                    var nuevo_tipo = response[11].toString().substr(1);
                    var PlantacionXActividadPlantGeneralAux = nuevo_tipo.substring(0, nuevo_tipo.length - 1).split('|');
                    var nuevo_tipoNombre = response[12].toString().substr(1);
                    var PlantacionXActividadGeneralPlantNombreAux = nuevo_tipoNombre.substring(0, nuevo_tipo.length - 1).split('|');
                    var nuevo_tipoPorcentaje = response[13].toString().substr(1);
                    var PlantacionXActividadGeneralPlantPorcentajeAux = nuevo_tipoPorcentaje.substring(0, nuevo_tipo.length - 1).split('|');
                    $('.PlantacionXActividad_plantacion').html('');
                    for (var i = 0; i < PlantacionXActividadPlantGeneralAux.length; i++) {
                        var plantacion = PlantacionXActividadPlantGeneralAux[i].substring(0, PlantacionXActividadPlantGeneralAux[i].length - 1).split('&');
                        $('.PlantacionXActividad_plantacion').append('<div id=' + plantacion[0].replace(/ /gi, '_') + '></div>');
                        var par = PlantacionXActividadGeneralPlantNombreAux[i].substring(0, PlantacionXActividadGeneralPlantNombreAux[i].toString().length - 1).split(',');
                        var par2 = PlantacionXActividadGeneralPlantPorcentajeAux[i].substring(0, PlantacionXActividadGeneralPlantPorcentajeAux[i].toString().length - 1).split(',');
                        //generarGrafica(plantacion[0].replace(/ /gi, '_'), plantacion[0], RR, myArray, myArray2, myArray3);
                        for (var xx = 0; xx < par2.length; xx++) { par2[xx] = parseFloat(par2[xx], 10); }
                        //generarGraficaXActividad(plantacion[0].replace(/ /gi, '_'), plantacion[0] + i + 'Pl', par, par2);

                        generarGraficaXActividad(plantacion[0].replace(/ /gi, '_'), plantacion[0], par, par2);
                    }
                }
                //se crea la informacion para las gráficas por actividad de la No plantacion
                if (response[20] == 'false') {
                    var nuevo_tipo2 = response[16].toString().substr(1);
                    var PlantacionXActividadNoPlantGeneralAux2 = nuevo_tipo2.substring(0, nuevo_tipo2.length - 1).split('|');
                    var nuevo_tipoNombre2 = response[17].toString().substr(1);
                    var PlantacionXActividadGeneralNoPlantNombreAux2 = nuevo_tipoNombre2.substring(0, nuevo_tipo2.length - 1).split('|');
                    var nuevo_tipoPorcentaje2 = response[18].toString().substr(1);
                    var PlantacionXActividadGeneralNoPlantPorcentajeAux2 = nuevo_tipoPorcentaje2.substring(0, nuevo_tipo2.length - 1).split('|');
                    $('.PlantacionXActividad_No_Plantacion').html('');
                    for (var i = 0; i < PlantacionXActividadNoPlantGeneralAux2.length; i++) {
                        var plantacion2 = PlantacionXActividadNoPlantGeneralAux2[i].substring(0, PlantacionXActividadNoPlantGeneralAux2[i].length - 1).split('&');
                        $('.PlantacionXActividad_No_Plantacion').append('<div id=' + plantacion2[0].replace(/ /gi, '_') + '></div>');
                        var par2np = PlantacionXActividadGeneralNoPlantNombreAux2[i].substring(0, PlantacionXActividadGeneralNoPlantNombreAux2[i].toString().length - 1).split(',');
                        var par22np = PlantacionXActividadGeneralNoPlantPorcentajeAux2[i].substring(0, PlantacionXActividadGeneralNoPlantPorcentajeAux2[i].toString().length - 1).split(',');
                        //generarGrafica(plantacion[0].replace(/ /gi, '_'), plantacion[0], RR, myArray, myArray2, myArray3);
                        for (var xx = 0; xx < par22np.length; xx++) { par22np[xx] = parseFloat(par22np[xx], 10); }
                        //generarGraficaXActividad(plantacion[0].replace(/ /gi, '_'), plantacion[0] + i + 'Pl', par, par2);
                        generarGraficaXActividad(plantacion2[0].replace(/ /gi, '_'), plantacion2[0], par2np, par22np);
                    }
                }

            });
        }
        function semaforizacion(control) {
           control.removeClass();
           if (parseInt(control.text()) < 69) {
               control.addClass('redg');
            }
           if ((parseInt(control.text()) >= 70) && (parseInt(control.text()) < 90)) {
               control.addClass('orange');
            }
           if (parseInt(control.text()) >= 90) {
               control.addClass('green');
            }

        }
        //Método que generará la gráfica con los parámetros que se le envían nameg='Nombre del Control' Html; Descrip_Grafica='Descripción de la gráfica-->Encabezado'; RR='Categorías';
        function generarGrafica(nameg, Descrip_Grafica, RR, myArray, myArray2, myArray3, calif_grafica, lblSumaTotalCalificacionPlantacion) {
            myArray2.push(null);
            //myArray2.push(null);
            myArray2.push(lblSumaTotalCalificacionPlantacion);
            myArray3.push(null);
            myArray3.push(null);
            RR.push('');
            RR.push('Calificación GH');
            //calif_grafica.push(parseFloat($('#lblSumaTotalCalificacionPlantacion').text()));
            
            Highcharts.theme = {
                colors: ['#7798BF', '#FF0000', '#f45b5b', '#7798BF', '#aaeeee', '#ff0066', '#eeaaee',
                        '#55BF3B', '#DF5353', '#7798BF', '#aaeeee']
            }
            Highcharts.setOptions(Highcharts.theme);
            Highcharts.chart(nameg, {
                chart: {
                    type: 'column',
                    events: {
                        load: function (chart) {
                            window.setTimeout(function () {
                                chart.target.reflow();
                            }, 100);
                        }
                    }
                },
                title: {
                    text: Descrip_Grafica
                },
                subtitle: {
                //text: 'Source: WorldClimate.com'
            },
            xAxis: {
                categories: RR,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Porcentaje(%)'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [
            //                {
            //                    name: 'Cumplimiento',
            //                    data: myArray

            //                }, 
                {
                name: 'Calificación',
                data: myArray2

            }, {
                name: 'Distribución',
                data: myArray3

            }
            //, { name: ' '}
            //, { name: 'Calificación GH',data: calif_grafica }
            ]
        });

        $('text.highcharts-credits').remove();

        }
        function generarGraficaXActividad(nameg, Descrip_Grafica, RR, myArray) {
            Highcharts.theme = {
                colors: ['#538DD6', '#FFC000', '#f45b5b', '#7798BF', '#aaeeee', '#ff0066', '#eeaaee',
                        '#55BF3B', '#DF5353', '#7798BF', '#aaeeee']
            }
            Highcharts.setOptions(Highcharts.theme);
            Highcharts.chart(nameg, {
                chart: {
                    type: 'column'
                },
                title: {
                    text: Descrip_Grafica
                },
                subtitle: {
                //text: 'Source: WorldClimate.com'
            },
            xAxis: {
                categories: RR,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Porcentaje(%)'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: 'Porcentaje',
                data: myArray

            }]
        });
        $('text.highcharts-credits').remove();
    }
    function generarGraficaGeneral(nameg, Descrip_Grafica, RR, myArray) {
        Highcharts.theme = {
            colors: ['#538DD6', '#FFC000', '#f45b5b', '#7798BF', '#aaeeee', '#ff0066', '#eeaaee',
                        '#55BF3B', '#DF5353', '#7798BF', '#aaeeee']
        }
        Highcharts.setOptions(Highcharts.theme);
        Highcharts.chart(nameg, {
            chart: {
                type: 'column'
            },
            title: {
                text: Descrip_Grafica
            },
            subtitle: {
            //text: 'Source: WorldClimate.com'
        },
        xAxis: {
            categories: ['calificación Plantacion', 'calificación No Plantacion', '', 'Calificación General'],
            crosshair: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Porcentaje(%)'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'Porcentaje',
            data: [parseFloat($('#idCalifTotalPlantacion').text(),8), parseFloat($('#idCalifTotalNoPlantacion').text()), 0, parseFloat($('#idCalifTotal').text())]

        }]
    });
    $('text.highcharts-credits').remove();
}
    </script>
     
    <%--<script type="text/javascript" id="Script1">
    
        Highcharts.chart('Div2', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Monthly Average Rainfall'
            },
            subtitle: {
                text: 'Source: WorldClimate.com'
            },
            xAxis: {
                categories: pais,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Rainfall (mm)'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: 'Cumplimiento',
                data: [.3852, .75, .6667, 1]//, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]//., 54.4]

            }, {
                name: 'Calificación',
                data: [.17334, .15, .13334, .15]//, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6]//, 92.3]

            }, {
                name: 'Distribución',
                data: [1, 1, 1, 0]//, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3]//, 51.2]

            }]
        });
    });--%>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1><asp:Label runat="server" Text="<%$ Resources:ReporteGral%>"></asp:Label></h1>
        <div>
            <table class="Semanas">
                <%--<tr><td><label>Semana</label></td>
                    <td>
                        <select id="ddlSemana"></select> 
                    </td>
                </tr>--%>
                <tr>
                    <td><asp:Label ID="TxtSemana" runat="server" Text="<%$ Resources:Semana%>"></asp:Label></td>
                    <td>
                        <select id="ddlSemana"></select> 
                    </td>
                    <td><asp:Label ID="TxtGrower" runat="server" Text="<%$ Resources:Grower%>"></asp:Label></td>
                    <td>
                         <select id="ddlGrower"></select> 
                    </td>
                    <td><asp:Label ID="TxtGerente" runat="server" Text="<%$ Resources:GerenteZona%>"></asp:Label></td>
                    <td>
                        <select id="ddlGerente"></select>
                    </td>
                    <td><asp:Label ID="TxtLider" runat="server" Text="<%$ Resources:Lider%>"></asp:Label></td>
                    <td>
                        <select id="ddlLider"></select>
                    </td>
                </tr>
            </table>
        </div>
            <%--<div class="headerRepGralGrowing">
        <table>
            <tr>
                <td colspan="4">&nbsp;</td>
                <td class="white right">GROWER:</td>
                <td colspan="2"><select id="ddlGrower"></select></td>
                <td>&nbsp;</td>
                <td>SEMANA:</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="right">PLANTA:</td>
                <td>SAN ISIDRO</td>
                <td>&nbsp;</td>
                <td class="white right">GTE. ZONA</td>
                <td colspan="2"><select id="ddlGerente"></select></td>
                <td>&nbsp;</td>
                <td class="blue1">2017-14</td>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">&nbsp;</td>
                <td class="white right">LIDER:</td>
                <td colspan="2"><select id="ddlLider"></select></td>
                <td>&nbsp;</td>
                <td colspan="3">&nbsp;</td>
            </tr>
        </table>
    </div>--%>
        <h3><asp:Label ID="Label5" runat="server" Text="<%$ Resources:ResumenPlant%>"></asp:Label></h3>
        <div class="ResumenPlantacion">
            <div>
                <table id="idResumenPlantacion">
                    <thead class="header">
                        <tr>
                            <th><asp:Label ID="Label1" runat="server" Text="<%$ Resources:Grupo%>"></asp:Label></th>
                            <th><asp:Label ID="Label2" runat="server" Text="<%$ Resources:Cumplimiento%>"></asp:Label></th>
                            <th><asp:Label ID="Label3" runat="server" Text="<%$ Resources:Calificacion%>"></asp:Label></th>
                            <th><asp:Label ID="Label4" runat="server" Text="<%$ Resources:Distribucion%>"></asp:Label></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%--<tr><td>PLANTACIÓN</td><td class="green">87%</td><td class="green">39%</td><td class="green">43%</td></tr>
                        <tr><td>PLANTACIÓN</td><td class="orange">87%</td><td class="orange">39%</td><td class="orange">43%</td></tr>
                        <tr><td>PLANTACIÓN</td><td class="green">87%</td><td class="green">39%</td><td class="green">43%</td></tr>
                        <tr><td>PLANTACIÓN</td><td class="orange">87%</td><td class="orange">39%</td><td class="orange">43%</td></tr>
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr class="calificacion"><td>CALIFICACION GH NO PLANTACION</td><td class="green"><label id="lblCalifTotalCumplimiento" for='male'>87%</label></td><td class="green"><label id="Label1" for='male'>39%</label></td><td>&nbsp;</td></tr>--%>
                    </tbody>
                </table>
                <table id="idProblemasPlantacion">
                    <tbody>
                       
                    </tbody>
                </table>
            </div>
             <div id="containerPlantacion">
           
            </div>
        </div>
        <h3><asp:Label ID="Label6" runat="server" Text="<%$ Resources:ResumenNoPlant%>"></asp:Label></h3>
        <div class="ResumenNoPlantacion">
            <div>
                <table id="idResumenNoPlantacion">
                    <thead class="header">
                        <tr>
                            <th><asp:Label ID="Label7" runat="server" Text="<%$ Resources:Grupo%>"></asp:Label></th>
                            <th><asp:Label ID="Label8" runat="server" Text="<%$ Resources:Cumplimiento%>"></asp:Label></th>
                            <th><asp:Label ID="Label9" runat="server" Text="<%$ Resources:Calificacion%>"></asp:Label></th>
                            <th><asp:Label ID="Label10" runat="server" Text="<%$ Resources:Distribucion%>"></asp:Label></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%--<tr><td>PLANTACIÓN</td><td class="green">87%</td><td class="green">39%</td><td class="green">43%</td></tr>
                        <tr><td>PLANTACIÓN</td><td class="orange">87%</td><td class="orange">39%</td><td class="orange">43%</td></tr>
                        <tr><td>PLANTACIÓN</td><td class="green">87%</td><td class="green">39%</td><td class="green">43%</td></tr>
                        <tr><td>PLANTACIÓN</td><td class="orange">87%</td><td class="orange">39%</td><td class="orange">43%</td></tr>
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr class="calificacion"><td>CALIFICACION GH NO PLANTACION</td><td class="green">87%</td><td class="green">39%</td><td>&nbsp;</td></tr>--%>
                    </tbody>
                </table>
                <table id="idProblemasNoPlantacion">
                    <tbody>
                        <%--tr><td colspan="2" class="blue1">PROBLEMA1</td><td colspan="2"></td></tr>
                        <tr><td colspan="2" class="blue2">PROBLEMA2</td><td colspan="2"></td></tr>
                        <tr><td colspan="2" class="blue3">PROBLEMA 3</td><td colspan="2"></td></tr>--%>
                    </tbody>
                </table>
            </div>
            <div id="containerNoPlantacion">
            <%--<div class="grafica">--%>
                <%--<img src="grafica.jpg" alt="">--%>
            </div>
        </div>
        <h3><asp:Label ID="Label11" runat="server" Text="<%$ Resources:GlobalRes%>"></asp:Label></h3>
        <div class="ResumenGlobal"><%--ResumenPlantacion ResumenGlobal--%>
            <div>
                <table id="idResumenGlobalTable">
                    <thead class="header">
                        <tr><th><asp:Label ID="Label12" runat="server" Text="<%$ Resources:Categoria%>"></asp:Label>
                        </th><th><asp:Label ID="Label13" runat="server" Text="<%$ Resources:Calificacion%>"></asp:Label></th></tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><asp:Label ID="Label17" runat="server" Text="<%$ Resources:CalifGH%>"></asp:Label></td>
                            <td><label id="idCalifTotalPlantacion" for='male'></label></td>
                        </tr>
                        <tr>
                            <td><asp:Label ID="Label18" runat="server" Text="<%$ Resources:CalifGHNo%>"></asp:Label></td>
                            <td><label id="idCalifTotalNoPlantacion" for='male'></label></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="calificacion">
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:CalifGeneral%>"></asp:Label>
                            </td>
                            <td class="green"><label id="idCalifTotal" for='male'></label></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="containerResumenGlobal" style="height: 200px; margin: 0; width: 50%;">
            
            <%--<div class="grafica">
                <img src="grafica.jpg" alt="">--%></div>
            </div>
        
        <h3><th><asp:Label ID="Label14" runat="server" Text="<%$ Resources:PorcentDist%>"></asp:Label></h3>
        <div class="PlantacionXActividad_plantacion">
        </div>
        <h3><asp:Label ID="Label15" runat="server" Text="<%$ Resources:PorcentDistNo%>"></asp:Label></h3>
        <div class="PlantacionXActividad_No_Plantacion">
        </div>
        <%--<div id="Div1" style="min-width: 310px; height: 400px; margin: 0 auto"></div>--%>
        <%--<div id="Div2" style="min-width: 310px; height: 400px; margin: 0 auto"></div>--%>
    </div>
</asp:Content>

