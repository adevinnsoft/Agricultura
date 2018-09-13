<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmPlagaIntervalos.aspx.cs" Inherits="configuracion_frmPlagaIntervalos"
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
    <script type="text/javascript">
        $(function () {
            registerControls();
            setTooltips();
            excel();
        });

        function excel() {
            $('.focus').change(function () {
                var id = $(this).attr('id').split('-')[1];

                $('.focus').each(
                    function () {
                        if ($("#min-" + id).val() == "") { $("#min-" + id).val(0) }
                        if ($("#max-" + id).val() == "") { $("#max-" + id).val(0) }

                        if (parseInt($("#min-" + id).val()) >= parseInt($("#max-" + id).val())) {
                            $("#min-" + id).removeClass('change');
                            $("#max-" + id).removeClass('change');
                            $("#min-" + id).addClass('Error help');
                            $("#min-" + id).attr('title', 'este intervalo debe ser menor');
                            $("#max-" + id).addClass('Error help');
                            $("#max-" + id).attr('title', 'este intervalo debe ser mayor');
                        }
                        else {
                            try {
                                $("#min-" + id).tooltipster('destroy');
                                $("#max-" + id).tooltipster('destroy');
                            } catch (e) { }
                            $("#min-" + id).removeClass('Error help');
                            $("#min-" + id).attr('title', null);
                            $("#max-" + id).removeClass('Error help');
                            $("#max-" + id).attr('title', null);
                        }
                    }
                );


                if (!$(this).hasClass('Error')) {
                 window.console && console.log('No Error');

                 $('.btnSave').val('<%=GetGlobalResourceObject("Commun","Editar")%>');
                    $('.btnClean').val('<%=GetGlobalResourceObject("Commun","Cancelar")%>');

                    $("#min-" + id).addClass('change');
                    $("#max-" + id).addClass('change');

                } else if ($("#min-" + id).hasClass('Error') && $("#min-" + id).hasClass('Error')) {
                    popUpAlert("El valor mínimo tiene que ser menor al valor máximo del intervalo.", "error");
                } else{}


                setTooltips();
            });


            var posi;
            $('#<%=divGridView.ClientID %> td').click(function () {
                var tdSelected = $(this);
                if (posi != (tdSelected.parent().index() + '' + tdSelected.index())) {
                    posi = tdSelected.parent().index() + '' + tdSelected.index();
                    
                    var inputCreated = $(tdSelected).find('input');
                    $(inputCreated).focus();
                    $(inputCreated).on('keydown', function (e) {

                        switch (e.which) {
                            case 37:
                                var tdPadre = $(this).parent();
                                tdPadre.prev().click();
                                break; //Izquierda
                            case 38:
                                var tdPadre = $(this).parent();
                                if (tdPadre.parent().prev().children().length) {
                                    tdPadre.parent().prev().children()[tdPadre.index()].click();
                                }
                                break; //Arriba
                            case 39:
                                var tdPadre = $(this).parent();
                                tdPadre.next().click();
                                break; //Derecha
                            case 40:
                                var tdPadre = $(this).parent();
                                if (tdPadre.parent().next().children().length) {
                                    tdPadre.parent().next().children()[tdPadre.index()].click();
                                }
                                break; //Abajo
                            default:

                                break;
                        }
                    });
                }
            });
        }

        function btnSave() {
            if ($('.maximo.change').length) {
                $.blockUI();
                var ids = "";
                var minimos = "";
                var maximos = "";
                $('.maximo.change').each(
                        function () { 
                            var id = $(this).attr('id').split('-')[1];
                            ids += '|' + id;
                            minimos += '|' + $("#min-" + id).val();
                            maximos += '|' + $("#max-" + id).val();
                        }
                    );
                PageMethods.guardaIntervalos(ids, minimos, maximos, saveCallback);
            } else if ($('.maximo.Error').length) {
                popUpAlert("Favor de verificar sus intervalos.", "error");
            }else{
                popUpAlert("No hay intervalos que guardar", "info");
            }
        }

        function saveCallback(result) {
            try {
                $.unblockUI();
                $(".change").removeClass('change');
                $('.btnSave').val('<%=GetGlobalResourceObject("Commun","Guardar")%>');
                $('.btnClean').val('<%=GetGlobalResourceObject("Commun","Limpiar")%>');
                popUpAlert(result[1], result[0]);
            } catch (err) { }
        }

        function btnClean() {
            $.blockUI();
            PageMethods.tablaInfestaciones(cleanCallBack);

            $('.btnSave').val('<%=GetGlobalResourceObject("Commun","Guardar")%>');
            $('.btnClean').val('<%=GetGlobalResourceObject("Commun","Limpiar")%>');
        }

        function cleanCallBack(result) {
            $("#<%=divGridView.ClientID %>").html(result);
            registerControls();
            setTooltips();
            excel();
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
        

    input[type="checkbox"], input[type="radio"]  {
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
        /*background:none;*/
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
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" meta:resourceKey="lblTitle" runat="server" Text=""></asp:Label></h1>
        <asp:Panel ID="form" runat="server">
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" meta:resourceKey="ltSubtituli" runat="server" Text=""></asp:Literal>
                                </h2>
                            </td>
                        </tr>

                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hddIdIntervalos" runat="server" />
                    </td>
                    <td colspan="2">    
                        <input id="save" class="btnSave" name="grupo" type="button" value="<%=GetLocalResourceObject("btnSave")%>" onclick="btnSave();"/>
                        <input id="clean" class="btnClean" name="grupo" type="button" value="<%=GetLocalResourceObject("btnClean")%>" onclick="btnClean();"/>
                    </td>
                </tr>
            </table>

            <div class="grid">
                <div id="pager" class="pager" style=" width:100%; min-width:100%;">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <span class="pagedisplay" style="top:-4px; position: relative;"></span>
                    <img alt="next" src="../comun/img/next.png" class="next" />
                    <img alt="last" src="../comun/img/last.png" class="last" />
                    <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px; top:-4px; position: relative;">
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="30">30</option>
                        <option value="40">40</option>
                        <option value="50">50</option>
                    </select>
                </div>
                <div ID="divGridView" runat="server" class=""/>

            </div>
        </asp:Panel>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>
