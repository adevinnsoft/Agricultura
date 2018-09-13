<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="ConfiguracionCapturaGrowing.aspx.cs" Inherits="configuracion_ConfiguracionCapturaGrowing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery-ui-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations.js"></script>
    <script type="text/javascript" src="../comun/scripts/inputValidations_EN.js"></script>
    <script type="text/javascript" id="variablesGlobales">
        var elementoSeleccionado = null;
        var Grupos = null;
    </script>
    <script type="text/javascript" id="funcionamientoDePantalla">

        $(function () {
            $('#activoAgregarGrupo').attr('checked', true);
            $('.Plantacion').attr('checked', true);
            $('#activoAgregarParametro').attr('checked', true);
            $('#activoAgregarPropiedad').attr('checked', true);
            $('#activoAgregarOpcion').attr('checked', true);
            $('#CapturarCumplimientoAgregarPropiedad').attr('checked', true);
            $('#CapturarCumplimientoAgregarOpcion').attr('checked', true);
            obtenerConfiguracionGrowing();
            validarInputs($('#txtAgregarESGrupo'), $('#txtAgregarENGrupo'));
            validarInputsNumeroyTexto();

            //Funcion para cargar los catalogos de acuerdo a la planta en sesión
            $('#ctl00_ddlPlanta').change(function () {
                obtenerConfiguracionGrowing();
            })
        });

        function recargarPagina() {
            setTimeout(function () {
                location.reload();
            },1000);
        }


        function validarInputsNumeroyTexto() {
            $('#txtCapturaNumeroPropiedad').each(function () {
                if (/^[0-9]+$/.test($(this).val()) == false) {
                    popUpAlert('El valor debe ser numérico.', 'warning');
                } else {
                    //se continua con el proceso
                }
            });

            $('#txtCapturaTextoPropiedad').each(function () {
                if (/^[a-zA-Z]+$/.test($(this).val()) == false) {
                    popUpAlert('El valor debe ser un texto.', 'warning');
                } else {
                    //se continua con el proceso
                }
            });

            $('#txtCapturaNumeroOpcion').each(function () {
                if (/^[0-9]+$/.test($(this).val()) == false) {
                    popUpAlert('El valor debe ser numérico.','warning');
                } else {
                    //se continua con el proceso
                }
            });

            $('#txtCapturaTextoOpcion').each(function () {
                if (/^[a-zA-Z]+$/.test($(this).val()) == false) {
                    popUpAlert('El valor debe ser un texto.', 'warning');
                } else {
                    //se continua con el proceso
                }
            });

            //validamos campos de texto de calificacion
//            $('#txtCalificacionAgregar, #txtCalificacionEditar').change(function () {
//                var cal = parseInt($(this).val());
//                if (/^[0-9]+$/.test(cal) == false) {
//                    popUpAlert('El valor de calificación debe ser numérico.', 'warning');
//                } else {
//                    if (cal > 100 || cal < 1) {
//                        popUpAlert('Ingrese una valor de calificación dentro del rango de 1 a 100.', 'warning');
//                    }
//                }
//            });
        }




        function agruparPropiedadesGrupos(imgObj,imgNextOrPrev) {
            if ($(imgObj).parent().find('[otherclass="Parametro"]:hidden').length == 0) {
                $(imgObj).parent().find('[otherclass="Parametro"]').hide();
                $(imgObj).hide();
                $(imgNextOrPrev).show();
            } else {
                $(imgObj).parent().find('[otherclass="Parametro"]').show();
                $(imgObj).hide();
                $(imgNextOrPrev).show();
            }
        }


        function agruparParametrosGrupos(imgObj, imgNextOrPrev) {
            if ($(imgObj).parent().find('[otherclass="Propiedad"]:hidden').length == 0) {
                $(imgObj).parent().find('[otherclass="Propiedad"]').hide();
                $(imgObj).hide();
                $(imgNextOrPrev).show();
            } else {
                $(imgObj).parent().find('[otherclass="Propiedad"]').show();
                $(imgObj).hide();
                $(imgNextOrPrev).show();
            }
        }

        function validarInputs(objtxtES,objtxtEN) {
            var limite = 255
            objtxtES.attr('maxlength', '255');
            objtxtEN.attr('maxlength', '255');

            $(objtxtES).keypress(function () {
                if ($(objtxtES).val().length == limite) {
                    popUpAlert('El limite de caracteres es 255', 'warning');
                    $(objtxtES).unbind('keypress');
                } else {
                    $(objtxtES).val().length + 1;
                }
            });


            $(objtxtES).keydown(function (tecla) {
                if (tecla.keyCode == 8) {

                    if ($(objtxtES).val().length == 0) {
                        $(objtxtES).unbind('keydown');
                    } else {
                        $(objtxtES).val().length - 1;
                    }
                }
            });


            $(objtxtEN).keypress(function () {
                if ($(objtxtEN).val().length == limite) {
                    popUpAlert('El limite de caracteres es 255', 'warning');
                    $(objtxtEN).unbind('keypress');
                } else {
                    $(objtxtEN).val().length + 1;
                }
            });


            $(objtxtEN).keydown(function (tecla) {
                if (tecla.keyCode == 8) {

                    if ($(objtxtEN).val().length == 0) {
                        $(objtxtEN).unbind('keydown');
                    } else {
                        $(objtxtEN).val().length - 1;
                    }
                }
            });
        }

        function ordenarNuevoElemento(elemento, tipo) {
            var orden = 0;
            $(elemento).each(function () {
                //Actualizamos el orden de lso elementos
                orden = orden + 1;
                $(this).attr('orden', orden);

                //Asignamos el atributo id del elemento
                if ($(this).attr('id' + tipo) == 0 && $(this).attr('id') == tipo + 'Nuevo') {
                    var consecutivoNuevos = $(this + '[id' + tipo + '="0"]').length;
                    switch (tipo) {
                        case 'Grupo':
                            $(this).addClass('SortableGrupo');
                            var idElemento = tipo + 'Nuevo' + consecutivoNuevos;
                            break;
                        case 'Parametro':
                            $(this).addClass('SortableParametro');
                            var idElemento = tipo + 'Nuevo' + consecutivoNuevos + '-' + $(this).parent().attr('idGrupo');
                            break;
                        case 'Propiedad':
                            $(this).addClass('SortablePropiedad');
                            var idElemento = tipo + 'Nuevo' + consecutivoNuevos + '-' + $(this).parent().attr('idParametro');
                            break;
                        case 'Opcion':
                            $(this).addClass('SortableOpcion');
                            var idElemento = tipo + 'Nuevo' + consecutivoNuevos + '-' + $(this).parent().attr('idPropiedad');
                            break;
                    }
                    $(this).attr('id', idElemento);

                    //Agregamos las clases al elemeto agregado para la funcionalidad sortable
                    $(this).addClass('ui-sortable');

                    //Agregamos la funcionalidad .sortable al papa del elemento seleccionado siempre y cuando este sea el primer hijo
                    if ($(this).parent().find('.Sortable' + tipo).length == 1) {
                        var idPadre = $(this).parent().attr('id');
                        $("div#" + idPadre).sortable({
                            items: "div:.Sortable" + tipo,
                            update: function (event, ui) {
                                //Actualizamos el orden de los parametros
                                var pivote = 0;
                                $('div#' + idPadre + ' div.Sortable' + tipo).each(function () {
                                    pivote = pivote + 1;
                                    $(this).attr('orden', pivote);
                                    $(this).attr('estado', 'modificado');
                                    $(this).parent().attr('estado', 'modificado');
                                });
                            }
                        });
                    } else {
                        //Ya no es necesario agregar la funcionalidad
                    }

                } else {
                    //No hay accion
                }
            });
        }

        function crearGrupo() {
            var Errores  ='';
            var nombreGrupoES = $('#txtAgregarESGrupo').val().trim();
            var nombreGrupoEN = $('#txtAgregarENGrupo').val().trim();
            var activoGrupo = $('#activoAgregarGrupo').prop('checked') == true ?  1 :  0;
            var calificacion = $('#txtCalificacionAgregar').val().trim();
            var calificacionNP = $('#txtCalificacionNoPlantacionAgregar').val().trim();
            var aplicaPlantacion = $('#txtCalificacionAgregar').val() >= 1 && $('#txtCalificacionAgregar').val() != "" ? 1 : 0; //$('table[class="index"] tr[class="trNuevoGrupo"] td input.Plantacion').is(':checked') == true ? 1 : 0
            var aplicaNoPlantacion = $('#txtCalificacionNoPlantacionAgregar').val() >= 1 && $('#txtCalificacionNoPlantacionAgregar').val() != "" ? 1 : 0; //$('table[class="index"] tr[class="trNuevoGrupo"] td input.noPlantacion').is(':checked') == true ? 1 : 0

            if (nombreGrupoES == '') {
                Errores += 'El nombre del grupo en español es requerido.<br/>';
                $('#txtAgregarESGrupo').addClass('error');
            } else {
                    $('#txtAgregarESGrupo').removeClass('error');
            }

            if (nombreGrupoEN == '') {
                Errores += 'El nombre del grupo en inglés es requerido.<br/>';
                $('#txtAgregarENGrupo').addClass('error');
            } else {
                $('#txtAgregarENGrupo').removeClass('error');
            }

            //Valida que al menos haya una calificacion
            if ((calificacion != '' || calificacionNP != '') && (calificacion != 0 || calificacionNP != 0)) {
                //Valida que la calificación este dentro del rango siempre y cuando el valor sea distinto a vacio (para identificar cuando se requiere solo una)
                if (!isNaN(calificacion) && calificacion != '') {
                    if ((calificacion > 100 || calificacion < 1)) {
                        Errores += 'La calificación de plantación para el grupo debe ser entre 1 y 100. </br>';
                        $('#txtCalificacionAgregar').addClass('error');
                    } else {
                        $('#txtCalificacionAgregar').removeClass('error');
                    }
                } else {
                    $('#txtCalificacionAgregar').removeClass('error');
                }

                //Valida que la calificacionNP este dentro del rango siempre y cuando el valor sea distinto a vacio (para identificar cuando se requiere solo una)
                if (!isNaN(calificacionNP) && calificacionNP != '') {
                    if ((calificacionNP > 100 || calificacionNP < 1)) {
                        Errores += 'La calificación de no plantación para el grupo debe ser entre 1  y 100. </br>';
                        $('#txtCalificacionNoPlantacionAgregar').addClass('error');
                    } else {
                    $('#txtCalificacionNoPlantacionAgregar').removeClass('error');
                    }
                } else {
                    $('#txtCalificacionNoPlantacionAgregar').removeClass('error');
                }
            } else {
                Errores += 'Debe introducir al menos una Calificación entre 1 y 100. </br>';
                $('#txtCalificacionAgregar').addClass('error');
                $('#txtCalificacionNoPlantacionAgregar').addClass('error');
            }

            $('#divGeneral').find('[otherClass="Grupo"]').each(function () {
                if (nombreGrupoES == $(this).attr('nombrees')) {
                    Errores += 'Ya existe un grupo en español con el nombre:' + ' ' + '' + nombreGrupoES + '.<br/>';
                }
                else {
                    //Diferente
                }
                if (nombreGrupoEN == $(this).attr('nombreen')) {
                    Errores += 'Ya existe un grupo en inglés con el nombre:' + ' ' + '' + nombreGrupoEN + '.<br/>';
                } else {
                    //Diferente
                }
            });

            if (Errores != '') {
                popUpAlert(Errores, 'error');
            }
            else {
                var grupoHTML = '<div id="GrupoNuevo" idGrupo="0" nombreES="' + nombreGrupoES + '" nombreEN="' + nombreGrupoEN + '" activo="' + activoGrupo + '" nuevo="1" otherclass="Grupo" estado="nuevo" class="NuevoGrupo" Ponderacion="' + calificacion + '" aplicaPlantacion="' + aplicaPlantacion + '" aplicaNoPlantacion="' + aplicaNoPlantacion + '" PonderacionNP="' + calificacionNP + '" orden="1">'+
                                    '<img src="../comun/img/sort_desc.png" id="imgASC" onclick="agruparPropiedadesGrupos($(this),$(this).next());">' +
                                    '<img src="../comun/img/sort_asc.png" id="imgDESC" onclick="agruparPropiedadesGrupos($(this),$(this).prev());">' +
                                    '<label class="accHeaderGrupo" onclick="cargarEdicionGrupo($(this).parent());"><h2>' + nombreGrupoES + '</h2></label>' +
                                '</div>';
                $('#divGeneral').prepend(grupoHTML);
                ordenarNuevoElemento($('#divGeneral div[otherclass="Grupo"]'), "Grupo");
                borrarFormularioNuevoGrupo();
            }           
        }

        function borrarFormularioNuevoGrupo() {
            $('#txtAgregarESGrupo').val('');
            $('#txtAgregarENGrupo').val('');
            $('#txtCalificacionAgregar').val('');
            $('#txtCalificacionNoPlantacionAgregar').val('');
            $('#activoAgregarGrupo').prop('checked', true);
            $('#plantacionAgregar').prop('checked', true);
            $('#noPlantacionAgregar').prop('checked', false);
        }

        function cargarEdicionGrupo(objGrupo) {
            $('#activoEditarGrupo').removeAttr('chuleado');
            $('#txtEditarESGrupo').removeClass('error');
            $('#txtEditarENGrupo').removeClass('error');
            $('#txtCalificacionEditar').removeClass('error');
            $('#txtAgregarESParametro').removeClass('error');
            $('#txtAgregarENParametro').removeClass('error');
            $('.trNuevoGrupo').hide();
            $('.trEdicionParametro').hide();
            $('.trEdicionPropiedad').hide();
            $('.trEdicionOpcion').hide();
            $('.trEdicionGrupo').show();
            $('#txtEditarESGrupo').val(objGrupo.attr('nombreES'));
            $('#txtEditarENGrupo').val(objGrupo.attr('nombreEN'));
            $('#txtAgregarESParametro').val('');
            $('#txtAgregarENParametro').val('');
            objGrupo.attr('activo') == "1" ? $('#activoEditarGrupo').attr('checked', true) : $('#activoEditarGrupo').attr('checked', false);
            //objGrupo.attr('AplicaPlantacion') == "1" ? $('#plantacionModificar').attr('checked', true) : $('#plantacionModificar').attr('checked', false);
            //objGrupo.attr('AplicaNoPlantacion') == "1" ? $('#noPlantacionModificar').attr('checked', true) : $('#noPlantacionModificar').attr('checked', false);
            elementoSeleccionado = objGrupo;
            validarInputs($('#txtAgregarESParametro'), $('#txtAgregarENParametro'));
            validarInputs($('#txtEditarESGrupo'), $('#txtEditarENGrupo'));
            var ponderacion = elementoSeleccionado.attr('Ponderacion');
            var ponderacionNP = elementoSeleccionado.attr('PonderacionNP');
            $('#txtCalificacionEditar').val(ponderacion);
            $('#txtCalificacionNoPlantacionEditar').val(ponderacionNP);
            $('#activoEditarGrupo').click(function () {
                $(elementoSeleccionado).attr('activoinactivo','1');
            });
            $(elementoSeleccionado).attr('esteno','1');
            $('#txtEditarENGrupo').change(function () {
                var textoEs = $(this).val();
                var textoEn = $('#txtEditarESGrupo').val();
                $('#divGeneral').find('[otherClass="Grupo"]').each(function () {
                    if (textoEn == $(this).attr('nombreen')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                        //$(elementoSeleccionado).removeAttr('noesta', '1');
                    } 
                });
            });

            $('#txtEditarESGrupo').change(function () {
                var textoEs = $(this).val();
                $('#divGeneral').find('[otherClass="Grupo"]').each(function () {
                    if (textoEs == $(this).attr('nombrees')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } 
                });
            });
        
        }



        function modificarGrupo() {
            var Errores = '';
            var nombreModificadoGrupoES = $('#txtEditarESGrupo').val().trim();
            var nombreModificadoGrupoEN = $('#txtEditarENGrupo').val().trim();
            var Ponderacion = $('#txtCalificacionEditar').val().trim();
            var PonderacionNP = $('#txtCalificacionNoPlantacionEditar').val().trim();
            var aplicaPlantacion = $('#txtCalificacionEditar').val() >= 1 && $('#txtCalificacionEditar').val() != "" ? 1 : 0;  //$('#plantacionModificar').prop('checked') == true ? 1 : 0;
            var aplicaNoPlantacion = $('#txtCalificacionNoPlantacionEditar').val() >= 1 && $('#txtCalificacionNoPlantacionEditar').val() != "" ? 1 : 0;  //$('#noPlantacionModificar').prop('checked') == true ? 1 : 0;
            var activoModificadoGrupo = $('#activoEditarGrupo').prop('checked') == true ? 1 : 0;

            //LOS GRUPOS CARGADOS SE PUEDEN ACTIVAR O DESACTIVAR
            $('#divGeneral').find('[otherClass="Grupo"]').each(function () {

                if ($(this).attr('esteno') == '1') {
                    console.log('este no');
                } else {
                    if (nombreModificadoGrupoES == $(this).attr('nombrees') && $(elementoSeleccionado).attr('yaesta') == '1') {//&& !$('#activoEditarGrupo').attr('chuleado') == '1'
                        Errores += 'Ya existe un grupo en español con el nombre:' + ' ' + '' + $(this).attr('nombrees') + '.<br/>';
                    }
                    else {

                    }
                    if (nombreModificadoGrupoEN == $(this).attr('nombreen') && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'Ya existe un grupo en inglés con el nombre:' + ' ' + '' + $(this).attr('nombreen') + '.<br/>';
                    } else {

                    }
                }
            });


            if (nombreModificadoGrupoES == '') {
                Errores += 'El nombre del grupo en español es requerido.<br/>';
                $('#txtEditarESGrupo').addClass('error');
            } else {
                $('#txtEditarESGrupo').removeClass('error');
                
            }

            if (nombreModificadoGrupoEN == '') {
                Errores += 'El nombre del grupo en inglés es requerido.<br/>';
                $('#txtEditarENGrupo').addClass('error');
            } else {
                $('#txtEditarENGrupo').removeClass('error');
            }

            //Valida que al menos haya una calificacion
            if ((Ponderacion != '' || PonderacionNP != '') && (Ponderacion != 0 || PonderacionNP != 0)) {
                //Valida que la calificacion este dentro del rango siempre y cuando el valor sea distinto a vacio (para identificar cuando se requiere solo una)
                if (!isNaN(Ponderacion) && Ponderacion != '') {
                    if ((Ponderacion > 100 || Ponderacion < 1)) {
                        Errores += 'La calificación de plantación para el grupo debe ser entre 1 y 100.<br/>';
                        popUpAlert(Errores, 'error');
                        $('#txtCalificacionEditar').addClass('error');
                    } else {
                        $('#txtCalificacionEditar').removeClass('error');
                    }
                } else {
                    $('#txtCalificacionEditar').removeClass('error');
                }

                //Valida que la calificacionNP este dentro del rango siempre y cuando el valor sea distinto a vacio (para identificar cuando se requiere solo una)
                if (!isNaN(PonderacionNP) && PonderacionNP != '') {
                    if ((PonderacionNP > 100 || PonderacionNP < 1)) {
                        Errores += 'La calificación de no plantación para el grupo debe ser entre 1 y 100.<br/>';
                        popUpAlert(Errores, 'error');
                        $('#txtCalificacionNoPlantacionEditar').addClass('error');
                    } else {
                        $('#txtCalificacionNoPlantacionEditar').removeClass('error');
                    }
                } else {
                    $('#txtCalificacionNoPlantacionEditar').removeClass('error');
                }
            } else {
                Errores += 'Debe introducir al menos una Calificación entre 1 y 100. </br>';
            }

            if (Errores != '') {
                popUpAlert(Errores, 'error');
            } else {
                if ($(elementoSeleccionado) != null) {
                    if ($(elementoSeleccionado).hasClass('CargadoGrupo')) {
                        $(elementoSeleccionado).find('.accHeaderGrupo').find('h2').text(nombreModificadoGrupoES);
                        $(elementoSeleccionado).attr('nombreES', nombreModificadoGrupoES);
                        $(elementoSeleccionado).attr('nombreEN', nombreModificadoGrupoEN);
                        $(elementoSeleccionado).attr('Ponderacion', Ponderacion);
                        $(elementoSeleccionado).attr('ponderacionNP', PonderacionNP);
                        $(elementoSeleccionado).attr('AplicaPlantacion', aplicaPlantacion);
                        $(elementoSeleccionado).attr('AplicaNoPlantacion', aplicaNoPlantacion);
                        $(elementoSeleccionado).attr('activo', activoModificadoGrupo);
                        $(elementoSeleccionado).removeClass('CargadoGrupo');
                        $(elementoSeleccionado).addClass('ModificadoGrupo');
                        $(elementoSeleccionado).attr('estado', 'modificado');
                        $(elementoSeleccionado).attr('nuevo', '0');

                    } else {
                        if ($(elementoSeleccionado).hasClass('NuevoGrupo')) {
                            $(elementoSeleccionado).find('.accHeaderGrupo').find('h2').text(nombreModificadoGrupoES);
                            $(elementoSeleccionado).attr('nombreES', nombreModificadoGrupoES);
                            $(elementoSeleccionado).attr('nombreEN', nombreModificadoGrupoEN);
                            $(elementoSeleccionado).attr('Ponderacion', Ponderacion);
                            $(elementoSeleccionado).attr('ponderacionNP', PonderacionNP);
                            $(elementoSeleccionado).attr('AplicaPlantacion', aplicaPlantacion);
                            $(elementoSeleccionado).attr('AplicaNoPlantacion', aplicaNoPlantacion);
                            $(elementoSeleccionado).attr('activo', activoModificadoGrupo);
                            $(elementoSeleccionado).removeClass('NuevoGrupo');
                            $(elementoSeleccionado).addClass('ModificadoGrupo');
                            $(elementoSeleccionado).attr('estado', 'modificado');
                            $(elementoSeleccionado).attr('nuevo', '0');
                        } else {
                            if ($(elementoSeleccionado).hasClass('ModificadoGrupo')) {
                                $(elementoSeleccionado).find('.accHeaderGrupo').find('h2').text(nombreModificadoGrupoES);
                                $(elementoSeleccionado).attr('nombreES', nombreModificadoGrupoES);
                                $(elementoSeleccionado).attr('nombreEN', nombreModificadoGrupoEN);
                                $(elementoSeleccionado).attr('Ponderacion', Ponderacion);
                                $(elementoSeleccionado).attr('ponderacionNP', PonderacionNP);
                                $(elementoSeleccionado).attr('AplicaPlantacion', aplicaPlantacion);
                                $(elementoSeleccionado).attr('AplicaNoPlantacion', aplicaNoPlantacion);
                                $(elementoSeleccionado).attr('activo', activoModificadoGrupo);
                                $(elementoSeleccionado).removeClass('ModificadoGrupo');
                                $(elementoSeleccionado).addClass('ModificadoGrupo');
                                $(elementoSeleccionado).attr('estado', 'modificado');
                                $(elementoSeleccionado).attr('nuevo', '0');

                            }
                        }
                    }
                } else {
                    //
                }
//                $(elementoSeleccionado).attr('modificado', '1');
                $(elementoSeleccionado).removeAttr('activoinactivo', '1');
            }


        
        }


        function crearParametro() {
            var nombreParametroES = $('#txtAgregarESParametro').val().trim();
            var nombreParametroEN = $('#txtAgregarENParametro').val().trim();
            var activoParametro = $('#activoAgregarParametro').prop('checked') == true ? 1 : 0;
            var parametroHTML = ''
            var flag = false;

            var Errores = '';
            if(nombreParametroES == '' ){
                $('#txtAgregarESParametro').addClass('error');
                Errores+='El nombre del parámetro en español es requerido.<br/>';
            }
            else{
                $('#txtAgregarESParametro').removeClass('error');
            }
            if(nombreParametroEN == '' ){
                $('#txtAgregarENParametro').addClass('error');
                Errores += 'El nombre del parámetro en inglés es requerido.<br/>';
            }
            else{
                $('#txtAgregarENParametro').removeClass('error');
            }

            //Grupo Seleccionado
             if ($(elementoSeleccionado).find('[otherclass="Parametro"][nombrees="'+nombreParametroES+'"]').length > 0) {
                Errores+='El parámetro '+nombreParametroES+' en español que se intenta agregar ya existe dentro del grupo.<br/>';
             }else{
                //Correcto
             }
              if ($(elementoSeleccionado).find('[otherclass="Parametro"][nombreen="'+nombreParametroEN+'"]').length > 0) {
                  Errores += 'El parámetro ' + nombreParametroEN + ' en inglés que se intenta agregar ya existe dentro del grupo.<br/>';
             }else{
                //Correcto
             }

             if(Errores!=''){
                popUpAlert(Errores, 'error');
             }else{
                parametroHTML = '<div id="ParametroNuevo" idParametro="0" nombreES="' + nombreParametroES + '" nombreEN="' + nombreParametroEN + '" activo="' + activoParametro + '" parametro="' + nombreParametroES + '" nuevo="1" otherclass="Parametro" estado="nuevo" class="NuevoParametro"><img src="../comun/img/sort_desc.png" id="imgASCParametro" onclick="agruparParametrosGrupos($(this),$(this).next());"><img src="../comun/img/sort_asc.png" id="imgDESCParametro" onclick="agruparParametrosGrupos($(this),$(this).prev());">' +
                   '<input id="' + nombreParametroES + '" type="checkbox" />' +
                   '<label class="accHeaderParametro" onclick="cargarEdicionParametro($(this).parent());"><h2>' + nombreParametroES + '</h2></label>' +
                   '</div>';
                    if ($(elementoSeleccionado).find('div[otherclass="Parametro"]').length == 0) {
                        $(elementoSeleccionado).append(parametroHTML);
                    } else {
                        $(elementoSeleccionado).find('div[otherclass="Parametro"]').first().before(parametroHTML);
                    }

                    ordenarNuevoElemento($('div#' + elementoSeleccionado.attr('id') + ' div[otherclass="Parametro"]'), "Parametro");
                    $(elementoSeleccionado).attr('estado', 'modificado');
                    $(elementoSeleccionado).parent('estado','modificado');
                   $('div[nombrees="' + nombreParametroES + '"]').show();
                   $('#txtAgregarESParametro').val('');
                   $('#txtAgregarENParametro').val('');
                   $('#activoAgregarParametro').prop('checked', true);
            }
        }

        function cargarEdicionParametro(objParametro) {
            $('#activoEditarParametro').removeAttr('chuleado');
            $('#txtEditarENParametro').removeClass('error');
            $('#txtEditarESParametro').removeClass('error');
            $('#txtAgregarESPropiedad').removeClass('error');
            $('#txtAgregarENPropiedad').removeClass('error');
            $('.trNuevoGrupo').hide();
            $('.trEdicionGrupo').hide();
            $('.trEdicionPropiedad').hide();
            $('.trEdicionOpcion').hide();
            $('.trEdicionParametro').show();
            $('#txtEditarESParametro').val(objParametro.attr('nombreES'));
            $('#txtEditarENParametro').val(objParametro.attr('nombreEN'));
            $('#txtAgregarESPropiedad').val('');
            $('#txtAgregarENPropiedad').val('');
            $('#CapturarNumeroAgregarPropiedad').attr('checked', false);
            $('#CapturarTextoAgregarPropiedad').attr('checked', false);
            objParametro.attr('activo') == "1" ? $('#activoEditarParametro').attr('checked', true) : $('#activoEditarParametro').attr('checked', false);
            elementoSeleccionado = objParametro;
            validarInputs($('#txtAgregarESPropiedad'), $('#txtAgregarENPropiedad'));
            validarInputs($('#txtEditarESParametro'), $('#txtEditarENParametro'));
            $(elementoSeleccionado).attr('esteno', '1');
            $('#txtEditarENParametro').change(function () {
                var textoEn = $(this).val();
                $(elementoSeleccionado).parent().find('[otherClass="Parametro"]').each(function () {
                    if (textoEn == $(this).attr('nombreen')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } else {
                    }
                });
            });

            $('#txtEditarESParametro').change(function () {
                var textoEs = $(this).val();
                $(elementoSeleccionado).parent().find('[otherClass="Parametro"]').each(function () {
                    if (textoEs == $(this).attr('nombrees')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } else {
                    }
                });
            });
        }


        function modificarParametro() {
            var Errores = '';
            var nombreModificadoParametroES = $('#txtEditarESParametro').val().trim();
            var nombreModificadoParametroEN = $('#txtEditarENParametro').val().trim();
            var activoModificadoParametro = $('#activoEditarParametro').prop('checked') == true ? 1 : 0;


            $(elementoSeleccionado).parent().find('[otherclass="Parametro"]').each(function () {

                //Parametro Seleccionado
                if ($(this).attr('esteno') == '1') {

                } else {
                    if ($(this).attr('nombrees') == nombreModificadoParametroES && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'El parámetro ' + $(this).attr('nombrees') + ' en español que se intenta agregar ya existe dentro del grupo.<br/>';
                    } else {
                        //Correcto
                    }
                    if ($(this).attr('nombreen') == nombreModificadoParametroEN && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'El parámetro ' + $(this).attr('nombreen') + ' en inglés que se intenta agregar ya existe dentro del grupo.<br/>';
                    } else {
                        //Correcto
                    }
                }

            });
            

            if (nombreModificadoParametroES == '') {
                Errores += 'El nombre del parámetro en español es requerido.<br/>';
                $('#txtEditarESParametro').addClass('error');
            } else {
                $('#txtEditarESParametro').removeClass('error');

            }

            if (nombreModificadoParametroEN == '') {
                Errores += 'El nombre del parámetro en inglés es requerido.<br/>';
                $('#txtEditarENParametro').addClass('error');
            } else {
                $('#txtEditarENParametro').removeClass('error');
            }


            if (Errores != '') {
                popUpAlert(Errores,'error');
            } else {
                if (elementoSeleccionado != null) {
                    if (elementoSeleccionado.hasClass('CargadoParametro')) {
                        elementoSeleccionado.find('.accHeaderParametro').find('h2').text(nombreModificadoParametroES);
                        elementoSeleccionado.attr('nombreES', nombreModificadoParametroES);
                        elementoSeleccionado.attr('nombreEN', nombreModificadoParametroEN);
                        elementoSeleccionado.attr('activo', activoModificadoParametro);
                        elementoSeleccionado.removeClass('CargadoParametro');
                        elementoSeleccionado.addClass('ModificadoParametro');
                        $(elementoSeleccionado).attr('estado', 'modificado');
                        $(elementoSeleccionado).parent().attr('estado', 'modificado');

                    } else {
                        if (elementoSeleccionado.hasClass('NuevoParametro')) {
                            elementoSeleccionado.find('.accHeaderParametro').find('h2').text(nombreModificadoParametroES);
                            elementoSeleccionado.attr('nombreES', nombreModificadoParametroES);
                            elementoSeleccionado.attr('nombreEN', nombreModificadoParametroEN);
                            elementoSeleccionado.attr('activo', activoModificadoParametro);
                            elementoSeleccionado.removeClass('NuevoParametro');
                            elementoSeleccionado.addClass('ModificadoParametro');
                            $(elementoSeleccionado).attr('estado', 'modificado');
                            $(elementoSeleccionado).parent().attr('estado', 'modificado');
                        } else {
                            if (elementoSeleccionadohasClass('ModificadoParametro')) {
                                elementoSeleccionado.find('.accHeaderParametro').find('h2').text(nombreModificadoParametroES);
                                elementoSeleccionado.attr('nombreES', nombreModificadoParametroES);
                                elementoSeleccionado.attr('nombreEN', nombreModificadoParametroEN);
                                elementoSeleccionado.attr('activo', activoModificadoParametro);
                                elementoSeleccionado.removeClass('ModificadoParametro');
                                elementoSeleccionado.addClass('ModificadoParametro');
                                $(elementoSeleccionado).attr('estado', 'modificado');
                                $(elementoSeleccionado).parent().attr('estado', 'modificado');
                            }
                        }
                    }
                } else {
                    //
                }
            }
        }


        function crearPropiedad() {
            var Errores = '';
            var nombrePropiedadES = $('#txtAgregarESPropiedad').val().trim();
            var nombrePropiedadEN = $('#txtAgregarENPropiedad').val().trim();

            var activoPropiedad =       $('#activoAgregarPropiedad').prop('checked') == true ? 1 : 0;
            var capturaNumero =         $('#CapturarNumeroAgregarPropiedad').prop('checked') == true ? 1 : 0;
            var capturaTexto =          $('#CapturarTextoAgregarPropiedad').prop('checked') == true ?  1 : 0;
            var capturaCumplimiento =   $('#CapturarCumplimientoAgregarPropiedad').prop('checked') == true ? 1 : 0;

            var txtCapturaNumero;
            var txtCapturaTexto;
            var propiedadHTML = '';
         
            $('#CapturarNumeroAgregarPropiedad').prop('checked') == true ? txtCapturaNumero = '<input type="text" id="txtCapturaNumeroPropiedad" placeholder="0.0"/>' : txtCapturaNumero = '';
            $('#CapturarTextoAgregarPropiedad').prop('checked') == true ? txtCapturaTexto = '<input type="text" id="txtCapturaTextoPropiedad" placeholder="Escriba algún texto"/>' : txtCapturaTexto = '';

            if (nombrePropiedadES == '') {
                $('#txtAgregarESPropiedad').addClass('error');
                Errores += 'El nombre de la propiedad en español es requerido.<br/>';
            }
            else {
                $('#txtAgregarESPropiedad').removeClass('error');
            }
            if (nombrePropiedadEN == '') {
                $('#txtAgregarENPropiedad').addClass('error');
                Errores += 'El nombre de la propiedad en inglés es requerido.<br/>';
            }
            else {
                $('#txtAgregarENPropiedad').removeClass('error');
            }

            //Grupo Seleccionado
            if ($(elementoSeleccionado).find('[otherclass="Propiedad"][nombrees="' + nombrePropiedadES + '"]').length > 0) {
                Errores += 'La propiedad ' + nombrePropiedadES + ' en español que se intenta agregar ya existe dentro del parámetro.<br/>';
            } else {
                //Correcto
            }
            if ($(elementoSeleccionado).find('[otherclass="Propiedad"][nombreen="' + nombrePropiedadEN + '"]').length > 0) {
                Errores += 'La propiedad ' + nombrePropiedadEN + ' en inglés que se intenta agregar ya existe dentro del parámetro.<br/>';
            } else {
                //Correcto
            }

            if (Errores != '') {
                popUpAlert(Errores, 'error');
            } else {


                propiedadHTML = '<div id="PropiedadNuevo" idPropiedad="0" nombreES="' + nombrePropiedadES + '" nombreEN="' + nombrePropiedadEN + '" activo="' + activoPropiedad + '" propiedad="' + nombrePropiedadES + '" tieneCapturaNumero="' + capturaNumero + '" tieneCapturaTexto="' + capturaTexto + '" tieneCapturaCumplimiento="' + capturaCumplimiento + '" nuevo="1" otherclass="Propiedad" estado="nuevo" class="NuevoPropiedad">' +
                                    '<input id="' + nombrePropiedadES + '" type="checkbox" />' +
                                    '<label class="accHeaderPropiedad" id="' + nombrePropiedadES + '" onclick="cargarEdicionPropiedad($(this).parent());">' + nombrePropiedadES + '</label>' +
                                    '</div>';
                if ($(elementoSeleccionado).find('div[otherclass="Propiedad"]').length == 0) {
                    $(elementoSeleccionado).append(propiedadHTML);
                } else {
                    $(elementoSeleccionado).find('div[otherclass="Propiedad"]').first().before(propiedadHTML);
                }

                ordenarNuevoElemento($('div#' + elementoSeleccionado.attr('id') + ' div[otherclass="Propiedad"]'), "Propiedad");
                $(elementoSeleccionado).attr('estado', 'modificado');
                $(elementoSeleccionado).parent().attr('estado', 'modificado');
                $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                $('div[nombrees="' + nombrePropiedadES + '"]').show();
                $('label[id="' + nombrePropiedadES + '"]').append(txtCapturaNumero);
                $('label[id="' + nombrePropiedadES + '"]').append(txtCapturaTexto);
                
                txtCapturaNumero = '';
                txtCapturaTexto = '';
                $('#txtAgregarESPropiedad').val('');
                $('#txtAgregarENPropiedad').val('');
                $('#activoAgregarPropiedad').prop('checked', true);
                $('#CapturarNumeroAgregarPropiedad').prop('checked', false);
                $('#CapturarTextoAgregarPropiedad').prop('checked', false);
                $('#CapturarCumplimientoAgregarPropiedad').prop('checked', true);
            }
        }


        function cargarEdicionPropiedad(objPropiedad) {
            $('#activoEditarPropiedad').removeAttr('chuleado');
            $('#txtPropiedadESEditar').removeClass('error');
            $('#txtPropiedadENEditar').removeClass('error');
            $('#txtAgregarESOpcion').removeClass('error');
            $('#txtAgregarENOpcion').removeClass('error');
            $('#txtOpcionCalificacionAgregar').removeClass('error');
            $('.trNuevoGrupo').hide();
            $('.trEdicionParametro').hide();
            $('.trEdicionGrupo').hide();
            $('.trEdicionOpcion').hide();
            $('.trEdicionPropiedad').show();
            $('#txtPropiedadESEditar').val(objPropiedad.attr('nombreES'));
            $('#txtPropiedadENEditar').val(objPropiedad.attr('nombreEN'));
            $('#txtAgregarESOpcion').val('');
            $('#txtAgregarENOpcion').val('');
            $('#txtOpcionCalificacionAgregar').val('');
            $('#CapturarNumeroAgregarOpcion').attr('checked', false);
            $('#CapturarTextoAgregarOpcion').attr('checked', false);
            objPropiedad.attr('tieneCapturaNumero') == "1" ? $('#CapturarNumeroEditarPropiedad').attr('checked', true) : $('#CapturarNumeroEditarPropiedad').attr('checked', false);
            objPropiedad.attr('tieneCapturaTexto') == "1" ? $('#CapturarTextoEditarPropiedad').attr('checked', true) : $('#CapturarTextoEditarPropiedad').attr('checked', false);
            objPropiedad.attr('tieneCapturaCumplimiento') == "1" ? $('#CapturarCumplimientoEditarPropiedad').attr('checked', true) : $('#CapturarCumplimientoEditarPropiedad').attr('checked', false);
            objPropiedad.attr('activo') == "1" ? $('#activoEditarPropiedad').attr('checked', true) : $('#activoEditarPropiedad').attr('checked', false);
            elementoSeleccionado = objPropiedad;
            validarInputs($('#txtAgregarESOpcion'), $('#txtAgregarENOpcion'));
            validarInputs($('#txtPropiedadESEditar'), $('#txtPropiedadENEditar'));
            $(elementoSeleccionado).attr('esteno', '1');
            $('#txtPropiedadENEditar').change(function () {
                var textoEn = $(this).val();
                $(elementoSeleccionado).parent().find('[otherClass="Propiedad"]').each(function () {
                    if (textoEn == $(this).attr('nombreen')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } else {
                    }
                });
            });

            $('#txtPropiedadESEditar').change(function () {
                var textoEs = $(this).val();
                $(elementoSeleccionado).parent().find('[otherClass="Propiedad"]').each(function () {
                    if (textoEs == $(this).attr('nombrees')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } else {
                    }
                });
            });

        }


        function modificarPropiedad() {
            var Errores = '';
            var nombreModificadoPropiedadES = $('#txtPropiedadESEditar').val().trim();
            var nombreModificadoPropiedadEN = $('#txtPropiedadENEditar').val().trim();
            var activoModificadoPropiedad = $('#activoEditarPropiedad').prop('checked') == true ? 1 : 0;
            var capturaNumeroModificado = $('#CapturarNumeroEditarPropiedad').prop('checked') == true ? 1 :  0;
            var capturaTextoModificado = $('#CapturarTextoEditarPropiedad').prop('checked') == true ?  1 : 0;
            var capturaCumplimientoModificado = $('#CapturarCumplimientoEditarPropiedad').prop('checked') == true ? 1 : 0;


            $(elementoSeleccionado).parent().find('[otherclass="Propiedad"]').each(function () {
                //Propiedad  Seleccionado
                if ($(this).attr('esteno') == '1') {

                } else {
                    if ($(this).attr('nombrees') == nombreModificadoPropiedadES && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'La propiedad ' + $(this).attr('nombrees') + ' en español que se intenta agregar ya existe dentro del parámetro.<br/>';
                    } else {
                        //Correcto
                    }
                    if ($(this).attr('nombrees') == nombreModificadoPropiedadEN && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'La propiedad ' + $(this).attr('nombrees') + ' en inglés que se intenta agregar ya existe dentro del parámetro.<br/>';
                    } else {
                        //Correcto
                    }
                }

            });

            if (nombreModificadoPropiedadES == '') {
                Errores += 'El nombre de la propiedad en español es requerido.<br/>';
                $('#txtPropiedadESEditar').addClass('error');
            } else {
                $('#txtPropiedadESEditar').removeClass('error');

            }

            if (nombreModificadoPropiedadEN == '') {
                Errores += 'El nombre de la propiedad en inglés es requerido.<br/>';
                $('#txtPropiedadENEditar').addClass('error');
            } else {
                $('#txtPropiedadENEditar').removeClass('error');
            }


            if (Errores != '') {
                popUpAlert(Errores,'error');
            } else {
                if (elementoSeleccionado != null) {
                    if ($(elementoSeleccionado).hasClass('CargadoPropiedad')) {
                        $(elementoSeleccionado).find('.accHeaderPropiedad').text(nombreModificadoPropiedadES);
                        $(elementoSeleccionado).attr('nombreES', nombreModificadoPropiedadES);
                        $(elementoSeleccionado).attr('nombreEN', nombreModificadoPropiedadEN);
                        $(elementoSeleccionado).attr('activo', activoModificadoPropiedad);
                        $(elementoSeleccionado).attr('tieneCapturaNumero', capturaNumeroModificado);
                        $(elementoSeleccionado).attr('tieneCapturaTexto', capturaTextoModificado);
                        $(elementoSeleccionado).attr('tieneCapturaCumplimiento', capturaCumplimientoModificado);
                        $(elementoSeleccionado).removeClass('CargadoPropiedad');
                        $(elementoSeleccionado).addClass('ModificadoPropiedad');
                        $(elementoSeleccionado).attr('estado', 'modificado');
                        $(elementoSeleccionado).parent().attr('estado', 'modificado');
                        $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                        if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().length == 0) {
                            $(elementoSeleccionado).find('.accHeaderPropiedad').after('<input type="text" id="txtCapturaTextoPropiedad" placeholder="Escriba algún texto"/>')
                        } else {
                            if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().length > 0) {

                            } else {
                                $(elementoSeleccionado).find('#txtCapturaTextoPropiedad').remove();
                            }
                        }
                        if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().next().length == 0) {
                            $(elementoSeleccionado).find('.accHeaderPropiedad').after('<input type="text" id="txtCapturaNumeroPropiedad" placeholder="0.0"/>')
                        } else {
                            if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().next().length > 0) {

                            } else {
                                $(elementoSeleccionado).find('#txtCapturaNumeroPropiedad').remove();
                            }
                        }

                        activoModificadoPropiedad == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().removeAttr('disabled');
                        activoModificadoPropiedad == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().next().removeAttr('disabled');

                    } else {
                        if ($(elementoSeleccionado).hasClass('NuevoPropiedad')) {
                            $(elementoSeleccionado).find('.accHeaderPropiedad').text(nombreModificadoPropiedadES);
                            $(elementoSeleccionado).attr('nombreES', nombreModificadoPropiedadES);
                            $(elementoSeleccionado).attr('nombreEN', nombreModificadoPropiedadEN);
                            $(elementoSeleccionado).attr('activo', activoModificadoPropiedad);
                            $(elementoSeleccionado).attr('tieneCapturaNumero', capturaNumeroModificado);
                            $(elementoSeleccionado).attr('tieneCapturaTexto', capturaTextoModificado);
                            $(elementoSeleccionado).attr('tieneCapturaCumplimiento', capturaCumplimientoModificado);
                            $(elementoSeleccionado).removeClass('NuevoPropiedad');
                            $(elementoSeleccionado).addClass('ModificadoPropiedad');
                            $(elementoSeleccionado).attr('estado', 'modificado');
                            $(elementoSeleccionado).parent().attr('estado', 'modificado');
                            $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                            if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().length == 0) {
                                $(elementoSeleccionado).find('.accHeaderPropiedad').after('<input type="text" id="txtCapturaTextoPropiedad" placeholder="Escriba algún texto"/>')
                            } else {
                                if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().length > 0) {

                                } else {
                                    $(elementoSeleccionado).find('#txtCapturaTextoPropiedad').remove();
                                }
                            }
                            if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().next().length == 0) {
                                $(elementoSeleccionado).find('.accHeaderPropiedad').after('<input type="text" id="txtCapturaNumeroPropiedad" placeholder="0.0"/>')
                            } else {
                                if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().next().length > 0) {

                                } else {
                                    $(elementoSeleccionado).find('#txtCapturaNumeroPropiedad').remove();
                                }
                            }
                            activoModificadoPropiedad == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().removeAttr('disabled');
                            activoModificadoPropiedad == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().next().removeAttr('disabled');
                        } else {
                            if ($(elementoSeleccionado).hasClass('ModificadoPropiedad')) {
                                $(elementoSeleccionado).find('.accHeaderPropiedad').text(nombreModificadoPropiedadES);
                                $(elementoSeleccionado).attr('nombreES', nombreModificadoPropiedadES);
                                $(elementoSeleccionado).attr('nombreEN', nombreModificadoPropiedadEN);
                                $(elementoSeleccionado).attr('activo', activoModificadoPropiedad);
                                $(elementoSeleccionado).attr('tieneCapturaNumero', capturaNumeroModificado);
                                $(elementoSeleccionado).attr('tieneCapturaTexto', capturaTextoModificado);
                                $(elementoSeleccionado).attr('tieneCapturaCumplimiento', capturaCumplimientoModificado);
                                $(elementoSeleccionado).removeClass('ModificadoPropiedad');
                                $(elementoSeleccionado).addClass('ModificadoPropiedad');
                                $(elementoSeleccionado).attr('estado', 'modificado');
                                $(elementoSeleccionado).parent().attr('estado', 'modificado');
                                $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                                if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().length == 0) {
                                    $(elementoSeleccionado).find('.accHeaderPropiedad').after('<input type="text" id="txtCapturaTextoPropiedad" placeholder="Escriba algún texto"/>')
                                } else {
                                    if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().length > 0) {

                                    } else {
                                        $(elementoSeleccionado).find('#txtCapturaTextoPropiedad').remove();
                                    }
                                }
                                if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().next().length == 0) {
                                    $(elementoSeleccionado).find('.accHeaderPropiedad').after('<input type="text" id="txtCapturaNumeroPropiedad" placeholder="0.0"/>')
                                } else {
                                    if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderPropiedad').next().next().length > 0) {

                                    } else {
                                        $(elementoSeleccionado).find('#txtCapturaNumeroPropiedad').remove();
                                    }
                                }
                                activoModificadoPropiedad == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().removeAttr('disabled');
                                activoModificadoPropiedad == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoPropiedadES + '"]').next().next().removeAttr('disabled');
                            }

                        }
                    }

                } else {
                    //
                }

            }
        }


        function crearOpcion() {
            var Errores = '';
            var nombreOpcionES = $('#txtAgregarESOpcion').val().trim();
            var nombreOpcionEN = $('#txtAgregarENOpcion').val().trim();
            var activoOpcion =   $('#activoAgregarOpcion').prop('checked') == true ? 1 : 0;
            var capturaNumero = $('#CapturarNumeroAgregarOpcion').prop('checked') == true ? 1 : 0;
            var capturaTexto = $('#CapturarTextoAgregarOpcion').prop('checked') == true ? 1 : 0;
            var capturaCumplimiento = $('#CapturarCumplimientoAgregarOpcion').prop('checked') == true ? 1 : 0;
            var calificacionOpcion = $('#txtOpcionCalificacionAgregar').val().trim();
            var sag = $('#chkSAGAgregar').prop('checked') == true ? 1 : 0;
            var txtCapturaNumero;
            var txtCapturaTexto;
            var opcionHTML = '';


            if (nombreOpcionES == '') {
                $('#txtAgregarESOpcion').addClass('error');
                Errores += 'El nombre de la opción en español es requerido.<br/>';
            }
            else {
                $('#txtAgregarESOpcion').removeClass('error');
            }
            if (nombreOpcionEN == '') {
                $('#txtAgregarENOpcion').addClass('error');
                Errores += 'El nombre de la opción en inglés es requerido.<br/>';
            }
            else {
                $('#txtAgregarENOpcion').removeClass('error');
            }

            //Opcion Seleccionado
            if ($(elementoSeleccionado).find('[otherclass="Opcion"][nombrees="' + nombreOpcionES + '"]').length > 0) {
                Errores += 'La opción' + nombreOpcionES + ' en español que se intenta agregar ya existe dentro de la propiedad.<br/>';
            } else {
                //Correcto
            }
            if ($(elementoSeleccionado).find('[otherclass="Opcion"][nombreen="' + nombreOpcionEN + '"]').length > 0) {
                Errores += 'La opción' + nombreOpcionEN + ' en inglés que se intenta agregar ya existe dentro de la propiedad.<br/>';
            } else {
                //Correcto
            }

            //Validamos que la calificacion este dentro del rango 0-100
            if (calificacionOpcion && calificacionOpcion.length > 0) {
                if (calificacionOpcion >= 0 && calificacionOpcion <= 100) {
                    $('#txtOpcionCalificacionAgregar').removeClass('error');
                } else {
                    Errores += 'La calificación debe ser entre 0 - 100';
                    $('#txtOpcionCalificacionAgregar').addClass('error');
                }
            } else {
            $('#txtOpcionCalificacionAgregar').removeClass('error');
            }

            if (Errores != '') {
                popUpAlert(Errores, 'error');
            } else {

                opcionHTML = '<div id="OpcionNuevo" idOpcion="0" nombreES="' + nombreOpcionES + '" nombreEN="' + nombreOpcionEN + '" activo="' + activoOpcion + '" opcion="' + nombreOpcionES + '" calificacionOpcion="' + calificacionOpcion + '" tieneCapturaNumero="' + capturaNumero + '" tieneCapturaTexto="' + capturaTexto + '" tieneCapturaCumplimiento="' + capturaCumplimiento + '" nuevo="1" otherclass="Opcion" estado="nuevo" class="NuevoOpcion" sag="' + sag + '">' +
                                    '<input id="' + nombreOpcionES + '" type="radio" />' +
                                    '<label class="accHeaderOpcion" id="' + nombreOpcionES + '" onclick="cargarEdicionOpcion($(this).parent());">' + nombreOpcionES + '</label>' +
                                    '</div>';

                if ($(elementoSeleccionado).first().find('div[otherclass="Opcion"]').length==0) {
                    $(elementoSeleccionado).append(opcionHTML);
                }else{
                    $(elementoSeleccionado).first().find('div[otherclass="Opcion"]').first().before(opcionHTML);
                }

                ordenarNuevoElemento($('div#' + elementoSeleccionado.attr('id') + ' div[otherclass="Opcion"]'), "Opcion");
                $(elementoSeleccionado).attr('estado', 'modificado');
                $(elementoSeleccionado).parent().attr('estado', 'modificado');
                $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                $(elementoSeleccionado).parent().parent().parent().attr('estado', 'modificado');
                $('div[nombrees="' + nombreOpcionES + '"]').show();
                $('label[id="' + nombreOpcionES + '"]').append(txtCapturaNumero);
                $('label[id="' + nombreOpcionES + '"]').append(txtCapturaTexto);

                $('#txtAgregarESOpcion').val('');
                $('#txtAgregarENOpcion').val('');
                $('#txtOpcionCalificacionAgregar').val('');
                $('#activoAgregarOpcion').prop('checked', true);
                $('#chkSAGAgregar').prop('checked', false);
            }

        }

        function cargarEdicionOpcion(objOpcion) {
            $('#actvoEditarOpcion').removeAttr('chuleado');
            $('#txtEditarESOpcion').removeClass('error');
            $('#txtEditarENOpcion').removeClass('error');
            $('#txtOpcionCalificacionEditar').removeClass('error');
            $('.trNuevoGrupo').hide();
            $('.trEdicionPropiedad').hide();
            $('.trEdicionParametro').hide();
            $('.trEdicionGrupo').hide();
            $('.trEdicionOpcion').show();
            $('#txtEditarESOpcion').val(objOpcion.attr('nombreES'));
            $('#txtEditarENOpcion').val(objOpcion.attr('nombreEN'));
            objOpcion.attr('tieneCapturaNumero') == "1" ? $('#CapturarNumeroEditarOpcion').attr('checked', true) : $('#CapturarNumeroEditarOpcion').attr('checked', false);
            objOpcion.attr('tieneCapturaTexto') == "1" ? $('#CapturarTextoEditarOpcion').attr('checked', true) : $('#CapturarTextoEditarOpcion').attr('checked', false);
            objOpcion.attr('tieneCapturaCumplimiento') == "1" ? $('#CapturarCumplimientoEditarOpcion').attr('checked', true) : $('#CapturarCumplimientoEditarOpcion').attr('checked', false);
            objOpcion.attr('activo') == "1" ? $('#actvoEditarOpcion').attr('checked', true) : $('#actvoEditarOpcion').attr('checked', false);
            $('#txtOpcionCalificacionEditar').val(objOpcion.attr('CalificacionOpcion'));
            objOpcion.attr('sag') == "1" ? $('#chkSAGEditar').attr('checked', true) : $('#chkSAGEditar').attr('checked', false);
            elementoSeleccionado = objOpcion;
            validarInputs($('#txtEditarESOpcion'), $('#txtEditarENOpcion'));
            $(elementoSeleccionado).attr('esteno', '1');
            $('#txtEditarENOpcion').change(function () {
                var textoEn = $(this).val();
                $(elementoSeleccionado).parent().find('[otherClass="Opcion"]').each(function () {
                    if (textoEn == $(this).attr('nombreen')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } else {
                    }
                });
            });

            $('#txtEditarESOpcion').change(function () {
                var textoEs = $(this).val();
                $(elementoSeleccionado).parent().find('[otherClass="Opcion"]').each(function () {
                    if (textoEs == $(this).attr('nombrees')) {//&& textoEn == $(this).attr('nombreen')
                        $(elementoSeleccionado).attr('yaesta', '1');
                    } else {
                    }
                });
            });
        }

        function modificarOpcion() {
            var Errores = '';
            var nombreModificadoOpcionES = $('#txtEditarESOpcion').val().trim();
            var nombreModificadoOpcionEN = $('#txtEditarENOpcion').val().trim();
            var activoModificadoOpcion = $('#actvoEditarOpcion').prop('checked') == true ?  1 : 0;
            var capturaNumeroModificado = $('#CapturarNumeroEditarOpcion').prop('checked') == true ? 1 :  0;
            var capturaTextoModificado = $('#CapturarTextoEditarOpcion').prop('checked') == true ?  1 :  0;
            var capturaCumplimientoModificado = $('#CapturarCumplimientoEditarOpcion').prop('checked') == true ? 1 : 0;
            var CalificacionOpcion = $('#txtOpcionCalificacionEditar').val().trim();
            var sag = $('#chkSAGEditar').prop('checked') == true ? 1 : 0;

            $(elementoSeleccionado).parent().find('[otherclass="Opcion"]').each(function () {
                //Opcion  Seleccionado
                if ($(this).attr('esteno') == '1') {

                } else {
                    if ($(this).attr('nombrees') == nombreModificadoOpcionES && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'La opción ' + $(this).attr('nombrees') + ' en español que se intenta agregar ya existe dentro de la propiedad.<br/>';
                    } else {
                        //Correcto
                    }
                    if ($(this).attr('nombreen') == nombreModificadoOpcionEN && $(elementoSeleccionado).attr('yaesta') == '1') {
                        Errores += 'La opción ' + $(this).attr('nombreen') + ' en inglés que se intenta agregar ya existe dentro de la propiedad.<br/>';
                    } else {
                        //Correcto
                    }
                }

            });
          

            if (nombreModificadoOpcionES == '') {
                Errores += 'El nombre de la opción en español es requerido.<br/>';
                $('#txtEditarESOpcion').addClass('error');
            } else {
                $('#txtEditarESOpcion').removeClass('error');

            }

            if (nombreModificadoOpcionEN == '') {
                Errores += 'El nombre de la opción en inglés es requerido.<br/>';
                $('#txtEditarENOpcion').addClass('error');
            } else {
                $('#txtEditarENOpcion').removeClass('error');
            }

            //Validamos que la calificacion este dentro del rango 0-100
            if (CalificacionOpcion && CalificacionOpcion.length > 0) {
                if (CalificacionOpcion >= 0 && CalificacionOpcion <= 100) {
                    $('#txtOpcionCalificacionEditar').removeClass('error');
                } else {
                    Errores += 'La Calificación debe estar entre 0 - 100';
                    $('#txtOpcionCalificacionEditar').addClass('error');
                }
            } else {
                $('#txtOpcionCalificacionEditar').removeClass('error');
            }


            if (Errores != '') {
                popUpAlert(Errores,'error');
                 
            } else {
                if (elementoSeleccionado != null) {
                    if ($(elementoSeleccionado).hasClass('CargadoOpcion')) {
                        $(elementoSeleccionado).find('.accHeaderOpcion').text(nombreModificadoOpcionES);
                        $(elementoSeleccionado).attr('nombreES', nombreModificadoOpcionES);
                        $(elementoSeleccionado).attr('nombreEN', nombreModificadoOpcionEN);
                        $(elementoSeleccionado).attr('activo', activoModificadoOpcion);
                        $(elementoSeleccionado).attr('CalificacionOpcion', CalificacionOpcion);
                        $(elementoSeleccionado).attr('sag', sag);
                        $(elementoSeleccionado).attr('tieneCapturaNumero', capturaNumeroModificado);
                        $(elementoSeleccionado).attr('tieneCapturaTexto', capturaTextoModificado);
                        $(elementoSeleccionado).attr('tieneCapturaCumplimiento', capturaCumplimientoModificado);
                        $(elementoSeleccionado).removeClass('CargadoOpcion');
                        $(elementoSeleccionado).addClass('ModificadoOpcion');
                        $(elementoSeleccionado).attr('estado', 'modificado');
                        $(elementoSeleccionado).parent().attr('estado', 'modificado');
                        $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                        $(elementoSeleccionado).parent().parent().parent().attr('estado', 'modificado');
                        if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().length == 0) {
                            $(elementoSeleccionado).find('.accHeaderOpcion').after('<input type="text" id="txtCapturaTextoOpcion" placeholder="Escriba algún texto"/>')
                        } else {
                            if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().length > 0) {

                            } else {
                                $(elementoSeleccionado).find('#txtCapturaTextoOpcion').remove();
                            }
                        }
                        if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().next().length == 0) {
                            $(elementoSeleccionado).find('.accHeaderOpcion').after('<input type="text" id="txtCapturaNumeroOpcion" placeholder="0.0"/>')
                        } else {
                            if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().next().length > 0) {

                            } else {
                                $(elementoSeleccionado).find('#txtCapturaNumeroOpcion').remove();
                            }
                        }
                        activoModificadoOpcion == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().removeAttr('disabled');
                        activoModificadoOpcion == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().next().removeAttr('disabled');

                    } else {
                        if ($(elementoSeleccionado).hasClass('NuevoOpcion')) {
                            $(elementoSeleccionado).find('.accHeaderOpcion').text(nombreModificadoOpcionES);
                            $(elementoSeleccionado).attr('nombreES', nombreModificadoOpcionES);
                            $(elementoSeleccionado).attr('nombreEN', nombreModificadoOpcionEN);
                            $(elementoSeleccionado).attr('activo', activoModificadoOpcion);
                            $(elementoSeleccionado).attr('CalificacionOpcion', CalificacionOpcion);
                            $(elementoSeleccionado).attr('sag', sag);
                            $(elementoSeleccionado).attr('tieneCapturaNumero', capturaNumeroModificado);
                            $(elementoSeleccionado).attr('tieneCapturaTexto', capturaTextoModificado);
                            $(elementoSeleccionado).attr('tieneCapturaCumplimiento', capturaCumplimientoModificado);
                            $(elementoSeleccionado).removeClass('NuevoOpcion');
                            $(elementoSeleccionado).addClass('ModificadoOpcion');
                            $(elementoSeleccionado).attr('estado', 'modificado');
                            $(elementoSeleccionado).parent().attr('estado', 'modificado');
                            $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                            $(elementoSeleccionado).parent().parent().parent().attr('estado', 'modificado');
                            if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().length == 0) {
                                $(elementoSeleccionado).find('.accHeaderOpcion').after('<input type="text" id="txtCapturaTextoOpcion" placeholder="Escriba algún texto"/>')
                            } else {
                                if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().length > 0) {

                                } else {
                                    $(elementoSeleccionado).find('#txtCapturaTextoOpcion').remove();
                                }
                            }
                            if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().next().length == 0) {
                                $(elementoSeleccionado).find('.accHeaderOpcion').after('<input type="text" id="txtCapturaNumeroOpcion" placeholder="0.0"/>')
                            } else {
                                if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().next().length > 0) {

                                } else {
                                    $(elementoSeleccionado).find('#txtCapturaNumeroOpcion').remove();
                                }
                            }

                            activoModificadoOpcion == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().removeAttr('disabled');
                            activoModificadoOpcion == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().next().removeAttr('disabled');
                        }
                        else {
                            if ($(elementoSeleccionado).hasClass('ModificadoOpcion')) {
                                $(elementoSeleccionado).find('.accHeaderOpcion').text(nombreModificadoOpcionES);
                                $(elementoSeleccionado).attr('nombreES', nombreModificadoOpcionES);
                                $(elementoSeleccionado).attr('nombreEN', nombreModificadoOpcionEN);
                                $(elementoSeleccionado).attr('activo', activoModificadoOpcion);
                                $(elementoSeleccionado).attr('CalificacionOpcion', CalificacionOpcion);
                                $(elementoSeleccionado).attr('sag', sag);
                                $(elementoSeleccionado).attr('tieneCapturaNumero', capturaNumeroModificado);
                                $(elementoSeleccionado).attr('tieneCapturaTexto', capturaTextoModificado);
                                $(elementoSeleccionado).attr('tieneCapturaCumplimiento', capturaCumplimientoModificado);
                                $(elementoSeleccionado).removeClass('ModificadoOpcion');
                                $(elementoSeleccionado).addClass('ModificadoOpcion');
                                $(elementoSeleccionado).attr('estado', 'modificado');
                                $(elementoSeleccionado).parent().attr('estado', 'modificado');
                                $(elementoSeleccionado).parent().parent().attr('estado', 'modificado');
                                $(elementoSeleccionado).parent().parent().parent().attr('estado', 'modificado');
                                if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().length == 0) {
                                    $(elementoSeleccionado).find('.accHeaderOpcion').after('<input type="text" id="txtCapturaTextoOpcion" placeholder="Escriba algún texto"/>')
                                } else {
                                    if (capturaTextoModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().length > 0) {

                                    } else {
                                        $(elementoSeleccionado).find('#txtCapturaTextoOpcion').remove();
                                    }
                                }
                                if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().next().length == 0) {
                                    $(elementoSeleccionado).find('.accHeaderOpcion').after('<input type="text" id="txtCapturaNumeroOpcion" placeholder="0.0"/>')
                                } else {
                                    if (capturaNumeroModificado == 1 && $(elementoSeleccionado).find('.accHeaderOpcion').next().next().length > 0) {

                                    } else {
                                        $(elementoSeleccionado).find('#txtCapturaNumeroOpcion').remove();
                                    }
                                }

                                activoModificadoOpcion == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().removeAttr('disabled');
                                activoModificadoOpcion == 0 ? $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().next().attr('disabled', 'disabled') : $(elementoSeleccionado).find('label[nombrees="' + nombreModificadoOpcionES + '"]').next().next().removeAttr('disabled');
                            }
                        }
                    }
                } else {
                    //
                }
            }
        }


        function regresarFormularioGrupo() {
            $('.trEdicionGrupo').hide();
            $('.trEdicionParametro').hide();
            $('.trEdicionPropiedad').hide();
            $('.trEdicionOpcion').hide();
            $('.trNuevoGrupo').show();
            $('.trNuevoGrupo td input[id="txtAgregarESGrupo"]').val('');
            $('.trNuevoGrupo td input[id="txtAgregarENGrupo"]').val('');
            $('.trNuevoGrupo td input[id="activoAgregarGrupo"]').attr('checked', true);
            $('.trNuevoGrupo td input[id="txtCalificacionAgregar"]').val('');
            $('.trNuevoGrupo td input[id="txtCalificacionNoPlantacionAgregar"]').val('');
            $('.trEdicionGrupo td input[id="txtAgregarESParametro"]').val('');
            $('.trEdicionGrupo td input[id="txtAgregarENParametro"]').val('');
            $('.trEdicionGrupo td input[id="activoAgregarParametro"]').attr('checked', true);
            $('.trEdicionParametro td input[id="txtAgregarESPropiedad"]').val('');
            $('.trEdicionParametro td input[id="txtAgregarENPropiedad"]').val('');
            $('.trEdicionParametro td input[id="CapturarNumeroAgregarPropiedad"]').attr('checked', true);
            $('.trEdicionParametro td input[id="CapturarTextoAgregarPropiedad"]').attr('checked', true);
            $('.trEdicionParametro td input[id="CapturarCumplimientoAgregarPropiedad"]').attr('checked', true);
            $('.trEdicionParametro td input[id="activoAgregarPropiedad"]').attr('checked', true);
            $('.trEdicionPropiedad td input[id="txtAgregarESOpcion"]').val('');
            $('.trEdicionPropiedad td input[id="txtAgregarENOpcion"]').val('');
            $('.trEdicionPropiedad td input[id="CapturarNumeroAgregarOpcion"]').attr('checked', true);
            $('.trEdicionPropiedad td input[id="CapturarTextoAgregarOpcion"]').attr('checked', true);
            $('.trEdicionPropiedad td input[id="CapturarCumplimientoAgregarOpcion"]').attr('checked', true);
            $('.trEdicionPropiedad td input[id="ActivoAgregarOpcion"]').attr('checked', true);
            $('#txtAgregarESGrupo').removeClass('error');
            $('#txtAgregarENGrupo').removeClass('error');
            $('#txtCalificacionAgregar').removeClass('error');
        }


        function LimpiarPantalla() {
            cargarFormularioGrupo();
        }


        function crearObjeto() {
            if (Grupos == null || Grupos.length == 0) {
                return Grupos = $('div[otherclass="Grupo"]:not([estado="cargado"])').map(function () {
                    return {
                        idGrupo: $(this).attr('idGrupo'),
                        nombreGrupoES: $(this).attr('nombreES'),
                        nombreGrupoEN: $(this).attr('nombreEN'),
                        activo: $(this).attr('activo'),
                        ponderacion: $(this).attr('Ponderacion') == '' ? 0 : isNaN(parseFloat($(this).attr('Ponderacion'))) ? 0 : parseFloat($(this).attr('Ponderacion')),
                        ponderacionNP: $(this).attr('PonderacionNP') == '' ? 0 : isNaN(parseFloat($(this).attr('PonderacionNP'))) ? 0 : parseFloat($(this).attr('PonderacionNP')),
                        aplicaPlantacion: $(this).attr('aplicaPlantacion'),
                        aplicaNoPlantacion: $(this).attr('aplicaNoPlantacion'),
                        ordenGrupo: $(this).attr('orden'),
                        Parametros: $(this).find('div[otherclass="Parametro"]:not([estado="cargado"])').length == 0 ? [] : $(this).find('div[otherclass="Parametro"]:not([estado="cargado"])').map(function () {
                            return {
                                idParametro: $(this).attr('idParametro'),
                                nombreParametroES: $(this).attr('nombreES'),
                                nombreParametroEN: $(this).attr('nombreEN'),
                                activo: $(this).attr('activo'),
                                ordenParametro: $(this).attr('orden'),
                                Propiedades: $(this).find('div[otherclass="Propiedad"]:not([estado="cargado"])').length == 0 ? [] : $(this).find('div[otherclass="Propiedad"]:not([estado="cargado"])').map(function () {
                                    return {
                                        idPropiedad: $(this).attr('idPropiedad'),
                                        nombrePropiedadES: $(this).attr('nombreES'),
                                        nombrePropiedadEN: $(this).attr('nombreEN'),
                                        activo: $(this).attr('activo'),
                                        tieneCapturaNumero: $(this).attr('tieneCapturaNumero'),
                                        tieneCapturaTexto: $(this).attr('tieneCapturaTexto'),
                                        tieneCapturaCumplimiento: $(this).attr('tieneCapturaCumplimiento'),
                                        ordenPropiedad: $(this).attr('orden'),
                                        Opciones: $(this).find('div[otherclass="Opcion"]:not([estado="cargado"])').length == 0 ? [] : $(this).find('div[otherclass="Opcion"]:not([estado="cargado"])').map(function () {
                                            return {
                                                idOpcion: $(this).attr('idOpcion'),
                                                nombreOpcionES: $(this).attr('nombreES'),
                                                nombreOpcionEN: $(this).attr('nombreEN'),
                                                activo: $(this).attr('activo'),
                                                calificacionOpcion: $(this).attr('calificacionOpcion') == '' ? 0 : isNaN(parseFloat($(this).attr('calificacionOpcion'))) ? 0 : parseFloat($(this).attr('calificacionOpcion')),
                                                sag: $(this).attr('sag'),
                                                tieneCapturaNumero: $(this).attr('tieneCapturaNumero'),
                                                tieneCapturaTexto: $(this).attr('tieneCapturaTexto'),
                                                tieneCapturaCumplimiento: $(this).attr('tieneCapturaCumplimiento'),
                                                ordenOpcion: $(this).attr('orden')
                                            }
                                        }).get()
                                    }
                                }).get()
                            }
                        }).get()
                    }
                }).get()
            } else {
              ///
            }

        }

        function validarOpciones() {
            var flag = true;
            if ($('div[otherclass="Propiedad"]').length > 0) {
                $('div[otherclass="Propiedad"]').each(function () {
                    if ($(this).find('[otherclass="Opcion"]').length == 0) {
                        $(this).css('color', 'red');

                        popUpAlert('Las propiedades marcadas en rojo deben de llevar mínimo una opción', 'warning');
                        
                    } else {

                    }

                });
               return flag = false;
            } else {
                //se procede a realizar el guardado de la configuracion
            }
       }

       function seleccionarRadios() {
           $('.CargadoGrupo').find('.radioGrupoNA').click(function () {
               var attr = $(this).parent().attr('nombrees');
               attr = attr.replace(/\s+/g, '');
               if ($(this).is(':checked')) {
                   $('.CargadoGrupo').find('.radioParametroNA' + attr + '').each(function () {
                       $(this).prop('checked', true);
                   });
               } else {
                   $('.CargadoGrupo').find('.radioParametroNA' + attr + '').each(function () {
                       $(this).prop('checked', false);
                   });
               }
           });

           $('.CargadoGrupo').find('.radioGrupoOK').click(function () {
               var attr = $(this).parent().attr('nombrees');
               attr = attr.replace(/\s+/g, '');
               if ($(this).is(':checked')) {
                   $('.CargadoGrupo').find('.radioParametroOK' + attr + '').each(function () {
                       $(this).prop('checked', true);
                   });
               } else {
                   $('.CargadoGrupo').find('.radioParametroOK' + attr + '').each(function () {
                       $(this).prop('checked', false);
                   });
               }
           });

           $('.CargadoGrupo').find('.radioGrupoX').click(function () {
               var attr = $(this).parent().attr('nombrees');
               attr = attr.replace(/\s+/g, '');
               if ($(this).is(':checked')) {
                   $('.CargadoGrupo').find('.radioParametroX' + attr + '').each(function () {
                       $(this).prop('checked', true);
                   });
               } else {
                   $('.CargadoGrupo').find('.radioParametroX' + attr + '').each(function () {
                       $(this).prop('checked', false);
                   });
               }
           });

       }



       function calcularCalificacion() {
          


       }


       function guardarConfiguracionGrowing() {
   
//            if (validarOpciones()) {
//                $('div[otherclass="Propiedad"]').each(function () {
//                    if ($(this).find('[otherclass="Opcion"]').length == 0) {
//                        $(this).css('color', 'black');
//                    } else {

//                    }

//                });
//                PageMethods.guardarConfiguracionGrowing(crearObjeto(), function (response) {
//                    if (response[0] == '1') {
//                        popUpAlert(response[1], response[2]);
//                    }
//                    else {
//                        popUpAlert(response[1], response[2]);
//                    }
//                });
            //            }
           //            console.log(crearObjeto());
           try {
               PageMethods.guardarConfiguracionGrowing(crearObjeto(), function (response) {
                   if (response[0] == '1') {
                       popUpAlertButtons(response[1], '<input  id="popup1" type="button" onclick="javascript:location.assign(\'ConfiguracionCapturaGrowing.aspx\');" value="ok"/>', response[2]);
                       //popUpAlert(response[1], response[2]);
                   }
                   else {
//                       popUpAlertButtons(response[1], '<input  id="popup1" type="button" value="ok"/>', response[2]);
                       popUpAlert(response[1], response[2]);
                   }
               });
           } catch (e) {
           console.log(e);
           }
        }


        function obtenerConfiguracionGrowing() {
            PageMethods.obtenerConfiguracionCapturaGrowing(function (response) {
                if (response[0] == '1') {
                    $('#divGeneral').html(response[2]);

                    //Agregamos las clases para la funcion sortable
                    $('div.CargadoGrupo').addClass('SortableGrupo');
                    $('div.CargadoParametro').addClass('SortableParametro');
                    $('div.CargadoPropiedad').addClass('SortablePropiedad');
                    $('div.CargadoOpcion').addClass('SortableOpcion');

                    //Funcionalidad de ordenamiento para los Grupos
                    $("div#divGeneral").sortable({
                        items: "div:.SortableGrupo",
                        update: function (event, ui) {
                            //Actualizamos el orden de los grupos
                            var pivote = 0;
                            $('div.SortableGrupo').each(function () {
                                pivote = pivote + 1;
                                $(this).attr('orden', pivote);
                                $(this).attr('estado', 'modificado');
                            });
                        }
                    });
                    $("div#divGeneral").disableSelection();

                    $('.CargadoGrupo').each(function () {
                        var idGrupo = $(this).attr('id');
                        var grupo = $(this).attr('nombreES');
                        var ponderacion = $(this).attr('Ponderacion');
                        var aplicaPlantacion = $(this).attr('AplicaPlantacion');
                        var aplicaNoPlantacion = $(this).attr('AplicaNoPlantacion');
                        $(this).prepend('<label  class="accHeaderGrupo" ponderacion="' + ponderacion + '" aplicaPlantacion="' + aplicaPlantacion + '" aplicaNoPlantacion="' + aplicaNoPlantacion + '" onclick="cargarEdicionGrupo($(this).parent());"><h2>' + grupo + '</h2></label><img src="../comun/img/sort_desc.png" id="imgASC" onclick="agruparPropiedadesGrupos($(this),$(this).next());"><img src="../comun/img/sort_asc.png" id="imgDESC" onclick="agruparPropiedadesGrupos($(this),$(this).prev());">');
                        //                        $(this).prepend('<input  type="checkbox" ></input>');
                        //                        $(this).append('<input type="checkbox" class="radioGrupoNA" id="N/A" name="radioGrupo" /><label class="lbltext" for="N/A">Select N/A All</label>');
                        //                        $(this).append('<input type="checkbox" class="radioGrupoOK" id="OK" name="radioGrupo"/><label class="lbltext" for="OK">Select OK All</label>');
                        //                        $(this).append('<input type="checkbox" class="radioGrupoX" id="X" name="radioGrupo"/><label class="lbltext" for="X">Select X All</label>');
                        //                        $(this).append('<input type="checkbox" class="checkPlantacion" id="Plantacion" name="radioPlantacion"/><label class="lbltext" for="Plantacion">Plantación</label>');
                        //                        $(this).append('<input type="checkbox" class="checkNoPlantacion" id="Noplantacion" name="radioPlantacion"/><label class="lbltext" for="Noplantacion">No plantación</label>');
                        //                        $(this).append('<label class="lbltext">Calificación:</label><input type="text" class="Calificacion" id="txtCalificacion"/>');
                        //grupo = grupo.replace(/\s+/g, ''); 

                        //Cambiamos la ponderaciones que sean igual a cero por ''
                        $(this).attr('Ponderacion', $(this).attr('Ponderacion') == 0 ? '' : $(this).attr('Ponderacion'));
                        $(this).attr('PonderacionNP', $(this).attr('PonderacionNP') == 0 ? '' : $(this).attr('PonderacionNP'));

                        //Funcionalidad de ordenamiento para los Parametros dentro de cada Grupo
                        $("div#" + idGrupo).sortable({
                            items: "div:.SortableParametro",
                            update: function (event, ui) {
                                //Actualizamos el orden de los parametros
                                var pivote = 0;
                                $('div#' + idGrupo + ' div.SortableParametro').each(function () {
                                    pivote = pivote + 1;
                                    $(this).attr('orden', pivote);
                                    $(this).attr('estado', 'modificado');
                                    $(this).parent().attr('estado', 'modificado');
                                });
                            }
                        });
                        $("div#" + idGrupo).disableSelection();

                        $(this).find('.CargadoParametro').each(function () {
                            var idParametro = $(this).attr('id');
                            var parametro = $(this).attr('nombreES');
                            $(this).prepend('<label for="' + parametro + '" class="accHeaderParametro" onclick="cargarEdicionParametro($(this).parent());"><h2>' + parametro + '</h2></label><img src="../comun/img/sort_desc.png" id="imgASCParametro" onclick="agruparParametrosGrupos($(this),$(this).next());"><img src="../comun/img/sort_asc.png" id="imgDESCParametro" onclick="agruparParametrosGrupos($(this),$(this).prev());">');
                            $(this).prepend('<input id="' + parametro + '" type="checkbox" ></input>');
                            //                            $(this).append('<input type="radio" class="radioParametroNA' + grupo + '" id="N/A" name="radioParametro'+parametro+'" parametro="'+parametro+'"/><label class="lblpar" for="N/A">N/A</label>');
                            //                            $(this).append('<input type="radio" class="radioParametroOK' + grupo + '" id="OK" name="radioParametro' + parametro + '" parametro="' + parametro + '"/><label class="lblpar" for="OK">OK</label>');
                            //                            $(this).append('<input type="radio" class="radioParametroX' + grupo + '" id="X" name="radioParametro' + parametro + '" parametro="' + parametro + '"/><label class="lblpar" for="X">X</label>');

                            //Funcionalidad de ordenamiento para las Propiedades dentro de cada Parametro
                            $("div#" + idParametro).sortable({
                                items: "div:.SortablePropiedad",
                                update: function (event, ui) {
                                    //Actualizamos el orden de las propiedades
                                    var pivote = 0;
                                    $('div#' + idParametro + ' div.SortablePropiedad').each(function () {
                                        pivote = pivote + 1;
                                        $(this).attr('orden', pivote);
                                        $(this).attr('estado', 'modificado');
                                        $(this).parent().attr('estado', 'modificado');
                                        $(this).parent().parent().attr('estado', 'modificado');
                                    });
                                }
                            });
                            $("div#" + idParametro).disableSelection();

                            $(this).find('.CargadoPropiedad').each(function () {
                                var idAttrPropiedad = $(this).attr('id');
                                var propiedad = $(this).attr('nombreES');
                                var idPropiedad = $(this).attr('idpropiedad');
                                var activo = $(this).attr('activo');
                                var tieneCapturaNumeroPropiedad;
                                var tieneCapturaTextoPropiedad;
                                $(this).attr('tienecapturanumero') == "0" || $(this).attr('tienecapturanumero') == undefined ? tieneCapturaNumeroPropiedad = 0 : tieneCapturaNumeroPropiedad = 1;
                                $(this).attr('tienecapturatexto') == "0" || $(this).attr('tienecapturatexto') == undefined ? tieneCapturaTextoPropiedad = 0 : tieneCapturaTextoPropiedad = 1;

                                //Funcionalidad de ordenamiento para las Opciones de cada Propiedad--https://jqueryui.com/sortable/#items jquery sortable callback
                                $("div#" + idAttrPropiedad).sortable({
                                    items: "div:.SortableOpcion",
                                    update: function (event, ui) {
                                        //Actualizamos el orden de las opciones
                                        var pivote = 0;
                                        $('div#' + idAttrPropiedad + ' div.SortableOpcion').each(function () {
                                            pivote = pivote + 1;
                                            $(this).attr('orden', pivote);
                                            $(this).attr('estado', 'modificado');
                                            $(this).parent().attr('estado', 'modificado');
                                            $(this).parent().parent().attr('estado', 'modificado');
                                            $(this).parent().parent().parent().attr('estado', 'modificado');
                                        });
                                    }
                                });
                                $("div#" + idAttrPropiedad).disableSelection();

                                if ($(this).find('.CargadoOpcion').length > 0) {
                                    $(this).prepend('<label class="accHeaderPropiedad" id="' + idPropiedad + '"  nombrees="' + propiedad + '" onclick="cargarEdicionPropiedad($(this).parent());">' + propiedad + '</label>');
                                    $(this).prepend('<input idPropiedad="' + idPropiedad + '" id="' + propiedad + '" type="checkbox" ></input>');
                                    tieneCapturaNumeroPropiedad == 0 ? $('label[nombrees="' + propiedad + '"]').after('') : $('label[nombrees="' + propiedad + '"]').after('<input type="text" id="txtCapturaNumeroPropiedad" placeholder="0.0"/>');
                                    tieneCapturaTextoPropiedad == 0 ? $('label[nombrees="' + propiedad + '"]').after('') : $('label[nombrees="' + propiedad + '"]').after('<input type="text" id="txtCapturaTextoPropiedad" placeholder="Escriba algún texto"/>');
                                    activo == 0 ? $(this).find('label[nombrees="' + propiedad + '"]').next().attr('disabled', 'disabled') : '';
                                    activo == 0 ? $(this).find('label[nombrees="' + propiedad + '"]').next().next().attr('disabled', 'disabled') : '';

                                    $(this).find('.CargadoOpcion').each(function () {
                                        var opcion = $(this).attr('nombreES');
                                        var idOpcion = $(this).attr('idopcion');
                                        var activo = $(this).attr('activo');
                                        var tieneCapturaNumeroOpcion;
                                        var tieneCapturaTextoOpcion;
                                        $(this).attr('tienecapturanumero') == "0" || $(this).attr('tienecapturanumero') == undefined ? tieneCapturaNumeroOpcion = 0 : tieneCapturaNumeroOpcion = 1;
                                        $(this).attr('tienecapturatexto') == "0" || $(this).attr('tienecapturatexto') == undefined ? tieneCapturaTextoOpcion = 0 : tieneCapturaTextoOpcion = 1;
                                        $(this).append('<input name="' + opcion + '" type="radio" idOpcion="' + idOpcion + '" />');
                                        $(this).append('<label for="' + idOpcion + '" id="' + idOpcion + '" class="accHeaderOpcion" nombrees="' + opcion + '" onclick="cargarEdicionOpcion($(this).parent());">' + opcion + '</label>');
                                        tieneCapturaNumeroOpcion == 0 ? $('label[nombrees="' + opcion + '"]').after('') : $('label[nombrees="' + opcion + '"]').after('<input type="text" id="txtCapturaNumeroOpcion" placeholder="0.0"/>');
                                        tieneCapturaTextoOpcion == 0 ? $('label[nombrees="' + opcion + '"]').after('') : $('label[nombrees="' + opcion + '"]').after('<input type="text" id="txtCapturaTextoOpcion" placeholder="Escriba algún texto"/>');
                                        activo == 0 ? $(this).find('label[nombrees="' + opcion + '"]').next().attr('disabled', 'disabled') : '';
                                        activo == 0 ? $(this).find('label[nombrees="' + opcion + '"]').next().next().attr('disabled', 'disabled') : '';
                                    });

                                    seleccionarRadios();
                                }
                                else {
                                    $(this).prepend('<label for="' + (idPropiedad + propiedad) + '" class="accHeaderPropiedad"  onclick="cargarEdicionPropiedad($(this).parent());">' + propiedad + '</label>');
                                    $(this).prepend('<input id="' + (idPropiedad + propiedad) + '" type="checkbox" class="cajaCh "></input>');
                                }
                            });
                        })
                    });
                }
                else {
                    $('#divGeneral').html('');
                    popUpAlert(response[1], response[2]);
                }
            });
        }
    </script>

    <style type="text/css">
        .trEdicionGrupo, .trEdicionParametro, .trEdicionPropiedad, .trEdicionOpcion
        {
            display: none;
        }
        
        .accHeaderGrupo, .accHeaderParametro, .accHeaderPropiedad, .accHeaderOpcion
        {
            cursor: pointer;
        }

        

        .accHeaderGrupo h2 {
	        color: #FFF;
	        font-size: 16px;
	        width: auto !important;
	        margin: 0;
	        margin-left: 10px;
        }

        .NuevoGrupo input, .CargadoGrupo input ,.ModificadoGrupo input{
	        margin:	10px;
        }

        /*.NuevoParametro[activo="1"], .CargadoParametro[activo="1"],.ModificadoParametro[activo="1"]{
	        background:#e4ead8;
	        -webkit-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        -moz-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
        }*/

        .NuevoParametro input[type="checkbox"], .CargadoParametro input[type="checkbox"],.ModificadoParametro input[type="checkbox"]{
	        margin-left:25px;
        }

        .NuevoParametro[activo="1"] h2, .CargadoParametro[activo="1"] h2, .ModificadoParametro[activo="1"] h2 {
	        color: #F60;
	        font-size: 14px;
	        display:inline;
        }

        .accHeaderParametro h2 
        {
            color: #FFF;
	        font-size: 16px;
	        display:inline;
        }
        
        .NuevoPropiedad[activo="1"],.CargadoPropiedad[activo="1"],.ModificadoPropiedad[activo="1"]  {
	        background: white;
        }

        .NuevoPropiedad input[type="checkbox"],.CargadoPropiedad input[type="checkbox"],.ModificadoPropiedad input[type="checkbox"] {
	        margin-left: 50px;
        }

        .accHeaderPropiedad h2, .accHeaderOpcion h2  {
	        font-size: 12px;
	        color:#000;
	        font-weight: normal;
	        display: inline;
        }

        .NuevoOpcion[activo="1"], .CargadoOpcion[activo="1"],.ModificadoOpcion[activo="1"] {
	        background: white;
	        color:#000;
        }

        .NuevoOpcion[activo="0"] label[class="accHeaderOpcion"], .CargadoOpcion[activo="0"] label[class="accHeaderOpcion"],.ModificadoOpcion[activo="0"] label[class="accHeaderOpcion"]{
	        /*background: white;*/
	        color: #999999;
        }
        .NuevoOpcion input[type="radio"] , .CargadoOpcion input[type="radio"],.ModificadoOpcion input[type="radio"]{
	        margin-left: 75px;
        }

        label.español, label.ingles {
	        font-size: 10px;
	        color: #f60;
	        float-left;
        }

        input#activoAgregarGrupo {
	        position: relative;
	        left: 10px;
	
        }


        /*.NuevoParametro[activo="0"],.CargadoParametro[activo="0"],.ModificadoParametro[activo="0"]{
	        background:#ededed;
		    -webkit-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        -moz-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        /*pointer-events: none;*/
	        cursor:auto;
        }*/

        .NuevoParametro[activo="0"] h2 ,.CargadoParametro[activo="0"] h2 ,.ModificadoParametro[activo="0"] h2
        {
            display: inline;
            color: Black;
            font-size: 14px;
        }

        .NuevoPropiedad[activo="0"] label[class="accHeaderPropiedad"],.CargadoPropiedad[activo="0"] label[class="accHeaderPropiedad"] ,.ModificadoPropiedad[activo="0"] label[class="accHeaderPropiedad"]
        {
            color:#999999;
            font-size: 12px;
        }
     
        #divGeneral
        {
            overflow: auto;
        }

        #imgCrearGrupo,#imgCrearPametro,#imgCrearPropiedad,#imgCrearOpcion{
          cursor:pointer;
        }

        #imgActualizarGrupo,#imgActualizarParametro,#imgActualizarPropiedad,#imgActualizarOpcion{
         cursor:pointer;
        }
        div#wrapper table.index span label 
        {
            float:left;
            margin-bottom: 0;
        }
        
        img#imgASC 
        {
            position: absolute;
            right: 7px;
            top: 7px;
            cursor: pointer;
        }
        
        img#imgDESC
        {  
            display:none;
            position: absolute;
            right: 7px;
            top: 12px;
            cursor: pointer;
        }
        
        img#imgASCParametro
        {
            float: left;
            padding-top: 4px;
            padding-right: 6px;
            cursor:pointer;
            position: relative;
            left:740px;
        }
        
        img#imgDESCParametro
        {
            float: left;
            padding-top: 12px;
            padding-right: 6px;
            position:relative;
            left:740px;
            display:none;
            cursor:pointer;
        }
        
        
        div[otherclass="Parametro"]
        {
            display:none;
        }
        
        div[otherclass="Propiedad"]
        {
            display:none;
        }
        
        input.Calificacion {
            width: 50px;
        }
        
        label
        {
            margin: 5px !important
        }
        
        label.lbltext {
            color: white;
            font-size: 11px;
            font-weight: bold;
        }
        
        label.lblpar {
            /* margin: 0px; */
            color: #000;
            font-size: 11px;
            font-weight: bold;
        }
        
        input.radioGrupo {
            margin: 0;
            position: relative;
            top: 3px;
            left: 7px;
        }
        input.Calificacion {
            margin-top: 3px;
        }
        
        table.index tr td input[type="text"]
        {
            float:left;
            font-size:12px;
            width:83px;
        }
        
        input#plantacionAgregar {
            margin-top: 6px;
        }

        input#noPlantacionAgregar {
            margin-top: 6px;
        }
        img#imgRegresarGrupo, img#imgRegresarParametro, img#imgRegresarPropiedad, img#imgRegresarOpcion {
            max-width: 25px;
            cursor: pointer;
        }
        
        .none
        {
            display: none;
            }
            
        label.español {
           position: relative;
           left: -45px;
        }
        
        label.ingles
        {
           position: relative;
           left: -45px; 
         }
         
         
         
         
         
        .NuevoGrupo, .CargadoGrupo, .ModificadoGrupo
        {
            -moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 10px;
            -webkit-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        -moz-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        width: 100%;
	        padding: 10px 0;
	        box-sizing: border-box;
	        position: relative;
	        
        }

        .NuevoGrupo[activo="1"], .CargadoGrupo[activo="1"], .ModificadoGrupo[activo="1"] {
	        background: #a5c38e;
        }

        .NuevoGrupo[activo="0"], .CargadoGrupo[activo="0"], .ModificadoGrupo[activo="0"] {
	        background: #C6C6C6;
        }
        
        
        
        
         .NuevoParametro[activo="1"], .CargadoParametro[activo="1"],.ModificadoParametro[activo="1"]{
	        -webkit-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        -moz-box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
	        box-shadow: inset 2px 2px 0px 0px rgba(92,92,92,0.3);
        }

         .NuevoParametro[activo="1"], .CargadoParametro[activo="1"],.ModificadoParametro[activo="1"]{
            background: #e4ead8;
        }


        .NuevoParametro[activo="0"], .CargadoParametro[activo="0"],.ModificadoParametro[activo="0"]{
            background: #d6d6d6;
        }
        
        .NuevoPropiedad[activo="0"],.CargadoPropiedad[activo="0"],.ModificadoPropiedad[activo="0"] {
	        background: #e4e4e4;
        }
        
        
        

            
         .error
         {
             border: 1px solid red !important;
             background:rgba(255,0,0,.5);
             }
                      
    </style>
    <script id="validaciones" type="text/javascript">
        function validarNumeroPorcentaje(txtField) {
            var id = $(txtField).attr('id');
            try {
                var numeroAsigando = parseFloat($('#' + id).val());
                if (numeroAsigando >= 0 && !isNaN(numeroAsigando)) {
                    if (numeroAsigando <= 100 && numeroAsigando > 0 /*&& numeroAsigando != NaN && numeroAsigando != ''*/) {
                        $('#' + id).removeClass('error');
                        return true;
                    } else {
                        $('#' + id).addClass('error');
                        return false;
                    }
                } else {
                    $('#' + id).removeClass('error');
                    return true;
                }
            } catch (e) {
                $('#' + id).addClass('error');
                console.log(e);
                return false;
            }
        }

        function validarCalificacionOpcion(txtField) {
            var id = $(txtField).attr('id');
            try {
                var numeroAsigando = $('#' + id).val();
                if ((numeroAsigando <= 100 && numeroAsigando >= 0 && numeroAsigando.length > 0) || numeroAsigando == '' /*&& numeroAsigando != NaN && numeroAsigando != ''*/) {
                    $('#' + id).removeClass('error');
                    return true;
                } else {
                    $('#' + id).addClass('error');
                    return false;
                }
            } catch (e) {
                $('#' + id).addClass('error');
                console.log(e);
                return false;
            }
        }

        function validarCajasdeTexto(txtField) {
            var id = $(txtField).attr('id');
            try {
                var valor = $('#' + id).val();
                if (valor != '') {
                    $('#' + id).removeClass('error');
                    return true;
                } else {
                    $('#' + id).addClass('error');
                    return false;
                }
            } catch (e) {
                $('#' + id).addClass('error');
                console.log(e);
                return false;
            }
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="Label1" runat="server">Configuración para Captura de Growing</asp:Label></h1>
        <table class="index" border="0">
           <tr class="trNuevoGrupo">
                <td colspan="8">
                    <h3 style="color:black">Nuevo Grupo</h3>
                </td>
            </tr>
            <tr class="trNuevoGrupo">
                <td>
                    <span>
                        <label class="grupoAgregar">
                            Nombre:</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarESGrupo" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarENGrupo" onchange="validarCajasdeTexto($(this))"/></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                            Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoAgregarGrupo" /></span>
                </td>
                <td>
                    <span>
                        <label for="activoAgregarGrupo">
                            Activo</label></span>
                </td>
            </tr>
            <tr class="trNuevoGrupo">
                <td>
                     <span><label>Calificación Plantación:</label></span>
                </td>
                <td style="text-align: left;">
                    <input type="text" id="txtCalificacionAgregar" class="Calificacion floatValidate" onchange="validarNumeroPorcentaje($(this))" value="" maxlength="5" /><label>%</label>
                </td>
                <td>
                    <span><label>Calificación No Plantación:</label></span>
                   <!--<span><input type="checkbox" class="Plantacion" id="plantacionAgregar" name="Plantacion" /><label for="plantacionAgregar">Plantación</label></span>-->
                </td>
                <td style="text-align: left;">
                    <input type="text" id="txtCalificacionNoPlantacionAgregar" class="Calificacion floatValidate" onchange="validarNumeroPorcentaje($(this))" value="" maxlength="5" /><label>%</label>
                   <!--<span><input type="checkbox" class="noPlantacion" id="noPlantacionAgregar" name="noPlantacion" /><label for="noPlantacionAgregar">No plantación</label></span>-->
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <img src="../comun/img/add-icon.png" alt="Crear" id="imgCrearGrupo" onclick="crearGrupo()" />
                </td>
            </tr>
            <tr class="trEdicionGrupo">
                <td colspan="8" class="left">
                    <img src="../comun/img/regresar.png" alt="Regresar" id="imgRegresarGrupo" onclick="regresarFormularioGrupo()" /><label>Capturar
                        Nuevo Grupo</label>
                </td>
            </tr>
            <tr class="trEdicionGrupo">
                <td colspan="8">
                    <h3 style="color:black">Grupo</h3>
                </td>
            </tr>
            <tr class="trEdicionGrupo">
                <td>
                    <span>
                        <label class="grupoEditar">
                            Nombre:</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtEditarESGrupo" onchange="validarCajasdeTexto($(this))"  /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtEditarENGrupo" onchange="validarCajasdeTexto($(this))"  /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoEditarGrupo" /></span>
                </td>
                <td>
                    <span>
                        <label for="activoEditarGrupo">
                            Activo</label></span>
                </td>
               
            </tr>
             <tr class="trEdicionGrupo">
                <td>
                 <span><label>Calificación Plantación:</label></span>
                </td>
                <td style="text-align: left;">
                    <input type="text" id="txtCalificacionEditar" class="Calificacion floatValidate" value="" maxlength="5" onchange="validarNumeroPorcentaje($(this));" /><label>%</label>
                </td>
                <td>
                    <span><label>Calificación No Plantación:</label></span>
                   <!--<span><input type="checkbox" class="Plantacion" id="plantacionModificar" name="Plantacion" /><label for="plantacionModificar">Plantación</label></span>-->
                </td>
                <td style="text-align: left;">
                   <input type="text" id="txtCalificacionNoPlantacionEditar" class="Calificacion floatValidate" value="" maxlength="5" onchange="validarNumeroPorcentaje($(this))" /><label>%</label>
                   <!--<span><input type="checkbox" class="noPlantacion" id="noPlantacionModificar" name="noPlantacion" /><label for="noPlantacionModificar">No plantación</label></span>-->
                </td>
                <td></td>
                <td></td>
                <td></td>
                 <td>
                    <img src="../comun/img/refresh_icon.png" alt="Actualizar" width='24' height='24' id="imgActualizarGrupo"
                        onclick="modificarGrupo()" />
                </td>
            </tr>
            <tr class="trEdicionGrupo">
                <td colspan="8">
                    <h3 style="color:black">Parámetro</h3>
                </td>
            </tr>
            <tr class="trEdicionGrupo">
                <td>
                    <span>
                        <label class="parametroAgregar">
                            Nombre:</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarESParametro" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarENParametro" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoAgregarParametro" /></span>
                </td>
                <td>
                    <span>
                        <label for="activoAgregarParametro">
                            Activo</label></span>
                </td>
                <td>
                    <img src="../comun/img/add-icon.png" alt="Crear" id="imgCrearPametro" onclick="crearParametro()" />
                </td>
            </tr>
            <tr class="trEdicionParametro">
                <td colspan="8" class="left">
                    <img src="../comun/img/regresar.png" alt="Regresar" id="imgRegresarParametro" onclick="regresarFormularioGrupo()" /><label>Capturar
                        Nuevo Grupo</label>
                </td>
            </tr>
            <tr class="trEdicionParametro">
                <td colspan="8">
                    <h3 style="color:black">Parámetro</h3>
                </td>
            </tr>

            <tr class="trEdicionParametro">
                <td>
                    <span>
                        <label class="parametroEditar">
                            Nombre:</label>
                    </span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtEditarESParametro" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtEditarENParametro" onchange="validarCajasdeTexto($(this))"  /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoEditarParametro" /></span>
                </td>
                <td>
                    <span>
                        <label for="activoEditarParametro">
                            Activo</label></span>
                </td>
                <td>
                    <img src="../comun/img/refresh_icon.png" alt="Actualizar" width='24' height='24' id="imgActualizarParametro"
                        onclick="modificarParametro()" />
                </td>
            </tr>
            <tr class="trEdicionParametro">
                <td colspan="8">
                    <h3 style="color:black">Propiedad</h3>
                </td>
            </tr>
            <tr class="trEdicionParametro">
                <td>
                    <span>
                        <label class="propiedadAgregar">
                            Nombre:</label>
                    </span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarESPropiedad" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarENPropiedad" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td class="none">
                    <span>
                        <input type="checkbox" id="CapturarNumeroAgregarPropiedad" class="invisible"/></span>
                </td>
                <td class="none">
                    <span>
                        <label for="CapturarNumeroAgregarPropiedad" class="invisible">
                            Capturar número</label></span>
                </td>
                <td class="none">
                    <span>
                        <input type="checkbox" id="CapturarTextoAgregarPropiedad" class="invisible"/></span>
                </td>
                <td class="none">
                    <span>
                        <label for="CapturarTextoAgregarPropiedad" class="invisible">
                            Capturar texto</label></span>
                </td>
                <td class="none">
                    <span>
                        <input type="checkbox" id="CapturarCumplimientoAgregarPropiedad" class="invisible"/></span>
                </td>
                <td class="none">
                    <span>
                        <label for="CapturarCumplimientoAgregarPropiedad" class="invisible">
                            Capturar Cumplimiento</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoAgregarPropiedad"/></span>
                </td>
                <td>
                    <span>
                        <label for="activoAgregarPropiedad">
                            Activo</label></span>
                </td>
                <td>
                    <img src="../comun/img/add-icon.png" alt="Crear" id="imgCrearPropiedad" onclick="crearPropiedad()" />
                </td>
            </tr>
            <tr class="trEdicionPropiedad">
                <td colspan="8" class="left">
                    <img src="../comun/img/regresar.png" alt="Regresar" id="imgRegresarPropiedad" onclick="regresarFormularioGrupo()" /><label>Capturar
                        Nuevo Grupo</label>
                </td>
            </tr>
            <tr class="trEdicionPropiedad">
                <td colspan="8">
                    <h3 style="color:black">Propiedad</h3>
                </td>
            </tr>
            <tr class="trEdicionPropiedad">
                <td>
                    <span>
                        <label class="propiedadEditar">
                            Nombre:</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtPropiedadESEditar" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtPropiedadENEditar" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarNumeroEditarPropiedad" class="invisible" /></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarNumeroEditarPropiedad" class="invisible">
                            Capturar número</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarTextoEditarPropiedad" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarTextoEditarPropiedad" class="invisible">
                            Capturar texto</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarCumplimientoEditarPropiedad" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarCumplimientoEditarPropiedad" class="invisible">
                            Capturar Cumplimiento</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoEditarPropiedad" /></span>
                </td>
                <td>
                    <span>
                        <label for="activoEditarPropiedad">
                            Activo</label></span>
                </td>
                <td>
                    <img src="../comun/img/refresh_icon.png" alt="Actualizar" width='24' height='24' id="imgActualizarPropiedad"
                        onclick="modificarPropiedad()" />
                </td>
            </tr>
            <tr class="trEdicionPropiedad">
                <td colspan="8">
                    <h3 style="color:black">Opción</h3>
                </td>
            </tr>
            <tr class="trEdicionPropiedad">
                <td>
                    <span>
                        <label class="opcionAgregar">
                            Nombre:</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarESOpcion" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtAgregarENOpcion" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarNumeroAgregarOpcion" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarNumeroAgregarOpcion" class="invisible">
                            Capturar número</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarTextoAgregarOpcion" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarTextoAgregarOpcion" class="invisible">
                            Capturar texto</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarCumplimientoAgregarOpcion" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarCumplimientoAgregarOpcion" class="invisible">
                            Capturar Cumplimiento</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="activoAgregarOpcion"/></span>
                </td>
                <td>
                    <span>
                        <label for="activoAgregarOpcion">
                            Activo</label></span>
                </td>
                <td>
                    <img src="../comun/img/add-icon.png" alt="Crear" id="imgCrearOpcion" onclick="crearOpcion()" />
                </td>
            </tr>
            <tr class="trEdicionPropiedad">
                <td>
                    <span><label>Calificación: </label></span>
                </td>
                <td>
                    <span><input type="text" id="txtOpcionCalificacionAgregar" class="Calificacion floatValidate" value="" maxlength="5" onchange="validarCalificacionOpcion($(this))" /><label>%</label></span>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><span><input type="checkbox" id="chkSAGAgregar" /></span></td>
                <td><span><label for="chkSAGAgregar"> S A G: </label></span></td>
            </tr>
            <tr class="trEdicionOpcion">
                <td colspan="8" class="left">
                    <img src="../comun/img/regresar.png" alt="Regresar" id="imgRegresarOpcion" onclick="regresarFormularioGrupo()" /><label>Capturar
                        Nuevo Grupo</label>
                </td>
            </tr>
            <tr class="trEdicionOpcion">
                <td>
                    <span>
                        <label class="opcionEditar">
                            Opción:</label>
                    </span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtEditarESOpcion" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="español">
                            Español</label></span>
                </td>
                <td>
                    <span>
                        <input type="text" id="txtEditarENOpcion" onchange="validarCajasdeTexto($(this))" /></span>
                </td>
                <td>
                    <span>
                        <label class="ingles">
                             Inglés</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarNumeroEditarOpcion" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarNumeroEditarOpcion" class="invisible">
                            Capturar número</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarTextoEditarOpcion" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarTextoEditarOpcion" class="invisible">
                            Capturar texto</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="CapturarCumplimientoEditarOpcion" class="invisible"/></span>
                </td>
                <td>
                    <span>
                        <label for="CapturarCumplimientoEditarOpcion" class="invisible">
                            Capturar Cumplimiento</label></span>
                </td>
                <td>
                    <span>
                        <input type="checkbox" id="actvoEditarOpcion" /></span>
                </td>
                <td>
                    <span>
                        <label for="actvoEditarOpcion">
                            Activo</label></span>
                </td>
                <td>
                    <img src="../comun/img/refresh_icon.png" alt="Actualizar" width='24' height='24' id="imgActualizarOpcion"
                        onclick="modificarOpcion()" />
                </td>
            </tr>
            <tr class="trEdicionOpcion">
                <td>
                    <span><label>Calificación: </label></span>
                </td>
                <td>
                    <span><input type="text" id="txtOpcionCalificacionEditar" class="Calificacion floatValidate" value="" maxlength="5" onchange="validarCalificacionOpcion($(this))" /><label>%</label></span>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><span><input type="checkbox" id="chkSAGEditar" /></span></td>
                <td><span><label for="chkSAGEditar">S A G:</label></span></td>
            </tr>
        </table>
        <div id="divGeneral">
        </div>
        <input id="btnGuardarConfiguracionGrowing" class="Guardar" type="button" value="Guardar"
            onclick="guardarConfiguracionGrowing()" />&nbsp;&nbsp;<asp:Button runat="server" ID="btnHidden" OnClientClick="return false;" Style="position: absolute;
                            top: -50%;" meta:resourcekey="btnHiddenResource1" />
        <%--<input id="btnLimpiarConfiguracionGrowing" class="Limpiar" type="button" value="Limpiar" />--%>
        <asp:Button ID="btnCancelar" runat="server" meta:resourcekey="btnCancelarResource1" OnClick="btnCancelar_Click" Text="Limpiar" />
    </div>
</asp:Content>
