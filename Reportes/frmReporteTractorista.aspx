<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteTractorista.aspx.cs" Inherits="frmReporteTractorista" EnableEventValidation="false" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script type="text/javascript">
        $(function () {
            setInterval(function () { gvReporte(); }, 60000);
            gvReporte();

            $('#ctl00_ddlPlanta').live('change', function () {
                gvReporte();
            });
        });

        function gvReporte() {
            try {
                //$.blockUI();

                PageMethods.tablaReporte(function (result) {
                    $("#<%=divReporte.ClientID %>").html(result);
                    $("#<%=divReporte.ClientID %>").show();

                    PageMethods.tablaHistoria(function (result) {
                        $("#<%=divHistoria.ClientID %>").html(result);
                        $("#<%=divHistoria.ClientID %>").show();

                        if ($("#tablaHistoria").find("tbody").find("tr").size() >= 1) {
                            var pagerOptions = { // Opciones para el  paginador
                                container: $("#pagerReporte"),
                                output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                            };

                            $("#tablaHistoria").tablesorter({
                                widthFixed: true, widgets: ['zebra', 'filter'],
                                headers: { 1: { filter: false },
                                2: { filter: false },
                                3: { filter: false },
                                4: { filter: false }
                                },
                                widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                            }).tablesorterPager(pagerOptions);
                            $(".tablesorter-filter.disabled").hide(); // hide disabled filters


                        }
                        else {
                            $("#pagerReporte").hide();
                        }
                    });

                    
                    //$.unblockUI();
                    tooltip();
                    //asignacionGuardar();
                }, function (e) {
                    //$.unblockUI();
                    console.log(e);
                });
            } catch (err) {
                console.log(err); //$.unblockUI();
            }
        }

        function tooltip() {
            $('.help').tooltipster({
                animation: 'fade',
                contentAsHTML: true,
                interactive: true,
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: true,
                trigger: 'hover',
                position: 'left'
            });
        }
    </script>
    <style type="text/css">
        .semaforo
        {
            width: 30px;
            height: 18px;
            /*margin-left: 30px;*/
            border: solid 1px black;
        }
        
        #tablaReporte .semaforo
        {
            margin-left: 29px;
        }
        
        #tablaReporte th
        {
            text-align: center;
        }
        
        table.gridView
        {
            max-width: 100% !important;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Monitoreo de Tractoristas"></asp:label>
        </h1>
        <table class="index">
            <tr>
                <td align="left" colspan="2">
                    <h2>
                        <asp:literal id="ltSubTitulo" text="Folios Completados" runat="server"></asp:literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <div id="gvReporte" class="grid">
                       
                        <div id="divReporte" runat="server" />
                    </div>
                </td>
            </tr>
        </table>

         <table class="index">
            <tr>
                <td align="left" colspan="2">
                    <h2>
                        <asp:literal id="Literal1" text="Reporte de Hoy" runat="server"></asp:literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <div id="Div1" class="grid">
                         <div id="pagerReporte" class="pager" style="width: 100%; min-width: 100%; display: none;">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                            <span class="pagedisplay"></span>
                            <img alt="next" src="../comun/img/next.png" class="next" />
                            <img alt="last" src="../comun/img/last.png" class="last" />
                            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                                <option value="20">20</option>
                                <option value="40">40</option>
                                <option value="60">60</option>
                                <option value="80">80</option>
                                <option value="100">100</option>
                            </select>
                            <select class="gotoPage" title="Select page number">
                            </select>
                        </div>
                        <div id="divHistoria" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <%--    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />--%>
</asp:content>
