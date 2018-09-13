<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmRol.aspx.cs" Inherits="Administration_frmRol" ValidateRequest="false" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<%--<script type="text/javascript" src="../comun/scripts/jquery-ui-1.8.21.custom.min.js"></script>
	<script type="text/javascript" src="../comun/scripts/jquery.ui.accordion.js"></script>--%>
	<script type="text/javascript">
	    $(function () {
	        registerControls();
	    });
	</script>

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
            j("#<%=gvRol.ClientID%>").dataTable({
                language: {
                    url: '../Scripts/' + lang + '.json'
                }
            });
        });
    </script>--%>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="container">
		<h1>
			<asp:Label ID="lblBienvenido" runat="server" meta:resourceKey="lblBienvenido"></asp:Label></h1>
		<table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
			<tr>
                    <td colspan="6" align="left">
                        <h2><asp:Literal ID="ltSubTitulo" runat="server" 
                                meta:resourcekey="ltSubTitulo"></asp:Literal></h2>
                    </td>
            </tr>
            <tr>
				<td align="right" style="width: 50px;"><asp:Literal ID="ltRol" runat="server" meta:resourceKey="ltRol"></asp:Literal></td>
				<td style="width:200px; display:block; text-align:left;">
                <asp:TextBox runat="server" ID="txtRol" MaxLength="25" ToolTip="<%$ Resources:Commun, in_ES %>"></asp:TextBox>
                <asp:Label ID="lt_ES" runat="server"  CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label>
                </td>
				<td align="right" style="width: 75px;"><asp:Literal ID="ltActivo" runat="server" Text="<%$ Resources:Commun, Active %>"></asp:Literal></td>
				<td><asp:CheckBox runat="server" ID="chkRolActivo" Checked="True" meta:resourcekey="chkRolActivoResource1" /></td>
			</tr>
            <td></td>
            <td style="display:block; text-align:left;">              <asp:TextBox runat="server" ID="txtRol_EN" MaxLength="25" ToolTip="<%$ Resources:Commun, in_EN %>"></asp:TextBox>
                <asp:Label ID="lt_EN" runat="server"  CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label></td>
			<td>&nbsp;</td>
            <td>&nbsp;</td>
            <tr>
				<td colspan="4" align="right">
					<asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
					<asp:HiddenField runat="server" Value="Añadir" ID="hddRol" />
					<asp:Button ID="btnActualizar" runat="server" OnClick="btnSaveRol_Click" Visible="False"
						meta:resourceKey="btnActualizar" />
					<asp:Button ID="btnSaveRol" runat="server" OnClick="btnSaveRol_Click" meta:resourceKey="btnSaveRol" />
					<asp:Button ID="btnLimpiar" runat="server" OnClick="btnCancelRol_Click" meta:resourceKey="btnLimpiar" />
					<asp:Button ID="btnCancelRol" runat="server" OnClick="btnCancelRol_Click" Visible="False"
						meta:resourceKey="btnCancelRol" />
				</td>
			</tr>
		</table>
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
			<asp:GridView ID="gvRol" runat="server" EnableModelValidation="True" AutoGenerateColumns="False"
				Width="800px" OnPreRender="gvRol_PreRender" CssClass="gridView" DataKeyNames="idRol"
				OnRowDataBound="gvRol_RowDataBound" SelectedRowStyle-CssClass="selected" OnSelectedIndexChanged="gvRol_SelectedIndexChanged"
				meta:resourceKey="gvRol">
				<Columns>
					<asp:TemplateField SortExpression="activo" HeaderStyle-HorizontalAlign="Center" meta:resourceKey="htActivo"
						ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
						<EditItemTemplate>
							<asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("bActivo") %>' meta:resourcekey="CheckBox1Resource1" />
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("bActivo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'
								Enabled="False" meta:resourcekey="lblActivoGridResource1" />
						</ItemTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>
					<asp:BoundField DataField="rolName" SortExpression="rol" meta:resourceKey="htRol"
						HeaderStyle-Width="550px">
						<HeaderStyle Width="463px"></HeaderStyle>
					</asp:BoundField>
				</Columns>
				<SelectedRowStyle CssClass="selected"></SelectedRowStyle>
			</asp:GridView>
		</div>
		<uc1:popUpMessageControl ID="popUpMessageControl1" style="display:none;" runat="server" />
	</div>
</asp:Content>