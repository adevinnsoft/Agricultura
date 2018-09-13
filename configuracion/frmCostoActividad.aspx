<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"  CodeFile="frmCostoActividad.aspx.cs" Inherits="configuracion_frmCostoActividad" %>

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
                if ((txtlen - dotpos) > 2)
                    return false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
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
                            <td>*<asp:Literal ID="Literal1" meta:resourceKey="ltPlanta" runat="server">Planta:</asp:Literal></td>
                            <td colspan="1" class="floatnone" align="right">
                                <asp:DropDownList ID="ddlPlanta" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlPlanta_SelectedIndexChanged1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="left" >
                                <asp:Literal ID="ltDepartamento" runat="server" meta:resourceKey="ltDepartamentoResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlDepartamento0" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlDepartamento0_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr align="center">
                            <td align="left">
                                <asp:Label ID="lblProducto" runat="server" Text="Producto"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlProducto" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="ltActividad0" runat="server" meta:resourceKey="ltActividadResource"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlActividad0" runat="server" meta:resourceKey="ddlPaisResource" AutoPostBack="True" OnSelectedIndexChanged="ddlActividad0_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trTipoCosecha" align="right" runat="server"  visible="false" >
                            <td align="right">
                                <asp:Literal ID="ltTipoCosecha" runat="server" Text="Tipo Cosecha" ></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlTipoCosecha" runat="server" meta:resourceKey="ddlPaisResource">
                                </asp:DropDownList>
                            </td>
                             <td align="right">
                                 <asp:Literal ID="ltTipoCaja" runat="server" Text="Tipo Caja"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlTipoCaja" runat="server" meta:resourceKey="ddlPaisResource">
                                </asp:DropDownList>
                            </td>
                           
                        </tr>
                        <tr align="right">
                            <td align="right">
                                <asp:Literal ID="ltCantidad1" runat="server" meta:resourceKey="ltCantidadResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtCantidad0" runat="server" meta:resourceKey="txtNombreCortoResource" onkeypress="return onlyDotsAndNumbers(this,event);" Width="81px"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Literal ID="ltUnidadMedida0" runat="server" meta:resourceKey="ltUnidadMedidaResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlUnidad0" runat="server" meta:resourceKey="ddlPaisResource">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltCostoTarea" runat="server" meta:resourceKey="ltCostoTareaResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtCosto" runat="server" meta:resourceKey="txtNombreCortoResource" Width="96px" onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkActivo" runat="server" Checked="True" meta:resourcekey="chkActivoResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">&nbsp;</td>
                            <td align="right">&nbsp;</td>
                            <td align="right">&nbsp;</td>
                            <td align="right">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
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
                DataKeyNames="idCostoActividad" meta:resourcekey="GvPlantasResource1"  
                     onprerender="GvPlantas_PreRender" onrowdatabound="GvPlantas_RowDataBound" 
                     onselectedindexchanged="GvPlantas_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="NombrePlanta" SortExpression="NombrePlanta" 
                             meta:resourcekey="BoundFieldResource1" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreDepartamento" SortExpression="NombreDepartamento" 
                             meta:resourcekey="BoundFieldResource2" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="Producto" SortExpression="Producto" 
                          HeaderText="Producto" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                     <asp:BoundField DataField="NombreHabilidad" SortExpression="NombreHabilidad" 
                             meta:resourcekey="BoundFieldResource3" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="TipoCosecha" SortExpression="TipoCosecha" 
                             HeaderText="Tipo Cosecha" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="TipoCaja" SortExpression="TipoCaja" 
                               HeaderText="Tipo Caja" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="Cantidad" SortExpression="Cantidad" 
                             meta:resourcekey="BoundFieldResource4" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UnidadMedida"   meta:resourcekey="BoundFieldResource5"
                            SortExpression="UnidadMedida" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Costo" SortExpression="Costo" 
                             meta:resourcekey="BoundFieldResourceCodigo" >
                        <HeaderStyle HorizontalAlign="Center" />
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