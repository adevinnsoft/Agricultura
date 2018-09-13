<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCandadoCapturaTrabajo.aspx.cs" EnableEventValidation="false" Inherits="configuracion_frmCandadoCapturaTrabajo" %>


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

  
    <style type="text/css">
        .auto-style2 {
            width: 424px;
        }
    </style>

  
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server"  Text="Candado Captura de Trabajo"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdnIdPlanta" runat="server" />
                    <table class="index3">
                        <tr>
                            <td colspan="2" align="left" >
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server"  Text="Registro Candado Captura Trabajo"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td  >
                                <asp:Literal ID="ltPais" runat="server" meta:resourceKey="ltPaisResource" Text="Día Inicio"></asp:Literal>
                            </td>
                            <td class="auto-style2" >
                                <asp:DropDownList ID="ddlDiaInicio" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltSucursal" runat="server" meta:resourceKey="ltSucursalResource" Text="Día Fin"></asp:Literal>
                            </td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlDiaFin0" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1" Text="Estatus"></asp:Literal>
                            </td>
                            <td class="auto-style2">
                                <asp:CheckBox ID="chkActivo" runat="server" Checked="True" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td></td>
                            <td class="auto-style2">
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
                DataKeyNames="idCapturaFecha" meta:resourcekey="GvSucursalesResource1"  
                     onprerender="GvSucursales_PreRender" onrowdatabound="GvSucursales_RowDataBound" 
                     onselectedindexchanged="GvSucursales_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="diaInicio" SortExpression="Dia Inicio" HeaderText="Dia Inicio" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                     <asp:BoundField DataField="diaFin" SortExpression="Dia Fin" HeaderText="Dia Fin">
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                   
                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
    </div>
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>

