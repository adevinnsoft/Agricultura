<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmReportePorZona.aspx.cs" Inherits="Reportes_frmReportePorZona" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/highcharts.js"></script>
    <style type="text/css">
        .container
        {
            width: 90%; /*margin: auto;*/
            box-sizing: border-box; /*padding: 0 10px;*/
        }
        
        .container table
        {
            width: 100%;
            border: 1px solid black;
            border-collapse: collapse;
            text-align: center;
        }
        
        .container table tr td
        {
            border: 1px solid black;
        }
        
        .container h3
        {
            display: block; /*background: #1f497d;*/
            text-align: center;
            color: white;
            border: 1px solid black;
            margin: 5px 2px;
            text-transform: uppercase;
            padding: 3px 0;
        }
        
        .ResumenGrupo, .ResumenInvernadero
        {
            width: 50%;
        }
        
        .flex
        {
            display: -webkit-flex;
            display: -moz-flex;
            display: -ms-flex;
            display: -o-flex;
            display: flex;
            min-height: 300px;
            flex-wrap: wrap;
        }
        
        .ResumenGrupo .flex > div
        {
            box-sizing: border-box;
            padding: 0 5px;
        }
        
        .ResumenGrupo .flex div:first-child
        {
            width: 50%;
        }
        
        .ResumenGrupo .flex div:nth-child(2)
        {
            width: 50%;
            padding: 0 !important;
        }
        
        .container img
        {
            max-width: 100%;
        }
        
        .problemas
        {
            margin-top: 10px;
        }
        
        .flex table.Plantacion thead
        {
            background: #1f497d;
            color: white;
            text-transform: uppercase;
        }
        
        .flex table.NoPlantacion thead
        {
            background: #4f81bd;
            color: white;
            text-transform: uppercase;
        }
        
        .problemas td
        {
            width: 50%;
        }
        
        .red
        {
            background: red;
        }
        
        .calificacion
        {
            font-weight: bold;
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
        
        .h3blue1
        {
            background: #1f497d;
        }
        
        .h3blue2
        {
            background: #4f81bd;
            }
        .ResumenGrupo, .ResumenInvernadero
        {
            float: left;
        }
        .green
        {
            background: #00b050;
        }
        .orange
        {
            background: #ffc000;
        }
        .CalificacionPorcentaje
        {
            font-weight: bold;
            width: 100%;
            display: flex;
            justify-content: space-between;
            padding: 0 5px;
            box-sizing: border-box;
            align-items: center;
        }
        table.Porcentaje
        {
            width: 200px;
            font-size: 20px;
        }
        table.Planta
        {
            width: 50%;
            border: none !important;
            text-align: right;
            font-size: 12px;
        }
        
        table.Planta tr td
        {
            border: none;
        }
        
        table.Planta tr td select
        {
            float: left;
            margin-left: 5px;
        }
        table tr
        {
            height: 25px;
        }
        
        table
        {
            font-size: 9px;
        }
        .ResumenInvernadero .ResInvContainer
        {
            display: flex;
            padding: 0;
            margin-bottom: 5px;
        }
        .ResumenInvernadero .ResInvContainer div
        {
            min-height: 160px;
        }
        
        .ResumenInvernadero .ResInvContainer div:first-child
        {
            display: flex;
            align-items: center;
            width: 15%;
        }
        
        .ResumenInvernadero .ResInvContainer div:nth-child(3), .ResumenInvernadero .ResInvContainer div:nth-child(4)
        {
            width: 25%;
        }
        .GH
        {
            height: 80px;
            font-weight: bold;
            text-transform: uppercase;
            font-size: 20px;
        }
        .divChart
        {
            width: 35%;
            min-height: 160px;
        }
        .GHVisitado
        {
            background: #1f497d;
            font-size: 12px;
            font-weight: bold;
            height: 40px;
            color: White;
        }
        .GHPorcentaje
        {
            font-size: 12px;
            font-weight: bold;
            height: 40px;
            color: White;
        }
        .GHProblemas
        {
            background: #EEECE1;
            font-size: 12px;
            font-weight: bold;
            height: 40px;
        }
        .tdProblema
        {
            /*height: 40px;*/
            font-weight: bold;
            width: 50%;
        }
        .Comentarios
        {
            height: 125px;
        }
        table[ID^="tblGHProblemasPlantacion"]
        {
            border-right: none;
        }
    </style>

    <script type="text/javascript" id="Generar Reporte">
        $(function () {
            //Cargamos la informacion inicial
            CargarInformacionInicial();

            //*****Mandamos llamar la funcion que genera el reporte cuando la semana, lider o planta cambien
            //Cargamos los combos cada que cambia de planta a la planta
            $('#ctl00_ddlPlanta').change(function () {
                CargarInformacionInicial();
                $('.ResInvContainer').remove();
                LimpiaPlantacion();
                LimpiaNOPlantacion();
                //ObtenerReporteXZona();
            });

            $('#ddlSemana').change(function () {
                //Funcion para filtrar los lideres de acuerdo a la semana de sus capturas
                var semana = $(this).val();
                $('#ddlLider option').addClass('invisible');
                $('#ddlLider option[semana=""]').removeClass('invisible').prop('selected', true);
                $('#ddlLider option[semana=' + semana + ']').removeClass('invisible');
                //Limpiamos
                $('.ResInvContainer').remove();
                LimpiaPlantacion();
                LimpiaNOPlantacion();
                //Mandamos llamar el reporte
                ObtenerReporteXZona();
            });

            $('#ddlLider').change(function () {
                ObtenerReporteXZona();
            });
            //****Fin Llamadas para generar reporte
        });

        function CargarInformacionInicial() {
            PageMethods.ObtieneCombos(function (response) {
                if (response[0] == '1') {
                    var splitPlanta = $.trim($('.Plant').text()).split(': ');
                    var Planta = $('#ctl00_ddlPlanta option:selected').text() != '' ? $('#ctl00_ddlPlanta option:selected').text() : splitPlanta[1];
                    $('#lblPlanta').html(Planta);
                    $('#ddlSemana').html(response[2]);
                    $('#ddlLider').html(response[3]);
                    $('#ddlLider option').addClass('invisible');
                    $('#ddlLider option[semana=""]').removeClass('invisible');
                }
            });
        }

        function ObtenerReporteXZona() {
            var semana = $('#ddlSemana option:selected').val();
            var idLider = $('#ddlLider option:selected').val();

            if (semana != "" && idLider != "") {
                PageMethods.GenerarReportePlantacion(semana, idLider, function (response) {
                    //Limpiamos la sección de resumen por invernadero
                    $('.ResInvContainer').remove();

                    if (response[0] == '1') {
                        //----*----*----*----*----*-- OBTENEMOS EL REPORTE DE LA SECCIÓN (GH PLANTACION) ----*----*----*----*----*--
                        if (response[2] == "" && response[5] == "") {
                            LimpiaPlantacion()
                        } else {
                            if (response[2] != "") {
                                $('#tblResumenXGrupoPlantacion tbody').html(response[2]);
                                $('#tblResumenXGrupoPlantacion tbody').append(response[3]);
                                $('#tblProblemasXGrupoPlantacion tbody').html(response[4]);

                                //Genera la Gráfica de Resumen X Grupo (GH Plantación)
                                var Grupos = [];
                                var x = 0;
                                $('#tblResumenXGrupoPlantacion tbody tr td:not(.info)').each(function () {
                                    Grupos[x] = $(this).text();
                                    x++;
                                });

                                var CalificacionY = [];
                                x = 0;
                                $('#tblResumenXGrupoPlantacion tbody tr td.CalPlantacionGrupo').each(function () {
                                    CalificacionY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });
                                CalificacionY[x] = $('#tblResumenXGrupoPlantacion tbody tr td.CalPlantacionGrupoTotal').text() != '' ? parseFloat($('#tblResumenXGrupoPlantacion tbody tr td.CalPlantacionGrupoTotal').text()) : 0;

                                var DistribucionY = [];
                                x = 0;
                                $('#tblResumenXGrupoPlantacion tbody tr td.DistPlantacionGrupo').each(function () {
                                    DistribucionY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });

                                Graficar('GraficaXGrupoPlantancion', Grupos, CalificacionY, DistribucionY);
                            }

                            //Llenamos la sección de resumen por invernadero
                            $('#ResumenInvernaderoPlantacion').append(response[5]);

                            //Genera la Gráfica de Resumen X Invernadero (GH Plantación)
                            $('#ResumenInvernaderoPlantacion .ResInvContainer').each(function () {
                                var GruposGH = [];
                                var CalificacionGHY = [];
                                var DistribucionGHY = [];
                                var x = 0;
                                var inverdadero = $(this).find('.GH').text();

                                $('#tblGrafica' + inverdadero + ' tr td.GrupoGH').each(function () {
                                    GruposGH[x] = $(this).text();
                                    x++;
                                });

                                x = 0;
                                $('#tblGrafica' + inverdadero + ' tr td.CalificacionGH').each(function () {
                                    CalificacionGHY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });

                                x = 0;
                                $('#tblGrafica' + inverdadero + ' tr td.DistribucionGH').each(function () {
                                    DistribucionGHY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });

                                Graficar('GHPlantacionGrafica' + inverdadero, GruposGH, CalificacionGHY, DistribucionGHY);
                            });
                        }

                        //----*----*----*----*----*-- OBTENEMOS EL REPORTE DE LA SECCIÓN (GH NO PLANTACION) ----*----*----*----*----*--
                        if (response[6] == "" && response[9] == "") {
                            LimpiaNOPlantacion();
                        } else {
                            if (response[6] != "") {
                                $('#tblResumenXGrupoNOPlantacion tbody').html(response[6]);
                                $('#tblResumenXGrupoNOPlantacion tbody').append(response[7]);
                                $('#tblProblemasXGrupoNOPlantacion tbody').html(response[8]);

                                //Genera la Gráfica de Resumen X Grupo (GH NO Plantación)
                                var Grupos = [];
                                var x = 0;
                                $('#tblResumenXGrupoNOPlantacion tbody tr td:not(.info)').each(function () {
                                    Grupos[x] = $(this).text();
                                    x++;
                                });

                                var CalificacionY = [];
                                x = 0;
                                $('#tblResumenXGrupoNOPlantacion tbody tr td.CalPlantacionGrupo').each(function () {
                                    CalificacionY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });
                                CalificacionY[x] = $('#tblResumenXGrupoNOPlantacion tbody tr td.CalPlantacionGrupoTotal').text() != '' ? parseFloat($('#tblResumenXGrupoNOPlantacion tbody tr td.CalPlantacionGrupoTotal').text()) : 0;

                                var DistribucionY = [];
                                x = 0;
                                $('#tblResumenXGrupoNOPlantacion tbody tr td.DistPlantacionGrupo').each(function () {
                                    DistribucionY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });

                                Graficar('GraficaXGrupoNOPlantancion', Grupos, CalificacionY, DistribucionY);
                            }

                            //Llenamos la sección de resumen por invernadero
                            $('#ResumenInvernaderoNOPlantacion').append(response[9]);

                            //Genera la Gráfica de Resumen X Invernadero (GH Plantación)
                            $('#ResumenInvernaderoNOPlantacion .ResInvContainer').each(function () {
                                var GruposGH = [];
                                var CalificacionGHY = [];
                                var DistribucionGHY = [];
                                var x = 0;
                                var inverdadero = $(this).find('.GH').text();

                                $('#tblGrafica' + inverdadero + ' tr td.GrupoGH').each(function () {
                                    GruposGH[x] = $(this).text();
                                    x++;
                                });

                                x = 0;
                                $('#tblGrafica' + inverdadero + ' tr td.CalificacionGH').each(function () {
                                    CalificacionGHY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });

                                x = 0;
                                $('#tblGrafica' + inverdadero + ' tr td.DistribucionGH').each(function () {
                                    DistribucionGHY[x] = parseFloat($(this).text() != '' ? $(this).text() : 0);
                                    x++;
                                });

                                Graficar('GHNOPlantacionGrafica' + inverdadero, GruposGH, CalificacionGHY, DistribucionGHY);
                            });
                        }

                        //----*----*----*----*----*-- OBTENEMOS LA CALIFICACIÓN DE LA ZONA ----*----*----*----*----*--
                        $('#trCalZona').html(response[11]);

                        //Lanzamos mensaje de alerta en caso de haberlo
                        if (response[10] != "") {
                            popUpAlert(response[10], 'info');
                        } else {
                            //No hay mensaje de alerta
                        }
                    } else {
                        //Limpiamos la seccion de Plantación
                        LimpiaPlantacion();

                        //Limpiamos la sección de NO Plantación
                        LimpiaNOPlantacion();

                        //Lanzamos mensaje de error
                        popUpAlert(response[1], response[2]);
                    }
                });
            }
            else {
                //No hay accion
            }
        }
        
        function Graficar(contenedor, ejeX, ejeY1, ejeY2) {            
            Highcharts.chart(contenedor, {
                chart: { type: 'column' },
                //title: { text: 'Hola' },
                //subtitle: { text: 'Adios' },
                xAxis: {
                    categories:
                        ejeX
                    ,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '%'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:8px">{point.key}</span><table>',
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
                    name: 'Calificación',
                    data: ejeY1

                }, {
                    name: 'Distribución',
                    data: ejeY2

                }]
            });
        }

        function LimpiaPlantacion() {
            $('#tblResumenXGrupoPlantacion tbody td.info').html('').removeClass('red').removeClass('orange').removeClass('green');
            $('#tblResumenXGrupoPlantacion tbody td.CumpPlantacionGrupoTotal').html('0%').removeClass('red').removeClass('orange').removeClass('green').addClass('red');
            $('#tblResumenXGrupoPlantacion tbody td.CalPlantacionGrupoTotal').html('0%').removeClass('red').removeClass('orange').removeClass('green').addClass('red');
            $('#tblProblemasXGrupoPlantacion tbody td.info').html('');
            $('#GraficaXGrupoPlantancion').html('');
            $('#trCalZona').html('<td class="red">0%</td>');
        }

        function LimpiaNOPlantacion() {
            $('#tblResumenXGrupoNOPlantacion tbody td.info').html('').removeClass('red').removeClass('orange').removeClass('green');
            $('#tblResumenXGrupoNOPlantacion tbody td.CumpPlantacionGrupoTotal').html('0%').removeClass('red').removeClass('orange').removeClass('green').addClass('red');
            $('#tblResumenXGrupoNOPlantacion tbody td.CalPlantacionGrupoTotal').html('0%').removeClass('red').removeClass('orange').removeClass('green').addClass('red');
            $('#tblProblemasXGrupoNOPlantacion tbody td.info').html('');
            $('#GraficaXGrupoNOPlantancion').html('');
            $('#trCalZona').html('<td class="red">0%</td>');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1>
            Reporte Por Zona</h1>
        <div class="CalificacionPorcentaje">
            <table class="Planta">
                <tr>
                    <td> Planta </td>
                    <td> <label id="lblPlanta"></label> </td>
                    <td> Semana </td>
                    <td> <select id="ddlSemana"><option value="" semanaCorta="0">-- Seleccione --</option></select> </td>
                    <td> Líder </td>
                    <td> <select id="ddlLider"><option value="" semana="">-- Seleccione --</option></select> </td>
                    <%--<td> Grower </td>
                    <td> <label></label> </td>--%>
                </tr>
            </table>
            <table class="Porcentaje">
                <tr>
                    <td class="blue2"> Calificación de la zona </td>
                </tr>
                <tr id="trCalZona">
                    <td class="red"> 0% </td>
                </tr>
            </table>
        </div>
        <div class="ResumenGrupo">
            <h3 class="h3blue1">
                Resumen x grupo (GH Plantación)</h3>
            <div class="flex">
                <div>
                    <table id="tblResumenXGrupoPlantacion" class="Plantacion">
                        <thead>
                            <tr>
                                <th> &nbsp; </th>
                                <th> CUMPLIMIENTO </th>
                                <th> CALIFICACIÓN </th>
                                <th> DISTRIBUCIÓN </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td> PLANTACIÓN </td>
                                <td class="info"> </td>
                                <td class="info CalPlantacionGrupo"> &nbsp; </td>
                                <td class="info DistPlantacionGrupo"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> CONTROL DE CLIMA </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info CalPlantacionGrupo"> &nbsp; </td>
                                <td class="info DistPlantacionGrupo"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> FERTIRRIEGO </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info CalPlantacionGrupo"> &nbsp; </td>
                                <td class="info DistPlantacionGrupo"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> POLINIZACIÓN </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info CalPlantacionGrupo"> &nbsp; </td>
                                <td class="info DistPlantacionGrupo"> &nbsp; </td>
                            </tr>
                            <tr id="trCalificacionGrupo" class="calificacion">
                                <td> CALIFICACIÓN </td>
                                <td class="red info CumpPlantacionGrupoTotal"> 0% </td>
                                <td class="red info CalPlantacionGrupoTotal"> 0% </td>
                            </tr>
                        </tbody>
                    </table>
                    <table id="tblProblemasXGrupoPlantacion" class="problemas">
                        <tr>
                            <td class="blue1 tdProblema"> PROBLEMA 1 </td>
                            <td class="info"> &nbsp; </td>
                        </tr>
                        <tr>
                            <td class="blue2 tdProblema"> PROBLEMA 2 </td>
                            <td class="info"> &nbsp; </td>
                        </tr>
                        <tr>
                            <td class="blue3 tdProblema"> PROBLEMA 3 </td>
                            <td class="info"> &nbsp; </td>
                        </tr>
                    </table>
                </div>
                <div id="GraficaXGrupoPlantancion">
                </div>
            </div>
        </div>
        <div class="ResumenGrupo">
            <h3 class="h3blue2">
                Resumen x grupo (GH No Plantación)</h3>
            <div class="flex">
                <div>
                    <table id="tblResumenXGrupoNOPlantacion" class="NoPlantacion">
                        <thead>
                            <tr>
                                <th> &nbsp; </th>
                                <th> CUMPLIMIENTO </th>
                                <th> CALIFICACIÓN </th>
                                <th> DISTRIBUCIÓN </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td> CONTROL DE CLIMA </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> TRABAJOS CULTURALES </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> PLAGAS Y ENFERMEDADES </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> TRAMPEO </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> LIMPIEZA DE INVERNADEROS </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> FERTIRRIEGO </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> ESTADO DEL FRUTO </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr>
                                <td> POLINIZACIÓN </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                                <td class="info"> &nbsp; </td>
                            </tr>
                            <tr class="calificacion">
                                <td> CALIFICACIÓN </td>
                                <td class="red info CumpPlantacionGrupoTotal"> 0% </td>
                                <td class="red info CalPlantacionGrupoTotal"> 0% </td>
                                <td> &nbsp; </td>
                            </tr>
                        </tbody>
                    </table>
                    <table id="tblProblemasXGrupoNOPlantacion" class="problemas">
                        <tr>
                            <td class="blue1 tdProblema"> PROBLEMA 1 </td>
                            <td class="info"> &nbsp; </td>
                        </tr>
                        <tr>
                            <td class="blue2 tdProblema"> PROBLEMA 2 </td>
                            <td class="info"> &nbsp; </td>
                        </tr>
                        <tr>
                            <td class="blue3 tdProblema"> PROBLEMA 3 </td>
                            <td class="info"> &nbsp; </td>
                        </tr>
                    </table>
                </div>
                <div id="GraficaXGrupoNOPlantancion">
                </div>
            </div>
        </div>
        <div id="ResumenInvernaderoPlantacion" class="ResumenInvernadero">
            <h3 class="h3blue1">Resumen x invernadero (GH Plantación)</h3>
        </div>
        <div id="ResumenInvernaderoNOPlantacion" class="ResumenInvernadero">
            <h3 class="h3blue2">
                Resumen x Invernadero (GH No Plantación)</h3>
        </div>
    </div>
</asp:Content>

