<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmBrixCaptura.aspx.cs" Inherits="frmBrixCaptura" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
	<script src="../Scripts/jComparation.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
	<script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <link href="../comun/css/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../comun/CSS/ui-lightness/jquery-ui-1.8.21.custom.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>

	<script type="text/javascript">
	    var preview = false;
        var nivelP = 0
        $(function () {
            registerControls();
            triggers();

        });

        function ddlInvernaderoTrigger() {
            //window.console && console.log('trigger');
            $('#ddlInvernaderos').change(function () {
                //$("#<%=hddIdBrix.ClientID%>").val("0");
                $.blockUI();
                PageMethods.comboCosechas($(this).val(), function (result) {
                    $("#<%=divComboCosechas.ClientID %>").html(result);
                    $("#ddlCosechas").chosen({ no_results_text: "No se encontró el invernadero: ", width: '250px', placeholder_text_single: "--Seleccione un invernadero--" });
                    $.unblockUI();
                    ddlCosechasTrigger();
                });
            });
        }

        function ddlCosechasTrigger() {
            $("#ddlCosechas").change(function () {
                $('#btnRegresar').show();
                $("#<%=lblCaptura.ClientID %>").text($("#ddlInvernaderos option:selected").text() + " > " + $(this).find("option:selected").text());
                $("#gvPreharvest").hide();
                $("#gvSecciones").show();
                PageMethods.tablaSecciones($("#ddlInvernaderos").val(), $(this).val(), function (result) {
                    $("#<%=divTablaSecciones.ClientID %>").html(result);
                    $("#<%=divTablaSecciones.ClientID %>").show();
                    if ($("#tablaSecciones").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pager2"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaSecciones").tablesorter(
                                    { widthFixed: true, widgets: ['zebra', 'filter'], headers: { /*0: { filter: false} */
                                },
                                        widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */
                                        }
                                    }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                    }
                    else {
                        $("#pager").hide();
                    }
                    excel();
                });
            });
        }


	    function excel() {
	        $('.focus').change(function () {
	            //var ctrl = $(this);
	            //var id = $(this).attr('id').split('-')[1];
	            var fila = $(this).parent().parent(); //.split('-')[1];
	            var id = fila.attr('id'); //.split('-')[1];

	            var idcolor = $(this).attr('id').split('-')[2];
	            var promedio;

	            if (!$(this).hasClass('Error')) {

	                $("#brix-" + id + "-" + idcolor).addClass('change');


	                if ($("#brix-" + id + "-1").hasClass("change") || $("#brix-" + id + "-2").hasClass("change") || $("#brix-" + id + "-3").hasClass("change")) {
	                    fila.addClass("save");
	                }
	                else {
	                    fila.removeClass("save");
	                }

	                var sum = 0;
	                $.each($("." + id), function (indice, valor) {
	                    sum += parseFloat($(this).val());
	                    //console.log('Indice es ' + indice + ' y valor es: ' + $(this).val());
	                });
	                promedio = sum / $("." + id).length;


	                var red70 = parseFloat($("#brix-" + id + "-1").val());
	                var red80 = parseFloat($("#brix-" + id + "-2").val());
	                var mix = parseFloat($("#brix-" + id + "-3").val());
	                PageMethods.obtenerCalidad(red70, red80, mix, promedio, function (result) {
	                    if (result[2] < 0) {
	                        $("#brix-" + id + "-" + idcolor).val("0.00");
	                        //$("#promedio-" + id).text(result[2]);
	                        $("#calidad-" + id).text('---');
	                        $("#color-" + id).css('background-color', '#FFFFFF');
	                        $("#brix-" + id + "-" + idcolor).removeClass('change');
	                        popUpAlert("Excede valor de Brix.", "error");

	                    } else {
	                        $("#promedio-" + id).text(result[2]);
	                        $("#calidad-" + id).text(result[0]);
	                        $("#calidad-" + id).attr("idcalidad", result[3]);
	                        $("#color-" + id).css('background-color', result[1]);

	                        if ((mix >= 7.5 || promedio >= 7.5) && red70 < 7) {
	                            $("#coment-" + id).text("Rechecar RED 70. Dos muestras");
	                            $("#coment-" + id).addClass("help");
	                            $("#coment-" + id).attr("title", "Para obtener calidad CLUB red80 y Mix o el promedio deben ser mayor o iagual a 7.2");
	                        }
	                        else {
	                            if (red70 >= red80 && result[0] == "COMMODITY") {
	                                $("#coment-" + id).text("Rechecar RED 80. Dos muestras");
	                                $("#coment-" + id).addClass("help");
	                                $("#coment-" + id).attr("title", "Para obtener calidad NATURESWEET red80 debe ser mayo o igual a 7.3 <br />y Mix o el promedio deben ser mayor o iagual a 7.5");
	                            } else {
	                                $("#coment-" + id).text("");
	                                $("#coment-" + id).addClass("help");
	                                $("#coment-" + id).text("");
	                            }
	                            setTooltips();
	                        }

	                    }
	                });
	            }
	        });


	        var posi;
	        $('#<%=divTablaSecciones.ClientID %> td').click(function () {
	            var tdSelected = $(this);
	            if (posi != (tdSelected.parent().index() + '' + tdSelected.index())) {
	                posi = tdSelected.parent().index() + '' + tdSelected.index();
	                /*var tdValue = $(this).html();

	                if (tdValue.indexOf('-') > 0)
	                return;

	                if (tdValue.indexOf('input') > 0)
	                { $(tdSelected).find('input').focus(); }
	                else {
	                $(this).html('<input type="text" value="' + tdValue + '" maxlength="10" style="width:15px; float:left" />');
	                }*/

	                var inputCreated = $(tdSelected).find('input');
	                $(inputCreated).focus();
	                /*$(inputCreated).focusout(function () {
	                $(tdSelected).html($(this).val());
	                });*/
	                $(inputCreated).on('keydown', function (e) {

	                    switch (e.which) {
	                        case 37:
	                            var tdPadre = $(this).parent();
	                            tdPadre.prev().click();
	                            //tdPadre.prev().children().select();
	                            break; //Izquierda
	                        case 38:
	                            var tdPadre = $(this).parent();
	                            if (tdPadre.parent().prev().children().length) {
	                                tdPadre.parent().prev().children()[tdPadre.index()].click();
	                            }
	                            break; //Arriba
	                        case 39:
	                            var tdPadre = $(this).parent();
	                            tdPadre.next().click();
	                            break; //Derecha
	                        case 40:
	                            var tdPadre = $(this).parent();
	                            if (tdPadre.parent().next().children().length) {
	                                tdPadre.parent().next().children()[tdPadre.index()].click();
	                            }
	                            break; //Abajo
	                        default:

	                            break;
	                    }
	                });
	                ultimaCelda = $(inputCreated);
	            }
	        });
	    }


	    function showTabla(fila, idInvernadero, idActividad) {
	        $("#ddlInvernaderos > option[invernadero=" + $(fila).attr("invernadero") + "]").attr('selected', 'selected'); $("#ddlInvernaderos").trigger("chosen:updated");

	        PageMethods.comboCosechas($("#ddlInvernaderos").val(), function (result) {
	            $("#<%=divComboCosechas.ClientID %>").html(result);
	            $("#ddlCosechas").chosen({ no_results_text: "No se encontró el invernadero: ", width: '250px', placeholder_text_single: "--Seleccione un invernadero--" });
	            $("#ddlCosechas > option[value=" + idActividad + "]").attr('selected', 'selected'); $("#ddlCosechas").trigger("chosen:updated");
	            ddlCosechasTrigger();
	        });

	        $("#<%=hddIdBrix.ClientID%>").val(idActividad);
	        $('#btnRegresar').show();
	        $("#<%=lblCaptura.ClientID %>").text($(fila).attr("invernadero") + " > " + $(fila).attr("fecha"));
	        $("#gvPreharvest").hide();
	        $("#gvSecciones").show();
	        PageMethods.tablaSecciones(idInvernadero, idActividad, function (result) {
	            $("#<%=divTablaSecciones.ClientID %>").html(result);
	            $("#<%=divTablaSecciones.ClientID %>").show();
	            if ($("#tablaSecciones").find("tbody").find("tr").size() >= 1) {
	                var pagerOptions = { // Opciones para el  paginador
	                    container: $("#pager2"),
	                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
	                };

	                $("#tablaSecciones").tablesorter({
	                    widthFixed: true,
	                    widgets: ['zebra', 'filter'],
	                    headers: { 6: { filter: false }, 7: { filter: false} },
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
	            excel();
	        });
	    }

        function btnClean() {
            if ($('.selGuardaNuevo').length) {
                    popUpAlertConfirm('<h4>¿Existen relaciones sin guardar, desea limpiar?</h4>',
               '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();"><input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="cleanCallBack();">  ', 'warning');
                }
                else {
                    cleanCallBack();
                } 
            }


            function cleanCallBack() {
                //closeJsPopUpAux();
                $('.selGuardaNuevo').each(
                                function () {
                                    $("#" + $(this).val().split('|')[2]).attr('disabled', null);
                                    $("#label" + $(this).val().split('|')[2]).text('---');
                                }
                            );

                $(".imgSinguardar").parent().parent().remove();

                $('.gridView').each(
                                function () {
                                    $("#tablaSecciones").fadeOut(function () {
                                        $(this).remove();
                                    });
                                }
                            );

                $("#<%=divTablaSecciones.ClientID %>").hide();
                $("#pager2").hide();
                $("#ddlInvernaderos > option").eq(0).attr('selected', 'selected'); $("#ddlInvernaderos").trigger("chosen:updated");
                $("#ddlInvernaderos").change();
                $('#btnRegresar').hide();
                $("#<%=lblCaptura.ClientID %>").text("");
                $("#gvPreharvest").show();
                $("#gvSecciones").hide();
            }

            function btnSave() {
                if ($("#ddlCosechas").val() != 0) {
                    var libras = $(".libras").val();

                    if ($('.save').length) {
                        $.blockUI();
                        var ids = "";
                        var red70 = "";
                        var red80 = "";
                        var mix = "";
                        var calidad = "";

                        $('.save').each(
                        function () {
                            //var id = $(this).attr('id').split('-')[1];
                            var id = $(this).attr('id'); //.split('-')[1];
                            ids += '|' + id;
                            red70 += '|' + $("#brix-" + id + "-1").val();
                            red80 += '|' + $("#brix-" + id + "-2").val();
                            mix += '|' + $("#brix-" + id + "-3").val();
                            calidad += '|' + $("#calidad-" + id).attr("idcalidad");
                        }
                    );
                        PageMethods.guardaBrix(ids, red70, red80, mix, calidad, libras, $("#ddlInvernaderos").val(), $("#ddlCosechas").val(), $("#<%=hddIdBrix.ClientID%>").val(), saveCallback);
                    } else {
                        popUpAlert("No hay captura de brix que guardar", "info");
                    }
                }
                else {
                    popUpAlert("Debe seleccionar una fecha de Cosecha", "info");
                }
            }

            function saveCallback(result) {
                try {
                    PageMethods.gvCapturaBrix(function (result) {
                        $("#<%=divGridView.ClientID %>").html(result);
                        registerControls();
                    });

                    $.unblockUI();
                    $(".change").removeClass('change');
                    popUpAlert(result[1], result[0]);
                } catch (err) { }
            }

            function gvCallBack(result) {
                $("#<%=divGridView.ClientID %>").html(result);
                gvTablesorter();
            }

            function gvTablesorter() {
                //
                if ($("#gvAsociados").find("tbody").find("tr").size() >= 1) {
                    var pagerOptions = { // Opciones para el  paginador
                        container: $("#pager2"),
                        output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                    };

                    $("#gvAsociados")
				 .tablesorter({
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
                    $("#pager2").hide();
                }
            }


            function triggers() {

                $(".brix").live("click", function () {
                    $(this).select();
                });

                $('#ctl00_ddlPlanta').live('change', function () {
                    PageMethods.comboInvernaderos(function (result) {
                        $("#<%=divComboInvernaderos.ClientID %>").html(result);
                        $("#ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });
                        ddlInvernaderoTrigger();

                    });
                });

                $("#ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione--" });
                $("#ddlCosechas").chosen({ no_results_text: "No hay Cosechas en el invernadero: ", width: '250px', placeholder_text_single: "--Seleccione--" });
                ddlInvernaderoTrigger();

                gvTablesorter();

            }

            function setTooltips() {
                if ($(".help.tooltipstered").length) {
                    $('.help.tooltipstered').tooltipster("destroy");
                }

                $('.help').tooltipster({
                    animation: 'fade',
                    delay: 100,
                    theme: 'tooltipster-shadow',
                    touchDevices: true,
                    trigger: 'hover',
                    position: 'right',
                    contentAsHTML: true
                });
            }

    </script>

<script type="text/javascript">
</script>

<style type="text/css">
        input.Error {
        border: 1px solid red !important;
        background: rgba(255,0,0,0.2);
    }
    
    input.change {
        border: 1px solid #65AB1B !important;
        color: #FF8400;
        font-weight: bold;
        background:white;
    }
    
    .focus{
        border: transparent 1px solid !important;
        background: none;
        border-style: none;
        box-shadow: none !important;
    }
    
    .focus:focus{
        border: 1px black solid !important;
        background: white;
        border-style: none;
    }
    
    input[type="checkbox"], input[type="radio"]  {
        display:none;
    }
    
    input[type="checkbox"] + label span {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
        cursor:pointer;
    }
    
        input[type="radio"] + label span {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left -39px  top no-repeat;
        cursor:pointer;
    }
    
    input[type="checkbox"]:checked + label span{
        background:url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
    }
    
    input[type="radio"]:checked + label span {
        background:url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
    }
    
    input[type="checkbox"]:disabled + label span {
        /*background:none;*/
        background:url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
    }
    
    input[type="radio"]:disabled + label span {
        background:url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
    }
    
    .check-with-label:checked + .label-for-check {
        font-weight: bold;
        color:#C12929;
    }

    .check-with-label:disabled + .label-for-check {
        color:gray;
    }

    .left
    {
        text-align:left !important;
    }
    
    #ctl00_ContentPlaceHolder1_divPreview h3
    {
        float:none !important;
    }
    #ctl00_ContentPlaceHolder1_divPreview .ui-accordion-content {
        background: #E5EED2;
    }
    #ctl00_ContentPlaceHolder1_divPreview .ui-accordion {
        min-width: 95%;
        margin: 10px;
    }
    
    .container {
        /*display: block;*/
    }
    
    .grid
    {
        padding-top:0px;
    }
    
    table.index 
    {
        width:100%;
        max-width: 1000px !important;
        min-width: 1000px !important;
        padding-top: 0px;
    }
    
    table.index tr td table.gridView {
        min-width: inherit;
        max-width: inherit;
    }

    .pagedisplay {
        background: transparent !important;
     }
     
    img#btnRegresar
    {
        vertical-align: middle;
        margin-right: 15px;
        display:none;
    }
     

</style>    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
		<asp:ValidationSummary ID="validaciones" runat="server" ValidationGroup="valida" meta:resourcekey="validacionesResource1" />
		<h1><asp:Label ID="lblTitulo" runat="server" Text="Pre-Harvest" ></asp:Label></h1>
        <table class="index">
            <tr>
                <td align="left" colspan="4">
                    <h2><asp:Literal ID="ltSubTitulo" Text="Captura de Brix" runat="server"></asp:Literal></h2>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbRFamilia" runat="server" Text="<%/*$ Resources: radios*/ %>">*Invernaderos:</asp:Label>
                </td>
                <td id="divComboInvernaderos" runat="server" class="left">
                </td>
                <td>
                    <asp:Label ID="lblCosechas" runat="server" Text="<%/*$ Resources: cosechas*/ %>">*Cosechas:</asp:Label>
                </td>
                <td id="divComboCosechas" runat="server" class="left">
                </td>
            </tr>
            <tr>
                <td align="right" colspan="4">
                    <input id="save" class="btnSave" name="grupo" type="button" value="Guardar" onclick="btnSave();" />
                    <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();" />
                </td>
            </tr>
        </table>
        <h1>
            <img src="../comun/img/regresar.png" alt="Regresar" id="btnRegresar" onclick="btnClean();" />
            <asp:Label runat="server" ID="lblCaptura" Text=""></asp:Label>
        </h1>

        <div id="gvSecciones" class="grid" style="display: none;">
            <%--<input id="grupo" name="grupo" type="button" value="Agregar Grupo" onclick="addGrupo();"/>--%>
            <div id="pager2" class="pager" style="width: 100%; min-width: 100%;">
                <img alt="first" src="../comun/img/first.png" class="first" />
                <img alt="prev" src="../comun/img/prev.png" class="prev" />
                <span class="pagedisplay"></span>
                <img alt="next" src="../comun/img/next.png" class="next" />
                <img alt="last" src="../comun/img/last.png" class="last" />
                <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                    <option value="30">30</option>
                    <option value="40">40</option>
                    <option value="50">50</option>
                    <option value="60">60</option>
                    <option value="70">70</option>
                </select>
                <select class="gotoPage" title="Select page number">
                </select>
            </div>
            <div id="divTablaSecciones" runat="server" class="grid" />
        </div>

        <asp:HiddenField runat="server" Value="0" ID="hddIdBrix" />

        <div id="gvPreharvest" class="grid">
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
                <select class="gotoPage" title="Select page number">
                </select>
            </div>
            <div id="divGridView" runat="server" class="GridViewContainer gridView" />
        </div>
    </div>
    

    
</asp:Content>