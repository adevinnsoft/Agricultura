<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ctrlConfirm.ascx.cs" Inherits="controls_ctrlConfirm" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.50401.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>


<asp:Panel ID="pnlCapturaDosisControl" runat="server" CssClass="modalPopup" 
    Style="padding:5px; display:none;" width="500px" 
    meta:resourcekey="pnlCapturaDosisControlResource1" >

    <table style="vertical-align:middle; text-align:left; height:100%; width:100%; background:rgb(240, 245, 229);">
             <tr>
            <td colspan="2">
                <h3 style="float:none;"><asp:Label runat="server" ID="Label2" 
                        Text ="¿Está seguro que desea Actualizar?" meta:resourcekey="Label2Resource1"></asp:Label></h3>
            </td>
        </tr> 
        
        <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
             
                        <img src="../comun/img/confirm.png" runat="server" id="img1"/>   
       &nbsp;</td><td> 
                        <asp:Label ID="lblMessageGralControl" runat="server" 
                                meta:resourcekey="lblMessageGralControlResource1"/>                    
        
                    </ContentTemplate>
                    </asp:UpdatePanel>

                </td>

            </tr>
            <tr>
                <td colspan="2">
                <asp:Button CssClass="button" runat="server" ID="btnCancelar" Text="No" 
                        meta:resourcekey="btnCancelarResource1"/>
                <asp:Button CssClass="button" runat="server" ID="btnConfirmar" Text="Sí" 
                        OnClick="btnConfirmar_Click" meta:resourcekey="btnConfirmarResource1" />                
                <asp:Button CssClass="button" runat="server" ID="btnOKMessageGralControl"  
                        Text="Cancelar" style="display: none;" 
                        meta:resourcekey="btnOKMessageGralControlResource1" />
                </td>
            </tr>
        </table>
        
        
        
</asp:Panel>
    
        <asp:LinkButton runat="server" ID="lnkHiddenMdlPopupControl"  
    Enabled="False" meta:resourcekey="lnkHiddenMdlPopupControlResource1"/>
        <asp:ModalPopupExtender ID="mdlPopupMessageGralControl" runat="server" 
            BackgroundCssClass="modalBackground"
            PopupControlID="pnlCapturaDosisControl" 
            TargetControlID="lnkHiddenMdlPopupControl" 
            CancelControlID="btnOKMessageGralControl" DynamicServicePath="" 
    Enabled="True">
        </asp:ModalPopupExtender>