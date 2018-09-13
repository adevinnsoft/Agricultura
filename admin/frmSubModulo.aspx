<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"CodeFile="frmSubModulo.aspx.cs" Inherits="Administration_frmSubModulo" ValidateRequest="false" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
	<script type="text/javascript" src="../comun/Scripts/inputValidations.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>

	<script type="text/javascript">
	    $(function () {
	        registerControls();
	    });
	</script>
<style>
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


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
		<h1><asp:Label ID="lblBienvenido" runat="server" meta:resourceKey="lblBienvenido"></asp:Label></h1>
			<asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">		
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>

					<table class="index">
						<tr><td colspan="4" align="left"><h2><asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal></h2></td></tr>
						<tr>
							<td align="right"><asp:Literal ID="ltM" runat="server" meta:resourceKey="ltM"></asp:Literal></td>
							<td align="left"><asp:DropDownList runat="server" ID="ddlModulo" DataTextField="modulo" DataValueField="idModulo" AutoPostBack="True" OnDataBound="ddlModulo_DataBound" CssClass="cajaLarga" OnSelectedIndexChanged="ddlModulo_SelectedIndexChanged" meta:resourcekey="ddlModuloResource1"></asp:DropDownList></td>
							<td align="right"><asp:Literal ID="ltActivo" runat="server" Text="<%$ Resources:Commun, Active %>"></asp:Literal></td>
							<td align="left" style=" text-align:left;">
                                <%--<asp:CheckBox runat="server" ID="chkActivo" Checked="True" meta:resourcekey="chkActivoResource1" />--%>
                                <input checked="checked"  type="checkbox" ID="chkActivo" runat="server" class="check-with-label" />
                                <label  id="Label1"  class='label-for-check' for="<%=chkActivo.ClientID %>" ><span></span></label>
                            </td>
						</tr>
						<tr>
							<td align="right"><asp:Literal ID="ltSubMP" runat="server" meta:resourceKey="ltSubMP"></asp:Literal></td>
							<td align="left">
								<asp:DropDownList runat="server" ID="ddlSunMP" CssClass="cajaLarga" DataValueField="idSubModulo" OnDataBound="ddlSunMP_DataBound" meta:resourcekey="ddlSunMPResource1">
									<asp:ListItem meta:resourceKey="Ninguno"></asp:ListItem>
								</asp:DropDownList>
							</td>
							
							
					        <%--
                            <td><asp:literal ID="ltAndroid" runat="server" Text="Android"  /></td>
                            <td align="left" style=" text-align:left;"><input type="checkbox" ID="ckAndroid" runat="server" css="check-with-label" /><label  id="Label2"  class='label-for-check' for="<%=ckAndroid.ClientID %>" ><span></span></label></td>
                            --%>
                            <td align="right"><asp:Literal ID="ltSubM" runat="server" meta:resourceKey="ltSubM"></asp:Literal></td>
							<td align="left" style="display: block; text-align: left;">
								<asp:TextBox CssClass="cajaLarga" runat="server" ID="txtSubM" MaxLength="45" ToolTip="<%$ Resources:Commun, in_ES %>"></asp:TextBox>
								<asp:Label ID="lt_ES" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label>
							</td>
						</tr>
                        
						<tr>
							<td></td>
							<td></td>
							<td></td>
							<td style="display: block; text-align: left;">
								<asp:TextBox CssClass="cajaLarga" runat="server" ID="txtSubM_EN" MaxLength="45" ToolTip="<%$ Resources:Commun, in_EN %>"></asp:TextBox>
								<asp:Label ID="lt_EN" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label>
							</td>
                            
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td></td>
							<td></td>
						</tr>
						<tr>
							<td align="right"><asp:Literal runat="server" ID="ltRuta" meta:resourceKey="ltRuta"></asp:Literal></td>
							<td colspan="3" align="left"><asp:TextBox runat="server" ID="txtRuta" CssClass="cajaLarga" Width="380px" MaxLength="250"></asp:TextBox></td>
						</tr>
						<tr>
							<td colspan="4" align="right">
								<asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
								<asp:Button ID="btnActualizar" runat="server" OnClick="btnSave_Click" Visible="False" meta:resourceKey="btnActualizar" />
								<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" meta:resourceKey="btnSave" />
								<asp:Button ID="btnLimpiar" runat="server" OnClick="btnCancel_Click" meta:resourceKey="btnLimpiar" />
								<asp:Button ID="btnCancel" runat="server" meta:resourceKey="btnCancel" OnClick="btnCancel_Click" Visible="False" />
							</td>
						</tr>
					</table>
				

                <script type="text/javascript">
                    Sys.Application.add_load(function () { registerControls();});
                </script>				<div class="grid">
					<div id="pager" class="pager">
						<img alt="first" src="../comun/img/first.png" class="first" />
						<img alt="prev" src="../comun/img/prev.png" class="prev" />
                        <span class="pagedisplay"></span>
						<img alt="next" src="../comun/img/next.png" class="next" />
						<img alt="last" src="../comun/img/last.png" class="last" />
						<select class="pagesize cajaCh " style="width: 50px; min-width: 50px; max-width: 50px;">
							<option value="10">10</option>
							<option value="20">20</option>
							<option value="30">30</option>
							<option value="40">40</option>
							<option value="50">50</option>
						</select>
					</div>
					<asp:GridView ID="gvSubM" runat="server" EnableModelValidation="True" Width="800px"
						AutoGenerateColumns="False" CssClass="gridView" DataKeyNames="idSubModulo" OnPreRender="gvSubM_PreRender"
						OnRowDataBound="gvSubM_RowDataBound" OnSelectedIndexChanged="gvSubM_SelectedIndexChanged"
						meta:resourceKey="gvSubM">
						<Columns>
							<asp:TemplateField SortExpression="bActivo" meta:resourceKey="htActivo"  HeaderStyle-CssClass="cajaCh" >
								<EditItemTemplate><asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("bActivo") %>' meta:resourcekey="CheckBox1Resource1" /></EditItemTemplate>
								<ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("bActivo")==true?(string)GetGlobalResourceObject("Commun","Si"):(string)GetGlobalResourceObject("Commun","No") %>' Enabled="False" meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
								<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
<%--                            <asp:TemplateField meta:resourceKey="HTandroid" SortExpression="Android" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cajaCh">
						        <EditItemTemplate><asp:CheckBox ID="cbAndroid" runat="server" Checked='<%# Bind("Android") %>' meta:resourcekey="CheckBox1Resource1" /></EditItemTemplate>
						        <ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("Android")==true? (string)GetGlobalResourceObject("Commun","Si"):(string)GetGlobalResourceObject("Commun","No") %>' meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
						        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						        <ItemStyle HorizontalAlign="Center"></ItemStyle>
					        </asp:TemplateField>--%>
							<asp:BoundField DataField="vModulo" meta:resourceKey="bfModulo" HeaderStyle-CssClass="cajaMed" />
							<asp:BoundField DataField="parent_subModulo" meta:resourceKey="bfParent_subModulo" HeaderStyle-CssClass="cajaMed" />
							<asp:BoundField DataField="subModulo" meta:resourceKey="bfSubModulo" HeaderStyle-CssClass="cajaMed"/>
							<asp:BoundField DataField="vRuta" meta:resourceKey="bfRuta"><ItemStyle CssClass="wrapped" /></asp:BoundField>
						</Columns>
						<SelectedRowStyle CssClass="selected" />
					</asp:GridView>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
        </asp:Panel>
		<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
	</div>
</asp:Content>