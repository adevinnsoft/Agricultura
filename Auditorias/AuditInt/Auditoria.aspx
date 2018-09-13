<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Auditoria.aspx.cs" Inherits="Auditorias_AuditInt_Auditoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../comun/scripts/jquery-ui.js"></script>
    <link href="../../comun/css/jquery-ui-1.8.21.custom.css" rel="stylesheet" />
     <script type="text/javascript">
        $(function () {
                $("#<%=stDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd', maxDate: 0 });
                $("#<%=endDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd', maxDate: 0 });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div style="width:100%; text-align:center;" class="TituloForma">
        <asp:Label ID="Label1" runat="server" Text="Internal Audits" />
    </div>

    <hr/>

    <table class="index">
        <tr><td><br /></td></tr>
        <tr>

             <td>Planta:</td>
                <td colspan="3">
                    <asp:DropDownList 
                        ID="ddlPlanta" 
                        CssClass="listMain"
                        runat="server" 
                        Style="float: left" 
                        Height="30px"
                        Width="100%" 
                        EnableViewState = "True">
                    </asp:DropDownList>
                </td>

        </tr>
             
        <tr>
            <td>Fecha Inicio:</td>
                <td>
                    <asp:TextBox ID="stDate" runat="server" style="text-align:center"></asp:TextBox>
                </td>

                <td>Fecha Fin:</td>
                <td>
                    <asp:TextBox ID="endDate" runat="server" style="text-align:center"></asp:TextBox>
                </td>

            <td colspan="4">
                <br />
                 <br />
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <center >
                    <asp:Button 
                            ID="btnBuscar" 
                            runat="server" 
                            Text="Buscar" 
                            CssClass="success"
                            BackColor="#ffcc00" 
                            ForeColor="#006600"
                            OnClick="btnBuscar_Click"/> 
                        <asp:Button 
                            ID="btnCancelar" 
                            runat="server" 
                            Text="Cancelar"   
                            CssClass="button" 
                            BackColor="#ffcc00"
                            ForeColor="#800000"
                            OnClick="btnCancelar_Click"/>
                </center>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
    </table>
    <br />

     <asp:GridView ID="gvAuditInt" runat="server"
            AutoGenerateColumns="True" 
            CssClass="gridView"
            HeaderStyle-CssClass="gridViewHeader"
            AlternatingRowStyle-CssClass="gridViewAlt"
            AllowPaging="true"
            AllowSorting="false"
            PageSize="20"
            EmptyDataText="No existen registros"
            EmptyDataRowStyle-CssClass="gridEmptyData"
            OnPageIndexChanging="gvAuditInt_PageIndexChanging"
            OnRowDataBound="gvAuditInt_RowDataBound"
            BorderStyle="Inset"
            Width="90%">
         <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
        </asp:GridView>



    
     <script type="text/javascript">

                //Remuevo la clase active de riego y auditoria
                $('#riego').removeClass('active');
                $('#feno').removeClass('active');
   
                //agrego active a fenologia
                $('#audit').addClass('active');

     </script>
        </div>
</asp:Content>

