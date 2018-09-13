<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Encuestas.aspx.cs" Inherits="Auditorias_AuditInt_Encuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div style="width:100%; text-align:center;" class="TituloForma">
            <asp:Label ID="Label2" runat="server" Text="Surveys (Encuestas)" />
        </div>

        <hr/>

        <asp:Label ID="idEncuesta" runat="server" Text="0" visible="false"></asp:Label> 
        <asp:Label ID="Label7" runat="server" Text="nada" visible="false"></asp:Label>    
                 
        <table class="index2" border="0">
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
                        <asp:Label ID="Label1" runat="server" Text="Encuesta (ES):"></asp:Label>
                    </span>
                </td>

                 <td style=" float:left; padding:5px">
                    <asp:TextBox ID="txtEncuestaEs" runat="server"> </asp:TextBox>  
                </td>

                <td style="text-align:right">
                    <span class="CampRequerido" style="color:red" >
                        <strong>*</strong>
                    </span>
                    <span>
                        <asp:Label ID="lblColor" runat="server" Text="Survey (EN):"/>
                    </span>
                </td>

                <td style=" float:left; padding:5px">
                    <asp:TextBox ID="txtEncuestaEn" runat="server"> </asp:TextBox>  
                </td>

            </tr>

            <tr>
                <td style="text-align:right">
                    
                    <span>
                        <asp:Label ID="Label3" runat="server" Text="Descripción (ES):"></asp:Label>
                    </span>
                </td>

                <td style="float:left; padding:5px">
                    <asp:TextBox ID="txtDescripcionEs" runat="server"></asp:TextBox> 
                </td> 

                <!-- -->
                <td style="text-align:right">
                    
                    <span>
                        <asp:Label ID="Label4" runat="server" Text="Description (EN):"></asp:Label>
                    </span>
                </td>

                <td style="float:left; padding:5px"">
                    <asp:TextBox ID="txtDescripcionEn" runat="server"></asp:TextBox> 
                </td>   
                                                                                                                                                                               
            </tr>

           <tr>

               
               <td style="text-align:right">
                    
                    <span>
                        <asp:Label ID="Label5" runat="server" Text="Modulo:"></asp:Label>
                    </span>
                </td>
               <td style="text-align:left; padding:5px" >
                <asp:DropDownList 
                    ID="ddlModulos" 
                    runat="server" 
                    CssClass="listMain"
                    Height="30px"
                    Width="75%">
                </asp:DropDownList>
            </td>

               <td style="text-align:right">
                    <span>
                        <asp:Label ID="Label6" runat="server" Text="Active?"></asp:Label>
                    </span>
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
                    <asp:CheckBoxList style="width:100%" runat="server" ID="checkPlants1" RepeatColumns="3"></asp:CheckBoxList>
                </td>
            </tr>

            <tr>
                <td colspan="4">
                    <br/>
                    <center>
                        <asp:Button 
                                ID="btnSaveEncuesta" 
                                runat="server" 
                                Text="Save" 
                                OnClick="btnSaveEncuesta_Click" 
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
            ID="gvEncuestas"
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
            style="width:90%;"
            BorderStyle="Ridge" 
            >
                      
            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>

        </asp:GridView>
      </div>

</asp:Content>

