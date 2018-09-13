<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="ConfiguracionCorreosPreHarvest.aspx.cs" Inherits="configuracion_ConfiguracionCorreosPreHarvest" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.pager.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.tablesorter.widgets.js"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" id="funcionamientoDePantalla">
        var listaDeDistribucion;//variable global
        function EliminarCorreo(objImg,objNombreCorreo) {
            if ($(objImg).parent().parent().attr('class').indexOf('Cargado') >= 0) {
                $(objImg).parent().parent().removeClass('Cargado').addClass('Eliminado');
            } 
            else 
            {
                $(objImg).parent().parent().remove();
            }
       }  

        function obtenerListaDeDistribucion() {
            PageMethods.obtenerListaDeDistribucion(function (response) {
                if (response[0] == '1') {
                    $('#gvPreHarvest').append(response[2].toString());

                }
                else {
                    popUpAlert(response[1],response[2]);
                }
            });
        }


        function agregarCorreos(){
           var SAMaccount = $('.CuentaPreHarvest').val().trim();

           try {
               $.blockUI();
               PageMethods.agregarCorreos(SAMaccount,function(response){
                     if(response[0] == '1'){
                        $('#gvPreHarvest').append(response[2]);
                     }
                     else
                     {
                        popUpAlert(response[1],response[2]);
                     }
               }); 
            } catch (e) {
                console.log(e)
            }
            finally{
               $.unblockUI();
            }
           
        }

        function verificarSAMaccount(){
            var SAMaccount = $('.CuentaPreHarvest').val().trim();
            var existente = false;
            var vacio = false;  
               
                if(SAMaccount == '')
                {
                 vacio = true;
                }
                else
                {
                 vacio = false;
                }
                    
                if(vacio)
                {
                popUpAlert("Ingrese una cuenta para continuar con el proceso","warning");
                return false;
                }
            else
                {

                try {
                  $.blockUI();
                      PageMethods.buscarEnActiveDirectory(SAMaccount, function (response){
                        if (response[0] == '1') {
                        $('.divDatos').html(response[2]);
                        $('.divDatos').show();

                        }
                        else {
                            popUpAlert(response[1],response[2]);
                        }
                    });
                } catch (e) {
                   console.log(e);
                }
                finally{
                   $.unblockUI();
                }
                
             }
        }


        function LimpiarDatos(){
          $('.CuentaPreHarvest').val('');
          $('.divDatos').empty();

        }


        function guardarListaDeDistribucion(){
         listaDeDistribucion = $('#table tbody tr td input[class="invisible"]').map(function () {
                    if ($(this).val() != '') {
                        return {
                            idCaptura : $('#gvPreHarvest tbody tr[idCaptura]').first().text() == '' ? <%=idCapturaDefault %> : $('#gvPreHarvest tbody tr[idCaptura]').first().text(),
                            etiqueta: $(this).val(),
                            correos: $('#gvPreHarvest tbody tr:not(.Cargado)').map(function () {//#gvPreHarvest tbody tr[class="Nuevo"]
                                if ($(this).text() != '') {
                                    return {
                                        correo: $(this).find('td[otherclass="Correo"]').text(),
                                        cuenta: $(this).find('td[otherclass="SAMaccount"]').text(),
                                        estado: $('#gvPreHarvest tbody tr[class="Nuevo"]').length > 0 ? 1 : 0
                                    }
                                }
                            }).get()  
                        }
                    }

                }).get();

                PageMethods.GuardarListaDeDistribucion(listaDeDistribucion, function (response) {
                    if (response[0] == "1") {
                        popUpAlert(response[1], response[2]);
                    } 
                    else {
                        popUpAlert(response[1], response[2]);
                    }
                });
        }


        $(function () {
           
           

              $('#btnBuscarSAMaccount').click(function(){
                 verificarSAMaccount();
              });


              $('#btnCancelar').click(function(){
                 LimpiarDatos();
              });

              $('#btnGuardarPreHarvest').click(function () { 
                 guardarListaDeDistribucion();
              });

            obtenerListaDeDistribucion(); 

        });
//            $('#btnEnviarMail').click(function(){
//               var Subject = 'Mi prueba';
//               var Message = 'Esto es un mensaje de prueba';
//               var To = []

//               To.push("israel.loera@dominio6.com");

//                   PageMethods.sendMail(To,Subject,Message, function (response) {
//                    if (response[0] == "1") {
//                        popUpAlert(response[1], response[2]);
//                    }
//                    else {
//                        popUpAlert(response[1], response[2]);
//                    }
//                });
//            });

             



           
            
   


    </script>
     <style type="text/css">
         .tablesorter .filtered
         {
             display: none;
         }
         /* Ajax error row */
         .tablesorter .tablesorter-errorRow td
         {
             text-align: center;
             cursor: pointer;
             background-color: #e6bf99;
         }
         .configCorreosPreHarvest h3
         {
             width: 100%;
             min-width: 100%;
             background: #F4D101;
             color: BLACK;
             padding: 10px;
             text-align: left;
             background-repeat: no-repeat;
             background-position-x: 99%;
             background-position-y: 7px;
             border: 1px solid white;
         }
         .configCorreosPreHarvest h3.open
         {
             width: 100%;
             background: #F4D101;
             padding: 10px;
             text-align: left;
             background-repeat: no-repeat;
             background-position-x: 99%;
             background-position-y: 7px;
         }
         td.correos
         {
             min-width: 48%;
             max-width: 48%;
             width: 48%;
             padding-right: 2%;
         }
         
         .accBody
         {
             border: 1px dotted orange;
             padding: 9px;
             width: 100%;
             background: #F0F5E5;
         }
         #gvPreHarvest
         {
             width: 90%;
         }
         
         .configCorreosPreHarvest
         {
             max-width: 805px;
         }
         .accHead
         {
             cursor: pointer;
         }
         .accHead:hover
         {
             cursor: pointer;
             background-color: #FFC10F;
             border: 1px solid #FFC10F;
         }
         .Eliminado
         {
             display: none;
         }
         .botonAgregar
         {
             width: 24px;
         }
         
         .divDatos
         {
             display: none;
         }
         input#btnBuscarSAMaccount
         {
             position: relative;
             right: 400px;
             top: -6px;
         }
         .divDatos
         {
             margin-top: 20px;
         }
         
         .divDatos span
         {
             display: block;
             margin-bottom: 10px;
         }
         
         label.lblNombre, label.lblCorreo
         {
             font-weight: bold;
         }
         
         img#btnAgregarPreHarvest
         {
             position: relative;
             left: 210px;
             bottom: 40px;
             cursor:pointer;
         }
         label.lblCorreoMostrado 
         {
             padding-left:5px;
         }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
                   <script type="text/javascript">
                       Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

                          
           
                       });

                   </script>
                  
                   <h1><asp:Label runat="server">Configuracion de Correos para Pre-Harvest</asp:Label></h1>
                   <div class="configCorreosPreHarvest">
                      <h3 class="accHead"><asp:Label ID="lblPreHarvest" runat="server" Text="Pre-Harvest"></asp:Label></h3>
                      <div class="accBody">
                        <table id="table">
                            <tr>
                                <td class="correos">
                                    <asp:Label ID="lblNombreDeLista" runat="server"  Text="Nombre de Lista" CssClass="invisible"></asp:Label>
                                    <asp:TextBox ID="txtNombreDeLista" class="inputNombreLista" runat="server" CssClass="invisible" MaxLength="200" Text="lista"></asp:TextBox>
                                    <asp:Label ID="lblCuentaPreHarvest" runat="server"  Text="Cuenta"></asp:Label>
                                    <asp:TextBox ID="txtCuentaPreHarvest" runat="server" class="CuentaPreHarvest" MaxLength="200"></asp:TextBox>
                                    <input  id="btnBuscarSAMaccount" class="Buscar" type="button"  value="Buscar"/> 
                                    <div class="divDatos">
                                        <label for="lblNombre" class="lblNombre">Nombre:</label>
                                        <label for="lblCorreo" class="lblCorreo">Correo:</label>
                                        <label id="lblNombreMostrado"></label>
                                        <label id="lblCorreoMostrado"></label>
                                    </div>
                                    <%--<img src="../comun/img/add-icon.png" id="btnAgregarPreHarvest" class="botonAgregar">--%>
                                    <table id="gvPreHarvest" class="gridView">
                                        <thead>
                                        <tr>
                                            <th rowspan="1" class="invisible">IdLista</th>
                                            <th rowspan="1" class="invisible">SAMaccount</th>
                                            <th colspan="1">Correo</th>
                                            <th colspan="1">Icono</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                      </div>
                      <input  id="btnGuardarPreHarvest" class="Guardar" type="button"  value="Guardar"/> 
                      <input  id="btnCancelar" class="Cancelar" type="button"  value="Limpiar"/> 
                   <%--   <input id="btnEnviarMail" type="button" value="Enviar" />--%>
                   </div>
                   <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
             </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
