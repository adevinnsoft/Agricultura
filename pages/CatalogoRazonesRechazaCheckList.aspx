<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatalogoRazonesRechazaCheckList.aspx.cs"
    Inherits="pages_CatalogoRazonesRechazaCheckList" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <style type="text/css">
        .style1
       
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
        input.disabled{ display:none;}
        
        span#ctl00_ContentPlaceHolder1_lt_ES, span#ctl00_ContentPlaceHolder1_lt_EN, span#ctl00_ContentPlaceHolder1_Label3, span#ctl00_ContentPlaceHolder1_Label2 {
	float: left;
}

    .grid table tr td {
	white-space: normal;
	max-width: 150px;
}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var pagerOptions = { // Opciones para el  paginador
                    container: $("#pager"),
                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                };
            $('#<%=gvRazonesDeRechazo.ClientID%>')
                .tablesorter({
                    // hidden filter input/selects will resize the columns, so try to minimize the change
                    widthFixed: true,

                    // initialize zebra striping and filter widgets
                    widgets: ["zebra", "filter"],

                    headers: {//0: { sorter: false, filter: false },
                        5: { sorter: false, filter: false, display: 'none'}
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
                .tablesorterPager(pagerOptions);
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class = "container" >
    <h1><asp:Label ID="lblTitulo" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label></h1>
    <table class="index">
    <tr>
        <td colspan="6"><h2><asp:Label ID="Label1" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label></h2></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblNombreES" Text="<%$ Resources:Nombre%>" runat="server"></asp:Label></td>
        <td><asp:TextBox ID="txtNombreEspanol" runat="server"></asp:TextBox><asp:Label ID="lt_ES" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label></td>
        <td><asp:Label ID="lblDescripcionES" Text="<%$ Resources:Descripcion%>" runat="server"></asp:Label></td>
        <td><asp:TextBox ID="txtDescripcionEspanol" runat="server"></asp:TextBox><asp:Label ID="Label3" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label></td>
        <td><asp:Label ID="lblActivo" Text="<%$ Resources:Activo%>" runat="server"></asp:Label></td>
        <td><asp:CheckBox ID="chkActivo" runat="server" Checked="True" /></td>
    </tr>
    <tr>
        <td></td>
        <td><asp:TextBox ID="txtNombreIngles" runat="server"></asp:TextBox><asp:Label ID="lt_EN" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label></td>
        <td></td>
        <td><asp:TextBox ID="txtDescripcionIngles" runat="server"></asp:TextBox><asp:Label ID="Label2" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label></td>
    </tr>
    <tr>
        <td colspan="6">
            <asp:Button ID="btnActualizar" runat="server" onclick="btnSave_Click" Text="<%$ Resources:Actualizar%>" Visible="False" />
            <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="<%$ Resources:Guardar%>" />
            <asp:Button ID="btnLimpiar" runat="server" onclick="btnCancelar_Click" Text="<%$ Resources:Limpiar%>"  />
            <asp:Button ID="btnCancelar" runat="server" onclick="btnCancelar_Click" Text="<%$ Resources:Cancelar%>"  Visible="False" />
        </td>
    </tr>
    
   </table>

    

    <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>
    <asp:SqlDataSource ID="dstRazonesDeRechazo" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dbConn %>" 
        SelectCommand="spr_ObtenerRazonesDeRechazo" 
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="idRazonDeRechazo" Type="Int32" />
            <asp:Parameter DefaultValue="true" Name="EsEnEspanol" Type="Boolean" />
            <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                Type="Int32" />
            <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    
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
      <div>
            <asp:GridView CssClass="gridView" ID="gvRazonesDeRechazo" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="idRazonDeRechazo" DataSourceID="dstRazonesDeRechazo"  
                        onselectedindexchanged="gvDefectos_SelectedIndexChanged" 
                onrowcreated="gvRazonesDeRechazo_RowCreated" 
                onrowdatabound="gvRazonesDeRechazo_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="IdRazonDeRechazo">
                            <ControlStyle Font-Size="1px" Width="1px" />
                        </asp:BoundField>
                        <asp:CommandField ShowSelectButton="True">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CommandField>
                        <asp:BoundField DataField="Activo" HeaderText="Activo" SortExpression="Activo" />
                        <asp:BoundField DataField="nombreEspanol" HeaderText="<%$ Resources:NombreES%>" />
                        <asp:BoundField DataField="nombreIngles" HeaderText="<%$ Resources:NombreEN%>" />
                        <asp:BoundField DataField="descripcionEspanol" HeaderText="<%$ Resources:DescripcionES%>">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="descripcionIngles" HeaderText="<%$ Resources:DescripcionEN%>">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                    <SelectedRowStyle BackColor="#0099FF" />
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>