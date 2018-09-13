<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VistaAerea.aspx.cs" Inherits="Jornales_Vista_Aerea" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
<script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
<script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
<style type="text/css">
.Invernadero {
    /*background: blue;*/
    /*border: 6px solid #F6A239;*/
    display: inline-block;
    /*padding: 10px;*/
    padding-top: 20px;
    /*padding-bottom: 0px;*/
    color: #000;
    min-width: 30px;
    min-height: 30px;
    width:62px;
    height:39px;
    text-align: center;
    margin: 10px;
    cursor: pointer;
    font-size:18px;
    font-weight: bold;
    border-radius: 10px;
    border: solid 1px #000;
    background-image: -webkit-gradient(linear, left top, left bottom, color-stop(0, #fefefe), color-stop(100, #CDCDCD));
    
}

.InvernaderoInfestado {
    /*background: blue;*/
    /*border: 6px solid #F6A239;*/
    display: inline-block;
    /*padding: 10px;*/
    padding-top: 20px;
    /*padding-bottom: 0px;*/
    color: #000;
    min-width: 30px;
    min-height: 30px;
    width:62px;
    height:39px;
    text-align: center;
    margin: 10px;
    margin-bottom: 10px;
    cursor: pointer;
    font-size:18px;
    font-weight: bold;
    border-radius: 10px;
    border: solid 1px #000;
    background-image: url(../comun/img/mosca.jpg);
    background-position: center;
}

label#lblLider, label#lblProducto, label#lblNumeroDeSecciones, label#lblNumeroDensidad, label#lblNumeroDeSurcos, label#lblSemanasCicloDeVida,label#lblVariableDeTecnologia,label#lblPromedioDensidad,label#lblEstadoInfestacion,label#lblNombreInfestacion,label#NivelDeInfestacion {
	font-weight: bold
}


div#divInvernadero {
    max-width: 800px;
}

input#btnVerDetalle.ActividadesProgramadasSI 
{
    position:relative;
    bottom:-60px;
    cursor: pointer;
    }
    
input#btnVerDetalle.ActividadesProgramadasNO 
{
    cursor:pointer;
    }
    
.divEstadoInfestacion 
{
    width:140px;
    position:absolute;
    left: 475px;
    top:61px;
    }
    
table#tblDatosInfestacion
{
    width: 215px;
    position:absolute;
    right:53px;
    top:90px;
    }

.InformacionGeneraldeInvernadero {
    background: #f0f5e5;
    border: 2px solid #adc995;
    padding: 5px;
    padding-top:10px;
    padding-bottom: 10px;
    padding-left: 20px;
    margin-left: 10px;
    overflow:hidden;
    border-radius: 10px;
    width:730px !important;
    position:relative;
}

.InformacionGeneraldeInvernadero span {
    display: block;
    padding: 4px;
}

div.tooltipster-content span 
{
    display:block;
    }
h2.InformacionGeneralTitulo {
    font-size: 16px;
    color: #000;
    margin-left: 0;
    margin-bottom: 5px;
    margin-top: 5px;}
    
h2.ActividadesProgramadasTitulo {
    font-size: 16px;
    color: #000;
    margin-left: 0;
    margin-bottom: 5px;
    margin-top: 15px;}
    
strong{
    margin-left:10px;
    }

.Actividad {
    display: inline-table;
    background: rgba(0,0,0,0);
    margin: 1px;
    padding-bottom: 0px;
}

.btnActividad {
    width: 60px;
    height: 60px;
    margin: 7px;
    border: 6px solid gray;
    font-size: 26px;
    border-radius: 5px;
}

.btnActividad span {
    font-size: 10px;
    display: block;
}

.Actividad span {
    display: block;
    margin-left: 7px;
}

span.actividadCodigo {
    font-size: 22px;
    margin: -3px;
    width: 100%;
    text-align: center;
}

span.actividadNombre {
    margin: -4px;
    width: 100%;
    text-align: center;
    text-transform: capitalize;
    font-size: 8px;
    padding-top: 1px;
}

span.actividadEtapa {
    margin: -3px;
    width: 100%;
    text-align: center;
    text-transform: capitalize;
    font-size: 11px;
    text-shadow: rgba(224, 224, 224,.5) 1px 1px 0px;
    margin-top: 0px;
}
#popUpSurcos{
    position: absolute;
    width: 845px;
    height: 77%;
    background: white;
    z-index: 9999;
    left: 0;
    top: 0;
    bottom:0;
    right:0;
    overflow-x: hidden;
    overflow-y: auto;
    border: 1px solid #cccccc;
    max-width: 845px;
    -moz-box-shadow: 0 0 9px #999999;
    -webkit-box-shadow: 0 0 9px #999999;
    box-shadow: 0 0 9px #999999;
    display: none;
    margin:auto;
}

div.popUpHeader{
    background: #000080;
    height: 42px;
    width: 100%;
    color:yellow;
}

div#divSurcos {
    width: 100%;
    height: 80%;
    overflow: auto;
}

div.popUpBotones{
    width: 100%;
    height: 40px;
    bottom: 0;
    position: relative;
    background: #F4D101;
}

        
.mapaPlan
{
    padding: 5px;
}
        
.accordionHeader
{
    display: inline-block;
    text-align: left;
    background-color: #ADC995;
    color: white;
    font-size: 18px;
    border: 1px solid #FF6600;
    width: 100%;
    padding-top: 5px;
    padding-bottom: 5px;
    font-size: 15px;
    padding-left: 6px;
}
        
/*.accordionHeader:hover
{
    cursor: pointer;
    background-color: #FAE258;
    color: #FF6600;
}*/
        
.accordionBody
{
    border: 1px solid #ADC995; /*padding: 5px;*/
    width: 100%;
    background: #FFF;
    display: table;
}
        
table.index
{
    max-width: 100% !important;
    width: 100% !important;
}
        
h2
{
    width: 400px !important;
}
        
/*Mapa*/
.seccion
{
    height: 250px;
    width: 100px;
}
        
.seccion tr
{
    border: 1px solid !important;
}
        
.surco
{
    width: 20px;
    position: relative;
}
        
.map
{
    /*width: auto !important;*/ /*border-left: 1px solid #ADC995;*/
    border-right: 1px solid #ADC995; /*margin-left: 10px;*/
    padding: 5px; /*background-color:white;*/
    float: left !important;
}
        
.mapa
{
    margin: 5px;
}

        
.mapHead td
{
    border: 1px solid !important;
    background: white;
    text-align: center !important;
}
        
.textVertical
{
    -webkit-writing-mode: vertical-lr;
    width: 80%;
    height: 100%;
    -ms-transform: rotate(180deg);
    -moz-transform: rotate(180deg);
    -o-transform: rotate(180deg);
    transform: rotate(180deg);
    text-align: center !important;
    color: #ffffff;
    z-index: 999;
    position: relative;
    text-shadow: 0px 2px 3px #555;
}  
.gridView
{
    min-width: 200px;
    margin-left:auto;
    margin-right:auto;
    margin-bottom:30px;
    -moz-border-radius:7px;
    -webkit-border-radius: 7px;
    border-radius: 7px;
} 
</style>


<script type="text/javascript">

    function verDetalleInvernadero(objInvernadero) {
        $(objInvernadero).hide();
        $(objInvernadero).next().show().animate({ 'width': '681px', 'height':'100%' }, 1000);
    }
    function ocultarDetalleInvernadero(objInvernadero) {
        $(objInvernadero).prev().show(1000);
        $(objInvernadero).animate({ 'width': '30px', 'height': '30px' }, 800).hide(30);
    }

    function abrirpopUpSurcos(objButton,objIdInvernadero,objNombreInvernadero){//idInvernadero {
        $(objButton).click(function () {
            PageMethods.cargaMapaPlan(objIdInvernadero, function (response) {
                //if (response[0] == '1') {
                if (response[0] == '1') {
                    $('#divMapaPlan').empty();
                    $('#divMapaPlan').append(formularioColmena(objIdInvernadero, objNombreInvernadero));
                    $('#divMapaPlan .mapaPlan[idInvernadero="' + objIdInvernadero + '"] div[class="mapa"]').html(response[2]);
                    setTooltips();
                    $('#popUpSurcos').show();
                }
                else {
                    popUpAlert(response[1], response[2]);
                }


            });
        });
    }

    $(function () {
        //        var idPlanta = 0;
        //        var nombreDePlanta = '';
        //        var idInvernadero = 0;
        //        idPlanta = $('.Plant select ').val();
        //        nombreDePlanta = $('.Plant select option:selected ').text();

        obtInvernaderosIniciales();
        seleccionarPlanta();


    });


    function obtInvernaderosIniciales() {
        var plantaActual = $('[id*="ddlPlanta"] option:selected').text();
        obtInvernaderosPlanta(plantaActual);
    }


    function obtInvernaderosPlanta(nombrePlanta) {
        PageMethods.obtenerInvernaderosPorPlanta(nombrePlanta, function (response) {
            if (response[0] == '1') {
                $('#lblTitulo').text('Invernaderos de la planta ' + nombrePlanta + '');
                $('#divInvernadero').html(response[2]);

            }
            else {
                popUpAlert(response[1], response[2]);
            }
        });
    }

    function seleccionarPlanta() {
        $('[id*=ddlPlanta]').change(function () {
            var nombrePlanta = $(this).text();
            obtInvernaderosPlanta(nombrePlanta);
        });
    }

    function formularioColmena(idInv, nombreInvernadero) {
        return '<div class="mapaPlan" idInvernadero="' + idInv + '" nombreInvernadero="'+nombreInvernadero+'">' +
        '       <div class="accordionHeader" ' + //onclick="acordeon($(this));">
        '           <label>Invernadero'+" "+nombreInvernadero+'</label>' +
        '       </div>' +
        '       <div class="accordionBody">' +
        '               <div class="mapa">' +
        '               </div>' +
        '       </div>' +
        '</div>';
    }
    function setTooltips() {
        //$('.tooltip').tooltipster('destroy');
        $('.tooltip').tooltipster({
            animation: 'grow',
            delay: 200,
            theme: 'tooltipster-punk',
            touchDevices: true,
            trigger: 'hover',
            contentAsHTML: true,
            interactive: true
        });
    }
</script> 
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container" id="divGeneral">
       <h1><label id="lblTitulo">Invernaderos</label></h1>
       <div id="divInvernadero"></div>


       
    </div>
    <div id="popUpSurcos">
        <div class="popUpHeader moveHandle">
           <img src="../comun/img/remove-icon.png" alt="X" onclick="$('#popUpSurcos').hide();" style="float: right; margin: 10px; cursor: pointer;" />
            <div id="divMapaPlan">
            </div>
            <div class="popUpBotones moveHandle">
    <%--            <input type="button" value="Aplicar" onclick="" />
                <input type="button" value="Cancelar" onclick="$('#popUpSurcos').hide();" />--%>
            </div>
        </div>
    </div>
</asp:Content>