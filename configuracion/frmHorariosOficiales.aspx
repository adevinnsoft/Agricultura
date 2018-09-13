<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmHorariosOficiales.aspx.cs" Inherits="configuracion_frmHorariosOficiales" 
    MasterPageFile="~/MasterPage.master" ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
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
            cargar();

            $('.initialTime').live('click', function () {
           //     $('.initialTime').attr('readonly', false);
                $('#popUpHora').show();
                ctrlHoraActual = $(this);
            });
            $('.finalTime').live('click', function () {
             //   $('.finalTime').attr('readonly', false);
                $('#popUpHora').show();
                ctrlHoraActual = $(this);
            //    $('.finalTime').attr('readonly', true);
            });
            $('.fecha').live('click', function () {
            //    $('.fecha').attr('readonly', false);
                $('#popUpFecha').show();
                ctrlFechaActual = $(this);
             //   $('.fecha').attr('readonly', true);
            });

            $("#DateDemo").AnyTime_picker({
                format: "%Y-%m-%d",
                hideInput: true,
                placement: "inline",
                labelTitle: "Fecha y hora",
                labelYear: "Año",
                labelMonth: "Mes",
                labelDayOfMonth: "Día del Mes",
                labelSecond: "Segundo",
                labelHour: "Hora",
                labelMinute: "Minuto"
            });

            $("#TimeDemo").AnyTime_picker({
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
                var este = $(this);
                var errorMsg = checkTime(este);
                if (null != errorMsg && errorMsg != "") {
                    popUpAlert(errorMsg, 'error');
                    este.addClass('Error');
                    este.val('00:00').focus();
                }
                else if ((este.is('.initialTime') || este.is('.finalTime')) && $('.initialTime').length && $.trim($('.initialTime').val()).length > 0 && $('.finalTime').length && $.trim($('.finalTime').val()).length > 0) {
                    var errorMsg = sonHorasValidas($('.initialTime').val(), $('.finalTime').val())
                    if (errorMsg != null && errorMsg != "") {
                        popUpAlert(errorMsg, 'error');
                        este.addClass('Error');
                        este.val('00:00').focus();
                    } else {
                        este.removeClass('Error');
                    }
                }
                else {
                    este.removeClass('Error');
                }
            });


        });


        $(".rblRepetir").live('change', function (e) {

            validaRepeticion();
        });

        function validaRepeticion() {
            if ($('.rblRepetir input:checked').val() == 2) {
                window.console && console.log("es " + $('.rblRepetir input:checked').val());
                $('.ckDias').show();
            } else {
                $('#ckDias').hide();
                window.console && console.log('disable when  ' + $('.rblRepetir input:checked').val());
                $('.ckDias').hide();
            }
        
        }

        function asignarHora() {
            //           window.console && console.log("asigna hora");
            $(ctrlHoraActual).attr('readonly', false);
            $(ctrlHoraActual).val($("#TimeDemo").val());
            $('#popUpHora').hide();
            $(ctrlHoraActual).change();
            $(ctrlHoraActual).attr('readonly', true);
        }

        function asignarFecha() {
            //            window.console && console.log("asigna fecha");
            $(ctrlFechaActual).attr('readonly', false);
            $(ctrlFechaActual).val($("#DateDemo").val());
            $('#popUpFecha').hide();
            $(ctrlFechaActual).change();
            $(ctrlFechaActual).attr('readonly', true);
        }

        
        function cargar() {
            registerControls();
            validaRepeticion();
        }

    </script>
    
    <style type="text/css">
        .oculto { display:none; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1><asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hiddenIdHorarioOficial" runat="server" Value= "0"/>
                    <table class="index" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan=6 align="left">
                                    <h2>
                                        <asp:Literal ID="ltSub" meta:resourceKey="ltSub" runat="server" />
                                    </h2>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2">*<asp:Literal ID="ltEvento" meta:resourceKey="ltEvento" runat="server" /></td>
                                <td class="left floatnone"><asp:TextBox ID="txtEvento" meta:resourceKey="txtEvento" runat="server"  CssClass="required "/>
                                <asp:Label ID="lngEs" runat="server" meta:resourceKey="lngEs" CssClass="lengua"></asp:Label>
                                </td>
                                 <td><asp:Literal ID="ltSeTrabaja" meta:resourceKey="ltSeTrabaja" runat="server" /></td>
                                <td class="floatnone checkboxes left"><asp:CheckBox ID="chkSeTrabaja" runat="server" meta:resourceKey="chkSeTrabaja"  CssClass="required "/></td>
                                <td><asp:Literal ID="ltActivo" meta:resourceKey="ltActivo" runat="server" /></td>
                                <td class="floatnone checkboxes left"><asp:CheckBox ID="chkActivo" meta:resourceKey="chkActivo" runat="server" Text="¿Activo?"/></td>
                            </tr>
                            <tr>
               
                            <td class="left floatnone"> <asp:TextBox ID="txtEvento_EN" meta:resourceKey="txtEvento_EN" runat="server"  CssClass="required"/>
                                <asp:Label ID="lngEn" runat="server" meta:resourceKey="lngEn" CssClass="lengua"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>*<asp:Literal ID="ltFecha" meta:resourceKey="ltFecha" runat="server" /></td>
                                <td><asp:TextBox ID="txtFecha" meta:resourceKey="txtFecha" runat="server" CssClass="required fecha"/></td>
                                <td><asp:Literal ID="ltHoraInicio" meta:resourceKey="ltHoraInicio" runat="server" /></td>
                                <td><asp:TextBox ID="txtHoraInicio" meta:resourceKey="txtHoraInicio" runat="server" CssClass="initialTime timeValidate" Width="68px" /></td>
                                <td><asp:Literal ID="ltHoraFin" meta:resourceKey="ltHoraFin" runat="server" /></td>
                                <td><asp:TextBox ID="txtHoraFin" meta:resourceKey="txtHoraFin" runat="server" CssClass="finalTime timeValidate" Width="68px" /></td>
                            </tr>
                            <tr>
                                <td><asp:Literal ID="ltRepetir" meta:resourceKey="ltRepetir" runat="server" /></td>
                                <td colspan = 5><asp:RadioButtonList ID="rblRepetir" meta:resourceKey="rblRepetir" runat="server" RepeatDirection="Horizontal"  CssClass="rblRepetir"/></td>
                            </tr>
                            <tr id="divcheks">
                                <td><div ID="ocultablel"><asp:Literal ID="ltDias" meta:resourceKey="ltDias" runat="server" /></div></td>
                                <td colspan = 5><div ID="ocultablec"><asp:CheckBoxList ID="ckDias" CssClass="ckDias" runat="server"  RepeatDirection="Horizontal"/></div></td>
                            </tr>
                            <tr>
                                <td><asp:Literal ID="ltPais" meta:resourceKey="ltPais" runat="server" /></td>
                                <td colspan=2>
                                    <asp:ListBox ID="lbPais" meta:resourceKey="lbPais" runat="server" 
                                        Rows=3 SelectionMode="Multiple" AutoPostBack="True" 
                                        OnSelectedIndexChanged="lbPais_SelectedIndexChanged"/></td>
                            </tr>
                            <tr>
                            
                                <td><asp:Literal ID="ltPlantas" meta:resourceKey="ltPlantas" runat="server" /></td>
                                <td colspan=2><asp:ListBox ID="lbPlantas" meta:resourceKey="lbPlantas" 
                                        runat="server" Width="100%" Rows=10  SelectionMode="Multiple" 
                                        OnSelectedIndexChanged="lbPlantas_SelectedIndexChanged"  AutoPostBack="True" /></td>
                                <td><asp:Literal ID="ltDepto" meta:resourceKey="ltDepto" runat="server" /></td>
                                <td colspan=2><asp:ListBox ID="lbDepto" meta:resourceKey="lbDepto" runat="server" 
                                        Width="100%"  Rows=10  SelectionMode="Multiple" 
                                        OnSelectedIndexChanged="lbDepto_SelectedIndexChanged" AutoPostBack="True" /></td>
                            </tr>
                            <tr>
                                <td><asp:Literal ID="ltLider" meta:resourceKey="ltLider" runat="server" /></td>
                                <td colspan=2><asp:ListBox ID="lbLider" meta:resourceKey="lbLider" runat="server" 
                                        Width="100%"  Rows=10  SelectionMode="Multiple" 
                                        OnSelectedIndexChanged="lbLider_SelectedIndexChanged" AutoPostBack="True"/></td>
                                <td><asp:Literal ID="ltInvernadero" meta:resourceKey="ltInvernadero" runat="server" /></td>
                                <td colspan=2><asp:ListBox ID="lbInvernadero" meta:resourceKey="lbInvernadero" 
                                        runat="server" Width="100%"  Rows=10  SelectionMode="Multiple"/></td>
                            </tr>
                            <tr>
                                <td colspan="6" align="right">
                                    <br />
                                    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btnSave" />
                                    <asp:Button runat="server" ID="btnClear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                    <script type="text/javascript">
                        Sys.Application.add_load(function () {
                            cargar();

                        
                        });
                    </script>
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
                        <asp:GridView ID="gvHorarioOficial" runat="server" CssClass="gridView" DataKeyNames="idHorarioOficial"
                            OnSelectedIndexChanged="gvHorarioOficial_SelectedIndexChanged" OnPreRender="gvHorarioOficial_PreRender"
                            EmptyDataText="No existen registros" ShowHeaderWhenEmpty="True" OnRowDataBound="gvHorarioOficial_RowDataBound"
                            AutoGenerateColumns="False" meta:resourcekey="gvHorarioOficialResource1" 
                            onPageIndexChanging="gvHorarioOficial_PageIndexChanging">
                            <AlternatingRowStyle CssClass="gridViewAlt" />
                            <Columns>
                                <asp:BoundField DataField="Evento" HeaderText="Evento" SortExpression="Evento"  meta:resourcekey="BoundFieldEvento" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha"  meta:resourcekey="BoundFieldHoraFecha" />
                                <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicial" SortExpression="HoraInicio"  meta:resourcekey="BoundFieldHoraInicio" />
                                <asp:BoundField DataField="HoraFin" HeaderText="Hora Final" SortExpression="HoraFin"  meta:resourcekey="BoundFieldHoraFin" />
                                <asp:BoundField DataField="TipoRepeticion" HeaderText="Tipo de repetición" SortExpression="TipoRepeticion"  meta:resourcekey="BoundFieldRepetir" />
                                <asp:BoundField DataField="vNombre" HeaderText="Modificado por" SortExpression="vNombre"  meta:resourcekey="BoundFieldUsuario" />
                                <asp:BoundField DataField="fechaModifico" HeaderText="Fecha Modificación" SortExpression="fechaModifico"  meta:resourcekey="BoundFieldFechaModifico" />
                                <asp:TemplateField meta:resourcekey="BoundFieldSeTrabaja" SortExpression="SeTrabaja" HeaderText="Se Trabaja">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("SeTrabaja") %>' meta:resourcekey="CheckBox1Resource1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" meta:resourcekey="lblActivoGridResource1"
                                                    Text='<%# (bool)Eval("SeTrabaja")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TemplateFieldActivo" SortExpression="Activo" HeaderText="Activo">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Activo") %>' meta:resourcekey="CheckBox1Resource1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblActivoGrid" runat="server" Enabled="False" meta:resourcekey="lblActivoGridResource1"
                                                    Text='<%# (bool)Eval("Activo")==true?(string)GetLocalResourceObject("Si"):(string)GetLocalResourceObject("No") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
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
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    <div id="popUpHora" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="TimeDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarHora" value="OK" onclick="asignarHora();" style="float:none;" /></td></tr>
       </table>
    </div>
    <div id="popUpFecha" class="popUp">
       <table style="width:100%;">
          <tr><td><input type="text" id="DateDemo" /></td></tr>
          <tr><td><input type="button" id="btnSeleccionarFecha" value="OK" onclick="asignarFecha();" style="float:none;" /></td></tr>
       </table>
    </div>

</asp:Content>
