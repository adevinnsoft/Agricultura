<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false" CodeFile="ColoresPalet.aspx.cs" Inherits="pages_ColoresPalet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <%--<script src="..code.jquery.com/ui/1.11.4/jquery-ui.js"></script>--%>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=gvConfiguracion.ClientID%>')
                .tablesorter({
                    // hidden filter input/selects will resize the columns, so try to minimize the change
                    widthFixed: true,

                    // initialize zebra striping and filter widgets
                    widgets: ["zebra", "filter"],

                    // headers: { 5: { sorter: false, filter: false } },

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
                .tablesorterPager({ container: $(".pager") });
        });

        function OnlyNumbers(Evento) {
            var iAscii;
            if (Evento.keyCode) {//Microsoft Compatibility
                iAscii = Evento.keyCode;
            }
            else if (Evento.which) {//Mozila Compatibility
                iAscii = Evento.which;
            }
            else {
                return false;
            }

            if (((iAscii < 48) || (iAscii > 57)) && (iAscii != 13)) {
                return false;
            }
        }
      </script>

    <style type="text/css">
     
        .tablesorter .filtered {
            display: none;
        }
        /* Ajax error row */
        .tablesorter .tablesorter-errorRow td {
            text-align: center;
            cursor: pointer;
            background-color: #e6bf99;
        }
        
        table#ctl00_ContentPlaceHolder1_chkVariedad {
    width:770px;
    margin-left: 30px;
}

        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class = "container">
        <h1><asp:Label ID="lblTitulo" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label></h1>
        <table class="index">
            <tr><td colspan="4"><h2> <asp:Label ID="lblDetalle" Text="<%$ Resources:Detalle%>" runat="server"></asp:Label></h2></td></tr>
            <tr>
                <td><asp:Label ID="lblMinutoInicio" Text="<%$ Resources:MinutoInicio%>" runat="server"></asp:Label></td>
                <td><asp:TextBox ID="txtMinutoInicio" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblActivo" Text="<%$ Resources:Activo%>" runat="server"></asp:Label></td>
                <td><asp:CheckBox ID="chkActivo" runat="server" ForeColor="Black" Checked="True" /></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblMinutoFin" Text="<%$ Resources:MinutoFin%>" runat="server"></asp:Label></td>
                <td><asp:TextBox ID="txtMinutoFin" runat="server"></asp:TextBox></td>
                <td> <asp:Label ID="lblPaletaColores" Text="<%$ Resources:PaletaColores%>" runat="server"></asp:Label></td>
                <td><asp:TextBox ID="txtColorP" runat="server" class="required color /*{pickerFaceColor:'transparent',pickerFace:3,pickerBorder:0,pickerInsetColor:'black'}*/" meta:resourceKey="txtColorPResource1"  ></asp:TextBox></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
            
            <td>
                <asp:Label ID="Label1" Text="<%$ Resources:Variedades%>" runat="server"></asp:Label>
            </td>
            </tr>
            <tr>

                <td colspan="4">
                 
                    <asp:CheckBoxList runat="server" ID="chkVariedad" RepeatDirection="Horizontal" RepeatColumns="8" ></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                   <asp:Button ID="btnGuardar" runat="server" Text="<%$ Resources:Guardar%>" OnClick="btnGuardar_Click" />
                   <asp:Button ID="btnActualizar" runat="server" Text="<%$ Resources:Actualizar%>" OnClick="btnGuardar_Click" Visible="False" />
                   <asp:Button ID="btnCancelar" runat="server" CssClass="bienvenida" Text="<%$ Resources:Cancelar%>" OnClick="btnCancelar_Click" Visible="False" />
                   <asp:Button ID="btnLimpiar" runat="server" CssClass="bienvenida" Text="<%$ Resources:Limpiar%>" OnClick="btnCancelar_Click"  />
                </td>
            </tr>
        </table>
         
         <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>

                <asp:SqlDataSource ID="dstColoresPalet" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                    SelectCommand="spr_ObtenerColoresDePalet" 
                    SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hidEsEnEspanol" DefaultValue="true" 
                            Name="EsEnEspanol" PropertyName="Value" Type="Boolean" />
                        <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                            Type="Int32" />
                        <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                            Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>

        <div class="grid">
        <div id="Div1" class="pager">
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
			 <asp:GridView ID="gvConfiguracion" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idColoresDePalet" DataSourceID="dstColoresPalet" 
                Width="" onselectedindexchanged="gvConfiguracion_SelectedIndexChanged" 
                onrowdatabound="gvConfiguracion_RowDataBound" CssClass="gridView" 
        onrowcreated="gvConfiguracion_RowCreated">
        <Columns>
            <asp:BoundField DataField="idColoresDePalet" ReadOnly="True" 
                SortExpression="idColoresDePalet">
            <ControlStyle Font-Size="1pt" Width="1px" />
            <HeaderStyle Font-Size="1pt" Width="1px" />
            <ItemStyle Font-Size="1pt" Width="1px" />
            </asp:BoundField>
            <asp:CommandField ShowSelectButton="True" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:CommandField>
            <asp:BoundField DataField="MinutoInicio" HeaderText="<%$ Resources:MinutoInicio%>" 
                InsertVisible="False" ReadOnly="True" SortExpression="MinutoInicio" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="MinutoFin" HeaderText="<%$ Resources:MinutoFin%>" 
                SortExpression="MinutoFin" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="Color" HeaderText="<%$ Resources:Color%>" 
                SortExpression="Color" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
             <asp:BoundField DataField="idVariedad" HeaderText="<%$ Resources:Variedad%>" 
                SortExpression="idVariedad" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                SortExpression="Activo" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
           
        </Columns>
        <SelectedRowStyle BackColor="#3399FF" />
    </asp:GridView>
        </div>
    </div>
           


  <%--  <div class = "container" style="width: 900px">
    <table bgcolor="#F0F5E5" class="style1" width="100%">
        <tr>
            <td class="style2">
                <strong>
                <asp:Label ID="lblTitulo" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label>
                </strong></td>
        </tr>
        <tr>
            <td class="style3">
                <strong>
                <asp:Label ID="lblDetalle" Text="<%$ Resources:Detalle%>" runat="server"></asp:Label>
                </strong></td>
        </tr>
        <tr>
            <td class="style2">
                <table class="style4">
                    <tr class="style9">
                        <td class="style13">
                            <strong>
                            <asp:Label ID="lblMinutoInicio" Text="<%$ Resources:MinutoInicio%>" runat="server"></asp:Label>
                            </strong></td>
                        <td>
                            <asp:TextBox ID="txtMinutoInicio" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td class="style11">
                            <strong>
                            <asp:Label ID="lblMinutoFin" Text="<%$ Resources:MinutoFin%>" runat="server"></asp:Label>
                            </strong></td>
                        <td width="150px">
                            <asp:TextBox ID="txtMinutoFin" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td class="style12">
                            <strong>
                            <asp:Label ID="lblPaletaColores" Text="<%$ Resources:PaletaColores%>" runat="server"></asp:Label>
                            </strong></td>
                        <td width="200px">
                            
                            <asp:TextBox ID="txtColorP" runat="server" class="required color {pickerFaceColor:'transparent',pickerFace:3,pickerBorder:0,pickerInsetColor:'black'}"
                                    meta:resourceKey="txtColorPResource1"></asp:TextBox>
&nbsp;</td>
                        <td class="style14">
                            <strong>
                            <asp:Label ID="lblActivo" Text="<%$ Resources:Activo%>" runat="server"></asp:Label>
                            </strong></td>
                        <td>
                            <asp:CheckBox ID="chkActivo" runat="server" ForeColor="Black" Checked="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style13">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td class="style12">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table class="style1">
                    <tr>
                    
                        <td valign="top" width="49%">
                            
                            <br />
                <asp:Button ID="btnGuardar" runat="server" Text="<%$ Resources:Guardar%>" 
                                onclick="btnGuardar_Click" Height="30px" Width="120px" />
                <asp:Button ID="btnActualizar" runat="server"  Text="<%$ Resources:Actualizar%>" 
                                onclick="btnGuardar_Click" Height="30px" Width="120px" Visible="False" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="bienvenida" 
                    Text="<%$ Resources:Cancelar%>" onclick="btnCancelar_Click" Height="30px" 
                                Width="120px" Visible="False" />
                <asp:Button ID="btnLimpiar" runat="server" CssClass="bienvenida" 
                    Text="<%$ Resources:Limpiar%>" onclick="btnCancelar_Click" Height="30px" 
                                Width="120px" />
                        </td>
                        <td valign="top" width="2%">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>

                <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>

                <asp:SqlDataSource ID="dstColoresPalet" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                    SelectCommand="spr_ObtenerColoresDePalet" 
                    SelectCommandType="StoredProcedure">
                    <SelectParameters>
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
    </table>
    <div id="pager" >
	<form>
		<img src="../comun/img/first.png" class="first"/>
		<img src="../comun/img/prev.png" class="prev"/>
		<span class="pagedisplay" style="font-size: 15px"></span>
		<img src="../comun/img/next.png" class="next"/>
		<img src="../comun/img/last.png" class="last"/>
		<select class="pagesize">
			<option value=""><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Limit%>" /></option>
			<option value="2"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:2PorPagina%>" /></option>
			<option value="5"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:5PorPagina%>" /></option>
			<option value="10"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:10PorPagina%>" /></option>
		</select>
	</form>
</div>
<div>
    <asp:GridView ID="gvConfiguracion" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idColoresDePalet" DataSourceID="dstColoresPalet" 
                Width="100%" onselectedindexchanged="gvConfiguracion_SelectedIndexChanged" 
                onrowdatabound="gvConfiguracion_RowDataBound" CssClass="grayView" 
        onrowcreated="gvConfiguracion_RowCreated">
        <Columns>
            <asp:BoundField DataField="idColoresDePalet" ReadOnly="True" 
                SortExpression="idColoresDePalet">
            <ControlStyle Font-Size="1pt" Width="1px" />
            <HeaderStyle Font-Size="1pt" Width="1px" />
            <ItemStyle Font-Size="1pt" Width="1px" />
            </asp:BoundField>
            <asp:CommandField ShowSelectButton="True" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:CommandField>
            <asp:BoundField DataField="MinutoInicio" HeaderText="<%$ Resources:MinutoInicio%>" 
                InsertVisible="False" ReadOnly="True" SortExpression="MinutoInicio" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="MinutoFin" HeaderText="<%$ Resources:MinutoFin%>" 
                SortExpression="MinutoFin" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="Color" HeaderText="<%$ Resources:Color%>" 
                SortExpression="Color" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                SortExpression="Activo" >
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
        </Columns>
        <SelectedRowStyle BackColor="#3399FF" />
    </asp:GridView>
</div>
</div>--%>
</asp:Content>



        
        
        
