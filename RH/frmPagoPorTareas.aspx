<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmPagoPorTareas.aspx.cs" Inherits="Reportes_frmPagoPorTareas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" 
    TagPrefix="uc1" %>
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


    <style type="text/css">
 
    .popUpStyle
    {
        font: normal 11px auto "Trebuchet MS", Verdana;   
        background-color:white;
        color:black; 
        padding:6px;     
    }
   
    .drag
    {
         background-color: #dddddd;
         cursor: move;
         border:solid 1px gray ;
    }
</style>



<script type="text/javascript" src="../comun/scripts/jquery-ui-v1.js"></script>
     



    <script type = "text/javascript">

        function Check_Click(objRef) {

            //Get the Row based on checkbox

            var row = objRef.parentNode.parentNode;

            if (objRef.checked) {

                //If checked change color to Aqua

                row.style.backgroundColor = "aqua";

            }

            else {

                //If not checked change back to original color

                if (row.rowIndex % 2 == 0) {

                    //Alternating Row Color

                    row.style.backgroundColor = "#C2D69B";

                }

                else {

                    row.style.backgroundColor = "white";

                }

            }



            //Get the reference of GridView

            var GridView = row.parentNode;



            //Get all input elements in Gridview

            var inputList = GridView.getElementsByTagName("input");



            for (var i = 0; i < inputList.length; i++) {

                //The First element is the Header Checkbox

                var headerCheckBox = inputList[0];



                //Based on all or none checkboxes

                //are checked check/uncheck Header Checkbox

                var checked = true;

                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                    if (!inputList[i].checked) {

                        checked = false;

                        break;

                    }

                }

            }

            headerCheckBox.checked = checked;



        }

</script>

    <script type = "text/javascript">

        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;

            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;

                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {

                        //If the header checkbox is checked

                        //check all checkboxes

                        //and highlight all rows

                        row.style.backgroundColor = "aqua";

                        inputList[i].checked = true;

                    }

                    else {

                        //If the header checkbox is checked

                        //uncheck all checkboxes

                        //and change rowcolor back to original

                        if (row.rowIndex % 2 == 0) {

                            //Alternating Row Color

                            row.style.backgroundColor = "#C2D69B";

                        }

                        else {

                            row.style.backgroundColor = "white";

                        }

                        inputList[i].checked = false;

                    }

                }

            }

        }

</script> 
    <script type = "text/javascript">

        function MouseEvents(objRef, evt) {

            var checkbox = objRef.getElementsByTagName("input")[0];

            if (evt.type == "mouseover") {

                objRef.style.backgroundColor = "#F4D101";

            }

            else {

                if (checkbox.checked) {

                    objRef.style.backgroundColor = "aqua";

                }

                else if (evt.type == "mouseout") {

                    if (objRef.rowIndex % 2 == 0) {

                        //Alternating Row Color

                        objRef.style.backgroundColor = "#D6DFD0";

                    }

                    else {

                        objRef.style.backgroundColor = "white";

                    }

                }

            }

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
    
    <script type="text/javascript">
        $(function () {
            registerControls();
        });
    </script>
    <style type="text/css">
        .auto-style5 {
            width: 93px;
        }
        .auto-style6 {
            width: 79px;
        }
        .auto-style7 {
            width: 100%;
        }
        .auto-style8 {
            height: 17px;
        }
    </style>

    <style type="text/css">
    body
    {
        font-family: Arial;
        font-size: 10pt;
    }
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=40);
        opacity: 0.4;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        width: 800px;
        height:400px;
        border: 3px solid #0DA9D0;
    }
    .modalPopup .header
    {
        background-color: #000080;
        height: 30px;
        color: White;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
    }
    .modalPopup .body
    {
        min-height: 50px;
        line-height: 30px;
        text-align: center;
        padding:5px
    }
    .modalPopup .footer
    {
        padding: 3px;
    }
    .modalPopup .button
    {
        height: 23px;
        color: White;
        line-height: 23px;
        text-align: center;
        font-weight: bold;
        cursor: pointer;
        background-color: #9F9F9F;
        border: 1px solid #5C5C5C;
    }
    .modalPopup td
    {
        text-align:left;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" meta:resourceKey="lblTitle"></asp:Label>
        </h1>
        <asp:Panel ID="form" runat="server" meta:resourceKey="formResource1">
          
                <asp:HiddenField id="hdnIdPlanta" runat="server" />
                    <table class="index">
                        <tr>
                            <td colspan="4" align="left">
                                <h2>
                                    <asp:Literal ID="ltSubtituli" runat="server" meta:resourceKey="ltSubtituli"></asp:Literal>
                                </h2>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Literal ID="ltRancho" runat="server" Text="Rancho"></asp:Literal>
                            </td>
                            <td class="auto-style5">
                                <asp:DropDownList ID="ddlRancho" runat="server" meta:resourceKey="ddlRanchoResource" AutoPostBack="True" OnSelectedIndexChanged="ddlRancho_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="ltInvernadero" runat="server" Text="Invernadero"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlInvernadero" runat="server" meta:resourceKey="ddlRanchoResource">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="ltCosecha2" runat="server" Text="Cantidad SIN Calcular"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxCantidad" runat="server" />
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
                            <td align="right" class="auto-style6">
                                <asp:Literal ID="ltCosecha1" runat="server"  Text="Cosecha"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxCosecha" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar" OnClick="btnCancelar_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnConsulta" runat="server"  Text="Consultar" OnClick="btnConsulta_Click" />
                            </td>

                        </tr>
                        
                    </table>
              </asp:Panel>
           

            <div >
                <table class="auto-style7">
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                <asp:Button ID="btnProcesar" runat="server" Text="PROCESAR A PAGO" OnClick="btnProcesar_Click" />


                <asp:Button ID="btnPreview" runat="server" Text="PREVIEW" OnClick="btnPreview_Click" Visible="False" />


                        </td>
                    </tr>
                </table>


                <asp:GridView ID="GridView1" runat="server" HeaderStyle-CssClass="header"
                    AutoGenerateColumns="false" Font-Names="Arial"
                    OnRowDataBound="RowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged"
                    Font-Size="10pt" AlternatingRowStyle-BackColor="#D6DFD0" >

                    <Columns>                    

                       
                             <asp:TemplateField  meta:resourcekey="BoundFieldResource1">
                                 <itemtemplate>                    
                                   <%--<a href="#" class="gridViewToolTip" onclick='SubmitData("<%# Eval("idAsociado")%>","<%# Eval("HoraInicio")%>","<%# Eval("HoraFin")%>","<%# Eval("NombreHabilidad")%>","<%# Eval("NombreAsociado")%>","<%# Eval("idHabilidad")%>")'><%# Eval("idAsociado")%></a>--%>                                                                    
                                    <asp:LinkButton ID="lnkEmpleado" Text='<%# Eval("idAsociado") %>'     runat="server" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" />
                                     <asp:HiddenField ID="hdfActividad" runat="server" Value='<%# Eval("idHabilidad")%>' />
                                      <asp:HiddenField ID="hdfCantidad" runat="server" Value='<%# Eval("cantTrabajado")%>' />
                                     <asp:HiddenField ID="hdfidCapturaHeader" runat="server" Value='<%# Eval("idCapturaHeader")%>' />
                                 </itemtemplate>
                             </asp:TemplateField>

                     
                        <asp:BoundField DataField="NombreAsociado" SortExpression="NombreAsociado"
                            meta:resourcekey="BoundFieldResource2">
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="NombreHabilidad" SortExpression="NombreHabilidad"
                            meta:resourcekey="BoundFieldResource3">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="TipoCosecha" SortExpression="TipoCosecha"
                            HeaderText="Tipo">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Invernadero" SortExpression="Invernadero"
                            meta:resourcekey="BoundFieldResource4">
                         
                        </asp:BoundField>
                         
                        <asp:BoundField DataField="HoraInicio" meta:resourcekey="BoundFieldResource5"
                            SortExpression="HoraInicio" DataFormatString="{0:dd/MM/yyyy}">                          
                        </asp:BoundField>

                        <asp:BoundField DataField="HoraFin" meta:resourcekey="BoundFieldResource10"
                            SortExpression="HoraFin" DataFormatString="{0:dd/MM/yyyy}">                          
                        </asp:BoundField>
                        <asp:BoundField DataField="cantTrabajado" SortExpression="CantidadSurcos"
                            meta:resourcekey="BoundFieldResource11" ItemStyle-HorizontalAlign="Right">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="Cantidad" meta:resourcekey="BoundFieldResource7"
                            SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Costo" meta:resourcekey="BoundFieldResource8"
                            SortExpression="Costo" ItemStyle-HorizontalAlign="Right">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="Unidad" meta:resourcekey="BoundFieldResource9"
                            SortExpression="Unidad" ItemStyle-HorizontalAlign="Right" >
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="Pago" meta:resourcekey="BoundFieldResource12"
                            SortExpression="Pago" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right">
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="TipoPago" meta:resourcekey="BoundFieldResource13"
                            SortExpression="TipoPago"  ItemStyle-HorizontalAlign="center">
                        
                        </asp:BoundField>
                         <asp:TemplateField HeaderText="Razón Rechazo">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlRazonRechazo"  AppendDataBoundItems="true"  runat ="server">
                                       <asp:ListItem Text="Sin Rechazo" Value="0" />
                                    </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Comentario Rechazo">
                            <ItemTemplate>
                                <asp:TextBox ID="txtComentarioRechazo" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>

                            <HeaderTemplate>

                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />

                            </HeaderTemplate>

                            <ItemTemplate>

                                <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
             
            </div>
    </div>
 

<br />
    <asp:LinkButton Text="" ID = "lnkFake" runat="server" />
    <asp:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></asp:ModalPopupExtender>
    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
    <div class="header">
        Details
    </div>
    <div class="body">
     <table border="1" width="100%">
        <tr>
            <td>
                DETALLE DE INFORMACIÓN 
            </td>
         </tr>
         <tr>
            <td class="auto-style8">
                No. Empleado 
            </td>
              <td class="auto-style8">
                Nombre
            </td>
             <td class="auto-style8">
                Actividad
            </td>
              <td class="auto-style8">
                Fecha Inicio 
            </td>
              <td class="auto-style8">
                Fecha Fin
            </td>   
             <td class="auto-style8">
                Cant Trabajado
            </td>              
        </tr>
        <tr>
            <td>
                <asp:Label id="lblNoEmpleado" runat="server"></asp:Label>
            </td>
              <td>
                 <asp:Label id="lblNombreEmpleado" runat="server"></asp:Label>
            </td>
             <td>
                 <asp:Label id="lblActividad" runat="server"></asp:Label>
            </td>
              <td>
                  <asp:Label id="lblFechaInicio" runat="server"></asp:Label>
            </td>
              <td>
                <asp:Label id="lblFechaFin" runat="server"></asp:Label>
            </td>      
             <td>
                <asp:Label id="lblCantidadSurcosTrabajados" runat="server"></asp:Label>
            </td>          
        </tr>
    </table>   
         <asp:GridView ID="GvDetalle" runat="server" HeaderStyle-CssClass="header"
                    AutoGenerateColumns="true" Font-Names="Arial"
                    OnRowDataBound="RowDataBound"
                    Font-Size="10pt" AlternatingRowStyle-BackColor="#C2D69B"   HorizontalAlign="Center" >
                     <RowStyle HorizontalAlign="Center" />
                      
                      <Columns>  
                        
                        </Columns>
             </asp:GridView>

    </div>
   
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" />
</asp:Panel>
    
  

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



