<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCostoVariedadExtraordinario.aspx.cs" Inherits="RH_frmCostoVariedadExtraordinario" ValidateRequest="false" EnableEventValidation="false" %>


<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>

    <script type="text/javascript" src="../comun/Scripts/jsColor.js"></script>
     <link href="../comun/scripts/anytime.5.1.1.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/anytime.5.1.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>

    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script src="../comun/scripts/inputValidations.js" type="text/javascript"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
     <style>  
        .headerCssClass{  
            background-color:#c33803;  
            color:white;  
            border:1px solid black;  
            padding:4px;  
        }  
        .contentCssClass{  
            background-color:#e59a7d;  
            color:black;  
            border:1px dotted black;  
            padding:4px;  
        }  
        .headerSelectedCss{  
            background-color:#808080;  
            color:white;  
            border:1px solid black;  
            padding:4px;  
        }  
    </style>  

    <script language="javascript" type="text/javascript">
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    checkUncheckSwitch = true;
                }
                else //checkbox unchecked
                {
                    var isAllSiblingsUnChecked = AreAllSiblingsUnChecked(srcChild);
                    if (!isAllSiblingsUnChecked)
                        checkUncheckSwitch = true;
                    else
                        checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsUnChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
    </script>

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
    <script type = "text/javascript">
        function functionx(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                alert("Allow Only Numbers");
                return false;
            }
        }
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                else
                    return false;
            }

            if (txt.value.indexOf(".") > 0) {
                var txtlen = txt.value.length;
                var dotpos = txt.value.indexOf(".");
                //Change the number here to allow more decimal points than 2
                if ((txtlen - dotpos) > 2)
                    return false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode <= 90 && charCode >= 65) || (charCode <= 122 && charCode >= 97) || charCode == 8)
                return true;

            return false;

        }
        </script>
    

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">

            <asp:HiddenField ID="hdnIdPlanta" runat="server" />
            <table class="index">
                <tr>
                    <td colspan="4" align="left">
                        <h2>
                            <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltRancho" runat="server" meta:resourceKey="ltRanchoResource"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRanchos" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlRanchos_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>*<asp:Literal ID="ltInvernadero0" runat="server" meta:resourceKey="ltInvernaderoResource"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlInvernaderos" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlInvernaderos_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="center">
                    <td align="left">
                        <asp:Literal ID="ltDepartamento" runat="server" meta:resourceKey="ltDepartamentoResource"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="ddlDepartamento0" runat="server" AutoPostBack="True" meta:resourceKey="ddlPaisResource" OnSelectedIndexChanged="ddlDepartamento0_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Literal ID="ltActividad0" runat="server" meta:resourceKey="ltActividadResource"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlActividad0" runat="server" meta:resourceKey="ddlPaisResource">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="right">
                    <td align="right">
                        <asp:Literal ID="ltCantidad1" runat="server" meta:resourceKey="ltCantidadResource"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:TextBox ID="txtCantidad0" runat="server" meta:resourceKey="txtNombreCortoResource" Width="81px" onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:Literal ID="ltUnidadMedida0" runat="server" meta:resourceKey="ltUnidadMedidaResource"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="ddlUnidad0" runat="server" meta:resourceKey="ddlPaisResource">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td align="right">
                        <asp:Literal ID="ltCostoTarea" runat="server" meta:resourceKey="ltCostoTareaResource"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:TextBox ID="txtCosto" runat="server" meta:resourceKey="txtNombreCortoResource" Width="96px" onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:Literal ID="ltActivo" runat="server" meta:resourcekey="ltActivoResource1"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:CheckBox ID="chkActivo" runat="server" Checked="True" meta:resourcekey="chkActivoResource1" />
                    </td>
                </tr>
                <tr>
                    <td align="right">*<asp:Literal ID="ltFechaPlantacion" runat="server"  Text="Fecha Inicio"></asp:Literal>
                            </td>
                            <td align="right" class="auto-style5">
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="required fecha"  />
                            </td>
                            <td align="right">*<asp:Literal ID="ltHarvestDate" runat="server" Text="Fecha FIn" ></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="required fecha" meta:resourceKey="txtFecha" ></asp:TextBox>
                            </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    <td>
                        <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Cancelar" />
                    </td>
                    <td>
                        <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
                    </td>
                    <td>&nbsp;</td>

                    <td>&nbsp;</td>

                </tr>
            </table>
        </asp:Panel>


        <table>
            <tr>
                <td>
                    <div style="width:400px; text-align:center">  
            <asp:accordion ID="Accordion1" runat="server" HeaderCssClass="headerCssClass" ContentCssClass="contentCssClass" HeaderSelectedCssClass="headerSelectedCss" FadeTransitions="true" TransitionDuration="500" AutoSize="None" SelectedIndex="1" RequireOpenedPane="False">  
                <Panes>  
                    <asp:AccordionPane ID="AccordionPane1" runat="server">  
                        <Header> <table>
                           
                                    <tr>
                                        <th>INVERNADERO</th>
                                        <th>
                                            <asp:Label ID="lblInvernadero" runat="server"></asp:Label></th>
                                        <th colspan="3">Pasillo en Medio</th>
                                        <th>
                                            <asp:Label ID="lblPasilloMedio" runat="server"></asp:Label>

                                        </th>
                                    </tr>
                              
                            </table> 
                        </Header>  
                        <Content>
                            <table style="width: 90%;">

                                    <tr>
                                        <td style="font-weight: 700" align="center">
                                            <asp:Label ID="lblSur" runat="server" Font-Bold="True" Text="SUR"></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td align="center">
                                            <asp:Label ID="lblNorte" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TreeView ID="tvInvernadero" runat="server" ShowCheckBoxes="All" onclick="OnTreeClick(event)" AfterClientCheck="CheckChildNodes();"  ></asp:TreeView>
                                        </td>
                                        <td style="background-color: lightgray" colspan="4" bgcolor="#999999">&nbsp;</td>
                                        <td align="right">
                                            <asp:TreeView ID="trPares" runat="server" ShowCheckBoxes="All" onclick="OnTreeClick(event)" AfterClientCheck="CheckChildNodes();"></asp:TreeView>
                                        </td>
                                    </tr>
                                </table>
                        </Content>  
                    </asp:AccordionPane>  
                   
                </Panes>  
            </asp:accordion>  
        </div>  

                </td>

            </tr>

        </table>
        
            
          <div class="grid">
                 <div id="pager" class="pager">
                    <img alt="first" src="../comun/img/first.png" class="first" />
                    <img alt="prev" src="../comun/img/prev.png" class="prev" />
                    <input type="text" class="pagedisplay" />
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
                 <asp:GridView ID="GvPlantas" runat="server" AutoGenerateColumns="False" 
                     CssClass="gridView" Width="90%" EmptyDataText="No existen registros" 
                DataKeyNames="idCostoActividadExtraordinario" meta:resourcekey="GvPlantasResource1"  
                     onprerender="GvPlantas_PreRender" onrowdatabound="GvPlantas_RowDataBound" 
                     onselectedindexchanged="GvPlantas_SelectedIndexChanged" >
                    <AlternatingRowStyle CssClass="gridViewAlt" />

                    <Columns>
                       
                    <asp:BoundField DataField="NombrePlanta" SortExpression="NombrePlanta" 
                             meta:resourcekey="BoundFieldResource1" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                        <asp:BoundField DataField="Invernadero" SortExpression="Invernadero" 
                             meta:resourcekey="BoundFieldResourceInvernadero" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreDepartamento" SortExpression="NombreDepartamento" 
                             meta:resourcekey="BoundFieldResource2" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField DataField="NombreHabilidad" SortExpression="NombreHabilidad" 
                             meta:resourcekey="BoundFieldResource3" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="Cantidad" SortExpression="Cantidad" 
                             meta:resourcekey="BoundFieldResource4" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UnidadMedida"   meta:resourcekey="BoundFieldResource5"
                            SortExpression="UnidadMedida" >
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Costo" SortExpression="Costo" 
                             meta:resourcekey="BoundFieldResourceCodigo" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="FechaInicio" SortExpression="FechaInicio" HeaderText="Fecha Inicio">
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                         <asp:BoundField DataField="FechaFin" SortExpression="FechaFin"  HeaderText="Fecha Fin" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Activo" SortExpression="Activo" 
                             meta:resourcekey="BoundFieldResource6" >
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                      
                    </Columns>
                  
                </asp:GridView>
            </div>
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



