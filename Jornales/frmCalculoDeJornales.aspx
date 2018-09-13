<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCalculoDeJornales.aspx.cs" Inherits="Jornales_frmCalculoDeJornales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $(".lider").click(function () {
            $(this).next().toggle();
        });

        $(".invernadero").click(function () {
            $(this).next().toggle();
        });

        $("#Mostrar").click(function () {
            $(".accordionBody").show();
        });

        $("#Ocultar").click(function () {
            $(".accordionBody").hide();
        });
    });
</script>
<style>
      h2.accordionHead{display: block;
        text-align: left;
        padding-bottom: 3px;
        padding-left:5px;
        padding-top: 3px;
        background:url('../comun/img/accordionBg.png');
        color: #000;
        font-size: 15px;
        border: 1px solid #f60;
        width: 795px;
        cursor:pointer;}
        h3.accordionHead {display:block; text-align:left; width:100%;}
     span.inv {display:block; width:100%; font-weight:bold; text-align:left;}
     table.index tr td div.accordionBody table.gridView{width:100%; min-width:100%; max-width:100%;}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>Cálculo de Jornales</h1>
             <table class="index">
        <tr><td class="left"><div>
            <input type="radio" id="Mostrar" name="Despliegue" value="Mostrar" /><label>Mostrar Todo</label>
            <input type="radio" id="Ocultar" name="Despliegue" value="Ocultar" /><label>Ocultar Todo</label>
            <!--<button id="Mostrar" type="button" value="Mostrar">Mostrar Todo</button>
            <button id="Ocultar" type="button" value="Ocultar">Ocultar Todo</button>-->
        </div></td></tr>
   <tr><td>
            <h2 class="lider accordionHead">Fernando Barreto</h2>
            <div class="accordionBody">
                <h3 class="invernadero accordionHead">A01</h3>
                <div class="accordionBody">
                    <table class="gridView">
                        <thead>
                            <tr>
                                <th>Actividad</th>
                                <th>Duración (hrs)</th>
                                <th>Jornales (Target)</th>
                                <th>Jornales (Histórico)</th>
                                <th>Jornales (Autorizados)</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>PYV E2</td>
                                <td>3</td>
                                <td>4</td>
                                <td>3</td>
                                <td><input type="text" id="txtAutorizados" value="5" /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <h3 class="invernadero accordionHead">A02</h3>
                <div class="accordionBody">
                <span class="inv">Invernadero A02</span>
                </div>
            </div>
            <h2 class="lider accordionHead">Marcos Aranda</h2>
            <div class="accordionBody">
                <h3 class="invernadero accordionHead">A03</h3>
                <div class="accordionBody">
                    Invernadero A03
                </div>
            </div>
        </td>
        </tr>
        </table>
    </div>
</asp:Content>
