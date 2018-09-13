<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmCategoriaPercance.aspx.cs" 
    Inherits="configuracion_frmCategoriaPercance"  MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
        
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
    <style type="text/css">
        .style3
        {
            height: 10px;
        }
        .oculto
        {
            display: none;
        }
        .style4
        {
            width: 502px;
        }
        .style5
        {
            height: 10px;
            width: 431px;
        }
        .style6
        {
            width: 431px;
        }
        .style7
        {
            height: 10px;
            width: 502px;
        }
        .style8
        {
            width: 152px;
        }
        .style9
        {
            height: 10px;
            width: 152px;
        }
        
        .popUp
        {
            background: rgba(0,0,0,.1);
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            text-align: center;
            padding-top: 5%;
            z-index: 99999999;
            display: none;
        }
        input.Error
        {
            border: 1px solid red;
            background: rgba(255,0,0,0.2);
        }
        
        input#ctl00_ContentPlaceHolder1_txtNombre, input#ctl00_ContentPlaceHolder1_txtNombre_EN
        {
            width: 159px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1><asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hiddenIdCategoriaPercance" runat="server"/>
                    <table class="index" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan=4 align="left">
                                    <h2>
                                        <asp:Literal ID="ltCategoriaPercance" meta:resourceKey="ltCategoriaPercance" runat="server" />
                                    </h2>
                                </td>
                            </tr>
                            <tr>
                                <td>*<asp:Literal ID="ltNombre" meta:resourceKey="ltNombre" runat="server" /></td>
                                <td class="left"><asp:TextBox ID="txtNombre" meta:resourceKey="txtNombre" runat="server" CssClass="required stringValidate"/>
                                <asp:Label ID="lngSp" runat="server" meta:resourceKey="lngSp" CssClass="lengua"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:TextBox ID="txtNombre_EN" meta:resourceKey="txtNombre_EN" runat="server" CssClass="required stringValidate"/>
                                <asp:Label ID="lngEn" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label>
                                </td>
                                <td><asp:Literal ID="ltActivo" meta:resourceKey="ltActivo" runat="server" /></td>
                                <td class="left"><asp:CheckBox ID="ckActivo"  meta:resourceKey="ckActivo" runat="server" /> </td>
                            </tr>
                            <tr>
                                <td>*<asp:Literal ID="ltDescripcion" meta:resourceKey="ltDescripcion" runat="server" /> </td>
                                <td colspan="3" class="left">
                                    <asp:TextBox ID="txtDescripcion" meta:resourceKey="txtDescripcion" runat="server" TextMode="MultiLine" CssClass="required longStringValidate"/>
                                    <asp:Label ID="Label1" runat="server" meta:resourceKey="lngSp" CssClass="lengua"></asp:Label>
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:TextBox ID="txtDescripcion_EN" meta:resourceKey="txtDescripcion_EN" runat="server"  TextMode="MultiLine" CssClass="required longStringValidate"/>
                                    <asp:Label ID="Label2" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label> </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <asp:Button runat="server" ID="btnSave" meta:resourceKey="btnSave" CssClass="btnSave"
                                        OnClick="btnSave_Click" />
                                    <asp:Button runat="server" ID="btnClear" meta:resourceKey="btnClear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                <ContentTemplate>
                    <script type="text/javascript">
                        Sys.Application.add_load(function () { registerControls(); });
                    </script>
                    <div class="grid">
                        <div id="pager" class="pager">
                        </div>
                    </div><asp:GridView ID="gvCategoriaPercances" runat="server" CssClass="gridView" Width="90%" DataKeyNames="idCategoriaPercance"
                            OnSelectedIndexChanged="gvCategoriaPercances_SelectedIndexChanged" OnPreRender="gvCategoriaPercances_PreRender"
                            EmptyDataText="No existen registros" ShowHeaderWhenEmpty="True" OnRowDataBound="gvCategoriaPercances_RowDataBound"
                            AutoGenerateColumns="False" meta:resourcekey="gvCategoriaPercancesResource1" 
                            onpageindexchanging="gvCategoriaPercances_PageIndexChanging">
                            <AlternatingRowStyle CssClass="gridViewAlt" />
                            <Columns>
                            
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre"  meta:resourcekey="BoundFieldNombre" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"  meta:resourcekey="BoundFieldDescripcion" />
                                <%--<asp:BoundField DataField="vUsuario" HeaderText="Modificado por" SortExpression="vUsuario"  meta:resourcekey="BoundFieldUsuario" />--%>
                                <%--<asp:BoundField DataField="FechaModifico" HeaderText="Fecha modificación" SortExpression="FechaModifico"  meta:resourcekey="BoundFieldFechaModifico" />--%>
                                <asp:TemplateField meta:resourcekey="TemplateFieldActivo" SortExpression="Activo" HeaderText="Activo">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>' meta:resourcekey="CheckBox1Resource1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" meta:resourcekey="lblActivoGridResource1"
                                                    Text='<%# (bool)Eval("Activo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'>
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div id="popUpHora" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="DateTimeDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarHora();" style="float:none;" /></td></tr>
       </table>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>