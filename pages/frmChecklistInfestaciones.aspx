<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmChecklistInfestaciones.aspx.cs"
    Inherits="frmChecklistInfestaciones" MasterPageFile="~/MasterPage.master" ValidateRequest="false"
    EnableEventValidation="false" MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../comun/css/tooltipster.css" />
    <script type="text/javascript" src="../comun/scripts/jquery.tooltipster.min.js"></script>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/chosen.jquery.js" type="text/javascript"></script>
    <script src="../comun/scripts/bootstrap-slider.min.js" type="text/javascript"></script>
    <link href="../comun/css/bootstrap-slider.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            triggers();

            $('#ctl00_ddlPlanta').live('change', function () {
                triggers();
            });
        });

        function triggers() {
            PageMethods.tablaChecklist(function (result) {
                $("#<%=divGridView.ClientID %>").html(result);
                registerControls();
                setTooltips();
                inicioSlider();
                $(".estados").slider();
                move();
                $(".estados").change();
            });
            registerControls();
            setTooltips();
        }

        function inicioSlider() {
            $(".estados").each(function () {
                $(this).attr("data-slider-value", $(this).data("slider-ticks-vals").indexOf($(this).data("slider-value")) + 1);
                $(this).attr("data-value", $(this).data("slider-ticks-vals").indexOf($(this).data("slider-value")) + 1);
                $(this).attr("value", $(this).data("slider-ticks-vals").indexOf($(this).data("slider-value")) + 1);
            });
        }

        function move() {
            $(".estados").change(function () {
                $($(this).parent().parent().find(".slider-tick-label")).css("color", "gray");
                $($(this).parent().parent().find(".slider-tick-label")[$(this).val() - 1]).css("color", "orange");
                $(this).parent().parent().find(".slider-handle").css("background-color", $(this).data("slider-ticks-res")[$(this).val() - 1] == "True" ? "blue" : "red");
                //$(this).parent().parent().find(".slider-handle").css("background-image", $(this).data("slider-ticks-res")[$(this).val() - 1] == "True" ? "webkit-linear-gradient(top, #FF0000 0%, #FFF000 100%)" : "webkit-linear-gradient(top, #00FF00 0%, #00FFF0 100%)");
                $(this).parent().parent().find(".slider-handle").css("background-image", $(this).data("slider-ticks-res")[$(this).val() - 1] == "True" ? "linear-gradient(to bottom, #54AD45 0%, #70B973 100%)" : "linear-gradient(to bottom, #FF8100 0%, #FFB100 100%)");
            });
        }

        function btnSave() {
            $.blockUI();
            var ids = "";
            var valores = "";
            /*$('.onoffswitch-checkbox').each(
            function () {
            ids += '|' + $(this).parent().parent().parent().attr('idInfestacion'); ;
            valores += '|' + ($(this).is(":checked") ? 4 : 1);
            }
            );*/
            $('.estados').each(
                        function () {
                            ids += '|' + $(this).parent().parent().parent().attr('idInfestacion'); ;
                            valores += '|' + $(this).data("slider-ticks-vals")[$(this).val() - 1];
                        }
                    );
            PageMethods.guardaCheckList(ids, valores, function (result) {
                $.unblockUI();
                triggers();
                popUpAlert(result[1], result[0]);
            });
        }

        function btnClean() {
            $.blockUI();
            $("#<%=divGridView.ClientID %>").html("");
            $("#ddlInvernaderos").val(0);
            $(".ddlInv").trigger("chosen:updated");
            $("#chkGeneral").attr("checked", null);
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
        input.Error
        {
            border: 1px solid red !important;
            background: rgba(255,0,0,0.2);
        }
        
        input.change
        {
            border: 1px solid #65AB1B !important;
            color: #FF8400;
            font-weight: bold;
            background: white;
        }
        
        .focus
        {
            border: transparent 1px solid !important;
            background: none;
            border-style: none;
            box-shadow: none !important;
        }
        
        .focus:focus
        {
            border: 1px black solid !important;
            background: white;
            border-style: none;
        }
        
        table.gridView
        {
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
        
        .onoffswitch
        {
            position: relative;
            width: 80px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
        }
        .onoffswitch-checkbox
        {
            display: none;
        }
        .onoffswitch-label
        {
            display: block;
            overflow: hidden;
            cursor: pointer;
            border: 2px solid #F2F2F2;
            border-radius: 30px;
            margin: 0px !important;
        }
        .onoffswitch-inner
        {
            display: block;
            width: 200%;
            margin-left: -100%;
            -moz-transition: margin 0.3s ease-in 0s;
            -webkit-transition: margin 0.3s ease-in 0s;
            -o-transition: margin 0.3s ease-in 0s;
            transition: margin 0.3s ease-in 0s;
        }
        .onoffswitch-inner:before, .onoffswitch-inner:after
        {
            display: block;
            float: left;
            width: 50%;
            height: 23px;
            padding: 0;
            line-height: 25px;
            font-size: 12px;
            color: white;
            font-family: Trebuchet, Arial, sans-serif;
            font-weight: bold;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }
        .onoffswitch-inner:before
        {
            content: "Si\00a0\00a0\00a0\00a0\00a0\00a0\00a0\00a0";
            padding-left: 21px;
            background-color: #44A12D;
            color: #FFFFFF;
        }
        .onoffswitch-inner:after
        {
            content: "No";
            padding-right: 21px;
            background-color: #FF5100;
            color: #FFFFFF;
            text-align: right;
        }
        .onoffswitch-switch
        {
            display: block;
            width: 9px;
            margin: 8px;
            background: #FFC400;
            border: 1px solid #F2F2F2;
            border-radius: 50px;
            position: absolute;
            top: 0;
            bottom: 0;
            right: 50px;
            -moz-transition: all 0.3s ease-in 0s;
            -webkit-transition: all 0.3s ease-in 0s;
            -o-transition: all 0.3s ease-in 0s;
            transition: all 0.3s ease-in 0s;
        }
        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner
        {
            margin-left: 0;
        }
        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch
        {
            right: 0px;
        }
        table#tablaCheckList tr td
        {
            white-space: normal;
        }
        
        table.index tr:nth-child(3) td
        {
            text-align: left;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:label id="lblTitle" runat="server" text="Checklist"></asp:label>
        </h1>
        <table class="index">
            <tr>
                <td colspan="4" align="left">
                    <h2>
                        <asp:literal id="ltSubtituli" runat="server" text="Capture o edite el checklist de Infestaciones">
                        </asp:literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div id="pager" class="pager" style="display: none; width: 100%; min-width: 100%;">
                        <img alt="first" src="../comun/img/first.png" class="first" />
                        <img alt="prev" src="../comun/img/prev.png" class="prev" />
                        <span class="pagedisplay" style="top: -4px; position: relative;"></span>
                        <img alt="next" src="../comun/img/next.png" class="next" />
                        <img alt="last" src="../comun/img/last.png" class="last" />
                        <select class="pagesize cajaCh" style="width: 50px; min-width: 50px; max-width: 50px;
                            top: -4px; position: relative;">
                            <option value="10">10</option>
                            <option value="20">20</option>
                            <option value="30">30</option>
                            <option value="40">40</option>
                            <option value="50">50</option>
                        </select>
                    </div>
                    <div id="divGridView" runat="server" class="" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input id="save" class="btnSave" name="grupo" type="button" value="Guardar" onclick="btnSave();" />
                    <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();" />
                </td>
            </tr>
        </table>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:content>
