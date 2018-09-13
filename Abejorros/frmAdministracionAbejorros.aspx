<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmAdministracionAbejorros.aspx.cs" Inherits="frmAdministracionAbejorros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/scripts/jquery-1.7.2.js"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../comun/scripts/slider/slick.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" id="escenarioInicial">

        function activeDirectory(cuenta) {
            PageMethods.activeDirectory($(cuenta).val(), function (response) {
                $("#mail-" + $(cuenta).attr("invernadero")).text(response);
                if (response != "") {
                    $("#cuenta-" + $(cuenta).attr("invernadero")).removeClass("error");

                } else {
                    $("#cuenta-" + $(cuenta).attr("invernadero")).addClass("error");
                }
            });
        }

        function solicitarColmenas(solicitud) {
            var registrar = true;
            var id = $(solicitud).attr("invernadero");
            $(".required[invernadero=" + id + "]").each(function () {
                if ($(this).val() == '') {
                    registrar = false;
                    $(this).addClass("error");
                }
                else {
                    $(this).removeClass("error");
                }
            });

            if (registrar) {

                if (!$("#cuenta-" + $(solicitud).attr("invernadero")).hasClass("error")) {
                    PageMethods.solicitarColmenas(id, $("#cantidad-" + id).val(), $("#semana-" + id).val(), $("#mail-" + id).text(), function (response) {
                        popUpAlert(response[2], response[1]);
                    });
                } else {
                    popUpAlert("El usuario a quien intenta enviar el correo no esta registrado", "alert");
                }
            }
            else {
                popUpAlert('Aún no ha ingresado todos los datos requeridos.', 'error');
            }
        }

        function editarFolio(option) {
            id = $(option).parent().attr("idinvernadero");
            if ($(option).find('option:selected').attr("editar") == "True") {
                $(option).find('option:selected').parent().parent().prev().children().removeAttr("readonly");
                $(option).find('option:selected').parent().parent().prev().children().removeClass("folioRead");
            } else {
                $(option).find('option:selected').parent().parent().prev().children().attr("readonly", "");
                $(option).find('option:selected').parent().parent().prev().children().addClass("folioRead");
                $(option).find('option:selected').parent().parent().prev().children().val($(option).find('option:selected').parent().parent().prev().children().attr("vprev"));
            }
        }


        function saveFolios(clase) {
            //$.blockUI();

                        var registrar = true;
            $("#tablaColmenas-" + clase + " .requerid").each(function () {
                if ($(this).val() == '') {
                    registrar = false;
                    $(this).css({ 'border': '1px solid red' });
                }
                else {
                    $(this).css({ 'border': '1px solid black' });
                }
            });

            if (registrar) {
             var repetidos = true;

                $("#tablaColmenas-" + clase + " .repetido").each(function () {
                        repetidos = false;
                });

                if (repetidos) {

                    var colmenas = "";
                    var folios = "";
                    var acciones = "";
                    var comentarios = "";

                    $(".folio-" + clase).each(function () {
                        if ($(this).parent().parent().hasClass("change")) {
                            colmenas += '|' + $(this).parent().parent().attr("idColmenas");
                            folios += '|' + $(this).val();
                            acciones += '|' + $(this).parent().next().children().find('option:selected').val();
                            comentarios += '|' + $(this).parent().next().next().children().val();
                        }
                    });

                    if (folios != "") {
                        PageMethods.guardaColmenas(colmenas, folios, acciones, comentarios, function (result) {
                            popUpAlert(result[1], result[0]);
                            //$("div[idinvernadero=1] table tr").removeClass("change");

                            PageMethods.cargaColmenasPorInvernadero(clase, function (response) {
                                $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + clase + '] .accordionBody').html(response);
                                $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + clase + '] table').css('width', '100%');
                                registerControlsMulti(clase);
                                cambio();
                            });
                        });
                    } else {
                        $.unblockUI();
                        popUpAlert("No hay colmenas que guardar", "info");
                    }
                }
                else {
                    popUpAlert('Existen folios previamente guardados o repetidos', 'error');
                }
            }
            else {
                popUpAlert('Por favor llene los campos requeridos', 'error');
            }
        }

        function formularioColmena(idInvernadero, nombreInvernadero) {
            return '<div class="mantenimientoColmena" idInvernadero="' + idInvernadero + '" nombreInvernadero="' + nombreInvernadero + '">                                                       ' +
        '       <div class="accordionHeader" onclick="acordeon($(this));">                                                  ' +
        '           <label>' + nombreInvernadero + '</label>                                                                    ' +
        '           <img class="accordionMuestra" alt="" src="../comun/img/sort_desc.png" style="float:right;"/>            ' +
        '           <img class="accordionOculta" alt="" src="../comun/img/sort_asc.png" style="float:right; display:none;"/>' +
        '       </div>                                                                                                      ' +
        '       <div class="accordionBody">                                                                                 ' +
        '       </div>                                                                                                      ' +
        '</div>';
        }

        $(function () {
            cargaInvernaderos();
        });

        function cambio() {
            $('.incidencias, .comentarios').change(function () {
                $(this).parent().parent().addClass("change");
            });


            $(".folios").change(function () {
                var folio = $(this);
                PageMethods.exiteFolio($(this).val(), function (result) {
                    if (result == "1") {
                        $(folio).addClass("Error");
                        $(folio).addClass("repetido");
                    } else {
                        var repetido = 0;
                        $(".folios").each(function () {

                            if ($(this).val() == $(folio).val()) {
                                ++repetido;
                            }

                        });
                        if (repetido > 1) {
                            $(folio).addClass("Error");
                            $(folio).addClass("repetido");
                        } else {
                            $(folio).removeClass("Error");
                            $(folio).removeClass("repetido");
                        }
                    }
                });
            });
        }

        function registerControlsMulti(id) {
            if ($("#tablaColmenas-" + id).find("tbody").find("tr").size() >= 1) {
                var pagerOptions = { // Opciones para el  paginador
                    container: $("#pager-" + id),
                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                };

                $("#tablaColmenas-" + id).tablesorter({
                    widthFixed: true,
                    widgets: ['zebra', 'filter'],
                    headers: { /*0: { filter: false} */
                },
                widgetOptions: {
                    zebra: ["gridView", "gridViewAlt"]
                    //filter_hideFilters: true // Autohide
                }
            }).tablesorterPager(pagerOptions);

            $(".tablesorter-filter.disabled").hide(); // hide disabled filters
        }
        else {
            $("#pager-" + id).hide();
        }
    } 

    </script>
    <script id="Invernaderos" type="text/javascript">

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

        function cargaInvernaderos() {
            PageMethods.cargaInvernaderosSlider(function (response) {
                $('#rollslider').removeClass();
                $('.invernaderos #rollslider').html(response);
                setInvernaderos();
            });
        }

        function setInvernaderos() { //Inicializa los controles Slider en los que se muestran las plantas
            $('#rollslider').slick({
                slidesToShow: $('#rollslider div').length < 12 ? $(this).length : 12,
                slidesToScroll: $('#rollslider div').length > 12 ? 5 : 2,
                infinite: false,
                variableWidth: true
            });

            $('.divInvernadero ').mousedown(function (event) {
                if (event.which == 1) {
                    $(this).addClass('clicked');
                }

            });

            $('.divInvernadero ').mouseup(function (event) {
                try {
                    var invernaderoID = $(this).attr('ID');
                    var claveInvernadero = $(this).text();

                    if ($(this).attr('class').indexOf('selected') > -1) {
                        $(this).removeClass('selected');
                        $(this).attr('selected', false);
                    }
                    else {
                        $(this).addClass('selected');
                        $(this).attr('selected', true);
                    }

                    if ($('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + invernaderoID + ']').length > 0) {
                        $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + invernaderoID + ']').toggle();
                    }
                    else {
                        PageMethods.cargaColmenasPorInvernadero(invernaderoID, function (response) {
                            $('#divMantenimientosColmenas').append(formularioColmena(invernaderoID, claveInvernadero));
                            $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + invernaderoID + '] .accordionBody').html(response);
                            $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + invernaderoID + '] table').css('width', '100%');
                            registerControlsMulti(invernaderoID);
                            cambio();
                        });
                    }


                } catch (e) {
                    window.console && console.log(e);
                } finally {
                    $(this).removeClass('clicked');
                }

            });
        }

    </script>
    <script id="" type="text/javascript">
        /*corregir función*/
        function almacenarMantenimientos() {
            window.console && console.log("..almacenar..");
            var insert = true;
            var manttos = [];
            //  CargaActividadesJson();
            //  changeEvents();
            $('#divMantenimientosColmenas div:not(.accordionHeader, .accordionBody)').each(function () {

                window.console && console.log("..each 1..");
                var divInvernadero = $(this);
                // var idInvernadero = $(this).attr('id').split('_')[1];
                if ($(divInvernadero).find('.error').length > 0) {
                    insert = false;
                    window.console && console.log("..each 1 err..");
                }

                if ($(this).css('display') != 'none') {

                    window.console && console.log("..dispay none.." + ($(this).css('display') == 'none'));

                    $(divInvernadero).find('.mantenimiento tbody tr:not(.trLoad)').each(function () {

                        if ($(this).find('.Mantenimiento').attr('idMantto') == 2) {
                            window.console && console.log("..each..  id " + $(this).attr('idColmenas').toString());
                            window.console && console.log("       idInv " + $(this).attr('idInvernadero'));
                            window.console && console.log("      semana " + $(this).find('.Semana').text());
                            window.console && console.log("      mantto " + $(this).find('.Mantenimiento').attr('idMantto'));
                            window.console && console.log("     colPlan " + $(this).find('.ColmenasPlaneadas').text());
                            window.console && console.log("       polin " + $(this).find('.Polinizacion').val());
                            window.console && console.log("    lvlPolin " + $(this).find('.NivelPolinizacion').val());
                            window.console && console.log("      Observ " + $(this).find('.Observaciones').val());


                            manttos.push({
                                'idColmenas': $(this).attr('idColmenas').toString(),
                                'idInvernadero': $(this).attr('idInvernadero'),
                                'SemanaNS': $(this).find('.Semana').text(),
                                'Mantenimiento': $(this).find('.Mantenimiento').attr('idMantto'),
                                'ColmenasPlaneadas': $(this).find('.ColmenasPlaneadas').text(),
                                'Polinizacion': $(this).find('.Polinizacion').val(),
                                'NivelPolinizacion': $(this).find('.NivelPolinizacion').val(),
                                'Observaciones': $(this).find('.Observaciones').val()

                            });
                        }
                    });
                }

            });

            if (insert) {
                PageMethods.almacenarMantenimientos(manttos, function (response) {
                    popUpAlert(response.split('|')[0], response.split('|')[1]);
                });
            } else {
                popUpAlert('<asp:Literal runat="server" Text="<%$ Resources:Commun, PorLlenar %>"/>', 'warning');
            }
        }


        $('#ctl00_ddlPlanta').live('change', function () {
            $.blockUI();
            cargaInvernaderos($.unblockUI())
        });

    </script>
    <script id="UtileriasYDiseño" type="text/javascript">
        function acordeon(header) {
            $(header).find('.accordionMuestra').toggle();
            $(header).find('.accordionOculta').toggle();
            $(header).next().toggle();
        }
    </script>
    <style type="text/css">
        .Error
        {
            border: 1px solid red !important;
            background: rgba(255,0,0,0.2) !important;
        }
        .accordionHeader
        {
            display: inline-block;
            text-align: left;
            background-color: #ADC995;
            color: white;
            font-size: 18px;
            border: 1px solid #FF6600;
            width: 100%;
            font-size: 15px;
            padding-top: 5px;
            padding-bottom: 5px;
        }
        .accordionHeader:hover
        {
            cursor: pointer;
            background-color: #FAE258;
            color: #FF6600;
        }
        .accordionBody
        {
            border: 1px solid #ADC995;
            width: 100%;
            background: #E7F9D8;
            display: table;
        }
        
        #rollslider div
        {
            width: 800px;
        }
        
        .slick-slide
        {
            width: 60px !important;
            cursor: pointer;
            display: none;
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
            display: block !important;
        }
        
        .invernaderos
        {
            padding: 0 25px;
        }
        .slidInvernaderos h2
        {
            color: #ADC995;
            margin: 0;
            display: table;
            width: auto;
            margin-left: auto;
            margin-right: auto;
            font-size: 19px;
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
        
        tr td table.colmenas
        {
            min-width: auto !important;
            max-width: auto;
            width: 100%;
        }
        
        table.solicitud
        {
            width: auto !important;
            float: right !important;
            padding: 10px;
        }
        
        input.error
        {
            border: 1px solid red !important;
            background: rgba(255,0,0,0.2);
        }
        
        .invisible
        {
            display: none !important;
        }
        
        table.colmenas
        {
            max-width: 100% !important;
        }
        .folioRead
        {
            border: none !important;
            background: transparent;
        }
        
        .wrap
        {
            width: 20px !important;
            white-space: pre-wrap !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
    <div class="container">
        <h1>
            <asp:Label ID="lblTitulo" runat="server" Text="Administración de abejorros"></asp:Label>
        </h1>
        <table class="index slidInvernaderos">
            <tr>
                <td>
                    <h2>
                        <asp:Literal ID="ltInvernaderos" meta:resourceKey="ltInvernaderos" runat="server" /></h2>
                </td>
                <td>
                    <input type="button" value="Eliminar Selección" onclick="EliminarSeleccionDeInvernaderos();" />
                    <input type="button" value="Seleccionar Todos" onclick="ClickACadaInvenradero();" />
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
                        <asp:Literal ID="ltSub" meta:resourceKey="ltSub" runat="server" />
                    </h2>
                </td>
            </tr>
            <tr>
                <td>
                    <%--<input type="button" value="Guardar Semana" onclick="almacenarMantenimientos();" />--%>
                    <input type="button" value="Colapsar" onclick="$('.accordionBody').slideUp();" />
                    <input type="button" value="Extender" onclick="$('.accordionBody').slideDown();" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divMantenimientosColmenas">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
