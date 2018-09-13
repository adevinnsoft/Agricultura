var mismodia;
var cosechasD = [];
var limpiezaok =[];
var limpiezano = [];
var podas = [];
var actLimpieza = [];
var actDeshoje = [];
var actPoda = [];
var ddl;

function crearCalendario(Actividades) {
    $('#calendar').fullCalendar({
        droppable: true,
        defaultView: 'agendaWeek',
        fixedWeekCount: true,
        height: 600,
        events: Actividades,
        allDaySlot: false,
        minTime: calendarioInicio,
        maxTime: calendarioFin,
        firstDay: diaInicioDeSemana,
        customButtons: {
            eliminar: {
                text: 'Eliminar actividades seleccionadas',
                click: function () {
                    eliminarSeleccionados();
                }
            }
        },
        selectable: true,
        lang: idiomaCalendario,
        header: { left: 'title', center: 'month, agendaWeek, agendaDay', right: 'eliminar, prev, today, next' },
        timeFormat: 'HH:mm',
        slotDuration: '00:30:00',
        slotLabelInterval: '01:00:00',
        snapDuration: '00:05:00',
        eventRender: function (event, element) {
            if (event.id != "Cal" && event.editable)
                $("<input type='checkbox' class='selectObject' idTr='" + event.idTr + "' idjson='" + event.id + "'/>").insertBefore(element.find('.fc-time span'));
        },
        eventClick: function (calEvent, jsEvent, view) {
            //$('a div div input[idtr=' + calEvent.idTr + ']').parents("a").attr('selected', true);
            //            if ($(this).attr('selected')) {
            //                $(this).attr('selected', false);
            //                $(this).css('border-color', 'black');
            //            }
            //            else {
            //                $(this)
            //                $(this).css('border-color', 'red');
            //            }
            if (calEvent.id != "Cal") {

                cleanActivityPopUp();

                $('#ActivityData').attr('idJson', calEvent.id).attr('idTr', calEvent.idTr);
                $('#ActivityData').find('input#ActividadInicio').val(calEvent.start.format("YYYY-MM-DD HH:mm"));
                $('#ActivityData').find('input#ActividadFin').val(calEvent.end.format("YYYY-MM-DD HH:mm"));
                $('#ActivityData').find('input#ActividadSurcos').val(calEvent.surcos);
                $('#ActivityData').find('#tdTitleActivity').text(calEvent.title);
                $('#ActivityData').find('input[type=text]').attr('disabled', true);
                $('#ActivityData').find('ul#Asociados').append(AsociadosPeriodo(calEvent));
                if (calEvent.editable) {
                    $('#ActivityData').find('span.addJornal').attr('idetapa', calEvent.idEtapa).attr('idjson', calEvent.id).attr('idtr', calEvent.idTr).attr('idCiclo', calEvent.idCiclo);
                }
                $('#ActivityData').find('span#ActividadSurcosEstimados').text(calculasurcos(calEvent));
                if (calEvent.editable == false) {

                    $('#ActivityData').find('.addJornal, .removeJornal, img.hint').remove();
                }
                //$('#ActivityData').find('span#ActividadSurcosEstimados').text();
                $('#popUpActividad').show();
                $('#ActivityData').find('input#ActividadSurcos').change();
            } else {
                //do nothing
            }

        },
        eventDragStart: function () {
            //$('#calendar').fullCalendar('removeEvents', 'Cal');
        },
        eventDragStop: function () {

        },
        eventResizeStart: function () {
            //$('#calendar').fullCalendar('removeEvents', 'Cal');
        },
        eventResizeStop: function () {

        },
        eventDrop: function (event, delta, revertFunc) {
            mismodia = event.start._i;
            change(event);
            if (escosecha(event.idHabilidad)) {
                separaCosechas(event);
            }
            filterbySite();
        },
        eventResize: function (event, delta, revertFunc) {
            if (esfumigacion(event.idHabilidad)) {
                revertFunc();
            } else {
                change(event);
                filterbySite();
            }
        }
    });


}

function eliminarSeleccionados() {
    var cantidad = $('input.selectObject:checked').length;
    if (cantidad == 0) {
        alert('No hay Actividades Seleccionadas');


    } else if (confirm('Seguro que desea eliminar ' + cantidad + ' actividades?')) {

        $('input.selectObject:checked').each(function () {
            $('tr[idjson="' + $(this).attr('idjson') + '"]').find('img.btnEliminarActividad').click();
        });
        filterbySite();
    }
}

function quitaActividad(idH) {

    for (var of = 0; of < dividedEvents.length; of++) {
        if (dividedEvents[of].id == idH) {
            dividedEvents.splice(of, 1);
        }

    }

}

function calculasurcos(Activity) {
    try {


        var eficiencia = 0;
        var tiempo = 0;
        var surcos = 0;

        for (var a = 0; a < Activity.Asociados.length; a++) {
            eficiencia += parseInt(Activity.Asociados[a].eficiencia);
        }
        tiempo = dateDiff(Activity.start, Activity.end, 'h', true);
        surcos = (eficiencia * tiempo / Activity.plantasPorSurco).toFixed(2);

        surcos = surcos > 0 ? surcos : 0;
    } catch (e) {
        return Activity.surcos;
    }
    return surcos;
}

function cleanActivityPopUp() {
    $('#ActivityData').attr('idJson', '').attr('idTr', '');
    $('#ActivityData').find('input#ActividadInicio').val('');
    $('#ActivityData').find('input#ActividadFin').val('');
    $('#ActivityData').find('input#ActividadSurcos').val('');
    $('#ActivityData').find('input[type=text]').attr('disabled', true); //!calEvent.editable);
    $('#ActivityData').find('input#ActividadSurcos').attr('disabled', false);
    $('#ActivityData').find('ul#Asociados').html('');
}

function escosecha(idH) {
    var result = false;
    //for (var h in Cosecha) {

    //    if (Cosecha[h] == idH) {
    //        result = true;
    //    }

    //}

    return result;
}

function eslimpieza(idh) {
    for (var h in limpiezas) {
        if (limpiezas[h] == idh) {
            return true;
        }
    }
    return false;
}

function esdeshoje(idh) {
    for (var h in deshojes) {
        if (deshojes[h] == idh) {
            return true;
        }
    }
    return false;
}

function espodayvuelta(idh) {
    for (var h in podayvuelta) {
        if (podayvuelta[h] == idh) {
            return true;
        }
    }
    return false;
}

function invert(hexTripletColor) {
    var color = hexTripletColor;
    if (color != undefined) {
        color = color.substring(1);           // remove #
        color = parseInt(color, 16);          // convert to integer
        color = 0xFFFFFF ^ color;             // invert three bytes
        color = color.toString(16);           // convert to hex
        color = ("000000" + color).slice(-6); // pad with leading zeros
        color = "#" + color;
    }              // prepend #
    return color;
}

function esfumigacion(idH) {
    var result = false;
    for (var h in fumigacion) {

        if (fumigacion[h] == idH) {
            result = true;
        }

    }

    return result;
}

function esPreparacion(idH) {
    var result = false;
    for (var h in preparacionSuelos) {

        if (preparacionSuelos[h] == idH) {
            result = true;
        }

    }

    return result;
}

function getinvernaderofromid(id) {
    return dividedEvents[id].invernadero;
}

function getWeek() {
    var moment = $('#calendar').fullCalendar('getDate');
    var date = moment.format();
    PageMethods.ObtieneSemanaNS(date, function (response) { return response; });
}

function getOnlyDate(fecha) {
    var fecha = moment(fecha);
    return fecha.format('YYYY-MM-DD');
}

function getOnlyTime(fecha) {
    var hora = moment(fecha);
    return hora.format('HH:mm');
}

function getDatetime(date) {
    var datetime = moment(date);
    return datetime.format('YYYY-MM-DD HH:mm');
}

function sameDay(inicio, fin) {

    var result = moment(getOnlyDate(inicio), "YYYY-MM-DD").isSame(moment(getOnlyDate(fin), "YYYY-MM-DD"));

    return result;
}

function dateDiff(inicio, fin, type, timevalid) {
    var inicio = moment(inicio);
    var fin = moment(fin);
    if (!timevalid) {
        inicio = getOnlyDate(inicio);
        fin = getOnlyDate(fin);
    }

    var diff = (Date.parse(fin) - Date.parse(inicio));

    switch (type) {
        case 'h':
        case 'H':
            return (diff / 1000 / 3600);
        case 'mm':
        case 'm':
            return (diff / 1000 / 60);
        case 'dd':
        case 'd':
            return (diff / 1000 / 3600 / 24);
        default:
            return 0;
    }
}

function addDay(fecha, days) {
    var mañana = moment(fecha);

    return mañana.add(days, 'days').format('YYYY-MM-DD');
}

function change(event) {
    var count = 1;
    if (esfumigacion(event.idHabilidad)) {
        dividirTraslapes(event);
        return;
    }

    var inicio = moment(event.start.format("YYYY-MM-DD HH:mm"));
    var fin = moment(event.end.format("YYYY-MM-DD HH:mm"));
    if (sameDay(inicio, fin)) {
        //el evento inicia y etmina el mismo día
        if (moment('2015-01-01 ' + getOnlyTime(fin)) <= moment('2015-01-01 ' + jornadaFin)) {
            if (moment(getOnlyTime(event.start), "HH:mm").isBefore(moment(jornadaInicio, "HH:mm"))) {
                event.start = moment(moment(getOnlyDate(inicio), "YYYY-MM-DD").format("YYYY-MM-DD ") + jornadaInicio);
                event.end = moment(event.start.format("YYYY-MM-DD HH:mm")).add(dateDiff(inicio, fin, 'm', true), 'minutes');
                change(event);
            }
            if (moment(getOnlyTime(event.end), "HH:mm").isAfter(moment(jornadaFin, "HH:mm"))) {

                if (confirm('Agregar horas extra?')) {
                    event.end = fin;
                } else {
                    event.end = moment(moment(getOnlyDate(fin), "YYYY-MM-DD").format("YYYY-MM-DD ") + jornadaFin); centro
                }
                change(event);
            }

            dividirTraslapes(event);
        }
        else {
            //el evento sobrepasa el horario de trabajo de la planta
            if (moment(getOnlyTime(event.start), "HH:mm").isAfter(moment(jornadaFin, "HH:mm"))) {
                event.start = moment(event.start.add(1, 'days').format("YYYY-MM-DD ") + jornadaInicio);
                event.end = moment(event.start.format("YYYY-MM-DD HH:mm")).add(1, 'hours');
                change(event);
            } else {
                if (confirm('La actividad sobrepasa el horario de planta, programar horas extra?')) {
                    dividirTraslapes(event);
                } else {
                    //                    var newEvent = copyEvent(event);
                    event.end = moment(getOnlyDate(inicio) + ' ' + jornadaFin);
                    if (event.end == event.start) {
                        event.start = moment(event.start.add(1, 'days').format("YYYY-MM-DD ") + jornadaInicio);
                        event.end = moment(event.start.format("YYYY-MM-DD HH:mm")).add(1, 'hours');
                    }
                    //                    newEvent.idPeriodo = undefined;
                    dividirTraslapes(event);
                }


                //                var inicio2 = moment(getOnlyDate(inicio));
                //                inicio2.add(1, 'days');
                //                inicio2 = moment(getOnlyDate(inicio2) + ' ' + jornadaInicio);

                //                var fin1 = moment(inicio2.format("YYYY-MM-DD HH:mm"));
                //                fin1.add(dateDiff(moment(jornadaFin, "HH:mm"), moment(getOnlyTime(fin), "HH:mm"), 'm', true), 'minutes');

                //                newEvent.start = inicio2;
                //                newEvent.end = fin1;

                //                dividirTraslapes(copyEvent(newEvent));
            }
        }
    }
    else {
        //el evento se salta al siguiente dia
        if (moment(getOnlyTime(event.start), "HH:mm").isAfter(moment(jornadaFin, "HH:mm"))) {
            event.end = moment(event.start.add(1, 'days').format("YYYY-MM-DD") + jornadaInicio).add(dateDiff(event.start, event.end, 'm', true));
            event.start = moment(event.start.add(1, 'days').format("YYYY-MM-DD") + jornadaInicio);
        } else {
            event.end = moment(getOnlyDate(event.start) + ' ' + jornadaFin);
        }

        dividirTraslapes(event);
        var newEvent = copyEvent(event);
        crearCosechasPorDia(newEvent);

        for (var c = 1; c < (dateDiff(inicio, fin, 'd', false)); c++) {
            newEvent.start = moment(getOnlyDate(addDay(newEvent.start, 1)) + ' ' + jornadaInicio);
            newEvent.end = moment(getOnlyDate(addDay(newEvent.end, 1)) + ' ' + jornadaFin);
            newEvent.surcos = Math.round(calculasurcos(newEvent));
            newEvent.idPeriodo = undefined;

            dividirTraslapes(copyEvent(newEvent));
            crearCosechasPorDia(newEvent);
        }

        var lastEvent = copyEvent(newEvent);
        lastEvent.start = moment(getOnlyDate(addDay(lastEvent.start, 1)) + ' ' + jornadaInicio);
        lastEvent.end = moment(getOnlyDate(addDay(lastEvent.end, 1)) + ' ' + getOnlyTime(fin));
        lastEvent.surcos = Math.round(calculasurcos(lastEvent));
        lastEvent.idPeriodo = undefined;
        dividirTraslapes(lastEvent);
    }
}

function crearCosechasPorDia(event) {

    if (escosecha(event.idHabilidad)) {
        event.idTr = 'normal_' + (Contador++);
        event.idActividad = undefined;
        event.act = 'N';
        event.directriz = 0;
        //agregarAtcividadHTML(event);
    }

}


function revisaNivelInfestacion() {
    //    console.log(new Date());

    $('li[number]').removeClass("Infestacion").removeAttr("title");
    var fecha = moment(rangosemana[0].StartDate);
    var endt = moment(rangosemana[0].EndDate);
    var actividades;

    //Filtra los periodos de todos los invernaderos cuando están en el mismo día de la semana
    for (fecha; fecha < endt; fecha.add(1, 'days')) {
        actividades = dividedEvents.filter(function (item, index, list) {
            if (item.start.get('date') == fecha.get('date') && escosecha(item.idHabilidad)) {
                return true;
            } else {

            } return false;
        });

        var idinvA = idinvB = 0;
        var infestacionA, infestacionB;
        for (var a in actividades) {

            if (idinvA != actividades[a].idInvernadero) {
                infestacionA = Infestaciones.filter(function (item, index, list) {
                    if (item.idInvernadero == actividades[a].idInvernadero) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                idinvA = actividades[a].idInvernadero;
            }


            for (var b in actividades) {
                if (idinvB != actividades[b].idInvernadero) {
                    infestacionB = Infestaciones.filter(function (item, index, list) {
                        if (item.idInvernadero == actividades[b].idInvernadero) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });

                    idinvB = actividades[b].idInvernadero;
                }
            }
            var existsInBoth = false, danger = false;
            for (var ia in infestacionA) {
                existsInBoth = false;
                for (var ib in infestacionB) {

                    if (actividades[a].idInvernadero != actividades[b].idInvernadero
                    && infestacionA[ia].idInvernadero != infestacionB[ia].idInvernadero) {
                        if (infestacionA[ia].idInfestacion == infestacionB[ib].idInfestacion) {
                            existsInBoth = true;

                            if (infestacionA[ia].nivelInfestacion > infestacionB[ib].nivelInfestacion
                            && actividades[a].end < actividades[b].start
                            || infestacionA[ia].nivelInfestacion < infestacionB[ib].nivelInfestacion
                            && actividades[b].end < actividades[a].start) {

                                for (var ia in actividades[a].Asociados) {
                                    for (var ib in actividades[b].Asociados) {
                                        if (actividades[a].Asociados[ia].idAsociado == actividades[b].Asociados[ib].idAsociado) {
                                            //console.log('invernaderoA: ' + actividades[a].idInvernadero + 'hora ' + actividades[a].start.format("YYYY-MM-DD HH:mm") + '-' + actividades[a].end.format("YYYY-MM-DD HH:mm") + ' infestacionA' + infestacionA[ia].nombreComun + infestacionA[ia].nivelInfestacion + ' invernaderoB ' + actividades[b].idInvernadero + 'hora ' + actividades[b].start.format("YYYY-MM-DD HH:mm") + '-' + actividades[b].end.format("YYYY-MM-DD HH:mm") + ' infestacionB' + infestacionB[ib].nombreComun + infestacionB[ib].nivelInfestacion);
                                            $('tr[idjson="' + actividades[a].id + '"]').find('li[number="' + actividades[a].Asociados[ia].idAsociado + '"]').addClass("Infestacion");
                                            $('tr[idjson="' + actividades[b].id + '"]').find('li[number="' + actividades[b].Asociados[ib].idAsociado + '"]').addClass("Infestacion");
                                            $('tr[idjson="' + actividades[a].id + '"]').find('li[number="' + actividades[a].Asociados[ia].idAsociado + '"]').attr("title", "Riesgo de Infestacion");
                                            $('tr[idjson="' + actividades[b].id + '"]').find('li[number="' + actividades[b].Asociados[ib].idAsociado + '"]').attr("title", "Riesgo de Infestacion");
                                            popUpAlert('riesgo de propagación de infestacion', 'warning');
                                        } //if
                                    } //for b
                                } //for a

                            } //if nivel >
                        } //if idInfes ==


                    } //if invernaderos <>
                } //for ib

                if (!existsInBoth && actividades[a].idInvernadero != actividades[b].idInvernadero
                && (actividades[a].end < actividades[b].start
                && infestacionA[ia] != undefined
                || actividades[b].end < actividades[a].start
                && infestacionA[ib] != undefined)) {
                    for (var ia in actividades[a].Asociados) {
                        for (var ib in actividades[b].Asociados) {
                            if (actividades[a].Asociados[ia].idAsociado == actividades[b].Asociados[ib].idAsociado) {
                                //console.log('invernaderoA: ' + actividades[a].idInvernadero + 'hora ' + actividades[a].start.format("YYYY-MM-DD HH:mm") + '-' + actividades[a].end.format("YYYY-MM-DD HH:mm") + ' infestacionA' + infestacionA[ia].nombreComun + infestacionA[ia].nivelInfestacion + ' invernaderoB ' + actividades[b].idInvernadero + 'hora ' + actividades[b].start.format("YYYY-MM-DD HH:mm") + '-' + actividades[b].end.format("YYYY-MM-DD HH:mm") + ' infestacionB' + infestacionB[ib].nombreComun + infestacionB[ib].nivelInfestacion);
                                $('tr[idjson="' + actividades[a].id + '"]').find('li[number="' + actividades[a].Asociados[ia].idAsociado + '"]').addClass("Infestacion");
                                $('tr[idjson="' + actividades[b].id + '"]').find('li[number="' + actividades[b].Asociados[ib].idAsociado + '"]').addClass("Infestacion");
                                $('tr[idjson="' + actividades[a].id + '"]').find('li[number="' + actividades[a].Asociados[ia].idAsociado + '"]').attr("title", "Riesgo de Infestacion");
                                $('tr[idjson="' + actividades[b].id + '"]').find('li[number="' + actividades[b].Asociados[ib].idAsociado + '"]').attr("title", "Riesgo de Infestacion");
                                popUpAlert('riesgo de propagación de infestacion: ' + actividades[a].invernadero + '->' + actividades[b].invernadero, 'warning');
                            } //if
                        } //for b
                    } //for a
                } // if existsInBoth

            } //for ia

        } //for actividades mismo dia

    } //for dia semana

    //    console.log(new Date());
}

//copiar un elemento JSON, hay otras formas pero son menos eficientes..(se ciclan* el objeto tiene una propiedad que se referncía a si mismo..).
function copyEvent(event) {
    var copiedEvent = {};

    copiedEvent.title = event.title;
    copiedEvent.start = moment(event.start);
    copiedEvent.end = moment(event.end);
    copiedEvent.surcos = event.surcos;
    copiedEvent.color = event.backgroundColor;
    copiedEvent.idInvernadero = event.idInvernadero;
    copiedEvent.editable = event.editable;
    copiedEvent.backgroundColor = event.backgroundColor;
    copiedEvent.borderColor = event.borderColor;
    copiedEvent.idTr = event.idTr;
    copiedEvent.textColor = invert(event.backgroundColor);
    copiedEvent.semanaInicio = event.semanaInicio;
    copiedEvent.semanaFin = event.semanaFin;
    copiedEvent.porTiempo = event.porTiempo;
    copiedEvent.target = event.target;
    copiedEvent.plantasPorSurco = event.plantasPorSurco;
    copiedEvent.idCiclo = event.idCiclo;
    copiedEvent.densidad = event.densidad;
    copiedEvent.nombreHabilidad = event.nombreHabilidad;
    copiedEvent.nombreEtapa = event.nombreEtapa;
    copiedEvent.elemento = event.elemento;
    copiedEvent.surcosT = event.surcosT;
    copiedEvent.razonesDirectriz = event.razonesDirectriz;
    copiedEvent.anio = event.anio;
    copiedEvent.directriz = event.directriz;
    copiedEvent.idEtapa = event.idEtapa;
    copiedEvent.idHabilidad = event.idHabilidad;
    copiedEvent.invernadero = event.invernadero;
    copiedEvent.numeroElementos = event.numeroElementos;
    copiedEvent.semana = event.semana;
    copiedEvent.idActividad = event.idActividad;
    copiedEvent.idPeriodo = event.idPeriodo;
    copiedEvent.Asociados = [];
    copiedEvent.act = event.act;
    copiedEvent.edad = event.edad;
    copiedEvent.programada = event.programada;
    copiedEvent.comentario = event.comentario;
    copiedEvent.razon = event.razon;
    copiedEvent.cantidadElemento = event.cantidadElemento;
    copiedEvent.esInterplanting = event.esInterplanting;
    copiedEvent.enviado = event.enviado;
    copiedEvent.variable = esPreparacion(event.idHabilidad) ? event.cantidadElemento : 0;
    copiedEvent.numeroactividad = event.numeroactividad;
    copiedEvent.idActividadNoP = event.idActividadNoP;
    copiedEvent.esColmena = event.esColmena;
    copiedEvent.surcoInicio = event.surcoInicio;
    copiedEvent.surcoFin = event.surcoFin;
    copiedEvent.aceptaColmena = event.aceptaColmena;
    copiedEvent.UUID = event.UUID;

    if (event.Asociados == undefined) {
        event.Asociados = [];
    } else {

        for (var a = 0; a < event.Asociados.length; a++) {
            copiedEvent.Asociados.push(jQuery.extend({}, event.Asociados[a]));
        }
    }
    return copiedEvent;
}

function agregarHabilidadACalendario(fechaInicio, fechaFin, titulo, descripcion, background, idTr, idInvernadero) {
    $('#calendar').fullCalendar('renderEvent', {
        title: titulo,
        start: fechaInicio,
        end: fechaFin,
        description: descripcion,
        color: background,
        idTr: idTr,
        idInvernadero: idInvernadero,
        stick: true
    });
}

function CargaActividadesJson() {
    Actividades = [];

    $('div#divTareasProgramadas div:not(".accordionBody")').each(function () {
        var divInvernadero = $(this);
        var idEtapa = '';
        var idInvernadero = divInvernadero.attr('id').split('_')[1];
        var idHabilidad = '';
        var Activity;


        $(divInvernadero).find('.programadas tbody tr:not(.trLoad)').each(function () {

            idHabilidad = $(this).find('.idHabilidad').text();

            change({
                'idInvernadero': idInvernadero,
                'idEtapa': $(this).find('.idEtapa').text(),
                'idHabilidad': idHabilidad,
                'editable': $(this).attr('editable'),
                'overlap': !esfumigacion(idHabilidad) && !escosecha(idHabilidad) ? true : false,
                'title': $(this).find('.idEtapa').parent().text() + '\n ' + $(this).parent().parent().parent().prev().find('label').text(),
                'start': moment($(this).find('.fechaInicio').val()).format('YYYY-MM-DD HH:mm'),
                'end': moment($(this).find('.fechaFin').val()).format('YYYY-MM-DD HH:mm'),
                'color': '#' + $(this).attr('color'),
                'idTr': $(this).attr('contador')
            })


            //if (esfumigacion(idHabilidad)) {
            //    change({
            //        'id': 'Cal',
            //        'idInvernadero': idInvernadero,
            //        'idEtapa': $(this).find('.idEtapa').text(),
            //        'idHabilidad': idHabilidad,
            //        'editable': false,
            //        'overlap': false,
            //        'title': $(this).find('.idEtapa').parent().text() + '\n ' + $(this).parent().parent().parent().prev().find('label').text() + '\n TIEMPO DE REENTRADA',
            //        'start': moment($(this).find('.fechaFin').val()).format('YYYY-MM-DD HH:mm'),
            //        'end': moment($(this).find('.fechaFin').val()).add($(this).find('.tiempoReentrada').text().split(' ')[0], 'm').format('YYYY-MM-DD HH:mm'),
            //        'color': '#' + $(this).attr('color')
            //    });
            //}

        });
    });

}

function getTecnologias() {

    var result = "<select class='ddltecnologias switchA' ><option>--Variables--</option>";
    for (var a in tecnologias) {
        result += "<option value='" + tecnologias[a].idVariable + "'>" + tecnologias[a].CodigoVariable + "-" + tecnologias[a].Descripcion + "</option>";
    }
    result += "</select>";
    return result;
}


//Separa las actividaddes del calendario para evitar solapes con fechas oficiales
function dividirTraslapes(Activity) {
    var eventbackup;
    var coincidence = false;
    var inicioA = moment(Activity.start.format("YYYY-MM-DD HH:mm"), "YYYY-MM-DD HH:mm");
    var finA = moment(Activity.end.format("YYYY-MM-DD HH:mm"), "YYYY-MM-DD HH:mm");

    if (!esfumigacion(Activity.idHabilidad)) {

        for (var i in oficiales) {
            //revisa que la Actividad y la fecha oficial pertenezcan al mismo invernadero
            if (oficiales[i].idInvernadero == Activity.idInvernadero && Activity.editable) {
                //si la actividad se repite semanalmente, la propiedad dow indica en un array los días de la semana que los eventos se repiten y a que hora
                if (oficiales[i].dow != undefined) {
                    //revisa los días que se repiten los eventos. y separa la actividad programada para que no se traslapen
                    for (var d in oficiales[i].dow) {
                        //divide las actividades solo en los días que coincidan con fechas oficiales. se utiliza la funcion day() para obtener el día de la semana de la actividad, y se compara con el array del evento.
                        if (oficiales[i].dow[d] == moment(Activity.start).day() || oficiales[i].dow[d] == moment(Activity.end).day()) {

                            var inicioA = moment(getOnlyTime(inicioA), "HH:mm");
                            var finA = moment(getOnlyTime(finA), "HH:mm");
                            var iniciof = moment(oficiales[i].start, "HH:mm");
                            var finf = moment(oficiales[i].end, "HH:mm");

                            //si el día de la semana del dia festivo oficial coincide con la actividad programada, se comparan los horarios y se divide la actividad para que no se traslapen.
                            //          Inicio               Fin
                            //            [ActividadProgramada]
                            //     [Fecha Oficial]
                            //   Inicio         Fin
                            if ((inicioA >= iniciof && inicioA <= finf) &&
                                    (inicioA <= finf && finA > finf)) {
                                coincidence = true;
                                //modifica la Actividad para que no se traslape
                                Activity.start = moment(Activity.start, "YYYY-MM-DD HH:mm").set('hour', finf.get('hour')).set('minute', finf.get('minute')).add(1, 'm');
                                //valida que no se vuelva a traslapar
                                dividirTraslapes(Activity);
                                return;
                            }

                            //  Inicio               Fin
                            //    [ActividadProgramada]
                            //                 [Fecha Oficial]
                            //               Inicio         Fin

                            else if ((inicioA < iniciof && finA <= finf) &&
                                    (iniciof <= finA && finA <= finf)) {
                                coincidence = true;
                                //modifica la Actividad para que no se traslape
                                Activity.end = moment(Activity.end, "YYYY-MM-DD HH:mm").set('hour', iniciof.get('hour')).set('minute', iniciof.get('minute')).subtract(1, 'm');

                                //se valida que no se traslape
                                dividirTraslapes(Activity);
                                return;
                            }

                            //   Inicio               Fin
                            //     [ActividadProgramada]
                            //        [Fecha Oficial]
                            //      Inicio         Fin

                            else if ((inicioA <= iniciof && finA >= finf)) {
                                coincidence = true;
                                //modifica la Actividad para que no se traslape
                                eventbackup = copyEvent(Activity);
                                eventbackup.end = eventbackup.end = moment(Activity.end, "YYYY-MM-DD HH:mm").set('hour', iniciof.get('hour')).set('minute', iniciof.get('minute')).subtract(1, 'm');
                                eventbackup.idPeriodo = undefined;
                                dividirTraslapes(eventbackup);

                                //se modifica la otra parte de la actividad. y 

                                Activity.start = moment(Activity.start, "YYYY-MM-DD HH:mm").set('hour', finf.get('hour')).set('minute', finf.get('minute'));
                                //                                Activity.end = moment(Activity.end, "YYYY-MM-DD HH:mm").set('hour', iniciof.get('hour')).set('minute', iniciof.get('minute'));
                                //se valida que no se solape.
                                dividirTraslapes(Activity);
                                return;
                            }

                            //       Inicio               Fin
                            //         [ActividadProgramada]
                            //      [    Fecha     Oficial    ]
                            //    Inicio                     Fin

                            else if ((inicioA >= iniciof && finA <= finf)) {
                                coincidence = true;
                                //modifica la Actividad para que no se traslape
                                Activity.start = moment(Activity.start, "YYYY-MM-DD HH:mm").set('hour', finf.get('hour')).set('minute', finf.get('minute')).add(1, 'm');
                                Activity.end = moment(Activity.end).add(moment(Activity.end).diff(moment(Activity.start), 'minutes'), 'minutes');

                                //Se valida que no se traslape
                                dividirTraslapes(Activity);
                                return;
                            }

                        }
                    }

                }
                //La fecha oficial no se repite semanalmente, se procede a comparar fechas específicas y dividir las actividades para que no se traslapen con fechas oficiales no laborables.
                else {


                    var iniciof = moment(oficiales[i].start, "YYYY-MM-DD HH:mm");
                    var finf = moment(oficiales[i].end, "YYYY-MM-DD HH:mm");
                    //          Inicio               Fin
                    //            [ActividadProgramada]
                    //     [Fecha Oficial]
                    //   Inicio         Fin
                    if ((inicioA >= iniciof && inicioA <= finf) &&
                                    (inicioA <= finf && finA > finf)) {
                        coincidence = true;
                        //modifica la Actividad para que no se traslape
                        Activity.start = finf.add(1, 'm');
                        // se valida quen o se traslape
                        dividirTraslapes(Activity);
                        return;
                    }

                    //   Inicio               Fin
                    //     [ActividadProgramada]
                    //        [Fecha Oficial]
                    //      Inicio         Fin

                    else if ((inicioA <= iniciof && finA >= finf)) {
                        coincidence = true;
                        //modifica la Actividad para que no se traslape
                        eventbackup = copyEvent(Activity);
                        eventbackup.idPeriodo = undefined;
                        eventbackup.end = iniciof.subtract(1, 'm');
                        //dividedEvents.push(copyEvent(eventbackup));

                        // se crea la segunda parte

                        Activity.start = finf.add(1, 'm');

                        //se valida que no se traslape
                        dividirTraslapes(Activity);
                        Activity = null;
                        return;
                    }

                    //  Inicio               Fin
                    //    [ActividadProgramada]
                    //                 [Fecha Oficial]
                    //               Inicio         Fin

                    else if ((inicioA < iniciof && finA <= finf) &&
                                    (iniciof <= finA && finA <= finf)) {
                        coincidence = true;
                        //modifica la Actividad para que no se traslape
                        Activity.end = iniciof.subtract(1, 'm');

                        //se valida que no se traslape
                        dividirTraslapes(Activity);
                        return;
                    }

                    //       Inicio               Fin
                    //         [ActividadProgramada]
                    //      [    Fecha     Oficial    ]
                    //    Inicio                     Fin

                    else if ((inicioA >= iniciof && finA <= finf)) {
                        coincidence = true;
                        //modifica la Actividad para que no se traslape
                        Activity.start = finf.add(1, 'm');
                        Activity.end = moment(Activity.end).add(moment(Activity.end).diff(moment(Activity.start), 'minutes'), 'minutes'); ;

                        //se valida que no se traslape
                        dividirTraslapes(Activity);
                        return;
                    }

                }
            }
        }

    }
    if (coincidence == false) {

        if (Activity.id == undefined) {
            Activity.id = Contador++;

            if (Activity.idTr != undefined) {
                dividedEvents.push(Activity);

            }


            agregarAtcividadHTML(Activity);


        } else {

            for (var c = 0; c < dividedEvents.length; c++) {
                if (dividedEvents[c].id == Activity.id && dividedEvents[c].idTr == Activity.idTr) {
                    dividedEvents[c] = Activity;
                    $('tr[idjson="' + Activity.id + '"]').remove();


                    agregarAtcividadHTML(Activity);
                }
            }
        }
        RevisaTiemposAsociados();
        revisaNivelInfestacion();
    }


}



function separaCosechas(evento) {
    cosechasD = [];
    actDeshoje = [];
    actLimpieza = [];
    actPoda = [];
    for (var c = 0; c < dividedEvents.length; c++) {
        if (escosecha(dividedEvents[c].idHabilidad)) {
            cosechasD.push(dividedEvents[c]);
        }
        if (esdeshoje(dividedEvents[c].idHabilidad)) {
            actDeshoje.push(dividedEvents[c]);
    }
        if (eslimpieza(dividedEvents[c].idHabilidad)) {
            actLimpieza.push(dividedEvents[c]);
        }
        if (espodayvuelta(dividedEvents[c].idHabilidad)) {
            actPoda.push(dividedEvents[c]);
        }

    }

    var evtA;
    var evtB = evento;
    var esunico = (selectActivityPeriods(evento.idTr).length == 1);
    var cscs = [];

    for (var c = 0; c < cosechasD.length; c++) {
        if (cosechasD[c].start.day() == evento.start.day() && cosechasD[c].id != evento.id && (evento.editable == 'true' || evento.editable == true))
            cscs.push(cosechasD[c]);
    }
    if (cscs.length > 0) {
        for (var c in cscs) {
            evtA = cscs[c];
            if (evtA.idCiclo == evtB.idCiclo && evtA.id != evtB.id && evtA.idTr != evtB.idTr) {

                if (esunico) {
                    Eliminadas.push(evtB.idActividad);
                    $('table[idtr=' + evtB.idTr + ']').remove();
                    evtB.idTr = cscs[0].idTr;
                    evtB.idActividad = cscs[0].idActividad;
                    evtB.idPeriodo = undefined;
                    evtB.act = 'N';

                } else {
                    if (cscs[0].idActividad == undefined && evtb.idActividad != undefined) {
                        $('tr[idjson=' + evtB.id + ']').remove();
                        cscs[0].idTr = evtB.idTr;
                        cscs[0].idActividad = evtB.idActividad;
                        evtB.idPeriodo = undefined;
                        evtB.act = 'N';
                    } else {
                        $('tr[idjson=' + evtB.id + ']').remove();
                        evtB.idTr = cscs[0].idTr;
                        evtB.idActividad = cscs[0].idActividad;
                        evtB.idPeriodo = undefined;
                        evtB.act = 'N';
                    }
                }

            }
            else {
                if (esunico) {

                } else {
                    $('tr[idjson=' + evtB.id + ']').remove();
                }
            }

        }

    }
    else {
        if (esunico) {
            $('table[idtr=' + evtB.idTr + ']').remove();
        }
        else {

            $('tr[idjson=' + evtB.id + ']').remove();
            evtB.idTr = 'normal_' + evtB.id;
            evtB.idActividad = undefined;
            evtB.idPeriodo = undefined;
            evtB.act = 'N';
        }
    }
    agregarAtcividadHTML(evtB);

}

function collapseAsociados(idInvernadero, idTr, sender) {
    if (sender.attr('act') == "1") {
        $('table.programadas[idTr="' + idTr + '"]').find('tr[idInvernadero=' + idInvernadero + '] td.jornales ul').slideUp('fast', 'linear');
        sender.attr('act', "2");
    } else {
        $('table.programadas[idTr="' + idTr + '"]').find('tr[idInvernadero=' + idInvernadero + '] td.jornales ul').slideDown('fast', 'linear');
        sender.attr('act', "1");
    }

}

function agregarAtcividadHTML(Activity) {

    var header = '       <table class="programadas ' + (Activity.directriz == "true" || Activity.directriz == "1" ? 'Directriz' : '') + '"  idTr="' + Activity.idTr + '">                  ' +
                            '           <thead>                                                            ' +
                            '               <tr >                                                           ' +
                            '                   <th class="habilidadHead ' + (Activity.directriz == "true" || Activity.directriz == "1" ? 'Directriz' : '') + '" >Habilidad</th>                   ' +
                            '                   <th >Inicio</th>                                            ' +
                            '                   <th >Fin</th>                                               ' +
                            '                   <th >Surcos</th>                                            ' +
    //              '                   <th>Target</th>                                            ' +

                            '                   <th >Comentarios</th>                                   ' +
                            '                   <th >Tiempo <br/> Estimado</th>                                   ' +
                            '                   <th >Empleados <br/> Estimado</th>                                ' +
                            '                   <th style="cursor:pointer;  width:200px; " act="1" onClick="collapseAsociados(' + Activity.idInvernadero + ',\'' + Activity.idTr + '\',$(this));" >Asociados</th> ' +
                            '                   <th style="min-width:32px; background-color:' + Activity.backgroundColor + ' !important" class="' + (!Activity.editable ? "blocked " : Activity.enviado ? "sent" : Activity.idActividad != undefined ? "saved " : "editable ") + '">&nbsp;</th> ' +
                            '               </tr>                                                          ' +
                            '           </thead>                                                           ' +
                            '           <tbody>                                                            ' +
                            '           </tbody>                                                           ' +
                            '       </table>                                                               ' +

            '';



    var html = '<tr act = "' + Activity.act + '" idjson="' + Activity.id + '" idciclo="' + Activity.idCiclo + '" idActividad="' + Activity.idActividad + '" class="' + (Activity.directriz == "true" || Activity.directriz == "1" ? 'habilidadDeDirectriz' : 'normal') + '" idInvernadero="' + Activity.idInvernadero + '" contador="' + Activity.idTr + '" ><td>' + Activity.nombreHabilidad +
    (Activity.aceptaColmena && escosecha(Activity.idHabilidad) ? '  - <input type="checkbox" class="chkColmena" checked="' + (Activity.esColmena == '1' ? 'checked' : '') + '" id="chkColmena_' + Activity.id + '" idjson="' + Activity.id + '" /><label class="Colmena" for="chkColmena_' + Activity.id + '">Colmena? </label> ' : '') +
    '</br> ' + Activity.nombreEtapa + '</br>';

    if (Activity.numeroElementos != undefined && Activity.elemento != "") {
        html += '<select onchange="cambioelementos($(this));" class="elementos cajaChica switchA" idjson="' + Activity.id + '">';

        for (var e = 0; e < Activity.numeroElementos; e++) {
            html += '<option val="' + (e + 1) + '">' + (e + 1) + '</option>';
        }
        html += '</select>' + Activity.elemento;
    }
    else if (esPreparacion(Activity.idHabilidad)) {
        html += getTecnologias();
    }
    else if (escosecha(Activity.idHabilidad)) {
        html += '<input type="text" onchange="cambioelementos($(this));" class="elementos cajaChica switchA intValidate required" value="' + 0  + '" idjson="' + Activity.id + '"> cajas';
    }

    if (Activity.numeroElementos == undefined && Activity.elemento != "") {
        html += '<input type="text" onchange="cambioelementos($(this));" class="elementos cajaChica switchA intValidate" idjson="' + Activity.id + '">' + Activity.elemento;
    }

    html += '</td>' +                   //Fecha Inicio
                                        '<td class="switchA"><input type="text" class="switchA fechaInicio" value="' + moment(Activity.start).format("YYYY-MM-DD HH:mm") + '" /> ' +
                                        (escosecha(Activity.idHabilidad) && (Activity.aceptaColmena == true || Activity.aceptaColmena == 'True') ? ('<span style="display: none;" class="Colmena">Surco:</span>  <input style="display: none;" type="text"  class="Colmena switchA surcoInicio intValidate" value="' + Activity.surcoInicio + '" /> ') : '') + ' </td>' +
    //FechaFin
                                        '<td class="switchA"><input type="text" class="switchA fechaFin" value="' + moment(Activity.end).format("YYYY-MM-DD HH:mm") + '" /> ' +
                                        (escosecha(Activity.idHabilidad) && (Activity.aceptaColmena == true || Activity.aceptaColmena == 'True') ? ('<span style="display: none;" class="Colmena">Surco:</span> <input style="display: none;" type="text"  class="Colmena switchA surcoFin intValidate" value="' + Activity.surcoFin + '" /> ') : '') + ' </td>' +
    //Surcos
                                        '<td class="switchA"><input type="text" class="switchA surcos intValidate" value="' + Activity.surcos + '"/> ' + calculasurcos(Activity) + '</td>' +

    //                        '<td class="target">' + target + '</td>' +
    //'<td class="switchB comentario\"><textarea class="coment required"> ' + (Activity.comentario == undefined ? "" : Activity.comentario) + '</textarea></td>' +

                                        '<td class="switchA"><textarea class="switchA comentario"> ' + (Activity.comentario == undefined ? "" : Activity.comentario) + '</textarea></td>' +
                                        '<td class="switchA tiempoEstimado"> ' + calculatiempo(Activity) + '</td>' +
                                        '<td class="switchA jornalesEstimados">' + calculajornales(Activity) + '</td>' +
    //Jornales, AsociadosPeriodo trae a los asociados utilizados en el periodo corespondiente, idEtapa(identifica la etapa para agregar la eficiencia ), idjson, idTr(identifica elemento html)
                                        '<td class="switchA jornales"><span class="addJornal" idCiclo="' + Activity.idCiclo + '" idEtapa="' + Activity.idEtapa + '" idJson="' + Activity.id + '" idTr="' + Activity.idTr + '">+</span><h3>' + (Activity.Asociados == undefined ? '0' :
                                       Activity.Asociados.length) + '</h3><ul>' + (Activity.Asociados == undefined ? '' :
                                       AsociadosPeriodo(Activity)) +
                                        '</ul></td>' +
    //agrega el dropdown de razones de eliminacion de actividades de directriz
                                         '<td class="switchB razon"> ' + Activity.razonesDirectriz + '  </td>' +
    //comentario de Actividad eliminada de directriz
                                        '<td class="switchB comentario\"><textarea class="coment required"> ' + (Activity.comentario == undefined ? "" : Activity.comentario) + '</textarea></td>' +
    //boton para eliminar o no programar la actividad
                                        '<td class="switchA"><img class="hint btnEliminarActividad" src="../comun/img/remove.ico" title ="No progaramar esta Actividad"  onclick="eliminarHabilidadProgramada($(this).parent().parent());\" /></td>' +
    //boton para regresar una actividad no programada
                                        '<td class="switchB"><img class="hint" src="../comun/img/goback.png" title ="Programar esta actividad" onclick="RegresaTareaDirectriz($(this));\" /></td>';

    //si la actividad ya se muestra en pantalla, agrega una fila a la tabla correspondiente a la actividad, de lo contrario agrega la tabla.
    if ($('#divInv_' + Activity.idCiclo + ' div.accordionBody table.programadas[idTr="' + Activity.idTr + '"] ').length == 0) {
        $('#divInv_' + Activity.idCiclo + ' .accordionBody').prepend(header);
        $('#divInv_' + Activity.idCiclo + ' table.programadas[idTr="' + Activity.idTr + '"] tbody ').append(html);
    } else {
        $('#divInv_' + Activity.idCiclo + ' .programadas[idtr="' + Activity.idTr + '"] tbody ').append(html);
    }

    //si es actividad de directriz y no se programó, se cambia a no programadas
    if (Activity.programada == "0") {
        $('tr[idjson="' + Activity.id + '"]').find('td.switchA img').click();
        $('tr[idjson="' + Activity.id + '"]').find('td.switchB.razon select option[value="' + Activity.razon + '"]').attr('selected', true);
    } else {//do nothing
    }


    if (Activity.porTiempo == "True" || Activity.porTiempo == true) {
        $('tr[idjson="' + Activity.id + '"]').find('input.switchA.surcos').parent().html('Por Tiempo');
    } else {
        //do nothing
    }

    $('tr[idjson="' + Activity.id + '"]').find('select.elementos.cajaChica.switchA').val(Activity.cantidadElemento);

    $('tr[idjson="' + Activity.id + '"]').find('select.ddltecnologias.switchA').val(Activity.variable);

    $('tr[idjson="' + Activity.id + '"]').find('input, select').attr('disabled', !Activity.editable);

    //bloquea los input cuando ya hay captura de trabajo en esa actividad
    if (Activity.editable == false) {

        $('tr[idjson="' + Activity.id + '"]').find('.addJornal, .removeJornal, img.hint').remove();
    }

    if (esfumigacion(Activity.idHabilidad)) {
        $('tr[idjson="' + Activity.id + '"]').find('.fechaInicio, .fechaFin').attr('disabled', true);
    }

    if (Activity.idActividad == undefined && Activity.programada == 1) {
        EnviarP = false;
    }

    if (Activity.esColmena == '1') {
        $('tr[idjson="' + Activity.id + '"]').find('input[type=text].Colmena, span.Colmena').show('slow');
        $('tr[idjson="' + Activity.id + '"]').find('input.chkColmena').attr('checked', true);
    } else {
        $('tr[idjson="' + Activity.id + '"]').find('input[type=text].Colmena, span.Colmena').hide();
        $('tr[idjson="' + Activity.id + '"]').find('input.chkColmena').attr('checked', false);
    }

    $('#divInv_' + Activity.idCiclo).find('.loading').remove();

    if (banderaGlobalDeCargarInvernaderos) {
        // No desbloquear pantalla, hasta el ultimo elemento en iteracion

    }
    else {
        $.unblockUI();
    }
}

function calculajornales(Actividad) {

    if (Actividad.surcos > Actividad.surcosT) {
        $(this).addClass('Error');
        $('label#ActividadJornales').text('El invernadero solo tiene ' + Actividad.surcosT + ' surcos.');
    } else {
        $(this).removeClass('Error');
        jornales = ((Actividad.plantasPorSurco * Actividad.surcos) / Actividad.target) / dateDiff(Actividad.start, Actividad.end, 'h', true);

    }
    try {
        jornales = jornales > 0 ? jornales : 0;
    } catch (Ex) {
        jornales = 0;
    }
    if (Actividad.porTiempo == "True" || Actividad.porTiempo == true) {
        return 'Por Tiempo.'
    }
    return Math.ceil(jornales);
}

function calculatiempo(event) {
    var result = "";
    var tiempo = 0.0;
    var eficiencia = 0.0;
    var plantas = event.plantasPorSurco * event.surcos;

    var eficiencia = eficienciaAsociadosPeriodo(event);

    if (event.porTiempo == "True" || event.porTiempo == true) {
        tiempo = dateDiff(event.start, event.end, 'h', true);
    } else {
        tiempo = plantas / eficiencia;

        tiempo = tiempo != Infinity ? tiempo : 0;
    }

    result = (tiempo.toFixed(2)) + " Horas </br>" + (tiempo / 8).toFixed(2) + " Dias"

    return result;
}

function eficienciaAsociadosPeriodo(event) {
    var eficiencia = 0.0;

    for (var a in event.Asociados) {
        eficiencia += parseInt(event.Asociados[a].eficiencia != undefined ? event.Asociados[a].eficiencia : 0);
    }

    return eficiencia;
}

function mostrarCalendario(imgClicked) {
    if ($('input[type="text"].Error').length) {
        popUpAlert('No es posible mostrar el calendario, por favor revise que las fechas capturadas sean correctas.', 'info');
    }
    else {
        $('#popUpCalendario').show().animate({ top: ($(window).scrollTop() + 50) + 'px' });

        crearCalendario(dividedEvents);
        $('#calendar').fullCalendar('gotoDate', rangosemana[0].StartDate);

        filterbySite();
    }
}


Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
}

function cambioelementos(sender) {
    var tr = sender.parent().parent();
    var Activity = select($(tr).attr('idjson'), $(tr).attr('contador'));
    Activity.cantidadElemento = parseInt($(sender).val());
    $('tr[contador="' + Activity.idTr + '"]').find('select.elementos').val($(sender).val());
}

function AsociadosPeriodo(activity) {
    var asociados = '';
    if (activity.Asociados != undefined) {
        if (activity.Asociados.length > 0)
            for (var of = 0; of < activity.Asociados.length; of++) {
                asociados += "<li number=" + activity.Asociados[of].idAsociado + "><span class='nombreasociado'>" + activity.Asociados[of].idAsociado + " - " + activity.Asociados[of].Nombre + "</span><span idJson='" + activity.id + "' number='" + activity.Asociados[of].idAsociado + "' class='removeJornal'>x</span>";
            }
    }
    return asociados;
}

function select(idJson, idTr) {
    var Actividad;
    dividedEvents.filter(function (i, n) {
        if (i.id == idJson && i.idTr == idTr) {
            Actividad = i;
            return;
        }
    });
    return Actividad;
}

function selectNoProgramada(idJson, idTr) {
    var Actividad;
    DirectrizNoP.filter(function (i, n) {
        if (i.id == idJson && i.idTr == idTr) {
            Actividad = i;
            return;
        }
    });
    return Actividad;
}

function cargaFamiliasAsociados() {
    for (var a in Familias) {
        $('div#popUpAsociados div#asociadosHead').append('<div class="tab" > <span idFamilia="' + Familias[a].idFamilia + ' ">' + Familias[a].vFamilia + ' </span></div>');
    }
}

function AsociadosFamilia(idEtapa) {
    var result = '<ul>';

    for (var ja = 0; ja < JsonAsociados.length; ja++) {
        if (JsonAsociados[ja].IdEtapa == idEtapa) {
            result += '<li class="asociadoAgregar" nombre="' + JsonAsociados[ja].NOMBRE + '" target="' + JsonAsociados[ja].eficiencia + '" idFamilia="' + JsonAsociados[ja].idFamilia + '" idAsociado="' + JsonAsociados[ja].ID_EMPLEADO + '">' + JsonAsociados[ja].ID_EMPLEADO + '-' + JsonAsociados[ja].NOMBRE + '</li>';
        }
    }
    return result;
}

function removeSelectedJonal(id) {
    for (var j = 0; j < dividedEvents.length; j++) {
        if (dividedEvents[j].id == id) {
            if (dividedEvents[j].Asociados != undefined) {
                for (var as = 0; as < dividedEvents[j].Asociados.length; as++) {
                    $('div#asociadosData li[idAsociado="' + dividedEvents[j].Asociados[as].idAsociado + '"]').addClass('added');
                    $('div#EquiposData li[idAsociado="' + dividedEvents[j].Asociados[as].idAsociado + '"]').addClass('added');
                }
            }
            return;
        }
    }
}



function selectActivityPeriods(idTr) {
    periods = [];
    for (var a = 0; a < dividedEvents.length; a++) {
        if (dividedEvents[a].idTr == idTr) {
            periods.push(dividedEvents[a]);
        }
    }
    return periods;
}

function selectInvernadero(id, idTr) {

    var element = select(id, idTr);

    var invernadero = element.idInvernadero;
    periods = [];
    for (var a = 0; a < dividedEvents.length; a++) {
        if (dividedEvents[a].idInvernadero == invernadero) {
            periods.push(dividedEvents[a]);
        }
    }
    return periods;
}

$(function () {

    $('#avisoAletraConfiguracion').hide();
    $('#popupDetalleConfiguracionInvernadero').hide();
    $('#popupConfiguraCopiaProgramacion').hide();

    
    $('#btnDetalleConfiguracionInvernadero').click(function () {
        console.log("click");
        //popUpTablaInvernaderos();
        //  $('#popupDetalleConfiguracionInvernadero').show();
        $('#popupDetalleConfiguracionInvernadero').show().animate({ top: ($(window).scrollTop() + 120) + 'px' });
    });
    $('#btnCopiarProgramaConfiguracion').click(function () {
        console.log("click");
        //popUpTablaInvernaderos();
        //  $('#popupDetalleConfiguracionInvernadero').show();
        $('#popupConfiguraCopiaProgramacion').show().animate({ top: ($(window).scrollTop() + 50) + 'px' });
        
    });


    $('div.ajusteHora').find('input.txtHoraInicio').val(calendarioInicio);
    $('div.ajusteHora').find('input.txtHoraFin').val(calendarioFin);

    semanaActual = parseInt($('span#ctl00_ltSemana').text());
    $('#txtSemanaCalendario').val(semanaActual);
    $('#txtAnioCalendario').val(new Date().getFullYear());
    PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
        rangosemana = JSON.parse(response);
        rangosemana[0].EndDate += 86340000;
    },
            function (onfail) {
                popUpAlert('Error de conexion, No se pudo obtener los datos de la semana, intente de nuevo.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
    //ARTURO
    //PageMethods.AusenciasAsociados(semanaActual, parseInt($('#txtAnioCalendario').val()), function (response) {
    //    Ausencias = JSON.parse(response);
    //},
    //        function (onfail) {
    //            popUpAlert('Error de conexion, No se pudo obtener Las ausencias de Asociados, recargue la pagina.', 'warning');
    //            console.log(onfail);
    //            $.unblockUI();
    //        });
    registerControls();
    cargaInvernaderos();
    
    $('#ctl00_ddlPlanta').live('change', function () {
        $('.slick-slide').html('');
        $('#rollslider2').html('');
        $('.slick-slide').css({ 'background-image': 'url("../comun/img/ajax-loader.gif")',
            'background-repeat': 'no-repeat',
            'background-position': 'center'
        });

        //ARTURO
        NombreYJornalesAutorizados();
        obtieneInvernaderosProgramadosJS();
        obtieneSemanaAnioJS();
        obtieneInvernaderosSINProgramadosJS();
        cargaInvernaderos();
        

    });

});

$('input.btnAddHora').live('click', function () {
    var txt = $(this).parent().parent().parent().find('input[type="text"]');
    var hour = moment(txt.val(), "HH:mm");
    hour.add(1, 'hours');
    txt.val(hour.format("HH:mm"));
    txt.change();
});

$('input.btnRemHora').live('click', function () {
    var txt = $(this).parent().parent().parent().find('input[type="text"]');
    var hour = moment(txt.val(), "HH:mm");
    hour.subtract(1, 'hours');
    txt.val(hour.format("HH:mm"));
    txt.change();
});

$('input.txtHoraInicio').live('change', function () {
    calendarioInicio = $(this).val();
    if (moment(calendarioInicio, "HH:mm").isAfter(moment(jornadaInicio, "HH:mm"))) {
        $(this).val(jornadaInicio);
    }
    $('#calendar').fullCalendar('destroy');
    crearCalendario();
    filterbySite();
});

$('input.txtHoraFin').live('change', function () {
    calendarioFin = $(this).val();
    if (moment(calendarioFin, "HH:mm").isBefore(moment(jornadaFin, "HH:mm"))) {
        $(this).val(jornadaFin);
    }
    $('#calendar').fullCalendar('destroy');
    crearCalendario();
    filterbySite();
});

$('input.selectObject').live('change', function () {
    $('#popUpActividad').hide();
});

$('div.fc-time span').live('click', function () {
    $(this).prev().click();
});

$('div#EquiposHead div.tab:has(span.tabEquipos)').live('click', function () {
    $('div#asociadosHead, div#asociadosData').hide();
    $('div#EquiposData').show();
    $('div#EquiposHead div.tab').removeClass('selected');
    $(this).addClass("selected");
});

$('div#EquiposHead div.tab:has(span.tabFamilias)').live('click', function () {
    $('div#EquiposData').hide();
    $('div#asociadosHead, div#asociadosData').show();
    $('div#EquiposHead div.tab').removeClass('selected');
    $(this).addClass("selected");
});

$('span.addJornal').live('click', function () {
    $('div#asociadosData').html('').addClass('loading');
    $('div#popUpAsociados').attr('idjson', $(this).attr('idjson')).attr('idEtapa', $(this).attr('idetapa')).attr('idTr', $(this).attr('idtr'));
    var id = $(this).attr('idjson');
    var result = "<ul>";
    var equipos = "<ul>";
    PageMethods.cargaAsociadosFamiliasEficiencia($(this).attr('idCiclo'), $(this).attr('idEtapa'), function (response) {
        var asociados = JSON.parse(response);
        for (var ja = 0; ja < asociados.length; ja++) {

            result += '<li ' + (asociados[ja].ID_EMPLEADO > 0 ? 'class="asociadoAgregar"' : '') + ' nombre="' + asociados[ja].NOMBRE + '" target="' + asociados[ja].eficiencia + '" idFamilia="' + asociados[ja].idFamilia + '" idAsociado="' + asociados[ja].ID_EMPLEADO + '">' + asociados[ja].ID_EMPLEADO + '-' + asociados[ja].NOMBRE + '</li>';

        }

        $('div#asociadosData').html(result + "</ul>").removeClass('loading');
        removeSelectedJonal(id);

    },
            function (onfail) {
                popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });

    PageMethods.cargaEquiposTrabajo($(this).attr('idciclo'), $(this).attr('idEtapa'), function (response) {
        var Equipos = JSON.parse(response);
        for (var e in Equipos) {
            equipos += '<li class="lEquipo"><div class="NombreEquipo" idEquipo="' + Equipos[e].idEquipoTrabajo + '" ><span>' + Equipos[e].NombreEquipo +'['+Equipos[e].Asociados.length +']</span><span class="equipoDropdown down">▼</span></div><div style="display: none;" class="listEquipos"  idEquipo="' + Equipos[e].idEquipoTrabajo + '" ><ul class="lEquipoAsoc" idEquipo="' + Equipos[e].idEquipoTrabajo + '" > ';
            for (var b in Equipos[e].Asociados) {
                var a = Equipos[e].Asociados[b];
                equipos += '<li ' + (a.idEmpleadoAsociado > 0 ? 'class="asociadoAgregar"' : '') + 'nombre ="' + a.NombreAsociado + '" target="' + a.Eficiencia + '" idAsociado="' + a.idEmpleadoAsociado + '">' + a.idEmpleadoAsociado + '-' + a.NombreAsociado + '</li>';
            }
            equipos += '</ul></div></li>';
        }
        equipos += '</ul>';

        $('div#EquiposData').html(equipos);
        $('ul.lEquipoAsoc').slideUp();
    });


    $('div#popUpAsociados').show();
    $('div#popUpAsociados .tab').removeClass('selected');
});

$('ul li.asociadoAgregar:not(.added)').live('click', function () {
    activities = [];
    var id = $('div#popUpAsociados').attr('idjson');
    $('ul li.asociadoAgregar[idasociado="' + $(this).attr('idasociado') + '"]').addClass('added');
    var tr = $('div#popUpAsociados').attr('idtr');

    if ($('input#asociadosInvernaderoAplicar').attr('checked') == 'checked') {
        activities = selectInvernadero(id, tr);
    } else if ($('input#asociadosActividadAplicar').attr('checked') == 'checked') {
        activities = selectActivityPeriods(tr);
    }
    else {
        activities.push(select(id, tr));
    }
    for (var a = 0; a < activities.length; a++) {
        if (activities[a].Asociados == undefined) {
            activities[a].Asociados = [];
        }

        //Agrega asociados al json de la actividad
        if (!existeAsociadoEnPeriodo($(this).attr('idasociado'), activities[a].Asociados) && activities[a].editable) {
            activities[a].Asociados.push({ "Nombre": $(this).attr('nombre'), "idAsociado": $(this).attr('idasociado'), "eficiencia": $(this).attr('target') });
        } else {
            //do nothing
        }

        //Listado de asociados en el periodo
        $('#ActivityData').find('ul#Asociados').html('').append(AsociadosPeriodo(activities[a]));
        $('tr[idjson="' + activities[a].id + '"]').find('td.jornales ul').html(AsociadosPeriodo(activities[a]));

        //cantidad de asociados en el periodo
        $('tr[idjson="' + activities[a].id + '"]').find('td.jornales h3').text(activities[a].Asociados.length);

        //Calculos de tiempo y jornales requeridos
        $('tr[idjson="' + activities[a].id + '"]').find('.tiempoEstimado').html(calculatiempo(activities[a]));
        $('tr[idjson="' + activities[a].id + '"]').find('.jornalesEstimados').html(calculajornales(activities[a]));

        RevisaTiemposAsociados();
        revisaNivelInfestacion();

    }


});

$('div.NombreEquipo span.equipoDropdown.down').live('click', function () {
    $(this).parent().next().slideDown();
    $(this).removeClass('down').addClass('up');
    $(this).text("▲");
});

$('div.NombreEquipo span.equipoDropdown.up').live('click', function () {
    $(this).parent().next().slideUp();
    $(this).removeClass('up').addClass('down');
    $(this).text("▼");

});

$('div.NombreEquipo span:not(.equipoDropdown)').live('click', function () {
   // $.blockUI();
    var idequipo = $(this).attr('idequipo');
    $(this).parent().next().find('li').click();
    //$.unblockUI();

});


$('ul li.asociadoAgregar.added').live('click', function () {
    activities = [];
    var id = $('div#popUpAsociados').attr('idjson');

    var tr = $('div#popUpAsociados').attr('idtr');

    if ($('input#asociadosInvernaderoAplicar').attr('checked') == 'checked') {
        activities = selectInvernadero(id, tr);
    } else if ($('input#asociadosActividadAplicar').attr('checked') == 'checked') {
        activities = selectActivityPeriods(tr);
    }
    else {
        activities.push(select(id, tr));
    }
    for (var a = 0; a < activities.length; a++) {
        if (activities[a].Asociados == undefined) {
            activities[a].Asociados = [];
        }

        //Agrega asociados al json de la actividad
        if (existeAsociadoEnPeriodo($(this).attr('idasociado'), activities[a].Asociados) && activities[a].editable) {
            for (var asociado = 0; asociado < activities[a].Asociados.length; asociado++) {
                if (activities[a].Asociados[asociado].idAsociado == $(this).attr('idasociado')) {
                    console.log(activities[a].Asociados[asociado].idAsociado);
                    activities[a].Asociados.splice(asociado, 1);
                }
            }

        } else {
            //do nothing
        }

        $('ul li.asociadoAgregar[idasociado="' + $(this).attr('idasociado') + '"]').removeClass('added');

        //Listado de asociados en el periodo
        $('#ActivityData').find('ul#Asociados').html('').append(AsociadosPeriodo(activities[a]));
        $('tr[idjson="' + activities[a].id + '"]').find('td.jornales ul').html(AsociadosPeriodo(activities[a]));

        //cantidad de asociados en el periodo
        $('tr[idjson="' + activities[a].id + '"]').find('td.jornales h3').text(activities[a].Asociados.length);

        //Calculos de tiempo y jornales requeridos
        $('tr[idjson="' + activities[a].id + '"]').find('.tiempoEstimado').html(calculatiempo(activities[a]));
        $('tr[idjson="' + activities[a].id + '"]').find('.jornalesEstimados').html(calculajornales(activities[a]));

        RevisaTiemposAsociados();
        revisaNivelInfestacion();
        
    }


});

$('input#BuscaAsociados').live('keyup', function () {
    if ($(this).val() == "") {
        $('#popUpAsociados').find('ul li').show();
    } else {
        $('#popUpAsociados').find('ul li').hide();
        $('#popUpAsociados').find('ul li.asociadoAgregar[nombre*="' + $(this).val() + '"]').show();
        $('#popUpAsociados').find('ul li.asociadoAgregar[idasociado*="' + $(this).val() + '"]').show();
    }

});

$('div#asociadosHead div.tab').live('click', function () {
    $('div#asociadosHead div.tab').removeClass('selected');
    $(this).addClass('selected');
    $('div#asociadosData li').hide();
    $('div#asociadosData li[idFamilia=' + $(this).find('span').attr('idFamilia') + ']').show();
});

$('input#ActividadSurcos').live('keyup', function () {
    var idJson = $('div#ActivityData').attr('idjson');
    var idTr = $('div#ActivityData').attr('idtr');
    var Actividad = select(idJson, idTr);
    var jornales = 0;
    var surcos = $(this).val();

    if (surcos > Actividad.surcosT) {
        $(this).addClass('Error');
        $('label#ActividadJornales').text('El invernadero solo tiene ' + Actividad.surcosT + ' surcos.');
    } else {
        $(this).removeClass('Error');
        jornales = ((Actividad.plantasPorSurco * surcos) / Actividad.target) / dateDiff(Actividad.start, Actividad.end, 'h', true);

        Math.round(jornales);

        $('label#ActividadJornales').text(Math.ceil(jornales));
    }
});

$('input#modificaActividad').live('click', function () {
    var idJson = $('div#ActivityData').attr('idjson');
    var idTr = $('div#ActivityData').attr('idtr');
    var Actividad = select(idJson, idTr);
    try {
        if (parseInt($('div#ActivityData input#ActividadSurcos').val()) > Actividad.surcosT) {
            //nothing
        }
        else {
            Actividad.surcos = $('div#ActivityData input#ActividadSurcos').val();
            $('#calendar').fullCalendar('refetchEvents');
            filterbySite();
            $('tr[idjson="' + Actividad.id + '"]').find('input.surcos').val(Actividad.surcos);
            $('#popUpActividad').hide();
        }
    } catch (e) {

    }


});

$('span.removeJornal').live('click', function () {
    var obj = $(this);
    var id = obj.attr('idjson');
    var idAsociado = obj.attr('number');
    var count = 0;
    var Activity;
    dividedEvents.filter(function (i, n) {
        if (i.id == id) {
            Activity = i;
            if (Activity.editable) {
                for (var asociado = 0; asociado < i.Asociados.length; asociado++) {
                    if (i.Asociados[asociado].idAsociado == idAsociado) {
                        i.Asociados.splice(asociado, 1);
                    }
                }
                $('tr[idjson="' + id + '"]').find('td.jornales ul').html(AsociadosPeriodo(i));
                count = i.Asociados.length;
            }
        }
    });
    if (Activity.Asociados != undefined) {
        $('#ActivityData').find('span#ActividadSurcosEstimados').html(calculasurcos(Activity));
        $('tr[idjson="' + Activity.id + '"]').find('.tiempoEstimado').html(calculatiempo(Activity));
        $('tr[idjson="' + Activity.id + '"]').find('.jornalesEstimados').html(calculajornales(Activity));
    }
    $('tr[idjson="' + id + '"]').find('td.jornales h3').text(count);
    obj.parent().remove();
    RevisaTiemposAsociados();
});

$('#txtSemanaCalendario').live('change', function () {
    PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
        rangosemana = JSON.parse(response);
        rangosemana[0].EndDate += 86340000;
    },
            function (onfail) {
                popUpAlert('Error de conexion, No se Pudo obtener los datos de la semana.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
});

$('div#divTareasProgramadas').find('h3').live('click', function () {
    $(this).next().slideToggle('fast', 'linear');
});

function existeAsociadoEnPeriodo(asociado, asociados) {
    for (var a in asociados) {
        if (asociados[a].idAsociado == asociado) {
            return true;
        }
    }
    return false;
}



function popUpTablaInvernaderos() {
    $('#avisoAletraConfiguracion').hide();
    $('#popupDetalleConfiguracionInvernadero').hide();
    var semanaActual = $('#txtSemanaCalendario').val(); anioActual = $('#txtAnioCalendario').val();
    PageMethods.buscarInvernaderosMalConfigurados(semanaActual, anioActual, function (response) {
        if (response[0] == '1') {
            var objJSON = JSON.parse(response[2]);
            if (!objJSON.hasOwnProperty('NoData')) {
                crearTabla(objJSON);
                $('#avisoAletraConfiguracion').show();
            }
            //else sin acciones
        } else {
            //sin acciones
        }


    });
}

function crearTabla(objJSON) {
    var Invernaderos = objJSON.Planta.Invernaderos;
    html = '<table class=\"gridView\" id=\"tblDetalleConfiguracionInvernadero\">';
    html += '<thead>';
    html += '<tr>';
    html += '<th>Invernadero</th><th>Variedad</th><th>Variable</th><th>Fecha de Plantacion</th><th>Edad</th><th>Completo</th><th>Ciclo Activo</th>';
    html += '<tr>';
    html += '</thead>';
    html += '<tbody>';
    for (var index in Invernaderos) {
        var idInvernadero = Invernaderos[index].Invernadero.IdInvernadero;
        var NombreCorto = Invernaderos[index].Invernadero.ClaveInvernadero;
        var Ciclos = Invernaderos[index].Invernadero.Ciclos;
        var cicloactivo = Invernaderos[index].Invernadero.cicloactivo;

        var bandera = 0;
        if (Ciclos != undefined) {
            var rowSpan = Invernaderos[index].Invernadero.Ciclos.length;
            for (var index in Ciclos) {
                var variedad = Ciclos[index].Ciclo.Variety;
                var variedadExiste = Ciclos[index].Ciclo.variedadExiste;
                var variable = Ciclos[index].Ciclo.variable;
                var idCycle = Ciclos[index].Ciclo.idCycle;
                var fechaPlantacion = Ciclos[index].Ciclo.PlantDate;
                var edad = Ciclos[index].Ciclo.edad;
                var completo = Ciclos[index].Ciclo.completo;

                html += '<tr>';
                bandera++;
                html += (bandera == 1 ? '<td rowspan=\"' + rowSpan + '\" class=\"tdinvernadero\" id=\"' + idInvernadero + '\">' + NombreCorto + '</td>' : '');
                html += '<td class=\"tdvariedad\">' + variedad + (variedadExiste == 1 ? '<img id="Existe" src=\"../comun/img/ok.png\">' : '<img id="Existe" src=\"../comun/img/error.png\">') + '</td>';
                html += '<td class=\"tdvariable\">' + variable + '</td>';
                html += '<td class=\"tdfechaPlantacion\">' + fechaPlantacion + '</td>';
                html += '<td class=\"tdedad\">' + edad + '</td>';
                html += '<td class=\"tdedad\">' + completo + '</td>';
                html += '<td class="ciclo" >' + cicloactivo + '</td>';
                html += '</tr>';
            }
        }
        else if (cicloactivo == '-NO-') {
            html += '<tr><td>' + NombreCorto + '</td><td colspan = 6>No Tiene Ciclo Activo</td>';
        }
    }
    html += '</tbody>';
    html += '</table>';

    //    html += '<span>*NOTA: Si estas configuraciones no son correctas reportelas a su gerente.</span>';
    //    html += '<div>';
    //    html += '<input type="button" value="Cerrar" id=\"btnCerrarPopUp\" />';
    //    html += '</div>';

    $('#contenidoDetalleConfiguracionInvernadero').html(html);

    //    $('#btnCerrarPopUp').click(function () {
    //        var popUp = $('#popupDetalleConfiguracionInvernadero');
    //        if (!popUp.is(':hidden')) {
    //            popUp.hide();
    //        }
    //    });
}





function cargaInvernaderos() {
    //$.blockUI();
    PageMethods.cargaInvernaderosSlider(function (response) {
        $('#rollslider').removeClass();
        $('.invernaderos #rollslider').html(response);
        setInvernaderos();
      //  $.unblockUI();
    },
            function (onfail) {
                popUpAlert('Error de conexion, No se pudo obtener la lista de invernaderos, intente de nuevo.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });

    popUpTablaInvernaderos();
    // $('#avisoAletraConfiguracion').hide();

}
function setInvernaderos() { //Inicializa los controles Slider en los que se muestran las plantas
    $('#rollslider').slick({
        slidesToShow: $('#rollslider div').length < 12 ? $(this).length : 12,
        slidesToScroll: $('#rollslider div').length > 12 ? 5 : 2,
        infinite: false,
        draggable: false,
        variableWidth: true
    });

    $('.divInvernadero ').mousedown(function (event) {
        if (event.button == 0)
            $(this).addClass('clicked');

    });



    $('.divInvernadero ').mouseup(function (event) {
        try {
            if (event.button == 0) {
                banderaGlobalDeCargarInvernaderos = false;
            }
            if (event.button == 0 || event.button == undefined) {
               // $.blockUI();
                var invernaderoID = $(this).attr('ID');
                var idCiclo = $(this).attr('idCiclo');
                var claveInvernadero = $(this).attr('invernadero');
                var densidad = $(this).attr('densidad');
                var Elemento = $(this).attr('elemento');
                var cantidadElementos = $(this).attr('cantidadElementos');
                var producto = $(this).attr('product');
                var invernadero = $(this).text();
                var surcos = $(this).attr('surcos');
                var infoInvernadero = $(this).text() + ' - ' + producto + ' - Edad: ' + (moment(rangosemana[0].EndDate).diff(moment($(this).attr('fechaplantacion')), 'weeks')) + ' surcos:' + surcos;
                var esInterplanting = $(this).attr('esInterplanting');
                var cortes = $(this).attr('cortes');
                var fechaPlantacion = $(this).attr('fechaplantacion');

                removefromjson(idCiclo);

                if ($(this).attr('class').indexOf('selected') > -1) {
                    $(this).removeClass('selected');
                    $(this).attr('selected', false);
                    $('#divInv_' + idCiclo).remove();
                    $('.divHabilidadProgramable .chk_' + idCiclo).remove();
                    removefromjson(idCiclo);
                    if (banderaGlobalDeCargarInvernaderos) {
                    } else {
                        $.unblockUI();
                    }
                }
                else {
                    $(this).addClass('selected');
                    $(this).attr('selected', true);
                    $('.divHabilidadProgramable').each(function (index, element) {
                        $(this).append('<span class="chk_' + idCiclo + '"  ><input type="checkbox" corte="' + cortes + '" id="chk_' + invernaderoID + '_' + index + '" esInterplanting="' + esInterplanting + '" densidad="' + densidad + '" elemento="' + Elemento + '" cantidadElementos="' + cantidadElementos + '" surcos="' + surcos + '" idCiclo="' + idCiclo + '" product="' + producto + '" invernadero="' + invernadero + '" ><label for="chk_' + invernaderoID + '_' + index + '">' + claveInvernadero + '</label></span>');
                    });
                    if ($('#divInv_' + idCiclo).length > 0) {
                        $('#divInv_' + idCiclo).show();

                    }
                    else {

                        if (surcos == "0") {
                            popUpAlert('El Invernadero ' + invernadero + ' no tiene surcos ni secciones configuradas.</br> No se permitirá realizar la programación Semanal. </br> contacte al Administrador para configurar el invernadero.', 'warning');
                            $.unblockUI();
                        } else {
                            $('#divTareasProgramadas').append(tablaDeActividadesPorInvernadero(invernaderoID, idCiclo, infoInvernadero));
                            $('#divInv_' + idCiclo).show();

                            PageMethods.FechasOficiales(idCiclo, invernaderoID, 4, function (response) {
                                oficiales = oficiales.concat(JSON.parse(response));
                                cargaHabilidadesDirectriz($('#divInv_' + idCiclo), invernaderoID, fechaPlantacion, idCiclo);
                                EnviarP = true;
                            },
            function (onfail) {
                popUpAlert('Error de conexion, No se pudo cargar las actividades del invernadero, Intente de nuevo.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
                        }
                    }
                }

                $(this).removeClass('clicked');
                $('div.filtroPlantas').html(function () {
                    var htm = '';
                    $('div.divInvernadero.selected').each(function () {
                        htm += '<input type="checkbox" idCiclo="' + $(this).attr('idciclo') + '" id="id_' + $(this).attr('idciclo') + '" class="filtroPlantas" value="' + $(this).attr('id') + '" checked/><label for="id_' + $(this).attr('idciclo') + '" >' + $(this).text() + '</label> ';
                    });
                    return htm;
                });

            }
        } catch (e) {
            console.log(e);
            $.unblockUI();
        }
        finally {
            if (banderaGlobalDeCargarInvernaderos) {
                // No desbloquear pantalla, hasta el ultimo elemento en iteracion
            }
            else {
                //                
            }
        }
    });

    $('.switchB textarea.coment').live('change', function () {
        var tr = $(this).parent().parent();
        $(this).removeClass('Error');
        var actividad = selectNoProgramada(tr.attr('idjson'), tr.attr('contador'));
        actividad.comentario = $(this).val();
    });

    $('.switchB select.ddlRazon').live('change', function () {
        var tr = $(this).parent().parent();
        var actividad = selectNoProgramada(tr.attr('idjson'), tr.attr('contador'));
        $(this).removeClass('Error');
        actividad.razon = $(this).val();
    });

    $('select.ddltecnologias.switchA').live('change', function () {
        var tr = $(this).parent().parent();
        var actividad = select(tr.attr('idjson'), tr.attr('contador'));
        actividad.variable = $(this).val();
    });
    $('div.divInvernadero[id]').attr('title', '');
    for (var i in Infestaciones) {
        var title = "Infestacion: ";

        if (Infestaciones[i].nombreComun != undefined) {

            title += $('div.divInvernadero[id*="' + Infestaciones[i].idInvernadero + '"]').attr('title') + ' ' + Infestaciones[i].nombreComun + ' nivel: ' + Infestaciones[i].nivelInfestacion;
            $('div.divInvernadero[id*="' + Infestaciones[i].idInvernadero + '"]').addClass('Infested').attr('title', title);
        }
    }
}

//quita del calendario y de memoria actividades y fechas oficiales de un invernadero deseleccionado
function removefromjson(idInv) {
    var cont = 0;
    var cont1 = 0;
    var cont2 = 0;

    for (var of = 0; of < oficiales.length; of = cont) {
        if (oficiales[cont].idCiclo == idInv) {
            oficiales.splice(cont, 1);
        }
        else {
            cont++;
        }
    }

    for (var of = 0; of < dividedEvents.length; of = cont1) {
        if (dividedEvents[cont1].idCiclo == idInv) {
            dividedEvents.splice(cont1, 1);
        }
        else {
            cont1++;
        }
    }

    for (var of = 0; of < DirectrizNoP.length; of = cont2) {
        if (DirectrizNoP[cont2].idCiclo == idInv) {
            DirectrizNoP.splice(cont2, 1);
        }
        else {
            cont2++;
        }
    }
}

function cargaHabilidadesDirectriz(divActividadesPorInv, idInvernadero, fechaPlantacion, idciclo) {
    var semana = $('#txtSemanaCalendario').val();
    var anio = $('#txtAnioCalendario').val();

    PageMethods.cargaHabilidadesDirectriz(idInvernadero, fechaPlantacion, semana, anio, idciclo, function (response) {
        $(divActividadesPorInv).find('.programadas tbody').append(ActividadTabla(response));
        $(divActividadesPorInv).find('.trLoad').hide();
        $(divActividadesPorInv).find('h3').css('cursor', 'pointer');
        if (response.length == 0) {
            $('td.loading[cic="' + idciclo + '"]').remove();

        }
        if (banderaGlobalDeCargarInvernaderos) {
            // No desbloquear pantalla, hasta el ultimo elemento en iteracion
            currentinv++;
            if (cantidadInvernaderos > currentinv) {
                banderaGlobalDeCargarInvernaderos = false;
            }
        }
        else {
            $.unblockUI();
        }

        //ARTURO
        //PageMethods.cargaFumigaciones(semanaActual, idInvernadero, anio, function (response) {
        //    $(divActividadesPorInv).find('.programadas tbody').append(ActividadTabla(response));

        //},
        //function (error) {
        //    console.log(error);
        //    popUpAlert('Ocurrió un error al traer las fumigaciones, intenta de nuevo.', 'info');
        //    $.unblockUI();
        //}
        //);
    },
            function (onfail) {
                popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
}

function ActividadTabla(response) {
    if (response.length > 0) {
        var actividadesInvernadero = JSON.parse(response);


        for (var actividad in actividadesInvernadero) {
            actividadesInvernadero[actividad].textColor = invert(actividadesInvernadero[actividad].backgroundColor);
            if (actividadesInvernadero[actividad].programada == 1) {

            }
            else {
                actividadesInvernadero[actividad].idActividadNoP = actividadesInvernadero[actividad].idActividad;
                actividadesInvernadero[actividad].idActividad = undefined;
                actividadesInvernadero[actividad].idPeriodo = undefined;

            }
            change(copyEvent(actividadesInvernadero[actividad]));
        }
    }
    else {
    }
}

function reload() {
    var InvernaderoID;
    dividedEvents = [];
    DirectrizNoP = [];
    Eliminadas = [];
    cosechasD = [];
    $('.divInvernadero.selected').each(function () {
        InvernaderoID = $(this).attr('idciclo');
        $('span.chk_' + InvernaderoID).remove();
        $('#divInv_' + InvernaderoID).remove();
        oficiales = [];
    });

    $('.divInvernadero.selected').each(function () {
        $(this).removeClass('selected');
        $(this).mouseup();
    });
    //obtieneInvernaderosProgramadosJS();
    //obtieneInvernaderosSINProgramadosJS();

}



function cargaHabilidadesDirectrizParaSemanaAnio(semana, anio) {
    var semana = $('#txtSemanaCalendario').val();
    var anio = $('#txtAnioCalendario').val();
    $('.invernaderos .selected').each(function () {
        var idInvernadero = $(this).attr('id');
        var fechaPlantacion = $(this).attr('fechaPlantacion');
        var divActividadesPorInv = "#divInv_" + idInvernadero;
        var ciclo = $(this).attr('idciclo');
        $("#divInv_" + ciclo + ' table.programadas').remove();
        $("#divInv_" + ciclo + ' table.canceladas tbody tr').remove();
        if ($("#divInv_" + idInvernadero + ' tbody [anio="' + anio + '"][semana="' + semana + '"]').length == 0) {
            PageMethods.cargaHabilidadesDirectriz(idInvernadero, fechaPlantacion, semana, anio, ciclo, function (response) {
                $(divActividadesPorInv).find('.programadas tbody').append(ActividadTabla(response));
                $(divActividadesPorInv).find('.trLoad').hide();
                $(divActividadesPorInv).find('h3').css('cursor', 'pointer');

            });


        }
        else {
            $("#divInv_" + idInvernadero + ' [anio="' + anio + '"][semana="' + semana + '"]').show();
        }

    },
        function (error) {
            console.log(error);
            popUpAlert('Ocurrió un error al traer las actividades, intenta de nuevo.', 'info')
            $.unblockUI();
        }
        );

}
function cargaHabilidades() {
    $('#rollslider2 .slick-slide').html('');
    $('#rollslider2 .slick-slide').css({ 'background-image': 'url("../comun/img/ajax-loader.gif")',
        'background-repeat': 'no-repeat',
        'background-position': 'center'
    });
    PageMethods.cargaHabilidadesPlanta((parseInt($('span#ctl00_ltSemana').text()) - parseInt($('#rollslider div.selected').attr('semana'))), $('#rollslider div.selected').attr('id'), function (response) {
        $('#rollslider2').removeClass();
        $('.habilidades #rollslider2').html(response);
        setHabilidades();
    },
            function (onfail) {
                popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
}

function tablaDeActividadesPorInvernadero(idInvernadero, idCiclo, info) {
    return ' <div id="divInv_' + idCiclo + '">                                               ' +

                            '   <h3 class="accordionHeader"><label>' + info + '<label></h3>                ' +
                            '   <div class="accordionBody">                                                ' +
                           '       <table class="canceladas">                                              ' +
                            '           <thead>                                                            ' +
                            '               <tr><th colspan="4"><h3>Actividades no Programadas</h3></th></tr>' +
                            '               <tr>                                                           ' +
                            '                   <th>Habilidad</th>                                         ' +
                            '                   <th>Razón</th>                                             ' +
                            '                   <th>Comentarios</th>                                       ' +
                            '                   <th>&nbsp;</th>                                            ' +
                            '               </tr>                                                          ' +
                            '               <tr><td colspan="4" class=\'loading\' cic="' + idCiclo + '"></td></tr>               ' +
                            '           </thead>                                                           ' +
                            '           <tbody                                                             ' +
                            '           </tbody>                                                           ' +
                            '       </table>                                                               ' +

                            '<img src="../comun/img/actividades.png" class="btnImage" onclick="agregaActividad($(this));"  />' +
                            '<img src="../comun/img/calendario.png" class="btnImage" onclick="mostrarCalendario($(this));"  />' +

    // '<img src="../comun/img/save_32x32.png"  class="btnImage" onclick="guardaActividadesDeUnInvernadero($(this));"  />' +
                            '   </div>                                                                 ' +
                            '</div>                                                                    ';
}

function agregaActividad(button) {

    $('#popUpHabilidades').show().animate({ top: ($(window).scrollTop() + 50) + 'px' });
}

function sortDate(a, b) {
    return moment(a.start) - moment(b.start);
}

function sortActividad(a, b) {
    return a.idEtapa - b.idEtapa;
}

function ordenaActividades() {

};

function setHabilidades() {//inicializa los controles Slider en los que se muestran las habilidades además de las funciones DragDrop.
    $('#rollslider2').slick({
        slidesToShow: $('#rollslider2 div').length < 12 ? $(this).length : 12,
        slidesToScroll: $('#rollslider2 div').length > 12 ? 5 : 2,
        infinite: false,
        variableWidth: true
    });

    $('#rollslider2 .slick-slide').draggable({
        helper: function () {
            return $(this).clone().appendTo('body').css({
                'zIndex': 10, 'display': 'block'
            });
        },
        cursor: 'move',
        containment: 'document'
    });

    $('#rollslider2 .slick-slide').mouseup(function () {
        var selected = $(this);
        if (selected.attr('selected')) {
            selected.removeClass('selected');
            selected.attr('selected', false);
        } else {
            selected.attr('selected', true);
            selected.addClass('selected');
        }
    });
}
var banderaGlobalDeCargarInvernaderos = false;
var cantidadInvernaderos = 0;
var currentinv = 0;
function ClickACadaInvenradero() {
    banderaGlobalDeCargarInvernaderos = true;
    currentinv = 0;
    cantidadInvernaderos = $('.divInvernadero:not([class*="selected"])').length;
    $('.divInvernadero:not([class*="selected"])').each(function (i, e) {
        try {

            $(this).mouseup();
        } catch (e) {

        } finally {
                     $.unblockUI();
        }
    });
}

function filterbySite() {
    var items = [];
    var fechas = [];

    $('#calendar').fullCalendar('removeEvents');

    $('input.filtroPlantas:checked').each(function () {
        var ciclo = $(this).attr('idCiclo');
        dividedEvents.filter(function (i, n) { if (i.idCiclo == ciclo) { items.push(i); } });
        oficiales.filter(function (i, n) { if (i.idCiclo == ciclo) { fechas.push(i); } });
    });

    //                $('#calendar').fullCalendar('destroy');
    //                crearCalendario(items);
    $('#calendar').fullCalendar('addEventSource', items);

    if ($('input.fechasoficiales').attr('checked') == 'checked')
        $('#calendar').fullCalendar('addEventSource', fechas);
}

function EliminarSeleccionDeInvernaderos() {
    $('.divInvernadero').each(function () {
        if ($(this).attr('class').indexOf('selected') > -1)
        { $(this).mouseup(); }
        else
        { }
    });
}

$(function () {
    //            $('#popUpHabilidades').draggable({ handle: '.moveHandle' });
    //            $('#popUpCalendario').draggable({ handle: '.moveHandle' }).resizable();


    PageMethods.CargarHabilidadesDelDepartamento(function (response) {
        $('#divHabilidades').append(response);
    },
            function (onfail) {
                popUpAlert('Error de conexion, No se Pudo Obtener el catalogo de habilidades.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });

    $('.fechaInicio').live('click', function () {
        $('input#DateTimeDemo').val($(this).val()).click();
        $('#popUpFechaHora').show();
        $('#SelectorInterior').css({ 'position': 'relative', 'top': '40%', 'margin-top': $('#SelectorInterior').height() / 2 * -1 });
        ctrlFechaActual = $(this);
    });
    $('.fechaFin').live('click', function () {
        $('input#DateTimeDemo').val($(this).val()).click();
        $('#popUpFechaHora').show();
        ctrlFechaActual = $(this);
    });



    $('input.filtroPlantas').live('change', function () {
        filterbySite();

    });

    $('.surcos').live('keyup', function () {
        var surcosElegidos = parseInt($(this).val());
        var activity = select($(this).parent().parent().attr('idjson'), $(this).parent().parent().attr('contador'));
        var surcosTotales = activity.surcosT;

        if (surcosElegidos < 1 || surcosElegidos > surcosTotales) {
            $(this).attr('title', 'El número indicado no es válido').addClass('Error');
            $(this).parent().parent().find('.jornalesEstimados').text('Indique surcos');
        }
        else {
            $(this).removeAttr('title').removeClass('Error');
            activity.surcos = surcosElegidos;
            $(this).parent().parent().find('.jornalesEstimados').text(calculajornales(activity));
            $(this).parent().parent().find('.tiempoEstimado').html(calculatiempo(activity));

        }
    });
    $('.comentario').live('keyup', function () {
        var comentario =$(this).val();
        var activity = select($(this).parent().parent().attr('idjson'), $(this).parent().parent().attr('contador'));
        
        activity.comentario = comentario;
          
    });


    $('.chkColmena').live('change', function () {
        var activity = select($(this).parent().parent().attr('idjson'), $(this).parent().parent().attr('contador'));
        activity.esColmena = $(this).attr('checked') == 'checked';
        if (activity.esColmena) {
            $(this).parent().parent().parent().find('input[type=text].Colmena, span.Colmena').show('slow');
            $(this).parent().parent().parent().find('input.switchA.surcos.intValidate').attr('readonly', true);
        } else {
            $(this).parent().parent().parent().find('input[type=text].Colmena, span.Colmena').hide('slow');
            $(this).parent().parent().parent().find('input.switchA.surcos.intValidate').attr('readonly', false);
        }
        $(this).parent().parent().parent().find('input[type=checkbox]').attr('checked', activity.esColmena);

    });

    $('.surcoInicio').live('keyup', function () {
        var surcosElegidos = parseInt($(this).val());
        var activity = select($(this).parent().parent().attr('idjson'), $(this).parent().parent().attr('contador'));
        var surcosTotales = activity.surcosT;

        if (surcosElegidos < 1 || surcosElegidos > surcosTotales || isNaN(surcosElegidos)) {
            $(this).attr('title', 'El número indicado no es válido').addClass('Error');
            $(this).parent().parent().find('.jornalesEstimados').text('Indique surcos');
        }
        else {
            $(this).removeAttr('title').removeClass('Error');
            if (surcosElegidos > activity.surcoFin) {
                activity.surcoInicio = surcosElegidos;
                $(this).parent().parent().parent().find('input.surcoInicio').val(surcosElegidos);
                $(this).attr('title', 'Surco Inicio no puede ser mayor a surco fin.').addClass('Error');
            }
            else {
                activity.surcoInicio = surcosElegidos;
                $(this).parent().parent().find('.surcoFin').removeClass('Error');
                $(this).parent().parent().parent().find('input.surcoInicio').val(surcosElegidos);
                $(this).parent().parent().parent().find('.surcos').val(activity.surcoFin - activity.surcoInicio + 1);
                activity.surcos = (activity.surcoFin - activity.surcoInicio + 1);
                $(this).parent().parent().parent().find('.surcos').keyup();
            }

        }
    });

    $('.surcoFin').live('keyup', function () {
        var surcosElegidos = parseInt($(this).val());
        var activity = select($(this).parent().parent().attr('idjson'), $(this).parent().parent().attr('contador'));
        var surcosTotales = activity.surcosT;

        if (surcosElegidos < 1 || surcosElegidos > surcosTotales || isNaN(surcosElegidos)) {
            $(this).attr('title', 'El número indicado no es válido').addClass('Error');
            $(this).parent().parent().find('.jornalesEstimados').text('Indique surcos');
        }
        else {
            $(this).removeAttr('title').removeClass('Error');
            if (surcosElegidos < activity.surcoInicio) {
                $(this).attr('title', 'Surco Inicio no puede ser mayor a surco fin.').addClass('Error');
            }
            else {
                activity.surcoFin = surcosElegidos;
                $(this).parent().parent().parent().find('input.surcoFin').val(surcosElegidos);
                $(this).parent().parent().find('.surcoInicio').removeClass('Error');
                $(this).parent().parent().parent().find('.surcos').val(activity.surcoFin - activity.surcoInicio + 1);
                activity.surcos = (activity.surcoFin - activity.surcoInicio + 1);
                $(this).parent().parent().parent().find('.surcos').keyup();
            }


        }
    });

    $('.fechaFin').live('change', function () {
        var row = $(this).parent().parent();
        var fechaInicio = row.find('.fechaInicio');
        var fechaFin = $(this);
        var informe = '';
        var event = select($(row).attr('idjson'), $(row).attr('contador'));
        informe += revisarFechas(fechaInicio.val(), fechaFin.val());

        if (informe != '') {
            event.start = event.start.toString() == 'Invalid date' ? moment(fechaInicio.val()) : moment(fechaInicio.val());
        }

        if (!moment(event.end).isBetween(rangosemana[0].StartDate, rangosemana[0].EndDate)) {
            informe += "La Fecha no pertenece a la semana seleccionada";
        }

        if (informe != '') {
            $(fechaInicio).attr('title', informe);
            $(fechaFin).attr('title', informe);
            $(fechaFin).addClass('Error');
            $(fechaInicio).addClass('Error');
            row.find('jornalesEstimados').text('0');
        }
        else {
            $(fechaInicio).removeAttr('title', informe);
            $(fechaFin).removeAttr('title', informe);
            $(fechaFin).removeClass('Error');
            $(fechaInicio).removeClass('Error');
            event.end = moment($(row).find('.fechaFin').val(), "YYYY-MM-DD HH:mm");
            event.start = moment(row.find('.fechaInicio').val());
            change(event);
        }
    });

    $('.fechaInicio').live('change', function () {
        var row = $(this).parent().parent();
        var fechaFin = row.find('.fechaFin');
        var fechaInicio = $(this);
        var informe = '';
        var event = select($(row).attr('idjson'), $(row).attr('contador'));

        informe += revisarFechas(fechaInicio.val(), fechaFin.val());
        if (informe == '') {
            event.start = event.start.toString() == 'Invalid date' ? moment(fechaInicio.val()) : moment(fechaInicio.val());
        }
        if (!moment(event.start).isBetween(rangosemana[0].StartDate, rangosemana[0].EndDate)) {
            informe += "La Fecha no pertenece a la semana seleccionada";
        }

        if (informe != '') {
            $(fechaInicio).attr('title', informe);
            $(fechaFin).attr('title', informe);
            $(fechaFin).addClass('Error');
            $(fechaInicio).addClass('Error');
            row.find('jornalesEstimados').text('0');
        } else {
            $(fechaInicio).removeAttr('title', informe);
            $(fechaFin).removeAttr('title', informe);
            $(fechaFin).removeClass('Error');
            $(fechaInicio).removeClass('Error');
            event.start = moment($(row).find('.fechaInicio').val(), "YYYY-MM-DD HH:mm");
            event.end = moment(row.find('.fechaFin').val());
            change(event);
        }
    });

    $('select#ddlLider').live('change', function () {
        var lider = $(this).find('option:selected');
        PageMethods.changeUser(lider.attr('value'), lider.attr('idempleado'), function (response) {
            DirectrizNoP = [];
            dividedEvents = [];
            cosechasD = [];
            $('.slick-slide').html('');
            $('#rollslider2').html('');
            $('.slick-slide').css({ 'background-image': 'url("../comun/img/ajax-loader.gif")',
                'background-repeat': 'no-repeat',
                'background-position': 'center'
            });
            cargaInvernaderos();
            
            $('div#divTareasProgramadas').children().remove();
            $('div.filtroPlantas').children().remove();
        },
            function (onfail) {
                popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
    });

    $('select#ddlInvernaderosProgramados').live('change', function () {
        var invernaderoProgramado = $(this).find('option:selected');
        PageMethods.ObtieneInvernaderosProgramadosPorIdInvernadero(semanaActual, parseInt($('#txtAnioCalendario').val()), invernaderoProgramado.attr('value'), function (response) {
            $('span#InvernaderosAprogramar').html(response.toString());
        },
            function (onfail) {
                popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
    });

    //window.onbeforeunload = Confirmation;

    //            $('input[type="checkbox"]').live('click', function () {
    //                $(this).parent().parent().parent().removeClass('fill');
    //                var jornal = $(this);
    //                var habilidad = jornal.parent().parent().parent().parent();
    //                var inicio = habilidad.find('.fechaInicio').val();
    //                var fin = habilidad.find('.fechaFin').val();
    //                if (jornal.attr('checked') != 'checked') {
    //                    $('input:checkbox[idempleado="' + jornal.attr('idEmpleado') + '"]:not(:checked)').each(function () {
    //                        var jornal1 = $(this);
    //                        var habilidad1 = jornal1.parent().parent().parent().parent();
    //                        var inicio1 = habilidad1.find('.fechaInicio').val();
    //                        var fin1 = habilidad1.find('.fechaFin').val();

    //                        if ((inicio >= fin1 && inicio >= inicio1) || (inicio1 >= inicio && fin1 <= fin) || (inicio1 <= fin && fin <= fin1) || (inicio1 <= inicio && fin <= fin1)) {
    //                            jornal1.attr('disabled', false);
    //                        }
    //                    });
    //                }

    //                calculaTiempoYJornales($(this).parent().parent().parent().parent());
    //                countAsociados(habilidad);
    //            });

    cargaFamiliasAsociados();
    NombreYJornalesAutorizados();
    obtieneInvernaderosProgramadosJS();
    obtieneSemanaAnioJS();
    obtieneInvernaderosSINProgramadosJS();


});

function NombreYJornalesAutorizados() {
    try {
        PageMethods.ObtieneJornalesAutorizados(semanaActual, parseInt($('#txtAnioCalendario').val()), function (response) {
            $('span#jornalesAprobados').text(response.toString().split('|')[0]);
            $('span#nombreLider').html(response.toString().split('|')[1]);
        });
    } catch (e) {
        console.log(e);
    }
}

function obtieneInvernaderosProgramadosJS() {
    try {
        PageMethods.ObtieneInvernaderosProgramadosWeb(semanaActual, parseInt($('#txtAnioCalendario').val()), function (response) {
            $('span#InvernaderosProgramados').html(response.toString());
        });
    } catch (e) {
        console.log(e);
  }
}

function obtieneSemanaAnioJS() {
    try {
        PageMethods.ObtieneSemanaAnioProgramadosWeb(semanaActual, parseInt($('#txtAnioCalendario').val()), function (response) {
            $('span#semanaAnio').html(response.toString());
        });
    } catch (e) {
        console.log(e);
    }
}

function obtieneInvernaderosSINProgramadosJS() {
    try {
        PageMethods.ObtieneInvernaderosSINProgramadosWeb(semanaActual, parseInt($('#txtAnioCalendario').val()), function (response) {
            $('span#InvernaderosAprogramar').html(response.toString());
        });
    } catch (e) {
        console.log(e);
    }
}


function Confirmation() {
    return '"Seguro que desea salir? los cambios no guardados se perderán';
}

function intToTime(time) {
    var hour = Math.trunc(time);
    var minuts = Math.trunc((time % 1) * 60);
    return hour + ':' + (minuts.toString().length == 1 ? ('0' + minuts) : minuts);
}

function revisarFechas(fechaInicio, fechaFin) {
    fechaInicio = moment(fechaInicio, "YYYY-MM-DD HH:mm");
    fechaFin = moment(fechaFin, "YYYY-MM-DD HH:mm");
    var Errores = '';
    //si el inicio de la actividad está después de la hora fin, muestra mensaje de error
    if (fechaInicio.isAfter(fechaFin) || fechaInicio.isSame(fechaFin))
        Errores += 'La fecha inicial debe ser menor que la final.\n';
    //    if (moment(getOnlyTime(fechaInicio), "HH:mm").isBefore(moment(jornadaInicio, "HH:mm")) || moment(getOnlyTime(fechaInicio), "HH:mm").isAfter(moment(jornadaFin, "HH:mm")))
    //        Errores += 'La hora inicio esta fuera del horario de trabajo.\n';
    //    if (moment(getOnlyTime(fechaFin), "HH:mm").isBefore(moment(jornadaInicio, "HH:mm")) || moment(getOnlyTime(fechaFin), "HH:mm").isAfter(moment(jornadaFin, "HH:mm")))
    //        Errores += 'La hora fin esta fuera del horario de trabajo.\n';
    return Errores;
}

function asignarFecha() {
    $(ctrlFechaActual).val($("#DateTimeDemo").val());
    $('#popUpFechaHora').hide();
    $(ctrlFechaActual).change();
}

function revisaAusencias(Actividad) {
    var dia = moment(getOnlyDate(Actividad.start));
    for (var as in Actividad.Asociados) {
        for (var aus in Ausencias) {
            if (Actividad.Asociados[as].idAsociado == Ausencias[aus].idAsociado && dia.isSame(moment(Ausencias[aus].fecha, "YYYY-MM-DD"))) {
                $('tr[idjson="' + Actividad.id + '"] ').find('li[number="' + Actividad.Asociados[as].idAsociado + '"]').addClass('Unavailable');
                popUpAlert("El asociado " + Actividad.Asociados[as].idAsociado + " " + Actividad.Asociados[as].Nombre + " no asiste el día " + Ausencias[aus].fecha, 'info');
                //Actividad.Asociados.splice(as, 1);
            }
        }
    }

}

function RevisaTiemposAsociados() {
     limpiezaok = [];
     limpiezano = [];
     podas = [];
    $('tr').removeClass('Error');
    $('td.jornales ul li').removeClass('Error');
    var evtA, evtB, coincidence = false;
    for (var a = 0; a < dividedEvents.length; a++) {
        evtA = dividedEvents[a];
        if (espodayvuelta(evtA.idHabilidad)) {
            var exist = false;
            for (var b in podas) {
                if (podas[b] == evtA.invernadero) {
                    exist = true;
                }
                
            }
            if (!exist) {
                podas.push(evtA.invernadero);
            }
        }

        revisaAusencias(evtA);

        var inicioA = moment(evtA.start.format("YYYY-MM-DD HH:mm"), "YYYY-MM-DD HH:mm");
        var finA = moment(evtA.end.format("YYYY-MM-DD HH:mm"), "YYYY-MM-DD HH:mm");
        for (var b = 0; b < dividedEvents.length; b++) {
            evtB = dividedEvents[b];

            if (evtA.id == evtB.id) {
                continue;
            }
            
            
            var inicioB = moment(evtB.start.format("YYYY-MM-DD HH:mm"), "YYYY-MM-DD HH:mm");
            var finB = moment(evtB.end.format("YYYY-MM-DD HH:mm"), "YYYY-MM-DD HH:mm");

            if (eslimpieza(evtA.idHabilidad) && espodayvuelta(evtB.idHabilidad) && evtA.idInvernadero == evtB.idInvernadero) {
                if (inicioB > inicioA && dateDiff(inicioA, inicioB, 'h', false) > 48) {
                    limpiezano.push(evtB.invernadero);
                    console.log('No cubierta '+evtB.id);
                }
                else if(inicioB<inicioA) {
					limpiezano.push(evtB.invernadero);
                    console.log('No cubierta '+evtB.id);
				}else{
                    limpiezaok.push(evtB.invernadero);
                    console.log('Cubierta '+evtB.id);
                }
            } 

            //si el día de la semana del dia festivo oficial coincide con la actividad programada, se comparan los horarios y se divide la actividad para que no se traslapen.
            //          Inicio        Fin
            //            [ActividadA]
            //     [ActividadB]
            //   Inicio         Fin
            if ((inicioA >= inicioB && inicioA < finB) && (inicioA <= finB && finA > finB)) {
                marcaAsociadosRepetidos(evtA, evtB);
                continue;

            }

            //  Inicio       Fin
            //    [ActividadA]cargaHabilidadesDirectriz
            //              [ActividadB]
            //            Inicio         Fin

            else if ((inicioA < inicioB && finA <= finB) && (inicioB < finA && finA <= finB)) {
                marcaAsociadosRepetidos(evtA, evtB);
                continue;
            }

            //   Inicio         Fin
            //     [ActividadA]
            //             [ActividadB]
            //          Inicio         Fin

            else if ((inicioA <= inicioB && finA >= finB)) {
                marcaAsociadosRepetidos(evtA, evtB);
                continue;
            }

            //       Inicio               Fin
            //               [ActividadA]
            //      [    Fecha     Oficial    ]
            //    Inicio                     Fin

            else if ((inicioA >= inicioB && finA <= finB)) {
                marcaAsociadosRepetidos(evtA, evtB);
                continue;
            }

        }
    }

    //    if (evtB != undefined) {
    //        $('tr[idjson="' + evtB.id + '"]').remove();
    //       // change(evtB);
    //    }
}

function testimado(activity) {
    var time;
    for (var a in dividedEvents) {
        if (dividedEvents[a].idTr == activity.idTr) {
            time += calculatiempo(dividedEvents[a]);
        }
    }
    return time;
}

function jornalesEstimados(activity) {
    var j = 0.0;
    var surcos = 0;

    for (var a in dividedEvents) {
        if (dividedEvents[a].idTr == activity.idTr) {
            surcos += dividedEvents[a].surcos;
        }
    }

    var tiempo = tiempoTotal(activity.idTr, 'm');

    var plantulas = surcos * activity.plantasPorSurco;

    var horas = plantulas / activity.target;

    var jornales = horas / tiempo;

    return jornales;
}

function marcaAsociadosRepetidos(evtA, evtB) {

    if ((esfumigacion(evtA.idHabilidad) && escosecha(evtB.idHabilidad) && evtA.idInvernadero == evtB.idInvernadero) || (esfumigacion(evtB.idHabilidad) && escosecha(evtA.idHabilidad) && evtA.idInvernadero == evtB.idInvernadero)) {
        popUpAlert('Fumigacion cruza con cosecha!', 'warning');
        $('tr[idjson="' + evtA.id + '"]').addClass('Error');
        $('tr[idjson="' + evtB.id + '"]').addClass('Error');
    } else {
        $('tr[idjson="' + evtA.id + '"]').removeClass('Error');
        $('tr[idjson="' + evtB.id + '"]').removeClass('Error');
    }

    var jA, jB;
    for (var a = 0; a < evtA.Asociados.length; a++) {
        jA = evtA.Asociados[a];
        for (var b = 0; b < evtB.Asociados.length; b++) {
            jB = evtB.Asociados[b];
            if (jA.idAsociado == jB.idAsociado) {
                $('tr[idjson="' + evtA.id + '"] ').find('li[number="' + jA.idAsociado + '"]').addClass('Error');
                $('tr[idjson="' + evtB.id + '"] ').find('li[number="' + jB.idAsociado + '"]').addClass('Error');
            }
        }
    }
}

function moverNoProgramadas() {
    $('tr[programado="0"]').each(function () {
        $(this).find('td.switchA img').click();
        $(this).attr('act', 'U');
    });
}

function AgregaInvernaderosACopiar() {
    
    var allInvCopia = [];
    var InvernaderosACopiar;
    if (confirm('Seguro que desea copiar el programa semanal ' + $('select#ddlInvernaderosProgramados').find('option:selected').text() + '?')) {

        $('#popupConfiguraCopiaProgramacion input:checked').each(function () {
            var idInvernadero = $(this).attr('id').split('_')[1];
            InvernaderosACopiar = [{
                "idInvernadero": idInvernadero
            }];

            allInvCopia.push({ 'invernaderosCopia': InvernaderosACopiar });
        });

        var idInvernaderoOrigen = $('select#ddlInvernaderosProgramados').find('option:selected').val();
        var semana = $('#txtSemanaCalendario').val();
        var anio = $('#txtAnioCalendario').val();

        PageMethods.copiarProgramacionCompleta(allInvCopia, idInvernaderoOrigen, semana, anio, function (response) {
            popUpAlert(response.split('|')[0], response.split('|')[1]);

            if (response.split('|')[1] == 'OK') {
                reload();

            }
            else {
                $.unblockUI();
            }

        },
                function (onfail) {
                    popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                    console.log(onfail);
                    $.unblockUI();
                });
    }

}

function AgregarHabilidadesAInvernaderos() {
    var n = 0;
    var Actividad;
   
    var inicio = moment(moment(rangosemana[0].StartDate).format('YYYY-MM-DD ') + jornadaInicio);
    $('#divHabilidades input:checked').each(function () {
        var directriz = $(this).parent().parent().find('.invernadero').length;
        var idInvernadero = $(this).attr('id').split('_')[1];
        var idCiclo = $(this).attr('idCiclo');
        var idEtapa = $(this).parent().parent().find('.idEtapa').text();
        var Habilidad = $(this).parent().parent().find('span[class="habilidad_descripcion"]').text()
        var idHabilidad = $(this).parent().parent().find('.idHabilidad').text();
        var target = $(this).parent().parent().find('.target').text();
        var portiempo = $(this).parent().parent().find('.portiempo').text();
        var Etapa = $(this).parent().parent().find('.habilidad_etapa').text();
        var color = rgb2hex($(this).parent().parent().find('.btnHabilidad').css('background-color'));
        var Elemento = $(this).parent().parent().find('.elemento').text();
        var cantidadElementos = $(this).parent().parent().find('.cantidadElementos').text();
        var producto = $(this).attr('product');
        var densidad = $(this).attr('densidad');
        var surcos = $(this).attr('surcos');
        var esInterplanting = $(this).attr('esInterplanting');
        var invernadero = $(this).attr('invernadero');
        var cortes = $(this).attr('corte');
        var semana = $('#txtSemanaCalendario').val();
        var anio = $('#txtAnioCalendario').val();
        //                var tr = '<tr act = "N" anio="' + anio + '" semana="' + semana + '" semanaInicio="' + moment(rangosemana[0].StartDate) + '" semanaFin="' + moment(rangosemana[0].EndDate) + '" editable=true idCiclo="' + idCiclo + '" target="' + target + '" portiempo="' + portiempo + '" class="' + (directriz > 0 ? 'habilidadDeDirectriz' : 'normal') + '" idInvernadero="' + idInvernadero + '" plantasPorSurco="' + (parseInt($(this).attr('densidad')) / parseInt($(this).attr('surcos'))) + '" contador="normal_' + (Contador++) + '" color="' + color + '" densidad="' + $(this).attr('densidad') + '"><td><span class=\"invisible idEtapa\">' + idEtapa + '</span><span class=\"invisible idHabilidad\">' + idHabilidad + '</span>' + Etapa + ' - ' + Habilidad + '</td>' +
        //                        '<td class="switchA"><input type="text" class="switchA fechaInicio" value="' + moment(moment(rangosemana[0].StartDate).format('YYYY-MM-DD ') + jornadaInicio).add(n, 'hours').format('YYYY-MM-DD HH:mm') + '" /> </td>' +
        //                        '<td class="switchA"><input type="text" class="switchA fechaFin" value="' + moment(moment(rangosemana[0].StartDate).format('YYYY-MM-DD ') + jornadaInicio).add(n + 1, 'hours').format('YYYY-MM-DD HH:mm') + '" /> </td>' +
        //                        '<td class="switchA"><input type="text" class="switchA surcos intValidate" value="' + $(this).attr('surcos') + '"/><span class="switchA"> /' + $(this).attr('surcos') + '</span></td>' +
        //                //                        '<td class="target">' + target + '</td>' +
        //                        '<td class="switchA tiempoEstimado"></td>' +
        //                        '<td class="switchA jornalesEstimados"></td>' +
        //                        '<td class="switchA jornales"><h3></h3><ul>' +
        //                       AsociadosEtapa(idEtapa, n++) +
        //                        '</ul></td>' +
        //                         '<td class="switchB razon">  <%= razonesDeEliminacion%>  </td>' +
        //                                  '<td class="switchB comentario\"><textarea></textarea></td>' +
        //                                  '<td class="switchA"><img class="hint" src="../comun/img/remove.ico" title ="No progaramar esta Actividad"  onclick="eliminarHabilidadProgramada($(this));\" /></td>' +
        //                                  '<td class="switchB"><img class="hint" src="../comun/img/goback.png" title ="Programar esta actividad" onclick="RegresaTareaDirectriz($(this));\" /></td>';
        //                $('#divInv_' + idInvernadero + ' .programadas tbody ').append(tr);
        $(this).attr('checked', false);

        if (moment(getOnlyTime(inicio), "HH:mm") > moment(jornadaFin, "HH:mm")) {
            inicio = moment(inicio.add(1, 'days').format("YYYY-MM-DD ") + jornadaInicio);

        }

        Actividad = [{ "anio": anio,
            "semanaInicio": moment(rangosemana[0].StartDate),
            "semanaFin": moment(rangosemana[0].EndDate),
            "semana": semana,
            "invernadero": invernadero,
            "idHabilidad": idHabilidad,
            "idEtapa": idEtapa,
            "porTiempo": portiempo,
            "target": target,
            "plantasPorSurco": (parseInt(densidad) / parseInt(surcos)),
            "densidad": densidad,
            "surcos": surcos,
            "surcosT": surcos,
            "idCiclo": idCiclo,
            "editable": true,
            "idInvernadero": idInvernadero,
            "idTr": "normal_" + (Contador++),
            "directriz": false,
            "title": invernadero + ":" + Habilidad + "-" + Etapa + "\n " + producto + "\n " + "Surcos:" + surcos,
            "backgroundColor": "#" + color,
            "nombreHabilidad": Habilidad,
            "nombreEtapa": Etapa,
            "elemento": Elemento,
            "Asociados": [],
            "cantidadElementos": cantidadElementos,
            "start": inicio,
            "end": moment(inicio.format("YYYY-MM-DD HH:mm")).add(1, 'hours'),
            "act": "N",
            "numeroactividad": cortes,
            "esInterplanting": esInterplanting
        }];

        change(copyEvent(Actividad[0]));
        inicio.add(1, 'hours');
    });

    $('#popUpHabilidades').hide();

}

function tiempoTotal(a, tt) {

    var minutes = 0;
    $.each(dividedEvents, function (i, n) {
        if (n.idTr == a) {
            minutes += dateDiff(n.start, n.end, tt, true);
        }
    });
    return minutes;
}

function AsociadosEtapa(e, c) {
    var result = "";
    var jsonFiltroAsociados = [];

    JsonAsociados.filter(function (i, n) {
        if (i.IdEtapa == e) {
            jsonFiltroAsociados.push(i);
        }
    });

    $.each(jsonFiltroAsociados, function (i, n) {
        result += "<li><input type=\"checkbox\" idEmpleado=\"" + n.ID_EMPLEADO + "\" eficiencia=\"" + n.eficiencia + "\" id=\"_" + n.ID_EMPLEADO + '_' + c + "\" ><label for=\"_" + n.ID_EMPLEADO + '_' + c + "\">" + n.NOMBRE + "</label>    </li>";
    });
    if (result.length == 0 || jsonFiltroAsociados.length == 0) {
        result = "No hay Asociados relacionados a la actividad seleccionada, <a target='_blank' href='../admin/frmRelacionNivel.aspx'>Haga click aquí</a> para agregar asociados a la familia-nivel correspondiente.";
    }

    return result;
}

function countAsociados(habilidad) {
    $('.programadas tbody tr:not(.trLoad)').each(function () {
        habilidad.find('td.jornales h3').text(habilidad.find('td.jornales li input[type="checkbox"]:checked').length);
    });
}

function eliminarHabilidadProgramada(tr) {
    //var tr = imgClicked.parent().parent();
    var activitys = [];
    tr.find('td.jornales input:checked').attr('checked', false);
    activitys = selectActivityPeriods($(tr).attr('contador'));
    if ($(tr).attr('class').indexOf('normal') > -1) {
        if (activitys.length == 1) {
            actividadRechazar(parseInt(tr.attr('idJson')));
            $(tr).parent().parent().remove();
        } else {

            eliminaDelCalendario(parseInt(tr.attr('idJson')));
            $(tr).remove();
        }


    }
    else {

        if (typeof tr.attr('idactividad') !== typeof undefined && tr.attr('idactividad') !== false) {
            tr.attr('act', 'N');
        } else {
            tr.attr('act', 'U');
        }

        if ($(tr).attr('class').indexOf('habilidadDeDirectriz') > -1) {

            $(tr).remove();
            //si la actividad de directriz tiene 1 solo periodo, se agrega a rechazadas.
            if (activitys.length == 1) {
                $('div#divInv_' + $(tr).attr('idCiclo')).find('.canceladas').append($(tr));
                $('table.programadas[idtr="' + $(tr).attr('contador') + '"]').remove();
                $(tr).find('.switchA').hide();
                $(tr).find('.switchB').show();
                //agrega la actividad a rechazadas,
                actividadRechazar(tr.attr('idJson'));
            } else {
                eliminaDelCalendario(parseInt(tr.attr('idJson')));
            }

        } else {
            //No es Actividad de directriz
        }
    }
    RevisaTiemposAsociados();
}

function actividadRechazar(id) {
    var eliminada;
    for (var of = 0; of < dividedEvents.length; of++) {

        if (dividedEvents[of].id == id) {
            if (dividedEvents[of].directriz == 'true' || dividedEvents[of].directriz == '1') {
                eliminada = copyEvent(dividedEvents[of]);
                eliminada.id = dividedEvents[of].id;
                if (eliminada.programada == undefined && eliminada.idperiodo == undefined) {
                    eliminada.act = 'N';
                }
                else if (eliminada.idActividadNoP != undefined && eliminada.programada == 0) {
                    eliminada.act = 'U';
                }
                else {
                    eliminada.act = 'N';
                }
                DirectrizNoP.push(eliminada);
                dividedEvents.splice(of, 1);
                return;
            } else {
                if (dividedEvents[of].idActividad != undefined) {
                    Eliminadas.push(dividedEvents[of].idActividad);
                    dividedEvents.splice(of, 1);
                    return;
                }
                else {
                    dividedEvents.splice(of, 1);
                    return;
                }
            }
        }
    }
}



function eliminaDelCalendario(id) {

    for (var of = 0; of < dividedEvents.length; of++) {

        if (dividedEvents[of].id == id) {

            dividedEvents.splice(of, 1);
            return;
        }

    }
}

function ddlRazonesDeCancelacion() {
    return '<select><option value="0">-- Seleccione --</option></select>';
}

function RegresaTareaDirectriz(imgClicked) {
    var tr = imgClicked.parent().parent();
    $(tr).remove();
    for (var d = 0; d < DirectrizNoP.length; d++) {
        if (DirectrizNoP[d].id == $(tr).attr('idjson') && DirectrizNoP[d].idTr == $(tr).attr('contador')) {

            if (DirectrizNoP[d].idActividad != undefined && DirectrizNoP[d].programada == 1) {
                DirectrizNoP[d].act = "U";
            } else {
                DirectrizNoP[d].act = "N";
            }
            DirectrizNoP[d].programada = 1;
            dividirTraslapes(copyEvent(DirectrizNoP[d]));
            DirectrizNoP.splice(d, 1);
        }
    }
}

function guardaActividadesDeUnInvernadero(imageClicked) {
    var divInvernadero = $(imageClicked).parent().parent();
    var idInvernadero = $(imageClicked).parent().parent().attr('id').split('_')[1];
    var siProgramado = [];
    $(divInvernadero).find('.programadas tbody tr:not(.trLoad)').each(function () {
        siProgramado.push({
            'idInvernadero': idInvernadero,
            'idEtapa': $(this).find('.idEtapa').text(),
            'fechaInicio': $(this).find('.fechaInicio').val(),
            'fechaFin': $(this).find('.fechaFin').val(),
            'surcos': $(this).find('.surcos').val(),
            'target': $(this).attr('target'),
            'tiempoEstimado': $(this).find('.tiempoEstimado').text(),
            'jornalesEstimados': $(this).find('.jornalesEstimados').text(),
            'jornales': $(this).find('input:checked').map(function () { return $(this).attr('idEmpleado'); }).get(),
            'idCiclo': $(this).attr('idCiclo'),
            'cantidadDeElementos': $(this).find('.elementos').val(),
            'semana': $(this).attr('semana'),
            'esDirectriz': $(this).attr('class').indexOf('habilidadDeDirectriz') > -1 ? 1 : 0,
            'esInterplanting': 0, //Buscar de donde se puede obtener esta información
            'periodos': filterByActivity($(this).attr('contador'))
        });
    });

    var noProgramado = [];
    $(divInvernadero).find('.canceladas tbody tr:not(.trLoad)').each(function () {
        noProgramado.push({
            'semanaProgramacion': $(this).attr('semana'),
            'anioProgramacion': $(this).attr('anio'),
            'idInvernadero': idInvernadero,
            'idEtapa': $(this).find('.idEtapa').text(),
            'razon': $(this).find('.razon option:selected').val(),
            'comentario': $(this).find('.comentario textarea').val(),
            'idCiclo': $(this).attr('idCiclo'),
            'cantidadDeElementos': 0,
            'esInterplanting': 0
        });
    });

    PageMethods.almacenarConfiguracionDeInvernadero(siProgramado, noProgramado, null, function (response) {
        popUpAlert('Funcionalidad en desarrollo', 'warning');
    });
}

function filterByActivity(id, Invernadero) {
    var p = [];

    dividedEvents.filter(function (i, n) {
        
        if (i.idTr == id && i.idInvernadero == Invernadero) {
            p.push({ "idPeriodo": i.idPeriodo, "inicio": moment(i.start).format("YYYY-MM-DD HH:mm"), "fin": moment(i.end).format("YYYY-MM-DD HH:mm"), "surcos": i.surcos, Asociados: i.Asociados });
        }
    });
    return p;
}
function filterByActivityComentario(id, Invernadero) {
    var p = [];

    dividedEvents.filter(function (i, n) {

        if (i.idTr == id && i.idInvernadero == Invernadero) {
            p.push({ "idPeriodo": i.idPeriodo, "inicio": moment(i.start).format("YYYY-MM-DD HH:mm"), "fin": moment(i.end).format("YYYY-MM-DD HH:mm"), "surcos": i.surcos, Asociados: i.Asociados, "comentario": i.comentario });
        }
    });
    return p;
}

function Enviar() {
    if (confirm("Seguro que desesa enviar?")) {
        guardaProgramacionCompleta(true);
		
    }
}

function securityVal(enviar) {
    var securityinv = '';

    for (var a in podas) {
        for (var b in limpiezaok) {
            if (podas[a] == limpiezaok[b]) {
                podas[a] = undefined;
            }
        }
        if (podas[a] != undefined) {
            securityinv += podas[a] + ',';
        }
    }
    if (securityinv == '') {
        if (enviar) {
            Enviar();
        } else {
            guardaProgramacionCompleta(false);
        }
    } else {
        popUpAlert("Para el(los) invernadero(s) " + securityinv + " no existe limpieza previo a la poda y vuelta. <br/> Asegurate que tu invernadero se encuentre limpio, recuerda que la seguridad es nuestra prioridad.<br/>", 'warning');
        $('td#jsPopoUpButtons').append("<input type='button' value='Aceptar' onclick='" + (enviar ? "Enviar()" : "guardaProgramacionCompleta(false);") + "' />");
        $('input#btnClosePopUp').attr('value', 'Regresar');
    }

}
function copiarPrograma(){

}

function checkSurcos() {
    for (var a in dividedEvents) {
        if (dividedEvents[a].porTiempo != 'True' && dividedEvents[a].surcos < dividedEvents[a].surcosp) {
            return false;
        }
    }
    return true;
}

function guardaProgramacionCompleta(enviar) {
	closeJsPopUp();
    var fullrequired = true;
    var insert = true;

    var allInv = [];
    try {
        //$.blockUI();



        $('#divTareasProgramadas div:not(.accordionHeader, .accordionBody)').each(function () {
            var actividadh;
            var divInvernadero = $(this);
            var idInvernadero = $(this).attr('id').split('_')[1];
            var siProgramado = [];
            if ($(divInvernadero).find('.Error').length > 0 || $(divInvernadero).find('.Infestacion').length > 0) {
                insert = false;
            }

            $(divInvernadero).find('table.programadas').each(function () {
                actividadh = selectActivityPeriods($(this).attr('idTr'))[0];

                if (actividadh.Asociados.length == 0) {
                    insert = false;
                    $(this).find('.jornales').addClass('fill');
                }
                insert = checkSurcos() && insert ? true : false;

                siProgramado.push({
                    'idActividad': actividadh.idActividad,
                    'idActividadNoP': actividadh.idActividadNoP,
                    'idInvernadero': actividadh.idInvernadero,
                    'idEtapa': actividadh.idEtapa,
                    'target': Math.round(actividadh.target),
                    'semana': actividadh.semana,
                    'jornalesEstimados': actividadh.porTiempo == 'True' ? 0 : jornalesEstimados(actividadh),
                    'idCiclo': actividadh.idCiclo,
                    'cantidadDeElementos': (actividadh.cantidadElemento > 0 ? actividadh.cantidadElemento : (esPreparacion(actividadh.idHabilidad) ? actividadh.variable : 0)),
                    'esDirectriz': actividadh.directriz == "1" ? 1 : actividadh.directriz == "true" ? 1 : 0,
                    'esInterplanting': actividadh.esInterplanting == "1" ? 1 : actividadh.esInterplanting == "true" ? 1 : 0,
                    'periodos': filterByActivityComentario(actividadh.idTr, actividadh.idInvernadero,actividadh.comentario),
                    'anio': actividadh.anio,
                    'surcoInicio': actividadh.surcoInicio,
                    'surcoFin': actividadh.surcoFin,
                    'esColmena': actividadh.esColmena == true ? 1 : 0,
                    'act': actividadh.act,
                    'comentario': actividadh.comentario == undefined ? "" : actividadh.comentario.trim()

                });
            });
            var noProgramado = [];

            for (var np = 0; np < DirectrizNoP.length; np++) {
                actividadh = DirectrizNoP[np];
                noProgramado.push({
                    'idActividad': actividadh.idActividad,
                    'idActividadNoP': actividadh.idActividadNoP,
                    'semanaProgramacion': actividadh.semana,
                    'anioProgramacion': actividadh.anio,
                    'idInvernadero': actividadh.idInvernadero,
                    'idEtapa': actividadh.idEtapa,
                    'semana': actividadh.semana,
                    'razon': actividadh.razon,
                    'comentario': actividadh.comentario == undefined ? undefined : actividadh.comentario.trim(),
                    'idCiclo': actividadh.idCiclo,
                    'cantidadDeElementos': actividadh.cantidadElemento,
                    'esInterplanting': actividadh.esInterplanting,
                    'act': actividadh.act
                });

                if (actividadh.razon == undefined) {
                    fullrequired = false;
                    $('tr[idjson="' + actividadh.id + '"][idciclo="' + actividadh.idCiclo + '"]').find('.ddlRazon').addClass('Error');
                }
                if (actividadh.comentario == undefined || actividadh.comentario == "") {
                    fullrequired = false;
                    $('tr[idjson="' + actividadh.id + '"][idciclo="' + actividadh.idCiclo + '"]').find('.coment').addClass('Error');
                }
            }

            allInv.push({ 'programadas': siProgramado, 'canceladas': noProgramado, 'eliminadas': Eliminadas });

        });

        if ((!insert && !enviar || insert && enviar && EnviarP || insert && !enviar) && fullrequired) {
            PageMethods.almacenarProgramacionCompleta(allInv, enviar, function (response) {
                popUpAlert(response.split('|')[0], response.split('|')[1]);

                if (response.split('|')[1] == 'OK') {
                    reload();

                }
                else {
                    $.unblockUI();
                }

            },
            function (onfail) {
                popUpAlert('Error de conexion, intenta nuevamente.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
        } else if (enviar && !EnviarP) {
            popUpAlert("Primero debes guardar tu Programacion para Poderla Enviar.", "Info");
            $.unblockUI();
        }
        else if (enviar) {
            popUpAlert("No es Posible Enviar, falta Información por llenar o hay incoherencias en los datos", 'warning');
            $.unblockUI();
        }

        else {
            popUpAlert("Aún falta Información por llenar o hay incoherencias en los datos", 'warning');
            $.unblockUI();
        }
    } catch (Exception) {
        console.log(Exception);
        popUpAlert("Aún falta Información por llenar o hay incoherencias en los datos", 'warning');
        $.unblockUI();
    } finally {


    }
}

function semanaAnterior() {
    var semana = parseInt($('#txtSemanaCalendario').val());
    var anio = parseInt($('#txtAnioCalendario').val());
    semanaActual = semana;
    maxAnioAnterior = 52;
    if (semana == 1) {
        semanaActual = semana = maxAnioAnterior;
        anio = parseInt($('#txtAnioCalendario').val()) - 1;
        $('#txtSemanaCalendario').val(semana);
        $('#txtAnioCalendario').val(anio);
    }
    else {
        semanaActual = semana = parseInt(semana) - 1;
        $('#txtSemanaCalendario').val(semana);
    }
    PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
        rangosemana = JSON.parse(response);
        rangosemana[0].EndDate += 86340000;

    },
            function (onfail) {
                popUpAlert('Error de conexion, No se pudo obtener los datos de la semana, intente de nuevo.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
    DirectrizNoP = [];
    dividedEvents = [];
    cosechasD = [];
    cargaHabilidadesDirectrizParaSemanaAnio(semana, anio);
  
    //ARTURO
    NombreYJornalesAutorizados();
    obtieneInvernaderosProgramadosJS();
    obtieneSemanaAnioJS();
    obtieneInvernaderosSINProgramadosJS();
}

function semanaSiguiente() {
    var semana = $('#txtSemanaCalendario').val();
    var anio = $('#txtAnioCalendario').val();
    semanaActual = semana;
    maxAnioActual = 53;
    maxAnioSiguiente = 52;
    if (semana == maxAnioActual) {
        semanaActual = semana = 1;
        anio = parseInt($('#txtAnioCalendario').val()) + 1;
        $('#txtSemanaCalendario').val(1);
        $('#txtAnioCalendario').val(anio);
    }
    else {
        semanaActual = semana = parseInt(semana) + 1;
        $('#txtSemanaCalendario').val(semana);
    }
    PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
        rangosemana = JSON.parse(response);
        rangosemana[0].EndDate += 86340000;
    },
            function (onfail) {
                popUpAlert('Error de conexion, No se pudo obtener los datos de la semana, intente de nuevo.', 'warning');
                console.log(onfail);
                $.unblockUI();
            });
    DirectrizNoP = [];
    dividedEvents = [];
    cosechasD = [];
    cargaHabilidadesDirectrizParaSemanaAnio(semana, anio);
    //ARTURO
    NombreYJornalesAutorizados();
    obtieneInvernaderosProgramadosJS();
    obtieneSemanaAnioJS();
    obtieneInvernaderosSINProgramadosJS();
}
