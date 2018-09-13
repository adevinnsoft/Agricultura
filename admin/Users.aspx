<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
	CodeFile="Users.aspx.cs" Inherits="Administration_Users" EnableEventValidation="false"
	ValidateRequest="false" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript">
          function isNumberKey(evt) {
              var charCode = (evt.which) ? evt.which : evt.keyCode;
              if (charCode > 31 && (charCode < 48 || charCode > 57))
                  return false;
              return true;
          }
    </script>
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
<%--<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />--%>	
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js" type="text/javascript"></script>

	<link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
	<style type="text/css">
        .style1
        {
            width: 120px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
		<h1>
			<asp:Label ID="lblBienvenido" runat="server" meta:resourceKey="lblBienvenido"></asp:Label></h1>
		<table class="index">
			<tr>
				<td align="left" colspan="6">
					<h2>
						<asp:Literal ID="Literal1" runat="server" meta:resourceKey="Literal1"></asp:Literal></h2>
				</td>
			</tr>
            <tr>
				<td align="right">
					<asp:Literal ID="ltActiveDirectory" runat="server" meta:resourceKey="ltActiveDirectory"></asp:Literal>
				</td>
				<td align="left" class="style1">
					
					<asp:CheckBox ID="cbxActiveDirectory" runat="server" Checked="false" OnCheckedChanged="cbxActiveDirectory_CheckedChanged" AutoPostBack="True" />
				</td>
			</tr>
            <tr>
				<td align="right">
					<asp:Literal ID="ltNoEmpleado" runat="server" meta:resourceKey="ltNoEmpleado"></asp:Literal>
				</td>
				<td align="left" class="style1">
					<asp:TextBox ID="txtNumeroEmpleado" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"
						meta:resourcekey="txtNumeroEmpleadoResource1"></asp:TextBox>
                    
				</td>
            </tr> 
			<tr>
				<td align="right">
					<asp:Literal ID="ltCuenta" runat="server" meta:resourceKey="ltCuenta"></asp:Literal>
				</td>
				<td align="left" class="style1">
					<asp:TextBox ID="txtCuenta" runat="server" MaxLength="20" AutoPostBack="True" OnTextChanged="txtCuenta_TextChanged" 
						meta:resourcekey="txtCuentaResource1"></asp:TextBox>
                    
				</td>
                
				<td align="right">
					<asp:Literal ID="lbDescricion" runat="server" Text="*Rol:" meta:resourcekey="lbDescricionResource1"></asp:Literal>
				</td>
				<td align="left">
					<asp:DropDownList ID="ddlTipo" runat="server" AppendDataBoundItems="True" 
                        Width="150px" meta:resourcekey="ddlTipoResource1">
					</asp:DropDownList>
				</td>
		<td align="right">
					<asp:Label ID="ltActivo" AssociatedControlID="checkActivo" runat="server" Text="<%$ Resources:Commun, Active %>"></asp:Label>
				</td>
                <td align="left" class="style1">
					<asp:CheckBox ID="checkActivo" runat="server" Checked="True" meta:resourcekey="checkActivoResource1" />
				</td>
			</tr>
            <tr>
                <td align="right">
                    <asp:Label ID="ltDepartamento" runat="server" meta:resourceKey="ltDepartamento"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlDepartamento" runat="server"></asp:DropDownList>
                </td>
            </tr>
           <%-- <tr>
                <td><asp:Literal runat="server" Text="Filtrar:" meta:resourcekey="filter" ID="filter"></asp:Literal></td>
                <td><asp:DropDownList runat="server" AutoPostBack="true" 
                        meta:resourceKey="ddlsucursal" ID="ddlsucursal" 
                        onselectedindexchanged="ddlsucursal_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>--%>
			<tr>
				<td align="right">
					<asp:Literal ID="ltNombre" runat="server" meta:resourceKey="ltNombre"></asp:Literal>
				</td>
				<td align="left" colspan="3" style="text-align: left;">
					<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Literal ID="ltNombreUsuario" runat="server" meta:resourcekey="ltNombreUsuarioResource1"></asp:Literal>
                            <asp:TextBox ID="txtNombre" runat="server" Width="402px" 	meta:resourcekey="txtNombreResource1"></asp:TextBox>
                        </ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="txtCuenta" EventName="TextChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</td>
                <td>Planta(s)</td>
                		<td rowspan="2" class="floatnone" style="white-space:nowrap;">
                <asp:UpdatePanel ID="UpdatePanelplant" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="ddlPlanta" runat="server" RepeatColumns="2" 
                            meta:resourcekey="ddlPlantaResource1" Height="24px">
					</asp:CheckBoxList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtCuenta" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
					
				</td>
			</tr>
			<%--<tr id="trLider" runat="server" visible="false">
				<td align="left">
					<asp:Literal ID="Lit_Lider" runat="server" Text="Lider:" meta:resourcekey="Lit_LiderResource1"></asp:Literal>
				</td>
				<td colspan="3" style="text-align: left;">
					<asp:DropDownList ID="ddl_Lider" runat="server" Width="300px" meta:resourcekey="ddl_LiderResource1">
					</asp:DropDownList>
				</td>
			</tr>--%>
			<tr>
				<td align="right">
					<asp:Literal ID="Literal3" runat="server" meta:resourceKey="Literal3"></asp:Literal>
				</td>
				<td align="left" colspan="4" style="text-align: left;">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:TextBox ID="txtEmail" runat="server" MaxLength="150" CssClass="cajaLarga" meta:resourcekey="ltEmailResource1"></asp:TextBox>
                            <asp:HiddenField ID="hdn_exist" runat="server" Value="" />
                            </ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="txtCuenta" EventName="TextChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</td>
			</tr>
		
			<tr>
				<td colspan="6">
					&nbsp;
					<asp:HiddenField ID="hdIdItem" runat="server" />
					<asp:HiddenField ID="hddRol" runat="server" />
					<asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click"
						meta:resourceKey="btnGuardar" />
					<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
						meta:resourceKey="btnCancelar" />
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
			<asp:GridView ID="grViewPendings" runat="server" AutoGenerateColumns="False" CssClass="gridView"
				HeaderStyle-CssClass="gridViewHeader" AlternatingRowStyle-CssClass="gridViewAlt"
				Width="800px" EmptyDataRowStyle-CssClass="gridEmptyData" SelectedRowStyle-CssClass="selected"
				PagerStyle-CssClass="gridViewPagerStyle" DataKeyNames="idUsuario" OnSelectedIndexChanged="grViewPendings_SelectedIndexChanged"
				OnRowDataBound="grViewPendings_RowDataBound" OnPreRender="grViewPendings_PreRender"
				meta:resourceKey="grViewPendings" EnableModelValidation="True">
				<AlternatingRowStyle CssClass="gridViewAlt"></AlternatingRowStyle>
				<Columns>
					<asp:TemplateField SortExpression="activo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cajaCh"
						HeaderStyle-Width="120px" meta:resourceKey="htActivo">
						<EditItemTemplate>
							<asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("bActivo") %>' meta:resourcekey="CheckBox1Resource1" /></EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("bActivo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'
								Enabled="False" meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
						<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:TemplateField>
                    <asp:BoundField DataField="idEmpleado" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="280px"
						meta:resourceKey="htNombre" HeaderText="No Empleado">
						<HeaderStyle Width="280px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="vNombre" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="280px"
						meta:resourceKey="htNombre">
						<HeaderStyle Width="280px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="vUsuario" meta:resourceKey="htCuenta" ItemStyle-HorizontalAlign="Left">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="email" meta:resourceKey="htmail" 
						ItemStyle-HorizontalAlign="Left">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
                    <asp:BoundField DataField="departamento" meta:resourceKey="department" 
						ItemStyle-HorizontalAlign="Left">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
                     <asp:BoundField DataField="plantaListNombre" meta:resourceKey="plantaListNombre"  HeaderText ="Rancho"
						ItemStyle-HorizontalAlign="Left">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="Tipo_Usr" meta:resourceKey="htTipo" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="cajaCh">
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="idUsuario" ReadOnly="True" Visible="false" meta:resourcekey="BoundFieldResource1" />
				</Columns>
				<EmptyDataRowStyle CssClass="gridEmptyData"></EmptyDataRowStyle>
				<HeaderStyle CssClass="gridViewHeader"></HeaderStyle>
				<PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
				<SelectedRowStyle CssClass="selected"></SelectedRowStyle>
			</asp:GridView>
		</div>
		<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
	</div>
</asp:Content>