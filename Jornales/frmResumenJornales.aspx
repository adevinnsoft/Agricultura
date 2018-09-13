<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmResumenJornales.aspx.cs" Inherits="Jornales_frmResumenJornales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%--<script type="text/javascript" src="../Scripts/jquery-1.10.2.min.js"></script>  --%>
 <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
 <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
 <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
<script type="text/javascript">

    $(function () {
        seleccionarPlanta();
        obtenerReporteInicial();
    });

    function obtenerReporteInicial() {
        var idPlantaActual = $('[id*="ddlPlanta"] option:selected').val();
        obtenerReporte(parseInt(idPlantaActual));
    }
    function seleccionarPlanta() {
        $('[id*=ddlPlanta]').change(function () {
            var idPlanta = $(this).val();
            obtenerReporte(parseInt(idPlanta));
        });
    }
    function obtenerReporte(idPlanta) {
        bloqueoDePantalla.bloquearPantalla();
        try {
            PageMethods.obtenerReporte(idPlanta, function (response) {
                if (response[0] == '1') {
                    $('#PanelA').html(response[2]);
                    $('#PanelB').html(response[3]);
                    agregarAttr();
                    calculoPromedioRequerido();
                    calculoSugerenciaContratacion();
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                } else {
                    console.log(response[2]);
                    popUpAlert(response[1], response[0]);
                }
            }, function (e) {
                bloqueoDePantalla.indicarTerminoDeTransaccion();
                console.log(e);
            });
        } catch (e) {
            console.log(e);
            bloqueoDePantalla.indicarTerminoDeTransaccion();
        }
        bloqueoDePantalla.desbloquearPantalla();
    }

    function agregarAttr() {
        var semanas = [];
        $('.tblDatosResumen thead th.thsemana').each(function () {
            semanas.push(parseInt($(this).attr('semana')));
        });

        var semanaActual = semanas[0];
        $('.tblDatosResumen tr').find('.Jornales[semana="' + semanaActual + '"]').each(function () {
            $(this).attr('semanaActual', '1');
        });
    }

    function calculoSugerenciaContratacion() {
        var sugerenciaContratacion = 0;
        $('.tblDatosResumen tr').each(function () {
            var Jornales = parseInt($(this).find('.Jornales[semanaactual="1"]').text());
            var PromedioRequerido = parseInt($(this).find('.PromedioRequerido').text());
            sugerenciaContratacion = Jornales - PromedioRequerido;
            $(this).find('.SugerenciaContratacion').text(sugerenciaContratacion);
        });
    }

    function calculoPromedioRequerido() {
        var PromedioRequerido = 0;
        $('.PromedioRequerido').each(function () {
            var sumatoria = 0;
            var contador = 0;
            $(this).parent().find('.Jornales:not(.Jornales[semanaactual="1"])').each(function () {
                sumatoria += parseInt($(this).text());
                contador++;
            });
            PromedioRequerido = sumatoria / contador;
            $(this).text(PromedioRequerido);
        });

    }




</script>
  <style type="text/css">
    div#contenderodReporte {
        display: flex;
        justify-content: space-between;
    }

    div#PanelB {
        max-width: 800px;
        overflow-x: auto;
        margin:0px;
        border-right: 1px dotted black;
    }

    div#contenderodReporte table th {
        white-space: nowrap;
        padding: 4px 8px;
        background: #76933C;
        color: white;
    }
    
    div#contenderodReporte table, div#contenderodReporte table tr
    {
        text-align: center; 
    }
    
    div#contenderodReporte table th {
        background: #76933c;
        color: White;
        font-weight: normal;
    }
    div#contenderodReporte table td {
        white-space: nowrap;
        padding: 5px 8px;
    }
    
    td.largeText {
        max-width: 251px;
        color: rgb(70, 46, 197);
        overflow: hidden;
        cursor:pointer;

    }
    
    table.tblJornalesLideres, table.tblDatosResumen
    {
        border-collapse: collapse;
    }
    
    table.tblJornalesLideres tr td, table.tblDatosResumen tr td
    {
        border: 1px dotted black;
    }
    
   table.tblJornalesLideres th, table.tblDatosResumen th
    {
        border: 1px dotted white;
    }
    
    table.tblDatosResumen tr th:first-child, table.tblDatosResumen tr td:first-child
    {
        border-left: none;    
    }
    table.tblDatosResumen tr th:last-child, table.tblDatosResumen tr td:last-child
    {
        border-right: none;    
    }
    table.tblDatosResumen td.JHA
    {
        background: red;
        color: White;    
    }
    table.tblDatosResumen td.Impar
    {
        background: #c4d79b;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="container">
        <h1><asp:Label runat="server" Text="Resumen de Jornales"></asp:Label></h1>
        <div id="contenderodReporte">
            <div id="PanelA"></div>
            <div id="PanelB"></div>
        </div>
   </div>
</asp:Content>