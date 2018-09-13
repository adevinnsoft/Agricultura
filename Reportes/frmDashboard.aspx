<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmDashboard.aspx.cs" Inherits="Reportes_frmDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../comun/css/comun.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/slider/slick.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/dates.js" type="text/javascript"></script>
    <link href="../comun/scripts/fullcalendar.min.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/highcharts.js" type="text/javascript"></script>    
    <script src="../comun/scripts/fullcalendar.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fullcalendar_es.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <script type="text/javascript" id="FuncionamientoDelCalendario">
        
        var arrPlan = [];
        var arrEjecucion = [];
        var calendar = null;
        var invernaderosSlick = null;
        var Errores;
        var timeOut = 850;
        
        function obtenerIdPlanta() {
            return $('[id*="ddlPlanta"] option:selected').val();
        }

        function cargarGraficaDeProgramacionSemanal() {
            if (calendar != null)
                calendar.fullCalendar('destroy');

            var idLider = $('#ddLider option:selected').attr('idLider');
            var idInvernadero = $('#roller .selected').attr('IdInvernadero');
            if (idInvernadero == undefined) return;
            var plan = $('.checkBoxPlan').prop('checked');
            var ejecucion = $('.checkBoxEjecucion').prop('checked');
            var semana = $('#imgSemanaCalendario').val();
            var anio = $('#imgSemanaAnioCalendario').val();

            if (plan) {
             var PlanObtenido = obtenerPlan(semana, anio, idLider, idInvernadero);
             if (PlanObtenido != null) {
                 mostrarTareas(PlanObtenido,true);
             } else {
                 PageMethods.ObtenerPlanDashboard(semana, anio, idLider, [idInvernadero], function (response) {
                     if (response[0] == '1') {
                         PlanObtenido = JSON.parse(response[2]);
                         arrPlan.push(PlanObtenido);
                          mostrarTareas(PlanObtenido,true);
                     }
                     else {
                         //popUpAlert(response[1], response[2]);
                     }
                 }, function (e) {
                     console.log(e);
                 });
              }
            }
            else { 
                eliminarTareas(true);
                //El usuario no desea mostrar planeacion
            }

            if(ejecucion)
            {
                var EjecucionObtenida = obtenerEjecucion(semana, anio, idLider, idInvernadero);
                 if (EjecucionObtenida != null) {
                     mostrarTareas(EjecucionObtenida, false);
                 } else {
                     PageMethods.ObtenerEjecucionDashboard(semana, anio, idLider, [idInvernadero], function (response) {
                         if (response[0] == '1') {
                             EjecucionObtenida = JSON.parse(response[2]);
                             arrEjecucion.push(EjecucionObtenida);
                              mostrarTareas(EjecucionObtenida, false);
                         }
                         else {
                             //popUpAlert(response[1], response[2]);
                         }
                     }, function (e) {
                         console.log(e);
                     });
                 }
            }
            else { 
                 eliminarTareas(false);
                 //El usuario no desea mostrar ejecucion
            }
        }
        
        function obtenerPlan(semana, anio, idLider, idInvernadero) {
            for (var i = 0; i < arrPlan.length; i++) {
                if (arrPlan[i].idLider == idLider && arrPlan[i].idInvernadero == idInvernadero && arrPlan[i].semana == semana && arrPlan[i].anio == anio) { 
                     return arrPlan[i]
                }
                else {
                    return null;
                }
            }
        }
        function obtenerEjecucion(semana, anio, idLider, idInvernadero) {
            for (var i = 0; i < arrEjecucion.length; i++) {
                if (arrEjecucion[i].idLider == idLider && arrEjecucion[i].idInvernadero == idInvernadero && arrEjecucion[i].semana == semana && arrEjecucion[i].anio == anio) { 
                     return arrEjecucion[i]
                }
                else {
                    return null;
                }
            }
        }

        function mostrarTareas(Tareas, isPlan) {
            var Actividades = [];
            if (isPlan) {
                for (var i = 0; i < Tareas.length; i++) {
                    Actividades.push({//obtenemos actividades planeadas
                        "idInvernaderoEvent": Tareas[i].idInvernadero,
                        "isPlan": isPlan,
                        "start": "" + Tareas[i].Plan.inicio + "",
                        "end": "" + Tareas[i].Plan.fin + "",
                        "title": "P" + Tareas[i].Plan.invernadero + "" + ":" + " " + Tareas[i].Plan.nombreHabilidad + "" + "-" + Tareas[i].Plan.etapa + "" + " " + Tareas[i].Plan.producto + "(" + Tareas[i].Plan.variedad + ")",
                        "backgroundColor": "#" + Tareas[i].Plan.color + "",
                        "textColor": 'black',
                        "className": 'actividadPlaneada'
                    });
                }
            }
            else {
                for (var i = 0; i < Tareas.length; i++) {
                    Actividades.push({//obtenemos actividades planeadas
                        "idInvernaderoEvent": Tareas[i].idInvernadero,
                        "isPlan": isPlan,
                        "start": "" + Tareas[i].Ejecucion.inicio + "",
                        "end": "" + Tareas[i].Ejecucion.fin + "",
                        "title": "E" + Tareas[i].Ejecucion.invernadero + "" + ":" + " " + Tareas[i].Ejecucion.nombreHabilidad + "" + "-" + Tareas[i].Ejecucion.etapa + "" + " " + Tareas[i].Ejecucion.producto + "(" + Tareas[i].Ejecucion.variedad + ")",
                        "backgroundColor": "#" + Tareas[i].Ejecucion.color + "",
                        "textColor": 'black',
                        "className": 'actividadEjecutada'
                    });
                }
            }
            if (Actividades.length == 0) {
                popUpAlert("No existen actividades planeadas para este invernadero", "warning");
            }
            else {
                if ($('.fc-time-grid-event').length == 0) {
                    crearCalendario(Actividades);
                }
                else {
                    $('#calendarProgramacion').fullCalendar('addEventSource', Actividades)
                }
            }
        }
        function eliminarTareas(isPlan){
            if(calendar!=null){
                calendar.fullCalendar('removeEvents', function(event) {
                    return event.isPlan == isPlan;
                });
            }else{
                //No existe un calendario, no es necesaria una instruccion
            }
        }

        Date.prototype.addHours = function(h) {    
           this.setTime(this.getTime() + (h*60*60*1000)); 
           return this;   
        }
        function crearCalendario(Activities) {//funcion para crear el calendario
        var opciones ={
                editable: false,
                droppable: false,
                draggable: false,
                defaultView: 'agendaWeek',
                fixedWeekCount: true,
                height: 600,
                header: { left: 'title', center: 'month, agendaWeek', right: '' },
                allDaySlot: false,
                firstDay: 0,
                selectable: false,
                timeFormat: 'HH:mm',
                slotDuration: '00:30:00',
                slotLabelInterval: '01:00:00',
                snapDuration: '00:05:00',
                eventResizeStart: function () {
                    $('#calendarProgramacion').fullCalendar('removeEvents', 'Cal');
                },
                eventAfterAllRender: function(view){
                     var horaMinima = new Date($($("#calendarProgramacion").fullCalendar('clientEvents')).map(function () { return this.start._i }).get().sort()[0]).addHours(-1);
                     var horaMaxima = new Date($($("#calendarProgramacion").fullCalendar('clientEvents')).map(function () { return this.end._i }).get().sort().reverse()[0]).addHours(1);
                     opciones.eventAfterAllRender = function () { return null; }
                     $("#calendarProgramacion").fullCalendar('destroy');
                     $("#calendarProgramacion").fullCalendar(
                        $.extend(opciones, {
                         minTime: horaMinima,
                         maxTime: horaMaxima
                        })
                     );
                },
                events: Activities
            };
            
            calendar = $('#calendarProgramacion').fullCalendar(opciones);

            $('#calendarProgramacion').fullCalendar('gotoDate', Activities[0].start);
       
        }

        function Limpiar(){
            if(calendar!=null)
                calendar.fullCalendar('destroy');
            if(invernaderosSlick!=null)
                $('#rollslider').slick('destroy');
            calendar = null;
            invernaderosSlick = null;  
            $('#ddLider option').first().prop('selected', true);
            $('.checkBoxPlan').prop('checked', false);
            $('.checkBoxEjecucion').prop('checked', false);
        }

</script>
<script type="text/javascript" id="FuncionamientoDeGraficasGenerales">
    var graficaLineal = null;
    var graficaPastel = null;
    
    function obtenerDatosDeGraficaLineal() {
        var idLider = $('#ddLider option:selected').attr('idLider');
        var idInvernadero = $('#roller .selected').attr('IdInvernadero');
        if (idInvernadero == undefined) return;
        var semana = $('#imgSemanaCalendario').val();
        var anio = $('#imgSemanaAnioCalendario').val();
        try {
            $.blockUI();
            PageMethods.ObtenerPromediosGraficas(semana, anio, idLider, [idInvernadero], function (avgsResponse) {
                if (avgsResponse[0] == '1') {
                    var auxiliar1 = avgsResponse[2].split(' ');
                    var auxiliar2 = avgsResponse[3].split(' ');

                    var avgsPlan = [];
                    var avgsEjecucion = [];
                    for (var pos = 0; pos < auxiliar1.length; pos++) {
                        avgsPlan.push(parseFloat(auxiliar1[pos]));
                    }

                    for (var pos = 0; pos < auxiliar1.length; pos++) {
                        avgsEjecucion.push(parseFloat(auxiliar2[pos]));
                    }
                    if (graficaLineal != null) {
                        $('#graficaPlanEjecucion').highcharts().destroy();
                        graficaLineal = null;
                    } else { 
                        //Se asignara la grafica en la siguiente instruccion.
                    }

                    graficaLineal = $('#graficaPlanEjecucion').highcharts({
                        xAxis: {
                            categories: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado']
                        },
                        yAxis: {
                            title: {
                                text: 'Surcos'
                            },
                            plotLines: [{
                                value: 0,
                                width: 1,
                                color: '#808080'
                            }]
                        },
                        title: {
                            text: 'Plan vs Ejecucion'
                        },
                        tooltip: {
                            valueSuffix: ' Surcos'
                        },
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle',
                            borderWidth: 0
                        },
                        series: [{
                            name: 'Plan',
                            data: avgsPlan
                        },
                        {
                            name: 'Ejecucion',
                            data: avgsEjecucion
                        }]
                    });
                }
                else {
                    if (graficaLineal != null) {
                        $('#graficaPlanEjecucion').highcharts().destroy();
                        graficaLineal = null;
                    } else {
                        //Se asignara la grafica en la siguiente instruccion.
                    }
                    //popUpAlert(avgsResponse[1], avgsResponse[2]);
                }
            });
        } catch (e) {
            if (graficaLineal != null) {
                $('#graficaPlanEjecucion').highcharts().destroy();
                graficaLineal = null;
            } else {
                //Se asignara la grafica en la siguiente instruccion.
            }
            console.log(e);
        } finally {
            window.setTimeout(function(){$.unblockUI();},timeOut);
        }
    }
    function obtenerDatosDeGraficaDePastel() {
            var idLider = $('#ddLider option:selected').attr('idLider');
            var idInvernadero = $('#roller .selected').attr('IdInvernadero');
            if (idInvernadero == undefined) return;
            var semana = $('#imgSemanaCalendario').val();
            var anio = $('#imgSemanaAnioCalendario').val();
            try {
                $.blockUI();
                PageMethods.ObtenerDetalleDashboard(semana, anio, idLider, [idInvernadero], function (r) {
                    if (r[0] == '1') {
                        var porcentaje = parseInt(r[3]);
                        if (graficaPastel != null) {
                            graficaPastel = null;
                            $('#graficaCumplimiento').highcharts().destroy();
                        } else { 
                            //La grafica se asignara en la siguiente instruccion
                        }
                        graficaPastel = $('#graficaCumplimiento').highcharts({
                            chart: {
                                plotBackgroundColor: null,
                                plotBorderWidth: null,
                                plotShadow: false,
                                type: 'pie'
                            },
                            title: {
                                text: 'Cumplimiento'
                            },
                            tooltip: {
                                pointFormat: '{series.name}: <b>{point.percentage:' + porcentaje + '}%</b>'
                            },
                            plotOptions: {
                                pie: {
                                    allowPointSelect: true,
                                    cursor: 'pointer',
                                    dataLabels: {
                                        enabled: true,
                                        format: '<b>{point.name}</b>: {point.percentage:' + porcentaje + '} %',
                                        style: {
                                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                        }
                                    }
                                }
                            },
                            series: [{
                                name: 'Brands',
                                colorByPoint: true,
                                data: [{
                                    name: 'Cumplimiento',
                                    y: porcentaje
                                }]
                            }]
                        });
                    } else {
                        if (graficaPastel != null) {
                            graficaPastel = null;
                            $('#graficaCumplimiento').highcharts().destroy();
                        } else {
                            //La grafica se asignara en la siguiente instruccion
                        }
                        //popUpAlert(r[1], r[2]);
                    }
                });
            } catch (e) {
                if (graficaPastel != null) {
                    graficaPastel = null;
                    $('#graficaCumplimiento').highcharts().destroy();
                } else {
                    //La grafica se asignara en la siguiente instruccion
                }
                console.log(e);   
            }
            finally {
                window.setTimeout(function(){$.unblockUI();},timeOut);
            }
    }
    
    
</script>

    <script type="text/javascript">
        var JSONinvernaderos = [];
        var JSONinvernaderos_2 = [];
        var semana = "";
        var semana_2 = "";
        var anio = "";
        var anio_2 = "";
        var idLider = "";
        var idLider_2 = "";
        var Invernaderos = [];
        var Invernaderos_2 = [];
        var Actividades = [];
        var Actividades_2 = [];
        var avgsPlan = [];
        var avgsEjecucion = []


        function semanaAnterior() {
            var semana = parseInt($('#imgSemanaCalendario').val());
            var anio = parseInt($('#imgSemanaAnioCalendario').val());
            semanaActual = semana;
            maxAnioAnterior = 52;
            if (semana == 1) {
                semana = maxAnioAnterior;
                anio = parseInt($('#imgSemanaAnioCalendario').val()) - 1;
                $('#imgSemanaCalendario').val(semana);
                $('#imgSemanaAnioCalendario').val(anio);
            }
            else {
                semana = parseInt(semana) - 1;
                $('#imgSemanaCalendario').val(semana);
            }

            $('#calendarProgramacion').fullCalendar('prev');
            PageMethods.InicioFinSemana($('#imgSemanaCalendario').val(), $('#imgSemanaAnioCalendario').val(), function (response) {
                rangosemana = JSON.parse(response);
                rangosemana.EndDate += 86340000;

            });
            cargarGraficas();
        }


        function semanaSiguiente() {
            var semana = $('#imgSemanaCalendario').val();
            var anio = $('#imgSemanaAnioCalendario').val();
            semanaActual = semana;
            maxAnioActual = 53;
            maxAnioSiguiente = 52;
            if (semana == maxAnioActual) {
                semana = 1;
                anio = parseInt($('#imgSemanaAnioCalendario').val()) + 1;
                $('#imgSemanaCalendario').val(1);
                $('#imgSemanaAnioCalendario').val(anio);
            }
            else {
                semana = parseInt(semana) + 1;
                $('#imgSemanaCalendario').val(semana);
            }

            $('#calendarProgramacion').fullCalendar('next');
            PageMethods.InicioFinSemana($('#imgSemanaCalendario').val(), $('#imgSemanaAnioCalendario').val(), function (response) {
                rangosemana = JSON.parse(response);
                rangosemana.EndDate += 86340000;
            });
            cargarGraficas();
        }
        function cargarInvernaderosDeLider() {

            var idLider = $('#ddLider option:selected').attr('idLider');
            if (idLider == 0) {
                // Se selecciono la opcion sin lider.
                if (invernaderosSlick != null) {
                    invernaderosSlick.slick('destroy');
                } else {
                    //}No hay nada que eliminar.
                }
            } else {
                var nombreUsuario = $('#ddLider option:selected').text();
                if (idLider != 0) {
                    try {
                        $.blockUI();
                        PageMethods.cargarInvernaderos(nombreUsuario, idLider, obtenerIdPlanta(), function (response) {
                            if (response[0] == '1') {
                                
                                if (invernaderosSlick != null) {
                                    invernaderosSlick.slick('destroy');
                                } else {
                                    //Se asignara el slick en el despues de introducri el HTML.
                                }

                                $('#rollslider').html(response[2]);
                                $('#rollslider').find('div[idLider]').hide();

                                invernaderosSlick = $('#rollslider').slick({
                                    slidesToShow: 5, //$('#rollslider div').length < 12 ? $(this).length : 12,
                                    slidesToScroll: 5, //$('#rollslider div').length > 12 ? 5 : 2,
                                    infinite: false,
                                    draggable: false,
                                    variableWidth: true
                                });

                                $('.Invernadero').click(function () {
                                    $('.Invernadero').removeClass('selected');
                                    $(this).addClass('selected');
                                    cargarGraficas();
                                });                                
                                
                                $('#rollslider').find('div[idLider="' + idLider + '"]').show();
                                $('#lblInvernaderos').show();
                                $('.invernaderos').show();

                            } else {
                                popUpAlert(response[2], response[1]);
                                $('#rollslider').html('');
                            }
                        });
                    } catch (e) {
                        console.log(e);
                    } finally {
                        window.setTimeout(function(){$.unblockUI();},timeOut);
                    }
                }
            }
        }

        function cargarLideres() {
            var idPlanta = $('[id*="ddlPlanta"] option:selected').val();
            try {
                $.blockUI();
                PageMethods.cargarCombo(idPlanta, function (response) {
                    if (response[0] == '1') {
                        $('#ddLider').html(response[2]);
                        $('#ddLider').change(function () {
                            cargarInvernaderosDeLider();
                        });
                    }
                    else {
                        popUpAlert(response[2], response[1]);
                    }
                });
            } catch (e) {
                console.log(e);
            } finally {
                window.setTimeout(function(){$.unblockUI();},timeOut);
            }            
        }

        function establecerFechaInicial() {
            semanaActual = parseInt($('span#ctl00_ltSemana').text());
            $('#imgSemanaCalendario').val(semanaActual);
            $('#imgSemanaAnioCalendario').val(new Date().getFullYear());
            PageMethods.InicioFinSemana($('#imgSemanaCalendario').val(), $('#imgSemanaAnioCalendario').val(), function (response) {
                rangosemana = JSON.parse(response);
                rangosemana.EndDate += 86340000;
            });
        }

        function mostrarDetalles() {
            semana = $('#imgSemanaCalendario').val();
            anio = $('#imgSemanaAnioCalendario').val();
            idLider = $("#ddLider option:selected").attr('idlider');
            var idsInvernaderos = [];
            var idInvernadero = 0;

            if ($('.selected').length == 0) {
                popUpAlert("Seleccione un Invernadero", "warning");
            } else {
                $('.selected').each(function () {
                    idsInvernaderos.push($(this).attr('idinvernadero'));
                });
                try {
                    $.blockUI();
                    PageMethods.ObtenerDetalleDashboard(semana, anio, idLider, idsInvernaderos, function (response) {
                        if (response[0] == '1') {
                            $('#divTabulares').empty();
                            $('#divTabulares').html(response[2]);
                            $('#popUpTabulares').show();
                        }
                        else {
                            //popUpAlert(response[1], response[2]);
                        }
                    });
                } catch (e) {
                    console.log(e);
                } finally {
                    window.setTimeout(function(){$.unblockUI();},timeOut);
                }
                

            }
        }
        function cargarGraficas() {
            cargarGraficaDeProgramacionSemanal();
            obtenerDatosDeGraficaDePastel();
            obtenerDatosDeGraficaLineal();
        }
    </script>
    <script type="text/javascript" id="Main">

        $(function () {
            $('[id*="ddlPlanta"').change(function () {
                Limpiar();
                cargarLideres();
            });

           
            $('.checkBoxPlan').click(function () {
                cargarGraficas();
            });
            $('.checkBoxEjecucion').click(function () {
                cargarGraficas();
            });

            establecerFechaInicial();
            cargarLideres();
        });

    
    </script>
    <style type="text/css">
        
        .inline
        {
            display: inline-block;
            width: 650px;

        }
        .graficas
        {
            width: 550px;
            height: 200px;
        }
        .grafica
        {
            width: 650px;
            height: 350px;
            display: inline-block;
        }
        .con
        {
            width: 1400px;
        }
        .boton
        {
            width: 100px;
        }
        .calendario
        {
            width: 1400px;
            height: 400px;
        }
    
         #rollslider div.Invernadero
        {
            width: 62px;
            
        }
        
        div.Invernadero {
        display: inline-block;
        padding-top: 20px;
        color: #000;
        min-width: 30PX;
        min-height: 30px;
        width: 62px;
        height: 39px;
        text-align: center;
        margin: 3px;
        cursor: pointer;
        font-size: 18px;
        font-weight: bold;
        border-radius: 10px;
        border: solid 1px #000;
        background-image: -webkit-gradient(linear,left top,left bottom,color-stop(0, #fefefe), color-stop(100, #CDCDCD));
     }
    
    .tdInvernadero {
        display: inline-block;
        margin: 2px;
     }
       div#rollslider {
        width: 877px;
    }
    
    .invernaderos
    {
        display:none;
    }
      
    .selected
    {
        border: 1px solid #adc995;
        -webkit-box-shadow: 0px 0px 3px 3px #adc995;
        box-shadow: 0px 0px 3px 3px #adc995;
    }   
    
    #popUpTabulares{
    position: fixed;
    width: 845px;
    height: 77%;
    background: white;
    z-index: 9999;
    left: 0;
    top: 0;
    bottom:0;
    right:0;
    overflow-x: hidden;
    overflow-y: auto;
    border: 1px solid #cccccc;
    max-width: 845px;
    -moz-box-shadow: 0 0 9px #999999;
    -webkit-box-shadow: 0 0 9px #999999;
    box-shadow: 0 0 9px #999999;
    display: none;
    margin:auto;
    }
    
    div.popUpHeader{
        background: #000080;
        height: 42px;
        width: 100%;
        color:yellow;
    }

    .divTabulares
    {
        padding: 5px;
    }

    #popUpTabulares
    {
        display:none;
    }
    
    #lblInvernaderos
    {
        display:none;
    }
    .fc-event .fc-content {
        position: relative;
        z-index: 2;
        margin-left: 20px;
        max-width: 100px;
        margin-top: 2px;
        overflow-wrap: normal;
        white-space: pre-line;
    }
    .actividadPlaneada {
        background-image: url('../comun/img/activity01.png');
        background-repeat: no-repeat;
        background-size: 18px;
        background-position-y: 1px;
        background-position-x: 1px;
    }
   .actividadEjecutada {
        background-image: url('../comun/img/activity02.png');
        background-repeat: no-repeat;
        background-size: 18px;
        background-position-y: 1px;
        background-position-x: 1px;
    }
    .imgCumplimiento
    {
        width: 27px;
        margin-top: -7px;
    }
    h2.invernaderoTabularTitulo {
        padding-top: 5px;
    }
    
    select#ddLider 
    {
        min-width: 170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Dashboard</asp:Label>
            <span id="product"></span>
        </h1>
        <table class="index3">
            <tr>
                <td>
                    <label>Semana:</label>
                </td>
                <td>
                    <img src="../comun/img/prev.png" style="float: none;" alt="" onclick="semanaAnterior();" />
                    <input id="imgSemanaCalendario" type="text" style="float: none; width: 60px; text-align: center;"
                        readonly="1" />
                    <input id="imgSemanaAnioCalendario" type="text" style="float: none; width: 60px; text-align: center;"
                        readonly="1" />
                    <img src="../comun/img/next.png" style="float: none;" alt="" onclick="semanaSiguiente();" />
                </td> 
                <td>
                    <label>Líder:</label>
                </td>               
                <td>                    
                    <select id="ddLider" class="selector">                        
                       
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label id="lblInvernaderos">Invernaderos</label>
                </td>
                <td colspan="3">
                    <div id="roller" class="invernaderos">
                        <div id="rollslider">
                        </div>
                    </div>
                </td>
            </tr>
            <tr class="trInvernaderos" id="Invernaderos">
   
            </tr>
            <tr>
                <td colspan="4">
                    <input type="checkbox" class="checkBoxPlan" id="plan"/>
                    <label for="plan">Plan</label>
                    <input type="checkbox" class="checkBoxEjecucion" id="ejecucion"/>
                    <label for="ejecucion">Ejecución</label>
                </td>
            </tr>
            <tr>
                <td colspan="4">              
                  <div id="calendarProgramacion"></div> <%--class="calendario"--%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="graficaPlanEjecucion" class="graficas"></div>
                </td>
                <td colspan="2">
                    <div id="graficaCumplimiento" class="graficas"></div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align:center">
                    <input type="button" id="btnDetalles" value="Ver Detalles" onclick="mostrarDetalles();"/>
                </td>
            </tr>
        </table>
    </div>
     <div id="popUpTabulares">
        <div class="popUpHeader moveHandle">
           <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popUpTabulares').hide();" style="float: right; margin: 10px; cursor: pointer;" />
            <div id="divTabulares">
            </div>
        </div>
    </div>
</asp:Content>
