<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmHabilidad.aspx.cs" Inherits="configuracion_Habilidad"
    MasterPageFile="~/MasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css" id="disenoParticular">
        .etapaGenerales th{ cursor:pointer; color:#000; background:url('../comun/img/accordionBg.png') repeat-x; border-collapse:collapse; padding:5px 0;}
        .etapaGenerales td{ background:#F0F5E5; text-align:left;}
        .imgHabilidad
        {
            width: 200px;
            height: 200px;
            text-align: center;
            display: table;
            margin: 0 auto;
            font-size: 80px;
            border-radius: 15px;
            border-width: 20px;
            border-style: solid;
            border-color: #F0F5E5;
            margin-right:50px;
        }
        .style1
        {
            width: 120px;
        }
        #lblImgHabilidad
        {
            position: relative;
            top: 25%;
        }
        #lblNmbHabilidad
        {
            display: block;
            font-size: 18px;
            margin-top: 50px;
            text-align: center;
        }
        #HabilidadGuardadas tr:hover
        {
            cursor:pointer;
        }
    </style>
    <style type="text/css" id="popUp">
        div.popUpWMP
        {
            position: absolute;
            width: 65%;
            height: 65%;
            background: white;
            z-index: 9999;
            overflow: hidden;
            border: 1px solid #cccccc;
            -moz-box-shadow: 0 0 9px #999999;
            -webkit-box-shadow: 0 0 9px #999999;
            box-shadow: 0 0 9px #999999;
            display: none;
        }
        div.popUpHeader
        {
            background: #000080;
            height: 42px;
            width: 100%;
            color:yellow;
        }
        div.popUpContenido
        {
            padding: 5px;
            max-height: 80%;
            overflow: auto;
            width: 98%;
        }
        div.popUpBotones
        {
            width: 100%;
            height: 40px;
            bottom: 0px;
            position: absolute;
            background: #F4D101;
        }
        .accordionHeader
        {
            width: 100%;
            background: #ADC995;
            padding-top: 5px;
            padding-bottom: 5px;
            margin-top: 3px;
        }
        div.etapa
        {
            text-align: center;
            font-size: 12px;
            margin-top: 0px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
            width: 800px;
            max-width: 800px;
            min-width: 800px;
           
        }
        .configuracionAdicionalDeEtapa { border-left:1px solid #ccc; border-right:1px solid #ccc; border-bottom:1px solid #ccc; height:auto;}
        .etapaGenerales
        {
            width: 100%;
            background: #ADC995;
            border:none;
            border-collapse:collapse;
        }
        #divEtapas input
        {
            max-width: 100px;
            margin:3px 2px;
        }
        #divEtapas select  
        {
            max-width: 100px;
            margin:3px 2px;}
        .configuacionPorProducto div
        {
            display: table-cell;
            border: 1px dashed #D2CFCF;
            width: 50%;
        }
        .porcenajesDeIncremento
        {
            display: inline-block;
        }
        .configuacionPorProducto div.target {padding:0; margin:5px;}
        .configuacionPorProducto div.materiales {padding:0; margin:5px;}
        .configuacionPorProducto div.materiales table.index5 {margin:5px;width:97%;}
        .configuacionPorProducto div.targetPorProducto {padding:0; background:#f1f1f1; display:block;}
        .configuacionPorProducto div.targetPorProducto h5 {margin:0px; padding:0px 5px; width:343px; font-size:11px; text-align:right; float:left;}
        .configuacionPorProducto div.targetPorProducto h3 {margin:5px;}
        .configuacionPorProducto div.materiales h3 {margin:5px;}
        .configuacionPorProducto div
        {
            border: 1px dashed #fff;
            width: 388px;
            display:table-cell;
            text-align: left;
            min-height: 84px;
        }
        .index5
        {
            text-align: center;
            border: 1px solid #adc995;
            background: #f0f5e5;
            font-size: 12px;
            margin-top: 0px;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
            width: 100%;
        }
        .invisible
        {
            display: none;
        }
        .porcentaje input
        {
            width:30px;
            }
            span.porcentaje
        {
            display: inline-block;
            width:72px;
        }
        span.porcentaje label {display:inline-block; margin:10px 3px; width:20px; text-align:right;}
    </style>
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
     <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" id="Globales">
     </script>
    <script type="text/javascript" id="funcionalidadInicial">

        function cargarGridHabilidades() {
            PageMethods.HabilidadesGuardadas(function (response) {
                $('#HabilidadGuardadas').trigger('destroy');
                $('#HabilidadGuardadas tbody').html(response);
                $('#HabilidadGuardadas').trigger('destroyPager');
                window.setTimeout(function () {
                    if ($("#HabilidadGuardadas").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pager"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };
                        $("#HabilidadGuardadas").tablesorter({
                            widthFixed: true,
                            widgets: ['zebra', 'filter'],
                            headers: { 3: { filter: false }
                            },
                            widgetOptions: {
                                zebra: ["gridView", "gridViewAlt"]
                                //filter_hideFilters: true // Autohide
                            }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                    }
                    else {
                        $("#pager").hide();
                    }
                }, 1500);
            });
        }

        $(function () {
            $('input[type="checkbox"]').attr('checked', true);

            var ln = 1;

            triggers();

            $('#ctl00_ddlPlanta').change(function () {
                $('#<%=hdnPlanta.ClientID %>').val($('#ctl00_ddlPlanta').val());
            });

            $('#<%=hdnPlanta.ClientID %>').val($('#ctl00_ddlPlanta').val());
        });

        function triggers() {
            $('#ctl00_ContentPlaceHolder1_txtColorP').change(function () {
                setColor();
            });

            $('#lblImgHabilidad').click(function () {
//                if ($('#<%=hdnIdHabilidad.ClientID%>').val().length == 0){ // cambiar nombre corto solo habilidades nuevas
                    if (window.ln == 1) {
                        popUpAlert("<input type=\"text\" maxlength=\"3\"  id=\"txtlbl\" onChange=\"$('#lblImgHabilidad').text($(this).val().substr(0,3).toUpperCase()); $('#<%=hdnNombreCorto.ClientID %>').val($(this).val().substr(0,3).toUpperCase()); \" />", 'info');
                    }
                    else {
                        popUpAlert("<input type=\"text\" maxlength=\"3\" id=\"txtlbl\" onChange=\"$('#lblImgHabilidad').text($(this).val().substr(0,3).toUpperCase()); $('#<%=hdnNombreCorto_EN.ClientID %>').val($(this).val().substr(0,3).toUpperCase()); \" />", 'info');
                    }
//                }
            });

            $('#<%=txtNameHabilidad.ClientID %>').change(function () {
                window.ln = 1;
                $('#<%=hdnNombreCorto.ClientID %>').val(setImgLbl($('#<%=txtNameHabilidad.ClientID %>')));
                $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad.ClientID %>').val());
            });

            $('#<%=txtNameHabilidad_EN.ClientID %>').change(function () {
                window.ln = 2;
                $('#<%=hdnNombreCorto_EN.ClientID %>').val(setImgLbl($('#<%=txtNameHabilidad_EN.ClientID %>')));
                $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad_EN.ClientID %>').val());
            });

            $('#<%=txtNameHabilidad.ClientID %>').click(function () {
                window.ln = 1;
                $('#lblImgHabilidad').text($('#<%=hdnNombreCorto.ClientID %>').val());
                $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad.ClientID %>').val());
            });

            $('#<%=txtNameHabilidad_EN.ClientID %>').click(function () {
                window.ln = 2;
                $('#lblImgHabilidad').text($('#<%=hdnNombreCorto_EN.ClientID %>').val());
                $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad_EN.ClientID %>').val());
            });

            if ($('#ctl00_ContentPlaceHolder1_txtColorP').value != '') {
                setImgLbl($('#<%=txtNameHabilidad.ClientID %>'));
            }

            if ($('#<%=txtNameHabilidad.ClientID %>').value != '' && $('#<%=hdnNombreCorto.ClientID %>').value == '') {
                setImgLbl($('#<%=txtNameHabilidad.ClientID %>'));
            }

            if ($('#<%=hdnNombreCorto.ClientID %>').value != '') {
                $('#lblImgHabilidad').text($('#<%=hdnNombreCorto.ClientID %>').val());
                $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad.ClientID %>').val());
                setColor();
            }

            if ($('#<%=txtNameHabilidad_EN.ClientID %>').value != '' && $('#<%=hdnNombreCorto_EN.ClientID %>').value == '') {
                setImgLbl($('#<%=txtNameHabilidad_EN.ClientID %>'));
            }

            if ($('#<%=hdnNombreCorto_EN.ClientID %>').value != '') {
                $('#lblImgHabilidad').text($('#<%=hdnNombreCorto_EN.ClientID %>').val());
                $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad_EN.ClientID %>').val());
            }
        }

        function getHexValue(num) {
            var hex = num.toString(16);
            if (hex.length == 1) {
                hex = '0' + hex;
            }
            return hex;
        }

        function getDecValue(hex) {
            return parseInt(hex, 16);
        }

        function setImgLbl(a) {
            var text = a.val();
            var text2 = text.split(' ');
            var abrev = '';

            if (text2.length == 3) {
                for (var i = 0; i < text2.length; i++) {
                    abrev = abrev + text2[i].substr(0, 1);
                }
            } else if (text2.length > 1) {
                abrev = abrev + text2[0].substr(0, 1);
                abrev = abrev + text2[text2.length - 1].substr(0, 1);
            } else {
                abrev = text.substr(0, 2);
            }

            $('#lblImgHabilidad').text(abrev.toUpperCase());

            return abrev.toUpperCase();
        }

        function setColor() {
            var txtcolor = new jscolor.color(document.getElementById('<%=txtColorP.ClientID %>'));
            var color = txtcolor.toString();

            var bcolor1 = getDecValue(color.substr(0, 2));
            var bcolor2 = getDecValue(color.substr(2, 2));
            var bcolor3 = getDecValue(color.substr(4, 2));

            bcolor1 = (bcolor1 - 30) > 0 ? (bcolor1 - 30) : 0;
            bcolor2 = (bcolor2 - 30) > 0 ? (bcolor2 - 30) : 0;
            bcolor3 = (bcolor3 - 30) > 0 ? (bcolor3 - 30) : 0;

            $('.imgHabilidad').attr('style', 'background-color:#' + color + '; border-color:#' + getHexValue(bcolor1) + getHexValue(bcolor2) + getHexValue(bcolor3) + ';');
            $('.imgHabilidad').children().attr('style', 'background-color:#' + color + ';');
        }

        
        
    </script>
    <script id="CargaDeHabilidades" type="text/javascript">
        var ability = null;
        function cargarFormularioDeHabilidad(idHabilidad) {
            limpiarFormulario();
            PageMethods.obtenerHabilidadPorId(idHabilidad, function (Habilidad) {
                ability = $.parseJSON(Habilidad);
                if (ability != null) {
                    try {
                        $('#<%=hdnIdHabilidad.ClientID%>').val(idHabilidad);
                        $('#<%=chkActive.ClientID%>').attr('checked', ability.activo);
                        $('#<%=ddlPlanta.ClientID%> input').removeAttr('checked').attr('readonly', 'readonly').hide();
                        $('#<%=ddlPlanta.ClientID%> [idPlanta="' + ability.plantas.pop() + '"] input').attr('checked', true).attr('disabled', true).show();
                        $('#<%=ddlDepartment.ClientID%> input').removeAttr('checked').attr('readonly', 'readonly').hide();
                        $('#<%=ddlDepartment.ClientID%> [idDepartamento=' + ability.departamentos.pop() + '] input').attr('checked', true).attr('disabled', true).show();
                        $('#<%=txtNameHabilidad.ClientID%>').val(ability.nombreActividad);
                        $('#<%=txtNameHabilidad_EN.ClientID%>').val(ability.nombreActividadEN);
                        $('#<%=txtColorP.ClientID%>').val(ability.color).css({ 'background': '#' + ability.color, 'color': '#' + ability.color });
                        $('#<%=chkEjecutable.ClientID%>').attr('checked', ability.ejecutable);
                        $('#<%=hdnNombreCorto.ClientID%>').val(ability.codigo).text(ability.codigo);
                        $('#<%=hdnNombreCorto_EN.ClientID%>').val(ability.codigoEN).text(ability.codigoEN);
                        $('#txtNumeroDeEtapas').val(ability.niveles.length).change();
                        $('div.etapa').each(function (indiceEtapa, elemento) {
                            $(this).find('.txtEtapasEs').val(ability.niveles[indiceEtapa].nombre);
                            $(this).find('.txtEtapaEn').val(ability.niveles[indiceEtapa].nombreEN);
                            var familia = $(this).find('.slNivel option[value=' + ability.niveles[indiceEtapa].idNivel + ']').attr('idFamilia');
                            $(this).find('.slFamilia option[value=' + familia + ']').attr('selected', true);
                            $(this).find('.slFamilia').change();
                            $(this).find('.slNivel option[value=' + ability.niveles[indiceEtapa].idNivel + ']').attr('selected', true);
                            $(this).find('.txtElemento').val(ability.niveles[indiceEtapa].elemento);
                            $(this).find('.txtElementoEN').val(ability.niveles[indiceEtapa].elementoEN);
                            $(this).find('.txtNumeroElemento ').val(ability.niveles[indiceEtapa].targetXproducto[0].porcentajesPorElemento == null ? '' : ability.niveles[indiceEtapa].targetXproducto[0].porcentajesPorElemento.length).change();
                            $(this).find('.chkActivo').attr('checked', ability.niveles[indiceEtapa].activo);
                            if (ability.niveles[indiceEtapa].targetXproducto != null) {
                                for (var i = 0; i < ability.niveles[indiceEtapa].targetXproducto.length; i++) {
                                    var idProducto = ability.niveles[indiceEtapa].targetXproducto[i].idProducto;
                                    var target = ability.niveles[indiceEtapa].targetXproducto[i].target;
                                    var porcentajes = ability.niveles[indiceEtapa].targetXproducto[i].porcentajesPorElemento;
                                    $(this).find('.targetPorProducto [idProducto=' + idProducto + ']').parent().find('input').first().val(target);
                                    $(this).find('.targetPorProducto [idProducto=' + idProducto + '] ').parent().find('.porcentaje input').each(function (indxPorcentaje, elemento) {
                                        $(this).val(porcentajes[indxPorcentaje].porcentaje);
                                    });
                                }
                            }
                            if (ability.niveles[indiceEtapa].herramientasYMateriales != null) {
                                for (var i = 0; i < ability.niveles[indiceEtapa].herramientasYMateriales.length; i++) {
                                    var idMaterial = ability.niveles[indiceEtapa].herramientasYMateriales[i].idMaterial;
                                    var cantidad = ability.niveles[indiceEtapa].herramientasYMateriales[i].cantidad;
                                    cargaMateriales(this, idMaterial, cantidad);
                                }
                            }
                            $('#<%=txtNameHabilidad.ClientID %>').click()
                        });

                    }
                    catch (Ex) {
                        //console.log(Ex);
                    }
                    finally {
                        cargaImagen();
                        $('.configuracionAdicionalDeEtapa').slideUp();
                        $(window).scrollTop(0);
                        $('#botoneraHabilidadNueva').hide();
                        $('#botoneraHabilidadActualizada').show();
                        $('#btnActualizar').attr('onclick', 'guardarHabilidad(' + idHabilidad + ')');
                    }
                }
                else {
                    popUpAlert('No se pudo cargar la información de la habilidad', 'error');
                }

            });
        }

        function cargaImagen() {
            $('#lblImgHabilidad').text($('#<%=hdnNombreCorto.ClientID %>').val());
            $('#lblNmbHabilidad').text($('#<%=txtNameHabilidad.ClientID %>').val());
            // $('#<%=hdnNombreCorto.ClientID %>').Change();
            // $('#<%=txtNameHabilidad.ClientID %>').Change();
            setColor();
        }


        function cargaMateriales(etapaDiv, material, cantidad) {
            var table = $(etapaDiv).find('div div div .tblMateriales');// $('.tblMateriales');
            $(table).find('tbody').append(
                  $('#tblMateriales input:not([type="search"])').filter(function () {
                      if ($(this).attr('idmaterial') == material)
                          return $(this).val(cantidad)
                  }).parent().parent().clone()
             );

            var seen = {};
            $(table).find('tbody tr input[idMaterial]').each(function () {
                var idMaterial = $(this).attr('idMaterial');
                if (seen[idMaterial]) {
                    $(table).find('tbody tr input[idMaterial=' + idMaterial + ']').val(parseInt($(table).find('tbody tr input[idMaterial=' + idMaterial + ']').val()) + parseInt($(this).val()));
                    $(this).parent().parent().remove();
                }
                else
                    seen[idMaterial] = true;
                $(this).append($(this).val());
            });

            $(table).find('tbody .invisible').removeClass('invisible');
            $('#popUpHerrmaientasYMateriales input:not([type=button])').val('');
            $('#popUpHerrmaientasYMateriales input[type="search"]').change();

            //agregando filtro a tabla
            if ($(table).find("tbody").find("tr").size() >= 1) {
                $(table).trigger('destroy');
                $(table).tablesorter(
                    {
                        widthFixed: true,
                        widgets: ['zebra', 'filter'],
                        headers: { 4: { filter: false }
                        },
                        widgetOptions: {
                            zebra: ["gridView", "gridViewAlt"]
                        }
                    });
                $(".tablesorter-filter.disabled").hide(); // hide disabled filters
            }
        }


        function cancelarActualizacion() {
            limpiarFormulario();

            $('#<%=hdnIdHabilidad.ClientID%>').val('');
            $('#<%=ddlPlanta.ClientID%> input').attr('checked', true).removeAttr('disabled').show();
            $('#<%=ddlDepartment.ClientID%> input').attr('checked', true).removeAttr('disabled').show();
            $('#txtNumeroDeEtapas ').val(1).change();
            $('.porcentaje').remove();
            $('.configuracionAdicionalDeEtapa').slideDown();
            $(window).scrollTop(0);
            $('#botoneraHabilidadNueva').show();
            $('#botoneraHabilidadActualizada').hide();
            $('#btnActualizar').attr('onclick', '');
            $.unblockUI();
        }

        function limpiarFormulario() {
            $('#<%=hdnIdHabilidad.ClientID%>').val('');
            $('#incrementoGral').val('');
            $('#targetGral').val('');
            $('#<%=txtNameHabilidad.ClientID%>').val('');
            $('#<%=txtNameHabilidad_EN.ClientID%>').val('');
            $('#txtNumeroDeEtapas ').val(1).change();
            $('.txtNumeroElemento').val('');
            $('#<%=txtColorP.ClientID%>').val('FFFFFF').css({ 'background': '#FFFFFF', 'color': '#FFFFFF' });
            $('#<%=chkEjecutable.ClientID%>').attr('checked', true);
            $('#<%=hdnNombreCorto.ClientID%>').val('').text('');
            $('#<%=hdnNombreCorto_EN.ClientID%>').val('').text('');
            $('.txtEtapasEs').val('');
            $('.txtEtapaEn').val('');
            $('.slFamilia option[value=0]').attr('selected', true);
            $('.slFamilia').change();
            $('.txtElemento').val('');
            $('.txtElementoEN').val('');
            $('.targetPorProducto input').val('');
            $('.tblMateriales tbody tr').remove();
            cargaImagen();
        }
    </script>
    <script type="text/javascript" id="fnEtapas">
        var Familias = '';
        var Niveles = '';
        var TargetPorEtapa = '';
        var etapaCode = '';
        $(function () {

            //cargarGridHabilidades();
            PageMethods.HerramientasYMateriales(function (res) {
                $('#tblMateriales tbody').append(res);
                registerControls();
            });

            PageMethods.targetPorProductos(function (res) {
                TargetPorEtapa = res;
                PageMethods.familiasYNiveles(function (response) {
                    Familias = response[0];
                    Niveles = response[1];
                    etapaCode = ' <div class="etapa">                                                                                                        ' +
                       '    <table class="etapaGenerales">                                                                                                   ' +
                       '        <thead>                                                                                                                      ' +
                       '         <tr>                                                                                                                        ' +
                       '            <th>Etapa</th>                                                                                                           ' +
                       '            <th>Familia</th>                                                                                                         ' +
                       '            <th>Nivel</th>                                                                                                           ' +
                       '            <th>Elemento</th>                                                                                                        ' +
                       '            <th>Número de Elementos</th>                                                                                             ' +
                       '            <th>Activo</th>                                                                                                          ' +
                       '         </tr>                                                                                                                       ' +
                       '        </thead>                                                                                                                     ' +
                       '        <tbody>                                                                                                                      ' +
                       '         <tr>                                                                                                                        ' +
                       '            <td><input type="text" class="txtEtapasEs stringValidate" maxlength="40"/><span class="lengua">Español</span>            ' +
                       '            <br/><input type="text" class="txtEtapaEn stringValidate" maxlength="40"/><span class="lengua">Inglés</span> </td>       ' +
                       '            <td><select class="slFamilia">' + Familias + '</select></td>                                                             ' +
                       '            <td><select class="slNivel">' + Niveles + '</select></td>                                                                ' +
                       '            <td><input type="text" class="txtElemento" maxlength="40"/><span class="lengua">Español</span></br><input type="text" class="txtElementoEN" maxlength="40"/><span class="lengua">Inglés</span></br></td>' +
                       '            <td><input type="text" class="txtNumeroElemento intValidate" maxlength="2"/></td>                                        ' +
                       '            <td><input type="checkbox" class="chkActivo" checked/></td>                                                              ' +
                       '          </tr>                                                                                                                      ' +
                       '        </tbody>                                                                                                                     ' +
                       '    </table>                                                                                                                         ' +
                       '    <div class="configuracionAdicionalDeEtapa">                                                                                      ' +
                       '        <div class="configuacionPorProducto">                                                                                        ' +
                       '            <div class="target">                                                                                                     ' +
                       '               <span class="llenadoGral">                                                                                            ' +
                       '                  <label>Target General:</label><input type="text" id="targetGral" value="" class="intValidate"/><br>                                    ' +
                       '                  <label>Decremento:</label><input type="text" id="incrementoGral" value=""  class="intValidate"/><br>                                    ' +
                       '                  <input type="button" value="Autocompletar" onclick="llenarTarget($(this));"/><br> <br>                             ' +
                       '               </span>                                                                                                               ' +
                            TargetPorEtapa +
                       '            </div>                                                                                                                   ' +
                       '            <div class="materiales">                                                                                                 ' +
                       '                <h3>Herramientas y Materiales</h3>                                                                                   ' +
                       '                <input type="button" value="Agregar" class="btnPopMateriales" style="float:none" onclick="popUpMostrar($(this));" /> ' +
                       '                <table class="tblMateriales index5">                                                                                 ' +
                       '                    <thead><tr><th>Categoría</th><th>Articulo</th><th>Cantidad</th><th>Unidad</th></tr></thead>                      ' +
                       '                    <tbody></tbody>                                                                                                  ' +
                       '                </table>                                                                                                             ' +
                       '            </div>                                                                                                                   ' +
                       '        </div>                                                                                                                       ' +
                       '    </div>                                                                                                                           ' +
                       '</div>';
                    agregarEtapa();
                })
            });
        });


        $(function () {
            $('.slFamilia').live('change', function () {
                $(this).parent().parent().find('.slNivel option:not([idFamilia=' + $(this).find('option:selected').val() + '])').hide();
                $(this).parent().parent().find('.slNivel option[idFamilia=' + $(this).find('option:selected').val() + ']').show();
                $(this).parent().parent().find('.slNivel option[value=0]').show().attr('selected', 'selected');
            });
            $('#txtNumeroDeEtapas').change(function () {
                var etapasIndicadas = parseInt($('#txtNumeroDeEtapas').val());
                var etapasActuales = $('div.etapa').length;
                if (etapasIndicadas < 1) {
                    popUpAlert('La habilidad debe tener al menos una etapa', 'error');
                    $('#txtNumeroDeEtapas').val(1)
                }
                else {
                    if (etapasIndicadas > etapasActuales) {
                        for (var i = 0; i < (etapasIndicadas - etapasActuales); i++) {
                            agregarEtapa();
                        }
                    } else {
                        for (var i = 0; i < (etapasActuales - etapasIndicadas); i++) {
                            eliminarEtapa();
                        }
                    }
                }
            });
            $('.etapaGenerales th').live('click', function () {
                var table = $(this).parent().parent().parent();
                var content = $(table).next();
                $(content).slideToggle();
            });

            $('.txtNumeroElemento').live('change', function () {
                var elementosElegidos = parseInt($(this).val() == '' ? 0 : $(this).val());
                var Etapa = $(this).parent().parent().parent().parent().parent();
                var elementosActuales = $(Etapa).find('.porcenajesDeIncremento').first().find('input[type="text"]').length;
                if (elementosElegidos > elementosActuales) {
                    elementosActuales++;
                    for (var i = elementosActuales; i <= elementosElegidos; i++) {
                        $(Etapa).find('.porcenajesDeIncremento').append('<span class="porcentaje"><label>' + i + '</label><input type="text" class="intValidate" /><span>');
                    }
                } else {
                    for (var i = 0; i < (elementosActuales - elementosElegidos); i++) {
                        $(Etapa).find('.porcenajesDeIncremento').each(function () {
                            $(this).find('.porcentaje').last().remove();
                        });
                    }
                }
            });

        });
        function eliminarEtapa() { $('div.etapa').last().remove(); }
        function agregarEtapa() {
            $('#divEtapas').append(etapaCode);



              
        }
        
    </script>
    <script type="text/javascript" id="fnPopUpMateriales">
        var btnMaterialesPresionado = null;

        function llenarTarget(btn) {
            //console.log('click');
            //console.log($(btn).parent().parent().find('#targetGral').val());
            var target = $(btn).parent().parent().find('#targetGral').val();
            var incremento = $(btn).parent().parent().find('#incrementoGral').val();
            var inc = 0;

            $(btn).parent().parent().find('.txtTarget').val(target);
            $(btn).parent().parent().find('.porcenajesDeIncremento').each(function () {
                inc = 0;
                $(this).find('input').each(function () {
                    $(this).val(incremento * inc++);
                });
            });
        }

        function popUpMostrar(btn) {
            btnMaterialesPresionado = btn;
            $('#popUpHerrmaientasYMateriales').css({
                top: '50%',
                left: '50%',
                'margin-left': ($('#popUpHerrmaientasYMateriales').width() * -0.5) + 'px',
                'margin-top': ($('#popUpHerrmaientasYMateriales').height() * -0.5 + $(window).scrollTop()) + 'px'
            }).show();
        }

        function AgregarMateriales() {
            var table = $(btnMaterialesPresionado).next();
            $(table).find('tbody').append(
                $('#tblMateriales input:not([type="search"])').filter(function () {
                    if (this.value.length != 0)
                        return $(this);
                }).parent().parent().clone()
             );

            var seen = {};
            $(table).find('tbody tr input[idMaterial]').each(function () {
                var idMaterial = $(this).attr('idMaterial');
                if (seen[idMaterial]) {
                    $(table).find('tbody tr input[idMaterial=' + idMaterial + ']').val(parseInt($(table).find('tbody tr input[idMaterial=' + idMaterial + ']').val()) + parseInt($(this).val()));
                    $(this).parent().parent().remove();
                }
                else
                    seen[idMaterial] = true;
                $(this).append($(this).val());
            });
            $(table).find('tbody .invisible').removeClass('invisible');
            $('#popUpHerrmaientasYMateriales input:not([type=button])').val('');
            $('#popUpHerrmaientasYMateriales input[type="search"]').change();

            //agregando filtro a tabla
            if ($(table).find("tbody").find("tr").size() >= 1) {
                $(table).trigger('destroy');
                $(table).tablesorter(
                    {
                        widthFixed: true,
                        widgets: ['zebra', 'filter'],
                        headers: { 4: { filter: false }
                    },
                    widgetOptions: {
                        zebra: ["gridView", "gridViewAlt"]
                    }
                }); 
                $(".tablesorter-filter.disabled").hide(); // hide disabled filters
            }
            
        }


        function llenarTabla() {
            $('.txtNumeroElemento').val(3).change();
            $('.slNivel option').last().show().attr('selected', true);
            $('input[type=text]').each(function () { $(this).val((Math.random()).toString().split('.')[1].substring(0, 4)); });
            $('.btnPopMateriales').each(function () {
                $(this).click();
                AgregarMateriales();
                $('#popUpHerrmaientasYMateriales').hide();
                $('.txtColorP').val('6392FF').change();
            });
        }

        function cerrarPopUpHerrmaientasYMateriales() {
            $('#popUpHerrmaientasYMateriales').hide();
            $('#popUpHerrmaientasYMateriales input:not([type=button])').val('');
            $('#popUpHerrmaientasYMateriales input[type="search"]').change();
        }
    </script>
    <script type="text/javascript" id="fnAlmacenado">
        function guardarHabilidad(idHabilidad) {

            $.blockUI();
            //////////////////////////////////////////// VALIDACIONES /////////////////////////////////////
            $('input[type="button"]').attr('disabled', 'disabled');
            var ErroresAntesDeGuardar = '';
            var plantas = $('#<%=ddlPlanta.ClientID%> input:checked').map(function () { return $(this).parent().attr('idPlanta'); }).get()
            if (plantas.length > 0) {
                $('#<%=ddlPlanta.ClientID%>').css('border', '0px');
            }
            else {
                ErroresAntesDeGuardar += 'Se debe seleccionar al menos una planta.';
                $('#<%=ddlPlanta.ClientID%>').css('border', '1px solid red');
            }
            var departamentos = $('#<%=ddlDepartment.ClientID%> input:checked').map(function () { return $(this).parent().attr('idDepartamento'); }).get()
            if (departamentos.length > 0) {
                $('#<%=ddlDepartment.ClientID%>').css('border', '0px');
            }
            else {
                ErroresAntesDeGuardar += 'Se debe seleccionar al menos un departamento.';
                $('#<%=ddlDepartment.ClientID%>').css('border', '1px solid red');
            }
            var nombreActividad = $('#<%=txtNameHabilidad.ClientID%>').val();
            if (nombreActividad != '') {
                $('#<%=txtNameHabilidad.ClientID%>').css('border', '1px solid black');
            }
            else {
                $('#<%=txtNameHabilidad.ClientID%>').css('border', '1px solid red');
                ErroresAntesDeGuardar += 'El nombre en español para la habilidad es requerido.';
            }
            var nombreActividad = $('#<%=txtNameHabilidad_EN.ClientID%>').val();
            if (nombreActividad != '') {
                $('#<%=txtNameHabilidad_EN.ClientID%>').css('border', '1px solid black');
            }
            else {
                ErroresAntesDeGuardar += 'El nombre en inglés para la habilidad es requerido.';
                $('#<%=txtNameHabilidad_EN.ClientID%>').css('border', '1px solid red');
            }
            $('.etapa').each(function () {
                var nombreEtapa = $(this).find('.txtEtapasEs').val();
                if (nombreEtapa != '') {
                    $(this).find('.txtEtapasEs').css('border', '1px solid black');
                }
                else {
                    ErroresAntesDeGuardar += 'No se escribió el nombre en español para una etapa.';
                    $(this).find('.txtEtapasEs').css('border', '1px solid red');
                }

                var nombreEtapa = $(this).find('.txtEtapaEn').val();
                if (nombreEtapa != '') {
                    $(this).find('.txtEtapaEn').css('border', '1px solid black');
                }
                else {
                    ErroresAntesDeGuardar += 'No se escribió el nombre en inglés para una etapa.';
                    $(this).find('.txtEtapaEn').css('border', '1px solid red');
                }
                var idNivel = $(this).find('.slNivel option:selected').val();
                if (idNivel != '0') {
                    $(this).find('.slNivel').css('border', '1px solid black');
                }
                else {
                    ErroresAntesDeGuardar += 'El nivel seleccionado no es válido.';
                    $(this).find('.slNivel').css('border', '1px solid red');
                }

                var elemento = $(this).find('.txtElemento').val();
                if ($(this).find('.txtNumeroElemento').val() != '') {
                    if (elemento != '') {
                        $(this).find('.txtElemento').css('border', '1px solid black');
                    }
                    else {
                        ErroresAntesDeGuardar += 'El nombre del elemento en español es requerido si se especifica un número de ellos.';
                        $(this).find('.txtElemento').css('border', '1px solid red');
                    }
                } else {
                    $(this).find('.txtElemento').css('border', '1px solid black');
                }
                var elemento = $(this).find('.txtElementoEN').val();
                if ($(this).find('.txtNumeroElemento').val() != '') {
                    if (elemento != '') {
                        $(this).find('.txtElementoEN').css('border', '1px solid black');
                    }
                    else {
                        ErroresAntesDeGuardar += 'El nombre del elemento en inglés es requerido si se especifica un número de ellos.';
                        $(this).find('.txtElementoEN').css('border', '1px solid red');
                    }
                } else {
                    $(this).find('.txtElementoEN').css('border', '1px solid black');
                }

            });

            $('.required').each(function () {
                if (!$.trim($(this).val()).length > 0) {
                    ErroresAntesDeGuardar += 'Campo requerido vacío. ';
                }

            });

            $('.etapa .intValidate:blank:not(.txtNumeroElemento )').css('border', '1px solid red')
            $('.etapa .intValidate:not(:blank):not(.txtNumeroElemento )').css('border', '1px solid black')


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (ErroresAntesDeGuardar == '') {
                var habilidad = {
                    idHabilidad: idHabilidad,
                    activo: $('#<%=chkActive.ClientID%>').prop('checked'),
                    plantas: $('#<%=ddlPlanta.ClientID%> input:checked').map(function () { return $(this).parent().attr('idPlanta'); }).get(),
                    departamentos: $('#<%=ddlDepartment.ClientID%> input:checked').map(function () { return $(this).parent().attr('idDepartamento'); }).get(),
                    nombreActividad: $('#<%=txtNameHabilidad.ClientID%>').val(),
                    nombreActividadEN: $('#<%=txtNameHabilidad_EN.ClientID%>').val(),
                    codigo: $('#<%=hdnNombreCorto.ClientID%>').val(),
                    codigoEN: $('#<%=hdnNombreCorto_EN.ClientID%>').val(),
                    color: $('#<%=txtColorP.ClientID%>').val(),
                    ejecutable: $('#<%=chkEjecutable.ClientID%>').prop('checked'),
                    niveles: $('.etapa').map(function () {
                        return nivel = {
                            nombre: $(this).find('.txtEtapasEs').val(),
                            nombreEN: $(this).find('.txtEtapaEn').val(),
                            idNivel: $(this).find('.slNivel option:selected').val(),
                            elemento: $(this).find('.txtElemento').val(),
                            elementoEN: $(this).find('.txtElementoEN').val(),
                            activo: $(this).find('.chkActivo').prop('checked'),
                            targetXproducto: $(this).find('.targetPorProducto').map(function () {
                                return target = {
                                    idProducto: $(this).find('.lblNombreProducto').attr('idProducto'),
                                    target: $(this).find('.lblTarget').next().val() != '' ? $(this).find('.lblTarget').next().val() : 0,
                                    porcentajesPorElemento: $(this).find('.porcentaje').map(function () {
                                        return PorcentajePorElemento = {
                                            cantidadDeElementos: $(this).find('label').text(),
                                            porcentaje: $(this).find('input').val() != '' ? $(this).find('input').val() : 0
                                        }
                                    }).get()
                                }
                            }).get(),
                            herramientasYMateriales: $(this).find('.tblMateriales input[idmaterial]').map(function () {
                                return HerramientasMateriales = {
                                    idMaterial: $(this).attr('idmaterial'),
                                    cantidad: $(this).val() != '' ? $(this).val() : 0
                                }
                            }).get()
                        }
                    }).get()
                }
                PageMethods.AlmacenarHabilidad(habilidad, function (response) {
                    popUpAlert(response.split('|')[0], response.split('|')[1]);
                    if (response.split('|')[1] == 'ok') {
                        limpiarFormulario();
                    }
                    cargarGridHabilidades();
                    $.unblockUI();
                });
            }
            else {
                popUpAlert('Campos requeridos en el formulario por llenar, se marcaron de color rojo.', 'error');
                $.unblockUI();
            }
            $('input[type="button"]').removeAttr('disabled');


        }

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnIdHabilidad" runat="server" />
                    <asp:HiddenField ID="hdnNombreCorto" runat="server" />
                    <asp:HiddenField ID="hdnNombreCorto_EN" runat="server" />
                    

                    <table class="index" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td colspan="4">
                                <h2><asp:Literal ID="ltSubtituli" meta:resourceKey="ltSubtituli" runat="server"></asp:Literal></h2>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Literal ID="ltActive" runat="server" meta:resourceKey="ltActive"></asp:Literal></td>
                            <td colspan="3" class="checkboxes"><asp:CheckBox ID="chkActive" runat="server" meta:resourceKey="chkActiveResource1" Checked="True" /></td>
                        </tr>
                        <tr>
                            <td>*<asp:Literal ID="ltPlanta" meta:resourceKey="ltPlanta" runat="server">Planta:</asp:Literal></td>
                            <td colspan="3" class="floatnone"><asp:CheckBoxList ID="ddlPlanta" CssClass="" runat="server" 
                                    RepeatDirection="Horizontal"></asp:CheckBoxList></td>
                        </tr>
                        <tr>
                            <td >*<asp:Literal ID="ltDepartment" meta:resourceKey="ltDepartment" runat="server"></asp:Literal></td>
                            <td colspan="3" class="floatnone"><asp:CheckBoxList ID="ddlDepartment" CssClass="" runat="server" 
                                    meta:resourceKey="ddlDepartmentResource1" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
                        </tr>
                         <tr>
                          <td colspan="2"></td>
                          <td rowspan="5" colspan="2" style="text-align: center;">
                                <div class="imgHabilidad" style="display: table; margin: 0 auto;">
                                    <label id="lblImgHabilidad">
                                    </label>
                                    <label id="lblNmbHabilidad"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; padding-top: 10px;">
                                *<asp:Literal ID="ltNameHabilidad" runat="server" meta:resourceKey="ltNameHabilidad"></asp:Literal>
                            </td>
                            <td style="text-align: left; vertical-align:top;">
                                <asp:TextBox ID="txtNameHabilidad" runat="server" meta:resourceKey="txtNameHabilidadResource1"
                                    CssClass="required"></asp:TextBox><asp:Label ID="lngSp" runat="server" meta:resourceKey="lngSp"
                                        CssClass="lengua"></asp:Label>
                                <br />
                                <br />
                                <asp:TextBox ID="txtNameHabilidad_EN" runat="server" meta:resourceKey="txtNameHabilidadResource1"
                                    CssClass="required"></asp:TextBox><asp:Label ID="lngEn" runat="server" meta:resourceKey="lngEn"
                                        CssClass="lengua"></asp:Label><asp:HiddenField ID="hdnPlanta" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>*Color:</td>
                            <td>
                                <asp:TextBox ID="txtColorP" runat="server" class="required color {pickerFaceColor:'transparent',pickerFace:3,pickerBorder:0,pickerInsetColor:'black'}"
                                    meta:resourceKey="txtColorPResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltEjecutable" runat="server" meta:resourceKey="ltEjecutable"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkEjecutable" runat="server" meta:resourceKey="chkActiveResource2" />
                            </td>
                        </tr>
                        <tr>
                            <td><label>Número de Etapas</label></td>
                            <td><input type="text" class="intValidate" id="txtNumeroDeEtapas" maxlength="1" value="1"/></td>
                        </tr>
                    </table>
                    <div id="divEtapas"></div>
                  <div>
                </div>
                    


                    <table width="800px" style="margin:0 auto;" cellpadding="0" cellspacing="0" border="0">
                     <tr>
                        <td id="botoneraHabilidadNueva">
                            <input type="button" id="btnGuardar" value="Guardar" onclick="guardarHabilidad(-1);" />
                            <input type="button" class="btnLimpiar" onclick="limpiarFormulario();" value="Limpiar" />
                        </td>
                         <td id="botoneraHabilidadActualizada" style="display:none;">
                            <input type="button" id="btnActualizar" value="Actualizar" />
                            <input type="button" class="btnCancelar" onclick="cancelarActualizacion();" value="Cancelar" />
                            <!-- input type="button" class="btnLimpiar" onclick="limpiarFormulario();" value="Limpiar" style="visibility:'hidden'"/ -->
                        </td>
                     </tr>
                    </table>




                    <script type="text/javascript">
                        //Sys.Application.add_load(triggers);
                        Sys.Application.add_load(function () { registerControls(); cargarGridHabilidades(); });
                    </script>

                    <div class="grid">
                        <div id="pager" class="pager">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                                <span class="pagedisplay"></span>
                            <img alt="next" src="../comun/img/next.png" class="next" />
                            <img alt="last" src="../comun/img/last.png" class="last" />
                            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                                <option value="10">10</option>
                                <option value="20">20</option>
                                <option value="30">30</option>
                                <option value="40">40</option>
                                <option value="50">50</option>
                            </select>
                        </div>
                        <div>
                            <table id="HabilidadGuardadas" >
                                <thead><tr><th>Activo</th><th>Habilidad</th><th>Clave</th><th>Color</th><th>Ejecutable</th><th>Planta</th><th>Departamento</th></thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <div id="popUpHerrmaientasYMateriales" class="popUpWMP">
            <div class="popUpHeader">
                <img src="../comun/img/remove-icon.png" alt="X" onclick="cerrarPopUpHerrmaientasYMateriales();" style=" margin-left: -536.5px;   margin-top: 247px;  float: right; margin: 10px; cursor: pointer;" />
            </div>
            <div class="popUpContenido" style="max-height: 80%;overflow: auto;">
            <!---->
                <h2>Herramientas y Materiales</h2>
                <table id="tblMateriales" class=" accordionContent gridView">
                    <thead><tr><th>Categoría</th><th>Material</th><th>Cantidad</th><th>Unidad</th></tr></thead>
                    <tbody></tbody>
                </table>        
            <!---->
            </div>
            <div class="popUpBotones">
                <input type="button" value="Aplicar" onclick="AgregarMateriales(); cerrarPopUpHerrmaientasYMateriales();"/>
                <input type="button" value="Cancelar" onclick="cerrarPopUpHerrmaientasYMateriales();" />
            </div>
        </div>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>
