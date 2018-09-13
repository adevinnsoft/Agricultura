<%@ Control Language="C#" AutoEventWireup="true" CodeFile="popUpMessageControl.ascx.cs" Inherits="controls_popUpControl" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

    <asp:Panel ID="pnlErrorMessageControl" runat="server" CssClass="modalPopup"  width="500px" style="display:none;">
        <table style="vertical-align:middle; text-align:center; height:100%; width:100%;">
            <tr>
                <td colspan="2" style="background:#ccc repeat;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div class="alerta">
                        <img src="../comun/img/error.png" alt="" runat="server" id="imgMessageGralControl"/>   
        
                        <asp:Label ID="lblMessageGralControl" runat="server"  Text=""/>                    
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button CssClass="button" runat="server" ID="btnOKMessageGralControl" Text="Ok" onclick="btnOKMessageGralControl_Click" /></td>                
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton runat="server" ID="lnkHiddenMdlPopupControl"  Text=""  Enabled="false"/>
    <asp:ModalPopupExtender ID="mdlPopupMessageGralControl" runat="server" 
        BackgroundCssClass="modalBackground"
        PopupControlID="pnlErrorMessageControl" 
        TargetControlID="lnkHiddenMdlPopupControl" 
        CancelControlID="btnOKMessageGralControl">
    </asp:ModalPopupExtender>