<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="CriteriosExt.aspx.cs" Inherits="Auditorias_AuditExt_CriteriosExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="container">

     <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label2" runat="server" Text="Internal Audit Criteries" />
        </div>

        <hr/>
                 
        <table class="index2">
            <tr>
                <td colspan="4">
                    <br/>
                </td>
            </tr>

            <tr>
                <td style="text-align:right; width: 20%;">
                    <span class="CampRequerido"style="color:red">
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblQuestion" runat="server" Text="Question:"></asp:Label>
                    </span>
                </td>
                <td colspan="3">
                    <asp:DropDownList 
                        ID="cboxQuestion" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="80%" >
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td colspan="4" style="text-align:center">
                    <asp:CheckBoxList style="width:100%" runat="server" ID="checkCriteries" RepeatColumns="4"></asp:CheckBoxList>
                </td>
            </tr>

            <tr>
                <td colspan="4">
                    <br/>
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
            OnSelectedIndexChanged="gvCriteries_SelectedIndexChanged" 
            OnRowDataBound="gvCriteries_RowDataBound"
            style="width:100%;" >
                      
            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

        </asp:GridView>
         </div>
</asp:Content>

