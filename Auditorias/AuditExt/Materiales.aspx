<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Materiales.aspx.cs" Inherits="Auditorias_AuditExt_Materiales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
       <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label2" runat="server" Text="Materiales Auditorias Externas" />
        </div>

        <hr/>
        
        <asp:Label runat="server" ID="lblIDMaterial" Visible="false" Text="0"></asp:Label>

        <table class="index2">
            <tr><td style="text-align:center" colspan="5"><br />Save new Material</td></tr>
            <tr>
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblNameLevel" runat="server" Text="Planta:"></asp:Label>
                    </span>
                </td>

                <td style="width: 30%">
                    <asp:DropDownList 
                        ID="ddlPlants" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="90%"
                        OnSelectedIndexChanged="ddlPlants_SelectedIndexChanged"
                        AutoPostBack="true"
                        >
                    </asp:DropDownList>
                </td>

                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label1" runat="server" Text="Encuesta:"></asp:Label>
                    </span>
                </td>

                 <td style="width: 30%">
                    <asp:DropDownList 
                        ID="ddlEncuenta" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="90%"
                        >
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label3" runat="server" Text="Material (ES):"></asp:Label>
                    </span>
                </td>

                <td>
                    <asp:TextBox ID="txtMaterialEs" runat="server" Width="90%" ></asp:TextBox> 
                </td>

                 <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label4" runat="server" Text="Material (EN):"></asp:Label>
                    </span>
                </td>

                <td>
                    <asp:TextBox ID="txtMaterialEn" runat="server" Width="90%" ></asp:TextBox> 
                </td>
            </tr>

            <tr>
                 <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label5" runat="server" Text="¿Activo?:"></asp:Label>
                    </span>
                </td>
                <td>
                    <asp:CheckBox ID="checkActive" runat="server" Checked="true" /> 
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="6">
                    <center>
                        <asp:Button 
                                ID="btnSaveCritery" 
                                runat="server" 
                                Text="Save" 
                                OnClick="btnSaveCritery_Click"
                                CssClass="buttonHigh" /> 
                            <asp:Button 
                                ID="btnCancel" 
                                runat="server" 
                                Text="Cancel" 
                                OnClick="btnCancel_Click"  
                                CssClass="button"/>
                    </center>
                    <br/>
                </td>
            </tr>
            
        </table>
       
        <br />
        
    <table class="index2">
         <tr><td>Search by Plants</td>
                <td style="width: 30%; float:left">
                    <asp:DropDownList 
                        ID="DropDownList1" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="90%"
                        OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                        AutoPostBack="true"
                        >
                    </asp:DropDownList>
                </td>
           </tr>
    </table>
    
        <br />

        <asp:GridView 
            ID="gvCriteries"
            runat="server"
            AutoGenerateColumns="True"
            CssClass="gridView"
            HeaderStyle-CssClass="gridViewHeader"
            AlternatingRowStyle-CssClass="gridViewAlt"
            AllowSorting="False"
            EmptyDataText="No existen registros"
            EmptyDataRowStyle-CssClass="gridEmptyData"
            PageSize="30"
            AllowPaging ="true"
            OnPageIndexChanging="gvCriteries_PageIndexChanging"
            OnRowDataBound="gvCriteries_RowDataBound"
            OnSelectedIndexChanged="gvCriteries_SelectedIndexChanged"
            style="width:100%;" >
                      
            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

        </asp:GridView>
        </div>
</asp:Content>

