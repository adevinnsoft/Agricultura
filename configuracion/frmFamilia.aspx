<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmFamilia.aspx.cs" Inherits="configuracion_frmFamilia" EnableEventValidation="false" meta:resourcekey="PageResource1"%>
    <%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
  <script type="text/javascript">
      var myname;
	    $(function () {
	        registerControls();

            $("#<%=txtNivel.ClientID %>").change(function(){
                if($('#<%=btnAgregar_nivel.ClientID%>').val() == 'Guardar')
                {
                    mostrarNiveles($(this));
                }
             });
	    });
	    var x_aux;
	    function crearTabla() {
	        var Datos_Niveles4 = '<%= cadenaNiveles%>';
	        //alert(Datos_Niveles4);
	        //alert("Si llegó, podemos proseguir ;)" + txt_Cant_Mrr);
	        //PageMethods.wmcadenaNivel($('#<%=txtFam.ClientID%>').val());
	        var tabla_Nivel = '<table class="grayView" id="MitblNivel" cellpadding="0" cellspacing="0" border="0" rules="all" style="background:#D6DFD0; min-width:500px; margin-bottom:15px;">';

	        var Monto_Total = 0;
	        tabla_Nivel += '<tr>';
	        tabla_Nivel += '<td colspan ="3" style ="text-align: center;">' + '<asp:Label ID="Lbl_gv_Mrr" runat="server"  Text="Niveles" Font-Bold ="true" Font-Size ="Large" ForeColor ="Green" ></asp:Label>' + '</td>';
	        tabla_Nivel += '</tr>';
	        tabla_Nivel += '<tr>';
	        tabla_Nivel += '<th style ="text-align: center;">' + '<asp:Label ID="Lbl_gv_Nivel" runat="server" Text="Nivel" Font-Bold ="true" ForeColor ="Black"></asp:Label>' + '</th>';
            tabla_Nivel += '<th style ="text-align: center;">' + '<asp:Label ID="Lbl_gv_Nombre" runat="server" Text="* Nombre" Font-Bold ="true" ForeColor ="Black"></asp:Label>' + '</th>';
	        tabla_Nivel += '<th style ="text-align: center;">' + '<asp:Label ID="Lbl_gv_Mrr_Ca" runat="server" Text="Activo" Font-Bold ="true" ForeColor ="Black"></asp:Label>' + '</th>';
	        //tabla_Nivel += '<th style ="text-align: center;">' + '<asp:Label ID="Lbl_gv_Mrr_Q" runat="server" Text="Quitar" Font-Bold ="true" ForeColor ="Black"  ></asp:Label>' + '</th>';
	        tabla_Nivel += '</tr>';

	        nivelfila = Datos_Niveles4.split(']');
	        var nivelcolumna;
	        nivelfila.pop();
            for (var x = 0; x <= nivelfila.length-1; x++) {
                //alert(x);
                nivelcolumna = nivelfila[x].split('|');
                tabla_Nivel += "<tr>";
                x_aux = x + 2;
                for (var y = 0; y <= nivelcolumna.length - 1; y++) {
                    //alert(nivelcolumna[y]);
                    if (y == 0) {
                        tabla_Nivel += "<td style =\"text-align: center;\">" + nivelcolumna[y] + "</td>";
                    }
                    if(y == 2)
                    {
                        tabla_Nivel += "<td style =\"text-align: center;\"><input style=\"float:none;\" type=\"text\" id=\"Nombre" + (x + 1) + "\" value=\"" + nivelcolumna[y] + "\" class=\"required selNombre\"></td>";
                    }
                    if (y == 3) {
                        if (nivelcolumna[y] == "True") {
                            tabla_Nivel += "<td style =\"text-align: center;\"><input style='float:none;' type=\"checkbox\" class=\"selcheck\" name=\"etapaHabilidad\" id='" + x + y + "' value='" + (x + 1) + "' checked=\"checked\"></td>";
                        }
                        else {
                            tabla_Nivel += "<td style =\"text-align: center;\"><input style='float:none;' type=\"checkbox\" class=\"selcheck\" name=\"etapaHabilidad\" id='" + x + y + "' value='" + (x + 1) + "'></td>";
                        }
                    }
                }
                tabla_Nivel += "</tr>";
            }
                tabla_Nivel += '</table></br>';
	        //alert(tabla_Mrr);
                $('#<%=ContendTabla.ClientID%>').html(tabla_Nivel);
                if(parseInt($('#<%=txtNivel.ClientID%>').val())!=0)
                {
                    $('#<%=AgreNiv.ClientID%>').html("<img alt=\"Agregar\" src=\"../comun/img/add-icon.png\"  onclick=\"javascript:addRow(\'MitblNivel\');\"/>");
                }
	        //alert("ya finalizó");
	    }
	    function NivSel() {
            if(parseInt($('#<%=txtNivel.ClientID%>').val())==0)
                {
                    popUpAlert("Una familia No se puede crear con 0 Niveles.","error");
                }
                else
                {
                    var guardar = true;
                    $('.required').map(function(){
                        if($(this).val() == "")
                        {
                            guardar = false;
                            $(this).css({ 'border': '1px solid red' });
                        }else{
                            $(this).css({ 'border': '1px solid black' });
                        }
                    });

                    if(guardar == false)
                    {
                        popUpAlert("Insertar datos requeridos.","error");
                    }
	                else
	                {        
            
	                    var NivelSelec="";
	                    $('.selcheck').each(function () {
                            if ($('#' + $(this).attr('id')).is(":checked")) {
                                NivelSelec += $(this).val() + "|1" + "|" + $(this).parent().parent().find('.selNombre').val();
                            }
                            else {
                                NivelSelec += $(this).val() + "|0" + "|" + $(this).parent().parent().find('.selNombre').val();
                            }

                            NivelSelec += "]";

                        });

                        $('.selcheck2').each(
                        function () {
                            if ($('#' + $(this).attr('id')).is(":checked")) {
                                cadnewNivels += $(this).val() + "|1" + "|" + $(this).parent().parent().find('.selNombre').val();
                            }
                            else {
                                cadnewNivels += $(this).val() + "|0" + "|" + $(this).parent().parent().find('.selNombre').val();
                            }
                            cadnewNivels += "]";
                        }
                        );
                        PageMethods.web_met($('#<%=txtFam.ClientID%>').val(), $('#<%=txtFam_EN.ClientID%>').val(), $('#<%=txtNivel.ClientID%>').val(), $('#<%=idActivo.ClientID%>').attr('checked') == 'checked' ? 'true' : 'false', $('#<%=Accion.ClientID%>').val(), agregados, cadnewNivels, NivelSelec, NivelCallback);                                 
                    }
                 }
        }
        function NivelCallback(Result) {
        $('#<%=idActivo.ClientID%>').prop( "checked", true );
            $('#<%=Accion.ClientID%>').val("Añadir");
            $('#<%=txtFam.ClientID%>').val("");
            $('#<%=txtFam_EN.ClientID%>').val("");
            $('#<%=txtNivel.ClientID%>').val("");
            $('#<%=btnCancelar.ClientID%>').val("Limpiar");
            $('#<%=btnAgregar_nivel.ClientID%>').val("Guardar");
            $('#<%=ContendTabla.ClientID%>').html("");
         
            popUpAlertButtons(Result[0], '<input  id="popup1" type="button" onclick="javascript:location.assign(\'frmFamilia.aspx\');" value="ok"/>', Result[1]);
            //__doPostBack('', '');
            //location.assign("frmFamilia.aspx");
        }
        //__dopostback
        var cadnewNivels = "";
        var agregados = false;
        
        var diferenciaaux=0,diferencianeg;
        var idfinal;
        var cantidadProv='<%= cant_niveles%>';

        function addRow(tableID) {
            var diferenciapos;
            var num_nivels = $('#<%=txtNivel.ClientID%>').val();
            if (num_nivels > <%= cant_niveles%>) {
                diferenciapos = num_nivels - cantidadProv;   
                diferencianeg=idfinal-num_nivels;
                if(diferenciapos<0)
                {
                    if (diferencianeg<diferenciaaux || diferenciaaux!=0)
                    {
                        for(idfinal;idfinal>$('#<%=txtNivel.ClientID%>').val();idfinal--)
                        {
                            $('#tr'+idfinal).remove();
                        }
                
                    }
                }
                else
                {
                for (var jj = 1; jj <= diferenciapos; jj++) {
                    agregados = true;
                    var table = document.getElementById(tableID);
                    var rowCount = table.rows.length;
                    var row = table.insertRow(rowCount);

                    //row.id=x_aux ;
                    row.id="tr"+(rowCount-1);
                    //var id = document.createAttribute("id");     //Este es para especificar un ID a una tabla
                     


                    //Insertamos la columna nivel
                    var stilo = document.createAttribute("style");
                    stilo.value = 'text-align: center;';
                    var cell1 = row.insertCell(0);
                    cell1.setAttributeNode(stilo);
                    //cell1.id=x_aux - 1;
                    var element1 = document.createElement("label");
                    //element1.innerHTML = x_aux;
                    element1.innerHTML=rowCount-1;
                    //cadnewNivels += x_aux + "|";
                    x_aux = rowCount-1;
                    cell1.appendChild(element1);

                    //Insertamos la columna Nombre
                    var cell2 = row.insertCell(1);
                    var stilo2 = document.createAttribute("style");
                    var x = jj + parseInt(cantidadProv);
                    stilo2.value = 'text-align: center;';
                    cell2.setAttributeNode(stilo2);
                    var element2 = "<input style=\"float:none;\" type=\"text\" id=\"Nombre" + x + "\" class=\"required selNombre\">";
                    $('#MitblNivel td').last().html(element2);

                    //Insertamos la columna Activo
                    var clase = document.createAttribute("class");       // Create a "class" attribute
                    clase.value = "selcheck2";
                    var stilo3 = document.createAttribute("style");
                    stilo3.value = 'text-align: center;';
                    var stilo33 = document.createAttribute("style");
                    stilo33.value = 'float:none;';
                    var cell3 = row.insertCell(2);
                    cell3.setAttributeNode(stilo3);
                    var element3 = document.createElement("input");
                    element3.type = "checkbox";
                    element3.setAttributeNode(clase);
                    element3.setAttributeNode(stilo33);
                    element3.name = "name";
                    element3.value = x;
                    element3.id = x_aux ;
                    //idfinal= x_aux - 1;
                    idfinal=rowCount-1;
                    cell3.appendChild(element3);
                }
                }
                diferenciaaux=diferenciapos;
                cantidadProv= $('#<%=txtNivel.ClientID%>').val();
            }
            else {
                $('#<%=txtNivel.ClientID%>').val(cantidadProv);
                popUpAlert("El número de niveles debe de ser mayor a la cantidad actual", "info");
            }
        }

        //Funcion para mostrar los niveles cuando el registro es nuevo
        function mostrarNiveles(este)
        {
            var niveles = $(este).val();

            if($('.Nivel').length == 0)
            {
                //Creamos la tabla con la cantidad de niveles insertados 
                var tblNiveles = '<table class="grayView" id="tblNiveles" cellpadding="0" cellspacing="0" border="0" rules="all" style="background:#D6DFD0; min-width:500px; margin-bottom:15px;">';
                tblNiveles += '<tr>';
                tblNiveles += '<td colspan ="3" style ="text-align: center; font-weight: bold; color: Green; font-size: Large;"> Niveles </td>';
                tblNiveles += '</tr>';
                tblNiveles += '<tr>';
                tblNiveles += '<th style ="text-align: center; font-weight: bold; color: black;" > Nivel </th>';
                tblNiveles += '<th style ="text-align: center; font-weight: bold; color: black;" > * Nombre del Nivel </th>';
                tblNiveles += '<th style ="text-align: center; font-weight: bold; color: black;" > Activo </th>';
                tblNiveles += '</tr>';

                for(x = 1; x <= niveles; x++)
                {
                    tblNiveles += '<tr>';
                    tblNiveles += '<td style ="text-align: center;" class="Nivel">' + x + '</td>';
                    tblNiveles += '<td style ="text-align: center;" class="Nombre"> <input style="float:none;" type="text" id="Nombre' + x + '" class="required selNombre"></td>';
                    tblNiveles += '<td style ="text-align: center;" class="Activo"> <input style="float:none;" type=\"checkbox\" id="nivel' + x + '" value="' + x + '" checked=\"checked\" class=\"selcheck\"> </td>';
                    tblNiveles += '</tr>';
                }

                tblNiveles += '</table>';

                $('#<%=ContendTabla.ClientID%>').html(tblNiveles);
            }else if(niveles > $('.Nivel').length)
            {                
                for(x = ($('.Nivel').length + 1); x <= niveles; x++)
                {
                    tblNiveles += '<tr>';
                    tblNiveles += '<td style ="text-align: center;" class="Nivel">' + x + '</td>';
                    tblNiveles += '<td style ="text-align: center;" class="Nombre"> <input style="float:none;" type="text" id="Nombre' + x + '" class="required selNombre"></td>';
                    tblNiveles += '<td style ="text-align: center;" class="Activo"> <input style="float:none;" type=\"checkbox\" id="nivel' + x + '" value="' + x + '" checked=\"checked\" class=\"selcheck\"> </td>';
                    tblNiveles += '</tr>';
                }

                $('.Nivel').last().parent().after(tblNiveles)
            }else{
                $('#<%=txtNivel.ClientID%>').val($('.Nivel').length);
                popUpAlert("El número de niveles debe de ser mayor a la cantidad actual", "info");
            }
        }
  </script>
  <style type="text/css">
table.index tr td table {
	float:none;
}

table.index tr td table tr td {
	padding: 4px;
}

table.index img 
{
    position:relative;
    left: -180px;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:Label ID="lblFamilia_prin" runat="server" Text="Familia" 
                meta:resourcekey="lblFamilia_prinResource1"></asp:Label></h1>
        <asp:Panel ID="form" runat="server" DefaultButton="btnHidden" 
            meta:resourcekey="formResource1">
            <table class="index" border="0">
                <tr>
                    <td align="left" colspan="8">
                        <h2>
                            <asp:Label runat="server" Text="Registro de Familia" 
                                meta:resourcekey="LabelResource1"></asp:Label>
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        &nbsp;
                    </td>
                </tr>
                <tr>
         
                    <td align="left">
                        <asp:Literal runat="server" ID="lblFamilia" Text="&nbsp;&nbsp;&nbsp;*Familia:" 
                            meta:resourcekey="lblFamiliaResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFam" Width="100px" MaxLength="50" 
                            CssClass="required" meta:resourcekey="txtFamResource1"></asp:TextBox><asp:Label ID="lngSp" runat="server" meta:resourceKey="lngSp"
                                        CssClass="lengua"></asp:Label>
                    </td>
                    
                    <td>
                        <asp:Literal ID="lblNivel" runat="server" 
                            Text="&nbsp;&nbsp;&nbsp;*Nivel:" meta:resourcekey="lblNivelResource1"></asp:Literal>
                    </td>
                     <td>
                         <asp:TextBox ID="txtNivel" runat="server" CssClass="intValidate required" MaxLength="50" 
                             Width="100px" meta:resourcekey="txtNivelResource1" ></asp:TextBox>
                    </td>
                    <td><div runat="server" id="AgreNiv"></div></td>
                    <td align="left" style="text-align: right;">
                        <asp:Literal ID="idltActivo" runat="server" Text="Activo" 
                            meta:resourcekey="idltActivoResource1"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox ID="idActivo" runat="server" Checked="True" 
                            meta:resourcekey="idActivoResource1"/>
                    </td>
                  
                </tr>
                <tr>
                            
                    <td align="left">
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFam_EN" Width="100px" MaxLength="50" 
                            CssClass="required" meta:resourcekey="txtFam_ENResource1"></asp:TextBox>   <asp:Label ID="lngEn" runat="server" meta:resourceKey="lngEn"
                                        CssClass="lengua"></asp:Label>
                    </td>
                    <td colspan="5">
                    &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                    &nbsp;
                    </td>
                </tr>
              <tr>
              
              <td colspan="7">
                <div runat="server" id="ContendTabla"></div>
              </td>           
              </tr>
                <tr>
                    <td colspan="8">
                        <asp:HiddenField ID="hdn_Act" runat="server" />
                        <asp:HiddenField ID="hdn_NivSel" runat="server" />
                        <asp:HiddenField ID="hdn_CherryT" runat="server" />
                        <asp:HiddenField ID="hdn_Morral" runat="server" />
                   </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                        <asp:HiddenField runat="server" Value="Añadir" ID="Accion" />
                    </td>        
                 
                    <td colspan="4">
                            <input runat="server" id="btnAgregar_nivel" type="button" onclick="javascript:NivSel();" value="Guardar"/>
                            &nbsp;&nbsp;<asp:Button runat="server" ID="btnHidden" OnClientClick="return false;" Style="position: absolute;
                            top: -50%;" meta:resourcekey="btnHiddenResource1" />
                       
                            <asp:Button ID="btnCancelar" runat="server" 
                                meta:resourcekey="btnCancelarResource1" OnClick="btnCancelar_Click" 
                                Text="Limpiar" />
                       
                    </td>
                </tr>
            </table>
            <br/><br/><br/>
            <div id="tbl_Nivel" class="index" style="display:none;"></div>
        </asp:Panel>
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
			<asp:Gridview runat="server" ID="gv_Familia" CssClass="gridView" 
                EmptyDataText="No existen registros" Width="800px"
        AutoGenerateColumns="False" DataKeyNames="IdFamilia" OnPageIndexChanging="gv_Familia_PageIndexChanging"
        OnPreRender="gv_Familia_PreRender" OnRowDataBound="gv_Familia_RowDataBound" CellPadding="4"
        ForeColor="#333333" GridLines="None"  
                OnSelectedIndexChanged="gv_Familia_SelectedIndexChanged" 
                meta:resourcekey="gv_FamiliaResource1" >
            <Columns>
                <asp:BoundField DataField="vFamilia" HeaderText="Familia" 
                    meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="Num_Niveles" HeaderText="Número de Niveles" 
                    meta:resourcekey="BoundFieldResource2" />
                <%--<asp:BoundField DataField="bActivo" HeaderText="Activo" 
                    meta:resourcekey="BoundFieldResource3" /> 
     --%>
         
                <asp:TemplateField HeaderText="Activo" SortExpression="Activo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><asp:Label ID="lblActivoGrid" runat="server" Text='<%# (bool)Eval("bActivo")==true? GetLocalResourceObject("lblActivoGridSi") :GetLocalResourceObject("lblActivoGridNo") %>' meta:resourcekey="lblActivoGridResource1" /></ItemTemplate>
                </asp:TemplateField>
     
         
           </Columns>
        </asp:Gridview>
			</div>
        </div>
        <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    </div>
</asp:Content>
