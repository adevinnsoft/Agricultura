<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Semillas.aspx.cs" Inherits="pages_Semillas" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    	<div class="container">
        <div id="divRecepcion">
         <h1><asp:Label runat="server" ID="lblRecepcionDeSemillaTitulo" 
                 meta:resourcekey="lblRecepcionDeSemillaTituloResource1">Recepción de Semilla</asp:Label></h1>
            <table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
                <tr><td><h3><asp:Label ID="lblRecepcionDeSemilla" runat="server" 
                        Text="Recepción de Semilla" meta:resourcekey="lblRecepcionDeSemillaResource1" ></asp:Label></h3></td></tr>
                <tr>
                    <td><asp:Label ID="lblRecepcionProveedor" runat="server" Text="Semillera" 
                            meta:resourcekey="lblRecepcionProveedorResource1"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="DropDownList1" 
                            meta:resourcekey="DropDownList1Resource1"></asp:DropDownList></td>
                    <td><asp:Label ID="lblRecepcionActivo" runat="server" Text="Activo" 
                            meta:resourcekey="lblRecepcionActivoResource1"></asp:Label></td>
                    <td><asp:CheckBox ID="chkRecepcionActivo" runat="server" 
                            meta:resourcekey="chkRecepcionActivoResource1" /></td>
                </tr>
                    <tr>
                    <td><asp:Label ID="lblRecepcionCodigoSemilla" runat="server" 
                            Text="Código de Semilla" meta:resourcekey="lblRecepcionCodigoSemillaResource1"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlProveedor0" 
                            meta:resourcekey="ddlProveedor0Resource1"></asp:DropDownList></td>
                    <td><asp:Label ID="lblRecepcionRecibir" runat="server" Text="Recibir" 
                            meta:resourcekey="lblRecepcionRecibirResource1"></asp:Label></td>
                    <td><asp:TextBox ID="txtRecepcionRecibir" runat="server" 
                            meta:resourcekey="txtRecepcionRecibirResource1"></asp:TextBox></td>
                </tr>
                    <tr><td><asp:Label ID="lblRecepcionCodigoNS" runat="server" Text="Código NS" 
                            meta:resourcekey="lblRecepcionCodigoNSResource1"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlRecepcionCodigoNS" 
                            meta:resourcekey="ddlRecepcionCodigoNSResource1"></asp:DropDownList></td>
                    <td><asp:Label ID="lblRecepcionComentarios" runat="server" Text="Comentarios" 
                            meta:resourcekey="lblRecepcionComentariosResource1"></asp:Label></td>
                    <td rowspan="2"><asp:TextBox ID="txtRecepcionComentarios" runat="server" Rows="4" 
                            TextMode="MultiLine" meta:resourcekey="txtRecepcionComentariosResource1"></asp:TextBox></td>
                </tr>
                    <tr><td><asp:Label ID="lblRecepcionExistencia" runat="server" Text="Existencia" 
                            meta:resourcekey="lblRecepcionExistenciaResource1"></asp:Label></td>
                    <td><asp:Label ID="lblRecepcionExistenciaMuestra" runat="server" Text="#'###,###" 
                            meta:resourcekey="lblRecepcionExistenciaMuestraResource1"></asp:Label></td>
                </tr>
             </table>
             <table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
                <tr>
                    <td>
                        <asp:Button ID="btnRecepcionGuardar" runat="server"  Text="Guardar" 
                            meta:resourcekey="btnRecepcionGuardarResource1"/>
                        <asp:Button ID="btnRecepcionLimpiar" runat="server"  Text="Limpiar  " 
                            meta:resourcekey="btnRecepcionLimpiarResource1"/>
                    </td>
               </tr>
            </table>
        </div>
        <div id="NuevaSemilla">
		    <h1><asp:Label runat="server" ID="lbl_Semilla" Text="Nueva Semilla" 
                    meta:resourcekey="lbl_SemillaResource1"></asp:Label></h1>
		        <table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
                    <tr><td><h3><asp:Label ID="lblDatosGenerales" runat="server" Text="Datos Generales" 
                            meta:resourcekey="lblDatosGeneralesResource1"></asp:Label></h3></td></tr>
                    <tr>
                        <td><asp:Label ID="lblProveedor" runat="server" Text="Proveedor" 
                                meta:resourcekey="lblProveedorResource1"></asp:Label></td>
                        <td><asp:DropDownList runat="server" ID="ddlProveedor" 
                                meta:resourcekey="ddlProveedorResource1"></asp:DropDownList></td>
                        <td><asp:Label ID="lblCodigoSemillaProveedor" runat="server" 
                                Text="Codigo de Semilla (Proveedor)" 
                                meta:resourcekey="lblCodigoSemillaProveedorResource1"></asp:Label></td>
                        <td><asp:TextBox ID="txtCodigoSemillaProveedor" runat="server" 
                                meta:resourcekey="txtCodigoSemillaProveedorResource1"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td><asp:Label ID="lblColorProveedor" runat="server" Text="Color (Proveedor)" 
                                meta:resourcekey="lblColorProveedorResource1"></asp:Label></td>
                        <td><asp:TextBox ID="txtColorProveedor" runat="server" 
                                meta:resourcekey="txtColorProveedorResource1"></asp:TextBox></td>
                        <td><asp:Label ID="lblStock" runat="server" Text="Stock Inicial" 
                                meta:resourcekey="lblStockResource1"></asp:Label></td>
                        <td><asp:TextBox ID="txtStock" runat="server" meta:resourcekey="txtStockResource1"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td><asp:Label ID="lblStockMinimo" runat="server" Text="Stock Mínimo" 
                                meta:resourcekey="lblStockMinimoResource1"></asp:Label></td>
                        <td><asp:TextBox ID="txtStockMinimo" runat="server" 
                                meta:resourcekey="txtStockMinimoResource1"></asp:TextBox></td>
                        <td><asp:Label ID="lblStockMaximo" runat="server" Text="Stock Máximo" 
                                meta:resourcekey="lblStockMaximoResource1"></asp:Label></td>
                        <td><asp:TextBox ID="txtStockMaximo" runat="server" 
                                meta:resourcekey="txtStockMaximoResource1"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td><asp:Label ID="lblColorNS" runat="server" Text="Color NS" 
                                meta:resourcekey="lblColorNSResource1"></asp:Label></td>
                        <td><asp:DropDownList runat="server" ID="ddlColorNS" 
                                meta:resourcekey="ddlColorNSResource1"></asp:DropDownList></td>
                        <td><asp:Label ID="lblSegmento" runat="server" Text="Segmento" 
                                meta:resourcekey="lblSegmentoResource1"></asp:Label></td><td>
                         <asp:DropDownList runat="server" ID="ddlSegmento" 
                                 meta:resourcekey="ddlSegmentoResource1"></asp:DropDownList></td>
                    </tr>
                     <tr>
                        <td><asp:Label ID="lblTamano" runat="server" Text="Tamaño" 
                                meta:resourcekey="lblTamanoResource1"></asp:Label></td><td>
                         <asp:DropDownList runat="server" ID="ddlTamano" 
                                 meta:resourcekey="ddlTamanoResource1"></asp:DropDownList></td>
                        <td><asp:Label ID="lblUso" runat="server" Text="Uso" 
                                meta:resourcekey="lblUsoResource1"></asp:Label></td><td>
                         <asp:DropDownList runat="server" ID="ddlUso" meta:resourcekey="ddlUsoResource1"></asp:DropDownList></td>
                    </tr>
                     <tr>
                        <td><asp:Label ID="lblTratamiento" runat="server" Text="Tratamiento" 
                                meta:resourcekey="lblTratamientoResource1"></asp:Label></td><td>
                         <asp:DropDownList runat="server" ID="ddlTratamiento" 
                                 meta:resourcekey="ddlTratamientoResource1"></asp:DropDownList></td>
                        <td><asp:Label ID="lblForma" runat="server" Text="Forma" 
                                meta:resourcekey="lblFormaResource1"></asp:Label></td><td>
                         <asp:DropDownList runat="server" ID="ddlForma" meta:resourcekey="ddlFormaResource1"></asp:DropDownList></td>
                    </tr>
                    <tr><td><asp:Label runat="server" ID="lblImagen" Text="Imagen" 
                            meta:resourcekey="lblImagenResource1"></asp:Label></td>
                        <td colspan="3"><img src="../comun/img/loadImage.png" alt="Imagen" style="width:120px;" /></td>
                    </tr>
                 </table>
             <table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
             <tr><td><h3><asp:Label runat="server" ID="lblAvisos" 
                     meta:resourcekey="lblAvisosResource1">- Avisos -</asp:Label></h3></td></tr>
                <tr>
                    <td><asp:CheckBox runat="server" ID="chkAvisoMinimo" 
                            meta:resourcekey="chkAvisoMinimoResource1" /></td>
                    <td><asp:Label ID="lblAvisoMinimo" runat="server" 
                            Text="Enviar aviso de mínimo alcanzado" 
                            meta:resourcekey="lblAvisoMinimoResource1"></asp:Label></td>
                    <td><asp:CheckBox runat="server" ID="chkEntrada" 
                            meta:resourcekey="chkEntradaResource1" /></td>
                    <td><asp:Label ID="lblEntrada" runat="server" Text="Enviar aviso de entrada" 
                            meta:resourcekey="lblEntradaResource1"></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:CheckBox runat="server" ID="chkAvisoMaximo" 
                            meta:resourcekey="chkAvisoMaximoResource1" /></td>
                    <td><asp:Label ID="lblAvisoMaximo" runat="server" 
                            Text="Enviar aviso de mínimo alcanzado" 
                            meta:resourcekey="lblAvisoMaximoResource1"></asp:Label></td>
                    <td><asp:CheckBox runat="server" ID="chkAvisoSalida" 
                            meta:resourcekey="chkAvisoSalidaResource1" /></td>
                    <td><asp:Label ID="lblAvisoSalida" runat="server" Text="Enviar aviso de entrada" 
                            meta:resourcekey="lblAvisoSalidaResource1"></asp:Label></td>
                </tr>
            </table>
        
             <table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
                <tr><td><h3><asp:Label runat="server" ID="lblCorreos" 
                        meta:resourcekey="lblCorreosResource1">- Correos -</asp:Label></h3></td></tr>
                <tr><td><asp:Label ID="lblDestinatario" runat="server" Text="Destinatario" 
                        meta:resourcekey="lblDestinatarioResource1"></asp:Label></td>
                    <td><asp:TextBox runat="server" ID="txtDestinatario" 
                            meta:resourcekey="txtDestinatarioResource1"></asp:TextBox></td></tr>
                <tr><td><asp:Label ID="lblDestinatariosElegidos" runat="server" 
                        Text="Destinatarios Elegidos:" 
                        meta:resourcekey="lblDestinatariosElegidosResource1"></asp:Label></td>
                    <td><asp:ListBox runat="server" ID="liDestinatariosElegidos" 
                            meta:resourcekey="liDestinatariosElegidosResource1"></asp:ListBox></td></tr>
            </table>

            <table class="index" style="width: 800px; max-width: 800px; min-width: 800px;">
                <tr>
                    <td>
                        <asp:Button ID="btnGuardar" runat="server"  Text="Guardar" 
                            meta:resourcekey="btnGuardarResource1"/>
                        <asp:Button ID="btnLimpiar" runat="server"  Text="Limpiar  " 
                            meta:resourcekey="btnLimpiarResource1"/>
                    </td>
               </tr>
            </table>
        </div>
        </div>

</asp:Content>

