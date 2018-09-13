<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultDatabase.aspx.cs" Inherits="pages_DefaultDatabase" Title="" MasterPageFile="~/MasterPage.master"%>
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
         
    </style>
   
      
  
    <script language="javascript" type="text/javascript">
// <![CDATA[


// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div class="column">
     <h1>Directrices Guardadas Recientemente</h1>
            
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
            <td colspan="2">&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="2">
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
                    <asp:TextBox ID="addColum" runat="server" Text="0" Width="50" MaxLength="4" CssClass="required nonZeroInt32"/>
                </td>
                <td style="width:150px;">
                    <asp:Button runat="server" ID='descargarTabla' Text='Exportar' Width="150px" onclick="save_Click"  OnClientClick="leerTabla();" />
                </td>
            </tr>
        </table>
        <%--<asp:GridView runat="server" ID="gv_Directriz" CssClass="grid"></asp:GridView>  <asp:TextBox type="text" ID="txtPlantaImportada" runat="server"/>--%>
    <table class="index">
      <tr><td colspan="4"> <asp:Button type="button" id="btn_GuardarTabla" 
              value="Guardar" runat="server" onclick="btn_GuardarTabla_Click" /></td></tr>
     </table>
    </div>
   
</div>

<uc1:popUpMessageControl runat="server" ID="popUpMessage" />
<input type="hidden" id="idDirectriz" value=""/>
    
</asp:Content>

