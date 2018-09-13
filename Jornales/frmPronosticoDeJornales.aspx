<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="frmPronosticoDeJornales.aspx.cs" 
    Inherits="Jornales_frmPronosticoDeJornales" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" 
    MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
   <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
   <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
   <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
   <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
   <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
   <script type="text/javascript" src="../comun/scripts/PronosticoDeJornales.js"></script>
   <script type="text/javascript" src="../comun/scripts/highcharts.js"></script>
   
   <style type="text/css">
    input.txtConfiguracion
    {
    width:60px;    
    }
    
    table .tablePronostico tr td 
    {
    height: 20px;
    }
    
#tblAvanzado tr:nth-child(odd)
    { background-color: #d6dfd0;
        border-spacing:0;
    }
    #tblAvanzado tr:nth-child(even)
    { background-color: #fff;
        border-spacing:0;
        }
  
        table.semanas {
	    border-spacing: 0;
    }

    table.tableLideres.tablePronostico {
	    width: 320px;
	    margin-top: -16px;
    }

    tr.superHeader td {
	    background: #adc995;
        text-align: center !important;
        text-transform: uppercase;
        font-weight:bold;
        color: White;
        border-style: none;
    }

    table .tablePronostico tr td {
	    padding: 3px !important;
	    text-align: center !important;
    }

    tr.header td 
    {
        font-weight:bold;
        color: #F60;
        text-transform: uppercase;
        }
  
    #divPronosticoAvansado > table {
	    border-spacing: 0 !important!;
    } 

    table#tblAvanzado 
    {
        border-spacing: 0;
        }
    
    div#divSemanas input[type="text"]
    {
        text-align:center;
        }
            
            
    /* TABLA CONFIGURACIONES*/
        
        
    #tblConfiguraciones tr:nth-child(odd) {
	background-color: #fff;}

    #tblConfiguraciones tr:nth-child(even) {
	background-color: #d6dfd0;}
	
	table#tblConfiguraciones 
	{
	    min-width:650px;
	}
	 table#tblConfiguraciones td 
	 {
	     height: 25px;
	     text-align: center;
	     text-transform: uppercase;
	     font-weight: bold;
	     color: #F60;
	     
	     }   
	#tblConfiguraciones input[type="text"]
	{
	    min-width: 80px;
	    text-align: center;
	    margin-left: 10px;
	    margin-right:10px;
	 }
	    
	input#txtNumSemanas, input#txtNumActividades 
	{
	    /*margin-left: 10px;
	    margin-right: 10px;*/
	    text-align: center;
	    }

	td.noOcultable {
        font-weight: bold !important;
    }
    td.total 
    {
      /*  color:Red;*/
    }
    
    tr.header {
	white-space: nowrap;
    }

    .subHeader {
	    background: #fff !important;
    }

    td.ocultable {
	    white-space: nowrap;
	    color: #358129;
	    font-weight: bold;
    }

    td.noOcultable.total {
	    background: #fff;
    }

    .tableLideres thead tr.header 
    {
        height: 54px;
    }
    .clickable 
    {
             cursor: pointer;
    }
       
    .scrollable table {
        width: 100%;
        text-align: center;
        background: white;
    }

    .scrollable table tr:nth-child(odd) {
        background: #d6dfd0;
    }

    .scrollable table tr td {
        padding: 3px 0;
    }

    table.index input[type="text"]
    {
        text-align: center;
    }
    
    div#divOpcionesDinamicas {
        width: 650px;
        overflow-x: auto;
        overflow-y: hidden;
        height: 152px;
    }

    td.curva {
        padding-bottom: 14px !important;
    }

    td.hora {
        padding-top: 14px !important;
    }

    table.tableSemana .expandir
    {
        background-image: url("../comun/img/icono_mas.png");
        background-position: left center;
        background-repeat: no-repeat;
        background-size: 9px 10px;
    }
    
        table.tableSemana .expandir span {
        margin-left: 10px;
    }

    table.tableSemana .colapsar
    {
        background-image: url("../comun/img/icono_menos.png");
        background-position: left center;
        background-repeat: no-repeat;
        background-size: 9px 10px;
    }
    
        table.tableSemana .colapsar span {
        margin-left: 10px;
    }
    
    
    
    /* JAVIER SALAS */
    

    table#tDesgloseActividades tr:nth-child(odd) {
        background: #d6dfd0;
    }

    table#tDesgloseDescripcion tr:nth-child(odd) {
        background: #d6dfd0;
    }
  
    input[type="number"]
    {
        text-align: center !important;
    }
        
    img.filtrarLideres {
        cursor: pointer;
        position: relative;
        top: 3px;
        max-width: 16px;
        margin-right: 3px;
    }
    
    .jsPopUp.modalPopup
    {
        margin: 0 !important;
        transform: translateX(-50%) translateY(-50%);
        overflow-x: hidden;
        }
            
    .porcentajeJornales {
    width: 90%;
    box-sizing: border-box;
    padding: 20px 10px 10px;
    background: #f1f1f1;
    text-align: center;
    }
    
    .chartsJornales {
    width: 90%;
    height: 90%;
    box-sizing: border-box;
    padding: 20px 10px 10px;
    background: #f1f1f1;
    text-align: center;
    }
    
    div#contenedorDesglose {
    width: 100%;
    display: flex;
    box-sizing: border-box;
    margin-top: 20px;
    overflow-x: hidden;
    overflow-y: auto;
}

div#contenedorDesglose table {
    background: white;
    text-align: center;
    white-space: nowrap;
}

div#divDesgloseDescripcion {
    width: 20%;
}

table#tDesgloseDescripcion {
    width: 100%;
}

div#divDesgloseActividades {
    width: 80% !important;
    overflow-x: auto;
    overflow-y: hidden;
    margin-left: -2px;
}

tr.alturaHeader {
    height: 42px !important;
}

@media screen and (max-width: 1366px){
    div#divDesgloseDescripcion {
      width: 30%;
    }
    div#divDesgloseActividades {
        width: 70% !important;
    }
}

@media screen and (max-width: 1024px){
    div#divDesgloseDescripcion {
      width: 35%;
    }
    div#divDesgloseActividades {
        width: 65% !important;
    }
}
    
</style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle">Pronósticos de Jornales</asp:Label>
        </h1>
        <table class="index">
            
            <tr>
                <td colspan="5">
                    <h2>
                        <asp:Literal ID="ltSubPronostico" runat="server" meta:resourceKey="ltSubPronostico" />
                        <span id="subPronostico"></span>
                    </h2>
                </td>
            </tr>
            <tr>
                <td>Semanas:</td>
                <td><input type="number" class="txtConfiguracion preConfiguracion intValidate" id="txtNumSemanas"/></td>
                <td>Año Partida:</td>
                <%--<td><input type="number" class="txtConfiguracion preConfiguracion intValidate required" id="txtAnioPartida"/></td>--%>
                
                <td><select class="txtConfiguracion preConfiguracion intValidate" id="txtAnioPartida"></select></td>
                <td width="260px">&nbsp;</td>
            </tr>
            <tr>
                <td>Eficiencia Histórica:</td>
                <td><input type="number" class="txtConfiguracion preConfiguracion intValidate" id="txtNumHistoricos" value="5"/></td>
                <td>Semana Partida:</td>
                <%--<td><input type="number" class="txtConfiguracion preConfiguracion intValidate required" id="txtSemanaPartida"/></td>--%>
                <td><select class="txtConfiguracion preConfiguracion intValidate required" id="txtSemanaPartida"></select></td>
                <td width="260px">&nbsp;</td>
            </tr>
            <%--<tr>
                <td colspan="5"><input type="button" id="btnCargaConfiguracion" value="Carga Configuración"/>
                </td>
            </tr>--%>
            <tr class="trConfiguraciones">
                <td></td>
                <td rowspan="5" colspan="4"><div id="divOpcionesDinamicas"></div></td>
            </tr>
            <tr class="trConfiguraciones">
                <td class="hora"><asp:Literal ID="ltHota" runat="server" meta:resourceKey="ltHora" >Hora:</asp:Literal></td>
            </tr>
            <tr class="trConfiguraciones">
                <td><asp:Literal ID="ltAusentismo" runat="server" meta:resourceKey="ltAusentismo" >Ausentismo(%):</asp:Literal></td>
            </tr>
            <tr class="trConfiguraciones">
                <td><asp:Literal ID="Literal1" runat="server" meta:resourceKey="ltAusentismo" >Capacitacitación(%):</asp:Literal></td>
            </tr>
            <tr class="trConfiguraciones">
                <td class="curva"><asp:Literal ID="Literal3" runat="server" meta:resourceKey="ltAusentismo" >Curva(%):</asp:Literal></td>
            </tr>
            <tr class="trConfiguraciones">
                <td>&nbsp;</td>
            </tr>
            <tr class="trConfiguraciones">
                <td colspan="5"><input type="Button" id="btnCalcular" value="Calcular"/></td>
            </tr>
        </table>
        <table class="index" id="tblPronostico">
            <tr>
                <td ><h2><asp:Literal ID="Literal2" runat="server" meta:resourceKey="ltSubResultado" /></h2></td>
            </tr>
            <tr>
                <td ><div id="divPronostico"></div></td>
            </tr>
            <tr>
                <td >
                    <div id="divPronosticoAvansado" style="display:block;" >
                        <table style="width:100%; " id="tblAvanzado" >
                            <tr>
                                <td width="300px"><div id="divLideres" style="display:block; width:auto;"></div></td>
                                <td><div id="divSemanas" style="display:inline-block; width:600px; min-width:600px; max-width:600px; overflow-x:scroll; overflow-y:none;"></div></td>

                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td><input type="button" id="btnGuardar" value="Guardar"/><input type="button" id="btnGraficar" value="Ver Gráfica"/></td>
            </tr>
        </table>
        <div id="eficiencias" style="display:none"></div>
        <%--<div id="divListaLider"  style="display:none">
            <table>
                <thead><tr><td colspan="2"><span>Lideres Visibles</span></td></tr></thead>
                <tfoot><tr>
                    <td><input type="checkbox" id="ckAllItems" checked /></td>
                    <td>Ver todos</td>
                </tr></tfoot>
                <tbody></tbody>
            </table>
        </div>--%>
       <%-- <div id="divListaDesgloseJornales">
            <table id="tDesgloseJornales">
                <thead>
                    <tr><td colspan="2">desglose de Actividades de semana NUMERO_SEMANA para NOMBRE_LIDER</td></tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <div id="divDesgloseDescripcion">
                                <table id="tDesgloseDescripcion">
                                    <thead>
                                        <tr><td><span>FAMILIA</span></td></tr>
                                        <tr><td><span>INVERNADERO</span></td><td><span>VARIEDAD</span></td><td><span>EDAD</span></td><td><span>DENSIDAD</span></td><td><span>CAJAS</span></td></tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div id="divDesgloseActividades">
                                <table id="tDesgloseActividades">
                                    <thead></thead>
                                    <tbody></tbody>
                                </table>    
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>--%>
  </div>

    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    <uc1:popUpMessageControl ID="popUpMessageControl2" runat="server" />
   
</asp:Content>
