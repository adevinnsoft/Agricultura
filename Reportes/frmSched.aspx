<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmSched.aspx.cs" Inherits="report_frmSched" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagPrefix="uc1" TagName="popUpMessageControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link href="../comun/css/jquery-ui-1.8.21.custom.css" rel="stylesheet" type="text/css" />
    <link href="App_LocalStyles/frmSched.aspx.css" rel="stylesheet" type="text/css" />
<%--<link rel="stylesheet" href="../css/ui-lightness/jquery-ui-1.8.21.custom.css" />--%>
<%--<link rel="stylesheet" type="text/css" href="../App_LocalStyles/frmSched.aspx.css" />--%>
<script type="text/javascript" src="../comun/scripts/jquery-1.7.2.js"></script>
<script type="text/javascript" src="../comun/scripts/jquery-ui-1.8.21.custom.min.js"></script>
<%--<script type="text/javascript" src="../scripts/jquery-ui-1.8.21.custom.min.js"></script>--%>
<script type="text/javascript" src="../comun/scripts/jquery-ui.js"></script>
<script type="text/javascript" src="../comun/scripts/jsPopUp.js"></script>
<link rel="stylesheet" type="text/css" href="App_LocalStyles/frmSched.aspx.css" />
<script type="text/javascript">
    function pageLoad() {
        function descargarTablaAXLS() {
            
        }
    }
</script>
  <script type="text/javascript">
      var MODE = "default";
      var clientX = 0;
      var clientY = 0;
      var factorDeAnimacion = 1;
      var accionActual = null;              /* accionActual : { null : sinAccion, 1: Movimiento2: Marca}*/
      var obSeleccionado = null;
      var obAMover = null;
      var response = ['ok', 'Okrrrrrr'];
      var cortesARemover = 3;


      function MostrarLibras() {
          if ($('#chkLibras').prop('checked')) {
              $('.corte').each(function () {
                  $(this).text($(this).attr('librasBrutas'))
              });
          } else {
              $('.corte').each(function () {
                  $(this).text($(this).attr('codigo'))
              });
          }
      }

      function cerrarPopUp() {
          $('#popUpActions').hide(250 * factorDeAnimacion);
      }
    

    
      $(function () {
          $('.tablaReporte td').click(function (e) {
              clientX = e.pageX;
              clientY = e.pageY;
              abrirDialogoDeOpciones($(this));
          });
          $('input[type=text]').keypress(function (e) {
              if (e.keyCode == 13)
                  return false;
          });
          if ($('.datepicker').length > 0)
              $('.datepicker').datepicker('destroy').datepicker({ 'dateFormat': 'dd/mm/yy' }).on('click', function () { $(this).attr('readonly', 'readonly') });

          $('.smallBox').click(function () {
              return false;
          });
      });
  </script>
  <script type="text/javascript">
      function descargarHojaDeCalculo(boton) {
          $('.report table').each(function () {
              var clone = $(this).clone();
              clone.find('.invisible').remove();
              clone.find('td').css({ 'border': '1px solid black' });

              window.open('data:application/vnd.ms-excel,' + encodeURIComponent('<style>' + $('#EstilosParaXLS').html() + '</style><table style="border:1 px solid black;">' + clone.html() + '</table>'));
          });
      }
  </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container" style="width: 100%">
    <h1>Schedule</h1>
    <table class="index">
        <tr>
            <td class="reporte">
                <table>
                    <tr>
                        <td colspan="11"><h3>Ingrese los datos que se piden:</h3></td>
                    </tr>
                    <tr id="trEjecucion" runat="server">
                        <td colspan="11">
                            <table>
                                <tr>
                                    <td>* Historico :</td>
                                    <td><asp:DropDownList ID="ddlUnion" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                    <td>Planta :</td>
                                    <td><asp:DropDownList ID="ddlPlanta" runat="server" AppendDataBoundItems="True" AutoPostBack="True" onselectedindexchanged="ddlPlanta_SelectedIndexChanged"  ></asp:DropDownList></td>
                                    <td><asp:Label runat="server" ID="lblAnioAMostrar">A&ntilde;o a mostrar</asp:Label></td>
                                    <td><asp:DropDownList runat="server" ID="ddlYear"></asp:DropDownList></td>
                                    <td><asp:Button ID="btnGetReporte" runat="server" Text="Obtener Reporte" OnClick="btnGet_Click"  CssClass="reporte" /></td>
                                    <td><asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" onclick="btnLimpiar_Click"  CssClass="reporte" /></td>
                                   <%-- <td colspan="4">
                                        <img alt="" src="../comun/img/download_xls.png" onclick="descargarHojaDeCalculo($(this));" />
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~" Height="35px" Width="30px"  CssClass="reporte" /></td>--%>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label runat="server" Text="Mostar Libras"></asp:Label></td>
                                    <td><input type="checkbox" id="chkLibras" onchange="MostrarLibras()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
               <div id="report" runat="server" class="report"></div> 
            </td>
        </tr>
    </table>
</div>
    <uc1:popUpMessageControl runat="server" id="popUpMessageControl1" />
<div id="EstilosParaXLS" class="invisible">
.primerCorte,.corte { background: #000080; color: #fff; }
.precosecha { background: #87CEFA;}
.plantacion { background-color: yellow;} 
.suelo { background: #B4B4B4; color: #fff; text-align:center; } 
.falla{background:Black;color:White;}
.greenCell{color: white; background: #238D23; border: black; text-align:center;}
.ni { color: green; background-color: gray; text-align:center;}
.ni2 { color: Blue; background: gray; text-align:center; }
.title1 { background: white; color: Black; text-align:center;}
.blueColor{ color:Blue !important;}
.redColor{ color:Red !important;}
.greenColor{ color:Green !important;}
.greenCellBack{ background:#D4FFD4;}
.bigBorder{ border-top: 3px solid black !important; border-bottom:3px solid black !important;}
.separador{ background:gray; color:White; border-top: 3px solid black !important; border-bottom:3px solid black !important;}
.blackCell{ background:black; color:white;}
.grayCell{ background:#CDCDCD/*808080*/; color:black;}
.cambio{ background:#000; color:#FFF;}
.suelo.cicloEnMovimiento{background:#8A8A8A; cursor: all-scroll;}
.plantacion.cicloEnMovimiento{background:#A7A7A7; cursor: all-scroll;}
.precosecha.cicloEnMovimiento{background:#D2D2D2; cursor: all-scroll;}
.corte.cicloEnMovimiento{background:#4A4A4A; cursor: all-scroll;}
.smallBox{width:20px;}
.bloqueado{ background: #CDCDCD;  color:Gray; cursor: no-drop !important;}
.suelo.bloqueado{background:#E7E2E2; }
.plantacion.bloqueado{background:#FFFFAA; }
.precosecha.bloqueado{background:#B2DCF5; }
.corte.bloqueado{background:#464693;}
.cambio.bloqueado{background:#525252;}
td{ border: 1px solid black;}
</div>
</asp:Content>

