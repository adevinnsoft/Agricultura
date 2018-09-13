 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmEquiposDeTrabajo.aspx.cs" Inherits="frmReporteEficiencias" meta:resourcekey="PageResource1" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <link type="text/css" href="../comun/css/fixed_table_rc.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet"href="../comun/css/tooltipster.css" />
    <link type="text/css" href="../comun/css/chosen.css" rel="stylesheet" />
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/fixedtables/widgets/widget-scroller.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jsPopUp.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>

    <link href="../comun/css/jquery-ui-1.8.21.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../comun/scripts/jquery-ui-1.8.21.custom.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../comun/scripts/jsPopUp.js"></script>
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <style type="text/css">
        .divConsulta {
            width: 1024px;
        }

        div#divEditor {
            width: 100%;
            display: flex;
            justify-content: space-between;
        }

        div#divChildA {
            width: 70%;
        }

        div#divChildB {
            width: 29.5%;
        }

        .divDatos {
            box-sizing: border-box;
        }

        div#divEquipoList {
            width: 100%;
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
        }

        .divItemEquipo {
            box-sizing:border-box;
            width: 49.65%;
            border: 1px solid #adc995;
            /*padding: 5px;*/
            background: #f0f5e5;
            margin-top: 5px;
        }

        h2{
            margin: 10px auto;
        }

        .divItemAsociado {
            display: flex;
            background: #f0f5e5;
            justify-content: space-between;
            padding: 2px 10px;
            border-bottom: 1px solid #adc995;
            align-items: center;
            transition: all 0.5s;
        }
        .divItemAsociado:hover{
            background: #d2e6c1;
        }
        span.ListaAsociados{
            background: #adc995;
            display: block;
            padding: 10px 0;
            text-align: center;
            color: white;
            text-transform: uppercase;
            font-weight: bold;
        }

        span.ListaEquipos {
            background: #adc995;
            display: block;
            padding: 10px 0;
            text-align: center;
            color: white;
            text-transform: uppercase;
            font-weight: bold;
        }

        .divItem-title {
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: #d2e6c1;
            text-transform: uppercase;
            font-weight: bold;
            padding: 3px;
        }

        .divItem-title .botones img{
            margin-left: 3px;
            cursor: pointer;
        }



        .itemChild span{
            display: block;
            padding: 5px 10px;
        }
        
        .itemChild:hover
        {
            background: #d2e6c1;
         }
        
        .itemChild img
        {
            display: none;
        }
        
        .itemChild:hover img
        {
            display: block;
        }
            
        select#ddlLider {
            width: 150px;
        }

        img#imgAddGroup {
            float: left;
            margin-left: 4px;
            cursor: pointer;
        }
        
        table.index tr td input[type="text"]
        {
            margin-top: 3px;
            }
        .divItemAsociado img
        {
            cursor: pointer;
        }
        .grupoSeleccionado
        {
            -webkit-box-shadow: 0px 0px 10px 0px rgba(97,133,65,0.7);
            -moz-box-shadow: 0px 0px 10px 0px rgba(97,133,65,0.7);
            box-shadow: 0px 0px 10px 0px rgba(97,133,65,0.7);
            background: #f5ffbf;
            
        }
        .itemChild img {
            width: 8px;
            height: 8px;
            padding-right: 8px;
            cursor: pointer;
        }

        .itemChild {
            display: flex;
            justify-content: space-between;
            align-items: center;
            transition: all 0.5s;
        }
        
        .divItem-title span {
    cursor: pointer;
        }

        .grupoSeleccionado .divItem-title {
            background: #f4d101;
        }
        

    </style>
    <script type="text/javascript" id="reporteEficiencia">

        var contadorEquipo = 0;
        
        var idEmpleadoLider = 0;
        var idLider = 0;
        var lider = '';


        var JSONAsociados;
        var JSONEquipos;

        $(function () {
            contadorEquipo = 0;

            loadTrigger();
            loadDropDownList();
        });

        function loadTrigger() {
             $('#ddlLider').change(function () {
                onChangeDdl();
            });

            if ($('#ctl00_ddlPlanta').length > 0) {
                $('#ctl00_ddlPlanta').change(function () {
                    loadDropDownList();
                });
            }
        }

        function loadDropDownList() {
            bloqueoDePantalla.bloquearPantalla();
            $('#ddlLider').html('');
            $('#divAsociadosList').html('')
            PageMethods.precargaDatos(function (response) {
                if (parseInt(response[0]) == 1) {
                    $('#ddlLider').append(response[2]).change();
                } else {
                    popUpAlert(response[1], response[2]);
                }
                bloqueoDePantalla.indicarTerminoDeTransaccion();
            });
            bloqueoDePantalla.desbloquearPantalla()
        }


        function onClickEquipo(equipo) {
            //var equipo = $('.divItemEquipo')[0]
            if ($(equipo).parent().parent().hasClass('grupoSeleccionado')) {
               $(equipo).parent().parent().removeClass('grupoSeleccionado')
            } else {
               $(equipo).parent().parent().addClass('grupoSeleccionado')
            }
       }

       function onClickNuevoEquipo() {
           var equipo = $('#txtNombreEquipo').val().trim().toUpperCase();
           if (equipo.length == 0) {
               console.log('campo vacío');
           } else if ($('.divItemEquipo:not(.eliminado) >.divItem-title').filter(function () { if ($(this).find('span').text().toUpperCase().trim() == $('#txtNombreEquipo').val().toUpperCase().trim()) return true; }).length > 0) {
               console.log('ya existe');
               popUpAlert('Ya se agregó un Equipo con el nombre de "' + equipo + '" a la lista.','warning');
               $('#txtNombreEquipo').focus().select();
           } else {
               $('#divEquipoList').append(getNewTeam(undefined,idLider, idEmpleadoLider, equipo, undefined, undefined));
               console.log('agregando');
               $('#txtNombreEquipo').val('');
           }
       }

       function onChangeDdl() {
           var ddl = $('#ddlLider');
            bloqueoDePantalla.bloquearPantalla();
            $('#divAsociadosList').html('');
            $('#divEquipoList').html('');
            idEmpleadoLider = $('#ddlLider option[value="' + ddl.val() + '"]').attr('idempleado');
            idLider = ddl.val();
            lider = $('#ddlLider option[value="' + ddl.val() + '"]').text();

            $('#spanLider').text(lider);

            PageMethods.obtenerDatosDeLider(idEmpleadoLider, function (response) {
                console.log(response[1]);
                if (parseInt(response[0]) == 1) {
                     JSONAsociados = response[2].length > 0 ? JSON.parse(response[2]) : undefined;
                     JSONEquipos = response[3].length > 0 ? JSON.parse(response[3]) : undefined;
                    $('#divAsociadosList').append(genListItemsAsociados(JSONAsociados));
                    $('#divAsociadosList img').click(function () {
                        agregaAsociadoAGrupo($(this).parent().attr('idEmpleado'), $(this).parent().find('span').text());
                    });

                    $('#divEquipoList').append(genListItemsEquipos(JSONEquipos, JSONAsociados))
                } else {
                    console.log(response[1]);
                }
                bloqueoDePantalla.indicarTerminoDeTransaccion();

            });
            console.log('idU:' + ddl.val() + ', idE:' + idEmpleadoLider );
            bloqueoDePantalla.desbloquearPantalla()
        }

        function genListItemsAsociados(JSONAsociados) {
            var code = '';
            if (JSONAsociados != undefined && JSONAsociados.length > 0) {
                for (var cont = 0; cont < JSONAsociados.length; cont++) {
                    code += '<div class="divItemAsociado" idEmpleado="' + JSONAsociados[cont].idEmpleadoAsociado + '"><span>' + JSONAsociados[cont].consecutivo.toString() + ".- " + JSONAsociados[cont].nombre.toString().toUpperCase() + '</span><img src="../comun/img/add-icon.png" alt="[+]" class="addAsociado"/> </div>';
                }
            }
            return code
        }

        function genListItemsEquipos(JSONEquipos, JSONAsociados) {
            var code = '';

            for (index in JSONEquipos) {
                code += getNewTeam(JSONEquipos[index].idEquipo, idLider, idEmpleadoLider, JSONEquipos[index].equipo, JSONEquipos[index].asociados, JSONAsociados)
            
            }
            return code
        }
        
        function agregaAsociadoAGrupo(idEmpleado, nombre) {
            console.log('agrega Lider ' + idEmpleado + ', nombre: ' + nombre);
            var cambios = 0;
            if ($('div.divItemEquipo.grupoSeleccionado').length > 0) {
                if ($('div.divItemEquipo.grupoSeleccionado').length != $('div.divItemEquipo.grupoSeleccionado').find('.itemChild[idEmpleado="' + idEmpleado + '"]').length) {
                    $('div.divItemEquipo.grupoSeleccionado').each(function () {
                        var idEquipoTemp = $(this).attr('idequipotemp');
                        var nombreEquipo = $(this).find('.divItem-title span').text();
                        if (!$(this).hasClass('modificado') && parseInt($(this).attr('idequipo')) > 0) {
                            $(this).addClass('modificado');
                            $(this).removeClass('noMod');
                        }
                        if ($(this).find('.divItem-Container .itemChild[idEmpleado="' + idEmpleado + '"]').length == 0) {
                            cambios++;
                            $(this).find('.divItem-Container').append(getNewTeamItem(idEmpleado, nombre, idEquipoTemp, nombreEquipo));
                        }
                    });
                }else {
                    console.log('Sin Acciones');
                }
            
                if(cambios > 0 ) {
                    console.log('Equipos con cambios:' + cambios);
                }else{
                    console.log('no Hay Cambios');
                }
            } else {
                console.log('No Hay Grupos Seleccionados.');

                popUpAlert('El Asociado no puede ser agregado, seleccione por lo menos un Equipo.', 'warning');
            }

        }

        function borrarAsociadoPop(idEquipoTemp, idEmpleado, nombreEquipo, nombreAsociado) {

            popUpAlertConfirm('<h4>¿Desea quitar a ' + nombreAsociado + ' del equipo ' + nombreEquipo + '?</h4>',
               	 '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();">'
				+ '<input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="borrarAsociadoDeGrupo(' + idEquipoTemp + ', ' + idEmpleado + ');">  ', 'warning');


        }
        function borrarEquipoPop(idEquipoTemp, nombreEquipo) {

            popUpAlertConfirm('<h4>¿Realmente desea eliminar ' + nombreEquipo + ' de la lista de equipos?</h4>',
               	 '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();">'
				+ '<input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="borrarEquipo(' + idEquipoTemp + ');">  ', 'warning');


        }

        function vasiarGrupoPop(idEquipoTemp, nombreEquipo) {
            popUpAlertConfirm('<h4>¿Desea Eliminar todos los Asociados del equipo ' + nombreEquipo +'?</h4>',
               	 '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();">'
				+ '<input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="vasiarGrupo(' + idEquipoTemp + ');">  ', 'warning');

        }

        function borrarAsociadoDeGrupo(idEquipoTemp, idEmpleado) {
            var asociadoDiv = $('.divItemEquipo[idequipotemp="' + idEquipoTemp + '"] .itemChild[idempleado="' + idEmpleado + '"]')[0];

            if (parseInt($(asociadoDiv).parent().parent().attr('idequipo')) > 0 && !$(asociadoDiv).parent().parent().hasClass('modificado')) {
                $(asociadoDiv).parent().parent().addClass('modificado');
                $(asociadoDiv).parent().parent().removeClass('noMod');
            } else {
                // no aplica
            }
            $(asociadoDiv).remove();
            closeJsPopUpAux();
        }

//        function borrarAsociadoDeGrupo(btnBorrar) {
//            if (parseInt($(btnBorrar).parent().parent().parent().attr('idequipo')) > 0 && !$(btnBorrar).parent().parent().parent().hasClass('modificado')) {
//                $(btnBorrar).parent().parent().parent().addClass('modificado');
//                $(btnBorrar).parent().parent().parent().removeClass('noMod');
//            } else {
//                // no aplica
//            }
//            $(btnBorrar).parent().remove();
//        }


        function borrarEquipo(idEquipoTemp) {
            var equipoDiv = $('.divItemEquipo[idequipotemp="' + idEquipoTemp + '"]');

            if (parseInt($(equipoDiv).attr('idequipo')) > 0 && !$(equipoDiv).hasClass('eliminado')) {
                if (!$(equipoDiv).hasClass('modificado')) {
                    $(equipoDiv).addClass('modificado');
                    $(equipoDiv).removeClass('noMod');
                } else {
                    // no aplica
                }
                $(equipoDiv).addClass('eliminado');
                $(equipoDiv).hide();
            } else {
                $(equipoDiv).remove();
            }
            closeJsPopUpAux();

        }

//        function borrarEquipo(btnBorrar) {
//            if (parseInt($(btnBorrar).parent().parent().parent().attr('idequipo')) > 0 && !$(btnBorrar).parent().parent().parent().hasClass('eliminado')) {
//                if (!$(btnBorrar).parent().parent().parent().hasClass('modificado')) {
//                    $(btnBorrar).parent().parent().parent().addClass('modificado');
//                    $(btnBorrar).parent().parent().parent().removeClass('noMod');
//                } else {
//                    // no aplica
//                }
//                $(btnBorrar).parent().parent().parent().addClass('eliminado');
//                $(btnBorrar).parent().parent().parent().hide();
//            } else {
//                $(btnBorrar).parent().parent().parent().remove();
//            }

//        }


        function vasiarGrupo(idEquipoTemp) {
            $('.divItemEquipo[idequipotemp="' + idEquipoTemp + '"] .divItem-Container .itemChild').remove();
            closeJsPopUpAux();

        }

//        function vasiarGrupo(btnBorrar) {
//            $(btnBorrar).parent().parent().parent().find('.divItem-Container').remove();
//        }


        function getNewTeamItem(idEmpleado, nombre, idEquipoTemp, nombreEquipo) {
            return '<div class="itemChild newItem" idEmpleado="' + idEmpleado + '"><span >' + nombre + '</span><img src="../comun/img/cross.png" alt="Borrar" onclick="borrarAsociadoPop(' + idEquipoTemp + ',' + idEmpleado + ',\'' + nombreEquipo + '\',\'' + nombre + '\');"/></div>'; //onclick="borrarAsociadoDeGrupo(this);"/></div>';
        }

        function getNewTeam(idEquipo, idLider, idEmpleadoLider, nombreEquipo, listaAsociados, JSONListaEquipoAsociados) {
            var code = '<div class="divItemEquipo'+ (idEquipo != undefined ? ' noMod':'') +'" idEquipo="' + (idEquipo != undefined ? idEquipo : 0) + '" idEquipoTemp="' + (++contadorEquipo) + '">'
                 + '<div class="divItem-title"><span  onclick="onClickEquipo(this)">' + nombreEquipo + '</span><div class="botones"><img src="../comun/img/empty-icon.png" alt="[-]" onclick="vasiarGrupoPop(' + contadorEquipo + ', \'' + nombreEquipo + '\');"><img src="../comun/img/remove-icon.png" alt="[X]" onclick="borrarEquipoPop(' + contadorEquipo + ', \'' + nombreEquipo + '\');"></div></div>'
                 + '<div class="divItem-Container">';

            if (listaAsociados != undefined && listaAsociados.length > 0 && JSONListaEquipoAsociados != undefined && JSONListaEquipoAsociados.length > 0) {
                console.log('agregando asociados a ' + JSONEquipos[index].equipo)
                for (a in listaAsociados) {
                    for (i in JSONListaEquipoAsociados) {
                        if (listaAsociados[a] == JSONListaEquipoAsociados[i].idEmpleadoAsociado) {
                            code += getNewTeamItem(JSONListaEquipoAsociados[i].idEmpleadoAsociado, JSONListaEquipoAsociados[i].nombre, contadorEquipo, nombreEquipo);
                        }
                    }
                }

            }
            
            code += '</div>'
                 + '</div>';

            return code;
        }

 
        function genJSONEquipos() {

            var json = $('div.divItemEquipo').not('.noMod').map(function () {
                return cabeza = {
                    idEquipo: $(this).attr("idequipo")
                    , idEquipoTemp: $(this).attr("idequipotemp")
			        , idEmpleadoLider:  $('#ddlLider option[value="' + $('#ddlLider').val() + '"]').attr('idempleado')
			        , idLider: $('#ddlLider').val()
                    , equipo: $(this).find('.divItem-title span').text()
			        , modificado: $(this).hasClass('modificado') ? 1 : 0
                    , eliminado: $(this).hasClass('eliminado') ? 1 : 0

                }
            }).get();
            return json;
        }

        function genJSONAsociadosEquipo() {

            var json = $('div.divItemEquipo ').not('.noMod, .eliminado').find('.divItem-Container .itemChild').map(function () {
                return head = {
                    idEmpleado: $(this).attr('idempleado')
		            , idEquipoTemp: $(this).parent().parent().attr('idequipotemp')
                }
            }).get();
            return json;
        }

        function guardar() {
            var jsonEquipos = genJSONEquipos();
            var jsonAsociados = genJSONAsociadosEquipo();
            if (jsonEquipos.length > 0) {
                PageMethods.guardar(jsonEquipos, jsonAsociados, function (response) {
                    bloqueoDePantalla.desbloquearPantalla();
                    if (response[0] == "1") {
                        popUpAlert(response[2], "info");
                        onChangeDdl();
                    } else {
                        popUpAlert("Problema al guardar, intentelo mas tarde.", "error");
                    }
                    bloqueoDePantalla.indicarTerminoDeTransaccion();
                });

            } else {
                popUpAlert("No se encontraron cambios", "info");
            }
        }


    </script>

</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:label id="lblTitulo" runat="server" text="Equipos de Trabajo"></asp:label></h1>
        <br />
        <div class="divDatos">
            <table id="tblDatos" class="index">
                <tr>
                    <td>
                        <span><asp:Label ID="LblLider" runat="server" >Lider:</asp:Label></span>&nbsp;
                    </td>
                    <td>
                        <select id="ddlLider"><option value="0"><asp:Label ID="optSeleccione" runat="server">--SELECCIONE--</asp:Label></option></select>
                    </td>
                    <td>
                        <span><asp:Label ID="lblEquipo" runat="server" >Nuevo Equipo:</asp:Label></span>
                    </td>
                    <td>
                        <input type="text" id="txtNombreEquipo" /><img src="../comun/img/add-icon.png" id="imgAddGroup" alt="[+]" onclick="onClickNuevoEquipo()"/> </td>
                </tr>
            </table>
            <h2><asp:Label ID="lblRelacion" runat="server">Relación de Equipos para: </asp:Label><span id="spanLider">Fernando Barreto</span></h2>
            <div class="divConsulta">
                <div class="divGuardar">
                    <input type="button" class="btnGuardar" value="Guardar" onclick="guardar();"/>
                </div>
                <div id="divEditor">
                    <div id="divChildB">
                        <span class="ListaAsociados"><asp:Label ID="Label1" runat="server">Lista de Asociados</asp:Label></span> 
                        <div id="divAsociadosList">
                            <div class="divItemAsociado" idEmpleado="34345"><span >José Alfredo Jimenes</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>Pepe el Toro Gonzales</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>Ernesto García Garza</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>Alonso Alejandro Alvares Arriaga</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>Alonso Alejandro Sanches Arriaga</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>Ernesto Josè García Garza</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>Pepe el Toro Jr. Gonzales Torres</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>empleado 8</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>empleado 9</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                            <div class="divItemAsociado"><span>empleado 10</span><img src="../comun/img/add-icon.png" alt="[+]"/> </div>
                        </div>
                    </div>
                    <div id="divChildA">
                        <span class="ListaEquipos"><asp:Label ID="lblEquipos" runat="server">Lista de Equipos </asp:Label></span>
                        
                        <div id="divEquipoList" >

                            <div class="divItemEquipo grupoSeleccionado">
                                <div class="divItem-title">
                                    <span onclick="onClickEquipo(this)">Fernando Barreto - Equipo Fumigación</span><div class="botones"><img src="../comun/img/empty-icon.png" alt="[-]" onclick="vasiarGrupo(this);"/><img src="../comun/img/remove-icon.png" alt="[X]" onclick="borrarEquipo(this);" /></div></div>
                                <div class="divItem-Container">
                                    <div class="itemChild"><span >José Alfredo Jimenes</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                    <div class="itemChild"><span >Ernesto García Garza</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                    <div class="itemChild"><span >Alonso Alejandro Alvares Arriaga</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                </div>
                            </div>
                            <div class="divItemEquipo">
                                <div class="divItem-title"><span onclick="onClickEquipo(this)">Fernando Barreto - Equipo Fumigación</span><div class="botones"><img src="../comun/img/empty-icon.png" alt="[-]" onclick="vasiarGrupo(this);" /><img src="../comun/img/remove-icon.png" alt="[X]" onclick="borrarEquipo(this);"/></div></div>
                                <div class="divItem-Container">
                                    <div class="itemChild"><span >José Alfredo Jimenes</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                    <div class="itemChild"><span >Pepe el Toro Gonzales</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                    <div class="itemChild"><span >Ernesto García Garza</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                    <div class="itemChild"><span >Alonso Alejandro Alvares Arriaga</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                </div>
                            </div>
                            <div class="divItemEquipo">
                                <div class="divItem-title"><span onclick="onClickEquipo(this)">Fernando Barreto - Equipo Fumigación</span><div class="botones"><img src="../comun/img/empty-icon.png" alt="[-]" onclick="vasiarGrupo(this);" /><img src="../comun/img/remove-icon.png" alt="[X]" onclick="borrarEquipo(this);"/></div></div>
                                <div class="divItem-Container">
                                    <div class="itemChild" idEmpleado="34345"><span >José Alfredo Jimenes</span><img src="../comun/img/cross.png" alt="Borrar"  onclick="borrarAsociadoDeGrupo(this);" /></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divGuardar">
                    <input type="button" class="btnGuardar" value="Guardar" onclick="guardar();"/>
                </div>
            </div>
        </div>
    </div>
      
</asp:content>