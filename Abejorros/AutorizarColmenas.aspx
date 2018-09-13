<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AutorizarColmenas.aspx.cs" Inherits="AutorizarColmenas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1><asp:Label runat="server" ID="lblTitulo" Text="Autorización de Colmenas"></asp:Label></h1>
        <table class="index">
            <tr>
                <td><h2><asp:Label runat="server" ID="lblMensaje" Text=""></asp:Label></h2></td>
            </tr>
        </table>
        
    </div>
</asp:Content>

