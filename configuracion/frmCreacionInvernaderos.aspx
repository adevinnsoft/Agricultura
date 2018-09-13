<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCreacionInvernaderos.aspx.cs" Inherits="configuracion_frmCreacionInvernaderos" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>

        <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type = "text/javascript">
        function functionx(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                alert("Allow Only Numbers");
                return false;
            }
        }
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                else
                    return false;
            }

            if (txt.value.indexOf(".") > 0) {
                var txtlen = txt.value.length;
                var dotpos = txt.value.indexOf(".");
                //Change the number here to allow more decimal points than 2
                if ((txtlen - dotpos) > 2)
                    return false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode <= 90 && charCode >= 65) || (charCode <= 122 && charCode >= 97) || charCode == 8)
                return true;

            return false;

        }
        </script>
    
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 153px;
        }
        .auto-style5 {
            width: 93px;
        }
        .auto-style6 {
            width: 79px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdnIdPlanta" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="6" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltPais" runat="server" meta:resourceKey="ltPaisResource"></asp:Literal>
                            </td>
                            <td class="auto-style5">
                                <asp:DropDownList ID="ddlSucursal" runat="server" meta:resourceKey="ddlPaisResource" AutoPostBack="True" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltRancho" runat="server" meta:resourceKey="RanchoResource"></asp:Literal>
                            </td>
                            <td class="auto-style1" colspan="2">
                                <asp:DropDownList ID="ddlRancho" runat="server" meta:resourceKey="ddlRanchoResource" AutoPostBack="True" OnSelectedIndexChanged="ddlRancho_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="ltConsecutivoInvernadero" runat="server" meta:resourceKey="ltConsecutivoInvernaderoResource"></asp:Literal>
                            </td>
                            <td><asp:Label ID="lblConsecutivoInvernadero" runat="server" ></asp:Label></td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                *<asp:Literal ID="ltNoInvernadero" runat="server" meta:resourceKey="ltInvernaderoResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                                <asp:TextBox id="txtNoInvernadero" runat="server"  Width="39px" ReadOnly="True"></asp:TextBox>
                            </td>
                             <td align="right">*<asp:Literal ID="ltClave" runat="server" meta:resourceKey="ltClaveResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:TextBox ID="txtClave" runat="server"  Width="39px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </td>
                             <td align="right">
                                 *<asp:Literal ID="ltGrupo" runat="server" meta:resourceKey="ltGrupoResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:TextBox id="txtGrupo" runat="server" Width="54px" onkeypress="return onlyDotsAndNumbers(this,event);" ></asp:TextBox>
                            </td>
                           
                            <td>*<asp:Literal ID="ltHectarea" runat="server" meta:resourceKey="ltHectareaResource"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHectarea" runat="server"  Width="39px" onkeypress="return onlyDotsAndNumbers(this,event);" ></asp:TextBox>
                            </td>
                           
                        </tr>
                        <tr>
                            <td align="right">
                                *<asp:Literal ID="ltSecciones" runat="server" meta:resourceKey="ltSeccionResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                               
                                <asp:TextBox ID="txtSecciones" runat="server"  Width="39px" onkeypress="return onlyDotsAndNumbers(this,event);" ></asp:TextBox>
                           
                            </td>

                            <td align="right">*<asp:Literal ID="tlSurcos" runat="server" meta:resourceKey="ltSurcosResource"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:TextBox ID="txtSurcos" runat="server"  Width="39px" onkeypress="return onlyDotsAndNumbers(this,event);" ></asp:TextBox>
                            </td>

                            <td>
                                <asp:Literal ID="ltClaveInvAgro" runat="server" meta:resourceKey="ltClaveInvAgroe"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdAgroSmart" runat="server" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100px"></asp:TextBox>
                            </td>

                            <td>
                                <asp:Literal ID="ltClaveInv" runat="server" meta:resourceKey="ltClaveInvResource"></asp:Literal>
                            </td>
                            <td><asp:Label ID="lblClaveInvernadero" runat="server" ></asp:Label></td>

                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:Literal ID="ltZonificado" runat="server" meta:resourcekey="ltZonificadoResource1"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                                <asp:CheckBox ID="chkZonificado" runat="server"  meta:resourcekey="chkZonificadoResource1" />
                            </td>
                            <td align="right">
                                <asp:Literal ID="ltInvestigacion" runat="server" meta:resourcekey="ltInvestigacionResource1"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:CheckBox ID="chkInvestigacion" runat="server"  meta:resourcekey="chkInvestigacionResource1" />
                            </td>
                            <td>
                                <asp:Literal ID="ltPasillo" runat="server" meta:resourcekey="ltPasilloResource1"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPasillo" runat="server"  meta:resourcekey="chkPasilloResource1" />
                            </td>
                        </tr>
                        
                        <tr>
                             <td align="right">
                                <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                            </td>
                            <td align="left" class="auto-style5">
                                <asp:CheckBox ID="chkActivo" runat="server" 
                                    meta:resourcekey="chkActivoResource1" Checked="True" />
                            </td>
                             <td align="left">&nbsp;</td>
                             <td align="left" class="auto-style6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>&nbsp;</td>
                            <td class="auto-style6">&nbsp;</td>
                            <td> <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" /></td>
                          <td>

                              <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
                          </td>
           
                        </tr>
                    </table>
              </asp:Panel>
           

            <div class="grid">
                 <div id="pager" class="pager">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <input type="text" class="pagedisplay" />
                    <img alt="next" src="../comun/img/next.png" class="next" />
                    <img alt="last" src="../comun/img/last.png" class="last" />
                    <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="30">30</option>
                        <option value="40">40</option>
                        <option value="50">50</option>
                    </select>
                </div>

                <asp:GridView ID="GvPlantas" runat="server" AutoGenerateColumns="False" 
                     CssClass="gridView" Width="90%" EmptyDataText="No existen registros" 
                DataKeyNames="idInvernadero" meta:resourcekey="GvPlantasResource1"  
                     onprerender="GvPlantas_PreRender" onrowdatabound="GvPlantas_RowDataBound" 
                     onselectedindexchanged="GvPlantas_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>

                        <asp:BoundField DataField="Sucursal" SortExpression="Sucursal"
                            meta:resourcekey="BoundFieldResource1">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Rancho" SortExpression="Rancho"
                            meta:resourcekey="BoundFieldResource2">
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="Invernadero" SortExpression="Invernadero"
                            meta:resourcekey="BoundFieldResource3">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="NoInvernadero" SortExpression="NoInvernadero"
                            meta:resourcekey="BoundFieldResource4">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="Zona" meta:resourcekey="BoundFieldResource5"
                            SortExpression="Zona">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Grupo" meta:resourcekey="BoundFieldResource7"
                            SortExpression="Grupo">
                          
                        </asp:BoundField>
                          <asp:BoundField DataField="idInvernaderoAGROSMART" HeaderText="id AgroSmart"
                            SortExpression="idInvernaderoAGROSMART">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Hectarea" meta:resourcekey="BoundFieldResource8"
                            SortExpression="Hectarea">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="Secciones" meta:resourcekey="BoundFieldResource9"
                            SortExpression="Secciones">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Surcos" meta:resourcekey="BoundFieldResource10"
                            SortExpression="Surcos">
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="Zonificacion" meta:resourcekey="BoundFieldResource11"
                            SortExpression="Zonificacion">
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="Investigacion" meta:resourcekey="BoundFieldResource12"
                            SortExpression="Investigacion">
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="PasilloMedio" meta:resourcekey="BoundFieldResource13"
                            SortExpression="PasilloMedio">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="Activo" SortExpression="Activo"
                            meta:resourcekey="BoundFieldResource6">
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
    </div>
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>


