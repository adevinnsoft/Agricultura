<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmChecklistColmenas.aspx.cs" Inherits="frmChecklistColmenas"
    MasterPageFile="~/MasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
	<script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
	<script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();
            setTooltips();
            triggers();
        });


        function triggers() {

            $('#ctl00_ddlPlanta').live('change', function () {
                PageMethods.comboInvernaderos(function (result) {
                    $("#<%=divComboInvernaderos.ClientID %>").html(result);
                    $("#ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });
                });
            });

            $("#ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });


            var cumple = false;
            $('#chkGeneral').change(function () {
                if (!cumple) {
                    $(".onoffswitch-checkbox").attr("checked", "");
                    cumple = true;
                }
                else {
                    $(".onoffswitch-checkbox").attr("checked", null);
                    cumple = false;
                }
            });


            $("#<%=ddlSemana.ClientID%>").change(function () {
                if ($("#ddlInvernaderos").val() != 0) {
                    PageMethods.tablaChecklist($("#<%=ddlSemana.ClientID%>").val(), $("#ddlInvernaderos").val(), function (result) {
                        $("#<%=divGridView.ClientID %>").html(result[0]);
                        $("#<%=hddIdCheck.ClientID %>").val(result[1]);
                        $("#<%=txtDescripcion_ES.ClientID %>").val(result[2]);
                        $("#<%=txtDescripcion_EN.ClientID %>").val(result[3]);
                        registerControls();
                    });
                } else {
                    $("#<%=divGridView.ClientID %>").html("");
                }
            });

            $("#ddlInvernaderos").change(function () {
                if ($("#ddlInvernaderos").val() != 0) {
                    PageMethods.tablaChecklist($("#<%=ddlSemana.ClientID%>").val(), $("#ddlInvernaderos").val(), function (result) {
                        $("#<%=divGridView.ClientID %>").html(result[0]);
                        $("#<%=hddIdCheck.ClientID %>").val(result[1]);
                        $("#<%=txtDescripcion_ES.ClientID %>").val(result[2]);
                        $("#<%=txtDescripcion_EN.ClientID %>").val(result[3]);
                        registerControls();
                    });
                } else {
                    $("#<%=divGridView.ClientID %>").html("");
                }
            });
        }

        

        function btnSave() {
                $.blockUI();
                var ids = "";
                var valores = "";
                $('.onoffswitch-checkbox').each(
                        function (index) {
                            if (index > 0) {
                                ids += '|' + $(this).attr('id').split('-')[1]; ;
                                valores += '|' + ($(this).is(":checked") ? 1 : 0);
                            }
                        }
                    );
                        PageMethods.guardaCheckList(ids, valores, $("#<%=ddlSemana.ClientID%>").val(), $("#ddlInvernaderos").val(), $("#<%=hddIdCheck.ClientID%>").val(), $("#<%=txtDescripcion_ES.ClientID%>").val(), $("#<%=txtDescripcion_EN.ClientID%>").val(), saveCallback);
        }

        function saveCallback(result) {
            try {
                $.unblockUI();
                PageMethods.tablaChecklist($("#<%=ddlSemana.ClientID%>").val(), $("#ddlInvernaderos").val(), function (result) {
                    $("#<%=divGridView.ClientID %>").html(result[0]);
                    $("#<%=hddIdCheck.ClientID %>").val(result[1]);
                    $("#<%=txtDescripcion_ES.ClientID %>").val(result[2]);
                    $("#<%=txtDescripcion_EN.ClientID %>").val(result[3]);
                    registerControls();
                    setTooltips(); 
                });
                popUpAlert(result[1], result[0]);
                $("#<%=hddIdCheck.ClientID%>").val(result[2]);
            } catch (err) { }
        }

        function btnClean() {
            $.blockUI();
            $("#<%=divGridView.ClientID %>").html("");
            $("#ddlInvernaderos").val(0);
            $(".ddlInv").trigger("chosen:updated");
            $("#chkGeneral").attr("checked", null);
            $("#<%=ddlSemana.ClientID %>").val($("#<%=hddSemanaNS.ClientID %>").val());
            $("#<%=txtDescripcion_ES.ClientID %>").val("");
            $("#<%=txtDescripcion_EN.ClientID %>").val("");
            $.unblockUI();
        }


        function setTooltips() {
            $('.help').tooltipster({
                animation: 'fade',
                delay: 100,
                theme: 'tooltipster-shadow',
                touchDevices: false,
                trigger: 'hover',
                position: 'right'
            });

            $('.tooltip').tooltipster({
                animation: 'grow',
                delay: 200,
                theme: 'tooltipster-punk',
                touchDevices: false,
                trigger: 'hover',
                contentAsHTML: true,
                interactive: true
            });
        }

        
    </script>
<style type="text/css">
    input.Error {
        border: 1px solid red !important;
        background: rgba(255,0,0,0.2);
    }
    
    input.change {
        border: 1px solid #65AB1B !important;
        color: #FF8400;
        font-weight: bold;
        background:white;
    }
    
    .focus{
        border: transparent 1px solid !important;
        background: none;
        border-style: none;
        box-shadow: none !important;
    }
    
    .focus:focus{
        border: 1px black solid !important;
        background: white;
        border-style: none;
    }
        
    table.gridView {
     min-width: 100% !important;
    }
    /* max-width: 300px; */
    /*input[type="checkbox"], input[type="radio"]  {
        display:none;
    }
    input[type="checkbox"] + label {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left 0px top no-repeat;
        cursor:pointer;
    }
    
        input[type="radio"] + label {
        display:inline-block;
        width:19px;
        height:19px;
        margin:-1px 4px 0 0;
        vertical-align:middle;
        background:url(../comun/img/check_radio_sheet.png) left -39px  top no-repeat;
        cursor:pointer;
    }
    
    input[type="checkbox"]:checked + label{
        background:url(../comun/img/check_radio_sheet.png) -19px top no-repeat;
    }
    
    input[type="radio"]:checked + label {
        background:url(../comun/img/check_radio_sheet.png) -58px top no-repeat;
    }
    
    input[type="checkbox"]:disabled + label {
        background:url(../comun/img/check_radio_sheet.png) -98px top no-repeat;
    }
    
    input[type="radio"]:disabled + label {
        background:url(../comun/img/check_radio_sheet.png) -78px top no-repeat;
    }
    
    .check-with-label:checked + .label-for-check {
        font-weight: bold;
        color:#C12929;
    }

    .check-with-label:disabled + .label-for-check {
        color:gray;
    }*/
    
    .onoffswitch {
    position: relative; width: 80px;
    -webkit-user-select:none; -moz-user-select:none; -ms-user-select: none;
}
.onoffswitch-checkbox {
    display: none;
}
.onoffswitch-label {
    display: block; overflow: hidden; cursor: pointer;
    border: 2px solid #F2F2F2; border-radius: 30px;
    margin:0px !important;
}
.onoffswitch-inner {
    display: block; width: 200%; margin-left: -100%;
    -moz-transition: margin 0.3s ease-in 0s; -webkit-transition: margin 0.3s ease-in 0s;
    -o-transition: margin 0.3s ease-in 0s; transition: margin 0.3s ease-in 0s;
}
.onoffswitch-inner:before, .onoffswitch-inner:after {
    display: block; float: left; width: 50%; height: 23px; padding: 0; line-height: 25px;
    font-size: 12px; color: white; font-family: Trebuchet, Arial, sans-serif; font-weight: bold;
    -moz-box-sizing: border-box; -webkit-box-sizing: border-box; box-sizing: border-box;
}
.onoffswitch-inner:before {
    content: "Si\00a0\00a0\00a0\00a0\00a0\00a0\00a0\00a0";
    padding-left: 21px;
    background-color: #44A12D; color: #FFFFFF;
}
.onoffswitch-inner:after {
    content: "No";
    padding-right: 21px;
    background-color: #FF5100; color: #FFFFFF;
    text-align: right;
}
.onoffswitch-switch {
    display: block; width: 9px; margin: 8px;
    background: #FFC400;
    border: 1px solid #F2F2F2; border-radius: 50px;
    position: absolute; top: 0; bottom: 0; right: 50px;
    -moz-transition: all 0.3s ease-in 0s; -webkit-transition: all 0.3s ease-in 0s;
    -o-transition: all 0.3s ease-in 0s; transition: all 0.3s ease-in 0s; 
}
.onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner {
    margin-left: 0;
}
.onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch {
    right: 0px; 
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Checklist"></asp:Label></h1>
        <asp:Panel ID="form" runat="server">
            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" Text="Capture o edite el checklist para instalaciones de abejorros."></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr >
                            <td style="width: 50px; vertical-align:top;">
                                <asp:Label ID="lbRFamilia" runat="server" Text="<%/*$ Resources: radios*/ %>">*Invernaderos:</asp:Label> 
                            </td>
                            <td ID="divComboInvernaderos" runat="server" style=" text-align: left;">
                                <div ></div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style=" vertical-align: top;">
                                <asp:Literal ID="ltSemana" runat="server" Text="Semana NS:"></asp:Literal>
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:DropDownList ID="ddlSemana" runat="server" CssClass="semana"></asp:DropDownList>
                            </td>
                        </tr>
                <tr>
                    <td rowspan="2" class="top">
                        <asp:Literal ID="ltDescripcion" runat="server" Text="Observaciones:" 
                            meta:resourcekey="ltDescripcionResource1"></asp:Literal>
                    </td>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtDescripcion_ES" CssClass="help" runat="server" 
                            TextMode="MultiLine" Rows="3" Columns="60"  
                            ToolTip="<%$ Resources:Commun, in_ES %>" 
                            meta:resourcekey="txtDescripcion_ESResource1"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_ES %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:left;">
                        <asp:TextBox ID="txtDescripcion_EN" CssClass="help" runat="server" 
                            TextMode="MultiLine" Rows="3" Columns="60"
                            ToolTip="<%$ Resources:Commun, in_EN %>" 
                            meta:resourcekey="txtDescripcion_ENResource1"></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" CssClass="lengua" 
                            Text="<%$ Resources:Commun, lt_EN %>"></asp:Label>
                    </td>
                </tr>
                <tr><td></td></tr>
                <tr>
                    <td align="right">
                                <asp:Literal ID="ltCheck" runat="server" Text="Grl Cumpliemiento:"></asp:Literal>
                            </td>
                            <td align="left" style=" text-align:left;">
                                <%--<input checked="checked"  type="checkbox" ID="chkActivo" runat="server" class="check-with-label" />
                                <label  id="Label1"  class='label-for-check' for="<%=chkActivo.ClientID %>" ><span></span></label>--%>
                                <div class='onoffswitch'>
                                <input type='checkbox' name='onoffswitch' class='onoffswitch-checkbox' id='chkGeneral'/>
                                <label class='onoffswitch-label' for='chkGeneral'>
                                <span class='onoffswitch-inner'></span>
                                <span class='onoffswitch-switch'></span>
                                </label>
                                </div>
                            </td>
                        </tr>
                <tr>
                    <td colspan="4">

                <div id="pager" class="pager" style="display:none; width:100%; min-width:100%;">
                    <%--<img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <span class="pagedisplay" style="top:-4px; position: relative;"></span>
                    <img alt="next" src="../comun/img/next.png" class="next" />
                    <img alt="last" src="../comun/img/last.png" class="last" />--%>
                    <select class="pagesize cajaCh" style="display:none; width: 50px; min-width: 50px; max-width: 50px; top:-4px; position: relative;">
                        <%--<option value="10">10</option>
                        <option value="20">20</option>
                        <option value="30">30</option>
                        <option value="40">40</option>--%>
                        <option value="50">50</option>
                    </select>
              </div> 
                        <div ID="divGridView" runat="server" class=""/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hddIdCheck" runat="server" />
                        <asp:HiddenField ID="hddSemanaNS" runat="server" />
                    </td>
                    <td colspan="2">    
                        <%--<asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="btnSave" OnClick="btnSave_Click" />--%>
                        <%--<asp:Button ID="btnClear" runat="server" Text="Cancelar" OnClick="btnClear_Click" />--%>
                        <input id="save" class="btnSave" name="grupo" type="button" value="Guardar" onclick="btnSave();"/>
                        <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();"/>
                    </td>
                </tr>
            </table>

                <%--<script type="text/javascript">
                    Sys.Application.add_load(function () { registerControls(); setTooltips(); });
                </script>
--%> 
                        
<%--                <asp:GridView ID="gvPalgaIntervalos" runat="server" AutoGenerateColumns="False" CssClass="gridView" EmptyDataText="No Data" ShowHeaderWhenEmpty="true"
                    meta:resourcekey="GridView1Resource1" DataKeyNames="idPlaga"
                    EmptyDataRowStyle-CssClass="no-results" onprerender="gvPalgaIntervalos_PreRender" 
                    onrowdatabound="gvPalgaIntervalos_RowDataBound" 
                    onselectedindexchanged="gvPalgaIntervalos_SelectedIndexChanged">
                    <AlternatingRowStyle CssClass="gridViewAlt" />
                    <Columns>
                    <asp:TemplateField SortExpression="esPlaga" HeaderText="Tipo">
                        <ItemTemplate>
                            <asp:Label ID="lblTipoGrid" runat="server" Enabled="False" 
                                Text='<%# (bool)Eval("esPlaga")==true?(string)GetGlobalResourceObject("Commun", "Si"):(string)GetGlobalResourceObject("Commun", "No") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="nombreComun" HeaderText="Nombre Comun" 
                            SortExpression="nombreComun"  
                            HeaderStyle-HorizontalAlign ="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombreCientifico" HeaderText="Nombre Cientifico" 
                            SortExpression="nombreCientifico"  
                            HeaderStyle-HorizontalAlign ="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField SortExpression="UsuarioModifica"  HeaderText="Modificó">
                        <ItemTemplate>
                            <asp:Label ID="lblUsuariogrid" runat="server" Enabled="False" CssClass="tooltip"
                                ToolTip='<%# Eval("FechaModifico") %>'
                                Text='<%# Eval("UsuarioModifica") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Activo" HeaderText="Activo">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>'  />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" 
                                Text='<%# (bool)Eval("Activo")==true?(string)GetGlobalResourceObject("Commun", "Si"):(string)GetGlobalResourceObject("Commun", "No") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Minimo" HeaderText="Minimo">
                        <ItemTemplate>
                            <asp:TextBox ID="lblActivoGrid" runat="server" Width="50px"  CssClass="requerid int32" MaxLength="5" style=" text-align: center;"
                                Text='<%# Eval("Minimo") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Maximo" HeaderText="Maximo">
                        <ItemTemplate>
                            <asp:TextBox ID="lblActivoGrid" runat="server" Width="50px"  CssClass="requerid int32" MaxLength="5" style=" text-align: center;"
                                Text='<%# Eval("MAximo") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="no-results" />
                </asp:GridView>
--%>                
            </div>
                            <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </asp:Panel>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>
