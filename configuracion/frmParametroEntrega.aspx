<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmParametroEntrega.aspx.cs"
    Inherits="configuracion_frmParametroEntrega" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();

            $(".gvParametro").each(function () {
                if ($(this).find("tbody").find("tr").size() >= 1) {

                    window.console && console.log('aplicaFormato');
                    $(this).trigger('destroy');
                    $(this).tablesorter({
                        widthFixed: true,
                        widgets: ['zebra', 'filter'],
                        headers: { 3: { filter: false }
                        },
                        widgetOptions: {
                            zebra: ["gvParametro", "gridViewAlt"]
                            //filter_hideFilters: true // Autohide
                        }
                    }); //.tablesorterPager(pagerOptions);
                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                }
            });
            $(".gvCriterio").each(function () {

                if ($(this).find("tbody").find("tr").size() >= 1) {

                    window.console && console.log('aplicaFormato');
                    $(this).trigger('destroy');
                    $(this).tablesorter({
                        widthFixed: true,
                        widgets: ['zebra', 'filter'],
                        headers: { 1: { filter: false }
                        },
                        widgetOptions: {
                            zebra: ["gvCriterio", "gridViewAlt"]
                            , filter_hideFilters: true // Autohide
                        }
                    }); //.tablesorterPager(pagerOptions);
                    $(".tablesorter-filter.disabled").hide(); // hide disabled filters

                }


            });

            $('.gvCriterio thead .tablesorter-filter-row td input').each(function () {
                if ($(this).attr('data-column') > 1) {
                    $(this).parent().addClass('oculto');
                }
            });

          
        });

    </script>

    <style type="text/css">
        .lengua {
        float:left;
            padding-top:2px;}
        
        table.index td 
        {
            vertical-align: top !important;
        }
        table.index 
        {
            padding:10px;
            }
        input#ctl00_ContentPlaceHolder1_ckActiveParametro, input#ctl00_ContentPlaceHolder1_ckActiveCriterio
        {
            margin-left: 9px;
            }   
        input#ctl00_ContentPlaceHolder1_rlTipo_0 
        {
            margin-top: -20px;
            }
        table.index input[type="image"] 
        {
            width:24px;
            position:relative;
            right:370px;
            }   
        input:enabled 
        {
            box-shadow:none !important;
            }     
        
        .oculto
        {
            display:none;
            }
            
        table.index tr td table.gridView 
        {
            min-width:790px !important;
            max-width:790px !important;
            }
            
        .gridView td 
        {
            white-space: normal !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                     
                    <asp:HiddenField ID="hiddenIdParametroEntrega" Value="" runat="server" />
                    <asp:HiddenField ID="hiddenIdCriterio" Value="" runat="server" />
                    
                    <asp:HiddenField ID="hiddenCountCriterioNuevo" runat="server" Value="" />

                    <table class="index" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubParametro" meta:resourceKey="ltSubParametro" runat="server" /></h2>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span>*<asp:Literal ID="ltNameParametro" runat="server" meta:resourceKey="ltNameParametro" /></span>
                            </td>
                            <td align="left" >
                                <asp:TextBox ID="txtNameParametro" runat="server" meta:resourceKey="txtNameParametro"
                                    CssClass="required stringValidate"></asp:TextBox>
                                <asp:Label ID="lngSp" runat="server" meta:resourceKey="lngSp" CssClass="lengua"></asp:Label>
                                <br />
                                <br />
                                <asp:TextBox ID="txtNameParametro_EN" runat="server" meta:resourceKey="txtNameParametro"
                                    CssClass="required stringValidate"></asp:TextBox>
                                <asp:Label ID="lngEn" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label>
                            </td>
                            <td align="right">
                                <span><asp:Literal ID="ltActiveParametro" runat="server" 
                                    meta:resourceKey="ltActive" /></span>
                                </td>
                            <td align="left">
                                <asp:CheckBox ID="ckActiveParametro" runat="server" Checked="True" 
                                    meta:resourceKey="ckActiveParametro" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td align="right" class="top">
                                <span><asp:Literal ID="ltPlanta" runat="server" meta:resourceKey="ltPlanta" /></span>
                            </td>
                            <td align="left" class="top">
                                <asp:CheckBoxList ID="cblPlanta" runat="server" RepeatDirection="Horizontal" meta:resourceKey="cblPlanta"/>
                            </td>
                            <td align="right" class="top">
                                <span>*<asp:Literal ID="ltTipo" runat="server" meta:resourceKey="ltTipo" /></span>
                            </td>
                            <td align="left" class="top">
                                <asp:RadioButtonList ID="rlTipo" runat="server" meta:resourceKey="rlTipo" 
                                    TextAlign="Right" />
                            </td>
                        
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <h2>
                                    <span><asp:Literal ID="ltSubCriterio" meta:resourceKey="ltSubCriterio" runat="server" /></span></h2>
                            </td>
                        </tr>
                        <tr> 
                            <td class="top">
                                <span>*<asp:Literal ID="ltDescripcion" meta:resourceKey="ltDescripcion" runat="server" /></span>
                            </td>
                            <td>
                            <asp:TextBox ID="txtDescripcion" runat="server" 
                                    meta:resourceKey="tbDescripcion" CssClass="longStringValidate"  
                                     TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="Label3" runat="server" meta:resourceKey="lngSp" CssClass="lengua"></asp:Label>
                            </td>
                               <td class="top">
                                <span><asp:Literal ID="ltActiveCriterio" runat="server" meta:resourceKey="ltActive" /></span>
                            </td>
                            <td class="top">
                                <asp:CheckBox ID="ckActiveCriterio" runat="server" Checked="True" 
                                    meta:resourceKey="ckActiveCriterio" />
                            </td>
                           
                        </tr>
                       
               
                     <tr><td></td>
                            <td align="left">
                                <asp:TextBox ID="txtDescripcion_EN" runat="server" 
                                    meta:resourceKey="tbDescripcion_EN" CssClass="longStringValidate"   
                                     TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="Label4" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label>
                                <br />
                            </td>
                            <td></td>
                            <td align="left" valign="top">
                                <asp:ImageButton ID="btnCancelaCriterio" runat="server" CssClass="btnSave"  ImageUrl="~/comun/img/remove-icon.png"
                                    meta:resourceKey="btnCancelaCriterio" OnClick="btnCancelaCriterio_Click" />  
                                <asp:ImageButton ID="btnAgregaCriterio" runat="server" CssClass="btnSave"  ImageUrl="~/comun/img/add-icon.png"
                                    meta:resourceKey="btnAgregaCriterio" OnClick="btnAgregaCriterio_Click" /> 
                            </td>
                    </tr>
                    <tr>
                            <td colspan="4">
                                
                                
                            </td>
                    </tr>
                        
                        <tr>
                            <td colspan="5" align="center">
                                <asp:GridView ID="gvCriterio" runat="server" CssClass="gridView gvCriterio" DataKeyNames="IdCriterioParametroEntrega"
                                    OnSelectedIndexChanged="gvCriterio_SelectedIndexChanged"  DataBound="javascript:aplicaFormato();"  OnPreRender="gvCriterio_PreRender"
                                    EmptyDataText="No existen registros" ShowHeaderWhenEmpty="True" OnRowDataBound="gvCriterio_RowDataBound"
                                    AutoGenerateColumns="False" meta:resourcekey="gvCriterio">
                                    <AlternatingRowStyle CssClass="gridViewAlt" />
                                    <Columns>
                                        <asp:BoundField DataField="descripcion" HeaderText="Descripcion" SortExpression="descripcion"  meta:resourcekey="BoundFieldDescripcion" />
                                        <%--<asp:BoundField DataField="vUsuario" HeaderText="Modificado Por" SortExpression="vUsuario"  meta:resourcekey="BoundFieldUsuarioModifica" />--%>
                                        <asp:BoundField DataField="idCriterioParametroEntrega" HeaderText="Criterio" SortExpression="idCriterioParametroEntrega" ItemStyle-CssClass="oculto" HeaderStyle-CssClass="oculto" />
                                        <asp:BoundField DataField="Activo" HeaderText="activo" SortExpression="activo" ItemStyle-CssClass="oculto" HeaderStyle-CssClass="oculto" />
                                        <asp:BoundField DataField="descripcion_aux" HeaderText="descripcion_aux" SortExpression="descripcion_aux" ItemStyle-CssClass="oculto" HeaderStyle-CssClass="oculto" />
                                        <asp:TemplateField meta:resourcekey="TemplateFieldActivo" SortExpression="activo" HeaderText="Activo">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("activo") %>' meta:resourcekey="CheckBox1Resource1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" meta:resourcekey="lblActivoGridResource1"
                                                    Text='<%# (bool)Eval("activo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                    </Columns>

                                    <EmptyDataRowStyle CssClass="gridEmptyData"></EmptyDataRowStyle>
                                    <HeaderStyle CssClass="gridViewHeader"></HeaderStyle>
                                    <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
                                    <SelectedRowStyle CssClass="selected"></SelectedRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button runat="server" ID="btnSave" meta:resourceKey="btnSave" CssClass="btnSave"
                                    OnClick="btnSave_Click" />
                                <asp:Button runat="server" ID="btnClear" meta:resourceKey="btnClear" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>
                   
                    <div class="grid">
                        <div id="pager" class="pager">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                                <span class="pagedisplay"></span>
                            <img alt="next" src="../comun/img/next.png" class="next" />
                            <img alt="last" src="../comun/img/last.png" class="last" />
                            <select class="pagesize cajaCh">
                                <option value="10">10</option>
                                <option value="20">20</option>
                                <option value="30">30</option>
                                <option value="40">40</option>
                                <option value="50">50</option>
                            </select>
                        </div>
                        <asp:GridView ID="gvParametro" runat="server" CssClass="gridView gvParametro" DataKeyNames="idParametroEntrega"
                            OnSelectedIndexChanged="gvParametro_SelectedIndexChanged" OnPreRender="gvParametro_PreRender"
                            EmptyDataText="No existen registros" ShowHeaderWhenEmpty="True" OnRowDataBound="gvParametro_RowDataBound"
                            AutoGenerateColumns="False" meta:resourcekey="gvParametroResource1" 
                            onpageindexchanging="gvParametro_PageIndexChanging">
                            <AlternatingRowStyle CssClass="gridViewAlt" />
                            <Columns>
                                
                                <asp:BoundField DataField="tipoParametroEntrega" HeaderText="Tipo" SortExpression="tipoParametroEntrega"  meta:resourcekey="BoundFieldTipo" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre"  meta:resourcekey="BoundFieldNombre" />
                                <%--<asp:BoundField DataField="vUsuario" HeaderText="Modificado Por" SortExpression="vUsuario"  meta:resourcekey="BoundFieldUsuarioModifica" />--%>
                                <asp:TemplateField meta:resourcekey="TemplateFieldActivo" SortExpression="activo"  HeaderText="Activo">
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("activo") %>' meta:resourcekey="CheckBox1Resource1" />
                                     </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" meta:resourcekey="lblActivoGridResource1" 
                                            Text='<%#  (((bool)Eval("activo"))==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            </Columns>
                            <EmptyDataRowStyle CssClass="gridEmptyData"></EmptyDataRowStyle>
                            <HeaderStyle CssClass="gridViewHeader"></HeaderStyle>
                            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
                            <SelectedRowStyle CssClass="selected"></SelectedRowStyle>
                        </asp:GridView>
                    </div>
                    
                    <script type="text/javascript">
                        Sys.Application.add_load(function () {
                            registerControls();
                            $('.gvCriterio thead .tablesorter-filter-row td input').each(function () {
                                if ($(this).attr('data-column') > 1) {
                                    $(this).parent().addClass('oculto');
                                }
                            });
                        
                        });
                    </script>
               </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>