<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmTemporales.aspx.cs" Inherits="catalogos_frmTemporales" meta:resourcekey="PageResource1" %>
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
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>

	<script type="text/javascript">
	    $(function () {
	        
	        triggers();
	    });


	    function triggers() {
	        $('#ctl00_ContentPlaceHolder1_txtColorP').change(function () {
	            setColor();
	        });


	        $("#<%=txtFechaEnd.ClientID %>").change(function () {
	            if (Date.parse($("#<%=txtFechaStart.ClientID %>").val()) >= Date.parse($("#<%=txtFechaEnd.ClientID %>").val())) {
	                $("#<%=txtFechaStart.ClientID %>").addClass("Error");
	                $("#<%=txtFechaEnd.ClientID %>").addClass("Error");
	            }
	            else {
	                $("#<%=txtFechaStart.ClientID %>").removeClass("Error");
	                $("#<%=txtFechaEnd.ClientID %>").removeClass("Error");
	            }
	        });

	        $('.help').tooltipster({
	            animation: 'fade',
	            delay: 100,
	            theme: 'tooltipster-shadow',
	            touchDevices: true,
	            trigger: 'hover',
	            position: 'right'
	        });

	        setTooltips();

	        registerControls();

	        $(".datepicker").datepicker('destroy');
	        $(".datepicker").datepicker(
                        {
                            dateFormat: "dd-mm-yy",
                            buttonImage: "../comun/img/calendario.png",
                            showOn: "both",
                            dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
                            dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                            dayNamesShort: ["Dom", "Lun", "Mar", "Mier", "Jue", "Vie", "Sab"],
                            monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                            monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                            changeYear: false,
                            changeMonth: true,
                            minDate: new Date((new Date).getFullYear(), 00, 01)//,
                            //maxDate: new Date($('#<%=ddlAño.ClientID%>').val(), 11, 31)
                            //maxDate: new Date(date),
                            //minDate: new Date(year,minda,1)
                        }
                    );

                    //setTooltips();

            $('#<%=ddlAño.ClientID%>').on('change', function () {
                PageMethods.dibujaLinea($('#<%=ddlAño.ClientID%>').val(), dibujaCallback);
            });
	    }

	    function setTooltips() {
	        //$('.tooltip').tooltipster('destroy');
	        $('.tooltip').tooltipster({
	            animation: 'grow',
	            delay: 200,
	            theme: 'tooltipster-punk',
	            touchDevices: false,
	            trigger: 'hover',
	            contentAsHTML: true,
	            interactive: true
	        });


	    }
	    function setColor() {
	        var txtcolor = new jscolor.color(document.getElementById('<%=txtColorP.ClientID %>'));
	    }

	    function moveSelect(mov) {
	        var selected = $('#<%=ddlAño.ClientID%>').find(":selected");
	        $("#tablita").disableSelection();

	        if (mov == 'prev') {
	            var before = selected.prev();
	            before.attr('selected', '');
	        } else {
	            var next = selected.next();
	            next.attr('selected', '');
	        }
	        if (selected.val() != $('#<%=ddlAño.ClientID%>').find(":selected").val()) {
	            //$('#<%=lbDisplay.ClientID%>').text($('#<%=lbDisplay.ClientID%>').text().split(":")[0] + ": " +$('#<%=ddlAño.ClientID%>').val())
	            PageMethods.dibujaLinea($('#<%=ddlAño.ClientID%>').val(), dibujaCallback);
	        }
	    }

	    /*redibuja la linea de tiempo segun el año*/
	    function dibujaCallback(result) {
	        try {
	            //$.blockUI();
	            $("#<%=divTiempo.ClientID%>").html(result);
	            setTooltips();
	        } catch (err) { }
	    }

    </script>

<style type="text/css">

        input.Error {
            border: 1px solid red;
            background: rgba(255,0,0,0.2);
        }
         .tablesorter-filter {
	        width: 98%;
         }
         .previo
         {
             display:inline;
             background-image: url('../comun/img/left.png');
             background-repeat: no-repeat;
             background-position: center center;
             padding:18px;
             padding-top: 22px;
             -webkit-touch-callout: none; -webkit-user-select: none; -khtml-user-select: none; -moz-user-select: moz-none; -ms-user-select: none; user-select: none;
             transition-duration: 0.5s;
             
         }
         .previo:hover
         {
             background-image: url('../comun/img/lefth.png');
             /*transform: scale(1.2);*/
         }
         
         .siguiente
         {
             display:inline;
             background-image: url('../comun/img/right.png');
             background-repeat: no-repeat;
             background-position: center center;
             padding:18px;
             padding-top: 22px;
             -webkit-touch-callout: none; -webkit-user-select: none; -khtml-user-select: none; -moz-user-select: moz-none; -ms-user-select: none; user-select: none;
             transition-duration: 0.5s;
         }
         
         .siguiente:hover
         {
            background-image: url('../comun/img/righth.png');
            /*transform: scale(1.2);*/
         }

    .pickcolor
    {
        border:1px black solid; 
        font-size:1px !important;
        text-align:center !important;
        width: 28px !important;
        height: 28px;
        background: url(../comun/img/select2.png) center;
        cursor: pointer;
    }

    .lineaMeses {
        overflow: hidden;
        position:relative;
        height: 20px;
        margin-bottom: 20px;
    }

    .lineaTemporal 
    {
        overflow: hidden;
        position:relative;
        height: 20px;
        background-color: #f5f5f5;
        -webkit-box-shadow: inset 0 1px 2px rgba(0,0,0,0.1);
        box-shadow: inset 0 1px 2px rgba(0,0,0,0.1);
        width:100%;
        height:60px;
        background-image: -webkit-linear-gradient(top, #CCCCCC 0%, #f5f5f5 100%); 
        background-repeat: repeat-x;
        background-image: -o-linear-gradient(top, #CCCCCC 0%, #f5f5f5 100%);
        background-image: -webkit-gradient(linear, left top, left bottom, from(#CCCCCC), to(#f5f5f5)); 
        background-image: linear-gradient(to bottom, #CCCCCC 0%, #f5f5f5 100%); 
        border: 1px solid #ADC995;
    }
  
    .lmeses
    { 
        border-left: 1px #ADC995 solid;
        text-align: left;
        padding: 3px;
    }
  
    .ltemporales
    { 
        transition-duration: 0.2s;
        height:60px;
        z-index:1;
    }
  
    .ltemporales:hover
    { 
        z-index:2;
        box-shadow: inset 0 0 0 1px black;
    }

    /*Checkbox y radiobutton*/
    input[type="checkbox"], input[type="radio"]  {
        display:none;
    }
    input[type="checkbox"] + label {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
        cursor:pointer;
    }
    
        input[type="radio"] + label {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left -39px  top no-repeat;
        cursor:pointer;
    }
    
    input[type="checkbox"]:checked + label{
        background:url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
    }
    
    input[type="radio"]:checked + label {
        background:url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
    }
    
    input[type="checkbox"]:disabled + label {
        /*background:none;*/
        background:url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
    }
    
    input[type="radio"]:disabled + label {
        background:url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
    }
    
    .check-with-label:checked + .label-for-check {
        font-weight: bold;
        color:#C12929;
    }

    .check-with-label:disabled + .label-for-check {
        color:gray;
    }
    
  
</style>    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
		<asp:ValidationSummary ID="validaciones" runat="server" ValidationGroup="valida" meta:resourcekey="validacionesResource1" />
		<h1><asp:Label ID="lblTitulo" runat="server" Text="Temporales" 
                meta:resourcekey="lblTituloResource1"></asp:Label></h1>
		<asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
			<table class="index">
            	<tr>
                    <td colspan="6" align="left">
                        <h2><asp:Literal ID="ltSubTitulo" Text="Configuración de Temporales" runat="server" 
                                meta:resourcekey="ltSubTituloResource1"></asp:Literal></h2>
                    </td>
                </tr>
				<tr>
					<td align="right" rowspan="2" style="vertical-align:text-top;">
                        <asp:Literal ID="lblTemporal" runat="server" Text="*Temporal:" 
                            meta:resourcekey="lblTemporalResource1"></asp:Literal>
                    </td>
                    <td style="text-align:left;">
                        <asp:TextBox runat="server" ID="txtTemporal" MaxLength="100" Width="200px" 
                            CssClass="required stringValidate help" ToolTip="<%$ Resources:Commun, in_ES %>" 
                            meta:resourcekey="txtTemporalResource1"></asp:TextBox>
                        <asp:Label ID="lt_ES" runat="server"  CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_ES %>" meta:resourcekey="lt_ESResource1"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Literal ID="ltFechaStart" runat="server" Text="*Fecha inicio:" 
                            meta:resourcekey="ltFechaStartResource1"></asp:Literal>
                    </td>
                    <td style="text-align:left;">
                        <asp:TextBox ID="txtFechaStart" runat="server" 
                            CssClass="required cajaLarga datepicker noValue" Width="100px" contentEditable="false" 
                            meta:resourcekey="txtFechaStartResource1"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:Label ID="ltActivo" runat="server" Text="<%$ Resources:Commun, Active %>" 
                            AssociatedControlID="chkActivo" meta:resourcekey="ltActivoResource1" ></asp:Label>
                    </td>
					<td style="text-align:left;">
                        <%--<asp:CheckBox ID="chkActivo" runat="server" Checked="True" />--%>
                        <input checked="checked"  type="checkbox" ID="chkActivo" runat="server" class="check-with-label" />
                        <label  id="Label1"  class='label-for-check' for="<%=chkActivo.ClientID %>" ><span></span></label>
                    </td>
                </tr>
                <tr>
                    <td style="display:block; text-align:left;">
                        <asp:TextBox runat="server" ID="txtTemporal_EN" MaxLength="100" Width="200px" 
                            CssClass="required stringValidate help" ToolTip="<%$ Resources:Commun, in_EN %>" 
                            meta:resourcekey="txtTemporal_ENResource1"></asp:TextBox>
                        <asp:Label ID="lt_EN" runat="server"  CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_EN %>" meta:resourcekey="lt_ENResource1"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Literal ID="ltFechaEnd" runat="server" Text="*Fecha fin:" 
                            meta:resourcekey="ltFechaEndResource1"></asp:Literal>
                    </td>
                    <td style="text-align:left;">
                        <asp:TextBox ID="txtFechaEnd" runat="server" 
                            CssClass="required cajaLarga datepicker noValue" Width="100px" contentEditable="false"  
                            meta:resourcekey="txtFechaEndResource1"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:Label ID="Literal1" runat="server" Text="Repetir anualmente:" 
                            AssociatedControlID="chkRepetir" meta:resourcekey="Literal1Resource1"></asp:Label>
                    </td>
					<td style="text-align:left;">
                        <%--<asp:CheckBox ID="chkRepetir" runat="server" Checked="True"/>--%>
                        <input checked="checked"  type="checkbox" ID="chkRepetir" runat="server" class="check-with-label" />
                        <label  id="Label4"  class='label-for-check' for="<%=chkRepetir.ClientID %>" ><span></span></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                    <td align="right">
                        <asp:Label ID="ltBorrado" runat="server" Text="Borrar?:" 
                            AssociatedControlID="chkBorrar" meta:resourcekey="ltBorradoResource1"></asp:Label>
                    </td>
					<td style="text-align:left;">
                        <%--<asp:CheckBox ID="chkBorrar" runat="server" Checked="False" />--%>
                        <input checked="checked"  type="checkbox" ID="chkBorrar" runat="server" class="check-with-label" />
                        <label  id="Label5"  class='label-for-check' for="<%=chkBorrar.ClientID %>" ><span></span></label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" style="vertical-align:text-top;">
                        <asp:Literal ID="ltDescripcion" runat="server" Text="Descripción:" 
                            meta:resourcekey="ltDescripcionResource1"></asp:Literal>
                    </td>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtDescripcion_ES" CssClass="help" runat="server" 
                            TextMode="MultiLine" Rows="3" Columns="50" 
                            ToolTip="<%$ Resources:Commun, in_ES %>" 
                            meta:resourcekey="txtDescripcion_ESResource1"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_ES %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal ID="ltEficiencia" runat="server" Text="*Eficiencia:" 
                            meta:resourcekey="ltEficienciaResource1"></asp:Literal>
                    </td>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtEficiencia" CssClass="required floatValidate" runat="server" Width="40px" MaxLength="5" 
                            meta:resourcekey="txtEficienciaResource1"></asp:TextBox>
                        <asp:Label ID="ltPociento" runat="server" Text="%" 
                            meta:resourcekey="ltPocientoResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtDescripcion_EN" CssClass="help" runat="server" 
                            TextMode="MultiLine" Rows="3" Columns="50" 
                            ToolTip="<%$ Resources:Commun, in_EN %>" 
                            meta:resourcekey="txtDescripcion_ENResource1"></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_EN %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal ID="ltColor" runat="server" Text="Color:" 
                            meta:resourcekey="ltColorResource1"></asp:Literal>
                    </td>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtColorP" runat="server" Width="20px" 
                            class="required color  pickcolor" 
                            meta:resourcekey="txtColorPResource1"></asp:TextBox>
                    </td>
                </tr>

				<tr>
                    
					<td colspan="6" align="right">
						<asp:Button ID="btnSave" CssClass="btnSave"  runat="server" Text="Guardar" 
                            OnClick="Guardar_Actualizar" ValidationGroup="valida" 
                            meta:resourcekey="btnSaveResource1"/>
						<asp:Button ID="btnCancel" runat="server" Text="Cancelar" 
                            OnClick="Cancelar_Limpiar" meta:resourcekey="btnCancelResource1"/>
					    <asp:HiddenField ID="idTemporal" runat="server" />
					</td>
				</tr>
			</table>

<table class="index" style=" width:100%; min-width:100%; /*display:none;*/">
    <tr>
        <td>
            <h2><asp:Label ID="lbTiempo" runat="server" Text="<%$ Resources: linea %>">Línea de tiempo</asp:Label></h2>
            <center>
            <table>				<tr>
					<td>
                        <%--<asp:Label ID="ltAño" runat="server" Text="Año:" 
                            meta:resourcekey="ltAñoResource1"></asp:Label>--%><h3 style="float:none;"><asp:Label ID="lbDisplay" runat="server" Text="<%$ Resources: display %>">Visualización de temporales: </asp:Label></h3>
                    </td>
					<td colspan="5">
                    <table id="tablita">
                        <tr>
                            <td>
                                <img class='previo' onclick="moveSelect('prev');" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAño" runat="server" CssClass="semana"> </asp:DropDownList>
                            </td>
                            <td>
                                <img class='siguiente' onclick="moveSelect('next');" />
                            </td>
                        </tr>
                    </table>

                    </td>
				</tr>
</table>
            </center>
        </td>
    </tr>
    <tr>
        <td>
            <div class="containerbar">   
                <div class="lineaTemporal" id="divTiempo" runat="server"></div>
                <div class="lineaMeses" id="divMeses" runat="server"></div>
            </div>
        </td>
    </tr>
</table>

		</asp:Panel>
		<div class="grid">
			<div id="pager" class="pager" style=" width:100%; min-width:100%;">
				<img alt="first" src="../comun/img/first.png" class="first" />
				<img alt="prev" src="../comun/img/prev.png" class="prev" />
				<span class="pagedisplay"></span><%--<input type="text" class="pagedisplay" />--%>
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
			<asp:GridView ID="gvTemporales" runat="server" 
                SelectedRowStyle-CssClass="selected" AutoGenerateColumns="False" AllowSorting="True"
				CssClass="gridView" DataKeyNames="idTemporal" EmptyDataText="No existen registros"
				OnPageIndexChanging="gvTemporales_PageIndexChanging" OnPreRender="gvTemporales_PreRender"
				OnRowDataBound="gvTemporales_RowDataBound" OnSelectedIndexChanged="gvTemporales_SelectedIndexChanged"
				OnSorting="gvTemporales_Sorting" OnSorted="gvTemporales_Sorted" 
                meta:resourcekey="gvCategoriaResource1">
				<Columns>
					<asp:TemplateField HeaderText="Activo" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cajaCh"
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource1">
						<ItemTemplate><asp:Label ID="lblActivo" runat="server" 
                                Text='<%# (bool)Eval("Activo")==true?(string)GetGlobalResourceObject("Commun","Si"):(string)GetGlobalResourceObject("Commun","No") %>' 
                                meta:resourcekey="lblActivoResource1"/></ItemTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>
                    <asp:TemplateField HeaderText="Repetir anualmente"  HeaderStyle-CssClass="cajaMed"
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
                    <asp:BoundField HeaderText="Fecha fin" HeaderStyle-CssClass="cajaMed" 
                        DataField="FechaEnd" ItemStyle-HorizontalAlign="Right" 
                        DataFormatString="{0:dd/MMM/yyyy}" meta:resourcekey="BoundFieldResource2">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Eficiencia" HeaderText="Eficiencia" HeaderStyle-CssClass="cajaMed"
                        meta:resourcekey="BoundFieldResource3"/>
                    <asp:BoundField DataField="NombreGv" HeaderText="Nombre" HeaderStyle-CssClass="cajaMed" 
                        meta:resourcekey="BoundFieldResource4"/>					
                    <asp:BoundField DataField="DescripcionGv" HeaderText="Descripcion" HeaderStyle-CssClass="cajaMed" 
                        meta:resourcekey="BoundFieldResource5"/>			
                    <asp:TemplateField meta:resourcekey="BoundFieldResource6" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cajaCh"
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
		<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
	</div>
</asp:Content>