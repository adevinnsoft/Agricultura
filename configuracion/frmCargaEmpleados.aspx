<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCargaEmpleados.aspx.cs" Inherits="configuracion_frmCargaEmpleados" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
        
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>

    <style type="text/css">
        .auto-style1 {
            height: 10px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
            <table class="index" border="0">
                <tr>
                    <td colspan="3">
                        <h2>Importación</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Cargar archivo:</label>
                    </td>
                    <td class="left middle">
                        <asp:FileUpload runat="server" ID="fu_Plantilla" />
                    </td>


                    <td>    <asp:Label ID="Label1" runat="server" Text="Tiene encabezado ?" />
            <asp:RadioButtonList ID="rbHDR" runat="server">
                <asp:ListItem Text = "Si" Value = "Yes" Selected = "True" >
                </asp:ListItem>
                <asp:ListItem Text = "No" Value = "No"></asp:ListItem>
            </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td colspan="1" class="auto-style1">
                        <asp:Label ID="lblCantidadEmpleados" runat="server" Text="Número de Registros" />
                    </td>
                    <td class="auto-style1" align="left">
                        <asp:Label ID="lblRegistros" runat="server" Text="0"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        <asp:Button runat="server" ID="btn_Importar"  Text="Importar" OnClick="btn_Importar_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" colspan="1">
                        <asp:Label ID="lblNoRegistroSinDuplicado" runat="server" Text="Número de Registros - Duplicados" />
                    </td>
                    <td align="left" class="auto-style1">
                        <asp:Label ID="lblRegistrosSinDuplicado" runat="server" Text="0"></asp:Label>
                    </td>
                    <td class="auto-style1">&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <div style="text-align:center">
        <table>
               <tr>
                    <td colspan="1" class="auto-style1">
                        <asp:Label ID="lblDuplicados" runat="server" Text="Número Duplicados" />
                    </td>
                    <td class="auto-style1" align="left">
                        <asp:Label ID="lblCantDuplicados" runat="server" Text="0"></asp:Label>
                    </td>
                    
                </tr>
         </table>
            </div>
         <div class="grid">           
             <asp:GridView ID="grvDuplicados" runat="server" AutoGenerateColumns="true" 
                     CssClass="gridView" Width="75%" EmptyDataText="No existen registros"  
                   meta:resourcekey="GvPlantasResource1"   PageSize="1000" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />                 
                      
                 </asp:GridView> 
         </div> 
         <div style="text-align:center">
        <table>
               <tr>
                    <td colspan="1" class="auto-style1">
                        <asp:Label ID="lblActualizados" runat="server" Text="Número Actualizados" />
                    </td>
                    <td class="auto-style1" align="left">
                        <asp:Label ID="lblNoActualizados" runat="server" Text="0"></asp:Label>
                    </td>
                    
                </tr>
         </table>
            </div>
        <div class="grid">           
             <asp:GridView ID="grvActualizados" runat="server" AutoGenerateColumns="true" 
                     CssClass="gridView" Width="75%" EmptyDataText="No existen registros"  
                   meta:resourcekey="GvPlantasResource1"   PageSize="1000" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />                 
                      
                 </asp:GridView> 
         </div> 
        <div style="text-align:center">
        <table>
               <tr>
                    <td colspan="1" class="auto-style1">
                        <asp:Label ID="lblNuevos" runat="server" Text="Número Nuevos" />
                    </td>
                    <td class="auto-style1" align="left">
                        <asp:Label ID="lblNoNuevos" runat="server" Text="0"></asp:Label>
                    </td>
                    
                </tr>
         </table>
            </div>
        <div class="grid">           
             <asp:GridView ID="grvEmpleadosNuevos" runat="server" AutoGenerateColumns="true" 
                     CssClass="gridView" Width="75%" EmptyDataText="No existen registros"  
                   meta:resourcekey="GvPlantasResource1"   PageSize="1000" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />                 
                      
                 </asp:GridView> 
         </div> 
         <div style="text-align:center">
        <table>
               <tr>
                    <td colspan="1" class="auto-style1">
                        <asp:Label ID="lblInactivos" runat="server" Text="Número Inactivados" />
                    </td>
                    <td class="auto-style1" align="left">
                        <asp:Label ID="lblNoInactivos" runat="server" Text="0"></asp:Label>
                    </td>
                    
                </tr>
         </table>
            </div>
         <div class="grid">           
             <asp:GridView ID="grvEmpleadosInactivos" runat="server" AutoGenerateColumns="true" 
                     CssClass="gridView" Width="75%" EmptyDataText="No existen registros"  
                   meta:resourcekey="GvPlantasResource1"   PageSize="1000" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />                 
                      
                 </asp:GridView> 
         </div> 
         <div class="grid">           
             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
                     CssClass="gridView" Width="75%" EmptyDataText="No existen registros"  
                   meta:resourcekey="GvPlantasResource1"   PageSize="1000" onprerender="GridView1_PreRender" onrowdatabound="GridView1_RowDataBound" 
                   >
                    <AlternatingRowStyle CssClass="gridViewAlt" />
                 
                    <Columns>

                        <asp:BoundField DataField="NOMBRE EMPLEADO" SortExpression="Cycle"
                            meta:resourcekey="BoundFieldResource1" HeaderText="NOMBRE EMPLEADO">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="ID EMPLEADO" SortExpression="Greenhouse" HeaderText="ID EMPLEADO"
                            meta:resourcekey="BoundFieldResource2">
                              </asp:BoundField>
                         <asp:BoundField DataField="ID SUPERVISOR" SortExpression="Greenhouse" HeaderText="ID SUPERVISOR"
                            meta:resourcekey="BoundFieldResource2">
                              </asp:BoundField>
                        </Columns> 
                      
                 </asp:GridView> 
         </div> 
    </div>
    
        <uc1:popUpMessageControl runat="server" ID="popUpMessage" />

  

</asp:Content>
