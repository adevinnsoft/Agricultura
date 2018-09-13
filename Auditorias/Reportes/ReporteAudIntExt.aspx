<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReporteAudIntExt.aspx.cs" Inherits="Auditorias_Reportes_ReporteAudIntExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Reporte Anual de Auditorias</title>
 
        <!--[if gte IE 9]><!-->
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
        <!--<![endif]-->

        <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.0/knockout-min.js"></script>
        <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
        <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.chartjs.js"></script>
        <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.webappjs.js"></script>
        <script type="text/javascript" src="../scripts/jszip.js"></script>
        <script type="text/javascript" src="../scripts/jszip.min.js"></script>

        <link rel="stylesheet" type="text/css" href="http://cdn3.devexpress.com/jslib/15.2.10/css/dx.common.css" />
        <link rel="stylesheet" type="text/css" href="../CSS/dx.light.css" />

        <link href="../CSS/Style.css" rel="Stylesheet" type="text/css" />
        <link href="../CSS/comun.css" rel="Stylesheet" type="text/css" />
        <link href="../CSS/chosen.css" rel="Stylesheet" type="text/css" />
                    
        <link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="../CSS/ui-lightness/jquery-ui-1.8.21.custom.css" rel="stylesheet" type="text/css" />
    
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
        <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
        <style>
            @media print {
                body {
                    -webkit-print-color-adjust: exact;
                }
            }

            #tablaReporte table {
                border-collapse: collapse;
                width: 100%;
            }

            #tablaReporte th {
                padding: 8px;
                border-bottom: 2px solid #3c3b3b;
            }

            #tablaReporte td {
                padding: 8px;
                text-align: left;
                border-bottom: 2px solid #3c3b3b;
            }

            #tablaReporte tr:hover {background-color: #f5f5f5}

            .centro {
                text-align: center;
            }
        </style>

        <script type="text/javascript">
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker({
                    dateFormat: 'yy-mm-dd',
                    firstDay: 1
                });
             });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
            <asp:Label ID="lbl1" runat="server"></asp:Label>
            <div id="filtros" style="margin: 0px auto; width: 100%; height: auto;">
                <table style="margin:0px auto; width:100%;">
                    <tr>
                        <td class="centro">Semana: </td>
                        <td class="centro">
                            <asp:TextBox ID="txtFecha" runat="server" class="centro" OnTextChanged="txtFecha_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td class="centro">
                            <asp:TextBox ID="txtSemana" runat="server" class="centro" readonly="true"></asp:TextBox>
                        </td>
                        <td class="centro">
                            <asp:TextBox ID="txtFechaInicio" runat="server" class="centro" disabled="disabled"></asp:TextBox>
                        </td>
                        <td class="centro"> - </td>
                        <td class="centro">
                            <asp:TextBox ID="txtFechaFin" runat="server" class="centro" disabled="disabled"></asp:TextBox>
                        </td>
                        <td class="centro"> Planta: </td>
                        <td class="centro">
                            <asp:DropDownList ID="ddlPlant" runat="server"></asp:DropDownList>
                        </td>
                        <td class="centro">
                            <asp:Button ID="btnGenerar" OnClick="btnGenerar_Click" runat="server" Text="Generar" />
                        </td>
                        <td class="centro">
                            <asp:Button ID="btnPrint" Visible="false" OnClick="btnPint_Click" runat="server" Text="Exportar Excel" />
                        </td>
                    </tr>
                </table>
            </div>

            <hr/>

            <div id="Detalles">
                <h3 class='centro'>
                    Reporte de Auditorías Internas y Externas
                </h3>
                <br />
                <div id="gridContainerDetalles" style="height: auto; max-width: 100%; margin: 0px auto">
                    <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
                </div>
                <br />
                <br />
            </div>

            <asp:label ID="lblHtm" Visible ="false" runat="server"></asp:label>
            </div>
</asp:Content>

