<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ParametrosGrupoGrowing.aspx.cs" Inherits="pages_ParametrosGrupoGrowing" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <style type="text/css">
        
        .accordionCabecera
{
border: 1px solid black;
background-color: #ffffff;
font-family: Arial, Sans-Serif;
font-size: 14px;
font-weight: bold;
padding: 4px;
margin-top: 4px;
cursor: pointer;
}

.accordionContenido
{
font-family: Sans-Serif;
background-color: #ffffff;
border: 1px solid black;
border-top: none;
font-size: 12px;
padding: 7px;
height:70%;
} 

        .style1
        {
            width: 100%;
        }
        .style2
        {
            font-size: medium;
            color: #FF9900;
        }
        .tablesorter .filtered {
            display: none;
        }
        /* Ajax error row */
        .tablesorter .tablesorter-errorRow td {
            text-align: center;
            cursor: pointer;
            background-color: #e6bf99;
        }
        .custom-combobox {
            position: relative;
            display: inline-block;
        }
        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }
        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }
        input.disabled{ display:none;}}
    </style>
    <script type="text/javascript">
       
        function pageLoad(sender, args) {
            $('#<%=gvGruposGrowing.ClientID%>')
                .tablesorter({
                    // hidden filter input/selects will resize the columns, so try to minimize the change
                    widthFixed: true,

                    // initialize zebra striping and filter widgets
                    widgets: ["zebra", "filter"],
                    
                   headers: { 0: { sorter: false, filter: false },
                        3: { sorter: false, filter: false },
                        4: { sorter: false, filter: false },
                        5: { sorter: false, filter: false },
                        6: { sorter: false, filter: false },
                        9: { sorter: false, filter: false },
                        10: { sorter: false, filter: false },
                        11: { sorter: false, filter: false },
                        12: { sorter: false, filter: false },
                        2: { sorter: false, filter: false, display: 'none'}
                    },

                    widgetOptions: {
                        // If there are child rows in the table (rows with class name from "cssChildRow" option)
                        // and this option is true and a match is found anywhere in the child row, then it will make that row
                        // visible; default is false
                        filter_childRows: false,

                        // if true, filters are collapsed initially, but can be revealed by hovering over the grey bar immediately
                        // below the header row. Additionally, tabbing through the document will open the filter row when an input gets focus
                        filter_hideFilters: false,

                        // Set this option to false to make the searches case sensitive
                        filter_ignoreCase: true,

                        // jQuery selector string of an element used to reset the filters
                        filter_reset: '.reset',

                        // Use the $.tablesorter.storage utility to save the most recent filters
                        filter_saveFilters: true,

                        // Delay in milliseconds before the filter widget starts searching; This option prevents searching for
                        // every character while typing and should make searching large tables faster.
                        filter_searchDelay: 300,

                        // Set this option to true to use the filter to find text from the start of the column
                        // So typing in "a" will find "albert" but not "frank", both have a's; default is false
                        filter_startsWith: false,

                        // if false, filters are collapsed initially, but can be revealed by hovering over the grey bar immediately
                        // below the header row. Additionally, tabbing through the document will open the filter row when an input gets focus
                        filter_hideFilters: false

                    }

                })
                .tablesorterPager({ container: $("#pager") });
            

                $('#<%=gvParametrosPorGrupo.ClientID%>')
                .tablesorter({
                    widthFixed: true,
                    widgets: ["zebra", "filter"],
                    headers: { 0: { sorter: false, filter: false },
                        3: { sorter: false, filter: false },
                        4: { sorter: false, filter: false },
                        5: { sorter: false, filter: false },
                        6: { sorter: false, filter: false },
                        7: { sorter: false, filter: false },
                        8: { sorter: false, filter: false },
                        10: { sorter: false, filter: false, display: 'none' }
                    },
                    widgetOptions: {
                        filter_childRows: false,
                        filter_hideFilters: false,
                        filter_ignoreCase: true,
                        filter_reset: '.reset',
                        filter_saveFilters: true,
                        filter_searchDelay: 300,
                        filter_startsWith: false,
                        filter_hideFilters: false
                    }

                })
                .tablesorterPager({ container: $("#pager1") });

                $('#<%=gvCatalogoListaN_OK_X.ClientID%>')
                .tablesorter({
                    widthFixed: true,
                    widgets: ["zebra", "filter"],
                    headers: { 0: { sorter: false, filter: false },
                        3: { sorter: false, filter: false, display: 'none' }
                    },
                    widgetOptions: {
                        filter_childRows: false,
                        filter_hideFilters: false,
                        filter_ignoreCase: true,
                        filter_reset: '.reset',
                        filter_saveFilters: true,
                        filter_searchDelay: 300,
                        filter_startsWith: false,
                        filter_hideFilters: false
                    }

                })
                .tablesorterPager({ container: $("#pager2") });

                $('#<%=gvCatalogoListaS_A_G_N.ClientID%>')
                .tablesorter({
                    widthFixed: true,
                    widgets: ["zebra", "filter"],
                    headers: { 0: { sorter: false, filter: false },
                        3: { sorter: false, filter: false, display: 'none' }
                    },
                    widgetOptions: {
                        filter_childRows: false,
                        filter_hideFilters: false,
                        filter_ignoreCase: true,
                        filter_reset: '.reset',
                        filter_saveFilters: true,
                        filter_searchDelay: 300,
                        filter_startsWith: false,
                        filter_hideFilters: false
                    }

                })
                .tablesorterPager({ container: $("#pager3") });
        }
            function OnlyNumbers(Evento) {
                var iAscii;
                if (Evento.keyCode) {//Microft Compatibility
                    iAscii = Evento.keyCode;
                }
                else if (Evento.which) {//Mozila Compatibility
                    iAscii = Evento.which;
                }
                else {
                    return false;
                }

                if (((iAscii > 47) && (iAscii < 58)) || (iAscii == 13) || (iAscii == 08) || (iAscii == 46) || (iAscii == 37) || (iAscii == 39) || (iAscii == 9)) {
                    return true;
                }
                else {
                    return false;
                }
            }
  
      </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            font-size: medium;
            color: #ACC9A3;
        }
        .style3
        {
            width: 100%;
        }
        .style4
        {
            width: 100px;
        }
        .style5
        {
            width: 226px;
        }
        .style6
        {
            width: 101px;
        }
        .style7
        {
            width: 268px;
        }
        .style8
        {
            width: 62px;
        }
        input.disabled{ display:none;}
        .style9
        {
            width: 176px;
        }
        .style10
        {
            width: 133px;
        }
        .style11
        {
            width: 171px;
        }
        .style12
        {
            width: 237px;
        }
        .style13
        {
            width: 300px;
        }
        .style14
        {
            width: 47%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class = "container" style="width: 1100px">
    <table bgcolor="#F0F5E5" class="style1" width="100%">
        <tr>
            <td class="style2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="style3">
                            <tr>
                                <td>

                                    
                                        

<asp:Accordion ID="Accordion1" runat="server"
FadeTransitions="True"
FramesPerSecond="50"
Width="100%" Height="100%"
TransitionDuration="200"
HeaderCssClass="accordionCabecera">
<%--ContentCssClass="accordionContenido"--%>
<Panes>
<asp:AccordionPane ID="AccordionPane1" runat="server">
<Header><strong>  <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Titulo%>"></asp:Label>  </strong></Header>
<Content><form>
		<img src="../comun/img/first.png" class="first"/>
		<img src="../comun/img/prev.png" class="prev"/>
		 <span class="pagedisplay" style="font-size: 15px"></span>
		<img src="../comun/img/next.png" class="next"/>
		<img src="../comun/img/last.png" class="last"/>
		<select class="pagesize">
			<option value="999999"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Limit%>" /></option>
			<option value="2"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:2PorPagina%>" /></option>
			<option value="5"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:5PorPagina%>" /></option>
			<option value="10"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:10PorPagina%>" /></option>
		</select>
	</form>
    <asp:GridView ID="gvGruposGrowing" runat="server" AutoGenerateColumns="False" 
                                                    BackColor="White" DataKeyNames="idGrupoGrowing" DataSourceID="dstGrupoGrowing" 
                                                    onprerender="gvGruposGrowing_PreRender" 
                                                    onrowcreated="gvGruposGrowing_RowCreated" 
                                                    onrowdatabound="gvGruposGrowing_RowDataBound" 
                                                    onselectedindexchanged="gvGruposGrowing_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:BoundField DataField="idGrupoGrowing">
                                                        <ControlStyle Font-Size="1px" Width="1px" />
                                                        </asp:BoundField>
                                                        <asp:CommandField SelectText="" ShowSelectButton="True">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:CommandField>
                                                        <asp:BoundField DataField="NombreEs" HeaderText="<%$ Resources:NombreES%>" />
                                                        <asp:BoundField DataField="NombreEn" HeaderText="<%$ Resources:NombreEN%>" />
                                                        <asp:CheckBoxField DataField="AplicaListaDeNA_OK_X" 
                                                            HeaderText="<%$ Resources:AplicaListaN_OK _X%>" />
                                                        <asp:CheckBoxField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" 
                                                            HeaderText="<%$ Resources:AplicaDetallesParaLista_N_OK_X%>" />
                                                        <asp:CheckBoxField DataField="AplicaListaDeS_A_G_N" 
                                                            HeaderText="<%$ Resources:AplicaListaS_A_G_N%>" />
                                                        <asp:CheckBoxField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" 
                                                            HeaderText="<%$ Resources:AplicaDetallesParaListaS_A_G_N%>" />
                                                        <asp:BoundField DataField="PuntajeAsignadoParaPlantacion" 
                                                            SortExpression="PuntajeAsignadoParaPlantacion" />
                                                        <asp:BoundField DataField="PuntajeAsignadoParaNoPlantacion" 
                                                            SortExpression="PuntajeAsignadoParaNoPlantacion" />
                                                        <asp:CheckBoxField DataField="ValidoParaPlantacion" 
                                                            HeaderText="<%$ Resources:ValidoParaPlantacion%>" />
                                                        <asp:CheckBoxField DataField="ValidoParaNoPlantacion" 
                                                            HeaderText="<%$ Resources:ValidoParaNoPlantacion%>" />
                                                        <asp:CheckBoxField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                                                            SortExpression="Activo">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:CheckBoxField>
                                                        <%-- <asp:TemplateField HeaderText="<%$ Resources:Seleccionar%>">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSeleccion" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#0099FF" />
                                                </asp:GridView>
           </Content>

</asp:AccordionPane>


<asp:AccordionPane ID="AccordionPane2" runat="server">
<Header> <strong class="style2">
                                                <asp:Label ID="lblSubTitulo" runat="server" Text="<%$ Resources:SubTitulo %>"></asp:Label>
                                                </strong></Header>
<Content><table class="style3">
                                                                <tr>
                                                                    <td class="style4">
                                                                        <asp:Label ID="lblNombreES0" runat="server" Text="<%$ Resources:NombreES%>"></asp:Label>
                                                                    </td>
                                                                    <td class="style5">
                                                                        <asp:TextBox ID="txtParametroNombreES" runat="server" Width="200px"></asp:TextBox>
                                                                    </td>
                                                                    <td class="style6">
                                                                        <asp:Label ID="lblNombreEN0" runat="server" Text="<%$ Resources:NombreEN%>"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="style7">
                                                                        <asp:TextBox ID="txtParametroNombreEN" runat="server" Width="250px"></asp:TextBox>
                                                                    </td>
                                                                    <td align="center" class="style8">
                                                                        <asp:Label ID="lblActivo0" runat="server" Text="<%$ Resources:Activo%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkParametroActivo" runat="server" Checked="True" />
                                                                    </td>
                                                                </tr>
                                                            </table><table class="style3">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkParametroAplicaListaDeNA_OK_X" runat="server" 
                                                                Text="Aplica Lista NA, OK y X" TextAlign="Left" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkParametroAplicaListaDeS_A_G_N" runat="server" 
                                                                Text="<%$ Resources:AplicaListaS_A_G_N%>" TextAlign="Left" />
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkParametroAplicaCatalogoDetalleListaDeNA_OK_X" 
                                                                runat="server" Text="Aplica Detalles Para la Lista NA, OK y X" 
                                                                TextAlign="Left" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkParametroAplicaCatalogoDetalleListaDeS_A_G_N" 
                                                                runat="server" Text="<%$ Resources:AplicaDetallesParaListaS_A_G_N%>" 
                                                                TextAlign="Left" />
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkParametroNValoresListaNA_OK_X" runat="server" 
                                                                Text="NValores Seleccionables Para Lista NA, OK y X" TextAlign="Left" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkParametroNValoresListaDeS_A_G_N" runat="server" 
                                                                Text="NValores Seleccionables Para Lista S, A, G y NA" TextAlign="Left" />
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                                <table class="style3">
                                                    <tr>
                                                        <td class="style9">
                                                            <asp:Label ID="lblPuntajeAsignado0" runat="server" 
                                                                Text="<%$ Resources:PuntajeAsignado %>"></asp:Label>
                                                        </td>
                                                        <td class="style10">
                                                            <asp:TextBox ID="txtParametroPuntajeAsignado" runat="server" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td class="style11">
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:Label ID="lblPuntajeAsignado1" runat="server" 
                                                                Text="<%$ Resources:PuntajeAsignadoNoPlantacion %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtParametroPuntajeAsignadoNoPlantacion" runat="server" 
                                                                Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:UpdatePanel ID="upnlParametrosSave" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnParametroSave" runat="server" Height="30px" 
                                                            onclick="btnParametroSave_Click" Text="<%$ Resources:Guardar%>" Width="120px" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnParametroSave" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:Button ID="btnParametroCancelar" runat="server" Height="30px" 
                                                    onclick="btnParametroCancelar_Click" Text="<%$ Resources:Cancelar%>" 
                                                    Width="120px" />

                                                <div ID="pager1">
                                                    <form>
                                                    <img src="../comun/img/first.png" class="first"/>
                                                    <img src="../comun/img/prev.png" class="prev"/>
                                                     <span class="pagedisplay" style="font-size: 15px"></span>
                                                    <img src="../comun/img/next.png" class="next"/>
                                                    <img src="../comun/img/last.png" class="last"/>
                                                    <select class="pagesize" name="D2">
                                                        <option value="999999">
                                                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Limit%>" />
                                                        </option>
                                                        <option value="2">
                                                            <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:2PorPagina%>" />
                                                        </option>
                                                        <option value="5">
                                                            <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:5PorPagina%>" />
                                                        </option>
                                                        <option value="10">
                                                            <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:10PorPagina%>" />
                                                        </option>
                                                    </select>
                                                    </form>
                                                </div>
                                                <asp:GridView ID="gvParametrosPorGrupo" runat="server" 
                                                    AutoGenerateColumns="False" DataKeyNames="idParametroPorGrupoGrowing" 
                                                    DataSourceID="dstParametros" onprerender="gvParametrosPorGrupo_PreRender" 
                                                    onrowcreated="gvParametrosPorGrupo_RowCreated" 
                                                    onrowdatabound="gvParametrosPorGrupo_RowDataBound" 
                                                    onselectedindexchanged="gvParametrosPorGrupo_SelectedIndexChanged" 
                                                    Width="100%" BackColor="White" ShowHeaderWhenEmpty="True">
                                                    <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" 
                                                            HeaderText="idParametroPorGrupoGrowing" InsertVisible="False" ReadOnly="True" 
                                                            SortExpression="idParametroPorGrupoGrowing" />
                                                        <asp:CommandField ShowSelectButton="True" SelectText="" />
                                                        <asp:BoundField DataField="NombreES" HeaderText="<%$ Resources:NombreES%>" 
                                                            SortExpression="NombreES" />
                                                        <asp:BoundField DataField="NombreEN" HeaderText="<%$ Resources:NombreEN%>" 
                                                            SortExpression="NombreEN" />
                                                        <asp:CheckBoxField DataField="AplicaListaDeNA_OK_X" 
                                                            HeaderText="<%$ Resources:AplicaListaN_OK _X%>" 
                                                            SortExpression="AplicaListaDeNA_OK_X" />
                                                        <asp:CheckBoxField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" 
                                                            HeaderText="<%$ Resources:AplicaDetallesParaLista_N_OK_X%>" 
                                                            SortExpression="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:CheckBoxField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" 
                                                            HeaderText="<%$ Resources:NValoresN_OK_X%>" 
                                                            SortExpression="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:CheckBoxField DataField="AplicaListaDeS_A_G_N" 
                                                            HeaderText="<%$ Resources:AplicaListaS_A_G_N%>" 
                                                            SortExpression="AplicaListaDeS_A_G_N" />
                                                        <asp:CheckBoxField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" 
                                                            HeaderText="<%$ Resources:AplicaDetallesParaListaS_A_G_N%>" 
                                                            SortExpression="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:CheckBoxField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" 
                                                            HeaderText="<%$ Resources:NValoresS_A_G_N%>" 
                                                            SortExpression="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" 
                                                            HeaderText="<%$ Resources:PuntajeAsignado %>" 
                                                            SortExpression="PuntajeAsignado" />
                                                        <asp:CheckBoxField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                                                            SortExpression="Activo" />
                                                             <asp:TemplateField HeaderText="<%$ Resources:Seleccionar%>">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSeleccion" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#0099FF" />
                                                </asp:GridView>
                                                </Content>
</asp:AccordionPane>
<asp:AccordionPane ID="AccordionPane3" runat="server">
<Header>Listas</Header>
<Content> <table class="style3">
                                                    <tr>
                                                        <td class="style14">
                                                            <table class="style3">
                                                                <tr>
                                                                    <td>
                                                                        <strong class="style2">
                                                                        <asp:Label ID="lblSubTituloListaN_OK_X" runat="server" 
                                                                            Text="<%$ Resources:SubTituloListaN_OK_X %>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td align="right" class="style12">
                                                                        <asp:Label ID="lblPara" runat="server" Text="<%$ Resources:Para %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDatoParametro" runat="server" style="font-weight: 700"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%">
                                                            <table class="style3">
                                                                <tr>
                                                                    <td>
                                                                        <strong class="style2">
                                                                        <asp:Label ID="lblSubTituloListaS_A_G_N" runat="server" 
                                                                            Text="<%$ Resources:SubTituloListaS_A_G_N %>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td align="right" class="style12">
                                                                        <asp:Label ID="lblPara0" runat="server" Text="<%$ Resources:Para %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDatoParametro0" runat="server" style="font-weight: 700"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style14">
                                                            <table class="style3">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblNombreES1" runat="server" Text="<%$ Resources:NombreES%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCatalogoNombreES_NA_OK_X" runat="server" Width="200px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblNombreEN1" runat="server" Text="<%$ Resources:NombreEN%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCatalogoNombreEN_NA_OK_X" runat="server" Width="250px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblActivo1" runat="server" Text="<%$ Resources:Activo%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCatalogoActivo_NA_OK_X" runat="server" Checked="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div ID="pager4">
                                                                            <img class="first" src="../comun/img/first.png" />
                                                                            <img class="prev" src="../comun/img/prev.png" />
                                                                            <span class="pagedisplay" style="font-size: 15px"></span>
                                                                            <img class="next" src="../comun/img/next.png" />
                                                                            <img class="last" src="../comun/img/last.png" />
                                                                            <select class="pagesize" name="D3">
                                                                                <option value="999999">
                                                                                    <asp:Literal ID="Literal17" runat="server" Text="<%$ Resources:Limit%>" />
                                                                                </option>
                                                                                <option value="2">
                                                                                    <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:2PorPagina%>" />
                                                                                </option>
                                                                                <option value="5">
                                                                                    <asp:Literal ID="Literal19" runat="server" Text="<%$ Resources:5PorPagina%>" />
                                                                                </option>
                                                                                <option value="10">
                                                                                    <asp:Literal ID="Literal20" runat="server" Text="<%$ Resources:10PorPagina%>" />
                                                                                </option>
                                                                            </select>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <table class="style13">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnCatalogoCancelar_NA_OK_X" runat="server" Height="30px" 
                                                                                        onclick="btnCatalogoCancelar_NA_OK_X_Click" Text="<%$ Resources:Cancelar%>" 
                                                                                        Width="120px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnCatalogoSave_NA_OK_X" runat="server" Height="30px" 
                                                                                        onclick="btnCatalogoSave_NA_OK_X_Click" Text="<%$ Resources:Guardar%>" 
                                                                                        Width="120px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:GridView ID="gvCatalogoListaN_OK_X" runat="server" 
                                                                            AutoGenerateColumns="False" BackColor="White" 
                                                                            DataKeyNames="idCatalogoListaNA_OK_X_PorParametro" 
                                                                            DataSourceID="dstCatalogoListaN_OK_X" 
                                                                            onprerender="gvCatalogoListaN_OK_X_PreRender" 
                                                                            onrowcreated="gvCatalogoListaN_OK_X_RowCreated" 
                                                                            onrowdatabound="gvCatalogoListaN_OK_X_RowDataBound" 
                                                                            onselectedindexchanged="gvCatalogoListaN_OK_X_SelectedIndexChanged" 
                                                                            ShowHeaderWhenEmpty="True" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="idCatalogoListaNA_OK_X_PorParametro" 
                                                                                    HeaderText="idCatalogoListaNA_OK_X_PorParametro" InsertVisible="False" 
                                                                                    ReadOnly="True" SortExpression="idCatalogoListaNA_OK_X_PorParametro" />
                                                                                <asp:CommandField SelectText="" ShowSelectButton="True" />
                                                                                <asp:BoundField DataField="DescripcionES" HeaderText="<%$ Resources:NombreES%>" 
                                                                                    SortExpression="DescripcionES" />
                                                                                <asp:BoundField DataField="DescripcionEN" HeaderText="<%$ Resources:NombreEN%>" 
                                                                                    SortExpression="DescripcionEN" />
                                                                                <asp:CheckBoxField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                                                                                    SortExpression="Activo" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#0099FF" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <br />
                                                                        <asp:SqlDataSource ID="dstCatalogoListaN_OK_X" runat="server" 
                                                                            ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                                                                            SelectCommand="spr_ObtenerCatalogoListaNA_OK_X_PorParametro" 
                                                                            SelectCommandType="StoredProcedure">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="hidIdParametroGrupoGrowing" DefaultValue="" 
                                                                                    Name="idParametroPorGrupoGrowing" PropertyName="Value" Type="Int32" />
                                                                                <asp:ControlParameter ControlID="hidEsEnEspanol" DefaultValue="true" 
                                                                                    Name="EsEnEspanol" PropertyName="Value" Type="Boolean" />
                                                                                <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                                                                                    Type="Int32" />
                                                                                <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                                                                                    Type="String" />
                                                                            </SelectParameters>
                                                                        </asp:SqlDataSource>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%">
                                                            <table class="style3">
                                                                <tr>
                                                                    <td width="100px">
                                                                        <asp:Label ID="lblNombreES2" runat="server" Text="<%$ Resources:NombreES%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCatalogoNombreES_S_A_G_N" runat="server" Width="200px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblNombreEN2" runat="server" Text="<%$ Resources:NombreEN%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCatalogoNombreEN_S_A_G_N" runat="server" Width="250px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblActivo2" runat="server" Text="<%$ Resources:Activo%>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCatalogoActivo_S_A_G_N" runat="server" Checked="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <table class="style13">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnCatalogoCancelar_S_A_G_N0" runat="server" Height="30px" 
                                                                                        onclick="btnCatalogoCancelar_S_A_G_N_Click" Text="<%$ Resources:Cancelar%>" 
                                                                                        Width="120px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnCatalogoSave_S_A_G_N" runat="server" Height="30px" 
                                                                                        onclick="btnCatalogoSave_S_A_G_N_Click" Text="<%$ Resources:Guardar%>" 
                                                                                        Width="120px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div ID="pager3">
                                                                            <form>
                                                                            <img src="../comun/img/first.png" class="first"/>
                                                                            <img src="../comun/img/prev.png" class="prev"/>
                                                                            <span class="pagedisplay" style="font-size: 15px"></span>
                                                                            <img src="../comun/img/next.png" class="next"/>
                                                                            <img src="../comun/img/last.png" class="last"/>
                                                                            <select class="pagesize" name="D1">
                                                                                <option value="999999">
                                                                                    <asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:Limit%>" />
                                                                                </option>
                                                                                <option value="2">
                                                                                    <asp:Literal ID="Literal14" runat="server" Text="<%$ Resources:2PorPagina%>" />
                                                                                </option>
                                                                                <option value="5">
                                                                                    <asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:5PorPagina%>" />
                                                                                </option>
                                                                                <option value="10">
                                                                                    <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:10PorPagina%>" />
                                                                                </option>
                                                                            </select>
                                                                            </form>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:GridView ID="gvCatalogoListaS_A_G_N" runat="server" 
                                                                            AutoGenerateColumns="False" BackColor="White" 
                                                                            DataKeyNames="idCatalogoListaS_A_G_N_PorParametro" 
                                                                            DataSourceID="dstCatalogoListaS_A_G_N" 
                                                                            onprerender="gvCatalogoListaS_A_G_N_PreRender" 
                                                                            onrowcreated="gvCatalogoListaS_A_G_N_RowCreated" 
                                                                            onrowdatabound="gvCatalogoListaS_A_G_N_RowDataBound" 
                                                                            onselectedindexchanged="gvCatalogoListaS_A_G_N_SelectedIndexChanged" 
                                                                            ShowHeaderWhenEmpty="True" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="idCatalogoListaS_A_G_N_PorParametro" 
                                                                                    HeaderText="idCatalogoListaS_A_G_N_PorParametro" InsertVisible="False" 
                                                                                    ReadOnly="True" SortExpression="idCatalogoListaS_A_G_N_PorParametro" />
                                                                                <asp:CommandField SelectText="" ShowSelectButton="True" />
                                                                                <asp:BoundField DataField="DescripcionES" HeaderText="<%$ Resources:NombreES%>" 
                                                                                    SortExpression="DescripcionES" />
                                                                                <asp:BoundField DataField="DescripcionEN" HeaderText="<%$ Resources:NombreEN%>" 
                                                                                    SortExpression="DescripcionEN" />
                                                                                <asp:CheckBoxField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                                                                                    SortExpression="Activo" />
                                                                            </Columns>
                                                                            <SelectedRowStyle BackColor="#0099FF" />
                                                                        </asp:GridView>
                                                                        <br />
                                                                        <asp:SqlDataSource ID="dstCatalogoListaS_A_G_N" runat="server" 
                                                                            ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                                                                            SelectCommand="spr_ObtenerCatalogoListaS_A_G_N_PorParametro" 
                                                                            SelectCommandType="StoredProcedure">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="hidIdParametroGrupoGrowing" DefaultValue="" 
                                                                                    Name="idParametroPorGrupoGrowing" PropertyName="Value" Type="Int32" />
                                                                                <asp:ControlParameter ControlID="hidEsEnEspanol" DefaultValue="true" 
                                                                                    Name="EsEnEspanol" PropertyName="Value" Type="Boolean" />
                                                                                <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                                                                                    Type="Int32" />
                                                                                <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                                                                                    Type="String" />
                                                                            </SelectParameters>
                                                                        </asp:SqlDataSource>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table></Content>
</asp:AccordionPane>


</Panes>
</asp:Accordion>      

<td><div id="pager" >
	    
    </div>
                                    <table class="style3">
                                        <tr>
                                            <td width="50%">
                                                
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </td>

                                      
                                    
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table bgcolor="#F0F5E5" class="style1" width="100%">
                                        <tr>
                                            <td>
                                                <asp:SqlDataSource ID="dstGrupoGrowing" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                                                    SelectCommand="spr_ObtenerGrupoGrowingActivos" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="0" Name="idGrupoGrowing" Type="Int32" />
                                                        <asp:ControlParameter ControlID="hidEsEnEspanol" DefaultValue="true" 
                                                            Name="EsEnEspanol" PropertyName="Value" Type="Boolean" />
                                                        <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                                                            Type="Int32" />
                                                        <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                                                            Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:HiddenField ID="hidApListaDeNA_OK_X" runat="server" />
                                                <asp:HiddenField ID="hidApCatalogoDetalleListaDeNA_OK_X" runat="server" />
                                                <asp:HiddenField ID="hidApListaDeS_A_G_N" runat="server" />
                                                <asp:HiddenField ID="hidApCatalogoDetalleListaDeS_A_G_N" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="nav">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>


                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:SqlDataSource ID="dstParametros" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                                                    SelectCommand="spr_ObtenerParametrosPorGrupoGrowing" 
                                                    SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hidIdGrupoGrowing" DefaultValue="" 
                                                            Name="idGrupoGrowing" PropertyName="Value" Type="Int32" />
                                                        <asp:ControlParameter ControlID="hidEsEnEspanol" DefaultValue="true" 
                                                            Name="EsEnEspanol" PropertyName="Value" Type="Boolean" />
                                                        <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                                                            Type="Int32" />
                                                        <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                                                            Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <br />
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="style3">
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td align="right">
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="style3">
                                                    <tr>
                                                        <td class="style4">
                                                            &nbsp;</td>
                                                        <td class="style5">
                                                            &nbsp;</td>
                                                        <td class="style6">
                                                            &nbsp;</td>
                                                        <td align="left" class="style7">
                                                            &nbsp;</td>
                                                        <td align="center" class="style8">
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hidEsEnEspanol" runat="server" />
                                                <asp:HiddenField ID="hidPKNA_OK_X" runat="server" />
                                                <asp:HiddenField ID="hidIdGrupoGrowing" runat="server" />
                                                <asp:HiddenField ID="hidPKS_A_G_N" runat="server" />
                                                <asp:HiddenField ID="hidIdParametroGrupoGrowing" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>

                &nbsp;</td>
        </tr>
    </table>
    
    <div>
    </div>

</div>
</asp:Content>