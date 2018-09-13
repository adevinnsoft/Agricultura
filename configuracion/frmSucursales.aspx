<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmSucursales.aspx.cs" Inherits="configuracion_frmSucursales" EnableEventValidation="false" meta:resourcekey="PageResource1" %>


<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>

        <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdnIdPlanta" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="3" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltPais" runat="server" meta:resourceKey="ltPaisResource"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPais" runat="server" meta:resourceKey="ddlPaisResource"></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltSucursal" runat="server" meta:resourceKey="ltSucursalResource"></asp:Literal>
                                *</td>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server" meta:resourceKey="txtEtapaResource"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkActivo" runat="server" Checked="True" meta:resourcekey="chkActivoResource1" />
                            </td>
                           
                        </tr>
                        
                        <tr>
                            <td colspan="1">&nbsp;</td>
                            <td align="left">
                                <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Cancelar" />
                            </td>
           
                        </tr>
                    </table>
              </asp:Panel>
           

            <div class="grid">
                 <div id="pager" class="pager">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <input type="text" class="pagedisplay" />
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

                <asp:GridView ID="GvSucursales" runat="server" AutoGenerateColumns="False" 
                     CssClass="gridView" Width="90%" EmptyDataText="No existen registros" 
                DataKeyNames="idSucursal" meta:resourcekey="GvSucursalesResource1"  
                     onprerender="GvSucursales_PreRender" onrowdatabound="GvSucursales_RowDataBound" 
                     onselectedindexchanged="GvSucursales_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="Nombre" SortExpression="Nombre" 
                             meta:resourcekey="BoundFieldResource1" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Pais" SortExpression="Pais" 
                             meta:resourcekey="BoundFieldResource3" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="Fecha" SortExpression="Fecha" 
                             meta:resourcekey="BoundFieldResource4" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Estatus" SortExpression="Activo" 
                             meta:resourcekey="BoundFieldResource6" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
    </div>
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>

