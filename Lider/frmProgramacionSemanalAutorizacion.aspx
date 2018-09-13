<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmProgramacionSemanalAutorizacion.aspx.cs" Inherits="Lider_ProgramacionSemanalAutorizacion" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/slider/slick.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/dates.js" type="text/javascript"></script>
    <link href="../comun/scripts/fullcalendar.min.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/fullcalendar.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/fullcalendar_es.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" id="Variables Globales">
    var dividedEvents = [];
</script>
<style type="text/css">
    #popUp_Lideres
    {
            position: fixed;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            top: 0% !important;
            z-index: 100000;
            left: 0%;
            overflow-x: hidden;
            overflow-y: auto;
            border: 1px solid #cccccc;
            max-width: 100%;
            -moz-box-shadow: 0 0 9px #999999;
            -webkit-box-shadow: 0 0 9px #999999;
            box-shadow: 0 0 9px #999999;
            display: none;
        }
        .popUpData
        {
      /*margin-top:10%;
      margin-left:35%;*/
      background:#fff; 
      border:2px solid black; 
      width:450px; 
      display:table;
      -moz-border-radius:6px;
      -webkit-border-radius:6px;
    border-radius:6px;
    text-align:-webkit-left;
    position:absolute;
    left: 50%;
    top: 50%;
    transform: translateX(-50%) translateY(-50%);
  }
  
  h1
  {
      width: 100%;
      }
  
  .gridView{
        min-width:450px !important;
    }
    
    .container
    {
        display: inherit !important;
        width: 1024px;
        max-width: 1280px;
        overflow: auto;
        }
     table.index3
     {
         width: 99% !important;
         }
         
         .container_table_lideres
         {
             width: 100%;
             max-width: 100%;
             overflow-x: auto;
             }
</style>
<script type="text/javascript" id="inicializacion">
    $(function () {
        semanaActual = parseInt($('[id*="ltSemana"]').text());
        $('#txtSemanaCalendario').val(semanaActual);
        $('#txtAnioCalendario').val(new Date().getFullYear());

        //Funcion para cargar Lideres y ensayos
        PageMethods.ObtenerLiderEInvernadero(function (response) {
            if (response[0] == '1') {
                $('#ddlLider').html(response[2]);
                $('.trInvernaderos').html(response[3]);
                $('table#LideresGerente').html(response[4]);

            } else {
                popUpAlert('No se encontraron lideres con invernaderos.', 'info');
                $('#ddlLider').html('<option value="" idLider="0">--Seleccione--</option>');
                $('.trInvernaderos').html("");
            }
        });

        $('select#ctl00_ddlPlanta').live('change', function () {
            CargarLider();
        });

        $('input[type="checkbox"].chkLider').live('change', function () {
            var obj = $(this);
            var idLider = obj.attr('idLider');
            var idGerente = obj.attr('idGerente');
            var act = obj.attr('checked') == 'checked' ? 1 : 0; ;
            try {
//                popUpAlert('sadfasdf', 'success');
                                PageMethods.QuitaAgregaLider(idGerente, idLider, act, function () { 
                if (act == 1) {
                    obj.parent().addClass('Owner');
                }
                else {
                    obj.parent().removeClass('Owner');
                }
                                });
            }
            catch (e) {
                console.log(e);
            }
        });

        $('#ddlLider').live('change', function () {
            var idLider = $('#ddlLider option:selected').attr('idLider');

            if (idLider != 0) {
                $('tr.trInvernaderos').find('input[idLider').parent().hide();
                $('tr.trInvernaderos').find('input[idLider').prop('checked', false);
                $('tr.trInvernaderos').find('input[idLider="' + idLider + '"]').parent().show();

                //Ejecutamos la funcion para obtener la programacion semanal
                getProgramacionSemanal();
                $('#SelectorSemana').show();
                $('#botones').show();
            } else {
                $('#popUpCalendario').hide();
                $('tr.trInvernaderos').find('input[idLider').parent().hide();
                $('#botones').hide();
                $('#SelectorSemana').hide();
            }
        });

    });

    function CargarLider() {
        PageMethods.ObtenerLiderEInvernadero(function (response) {
            if (response[0] == '1') {
                $('#ddlLider').html(response[2]);
                $('.trInvernaderos').html(response[3]);
                $('table#LideresGerente').html(response[4]);

//                $('#ddlLider').change(function () {
//                    var idLider = $('#ddlLider option:selected').attr('idLider');

//                    if (idLider != 0) {
//                        $('tr.trInvernaderos').find('input[idLider').parent().hide();
//                        $('tr.trInvernaderos').find('input[idLider').prop('checked', false);
//                        $('tr.trInvernaderos').find('input[idLider="' + idLider + '"]').parent().show();

//                        //Ejecutamos la funcion para obtener la programacion semanal
//                        getProgramacionSemanal();
//                        $('#SelectorSemana').show();
//                        $('#botones').show();
//                    } else {
//                        $('#popUpCalendario').hide();
//                        $('tr.trInvernaderos').find('input[idLider').parent().hide();
//                        $('#botones').hide();
//                        $('#SelectorSemana').hide();
//                    }
//                });
            } else {
                $('#ddlLider').html('<option value="" idLider="0">--Seleccione--</option>');
                $('.trInvernaderos').html("");
                popUpAlert('No se encontraron lideres con invernaderos.', 'info');
            }
        });
    }

    function getProgramacionSemanal() {
        var semana = $('#txtSemanaCalendario').val();
        var anio = $('#txtAnioCalendario').val();
        var idPlanta = $('#ddlLider option:selected').attr('idPlanta');
        var idLider = $('#ddlLider option:selected').attr('idLider');

        PageMethods.ObtenerProgramacionSemanal(semana, anio, idPlanta, idLider, function (response) {
            if (response.length > 0) {
                dividedEvents = JSON.parse(response);
            }
            mostrarCalendario();
        });
    }

    function crearCalendario(dividedEvents) {
        $('#calendar').fullCalendar({
            editable: true,
            droppable: false,
            draggable: false,
            defaultView: 'agendaWeek',
            fixedWeekCount: true,
            height: 600,
            header: { left: 'title', center: 'month, agendaWeek', right:''},
            allDaySlot: false,
            firstDay: 0,
            selectable: false,
            timeFormat: 'HH:mm',
            slotDuration: '00:30:00',
            slotLabelInterval: '01:00:00',
            snapDuration: '00:05:00',
            eventResizeStart: function () {
                $('#calendar').fullCalendar('removeEvents', 'Cal');
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
   

    function mostrarCalendario() {

        $('#popUpCalendario').show().animate({ top: ($(window).scrollTop() + 50) + 'px' });
        crearCalendario(dividedEvents);
        filterbySite();
        $('.fc-agendaWeek-view').append('<div id="readOnlyCalendar"></div>');
        $('#readOnlyCalendar').css({ 'width': ($('.fc-agendaWeek-view').width()-15) + 'px', 'height': $('.fc-agendaWeek-view').height() + 'px', ' display': ' block', 'float': ' left', 'position': ' absolute', 'top': ' 0px', 'z-index': '999', 'background': ' rgba(5, 5, 5, 0.0000001)' });
       
    }

    function filterbySite() {
        var items = [];
        var fechas = [];

        $('#calendar').fullCalendar('removeEvents');

        $('input.filtroInvernadero:checked').each(function () {
            var option = $(this).attr('idinvernadero');
            dividedEvents.filter(function (i, n) { if (i.idInvernadero == option) { items.push(i); } });
        });
        
        //                $('#calendar').fullCalendar('destroy');
        //                crearCalendario(items);
        $('#calendar').fullCalendar('addEventSource', items);
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

    function Guardar(aprobado) {
        var ParaAutorizar=[];
        //var AutorizarPlan = [];
        try {

            $('input.filtroInvernadero:checked').each(function () {
                    for (var a in dividedEvents) {
                        if (dividedEvents[a].idInvernadero == $(this).attr('idInvernadero')) {
                            ParaAutorizar.push({
                                'anio': dividedEvents[a].anio,
                                
                                
                                'idActividad': dividedEvents[a].idActividad,
                                'idInvernadero': dividedEvents[a].idInvernadero,
                                
                                'idPeriodo': dividedEvents[a].idPeriodo,
                                'semana': dividedEvents[a].semana
                            });

                            
                        }
                    }
                });

                ProgramacionAutoriza = {
                    AutorizarPlan: ParaAutorizar,
                    aprobado: aprobado,
                    comentarios: $('textarea#comentarios').val(),
                    idLider: $('#ddlLider option:selected').attr('idLider')
            }

            PageMethods.GuardarAutorizacion(ProgramacionAutoriza, function (response) {
                popUpAlert(response.split('|')[0], response.split('|')[1]);

                if (response.split('|')[0] == 'OK') {
                    getProgramacionSemanal();
                    $('#comentarios').val('');
                }
                else {
                    popUpAlert(response.split('|')[0], 'error');
                }
            });

        } catch (e) {
            Console.Log(e);
        }
    }

//    $('#txtSemanaCalendario').live('change', function () {
//        PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
//            rangosemana = JSON.parse(response);
//            rangosemana[0].EndDate += 86340000;
//        });
//    });

    function semanaAnterior() {
        var semana = parseInt($('#txtSemanaCalendario').val());
        var anio = parseInt($('#txtAnioCalendario').val());
        maxAnioAnterior = 52;
        if (semana == 1) {
            semana = maxAnioAnterior;
            anio = parseInt($('#txtAnioCalendario').val()) - 1;
            $('#txtSemanaCalendario').val(semana);
            $('#txtAnioCalendario').val(anio);
        }
        else {
            semana = parseInt(semana) - 1;
            $('#txtSemanaCalendario').val(semana);
        }
//        PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
//            rangosemana = JSON.parse(response);
//            rangosemana[0].EndDate += 86340000;
//        });
        //cargaHabilidadesDirectrizParaSemanaAnio(semana, anio);
        $('#calendar').fullCalendar('prev');
        getProgramacionSemanal();
    }

    function semanaSiguiente() {
        var semana = $('#txtSemanaCalendario').val();
        var anio = $('#txtAnioCalendario').val();
        maxAnioActual = 53;
        maxAnioSiguiente = 52;
        if (semana == maxAnioActual) {
            semana = 1;
            anio = parseInt($('#txtAnioCalendario').val()) + 1;
            $('#txtSemanaCalendario').val(1);
            $('#txtAnioCalendario').val(anio);
        }
        else {
            semana = parseInt(semana) + 1;
            $('#txtSemanaCalendario').val(semana);
        }
//        PageMethods.InicioFinSemana($('#txtSemanaCalendario').val(), $('#txtAnioCalendario').val(), function (response) {
//            rangosemana = JSON.parse(response);
//            rangosemana[0].EndDate += 86340000;
//        });
        //cargaHabilidadesDirectrizParaSemanaAnio(semana, anio);
        $('#calendar').fullCalendar('next');
        getProgramacionSemanal();
    }

    function getWeek() {
        var moment = $('#calendar').fullCalendar('getDate');
        var date = moment.format();
        PageMethods.ObtieneSemanaNS(date, function (response) { return response; });
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Programación Semanal Autorización</asp:Label>
            <span id="product"></span>
            <input type="button" id="btn_config" value="Seleccionar Lideres" onClick="$('#popUp_Lideres').show('slow');" style="visibility:hidden"/>
        </h1>
        <div id="divLideres">
        <div class="container_table_lideres">
              <table class="index3">
                <tr>
                    <td><asp:Label runat="server" ID="lblLider" Text="Lider:"></asp:Label></td>
                    <td colspan="2"><select id="ddlLider" class="selector">
                            <option value="0">--Seleccione--</option>
                        </select>
                    </td>
                </tr>
                  <tr class="trInvernaderos">
                </tr>
                </table>
            </div>
            <br />

            <table id="SelectorSemana" style="display:none">
                <tr>
                    <td align="center" style="text-align: center;">
                        <img src="../comun/img/prev.png" style="float: none;" onclick="semanaAnterior();" />
                        <input id="txtSemanaCalendario" type="text" style="float: none; width: 60px; text-align: center;"
                            readonly />
                        <input id="txtAnioCalendario" type="text" style="float: none; width: 60px; text-align: center;"
                            readonly />
                        <img src="../comun/img/next.png" style="float: none;" onclick="semanaSiguiente();" />
                    </td>
                </tr>
            </table><br />
        </div>
        <div id="popUpCalendario">
            <div id="popUpContenido" style="max-height: 80%;">
                <div id="calendar" class="programacionSemanal">
                </div>
            </div>
        </div><br />
        <div id="botones" style="display:none">
            <table>
                <tr>
                    <td><label id="lblComentarios">Comentarios:</label></td>
                </tr>
                <tr>
                    <td><textarea cols="40" rows="4" id="comentarios"></textarea></td>
                </tr>
            </table>
            <table align="right">
                <tr>
                    <td><input type="button" value="Rechazar" onclick="Guardar(false);" /></td>
                    <td><input type="button" value="Aceptar" onclick="Guardar(true);" /></td>
                </tr>
            </table>
        </div>
    </div>

    <div class="popUp" id="popUp_Lideres" >
        <div class="popUpData" >
            <table id="LideresGerente" class="gridView"><tr><th>Lider</th><th>Gerente</th></tr></table>
             <input type='button' value="Cerrar" onclick="$('#popUp_Lideres').hide('slow');" />
        </div>
        
       
    </div>
</asp:Content>

