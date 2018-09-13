<%@ Page Title="Bonanza Fresh" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Head" runat="server" ContentPlaceHolderID="Head">
    <style type="text/css">
        .style1
        {
            width: 498px;
            text-align:left;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentPlaceHolder1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <%--    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
    </p>--%>

	<div class="container">
		<h1><asp:Label ID="lblBienvenido" runat="server"></asp:Label></h1>
		<asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
			<table class="index">
				<tr>
                    <td colspan="2" align="left">
                        <h2><span></span></h2>
                    </td>
                    </tr>
                    <tr>
                    <td align="center" class="style1" >
                        <span>
                           <
                        </span>
                        <br/>
                        <br/>
                        <span>
                            <b>Año: </b><%DateTime.Now.Year.ToString(); %>.
                        </span>
                    </td>                    
                    <td align="left">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <a href="frmLogin.aspx">INICIAR SESION</a>
                    </td>
                    
                </tr>
			</table>
		</asp:Panel>
	</div>

</asp:Content>
