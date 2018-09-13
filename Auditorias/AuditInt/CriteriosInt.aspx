<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="CriteriosInt.aspx.cs" Inherits="Auditorias_AuditInt_CriteriosInt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label2" runat="server" Text="Internal Audit Criteries" />
        </div>

        <hr/>

        <asp:Label runat="server" ID="lblIDCritery" Visible="false" Text="0"></asp:Label>
                 
        <table class="index2">
            <tr>
                <td colspan="4">
                    <br/>
                </td>
            </tr>

            <tr>
                <td style="text-align:right; width: 15%">
                    <span class="CampRequerido"style="color:red">
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblQuestion" runat="server" Text="Question: "></asp:Label>
                    </span>
                </td>
                <td style="width: 35%">
                    <asp:DropDownList 
                        ID="cboxQuestion" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="90%" >
                    </asp:DropDownList>
                </td>
                <td style="text-align:right; width: 15%">
                    <span class="CampRequerido" style="color:red">
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblLevel" runat="server" Text="Level: "></asp:Label>
                    </span>
                </td>
                <td style="width: 35%">
                    <asp:DropDownList 
                        ID="cboxLevel" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="90%" >
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td style="text-align:right; width: 15%">
                    <span class="CampRequerido" style="color:red">
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblCriteryES" runat="server" Text="Criterio (ES):"></asp:Label>
                    </span>
                </td>

                <td style="width: 35%">
                    <asp:TextBox ID="txtCriteryES" runat="server" Width="90%" />
                </td>

                <td style="text-align:right; width: 15%">
                    <span class="CampRequerido" style="color:red">
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblCriteryEN" runat="server" Text="Critery (EN):" />
                    </span> 
                </td>

                <td style="width: 35%">
                    <asp:TextBox ID="txtCriteryEN" runat="server" Width="90%" ></asp:TextBox> 
                </td>
            </tr>

            <tr>
                <td style="text-align:right; width: 15%">
                    <span class="CampRequerido" style="color:red">
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblValueCritery" runat="server" Text="Critery Value:"/>
                    </span> 
                </td>

                <td style="width: 35%">
                    <asp:TextBox ID="txtValueCritery" runat="server" Type="number" step="any" Text="0" Width="90%"> </asp:TextBox>  
                </td>

                <td style="text-align:right">
                    <span>
                        <asp:Label ID="lbActive" runat="server" Text="Active:"></asp:Label>
                    </span>  
                </td>
                <td>
                    <asp:CheckBox ID="checkActive" runat="server" Checked="true" /> 
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

        <table class="index2" style="text-align:center">
            <tr>
                <td>
                    <asp:Label ID="LabelFilter" runat="server" Text="Seleccione Filtro: "></asp:Label>
                </td>
                <td style="width: 50%">
                    <asp:DropDownList 
                        ID="DDLFilter" 
                        runat="server"
                        CssClass="listMain"
                        Height="30px"
                        Width="90%" 
                        OnSelectedIndexChanged ="DDLFilter_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
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
            style="width:100%; margin:0px auto;" >
                      
            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

        </asp:GridView>
        </div>

</asp:Content>

