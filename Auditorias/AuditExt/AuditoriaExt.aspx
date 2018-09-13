<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="AuditoriaExt.aspx.cs" Inherits="Auditorias_AuditExt_AuditoriaExt" %>

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

    <!-- .datepicker("setDate", new Date())
        .datepicker("setDate", new Date()-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label1" runat="server" Text="External Audits" />
        </div>
        <hr/>
        <table class="index" >
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    Planta:
                </td>
                <td colspan="3">
                    <asp:DropDownList 
                        ID="ddlPlanta" 
                        runat="server" 
                        CssClass="listMain"
                        Height="30px"
                        Width="100%">
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
            </tr>
            <tr>
                <td colspan="4">
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
                        <br /><br />
                    </center>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView 
        ID="gvAuditInt"
        runat="server"
        AutoGenerateColumns="True"
        CssClass="gridView"
        HeaderStyle-CssClass="gridViewHeader"
        AlternatingRowStyle-CssClass="gridViewAlt"
        AllowSorting="false"
        EmptyDataText="No existen registros"
        EmptyDataRowStyle-CssClass="gridEmptyData"
        PageSize="20"
        AllowPaging ="true"
        OnPageIndexChanging="gvAuditInt_PageIndexChanging"
        OnRowDataBound="gvAuditInt_RowDataBound"
        style="width:100%;" >
                     
        <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

    </asp:GridView>
    </div>
</asp:Content>    


