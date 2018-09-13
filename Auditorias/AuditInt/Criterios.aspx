<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Criterios.aspx.cs" Inherits="Auditorias_AuditInt_Criterios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div style="width:100%; text-align:center;" class="TituloForma">
                        <asp:Label ID="Label2" runat="server" Text="Criteria (Criterios)" />
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
                                    <asp:Label ID="lblCriterio" runat="server" Text="Criterio Español" />
                                </span> 
                            </td>
                            <td >
                                <asp:TextBox ID="txtCriterio" runat="server" Width="261px"  ></asp:TextBox>  
                            </td> 
                            <td style="width: 91px" > 
                                <span>
                                    <asp:Label ID="lblCriterioEng" runat="server" Text="Criteria English" />
                                </span> 
                            </td>
                            <td >
                                <asp:TextBox ID="txtCriterioEng" runat="server" Width="261px"  ></asp:TextBox>  
                            </td>                            
                        </tr>
                        
                        <tr>
                            <td style="width: 91px">
                                <asp:Label ID="lblEncuesta" runat="server" Text="Survey" ></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEncuesta" runat="server" AutoPostBack="True" Height="25px" Width="262px" OnSelectedIndexChanged="ddlEncuesta_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            <td style="width: 91px">
                                <asp:Label ID="lblPregunta" runat="server" Text="Question" ></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPregunta" runat="server" AutoPostBack="True" Height="25px" Width="262px" ></asp:DropDownList>
                            </td>                            
                        </tr>
                        <tr>
                            <td style="width: 91px">
                                <asp:Label ID="lblActivo" Text="Active" runat="server"></asp:Label>
                            </td> 
                            <td>
                                <asp:Checkbox runat="server" ID="chkActivo" TextAlign="Right" Text="Active" Width="212px"></asp:Checkbox>
                            </td> 
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
                                       CssClass="button" OnClick="btnCancel_Click"/></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="cajaMed" style="width: 91px" ><asp:Label ID="lblIdCriterio" runat="server" Visible="False" ></asp:Label><asp:Label ID="lblIdEncuesta" runat="server" Visible="False" ></asp:Label></td>                                
                        </tr>         
                    </table>  
                    <br />
                    <br />
                    <br />
                    <asp:GridView 
                        ID="gvCriteria" runat="server" style="width:100%"
                        AutoGenerateColumns="True" 
                        CssClass="gridView"                                        
                        HeaderStyle-CssClass="gridViewHeader" 
                        AlternatingRowStyle-CssClass="gridViewAlt" 
                        AllowSorting="False" 
                        EmptyDataText="No existen criterios registrados"
                        EmptyDataRowStyle-CssClass="gridEmptyData" 
                        PageSize ="50" 
                        AllowPaging  ="true" OnPageIndexChanging="gvCriteria_PageIndexChanging" OnRowDataBound="gvCriteria_RowDataBound" OnSelectedIndexChanged="gvCriteria_SelectedIndexChanged">
                    <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
                    </asp:GridView>   
        </div>
</asp:Content>

