<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Notificaciones.aspx.cs" Inherits="Notificaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <style type="text/css">
        .tablesorter .filtered
        {
            display: none;
        }
        /* Ajax error row */
        .tablesorter .tablesorter-errorRow td
        {
            text-align: center;
            cursor: pointer;
            background-color: #e6bf99;
        }
        input.disabled
        {
            display: none;
        }
        .gridView td
        {
            white-space: pre !important;
            overflow: overlay;
            max-width:200px;
        }
        
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=gvNotificacionesClient.ClientID%>')
                .tablesorter({
                    // hidden filter input/selects will resize the columns, so try to minimize the change
                    widthFixed: true,

                    // initialize zebra striping and filter widgets
                    widgets: ["zebra", "filter"],

                    headers: { 0: { sorter: false, filter: false} },

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblCentroDeNotificaciones" runat="server" Text="<%$ Resources:Titulo%>"></asp:Label></h1>
        <table class="index">
            <tr>
              <td><asp:Label ID="lblDepartamento0" runat="server" Text="Departamento"></asp:Label></td>
              <td><asp:DropDownList ID="ddlDepartamento" runat="server" ></asp:DropDownList></td>
              <td><asp:Label ID="lblRol0" runat="server" Text="Rol"></asp:Label></td>
              <td><asp:DropDownList ID="ddlRol" runat="server" ></asp:DropDownList></td>
            </tr>
            <tr>
              <td><asp:Label ID="lblUsuario0" runat="server"  Text="Usuario"></asp:Label></td>
              <td><asp:DropDownList ID="ddlUser" runat="server" ></asp:DropDownList></td>
              <td><asp:Label ID="lblEsParaTodos0" runat="server" Text="Es Para Todos"></asp:Label></td>
              <td><asp:DropDownList ID="ddlEsParaTodos" runat="server" >
                    <asp:ListItem Value="0">No</asp:ListItem>
                    <asp:ListItem Value="1">Si</asp:ListItem>
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td><asp:Label ID="lblMensaje0" runat="server" Text="Mensaje"></asp:Label></td>
              <td colspan="3"><asp:TextBox ID="txtMensajeEdit" runat="server" TextMode="MultiLine" ></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="4"><asp:Button ID="btnEnviarNotificacion" runat="server" Text="Enviar"  OnClick="btnEnviarNotificacion_Click" /></td>
            </tr>
        </table>
        
       <div class="grid">
           <div id="pager" class="pager">
                    <form>
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
                    </form>
                </div>



                <asp:GridView  ID="gvNotificacionesClient" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="idNotificacion" DataSourceID="dstNotificaciones" Width="100%" OnRowCreated="gvNotificaciones_RowCreated"
                    OnRowDataBound="gvNotificaciones_RowDataBound" CssClass="gridView" OnPreRender="gvNotificaciones_PreRender">
                    <Columns>
                        <asp:BoundField DataField="idNotificacion" HeaderText="idNotificacion" InsertVisible="False"
                            ReadOnly="True" SortExpression="idNotificacion"></asp:BoundField>
                        <asp:CommandField ShowSelectButton="True" SelectText="Ver" />
                        <asp:BoundField DataField="Departamento" HeaderText="<%$ Resources:Departamento%>"
                            ReadOnly="True" SortExpression="Departamento"></asp:BoundField>
                        <asp:BoundField DataField="Rol" HeaderText="<%$ Resources:Rol%>" ReadOnly="True"
                            SortExpression="Rol"></asp:BoundField>
                        <asp:BoundField DataField="Usuario" HeaderText="<%$ Resources:Usuario%>" SortExpression="Usuario">
                        </asp:BoundField>
                        <asp:BoundField DataField="EsParaTodos" HeaderText="<%$ Resources:EsParaTodos%>"
                            ReadOnly="True" SortExpression="EsParaTodos"></asp:BoundField>
                        <asp:BoundField DataField="Mensaje" HeaderText="<%$ Resources:Mensaje%>" SortExpression="Mensaje" />
                        <asp:BoundField DataField="FechaCaptura" HeaderText="<%$ Resources:FechaCaptura%>"
                            SortExpression="FechaCaptura" />
                        <asp:BoundField DataField="Leida" HeaderText="<%$ Resources:Leida%>" ReadOnly="True"
                            SortExpression="Leida" />
                    </Columns>
                    <SelectedRowStyle BackColor="#0099FF" />
                </asp:GridView>
     
                <asp:SqlDataSource ID="dstNotificaciones" runat="server" ConnectionString="<%$ ConnectionStrings:dbConn %>"
                    SelectCommand="spr_ObtenerListaDeNotifcaciones" SelectCommandType="StoredProcedure"
                    ProviderName="System.Data.SqlClient">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="0" Name="idUsuario" SessionField="userIDInj"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="hidEsEnEspanol" DefaultValue="true" Name="EsEnEspanol"
                            PropertyName="Value" Type="Boolean" />
                        <asp:Parameter DefaultValue="-1" Name="NotificacionesPorMostrar" Type="Int32" />
                        <asp:Parameter DefaultValue="0" Name="NumeroDeError" Type="Int32" Direction="InputOutput" />
                        <asp:Parameter DefaultValue="  " Name="MensajeDeError" Type="String" Direction="InputOutput" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>
       </div>
    </div>
</asp:Content>
