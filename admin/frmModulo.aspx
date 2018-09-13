<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmModulo.aspx.cs" Inherits="Administration_frmModulo" ValidateRequest="false" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
	<script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
	<script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>

<%-- Paginador con busqueda rapida
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <link href="../Styles/jquery.dataTables.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.dataTables.js" type="text/javascript"></script>
    <script type="text/javascript">
        var j = jQuery.noConflict();
        j(function () {
            var lang = '<%=this.Session["Locale"]%>';
            if (lang != 'es-MX' && lang != 'en-US')
                lang = 'es-MX';
            j("#<%=gvModulo.ClientID%>").dataTable({
                language: {
                    url: '../Scripts/' + lang + '.json'
                }
            });
        });
    </script>--%>
<link rel="stylesheet" type="text/css" href="../comun/css/ts_theme.jui.css" />
	<script type="text/javascript">
	    $(function () {
	        registerControls();
	    });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
		<h1><asp:Label ID="lblBienvenido" runat="server" meta:resourceKey="lblBienvenido"></asp:Label></h1>
		<asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
			<table class="index">
				<tr><td colspan="4" align="left"><h2><asp:Literal ID="ltSubtituli" meta:resourceKey="ltSubtituli" runat="server"></asp:Literal></h2></td></tr>
				<tr>
					<td align="right"><asp:Literal ID="ltModulo" runat="server" meta:resourceKey="ltModulo"></asp:Literal></td>
					<td style="display: block; text-align: left;">
						<asp:TextBox runat="server" ID="txtModulo" MaxLength="30" ToolTip="<%$ Resources:Commun, in_ES %>"></asp:TextBox>
						<asp:Label ID="lt_ES" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label>
					</td>
					<td align="right"><asp:Label ID="ltActivo" AssociatedControlID="chkActivo" runat="server" Text="<%$ Resources:Commun, Active %>" ></asp:Label></td>
					<td><asp:CheckBox ID="chkActivo" runat="server" Checked="True" meta:resourcekey="chkActivoResource1" /></td>
				</tr>
				<tr>
					<td align="right"></td>
					<td style="display: block; text-align: left;">
						<asp:TextBox runat="server" ID="txtModulo_EN" MaxLength="30" ToolTip="<%$ Resources:Commun, in_EN %>"></asp:TextBox>
						<asp:Label ID="lt_EN" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label>
					</td>
                    <td align="right"><asp:Literal ID="ltOrden" runat="server" meta:resourceKey="ltOrden"></asp:Literal></td>
					<td><asp:TextBox runat="server" ID="txtOrden" MaxLength="3" CssClass="intValidate required" meta:resourcekey="txtOrdenResource1"></asp:TextBox></td>
				</tr>
				<tr>
					<td align="right"><asp:Literal ID="ltRuta" runat="server" meta:resourceKey="ltRuta"></asp:Literal></td>
					<td><asp:TextBox runat="server" ID="txtRuta" CssClass="cajaLarga" Width="380px" MaxLength="250" meta:resourcekey="txtRutaResource1"></asp:TextBox></td>
					<td>
                    <%--<asp:literal ID="ltAndroid" runat="server" Text="Android"/>--%>
                    </td>
                    <td>
                    <%--<asp:CheckBox ID="ckAndroid" runat="server" />--%>
                    </td>
				</tr>
				<tr>
					<td colspan="4" align="right">
						<asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
						<asp:Button ID="btnActualizar" runat="server" meta:resourceKey="btnActualizar" Visible="False" OnClick="Guardar_Actualizar" />
						<asp:Button ID="btnSave" runat="server" meta:resourceKey="btnSave" OnClick="Guardar_Actualizar" />
						<asp:Button ID="btnLimpiar" runat="server" meta:resourceKey="btnLimpiar" OnClick="Cancelar_Limpiar" />
						<asp:Button ID="btnCancel" runat="server" meta:resourceKey="btnCancel" OnClick="Cancelar_Limpiar" Visible="False" />
					</td>
				</tr>
			</table>
		</asp:Panel>
		
		<div class="grid">
			<div id="pager" class="pager">
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
			</div>
			<asp:GridView ID="gvModulo" runat="server" EnableModelValidation="True" 
				AutoGenerateColumns="False" CssClass="gridView" DataKeyNames="idModulo" meta:resourceKey="gvModulo"
				OnPageIndexChanging="gvModulo_PageIndexChanging" SelectedRowStyle-CssClass="selected"
				OnPreRender="gvModulo_PreRender" OnRowDataBound="gvModulo_RowDataBound" OnSelectedIndexChanged="gvModulo_SelectedIndexChanged">
				<Columns>
					<asp:TemplateField meta:resourceKey="htActivo" SortExpression="activo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cajaCh">
						<EditItemTemplate><asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("bActivo") %>' meta:resourcekey="CheckBox1Resource1" /></EditItemTemplate>
						<ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("bActivo")==true? GetLocalResourceObject("lblActivoGridSi") :GetLocalResourceObject("lblActivoGridNo") %>' meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>
<%--					<asp:TemplateField meta:resourceKey="HTandroid" SortExpression="android" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cajaCh">
						<EditItemTemplate><asp:CheckBox ID="cbAndroid" runat="server" Checked='<%# Bind("Android") %>' meta:resourcekey="CheckBox1Resource1" /></EditItemTemplate>
						<ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("Android")==true? GetLocalResourceObject("lblActivoGridSi") :GetLocalResourceObject("lblActivoGridNo") %>' meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>--%>
					<asp:BoundField DataField="vModulo" meta:resourceKey="bfModulo" HeaderStyle-CssClass="cajaMed"/>
					<asp:BoundField DataField="vRuta" meta:resourceKey="bfRuta" ItemStyle-CssClass="wrapped">
						<ItemStyle CssClass="wrapped"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="iOrden" meta:resourceKey="bfOrden" HeaderStyle-CssClass="cajaCh"/>
				</Columns>
				<SelectedRowStyle CssClass="selected"></SelectedRowStyle>
			</asp:GridView>
		</div>
		<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
	</div>
</asp:Content>