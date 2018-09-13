<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmEdoEnvioPlantula.aspx.cs" Inherits="configuracion_frmEdoEnvioPlantula" EnableEventValidation="false" meta:resourcekey="PageResource1"%>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 83px;
        }
        .style2
        {
            width: 116px;
        }
    </style>
     <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>

    <script type="text/javascript">
            $(function () {
	        registerControls();
	    });
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
        <h1>
            <asp:Label ID="lblFamilia_prin" runat="server" Text="Estados de Envío por Plántula" 
                meta:resourcekey="lblFamilia_prinResource1"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" DefaultButton="btnHidden" 
            meta:resourcekey="formResource1">
            <table class="index">
                <tr>
                    <td align="left" colspan="8">
                        <h2>
                            <asp:Label ID="Label1" runat="server" Text="Registro de estados de envío" meta:resourcekey="Label1Resource1" 
                               ></asp:Label>
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                
                    <td align="left" class="style2">
                        *Estados de Envío:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEdo" Width="350px" MaxLength="50" 
                            CssClass="required" meta:resourcekey="txtEdoResource1"></asp:TextBox>
                    </td>
                    
                    <td><asp:Label ID="lngSp" runat="server" meta:resourceKey="lngSp"
                                        CssClass="lengua"></asp:Label>
                     </td>
                    
                    <td align="left" style="text-align: right;">
                        <asp:Literal ID="idltActivo" runat="server" Text="Activo" meta:resourcekey="idltActivoResource1" 
                            ></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="Activo" runat="server" Checked="True" meta:resourcekey="ActivoResource2" 
                            />
                    </td>
           
                </tr>
                <tr>
                    
                    <td align="left" class="style2">
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEdo_EN" Width="350px" MaxLength="50" 
                            CssClass="required" meta:resourcekey="txtEdo_ENResource1" ></asp:TextBox>
                    </td>
                    <td>
                    <asp:Label ID="lngEn" runat="server" meta:resourceKey="lngEn"
                                        CssClass="lengua"></asp:Label>
                    </td>
                </tr>
         
              <tr>
              <td colspan="2"></td>
              <td colspan="2">
                <div runat="server" id="ContendTabla"></div>
              </td>
              <td><div runat="server" id="AgreNiv"></div></td>
          
              </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;<asp:HiddenField ID="hdn_Act" runat="server" />
                        <asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
                    </td>
                    
               
                        
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnCancelar" OnClick="btnCancelar_Click" 
                            Text="Limpiar" meta:resourcekey="btnCancelarResource1" />
                    </td>
                    <td colspan="2">
                              <asp:Button ID="btn_Enviar" runat="server" Text="Guardar" OnClick="btn_Guardar_Click"
                        CssClass="btnSave" OnClientClick="javascript:guardando();" meta:resourcekey="btn_EnviarResource1">
                    </asp:Button>
                            <asp:Button runat="server" ID="btnHidden" OnClientClick="return false;" Style="position: absolute;
                            top: -50%;" meta:resourcekey="btnHiddenResource1"  />
                       
                    </td>
                </tr>
            </table>
            <div id="tbl_Nivel" class="index" style="display:none;"></div>
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
			<asp:Gridview runat="server" ID="gv_EdoEnvio" CssClass="gridView" 
                EmptyDataText="No existen registros" Width="800px"
        AutoGenerateColumns="False" DataKeyNames="idEstado" OnPageIndexChanging="gv_EdoEnvio_PageIndexChanging"
        OnPreRender="gv_EdoEnvio_PreRender" OnRowDataBound="gv_EdoEnvio_RowDataBound" CellPadding="4"
        ForeColor="#333333" GridLines="None"  
                OnSelectedIndexChanged="gv_EdoEnvio_SelectedIndexChanged" meta:resourcekey="gv_EdoEnvioResource1" 
                 >
            <Columns>
                <asp:BoundField DataField="vEstado" HeaderText="Estado" meta:resourcekey="BoundFieldResource1" 
                     />
                <asp:TemplateField HeaderText="Activo" SortExpression="Activo" 
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" 
                    meta:resourcekey="TemplateFieldResource1">
                <ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" 
                        Text='<%# (bool)Eval("bActivo")==true? GetLocalResourceObject("Label1ResourceSi") :GetLocalResourceObject("Label1ResourceNo") %>' 
                        meta:resourcekey="lblActivoGridResource1"  /></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
     
         
           </Columns>
        </asp:Gridview>
			</div>
        </div>
        <uc1:popupmessagecontrol ID="popUpMessageControl1" runat="server" />
    </div>
</asp:Content>

