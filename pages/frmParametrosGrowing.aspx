<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmParametrosGrowing.aspx.cs" Inherits="pages_frmParametrosGrowing" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
	<script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
	<script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                var spanish = '<%= Session["Locale"].ToString() %>' == 'es-MX' ? true : false;
                //1.- Obtener los grupos
                PageMethods.ObtenerGrupos(function (response) {
                    if (response[0] == 'ok')
                        $('#GruposGrowing').html(response[1]);
                    else
                        popUpAlert(spanish ? response[1] : response[2], response[0]);
                });
            });
        }

        function obtenerParametrosDelGrupo(btnGrupo) {

            var idGrupo = $(btnGrupo).attr('idGrupo');
            var nombreGrupo = $(btnGrupo).find('label').text();
 

            if (!$(btnGrupo).hasClass('selected')) {
                $(btnGrupo).addClass('selected');
                PageMethods.ObtenerParametrosDeGrupo(idGrupo, nombreGrupo, function (response) {
                    $('#ParemetrosPorGrupo').append(response);
                });
            }
            else {
                $(btnGrupo).removeClass('selected');
                $('#ParemetrosPorGrupo [idGrupo=' + idGrupo + ']').remove();
            }
        }
       

        function AgregaParametro(obj) {
            obj.parents('.grupo').each(function () {
                if ($(this).attr('idGrupo') == obj.parent().parent().parent().parent().parent().attr('idGrupo')) {
                    var response = '<table id="NAOKX" class="index" style="background:#F0F5E5;">                                                                                                                                                                                       ' +
                        '   <tr idParametro = "0">                                                                                                                                                                                                     ' +
                        '       <td>Nombre</td><td><input /></td>                                                                                                                                                                    ' +
                        '       <td>Nombre (Inglés)</td><td><input /></td>                                                                                                                                                           ' +
                        '       <td>Puntaje Plantación</td><td><input class="cajaCh" /></td>                                                                                                                                         ' +
                        '       <td>Puntaje No Plantación</td><td><input class="cajaCh" /></td>                                                                                                                                      ' +
                        '       <td>Activo</td><td><input type="checkbox" checked /></td>                                                                                                                                            ' +
                        '       <td><img src="../comun/img/remove-icon.png" onclick="EliminaParametro($(this));" />&nbsp;&nbsp;<img src="../comun/img/add-icon.png" onclick="AgregaParametro($(this));" /></td>' +
                        '   </tr>                                                                                                                                                                                                    ' +
                        '   <tr>                                                                                                                                                                                                     ' +
                        '       <td> NA, OK y X</td>                                                                                                                                                                                  ' +
                        '       <td colspan="5">                                                                                                                                                                                     ' +
                        '           <table class="index2" style="background:#F0F5E5;">                                                                                                                                                                           ' +
                        '                <thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>                                                                                               ' +
                        '                <tr idParametroNAOKX = "0"><td><input /></td><td><input /></td><td><input type="checkbox" checked/></td><td><img src="../comun/img/remove-icon.png" onclick="EliminaParametroNAOKX($(this));" />&nbsp;&nbsp;<img src="../comun/img/add-icon.png"  onclick="AgregaParametroNAOKX($(this));"/></td></tr> ' +
                        '           </table>                                                                                                                                                                                         ' +
                        '       </td>                                                                                                                                                                                                ' +
                        '                                                                                                                                                                                                            ' +
                        '       <td>S, A, G y NA</td>                                                                                                                                                                                ' +
                        '       <td colspan="5">                                                                                                                                                                                     ' +
                        '           <table class="index2" style="background:#F0F5E5;">                                                                                                                                                                           ' +
                        '                <thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>                                                                                               ' +
                        '                <tr idParametroSAGNA="0"><td><input /></td><td><input /></td><td><input type="checkbox" checked/></td><td><img src="../comun/img/remove-icon.png" onclick="EliminaParametroSAGN($(this));" />&nbsp;&nbsp;<img src="../comun/img/add-icon.png"  onclick="AgregaParametroSAGN($(this));"  /></td></tr> ' +
                        '           </table>                                                                                                                                                                                         ' +
                        '       </td>                                                                                                                                                                                                ' +
                        '   </tr>                                                                                                                                                                                                    ' +
                        '</table>                                                                                                                                                                                           ';

                    $('#ParemetrosPorGrupo [idGrupo=' + $(this).attr('idGrupo') + ']').append(response);
                }
            });
        }


        function AgregaParametroNAOKX(obj) {
            var response = '<tr idParametroNAOKX = "0"><td><input /></td><td><input /></td><td><input type="checkbox" checked/></td><td><img src="../comun/img/remove-icon.png" onclick="EliminaParametroNAOKX($(this));" />&nbsp;&nbsp;<img src="../comun/img/add-icon.png"  onclick="AgregaParametroNAOKX($(this));" /></td></tr> ';
            obj.parent().parent().after(response);
        }

        function AgregaParametroSAGN(obj) {
            var response = '<tr idParametroSAGNA="0"><td><input /></td><td><input /></td><td><input type="checkbox" checked/></td><td><img src="../comun/img/remove-icon.png" onclick="EliminaParametroSAGN($(this));" />&nbsp;&nbsp;<img src="../comun/img/add-icon.png"  onclick="AgregaParametroSAGN($(this));" /></td></tr> ';
            obj.parent().parent().after(response);
        }


        function EliminaParametro(obj) {
            obj.parents('table .index').addClass('Deleted');
        }

        function EliminaParametroNAOKX(obj) {
            obj.parent().parent().addClass('Deleted');
        }

        function EliminaParametroSAGN(obj) {
            obj.parent().parent().addClass('Deleted');
        }


    </script>
    <script id="IntercambiarPorPageMethods" type="text/javascript">
        function ObtenerParametrosDeGrupo(idGrupo) {
            var retorno = '<span class="grupo" idGrupo  ="' + idGrupo + '"><h3>' + nombreGrupo + '</h3><table class="index" style="background:#F0F5E5;">                                                                                                                                                                                       ' +
                        '   <tr>                                                                                                                                                                                                     ' +
                        '       <td>Nombre</td><td><input /></td>                                                                                                                                                                    ' +
                        '       <td>Nombre (Inglés)</td><td><input /></td>                                                                                                                                                           ' +
                        '       <td>Puntaje Plantación</td><td><input class="cajaCh" /></td>                                                                                                                                         ' +
                        '       <td>Puntaje No Plantación</td><td><input class="cajaCh" /></td>                                                                                                                                      ' +
                        '       <td>Activo</td><td><input type="checkbox" checked /></td>                                                                                                                                            ' +
                        '       <td><img src="../comun/img/remove-icon.png">&nbsp;&nbsp;<img src="../comun/img/add-icon.png"></td>' +
                        '   </tr>                                                                                                                                                                                                    ' +
                        '   <tr>                                                                                                                                                                                                     ' +
                        '       <td> id="NAOKX" NA, OK y X</td>                                                                                                                                                                                  ' +
                        '       <td colspan="5">                                                                                                                                                                                     ' +
                        '           <table class="index2" style="background:#F0F5E5;">                                                                                                                                                                           ' +
                        '                <thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>                                                                                               ' +
                        '                <tr><td><input /></td><td><input /></td><td><input type="checkbox" checked/></td><td><img src="../comun/img/remove-icon.png" >&nbsp;&nbsp;<img src="../comun/img/add-icon.png" /></td></tr> ' +
                        '           </table>                                                                                                                                                                                         ' +
                        '       </td>                                                                                                                                                                                                ' +
                        '                                                                                                                                                                                                            ' +
                        '       <td>S, A, G y NA</td>                                                                                                                                                                                ' +
                        '       <td colspan="5">                                                                                                                                                                                     ' +
                        '           <table class="index2" style="background:#F0F5E5;">                                                                                                                                                                           ' +
                        '                <thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>                                                                                               ' +
                        '                <tr><td><input /></td><td><input /></td><td><input type="checkbox" checked/></td><td><img src="../comun/img/remove-icon.png" >&nbsp;&nbsp;<img src="../comun/img/add-icon.png" /></td></tr> ' +
                        '           </table>                                                                                                                                                                                         ' +
                        '       </td>                                                                                                                                                                                                ' +
                        '   </tr>                                                                                                                                                                                                    ' +
                        '</table>  </span>                                                                                                                                                                                           ';
            return retorno;
        }
    
    </script>

    <style type="text/css">
        table.inicio
        {
            text-align: left;
            border: 1px solid #adc995;
            background: #f0f5e5;
            font-size: 12px;
            margin-top: 0px;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
            width: 800px;
            max-width: 800px;
            min-width: 800px;
            padding-bottom: 13px;
        }
        table.inicio span.grupoGrowing
        {
            display: inline-block;
            padding: 3px;
            min-width: 250px;
            max-width: 250px;
            overflow: hidden;
            background-color: #FFFFFF;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
            cursor:pointer;
            border: 1px solid orange;
        }
         table.inicio span.grupoGrowing.selected
        {
             background-color: #ADC995;
        }
        .ParametrosPorGrupo th {
            overflow: hidden;
            text-overflow: ellipsis;
            max-width: 90px;
        }
        .grupo h3
        {
            width:100%;
            background:#ADC995;
            padding-top:5px;
            padding-bottom:5px;
            text-align:center;
            }
            
         .Deleted
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class = "container">
        <h1>Parmámetros de Evaluación</h1>


        

       



        <table class="inicio"> <%--El Contenido de este div debe ser obtenido dinámicamente--%>
            <tr>
                <td>
                    <input type="button" value="Eliminar Selección" />
                    <input type="button" value="Seleccionar Todo" />
                </td>
            </tr>
            <tr>
                <td id="GruposGrowing">
                    <span class="grupoGrowing" idGrupo="1">
                        <label>Plantación</label>
                   </span>
                 </td>
             </tr>
             <tr>
                <td id="ParemetrosPorGrupo">
                
                </td>
             </tr>
        </table>


       <div >
           
       </div>

     <%--   <div class="grid">
        <div id="Div1" class="pager">
				<img alt="first" src="../comun/img/first.png" class="first" />
				<img alt="prev" src="../comun/img/prev.png" class="prev" />
				<input type="text" class="pagedisplay" />
				<img alt="next" src="../comun/img/next.png" class="next" />
				<img alt="last" src="../comun/img/last.png" class="last" />
				<select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
					<option value="10">10</option>
					<option value="20">20</option>
					<option value="30">30</option>
					<option value="40">40</option>
					<option value="50">50</option>
				</select>
			</div>
			<asp:GridView ID="GridView1" runat="server"></asp:GridView>
        </div>--%>
    </div>
</asp:Content>

