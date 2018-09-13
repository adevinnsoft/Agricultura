<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmGrowingLiderDeLaSemana.aspx.cs" Inherits="Reportes_frmGrowingLiderDeLaSemana" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <style type="text/css">
        .container
        {
            width: 85% !important;
        }
        .containerTables
        {
            width: 100%;
            display: flex;
            justify-content: space-between;
        }
        .tablaPosicion, .tablaPlantacion, .tablaNoPlantacion
        {
            width: 32%;
        }
        .tablaPosicion label, .tablaPlantacion label, .tablaNoPlantacion label
        {
            font-size: 10px;
        }
        .containerTables table
        {
            width: 100%;
            text-align: center;
            font-size: 11px;
        }
        
        .containerTables table h3
        {
            display: block;
            background: #262626;
            color: White;
            font-weight: normal;
            padding: 5px 0;
            margin-bottom: 3px;
            border-radius: 15px;
        }
            #lblPlanta
            {
                font-size: 14px;    
            }
      
        .containerTables table tr td
        {
            padding: 3px;
            /*width: 33.33%;*/
            overflow: hidden;
        }
        
        .containerTables table tbody tr
        {
            height: 27px;
            }
        table.Posicion thead tr:nth-child(2)
        {
            background: #77933c;
            color: White;
            font-weight: bold;
            text-transform: uppercase;
            height: 26px;
        }
        .marino
        {
            background: #10243e;
            color: White;
        }
        .cyan
        {
            background: #c6d9f1;
            text-transform: uppercase;
            color: Black;
        }
        .red
        {
            background: #d99694;
            color: #984807;
            font-weight: bolder;
        }
        
        table.Plantacion thead tr:nth-child(2) th:first-child, table.NoPlantacion thead tr:nth-child(2) th:first-child
        {
            background: red;
            color: White;
        }
        
        table.Plantacion thead tr:nth-child(2) th:nth-child(2), table.NoPlantacion thead tr:nth-child(2) th:nth-child(2)
        {
            background: #e46c0a;
            color: White;
        }
        
        table.Plantacion thead tr:nth-child(2) th:nth-child(3), table.NoPlantacion thead tr:nth-child(2) th:nth-child(3)
        {
            background: #ffc000;
            color: White;
        }
        
        .semanas
        {
            margin-bottom: 10px;
            float: right;
        }
        
        .semanas select
        {
            margin-left: 10px;
        }
        .encabezado
        {
            height: 26px;
            }
        .Bloquear 
        {
            display: block;
            text-align: center;
        }
        .DesBloquear
        {
            display: none;
            text-align: center;
        }
    </style>

    <script type="text/javascript" id="Datos Iniciales">
        $(function () {
            if ($('#ctl00_ddlPlanta option:selected').text() == '') {
                var nuevo_tipoNombre2 = $('.Plant').text().split(':', 2)
                var nuevo_tipoNombre22 = nuevo_tipoNombre2[1];
                $('#lblPlanta').text(nuevo_tipoNombre22.trim());
            }
            else {
                $('#lblPlanta').text($('#ctl00_ddlPlanta option:selected').text());
            }
            //$('#lblPlanta').text($('#ctl00_ddlPlanta option:selected').text());
            $('#ctl00_ddlPlanta').live('change', function () {
                var tree = 0;
                $('#btn_ConsultarTabla').click();
                $('#lblPlanta').text($('#ctl00_ddlPlanta option:selected').text());
                $.blockUI();
                PageMethods.ObtenerDatosIniciales2(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), $('#ddlSemana').val(), function (response) {
                    $.unblockUI();
                    if (response[0] == 'ok') {
                        $('.Posicion tbody').html(response[1]);
                        $('.Plantacion tbody').html(response[2]);
                        $('.NoPlantacion tbody').html(response[3]);
                        if (
                                 (
                                     $('.Posicion tbody tr').length && $('.tablaPlantacion tbody tr').length && $('.tablaNoPlantacion tbody tr').length
                                 ) <= 0
                                ) {
                            $('#blackMessage').removeClass();
                            $('#blackMessage').addClass('Bloquear')
                        }
                        else {
                            $('#blackMessage').removeClass();
                            $('#blackMessage').addClass('DesBloquear')
                        }
                        //$('#olHistorial').html(response[1]);
                    }
                    else {
                        //popUpAlert(response[1], response[0]);
                    }
                });
            });
            $.blockUI();
            PageMethods.ObtenerDatosIniciales2(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), '0', function (response) {
                $.unblockUI();
                if (response[0] == 'ok') {
                    $('.Posicion tbody').html(response[1]);
                    $('.Plantacion tbody').html(response[2]);
                    $('.NoPlantacion tbody').html(response[3]);
                    if (($('.Posicion tbody tr').length && $('.tablaPlantacion tbody tr').length && $('.tablaNoPlantacion tbody tr').length) <= 0) {
                        $('#blackMessage').removeClass();
                        $('#blackMessage').addClass('Bloquear')
                    }
                    else {
                        $('#blackMessage').removeClass();
                        $('#blackMessage').addClass('DesBloquear')
                    }
                    //$('#olHistorial').html(response[1]);
                }
                else {
                    //popUpAlert(response[1], response[0]);
                }
            });

            PageMethods.ObtenerSemanasNS(function (response) {

                $('#ddlSemana').html(response);

            });
            $('#ddlSemana').live('change', function () {
                $.blockUI();
                var tree = 0;
                $('#btn_ConsultarTabla').click();
                PageMethods.ObtenerDatosIniciales2(($('#ctl00_ddlPlanta').val() == null ? 0 : $('#ctl00_ddlPlanta').val()), $('#ddlSemana').val(), function (response) {
                    $.unblockUI();
                    if (response[0] == 'ok') {
                        $('.Posicion tbody').html(response[1]);
                        $('.Plantacion tbody').html(response[2]);
                        $('.NoPlantacion tbody').html(response[3]);

                        if (($('.Posicion tbody tr').length && $('.tablaPlantacion tbody tr').length && $('.tablaNoPlantacion tbody tr').length) <= 0) {
                            $('#blackMessage').removeClass();
                            $('#blackMessage').addClass('Bloquear')
                        }
                        else {
                            $('#blackMessage').removeClass();
                            $('#blackMessage').addClass('DesBloquear')
                        }
                        //$('#olHistorial').html(response[1]);
                    }
                    else {
                        //popUpAlert(response[1], response[0]);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1>
            Líder de la Semana</h1>
        <div class="semanas">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label Text="Semanas:" runat="server" />
                    </td>
                    <td>
                    <%--<asp:DropDownList ID="ddlRol" runat="server" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="getSections" ></asp:DropDownList>--%>
                        <select id="ddlSemana">
                            
                        </select> 
                    </td>
                </tr>
            </table>
        </div>
        <div class="containerTables">
            <div class="tablaPosicion">
                <table border="0" cellpadding="0" cellspacing="0" class="Posicion">
                    <thead>
                        <tr>
                        <th colspan="3"><h3 id="ThPlanta"><label id="lblPlanta" for='male'> </label></h3></th>
                        </tr>
                        <tr>
                            <th>Posición</th>
                            <th>Líder Multihabilidades</th> 
                            <th>Calificación General</th>
                        </tr>
                    </thead>
                    <tbody>
                      
                    </tbody>
                </table>
            </div>
            <div class="tablaPlantacion">
                <table border="0" cellpadding="0" cellspacing="0" class="Plantacion">
                <thead>
                        <tr>
                        <th colspan="3"><h3>Problemática (GH Plantación)</h3></th>
                        </tr>
                        <tr class="encabezado">
                            <th>1</th>
                            <th>2</th> 
                            <th>3</th>
                        </tr>
                    </thead>
                    <tbody>
                        
                    </tbody>
                </table>
            </div>
            <div class="tablaNoPlantacion">
                <table border="0" cellpadding="0" cellspacing="0" class="NoPlantacion">
                <thead>
                        <tr>
                        <th colspan="3"><h3>Problemática (GH NO Plantación)</h3></th>
                        </tr>
                        <tr class="encabezado">
                            <th>1</th>
                            <th>2</th> 
                            <th>3</th>
                        </tr>
                    </thead>
                    <tbody>
                       
                    </tbody>
                </table>
            </div>
        </div>
        <div id="blackMessage">
            <h1>No existen datos correspondientes a los filtros seleccionados</h1>
        </div>
    </div>
    
</asp:Content>

