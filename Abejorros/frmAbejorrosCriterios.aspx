<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAbejorrosCriterios.aspx.cs" Inherits="frmAbejorrosCriterios"
    MasterPageFile="~/MasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

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
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();
            setTooltips();
        });

        function setTooltips() {
            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: false,
                trigger: 'hover',
                position: 'right'
            });

            $('.tooltip').tooltipster({
                animation: 'grow',
                delay: 200,
                theme: 'tooltipster-punk',
                touchDevices: false,
                trigger: 'hover',
                contentAsHTML: true,
                interactive: true
            });
        }
        
    </script>
<style type="text/css">
    input.Error {
        border: 1px solid red;
        background: rgba(255,0,0,0.2);
    }
    input[type="checkbox"], input[type="radio"]  {
        display:none;
    }
    input[type="checkbox"] + label {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
        cursor:pointer;
    }
    
        input[type="radio"] + label {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left -39px  top no-repeat;
        cursor:pointer;
    }
    
    input[type="checkbox"]:checked + label{
        background:url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
    }
    
    input[type="radio"]:checked + label {
        background:url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
    }
    
    input[type="checkbox"]:disabled + label {
        /*background:none;*/
        background:url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
    }
    
    input[type="radio"]:disabled + label {
        background:url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
    }
    
    .check-with-label:checked + .label-for-check {
        font-weight: bold;
        color:#C12929;
    }

    .check-with-label:disabled + .label-for-check {
        color:gray;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Criterios"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" Text="Capture o edite los criterios para los abejorros"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" rowspan="2" class="top">
                                <asp:Literal ID="ltEtapa" runat="server" Text="*Criterio:"></asp:Literal>
                            </td>
                            <td class="left floatnone">
                                <asp:TextBox ID="txtCriterio" runat="server" CssClass="required help cajaLarga"  MaxLength="150" ToolTip="<%$ Resources:Commun, in_ES %>"></asp:TextBox>
                                <asp:Label ID="lngSp" runat="server" Text="<%$ Resources:Commun, lt_ES %>" CssClass="lengua"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" Text="<%$ Resources:Commun, Active %>"></asp:Literal>
                            </td>
                            <td align="left" style=" text-align:left;">
                                <%--<asp:CheckBox ID="chkActivo" runat="server" meta:resourcekey="chkActivoResource1" Checked="True" />--%>
                                <input checked="checked"  type="checkbox" ID="chkActivo" runat="server" class="check-with-label" />
                                <label  id="idActivolb"  class='label-for-check' for="<%=chkActivo.ClientID %>" ><span></span></label>

                            </td>
                        </tr>
                         <tr>
                            <td class="left floatnone">
                                <asp:TextBox ID="txtCriterio_EN" runat="server" CssClass="required help cajaLarga"  MaxLength="150" ToolTip="<%$ Resources:Commun, in_EN %>"></asp:TextBox>
                                <asp:Label ID="lngEn" runat="server" Text="<%$ Resources:Commun, lt_EN %>" CssClass="lengua"></asp:Label>
                            </td>
                            <td align="right" colspan="2" style="text-align: center;">
                                
                            </td>
                        </tr>
                <tr>
                    <td rowspan="2" class="top">
                        <asp:Literal ID="ltDescripcion" runat="server" Text="Descripción:" 
                            meta:resourcekey="ltDescripcionResource1"></asp:Literal>
                    </td>
                    <td colspan="3" class="left floatnone">
                        <asp:TextBox ID="txtDescripcion_ES" CssClass="help" runat="server" 
                            TextMode="MultiLine" Rows="3" Columns="60"  
                            ToolTip="<%$ Resources:Commun, in_ES %>" 
                            meta:resourcekey="txtDescripcion_ESResource1"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_ES %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtDescripcion_EN" CssClass="help" runat="server" 
                            TextMode="MultiLine" Rows="3" Columns="60"
                            ToolTip="<%$ Resources:Commun, in_EN %>" 
                            meta:resourcekey="txtDescripcion_ENResource1"></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_EN %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hddIdCriterio" runat="server" />
                    </td>
                    <td colspan="2">    
                        <asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="btnSave" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Cancelar" OnClick="btnClear_Click" />
                    </td>
                </tr>
            </table>

               <script type="text/javascript">
                   Sys.Application.add_load(function () { registerControls(); setTooltips(); });
               </script>
            <div class="grid">
                <div id="pager" class="pager">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <span class="pagedisplay"></span><%--<input type="text" class="pagedisplay" />--%>
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
                <asp:GridView ID="gvEtapas" runat="server" AutoGenerateColumns="False" CssClass="gridView" EmptyDataText="No Data" ShowHeaderWhenEmpty="true"
                    meta:resourcekey="GridView1Resource1" DataKeyNames="idCriterio"
                    EmptyDataRowStyle-CssClass="no-results" onprerender="gvEtapas_PreRender" 
                    onrowdatabound="gvEtapas_RowDataBound" 
                    onselectedindexchanged="gvEtapas_SelectedIndexChanged">
                    <AlternatingRowStyle CssClass="gridViewAlt" />
                    <Columns>
                    <asp:TemplateField HeaderText="Nombre" HeaderStyle-CssClass="cajaMed"
                        SortExpression="nCriterio">
                        <ItemTemplate>
                            <asp:Label ID="lblUsuariogrid" runat="server" Enabled="False" CssClass="tooltip"
                                ToolTip='<%# Eval("nDescripcion") %>'
                                Text='<%# Eval("nCriterio") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource1" HeaderText="Activo" HeaderStyle-CssClass="cajaCh"
                        SortExpression="Activo">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>' 
                                meta:resourcekey="CheckBox1Resource1" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" 
                                meta:resourcekey="lblActivoGridResource1" 
                                Text='<%# (bool)Eval("Activo")==true?(string)GetGlobalResourceObject("Commun", "Si"):(string)GetGlobalResourceObject("Commun", "No") %>'></asp:Label>
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
