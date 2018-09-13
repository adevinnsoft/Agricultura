<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmAsignacionLideres.aspx.cs" Inherits="frmAsignacionLideres" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
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
    <script type="text/javascript">
        var preview = false;
        var nivelP = 0;

        $(function () {
            //triggers();
            gvAsociadosLider();

            $('#ctl00_ddlPlanta').live('change', function () {
                gvAsociadosLider();
            });

            $("input.estados").change(function () {
                var box = $(this);
                var group = $("input.estados");
                $(group).prop("checked", false);
                $(box).prop("checked", true);

                //$('input.estados:checked').each(function () {
                var idEstado = $('input.estados:checked').attr('idEstado');
                if (idEstado == 0) {
                    $('#tablaLider tbody tr').removeClass('invisible').show();
                }
                else {
                    $('#tablaLider tbody tr').addClass('invisible');
                    $('#tablaLider tbody tr[estado="' + idEstado + '"]').removeClass('invisible').show();
                }
                //});
            });
        });



        function gvAsociadosLider() {
            try {
                $.blockUI();

                PageMethods.tablaLider(function (result) {
                    $("#<%=divAsociadosLider.ClientID %>").html(result);
                    $("#<%=divAsociadosLider.ClientID %>").show();
                    if ($("#tablaLider").find("tbody").find("tr").size() >= 1) {
                        var pagerOptions = { // Opciones para el  paginador
                            container: $("#pagerAsociadosLider"),
                            output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                        };

                        $("#tablaLider").tablesorter({
                            widthFixed: true, widgets: ['zebra', 'filter'],
                            headers: { 0: { filter: true }, 1: { filter: false }, 2: { filter: false }, 3: { filter: false }, 4: { filter: false }, 5: { filter: false }, 6: { filter: false }, 7: { filter: false }, 8: { filter: false }
                            },
                            widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                        }).tablesorterPager(pagerOptions);
                        $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                    }
                    else {
                        $("#pagerAsociadosLider").hide();
                    }
                    $.unblockUI();
                    tooltip();
                    asignacionGuardar();
                }, function (e) {
                    $.unblockUI();
                    console.log(e);
                });
            } catch (err) {
                console.log(err); $.unblockUI();
            }
        }


        function asignacionGuardar() {
            $("input:radio.gerenteLider").change(function () {
                //$.blockUI();
                var idLider = $(this).val().split('|')[0];
                var idGerente = $(this).val().split('|')[1];

                PageMethods.guardaRelacion(idLider, idGerente, function (result) {
                    try {
                        if (result[0] == "OK") {
                            $("#img_" + idLider + "_" + idGerente).fadeIn().fadeOut();
                        }
                        //$.unblockUI();
                    } catch (err) {
                        console.log(err); $.unblockUI();
                    }
                });
            });
        }


        function tooltip() {
            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: true,
                trigger: 'hover',
                position: 'right'
            });
        }


    </script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        input[type="checkbox"], input[type="radio"]
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
            /*background:none;*/
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
        }
        
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
            width: 89%;
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
        
        table.index tr td table.gridView
        {
            min-width: inherit;
            max-width: inherit;
        }
        
        .pagedisplay
        {
            background: transparent !important;
        }
        
        .imgSinguardar
        {
            background-image: url('../comun/img/smallinfo.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 18px;
            height: 18px;
        }
        
        .imgGuardado
        {
            background: url("../comun/img/smallcheck.png") no-repeat;
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 18px;
            height: 18px;
        }
        
        .grid
        {
            width: 100%;
            max-width: 100% !important;
            float: left; /*min-width: 800px !important;*/
        }
        
        .grid table
        {
            width: 100%;
        }
        
        table.index
        {
            width: 100%;
            max-width: 1000px !important;
            min-width: 1000px !important;
            padding-top: 0px;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <asp:validationsummary id="validaciones" runat="server" validationgroup="valida"
            meta:resourcekey="validacionesResource1" />
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Asignación de Lideres"></asp:label></h1>
        <table class="index">
            <tr>
                <td align="left" colspan="2">
                    <h2>
                        <asp:literal id="ltSubTitulo" text="Lista de Gerentes y líderes" runat="server"></asp:literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <div id="gvAsociadosLider" class="grid">
                        <div id="pagerAsociadosLider" class="pager" style="width: 100%; min-width: 100%;
                            display: none;">
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
                        <div id="divAsociadosLider" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:content>