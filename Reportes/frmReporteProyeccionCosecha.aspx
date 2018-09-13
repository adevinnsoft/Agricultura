<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmReporteProyeccionCosecha.aspx.cs" Inherits="frmReporteCumplimientoInvCosechados"
    meta:resourcekey="PageResource1" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <%--<script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>--%>
    <!--[if lt IE 9]>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <![endif]-->
    <!--[if gte IE 9]><!-->
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <!--<![endif]-->
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../comun/CSS/ui-lightness/jquery-ui-1.8.21.custom.css" />
    <script src="../comun/scripts/jsZip/jszip.js" type="text/javascript"></script>
    <link href="../comun/css/dx.common.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/dx.light.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/dx.all.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            triggers();
            //$('#ctl00_ddlPlanta').change(function () {
            //    //triggers();
            //    gvReporte();
            //});
        });

        function gvReporte() {
            try {
                $.blockUI();
                var dia = $(".datepicker").val();
                var pesoCaja = $('input#nPesoCaja').val();
                var equivalente = $('input#equivalente').val();
                var idPlanta = $('select#ctl00_ddlPlanta').val() == undefined ? 0 : $('select#ctl00_ddlPlanta').val();

                //if (variety != 0) {
                PageMethods.ObtenerReporte(dia, idPlanta, pesoCaja, equivalente, function (result) {
                    if (result[0] == 'ok') {


                        $("#gridContainer").dxDataGrid({
                            dataSource: JSON.parse(result[1]),
                            allowColumnResizing: false,
                            columnAutoWidth: true,
                            showColumnLines: false,
                            showRowLines: false,
                            rowAlternationEnabled: true,
                            showBorders: false,
                            rowTemplate: function (container, item) {

                                if (null == item.data.FechaActividad) {
                                    item.data.FechaActividad = "N/D";
                                }
                                if (null == item.data.FechaProgramo) {
                                    item.data.FechaProgramo = "N/D";
                                }
                                if (null == item.data.FechaEjecucion) {
                                    item.data.FechaEjecucion = "N/D";
                                }
                                var data = item.data, markup = "<tbody onmouseover='tooltip();' class='tblInv " + ((item.rowIndex % 2) ? 'dx-row-alt' : '') + "'>" +
                                            "<tr class='dx-row main-row'>" +
                                            "<td>" + data.invernadero + "</td>" +
                                            "<td>" + data.variedad + "</td>" +
                                            "<td>" + data.fechapreharvest + "</td>" +
					                        "<td>" + data.cajasProyeccion + "</td>" +
                                            "<td>" + data.libras + "</td>" +
					                        "<td>" + data.porcentajeNS + "</td>" +
                                            "<td>" + data.porcentajeCM + "</td>" +
                                            "<td>" + data.porcentajeMR + "</td>" +
                                            "<td>" + data.librasNS + "</td>" +
                                            "<td>" + data.librasCM + "</td>" +
                                            "<td>" + data.librasMR + "</td>" +
                                            "<td>" + data.cajasNS + "</td>" +
                                            "<td>" + data.cajasCM + "</td>" +
                                        "</tr>" +
                                    "</tbody>";
                                container.append(markup);
                                //$(".tblInv>.dx-row").css("text-align", "center");
                                //tooltip();
                            },
                            export: {
                                enabled: true,
                                fileName: "Reporte"
                            },
                            columnWidth: "auto",
                            columns: [
                                    { dataField: "invernadero", caption: "<%=GetLocalResourceObject("Invernadero")%>", alignment: "center" },
                                    { dataField: "variedad", caption: "<%=GetLocalResourceObject("Variedad")%>", alignment: "center" },
                                    { dataField: "fechapreharvest", caption: "<%=GetLocalResourceObject("FullPreharvest")%>", alignment: "center" },
                                    { dataField: "cajasProyeccion", caption: "<%=GetLocalResourceObject("Cajas")%>", alignment: "center" },
                                    { dataField: "libras", caption: "<%=GetLocalResourceObject("Libras")%>", alignment: "center", },
                                    { dataField: "porcentajeNS", caption: "<%=GetLocalResourceObject("NS")%>", alignment: "center", },
                                    { dataField: "porcentajeCM", caption: "<%=GetLocalResourceObject("Commodity")%>", alignment: "center" },
                                    { dataField: "porcentajeMR", caption: "<%=GetLocalResourceObject("Merma")%>", alignment: "center" },
                                    { dataField: "librasNS", caption: "<%=GetLocalResourceObject("LbsNs")%>", alignment: "center" },
                                    { dataField: "librasCM", caption: "<%=GetLocalResourceObject("LbsCo")%>", alignment: "center" },
                                    { dataField: "librasMR", caption: "<%=GetLocalResourceObject("LbsMerma")%>", alignment: "center" },
                                    { dataField: "cajasNS", caption: "<%=GetLocalResourceObject("CajasNs")%>", alignment: "center" },
                                    { dataField: "cajasCM", caption: "<%=GetLocalResourceObject("CajasCo")%>", alignment: "center" }

                            ],
                            summary:{
                                totalItems:[{
                                    column:"invernadero",
                                    summaryType: "count",
                                    customizeText: function (data) {
                                        return "TOTAL: "+ data.value;
                                    }
                                },
                                {
                                    column:"librasNS",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                },
                                {
                                    column:"librasCM",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                },
                                {
                                    column:"librasMR",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                },
                                {
                                    column:"cajasProyeccion",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                },
                                {
                                    column:"libras",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                },
                                {
                                    column:"porcentajeNS",
                                    summaryType: "avg",
                                    customizeText: function (data) {
                                        return data.value.toFixed(2) * 100 + "%";
                                    }
                                },
                                {
                                    column:"porcentajeCM",
                                    summaryType: "avg",
                                    customizeText: function (data) {
                                        return data.value.toFixed(2) * 100 + "%";
                                    }
                                },
                                {
                                    column:"porcentajeMR",
                                    summaryType: "avg",
                                    customizeText: function (data) {
                                        return data.value.toFixed(2) * 100 + "%";
                                    }
                                },
                                {
                                    column: "cajasNS",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                },
                                {
                                    column: "cajasCM",
                                    summaryType: "sum",
                                    customizeText: function (data) {
                                        return data.value;
                                    }
                                }
                                ]
                            },
                            //filterRow: {
                            //    visible: true,
                            //    applyFilter: "auto"
                            //},
                            paging: {
                                pageSize: 20
                            },
                            pager: {
                                showPageSizeSelector: true,
                                allowedPageSizes: [0, 20, 40, 60, 100],
                                showInfo: true
                            }
                            //,searchPanel: {
                            //    visible: true,
                            //    width: 240,
                            //    placeholder: "Search..."
                            //},
                            //headerFilter: {
                            //    visible: true
                            //}
                        });

                        $("#gCharts").css("display", "flex");
                        $("#gridContainer").css("display", "flex");

                      

                    } else {
                        popUpAlert(result[1], result[0]);
                        //setTimeout(function () { closeJsPopUp(); }, 3000);
                        $(".gridView").trigger("destroy");
                        $("#tblReporte").html("");
                        $("#gCharts").css("display", "none");
                        $("#gridContainer").css("display", "none");
                    }
                    $.unblockUI();
                    //$("td.help").on({ mouseover: function () { tooltip(); } });
                });
                //}
                //else {
                    //popUpAlert("Necesita especificar una variedad", "info");
                    //setTimeout(function () { closeJsPopUp(); }, 3000);
                    //$.unblockUI();
                //}
            } catch (err) {
                console.log(err);
                $.unblockUI();
            }
        }

     
        function triggers() {
           

            $("#btnObtenerReporte").click(function () {
                gvReporte();
            });

            $(".datepicker").val(new Date().format("yyyy-MM-dd"));
            $(".datepicker").datepicker("destroy");
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                buttonImage: "../comun/img/calendario.png",
                showOn: "both",
                dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
                dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                dayNamesShort: ["Dom", "Lun", "Mar", "Mier", "Jue", "Vie", "Sab"],
                monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                changeYear: true,
                changeMonth: true
            });

            //$(".datepicker").change(function () {
            //    gvReporte();
            //});


        }
       
    </script>
    <style type="text/css">
        #tblDatos tbody tr[id="filtros"]
        {
            display: none;
        }
        div#ui-datepicker-div
        {
            z-index: 99 !important;
        }
        
        .dx-row-alt
        {
            background: #D6DFD0;
        }
        .dx-header-row
        {
            border-top: 1px solid #adc995;
            border-bottom: 1px solid #adc995;
            border-left: none;
            border-right: none;
            background: #f0f5e5;
            color: #000000;
            font-weight: bold !important;
            font-family: ProximaNovaRgBold, ProximaNovaLtBold,ProximaNovaThBold, Arial, Helvetica,Sans serif !important;
            font-size: 12px !important;
            text-transform: uppercase;
            white-space: nowrap;
            text-align: center !important;
            color: #000000;
        }
        
        .tblInv tr:hover
        {
            background-color: #F1FFA3;
            cursor: pointer;
        }
        
        tr.dx-row.main-row
        {
            text-align: center !important;
        }
        
        .container
        {
            width: 1024px;
        }
        
        div#gridContainer
        {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
        }
        
        table.dx-datagrid-table.dx-datagrid-table-fixed tr td
        {
            padding: 5px;
        }
        .ui-datepicker-trigger img
        {
            top: 5px;
            cursor: pointer;
        }
        
        /* SALAS */
        
        div#gridContainer
        {
            width: 1170px;
            overflow-x: auto;
        }
    </style>
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server"  
                meta:resourcekey="lblTituloResource1"></asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index">
                <tr>
                    <td>
                        <span>
                            <asp:label id="lblTitulo2" runat="server" 
                            meta:resourcekey="lblTitulo2Resource1"></asp:label>
                        </span>
                    </td>
                    <td style="display: block; text-align: left;">
                        <span>
                            <input id="txtSemanaDesde" class="datepicker" type="text" style="width: 75px; float: none !important;"
                                readonly placeholder="YYYY-MM-DD" />
                        </span>
                    </td>
                     <td>
                        <span>
                            <asp:label id="Label1" runat="server" 
                             meta:resourcekey="Label1Resource1" ></asp:label>
                        </span>
                    </td>
                    <td style="display: block; text-align: left;">
                       <input id="nPesoCaja" type="number" style="width: 50px; float: none !important;"  value="13.0"/>
                   </td>
                    <td>
                        <span>
                            <asp:label id="Label2" runat="server" 
                            meta:resourcekey="Label2Resource1" ></asp:label>
                        </span>
                    </td>
                     <td style="display: block; text-align: left;">
                       <input id="equivalente" type="number" style="width: 50px; float: none !important;"  value="10.61" />
                   </td>
                    <td style="display: block; text-align: left;">
                        <div id="divCombo">
                        </div>
                    </td>
                    <td>
                        <input id="btnObtenerReporte" type="button" value="<%=GetLocalResourceObject("ObtenerReporte")%>" />
                    </td>
                </tr>
            </table>
        </div>
     
        <div id="gridContainer">
        </div>
     
    </div>
</asp:content>