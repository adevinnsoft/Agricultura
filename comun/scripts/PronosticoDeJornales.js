var hora = 0,
ausentismo = 0,
capacitacion = 0,
curva = 0;
var STR_HORA = "Hora",
STR_AUSENTISMO = "Ausentismo",
STR_CURVA = "Curva",
STR_CAPACITACION = "Capacitacion";
var planta = 0;
var cargandoAnios = true
    , cargandoSemanas = true;
var   jsonAnios = null
    , jsonVariedades = null
    , jsonEficiencias = null
    , jsonCategorias = null
    , jsonLideres = null
    , jsonEtapas = null
    , jsonDesgloseActividades = null;

var primeraVez = 0;
$(function () {
    
    PageMethods.getAnios(function (response) {
        if (response != null && response != '') {
            jsonAnios = JSON.parse(response);
            var strAnios = '';
            var strSemanas = '';
            if (jsonAnios.length > 0) {
                for (var anio in jsonAnios) {
                    strAnios += '<option value="' + jsonAnios[anio].anio + '" ultimasemana="' + jsonAnios[anio].ultimaSemana + '" '/* + (jsonAnios[anio].anio == (new Date).getFullYear() ? 'selected' : '') */ + '>' + jsonAnios[anio].anio + '</option>';
                    if (jsonAnios[anio].anio == (new Date).getFullYear()) {
                        strSemanas += '';
                        for (cont = 1; cont <= jsonAnios[anio].ultimaSemana; cont++) {
                            strSemanas += '<option value="' + cont + '"' /* + (cont == parseInt($(".semana span").text()) ? 'selected' : '')*/ + '>' + cont + '</option>';
                        }

                        $("#txtSemanaPartida").empty();
                        $("#txtSemanaPartida").append(strSemanas)//.val($(".semana span").text());
                        cargandoSemanas = false;
                    } else {
                        cargandoSemanas = false;
                    }

                }
                $('#txtAnioPartida').empty();
                $('#txtAnioPartida').append(strAnios)//.val((new Date).getFullYear());
                cargandoAnios = false;
            } else {
                cargandoAnios = false;
            }
        } else {
            cargandoAnios = false;
            cargandoSemanas = false;
        }
    });

//    $('#txtAnioPartida').change(function () {
//        var anio = $(this).val();
//        var totalSemana = $(this).find('option[value="' + anio + '"]').attr('ultimaSemana');
//        var semanaActual = $("#txtSemanaPartida").val();
//        var strSemanas = '';
//        if (totalSemana != undefined && totalSemana > 0) {
//            strSemanas += '';
//            for (cont = 1; cont <= totalSemana; cont++) {
//                strSemanas += '<option value="' + cont + '"'/* + (cont == parseInt($(".semana span").text()) ? 'selected' : '')*/ + '>' + cont + '</option>';
//            }

//            $("#txtSemanaPartida").empty().append(strSemanas).val(semanaActual);
//        }
//        console.log(totalSemana);
//    });

    //bloqueoDePantalla.bloquearPantalla(); //window.console && console.log("load!");
    var semanaActual = $("#ctl00_ltSemana").text()

    $("#subPronostico").text(" " + parseInt(semanaActual));
    $(".preConfiguracion").change(function () {
        bloqueoDePantalla.bloquearPantalla();
        limpiarTablaPronosticos();
        var nSemanas = $('#txtNumSemanas').val();
        var nHistoricos = $('#txtNumHistoricos').val();
        var semanaPartida = $('#txtSemanaPartida').val();
        var anioPartida = $('#txtAnioPartida').val();
        //window.console && console.log("change! " + nSemanas);
        if (nSemanas != null && parseInt(nSemanas) > 0
                    && nHistoricos != null && parseInt(nHistoricos) > 0
                    && semanaPartida != null && parseInt(semanaPartida) > 0
                    && anioPartida != null && parseInt(anioPartida) > 0) {
            genTablaConfiguraciones(nSemanas, nHistoricos, semanaPartida, anioPartida);
        } else {
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        }
        bloqueoDePantalla.desbloquearPantalla();

//        limpiarTablaPronosticos();
//        $('.trConfiguraciones').hide();
    });

    $('#ctl00_ddlPlanta').change(function () {
        if ($('#ctl00_ddlPlanta').val() != undefined && planta != parseInt($('#ctl00_ddlPlanta').val())) {
            planta = parseInt($('#ctl00_ddlPlanta').val());
            primeraVez = 0;
            $(".configuracion").off('change');
            $('.trConfiguraciones').hide();
            window.console && console.log("farm change! ");
            cargaConfiguracion();
        }
    });

    
    







    $("#btnCalcular").click(function () {
        
        //        if ($("intpu#txtNumActividades").val() == "") {
        //            $("intpu#txtNumActividades")
        //        }
        bloqueoDePantalla.bloquearPantalla();
        //window.console && console.log("change! " + $(this).val());
        var nSemanas = $("#txtNumSemanas").val();
        var semanasAtras = $("#txtNumHistoricos").val();
        var semanaPartida = $("#txtSemanaPartida").val();
        var anioPartida = $("#txtAnioPartida").val();

        PageMethods.calcularPorNiveles(nSemanas, semanasAtras, semanaPartida, anioPartida, function (response) {
            //window.console && console.log("calcular por Niveles! ");
            if (response != null && response != '') {
                var json = JSON.parse(response);
                if (genTablaNiveles(json)) {
                    //   verAdvertencia(json['N']);
                    $('#tblPronostico').show();
                }
            }

            calcular();
            initializeFooterValue();
            
            triggerReload();
        });

        bloqueoDePantalla.desbloquearPantalla();

    });

    $("#btnGuardar").click(function () {
        bloqueoDePantalla.bloquearPantalla(); ;
        //window.console && console.log("change! " + $(this).val());
        var nSemanas = $("#txtNumSemanas").val();
        var eficienciaHistorica = $("#txtNumHistoricos").val();
        var semanaPartida = $("#txtSemanaPartida").val();
        var anioPartida = $("#txtAnioPartida").val();

        var configuraciones = getJSONConfiguraciones();
        var totalSemana = getJSONSemanas();
        var detalle = getJSONDetalle();
        var fijos = getJSONFijos();
        var inactivos = getJSONInactivos();
        var desglose = getJSONDesglose();
        var targets = getJSONEficiencia();
        // (configuraciones, totalSemana, detalles, fijos, semanaPartida, anioPartida, nSemanas, eficienciaHistorica)
        PageMethods.guarda(configuraciones, totalSemana, detalle, fijos, inactivos, semanaPartida, anioPartida, nSemanas, eficienciaHistorica, desglose, targets, function (response) {
            //window.console && console.log("guarda! ");
            bloqueoDePantalla.desbloquearPantalla();
            if (response == "OK") {
                popUpAlert("Pronostico guardado correctamente.", "info");
            } else {
                popUpAlert("Problema al guardar, intentelo mas tarde.", "error");
            }
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        });
    });

    $('#btnNoConfigurados').click(function () {
        $('#tblInvernaderosNoConf').hide();
    });




    $('#tblPronostico').hide();
    $('#tblInvernaderosNoConf').hide();

    $('#btnGraficar').click(function () { 
        muestraCharts();    
    }) ;   

    var i = setInterval(function () {
        if (cargandoSemanas == false && cargandoAnios == false) {



            clearInterval(i);

            $("#txtAnioPartida").val((new Date).getFullYear()); //.change();
            $("#txtSemanaPartida").val($(".semana span").text()); //.change();
            $("#txtNumHistoricos").val("5");
            $("#txtNumSemanas").val("6");
            cargaConfiguracion();
        }
    }, 200);

        
    $('#btnCargaConfiguracion').click(function(){
//        var anio = $('#txtAnioPartida').val();
//        var totalSemana = $(this).find('option[value="' + anio + '"]').attr('ultimaSemana');
//        var semanaActual = $("#txtSemanaPartida").val();
//        var strSemanas = '';
//        if (totalSemana != undefined && totalSemana > 0) {
//            strSemanas += '';
//            for (cont = 1; cont <= totalSemana; cont++) {
//                strSemanas += '<option value="' + cont + '"'/* + (cont == parseInt($(".semana span").text()) ? 'selected' : '')*/ + '>' + cont + '</option>';
//            }

//            $("#txtSemanaPartida").empty().append(strSemanas).val(semanaActual);
//        }
//        console.log(totalSemana);
//    

       cargaConfiguracion();
    });


});

function cargaConfiguracion() {
        bloqueoDePantalla.bloquearPantalla();
        limpiarTablaPronosticos();
        var nSemanas = $('#txtNumSemanas').val();
        var nHistoricos = $('#txtNumHistoricos').val();
        var semanaPartida = $('#txtSemanaPartida').val();
        var anioPartida = $('#txtAnioPartida').val();
        //window.console && console.log("change! " + nSemanas);
        if (nSemanas != null && parseInt(nSemanas) > 0
                    && nHistoricos != null && parseInt(nHistoricos) > 0
                    && semanaPartida != null && parseInt(semanaPartida) > 0
                    && anioPartida != null && parseInt(anioPartida) > 0) {
            genTablaConfiguraciones(nSemanas, nHistoricos, semanaPartida, anioPartida);
        } else {
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        }
        bloqueoDePantalla.desbloquearPantalla();
        $(".configuracion").off('change');

}

function triggerReload() {
    $(".configuracion").off('change');
    $(".configuracion").change(function () {
        bloqueoDePantalla.bloquearPantalla();
        calcular();
        window.console && console.log("change! -Cacular");
        bloqueoDePantalla.desbloquearPantalla();
        initializeFooterValue();
    });
    
    
}

function getJSONFijos() {
    var JSONFijos = $('.divSemana[nsemana=0] .tableSemana tbody tr td.noOcultable[familia="F"][tipocelda="fijos"]').map(function () //
    {
        var fijos = $(this).find('input').val();
        return cabeza = {
            idLider: $(this).parent().attr("idLider"),
            idUsuario: $(this).parent().attr("idUsuario"),
            semana: $(this).attr('semana'),
            anio: $(this).attr('anio'),
            jornalesFijos: fijos
        }

    }).get();
    return JSONFijos;
}
function getJSONInactivos() {
    var JSONFijos = $('.divSemana[nsemana=0] .tableSemana tbody tr td.noOcultable[familia="I"][tipocelda="fijos"]').map(function () //
    {
        var inactivos = $(this).find('input').val();
        return cabeza = {
            idLider: $(this).parent().attr("idLider"),
            idUsuario: $(this).parent().attr("idUsuario"),
            semana: $(this).attr('semana'),
            anio: $(this).attr('anio'),
            jornalesInactivos: inactivos
        }

    }).get();
    return JSONFijos;
}

function getJSONDetalle() {
    var pronosticos = $('.divSemana[nsemana] .tableSemana tbody tr td.categoria').map(function () {
        return totales = {
            idLider: $(this).parent().attr('idLider'),
            idUsuario: $(this).parent().attr('idusuario'),
            nSemana: $(this).attr('nSemana'),
            semana: $(this).attr('semana'),
            anio: $(this).attr('anio'),
            idFamilia: $(this).attr('familia'),
            idCategoria: ($(this).attr('nivel') != 'C' && $(this).attr('nivel') != 'P' && $(this).attr('nivel') != 'T') ? $(this).attr('nivel') : 0,
            esCosecha: $(this).attr('nivel') == 'C' ? 1 : 0,
            esPreparacionSuelo: $(this).attr('nivel') == 'P' ? 1 : 0,
            jornales: $(this).find('span').text()
        }
    }).get();

    return pronosticos;
}

function getJSONSemanas() {
    var pronosticos = $('.divSemana[nsemana] .tableSemana tbody tr').map(function () {
        return totales = {
            idLider: $(this).attr('idLider'),
            idUsuario: $(this).attr('idUsuario'),
            nSemana: $(this).attr('nSemana'),
            semana: $(this).attr('semana'),
            anio: $(this).attr('anio'),
            fijos: $(this).find('td[familia="F"][nivel="T"] input').val() != null && $(this).find('td[familia="F"][nivel="T"] input').val() != undefined ? $(this).find('td[familia="F"][nivel="T"] input').val() : null,
            inactivos: $(this).find('td[familia="I"][nivel="T"] input').val(),
            totalJornales: $(this).find('td[familia="A"][nivel="T"] span').text(),
            totalGerente: $(this).find('td[familia="A"][nivel="G"] input').val() != null && $(this).find('td[familia="A"][nivel="G"] input').val() != undefined ? $(this).find('td[familia="A"][nivel="G"] input').val() : null,
            totalFinal: $(this).find('td[familia="A"][nivel="A"] input').val() != null && $(this).find('td[familia="A"][nivel="A"] input').val() != undefined ? $(this).find('td[familia="A"][nivel="A"] input').val() : null
        }
    }).get();
    return pronosticos;
}

function getJSONConfiguraciones() {
    var configuraciones = $('#tblConfiguraciones tbody tr#rowHora td input').map(function () {
        var nSemana = $(this).attr('nsemana');
        var semana = $(this).attr('semana');
        var anio = $(this).attr('anio');
        return conf = {
            semana: semana,
            anio: anio,
            ausentismo: $('tr#rowAusentismo td input[nsemana=' + nSemana + ']').val(),
            capacitacion: $('tr#rowCapacitacion td input[nsemana=' + nSemana + ']').val(),
            curva: $('tr#rowCurva td input[nsemana=' + nSemana + ']').val(),
            horas: $('tr#rowHora td input[nsemana=' + nSemana + ']').val()
        }
    }).get();

    return configuraciones;

}

function getJSONActividades() { return jsonEtapas;}

function getJSONDesglose() {

    var jsonDesglose = [];
    for (da in jsonDesgloseActividades) {
        var nSemana = parseInt( jsonDesgloseActividades[da].nSemana)
        var inv = jsonDesgloseActividades[da].invernadero
        var densidad = parseFloat(jsonDesgloseActividades[da].densidad)
        var cajas = parseFloat(jsonDesgloseActividades[da].cajas)
        var strVariedad = jsonDesgloseActividades[da].variedadHeader
        var idProd = parseInt(strVariedad != null && strVariedad != undefined && strVariedad.split('__').length == 2 ? strVariedad.split('__')[0] : 0)
        var idVariedad = parseInt(strVariedad != null && strVariedad != undefined && strVariedad.split('__').length == 2 ? strVariedad.split('__')[1] : 0)
        var edad = parseInt(jsonDesgloseActividades[da].edad)
        var idLider = parseInt(jsonDesgloseActividades[da].idUsuario)
        for (e in jsonEtapas) {
            var hhstr = jsonDesgloseActividades[da][jsonEtapas[e].clave]
            var hh = parseFloat(hhstr != null && hhstr != undefined && hhstr.split("/").length == 2 ? hhstr.split("/")[0] : 0.00);
            var rep = parseFloat(hhstr != null && hhstr != undefined && hhstr.split("/").length == 2 ? hhstr.split("/")[1] : 0.00);
            var idEtapa = jsonEtapas[e].idEtapa
            var esCosecha = jsonEtapas[e].esCosecha
            var jor = 0.00;
            if (jsonDesgloseActividades[da][jsonEtapas[e]['clave']] != null && jsonDesgloseActividades[da][jsonEtapas[e]['clave']] != undefined) {

                jor = 0;
                var hj = $('tr#rowHora td input[nsemana=' + nSemana + ']').val();
                var pa = $('tr#rowAusentismo td input[nsemana=' + nSemana + ']').val();
                var pc = $('tr#rowCapacitacion td input[nsemana=' + nSemana + ']').val();
                var cv = $('tr#rowCurva td input[nsemana=' + nSemana + ']').val();

                jor = esCosecha ? hh * (cv / 100) : hh;
                jor = (jor + (jor * (pa / 100)) + (jor * (pc / 100))) / hj;
                jor = jor.toFixed(2);
            }
            if (rep > 0) {
                jsonDesglose.push({
                    idUsuario: idLider
                   , idVariedad: idVariedad
                   , idProducto: idProd
                   , invernadero: inv
                   , edad: edad
                   , densidad: densidad
                   , cajas: cajas
                   , idEtapa: idEtapa
                   , esCosecha: esCosecha
                   , nSemana: nSemana
                   , repeticiones: rep
                   , horas: hh
                   , jornales: parseFloat(jor)
                })
            }

        }
    }
   return jsonDesglose;
}

function getJSONEficiencia() {
    var jsonTarget = [];
    for (t in jsonEficiencias) {
        var idUsuario = jsonEficiencias[t].idUsuario
        var idEtapa = jsonEficiencias[t].idEtapa
        for (v in jsonVariedades) {
            var vstr = jsonEficiencias[t][jsonVariedades[v].header]
            var targets = vstr == null ? null : vstr.split('/')
            var targetDefault = vstr == null ? 0.0 : parseFloat(targets[0])
            var targetInterplanting = vstr == null ? 0.0 : parseFloat(targets[1])
            var eficiencia = vstr == null ? 0.0 : parseFloat(targets[2])
            var idProducto = jsonVariedades[v].idProducto
            var idVariedad = jsonVariedades[v].idVariedad
            if (vstr != null && vstr != undefined) {
                jsonTarget.push({
                    idUsuario: idUsuario
                 , idEtapa: idEtapa
                 , idProducto: idProducto
                 , idVariedad: idVariedad
                 , eficiencia: (eficiencia == 0.0000 ? targetDefault : eficiencia)
                })
            }
        }
    }
    return jsonTarget
}

function genTablaConfiguraciones(nSemanas, nHistoricos, semanaPartida, anioPartida) {
    var idplanta = 0;
    if(undefined != $('#ctl00_ddlPlanta')){
        idplanta = $('#ctl00_ddlPlanta').val();
    }
    
    PageMethods.generarConfiguracion(nSemanas, nHistoricos, semanaPartida, anioPartida, idplanta, function (response) {
        //window.console && console.log("JSON " + response);
        var json = JSON.parse(response);
        var code = " <table id=\"tblConfiguraciones\">";
        code += genHeaderSemanas(json['C']);
        code += genTextRow(nSemanas, STR_HORA, json['C']);
        code += genTextRow(nSemanas, STR_AUSENTISMO, json['C']);
        code += genTextRow(nSemanas, STR_CAPACITACION, json['C']);
        code += genTextRow(1, STR_CURVA, json['C']);
        code += "</table>";
        $("#divOpcionesDinamicas").html(code);
        
        $('.trConfiguraciones').show();

        if (primeraVez == 0) {
            verAdvertencia(json['N']);
            primeraVez = 1;
        }
//        $(".configuracion").change(function () {
//            bloqueoDePantalla.bloquearPantalla();
//            calcular();
//            window.console && console.log("change! -Cacular");

//            initializeFooterValue();
//            bloqueoDePantalla.desbloquearPantalla();
//        });
        bloqueoDePantalla.indicarTerminoDeTransaccion();
    });

}

function genHeaderSemanas(json) {
  //  var nSemana = $("#txtSemanaPartida").val();
    var code = "<tr>";
    var cont = 0;
    while (cont < json.length) {
     //   nSemana = (nSemana == 52 ? 0 : nSemana);
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
        code += "<td><input type=\"text\" class=\"txtConfiguracion configuracion floatValidate\" " /*/id=\"txtNumSemanas\"*/ + " value=\""
                        + (name == STR_HORA ? json[cont].hora : (name == STR_AUSENTISMO ? json[cont].ausentismo : (name == STR_CAPACITACION ? json[cont].capacitacion : (name == STR_CURVA ? json[cont].curva : getConstante(name)))))
                        + "\" nSemana=\"" + json[cont].cont + "\" semana=\"" + json[cont].semanaNS + "\" anio=\"" + json[cont].anioNS + "\"/></td>";
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


function muestraEficiencias(idUsuario, lider) {
    console.log('click lider' + idUsuario);
    var idProducto = 0;

    var productos = '';
    var variedades = '';
    var row = '';
    for (va in jsonVariedades) {
        variedades += "<td\>" + jsonVariedades[va]['variedad'] + "</td>";
        if (idProducto == 0 || idProducto != parseInt(jsonVariedades[va]['idProducto'])) {
            idProducto = parseInt(jsonVariedades[va]['idProducto']);
            productos += "<td colspan=\"" + jsonVariedades[va]['totalVariedades'] + "\">" + jsonVariedades[va]['segmento'] + "</td>";
        }
    }

    for (ef in jsonEficiencias) {
        var idHabilidad = jsonEficiencias[ef]['idHabilidad'];
        var idEtapa = jsonEficiencias[ef]['idEtapa'];
        var habilidad = jsonEficiencias[ef]['Habilidad'];
        var etapa = jsonEficiencias[ef]['Etapa'];
        var idCategoria = jsonEficiencias[ef]['idCategoria'];
        var categoria = '';
        var esCosecha = jsonEficiencias[ef]['esCosecha'];
        for (cat in jsonCategorias) {
            if ((idCategoria == jsonCategorias[cat]['claveCat'] && esCosecha == 0) || (esCosecha == 1 && jsonCategorias[cat]['claveCat'] == 'C')) {
                categoria = jsonCategorias[cat]['categoria']
                break;
            }
        }
        if (idUsuario == parseInt(jsonEficiencias[ef]['idUsuario'])) {
            var strEficiencias = '';
            for (va in jsonVariedades) {
                var header = jsonVariedades[va]['header'];
                if (header != null && header != undefined && header != undefined) {
                    var str = jsonEficiencias[ef][header];
                    var targets = str == null ? null : str.split('/');
                    var targetDefault = str == null ? 0.0 : parseFloat(targets[0]);
                    var targetInterplanting = str == null ? 0.0 : parseFloat(targets[1]);
                    var eficiencia = str == null ? 0.0 : parseFloat(targets[2]);
                    strEficiencias += "<td id=\"eficienciaVariedad\" idVariedad=\"" + jsonVariedades[va]['idVariedad'] + "\"  targetDefault=\"" + targetDefault
                                                        + "\" targetInterplantingDefault=\"" + targetInterplanting + "\" eficienciaDefault=\"" + eficiencia
                                                        + "\"><span>" + (eficiencia == 0.0000 ? targetDefault : eficiencia) + "</span></td>";
                }
            }
            row += "<tr idHabilidad=\"" + idHabilidad + "\" idEtapa=\"" + idEtapa + "\">";
            row += "<td>" + habilidad + "</td>";
            row += "<td>" + etapa + "</td>";
            row += "<td>" + categoria + "</td>";
            row += strEficiencias;
            row += "</tr>";
        }
    } // end for jsonEficiencias
    var table = "<table class=\"tblEficiencia\" idUsuario=\"" + idUsuario + "\"><thead><tr><td rowspan=\"2\">ACTIVIDAD</td><td rowspan=\"2\">ETAPA</td><td rowspan=\"2\">CATEGORÍA</td>";
    table += productos
    table += "</tr><tr>";
    table += variedades;
    table += "</tr>";
    table += "</thead><tbody>";
    table += row;
    table += "</tbody></table>";


    var code = "";
    code = "<table width=\"400px\" style=\"display: table;\"><tbody>"
        + "<tr><td>"
        + "<span>Configuración de Eficiencias de " + lider + ":</span></td></tr>"
        + "<tr><td>"
        + "<div class=\"scrollable\" style=\"width: 400px; height: 200px; overflow-y: scroll;\">"
        + table
        + "</div>"
        + "</td></tr>"
        + "</tbody></table>"

    popUpAlert(code, "info");

    if ($('div#eficiencias>table.tblEficiencia[idUsuario=\"' + idUsuario + '\"]').length == 0) {
        $('div#eficiencias').append(table);
    }
}


function genRowsLider(json, ckeck) {
    var semanaActual = $("#txtSemanaPartida").val();
    var code = "";
    var n = 0;
    while (n < Object.keys(json).length) {

        code += "<tr class=\"" + ((ckeck == true)? "ckRowLider" : "rowLider") +"\" row=\"" + n + "\" idLider=\"" + json[n]["idLider"] + "\" idUsuario=\"" + json[n]["idUsuario"] + "\" asociados=\"" + json[n]['asociados'] + "\""
        code += (ckeck == false) ? " onclick=\"muestraEficiencias(" + json[n]["idUsuario"] + ", '" + json[n]["nombreLider"] + "')\"" : " "
        code += ">";
        code += (ckeck == true) ? "<td><input type=\"checkbox\" idUsuario=\"" + json[n]["idUsuario"] + "\" class=\"ckLider\" onclick=\"mostrarOcultarLider(this);recalcularFoot();\" checked /></td>" : "<td><span>" + json[n]["idLider"] + "</span></td>"
        code += "<td  class=\"clickable\"><span>" + json[n]["nombreLider"] + "</span></td>"
              + "</tr>";
        n++;
    }

    return code;
}

function getNombrefamilias(jsonNiveles) {
    var familias = [];
    for (nivel in jsonNiveles) {
        if (familias.indexOf(jsonNiveles[nivel]['nombreFamilia']) < 0) {
            familias.push(jsonNiveles[nivel]['nombreFamilia']);
        }
    }

    return familias;
}
function getTotalFamilias(jsonNiveles) {
    var familias = getNombrefamilias(jsonNiveles); //[];
    return familias.length;
}

function getTotalCategorias(jsonNiveles) {
    var suma = 0;
    for (nivel in jsonNiveles) {
        suma += parseInt(jsonNiveles[nivel]['totalCategorias']);
    }

    return suma;
}

function getTotalNivelesPorFamilia(idFamilia) {
    var cont = 0;
    for (nivel in jsonNiveles) {
        if (idFamilia == jsonNiveles[nivel]['idFamilia']) {
            cont = parseInt(jsonNiveles[nivel]['totalCategorias']);
            break;
        }
    }

    return cont;
}

function getArrayFamilia(jsonNiveles) {
    familias = [];
    for (nivel in jsonNiveles) {
        if (familias.length == 0) {
            familias.push([]);
            familias[0].push(jsonNiveles[nivel]['idFamilia']);
            familias[0].push(jsonNiveles[nivel]['nombreFamilia']);
            familias[0].push(parseInt(jsonNiveles[nivel]['totalCategorias']));
        } else {
            var existe = 0;
            for (index in familias) {
                //console.log('index: ' + index);
                if (familias[index][0] == jsonNiveles[nivel]['idFamilia']) {
                    existe = 1;
                    break;
                }
            }
            if (existe == 0) {
                familias.push([]);
                familias[familias.length - 1].push(jsonNiveles[nivel]['idFamilia']);
                familias[familias.length - 1].push(jsonNiveles[nivel]['nombreFamilia']);
                familias[familias.length - 1].push(parseInt(jsonNiveles[nivel]['totalCategorias']));
            }
        }
    }
    return familias;
}

function genTablaSemanasNiveles(nSemanas, jsonNiveles, jsonSemanas) {
    var nSemanas = nSemanas++;
    var totalNiveles = getTotalCategorias(jsonNiveles);
    var totalFamilias = getTotalFamilias(jsonNiveles);
    //suma de campos target&historico de niveles + campos para total de target&historico por familas + campos para total de target & historico
    var totalColumnas = totalNiveles + totalFamilias + 1; //TARGETS
    //var totalColumnas = (totalNiveles * 2) + (totalFamilias * 2) + (2); //TARGETS HISTORICOS
    var semanaActual = $("#txtSemanaPartida").val();
    var semanaActualT = $("#txtSemanaPartida").val();
    var code = "<table class=\"semanas\"><tr>";
    var n = 0;

    var familias = getArrayFamilia(jsonNiveles);

    var anioPartida = $('#txtAnioPartida').val();


    while (n <= nSemanas) {
        
        var strHeader = '';
        var strRow = '';
        if (n == 0) {
            strHeader += "<td nSemana=\"" + n + "\" semana=\"" + semanaActual + "\" anio=\"" + (n == 0 ? anioPartida : $('table#tblConfiguraciones td[nsemana = ' + n + ']').attr('anio')) + "\" familia=\F\" colspan=\"1\"><span>Disponibles</span></td>"; //TARGET
            strHeader += "<td nSemana=\"" + n + "\" semana=\"" + semanaActual + "\" anio=\"" + (n == 0 ? anioPartida : $('table#tblConfiguraciones td[nsemana = ' + n + ']').attr('anio')) + "\" familia=\F\" colspan=\"1\"><span>Inactivos</span></td>"; //TARGET
            strRow += "<td semana=\"" + semanaActual + "\" familia=\"F\" nivel=\"T\" colspan=\"1\" class=\"noOcultable\"><span>TOTAL</span></td>";
            strRow += "<td semana=\"" + semanaActual + "\" familia=\"F\" nivel=\"T\" colspan=\"1\" class=\"noOcultable\"><span>TOTAL</span></td>";


        } else {
            semanaActual = /*jsonSemanas[n - 1]['anio'] + '-' + */jsonSemanas[n - 1]['semana'];
            semanaActualT = jsonSemanas[n - 1]['anio'] + '-' + jsonSemanas[n - 1]['semana'];
        }

        strHeader += "<td nSemana=\"" + n + "\" semana=\"" + semanaActual + "\" anio=\"" + (n == 0 ? anioPartida : $('table#tblConfiguraciones td[nsemana = ' + n + ']').attr('anio')) + "\" familia=\F\" colspan=\"1\"><span>Fijos</span></td>"; //TARGET
        strRow += "<td semana=\"" + semanaActual + "\" familia=\"F\" nivel=\"T\" colspan=\"1\" class=\"noOcultable\"><span>TOTAL</span></td>";
       

        for (indexFamila in familias) {
            strHeader += "<td nSemana=\"" + n + "\" semana=\"" + semanaActual + "\" anio=\"" + (n == 0 ? anioPartida : $('table#tblConfiguraciones td[nsemana = ' + n +']').attr('anio')) + "\" familia=\"" + familias[indexFamila][0] + "\" colspan=\"" + ( (n != 0 ? familias[indexFamila][2] : 0) + 1) + "\"><span>" + "Familia " + familias[indexFamila][1] + "</span></td>"; //TARGET
        }
        strHeader += "<td nSemana=\"" + n + "\" semana=\"" + semanaActual + "\" familia=\"A\" colspan=\"" + (n == 0 ? 1 : n >= 1 ? 4 : 2) + "\"><span>ACTIVOS</span></td>"; //TARGET
        
        var indexFam = 1;
        var contCategoria = 0;
      //  var idFam= 0;
        //sub titulos
        for (nivel in jsonNiveles) {
            if (jsonNiveles[nivel]['totalCategorias'] > 0) {
                contCategoria++;
                if (n != 0) {
                    strRow += "<td semana=\"" + semanaActual + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" nivel=\"" + jsonNiveles[nivel]['claveCat'] + "\" colspan=\"1\" class=\"ocultable \"><span>" + (jsonNiveles[nivel]['categoria'] != '' ? jsonNiveles[nivel]['categoria'] : ('Nivel ' + jsonNiveles[nivel]['Nivel'])) + "</span></td>";
                }
            }
           // if ((idFam != parseInt(jsonNiveles[nivel]['idFamilia'] ) && indexFam == parseInt(jsonNiveles[nivel]['totalCategorias']) ) || parseInt(jsonNiveles[nivel]['totalCategorias']) == 0) {
             if (contCategoria == parseInt(jsonNiveles[nivel]['totalCategorias'])){
                 strRow += "<td semana=\"" + semanaActual + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" nivel=\"T\" colspan=\"1\" class=\"noOcultable " + (n != 0 && parseInt(jsonNiveles[nivel]['totalCategorias']) != 0 ? "clickable" : "") + "\"><div class=\"" + (n != 0 ? ""/*"clickable"*/ : "") + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" todos=\"1\"><span>TOTAL</span></div></td>";
                 if (indexFam < familias.length) {
                  //  indexFam += familias[indexFam][2];
                    ++indexFam;
                    contCategoria= 0;
                 //  idFam = parseInt(jsonNiveles[nivel]['idFamilia']);
                }

            }
        }

        strRow += "<td semana=\"" + semanaActual + "\" familia=\"A\" nivel=\"T\" colspan=\"1\" class=\"noOcultable total\"><span>TOTAL</span></td>";
      //  if (n == 1) {
    //   }
        if (n >= 1) {
            strRow += "<td semana=\"" + semanaActual + "\" familia=\"A\" nivel=\"G\" class=\"noOcultable\"><span>Gerente</span></td>";
            strRow += "<td semana=\"" + semanaActual + "\" familia=\"A\" nivel=\"A\" class=\"noOcultable\"><span>Final</span></td>";
            strRow += "<td semana=\"" + semanaActual + "\" familia=\"A\" nivel=\"D\" class=\"noOcultable\"><span>DIFERENCIA</span></td>";
        }




        code += "<td><div class=\"divSemana\" nSemana=\"" + n + "\" semana=\"" + semanaActual + "\">";
        code += "  <table  class=\"tableSemana  tablePronostico\">";
        code += "       <thead>";
        code += "           <tr class=\"superHeader\" nSemana=\"" + n + "\" >";
        code += "               <td colspan=\"" + ((n >= 1? 2: 3) + (n >= 1 ? (totalColumnas + 2) : totalColumnas)) + "\"><span>" + (n == 0 ? "Actual" :"Semana " + semanaActualT) + "</span></td>";
        code += "           </tr>";
        code += "           <tr class=\"header\" nSemana=\"" + n + "\"  semana=\"" + semanaActual + "\">";
        code += strHeader;
        code += "           </tr>";
        code += "           <tr class=\"subHeader\" nSemana=\"" + n + "\" semana=\"" + semanaActual + "\" >";
        code += strRow;
        code += "           </tr>";
        code += "       </thead>";
        code += "       <tbody>";
        code += "       </tbody>";
        code += "       <tfoot>";
        code += "       </tfoot>";
        code += "   </table>";
        code += "</div></td>";
        ++n;

     //   semanaActual = semanaActual == 52 ? 1 : ++semanaActual;


    }
    code += "</tr></table>";
    return code;
}

function genRowsSemanaNiveles(tbody, tfoot, nSemana, semana, jsonNiveles, jsonSemanas, jsonFijos, jsonLideres, jsonActual, jsonFamilia) {
    var totalNiveles = jsonNiveles.length;
    var totalFamilias = getTotalFamilias(jsonNiveles);
    //suma de campos target&historico de niveles + campos para total de target&historico por familas + campos para total de target & historico
    var totalColumnas = totalNiveles + totalFamilias + (1); //TARGETS TOTAL NIVELES MAS SUMA DE COLS TOTALESXFAMILIA + COLUMNA ACTIVOS
    var semanaPartida = $("#txtSemanaPartida").val();
    var semanaActual = semanaPartida;
    
    var familias = getArrayFamilia(jsonNiveles);

    var anio = nSemana == 0 ? $('#txtAnioPartida').val() : $('table#tblConfiguraciones td[nsemana = ' + nSemana + ']').attr('anio');

    var code = "";
    var footer = "";
    var fijos = 0;
    var ausentes = 0;
    var footercount = 0;

    if (nSemana == 0) {
        if (jsonLideres != null && jsonLideres != undefined) {

            for (l in jsonLideres) {
                var idLider = jsonLideres[l]['idLider'];
                var idUsuario = jsonLideres[l]['idUsuario']
                var ausentes = 0 //jsonLideres[l]['incapacitados'];
                var asociados = jsonLideres[l]['asociados'];

                var strRow = '';
                var sumTotal = 0;
                
                if (jsonFijos != null && jsonFijos != undefined) {
                    for (f in jsonFijos) {
                        if (jsonFijos[f]['lider'] == idLider && jsonFijos[f]['usuario'] == idUsuario) {
                            fijos = jsonFijos[f]['fijos'];
                            sumTotal += fijos;
                            break;
                        }
                    }
                }

                code = "<tr  row=\"" + l + "\" idLider=\"" + idLider + "\" idUsuario=\"" + idUsuario + "\"  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\">";
                strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"D\" nivel=\"T\" tipoCelda=\"disponibles\"  JD=\"" + asociados + "\"  class=\"noOcultable\"><span>" + asociados + "</span></td>";
                strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"I\" nivel=\"T\" tipoCelda=\"fijos\"  JA=\"" + ausentes + "\"  class=\"noOcultable\"><input type=\"text\" class=\"txtConfiguracion floatValidate\" value=\"0\"></td>";
                strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"F\" nivel=\"T\" tipoCelda=\"fijos\"  JF=\"" + fijos + "\"  class=\"noOcultable\"><input type=\"text\" class=\"txtConfiguracion floatValidate\" value=\"" + fijos + "\"  onchange=\"changeFijoActual(" + l + ");\" ></td>";

              

                if (jsonActual != null && jsonActual != undefined) {
                    for (a in jsonActual) {
                        if (/*jsonActual[a]['idLider'] == idLider && */ jsonActual[a]['idUsuario'] == idUsuario) {
                            if (jsonFamilia != null && jsonFamilia != undefined) {
                                for (f in jsonFamilia) {

                                    var familia = '' + jsonFamilia[f]['idFamilia'];
                                    var jornalFamilia = jsonActual[a][familia] != null && jsonActual[a][familia] != 'null' ? jsonActual[a][familia] : 0;
                                    sumTotal += jornalFamilia
                                    strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"" + familia + "\" nivel=\"T\" tipoCelda=\"fijos\"  JT=\"" + jornalFamilia + "\"  class=\"noOcultable categoria\"><span>" + jornalFamilia + "</span></td>";
                                }
                            }
                            break;
                        }
                    }
                }

                strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"T\" tipoCelda=\"target\" class=\"noOcultable total\"><span>" + sumTotal + /* "/" + $('.rowLider[idLider="' + +jsonSemanas[n]["idLider"] + '"][idUsuario="' + jsonSemanas[n]["idUsuario"] + '"]').attr('asociados') +*/"</span></td>";
                code += strRow;
                code += "</tr>";

                $(tbody).append(code);
            }

            footer = "<tr  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" class=\"footerSemana\">";
            footer += "<td  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"D\"  nivel=\"T\" class=\"noOcultable\"><span>0</span></td>";
            footer += "<td  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"I\"  nivel=\"T\" class=\"noOcultable\"><span>0</span></td>";
            footer += "<td  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"F\"  nivel=\"T\" class=\"noOcultable\"><span>0</span></td>";
            if (jsonFamilia != null && jsonFamilia != undefined) {
                for (f in jsonFamilia) {
                    var familia = '' + jsonFamilia[f]['idFamilia'];
                    var jornalFamilia = jsonActual[a][familia] != null && jsonActual[a][familia] != 'null' ? jsonActual[a][familia] : 0;
                    footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"" + familia + "\" nivel=\"T\" tipoCelda=\"fijos\" class=\"noOcultable categoria\"><span>0</span></td>";
                }
            }
            footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"T\" tipoCelda=\"target\" class=\"noOcultable total\"><span>0</span></td>";
            footer += "</tr>";

            $(tfoot).append(footer);
        }

    }
    else {
        var rowCont = 0
        var semanaCont = 0;
        for (n in jsonSemanas) {
            var idLider = jsonSemanas[n]["idLider"];
            var idUsuario = jsonSemanas[n]["idUsuario"];
            var strRow = '';
            var indexFam = 1;
            var contFam = familias[0][2];
            var valTotalTarget = 0.00;
            var valTotalHistorico = 0.00;
            var valSubTotalTarget = 0.00;
            var valSubTotalHistorico = 0.00;

            if (nSemana == jsonSemanas[n]['nSemana']) {

                if (jsonFijos != null && jsonFijos != undefined) {
                    for (f in jsonFijos) {
                        if (jsonFijos[f]['lider'] == idLider && jsonFijos[f]['usuario'] == idUsuario) {
                            fijos = jsonFijos[f]['fijos'];
                            sumTotal += fijos;
                            break;
                        }
                    }
                } else {
                    fijos = 0;
                }
                code = "<tr  row=\"" +/* n */ rowCont + "\" idLider=\"" + idLider + "\" idUsuario=\"" + idUsuario + "\"  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\">";
                strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"F\" nivel=\"T\" tipoCelda=\"fijos\"  JF=\"" + fijos + "\"  class=\"noOcultable\"><input type=\"text\" class=\"txtConfiguracion floatValidate\" value=\"" + fijos + "\"  onchange=\"changeFijos(" + /* n */rowCont++ + "," + nSemana + ");\"></td>";

                footer = "<tr  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" class=\"footerSemana\">";
                footer += "<td  anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"F\"  nivel=\"T\" class=\"noOcultable\"><span>0</span></td>";
            
                for (nivel in jsonNiveles) {

                    var strjornales = jsonSemanas[n][jsonNiveles[nivel]['col']];
                    var nCategorias = parseInt(jsonNiveles[nivel]['totalCategorias']);
                    var valTarget = 0.00;
                    var valHistorico = 0.00;
                    if (strjornales != null) {
                        valTarget = parseFloat(strjornales);
                    }
                    valSubTotalTarget += valTarget;
                    //   valSubTotalHistorico += valHistorico;
                    if (nCategorias > 0) {
                        strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" nivel=\"" + jsonNiveles[nivel]['claveCat'] + "\" tipoCelda=\"target\"  hh=\"" + valTarget + "\"  class=\"ocultable categoria\"><span>" + /*Math.ceil*/Math.round(valTarget) + "</span></td>";
                        if (footercount == 0) {
                            footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" nivel=\"" + jsonNiveles[nivel]['claveCat'] + "\" tipoCelda=\"target\" class=\"ocultable categoria\"><span>0</span></td>";
                        
                        }
                    }
                    if (nivel == (jsonNiveles.length - 1) || jsonNiveles[parseInt(nivel)]['idFamilia'] != jsonNiveles[parseInt(nivel) + 1]['idFamilia']) { 
                        valTotalTarget += valSubTotalTarget;
                        strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" nivel=\"T\" class=\"noOcultable \"><span>" + /*Math.ceil*/Math.round(valSubTotalTarget) + "</span></td>";
                        if (footercount == 0) {
                            footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"" + jsonNiveles[nivel]['idFamilia'] + "\" nivel=\"T\" class=\"noOcultable \"><span>0</span></td>";
                        }
                        if (indexFam < familias.length) {
                            contFam += familias[indexFam][2];
                            ++indexFam;
                        }

                        valSubTotalTarget = 0;
                        valSubTotalHistorico = 0;
                    }
                }

                // generando rows de total de familias y generando celdas de gerente y final para
                strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"T\" tipoCelda=\"target\" class=\"noOcultable total\" onclick=\"genDesgloseJornales(" + idUsuario + "," + nSemana + ")\"><span " + (valTotalTarget > 0 ? "class=\"clickable\"" : "") + ">" + /*Math.ceil*/Math.round(valTotalTarget) + "</span></td>";
                if (footercount == 0) {
                    footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"T\" tipoCelda=\"target\" class=\"noOcultable total\"><span>0</span></td>";
                }
                if (/*(semanaPartida == 52 && semana == 1) || (semana == (parseInt(semanaPartida) + 1))*/nSemana >= 1) {
                    strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"G\" class=\"noOcultable total\"><input type=\"text\" class=\"txtConfiguracion floatValidate\" value=\"0\"></td>";
                    strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"A\" class=\"noOcultable total\"><input type=\"text\" class=\"txtConfiguracion floatValidate\" value=\"0\"></td>";
                    if (footercount == 0) {
                        footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"G\" class=\"noOcultable total\"><span>0</span></td>";
                        footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"A\" class=\"noOcultable total\"><span>0</span></td>";
                    }
                }

                if (/*(semanaPartida == 52 && semana == 1) || (semana == (parseInt(semanaPartida) + 1))*/nSemana >= 1) {
                    strRow += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"D\" class=\"noOcultable total\"><span class=\"floatValidate\" >0</span></td>";
                    if (footercount == 0) {
                        footer += "<td anio=\"" + anio + "\"  semana=\"" + semana + "\"  nSemana=\"" + nSemana + "\" familia=\"A\" nivel=\"D\" class=\"noOcultable total\"><span>0</span></td>";
                    }
                }
                code += strRow;

                code += "</tr>";
                $(tbody).append(code);
            }


        }

        footercount++;
        footer += "</tr>";
        $(tfoot).append(footer);

    }
}



function genTablaLideres() {
    var n = 0;
    var code = "<table class=\"tableLideres  tablePronostico\"><thead>"
                    + "<tr class=\"superHeader\"><td colspan=\"4\">&nbsp;</td></tr>"
                    + "<tr class=\"header\">"
                    + " <td><span>CÓDIGO</span></td><td><img src=\"../comun/img/filtro.png\" alt=\"Filtrar\"  class=\"filtrarLideres\" onclick=\"showFiltrarLider()\"/><span>Lider</span></td>"
                    + "</tr></thead><tfoot><tr><td><span></span></td><td><span>TOTALES</span></td></tr></tfoot></table>";
    return code;
}

function limpiarTablaPronosticos() {
    $("#divLideres").html("");
    $("#divSemanas").html("");

    $('#tblPronostico').hide();
}

function genTablaNiveles(json) {

    //window.console && console.log("genTablaNiveles!");
    limpiarTablaPronosticos();
    var strConfiguraciones = '';
    if(json != null) {
    
        strConfiguraciones += ( json['PE'] == undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'plan de ejecución importado desactualizado')
        strConfiguraciones += ( json['L'] != undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'Lideres-Asociados')
        strConfiguraciones += ( json['V'] != undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'Variedades')
        strConfiguraciones += (  json['L'] != undefined  &&  json['PE'] == undefined && json['E'] == undefined ?  (strConfiguraciones.length > 0 ? ' ,':'') + 'Eficiencias' : '')
        strConfiguraciones += ( json['C'] != undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'Categorias de Etapa')
        strConfiguraciones += ( json['H'] != undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'Habilidades')
        strConfiguraciones += ( json['D'] != undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'Semanas NS')
        strConfiguraciones += ( json['F'] != undefined ? '': (strConfiguraciones.length > 0 ? ' ,':'') + 'Familias');
    


    
    }

    if (json != null 
        && json != undefined 
        && json['V'] != undefined
        && json['C'] != undefined
        && json['L'] != undefined
        && json['H'] != undefined
        && json['D'] != undefined 
        && json['F'] != undefined 
        && Object.keys(json).length >= 6) {

        jsonVariedades = json['V'];
        jsonEficiencias = json['E'];
        jsonCategorias = json['C'];
        jsonLideres = json['L'];
        jsonEtapas = json['H'];
        jsonDesgloseActividades = json['HJ'];

       // $("#divListaLider table tbody").html(genRowsLider(json['L'], true));

        $("#divLideres").html(genTablaLideres()).find(".tableLideres").append(genRowsLider(json['L'], false));
        $("#divSemanas").html(genTablaSemanasNiveles($("#txtNumSemanas").val(), jsonCategorias, json['D'])).find(".tableSemana").each(function () { // descontando item de lideres ('L')
            var nSemana = $(this).parent().attr("nsemana");
            var semana = $(this).parent().attr("semana");
            //window.console && console.log("num " + numSemana);
            genRowsSemanaNiveles($(this).find('tbody'), $(this).find('tfoot'), nSemana, semana, json['C'], json['S'], json['J'], json['L'], json['A'], json['F']);
        });

        $('tr.subHeader td.noOcultable.clickable[nivel="T"]').click(function () {
            //console.log('familia:' + $(this).attr('familia'));
            //console.log('semana:' + $(this).parent().attr('semana'));
            var ocultables = $('tr.subHeader[nsemana="' + $(this)/**/.parent().attr('nsemana') + '"] td[familia="' + $(this).find('div').attr('familia') + '"].ocultable').length;
            //var ocultables = $('tr.subHeader[semana="' + $(this).parent().attr('semana') + '"] td[familia="' + $(this).attr('familia') + '"].ocultable').length * 2;
            var colActualesHeader = $(this)/*.parent()*/.parent().parent().find('.header[nsemana="' + $(this)/*.parent()*/.parent().attr('nsemana') + '"] td[familia="' + $(this).find('div').attr('familia') + '"]').attr('colspan');
            var colActualesSuper = $(this)/*.parent()*/.parent().parent().find('tr.superHeader td').attr('colspan');
            var colspanHeader = 0;
            var colspanSuper = 0;
            //console.log('ocultables: ' + ocultables)
            //console.log('colActualesHeader:' + colActualesHeader);
            //console.log('colActualesSuper :' + colActualesSuper);
            if ($(this).find('div').attr('todos') == 1) {
                colspanHeader = parseInt(colActualesHeader) - parseInt(ocultables);
                colspanSuper = parseInt(colActualesSuper) - parseInt(ocultables);
                $('tr[nsemana="' + $(this)/*.parent()*/.parent().attr('nsemana') + '"]').find('.ocultable[familia="' + $(this).find('div').attr('familia') + '"]').hide();
                $(this).find('div').attr('todos', '0');
                $(this).find('div').removeClass('colapsar');
                $(this).find('div').addClass('expandir');
            } else {
                colspanHeader = parseInt(colActualesHeader) + parseInt(ocultables);
                colspanSuper = parseInt(colActualesSuper) + parseInt(ocultables);
                $('tr[nsemana="' + $(this)/*.parent()*/.parent().attr('nsemana') + '"]').find('.ocultable[familia="' + $(this).find('div').attr('familia') + '"]').show();
                $(this).find('div').attr('todos', '1');
                $(this).find('div').removeClass('expandir');
                $(this).find('div').addClass('colapsar');
            }
            $(this)/*.parent()*/.parent().parent().find('.header[nsemana="' + $(this)/*.parent()*/.parent().attr('nsemana') + '"] td[familia="' + $(this).find('div').attr('familia') + '"]').attr('colspan', colspanHeader);
            $(this)/*.parent()*/.parent().parent().find('tr.superHeader td').attr('colspan', colspanSuper);
        }).click();
        
        if (strConfiguraciones.length > 0 ) {
        
            popUpAlert("Existen configuraciones pendientes en el sistema." + (strConfiguraciones.length > 0 ? " <br/> Compruebe la siguiente información: " + strConfiguraciones : ""), "Error");
        }
        return true;

    } else {
        popUpAlert("No se pudieron obtener los datos necesarios para generar el Pronostico." + (strConfiguraciones.length > 0 ? " <br/> Compruebe la siguiente información: " + strConfiguraciones : ""), "Error");
        return false;
    }
}

function verAdvertencia(jsonNoConf) {
   

    if (jsonNoConf != null && jsonNoConf != undefined && Object.keys(jsonNoConf).length > 0) {
        var n = 0;
        var code = "";
         code = "<table width=\"400px\" style=\"display: table;\"><tbody>"
            + "<tr><td>"
            + "<span>ADVERTENCIA: Existen configuraciones pendientes de surcos o densidades en algunos Invernaderos de esta planta, si continúa, toda la informacón relacionada con estos invernaderos podria no ser tomada en cuenta</span></td></tr>"
            + "<tr><td>"
            + "<div class=\"scrollable\" style=\"width: 400px; height: 200px; overflow-y: scroll;\">"
            + "<table><thead><tr><td colspan=\"3\"><span>INVERNADEROS NO CONFIGURADOS</span></td></tr><tr><td>INVERNADERO</td><td>SURCOS</td><td>DENSIDAD</td></tr></thead><tbody>";

        while (n < Object.keys(jsonNoConf).length) {

            code += "<tr row=\"" + n + "\">"
                    + "<td><span>" + jsonNoConf[n]['InvernaderoNoConf'] + "</span></td>"
                    + "<td><span>" + jsonNoConf[n]['Surcos'] + "</span></td>"
                    + "<td><span>" + jsonNoConf[n]['Densidad'] + "</span></td>"
                    + "</tr>";
         //   $('table#tblInvernaderosNoConf table>tbody').append(code);
            n++;
        }
        code += "</tbody>"
            + "</table></div>"
            + "</td></tr>"
            + "</tbody></table>"

        popUpAlert(code, "warning");

    }
}

function changeFijoActual(nrow) {
    
    var fijo = $('tr[row="' + nrow + '"][nsemana="0"] td[tipocelda="fijos"][familia="F"] input')

    var spanTotal = $(fijo).parent().parent().find('td[familia="A"][tipocelda="target"] span')
    var jornalActual = parseInt($(fijo).val())
    $(fijo).parent().parent().find('td.categoria[nivel="T"] span').each(function () {
        jornalActual += parseInt($(this).text())
    })
    $(spanTotal).text(jornalActual)

    $('tr:not([nsemana="0"])[row="' + nrow + '"] td[familia="F"] input').each(function () { $(this).change() })
    recalcularFoot()
}

function changeFijos(nrow, nsemana) {
    console.log('row:' + nrow + ',remana:' + nsemana);
    /*
    var nrow = 0
    var nsemana = 1
    */
    var input = $('tr[row="'+nrow + '"][nSemana="' + nsemana + '"] td[tipocelda="fijos"] input');
    var nfijos = $(input).val()
    var suma = parseFloat(nfijos)
    $(input).parent().parent().find('td:not([tipocelda])[nivel="T"]').each(function () { suma += parseFloat($(this).text()) })
    $(input).parent().parent().find('td[familia="A"][nivel="T"] span').text(suma)
    $(input).parent().parent().find('td[familia="A"][nivel="A"] input').val(suma)

    var totalActual = parseInt($('tr[row="' + nrow + '"][nsemana="0"]').find('td[familia="A"][tipocelda="target"] span').text())
    var totalSemana = totalActual - suma;

    $(input).parent().parent().find('td[familia="A"][nivel="D"] span').text(totalSemana)

    recalcularFoot();
}

function calcular() {
    $($('table#tblConfiguraciones tbody tr')[0]).find('td').each(function () {
        var nSemana = $(this).attr('nsemana');
        var semana = $(this).attr('semana');
        var anio = $(this).attr('anio');
        var hj = $('tr#rowHora td input[nsemana=' + nSemana + ']').val();
        var pa = $('tr#rowAusentismo td input[nsemana=' + nSemana + ']').val();
        var pc = $('tr#rowCapacitacion td input[nsemana=' + nSemana + ']').val();
        var cv = $('tr#rowCurva td input[nsemana=' + nSemana + ']').val();
        // window.console && console.log('nSemana: ' + nSemana + ', semana: ' + semana + ', anio: ' + anio + ', hj:' + hj + ', pa:' + pa + ', pc:' + pc);

        $('tr:not(.subHeader,.footerSemana)[nsemana=' + nSemana + '] td[familia="A"][nivel="T"].noOcultable').each(function () {
            var tj = 0;

            $(this).parent().find('td[familia!="A"].noOcultable').each(function () {
                var jornales = 0;

                $(this).parent().find('td[familia=' + $(this).attr('familia') + '][nivel!="T"][nivel!="F"]').each(function () {

                    var hh = parseInt($(this).attr('hh'));
                    var jor = $(this).attr('nivel') == 'C' ? hh * (cv / 100) : hh;
                    jor = (jor + (jor * (pa / 100)) + (jor * (pc / 100))) / hj;
                    jor = /*Math.ceil*/ Math.round(jor);
                    $(this).find('span').text(jor);

                    //     window.console && console.log('hh:' + hh + ', jor:' + jor);
                    jornales += jor;
                })
                $(this).find('span').text(jornales);

                tj += jornales;
            })
            $(this).find('span').text(tj);
            $(this).parent().find('td[familia="A"][nivel="A"] > input').val(tj);
            var idL = $(this).parent().attr('idLider')
            var idU = $(this).parent().attr('idUsuario')
            var ta0 = parseFloat($('tr:not(.subHeader)[nsemana=0][idLider="' + idL + '"][idUsuario="' + idU + '"] td[familia="A"][nivel="T"].noOcultable span').text())
            var ta1 = parseFloat($(this).parent().find('td[familia="A"][nivel="T"] span').text())
            var tat = ta0 - ta1;
            $(this).parent().find('td[familia="A"][nivel="D"].noOcultable span').text(tat);
        });

    });

    window.console && console.log('calculado');
    bloqueoDePantalla.indicarTerminoDeTransaccion();

}

/*********************************************************************************
 *
 *  Funcionalidad footer
 *
 *********************************************************************************/

function initializeFooterValue() {

    window.console && console.log('initializeFooterValue');
    $('div.divSemana tfoot').each(function () {
        $(this).find('td').each(function () {
            changeColumnFooter($(this));
        });
    });
}

function changeColumnFooter(td) {
    var valorTotal = 0.0
    cont = 0;
    $(td).parent().parent().parent().find('tbody tr td[familia=\"' + $(td).attr('familia') + '\"][nivel=\"' + $(td).attr('nivel') + '\"]').each(function () {
        var valor = 0.0;
        if ($(this).find('span').length > 0) {
            valor = parseFloat($($(this).find('span')[0]).text())
        } else {
            valor = parseFloat($($(this).find('input')[0]).val())
        }
        valorTotal += valor;
        cont++;
    });
    while (cont < $(td).parent().parent().parent().find('tbody tr td[familia=\"' + $(td).attr('familia') + '\"][nivel=\"' + $(td).attr('nivel') + '\"]').length) {
        //wondow&&console.log('cont ' + cont);
    }
    $(td).find('span').text(valorTotal)
}

function mostrarOcultarLider(ck) {

    console.log('seleccionado: ' + $(ck).attr('idUsuario') + ' is ' + ($(ck).is(':checked')) ? 'checked' : 'not checked');
    if ($(ck).is(':checked')) {
        console.log('seleccionado: ' + $(ck).attr('idUsuario') + ' show');
        $('tr:not(.ckRowLider)[idUsuario="' + $(ck).attr('idUsuario') + '"]').show();
    } else {
        console.log('seleccionado: ' + $(ck).attr('idUsuario') + ' hide');
        $('tr:not(.ckRowLider)[idUsuario="' + $(ck).attr('idUsuario') + '"]').hide();
    }
}

function clickAllItems(ck) {
    console.log('todos:' + (($(ck).is(':checked')) ? ' seleccionar' : ' deseleccionar'));
    $('.ckLider').prop('checked', $(ck).is(':checked'));
    $('.ckLider').each(function () {
        mostrarOcultarLider($(this));
    });
    recalcularFoot();
}


function showFiltrarLider() {
    console.log('filtrarLider');
    var code = "<div id=\"divListaLider\" >"
        +  "    <table>"
        +  "      <thead><tr><td colspan=\"2\"><span>Lideres Visibles</span></td></tr></thead>"
        +  "      <tfoot><tr>"
        + "          <td><input type=\"checkbox\" id=\"ckAllItems\" checked onclick=\"clickAllItems(this);recalcularFoot();\"/></td>"
        +  "          <td>Ver todos</td>"
        +  "      </tr></tfoot>"
        +  "      <tbody>"
        + genRowsLider(jsonLideres, true) + "</tbody>"
        +  "  </table>"
        +  "</div>";


    popUpAlert(code, "info");
    $('tr:not(.ckRowLider)[idUsuario]').show();

    recalcularFoot();
}

function genDesgloseJornales(idLider, nSemana) {
//    var idLider = 54;
//    var nSemana = 1;

    var strHeaderActividades = '';
    var strBDescripcion = '';
    var strBEtapas = '';
    var strHCategoria = '<tr>';
    var strHEtapa = '<tr>';
    var strHJR = '<tr>';
    var nombreLider = '';
    var esCosecha = false;
    for (index in jsonLideres) {
        if (idLider == jsonLideres[index]['idUsuario']) {
            nombreLider = jsonLideres[index]['nombreLider'];
            break;
        }
    }
    if (nombreLider.length > 0) {

        for (index in jsonEtapas) {
            strHCategoria += '<td colspan = "2"><span>' + jsonEtapas[index]['categoria'] + '</span></td>';
            strHEtapa += '<td colspan = "2"><span>' + jsonEtapas[index]['nombreHabilidad'] + '<br/>' + jsonEtapas[index]['nombreEtapa'] + '</span></td>';
            strHJR += '<td><span>T.Jornales</span></td><td><span>N.Rep</span></td>';
        }
        strHCategoria += '</tr>';
        strHEtapa += '</tr>';
        strHJR += '</tr>';

        strHeaderActividades += strHCategoria + strHEtapa + strHJR;
        for (index in jsonDesgloseActividades) {
            if (idLider == jsonDesgloseActividades[index]['idUsuario'] && nSemana == jsonDesgloseActividades[index]['nSemana']) {
                window && console.log(jsonDesgloseActividades[index]['idUsuario'] + ' ' + jsonDesgloseActividades[index]['nSemana'] + ' E:' + jsonDesgloseActividades[index]['edad'])
                var header = jsonDesgloseActividades[index]['variedadHeader']
                
                for (indexv in jsonVariedades) {
                    if (jsonVariedades[indexv]['header'] == header) {
                        window && console.log(jsonVariedades[indexv]['segmento'] + '-' + jsonVariedades[indexv]['variedad'])
                        strBDescripcion += '<tr><td><span>' + jsonDesgloseActividades[index]['invernadero'] + '</span></td><td><span>' + jsonVariedades[indexv]['segmento'] + '-' + jsonVariedades[indexv]['variedad'] + '</span></td>';
                        break;
                    }
                }
                strBDescripcion += '</span></td><td><span>' + jsonDesgloseActividades[index]['edad'] +
                                '</span></td><td><span>' + jsonDesgloseActividades[index]['densidad'] +
                                '</span></td><td><span>' + Math.round( parseFloat( jsonDesgloseActividades[index]['cajas'] != null || jsonDesgloseActividades[index]['cajas'] != undefined ? jsonDesgloseActividades[index]['cajas'] : 0.000)) + '</span></td></tr>';
                strBEtapas += '<tr>'
                for (indexEtapa in jsonEtapas) {
                    esCosecha = (jsonEtapas[indexEtapa]['esCosecha'] == 1) ? true : false;
                    var jor = 0;
                    var rep = 0;
                    if (jsonDesgloseActividades[index][jsonEtapas[indexEtapa]['clave']] != null && jsonDesgloseActividades[index][jsonEtapas[indexEtapa]['clave']] != undefined) {
                     
                         jor = 0;
                        var hj = $('tr#rowHora td input[nsemana=' + nSemana + ']').val();
                        var pa = $('tr#rowAusentismo td input[nsemana=' + nSemana + ']').val();
                        var pc = $('tr#rowCapacitacion td input[nsemana=' + nSemana + ']').val();
                        var cv = $('tr#rowCurva td input[nsemana=' + nSemana + ']').val();
                        var rep = 0;
                        var hh = 0;
                        var arr = jsonDesgloseActividades[index][jsonEtapas[indexEtapa]['clave']].split('/')
                        if (arr != null && arr != undefined && arr.length == 2) {
                            hh = parseFloat(arr[0]);
                            rep = parseFloat(arr[1]);
                        }
                         jor = esCosecha ? hh * (cv / 100) : hh;
                        jor = (jor + (jor * (pa / 100)) + (jor * (pc / 100))) / hj;
                        jor = jor.toFixed(2);
                    }


                    strBEtapas += '<td hh="' + hh + '"><span hh="' + hh + '">' + jor + '</span></td>';
                    strBEtapas += '<td><span>' + rep + '</span></td>';
                }
                strBEtapas += '</tr>';
            }
        }
        if (strBDescripcion.length > 0 && strBEtapas.length > 0) {

            var code = '<div id="divListaDesgloseJornales">'
            code += '               <span>DESGLOSE DE ACTIVIDADES DE ' + nombreLider.toUpperCase() + ' (' + $('.superHeader[nsemana="' + nSemana + '"] span').text().toUpperCase() + ') </span>'

              code += ' <div id="contenedorDesglose"> '
            code += '                       <div id="divDesgloseDescripcion">'
            code += '                           <table id="tDesgloseDescripcion">'
            code += '                               <thead>'
            code += '                                   <tr><td colspan="5"><span>FAMILIA</span></td></tr>'
            code += '                                   <tr class="alturaHeader"><td><span>INVERNADERO<br/>&nbsp;</span></td><td><span>VARIEDAD<br/>&nbsp;</span></td><td><span>EDAD<br/>&nbsp;</span></td><td><span>DENSIDAD<br/>&nbsp;</span></td><td><span>CAJAS<br/>&nbsp;</span></td></tr>'
            code += '                               </thead>'
            code += '                               <tbody>' + strBDescripcion + '</tbody>'
            code += '                           </table>'
            code += '                       </div>'
            code += '                       <div id="divDesgloseActividades" overflow-x: auto;\">'
            code += '                           <table id="tDesgloseActividades">'
            code += '                               <thead>' + strHeaderActividades + '</thead>'
            code += '                               <tbody>' + strBEtapas + '</tbody>'
            code += '                           </table>'
            code += '                       </div> </div>'
            code += '   </div>'


            popUpAlertCustomMax(code, "info", 920, 370, false);
            $('.jsPopUp').addClass("porcentajeJornales");

        }

    }
}

//function recalcularFoot() {
//    $('tfoot>tr:visible> td[nsemana][familia][nivel]').each(function () {
//        var total = 0;
//        $('tbody>tr:not(:hidden)> td[nsemana="' + $(this).attr('nsemana') + '"][familia="' + $(this).attr('familia') + '"][nivel="' + $(this).attr('nivel') + '"]').each(function () {
//            if ($(this).find('span')[0] != undefined) {
//                total += parseInt($(this).find('span').text());
//            } else {
//                total += parseInt($(this).find('input').val());
//            }
//        });
//        $(this).find('span').text(total)
//    });

//    console.log('recalcularFoot')
//}



function recalcularFoot() {
    $('tfoot>tr:visible td[nsemana][familia][nivel]').each(function () {
        var total = 0;
        $('tbody>tr:visible> td[nsemana="' + $(this).attr('nsemana') + '"][familia="' + $(this).attr('familia') + '"][nivel="' + $(this).attr('nivel') + '"]').each(function () {
            if ($(this).find('span')[0] != undefined) {
                total += parseInt($(this).find('span').text());
            } else {
                total += parseInt($(this).find('input').val());
            }
        });
        $(this).find('span').text(total)
    });

//    console.log('recalcularFoot')
}

function muestraCharts() {       

//var code ="<table><tbody><tr><td><div id=\"charLider\"></div></td><td><div id=\"charPlanta\"></div></td></tr></table>"
//       var code = '<div id="charLider"></div><div id="charPlanta"></div>'
//       popUpAlertCustomMax(code, "info", 920, 370, true);
//       $('.jsPopUp').addClass("chartsJornales");
//    var code = '<div id="chartsContainer"  style=" min-width:1000px"><div id="chartLider" style="width: 100%; height: 350px; margin: 0 auto;"></div><div id="chartPlanta"  style="width: 100%; height: 250px; margin: 0 auto;"></div></div>';
//    popUpAlertCustomMax(code, "info", 920, 900, false);
//    $('.jsPopUp').addClass("porcentajeJornales");
//    genCharts();
﻿﻿﻿     var code = '<div id="chartsContainer"><div id="chartLider" style="width: 100%; height: 500px; margin: 5 auto;"></div><div id="chartPlanta"  style="width: 100%; height: 300px; margin: 5 auto;"></div></div>';     popUpAlertCustomMax(code, "info", 920, 900, false);     $('.jsPopUp').addClass("chartsJornales");
     genCharts();     
     }
     
 function genCharts() {
     var rows = new Array(0)
     var lideres = new Array(0)    
     var semanas = new Array(0)    
     var nombreSemanas = new Array(0)    
     // para col promedio por lider    
     var listaSujeridos = Array(0)    
     $('.rowLider:visible').each(function () {        
            rows.push($(this).attr('row'))        
            lideres.push($(this).find('td.clickable span').text())        
            //para col promedio por lider        
            var totalRow = 0.0        
            var nRows = parseFloat($('.divSemana tr:visible[row="' + $(this).attr('row') + '"] td[familia="A"][nivel="T"] span.clickable').length)        
            $('.divSemana tr:visible[row="' + $(this).attr('row') + '"] td[familia="A"][nivel="T"] span.clickable').each(function () {            
            totalRow += parseFloat($(this).text())       
            })        
            listaSujeridos.push((totalRow != 0) ? Math.round(totalRow / nRows) : 0)    })    
            $('.divSemana').each(function () {       
                  var lista = new Array(0)       
                  nombreSemanas.push($(this).find('.superHeader td span').text())        
                  $(this).find('tr:visible[row]').each(function () {            
                  lista.push($(this).find('td[familia="A"][nivel="T"] span').text())        
            })        
            semanas.push(lista)   
     })    
     nombreSemanas.push('Sugerencia de Contratación')    
     semanas.push(listaSujeridos)    
     var strLideres = '['    
     for (var index in lideres) {        
     strLideres += (strLideres.length > 1 ? ',' : '') + '"' + lideres[index] + '"'    }    strLideres += ']'    
     var strJSON = '['    
     var strColor = '['    
     var strPlanta = '['   
     var semanaPlanta = new Array(0)    
     for (var index in nombreSemanas) {        strColor += (index == 0) ? '"#00b050"' : ((nombreSemanas.length - 1) != index) ? ',"#558ed5"' : ',"#ff0000"'        
     strJSON += (strJSON.length > 1 ? ',' : '') + '{"name":"' + nombreSemanas[index] + '", "data":[' + semanas[index].toString() + ']}'        
     // datos para chart planta       
     var total = 0
        for (var i in semanas[index]) {
        total+= parseInt(semanas[index][i])        
        }        
        semanaPlanta.push(total);        
        strPlanta += (strPlanta.length > 1 ? ',' : '') + '{"name":"' + nombreSemanas[index] + '", "data":[' + total + ']}'    
     }    
     strColor += ',"#f7a35c"]'    
     strJSON += ']'    
     strPlanta += ']'    
     var sJSON = JSON.parse(strJSON);    
     var pJSON = JSON.parse(strPlanta);    
     var strChartLider = '    {'
     + '        "chart": {'
     + '            "type": "column"'
     + '        },'+ '        "title": {'
     + '            "text": "Pronostico de Jornales a ' + (nombreSemanas.length - 2) + ' Semanas por lider"'+ '        },'
     + '        "subtitle": {'+ '            "text": ""'+ '        },'
     + '        "xAxis": {'+ '            "categories":  ' + strLideres + ','
     + '            "crosshair": "true"'+ '        },'+ '        "yAxis": {'
     + '            "min": 0,'+ '            "title": {'
     + '                "text": "Jornales"'+ '            }'
     + '        },'+ '        "tooltip": {'
     + '            "headerFormat": "<span style=\\"font-size:10px\\">{point.key}</span><table>",'
     + '            "pointFormat": "<tr><td style=\\"color:{series.color};padding:0\\">{series.name}: </td><td style=\\"padding:0\\"><b>{point.y:.1f} Jornales</b></td></tr>",'
     + '            "footerFormat": "</table>",'
     + '            "shared": "true",'+ '            "useHTML": "true"'
     + '        },'
     + '        "plotOptions": {'
     + '            "column": {'
     + '                "pointPadding": 0,'
     + '                "borderWidth": 0'+ '            }'
     + '        },'
     + '        "series": ' + strJSON+ '    }'    
     
     var strChartPlanta = '    {'
     + '        "chart": {'
     + '            "type": "column"'
     + '        },'+ '        "title": {'
     + '            "text": "Pronostico de Jornales a ' + (nombreSemanas.length - 2) + ' Semanas por Planta"'+ '        },'
     + '        "subtitle": {'+ '            "text": ""'+ '        },'
     + '        "xAxis": {'
     + '            "categories":' +'["GRAN TOTAL"]' + ','
     + '            "crosshair": "true"'+ '        },'
     + '        "yAxis": {'
     + '            "min": 0,'+ '            "title": {'
     + '                "text": "Jornales"'+ '            }'+ '        },'
     + '        "tooltip": {'+ '            "headerFormat": "<span style=\\"font-size:10px\\">{point.key}</span><table>",'
     + '            "pointFormat": "<tr><td style=\\"color:{series.color};padding:0\\">{series.name}: </td><td style=\\"padding:0\\"><b>{point.y:.1f} Jornales</b></td></tr>",'
     + '            "footerFormat": "</table>",'+ '            "shared": "true",'
     + '            "useHTML": "true"'
     + '        },'
     + '        "plotOptions": {'
     + '            "column": {'
     + '                "pointPadding": 0.2,'+ '                "borderWidth": 0'+ '            }'
     + '        },'+ '        "series": ' + strPlanta+ '    }'    
     var JSONchartsLider = JSON.parse(strChartLider)    
     var JSONchartsPlanta = JSON.parse(strChartPlanta)    
     var JSONcolors = JSON.parse('{"colors":' + strColor + '}')    
     Highcharts.theme = JSONcolors;   
     Highcharts.setOptions(Highcharts.theme);    
     Highcharts.chart('chartLider', JSONchartsLider);    
     Highcharts.chart('chartPlanta', JSONchartsPlanta);
}



