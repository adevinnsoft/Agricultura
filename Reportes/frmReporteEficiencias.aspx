<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteEficiencias.aspx.cs" Inherits="frmReporteEficiencias" meta:resourcekey="PageResource1" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript" id="reporteEficiencia">
        var idLider = 0;

        $(function () {
            $('#txtSemanaDesde').val($("#ctl00_ltSemana").text());
            $('#txtSemanaHasta').val($("#ctl00_ltSemana").text());
            $('#txtAnioDesde').val(new Date().getFullYear());
            $('#txtAnioHasta').val(new Date().getFullYear());
            triggers();
            gvReporte(idLider);
            $('#ctl00_ddlPlanta').live('change', function () {
                triggers();
                gvReporte(idLider);
            });
        });

        function gvReporte(idlider) {
            try {
                //$.blockUI();

                var semanaDesde = $('#txtSemanaDesde').val().trim();
                var anioDesde = $('#txtAnioDesde').val().trim();
                var semanaHasta = $('#txtSemanaHasta').val().trim();
                var anioHasta = $('#txtAnioHasta').val().trim();

                if ((semanaDesde == '' || anioDesde == '') || (semanaDesde == '' && anioDesde == '')) {
                    popUpAlert('Porfavor, Ingrese una semana y/o año iniciales', 'warning');
                    return false;
                }
                else {
                    if ((/^[0-9]+$/.test(semanaDesde)) == false || (/^[0-9]+$/.test(anioDesde) == false)) {
                        popUpAlert('El valor de los campos debe ser numérico.', 'warning');
                        return false;
                    }
                }

                if ((semanaHasta == '' || anioHasta == '') || (semanaHasta == '' && anioHasta == '')) {
                    popUpAlert('Porfavor, Ingrese una semana y/o año finales', 'warning');
                    return false;
                }
                else {
                    if ((/^[0-9]+$/.test(semanaHasta)) == false || (/^[0-9]+$/.test(anioHasta) == false)) {
                        popUpAlert('El valor de los campos debe ser numérico.', 'warning');
                        return false;
                    }
                }

                PageMethods.ObtenerEficiencia(idlider, semanaDesde, anioDesde, semanaHasta, anioHasta, function (result) {
                    if (result[0] == '1') {
                        $(".gridView").trigger("destroy");
                        $('#tblEficiencia').html(result[2].split('@')[0]);

                        if ($("#tblEficiencia").find("tbody").find("tr").size() >= 1) {
                            var pagerOptions = { // Opciones para el  paginador
                                container: $("#pagerReporte"),
                                output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                            };

                            $("#tblEficiencia").tablesorter({
                                widthFixed: true, widgets: ['zebra', 'filter'],
                                headers: { /*1: { filter: false }, 2: { filter: false }, 3: { filter: false }, 4: { filter: false }*/
                                },
                                widgetOptions: { zebra: ["gridView", "gridViewAlt"] /*filter_hideFilters: true Autohide */ }
                            }).tablesorterPager(pagerOptions);
                            $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                            $('#tblEficienciaLider').html(result[2].split('@')[1]);

                        }
                        else {
                            $("#pagerReporte").hide();
                        }
                    } else {
                        popUpAlert(result[2], result[1]);
                        $(".gridView").trigger("destroy");
                        $('#tblEficiencia').html("");
                    }
                });
                //$.unblockUI();
                tooltip();
                //asignacionGuardar();
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

        function triggers() {
            PageMethods.comboLideres(function (result) {
                $("#divComboLideres").html(result);
                $("#ddlLideres").chosen({ no_results_text: "No se encontró ningun lider", width: '500px', placeholder_text_single: "--Seleccione un Lider--" });

                $("#ddlLideres").chosen().change(function () {
                    idLider = $("#ddlLideres").val();
                    gvReporte(idLider);
                });
            });

        }
       
    </script>
    <style type="text/css">
        #tblDatos tbody tr[id="filtros"]
        {
            display: none;
        }
        
        table.index2 input[type="text"]
        {
            text-align: center;
        }
        
        table#tblEficiencia
        {
            max-width: 900px;
        }
        table#tblDatos
        {
            text-align: right;
        }
    </style>
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="ReporteEficiencia"></asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index2">
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblTitulo2" runat="server" text="Reporte de Eficiencia de la semana">
                            </asp:label></span>
                    </td>
                    <td>
                        <span>
                            <input id="txtSemanaDesde" class="Texto" type="text" value="" placeholder="ww" />
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:label id="lblTitulo3" runat="server" text="y año"></asp:label>
                        </span>
                    </td>
                    <td>
                        <span>
                            <input id="txtAnioDesde" class="Texto" type="text" value="" placeholder="yyyy" />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblTitulo4" runat="server" text="a la semana"></asp:label>
                        </span>
                    </td>
                    <td>
                        <span>
                            <input id="txtSemanaHasta" class="Texto" type="text" value="" placeholder="ww" />
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:label id="lblTitulo5" runat="server" text="y año"></asp:label></span>
                    </td>
                    <td>
                        <span>
                            <input id="txtAnioHasta" class="Texto" type="text" value="" placeholder="yyyy" />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>
                            <asp:label id="lbLider" runat="server" text="Lider"></asp:label>
                        </span>
                    </td>
                    <td colspan="4">
                        <div id="divComboLideres">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <input id="btnObtenerReporteEficiencia" type="button" value="Obtener Reporte" onclick="$('#ddlLideres').chosen().change();" />
                    </td>
                </tr>
            </table>
        </div>
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
        <table class="gridView" id="tblEficiencia">
        <table class="gridView" id="tblEficienciaLider">
        </table>
    </div>
</asp:content>