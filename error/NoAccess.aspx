<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NoAccess.aspx.cs" Inherits="NoAccess" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="container">
      <table class="index" style="margin:20px;" cellspacing="0" cellpadding="0" border="0" align="center"><tr><td><h2 style="float:left; text-align:left; color:#f60;">
   
          <asp:Literal ID="Literal1" runat="server" meta:resourceKey="Literal1"></asp:Literal>
          </h2>
    </td></tr>
<tr><td ><h3 style="text-align:left; color:#000;"> <asp:Literal ID="Literal2" runat="server" meta:resourceKey="Literal2"></asp:Literal>
.</h3></td></tr>
<tr><td  style="text-align:left;"><h3 style="text-align:left; color:#000;">  <asp:Literal ID="Literal3" runat="server" meta:resourceKey="Literal3"></asp:Literal>

    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/frmLogin.aspx" CssClass="inicio" meta:resourceKey="HyperLink1"></asp:HyperLink></h3></td></tr>
</table>

    </div>

 </asp:Content>