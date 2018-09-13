<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ctrlYesNo.ascx.cs" Inherits="controls_ctrlYesNo" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.50401.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<%--<asp:Panel ID="pnlCapturaDosisControl" runat="server" CssClass="modalPopup" Style="padding:5px; display:none;" width="500px" >--%>

    <table class="index3" align="center" style="min-width:400px; margin:5px;">
        <tr>
            <td colspan="2" style="text-align:right; background:#ffa05f;" >
                <h4><asp:Label runat="server" ID="lblBienvenida" 
                        Text ="¿Está seguro que desea actualizar el registro?" 
                        meta:resourcekey="lblBienvenidaResource1"></asp:Label></h4>
            </td>
        </tr>
        <tr>
           <td>
           
           </td>
        </tr>
        <tr>
            <td colspan="2" class="floatnone">           
                <asp:Button CssClass="button" runat="server" ID="save" Text="Sí" 
                    OnClick="save_OnClick" meta:resourcekey="saveResource1" />
                <asp:Button CssClass="button" runat="server" ID="Button1" Text="No" 
                    OnClick="cancelar2_OnClick" meta:resourcekey="Button1Resource1" />
                <asp:Button CssClass="button" runat="server" ID="btnOKMessageGralControl"  
                    Text="Cancelar" style="display: none;" 
                    meta:resourcekey="btnOKMessageGralControlResource1" />
            </td>
        </tr>
    </table>
<%--</asp:Panel>--%>
    
        <asp:LinkButton runat="server" ID="lnkHiddenMdlPopupControl"  
    Enabled="False" meta:resourcekey="lnkHiddenMdlPopupControlResource1"/>
        <asp:ModalPopupExtender ID="mdlPopupMessageGralControl" runat="server" 
            BackgroundCssClass="modalBackground"
            PopupControlID="pnlCapturaDosisControl" 
            TargetControlID="lnkHiddenMdlPopupControl" 
            CancelControlID="btnOKMessageGralControl" DynamicServicePath="" 
    Enabled="True">
        </asp:ModalPopupExtender>
        
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
