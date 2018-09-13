<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmRecepcionPlantulas.aspx.cs" Inherits="frmRecepcionPlantulas" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
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
            triggers();

            $('#<%=chkEstado.ClientID %>').change(function () {
                $('#tablaRequerimientos tbody tr').addClass('invisible');
                $(this).find('input:checked').each(function () {
                    var idEstado = $(this).parent().attr('idEstado');
                    $('#tablaRequerimientos tbody tr[estado="' + idEstado + '"]').removeClass('invisible').show();
                });
            });

            $('#ctl00_ddlPlanta').live('change', function () {
                dibujaTabla();
            });

        });

        function dibujaTabla() {
            $.blockUI();
            PageMethods.obtieneRequerimeintos(function (result) {
                $("#<%=divTablaRequerimientos.ClientID %>").html(result);
                if ($("#tablaRequerimientos").find("tbody").find("tr").size() >= 1) {
                    $(".pager, .forma").show();
                    var pagerOptions = { // Opciones para el  paginador
                        container: $("#pager2"),
                        output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                    };

                    $("#tablaRequerimientos").tablesorter({
                        widthFixed: true,
                        widgets: ['zebra', 'filter'],
                        headers: { 8: { filter: false }, 9: { filter: false }, 10: { filter: false} },
                        widgetOptions: {
                            zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */
                        }
                    }).tablesorterPager(pagerOptions);
                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                }
                else {
                    $(".pager, .forma").hide();
                }
                $.unblockUI();
                excel();
                $('#<%=chkEstado.ClientID %>').change();
                $(".razones").chosen({ no_results_text: "No se encontró la razon: ", width: '200px', placeholder_text_single: "--Seleccione--" });

                $('.razones').each(
                        function () {
                            //$("#" + $(this).attr("id") + " > option").eq($(this).attr("vprev")).attr('selected', 'selected'); $("#" + $(this).attr("id")).trigger("chosen:updated");
                            $("#" + $(this).attr("id")).val($(this).attr("vprev")); $("#" + $(this).attr("id")).trigger("chosen:updated");
                        }
                    );

            });
        }

        function excel() {
            $(".enviados, .razones, .comentarios").change(function () {
                var fila = $(this).parent().parent(); //.split('-')[1];
                var id = fila.attr('id'); //.split('-')[1];

                //if (!$(this).hasClass('Error')) {

                if ($("#envio-" + id).val() != $("#envio-" + id).attr("vprev")) {
                    $("#envio-" + id).addClass("change");
                } else {
                    $("#envio-" + id).removeClass("change");
                }
                $("#merma-" + id).text(numberWithCommas(parseInt($("#cantidad-" + id).text().replace(/,/g, '') - $("#envio-" + id).val())));
                if ($("#merma-" + id).text() < 0) {
                    $("#envio-" + id).removeClass("change");
                    $("#envio-" + id).addClass("error");
                } else {
                    $("#envio-" + id).removeClass("error");
                }

                if ($("#razon-" + id).val() != $("#razon-" + id).attr("vprev")) {
                    $("#razon-" + id).addClass("change");
                } else {
                    $("#razon-" + id).removeClass("change");
                }

                if ($("#comentario-" + id).val() != $("#comentario-" + id).attr("vprev")) {
                    $("#comentario-" + id).addClass("change");
                } else {
                    $("#comentario-" + id).removeClass("change");
                }

                if ($("#envio-" + id).hasClass("change") || $("#razon-" + id).hasClass("change") || $("#comentario-" + id).hasClass("change")) {
                    if (!$("#envio-" + id).hasClass("error")) {
                        fila.addClass("save");
                    }
                }
                else {
                    fila.removeClass("save");
                }
                //}
                //setTooltips();
            });


            var posi;
            $('#<%=divTablaRequerimientos.ClientID %> td').on('click', function () {
                var tdSelected = $(this);
                if (posi != (tdSelected.parent().index() + '' + tdSelected.index())) {
                    posi = tdSelected.parent().index() + '' + tdSelected.index();

                    var inputCreated = $(tdSelected).find('input');
                    $(inputCreated).focus();

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
                                    //tdPadre.parent().prev().children()[tdPadre.index()].click();
                                    $(tdPadre.parent().prev().children()[tdPadre.index()]).children()[1].click();
                                }
                                break; //Arriba
                            case 39:
                                var tdPadre = $(this).parent();
                                tdPadre.next().click();
                                break; //Derecha
                            case 40:
                                var tdPadre = $(this).parent();
                                if (tdPadre.parent().next().children().length) {
                                    //tdPadre.parent().next().children()[tdPadre.index()].click();
                                    $(tdPadre.parent().next().children()[tdPadre.index()]).children()[1].click();
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

        function btnSave() {
            var registrar = true;
            $('.required').each(function () {
                if ($(this).val() == '') {
                    registrar = false;
                    $(this).css({ 'border': '1px solid red' });
                }
                else {
                    $(this).css({ 'border': '1px solid black' });
                }
            });

            if (registrar) {
                if ($('.save').length) {
                    $.blockUI();
                    var ids = "";
                    var plantulas = "";
                    var mermas = "";
                    var razones = "";
                    var observaciones = "";
                    $('.save').each(
                        function () {
                            var id = $(this).attr('id'); //.split('-')[1];
                            ids += '|' + id;
                            plantulas += '|' + $("#envio-" + id).val();
                            mermas += '|' + $("#merma-" + id).text().replace(/,/g, '');
                            razones += '|' + $("#razon-" + id).val();
                            observaciones += '|&nbsp;' + $("#comentario-" + id).val();
                        }
                    );
                        PageMethods.guardaEnvio(ids, plantulas, mermas, razones, observaciones, function (result) {
                            try {
                                $.unblockUI();
                                if (result[0] != "error") {
                                    $(".change").removeClass('change');
                                    dibujaTabla(); //$(".save").attr("estado", 2);
                                }
                                popUpAlert(result[1], result[0]);
                                dibujaTabla();
                            } catch (err) { }
                        });
                    } else {
                        if ($('.error').length) {
                            popUpAlert("No puede haber mermas negativas", "info");
                        }
                    else {
                        popUpAlert("No hay modificaciones para guardar", "info");
                    }
                }
            }
            else {
                popUpAlert('No se permiten valores vacios en el campo Recibido.', 'error');
            }
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function triggers() {
            dibujaTabla();

            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: false,
                trigger: 'hover',
                position: 'right'
            });


            $(".enviados").live("click", function () {
                $(this).select();
            });
        }

    </script>

    <style type="text/css">
        input.error
        {
            border: 1px solid red !important;
            background: rgba(255,0,0,0.2);
        }
        
        input.change, textarea.change, .change + .chosen-container .chosen-single
        {
            border: 1px solid #65AB1B !important;
            color: #FF8400;
            font-weight: bold;
            background: white;
        }
        
        .enviados
        {
            border: black 1px solid !important;
            background: white;
            border-style: none;
            box-shadow: none !important;
        }
        
        .enviados:focus
        {
            border: 1px black solid !important;
            background: white;
            border-style: none;
        }
        
        /*input[type="checkbox"], input[type="radio"]
        {
            display: none;
        }
        input[type="checkbox"] + label span
        {
            display: inline-block;
            width: 19px;
            height: 19px;
            margin: -1px 4px 0 0;
            vertical-align: middle;
            background: url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
            cursor: pointer;
        }
        
        input[type="radio"] + label span
        {
            display: inline-block;
            width: 19px;
            height: 19px;
            margin: -1px 4px 0 0;
            vertical-align: middle;
            background: url(../comun/img/check_radio_sheet.png) left -39px top no-repeat;
            cursor: pointer;
        }
        
        input[type="checkbox"]:checked + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
        }
        
        input[type="radio"]:checked + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
        }
        
        input[type="checkbox"]:disabled + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
        }
        
        input[type="radio"]:disabled + label span
        {
            background: url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
        }
        
        .check-with-label:checked + .label-for-check
        {
            font-weight: bold;
            color: #C12929;
        }
        
        .check-with-label:disabled + .label-for-check
        {
            color: gray;
        }*/
        
        .left
        {
            text-align: left !important;
        }
        
        #ctl00_ContentPlaceHolder1_divPreview h3
        {
            float: none !important;
        }
        #ctl00_ContentPlaceHolder1_divPreview .ui-accordion-content
        {
            background: #E5EED2;
        }
        #ctl00_ContentPlaceHolder1_divPreview .ui-accordion
        {
            min-width: 95%;
            margin: 10px;
        }
        
        .container
        {
            /*display: block;*/
        }
        
        table.index
        {
            width: 100%;
            max-width: 1000px !important;
            min-width: 1000px !important;
            padding-top: 0px;
        }
        
        table.index tr td table.gridView
        {
            min-width: inherit;
            max-width: inherit;
        }
        
        .pagedisplay
        {
            background: transparent !important;
        }
        
        .invisible
        {
            display: none !important;
        }
        
        .grid table tr td {
	        white-space: normal;
	        padding-left: 1px;
	        padding-right: 1px;
        }
        
        .tablesorter-header-inner
        {
            white-space: normal;
            }
        

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <%--<asp:ValidationSummary ID="validaciones" runat="server" ValidationGroup="valida"
            meta:resourcekey="validacionesResource1" />--%>
        <h1>
            <asp:Label ID="lblTitulo" runat="server" Text="Recepción de Plántulas"></asp:Label>
        </h1>
        <table class="index forma">
            <tr>
                <td colspan="4">
                    <asp:CheckBoxList runat="server" ID="chkEstado" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <input id="save" name="grupo" type="button" value="Guardar" onclick="btnSave();" />
                    <%--<input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();" />--%>
                </td>
            </tr>
        </table>

            <div id="gv" class="grid">
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
                <div id="divTablaRequerimientos" runat="server" class="grid" />
            </div>
            <asp:HiddenField runat="server" Value="0" ID="hddIdBrix" />

    </div>

    </div>

</asp:Content>
