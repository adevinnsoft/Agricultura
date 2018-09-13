<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmHorarioPlanta.aspx.cs" 
    Inherits="configuracion_frmHorarioPlanta"  MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
        
    <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript">
        $(function () {
            registerControls();

            $('.initialTime').live('click', function () {
                $('#popUpHora').show();
                ctrlFechaActual = $(this);
            });
            $('.finalTime').live('click', function () {
                $('#popUpHora').show();
                ctrlFechaActual = $(this);
            });

            $("#DateTimeDemo").AnyTime_picker({
                format: "%H:%i",
                hideInput: true,
                placement: "inline",
                labelTitle: "Hora",
                labelYear: "Año",
                labelMonth: "Mes",
                labelDayOfMonth: "Día del Mes",
                labelSecond: "Segundo",
                labelHour: "Hora",
                labelMinute: "Minuto"
            });


            $(".timeValidate").live('change', function (e) {
                window.console && console.log('change');
                var este = $(this);
                if (este.val().length > 5)
                    este.val(este.val().substring(0, 4));
                var errorMsg = checkTime(este);
                if (null != errorMsg && errorMsg != "") {
                    popUpAlert(errorMsg, 'error');
                    este.addClass('Error');
                    este.val('').focus();
                }
                else if ((este.is('.initialTime') || este.is('.finalTime')) && $('.initialTime').length && $.trim($('.initialTime').val()).length > 0 && $('.finalTime').length && $.trim($('.finalTime').val()).length > 0) {
                    var errorMsg = sonHorasValidas($('.initialTime').val(), $('.finalTime').val())
                    if (errorMsg != null && errorMsg != "") {
                        popUpAlert(errorMsg, 'error');
                        este.addClass('Error');
                        este.val('').focus();
                    } else {
                        este.removeClass('Error');
                    }
                }
                else {
                    este.removeClass('Error');
                }
            });
            $('.selectAll').live('change', function () {
                var val = $(this).attr('checked') == 'checked' ? true : false;
                $(this).parent().parent().next().find('input[type="checkbox"]').attr('checked', val);
            });
        });

        function asignarHora() {
            $(ctrlFechaActual).val($("#DateTimeDemo").val());
            $('#popUpHora').hide();
            $(ctrlFechaActual).change();
        }
    </script>
    <style type="text/css">

        .style3
        {
            height: 10px;
        }
        .oculto
        {
            display:none;
            }
        .style4
        {
            width: 502px;
        }
        .style5
        {
            height: 10px;
            width: 431px;
        }
        .style6
        {
            width: 431px;
        }
        .style7
        {
            height: 10px;
            width: 502px;
        }
        .style8
        {
            width: 152px;
        }
        .style9
        {
            height: 10px;
            width: 152px;
        }
        
         .popUp
        {
            background: rgba(0,0,0,.1);
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            text-align: center;
            padding-top: 5%;
            z-index:99999999;
            display:none;

        }
        input.Error {
            border: 1px solid red;
            background: rgba(255,0,0,0.2);
        }

    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1><asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hiddenIdHorarioPlanta" Value="0" runat="server" />
                    <asp:HiddenField ID="hiddenIdPlanta" Value="0" runat="server" />
                    <table class="index" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan=4 align="left">
                                <h2>
                                    <asp:Literal ID="ltHorarioPlanta" meta:resourceKey="ltHorarioPlanta" runat="server" />
                                </h2>
                            </td>
                        </tr>
                        <tr>
                           <td><asp:Label ID="lblSelectAll" runat="server" meta:resourceKey="lblSelectAll"></asp:Label></td>
                            <td>
                                <input type="checkbox" class="selectAll" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan = "2" class="top">*<asp:Literal ID="ltPlanta" meta:resourceKey="ltPlanta" runat="server"/></td>
                            <td rowspan = "2" class="top"><asp:CheckBoxList ID="cblPlanta" meta:resourceKey="cblPlanta" runat="server" CssClass="required" /> </td>

                            <td class="top">*<asp:Literal ID="ltDiaInicio" meta:resourceKey="ltDiaInicio" runat="server"/></td>
                            <td class="top"><asp:DropDownList ID="dplDiaInicio" meta:resourceKey="dplDiaInicio" runat="server"   CssClass="required"/>
                                
                            </td>
                        </tr>
                        <tr>
                           
                            <td ><asp:Literal ID="ltColmena" runat ="server" meta:resourceKey="ltColmena"/> </td>
                            <td><asp:CheckBox ID="chkColmena" runat="server" CssClass="required"/></td>
                        </tr>
                        <tr>
                            <td>*<asp:Literal ID="ltHoraInicio" meta:resourceKey="ltHoraInicio" runat="server" /></td>
                            <td><asp:TextBox ID="txtHoraInicio" meta:resourceKey="txtHoraInicio" runat="server" CssClass="initialTime required timeValidate" Width="66px"/></td>

                            <td>*<asp:Literal ID="ltHoraFin" meta:resourceKey="ltHoraFin" runat="server" /></td>
                            <td><asp:TextBox ID="txtHoraFin" meta:resourceKey="txtHoraFin" runat="server"  CssClass="finalTime required timeValidate" Width="66px"/></td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <br />
                                <asp:Button runat="server" ID="btnSave" meta:resourceKey="btnSave" OnClick="btnSave_Click" CssClass="btnSave" />
                                <asp:Button runat="server" ID="btnClear" meta:resourceKey="btnClear" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <script type="text/javascript">
                        Sys.Application.add_load(function () { registerControls(); });
                    </script>
                    <div class="grid">
                        <div id="pager" class="pager">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                                <span class="pagedisplay"></span>
                            <img alt="next" src="../comun/img/next.png" class="next" />
                            <img alt="last" src="../comun/img/last.png" class="last" />
                            <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;">
                                <option value="10">10</option>
                                <option value="20">20</option>
                                <option value="30">30</option>
                                <option value="40">40</option>
                                <option value="50">50</option>
                            </select>
                        </div>
                        <asp:GridView ID="gvHorarioPlanta" runat="server" CssClass="gridView" Width="90%" DataKeyNames="idHorarioPlanta"
                            OnSelectedIndexChanged="gvHorarioPlanta_SelectedIndexChanged" OnPreRender="gvHorarioPlanta_PreRender"
                            EmptyDataText="No existen registros" ShowHeaderWhenEmpty="True" OnRowDataBound="gvHorarioPlanta_RowDataBound"
                            AutoGenerateColumns="False" meta:resourcekey="gvHorarioPlantaResource1" 
                            onpageindexchanging="gvHorarioPlanta_PageIndexChanging">
                            <AlternatingRowStyle CssClass="gridViewAlt" />
                            <Columns>
                                <asp:BoundField DataField="NombrePlanta" HeaderText="Planta" SortExpression="NombrePlanta"  meta:resourcekey="BoundFieldPlanta" />
                                <asp:BoundField DataField="diaInicioSemana" HeaderText="Dia de Inicio de Semana" SortExpression="diaInicioSemana"  meta:resourcekey="BoundFieldDiaInicio" />
                                <asp:BoundField DataField="horaInicio" HeaderText="Hora Inicial" SortExpression="horaInicio"  meta:resourcekey="BoundFieldHoraInicio" />
                                <asp:BoundField DataField="horaFin" HeaderText="Hora Final" SortExpression="horafin"  meta:resourcekey="BoundFieldGoraFin" />       
                                <asp:BoundField DataField="aceptaColmena" HeaderText="Cosecha Colmena" SortExpression ="aceptaColmena" meta:resourceKey="BoundFieldColmena" />
                            </Columns>
                            <EmptyDataRowStyle CssClass="gridEmptyData"></EmptyDataRowStyle>
                            <HeaderStyle CssClass="gridViewHeader"></HeaderStyle>
                            <PagerStyle CssClass="gridViewPagerStyle"></PagerStyle>
                            <SelectedRowStyle CssClass="selected"></SelectedRowStyle>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div id="popUpHora" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="DateTimeDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarHora();" style="float:none;" /></td></tr>
       </table>
    </div>
    <div id="popUpFecha" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="Text1" /></td></tr>
          <tr><td><input type="button" id="Button1" value="OK" onclick="asignarHora();" style="float:none;" /></td></tr>
       </table>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>