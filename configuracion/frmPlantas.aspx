<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmPlantas.aspx.cs" Inherits="configuracion_frmPlantas" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

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
                            <td colspan="4" align="left">
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
                                <asp:DropDownList ID="ddlSucursal" runat="server" meta:resourceKey="ddlPaisResource"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                *<asp:Literal ID="ltPlanta" runat="server" meta:resourceKey="ltPlantaResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox id="txtPlanta" runat="server" meta:resourceKey="txtEtapaResource"></asp:TextBox>
                            </td>
                             <td align="right">
                                 *<asp:Literal ID="ltNombreCorto" runat="server" meta:resourceKey="ltNombreCortoResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox id="txtNombreCorto" runat="server" meta:resourceKey="txtNombreCortoResource"></asp:TextBox>
                            </td>
                           
                        </tr>
                        <tr>
                            <td align="right">
                                *<asp:Literal ID="ltColor" runat="server" meta:resourceKey="ltColorResource"></asp:Literal>
                            </td>
                            <td align="right">
                               
                                <asp:TextBox ID="txtColorP" runat="server" class="required color {pickerFaceColor:'transparent',pickerFace:3,pickerBorder:0,pickerInsetColor:'black'}"
                                    meta:resourceKey="txtColorPResource1"></asp:TextBox>
                           
                            </td>

                        </tr>
                        
                        <tr>
                             <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkActivo" runat="server" 
                                    meta:resourcekey="chkActivoResource1" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>   &nbsp;</td>
                          <td>

                              <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
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

                <asp:GridView ID="GvPlantas" runat="server" AutoGenerateColumns="False" 
                     CssClass="gridView" Width="90%" EmptyDataText="No existen registros" 
                DataKeyNames="IdPlanta" meta:resourcekey="GvPlantasResource1"  
                     onprerender="GvPlantas_PreRender" onrowdatabound="GvPlantas_RowDataBound" 
                     onselectedindexchanged="GvPlantas_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="NombrePlanta" SortExpression="NombrePlanta" 
                             meta:resourcekey="BoundFieldResource1" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreCorto" SortExpression="NombreCorto" 
                             meta:resourcekey="BoundFieldResource2" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Sucursal" SortExpression="Sucursal" 
                             meta:resourcekey="BoundFieldResource3" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="FechaCreacion" SortExpression="FechaCreacion" 
                             meta:resourcekey="BoundFieldResource4" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HexColor"   meta:resourcekey="BoundFieldResource5"
                            SortExpression="Color" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Activo" SortExpression="Activo" 
                             meta:resourcekey="BoundFieldResource6" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
    </div>
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>

