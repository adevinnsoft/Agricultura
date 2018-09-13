<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TargetVariedad.aspx.cs" Inherits="configuracion_TargetVariedad" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script type="text/javascript" id="targetVariedad">

        $(function () {
            obtenerTargetVariedadInicial();
            seleccionarPlanta();
            validacionChange();
            $("#btnGuardarTargetVariedad").click(function () {
//                $('input.Target').each(function () {
//                    Target = $(this).val();
//                    if (/^[a-zA-Z]+$/.test(Target)) {
//                        popUpAlert("El valor de target " + Target + " no tienen el formato correcto.", "warning");
//                        $(this).focus();
//                        return false;
//                    } else {
//                        if (/^\d{1,24}(\.\d{1,12})?$/.test(Target)) {
//                            return true;
//                        } else {
//                            popUpAlert("El valor de target " + Target + " excede el número de dígitos permitidos.", "warning");
//                            $(this).focus();
//                            return false;
//                        }
//                    }
//                });
                guardarConfiguracion();
            });
        });


        function validacionChange() {
            $('input.Target').change(function () {
                Target = $(this).val();
                if (/^[a-zA-Z]+$/.test(Target)) {
                    popUpAlert("El valor de target " + Target + " no tienen el formato correcto.", "warning");
                    $(this).focus();
                    return false;
                } else {
                    if (/^\d{1,24}(\.\d{1,12})?$/.test(Target)) {
                        return true;
                    } else {
                        popUpAlert("El valor de target " + Target + " excede el número de dígitos permitidos.", "warning");
                        $(this).focus();
                        return false;
                    }
                }
            });
        }

        function obtenerTargetVariedadInicial() {
            var idPlantaActual = $('[id*="ddlPlanta"] option:selected').val();
            obtenerTargetVariedad(idPlantaActual);
        }


        function seleccionarPlanta() {
            $('[id*=ddlPlanta]').change(function () {
                var idPlanta = $(this).val();
                //obtenerTargetVariedad(idPlanta);
                obtenerTargetVariedad();
            });
        }

        function obtenerTargetVariedad() {
            PageMethods.obtenerTargetVariedad( function (response) {
                if (response[0] == '1') {
                    $('#tblTargetVariedad-container').html(response[2]);
                    agregarAtributo();
                    redondearTarget();
                    registerControls2();
                } else {
                    popUpAlert(response[1], response[2]);
                }
            });
        }


        function registerControls2() {
            if ($(".gridView").find("tbody").find("tr").size() >= 1) {
                var pagerOptions = { // Opciones para el  paginador
                    container: $("#pager"),
                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                };

                $(".gridView")
				 .tablesorter({
				     widthFixed: true,
				     widgets: ['zebra', 'filter'],
				     headers: { 5: { filter: false}
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
        } 

        function agregarAtributoCar() {
            $('input.Target').each(function () {
                if ($(this).val() != '') {
                    $(this).removeAttr('modificado');
                    $(this).attr('cargado', '1');
                    $(this).parents('tr.Producto').removeAttr('modificado');
                    $(this).parents('tr.Producto').attr('cargado', '1');
                }

            });
        }

        function agregarAtributo() {
            $('input.Target').change(function () {
                if ($(this).val() != '') {
                    if (parseInt($(this).val()) == 0) {
                    } else {
                        $(this).removeAttr('cargado');
                        $(this).attr('modificado', '1');
                        $(this).parents('tr.Producto').removeAttr('cargado');
                        $(this).parents('tr.Producto').attr('modificado', '1');
                     
                    }
                    
                }
            });
        }

        function generarJsonTargetVariedad() {
            try {
                return JSONTargetVariedad = $('#tblTargetVariedad tr.Producto:not([cargado="1"])').map(function () {
                    return {
                        idEtapa: $(this).attr('idEtapa'),
                        idVariedad: $(this).attr('idVariedad'),
                        idPlanta: $(this).attr('idPlanta'),
                        Target: $(this).find('input.Target:not([cargado="1"])').val(),
                        idTargetVariedad: $(this).attr('idTargetVariedad') == 0 ? 0 : $(this).attr('idTargetVariedad')
                    }
                }).get();
            } catch (e) {
               console.log(e);
            }
        }

        function redondearTarget() {
            $('input.Target').each(function () {
                var Target = parseFloat($(this).val());
                Target = (isNaN(Target) ? 0 : Target.toFixed(4));
                $(this).val(Target);
            });
        }

        function guardarConfiguracion() {
            bloqueoDePantalla.bloquearPantalla();
            try {
                PageMethods.guardarConfiguracion(generarJsonTargetVariedad(), function (response) {
                    if (response[0] == '1') {
                        popUpAlert(response[1], response[2]);
                        agregarAtributoCar();

                        obtenerTargetVariedad();
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                    } else {
                        bloqueoDePantalla.indicarTerminoDeTransaccion();
                        popUpAlert(response[1], response[2]);

                    }
                }, function (e) {
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                    console.log(e);
                });
            } catch (e) {
                console.log(e);
                bloqueoDePantalla.indicarTerminoDeTransaccion();
            }
            bloqueoDePantalla.desbloquearPantalla();
        }
    </script>
    <style type="text/css">
        input.Target
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
       <h1><asp:Label ID="lblTitulo" runat="server" Text="Target Variedad" ></asp:Label></h1>
       <h2>Capture y modifique Target por Variedad</h2>
       <br />
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
       <div id="tblTargetVariedad-container">
       </div>
       <div id="btn-container">
          <input type="button" class="button" id="btnGuardarTargetVariedad" value="Guardar" />
       </div>
    </div>
</asp:Content>