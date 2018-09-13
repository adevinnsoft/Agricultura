<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ctrlRecuperaContrasena.ascx.cs" Inherits="controls_ctrlRecuperaContrasena" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
  
 
           

  
 
           
 <div style="text-align:center">        
   
     <asp:Image ID="Image1" runat="server" ImageUrl="~/comun/img/bonanza.png" />
   
 </div>    
            
 <asp:Label ID="lblError" runat="server" Font-Bold="True" CssClass="alert" ></asp:Label>        
<table class="index2" align="center" runat="server" id="tblLogin">
<tr><td colspan="3" class="auto-style1">
 <h1 style="width:350px; max-width:350px; min-width:350px;" >
     <asp:Literal ID="ltBienvenido" runat="server"  meta:resourceKey="ltBienvenido" Text="PORTAL CONTROL DE INVERNADEROS"></asp:Literal>
 
 </h1>
<h1 style="width:350px;">RECUPERA TU CONTRASEÑA</h1>
</td>
</tr> 
    <tr>
        <td style="text-align:right; white-space:nowrap;">
           <h3> <asp:Literal ID="Literal1" runat="server" Text="Usuario" meta:resourceKey="Literal1"></asp:Literal></h3>
            
        </td>
        <td align="left" style="width:150px;">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="controls" Width="150px"></asp:TextBox>
            
        </td>
        <td align="left" style="width:150px; white-space:nowrap;">
        <asp:RequiredFieldValidator meta:resourceKey="validator1" ID="rfvUsername" runat="server" ErrorMessage="*Campo requerido" ForeColor="#ff6600" ControlToValidate="txtUsername" ></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td style="text-align:right; white-space:nowrap;">
            &nbsp;</td>
        <td align="left" style="width:150px;">
            &nbsp;</td>
        <td align="left" style="width:150px; white-space:nowrap;">
            &nbsp;</td>
    </tr>
    <tr>
        <td style="text-align:right; white-space:nowrap;">
           <h3> <asp:Literal ID="Literal2" runat="server" Text="Contraseña" meta:resourceKey="Literal2"></asp:Literal></h3>
          
        </td>
        <td align="left"> 
            <asp:TextBox ID="txtPassword" runat="server" CssClass="controls" TextMode="Password"  Text="3p7sGC" Width="150px"></asp:TextBox>
            
        </td>
        <td>
            

            <asp:PasswordStrength ID="PasswordStrength1"
                runat="server"
                TargetControlID="txtPassword"
                PreferredPasswordLength="10"
                StrengthIndicatorType="Text"
                PrefixText="Seguridad:"
                MinimumNumericCharacters="2"
                MinimumSymbolCharacters="1"
                HelpStatusLabelID="Label1"
                TextStrengthDescriptions="<div class='ps1'>Muy Debil</div>;<div class='ps2'>Debil</div>;<div class='ps3'>Segura</div>;<div class='ps4'>Muy Segura</div>;<div class='ps5'>Excelente</div>"
                RequiresUpperAndLowerCaseCharacters="true">
            </asp:PasswordStrength>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="*Campo requerido" ForeColor="#ff6600" ControlToValidate="txtPassword" ></asp:RequiredFieldValidator> 
        </td>
        <td align="left">&nbsp;</td>
    </tr>
          
    <tr>
        <td style="text-align:left; white-space:nowrap;" colspan="3"> &nbsp;
          <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Brown"></asp:Label>
        </td>
    </tr>
          
     <tr>
        <td style="text-align:right; white-space:nowrap;">
           <h3> <asp:Literal ID="Literal3" runat="server" Text="Confirma Contraseña" meta:resourceKey="Literal2"></asp:Literal></h3>
          
        </td>
        <td align="left"> 
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="controls" TextMode="Password"  Text="3p7sGC" Width="150px"></asp:TextBox>
            
        </td>
        <td align="left"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*Campo requerido" ForeColor="#ff6600" ControlToValidate="txtConfirmPassword" ></asp:RequiredFieldValidator> 
         </td>
    </tr>
    <tr>

        <td colspan="3">
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Contraseñas deben ser iguales" ForeColor="#FF6600"></asp:CompareValidator>
        </td>
    </tr>
   
    <tr>

        <td colspan="3">
            <asp:LinkButton ID="lnkRecuperar" runat="server"  CssClass="save"  
                 onclick="lnkRecuperar_Click"   meta:resourceKey="lnkLogin" Text="RECUPERAR" BackColor="Yellow" ForeColor="Navy"></asp:LinkButton>
        </td>
    </tr>
   
    <tr>

        <td colspan="3">
            &nbsp;</td>
    </tr>
   
</table>
 

