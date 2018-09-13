<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmReporteVistaAerea.aspx.cs" Inherits="Reportes_frmReporteVistaArea" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>

    <style type="text/css">
    .custom-combobox
    {
       border-radius: 86px 16px 16px 16px;
        -moz-border-radius: 86px 16px 16px 16px;
        -webkit-border-radius: 86px 16px 16px 16px;
        border: 5px solid #000000;
    }
  
 </style>  
    <style type="text/css">
        .onoffswitch
        {
            position: relative;
            width: 80px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            left: 35%;
        }
        .onoffswitch-checkbox
        {
            display: none;
        }
        .onoffswitch-label
        {
            display: block;
            overflow: hidden;
            cursor: pointer;
            border: 2px solid #F2F2F2;
            border-radius: 30px;
            margin: 0px !important;
        }
        .onoffswitch-inner
        {
            display: block;
            width: 200%;
            margin-left: -100%;
            -moz-transition: margin 0.3s ease-in 0s;
            -webkit-transition: margin 0.3s ease-in 0s;
            -o-transition: margin 0.3s ease-in 0s;
            transition: margin 0.3s ease-in 0s;
        }
        .onoffswitch-inner:before, .onoffswitch-inner:after
        {
            display: block;
            float: left;
            width: 50%;
            height: 23px;
            padding: 0;
            line-height: 25px;
            font-size: 12px;
            color: white;
            font-family: Trebuchet, Arial, sans-serif;
            font-weight: bold;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }
        .onoffswitch-inner:before
        {
            content: "Inver";
            padding-left: 26px;
            background-color: #44A12D;
            color: #FFFFFF;
        }
        .onoffswitch-inner:after
        {
            content: "Surco";
            padding-right: 8px;
            background-color: #FF5100;
            color: #FFFFFF;
            text-align: right;
        }
        .onoffswitch-switch
        {
            display: block;
            width: 9px;
            margin: 8px;
            background: #FFC400;
            border: 1px solid #F2F2F2;
            border-radius: 50px;
            position: absolute;
            top: 0;
            bottom: 0;
            right: 50px;
            -moz-transition: all 0.3s ease-in 0s;
            -webkit-transition: all 0.3s ease-in 0s;
            -o-transition: all 0.3s ease-in 0s;
            transition: all 0.3s ease-in 0s;
        }
        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner
        {
            margin-left: 0;
        }
        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch
        {
            right: 0px;
        }
    </style>
   
   <style type="text/css" id="popUp">
        div.popUpWMP
        {
            position: absolute;
            width: 65%;
            height: 65%;
            background: white;
            z-index: 9999;
            overflow: hidden;
            border: 1px solid #cccccc;
            -moz-box-shadow: 0 0 9px #999999;
            -webkit-box-shadow: 0 0 9px #999999;
            box-shadow: 0 0 9px #999999;
            display: none;
        }
        div.popUpHeader
        {
            background: #000080;
            height: 42px;
            width: 100%;
            color:yellow;
        }
        div.popUpContenido
        {
            padding: 5px;
            max-height: 80%;
            overflow: auto;
            width: 98%;
        }
        div.popUpBotones
        {
            width: 100%;
            height: 40px;
            bottom: 0px;
            position: absolute;
            background: #F4D101;
        }
        .accordionHeader
        {
            width: 100%;
            background: #ADC995;
            padding-top: 5px;
            padding-bottom: 5px;
            margin-top: 3px;
        }
        div.etapa
        {
            text-align: center;
            font-size: 12px;
            margin-top: 0px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
            width: 800px;
            max-width: 800px;
            min-width: 800px;
           
        }
        .configuracionAdicionalDeEtapa { border-left:1px solid #ccc; border-right:1px solid #ccc; border-bottom:1px solid #ccc; height:auto;}
        .etapaGenerales
        {
            width: 100%;
            background: #ADC995;
            border:none;
            border-collapse:collapse;
        }
        #divEtapas input
        {
            max-width: 100px;
            margin:3px 2px;
        }
        #divEtapas select  
        {
            max-width: 100px;
            margin:3px 2px;}
        .configuacionPorProducto div
        {
            display: table-cell;
            border: 1px dashed #D2CFCF;
            width: 50%;
        }
        .porcenajesDeIncremento
        {
            display: inline-block;
        }
        .configuacionPorProducto div.target {padding:0; margin:5px;}
        .configuacionPorProducto div.materiales {padding:0; margin:5px;}
        .configuacionPorProducto div.materiales table.index5 {margin:5px;width:97%;}
        .configuacionPorProducto div.targetPorProducto {padding:0; background:#f1f1f1; display:block;}
        .configuacionPorProducto div.targetPorProducto h5 {margin:0px; padding:0px 5px; width:343px; font-size:11px; text-align:right; float:left;}
        .configuacionPorProducto div.targetPorProducto h3 {margin:5px;}
        .configuacionPorProducto div.materiales h3 {margin:5px;}
        .configuacionPorProducto div
        {
            border: 1px dashed #fff;
            width: 388px;
            display:table-cell;
            text-align: left;
            min-height: 84px;
        }
        .index5
        {
            text-align: center;
            border: 1px solid #adc995;
            background: #f0f5e5;
            font-size: 12px;
            margin-top: 0px;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
            width: 100%;
        }
        .invisible
        {
            display: none;
        }
        .porcentaje input
        {
            width:30px;
            }
            span.porcentaje
        {
            display: inline-block;
            width:72px;
        }
        span.porcentaje label {display:inline-block; margin:10px 3px; width:20px; text-align:right;}
    
   
   
   
   
   
   
   
   
   table.dd {
	text-align: center;
	border: 1px solid #adc995;
	background: #f0f5e5;
	font-size: 12px;
	margin: 0 auto;
	-moz-border-radius:10px;
	-webkit-border-radius: 10px;
	border-radius: 10px;
	margin-left: auto;
	margin-right: auto;
	width: auto;
	max-width: 450px;
	min-width: 450px;
	width: 450px;
	white-space: nowrap;
}
table.dd tr td span.nobrd input {
	border: none;
	width: 20px;
	
}
.dd
   {border-color inherit !important;}
   
   
   
   
   
</style>
    <script type="text/javascript">
        $(function () {
            $('#chkGeneral').click(function () {
                if ($('#chkGeneral').is(':checked')) {
                    $('#DivInvernadero').show();
                    $('#surcos').hide();
                }
                else {
                    $('#DivInvernadero').hide();
                    $('#surcos').show();
                }
            });
            //setInterval(function () { llenartablaFormaA(); }, 10000);
            //registerControls();
            //alert("si salió");
            //llenatablaFormaA
//            fecha = moment('2015-11-18 17:31:13.000');
//            console.log(fecha.format('YYYY-MM-DD'));
//            var fecha2 = moment('2015-11-18 17:31:13.000');
//            console.log(fecha2.format('HH:mm:ss'));
            llenarNivelInvernaderos();
            //alert("si salió");
        });
        var fecha;
        function llenarNivelInvernaderos() {
            PageMethods.llenatablaInvernadero(function (response) {
                var a = 10;
                $('#idInvernadero').html(response); //.slideToggle();
            });
        }
        function popUpMostrar(btn) {
            btnMaterialesPresionado = btn;
            $('#popUpHerrmaientasYMateriales').css({
                top: '50%',
                left: '50%',
                'margin-left': ($('#popUpHerrmaientasYMateriales').width() * -0.5) + 'px',
                'margin-top': ($('#popUpHerrmaientasYMateriales').height() * -0.5 + $(window).scrollTop()) + 'px'
            }).show();
            PageMethods.ObtenerContenidoInvernadero(function (response) {
                $('#surcos').html(response);
            });
        }
       </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
                            <div class="container">
                                <h1>    
                                    <asp:Label ID="lblTitle" runat="server" Text="Invernaderos"></asp:Label>
                                </h1>
                                <div id="idInvernadero"></div>
                            <%--<table id="StatusFormasA" >
                                <thead class="encabezadotabla"><tr><th>Invernadero</th><th>Lider</th><th>Forma</th><th>Hora</th><th>Color</th><th>Estado</th></tr></thead>
                                <tbody></tbody>
                            </table>--%>
                            </div>



                            <div id="popUpHerrmaientasYMateriales" class="popUpWMP" ">
            <div class="popUpHeader">
                <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popUpHerrmaientasYMateriales').hide();" style=" margin-left: -536.5px;   margin-top: 247px;  float: right; margin: 10px; cursor: pointer;" />
            </div>
             <div class='onoffswitch'>
                                        <input type='checkbox' name='onoffswitch' class='onoffswitch-checkbox' id='chkGeneral' />
                                        <label class='onoffswitch-label' for='chkGeneral'>
                                            <span class='onoffswitch-inner'></span><span class='onoffswitch-switch'></span>
                                        </label>
                                    </div>
                                    <div id="DivInvernadero" class="popUpContenido" style="max-height: 80%;overflow: auto; display: none;">
                                        <h3 id='Seccion1' >Nombre del Invernadero</h3> 
                                        <p>Nombre Clave:</p><p>Variedad</p>
                                        <p>Infestación:</p><p># Surcos.</p>
                                        <p>% Avance.</p><p>% Cumplimiento</p>
                                    </div>
            <div id="surcos" class="popUpContenido" style="max-height: 80%;overflow: auto;">
            <!---->
                <%--<h2>Surcos</h2>
                <div id="surcosy" class="gridView">
                    <h3 id='Seccion1' >Sección 1</h3>
                </div>--%>
                <%--<table id="tblMateriales" class=" accordionContent gridView">
                    <thead><tr><th>Categoría</th><th>Material</th><th>Cantidad</th><th>Unidad</th></tr></thead>
                    <tbody></tbody>
                </table>     --%>
                <!---->
            </div>
            <div class="popUpBotones">
                <%--<input type="button" value="Aplicar" onclick="AgregarMateriales(); $('#popUpHerrmaientasYMateriales').hide();"/>--%>
                <input type="button" value="Cancelar" onclick="$('#popUpHerrmaientasYMateriales').hide();" />
            </div>
        </div>



</asp:Content>

