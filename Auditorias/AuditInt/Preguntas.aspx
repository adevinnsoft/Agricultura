<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Preguntas.aspx.cs" Inherits="Auditorias_AuditInt_Preguntas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div style="width:100%; text-align:center;" class="TituloForma">
        <asp:Label ID="Label2" runat="server" Text="Questions (Preguntas)" />
    </div> 

    <hr/>

    <asp:Label runat="server" ID="lblIDQuestion" Visible="false" Text="0"></asp:Label>

    <table class="index2">
        <tr>
            <td colspan="4">
                <br/>
            </td>
        </tr>

        <tr>
            <td style="text-align:right">
                <span class="CampRequerido"style="color:red">
                    <strong>*</strong>
                </span>
                <span>
                    <asp:Label ID="lblQuestionES" runat="server" Text="Pregunta (ES): "></asp:Label>
                </span>
            </td>

            <td>
                <asp:TextBox ID="txtQuestionES" runat="server"  width="90%" />
            </td>

            <td style="text-align:right">
                <span class="CampRequerido"style="color:red">
                    <strong>*</strong>
                </span>
                <span>
                    <asp:Label ID="lblQuestionEN" runat="server" Text="Question (EN): "></asp:Label>
                </span>
            </td>

            <td>
                <asp:TextBox ID="txtQuestionEN" runat="server" width="90%"/>
            </td>
        </tr>

        <tr>
            <td style="text-align:right">
                <span>
                    <asp:Label ID="lblDescriptionES" runat="server" Text="Descripción (ES): "></asp:Label>
                </span>
            </td>

            <td>
                <asp:TextBox ID="txtDescriptionES" runat="server"  width="90%" />
            </td>

            <td style="text-align:right">
                <span>
                    <asp:Label ID="lblDescriptionEN" runat="server" Text="Description (EN): "></asp:Label>
                </span>
            </td>

            <td>
                <asp:TextBox ID="txtDescriptionEN" runat="server" width="90%"/>
            </td>
        </tr>

        <tr>
            <td style="text-align:right">
                <span class="CampRequerido"style="color:red">
                    <strong>*</strong>
                </span>
                <span>
                    <asp:Label ID="lblSurvey" runat="server" Text="Survey: "></asp:Label>
                </span>
            </td>

            <td>
                <asp:DropDownList 
                    ID="cboxSurvey" 
                    runat="server" 
                    CssClass="listMain"
                    Height="30px"
                    Width="90%"></asp:DropDownList>
            </td>

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
                            ID="btnSaveQuestion" 
                            runat="server" 
                            Text="Save" 
                            OnClick="btnSaveQuestion_Click" 
                            CssClass="buttonHigh" /> 
                        <asp:Button 
                            ID="btnCancel" 
                            runat="server" 
                            Text="Cancel"   
                            CssClass="button" 
                            OnClick="btnCancel_Click"/>
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
    
    <asp:GridView 
        ID="gvQuestions"
        runat="server"
        style="width:100%;"
        AutoGenerateColumns="True" 
        CssClass="gridView"                                        
        HeaderStyle-CssClass="gridViewHeader" 
        AlternatingRowStyle-CssClass="gridViewAlt" 
        AllowSorting="False" 
        EmptyDataText="No existen preguntas registradas"
        EmptyDataRowStyle-CssClass="gridEmptyData" 
        PageSize ="30" 
        AllowPaging  ="true" 
        OnPageIndexChanging="gvQuestions_PageIndexChanging"
        OnSelectedIndexChanged="gvQuestions_SelectedIndexChanged"
        OnRowDataBound="gvQuestions_RowDataBound" >
        
        <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
    </asp:GridView>
        </div>

</asp:Content>

