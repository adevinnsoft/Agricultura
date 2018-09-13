<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DiasEntreCorte.aspx.cs" Inherits="pages_DiasEntreCorte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <style type="text/css">
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
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=gvDias.ClientID%>')

                .tablesorter({
                    // hidden filter input/selects will resize the columns, so try to minimize the change
                    widthFixed: true,

                    // initialize zebra striping and filter widgets
                    widgets: ["zebra", "filter"],

                    headers: { 0: { sorter: false, filter: false },
                        3: { sorter: false, filter: false }
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
        .style1
        {
            width: 100%;
        }
        .style2
        {
            font-size: medium;
            color: #ACC9A3;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class = "container" style="width: 900px">
    <table bgcolor="#F0F5E5" class="style1" width="100%">
        <tr>
            <td class="style2">
                <strong>
                <asp:Label ID="lblDiasEntreCorte" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label>
                </strong></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnGuardar" runat="server" Text="<%$ Resources:Guardar%>"
                    onclick="btnGuardar_Click" Height="30px" Width="120px" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="bienvenida" 
                    Text="<%$ Resources:Cancelar%>" onclick="btnCancelar_Click" Height="30px" Width="120px" />
                </td>
        </tr>
        <tr>
            <td>

                <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>

                <asp:SqlDataSource ID="dstDiasEntreCortes" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:dbConn %>" 
                    SelectCommand="spr_ObtenerDiasEntreCorte" 
                    SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="0" Name="idUsuario" 
                            SessionField="userIDInj" Type="Int32" />
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
    <br />
   <%-- <div id="pager" >
	    <form>
		    <img src="../comun/img/first.png" class="first"/>
		    <img src="../comun/img/prev.png" class="prev"/>
		    <span class="pagedisplay" style="font-size=15px"></span>
		    <img src="../comun/img/next.png" class="next"/>
		    <img src="../comun/img/last.png" class="last"/>
		    <select class="pagesize">
			    <option value="999999"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Limit%>" /></option>
			    <option value="2"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:2PorPagina%>" /></option>
			    <option value="5"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:5PorPagina%>" /></option>
			    <option value="10"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:10PorPagina%>" /></option>
		    </select>
	    </form>
    </div>--%>
    <div>
        <asp:GridView ID="gvDias" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="Cycle" DataSourceID="dstDiasEntreCortes" 
            CssClass="grayView" onrowcreated="gvDias_RowCreated" 
            onrowdatabound="gvDias_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Cycle" InsertVisible="False" 
                    ReadOnly="True">
                <ItemStyle Font-Size="1pt" Width="1px" />
                </asp:BoundField>
                <asp:BoundField DataField="Invernadero" HeaderText="Invernadero"
                    InsertVisible="False" ReadOnly="True" SortExpression="Invernadero" />
                <asp:BoundField DataField="Variedad" HeaderText="<%$ Resources:Variedad%>" 
                    SortExpression="Variedad" >
                </asp:BoundField>
                <asp:BoundField DataField="Variable" HeaderText="<%$ Resources:Variable%>" 
                    SortExpression="Variable" />
                <asp:TemplateField HeaderText="<%$ Resources:Dias%>">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDias" runat="server" Text='<%# Bind("Dias") %>' > 
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
</asp:Content>
