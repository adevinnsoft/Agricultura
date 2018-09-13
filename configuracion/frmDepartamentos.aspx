<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmDepartamentos.aspx.cs" Inherits="configuracion_frmDepartamentos" EnableEventValidation="false" meta:resourcekey="PageResource1"  %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            registerControls();
        });
       </script>
       <style type="text/css">
           .index tr td span.lengua
           {
               float: left;
           }
       </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
        <h1>
            <asp:Label ID="lblFamilia_prin" runat="server" Text="Zonas de Monitoreo" 
                meta:resourcekey="lblFamilia_prinResource1"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" DefaultButton="btnHidden" 
            meta:resourcekey="formResource1">
             <asp:HiddenField id="hdnIdDepartamento" runat="server" />
            <table class="index">
                <tr>
                    <td align="left" colspan="4">
                        <h2>
                            <asp:Label ID="Label1" runat="server" Text="Registro de Zonas" 
                                meta:resourcekey="Label1Resource1"></asp:Label>
                        </h2>
                    </td>
                </tr>
           
                <tr>
          
                    <td align="left" class="style2">
                    <asp:Literal runat="server" ID="lblFamilia" Text="&nbsp;&nbsp;&nbsp;Zona:" 
                           meta:resourcekey="ResourceZona"></asp:Literal>
                        </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDepartametno" Width="350px" MaxLength="50" 
                            CssClass="required" meta:resourcekey="txtZonaResource1"></asp:TextBox>
                            <asp:Label ID="Esp" runat="server" CssClass="lengua" Text="<%$ Resources:Español %>"></asp:Label>
                    </td>
                    
                
                    <td align="left" style="text-align: right;">
                        <asp:Literal ID="idltActivo" runat="server" Text="Activo" 
                            meta:resourcekey="idltActivoResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbxActivo" runat="server" Checked="True" 
                            meta:resourcekey="ActivoResource2"/>
                    </td>
              
                </tr>
                <tr>
                             
                    <td align="left" class="style2">
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDepartmento_EN" Width="350px" MaxLength="50" 
                            CssClass="required" meta:resourcekey="txtZona_ENResource1"></asp:TextBox>
                            <asp:Label ID="Ing" runat="server" CssClass="lengua" Text="<%$ Resources:Ingles %>"></asp:Label>
                    </td>
                 
                </tr>
             
              <tr>
        
              <td colspan="2">
                <div runat="server" id="ContendTabla"></div>
              </td>
              <td colspan="2"><div runat="server" id="AgreNiv"></div></td>
   
              </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="hdn_Act" runat="server" />
                   </td>
                </tr>
                <tr>
                    <td>
                       
                        <asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
                    </td>
             
                        
                    <td colspan="3">
                        <asp:Button ID="btn_Enviar" runat="server" Text="Guardar" OnClick="btn_Guardar_Click"
                        CssClass="btnSave" 
                                >
                    </asp:Button>
                    <asp:Button runat="server" ID="btnCancelar" OnClick="btnCancelar_Click" 
                            Text="Limpiar"/>
                            <asp:Button runat="server" ID="btnHidden" OnClientClick="return false;" Style="position: absolute;
                            top: -50%;" meta:resourcekey="btnHiddenResource1"/>
                    </td>
                   
                </tr>
            </table>
            <br/><br/><br/>
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
        </div>
			<asp:Gridview runat="server" ID="gv_ZonaMonitor" CssClass="gridView" 
                EmptyDataText="No existen registros" Width="800px"
        AutoGenerateColumns="False" DataKeyNames="idDepartamento" OnPageIndexChanging="gv_ZonaMonitor_PageIndexChanging"
        OnPreRender="gv_ZonaMonitor_PreRender" OnRowDataBound="gv_ZonaMonitor_RowDataBound" CellPadding="4"
        ForeColor="#333333" GridLines="None"  
                OnSelectedIndexChanged="gv_ZonaMonitor_SelectedIndexChanged" 
                    meta:resourcekey="gv_ZonaMonitorResource1">
            <Columns>
                <asp:BoundField DataField="NombreDepartamento" HeaderText="Departamento" 
                    meta:resourcekey="BoundFieldResource1"/>
                 <asp:BoundField DataField="NombreDepartamento_En" HeaderText="Departamento_EN" 
                    meta:resourcekey="BoundFieldResource1"/>
                <asp:TemplateField HeaderText="Activo" SortExpression="Activo" 
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" 
                    meta:resourcekey="TemplateFieldResource1">
                <ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" 
                        Text='<%# (bool)Eval("Activo")==true? GetLocalResourceObject("Label1ResourceSi") :GetLocalResourceObject("Label1ResourceNo") %>' 
                        meta:resourcekey="lblActivoGridResource1"/></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
     
         
           </Columns>
        </asp:Gridview>
			</div>

        <uc2:popupmessagecontrol ID="popUpMessageControl1" runat="server" />
    </div>
</asp:Content>


