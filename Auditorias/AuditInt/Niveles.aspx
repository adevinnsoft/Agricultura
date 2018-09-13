<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Niveles.aspx.cs" Inherits="Auditorias_AuditInt_Niveles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label2" runat="server" Text="Levels" />
        </div>

        <hr/>

        <asp:Label ID="idLevel" runat="server" Text="0" visible="FALSE"></asp:Label>    
                 
        <table class="index2">
            <tr>
                <td colspan="4">
                    <br/>
                </td>
            </tr>
            <tr>
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblNameLevel" runat="server" Text="Level name:"></asp:Label>
                    </span>
                </td>

                <td>
                    <asp:TextBox ID="txtNameLevel" runat="server" />
                </td>

                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblValueLevel" runat="server" Text="Level value:" />
                    </span>
                </td>

                <td>
                    <asp:TextBox ID="txtValueLevel" runat="server" Type="number" Text="0"></asp:TextBox> 
                </td>
            </tr>

            <tr>
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblColorLevel" runat="server" Text="HEX color:"/>
                    </span>
                </td>

                <td >
                    <asp:TextBox ID="txtColorLevel" runat="server" class="jscolor" value="000000"> </asp:TextBox>  
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
                                ID="btnSaveLevel" 
                                runat="server" 
                                Text="Save" 
                                OnClick="btnSaveLevel_Click" 
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
            ID="gvLevels"
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
            OnPageIndexChanging="gvLevels_PageIndexChanging"
            OnSelectedIndexChanged="gvLevels_SelectedIndexChanged" 
            OnRowDataBound="gvLevels_RowDataBound"
            style="width:100%;" >
                      
            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

        </asp:GridView>
      </div>

</asp:Content>

