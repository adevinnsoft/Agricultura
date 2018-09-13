<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCumplimiento.aspx.cs" Inherits="Reportes_frmCumplimiento" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
        
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>


    <style type="text/css">
 
    .popUpStyle
    {
        font: normal 11px auto "Trebuchet MS", Verdana;   
        background-color:white;
        color:black; 
        padding:6px;     
    }
   
    .drag
    {
         background-color: #dddddd;
         cursor: move;
         border:solid 1px gray ;
    }
</style>



<script type="text/javascript" src="../comun/scripts/jquery-ui-v1.js"></script>
     


    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
   </script>

<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '80'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "../Images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "../Images/plus.png");
        $(this).closest("tr").next().remove();
    });
</script>

    
    
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
    <style type="text/css">
        .auto-style5 {
            width: 93px;
        }
        .auto-style6 {
            width: 79px;
        }
        .auto-style7 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Reporte de Cumplimiento"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdnIdPlanta" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server"  Text="Cumplimiento por invernadero"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltRancho" runat="server" Text="Rancho"></asp:Literal>
                            </td>
                            <td class="auto-style5">
                                <asp:DropDownList ID="ddlRancho" runat="server" meta:resourceKey="ddlRanchoResource" ></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">*<asp:Literal ID="ltFechaPlantacion" runat="server" Text="Sem. Inicio"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSemanaInicio" runat="server" Width="50px"  onkeypress="return isNumberKey(event)"/>
                            </td>
                            <td align="right">*<asp:Literal ID="ltHarvestDate" runat="server" Text="Sem. Fin"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:TextBox ID="txtSemanaFin" runat="server" Width="50px" onkeypress="return isNumberKey(event)"/>
                            </td>
                             <td align="right">*<asp:Literal ID="ltAnio" runat="server" Text="Año" ></asp:Literal>
                            </td>
                            <td align="right" class="auto-style6">
                                <asp:TextBox ID="txtAnio" runat="server" Width="50px" onkeypress="return isNumberKey(event)"/>
                            </td>
                            <td>
                                <asp:Button ID="btnConsulta" runat="server" OnClick="btnConsulta_Click" Text="Consultar" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td></td>
                            <td></td>
                            <td>
                                &nbsp;</td>

                        </tr>
                        
                    </table>
              </asp:Panel>
           

            <div >

                 <asp:GridView ID="grvRanchos" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                    DataKeyNames="idPlanta" OnRowDataBound="OnRowDataBoundRanchos" onprerender="grvRanchos_PreRender"  onselectedindexchanged="grvRanchos_SelectedIndexChanged" Font-Size="8pt" >
                      <AlternatingRowStyle CssClass="gridViewAlt" />
                    <Columns>                        
                        <asp:BoundField ItemStyle-Width="150px" DataField="NombrePlanta" HeaderText="Rancho" ItemStyle-HorizontalAlign ="Center" />
                        <asp:BoundField ItemStyle-Width="150px" DataField="surcosPlaneados" HeaderText="Surcos Planeados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField ItemStyle-Width="150px" DataField="SurcosTrabajados" HeaderText="Surcos Trabajados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField ItemStyle-Width="150px" DataField="cumplimiento" HeaderText="Cumplimiento" DataFormatString="{0:0.00}%" ItemStyle-HorizontalAlign="Right"/>
                    </Columns>
                </asp:GridView>
                <br />
                 <asp:GridView ID="gvActividades" runat="server" AutoGenerateColumns="false" CssClass="gridView" >
                      <AlternatingRowStyle CssClass="gridViewAlt" />
                                        <Columns>                                           
                                            <asp:BoundField ItemStyle-Width="150px" DataField="NombreHabilidad" HeaderText="Actividad" />
                                             <asp:BoundField ItemStyle-Width="150px" DataField="surcosPlaneados" HeaderText="Surcos Planeados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"/>
                                             <asp:BoundField ItemStyle-Width="150px" DataField="surcosTrabajados" HeaderText="Surcos Trabajados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"/>
                                             <asp:BoundField ItemStyle-Width="150px" DataField="cumplimiento" HeaderText="% Cumplimiento" DataFormatString="{0:0.00}%" ItemStyle-HorizontalAlign="Right"/>
                                        </Columns>
                                    </asp:GridView>
                <br />
                <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                    DataKeyNames="idInvernadero" OnRowDataBound="OnRowDataBound"  Font-Size="8pt" AlternatingRowStyle-BackColor="#C2D69B" Visible="False">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <img alt="" style="cursor: pointer" src="../Images/plus.png" />
                                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                        <Columns>                                           

                                            <asp:BoundField ItemStyle-Width="150px" DataField="NombreHabilidad" HeaderText="Actividad" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="horaInicio" HeaderText="Inicio" HtmlEncode="false" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign ="Center" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="horaFin" HeaderText="Fin" HtmlEncode="false" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign ="Center"/>
                                             <asp:BoundField ItemStyle-Width="150px" DataField="surcosPlaneados" HeaderText="Surcos Planeados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"/>
                                             <asp:BoundField ItemStyle-Width="150px" DataField="surcosTrabajados" HeaderText="Surcos Trabajados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"/>
                                             <asp:BoundField ItemStyle-Width="150px" DataField="cumplimiento" HeaderText="% Cumplimiento" DataFormatString="{0:0.00}%" ItemStyle-HorizontalAlign="Right"/>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="150px" DataField="GreenHouse" HeaderText="Invernadero" ItemStyle-HorizontalAlign ="Center" />
                        <asp:BoundField ItemStyle-Width="150px" DataField="surcosPlaneados" HeaderText="Surcos Planeados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField ItemStyle-Width="150px" DataField="SurcosTrabajados" HeaderText="Surcos Trabajados" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField ItemStyle-Width="150px" DataField="cumplimiento" HeaderText="Cumplimiento" DataFormatString="{0:0.00}%" ItemStyle-HorizontalAlign="Right"/>
                    </Columns>
                </asp:GridView>

                
            </div>
    </div>
 

<br />
    


     <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />

</asp:Content>



