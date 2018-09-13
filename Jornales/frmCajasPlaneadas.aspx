<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCajasPlaneadas.aspx.cs" Inherits="Jornales_frmCajasPlaneadas" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../comun/scripts/jquery-1.7.2.min.js"></script>

    <style type="text/css">
        /*Cambia el cursor a puntero en la imagen de descarga*/
        .imgDescarga
        {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1><asp:Literal runat="server" ID="ltTitulo" meta:reesource="ltTitulo">Cajas Planeadas</asp:Literal></h1>

        <table class="index" border="0">
            <tr>
                <td colspan="2"><h2>Plantilla de Cajas Planeadas Precargada</h2></td>
            </tr>
            <tr>
                <td>
                    Descargar Plantilla
                    <asp:ImageButton runat="server" ID="btnPlantilla" ImageUrl="~/comun/img/download_xls.png" OnClick="btnPlantilla_Click" Style="width: 30px;" />
                </td>
                <td>
                    Importar Plantilla
                    <asp:FileUpload runat="server" ID="fu_Plantilla" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnImportar" runat="server" onclick="btnImportar_Click" Text="Importar Plantilla" />
                </td>
            </tr>
        </table>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>

