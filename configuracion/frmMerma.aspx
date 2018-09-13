<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmMerma.aspx.cs" Inherits="configuracion_Merma" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <%--<link href="../comun/css/ts_theme.jui.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">

        $(function () {
            //Inicio
            //Paginador      
            registerControls();
            
//            //Validación de campos antes de bd
//            $('#<%=btn_Enviar.ClientID%>').click(
//            function () {
//                var razon = $('#<%=txtMerma.ClientID%>').val().trim();
//                var Errores = "";
//                Errores += razon.length == 0 ? 'El campo razón es requerido. </br>' : '';
//                if (Errores.length > 0) {
//                    popUpAlert(Errores, 'error');
//                    return false;
//                }
//                else {
//                    return true;
//                }
//            });




            //FIN
        });
        //         var pagerOptions = { // Opciones para el  paginador
        //             container: $("#pager"),
        //             output: '{page} ' + 'de' + ' {totalPages}'
        //         };

        //         $("#ctl00_ContentPlaceHolder1_gvColor")
        //     .tablesorter({
        //         widthFixed: true,
        //         widgets: ['zebra', 'filter'],
        //         widgetOptions: {
        //             zebra: ["gridView", "gridViewAlt"]
        //             //filter_hideFilters: true // Autohide
        //         }
        //     }).tablesorterPager(pagerOptions);
        // });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
    <h1>
        &nbsp;
        <asp:Label runat="server" meta:resourcekey="Merma" Text=""></asp:Label>
    </h1>
    <asp:Panel ID="form" runat="server" DefaultButton="btnHidden" 
            meta:resourcekey="formResource1">
        <table class="index">
            <tr>
                <td align="left" colspan="6">
                    <h2>
                        <asp:Label runat="server" Text="" meta:resourcekey="R_Merma"></asp:Label>
                    </h2>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="blockscreen" runat="server" style="display: none">
                        <div style="width: 550px; height: 44px; position: absolute; top: 50%; left: 50%;
                            margin-left: -275px; margin-top: -22px; text-align: center;">
                            <img alt="" src="../comun/img/cargando.gif" />
                        </div>
                    </div>
                </td>
            </tr>
            <%--<table width="800">--%>
            <tr>
                <td>
                    <asp:Literal ID="ltMaxHarvest" runat="server" meta:resourcekey="ltMaxHarvestResource1"
                        Text="*Razón :"></asp:Literal>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtMerma" runat="server" Width="300px" MaxLength="50" CssClass="required"
                        meta:resourcekey="txtMermaResource1"></asp:TextBox>
                </td>
              <td rowspan=2><asp:Literal ID="Literal1" runat="server" meta:resourcekey="ltCategoria"
                        Text="*Categoria:">*Categoría</asp:Literal> </td>
                <td rowspan=2>
                    <asp:DropDownList runat="server" ID="ddlCategoria" DataTextField="Categoria" DataValueField="idCategoria" AutoPostBack="True" OnDataBound="ddlCategoria_DataBound" CssClass="cajaLarga" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" ></asp:DropDownList>
                </td>
                <td rowspan=2 align="right" style="text-align: right;">
                    <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1" Text="Activo"></asp:Literal>
                </td>
                <td rowspan=2 align="left">
                    <asp:CheckBox ID="Activo" runat="server" Checked="True" meta:resourcekey="ActivoResource2" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtMerma_EN" runat="server" Width="300px" CssClass="required" MaxLength="50"></asp:TextBox>
                </td>
                
                
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
                <td>
                    <asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
                    <asp:Button runat="server" ID="btnCancelar" OnClick="btnCancelar_Click" Text="Limpiar"
                        meta:resourcekey="btnCancelarResource1" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnHidden" OnClientClick="return false;" Style="position: absolute;
                        top: -50%;" meta:resourcekey="btnHiddenResource1" />
                    <asp:Button ID="btn_Enviar" runat="server" Text="Guardar" OnClick="btn_Enviar_Click"
                        CssClass="btnSave" OnClientClick="javascript:guardando();" meta:resourcekey="btn_EnviarResource1">
                    </asp:Button>
                </td>
               
            </tr>
        </table>
    </asp:Panel>
    <%--Grid View para la merma--%>
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
        <%--<asp:GridView ID="gv_Merma" >
                        onpageindexchanging="gvMerma_PageIndexChanging" Este método de utiliza para manejar el GridView con los controles del paginador
                 </asp:GridView>--%>
        <asp:GridView runat="server" ID="gv_Merma" CssClass="gridView" EmptyDataText="No existen registros"
            Width="800px" AutoGenerateColumns="False" DataKeyNames="idMerma" OnPageIndexChanging="gvMerma_PageIndexChanging"
            OnPreRender="gvMerma_PreRender" OnRowDataBound="gvMerma_RowDataBound" CellPadding="4"
            ForeColor="#333333" GridLines="None" 
            OnSelectedIndexChanged="gv_Merma_SelectedIndexChanged" 
            meta:resourcekey="gv_MermaResource1">
       
            <Columns>
                <%--<asp:BoundField DataField="Activo" HeaderText="Activo" 
                    meta:resourcekey="BoundFieldResource1" />--%>

                <asp:TemplateField SortExpression="Activo"  
                    meta:resourceKey="BoundFieldResource1" HeaderText="Activo">
								<ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("Activo")==true?(string)GetGlobalResourceObject("Commun","Si"):(string)GetGlobalResourceObject("Commun","No") %>' Enabled="False" meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
								<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>

                <asp:BoundField DataField="Razon" HeaderText="Razón" SortExpression="Razon" 
                    meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="NombreCategoria" HeaderText="Categoria" SortExpression="Categoria" />
            </Columns>

        </asp:GridView>
        <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    </div>
   
   </div>
   
</asp:Content>
