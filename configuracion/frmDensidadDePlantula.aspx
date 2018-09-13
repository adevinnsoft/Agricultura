<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmDensidadDePlantula.aspx.cs" Inherits="configuracion_frmDensidadDePlantula" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>

    <script type="text/javascript">
        
        function ObtenerSurcosPorInvernadero() {
            PageMethods.obtenerSurcosPorInvernadero(function (response) {
                if (response[0] == '1') {
                    $('#tblDensidad tbody').html(response[2].toString());
                    registerControls();
                }
                else {
                    popUpAlert(response[1])
                }
            });
            
        }


        $(function () {

            Array.prototype.uniqueObjectsDensidad = function () {
                function compare(a, b) {
                    for (var prop in a) {
                        if (a[prop] != b[prop]) {
                            return false;
                        }
                    }
                    return true;
                }
                return this.filter(function (item, index, list) {
                    for (var i = 0; i < index; i++) {
                        if (compare(item, list[i])) {
                            return false;
                        }
                    }
                    return true;
                });
            }


            var valuetxt = '';
            ObtenerSurcosPorInvernadero();
            //definimos la densidad para todos los surcos
            $('#btnDefinirDensidadTodos').click(function () {
                valuetxt = $('#densidadParaLosSurcosVisibles').val();

                $('#tblDensidad tbody tr').each(function () {
                    if ($(this).find('td input[type="text"]').val() != '') {
                        $(this).attr("value", valuetxt);
                        $(this).find('td input[type="text"]').val(valuetxt);
                    }
                    else {

                        $(this).attr("value", valuetxt);
                        $(this).find('td input[type="text"]').val(valuetxt);

                    }
                });

            });

            //definimos la densidad para los surcos visibles
            $('#btnDefinirDensidadVisibles').click(function () {
                valuetxt = $('#densidadParaLosSurcosVisibles').val();
                $('#tblDensidad tbody .densidadPlantula:not([style*="display: none;"]):not(.filtered) td input[type="text"]').val(valuetxt).change();
            });

            $('input.Densidad').live('change', function (a) {
                $(this).parent().parent().addClass("modified");
            });

            $('select#ctl00_ddlPlanta').live('change', function (a) {
                ObtenerSurcosPorInvernadero();
            });

            //funcionamiento de boton de guardado 
            $('#btnGuardarDensidad').click(function () {
                var DensidadFinal = [], Invernaderos;
                var datosIncompletos = false;
                $('#tblDensidad tbody tr td input[type="text"]').each(function () {//validaacion
                    if ($(this).val() == '') {
                        datosIncompletos = true;
                    }
                });

                if (datosIncompletos) {
                    popUpAlert("Datos incompletos", "warning")
                    return false;
                }


                misObjetosDensidadDePlantulasPorSurco = $('#tblDensidad tbody tr.modified[miAtributo="LosTRs"]').map(function () {
                    return {
                        IdInvernadero: $(this).find('.idInvernaderos').val()
                    }
                }).get();



                Invernaderos = misObjetosDensidadDePlantulasPorSurco.uniqueObjectsDensidad();

                for (var a in Invernaderos) {
                    DensidadFinal.push({
                        IdInvernadero: Invernaderos[a].IdInvernadero,
                        densidadSurcoDetalle: $('#tblDensidad tbody tr.modified[miAtributo="LosTRs"][idinvernadero="' + Invernaderos[a].IdInvernadero + '"]').map(function () {
                            return {

                                NumeroDeSurco: $(this).find('.NumeroDeSurcos').val(),
                                Densidad: $(this).find('.Densidad').val()

                            }
                        }).get()
                    });
                }

                PageMethods.AlmacenarDensidadDePlantulaPorSurco(DensidadFinal, function (response) {
                    if (response[0] == "1") {
                        popUpAlert(response[1], response[2]);
                        $('tr.modified').removeClass('modified');
                    }
                    else {
                        popUpAlert(response[1], response[2]);
                    }
                });
            });

        });
       
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
       <h1><asp:Label ID="lblTitulo" runat="server">Densidad de Plántula</asp:Label></h1>
       <br />
       <h2><asp:Label ID="lblTitulo2" runat="server">Densidad para los surcos Visibles:</asp:Label>&nbsp
           <input type="text" id="densidadParaLosSurcosVisibles" class="densidadSurcosVisibles" />
           <input id="btnDefinirDensidadTodos" type="button" value="Todos" />
           <input id="btnDefinirDensidadVisibles" type="button" value="Visibles" />
       </h2>
       <br />
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
    <table class="gridView" id="tblDensidad">
        <thead>
            <tr>
                <th rowspan="1">Invernadero</th>
                <th rowspan="1">Surco</th>
                <th colspan="1">Variedad</th>
                <th colspan="1">Densidad</th>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
    <input  id="btnGuardarDensidad" type="button"  value="Guardar"/> 
    </div>
</asp:Content>