<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEtapas.aspx.cs" Inherits="configuracion_frmEtapas"
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
    
    input[type="radio"]/* + label*/ {
        display:inline-block;
        width:15px;
        height:15px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left -39px  top no-repeat;
        cursor:pointer;
    }
    
    input[type="checkbox"]:checked + label{
        background:url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
    }
    
    /*input[type="radio"]:checked + label {
        background:url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
    }*/
    
    input[type="checkbox"]:disabled + label {
        /*background:none;*/
        background:url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
    }
    
    /*input[type="radio"]:disabled + label {
        background:url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
    }*/
    
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
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnIdEtapa" runat="server" />
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
                                <asp:Literal ID="ltHabilidad" runat="server" 
                                    meta:resourcekey="ltHabilidadResource1"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlHabilidades" runat="server" CssClass="required"
                                    meta:resourcekey="ddlEtapaResource1">
                                </asp:DropDownList>
                            </td>
                             <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                            </td>
                            <td align="left" style=" text-align:left;">
                                <%--<asp:CheckBox ID="chkActivo" runat="server" meta:resourcekey="chkActivoResource1" Checked="True" />--%>
                                <input checked="checked"  type="checkbox" ID="chkActivo" runat="server" class="check-with-label" />
                                <label  id="idActivolb"  class='label-for-check' for="<%=chkActivo.ClientID %>" ><span></span></label>

                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltEtapa" runat="server" meta:resourcekey="ltEtapaResource1"></asp:Literal>
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtEtapa" runat="server" CssClass="required stringValidate help"  ToolTip="<%$ Resources:Commun, in_ES %>"></asp:TextBox>
                                <asp:Label ID="lngSp" runat="server" Text="<%$ Resources:Commun, lt_ES %>" CssClass="lengua"></asp:Label>
                            </td>
                              <td align="right" style="display:none;">
                                <asp:Literal ID="ltPorTiempo" runat="server" 
                                    meta:resourcekey="ltPorTiempoResource1"></asp:Literal>
                            </td>
                            <td align="left" style=" text-align:left; display:none;">
                                <%--<asp:CheckBox ID="chkPorTiempo" runat="server" meta:resourcekey="chkPorTiempoResource1" />--%>
                                <input checked="checked"  type="checkbox" ID="chkPorTiempo" runat="server" class="check-with-label" />
                                <label  id="Label1"  class='label-for-check' for="<%=chkPorTiempo.ClientID %>" ><span></span></label>
                            </td>
                        </tr>
                         <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtEtapa_EN" runat="server" CssClass="required stringValidate help" ToolTip="<%$ Resources:Commun, in_EN %>"></asp:TextBox>
                                <asp:Label ID="lngEn" runat="server" Text="<%$ Resources:Commun, lt_EN %>" CssClass="lengua"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Literal ID="ltTarget" runat="server" Text="*Target"></asp:Literal>
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtTarget" runat="server" CssClass="required nonZeroInt32" Width="80px" ></asp:TextBox>
                            </td>
                        </tr>
                                                <tr>
                     <td style="vertical-align:top;">
                                    <asp:Label ID="lbRFamilia" runat="server" Text="<%/*$ Resources: radios*/ %>">*Familia:</asp:Label> 
                                </td>
                    <td colspan="3">
                       <table>
                            <tr style=" text-align: left;">
                                <td class="checkboxes">
                                    <asp:RadioButtonList ID="rblFamilias" runat="server" RepeatColumns="8" CssClass="label-for-check required"
                                        AutoPostBack="true" onselectedindexchanged="rblFamilias_SelectedIndexChanged" ></asp:RadioButtonList>
                                    <%-- <div ID="divRadiosFamilias" runat="server"></div>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                      <td style="vertical-align:top;">
                                    <asp:Label ID="lbRNivel" runat="server" Text="<%/*$ Resources: radios*/ %>">*Nivel:</asp:Label> 
                                </td>
                    <td colspan="3">
                        <table>
                            <tr style=" text-align: left;">
                                <td class="checkboxes">
                                    <asp:RadioButtonList ID="rblNiveles" runat="server" RepeatColumns="8" CssClass="required" 
                                        ></asp:RadioButtonList>
                                    <%--<div ID="divRadiosNiveles" runat="server"></div>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                        <tr><td colspan="2">&nbsp;<asp:HiddenField ID="hddIdNivel" runat="server" /></td><td colspan="2">    <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSaveResource1" CssClass="btnSave"
                OnClick="btnSave_Click" />
            <asp:Button ID="btnClear" runat="server" meta:resourcekey="btnClearResource1" OnClick="btnClear_Click" /></td></tr>
                         </table>

               <script type="text/javascript">
                   Sys.Application.add_load(function () { registerControls(); setTooltips(); });
               </script>
            <div class="grid">
                <div id="pager" class="pager" style=" width:100%; min-width:100%;">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <span class="pagedisplay"style="top:-4px; position: relative;"></span>
                    <img alt="next" src="../comun/img/next.png" class="next" />
                    <img alt="last" src="../comun/img/last.png" class="last" />
                    <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px; top:-4px; position: relative;">
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="30">30</option>
                        <option value="40">40</option>
                        <option value="50">50</option>
                    </select>
                </div>
                <asp:GridView ID="gvEtapas" runat="server" AutoGenerateColumns="False" CssClass="gridView" EmptyDataText="No Data" ShowHeaderWhenEmpty="true"
                    meta:resourcekey="GridView1Resource1" DataKeyNames="IdEtapa"
                    EmptyDataRowStyle-CssClass="no-results" onprerender="gvEtapas_PreRender" 
                    onrowdatabound="gvEtapas_RowDataBound" 
                    onselectedindexchanged="gvEtapas_SelectedIndexChanged">
                    <AlternatingRowStyle CssClass="gridViewAlt" />
                    <Columns>
                    <asp:BoundField DataField="nEtapa" HeaderText="Nombre" HeaderStyle-CssClass="cajaCh" 
                        SortExpression="nEtapa" meta:resourcekey="BoundFieldResource1" 
                        HeaderStyle-HorizontalAlign ="Center" >
                    <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <%--<asp:TemplateField meta:resourcekey="BoundFieldResource2" HeaderStyle-CssClass="cajaCh" 
                        SortExpression="PorTiempo">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("PorTiempo") %>' 
                                meta:resourcekey="CheckBox1Resource1" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" 
                                meta:resourcekey="lblActivoGridResource1" 
                                Text='<%# (bool)Eval("PorTiempo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="Familia" HeaderText="Familia" 
                            SortExpression="Familia"  
                            HeaderStyle-HorizontalAlign ="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nivel" HeaderText="Nivel" 
                            SortExpression="Nivel"  
                            HeaderStyle-HorizontalAlign ="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="target" HeaderText="Target" 
                            SortExpression="target"  
                            HeaderStyle-HorizontalAlign ="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource2" HeaderStyle-CssClass="cajaCh" 
                        SortExpression="UsuarioModifica">
                        <ItemTemplate>
                            <asp:Label ID="lblUsuariogrid" runat="server" Enabled="False" CssClass="tooltip"
                                ToolTip='<%# Eval("FechaModifico") %>'
                                Text='<%# Eval("UsuarioModifica") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" CssClass="cajaCh" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource1" 
                        SortExpression="Activo">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>' 
                                meta:resourcekey="CheckBox1Resource1" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" 
                                meta:resourcekey="lblActivoGridResource1" 
                                Text='<%# (bool)Eval("Activo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'></asp:Label>
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
