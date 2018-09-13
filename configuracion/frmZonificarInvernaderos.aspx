<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmZonificarInvernaderos.aspx.cs" Inherits="configuracion_frmZonificarInvernaderos" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>

        <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
       <style>  
        .headerCssClass{  
            background-color:#c33803;  
            color:white;  
            border:1px solid black;  
            padding:4px;  
        }  
        .contentCssClass{  
            background-color:#e59a7d;  
            color:black;  
            border:1px dotted black;  
            padding:4px;  
        }  
        .headerSelectedCss{  
            background-color:#808080;  
            color:white;  
            border:1px solid black;  
            padding:4px;  
        }  
    </style>  
     <script language="javascript" type="text/javascript">
         function OnTreeClick(evt) {
             var src = window.event != window.undefined ? window.event.srcElement : evt.target
             var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
             if (isChkBoxClick) {
                 var parentTable = GetParentByTagName("table", src);
                 var nxtSibling = parentTable.nextSibling;
                 if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                 {
                     if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                     {
                         //check or uncheck children at all levels
                         CheckUncheckChildren(parentTable.nextSibling, src.checked);
                     }
                 }
                 //check or uncheck parents at all levels
                 CheckUncheckParents(src, src.checked);
             }
         }

         function CheckUncheckChildren(childContainer, check) {
             var childChkBoxes = childContainer.getElementsByTagName("input");
             var childChkBoxCount = childChkBoxes.length;
             for (var i = 0; i < childChkBoxCount; i++) {
                 childChkBoxes[i].checked = check;
             }
         }

         function CheckUncheckParents(srcChild, check) {
             var parentDiv = GetParentByTagName("div", srcChild);
             var parentNodeTable = parentDiv.previousSibling;

             if (parentNodeTable) {
                 var checkUncheckSwitch;

                 if (check) //checkbox checked
                 {
                     checkUncheckSwitch = true;
                 }
                 else //checkbox unchecked
                 {
                     var isAllSiblingsUnChecked = AreAllSiblingsUnChecked(srcChild);
                     if (!isAllSiblingsUnChecked)
                         checkUncheckSwitch = true;
                     else
                         checkUncheckSwitch = false;
                 }

                 var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                 if (inpElemsInParentTable.length > 0) {
                     var parentNodeChkBox = inpElemsInParentTable[0];
                     parentNodeChkBox.checked = checkUncheckSwitch;
                     //do the same recursively
                     CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                 }
             }
         }

         function AreAllSiblingsUnChecked(chkBox) {
             var parentDiv = GetParentByTagName("div", chkBox);
             var childCount = parentDiv.childNodes.length;
             for (var i = 0; i < childCount; i++) {
                 if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                 {
                     if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                         var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                         //if any of sibling nodes are not checked, return false
                         if (prevChkBox.checked) {
                             return false;
                         }
                     }
                 }
             }
             return true;
         }

         //utility function to get the container of an element by tagname
         function GetParentByTagName(parentTagName, childElementObj) {
             var parent = childElementObj.parentNode;
             while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                 parent = parent.parentNode;
             }
             return parent;
         }
    </script>
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
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltRancho" runat="server" meta:resourceKey="ltRanchoResource"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRanchos" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlRanchos_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </td>
                            <td>

                                <asp:Literal ID="ltDepartamento" runat="server" meta:resourceKey="ltInvernaderoResource" Text="Departamento"></asp:Literal>

                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDepartamento0" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlDepartamento0_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                *<asp:Literal ID="ltInvernadero" runat="server" meta:resourceKey="ltInvernaderoResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlInvernaderos" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlInvernaderos_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                             <td align="right">
                                 *<asp:Literal ID="ltEmpleados" runat="server" meta:resourceKey="ltEmpleadosResource"></asp:Literal>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="ddlEmpleados" runat="server" meta:resourceKey="ddlPaisResource" AutoPostBack="True" OnSelectedIndexChanged="ddlEmpleados_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           
                            <td>
                                <asp:Literal ID="ltZonificaciones" runat="server"  Text="Zonificaciones de Empleado"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlZonificacionesEmpleado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlZonificacionesEmpleado_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>   
                                <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
                            </td>
                          <td>

                              <asp:Button ID="btnEliminar" runat="server"  Text="Eliminar Zonificacion Invernadero"  Width="248px" OnClick="btnEliminar_Click" />
                          </td>
           
                            <td>
                                <asp:Button ID="btnEliminarZonEmpleado" runat="server"  Text="Eliminar Zonificacion Empleado" Width="231px" OnClick="btnEliminarZonEmpleado_Click" />
                            </td>
           
                        </tr>
                    </table>
              </asp:Panel>
           
                 
        <div>

             <table >
                    <thead>
                        <tr>
                            <th style="font-size: xx-large">ACTIVIDADES</th>                           
                        </tr>
                    </thead>
                 <tr>
                     <td>


                         <asp:CheckBoxList ID="cbxListActividades" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Font-Bold="True" Font-Size="Medium">
                         </asp:CheckBoxList>


                     </td>
                 </tr>
                </table> 
        </div>

    <div>
        <div style="width:100%; text-align:center">  
            <asp:accordion ID="Accordion1" runat="server" HeaderCssClass="headerCssClass" ContentCssClass="contentCssClass" HeaderSelectedCssClass="headerSelectedCss" FadeTransitions="true" TransitionDuration="500" AutoSize="None" SelectedIndex="1" RequireOpenedPane="False">  
                <Panes>  
                    <asp:AccordionPane ID="AccordionPane1" runat="server">  
                        <Header> <table>
                           
                                    <tr>
                                        <th>INVERNADERO: &nbsp;</th>
                                        <th>
                                            <asp:Label ID="lblInvernadero" runat="server"></asp:Label></th>
                                        <th colspan="3">&nbsp;Pasillo en Medio: &nbsp;</th>
                                        <th>
                                            <asp:Label ID="lblPasilloMedio" runat="server"></asp:Label>

                                        </th>
                                    </tr>
                              
                            </table> 
                        </Header>  
                        <Content>
                            <table style="width: 90%;">

                                    <tr>
                                        <td style="font-weight: 700" align="center">
                                            <asp:Label ID="lblSur" runat="server" Font-Bold="True" Text="SUR"></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td align="center">
                                            <asp:Label ID="lblNorte" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                 
                                </table>
                            <div style=" width:100%; height:500px; overflow: scroll;">
                                <table style="width: 100%;">
                                       <tr>
                                        <td>
                                            <asp:TreeView ID="tvInvernadero" runat="server" ShowCheckBoxes="All" onclick="OnTreeClick(event)" AfterClientCheck="CheckChildNodes();"  ></asp:TreeView>
                                        </td>
                                        <td style="background-color: lightgray" colspan="4" bgcolor="#999999">&nbsp;</td>
                                        <td align="right">
                                            <asp:TreeView ID="trPares" runat="server" ShowCheckBoxes="All" onclick="OnTreeClick(event)" AfterClientCheck="CheckChildNodes();"></asp:TreeView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </Content>  
                    </asp:AccordionPane>  
                   
                </Panes>  
            </asp:accordion>  
        </div>  
    </div>
        </div> 
     
     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>


