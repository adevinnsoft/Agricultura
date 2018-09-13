<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReporteGeneralAudInt.aspx.cs" Inherits="Auditorias_Reportes_ReporteGeneralAudInt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Reporte General de Auditorias Internas</title>
 
        <!--[if gte IE 9]><!-->
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
        <!--<![endif]-->

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

            .centro{
                text-align: center;
            }
        </style>

        <script type="text/javascript">
            $(function () {
               $("#<%= txtFecha.ClientID %>").datepicker({
                    dateFormat: 'yy-mm-dd',
                    firstDay: 1
                });

                $("#btnPrint").click(function () {
                    window.print();
                });
            });
        </script>

          <style>
             @media print {
                body {
                    -webkit-print-color-adjust: exact;
                }
            }

            #tablaReporte table, .tblTotales table {
                border-collapse: collapse;
                width: 100%;
            }

            #tablaReporte th, .tblTotales th{
                padding: 8px;
                border-bottom: 2px solid #3c3b3b;
            }

            #tablaReporte td, .tblTotales td {
                padding: 8px;
                text-align: left;
                border-bottom: 2px solid #3c3b3b;
            }

            #tablaReporte tr:hover {background-color: #f5f5f5}

            .centro {
                text-align: center;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
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
                    </tr>
                </table>
            </div>

            <hr>

            <div id="Detalles">
                <p style="margin:0px auto; width:100%; height:auto;">
                    <h3 class='centro'>Reporte General de Auditorias Internas</h3>
                </p>
                <br />
                <div id="gridContainerDetalles" style="height: auto; max-width: 90%; margin: 0px auto">
                    <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
                </div>
                <br />
                <br />
            </div>
            </div>
</asp:Content>

