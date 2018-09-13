<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCategoriaEtapa.aspx.cs" Inherits="configuracion_frmCategoriaEtapa" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
<script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
<script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
<script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
<script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
<script type="text/javascript" id="variablesGlobales">
    objCategoriaEtapa = null;
</script>
<script type="text/javascript">
    $(function () {
        obtenerHabilidades();
        obtenerHabilidadesPlanta();
        obtenerHabilidadesIniciales();
    });



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
				     headers: { 2: { filter: false }
				     },
				     widgetOptions: {
				         zebra: ["gridView", "gridViewAlt"]
				         //filter_hideFilters: true // Autohide
				     }
				 })

            $(".tablesorter-filter.disabled").hide(); // hide disabled filters
        }
        else {
        }
    }

    function seleccionarTodosPisoAire() {
        $('input.checkbox').click(function () {
            var Checkbox = $(this);
            var idCategoria = $(Checkbox).attr('idCategoria');
            var id = $(Checkbox).attr('id');
            switch (id) {
                case 'TodosPISO':
                    if ($(Checkbox).is(':checked')) {
                        $('.gridView tr.Habilidad:not(.filtered) .radioPISO').prop('checked', true);
                        $('.gridView tr.Habilidad:not(.filtered)').attr('modificado', '1');
                        $('.gridView tr.Habilidad:not(.filtered)').attr('idCategoria', idCategoria);
                    } else {
                        $('.gridView tr.Habilidad:not(.filtered) .radioPISO').prop('checked', false);
                        $('.gridView tr.Habilidad:not(.filtered)').removeAttr('modificado');
                    }
                    break;

                case 'TodosAIRE':
                    if ($(Checkbox).is(':checked')) {
                        $('.gridView tr.Habilidad:not(.filtered) .radioAIRE').prop('checked', true);
                        $('.gridView tr.Habilidad:not(.filtered)').attr('modificado', '1');
                        $('.gridView tr.Habilidad:not(.filtered)').attr('idCategoria', idCategoria);
                    } else {
                        $('.gridView tr.Habilidad:not(.filtered) .radioAIRE').prop('checked', false);
                        $('.gridView tr.Habilidad:not(.filtered)').removeAttr('modificado');
                    }
                    break;
            }
        });

    }

    function obtenerHabilidades() {
        try {
            bloqueoDePantalla.bloquearPantalla();
            PageMethods.obtenerHabilidadesEtapas(function (response) {
                if (response[0] == '1') {
                    $('div.index').html(response[2]);
                    registerControls2();
                    seleccionarTodosPisoAire();
                    obtenerConfiguracion();
                    trModificado();
                    seleccionarRadios();
                    $('.gridView thead th:eq(2)').attr('class', 'sorter-false');
                    quitarSubrayadoCategoria();
                } else {
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                    popUpAlert(response[1], response[2]);
                }

            }, function (e) {
                bloqueoDePantalla.indicarTerminoDeTransaccion();
                console.log(e);
            });
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        } catch (e) {
            console.log(e);
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        }
        bloqueoDePantalla.desbloquearPantalla();
    }

    function obtenerHabilidadesPlanta() {
        $('[id*=ddlPlanta]').change(function () {
            var idPlanta = parseInt($(this).val());
            obtenerHabilidades();
        });
    }


    function obtenerHabilidadesIniciales() {
        var idPlantaActual = parseInt($('[id*="ddlPlanta"] option:selected').val());
        obtenerHabilidades();
    }

    function obtenerConfiguracion() {
        $('tr.Habilidad').each(function () {
            var Habilidad = $(this);
            var idCategoria = $(Habilidad).attr('idCategoria');
            switch (idCategoria) {
                case '1':
                    $(Habilidad).find('td').find('input.radioPISO').prop('checked', true);
                    break;

                case '2':
                    $(Habilidad).find('td').find('input.radioAIRE').prop('checked', true);
                    break;
            }
        });
    }

    function trModificado() {
        $('input[type="radio"]').click(function () {
            var Radio = $(this);
            var idCategoria = $(Radio).attr('idCategoria');
            $(Radio).parent().parent().attr('modificado', '1');
            $(Radio).parent().parent().attr('idCategoria', idCategoria);
        });
    }


    function quitarSubrayadoCategoria() {
        $('.gridView thead th:eq(2) .tablesorter-header-inner').hover(function () {
            $(this).css('text-decoration', 'none');
        });
    }

    function seleccionarRadios() {
        var i = 1;
        $('table  tr.Habilidad').each(function () {
            var radioPiso = $(this).find('td input.radioPISO');
            var radioAire = $(this).find('td input.radioAIRE');
            var labelPiso = $(this).find('td label.PISO');
            var labelAire = $(this).find('td label.AIRE');
            $(radioPiso).attr('id', 'r' + (i++) + '');
            $(radioAire).attr('id', 'r' + (i++) + '');
            var idPiso = $(radioPiso).attr('id');
            var idAire = $(radioAire).attr('id');
            $(labelPiso).attr('for', idPiso);
            $(labelAire).attr('for', idAire);
        });
    }

    function ObtenerobjCategoriaEtapa() {
        return objCategoriaEtapa = $('table.gridView tbody tr.Habilidad[modificado="1"]').map(function () {
            return {
                idEtapa: $(this).attr('idEtapa'),
                idCategoria: $(this).attr('idCategoria')
            }
        }).get();
    }

    function guardarConfiguracion() {
        try {
            bloqueoDePantalla.bloquearPantalla();
            PageMethods.guardarConfiguracion(ObtenerobjCategoriaEtapa(), function (response) {
                if (response[0] == '1') {
                    popUpAlert(response[1], response[2]);
                }
                else {
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                    popUpAlert(response[1], response[2]);
                }
            }, function (e) {
                bloqueoDePantalla.indicarTerminoDeTransaccion();
                console.log(e);
            });
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        } catch (e) {
            console.log(e);
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        }
        bloqueoDePantalla.desbloquearPantalla();
    }

 
</script>
<style type="text/css">
    .name
    {
        width: 100%;
        background: #a5c38e;
        margin-bottom: 2px;
        padding: 8px;
        border-radius: 10px;
        color: white;
        font-weight: bold;
        position: relative;
        font-size: 14px;
    }
    
    .name #imgDESC
    {
        position: absolute;
        right: 10px;
        cursor: pointer;
        bottom: 10px;
    }
    
    .name #imgASC
    {
        position: absolute;
        right: 10px;
        cursor: pointer;
        bottom: 2px;
    }
    
    #tblEtapasQuitar .trRow
    {
        background: blue;
    }
    
    .tblEtapas-container
    {
        background: #e4ead8;
        width: 100%;
        border-radius: 10px;
        padding-bottom: 3px;
        padding-top: 3px;
        padding-left: 16px;
        margin-bottom: 2px;
    }
    
    .tblEtapas-container
    {
        display: none;
    }
    
    .Arriba
    {
        display: none;
    }
    
    
    div.index table.gridView
    {
        width: 100%;
        text-align: center;
    }
    div.index table.gridView thead
    {
        display: block;
    }
    
    div.index table.gridView thead tr th:first-child, table.gridView tbody tr td:first-child
    {
        width: 250px !important;
        min-width: 250px !important;
    }
    
    div.index table.gridView thead tr th:nth-child(2), table.gridView tbody tr td:nth-child(2)
    {
        width: 200px !important;
        min-width: 200px !important;
    }
    
    div.index table.gridView thead tr th:nth-child(3), table.gridView tbody tr td:nth-child(3)
    {
        width: 350px !important;
        min-width: 350px !important;
    }
    
    div.index table.gridView tbody
    {
        display: block;
        width: 100%;
        height: 450px;
        overflow: auto;
        overflow-x: hidden;
    }
    
    @media screen and (max-width:1366px)
    {
        div.index table.gridView tbody
        {
            height: 250px;
        }
    }
    
    @media screen and (max-width:1024px)
    {
        div.index table.gridView tbody
        {
            height: 200px;
        }
    }
    
    
    .tablesorter-header-inner label
    {
        cursor: pointer;
    }
    
    tr.Habilidad.gridViewAlt label
    {
        cursor: pointer;
    }
    
    h2
    {
        margin-bottom: 20px;
    }
</style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Categoría Etapa</asp:Label>
        </h1>
        <h2>Selección de Categoría por Etapa</h2>
       <%-- <div id="divGeneral">
        </div>
        <input type="button" id="btnGuardar" value="Guardar" />
        <input type="button" id="btnLimpiar" value="Limpiar" />--%>
        <div class="index">
<%--           <table class="gridView" cellspacing="0">
              <thead>
                <tr>
                  <th>Habilidad</th>
                  <th>Etapa</th>
                  <th>
                    <label>Categoria</label><br>
                    <input type="checkbox" class="checkbox" id="TodosPiso" />
                    <label for="TodosPiso">Todos Piso</label>
                    <input type="checkbox" class="checkbox" id="TodosAire" />
                    <label for="TodosAire">Todos Aire</label>
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr class="Habilidad">
                  <td>Plantación</td>
                  <td>Etapa1</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox" />
                    <label for="chk2">Aire</label>
                  </td> 
                </tr>
                <tr class="Habilidad">
                  <td>Amarre de Planta</td>
                  <td>Etapa2</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox2" />
                    <label for="chk3">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox2" />
                    <label for="chk4">Aire</label>
                  </td>
                </tr>
                  <tr class="Habilidad">
                  <td>Colocación de Ganchos</td>
                  <td>Etapa1</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox3" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox3" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Colocación de Ganchos</td>
                  <td>Etapa2</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox4" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox4" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Desbrote Mediano</td>
                  <td>Etapa1</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox5" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox5" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Desbrote Mediano</td>
                  <td>Etapa2</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox6" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox6" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Abrir Planta</td>
                  <td>Etapa2</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox7" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox7" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Cosecha</td>
                  <td>Etapa1</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox8" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox8" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Cosecha</td>
                  <td>Etapa2</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox9" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox9" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Amarre de Tallo</td>
                  <td>Etapa1</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox10" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox10" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
                <tr class="Habilidad">
                  <td>Poda y Vuelta</td>
                  <td>Etapa1</td>
                  <td>
                    <input type="radio" class="radioPiso" id="chk1" name="checkbox11" />
                    <label for="chk1">Piso</label>
                    <input type="radio" class="radioAire" id="chk2" name="checkbox11" />
                    <label for="chk2">Aire</label>
                  </td>
                </tr>
              </tbody>
           </table>
           <input type="button" class="btnGuardarConfiguracion" value="Guardar Configuración" />--%>
        </div>
    </div>
</asp:Content>