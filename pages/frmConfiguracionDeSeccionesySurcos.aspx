<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmConfiguracionDeSeccionesySurcos.aspx.cs"
    Inherits="pages_frmInvernaderosPorLider" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

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
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations.js"></script>
    <style type="text/css">
        .Error{ border: 1px solid red !important; }
    </style>
    <script type="text/javascript">

        var bloqueoDePantalla = {
            transaccionTerminada: false,
            bloquearPantalla: function () {
                $.blockUI();
            },
            indicarTerminoDeTransaccion: function () {
                transaccionTerminada = true;
            },
            desbloquearPantalla: function () {
                var intervalo = window.setInterval(function () {
                    if (transaccionTerminada) {
                        $.unblockUI();
                        window.clearInterval(intervalo);
                        transaccionTerminada = false;
                    }
                }, 10);                
            }
        }

        function obtenerInformacion() {
            var ultimoSurco = 0;
            return Invernaderos = $('.DivAcordion').map(function () {
                return invernadero = {
                    idInvernadero: $(this).attr('idInv'),
                    clave: $(this).attr('invernadero'),
                    Secciones: $(this).find('.Seccion').map(function (indice, objeto) {
                        return seccion =
                        {
                            IdSeccion: 0,
                            NumeroSeccion: indice + 1,
                            NombreSeccion: indice + 1,                                           
                            //NombreSeccion: indice + 1, 
                            Surcos: $($(this).find('tr:not(.Deleted)').map(function () {
                                if ($(this).find('.numeroDeSurco').attr('noSurco') != undefined)
                                    return this;
                            }).get()).map(function (indx, obj) {
                                if (indice == 0 && indx == 0) {
                                    ultimoSurco = 1;
                                } else {
                                    ultimoSurco++;
                                }
                                return surco = {
                                    IdSurco: 0,
                                    ClaveSurco: ultimoSurco,
                                    Longitud: $(this).find('input[type="text"]').first().val(),
                                    EsRD: $(this).find('input[type="checkbox"]').eq(0).prop('checked'),
                                    Activo: $(this).find('input[type="checkbox"]').eq(1).prop('checked')
                                }
                            }).get()
                        }
                    }).get()
                }
            }).get();
            }



            function guardarCambios(event) {
                //Valida
                if ($('input[type="text"][invernadero]').map(function () {
                    if ($(this).val() == '') {
                        $(this).addClass('Error');
                        return $(this);
                    }
                    else {
                        $(this).removeClass("Error");
                    }
                }).get().length == 0) {
                    try {
                        bloqueoDePantalla.bloquearPantalla();
                        var finalizoTransaccion = false;
                        PageMethods.AlmacenarInvernaderos(obtenerInformacion(), function (response) {
                            if (response[0] == 'ok') {
                                popUpAlert(response[1], response[0]);
                            } else {
                                popUpAlert(response[1], response[0]);
                            }
                            bloqueoDePantalla.indicarTerminoDeTransaccion();
                        }, function () {
                            popUpAlert('Falla en servicio. Contacte con el administrador del sistema.', 'error');
                            bloqueoDePantalla.indicarTerminoDeTransaccion();
                        });
                        bloqueoDePantalla.desbloquearPantalla();
                    } catch (e) {
                        console.log(e);
                        $.unblockUI();
                    }
                }
                else {
                    popUpAlert('No se ha llenado la longitud de todos los surcos.', 'error');
                }
        }

    </script>

    <script type="text/javascript">
        function GenerarGridInvernadero(obj) {
            var Secciones = document.getElementById("txtSeccion" + obj.attr('Inv')).value;
            var Surcos = document.getElementById("txtSurco" + obj.attr('Inv')).value;
            var Longitud = document.getElementById("txtLongitud" + obj.attr('Inv')).value;
            var Investigacion =  $('#chkSeccion' + obj.attr('Inv')).prop('checked'); // document.getElementById().value;
            var idInvernadero =obj.attr('IdInv');
            var invernadero = obj.attr('Inv');
            var errores="";

            

            errores+= (invernadero == undefined || invernadero==null || invernadero == '') ? 'No se encontró el invernadero.<br/>':'';
            errores+= (Surcos == undefined || Surcos == null || Surcos =='') ? 'Debe indicar el número de surcos.<br/>':'';
            errores+= (Secciones == undefined || Secciones == null || Secciones =='') ? 'Debe indicar el número de secciones.<br/>':'';
            errores+= (idInvernadero == undefined || idInvernadero == null || idInvernadero =='') ? 'No se encontró el invernadero.<br/>':'';
            if(errores =='')
            {
                PageMethods.ObtenerContenidoInvernadero(invernadero, idInvernadero, Secciones, Surcos, Longitud, Investigacion, function (response) {
                    $("#DIV" + obj.attr('Inv')).empty();
                    $("#DIV" + obj.attr('Inv')).append("<span class=\"generadorDeSurcosYSecciones\"> Seccion:<input class=\"cajaCh\" id='txtSeccion" + obj.attr('Inv') + "' type='text' /> Surcos:<input class=\"cajaCh\" id='txtSurco" + obj.attr('Inv') + "' type='text' /> Longitud Promedio:<input class=\"cajaCh\" id='txtLongitud" + obj.attr('Inv') + "' type='text' /> "+
                     "<label>Investigación</label><input id=\"chkSeccion" + obj.attr('Inv') + "\" type=\"checkbox\" />" +
                     "<input type='button' IdInv='" + obj.attr('IdInv') + "' Inv='" + obj.attr('Inv') + "' value='¡Generar!' onclick='GenerarGridInvernadero($(this));' /> </span>");
                    $("#DIV" + obj.attr('Inv')).append(response);
                    $('#accordion').accordion('destroy').accordion({ active: false, collapsible: true });
                    $("#DIV" + obj.attr('Inv')).show();
                });
            }
            else
            {
                popUpAlert(errores,'error');
            }
        }
        $(function () {

            $('.buscador').live('keyup',function(){
                var texto = $(this).val().toUpperCase();
                if(texto =='' || texto == undefined){
                      $('div.slick-slide').show();
                }else{
                     $('div.slick-slide').hide();
                     $('div.slick-slide:contains("'+texto.toUpperCase() +'")').show();
                }
                
            });

            $("#accordion").accordion({ active: false, collapsible: true });
            registerControls();
            $('#ctl00_ddlPlanta').change(function () {
                $('.slick-slide').html('');
                $('.slick-slide').css({ 'background-image': 'url("../comun/scripts/slider/ajax-loader.gif")',
                    'background-repeat': 'no-repeat',
                    'background-position': 'center'
                });
                PageMethods.cargaInvernaderosPlanta(($('#ctl00_ddlPlanta').length > 0 ? $('#ctl00_ddlPlanta').val() : <%=Session["idPlanta"].ToString()%>) , function (response) {
                    $('#rollslider2').removeClass();
                    $('.habilidades #rollslider2').html(response);
                    setHabilidades();
                });
            });
            PageMethods.cargaInvernaderosPlanta(($('#ctl00_ddlPlanta').length > 0 ? $('#ctl00_ddlPlanta').val() : <%=Session["idPlanta"].ToString()%>) , function (response) {
                $('#rollslider2').removeClass();
                $('.habilidades #rollslider2').html(response);
                setHabilidades();
            });
              setHabilidades();
        });

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
                        'zIndex': 9
                    });
                },
                cursor: 'move',
                containment: 'document'
            });

            $('#rollslider2 .slick-slide').mouseup(function (e) {
                bloqueoDePantalla.bloquearPantalla();
                var selected = $(this);
                if (selected.attr('selected')) {
                    selected.removeClass('selected');
                    selected.attr('selected', false);
                    $('#DIV' + $(this).text()).remove();
                    $('#H3' + $(this).text()).remove();
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                } else {
                    selected.attr('selected', true);
                    selected.addClass('selected');
                    var Inv = $(this).text();
                    if ($('#DIV' + $(this).text()).length) {
                        $('#DIV' + $(this).text()).remove();
                        $('#H3' + $(this).text()).remove();
                    }
                    PageMethods.ObtenerConfiguracionActual($(this).text(), $(this).attr('id'), function (response) {
                        $('#accordion').append(response)
                        $('#accordion').accordion('destroy').accordion({ active: false, collapsible: true });
                        $("#DIV" + Inv).show();
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                    }, function(e){
                        console.log(e);
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                    });
                }
                bloqueoDePantalla.desbloquearPantalla();
            });
        }

        function numerate(obj){
         var NuevoNoSurco = 0;
             $('.numeroDeSurco[invernadero=' + obj.parent().attr('Invernadero') + ']').each(function(indice,elemento){
                if ($(elemento).parents('tr').hasClass('Deleted') == false)
                {
                    NuevoNoSurco = NuevoNoSurco + 1;
                    $(elemento).text((NuevoNoSurco).toString());
                }
            });

        }

        function EliminarSurco(obj) {
            // Tomo el indice nuevo para agregar la fila a la tabla:
            var NoFila = parseInt(obj.parent().attr('NoFila'));
            var NoSurco = parseInt(obj.parent().attr('NoSurco'));

            // Asigno nuevos valores a los surcos antes de insertar la fila:
           
            var NuevoNoFila = 0;

            // Elimino de la vista del usuario el renglon
            obj.parents('tr').addClass('Deleted');

            // Calculo el nuevo indice:
           numerate(obj);

        }

        function AgregarSurco(obj) {
            // Tomo el indice nuevo para agregar la fila a la tabla:
            var NoFila = parseInt(obj.parent().attr('NoFila')) + 1;
            var NoSurco = parseInt(obj.parent().attr('NoSurco')) + 1;

            // Asigno nuevos valores a los surcos antes de insertar la fila:
            var NuevoNoSurco = 0;
            var NuevoNoFila = 0;

            $("#DIV" + obj.parent().attr('Invernadero')).each(function () {

                $('.Seccion tbody tr').each(function () {

                    if ($(this).find("td").eq(5).find('span').attr('idInv') == obj.parent().attr('idInv')) {

                        NuevoNoSurco = $(this).find("td").eq(0).find('span').html();
                        NuevoNoFila = $(this).find("td").eq(5).find('span').attr('NoFila');

                        if (parseInt(NuevoNoSurco) >= parseInt(NoSurco)) {
                            NuevoNoSurco = (parseInt(NuevoNoSurco) + 1).toString();
                            NuevoNoFila = (parseInt(NuevoNoFila) + 1).toString();

                            // Cambio el numero de surco a la vista del usuario:
                            $(this).find("td").eq(0).find('span').text(NuevoNoSurco);

                            // Cambio los atributos de los elementos de cada columna:
                            $(this).find("td").eq(5).find('span').attr({
                                'NoFila': NuevoNoFila,
                                'NoSurco': NuevoNoSurco,
                                'id': "Surco" + NuevoNoSurco + ""
                            });
                        }
                    }
                });
            });

            // Genero el codigo HTML del renglon:
            var html = "<tr>";
            html = html + "<td><span id='Surco" + NoSurco + "' class='numeroDeSurco' idInv='" + obj.parent().attr('idInv') + "' NoSurco='" + NoSurco + "' NoSeccion='" + obj.parent().attr('NoSeccion') + "' Invernadero='" + obj.parent().attr('Invernadero') + "' NoFila='" + NoFila + "'>" + NoSurco + "</span></td>";
            html = html + "<td><input class=\"floatValidate\" type='text' id='Surco" + NoSurco + "' idInv='" + obj.parent().attr('idInv') + "' NoSurco='" + NoSurco + "' NoSeccion='" + obj.parent().attr('NoSeccion') + "' Invernadero='" + obj.parent().attr('Invernadero') + "' NoFila='" + NoFila + "' /></td>";
            html = html + "<td><input type='checkbox' id='Surco" + NoSurco + "' idInv='" + obj.parent().attr('idInv') + "' NoSurco='" + NoSurco + "' NoSeccion='" + obj.parent().attr('NoSeccion') + "' Invernadero='" + obj.parent().attr('Invernadero') + "' NoFila='" + NoFila + "' /></td>";
            html = html + "<td><input type='checkbox' id='Surco" + NoSurco + "' idInv='" + obj.parent().attr('idInv') + "' NoSurco='" + NoSurco + "' NoSeccion='" + obj.parent().attr('NoSeccion') + "' Invernadero='" + obj.parent().attr('Invernadero') + "' NoFila='" + NoFila + "' checked /></td>";
            html = html + "<td><span id='Surco" + NoSurco + "' idInv='" + obj.parent().attr('idInv') + "' NoSurco='" + NoSurco + "' NoSeccion='" + obj.parent().attr('NoSeccion') + "' Invernadero='" + obj.parent().attr('Invernadero') + "' NoFila='" + NoFila + "' ><img src='../comun/img/remove-icon.png' onclick='EliminarSurco($(this));' /></span></td>";
            html = html + "<td><span id='Surco" + NoSurco + "' idInv='" + obj.parent().attr('idInv') + "' NoSurco='" + NoSurco + "' NoSeccion='" + obj.parent().attr('NoSeccion') + "' Invernadero='" + obj.parent().attr('Invernadero') + "' NoFila='" + NoFila + "' ><img src='../comun/img/add-icon.png' onclick='AgregarSurco($(this));' /></span></td>";
            html = html + "</tr>";

            // Envio el renglon a la tabla:
            obj.parents('tr').after(html);
            numerate(obj);
        }

    </script>
    <style type="text/css">
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
        .habilidades
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
        .Deleted
        {
            display: none;
        }
        .Seccion
        {
            border: 1px solid black;    
        }
        .DivAcordion
		{
		    position: relative;
		    overflow: auto;
		    height: 500px;
		    text-align: center;
		}
		.DivAcordion ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active
		{
		    position: relative;
		    overflow: auto;
		    height: 500px;
		}
		#accordion h3.ui-accordion-header
        {
            display: block;
            text-align: left;
            padding-bottom: 3px;
            padding-top: 8px;
            background-color: #ADC995;
            color: white;
            font-size: 18px;
            border: 1px solid #E5EED2;
            width: 100%;
        }
        #accordionBody div.DivAcordion
        {
            border: 1px solid;
            border-color: #E5F1E5;
            padding-top 10px;
        }
        span.generadorDeSurcosYSecciones {
            background-color: #CDCDCD;
            padding-top: 10px;
            display: table;
            width: 100%;
        }
        span.generadorDeSurcosYSecciones input
        {
            float:none;
        }
        numeroDeSurco
        {
            display:inherit;    
        }
        h2
        {
            width:576px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" 
                text="Secciones y Surcos " meta:resourcekey="lblTitleResource1"></asp:Label></h1>
        <table class="index">
            <tr>
                <td>
                    <h2>Invernaderos</h2>
                </td>
                <td>
                    <input type="text" class="buscador" placeholder="Filtro"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="roller" class="habilidades">
                        <div id="rollslider2"> 
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    <br />
    <input type="button" value="Almacenar todo" onclick="guardarCambios(event);" />
    <br />
    <br />
    <div id="accordion" class="accordion">

    </div>
    <br />
    <div id="Resultado"></div>
    </div>
</asp:Content>
