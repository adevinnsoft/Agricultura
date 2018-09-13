<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmEmpleados.aspx.cs" Inherits="RH_frmEmpleados"  EnableEventValidation="false"  %>


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
                                <asp:Literal ID="ltRancho" runat="server" meta:resourceKey="ltRanchoResource"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRanchos" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlRanchos_SelectedIndexChanged"  >
                                </asp:DropDownList>
                            </td>
                            <td><asp:Literal ID="ltLider" runat="server" meta:resourceKey="ltLiderResource"></asp:Literal></td>
                            <td>
                                <asp:DropDownList ID="ddlLider" runat="server"  style="width:auto;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltInvernadero" runat="server" meta:resourceKey="ltNoEmpleadoResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtNoEmpleado" runat="server"></asp:TextBox>
                            </td>
                           
                        </tr>
                        <tr>
                            <td align="right"><asp:Literal ID="ltNombreEmpleado" runat="server" meta:resourceKey="ltNombreEmpleadoResource"></asp:Literal></td>
                            <td align="right" colspan="3">
                                <asp:TextBox ID="txtNombreEmpleado" runat="server" Width="569px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>   
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                            </td>
                          <td>

                              <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar"  Width="150px" OnClick="btnCancelar_Click"  />
                          </td>
           
                            <td>
                                &nbsp;</td>
           
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
                DataKeyNames="ID_EMPLEADO" meta:resourcekey="GvPlantasResource1"   >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="ID_EMPLEADO" SortExpression="ID_EMPLEADO" 
                             meta:resourcekey="BoundFieldResource1" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" SortExpression="Nombre" 
                             meta:resourcekey="BoundFieldResource2" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField DataField="NombrePlanta" SortExpression="NombrePlanta" 
                             meta:resourcekey="BoundFieldResource3" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField  DataField="id_Lider" SortExpression="id_Lider" 
                             meta:resourcekey="BoundFieldResource4" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre_Lider"   meta:resourcekey="BoundFieldResource5"
                            SortExpression="Nombre_Lider" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
            
        </div> 
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>



