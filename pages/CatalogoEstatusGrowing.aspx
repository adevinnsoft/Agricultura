<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false" CodeFile="CatalogoEstatusGrowing.aspx.cs" Inherits="pages_CatalogoEstatusGrowing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
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
    </style>
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                $('#<%=gvEstatusGrowing.ClientID%>')
                .tablesorter({
                    widthFixed: true,
                    widgets: ["zebra", "filter"],
                    widgetOptions: {
                        filter_childRows: false,
                        filter_hideFilters: false,
                        filter_ignoreCase: true,
                        filter_reset: '.reset',
                        filter_saveFilters: false,
                        filter_searchDelay: 300,
                        filter_startsWith: false,
                        filter_hideFilters: false
                    }
                })
                .tablesorterPager({ container: $("#pager") });


            });
        }
      
        function OnlyNumbers(Evento) {
            var iAscii;
            if (Evento.keyCode) {
                iAscii = Evento.keyCode;
            }
            else if (Evento.which) {
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
        input.disabled{ display:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container">
		<h1><asp:Label ID="lblBienvenido" runat="server" meta:resourceKey="lblBienvenido"></asp:Label></h1>
		  <asp:UpdatePanel ID="updPrincipal" runat="server">
            <ContentTemplate>
            <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
			 <h1><asp:Label ID="lblTitulo" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label></h1>
             <table class="index">
                <tr>
                    <td colspan="2"><h2><asp:Label ID="Label1" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label></h2></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblNombreES" Text="<%$ Resources:Nombre%>" runat="server"></asp:Label></td>
                    <td style="display: block; text-align: left;"><asp:TextBox ID="txtNombreEspanol" runat="server" ></asp:TextBox><asp:Label ID="lt_ES" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label></td>
                    <td><asp:Label ID="lblActivo" Text="<%$ Resources:Activo%>" runat="server"></asp:Label></td>
                    <td><asp:CheckBox ID="chkActivo" runat="server" Checked="True" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td style="display: block; text-align: left;"><asp:TextBox ID="txtNombreIngles" runat="server"  ></asp:TextBox><asp:Label ID="lt_EN" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources:Guardar%>" />
                        <asp:Button ID="btnActualizar" runat="server"  OnClick="btnSave_Click" Text="<%$ Resources:Actualizar%>" Visible="False" />
                        <asp:Button ID="btnLimpiar" runat="server" OnClick="btnCancelar_Click" Text="<%$ Resources:Limpiar%>" />
                        <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="<%$ Resources:Cancelar%>" Visible="False" />
                    </td>
                </tr>
            </table>
                <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>
                <asp:SqlDataSource ID="dstEstatusGrowing" runat="server" ConnectionString="<%$ ConnectionStrings:dbConn %>"
                    SelectCommand="spr_ObtenerEstatusGrowing" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="idEstatusGrowing" Type="Int32" DefaultValue="0" />
                        <asp:Parameter DefaultValue="true" Name="EsEnEspanol" Type="Boolean" />
                        <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" Type="Int32" />
                        <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
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
                <div>
                    <asp:GridView CssClass="gridView" ID="gvEstatusGrowing" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="idEstatusGrowing" DataSourceID="dstEstatusGrowing" Width="100%"
                        OnSelectedIndexChanged="gvEstatusGrowing_SelectedIndexChanged" OnRowCreated="gvEstatusGrowing_RowCreated"
                        OnRowDataBound="gvEstatusGrowing_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="idEstatusGrowing" HeaderText="idEstatusGrowing" InsertVisible="False"
                                ReadOnly="True" SortExpression="idEstatusGrowing" />
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="NombreES" HeaderText="<%$ Resources:Nombre%>" SortExpression="NombreES">
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreEN" HeaderText="<%$ Resources:NombreEN%>" SortExpression="NombreEN">
                            </asp:BoundField>
                            <asp:BoundField DataField="Activo" HeaderText="<%$ Resources:Activo%>" SortExpression="Activo">
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle BackColor="#0099FF" />
                    </asp:GridView>
              </div>
            </div>
            </asp:Panel>
            </ContentTemplate>
              <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="btnSave" />
                  <asp:AsyncPostBackTrigger ControlID="gvEstatusGrowing" />
              </Triggers>
          </asp:UpdatePanel>
     </div>
</asp:Content>