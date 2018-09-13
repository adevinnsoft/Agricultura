<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmZonificacion.aspx.cs" Inherits="configuracion_frmZonificacion" ValidateRequest="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="asp" Namespace="NewControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript">

	    function pageLoad() {
	        $('#ctl00_ContentPlaceHolder1_listaSecciones_0').change(function () {
	            $('#ctl00_ContentPlaceHolder1_listaSecciones input[type="checkbox"]').each(function () {
	                this.checked = $('#ctl00_ContentPlaceHolder1_listaSecciones_0').prop('checked');
	            });
	        });
	    }

	   
	</script>

    <style>
        .android
        {
            background-image:url('../Images/wifi.png');    
        }
    </style>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
		<h1><asp:Label ID="lblBienvenido" runat="server" meta:resourceKey="lblBienvenido"></asp:Label></h1>
		<table class="index">
			<tr><td align="left" colspan="4"><h2><asp:Literal ID="Literal1" runat="server" meta:resourceKey="Literal1"></asp:Literal></h2></td></tr>
			<tr>
				<td style="width: 50px;" align="right"><asp:Literal ID="ltRol" runat="server" meta:resourcekey="ltRolResource1"></asp:Literal></td>
				<td style="width: 50px;" align="left">
                    <asp:DropDownList ID="ddlRancho" runat="server"  AutoPostBack="True"  meta:resourcekey="ddlRolResource1" OnSelectedIndexChanged="ddlRancho_SelectedIndexChanged"></asp:DropDownList></td>
				<td align="right"><asp:Literal ID="ltModulo" runat="server" meta:resourcekey="ltModuloResource1"></asp:Literal></td>
				<td align="left"><asp:DropDownList ID="ddlInvernadero" runat="server"  meta:resourcekey="ddlModuloResource1"></asp:DropDownList></td>
                <td align="left"> <asp:Button ID="btnBuscar" runat="server" Text="Configuracion" OnClick="btnBuscar_Click" /></td> 
			</tr>
			<tr>
            
				<td align="right" colspan="4">
             
                        <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" meta:resourceKey="btnGuardar" />
					
                  
					
				</td>
			</tr>
		</table>
		<div class="grid">
		
		<asp:CheckBoxListWithAttributes ID="listaSecciones" runat="server" CssClass="permisos gridView" OnDataBound="listaSecciones_DataBound" meta:resourcekey="listaSeccionesResource1"></asp:CheckBoxListWithAttributes>
				
		</div>
		<uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
	</div>
</asp:Content>

