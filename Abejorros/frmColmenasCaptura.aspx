<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmColmenasCaptura.aspx.cs" Inherits="frmColmenasCaptura"
    MasterPageFile="~/MasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
	<script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
	<script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
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

        function comboInvernaderos() {
            PageMethods.comboInvernaderos(function (result) {
                comboInv = result[0];
                planta = result[1];
                $("#<%=divComboInvernaderos.ClientID %>").html(comboInv);
                $(".ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });
                ocultarOptionChosen();
                cloneComboInvernaderos();
            });
        }


        function cloneComboInvernaderos() {
            $("#<%=divComboInvernaderos.ClientID %>").html(comboInv);
            $(".divComboInvernaderos").each(function (index) {
                //if (index >= pos) {
                    $(this).html(comboInv);
                //    console.log(index);
              //  }
            });
            $(".ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });
            ocultarOptionChosen();
        }

        function ocultarOptionChosen(){
            var expreg = new RegExp("^" + planta);
            $(".ddlInvernaderos + .chosen-container div.chosen-drop .chosen-results li").each(function (index) {
                $(this).addClass("invisible");
                if (expreg.test($(this).text())) {
                    $(this).removeClass("invisible");
                }
            });
            setItemPost();
        }

        function triggers() {
            $("#ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });
            comboInvernaderos();

            $("#<%=txtNumero.ClientID %>").change(function () {
                var num = parseInt($("#<%=txtNumero.ClientID %>").val()) == 0 ? 1 : parseInt($("#<%=txtNumero.ClientID %>").val());
                if (num >= $(".folios[disabled]").length) {
                    if (num <= parseInt($("#<%=hddDisponible.ClientID %>").val())) {
                        $("#<%=txtNumero.ClientID %>").removeClass("Error");
                        if (num > numrows) {
                            for (var i = numrows; i < num; i++) {
                                addNewRow();
                            }
                            numrows = num;
                            cloneComboInvernaderos();
                        } else {
                            var dif = numrows - num;
                            numrows = num;
                            for (var i = 0; i < dif; i++) {
                                removeLastRow();
                            }

                        }
                    } else {
                        $("#<%=txtNumero.ClientID %>").addClass("Error");
                        popUpAlert("No existe stock dispoible para esta planta", "info");
                    }
                } else {
                    $("#<%=txtNumero.ClientID %>").val($(".folios[disabled]").length);
                }
                triggers();
            });


            $("#<%=divComboInvernaderos.ClientID %>").change(function () {
                var idcombo = $("#<%=divComboInvernaderos.ClientID %>").find('option:selected').val();
                $('.divComboInvernaderos').each(
                function () {
                    $("#" + $(this).attr("id")).find(".ddlInvernaderos:not(:disabled)").val(idcombo);
                }
            );
                $(".ddlInvernaderos").trigger("chosen:updated");
            });


            $(".folios").change(function () {
                var folio = $(this);
                PageMethods.exiteFolio($(this).val(), function (result) {
                    if (result == "1") {
                        $(folio).addClass("Error");
                        $(folio).addClass("repetido");
                    } else {
                        var repetido = 0;
                        $(".folios").each(function () {

                            if ($(this).val() == $(folio).val()) {
                                ++repetido;
                            }

                        });
                        if (repetido > 1) {
                            $(folio).addClass("Error");
                            $(folio).addClass("repetido");
                        } else {
                            $(folio).removeClass("Error");
                            $(folio).removeClass("repetido");
                        }
                    }
                });
            });
        }

        function changeCombo() {
            $(".ddlInvernaderos").chosen().change(function () {
                //alert($(this).val());
                if ($(this).val() == 0) {
                    $($($(this).parent().children()[1]).children()[0]).addClass("Error");
                } else {
                    $($($(this).parent().children()[1]).children()[0]).removeClass("Error");
                }
            });
        }


        var planta = "";
        var comboInv = "";
        var numrows = 1;
        var numero = 1;
        function addNewRow() {
            var TABLE = document.getElementById("tablaColmenas");
            var TROW =  document.getElementById("celda");
            //var content = TROW.getElementsByTagName("td");
            var newRow = TABLE.insertRow(-1);
            newRow.className = "celda";// TROW.attributes['class'].value;
            var newCell1 = newRow.insertCell(newRow.cells.length);
            var newCell2 = newRow.insertCell(newRow.cells.length);
            var newCell3 = newRow.insertCell(newRow.cells.length);
            var newCell4 = newRow.insertCell(newRow.cells.length);
            newCell1.className = "alineacion";
            newCell3.className = "alineacion";
            newCell4.className = "combo";
            newID = (++numrows);
            cel1 = '<label>folio ' + newID + ': </label>'
            cel2 = '<input type="text" id="folio-' + newID + '" maxlength="20" class="folios alphanumeric" />'
            cel3 = '<label>Invernadero: </label>'
            cel4 = '<div id="invernadero-' + newID + '" class="divComboInvernaderos"></div>'
            newCell1.innerHTML = cel1
            newCell2.innerHTML = cel2
            newCell3.innerHTML = cel3
            newCell4.innerHTML = cel4
        }

        function removeLastRow() {
            var TABLE = document.getElementById("tablaColmenas");
            if (TABLE.rows.length > 1) {
                TABLE.deleteRow(TABLE.rows.length - 1);
                --numero;
            }
        }

        function btnSave() {
            changeCombo();
            var registrar = true;
            $('.requerid').each(function () {
                if ($(this).val() == '') {
                    registrar = false;
                    $(this).css({ 'border': '1px solid red' });
                }
                else {
                    $(this).css({ 'border': '1px solid black' });
                }
            });

            if (registrar) {
                var repetidos = true;

                $('.repetido').each(function () {
                        repetidos = false;
                });

                if (repetidos) {

                    if ($("#disponible").text() != 0) {
                        if ($('#tablaColmenas >tbody >tr').length) {
                            $.blockUI();
                            var filas = $('#tablaColmenas >tbody >tr').length
                            var folios = "";
                            var invernaderos = "";
                            var acciones = "";
                            var semanasentre = $("#<%=txtSemanas.ClientID %>").val();
                            var mantenimientos = $("#<%=txtMtto.ClientID %>").val();
                            for (var i = 1; i <= filas; i++) {
                                if ($("#folio-" + i).val() != 0 && $("#invernadero-" + i).find('option:selected').val() != 0 /*&& $("#semanasentre-" + i).val() != 0 && $("#mantenimeintos-" + i).val() != 0*/ && !$("#folio-" + i).hasClass("Save")) {
                                    folios += '|' + $("#folio-" + i).val();
                                    invernaderos += '|' + $("#invernadero-" + i).find('option:selected').val();
                                    acciones += '|' + ($("#folio-" + i).attr("disabled") ? "0" : "1");

                                    $("#folio-" + i).addClass("Save");

                                    if ($("#folio-" + i).hasClass("Error")) {
                                        $("#folio-" + i).removeClass("Error");
                                    }
                                    $("#folio-" + i).attr("disabled", "");

                                    if ($($($("#invernadero-" + i).children()[1]).children()[0]).hasClass("Error")) {
                                        $($($("#invernadero-" + i).children()[1]).children()[0]).removeClass("Error");
                                    }
                                    $("#invernadero-" + i).find('.ddlInvernaderos').prop('disabled', true).trigger("chosen:updated");

                                } else {
                                    if (!$("#folio-" + i).hasClass("Save")) {
                                        if ($("#folio-" + i).val() == ""){$("#folio-" + i).addClass("Error");}
                                        if ($("#invernadero-" + i).find('option:selected').val() == 0) { $($($("#invernadero-" + i).children()[1]).children()[0]).addClass("Error"); }
                                    }
                                }
                            }

                            if (folios != "") {
                                PageMethods.guardaColmenas(folios, invernaderos, acciones, semanasentre, mantenimientos, saveCallback);
                            } else {
                                $.unblockUI();
                                popUpAlert("No hay colmenas que guardar", "info");
                            }
                        }
                    } else {
                        popUpAlert("No no se puede guardar ninguna colmena, no hay stock suficiente", "info");
                    }
                }
                else {
                    popUpAlert('Existen folios previamente guardados o repetidos', 'error');
                }
            }
            else {
                popUpAlert('Por favor llene los campos requeridos', 'error');
            }
        }

        function saveCallback(result) {
            try {
                $.unblockUI();
                PageMethods.gvColmenas(function (result) {
                    $("#<%=divGridView.ClientID %>").html(result);
                    registerControls();
                });

                PageMethods.stockColmenas(function (result) {
                    $("#disponible").text(result);
                    $("#<%=hddDisponible.ClientID %>").val(result);
                });

                popUpAlert(result[1], result[0]);
            } catch (err) { }
        }

        function showColmena(fila, idColmenas, btnClean) {
            PageMethods.cargaColmenas(idColmenas, function (result) {
                $("#tablaColmenas").html(result[0]);
                planta = result[1];
                $("#<%=txtMtto.ClientID %>").val($(fila).attr("mantenimientos"));
                $("#<%=txtSemanas.ClientID %>").val($(fila).attr("semanas"));
                $(".folios").attr('disabled', '');
                $("#<%=txtNumero.ClientID %>").val($("#tablaColmenas tr").length);
                numrows = $("#tablaColmenas tr").length;
                //comboInvernaderos(cloneComboInvernaderos, setItemPost);
                comboInvernaderos();
                setItemPost();
                $("#save").css("display", $(fila).attr("bloqueo") == "1" ? "none" : "block");
                //$(".divComboInvernaderos").html(result);
                //$(".ddlInvernaderos").chosen({ no_results_text: "No se encontró el invernadero: ", width: '150px', placeholder_text_single: "--Seleccione un invernadero--" });
            });

            PageMethods.stockColmenas(function (result) {
                $("#disponible").text(result);
                $("#<%=hddDisponible.ClientID %>").val(result);
            });
            
//            $("#invernadero-1").find(".ddlInvernaderos").val(idInvernadero);
//            $(".ddlInvernaderos").trigger("chosen:updated");
//            $("#folio-1").val(folio);
//            $("#semana-1").val(semana);
        }

        function setItemPost() {
            $('.divComboInvernaderos').each(
                        function (index) {
                            if (index >= 0) {
                                //ids += '|' + $(this).attr('id').split('-')[1]; ;
                                $("#" + $(this).attr("id")).find(".ddlInvernaderos").val($("#" + $(this).attr("id")).attr('invernadero'));
                                $(".ddlInvernaderos").trigger("chosen:updated");
                            }
                        });
        }

        function btnClean() {
            var filas = $("#tablaColmenas tr").length;
            $("#save").css("display", "block");
            for (var i = 0; i <= filas; i++) {
                removeLastRow();
            }
            $("#<%=txtNumero.ClientID %>").val(1);
            $("#<%=txtSemanas.ClientID %>").val("");
            $("#<%=txtMtto.ClientID %>").val("");
            $("#folio-1").val("").attr("disabled", null);
            numrows = 1;
            numero = 1;
            $($(".ddlInvernaderos")[0]).val(0).trigger("chosen:updated");
            $($(".ddlInvernaderos")[1]).val(0).trigger("chosen:updated");
            $($(".ddlInvernaderos")[1]).prop('disabled', null).trigger("chosen:updated");
            $(".Save").removeClass("Save");
            //addNewRow();
            //comboInvernaderos();
        }

        $('#ctl00_ddlPlanta').live('change', function () {
            $("#<%=txtNumero.ClientID %>").removeClass("Error");
            $("#<%=txtNumero.ClientID %>").val("1");
            btnClean();
            comboInvernaderos();

            PageMethods.stockColmenas(function (result) {
                $("#disponible").text(result);
                $("#<%=hddDisponible.ClientID %>").val(result);
            });
        });



    </script>
<style type="text/css">
    .Error {
        border: 1px solid red !important;
        background: rgba(255,0,0,0.2) !important;
    }
    
    .alineacion
    {
        text-align:right !important;
    }
    
    .combo
    {
        text-align:left !important;
    }
        
    .gridView {
        min-width: 100% !important;
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
    
    .invisible{ display:none !important;}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Colmenas"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" meta:resourcekey="formResource1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="index">
                        <tr>
                            <td colspan="6" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" Text="Captura de colmenas en invernadero"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="top" id="numero">
                                <asp:Literal ID="ltNumero" runat="server" Text="*No de Colmenas:"></asp:Literal>
                            </td>
                            <td align="left" style="text-align: left;">
                                <asp:TextBox ID="txtNumero" runat="server" CssClass="nonZeroInt32 help" MaxLength="10" Width="60px"></asp:TextBox>
                            </td>
                            <td align="right" class="top">
                                <asp:Literal ID="ltDisponible" runat="server" Text="Colmenas disponibles:"></asp:Literal>
                            </td>
                            <td align="left" class="top combo" id="disponible">
                                <asp:literal ID="txtDisponible" runat="server" Text=""></asp:literal>
                            </td>
                         </tr>
                         <tr>
                            <td align="right">
                                <asp:Literal ID="ltInvernadero" runat="server" Text="Gral Invernadero"></asp:Literal>
                            </td>
                                <td align="left" style=" text-align:left;">
                                    <div ID="divComboInvernaderos" runat="server"></div>
                                </td>
                            <td align="right">
                                <asp:Literal ID="lMtto" runat="server" Text="*Mantenimientos:"></asp:Literal>
                            </td>
                            <td align="left" style=" text-align:left;">
                                <asp:TextBox ID="txtMtto" runat="server" CssClass="requerid nonZeroInt32 help" MaxLength="10" Width="60px"></asp:TextBox>
                            </td>
                             <td align="right">
                                <asp:Literal ID="ltSemana" runat="server" Text="*Semanas entre Mtto:"></asp:Literal>
                            </td>
                            <td align="left" style=" text-align:left;">
                                <asp:TextBox ID="txtSemanas" runat="server" CssClass="requerid nonZeroInt32 help" MaxLength="10"  Width="60px"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td align="left" colspan="6" style="text-align: left;">
                                <div  style=" width:100%;">
                                    <table id="tablaColmenas" style=" width:100%;" class="gridView">
                                        <tr id="celda"  class="celda">
                                            <td class="alineacion">
                                                <label>folio 1:</label>
                                            </td>
                                            <td>
                                                <input type="text" id="folio-1" maxlength="20" class="folios alphanumeric" />
                                            </td>
                                            <td class="alineacion">
                                                <label>invernadero:</label>
                                            </td>
                                            <td class="combo">
                                                <div id="invernadero-1" class="divComboInvernaderos""></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hddIdCriterio" runat="server" />
                        <asp:HiddenField ID="hddDisponible" runat="server" />
                    </td>
                    <td colspan="4">    
                        <input id="save" class="btnSave" name="grupo" type="button" value="Guardar" onclick="btnSave();"/>
                        <input id="clean" class="btnClean" name="grupo" type="button" value="Limpiar" onclick="btnClean();"/>
                    </td>
                </tr>
            </table>

               <script type="text/javascript">
                   Sys.Application.add_load(function () { registerControls(); setTooltips(); });
               </script>
                    <div class="grid">
                        <div id="pager" class="pager">
                            <img alt="first" src="../comun/img/first.png" class="first" />
                            <img alt="prev" src="../comun/img/prev.png" class="prev" />
                            <span class="pagedisplay"></span>
                            <%--<input type="text" class="pagedisplay" />--%>
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
                        <div id="divGridView" runat="server" class="" style="" />
                    </div>
                           </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
</asp:Content>
