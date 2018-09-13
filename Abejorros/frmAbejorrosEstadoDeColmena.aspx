<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmAbejorrosEstadoDeColmena.aspx.cs" Inherits="Abejorros_frmAbejorrosEstadoDeColmena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/scripts/jquery-1.7.2.js"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../comun/scripts/slider/slick.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations.js" ></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    
    <script type="text/javascript" id="escenarioInicial">
    
    function formularioColmena(idInvernadero,nombreInvernadero) {
        return '<div class="mantenimientoColmena" idInvernadero="' + idInvernadero + '" nombreInvernadero="' + nombreInvernadero + '">                                                       ' +
        '       <div class="accordionHeader" onclick="acordeon($(this));">                                                  ' +
        '           <label>'+nombreInvernadero+'</label>                                                                    ' +
        '           <img class="accordionMuestra" alt="" src="../comun/img/sort_desc.png" style="float:right;"/>            ' +
        '           <img class="accordionOculta" alt="" src="../comun/img/sort_asc.png" style="float:right; display:none;"/>' +
        '       </div>                                                                                                      ' +
        '       <div class="accordionBody">                                                                                 ' +
        '           <table class="gridView mantenimiento">                                                                                ' +
        '               <thead>                                                                                             ' +
        '                   <tr>                                                                                            ' +
        '                       <th><asp:Literal ID="ltSemana" meta:resourceKey="ltSemana" runat="server" /></th>                                                                             ' +
 //       '                       <th>Fecha Ingreso</th>                                                                      ' +
        '                       <th><asp:Literal ID="ltMantenimiento" meta:resourceKey="ltMantenimiento" runat="server" /></th>                                                                      ' +
        '                       <th><asp:Literal ID="ltPlaneadas" meta:resourceKey="ltPlaneadas" runat="server" /></th>                                                                 ' +
        '                       <th><asp:Literal ID="ltPolinizacion" meta:resourceKey="ltPolinizacion" runat="server" /></th>                                                         ' +
        '                       <th><asp:Literal ID="ltNivelPolinizacion" meta:resourceKey="ltNivelPolinizacion" runat="server" /></th>                                                              ' +
        '                       <th><asp:Literal ID="ltObservaciones" meta:resourceKey="ltObservaciones" runat="server" /></th>                                                         ' +
        '                   </tr>                                                                                           ' +
        '               </thead>                                                                                            ' +
        '               <tbody>                                                                                             ' +
        '               </tbody>                                                                                            ' +
        '           </table>                                                                                                ' +
        '       </div>                                                                                                      ' +
        '</div>';
    }
    $(function () {
      registerControls();
     
        $('.porcentajeDePolinizacion').change(function () {
            var val = $(this).val();
            if (val != '') {

            }
            else {
                $(this).addClass('');
            }
        });

        cargaInvernaderos();
    });
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
                            $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + invernaderoID + '] tbody').html(response);
                            $('#divMantenimientosColmenas .mantenimientoColmena[idInvernadero=' + invernaderoID + '] table').css('width','100%');
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
            cargaInvernaderos();
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
        .accordionHeader
        {
            display: inline-block;
            text-align: left;
            background-color: #ADC995;
            color: white;
            font-size: 18px;
            border: 1px solid #E5EED2;
            width: 100%;
            padding-top: 5px;
            padding-bottom: 5px;
            font-size: 15px;
        }
        .accordionHeader:hover
        {
            cursor: pointer;
            background-color: #FAE258;
            color: #FF6600;
        }
        .accordionBody
        {
            border: 1px solid;
            border-color: #E5F1E5;
            padding: 5px;
            width: 100%;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script type="text/javascript">
        $(function () {
            registerControls();
            });
            </script>
      <div class="container">

        <h1><asp:Literal ID="ltTitle" meta:resourceKey="ltTitle" runat="server" /></h1>


        <table class="index slidInvernaderos">
            <tr>
                <td>
                    <h2><asp:Literal ID="ltInvernaderos" meta:resourceKey="ltInvernaderos" runat="server" /></h2>
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
                    <input type="button" value="Guardar Semana" onclick="almacenarMantenimientos();" />
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

