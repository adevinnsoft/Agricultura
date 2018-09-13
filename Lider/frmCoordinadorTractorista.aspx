<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmCoordinadorTractorista.aspx.cs" Inherits="Lider_frmCoordinadorTractorista"
    EnableEventValidation="false" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/moment.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript"  src="../comun/scripts/jquery.tablesorter.js"></script> --%>
    <script type="text/javascript">
        $(function () {
            setInterval(function () { llenartablaFormaA(); }, 60000);
            registerControls();
            //alert("si salió");
            //llenartablaFormaA();
            fecha = moment('2015-11-18 17:31:13.000');
            console.log(fecha.format('YYYY-MM-DD'));
            var fecha2 = moment('2015-11-18 17:31:13.000');
            console.log(fecha2.format('HH:mm:ss'));
            llenartablaFormaA();
            //alert("si salió");
        });
        var fecha;
        function llenartablaFormaA() {
            PageMethods.llenatablaFormaA(function (response) {
                var a = 10;
                $('#StatusFormasA tbody').html(response); //.slideToggle();
            });
        }
    </script>
    <style type="text/css">
        #StatusFormasA input[type="text"]
        {
            max-width: 25px;
            border: none;
        }
        
        #StatusFormasA th
        {
            text-align: center;
        }
        
        div#idInvernadero .aerea
        {
            width: 250px;
            background: black;
            height: 150px;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:label id="lblTitle" runat="server" text="Monitoreo de Coordinador"></asp:label></h1>
        <asp:panel id="form" runat="server">
        </asp:panel>
        <div class="grid">
            <div>
                <table id="StatusFormasA">
                    <thead class="encabezadotabla">
                        <tr>
                            <th>
                                Invernadero
                            </th>
                            <th>
                                Lider
                            </th>
                            <th>
                                Forma
                            </th>
                            <th>
                                Hora Inicio
                            </th>
                            <th>
                                Tiempo en Campo
                            </th>
                            <th>
                                Estado
                            </th>
                            <th>
                                Variedad
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:content>
