<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmGrowingConditions.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 300px;
        }
    </style>
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/slider/slick.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/moment.min.js" type="text/javascript"></script>
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {
                $('#accordion').accordion('destroy').accordion({ collapsible: true });
            });      
            function CargaAcordion() {
                $('#accordion').accordion('destroy').accordion({ collapsible: true });
            }

        }
     
//     function Regenerar(intGrupo) {
//                                     PageMethods.btnRestablecer($('#ctl00_ddlPlanta').val(), function (response) {
//                $('#rollslider2').removeClass();
//                $('.habilidades #rollslider2').html(response);
//                setHabilidades();
//            });

     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <asp:UpdatePanel ID="updGrowingConditions" runat="server">
            <ContentTemplate>
                <table class="index">
                    <tr>
                        <td colspan="9">
                           <h2> <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:CAPTURADEGROWINGCONDITIONS%>" ></asp:Label></h2>
                        </td>
                    </tr>
                    <tr>
                   
                                    <td>
                                        <asp:Label ID="lblPlanta" runat="server" ForeColor="Black" Text="<%$ Resources:Planta%>"> </asp:Label>
                                    </td>
                                      <td >
                                        <asp:DropDownList ID="ddlPlanta" runat="server" AutoPostBack="True" 
                                            onselectedindexchanged="ddlPlanta_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblInvernadero" runat="server" ForeColor="Black" 
                                            Text="<%$ Resources:Invernadero%>"></asp:Label>
                                    </td>
                                     <td>
                                        <asp:DropDownList ID="ddlInvernadero" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEstatus" runat="server" ForeColor="Black" Text="<%$ Resources:Estatus%>"  ></asp:Label>
                                    </td>
                                        <td>
                                        <asp:DropDownList ID="ddlEstatus" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                 
                                    <td>
                                        <asp:Label ID="lblSemana" runat="server" ForeColor="Black" Text="<%$ Resources:Semana%>" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSemana" runat="server">
                                        </asp:DropDownList>
                                    </td>     
                                    <td>
                                        <asp:RadioButtonList ID="rdbPlantacion" runat="server" 
                                            RepeatDirection="Horizontal" Width="99%">
                                            <asp:ListItem Value="1">Plantación</asp:ListItem>
                                            <asp:ListItem Value="2">No Plantación</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                           
                    <tr>
           
                                    <td width="250px">
                                        <asp:Label ID="lblGrower" runat="server" ForeColor="Black" Text="Grower:"></asp:Label>
                                    </td>      
                                   <td width="250px">
                                        <asp:TextBox ID="txtGrower" runat="server" 
                                            ForeColor="Black" Width="99%"></asp:TextBox>
                                        <asp:HiddenField ID="hidPKGrower" runat="server" />
                                    </td>
                                    <td width="250px">
                                        <asp:Label ID="lblGteZona" runat="server" ForeColor="Black" Text="<%$ Resources:GteZona%>"></asp:Label>
                                    </td> 
                                      <td width="250px">
                                        <asp:TextBox ID="txtGteZona" runat="server" 
                                            ForeColor="Black" Width="99%"></asp:TextBox>
                                        <asp:HiddenField ID="hidPKGteZona" runat="server" />
                                    </td>
                                    <td width="250px">
                                        <asp:Label ID="lblLider" runat="server" ForeColor="Black" Text="<%$ Resources:Lider%>"></asp:Label>
                                    </td>     
                                    <td width="250px" colspan="4">
                                        <asp:TextBox ID="txtLider" runat="server" 
                                            ForeColor="Black" Width="99%"></asp:TextBox>
                                        <asp:HiddenField ID="hidPKLider" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                               
                                 
                               
                                    <td align="left" colspan="9">
                                        <asp:Button ID="btnCargarDatos" runat="server" onclick="btnCargarDatos_Click" 
                                            Text="<%$ Resources:Cargar%>" />
                                    </td>
                                </tr>
                           
                    <tr>
        
                                    <td colspan="9">
                                        <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click" 
                                           Text="<%$ Resources:Guardar%>" />
                                 
                                        <asp:Button ID="btnCancelar" runat="server" onclick="btnCancelar_Click" 
                                             Text="<%$ Resources:Cancelar%>" />
                                    </td>
                         
                    </tr>
                    <tr>
                        <td colspan="9">
                            <div ID="accordion" class="accordion">
                                <h2>
                                    Plantación</h2>
                                <div>
                           
                                           
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaPlantacion" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                onselectedindexchanged="rdbOpcionMasivaPlantacion_SelectedIndexChanged" 
                                                                AutoPostBack="True">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                        
                                                            <asp:Button ID="btnRestablecerPlantacion" runat="server" Text="<%$ Resources:Restablecer%>"
                                                                onclick="btnRestablecerPlantacion_Click" />
                                                       
                                       
                                          
                                                <asp:GridView ID="grdPlantacion" runat="server" 
                                                    onrowcreated="grdPlantacion_RowCreated" 
                                                    onrowdatabound="grdPlantacion_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro"   HeaderText="<%$ Resources:Parametro%>"  />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                         
                                     
                                </div>
                                <h2>
                                    Control de Clima</h2>
                                <div>
                                    
        
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaControlClima" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaControlClima_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                              
                                                            <asp:Button ID="btnRestablecerControlClima" runat="server" Text="<%$ Resources:Restablecer%>" 
                                                                onclick="btnRestablecerControlClima_Click" />
                                                      
                                         
                                                <asp:GridView ID="grdControlClima" runat="server" 
                                                    onrowcreated="grdControlClima_RowCreated" 
                                                    onrowdatabound="grdControlClima_RowDataBound" AutoGenerateColumns="False">
                                               <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>" />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                           
                                </div>
                                <h2>
                                    Trabajos Culturales</h2>
                                <div>
                                    
                                            
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaTrabajoCultural" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaTrabajoCultural_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                    
                                                            <asp:Button ID="btnRestablecerTrabajoCultural" runat="server" 
                                                                Text="<%$ Resources:Restablecer%>" onclick="btnRestablecerTrabajoCultural_Click" />
                                                       
                                         
                                                <asp:GridView ID="grdTrabajoCultural" runat="server" 
                                                    onrowcreated="grdTrabajoCultural_RowCreated" 
                                                    onrowdatabound="grdTrabajoCultural_RowDataBound" 
                                                    AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>"/>
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                         
                                </div>
                                <h2>
                                    Plagas y Enfermedades</h2>
                                <div>
                      
                                
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaPlagas" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaPlagas_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                      
                                                            <asp:Button ID="btnRestablecerPlagas" runat="server" Text="<%$ Resources:Restablecer%>"
                                                                onclick="btnRestablecerPlagas_Click" />
                                                       
                                           
                                                <asp:GridView ID="grdPlagas" runat="server" onrowcreated="grdPlagas_RowCreated" 
                                                    onrowdatabound="grdPlagas_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>" />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                          
                                </div>
                                <h2>
                                    Trampeo</h2>
                                <div>
                                  
                                            
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaTrampeo" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaTrampeo_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                     
                                                            <asp:Button ID="btnRestablecerTrampeo" runat="server" Text="<%$ Resources:Restablecer%>"
                                                                onclick="btnRestablecerTrampeo_Click" />
                                                      
                                           
                                                <asp:GridView ID="grdTrampeo" runat="server" 
                                                    onrowcreated="grdTrampeo_RowCreated" 
                                                    onrowdatabound="grdTrampeo_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>"/>
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                       
                                </div>
                                <h2>
                                    Limpieza de Invernaderos</h2>
                                <div>
                               
                                      
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaLimpieza" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaLimpieza_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                   
                                                            <asp:Button ID="btnRestablecerLimpieza" runat="server" Text="<%$ Resources:Restablecer%>"
                                                                onclick="btnRestablecerLimpieza_Click" />
                                      
                                                <asp:GridView ID="grdLimpieza" runat="server" 
                                                    onrowcreated="grdLimpieza_RowCreated" 
                                                    onrowdatabound="grdLimpieza_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>" />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                         
                                </div>
                                <h2>
                                    Fertirriego</h2>
                                <div>
                                
                                               
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaFertirriego" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaFertirriego_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                     
                                                            <asp:Button ID="btnRestablecerFertirriego" runat="server" Text="<%$ Resources:Restablecer%>" 
                                                                onclick="btnRestablecerFertirriego_Click" />
                                                       
                                          
                                                <asp:GridView ID="grdFertirriego" runat="server" 
                                                    onrowcreated="grdFertirriego_RowCreated" 
                                                    onrowdatabound="grdFertirriego_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>" />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                        
                                </div>
                                <h2>
                                    Estado del Fruto</h2>
                                <div>
                                
                                             
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaEstadoDelFruto" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaEstadoDelFruto_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                      
                                                            <asp:Button ID="btnRestablecerEstadoDelFruto" runat="server" 
                                                                Text="<%$ Resources:Restablecer%>" onclick="btnRestablecerEstadoDelFruto_Click" />
                                                       
                                         
                                                <asp:GridView ID="grdEstadoDelFruto" runat="server" 
                                                    onrowcreated="grdEstadoDelFruto_RowCreated" 
                                                    onrowdatabound="grdEstadoDelFruto_RowDataBound" 
                                                    AutoGenerateColumns="False">
                                                <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>" />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                          
                                </div>
                                <h2>
                                    Polinización</h2>
                                <div>
                                
                                          
                                                            <asp:RadioButtonList ID="rdbOpcionMasivaPolinizacion" runat="server" 
                                                                BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
                                                                Font-Bold="True" RepeatDirection="Horizontal" Width="99%" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="rdbOpcionMasivaPolinizacion_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Select all N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">Select all OK</asp:ListItem>
                                                                <asp:ListItem Value="3">Select all X</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                     
                                                            <asp:Button ID="btnRestablecerPolinizacion" runat="server" Text="<%$ Resources:Restablecer%>"
                                                                onclientclick="Regenerar(9);" onclick="btnRestablecerPolinizacion_Click" />
                                                     
                                        
                                                <asp:GridView ID="grdPolinizacion" runat="server" AutoGenerateColumns="False" 
                                                    onrowcreated="grdPolinizacion_RowCreated" 
                                                    onrowdatabound="grdPolinizacion_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="idParametroPorGrupoGrowing" />
                                                        <asp:BoundField DataField="idGrupoGrowing" />
                                                        <asp:BoundField DataField="NombreParametro" HeaderText="<%$ Resources:Parametro%>" />
                                                        <asp:BoundField DataField="AplicaListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeNA_OK_X" />
                                                        <asp:BoundField DataField="AplicaListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="AplicaCatalogoDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="NValoresSeleccionableParaDetalleDeListaDeS_A_G_N" />
                                                        <asp:BoundField DataField="PuntajeAsignado" />
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblNOKX" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                                <asp:ListItem>Ok</asp:ListItem>
                                                                                <asp:ListItem>X</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroNAOKX" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSAGN" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem>S</asp:ListItem>
                                                                                <asp:ListItem>A</asp:ListItem>
                                                                                <asp:ListItem>G</asp:ListItem>
                                                                                <asp:ListItem>N/A</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:RadioButtonList ID="rblSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style="border-style: solid; border-width: thin">
                                                                            <asp:CheckBoxList ID="chkSubParametroSAGN" runat="server" 
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                         
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones:"></asp:Label>
                        </td>
                        <td colspan="8">
                            <asp:TextBox ID="txtObservaciones" runat="server" Height="80px" Width="99%" 
                                MaxLength="2500" TextMode="MultiLine"></asp:TextBox>
                           
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlPlanta" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerControlClima" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerEstadoDelFruto" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerFertirriego" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerLimpieza" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerPlagas" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerPlantacion" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerPolinizacion" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerTrabajoCultural" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerTrampeo" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaPolinizacion" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaControlClima" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaEstadoDelFruto" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaFertirriego" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaLimpieza" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaPlagas" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerPlantacion" />
                <asp:AsyncPostBackTrigger ControlID="btnRestablecerTrabajoCultural" />
                <asp:AsyncPostBackTrigger ControlID="rdbOpcionMasivaTrampeo" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

