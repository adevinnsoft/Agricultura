<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmCapturaTrabajo.aspx.cs"
    Inherits="Lider_frmCapturaTrabajo" MasterPageFile="~/MasterPage.master" ValidateRequest="false"
    EnableEventValidation="false" MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/slider/slick.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/moment.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" id="Globales">

        var JsonAsociados;
        var densidadPorSurco = [];
        $(function () {
            JsonAsociados = JSON.parse('<%=AsociadosLider%>');
        });
    </script>
    <script type="text/javascript">
        $(function () {
            registerControls();


            //            $(window).scroll(function () {
            //                if (window.scrollY >= 228) {
            //                    $('div#roller.invernaderos').addClass('fixed');
            //                } else {
            //                    $('div#roller.invernaderos').removeClass('fixed');
            //                }
            //            });


            $('.horaInicio').live('click', function () {
                $('#TimeDemo').val($(this).val()).click();
                $('#popUpHora').show();
                ctrlFechaActual = $(this);
            });
            $('input.fechaCaptura').val(function () {
                var fecha = new Date();
                var yyyy = fecha.getFullYear().toString();
                var mm = (fecha.getMonth() + 1).toString(); // getMonth() is zero-based
                var dd = fecha.getDate().toString();
                return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]); // padding
            });
            $('.horaFin').live('click', function () {
                $('#TimeDemo').val($(this).val()).click();
                $('#popUpHora').show();
                ctrlFechaActual = $(this);
            });
            $('.fechaCaptura').live('click', function () {
                $('#DateDemo').val($(this).val()).click();
                $('#popUpFecha').show();
                ctrlFechaActual = $(this);
            });

            $('.horaInicio').live('change', function () {
                var inicio = $(this);
                var final = $(this).parent().parent().find('.horaFin');

                comparaInicioFin(inicio, final);

                revisarCaptura($(this).parent().parent());
            });

            $('.horaFin').live('change', function () {
                var final = $(this);
                var inicio = $(this).parent().parent().find('.horaInicio');
                comparaInicioFin(inicio, final);
                revisarCaptura($(this).parent().parent());
            });

            $('.surcoInicio').live('keyup', function () {
                var inicio = $(this);
                var final = $(this).parent().parent().find('.surcoFin');
                var surcosTotales = parseInt($(this).parent().parent().attr('totalSurcos'));
                if (inicio != '' && final != '') {
                    comparaInicioFin(inicio, final);
                    revisarCaptura($(this).parent().parent());
                    if (parseInt(inicio.val()) > surcosTotales) {
                        inicio.attr('title', 'Excede los surcos Totales').addClass('error');
                    }

                    if (parseInt(final.val()) > surcosTotales) {
                        final.attr('title', 'Excede los surcos Totales').addClass('error');
                    }
                }
                CalculaSurcos($(this));
            });

            $('.surcoFin').live('keyup', function () {
                var final = $(this);
                var inicio = $(this).parent().parent().find('.surcoInicio');
                var surcosTotales = parseInt($(this).parent().parent().attr('totalSurcos'));
                if (inicio != '' && final != '') {
                    comparaInicioFin(inicio, final);
                    revisarCaptura($(this).parent().parent());
                    if (parseInt(inicio.val()) > surcosTotales) {
                        inicio.attr('title', 'Excede los surcos Totales').addClass('error');
                    }
                    if (parseInt(final.val()) > surcosTotales) {
                        final.attr('title', 'Excede los surcos Totales').addClass('error');
                    }
                }
                CalculaSurcos($(this));
            });

            $('span.idAsociado.clicker').live('click', function () {
                var control = $(this);
                var jsonFiltroAsociados = [];
                control.removeClass('idAsociado');
                var idEmpleado = control.parent().parent().attr('id');
                control.html('<select type="text" class="cajaChica Asociado" idEtapa="' + control.parent().attr('idEtapa') + '" value="' + idEmpleado + '"></select>');
                JsonAsociados.filter(function (i, n) {
                    if (i.IdEtapa == control.parent().attr('idEtapa')) {
                        jsonFiltroAsociados.push(i);
                    }
                });

                $.each(jsonFiltroAsociados, function (i, n) {
                    control.children().append('<option value ="' + n.ID_EMPLEADO + '"nombre="' + n.NOMBRE + '" eficiencia="' + n.eficiencia + '" >' + n.ID_EMPLEADO + '_' + n.NOMBRE + '</option>');
                });
                open(control.find('.Asociado'));

            });

            $('select.cajaChica.Asociado').live('change', function () {
                var row = $(this).parent().parent().parent();

                var selected = $(this).find('option:selected')
                row.attr('idasociado', selected.val());
                row.find('.nombreAsociado').text(selected.attr('nombre'));
                row.find('span').addClass('idAsociado').text(selected.val());
            });


            $('.addRow').live('click', function () {
                var row = $(this).parent().parent().clone();
                row.find('.horaInicio').val($(this).parent().parent().find('.horaFin').val());
                row.find('span.idAsociado').addClass('clicker');
                row.find('.horaFin').val('');
                row.find('.surcoInicio').val('');
                row.find('.surcoFin').val('');
                row.find('.addRow').attr('src', '../comun/img/remove-icon.png');
                row.find('.removeUser').remove();
                row.find('.addRow').addClass('removeRow');
                row.find('.removeRow').removeClass('addRow');

                var num = $(this).parent().parent().index();
                row.insertAfter($(this).parent().parent());
            });

            $('#ctl00_ddlPlanta').live('change', function () {
                $('.slick-slide').html('');
                $('#rollslider2').html('');
                $('.slick-slide').css({ 'background-image': 'url("../comun/img/ajax-loader.gif")',
                    'background-repeat': 'no-repeat',
                    'background-position': 'center'
                });
                cargaInvernaderos();
            });

            $('.removeRow').live('click', function () {
                $(this).parent().parent().remove();
            });

            $('.removeUser').live('click', function () {
                $(this).parent().parent().find('input:text').attr('disabled', true);
                $(this).parent().parent().addClass('disabled');
                $(this).parent().parent().find('input.enabled').removeClass('enabled');
                $(this).parent().parent().find('.addRow').hide();
                $(this).parent().parent().removeClass('enabled');
                $(this).addClass('addUser');
                $(this).removeClass('removeUser');
                $(this).attr('src', '../comun/img/add-icon.png');
                ordersurcos($(this));
            });
            $('.addUser').live('click', function () {
                $(this).parent().parent().find('input:text').attr('disabled', false);
                $(this).parent().parent().removeClass('disabled');
                $(this).parent().parent().find('.addRow').show();
                $(this).parent().parent().addClass('enabled');
                $(this).removeClass('addUser');
                $(this).addClass('removeUser');
                $(this).attr('src', '../comun/img/remove-icon.png');
                ordersurcos($(this));
            });

            $('.asignarCalidad').live('click', function () {
                var valor = $(this).parent().parent().parent().find('.Calificacion').val();
                $(this).parent().parent().parent().parent().parent().find('.Calidad').val(valor);
            });

            $('.asignarInicioFin').live('click', function () {
                var inicio = $(this).parent().parent().parent().find('.horaInicio').val();
                $(this).parent().parent().parent().parent().parent().find('.horaInicio').val(inicio);
                var fin = $(this).parent().parent().parent().find('.horaFin').val();
                $(this).parent().parent().parent().parent().parent().find('.horaFin').val(fin);
            });

            $('input.btnSave.Actividad').live('click', function () {
                var activida = $(this).attr('actividad');
                GuardaActividad($('div.Actividad[actividad=' + activida + ']'));
            })

            $('#divActividades').find('h3').live('click', function () {
                $(this).parent().next().slideToggle('fast', 'linear');
            });

            $('.fechaCaptura').change(function () {
                $('div#divActividades').children().remove();

                $('.divInvernadero.selected').each(function () {
                    var inv = $(this);

                    inv.removeClass('selected');
                    inv.attr('selected', false);

                });

                if (Date.parse($(this).val()) > new Date) {
                    popUpAlert('No se puede capturar fechas futuras', 'warning');
                    $('.fechaCaptura').val(moment().format("YYYY-MM-DD"));
                }

                cargaProgreso();

            });



            cargaInvernaderos();

        });

        function open(elem) {
            if (document.createEvent) {
                var e = document.createEvent("MouseEvents");
                e.initMouseEvent("mousedown", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                elem[0].dispatchEvent(e);
            } else if (element.fireEvent) {
                elem[0].fireEvent("onmousedown");
            } 
        }

        function comparaInicioFin(inicio, final, surcosTotales) {
            inicio.removeClass('error');
            final.removeClass('error');
            if (inicio.val() > final.val()) {
                inicio.addClass('error');
                final.addClass('error');
            }
            
        }

        function CalculaSurcos(e) {
            //var counter = 0;
            //var inicio = 0;
            //var plantulas = 0;
            //var fin = 0;
            //var table = e.parent().parent().parent();
            //var totalSurcosTrabajados = 0;
            //table.find('tr.enabled').each(function () {
            //    inicio = $(this).find('input.surco.surcoInicio.enabled').val();
            //    fin = $(this).find('input.surco.surcoFin.enabled').val();
            //    counter += fin - inicio + 1;

            //    plantulas += getplantulas(inicio, fin);
                

            //});
            //totalSurcosTrabajados = table.parent().prev().find('td.surcosAcumulados').text;
            //table.parent().prev().find('td.surcosAcumulados').text(counter +',  plantulas:' + plantulas + '');
        }

        function getplantulas(inicio, fin) {
            var si = parseInt(inicio);
            var sf = parseInt(fin);
            var plantulas = 0;
            for (var a in densidadPorSurco) {
                if (densidadPorSurco[a].surco >= inicio && densidadPorSurco[a].surco <= fin) {
                    plantulas += densidadPorSurco[a].densidad;
                }
            }
            return plantulas;
        }

        function revisarCaptura(row) {
            var user = row.attr('idAsociado');
            var surcoI = parseInt(row.find('input.surcoInicio').val());
            var surcoF = parseInt(row.find('input.surcoFin').val());
            var horaI = moment(row.find('input.horaInicio').val(), "MM-DD HH:mm");
            var horaF = moment(row.find('input.horaFin').val(), "MM-DD HH:mm");

            row.find('input.surcoInicio, input.surcoFin').attr('title', 'surco').removeClass('error');
            row.find('input.surcoInicio, input.surcoFin').attr('title', 'surco').removeClass('error');
            row.find('input.horaInicio, input.horaFin').attr('title', 'hora').removeClass('error');
            row.find('input.horaInicio, input.horaFin').attr('title', 'hora').removeClass('error');

            $('tr[idasociado=' + user + ']').each(function () {

                if (row.html() != $(this).html()) {
                    var traslape = false;

                    var surcoI2 = parseInt($(this).find('input.surcoInicio').val());
                    var surcoF2 = parseInt($(this).find('input.surcoFin').val());
                    var horaI2 = moment($(this).find('input.horaInicio').val(), "MM-DD HH:mm");
                    var horaF2 = moment($(this).find('input.horaFin').val(), "MM-DD HH:mm");

                    if (horaI < horaF2 && horaI > horaI2 && horaI < horaF2 && horaF > horaF2) {
                        traslape = true;
                    }
                    else if (horaF > horaI2 && horaF < horaF2 && horaI2 > horaI && horaI2 < horaF) {
                        traslape = true;
                    }
                    else if (horaI < horaI2 && horaF > horaF2) {
                        traslape = true;
                    }
                    else if (horaI > horaI2 && horaF < horaF2) {
                        traslape = true;
                    }

                    if (traslape) {
                        row.find('input.horaInicio, input.horaFin').attr('title', 'No puede trabajar en varias tareas al mismo tiempo').addClass('error');
                        $(this).find('input.horaInicio, input.horaFin').attr('title', 'No puede trabajar en varias tareas al mismo tiempo').addClass('error');
                    }
                    else {
                        $(this).find('input.horaInicio, input.horaFin').attr('title', 'Hora').removeClass('error');
                        row.find('input.horaInicio, input.horaFin').attr('title', 'Hora').removeClass('error');
                    }


                    traslape = false;
                    if (surcoI <= surcoF2 && surcoI >= surcoI2 && surcoI <= surcoF2 && surcoF >= surcoF2) {
                        traslape = true;
                    }
                    else if (surcoF >= surcoI2 && surcoF <= surcoF2 && surcoI2 >= surcoI && surcoI2 <= surcoF) {
                        traslape = true;
                    }
                    else if (surcoI <= surcoI2 && surcoF >= surcoF2) {
                        traslape = true;
                    }
                    else if (surcoI >= surcoI2 && surcoF <= surcoF2) {
                        traslape = true;
                    }


                    if (traslape && row.find('td[idetapa]').attr('idetapa') == $(this).find('td[idetapa]').attr('idetapa')) {
                        row.find('input.surcoInicio, input.surcoFin').attr('title', 'Surcos Retrabajados').addClass('error');
                        $(this).find('input.surcoInicio, input.surcoFin').attr('title', 'Surcos Retrabajados').addClass('error');
                    } else {
                        $(this).find('input.surcoInicio, input.surcoFin').attr('title', 'Surcos').removeClass('error');
                        row.find('input.surcoInicio, input.surcoFin').attr('title', 'Surcos').removeClass('error');
                    }

                }
            });
        }

        function EliminarSeleccionDeInvernaderos() {
            $('.divInvernadero').each(function () {
                if ($(this).attr('class').indexOf('selected') > -1)
                { $(this).mouseup(); }
                else
                { }
            });
        }

        function ClickACadaInvenradero() {
            $('.divInvernadero').each(function () {
                if ($(this).attr('class').indexOf('selected') > -1)
                { }
                else
                { $(this).mouseup(); }
            });
        }

        function ordersurcos(a) {

        }

        function GuardaCapturas() {
            var Captura = [];
            var json;
            var insert = true;

            if ($('div#divActividades').find('.Actividad.enabled').length == 0) {
                popUpAlert('No hay Actividades para capturar o no hay registro de Asociados', 'warning');
                return;
            }

            $('div#divActividades').find('.Actividad.enabled').each(function () {
                var Asociados = [];

                $(this).find('input.required').each(function () {
                    var elemento = $(this);
                    if (elemento.val() == '') {
                        insert = false;
                        elemento.addClass('error');
                    }
                    else {
                        elemento.removeClass('error');
                    }
                });

                if ($('input.error').length > 0) {
                    insert = false;
                }

                $(this).find('table.captura tr.enabled').each(function () {
                    if ($(this).attr('idAsociado') > 0) {
                        Asociados.push({
                            "idAsociado": $(this).attr('idAsociado'),
                            "surcoInicio": $(this).find('.surcoInicio').val(),
                            "surcoFin": $(this).find('.surcoFin').val(),
                            "horaInicio": $(this).find('.horaInicio').val(),
                            "horaFin": $(this).find('.horaFin').val(),
                            "Calidad": $(this).find('.Calidad').val(),
                            "CantidadSurcos": $(this).find('.CantidadSurcos').val(),
                            "realizado": ($(this).find('.realizado').val() == 'on' ? true : false)
                        });
                    }
                });

                Captura.push({
                    "idInvernadero": $(this).parent().parent().attr('invernadero'),
                    "idActividad": $(this).attr('actividad'),
                    "idPeriodo": $(this).attr('idperiodo'),
                    "asociados": Asociados,
                    "comentarios": $(this).find('.comentarios').val()
                });
            });
            
            if (insert) {
                SaveInDb(Captura);
            } else {
                popUpAlert('No se han capturado todos los campos requeridos','warning');
            }
        }

        function GuardaPorInvernadero(element) {
            var div = element.parent().parent();
            alert(div);
        }

        function SaveInDb(Captura) {

            PageMethods.GuardaCaptura(Captura, function (response) {
                var result = response.split(',');
                if (result[1] == 'success') {
                    $('#divActividades div.divCaptura').remove();
                    $('#divActividades div.accordionBody').remove();
                    $('.divInvernadero.selected').each(function () {
                        var fechaplantacion = new Date($(this).attr('fechaplantacion'));
                        var info = $(this).text() + ' - ' + $(this).attr('product') + ' - <%=Resources.Commun.Edad.ToString() %>' + (Math.round((Math.abs(fechaplantacion.getTime() - new Date().getTime())) / 604800000));
                        var densidad = $(this).attr('densidad');
                        var idCiclo = $(this).attr('idciclo');
                        cargaActividadesProgramadas($(this).attr('idInvernadero'), info, densidad,idCiclo);

                    });
                    cargaProgreso();

                }
                popUpAlert((result[0] == 'ok' ? "Guardado" : "No se Guardó la Informacion"), result[1]);
            });

        }

        function GuardaActividad(Actividad) {
            var Captura = [];
            var Asociados = [];
            var insert = true;

            if (Actividad.find('table.captura tr.enabled').length == 0) {
                popUpAlert('No hay informacion para guardar', 'warning');
                return;
            }

            Actividad.find('input.enabled').each(function () {
                var elemento = $(this);
                if (elemento.val() == '') {
                    insert = false;
                    elemento.addClass('error');
                }
                else {
                    elemento.removeClass('error');
                }
            });

            Actividad.find('table.captura tr.enabled').each(function () {
                if ($(this).attr('idAsociado') > 0) {
                    Asociados.push({
                        "idAsociado": $(this).attr('idAsociado'),
                        "surcoInicio": $(this).find('.surcoInicio').val(),
                        "surcoFin": $(this).find('.surcoFin').val(),
                        "horaInicio": $(this).find('.horaInicio').val(),
                        "horaFin": $(this).find('.horaFin').val(),
                        "Calidad": $(this).find('.Calidad').val(),
                        "realizado": ($(this).find('.realizado').val() == 'on' ? true : false)
                    });
                }
            });

            Captura.push({
                "idInvernadero": Actividad.parent().parent().attr('invernadero'),
                "idActividad": Actividad.attr('actividad'),
                "asociados": Asociados,
                "comentarios": Actividad.find('.comentarios').val()
            });

            if (insert) {
                SaveInDb(Captura);
            } else {
                popUpAlert('No se han capturado todos los campos requeridos','warning');
            }
        }

        function asignarFecha() {
            $(ctrlFechaActual).val($("#DateDemo").val());
            $('#popUpFecha').hide();
            $(ctrlFechaActual).change();
        }
        function asignarHora() {
            $(ctrlFechaActual).val($("#TimeDemo").val());
            $('#popUpHora').hide();
            $(ctrlFechaActual).change();
        }

        function cargaInvernaderos() {
            PageMethods.cargaInvernaderosSlider(function (response) {
                $('#rollslider').removeClass();
                $('.invernaderos #rollslider').html(response);
                setInvernaderos();

                cargaProgreso();
            });
        }

        function cargaProgreso() {
            PageMethods.ContadorProgresoActividades($('input.fechaCaptura').val(), function (response) {
                var progresos = JSON.parse(response);

                progresos.forEach(function (a) {
                    $('.divInvernadero[idInvernadero="' + a.idInvernadero + '"][idciclo ="' + a.idCiclo + '"] span.avance').text(a.progreso) 
                });
            });
        }   

        function setInvernaderos() { //Inicializa los controles Slider en los que se muestran las plantas
            $('#rollslider').slick({
                slidesToShow: $('#rollslider div').length <= 12 ? $(this).length : 12,
                slidesToScroll: $('#rollslider div').length >= 12 ? 5 : 2,
                infinite: false,
                variableWidth: true
            });

            $('.divInvernadero ').mousedown(function (event) {
                if (event.which == 1) {
                    $(this).addClass('clicked');
                }

            });

            $('.buscador').live('keyup', function () {
                var texto = $(this).val().toUpperCase();
                if (texto == '' || texto == undefined) {
                    $('div.slick-slide').show();
                } else {
                    $('div.slick-slide').hide();
                    $('div.slick-slide:contains("' + texto + '")').show();
                    $('.slick-track').css("transform", "translate3d(12px,0px,0px)");
                }

            });

            $('.divInvernadero ').mouseup(function (event) {
                if (event.which == 1 || event.isTrigger) {
                    var invernaderoID = $(this).attr('idInvernadero');
                    var idCiclo = $(this).attr('idCiclo');
                    var claveInvernadero = $(this).text();
                    var densidad = $(this).attr('densidad');
                    var interplanting = $(this).attr('interplanting');
                    var fechaplantacion = new Date($(this).attr('fechaplantacion'));
                    var info = $(this).text() + ' - ' + $(this).attr('product') + ' - <%=Resources.Commun.Edad.ToString() %>' + (Math.round((Math.abs(fechaplantacion.getTime() - new Date().getTime())) / 604800000));
                    
                    debugger;

                    if ($(this).attr('class').indexOf('selected') > -1) {
                        $(this).removeClass('selected');
                        $(this).attr('selected', false);
                        $('#divInv_' + idCiclo).remove();
                        $('#divAcc_' + idCiclo).remove();
                    }
                    else {
                        $(this).addClass('selected');
                        $(this).attr('selected', true);
                        if ($('#divInv_' + idCiclo).length > 0) {
                            $('#divInv_' + idCiclo).show();
                        }
                        else {
                            cargaActividadesProgramadas(invernaderoID, info, densidad, idCiclo);
                            cargaDensidadPorsurco(invernaderoID);
                        }
                    }

                    $(this).removeClass('clicked');
                }
            });
        }

        function cargaDensidadPorsurco(invernaderoID) {
            PageMethods.getDensidadSurcos(invernaderoID, function (response) {
                var info = JSON.parse(response);

                if (info.length == 0) {
                    popUpAlert('El Invernadero no tiene Configuracion de Densidad por surco, se tomará el promedio', 'info');
                }

                for (var a in info) {
                    densidadPorSurco.push({
                        idInvernadero: info[a].IdInvernadero,
                        surco: info[a].NumeroDeSurco,
                        densidad: info[a].Densidad
                    });
                }
            });
        }

        function cargaActividadesProgramadas(invernaderoID, info, densidad, idCiclo) {
            PageMethods.cargaActividadesProgramadas(invernaderoID, $('input.fechaCaptura').val(), idCiclo, function (response) {
                if (response == "0") {
                    popUpAlert('No hay Actividades Registradas para este invernadero', 'info');
                } else {
                    $('#divActividades').append(tablaDeActividadesPorInvernadero(invernaderoID, info, response, densidad, idCiclo));
                    $('#divInv_' + invernaderoID).show();
                    $('#divActividades').find('h3').css('cursor', 'pointer');
                }
            });
        }

        function tablaDeActividadesPorInvernadero(idInvernadero, info, response,densidad,idCiclo) {
            return '   <div class="divCaptura" id="divInv_' + idCiclo + '" invernadero="' + idInvernadero + '" densidad="' + densidad + '> ' +
                       ' <div style="width:100%; background-color:#ADC995;height: 36px;"><h3 class="accordionHeader" >                         ' +
                       '         <label>                                           ' +
                       '              ' + info + '<label></label></label></h3>     ' +
                       '<input type="button"  style="display:none; border-width: 2px;" onclick="GuardaPorInvernadero($(this));" value="Guardar Invernadero"/></div>' +
                       '     <div class="accordionBody" id="divAcc_' + idCiclo + '">                          ' +
                       response +
                       ' </div> </div>';

        }

    </script>
    <style type="text/css">
        th.separator
        {
            background-color: rgb(219, 219, 219) !important;
            height: 11px;
            border: 13px solid #F0F5E5;
        }
        td.capturado
        {
            background: url(../comun/img/ok.png);
            background-size: contain;
            background-repeat: no-repeat;
        }
        div.divCaptura
        {
                display: inline-table;
                min-width: 800px;
        }
        td.realizado
        {
            background: url(../comun/img/ok.png);
            background-size: contain;
            background-repeat: no-repeat;
        }
        input.asignarCalidad, input.asignarInicioFin
        {
            width: 65px;
        }
        .popUp
        {
            background: rgba(0,0,0,.1);
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            text-align: center;
            padding-top: 5%;
            z-index: 99999999;
            display: none;
        }
        input.error
        {
            border: 1px solid red;
            background: rgba(255,0,0,0.2);
        }
        #calendar
        {
            width: 900px;
        }
        .slick-slide
        {
            width: 60px !important;
            cursor: pointer;
            display: none;
        }
        .stick-prev
        {
            background: inherit;
        }
        .slick-active
        {
            display: block;
        }
        #rollslider div
        {
            width: 800px;
        }
        #rollslider2 div
        {
            width: 800px;
        }
        .invernaderos
        {
            padding: 0 25px;
        }
        .clicked
        {
            background: #3f3f3f; /* Old browsers */
            background: -moz-linear-gradient(top,  #3f3f3f 0%, #cccccc 77%, #f2f2f2 99%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#3f3f3f), color-stop(77%,#cccccc), color-stop(99%,#f2f2f2)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* IE10+ */
            background: linear-gradient(to bottom,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#3f3f3f', endColorstr='#f2f2f2',GradientType=0 ); /* IE6-9 */
            -webkit-box-shadow: inset 0px 3px 3px 3px rgba(0,0,0,0.5);
            box-shadow: inset 0px 3px 3px 3px rgba(0,0,0,0.5);
        }
        .selected
        {
            border: 1px solid #adc995;
            -webkit-box-shadow: 0px 0px 3px 3px #adc995;
            box-shadow: 0px 0px 3px 3px #adc995;
        }
        .panel-heading
        {
            padding: 10px;
            background: #000000 !important;
            font-size: 18px;
            font-variant: small-caps;
            color: #FFFFFF;
        }
        .panel-heading input
        {
            float: left;
            margin-right: 10px;
            margin-top: 4px;
        }
        #divActividades table
        {
            width: 100%;
        }
        h3.accordionHeader, .divCaptura
        {
            display: block;
            text-align: left;
            padding-bottom: 3px;
            padding-top: 8px;
            background-color: #ADC995;
            color: white;
            font-size: 18px;
            width: 75%;
        }
        .accordionBody td
        {
            border: 1px solid;
            border-color: #E5F1E5;
        }
        .accordionBody th
        {
            background-color: #FEE435;
            text-align: center; /* width: 100px !important; */
        }
        .accordionBody input
        {
            max-width: 105px;
        }
        .accordionBody
        {
            width: 99%;
        }
        .invisible
        {
            display: none;
        }
        table.index h2
        {
            width: auto;
        }
        .btnHabilidad span
        {
            font-size: 10px;
            display: block;
        }
        .btnHabilidad
        {
            width: 60px;
            height: 60px;
            margin: 7px;
            border: 6px solid gray;
            font-size: 26px;
            border-radius: 5px;
        }
        .divHabilidadProgramable
        {
            display: inline-table;
            background: #F0F5E5;
            margin: 1px;
            padding-bottom: 5px;
        }
        .divHabilidadProgramable span
        {
            display: block;
            margin-left: 15px;
        }
        
        span.habilidad_icono
        {
            font-size: 22px;
            margin: 0px;
            width: 100%;
            text-align: center !important;
        }
        span.habilidad_descripcion
        {
            margin: 0px;
            width: 100%;
            text-align: center !important;
            text-transform: capitalize;
            font-size: 8px;
        }
        span.habilidad_etapa
        {
            margin: 0px;
            width: 100%;
            text-align: center !important;
            text-transform: capitalize;
            font-size: 11px;
            color: #000;
            text-shadow: rgba(224, 224, 224,.5) 1px 1px 0px;
            margin-top: 2px;
        }
        table.slidInvernaderos
        {
            height: 137px;
        }
        div#roller.invernaderos.fixed
        {
            position: fixed;
            top: 35px;
            margin-top: 5px;
            background: rgb(255,255,255);
            z-index: 30;
        }
        .divCaptura
        {
            height: 36px;
        }
        #SelectorInterior
        {
            background: #fff;
            border: 2px solid black;
            width: 221px;
            margin: 0 auto;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
        }
        #SelectorHora
        {
            background: #fff;
            border: 2px solid black;
            width: 357px;
            margin: 0 auto;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
        }
        img.imgPopUp
        {
            height: 35px;
        }
        
        td.derecha {
            position: relative;
        }

        input.buscador.cajaChica {
            position: absolute;
            right: 7px;
            top: 2px;
            width: 146px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Captura de Trabajo</asp:Label>
            <span style="margin-left: 45%;">Fecha:</span>
            <input type="text" class="fechaCaptura" />
        </h1>
        <table class="index slidInvernaderos">
            <tr>
                <td>
                    <h2>
                        Invernaderos</h2>
                </td>
                <td>
                    <input type="button" value="Eliminar Selección" onclick="EliminarSeleccionDeInvernaderos();" />
                    <input type="button" value="Seleccionar Todos" onclick="ClickACadaInvenradero();" />
                    
                </td>
            </tr>

            <tr>
                <td colspan="2" class="derecha">
                    <input type="text" placeholder="Filtrar" class="buscador cajaChica" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <div id="roller" class="invernaderos">
                        <div id="rollslider">
                        </div>
                    </div>
                </td>
            </tr>
        </table>


        <table class="index">
            <tr>
                <td>
                    <h2>
                        Actividades</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" class="btnSave" value="Guardar Todo" onclick="GuardaCapturas();" />
                    <input type="button" value="Contraer" onclick="$('.accordionBody').slideUp();" />
                    <input type="button" value="Extender" onclick="$('.accordionBody').slideDown();" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divActividades">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="roller" class="habilidades">
                        <div id="rollslider2">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div id="popUpHora" class="popUp">
            <div id="SelectorHora">
                        <input type="text" id="TimeDemo" />
                   <br/>
                    <input type="button" id="Button2" value="Cancelar" class="" onclick="$('#popUpHora').hide();"
                    style="float: none;" />
                        <input type="button" id="btnSeleccionarHora" value="OK" onclick="asignarHora();"
                            style="float: none;" />
                    </div>
            <script type="text/javascript">
                $("#TimeDemo").AnyTime_picker({
                    format: "%m-%d %H:%i",
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
       <%-- <div id="popUpFecha" class="popUp">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <input type="text" id="DateDemo" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarFecha();"
                            style="float: none;" />
                    </td>
                </tr>
            </table>
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
        </div>--%>
    </div>
</asp:Content>
