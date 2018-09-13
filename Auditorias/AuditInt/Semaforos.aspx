<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Semaforos.aspx.cs" Inherits="Auditorias_AuditInt_Semaforos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
     <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label2" runat="server" Text="Traffic Lights" />
        </div>

        <hr/>

        <asp:Label ID="idSemaforo" runat="server" Text="0" visible="true"></asp:Label>    
                 
        <table class="index2">
            <tr>
                <td colspan="4">
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label1" runat="server" Text="Module:"></asp:Label>
                    </span>
                </td>

                <td style="width: 90%; float:left; padding:5px">
                    <asp:DropDownList 
                        ID="ddlModulo" 
                        runat="server"
                        Height="30px"
                        Width="75%" 
                        CssClass="listMain">
                    </asp:DropDownList>
                </td>

                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblColor" runat="server" Text="HEX color:"/>
                    </span>
                </td>

                <td style="width: 90%; float:left; padding:5px">
                    <asp:TextBox ID="txtColorSemaforo" runat="server" class="jscolor" value="000000"> </asp:TextBox>  
                </td>

            </tr>

            <tr>
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label3" runat="server" Text="Initial Value:"></asp:Label>
                    </span>
                </td>

                <td style="width: 90%; float:left; padding:5px">
                    <asp:TextBox ID="txtInitial" runat="server" Type="number" Text="0"></asp:TextBox> 
                </td> 

                <!-- -->
                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="Label4" runat="server" Text="Final Value:"></asp:Label>
                    </span>
                </td>

                <td style="width: 90%; float:left; padding:5px">
                    <asp:TextBox ID="txtFinal" runat="server" Type="number" Text="0"></asp:TextBox> 
                </td>   
                                                                                                                                                                               
            </tr>
            <tr>
                <td style="text-align:right">
                <span>
                    <asp:Label ID="lblActive" runat="server" Text="Active: "></asp:Label>
                </span>
            </td>

            <td>
                <asp:Checkbox runat="server" ID="checkActive" Checked="true"></asp:Checkbox>
            </td>
            </tr>

            <tr>
                <td colspan="4">
                    <br/>
                    <center>
                        <asp:Button 
                                ID="btnSaveSemaforo" 
                                runat="server" 
                                Text="Save" 
                                OnClick="btnSaveSemaforo_Click" 
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
            ID="gvSemaforos"
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
            style="width:63%;"
            BorderStyle="Ridge" 
            >
                      
            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

        </asp:GridView>
      </div>

</asp:Content>

