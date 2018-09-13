<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmTipoCajasCosecha.aspx.cs" Inherits="configuracion_frmTipoCajasCosecha" EnableEventValidation="false" meta:resourcekey="PageResource1"%>


<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>


      <script type = "text/javascript">
          function functionx(evt) {
              if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                  alert("Allow Only Numbers");
                  return false;
              }
          }
          function onlyDotsAndNumbers(txt, event) {
              var charCode = (event.which) ? event.which : event.keyCode
              if (charCode == 46) {
                  if (txt.value.indexOf(".") < 0)
                      return true;
                  else
                      return false;
              }

              if (txt.value.indexOf(".") > 0) {
                  var txtlen = txt.value.length;
                  var dotpos = txt.value.indexOf(".");
                  //Change the number here to allow more decimal points than 2
                  if ((txtlen - dotpos) > 2)
                      return false;
              }

              if (charCode > 31 && (charCode < 48 || charCode > 57))
                  return false;

              return true;
          }

          function isNumberKey(evt) {
              var charCode = (evt.which) ? evt.which : event.keyCode
              if ((charCode <= 90 && charCode >= 65) || (charCode <= 122 && charCode >= 97) || charCode == 8)
                  return true;

              return false;

          }
        </script>

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
            <asp:Label ID="lblFamilia_prin" runat="server" Text="Tipo de Cajas Cosecha " 
                meta:resourcekey="lblFamilia_prinResource1"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" DefaultButton="btnHidden" 
            meta:resourcekey="formResource1">
             <asp:HiddenField id="hdnIdDepartamento" runat="server" />
            <table class="index">
                <tr>
                    <td align="left" colspan="4">
                        <h2>
                            <asp:Label ID="Label1" runat="server" Text="Registro Tipo Cajas Cosecha" 
                                meta:resourcekey="Label1Resource1"></asp:Label>
                        </h2>
                    </td>
                </tr>
           
                <tr>
          
                    <td align="left" class="style2">
                    <asp:Literal runat="server" ID="lblNombre" Text="NOMBRE:" 
                           meta:resourcekey="ResourceZona"></asp:Literal>
                        </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNombre" Width="350px" MaxLength="50" 
                            CssClass="required" ></asp:TextBox>
                          
                    </td>                             
              
                </tr>
                <tr>
                             
                   <td align="left" class="style2">
                    <asp:Literal runat="server" ID="Literal1" Text="PESO CON PRODUCTO:" 
                           meta:resourcekey="ResourceZona"></asp:Literal>
                        </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPesoProducto" Width="350px" MaxLength="50" 
                            CssClass="required" ></asp:TextBox>
                          
                    </td>   
                 
                </tr>
                 <tr>
                             
                   <td align="left" class="style2">
                    <asp:Literal runat="server" ID="Literal2" Text="PESO TARA:" 
                           meta:resourcekey="ResourceZona"></asp:Literal>
                        </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPesoTara" Width="350px" MaxLength="50" 
                            CssClass="required" onkeypress="return onlyDotsAndNumbers(this,event);" ></asp:TextBox>
                          
                    </td>   
                 
                </tr>
                <tr>
                 <td align="left" style="text-align: right;">
                        <asp:Literal ID="idltActivo" runat="server" Text="ACTIVO" 
                            meta:resourcekey="idltActivoResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbxActivo" runat="server" Checked="True" 
                            meta:resourcekey="ActivoResource2"/>
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
        AutoGenerateColumns="False" DataKeyNames="idCajaTipo" OnPageIndexChanging="gv_ZonaMonitor_PageIndexChanging"
        OnPreRender="gv_ZonaMonitor_PreRender" OnRowDataBound="gv_ZonaMonitor_RowDataBound" CellPadding="4"
        ForeColor="#333333" GridLines="None"  
                OnSelectedIndexChanged="gv_ZonaMonitor_SelectedIndexChanged" 
                    meta:resourcekey="gv_ZonaMonitorResource1">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                    meta:resourcekey="BoundFieldResource1"/>
                 <asp:BoundField DataField="PesoProducto" HeaderText="Peso con Producto" 
                    meta:resourcekey="BoundFieldResource1"/>
                 <asp:BoundField DataField="PesoTara" HeaderText="Peso Tara" 
                    meta:resourcekey="BoundFieldResource1"/>
                 <asp:BoundField DataField="Estatus" HeaderText="Estatus" 
                    meta:resourcekey="BoundFieldResource1"/>
     
         
           </Columns>
        </asp:Gridview>
			</div>

        <uc2:popupmessagecontrol ID="popUpMessageControl1" runat="server" />
    </div>
</asp:Content>

