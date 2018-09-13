 <%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmReporteInfestaciones.aspx.cs" Inherits="Reportes_frmReporteInfestaciones" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>

    <script type="text/javascript" id="reporteInfestaciones">

        function obtenerInfestacionesInvernadero() {

            var semanaDesde = $('#txtSemanaDesde').val().trim();
            var anioDesde = $('#txtAnioDesde').val().trim();
            var semanaHasta = $('#txtSemanaHasta').val().trim();
            var anioHasta = $('#txtAnioHasta').val().trim();

            if ((semanaDesde == '' || anioDesde == '') || (semanaDesde == '' && anioDesde == '')) {
                popUpAlert('Porfavor, Ingrese una semana y/o año iniciales', 'warning');
                return false;
            }
            else {
                if ((/^[0-9]+$/.test(semanaDesde)) == false || (/^[0-9]+$/.test(anioDesde) == false)) {
                    popUpAlert('El valor de los campos debe ser numérico.', 'warning');
                    return false;
                }
            }

            if ((semanaHasta == '' || anioHasta == '') || (semanaHasta == '' && anioHasta == '')) {
                popUpAlert('Porfavor, Ingrese una semana y/o año finales', 'warning');
                return false;
            }
            else {
                if ((/^[0-9]+$/.test(semanaHasta)) == false || (/^[0-9]+$/.test(anioHasta) == false)) {
                    popUpAlert('El valor de los campos debe ser numérico.', 'warning');
                    return false;
                }
            }
           
            try {
                $.blockUI();
                PageMethods.ObtenerInfestacionesInvernadero(semanaDesde, anioDesde, semanaHasta, anioHasta, function (response) {
                    if (response[0] == '1') {
                        $(".gridView").trigger("destroy");
                        $('#tblInfestaciones').html(response[2]);
                        registerControls();
                        $('input.tablesorter-filter[data-column="3"]').hide();
                        $('input.tablesorter-filter[data-column="4"]').hide();
                    } else {
                        popUpAlert(response[1], response[2]);
                    }
                });
            } catch (e) {
                console.log(e);
            } finally {
                $.unblockUI();
            }
           
        }
       
    </script>

    <style type="text/css">
       #tblDatos tbody tr[id="filtros"]
       {
           display:none;
       }
       
       table.index2 input[type="text"]
       {
           text-align: center;
       }
       
       table#tblInfestaciones 
       {
           max-width:900px;
       }
       table#tblDatos
       {
            text-align:right;    
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
       <h1><asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:ReporteInfestaciones %>" ></asp:Label></h1>
       <br />
       <div class="divDatos">
           <table id="tblDatos" class="index2">
             <tr>
                <td><span><asp:Label ID="lblTitulo2" runat="server" Text="Reporte de Infestaciones de la semana"></asp:Label></span></td>
                <td><span><input id="txtSemanaDesde" class="Texto" type="text" value="" placeholder="ww" /></span></td>
                <td><span><asp:Label ID="lblTitulo3" runat="server" Text="y año"></asp:Label></span></td>
                <td><span><input id="txtAnioDesde" class="Texto" type="text" value="" placeholder="yyyy"/></span></td>
             </tr>   
             <tr>
                <td><span><asp:Label ID="lblTitulo4" runat="server" Text="a la semana"></asp:Label></span></td>
                <td><span><input id="txtSemanaHasta" class="Texto" type="text" value="" placeholder="ww" /></span></td>
                <td><span><asp:Label ID="lblTitulo5" runat="server" Text="y año"></asp:Label></span></td>
                <td><input id="txtAnioHasta" class="Texto" type="text" value="" placeholder="yyyy"/><span></span></td>
                <td><input id="btnObtenerReporteInfestaciones" type="button" value="Obtener Reporte" onclick="obtenerInfestacionesInvernadero();"/></td>
             </tr>   
           </table>
       </div>
       <div id="pager" class="pager">
		    <img alt="first" src="../comun/img/first.png" class="first" />
		    <img alt="prev" src="../comun/img/prev.png" class="prev" />
            <span class="pagedisplay"></span>
		    <img alt="next" src="../comun/img/next.png" class="next" />
		    <img alt="last" src="../comun/img/last.png" class="last" />
		    <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
			    <option value="20">20</option>
			    <option value="40">40</option>
			    <option value="60">60</option>
			    <option value="80">80</option>
                <option value="100">100</option>
		    </select>
	   </div>
       <table class="gridView" id="tblInfestaciones">
        </table>
    </div>
</asp:Content>