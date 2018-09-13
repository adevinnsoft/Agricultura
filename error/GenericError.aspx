<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GenericError.aspx.cs" Inherits="error_GenericError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="container" style="padding:20px;">
    <table class="index" style="margin:20px;">
        <tr>
            <td><h1 style="float:left;"><asp:Literal ID="Literal1" runat="server" meta:resourceKey="Literal1"></asp:Literal></h1></td>
        </tr>
        <tr>
            <td><h2><asp:Literal ID="Literal2" runat="server" meta:resourceKey="Literal2"></asp:Literal></h2></td>
        </tr>
    </table>
</div>
</asp:Content>

