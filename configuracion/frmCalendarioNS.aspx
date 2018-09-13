<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCalendarioNS.aspx.cs" Inherits="configuracion_frmCalendarioNS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div id="Controles">
            <img src="../comun/img/prev.png" alt="prev" />
            <asp:DropDownList runat="server" ID="ddlAnio"></asp:DropDownList>
            <img src="../comun/img/prev.png" alt="next" />
        </div>
        <div id="Calendario" runat="server">
            
        </div>
    </div>
</asp:Content>

