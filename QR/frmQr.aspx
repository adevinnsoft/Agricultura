<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmQr.aspx.cs" Inherits="QR_frmQr"
    MasterPageFile="~/MasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
    <table>
        <tr>
        <td>
            <asp:Label runat="server" ID="lblPrefix" Text="Prefijo"></asp:Label>
        </td>
            <td>
            <asp:TextBox runat="server" ID="txt_Prefix"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            <asp:Label runat="server" ID="Label1" Text="Inicio"></asp:Label>
            </td>
            <td>
            <asp:TextBox runat="server" ID="txtInicio"></asp:TextBox>
            </td>
            <td>
            <asp:Label runat="server" ID="Label2" Text="Fin"></asp:Label>
            </td>
            <td>
            
            <asp:TextBox runat="server" ID="txtFin"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="Genera Codigos" ID="GeneraQr" onclick="GeneraQr_Click" />
            </td>
        </tr>
    </table>
        
        
        
        
    </div>
</asp:Content>
