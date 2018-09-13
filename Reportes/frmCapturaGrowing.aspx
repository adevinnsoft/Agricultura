<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  EnableEventValidation="false" AutoEventWireup="true" CodeFile="frmCapturaGrowing.aspx.cs" Inherits="Growing_frmCapturaGrowing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../comun/css/comun.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations_EN.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations_EN.js"></script>
    <script type="text/javascript" src="../comun/scripts/jsPopUp.js"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js" type="text/javascript"></script>
     <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" href="../comun/CSS/ui-lightness/jquery-ui-1.8.21.custom.css" />
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>

    <style type="text/css">
        div.propiedad select {
            float: none;
        }

        span.style2 {
            margin-left: 40px;
        }
        
        div[es*='<title>'] h3 {
                                color: BLACK !important;
                                text-transform: uppercase;
                                }

                                div[es*="<title>"] span{
                                    background: white !important;
                                }

        
        
        
        .grupo h2 {display: inline-block; width: auto;}
        .parametro input{margin-left:30px;}
        .propiedad {display: inline-block; padding-top: 4px; padding-bottom: 18px; min-width: 150px; vertical-align: top;}
        .propiedad label { text-align: center;}
        .propiedad label.titulo {margin-left: 50px;}
        .propiedad input{ margin-left:50px;}
        .parametro{padding-top: 2px;}
        .grupo{background: #F0F5E5;text-align: left;padding: 15px; /* display: table-cell; */border: 1px solid #D4CEAA;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 10px;}
        div#divFormulario{width: 100%;     max-width: 800px;}
        .accHeader{
            background-image: url('../comun/img/sort_asc.png');
            background-repeat: no-repeat;
            background-position-x: 99%;
            background-position-y: 7px;
            background-color: #ADC995;
            padding-top: 8px;
            padding-bottom: 3px;
            padding-left: 10px;
            color: white;
            cursor:pointer;
            -moz-border-radius: 3px; -webkit-border-radius: 3px;border-radius: 3px;
            width: 100%;
            display: inline-block;
               
        }
        .accHeader h2 
        {
            color:White; 
            min-width: 160px;
            max-width:160px;
            overflow:hidden;
        }
        table.general {
            border: 1px solid #adc995;
            background: #f0f5e5;
            font-size: 12px;
            margin-top: 0px;
            -moz-border-radius: 10px; -webkit-border-radius: 10px;border-radius: 10px;
            padding-bottom: 13px;
            padding-top: 13px;
            padding-left:3px;
            padding-right:3px;
        }
        #<%=txtObseraciones.ClientID%>{ width:80%;}
        
      
        .top{ vertical-align:top; border: 1px solid #D4CEAA; padding:10px;
              -moz-border-radius: 5px; -webkit-border-radius: 5px; border-radius: 5px; min-width: 250px;
              }
        #olHistorial { list-style-type:none; -webkit-padding-start: 0px; width: 100%; }
        #olHistorial li {
            border: 1px solid #ADC995;
            padding: 2px;
            margin: 2px;
            width: 96%;
            text-align: left;
            background: rgba(212, 206, 170, 0.25);
        }
        #olHistorial li:hover    
        {
            border: 1px solid #FF6600;
            padding: 2px;
            margin: 2px;
            width: 96%;
            text-align: left;
            background: #ADC995;
            color: #FFFFFF;
            font-weight: bold;
            cursor:pointer;
        }
        .span_parametro
        {
            background: #E7ECDC;
            width: 89%;
            padding: 6px;
            -moz-border-radius: 5px; -webkit-border-radius: 5px; border-radius: 5px;
            display: inline-block;
            margin-left: 26px;
           
        }
        .span_parametro h3 {
            display: inline-block;
            margin-left: 26px;
             min-width: 225px;
        }
        span.span_grupo.accHeader .ocultarMostrar {
            float: right;
            height: 25px;
            margin-right: 7px;
            margin-top: -5px;
            position: relative;
            opacity: 0.1;
            cursor: pointer;
        }
        .CajaCh
        {
            width:40px;    
        }
        input:read-only:not(.datePicker)
        {
            border: 1px solid gray;
            background: #CDCDCD;    
        }
        .multiopcion {
            background: url('../comun/img/lighter.png');
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            background-repeat: no-repeat;
            background-position-x: 41px;
            background-repeat-y: inherit;
            background-size: cover;
        }
        .smallButton
        {
            max-width: 46px;
            float: none !important;
            cursor: pointer;
            background: #82ab5f !important;
            color: White !important;
            box-shadow: none !important;
            border: none !important;
        }
        
        .smallButton:hover
        {
            color: Black !important;
        }
        
        .accHeader h2
        {
            margin-bottom: -5px;
            }
       
        </style>
    <script type="text/javascript">
        var spanish = '<%= Session["Locale"].ToString() %>' == 'es-MX' ? true : false;

        $('select#ddl_Status').chosen({ no_results_text: " ", width: '190px', placeholder_text_single: " " }).trigger('chosen:updated');

        $('select#ddl_Gerente').live('change', function () {
            var ddl = $(this);
            PageMethods.gerenteChange($(this).val(), function (response) {
                if (response.length > 0) {
                    $('select#ddl_lider').html('<option value>' + (spanish ? 'Seleccione' : 'Select') + '</option>' + response[0]);
                    $('#<%=ddlInvernadero.ClientID%>').html('<option value>' + (spanish ? 'Seleccione' : 'Select') + '</option>' + response[1]);
                    $('select#ddl_Grower').html('<option value>' + (spanish ? 'Seleccione' : 'Select') + '</option>' + response[2]);
                    $('#<%=ddlInvernadero.ClientID%>').trigger('chosen:updated');
                    $('select#ddl_lider').chosen({ no_results_text: " ", width: '190px', placeholder_text_single: " " }).trigger('chosen:updated');
                    $('select#ddl_Grower').chosen({ no_results_text: " ", width: '190px', placeholder_text_single: " " }).trigger('chosen:updated');
                 
                  
                } else {

                }
            });
        });

        $('select.ddl_filter').live('change', function () {
            Limpiar();
            obtenerHistorial();
        });

        $('select#ddl_lider').live('change', function () {
            obtenerInvernaderos($(this).val());
        });

        function ObtenerObjetoCaptura() {
            return captura = {
                idCaptura: parseInt($('#divCaptura').html()),
                Etiqueta: $('#<%=txtEtiqueta.ClientID %>').val(),
                Comentarios: $('#<%=txtObseraciones.ClientID %>').val(),
                FechaCaptura: $('#<%=txtFecha.ClientID %>').val(),
                idInvernadero: $('#<%=ddlInvernadero.ClientID %> option:selected').attr('idInvernadero'),
                CalificacionCalculada: ZeroIfNullOrEmpty($('.CalificacionTotalCalculada').first().val()),
                Calificacion: ZeroIfNullOrEmpty($('.CalificacionTotalUsuario').first().val()),
                Plantacion: $('#aplicaPlantacion').prop('checked') ? 1 : 0,
                grupo: $('.grupo:not(:hidden)').map(function () {
                    return {
                        idGrupo: $(this).attr('idGrupo'),
                        CalificacionCalculada: ZeroIfNullOrEmpty($(this).find('.CalificacionCalculada').first().val()),
                        Calificacion: ZeroIfNullOrEmpty($(this).find('.CalificacionUsuario').first().val())
                        //Cumplimiento: $(this).find('input[type="checkbox"]').first().prop('checked'),
                    }
                }).get(),
                parametro: $('.grupo:not(:hidden) .parametro').map(function () {
                    return {
                        idParametro: $(this).attr('idParametro'),
                        Calificacion: $(this).find('input[type="textbox"]').first().val(),
                        Cumplimiento: $(this).find('.span_parametro input:checked').val()
                    }
                }).get(),
                propiedad: $('.grupo:not(:hidden) .propiedad').map(function () {
                    return {
                        idPropiedad: $(this).attr('idpropiedad'),
                        //Calificacion: $(this).find('input[type="textbox"]').first().val(),
                        Cumplimiento: $(this).find('input[type="checkbox"]').first().prop('checked'),
                        OpcionSeleccionada: $(this).find('input[type="radio"]:checked').attr('idOpcion')
                    }
                }).get()
            }
        }

        function camposValidos() {
            var Errores = '';
            if ($('#<%=txtEtiqueta.ClientID %>').val() == '') {
                $('#<%=txtEtiqueta.ClientID %>').css({ 'border': '1px solid red' });
                Errores += 'El nombre para la captura es requerido';
            }
            else {
                $('#<%=txtFecha.ClientID %>').css({ 'border': '1px solid black' });
            }
            if ($('#<%=txtFecha.ClientID %>').val() == '') {
                $('#<%=txtFecha.ClientID %>').css({ 'border': '1px solid red' });
                Errores += 'El nombre para la captura es requerido';
            }
            else {
                $('#<%=txtFecha.ClientID %>').css({ 'border': '1px solid black' });
            }


            return Errores == '';
        }
        function Guardar() {
            if (camposValidos()) {
                PageMethods.guardarCaptura(ObtenerObjetoCaptura(), function (response) {
                    if (response[0] == 'ok') Limpiar();
                    popUpAlert(response[1], response[0]);
                });
                obtenerHistorial($('select[id*="ddlPlanta"] option:selected').val());
            }
            else {
                popUpAlert('Los campos marcados con rojo son requeridos.', 'error');
            }
        }
        function cargarCaptura(idCaptura) {
            $.blockUI();
            PageMethods.cargarDatosDeCaptura(idCaptura, function (response) {
                var Captura;
                $('div.container input, textarea,div.container select').not(':button, .ddl_filter').attr('disabled', false);
                if (response[0] == 'ok') {
                    Limpiar();

                    Captura = $.parseJSON(response[1]);

                    $('select#ddl_lider option[value="' + Captura.idLider + '"]').attr('selected', true);
                    $('select#ddl_Grower option[value="' + Captura.idGrower + '"]').attr('selected', true);
                    $('select#ddl_Gerente option[value="' + Captura.idGerente + '"]').attr('selected', true);
                    $('select#ddl_Status option[value="' + Captura.idStatus + '"]').attr('selected', true);
                    $('#<%=ddlInvernadero.ClientID%> option[value="' + Captura.idInvernadero + '"]').attr('selected', true);

                    $('select').trigger('chosen:updated');

                    if (Captura.Plantacion == 1) {
                        $('#aplicaPlantacion').click();
                    } else {
                        $('#aplicaNoPlantacion').click();
                    }

                    if (Captura.grupo != null)
                        for (var i = 0; i < Captura.grupo.length; i++) {
                            $('[idGrupo="' + Captura.grupo[i].idGrupo + '"] .CalificacionCalculada').val(Captura.grupo[i].CalificacionCalculada);
                            $('[idGrupo="' + Captura.grupo[i].idGrupo + '"] .CalificacionUsuario').val(Captura.grupo[i].Calificacion);
                        }
                    if (Captura.parametro != null)
                        for (var i = 0; i < Captura.parametro.length; i++) {
                            $('[idParametro="' + Captura.parametro[i].idParametro + '"] .span_parametro input[type="radio"][value="' + Captura.parametro[i].Cumplimiento + '"]').click();
                            $('[idParametro="' + Captura.parametro[i].idParametro + '"] input[type="text"]').first().val(Captura.parametro[i].Calificacion);
                        }
                    if (Captura.propiedad != null)
                        for (var i = 0; i < Captura.propiedad.length; i++) {
                            if (Captura.propiedad[i].OpcionSeleccionada > 0) {
                                var selector = '[idPropiedad="' + Captura.propiedad[i].idPropiedad + '"] input[type="radio"][id*="' + Captura.propiedad[i].OpcionSeleccionada + '"]';
                                $(selector).prop('checked', true);
                            }
                            else {
                                $('[idPropiedad="' + Captura.propiedad[i].idPropiedad + '"] input[type="checkbox"]').first().prop('checked', Captura.propiedad[i].Cumplimiento);
                                $('[idPropiedad="' + Captura.propiedad[i].idPropiedad + '"] input[type="text"]').first().val(Captura.propiedad[i].Calificacion);
                            }
                        }

                    $('div.container input, textarea,div.container select').not(':button, .ddl_filter').attr('disabled', true);
                }
                else {
                    popUpAlert(response[1], response[0]);
                }
                $('#divCaptura').html(Captura.idCaptura);
                $('#<%=txtEtiqueta.ClientID%>').val(Captura.Etiqueta);
                $('#<%=txtFecha.ClientID%>').val(Captura.FechaCaptura);
                $('#<%=ddlInvernadero.ClientID%> option[idInvernadero="' + Captura.idInvernadero + '"]').prop('selected', true);
                $('#<%=ddlInvernadero.ClientID%>').trigger('chosen:updated');
                $('#<%=txtObseraciones.ClientID%>').val(Captura.Comentarios);


                $('CalificacionTotalCalculada').val(Captura.CalificacionCalculada);
                $('CalificacionTotalUsuario').val(Captura.Calificacion);

                $('#btnActualizar').show();
                $('#btnCancelar').show();
                $('#btnGuardar').hide();
                $('#btnLimpiar').hide();

                $('input#ctl00_ContentPlaceHolder1_TextBox1').val(function () {
                    var cal = 0;
                    $('.grupo:visible input.CalificacionCalculada.CajaCh').each(function () {
                        cal += parseInt($(this).val()) > 0 ? parseInt($(this).val()) : 0;

                    });
                    return cal;
                });

                $.unblockUI();
                $('select').trigger('chosen:updated');
            });

        }

        function cancelarEdicion() {
            Limpiar();
        }

        //function pageLoad() {



        function obtenerInvernaderos(idLider) {
            if (idLider == undefined || idLider == '')
                idLider = 0;
            PageMethods.obtenerInvernaderos(idLider, function (response) {
                if (response[0] == 'ok') {
                    $('#<%=ddlInvernadero.ClientID %>').html('<option value>' + (spanish ? 'Seleccione ' : 'Select') +'</option>'+ response[1]);
                    $('#<%=ddlInvernadero.ClientID %>').chosen({ no_results_text: " ", width: '150px', placeholder_text_single: " " }).trigger('chosen:updated');

                }
                else {
                    popUpAlert(response[1], response[0]);
                    $('#<%=ddlInvernadero.ClientID %>').html('<option value>' + (spanish ? 'Seleccione Lider' : 'Select Leader') +'</option>');
                    $('#<%=ddlInvernadero.ClientID %>').chosen({ no_results_text: " ", width: '150px', placeholder_text_single: " " }).trigger('chosen:updated');
                }
            });
        }
        function obtenerHistorial() {
            $.blockUI();
            var idPlanta, idGerente, idLider, idGrower, idInvernadero;

            idPlanta = $('select#ctl00_ddlPlanta').val() == undefined ? '0':$('select#ctl00_ddlPlanta').val();
            idLider = $('select#ddl_lider').val() == undefined ? '0' : $('select#ddl_lider').val();
            idGerente = $('select#ddl_Gerente').val() == undefined ? '0' : $('select#ddl_Gerente').val();
            idGrower = $('select#ddl_Grower').val() == undefined ? '0':$('select#ddl_Grower').val();
            idInvernadero = $('#<%=ddlInvernadero.ClientID%>').val() == undefined ? '0':$('#<%=ddlInvernadero.ClientID%>').val();

            if (idPlanta == undefined || idPlanta == '')
                idPlanta = 0;
            try{
                PageMethods.obtenerCapturas(idPlanta, idLider, idGerente, idGrower,idInvernadero, function (response) {
                    if (response[0] == 'ok') {
                        $('#olHistorial').html(response[1]);
                    }
                    else {
                        $('#olHistorial').html(response[1]);
                        //popUpAlert(response[1], response[0]);
                    }
                    $.unblockUI();
                });
            } catch (Exce) {
                $.unblockUI();
            }

        }

        function ocultarPropiedades(radio) {
            radio.parent().parent().find('.propiedad').hide();
            radio.parent().parent().find('.propiedad input').prop('checked', false);
            CalcularCalificaciones();
        }

        function mostrarPropiedades(radio) {
            radio.parent().parent().find('.propiedad').show();
            CalcularCalificaciones();
        }

        //Manejo de cambio en las opciones, para el cálculo de la calificación
        function CalcularCalificacionOpcion(radio) {
            radio.parent().parent().find('.propiedad').show();
            //CalcularCalificacionesOpcion();
            CalcularCalificaciones();
        }
        //Fin de manejo de cambio en las opciones, para el cálculo de la calificación

        function modificarParametros(objeto, valor) {
            objeto.parent().parent().find('.span_parametro input[type="radio"][value="' + valor + '"]').click();
            CalcularCalificaciones();
        }
        function cargaddls() {
            var idplanta = $('select#ctl00_ddlPlanta').val() == undefined ? '0' : $('select#ctl00_ddlPlanta').val();
            PageMethods.obtieneGerentes(idplanta, function (response) {
                $('select#ddl_Gerente').html('<option value>' + (spanish ? 'Seleccione' : 'Select') + '</option>' + response);
                $('select').trigger('chosen:updated');

            });
            $('select#ddl_lider, select#ddl_Grower').html('<option value>' + (spanish ? 'Seleccione Gerente' : 'Select Manager') + '</option>');
            $('select#ddl_Gerente, select#ddl_lider, select#ddl_Grower, select#ddl_Status').chosen({ no_results_text: " ", width: '190px', placeholder_text_single: " " }).trigger('chosen:updated');
            $('#<%=ddlInvernadero.ClientID%>').chosen({ no_results_text: " ", width: '150px', placeholder_text_single: " " }).trigger('chosen:updated');
        }

        function obtenerFormulario() {
            cargaddls();

            var CalificacionCalulada = spanish ? "Calificación:" : "Qualification:";
            //var CalificacionUsuario = spanish ? "Calificación Final:" : " Final qualification:";
            var cont = 0;
            PageMethods.obtenerFormulario(function (response) {
                if (response[0] == 'ok') {
                    $('#divFormulario').html(response[1]);
                    $('.grupo').each(function () {
                        //                            var grupo = (spanish ? $(this).attr('es') : $(this).attr('en'));
                        //                            $(this).prepend('<label  class="accHeader"><h2>' + grupo + '</h2></label>');
                        //                            $(this).prepend('<input  type="checkbox" ></input>');

                        cont = cont + 1;
                        var grupo = (spanish ? $(this).attr('es') : $(this).attr('en'));
                        var GrupoHTML = '';
                        GrupoHTML += '<span class="span_grupo accHeader">';
                        GrupoHTML += '<h2>' + grupo + '</h2>';
                        //GrupoHTML += ' <input onclick="modificarParametros($(this),1);" class="smallButton" type="button" value="N/A" id="NA_' + grupo + '"  >';
                        //GrupoHTML += ' <input onclick="modificarParametros($(this),2);" class="smallButton" type="button" value="OK" id="OK_' + grupo + '"   >';
                        //GrupoHTML += ' <input onclick="modificarParametros($(this),3);" class="smallButton" type="button" value="X" id="X_' + grupo + '"     >';
                        GrupoHTML += ' <label>' + CalificacionCalulada + '</label><input type="text" class="CalificacionCalculada CajaCh" readonly>';
                        //GrupoHTML += ' <label>' + CalificacionUsuario + '</label><input type="text" class="CalificacionUsuario CajaCh intValidate" maxlength="3" onchange="CalcularCalificaciones();">';
                        GrupoHTML += ' <img src="../comun/img/semi_transparente.png" class="ocultarMostrar" />';
                        GrupoHTML += '</span>';

                        $(this).prepend(GrupoHTML);


                        $(this).find('.parametro').each(function () {

                            var parametro = (spanish ? $(this).attr('es') : $(this).attr('en'));
                            var parametroHTML = "";
                            parametroHTML += '<span class="span_parametro">';
                            //var parametro2=parametro;
                            if (parametro.indexOf('>') != -1) {
                                var parametro2 = parametro.split('>');
                                parametro = parametro2[1];
                                parametroHTML += '<h3>' + parametro + '</h3>';
                                parametroHTML += '</span>';
                            }
                            else {




                                parametroHTML += '<h3>' + parametro + '</h3>';
                                parametroHTML += ' <input onclick="ocultarPropiedades($(this));" name="' + grupo + '_' + parametro + '" type="radio" value="1" id="NA_' + grupo + '_' + parametro + '" checked><label for="NA_' + grupo + '_' + parametro + '">N/A</label>';
                                parametroHTML += ' <input onclick="ocultarPropiedades($(this));" name="' + grupo + '_' + parametro + '" type="radio" value="2" id="OK_' + grupo + '_' + parametro + '" ><label for="OK_' + grupo + '_' + parametro + '">OK</label>';
                                parametroHTML += ' <input onclick="mostrarPropiedades($(this));" name="' + grupo + '_' + parametro + '" type="radio" value="3" id="X_' + grupo + '_' + parametro + '" ><label for="X_' + grupo + '_' + parametro + '">X</label>';
                                parametroHTML += '</span>';
                            }
                            $(this).prepend(parametroHTML);
                            //$(this).prepend('<input id="' + parametro + '" type="checkbox" ></input>');



                            $(this).find('.propiedad').map(function () {
                                var propiedad = (spanish ? $(this).attr('es') : $(this).attr('en'));
                                var idPropiedad = $(this).attr('idpropiedad');

                                var propiedadHTML = '';
                                propiedadHTML += "";
                                if ($(this).find('.opcion').length > 0) {
                                    propiedadHTML += '<span class="span_propiedad multiopcion">';
                                    propiedadHTML += '<label class="titulo">' + propiedad + '</label>';
                                    propiedadHTML += '<form idpropiedad="' + idPropiedad + '" class="multiopcion">';

                                    $(this).find('.opcion').each(function () {
                                        var opcion = (spanish ? $(this).attr('es') : $(this).attr('en'));
                                        var idOpcion = $(this).attr('idOpcion');
                                        var idGenerico = opcion + idOpcion;
                                        var porcentaje = $(this).attr('porcentaje');
                                        propiedadHTML += '<div idopcion="' + idOpcion + '" es="' + $(this).attr('es') + '" en="' + $(this).attr('en') + '" class="opcion">';
                                        propiedadHTML += '  <input onclick="CalcularCalificacionOpcion($(this));" porcentaje="' + porcentaje + '" name="' + idPropiedad + '" id="' + idGenerico + '" type="radio" idOpcion="' + idOpcion + '" />';
                                        propiedadHTML += '  <label for="' + idGenerico + '">' + opcion + '</label>';
                                        propiedadHTML += '</div>';
                                    });
                                    propiedadHTML += '</form>';
                                }
                                else {

                                    var propiedadcomparar = $(this).attr('sel')
                                    if (propiedadcomparar == 1) {
                                        //alert('Hola'+ propiedadcomparar);
                                        propiedadHTML += '<span class="span_propiedad style2">';
                                        //propiedadHTML += '<input ID="' + (idPropiedad + propiedad) + '" type="checkbox" class="cajaCh "></input>';
                                        propiedadHTML += '<label for="' + (idPropiedad + propiedad) + '">' + propiedad + '</label>';
                                        propiedadHTML += '<select>';
                                        propiedadHTML += '<option value="0">1</option>';
                                        propiedadHTML += '<option value="1">2</option>';
                                        propiedadHTML += '<option value="2">3</option>';
                                        propiedadHTML += '<option value="3">4</option>';
                                        propiedadHTML += '<option value="4">5</option>';
                                        propiedadHTML += '<option value="5">6</option>';
                                        propiedadHTML += '<option value="6">7</option>';
                                        propiedadHTML += '<option value="7">8</option>';
                                        propiedadHTML += '<option value="8">9</option>';
                                        propiedadHTML += '<option value="9">10</option>';
                                        propiedadHTML += '<option value="10">11</option>';
                                        propiedadHTML += '<option value="11">12</option>';
                                        propiedadHTML += '</select>';
                                    }
                                    else {
                                        propiedadHTML += '<span class="span_propiedad">';
                                        propiedadHTML += '<input ID="' + (idPropiedad + propiedad) + '" type="checkbox" class="cajaCh "></input>';
                                        propiedadHTML += '<label for="' + (idPropiedad + propiedad) + '">' + propiedad + '</label>';
                                    }
                                }
                                propiedadHTML += '</span>';
                                $(this).append(propiedadHTML);
                            });
                        });
                    });

                    registrarFuncionalidadDelFormulario();
                    $('.propiedad').hide();
                    $('.accHeader').click();
                    $('.grupo').hide();
                    $('#aplicaPlantacion').click();
                    $('div.container input, textarea,div.container select').not(':button, .ddl_filter').attr('disabled', true);
                    $('select').trigger('chosen:updated');
                }
                else {
                    popUpAlert(response[1], response[0]);
                }
            });
        }

        function registrarFuncionalidadDelFormulario() {
            $('.accHeader .ocultarMostrar, h2').click(function () {
                $(this).parent().parent().find('.parametro').toggle(100);
                if ($(this).parent().css('background-image').indexOf('asc') >= 0) {
                    $(this).parent().css('background-image', 'url(\'../comun/img/sort_desc.png\')');
                    $(this).parent().css('background-position-y', '6px');
                }
                else {
                    $(this).parent().css('background-image', 'url(\'../comun/img/sort_asc.png\')');
                    $(this).parent().css('background-position-y', '13px');
                }
            });
            /*
            $('.grupo').each(function () {
                $(this).find('input[type="checkbox"]').first().click(function () {
                    if ($(this).prop('checked')) {
                        $(this).parent().find('.parametro').each(function(){
                            $(this).find('input[type="checkbox"]').first().prop('checked', true);
                        });
                    }
                    else {
                        $(this).parent().find('.parametro').each(function () {
                            $(this).find('input[type="checkbox"]').first().prop('checked', false);
                        });
                    }
                });
            });
            */
        }

        $(function () {
            $('#<%=txtObseraciones.ClientID %>').change(function () {
                    if ($(this).val().length > 500)
                        $(this).val($(this).val().substring(0, 500));
                });

                $('select[id*="ddlPlanta"]').change(function () {
                    var idPlanta = $(this).val();
                    Limpiar();
                    obtenerFormulario();
                });

                //obtenerInvernaderos($('select[id*="ddlPlanta"] option:selected').val());
                obtenerFormulario();
                //obtenerHistorial($('select[id*="ddlPlanta"] option:selected').val());

                $(".datePicker").datepicker('destroy');
                var FechaMinima = new Date();
                FechaMinima = FechaMinima -
                $(".datePicker").prop('readonly', true).datepicker(
                        {
                            dateFormat: "yy-mm-dd",
                            //buttonImage: "../comun/img/calendario.png",
                            //showOn: "both",
                            dayNames: spanish ? ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"] : ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
                            dayNamesMin: spanish ? ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"] : ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
                            dayNamesShort: spanish ? ["Dom", "Lun", "Mar", "Mier", "Jue", "Vie", "Sab"] : ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                            monthNames: spanish ? ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"] : ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
                            monthNamesShort: spanish ? ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"] : ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                            changeYear: false,
                            changeMonth: true,
                            minDate: '-15D',
                            maxDate: 0
                        }
                    );
                registerControls();

            });

                //}

                function CalcularCalificaciones() {
                    $('div.container input, textarea').attr('readonly', true);
                    //$('.grupo').each(function () {
                    //    var plantacion = $('input[name="Plantacion"]:checked').val() == $(this).attr('aplicaplantacion');
                    //    var ponderacion = plantacion ? parseInt($(this).attr('ponderacion')) : parseInt($(this).attr('ponderacionNP'));
                    //    var ParametrosActivos = $(this).find('.span_parametro input[type="radio"][value!="1"]:checked').length;
                    //    var ParametrosCorrectos = $(this).find('.span_parametro input[type="radio"][value="2"]:checked').length;
                    //    var calificacion = (ponderacion / ParametrosActivos) * ParametrosCorrectos;
                    //    //Calculo de las calificaciones incorporando las opciones
                    //    $(this).find('.span_parametro input[type="radio"][value="3"]:checked').each(function () {
                    //        var CalificacionGrupo = $(this).parent().parent().find('span input:radio[porcentaje]:checked').attr('porcentaje'); //$(this).attr('porcentaje');
                    //        if (undefined != CalificacionGrupo && (CalificacionGrupo != 0 || CalificacionGrupo != 0.00)) {
                    //            calificacion = calificacion + (CalificacionGrupo / 100);
                    //            //$('[idgrupo=3] div[idparametro=13] span input:radio[porcentaje]:checked').attr('porcentaje')
                    //        }
                    //    });
                    //    //Fin del Cálculo de las calificaciones incorporando las opciones


                    //    if (isNaN(calificacion)) {
                    //        $(this).find('.CalificacionCalculada').val('');
                    //        $(this).find('.CalificacionUsuario ').val('').prop('readonly', true);
                    //    }
                    //    else {
                    //        $(this).find('.CalificacionCalculada').val(calificacion.toFixed(0));
                    //        $(this).find('.CalificacionUsuario ').prop('readonly', false);
                    //        if ($(this).find('.CalificacionUsuario ').val() == '') {
                    //            $(this).find('.CalificacionUsuario ').val(calificacion.toFixed(0));
                    //        } else {
                    //            //Mantener el valor indicado por el usuario.
                    //        }
                    //    }
                    //});
                    //var CalificacionTotalCalculada = 0;
                    //$('.grupo:not(:hidden) .CalificacionCalculada').each(function () {
                    //    CalificacionTotalCalculada += isNaN(parseInt($(this).val())) ? 0 : parseInt($(this).val());
                    //});
                    //$('.CalificacionTotalCalculada').val(CalificacionTotalCalculada.toFixed(0));

                    //var CalificacionTotalUsuario = 0;
                    //$('.grupo:not(:hidden) .CalificacionUsuario').each(function () {
                    //    CalificacionTotalUsuario += isNaN(parseInt($(this).val())) ? 0 : parseInt($(this).val());
                    //});
                    //$('.CalificacionTotalUsuario').val(CalificacionTotalUsuario.toFixed(0));
                }

                function CalcularCalificacionesOpcion() {


                    //alert('Opcíon elegida');
                    //                $('.grupo').each(function () {
                    //                    var ponderacion = parseInt($(this).attr('ponderacion'));
                    //                    var ParametrosActivos = $(this).find('.span_parametro input[type="radio"][value!="1"]:checked').length;
                    //                    var ParametrosCorrectos = $(this).find('.span_parametro input[type="radio"][value="2"]:checked').length;
                    //                    var calificacion = (ponderacion / ParametrosActivos) * ParametrosCorrectos;
                    //                    if (isNaN(calificacion)) {
                    //                        $(this).find('.CalificacionCalculada').val('');
                    //                        $(this).find('.CalificacionUsuario ').val('').prop('readonly', true);
                    //                    }
                    //                    else {
                    //                        $(this).find('.CalificacionCalculada').val(calificacion.toFixed(0));
                    //                        $(this).find('.CalificacionUsuario ').prop('readonly', false);
                    //                        if ($(this).find('.CalificacionUsuario ').val() == '') {
                    //                            $(this).find('.CalificacionUsuario ').val(calificacion.toFixed(0));
                    //                        } else {
                    //                            //Mantener el valor indicado por el usuario.
                    //                        }
                    //                    }
                    //                });
                    //                var CalificacionTotalCalculada = 0;
                    //                $('.grupo:not(:hidden) .CalificacionCalculada').each(function () {
                    //                    CalificacionTotalCalculada += isNaN(parseInt($(this).val())) ? 0 : parseInt($(this).val());
                    //                });
                    //                $('.CalificacionTotalCalculada').val(CalificacionTotalCalculada.toFixed(0));

                    //                var CalificacionTotalUsuario = 0;
                    //                $('.grupo:not(:hidden) .CalificacionUsuario').each(function () {
                    //                    CalificacionTotalUsuario += isNaN(parseInt($(this).val())) ? 0 : parseInt($(this).val());
                    //                });
                    //                $('.CalificacionTotalUsuario').val(CalificacionTotalUsuario.toFixed(0));
                }


                function mostrarGruposSegunPlantacion(valor) {
                    switch (valor) {
                        case "1":
                            $('.grupo').hide();
                            $('.grupo[aplicaPlantacion="1"]').show();
                            break;
                        case "2":
                            $('.grupo').hide();
                            $('.grupo[aplicaNoPlantacion="1"]').show();
                            break;
                        default:
                            $('.grupo').show();
                            break;
                    }
                    CalcularCalificaciones();
                }

                function resetFormulario() {
                    $('#aplicaPlantacion').prop('checked', false);
                    $('#aplicaNoPlantacion').prop('checked', false);
                    $("#<%=txtEtiqueta %>").val('');
                $("#<%=txtFecha %>").val('');
                $("#<%=ddlInvernadero.ClientID%>").trigger("chosen:updated");
                $('#divFormulario input').prop('checked', false);
                $('#divFormulario input[type="text"]').val('');
                $('#divFormulario .span_grupo input[type="radio"][value="1"]').click();
            }
            function mostrarTodo() {
                $('.parametro:not(:visible)').toggle(100);
                if ($('.parametro:not(:visible)').parent().find('span.accHeader').css('background-image').indexOf('asc') >= 0) {
                    $('.parametro:not(:visible)').parent().find('span.accHeader').css('background-image', 'url(\'../comun/img/sort_desc.png\')');
                    $('.parametro:not(:visible)').parent().find('span.accHeader').css('background-position-y', '6px');
                }
                else {
                    $('.parametro:not(:visible)').parent().find('span.accHeader').css('background-image', 'url(\'../comun/img/sort_asc.png\')');
                    $('.parametro:not(:visible)').parent().find('span.accHeader').css('background-position-y', '13px');
                }
                $('input#ocultar').show();
                $('input#mostrar').hide();
            }
            function ocultarTodo() {
                $('.parametro:visible').toggle(100);
                if ($('.parametro:visible').parent().find('span.accHeader').css('background-image').indexOf('asc') >= 0) {
                    $('.parametro:visible').parent().find('span.accHeader').css('background-image', 'url(\'../comun/img/sort_desc.png\')');
                    $('.parametro:visible').parent().find('span.accHeader').css('background-position-y', '6px');
                }
                else {
                    $('.parametro:visible').parent().find('span.accHeader').css('background-image', 'url(\'../comun/img/sort_asc.png\')');
                    $('.parametro:visible').parent().find('span.accHeader').css('background-position-y', '13px');
                }
                $('input#ocultar').hide();
                $('input#mostrar').show();
            }

            function Limpiar() {
                resetFormulario();
                $('input:not([type="button"],[type="radio"])').val('').prop('checked', false);
                $('#divCaptura').html('0');
                $('textarea').val('');
                $('#btnActualizar').hide();
                $('#btnCancelar').hide();
                $('#btnGuardar').show();
                $('#btnLimpiar').show();
                $('input').css({ 'border': '1px solid black' });
                $('.grupo').hide();
                $('#aplicaPlantacion').click();
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitulo" runat="server" Text="Captura Growing"></asp:Label></h1>
        <div id="divCaptura" class="invisible">0</div>
        <table width="100%" class="general">
            <tr>
                <td><span>Gerente:</span></td>
                <td colspan="2">
                    <select class="ddl_filter" id="ddl_Gerente"></select>
                </td>
                <td><span>Grower:</span></td>
                <td colspan="2">
                    <select class="ddl_filter" id="ddl_Grower"></select>
                </td>

            </tr>
            <tr>

                <td><span>Líder:</span></td>
                <td colspan="2">
                    <select class="ddl_filter" id="ddl_lider"></select>
                </td>
                <td><span>Status:</span></td>
                <td>
                    <select id="ddl_Status">
                        <option value="0">GH. Visitado</option>
                        <option value="1">GH. Fuera</option>
                        <option value="2">GH. Aplicado</option>
                        <option value="3">GH. Inactivo</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Tipo de Captura:"></asp:Label>
                </td>
                <td colspan="5">
                    <input type="radio" name="Plantacion" id="aplicaPlantacion" value="1" onclick="mostrarGruposSegunPlantacion($(this).val());" /><label for="aplicaPlantacion">Plantación</label>
                    <input type="radio" name="Plantacion" id="aplicaNoPlantacion" value="2" onclick="mostrarGruposSegunPlantacion($(this).val());" /><label for="aplicaNoPlantacion">No Plantación</label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEtiqueta" runat="server" Text="Nombre para la captura:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtEtiqueta" runat="server" MaxLength="50"></asp:TextBox></td><td></td>
                <td>
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="datePicker"></asp:TextBox></td>
                <td>
                    <asp:Label ID="lblInvernadero" runat="server" Text="Invernadero:"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlInvernadero" runat="server" CssClass="ddl_filter"></asp:DropDownList></td>
                <td rowspan="5" class="top">
                    <h3>
                        <img src="../comun/img/left_triangle.png" class="invisible" /><img src="../comun/img/right_triangle.png" /><asp:Label runat="server" Text="Historial de Capturas"></asp:Label></h3>
                    <div style="height: 341px; overflow: auto;">
                        <ol id="olHistorial"></ol>
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <input id="ocultar" type="button" value="Ocultar Todo" class="" onclick="ocultarTodo();" />
                    <input id="mostrar" type="button" value="Mostrar Todo" class="invisible" onclick="mostrarTodo();" />
                </td>
            </tr>
            <tr>
                <td colspan="7" valign="top">
                    <div id="divFormulario"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Calificación Total Calculada:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="3" CssClass="intValidate CalificacionTotalCalculada CajaCh" ReadOnly></asp:TextBox></td>
                <%--<td><asp:Label ID="Label2" runat="server" Text="Calificación Total Final:"></asp:Label></td>
                <td><asp:TextBox ID="TextBox2" runat="server" MaxLength="3" CssClass="intValidate CalificacionTotalUsuario CajaCh" ></asp:TextBox></td>--%>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones:"></asp:Label></td>
                <td colspan="5">
                    <asp:TextBox ID="txtObseraciones" runat="server"
                        TextMode="MultiLine" MaxLength="500"></asp:TextBox></td>
            </tr>
            <%--  <tr>
                <td colspan="8">
                    <input ID="btnActualizar" type="button" value="Actualizar" class="invisible" onclick="Guardar();" />
                    <input ID="btnCancelar" type="button" value="Cancelar" class="invisible" onclick="cancelarEdicion();"/>
                    <input ID="btnGuardar" type="button" value="Guardar" onclick="Guardar();" />
                    <input ID="btnLimpiar" type="button" value="Limpiar" onclick="Limpiar();" />
                </td>
            </tr>--%>
        </table>
        <div></div>
    </div>
</asp:Content>

