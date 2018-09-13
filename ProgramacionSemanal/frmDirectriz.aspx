<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmDirectriz.aspx.cs" Inherits="ProgramacionSemanal_frmDirectriz" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../comun/scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
     <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
     <script src="../comun/scripts/jquery.blockUI.js" type="text/javascript"></script>
     	<script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <style type="text/css">
        .celdasGrandes td
        {
            padding: 4px !important;
        }
        
        .grid
        {
            min-width: 100%;
        }
        .grid th
        {
            text-align: center;
        }
        .invisible{ display:none;}
        .column
        {
            display:inline-table;    
           
        }
        .detalle
        {
            display:none;
        }
        #divCargadas
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
            padding:10px;
            display:table;
        }
        
         #divCargadas .group 
         {
            border:none;
            padding:5px 1px;
            margin: 0 5px 0 0; 
            cursor:pointer;   
            min-width:115px;
            width:115px;
            display:table-cell;
         }
          #divCargadas h3
         {
            border: 1px solid #adc995;
            padding:5px; 
            cursor:pointer;
            background:#fcf8e3; 
         }
          
          
          #divCargadas h3:hover
          {
              background:#adc995;
              color:White;
              }
         .detalle {background:#fff;border:1px solid #adc995; display:none;}
         
         input.search2
         {
            padding-right: 18px;
            background-image: url(../comun/img/lupa.png);
            background-size: 14px;
            background-repeat: no-repeat;
            background-position: right;
         }
         
         .exportTable
         {
             width:100%; 
             text-align: right; 
             display:none; 
             margin-bottom: 20px;
         }
         
         h3 
         {
             color: #000 ;
             }
         
        .auto-style1 {
            height: 10px;
        }
         
    </style>
    <script type="text/javascript">
        function funcionesDelGrid() {
            $('#gv_Directriz td').addClass('narrowpad');
            $('#gv_Directriz td').click(function () {
                var tdSelected = $(this);
                var tdValue = $(this).html();

                if (tdValue.indexOf('-') > 0)
                    return;

                if (tdValue.indexOf('input') > 0)
                { $(tdSelected).find('input').focus(); }
                else {
                    $(this).html('<input type="text" value="' + tdValue + '" maxlength="10" style="width:20px; text-align:center; padding: 0px 0px; border:none;" class="intValidate" />');
                }

                var inputCreated = $(tdSelected).find('input');
                $(inputCreated).focus();
                $(inputCreated).focusout(function () {
                    $(tdSelected).html($(this).val());
                });
                $(inputCreated).on('keydown', function (e) {

                    switch (e.which) {
                        case 37:
                            var tdPadre = $(this).parent();
                            tdPadre.prev().click();
                            break; //Izquierda
                        case 38: var tdPadre = $(this).parent();
                            if (tdPadre.parent().prev().children().length) {
                                tdPadre.parent().prev().children()[tdPadre.index()].click();
                            }
                            break; //Arriba
                        case 9:
                            var tdPadre = $(this).parent();
                            tdPadre.next().click();
                            break; //Derecha
                        case 39:
                            var tdPadre = $(this).parent();
                            tdPadre.next().click();
                            break; //Derecha
                        case 40:
                            var tdPadre = $(this).parent();
                            if (tdPadre.parent().next().children().length) {
                                tdPadre.parent().next().children()[tdPadre.index()].click();
                            }
                            break; //Abajo
                        default:

                            break;
                    }

                });
            });
        }

        function cargaDeDirectricesHistoricas() {
            PageMethods.GuardadasRecientemente(function (response) {
                $('#divCargadas').html(response);
                $('.detalle').click(function () { $(this).toggle(500); });
                $('.titulo').click(function () {
                    if ($(this).next().html().length > 0) {
                        $(this).next().slideToggle();
                    }
                    else {
                        var idDirectriz = $(this).attr('ID');
                        var divDetalle = $(this).next();
                        PageMethods.ObtenerDetallesDeDirectriz(idDirectriz, function (response) {
                            $(divDetalle).html(response).slideToggle();
                        });
                    }
                });


                function actualizaChecks(contenedorStr, ids) {
                    var contenedor = $(contenedorStr);
                    $(contenedor).find(' tbody tr td > input').prop('checked', false);
                    $(contenedor).find(' tbody tr td').each(
	                   function () {
	                       var id = parseInt($(this).find('label > span').html());
	                       for (var i = 0; i < ids.length; i++) {
	                           if (id == ids[i]) {
	                               $(this).find('input').prop('checked', true);
	                           }
	                       }
	                       console.log(id);
	                   }
	                );
                }

                $('.cargarTablaDirectriz').click(function () {
                    var idDirectriz = $(this).attr('id');

                    PageMethods.DirectrizObtenerTabla(idDirectriz, function (Directriz) {
                        var directriz = $.parseJSON(Directriz);
                        $('.txtNombre').val(directriz["nombre"]);
                        $('.grid').html(directriz["tabla"]);
                        $('#idDirectriz').val(idDirectriz);
                        actualizaChecks('#ctl00_ContentPlaceHolder1_chkl_Variedad', directriz["variedades"])
                        actualizaChecks('#ctl00_ContentPlaceHolder1_chkl_Variable', directriz["variables"])
                        actualizaChecks('#ctl00_ContentPlaceHolder1_chk_Temporales', directriz["temporales"])
                        funcionesDelGrid();
                        $("#exportarTabla").css('display', 'table');
                    });



                });

            });

        }



        $(function () {

            funcionesDelGrid();
            cargaDeDirectricesHistoricas();

            $('#btn_GuardarTabla').click(function () {
                $.blockUI();
                if ($('#gv_Directriz tbody tr').length > 0) {
                    window.console && console.log('gv_Directriz ' + $('#gv_Directriz').length);
                    var row = '';
                    var trCount = $('#gv_Directriz tbody tr').length;
                    var tdCount = $('#gv_Directriz tbody tr').first().find('td').length;
                    var matriz = '';

                    $('#gv_Directriz tbody tr').each(function () {
                        $(this).find('td').each(function () {
                            matriz += $(this).text().trim() + ',';
                        });
                        matriz = matriz.substr(0, matriz.length - 1);
                        matriz += '|';
                    });
                    matriz = matriz.substr(0, matriz.length - 1);
                    var nombreDeDirectriz = $('#<%=txtNombre.ClientID%>').val();
                    var idVariedad = '';
                    var idVariable = '';
                    var idTemporal = '';
                    $('#<%=chkl_Variedad.ClientID%> input:checked').parent().find('.invisible').each(function () {
                        idVariedad += $(this).text() + ',';
                    });
                    $('#<%=chkl_Variable.ClientID%> input:checked').parent().find('.invisible').each(function () {
                        idVariable += $(this).text() + ',';
                    });
                    $('#<%=chk_Temporales.ClientID%> input:checked').parent().find('.invisible').each(function () {
                        idTemporal += $(this).text() + ',';
                    });
                    var idPlanta = $('.txtPlantaImportada').val().length == 0 ? $('#<%=lblPlantaImportada.ClientID%>').text() : $('.txtPlantaImportada').val();
                    var normal = $('#<%=chkNormal.ClientID%>').prop("checked");
                    var interplanting = $('#<%=chkInterplanting.ClientID%>').prop("checked");
                    var Errores = '';
                    var idDirectriz = $('#idDirectriz').val().length > 0 ? $('#idDirectriz').val() : 0;
                    Errores += nombreDeDirectriz.length == 0 ? 'Se requiere un nombre para la directriz.</br>' : '';
                    Errores += idVariedad.length == 0 ? 'Se requiere elegir una variedad.</br>' : '';
                    Errores += idVariable.length == 0 ? 'Se requiere elegir una variable.</br>' : '';
                    Errores += idTemporal.length == 0 ? 'Se requiere elegir un temporal.</br>' : '';
                    if (Errores.length == 0) {
                        PageMethods.GuardarDirectriz(matriz.split('|'), idDirectriz, nombreDeDirectriz, idVariedad.substring(0, idVariedad.length - 1), idVariable.substring(0, idVariable.length - 1), idTemporal.substring(0, idTemporal.length - 1), idPlanta, normal, interplanting, function (resultado) {
                            popUpAlert(resultado.split(';')[1], resultado.split(';')[0]);
                            cargaDeDirectricesHistoricas();
                        });
                    }
                    else {
                        popUpAlert(Errores, 'error');
                    }
                }
                else {
                    popUpAlert('No se cargó la directriz.', 'error');
                    window.console && console.log('No se cargó la directriz.');
                }
                $.unblockUI();

            });
            var cantidad = '<%=ConfigurationManager.AppSettings["showNumDirectriz"].ToString() %>';
            $('#searchDiv').on('keyup', function () {
                //alert($(this).val());
                var texto = $(this).val().trim();
                var show = 1;
                $('.column div h3').each(
                function () {
                    if (texto != "") {
                        if ($(this).text().split('-')[1].toLowerCase().indexOf(texto.toLowerCase()) !== -1) {
                            $(this).parent().css('display', 'inline-table');
                        }
                        else {
                            $(this).parent().css('display', 'none');
                        }

                    }
                    else {
                        if (show <= cantidad) {
                            $(this).parent().css('display', 'inline-table');
                        } else { $(this).parent().css('display', 'none'); }
                        show++;
                    }
                }
            );
            });

        });

        function leerTabla() {
            $("#<%=hddTabla.ClientID %>").val(($("#ctl00_ContentPlaceHolder1_divGrid").html().replace(/"/g, '\'')).replace(/</g, '☺'));
        }
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div class="column">
     <h1>Directrices Guardadas Recientemente</h1>
         <table style=" margin-bottom:10px;" id="Table1" border="0" >
        <tr>
            <td>
               <asp:Label ID="lbSearch" runat="server">Buscar:</asp:Label>
            </td>
            <td> 
                <input type="text" id="searchDiv" class="search2"/>
            </td>
            </tr></table>
     
    <div class="column" id="divCargadas" style=" min-width:800px;"></div>
    
    <h1>
        Directriz
    </h1>
    <table class="index" id="tbl_formulario" border="0" runat="server">
        <tr>
            <td colspan="6">
                <h2>Descarga de Plantilla</h2>
            </td>
        </tr>
        <tr>
            <td>
            <asp:Label ID="lbl_Planta" runat="server">*Planta:</asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_Planta"></asp:DropDownList>
            </td>
            <td>
            <asp:Label ID="lbl_Semanas" runat="server">*Semanas:</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_Semanas" CssClass="required nonZeroInt32"></asp:TextBox>
            </td>

         <td class="left">
            &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Descarga XLS" 
                    ImageUrl="~/comun/img/download_xls.png" onclick="ImageButton1_Click"
                    style="width:32px;" />
             </td>
             <td class="left">Descargar Plantilla:</td>
        </tr>
    </table>
    <table class="index" border="0">
        <tr>
            <td colspan="5">
                <h2>Importación</h2>
            </td>
        </tr>
         <tr>
            <td>
                <label>Cargar archivo:</label>
            </td>
            <td class="left middle">
                <asp:FileUpload runat="server" ID="fu_Plantilla"/>
            </td>
            
            
             <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="auto-style1"></td>
            <td class="auto-style1"></td>
            <td colspan="2" class="auto-style1">
            <asp:Button runat="server" ID="btn_Importar" onclick="btn_Importar_Click" Text="Importar" />
            </td>
        </tr>
    </table>
    <div class="grid" id="divGrid" runat="server">
        <table cellspacing="0" rules="all" border="1" id="gv_Directriz" style="border-collapse:collapse;"></table>

        </div>        
        <table id="exportarTabla" class="exportTable">
            <tr>
                <td><asp:HiddenField ID="hddTabla" runat="server" />
                    <asp:Label ID="Label1" runat="server" Text="Añadir semanas extras:"/>
                    <asp:TextBox ID="addColum" runat="server" Text="0" Width="50" MaxLength="4" CssClass="required intValidate"/>
                </td>
                <td style="width:150px;">
                    <asp:Button runat="server" ID='descargarTabla' Text='Exportar' Width="150px" onclick="save_Click"  OnClientClick="leerTabla();" />
                </td>
            </tr>
        </table>
        <%--<asp:GridView runat="server" ID="gv_Directriz" CssClass="grid"></asp:GridView>  <asp:TextBox type="text" ID="txtPlantaImportada" runat="server"/>--%>
    <table class="index">
       <tr><td><asp:Label ID="lblNombre" runat="server" Text="Nombre de Directriz:"></asp:Label></td><td colspan="3"><asp:TextBox ID="txtNombre" runat="server" Width="90%" CssClass="txtNombre"></asp:TextBox></td></tr>
       <tr><td><asp:Label ID="lblVariedad" runat="server" Text="Variedades:"></asp:Label></td><td colspan="3"><asp:CheckBoxList runat="server" ID="chkl_Variedad"   RepeatDirection="Horizontal" RepeatColumns="5" CssClass="celdasGrandes"></asp:CheckBoxList></td></tr>        
       <tr><td><asp:Label ID="lblVariable" runat="server" Text="Variables:"></asp:Label></td><td colspan="3"><asp:CheckBoxList runat="server" ID="chkl_Variable" RepeatDirection="Horizontal"  RepeatColumns="8"  CssClass="celdasGrandes"></asp:CheckBoxList></td></tr>
       <tr><td><asp:Label ID="lblTemporales" runat="server" Text="Temporales:"></asp:Label></td><td colspan="3"><asp:CheckBoxList runat="server" ID="chk_Temporales" RepeatDirection="Horizontal"  RepeatColumns="8"  CssClass="celdasGrandes"></asp:CheckBoxList></td></tr>
       <tr style="display:none;">
            <td><asp:Label ID="lblCiclo" runat="server" Text="Ciclo:"></asp:Label></td><td class="checkboxes"><asp:CheckBox runat="server" ID="chkNormal" RepeatDirection="Horizontal"  RepeatColumns="8"  Text="Normal" Checked="true"></asp:CheckBox></td>
            <td class="checkboxes"><asp:CheckBox runat="server" ID="chkInterplanting" RepeatDirection="Horizontal"  RepeatColumns="8" Text="Interplanting"></asp:CheckBox></td>
       </tr>
       <tr style="display:none;"><td ><asp:Label ID="lblPlanta" runat="server" Text="Planta:"></asp:Label></td><td><asp:TextBox type="text" ID="txtPlantaImportada" runat="server"  CssClass="txtPlantaImportada"/><%--<input type="text" class="txtPlantaImportada"></input>--%> <asp:Label runat="server" ID="lblPlantaImportada"></asp:Label></td></tr>        
       <tr><td colspan="4"> <input type="button" id="btn_GuardarTabla" value="Guardar" /></td></tr>
     </table>
    </div>
   
</div>

<uc1:popUpMessageControl runat="server" ID="popUpMessage" />
<input type="hidden" id="idDirectriz" value=""/>
    
</asp:Content>

