<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="AuditoriaConfig.aspx.cs" Inherits="Auditorias_AuditInt_AuditoriaConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div style="width:100%; text-align:center;" class="TituloForma">
        <asp:Label ID="Label2" runat="server" Text="Surveys (Encuestas)" />
    </div>

    <hr/>

    <asp:Label ID="lblIdEncuesta" runat="server" Text="" Visible="false" ></asp:Label>

    <table class="index2">
        <tr>
            <td colspan="4">
                <br/>
            </td>
        </tr>

        <tr>
            <td style="text-align:right;">
                <span class="CampRequerido"style="color:red">
                    <strong>*</strong>
                </span>                              
                <span>
                    <asp:Label ID="lblNombreEs" runat="server" Text="Encuesta (ES): "></asp:Label>
                </span>   
            </td> 
            <td >
                <asp:TextBox ID="txtNombreEs" runat="server" Width="90%" ></asp:TextBox>  
            </td>                  
            <td style="text-align:right;">
                <span class="CampRequerido"style="color:red">
                    <strong>*</strong>
                </span>
                <span>
                    <asp:Label ID="lblNombreEn" runat="server" Text="Survey (EN): " />
                </span> 
            </td>
            <td >
                <asp:TextBox ID="txtNombreEn" runat="server" Width="90%" > </asp:TextBox>  
            </td>
        </tr>

        <tr>
            <td style="text-align:right;">
                <asp:Label ID="lblDescEs" runat="server" Text="Descripción (ES): " ></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDescEs" runat="server" Width="90%" ></asp:TextBox>
            </td>
            <td style="text-align:right;">
                <asp:Label ID="lblDescEn" runat="server" Text="Description (EN): " ></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDescEn" runat="server" Width="90%" ></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td style="text-align:right;">
                <span class="CampRequerido"style="color:red">
                    <strong>*</strong>
                </span>
                <asp:Label ID="lblModulos" runat="server" Text="Module: " ></asp:Label>
            </td>
            <td>
                <asp:DropDownList 
                    ID="ddlModulos" 
                    runat="server" 
                    CssClass="listMain"
                    Height="30px"
                    Width="90%" ></asp:DropDownList>
            </td>
            <td style="text-align:right;">
                <asp:Label ID="lblActivo" Text="Active" runat="server"></asp:Label>
            </td> 
            <td>
                <asp:Checkbox 
                    runat="server" 
                    ID="chkActivo"
                    Checked="true" >
                </asp:Checkbox>
            </td>
        </tr>

        <tr>
            <td colspan="4">
                <br/>
                <center>
                    <asp:Button 
                        ID="btnSaveUser" 
                        runat="server" 
                        Text="Save"  
                        CssClass="buttonHigh" OnClick="btnSaveUser_Click" /> 
                    <asp:Button 
                        ID="btnCancel" 
                        runat="server" 
                        Text="Cancel"   
                        CssClass="button" OnClick="btnCancel_Click"/>
                </center>
                <br/>
            </td>
        </tr>
    </table>

    <br />
    <br />

    <asp:GridView 
        ID="gvSurveys" 
        runat="server" 
        style="width:100%;"
        OnPageIndexChanging="gvSurveys_PageIndexChanging" 
        OnSelectedIndexChanged="gvSurveys_SelectedIndexChanged"
        AutoGenerateColumns="True" 
        CssClass="gridView"                                        
        HeaderStyle-CssClass="gridViewHeader" 
        AlternatingRowStyle-CssClass="gridViewAlt" 
        AllowSorting="False" 
        EmptyDataText="No existen encuestas registradas"
        EmptyDataRowStyle-CssClass="gridEmptyData" 
        PageSize ="30" 
        AllowPaging  ="true" 
        OnRowDataBound="gvSurveys_RowDataBound" 
        OnRowCreated="gvSurveys_RowCreated"  >

        <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

    </asp:GridView>
        </div>
</asp:Content>

