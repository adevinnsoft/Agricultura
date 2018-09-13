<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeFile="GrupoGrowing.aspx.cs" Inherits="pages_GrupoGrowing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- load jQuery and tablesorter scripts -->
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <style type="text/css">
       
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

        function pageLoad() {
            var spanish = '<%=Session["Locale"].ToString() %>' == 'es-MX' ? true : false;
            $('.gridView td').map(function () {
                switch ($(this).text()) {
                    case 'False': $(this).text(spanish ? 'No' : 'No'); break;
                    case 'True': $(this).text(spanish ? 'Si' : 'Yes'); break;
                    default: break;
                }
            });
            $('#<%=gvGruposGrowing.ClientID%>')
                .tablesorter({
                    widthFixed: true,
                    widgets: ["zebra", "filter"],
                    headers: { 0: { sorter: true, filter: true },
                        3: { sorter: true, filter: true },
                        4: { sorter: true, filter: true },
                        5: { sorter: true, filter: true },
                        6: { sorter: true, filter: true },
                        8: { sorter: true, filter: true },
                        10: { sorter: true, filter: true },
                        11: { sorter: true, filter: true }
                    },
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

                $(function () {

                    $('#<%=txtPuntajeAsignadoPlantacion.ClientID%>').live('change', function () {
                        var txt = $(this);
                        if ($(this).val() != '') {
                            $(txt).prev().attr('checked', true);
                        }
                        else {
                            $(txt).prev().removeAttr('checked');
                        }
                    });
                    $('#<%=txtPuntajeAsignadoNoPlantacion.ClientID%>').live('change', function () {
                        var txt = $(this);
                        if ($(this).val() != '') {
                            $(txt).prev().attr('checked', true);
                        } else {
                            $(txt).prev().removeAttr('checked');
                        }
                    });
                });
        }
       

    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>


    <div class = "container">
    <h1> <asp:Label ID="lblTitulo" Text="<%$ Resources:Titulo%>" runat="server"></asp:Label></h1>
    <table class="index">
      <tr>
         <td><asp:Label ID="lblNombreES" Text="<%$ Resources:NombreES%>" runat="server"></asp:Label></td>  
         <td class="floatnone left"><asp:TextBox ID="txtNombreEspanol" runat="server"></asp:TextBox><asp:Label ID="lt_ES" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_ES %>"></asp:Label></td>  
         
         <td><asp:Label ID="Label1" Text="<%$ Resources:AplicaListaN_OK _X%>" runat="server"></asp:Label></td>  
         <td><asp:CheckBox ID="chkAplicaListaDeNA_OK_X" runat="server" Text="" /></td>  
         
         <td><asp:Label ID="Label2" Text="<%$ Resources:AplicaDetallesParaListaS_A_G_N%>" runat="server"></asp:Label></td>  
         <td><asp:CheckBox ID="chkAplicaCatalogoDetalleListaDeS_A_G_N" runat="server"  /></td>  
         
         <td><asp:Label ID="lblActivo" Text="<%$ Resources:Activo%>" runat="server"></asp:Label></td>  
         <td><asp:CheckBox ID="chkActivo" runat="server" Checked="True" /></td>  
     </tr>
     <tr>
         <td></td>
         <td class="floatnone left"><asp:TextBox ID="txtNombreIngles" runat="server" ></asp:TextBox><asp:Label ID="lt_EN" runat="server" CssClass="lengua" Text="<%$ Resources:Commun, lt_EN %>"></asp:Label></td>  
         <td><asp:Label ID="Label3"  Text="<%$ Resources:AplicaListaS_A_G_N%>" runat="server"></asp:Label></td>  
         <td><asp:CheckBox ID="chkAplicaListaDeS_A_G_N" runat="server"  /></td>  
         <td><asp:Label ID="Label4"   runat="server" Text="<%$ Resources:AplicaDetallesParaLista_N_OK_X%>"></asp:Label></td>  
         <td><asp:CheckBox ID="chkAplicaCatalogoDetalleListaDeNA_OK_X" runat="server" /></td>  
         <td></td>
         <td></td>
     </tr>
     <tr runat="server" visible="false">
         <td><asp:Label ID="Label5" Text="<%$ Resources:ValidoParaPlantacion%>"  runat="server"></asp:Label></td>  
         <td colspan="2" class="left floatnone">
            <asp:CheckBox ID="chkValidoParaPlantacion" runat="server"  hidden/>
            <asp:TextBox ID="txtPuntajeAsignadoPlantacion" runat="server" Text="0"></asp:TextBox>
            <asp:Label ID="lblPuntajeAsignado"  CssClass="lengua" Text="<%$ Resources:PuntajeAsignado %>" runat="server"></asp:Label>
         </td>  
         <td></td>
         <td><asp:Label ID="Label6"  Text="<%$ Resources:ValidoParaNoPlantacion%>" runat="server"></asp:Label></td>  
         <td colspan="2" class="left floatnone">
            <asp:CheckBox ID="chkValidoParaNoPlantacion" runat="server"  hidden/>
            <asp:TextBox ID="txtPuntajeAsignadoNoPlantacion" runat="server"  Text="0"></asp:TextBox>
            <asp:Label ID="lblPuntajeAsignado0"  CssClass="lengua" Text="<%$ Resources:PuntajeAsignado %>" runat="server"></asp:Label>
         </td>   
         <td></td>
     </tr>
     <tr>
         <td colspan="8">
             <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="<%$ Resources:Actualizar%>" Visible="False" /> 
             <asp:Button ID="btnGuardar" runat="server" onclick="btnSave_Click" Text="<%$ Resources:Guardar%>" />
             <asp:Button ID="btnCancelar" runat="server" onclick="btnCancelar_Click" Text="<%$ Resources:Cancelar%>" Visible="False" />
             <asp:Button ID="btnLimpiar" runat="server" onclick="btnCancelar_Click" Text="<%$ Resources:Limpiar%>" />
          </td> 
      </tr> 
    </table>


    <div class="grid">
        <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>
        <asp:SqlDataSource ID="dstGrupoGrowing" runat="server" 
            ConnectionString="<%$ ConnectionStrings:dbConn %>" 
            SelectCommand="spr_ObtenerGrupoGrowing" 
            SelectCommandType="StoredProcedure" ProviderName="System.Data.SqlClient">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="idGrupoGrowing" Type="Int32" />
                <asp:Parameter DefaultValue="true" Name="EsEnEspanol" Type="Boolean" />
                <asp:Parameter DefaultValue="0" Direction="InputOutput" Name="NumeroDeError" 
                    Type="Int32" />
                <asp:Parameter DefaultValue=" " Direction="InputOutput" Name="MensajeDeError" 
                    Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        
      <div id="pager" class="pager">
        <form>
            <img src="../comun/img/first.png" class="first"/>
            <img src="../comun/img/prev.png" class="prev"/>
            <span class="pagedisplay" style="font-size: 15px"></span>
            <img src="../comun/img/next.png" class="next"/>
            <img src="../comun/img/last.png" class="last"/>
            <select class="pagesize text-center" name="D1">
                <option value="99999"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Limit%>" /></option>
                <option value="2"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:2PorPagina%>" /></option>
                <option value="5"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:5PorPagina%>" /></option>
                <option value="10"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:10PorPagina%>" /></option>
            </select>
        </form>
        </div>
        <div>
            <asp:GridView CssClass="gridView" ID="gvGruposGrowing" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="idGrupoGrowing" DataSourceID="dstGrupoGrowing" Width="100%" 
                        onselectedindexchanged="gvGruposGrowing_SelectedIndexChanged" 
                onrowcreated="gvGruposGrowing_RowCreated" 
                onrowdatabound="gvGruposGrowing_RowDataBound" 
                onprerender="gvGruposGrowing_PreRender">
                    <Columns>
                        <asp:BoundField DataField="idGrupoGrowing">
                        <ControlStyle Font-Size="1px" Width="1px" />
                        </asp:BoundField>
                        <asp:CommandField ShowSelectButton="True">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CommandField>
                        <asp:BoundField DataField="NombreEs" HeaderText="<%$ Resources:NombreES%>" >
                        </asp:BoundField>
                          <asp:BoundField DataField="NombreEn" HeaderText="<%$ Resources:NombreEN%>" >
                        </asp:BoundField>
                        <asp:BoundField DataField="AplicaListaDeNA_OK_X"    HeaderText="<%$ Resources:AplicaListaN_OK _X%>">  </asp:BoundField>
                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" 
                            HeaderText="<%$ Resources:AplicaDetallesParaLista_N_OK_X%>" />
                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" 
                            HeaderText="<%$ Resources:AplicaListaS_A_G_N%>" />
                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" 
                            HeaderText="<%$ Resources:AplicaDetallesParaListaS_A_G_N%>" />
                        <asp:BoundField DataField="PuntajeAsignadoParaPlantacion" HeaderText="<%$ Resources:PuntajeAsignado%>" >
                        </asp:BoundField>
                        <asp:BoundField DataField="ValidoParaPlantacion" 
                            HeaderText="<%$ Resources:ValidoParaPlantacion%>" />
                        <asp:BoundField DataField="PuntajeAsignadoParaNoPlantacion" HeaderText="<%$ Resources:PuntajeAsignado%>" >
                        </asp:BoundField>
                        <asp:BoundField DataField="ValidoParaNoPlantacion" 
                            HeaderText="<%$ Resources:ValidoParaNoPlantacion%>"  />
                        <asp:BoundField DataField="Activo" HeaderText="<%$ Resources:Activo%>" 
                            SortExpression="Activo">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                    <SelectedRowStyle BackColor="#0099FF" />
            </asp:GridView>
        </div>
    </div>
</div>
 </ContentTemplate>
   </asp:UpdatePanel>
</asp:Content>