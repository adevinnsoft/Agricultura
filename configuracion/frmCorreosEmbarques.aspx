<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="frmCorreosEmbarques.aspx.cs" Inherits="frmCorreosEmbarques" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript">
        var idCapturaDefault = <%=idCapturaDefault %>;
        var listaDeDistribucion;
    </script>
    <script type="text/javascript">

        function EliminarCorreo(obj) {
            if ($(obj).parent().parent().hasClass("Cargado")) {
                $(obj).parent().parent().removeClass("Cargado").addClass("Eliminado");
            }
            else {
                if (!$(obj).parent().parent().hasClass("Eliminado")) {
                    $(obj).parent().parent().remove();
                    tablavacia();
                }
            }
        }

        function tablavacia() {
            if ($('#gvEmbarques tbody tr').length == 0) {
                $('#gvEmbarques').append("<tr id='null'><td colspan='2'>No hay correos configurados actualmente</td></tr>");
            }
        }

        function obtenerListaDeDistribucion() {
            PageMethods.obtenerListaDeDistribucion(function (response) {
                if (response[0] == '1') {
                    $('#gvEmbarques').html(response[2].toString());
                    tablavacia();
                }
                else {
                    popUpAlert(response[1], response[2]);
                }
            });
        }

        function agregarCorreos() {
            var SAMaccount = $('.CuentaEmbarques').val().trim();

            try {
                if ($(".gridView").find("[otherClass=SAMaccount]").text().indexOf($(".lblCuentaMostrado").text()) < 0) {
                    $.blockUI();

                    PageMethods.agregarCorreos(SAMaccount, function (response) {
                        if (response[0] == '1') {
                            $("#null").remove();
                            $('#gvEmbarques').append(response[2]);
                        }
                        else {
                            popUpAlert(response[1], response[2]);
                        }
                        $.unblockUI();
                    });
                }
                else {
                    popUpAlert("El correo de esta cuenta ya existe", "info");
                }
            } catch (e) {
                $.unblockUI();
                console.log(e);
            }
        }

        function verificarSAMaccount() {
            var SAMaccount = $('.CuentaEmbarques').val().trim();
            var existente = false;
            var vacio = false;

            if (SAMaccount == '') {
                vacio = true;
            }
            else {
                vacio = false;
            }

            if (vacio) {
                popUpAlert("Ingrese una cuenta para continuar con el proceso", "warning");
                return false;
            }
            else {

                try {
                    $.blockUI();
                    PageMethods.buscarEnActiveDirectory(SAMaccount, function (response) {
                        if (response[0] == '1') {
                            $('.divDatos').html(response[2]);
                            $('.divDatos').show();

                        }
                        else {
                            popUpAlert(response[1], response[2]);
                        }
                        $.unblockUI();
                    });
                } catch (e) {
                    $.unblockUI();
                    console.log(e);
                }
            }
        }

        function LimpiarDatos() {
            $(".CuentaEmbarques").val("");
            $(".divDatos").empty();
            $(".Eliminado").removeClass("Eliminado");
            $(".Nuevo").removeClass("Eliminado");
            $(".Nuevo").remove();
            obtenerListaDeDistribucion();
        }

        function guardarListaDeDistribucion() {
            listaDeDistribucion = $('#table tbody tr td input[class="invisible"]').map(function () {
                if ($(this).val() != '') {
                    return {
                        idCaptura: ($('#gvEmbarques tbody tr[idCaptura]').first().text() == '' ? idCapturaDefault : $('#gvEmbarques tbody tr[idCaptura]').first().text()),
                        etiqueta: $(this).val(),
                        correos: $('#gvEmbarques tbody tr:not(.Cargado)').map(function () {
                            if ($(this).text() != '') {
                                return {
                                    correo: $(this).find('td[otherclass="Correo"]').text(),
                                    cuenta: $(this).find('td[otherclass="SAMaccount"]').text(),
                                    estado: ($('#gvEmbarques tbody tr[class="Nuevo"]').length > 0 ? 1 : 0)
                                }
                            }
                        }).get()
                    }
                }

            }).get();

            PageMethods.GuardarListaDeDistribucion(listaDeDistribucion, function (response) {
                if (response[0] == "1") {
                    popUpAlert(response[1], response[2]);
                }
                else {
                    popUpAlert(response[1], response[2]);
                }
                LimpiarDatos();
            });
        }

        $(function () {
            $('#btnBuscarSAMaccount').click(function () {
                verificarSAMaccount();
            });

            $('#btnCancelar').click(function () {
                LimpiarDatos();
            });

            $('#btnGuardarEmbarques').click(function () {
                guardarListaDeDistribucion();
            });

            obtenerListaDeDistribucion();
        });
           
    </script>
    <style type="text/css">
        .tablesorter .filtered
        {
            display: none;
        }
        /* Ajax error row */
        .tablesorter .tablesorter-errorRow td
        {
            text-align: center;
            cursor: pointer;
            background-color: #e6bf99;
        }
        .configCorreosEmbarques h3
        {
            width: 100%;
            min-width: 100%;
            background: #F4D101;
            color: BLACK;
            padding: 10px;
            text-align: left;
            background-repeat: no-repeat;
            background-position-x: 99%;
            background-position-y: 7px;
            border: 1px solid white;
        }
        .configCorreosEmbarques h3.open
        {
            width: 100%;
            background: #F4D101;
            padding: 10px;
            text-align: left;
            background-repeat: no-repeat;
            background-position-x: 99%;
            background-position-y: 7px;
        }
        td.correos
        {
            min-width: 48%;
            max-width: 48%;
            width: 48%;
            padding-right: 2%;
        }
        
        .accBody
        {
            border: 1px dotted orange;
            padding: 9px;
            width: 100%;
            background: #F0F5E5;
        }
        #gvEmbarques
        {
            width: 90%;
        }
        
        .configCorreosEmbarques
        {
            max-width: 805px;
        }
        .accHead
        {
            cursor: pointer;
        }
        .accHead:hover
        {
            cursor: pointer;
            background-color: #FFC10F;
            border: 1px solid #FFC10F;
        }
        .Eliminado
        {
            /*display: none;*/
            filter: alpha(opacity=50);
            opacity: 0.5;
            color: Red;
        }
        .botonAgregar
        {
            width: 24px;
        }
        
        .divDatos
        {
            display: none;
        }
        input#btnBuscarSAMaccount
        {
            position: relative;
            right: 400px;
            top: -6px;
        }
        .divDatos
        {
            margin-top: 20px;
        }
        
        .divDatos span
        {
            display: block;
            margin-bottom: 10px;
        }
        
        label.bold
        {
            font-weight: bold;
        }
        
        img#btnAgregarEmbarques
        {
            position: relative;
            left: 210px;
            bottom: 40px;
            cursor: pointer;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:label id="Label1" runat="server">Configuracion de Correos para Embarques</asp:label></h1>
        <div class="configCorreosEmbarques">
            <h3 class="accHead">
                <asp:label id="lblEmbarques" runat="server" text="Embarques"></asp:label></h3>
            <div class="accBody">
                <table id="table">
                    <tr>
                        <td class="correos">
                            <asp:label id="lblNombreDeLista" runat="server" text="Nombre de Lista" cssclass="invisible">
                            </asp:label>
                            <asp:textbox id="txtNombreDeLista" class="inputNombreLista" runat="server" cssclass="invisible"
                                maxlength="200" text="lista">
                            </asp:textbox>
                            <asp:label id="lblCuentaEmbarques" runat="server" text="Cuenta"></asp:label>
                            <%--<asp:TextBox ID="txtCuentaEmbarques" runat="server" class="CuentaEmbarques"
                                        MaxLength="200"></asp:TextBox>--%>
                            <input type="text" id="txtCuentaEmbarques" class="CuentaEmbarques" maxlength="200" />
                            <input id="btnBuscarSAMaccount" class="Buscar" type="button" value="Buscar" />
                            <div class="divDatos">
                                <label for="lblNombre" class="lblNombre">
                                    Nombre:</label>
                                <label for="lblCuenta" class="lblCuenta">
                                    Cuenta:</label>
                                <label id="lblNombreMostrado">
                                </label>
                                <label id="lblCuentaMostrado">
                                </label>
                            </div>
                            <%--<img src="../comun/img/add-icon.png" id="btnAgregarPreHarvest" class="botonAgregar">--%>
                            <table id="gvEmbarques" class="gridView">
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <input id="btnGuardarEmbarques" class="Guardar" type="button" value="Guardar" />
            <input id="btnCancelar" class="Cancelar" type="button" value="Limpiar" />
            <%--   <input id="btnEnviarMail" type="button" value="Enviar" />--%>
        </div>
        <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    </div>
</asp:content>
