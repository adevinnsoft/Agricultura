<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmCategoriaArticulos.aspx.cs"
    Inherits="Materiales_frmUnidadesDeMedida" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
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
        });
    </script>
    <style type="text/css">
        input#ctl00_ContentPlaceHolder1_txtCategoria, input#ctl00_ContentPlaceHolder1_txtCategoria_EN
        {
            min-width: 159px;
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
                    <asp:HiddenField ID="hdnIdCategoria" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltCategoria" runat="server" meta:resourcekey="ltCategoriaResource1"></asp:Literal>
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtCategoria" runat="server" meta:resourcekey="txtCategoriaResource1"
                                    CssClass="required"></asp:TextBox><asp:Label ID="lngSp" runat="server" meta:resourceKey="lngSp"
                                        CssClass="lengua"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkActivo" runat="server" meta:resourcekey="chkActivoResource1"
                                    Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtCategoria_EN" runat="server" CssClass="required" meta:resourcekey="txtCategoria_ENResource1"></asp:TextBox><asp:Label
                                    ID="lngEn" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Literal ID="ltDescripcion" runat="server" meta:resourceKey="ltDescripcion"></asp:Literal>
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtDescripcion" CssClass="required" runat="server" TextMode="MultiLine"
                                    Rows="3"></asp:TextBox>
                                <asp:Label ID="Label1" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtDescription_EN" CssClass="required" runat="server" TextMode="MultiLine"
                                    Rows="3"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" meta:resourceKey="lngSp" CssClass="lengua"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnClear" runat="server" meta:resourcekey="btnClearResource1" OnClick="btnClear_Click" />
                                <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSaveResource1" CssClass="btnSave"
                                    OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <script type="text/javascript">
                        //Sys.Application.add_load(triggers);
                        Sys.Application.add_load(function () { registerControls(); });
                    </script>
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
                        <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="False" CssClass="gridView"
                            meta:resourcekey="GridView1Resource1" DataKeyNames="IdCategoriaArticulo" OnPreRender="gvCategorias_PreRender"
                            OnRowDataBound="gvCategorias_RowDataBound" OnSelectedIndexChanged="gvCategorias_SelectedIndexChanged">
                            <AlternatingRowStyle CssClass="gridViewAlt" />
                            <Columns>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource1" SortExpression="Activo">
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>' meta:resourcekey="CheckBox1Resource1" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" meta:resourcekey="lblActivoGridResource1"
                                            Text='<%# (bool)Eval("Activo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="NombreCategoria" HeaderText="Nombre" SortExpression="NombreCategoria"
                                    meta:resourcekey="BoundFieldResource1">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ItemStyle-CssClass="wrapped"
                                    SortExpression="Descripcion" meta:resourcekey="BoundFieldResource2">
                                    <HeaderStyle HorizontalAlign="Center" Width="300px" />
                                </asp:BoundField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource2" SortExpression="vNombre">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsuariogrid" runat="server" Enabled="False" ToolTip='<%# Eval("FechaModifico") %>'
                                            Text='<%# Eval("vNombre") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="no-results" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>
