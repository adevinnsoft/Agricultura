<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmRelacionNivel.aspx.cs" Inherits="frmRelacionNivel" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
	<script src="../Scripts/jComparation.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
	<script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <link href="../comun/css/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../comun/CSS/ui-lightness/jquery-ui-1.8.21.custom.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>

	<script type="text/javascript">
	    var preview = false;
        var nivelP = 0
	    $(function () {

	        $("#<%=divRadiosFamilias.ClientID %> input[type='radio']").change(function () {
	            if ($(this).attr("name") == "familias") {
	                PageMethods.dibujaRadiosNiveles($(this).val().split('|')[0], dibujaCallback);
	            }
	        });
	        triggers();
	    });

        /*redibuja radios de niveles*/
	    function dibujaCallback(result) {
	        try {
	            $("#<%=divRadiosNiveles.ClientID%>").html(result);
	            

	            if (preview) {
	                $('.selNivel').each(
                function () {
                    if ($(this).val() == nivelP) { $(this).attr('checked', 'checked'); }
                    }
                );
	                addGrupo();
	            }
                setTooltips();

	        } catch (err) { }
	    }

	    function showGrupo(familia, nivel) {
	        nivelP = nivel;
	        preview = true;
	        $('.selFamilia').each(
                function () {
                    if ($(this).val().split('|')[0] == familia) {
                        $(this).attr('checked', 'checked');
                        PageMethods.dibujaRadiosNiveles($(this).val().split('|')[0], dibujaCallback);
                    }
                }
            );

        }

	    function addGrupo() {
	        //alert(".");
	        var familia = "", nivel = "", tabla = "", lista = "";
	        $('.selFamilia:checked').each(
                function () {
                    familia = $(this).val();
                }
            );

	        $('.selNivel:checked').each(
                function () {
                    nivel = $(this).val();
                }
            );

	        if (familia != "" && nivel != "") {
	            $('.selAsociados:checked').each(
                function () {
                    lista += "<tr><td><input class='selGuardaNuevo' type='hidden' value='" + familia.split('|')[0] + "|" + nivel + "|" + $(this).val().split('|')[0] + "' /><div class='imgSinguardar' id='img" + $(this).val().split('|')[0] + "' title='Para guardar a este asociado debe guardar los cambios.'/></td><td>" + $(this).val().split('|')[0] + " - " +$(this).val().split('|')[1] + "</td><td style='text-align:center;'><img src='../comun/img/remove-icon.png' alt='eliminar' title='Eliminar' width='20' height='20'  onClick='regresarAsociado(\"" + $(this).val() + "\", this);'> </td></tr>";
                    $(this).attr('disabled', 'disabled')
                    $(this).attr('checked', null)
                    $("#label" + $(this).val().split('|')[0]).text(familia.split('|')[1] + " / " + $(".selNivel:checked").next().text());
                }
                );

                if (lista != "" || preview) {
                        preview = false;
                        if ($("#tabla" + familia.split('|')[0] + "" + nivel).length) {
                            $("#tabla" + familia.split('|')[0] + "" + nivel + " tr:last").after(lista);
                        }
                        else {
                            tabla += "<div class='accordion'>"
                    + "<h3><a href='#'>Familia: " + familia.split('|')[1] + " (Nivel - " + $(".selNivel:checked").next().text() + ")</a></h3>"
                    + "<div>"
                    + "<table  style='width:100%;'><tr>"
                    + "<td style='vertical-align:top;' style='width:50%;'>"
                    + "<div class='grid'><table id='tabla" + familia.split('|')[0] + "" + nivel + "' class='tblGrupo'><tr><th style='width:15px;'><input id='hd" + familia.split('|')[0] + "" + nivel + "' type='hidden' value='0' /></th><th>Nombre</th><th style='width:20px;'>Accion</th></tr>" + lista + "</table></div>"
                    + "</td>"
                    + "</tr></table>"
                    + "</div></div>";


                            $("#<%=divPreview.ClientID %>").html($("#<%=divPreview.ClientID %>").html() + tabla);
                        }

                        if ($("#hd" + familia.split('|')[0] + "" + nivel).val() == 0) {
                            PageMethods.addRows(familia.split('|')[0], nivel, addRowsCallback);
                        }
                        

                        $('.accordion').accordion({ header: 'h3', collapsible: true, heightStyle: "content", autoHeight: false });
                    }
                    else {
                        popUpAlert("Por favor seleccione almenos a uno de los asociados.", 'info');
                    }

	        } else {
	            popUpAlert("Para agregar una relacion primero debe seleccionar familia y nivel.", 'info');
	        }
	    }

	    function addRowsCallback(result) {
	        $("#hd" + result[0]).val(1);
	        $("#tabla" + result[0] + " tr:last").after(result[1]);
	    }


            function regresarAsociado(asociado, row) {
                asociado = asociado.split('|')[0];

                if ($("#img" + asociado).attr('class') == "imgGuardado") {
                    popUpAlertConfirm('<h4>¿Desea quitar al asociado de esta familia y nivel?</h4>',
               '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();"><input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="borrarAsociado(\'' + asociado + '\');">  ', 'warning');

                    
                }
                else {
                    var result = new Array(2);
                    result[0] = "ok";
                    result[1] = "";
                    result[2] = asociado;
                    deleteCallback(result);
                    
                }
            }

            function borrarAsociado(asociado) {
                closeJsPopUpAux();
                PageMethods.borrarAsociado(asociado, deleteCallback);
            }

            function deleteCallback(result) {

                if (result[0] == "ok") {
                    var nodo = $("#img" + result[2]).parent().parent().parent();
                    //coloca al asociado para ser seleccionado nuevamente de la lista
                    $("#img" + result[2]).parent().parent().remove();
                    $("#" + result[2]).attr('disabled', null);
                    $("#label" + result[2]).text('---');
                    
                    //quita tabla si se queda sin tuplas
                    if ($("#" + nodo.parent().attr('id') + " tr").length <= 1) {
                        nodo.parent().parent().parent().parent().parent().parent().parent().parent().fadeOut(function () {
                            $(this).remove();
                            PageMethods.gvRelacionAsociados(gvCallBack);
                        });
                    }
                } else {
                    popUpAlert(result[1], result[0]);
                }
            }


            function btnClean() {
                if ($('.selGuardaNuevo').length) {
                    popUpAlertConfirm('<h4>¿Existen relaciones sin guardar, desea limpiar?</h4>',
               '<input id="cancel" name="btncancel" type="button" value="No" style="width:40px;" onclick="closeJsPopUpAux();"><input id="borra" name="btnborra" type="button" value="Si" style="width:40px;" onclick="cleanCallBack();">  ', 'warning');
                }
                else {cleanCallBack(); } 
            }


            function cleanCallBack() {
                closeJsPopUpAux();
                $('.selGuardaNuevo').each(
                                function () {
                                    $("#" + $(this).val().split('|')[2]).attr('disabled', null);
                                    $("#label" + $(this).val().split('|')[2]).text('---');
                                }
                            );

                $(".imgSinguardar").parent().parent().remove();


                $('.tblGrupo').each(
                                function () {
                                    //if ($("#" + $(this).attr('id') + ' tr').length <= 1) {
                                    $("#" + $(this).attr('id')).parent().parent().parent().parent().parent().parent().parent().fadeOut(function () {
                                        $(this).remove();
                                    });
                                    //}
                                }
                            );
                                $('input[type="checkbox"]:checked').attr('checked', false);
            }

            function btnSave() {
                if ($('.selGuardaNuevo').length) {
                $.blockUI();
                    var asociados = "";
                    //var familias = "";
                    var niveles = "";
                    $('.selGuardaNuevo').each(
                        function () {
                            //familias += '|' + $(this).val().split('|')[0];
                            niveles += '|' + $(this).val().split('|')[1];
                            asociados += '|' + $(this).val().split('|')[2];
                        }
                    );
                    //alert(guardar);
                    PageMethods.guardaRelacion(/*familias,*/ niveles, asociados, saveCallback);
                } else {
                    popUpAlert("No hay asociados que guardar", "info");
                }
            }

            function saveCallback(result) {
                try {
                    $.unblockUI();
                    $(".selGuardaNuevo").remove();
                    $(".imgSinguardar").attr('class', 'imgGuardado');
                    PageMethods.gvRelacionAsociados(gvCallBack);
                    popUpAlert(result[1], result[0]);
                } catch (err) { }
            }

            function gvCallBack(result) {
                $("#<%=divGridView.ClientID %>").html(result);
                gvTablesorter();
            }

            function gvTablesorter() {
                //
                if ($("#gvAsociados").find("tbody").find("tr").size() >= 1) {
                    var pagerOptions = { // Opciones para el  paginador
                        container: $("#pager2"),
                        output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                    };

                    $("#gvAsociados")
				 .tablesorter({
				     widthFixed: true,
				     widgets: ['zebra', 'filter'],
				     headers: { /*0: { filter: false} */
				 },
				 widgetOptions: {
				     zebra: ["gridView", "gridViewAlt"]
				     //filter_hideFilters: true // Autohide
				 }
	}).tablesorterPager(pagerOptions);

                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters
                }
                else {
                    $("#pager2").hide();
                }
            }


	    function triggers() {
	        //$('table.index tr td table.gridView tr th').css('background', null)
	        $('.help').tooltipster({
	            animation: 'fade',
	            delay: 100,
	            theme: 'tooltipster-shadow',
	            touchDevices: false,
	            trigger: 'hover',
	            position: 'right'
	        });

            //
	        gvTablesorter();
            //

	        if ($("#tablaAsociados").find("tbody").find("tr").size() >= 1) {
	            var pagerOptions = { // Opciones para el  paginador
	                container: $("#pager"),
	                output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
	            };

	            $("#tablaAsociados")
				 .tablesorter({
				     widthFixed: true,
				     widgets: ['zebra', 'filter'],
				     headers: { /*0: { filter: false} */
				 },
				 widgetOptions: {
				     zebra: ["gridView", "gridViewAlt"]
				     //filter_hideFilters: true // Autohide
				 }
	            }).tablesorterPager(pagerOptions);

	            $(".tablesorter-filter.disabled").hide(); // hide disabled filters
	        }
	        else {
	            $("#pager").hide();
	        }

	        //registerControls();
	    }


</script>

<script type="text/javascript">
</script>

<style type="text/css">
    input[type="checkbox"], input[type="radio"]  {
        display:none;
    }
    input[type="checkbox"] + label span {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
        cursor:pointer;
    }
    
        input[type="radio"] + label span {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left -39px  top no-repeat;
        cursor:pointer;
    }
    
    input[type="checkbox"]:checked + label span{
        background:url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
    }
    
    input[type="radio"]:checked + label span {
        background:url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
    }
    
    input[type="checkbox"]:disabled + label span {
        /*background:none;*/
        background:url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
    }
    
    input[type="radio"]:disabled + label span {
        background:url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
    }
    
    .check-with-label:checked + .label-for-check {
        font-weight: bold;
        color:#C12929;
    }

    .check-with-label:disabled + .label-for-check {
        color:gray;
    }

    .left
    {
        text-align:left !important;
    }
    
    #ctl00_ContentPlaceHolder1_divPreview h3
    {
        float:none !important;
    }
    #ctl00_ContentPlaceHolder1_divPreview .ui-accordion-content {
        background: #E5EED2;
        width:89%;
    }
    #ctl00_ContentPlaceHolder1_divPreview .ui-accordion {
        min-width: 95%;
        margin: 10px;
    }
    
    .container {
        /*display: block;*/
    }
     
    table.index tr td table.gridView {
        min-width: inherit;
        max-width: inherit;
    }

    .pagedisplay {
        background: transparent !important;
     }
     
     .imgSinguardar
     {         
       background-image: url('../comun/img/smallinfo.png');
       background-repeat: no-repeat;
       background-position: center center;
       background-size:18px;
       height:18px;
     }

     .imgGuardado
     {
       background:  url("../comun/img/smallcheck.png") no-repeat;
       background-repeat: no-repeat;
       background-position: center center;
       background-size:18px;
       height:18px;
     }
    
    .grid
    {
        width:100%;
        max-width: 500px !important;
        float:left;
        /*min-width: 800px !important;*/

    }
    
    .grid table {
        width: 100%;
    }
    
    table.index 
    {
        width:100%;
        max-width: 1000px !important;
        min-width: 1000px !important;
        padding-top: 0px;
    }
    

</style>    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
		<asp:ValidationSummary ID="validaciones" runat="server" ValidationGroup="valida" meta:resourcekey="validacionesResource1" />
		<h1><asp:Label ID="lblTitulo" runat="server" Text="Configuración de Asociados" ></asp:Label></h1>
		<asp:Panel ID="form2" runat="server" meta:resourcekey="formResource1">
			<table class="index">
            	<tr>
                    <td align="left" colspan="2">
                        <h2><asp:Literal ID="ltSubTitulo" Text="Relación de Asociados" runat="server" ></asp:Literal></h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr style=" text-align: left;">
                                <td style="width: 50px; vertical-align:top;">
                                    <asp:Label ID="lbRFamilia" runat="server" Text="<%/*$ Resources: radios*/ %>">*Familia:</asp:Label> 
                                </td>
                                <td class="checkboxes">
                                    <div ID="divRadiosFamilias" runat="server"></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr style=" text-align: left;">
                                <td style="width: 50px; vertical-align:top;">
                                    <asp:Label ID="lbRNivel" runat="server" Text="<%/*$ Resources: radios*/ %>">*Nivel:</asp:Label> 
                                </td>
                                <td class="checkboxes">
                                    <div ID="divRadiosNiveles" runat="server"></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
					<td style="vertical-align: top; width:50%;">
						<div ID="divTablaAsociados" runat="server" class="grid"><input id="grupo" name="grupo" type="button" value="Agregar Grupo" onclick="addGrupo();"/>
                            <div id="pager" class="pager" style=" width:100%; min-width:100%;">
                                <img alt="first" src="../comun/img/first.png" class="first" />
                                <img alt="prev" src="../comun/img/prev.png" class="prev" />
                                <span class="pagedisplay"></span>
                                <img alt="next" src="../comun/img/next.png" class="next" />
                                <img alt="last" src="../comun/img/last.png" class="last" />
                                <select class="pagesize cajaCh" 
                                    style="width: 50px; min-width: 50px; max-width: 50px;">
                                    <option value="30">30</option>
                                    <option value="40">40</option>
                                    <option value="50">50</option>
                                    <option value="60">60</option>
                                    <option value="70">70</option>
                                </select>
                                <select class="gotoPage" title="Select page number">
                                </select>
                            </div>
                        </div>


                        

					</td>
				    <td style="vertical-align: top; width:50%;">
                        <div ID="divPreview" runat="server" class=""/>
                    </td>
				</tr>
                   <tr>
                    <td align="right" colspan="6">
                        <%--<asp:Button ID="btnSave" CssClass="btnSave"  runat="server" Text="Guardar" OnClick="Guardar_Actualizar" ValidationGroup="valida" />--%>
                        <%--<asp:Button ID="btnCancel" runat="server" OnClick="Cancelar_Limpiar" Text="Cancelar" />--%>
                        <input id="save" class="btnSave" name="grupo" type="button" value="Guardar" onclick="btnSave();"/>
                        <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();"/>
                        <%--<asp:HiddenField ID="idTemporal" runat="server" />--%>
                    </td>
                </tr>

			</table>

		</asp:Panel>
		<div class="grid" style=" min-width:100%;">
			<div id="pager2" class="pager" style=" width:100%; min-width:100%;">
				<img alt="first" src="../comun/img/first.png" class="first" />
				<img alt="prev" src="../comun/img/prev.png" class="prev" />
				<span class="pagedisplay"></span>
				<img alt="next" src="../comun/img/next.png" class="next" />
				<img alt="last" src="../comun/img/last.png" class="last" />
				<select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
					<option value="10">10</option>
					<option value="20">20</option>
					<option value="30">30</option>
					<option value="40">40</option>
					<option value="50">50</option>
				</select>
                <select class="gotoPage" title="Select page number"></select>
			</div>
            <div ID="divGridView" runat="server" class="" style=""/>
			<%--<asp:GridView ID="gvTemporales" runat="server" 
                SelectedRowStyle-CssClass="selected" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
				CssClass="gridView" DataKeyNames="idTemporal" EmptyDataText="No existen registros"
				OnPageIndexChanging="gvTemporales_PageIndexChanging" OnPreRender="gvTemporales_PreRender"
				OnRowDataBound="gvTemporales_RowDataBound" OnSelectedIndexChanged="gvTemporales_SelectedIndexChanged"
				OnSorting="gvTemporales_Sorting" OnSorted="gvTemporales_Sorted" 
                meta:resourcekey="gvCategoriaResource1">
				<Columns>
					<asp:TemplateField HeaderText="Activo" HeaderStyle-HorizontalAlign="Center" 
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource1">
						<ItemTemplate><asp:Label ID="lblActivo" runat="server" 
                                Text='<%# (bool)Eval("Activo")==true?(string)GetGlobalResourceObject("Commun","Si"):(string)GetGlobalResourceObject("Commun","No") %>' 
                                meta:resourcekey="lblActivoResource1"/></ItemTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>
                    <asp:TemplateField HeaderText="Repetir anualmente" 
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" 
                        meta:resourcekey="TemplateFieldResource2">
						<ItemTemplate><asp:Label ID="lblRepetir" runat="server" 
                                Text='<%# (bool)Eval("RepetirAnual")==true?(string)GetGlobalResourceObject("Commun","Si"):(string)GetGlobalResourceObject("Commun","No") %>' 
                                meta:resourcekey="lblRepetirResource1"/></ItemTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>
                    <asp:BoundField HeaderText="Fecha inicio" 
                        DataField="FechaStart" ItemStyle-HorizontalAlign="Right" 
                        DataFormatString="{0:dd/MMM/yyyy}" meta:resourcekey="BoundFieldResource1">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha fin" 
                        DataField="FechaEnd" ItemStyle-HorizontalAlign="Right" 
                        DataFormatString="{0:dd/MMM/yyyy}" meta:resourcekey="BoundFieldResource2">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Eficiencia" HeaderText="Eficiencia" 
                        meta:resourcekey="BoundFieldResource3"/>
                    <asp:BoundField DataField="NombreGv" HeaderText="Nombre" 
                        meta:resourcekey="BoundFieldResource4"/>					
                    <asp:BoundField DataField="DescripcionGv" HeaderText="Descripcion" 
                        meta:resourcekey="BoundFieldResource5"/>			
                    <asp:TemplateField meta:resourcekey="BoundFieldResource6" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" HeaderText="Color">
                        <ItemTemplate>
                            <center>
                                <div style="width: 16px; height: 16px; border:1px black solid; background-color: #<%#Eval("Color")%>"></div>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>		
				</Columns>
				<SelectedRowStyle CssClass="selected"></SelectedRowStyle>
			</asp:GridView>
		</div>
		<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />--%>
	</div>
    </div>


    </div>


</asp:Content>