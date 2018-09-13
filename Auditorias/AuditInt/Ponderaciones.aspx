<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Ponderaciones.aspx.cs" Inherits="Auditorias_AuditInt_Ponderaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
     <div style="width:100%; text-align:center;" class="TituloForma">
                        <asp:Label ID="Label2" runat="server" Text="Weightings (Ponderaciones)" />
                    </div> 
                            <hr class="hr_2"/>
                    <table class="index2">
                        <tr>
                            <td style="width: 91px"></td>
                            <td></td>
                            <td class="cajaMed" style="width: 91px"></td>
                            <td></td>  
                        </tr>
                        <tr>
                            <td style="width: 91px" > 
                                <span>
                                    <asp:Label ID="lblName" runat="server" Text="Nombre" />
                                </span> 
                            </td>
                            <td >
                                <asp:TextBox ID="txtName" runat="server"  >
                                </asp:TextBox>  
                            </td>
                            <td style="width: 91px">
                                <asp:Label ID="lblDescription" runat="server" Text="Descripción" ></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" ></asp:TextBox>
                            </td> 
                        </tr>
                        <tr>
                            <td style="width: 91px" > 
                                <span>
                                    <asp:Label ID="lblNameEN" runat="server" Text="Name" />
                                </span> 
                            </td>
                            <td >
                                <asp:TextBox ID="txtNameEN" runat="server"  >
                                </asp:TextBox>  
                            </td>
                            <td style="width: 91px">
                                <asp:Label ID="lblDescriptionEN" runat="server" Text="Description" ></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescriptionEN" runat="server" ></asp:TextBox>
                            </td> 
                        </tr>
                        <tr>
                            <td style="width: 91px">
                                <asp:Label ID="lblWeightings" runat="server" Text="Value" ></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnMenos" runat="server" Text="-" OnClick="btnMenos_Click" />&nbsp;
                                <asp:TextBox ID="txtWeightings" runat="server" width="30px" Text="0" style="text-align:center;" ></asp:TextBox>&nbsp;
                                <asp:Button ID="btnMas" runat="server" Text="+" OnClick="btnMas_Click" />
                            </td>
                            <td style="width: 91px">
                                <asp:Label ID="Label1" Text="Active" runat="server"></asp:Label>
                            </td> 
                            <td>
                                <asp:Checkbox runat="server" ID="chkActive" TextAlign="Right" Text="Active" Width="212px"></asp:Checkbox>
                            </td>
                        </tr>
                        <tr>
                            <td class="cajaMed" style="width: 91px" ><asp:Label ID="lblIdPonderacion" runat="server" Text="" Visible="false" ></asp:Label></td> 
                               <td colspan="2"> 
                                   <asp:Button 
                                       ID="btnSaveUser" 
                                       runat="server" 
                                       Text="Save"  
                                       CssClass="buttonHigh" OnClick="btnSaveUser_Click"/> 
                                   <asp:Button 
                                       ID="btnCancel" 
                                       runat="server" 
                                       Text="Cancel"   
                                       CssClass="button" OnClick="btnCancel_Click"/>
                               </td>
                        </tr>         
                    </table>  
                    <br />
                    <br />
                    <br />
                    <asp:GridView 
                        ID="gvWeightings" runat="server"  style="width:100%"
                        AutoGenerateColumns="True" 
                        CssClass="gridView"                                        
                        HeaderStyle-CssClass="gridViewHeader" 
                        AlternatingRowStyle-CssClass="gridViewAlt" 
                        AllowSorting="False" 
                        EmptyDataText="No existen ponderaciones registradas"
                        EmptyDataRowStyle-CssClass="gridEmptyData" 
                        PageSize ="30" 
                        AllowPaging  ="true"
                        OnRowDataBound="gvWeightings_RowDataBound"
                        OnSelectedIndexChanged="gvWeightings_SelectedIndexChanged" OnPageIndexChanged="gvWeightings_PageIndexChanged" OnPageIndexChanging="gvWeightings_PageIndexChanging">
                    <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
                    </asp:GridView>                    
        </div>
</asp:Content>

