<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" 
    CodeFile="ReporteAnualAuditorias.aspx.cs" Inherits="Auditorias_Reportes_ReporteAnualAuditorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        .container{
            max-width:90%;
            min-width:90%;
        }
        @media print
        {
            body
            {
                -webkit-print-color-adjust: exact;
            }
        }
        
        .centro
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#txtFechaInicio").datepicker({
                dateFormat: 'yy-mm-dd',
                firstDay: 1
            });

            $("#txtFechaFin").datepicker({
                dateFormat: 'yy-mm-dd',
                firstDay: 1
            });

            $('#<%= btnGenerar.ClientID %>').click(function () {

                var fInicio = $("#txtFechaInicio").val();
                var fFinal = $("#txtFechaFin").val();

                if (fInicio != '' && fFinal != '') {
                    $('#grid').attr('src', "http://wmp.naturesweet.com/reportesauditorias/reportes/Ejecucion.aspx?fInicio=" + fInicio + "&fFinal=" + fFinal + "");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div class="container">
    <div id="filtros" style="margin: 0px auto; width: 60%; height: auto;">
        <table style="margin: 0px auto; width: 100%;">
            <tr>
                <td class="centro">
                    Fecha Inicio:
                </td>
                <td class="centro">
                    <input type="text" id="txtFechaInicio" class="centro" />
                </td>
                <td class="centro">
                    Fecha Fin:
                </td>
                <td class="centro">
                    <input type="text" id="txtFechaFin" class="centro" />
                </td>
                <td class="centro">
                    <asp:Button id="btnGenerar" OnClientClick="return false;" runat="server" Text="Generar"/>
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <h3 class='centro'>
        Reporte Anual de Auditorias</h3>
    <br />
    <div style="width: 100%; height: auto;">
        <iframe id="grid" src="" style="width: 100%; height: 3000px;"></iframe>
    </div>
    </div>
</asp:Content>
