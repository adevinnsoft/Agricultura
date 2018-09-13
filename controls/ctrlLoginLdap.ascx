<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ctrlLoginLdap.ascx.cs" Inherits="controls_ctrlLoginLdap" %>

    

     
 
           
 <div style="text-align:center">        
   
     <asp:Image ID="Image1" runat="server" ImageUrl="~/comun/img/bonanza.png" />
   
 </div>    
            
 <asp:Label ID="lblError" runat="server" Font-Bold="True" CssClass="alert" ></asp:Label>        
<table class="index2" align="center" runat="server" id="tblLogin">
<tr><td colspan="3">
 <h1 style="width:350px; max-width:350px; min-width:350px;" >
     <asp:Literal ID="ltBienvenido" runat="server"  meta:resourceKey="ltBienvenido"></asp:Literal>
 
 </h1>
<h1 style="width:350px;">Portal de Administración de Invernaderos</h1>
<h1 style="width:350px; max-width:350px; min-width:350px;">
<asp:Label ID="ltWelcome" runat="server" meta:resourceKey="ltWelcome" Text="Greenhouse Management Portal"></asp:Label>
</h1>
</td>
</tr> 
    <tr>
        <td style="text-align:right; white-space:nowrap;">
           <h3> <asp:Literal ID="Literal1" runat="server" Text="Username / Usuario" meta:resourceKey="Literal1"></asp:Literal></h3>
            
        </td>
        <td align="left" style="width:150px;">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="controls" Width="150px"></asp:TextBox>
            
        </td>
        <td align="left" style="width:150px; white-space:nowrap;">
        <asp:RequiredFieldValidator meta:resourceKey="validator1" ID="rfvUsername" runat="server" ErrorMessage="*Campo requerido/Required Field" ForeColor="#ff6600" ControlToValidate="txtUsername" ></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td style="text-align:right; white-space:nowrap;">
           <h3> <asp:Literal ID="Literal2" runat="server" Text="Password / Contraseña" meta:resourceKey="Literal2"></asp:Literal></h3>
          
        </td>
        <td align="left"> 
            <asp:TextBox ID="txtPassword" runat="server" CssClass="controls" TextMode="Password"  Text="3p7sGC" Width="150px"></asp:TextBox>
            
        </td>
        <td align="left"><asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="*Campo requerido/Required Field" ForeColor="#ff6600" ControlToValidate="txtPassword" ></asp:RequiredFieldValidator> </td>
    </tr>
          
    <tr>

        <td>
            <asp:HyperLink ID="lnkRecuperaContrasena" runat="server" ForeColor="Yellow" NavigateUrl="~/frmRecuperaContrasena.aspx">Recuperar contraseña</asp:HyperLink>
        </td>

        <td colspan="2">
            <asp:LinkButton ID="lnkLogin" runat="server"  CssClass="save"  
                onclick="lnkLogin_Click" meta:resourceKey="lnkLogin" Text="LOGIN / ENTRAR" BackColor="Yellow" ForeColor="Navy"></asp:LinkButton>
        </td>
    </tr>
   
    <tr>

        <td colspan="3">
            &nbsp;</td>
    </tr>
   
</table>
 
