<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmRecuperaContrasena.aspx.cs" Inherits="frmRecuperaContrasena" %>

<%@ Register Src="controls/ctrlRecuperaContrasena.ascx" TagName="ctrlRecuperaContrasena" TagPrefix="uc2"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>BONANZA MANUFACTURA DE CAMPO </title>    
    <link href="comun/css/Style.css" rel="stylesheet" type="text/css" />
    <link href="comun/css/login.css" rel="Stylesheet" type="text/css" />
    <script src="comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <style type="text/css">

div.ps1 { font-weight: bold; color: #000000; background-color:red; border: 1px; width:100px; border-style: solid; border-color: black; text-align: center }

div.ps2 { font-weight: bold; color: #000000; background-color:orange; border: 1px; width:100px; border-style: solid; border-color: black; text-align: center }

div.ps3 { font-weight: bold; color: #000000; background-color:yellow; border: 1px; width:100px; border-style: solid; border-color: black; text-align: center }

div.ps4 { font-weight: bold; color: #FFFFFF; background-color:blue; border: 1px; width:100px; border-style: solid; border-color: black; text-align: center }

div.ps5 { font-weight: bold; color: #FFFFFF; background-color:green; border: 1px; width:100px; border-style: solid; border-style: solid; border-color: black; text-align: center }

</style>
</head>
<body>    
    <form runat="server" id="form1" >



<div>
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td align="center" style="white-space:nowrap;">
           
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                  </asp:ToolkitScriptManager>
           
               <uc2:ctrlRecuperaContrasena ID="ctrlRecuperaContrasena1" runat="server" />
            </td>
        </tr>
    </table>
  </div>
        <br />
<span></span>
 <div style="text-align:center" >
 
           
              <asp:Image ID="Image2" runat="server" ImageUrl="~/comun/img/tomateBonanza.jpg" Width="152px" Height="71px" />
            
           
              <asp:Image ID="Image3" runat="server" ImageUrl="~/comun/img/aguacateBonanza.jpg" Width="152px" Height="71px" />
     <asp:Image ID="Image1" runat="server" ImageUrl="comun/img/greenhouse.png" Width="152px" Height="71px" />
   
 </div>
         <div style="position:fixed; width:100%; bottom:0;">
        <div class="footer" style="position:absolute;">
   <span></span> </div>  </div> 
       </form>
</body>  
</html>

