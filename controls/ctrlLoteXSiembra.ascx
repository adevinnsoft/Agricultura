<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ctrlLoteXSiembra.ascx.cs" Inherits="controls_ctrlLoteXSiembra" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.50401.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

 <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script src="../comun/scripts/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.validate.js" type="text/javascript"></script>

    <%--Estas funciones se comunican con la pagina padre, ya sean callback o que de aqui se manden llamar--%>
    <script type="text/javascript">


        /*
        callback de la funcion save(idCharola, idSustrato, idSiembra)
        para saber que ya se guardo los lotes para la siembra
        limpia la fila 1 de los lotes y borra las demas
         manda llamar otra funcion del padre para que llene el TempLotes, pues es un webMethod lo que necesito
        */
        function limpiar(guardado) {
            if (parseInt(guardado) == -1) {
                alert('Se perdio la sesion');
            }

            else if (parseInt(guardado) == 1) {
                //borrar las filas (a partir de la tercera)
                $("#tblLotes").find("tr:gt(2)").remove();
                //limpar la segunda
                $("#tblLotes tr:nth-child(3) td:nth-child(1) input").val("");
                $("#tblLotes tr:nth-child(3) td:nth-child(3) input").val("");
                $("#tblLotes tr:nth-child(3) td:nth-child(4) input").val("");
                $("#tblLotes tr:nth-child(3) td:nth-child(5) input").val("");
                $("#tblLotes tr:nth-child(3) td:nth-child(6) input").val("");
                        

                //actualizar control, esta funcion esta en el padre, no en el control
                actualizaTempLotes();
            }
            else {
                alert(guardado);
            }
        }

        /*
        callback de la funcion actualizaTempLotes()
        es para llenar la variable LotesTmp con los datos nuevos
        manda llamar otra funcion del padre para que llene el ddl Lotes, pues es un webMethod lo que necesito
        */
        function llenaTempLotes(guardado) {

            $('#<%=LotesTmp.ClientID %>').val(guardado);
            //llenaDdlLotes();                   
            lotesLoad();
            existenciasLote();

        }

        /*
        callback de la funcion llenaDdlLotes()
        es para llenar la variable el ddl de Lotes con los datos nuevos       
        
        function llenaDdl(result) {
            fillDropDown("#<%= ddlLote.ClientID %>", result, "idLote", "lote", false);
        }

        /*
        llena el ddl Lotes
        
        function fillDropDown(ddlname, jsonList, value, name, seleccione) {

            var listItems = "";
            var obj = $.parseJSON(jsonList);
            if (seleccione) {
                listItems += "<option value=\"\"> -Seleccione-</option>";
            }
            
            for (var i = 0; i < obj.length; i++) {
                listItems += "<option value='" + obj[i][value] + "'>" + obj[i][name] + "</option>";
            }
            $(ddlname).html(listItems);
            actualizaControl();
        }
        */       
    </script>

    <%--botones del control--%>
    <script type="text/javascript">
        
        //-- registrar combi box ---------------------------------------------
        function registerCombobox() {
            $("select[name='<%= ddlLote.UniqueID %>']").combobox();
        }

        //-- boton agregar lote ---------------------------------------------
        function addLote() {
            var txt0 = $("#<%= txtID.ClientID %>").clone().html();
            var ddl = $("#<%= ddlLote.ClientID %>").clone().html();
            var txt = $("#<%= txtCantidad.ClientID %>").clone().html();
            var txt1 = $("#<%= txtGramReq.ClientID %>").clone().html();
            var txt2 = $("#<%= txtExistenciSemillas.ClientID %>").clone().html();
            var txt3 = $("#<%= txtExistenciaGrms.ClientID %>").clone().html();

            var html = '<tr><td align="center"> <input name="<%= txtID.UniqueID %>"   class="txtID" style="width:60px;" disabled >' + txt0 + '</input></td>';           
            html += '<td><select name="<%= ddlLote.UniqueID %>" class="ddlLote" onchange="existenciasLoteDDL( this );">' + ddl + '</select></td>';          
            html += '<td><input name="<%= txtCantidad.UniqueID %>" class="txtCantidad" style="width:60px;" onkeypress="javascript:return ValidTec( event );"> ' + txt + '</input></td>';
            html += '<td align="center"> <input name="<%= txtGramReq.UniqueID %>"   class="txtGramReq" style="width:60px;" disabled >' + txt1 + '</input></td>';
            html += '<td align="center"> <input name="<%= txtExistenciSemillas.UniqueID %>"   class="txtExistenciSemillas" style="width:60px;" disabled >' + txt2 + '</input></td>';
            html += '<td align="center"> <input name="<%= txtExistenciaGrms.UniqueID %>"   class="txtExistenciaGrms" style="width:60px;" disabled >' + txt3 + '</input></td>';
            html += '<td><img alt="Quitar" src="../comun/img/remove-icon.png" class="btnRemoverPlaga"/></td>';
            html += '<td><img alt="Enviar a Siembra" src="../comun/img/siembra.jpg" width="40" class="enviarSiembra"  style="display:none" onClick="javascript:enviarSiembra(this);" /></td>';
            html += '<td><img alt="Sobrepasa Cantidad" src="../comun/img/error.png" width="40" style="display:none" /></td>';
            html += '</tr>';

            $('#tblLotes').append(html);
            existenciasLoteBotonaso();            
        }

        //-- boton remover plaga ---------------------------------------------
        $('.btnRemoverPlaga').live('click', function () {
            $(this).parent().parent().remove();
            sumaLoQueHay();
        });

        //-- boton enviar a siembra ---------------------------------------------
        function enviarSiembra(boton) {
            var row = saberRow(boton);

            //-- sacar existencias de esos lotes 
            var lotes = $('#<%=LotesTmp.ClientID %>').val();
            
            if (lotes) {
                //var itemArray = lotes.split('@');
                var idreq = $("#tblLotes tr:nth-child(" + row + ") td:nth-child(1) input").val();// itemArray[row - 3].split('|')[5];

                //-- llamar el procedimiento del servidor
                enviarSiembraPage(idreq, row);
            }
            
            delete row;
            delete lotes;
        }

        //-- boton guardar -----------------------------------------------------
        
        /*
        Esta la hice para mandar el id del ddl de charola y sustrato a la funcion que esta en la pagina padre, 
        por que desde la otra funcion (save(idCharola, idSustrato)) no los encuentro los ddl :S
        */

        function save_Control() {
            var ddlCharola = $("#<%= ddlCharola.ClientID %>");
            var idCharola = ddlCharola[0].value;

            var ddlSustrato = $("#<%= ddlSustrato.ClientID %>");
            var idSustrato = ddlSustrato[0].value;

            var idSiembra = '<%=Session["idSiembra"] %>';

            save(idCharola, idSustrato, idSiembra);
        }
    </script>
        
     <%--suma lo que hay cuando se borra una fila--%>
     <script type="text/javascript">
         
         function sumaLoQueHay() {
                      
             //-- sumar todo lo que tienen las cantidades   
             var i = 0;
             var suma = 0;
             
             $.each($('.txtCantidad'), function () {
                 suma += parseFloat($('.txtCantidad')[i].value);
                 i++;
             });
             
             $('#<%=lblSemAcum.ClientID %>').text(suma);

             var requerida = $('#<%=lblSemReq.ClientID %>').text()
             $('#<%=lblSemFalt.ClientID %>').text(parseFloat(requerida) - suma);

             //avisar si hay sobrepedido
             if (parseFloat(requerida) < suma) {
                 $('#imgSobrePedido').show();
                 $('.mensajeTexto').fadeIn();
             }
             else {
                 $('#imgSobrePedido').hide();
                 $('.mensajeTexto').fadeOut();
             }
            
         }
     </script>
    
     <%--validaciones y cambios al insertar numeros--%>
     <script type="text/javascript">

         //-- ponerle las cualidades a los campos para que valide--------------
         $(".txtCantidad").live("blur", function (event) {
             //este sirve cuando haces cualquiero cosa
         });

         $(".txtCantidad").live("keyup", function (event) {
             $(this).addClass('keyupping');
             suma(this);
             $(this).removeClass('keyupping');
         });

         //-- que solo permita ingresar numeros--------------------------------
         function ValidTec(e) {
             var key;
             if (window.event) // IE
             {
                 key = e.keyCode;
             }
             else if (e.which) // Netscape/Firefox/Opera
             {
                 key = e.which;
             }
             if (key < 48 || key > 57) {
                 return false;
             }
             return true;
         }

         //-- que sume los numeros ingresados -------------------------------------
         //-- así como mostrar imagen de alerta si se sobrepasa la cantidad--------
         function suma(e) {
             
             //-- tambien valido el teclaso
             var key;
             if (window.event) // IE
             {
                 key = e.keyCode;
             }
             else if (e.which) // Netscape/Firefox/Opera
             {
                 key = e.which;
             }

             if (key < 48 || key > 57) {
                 return false;
             }

             //var te = String.fromCharCode(key); //numero apretado

             //-- ver en que row de la tabla meto el valor:
             var row = saberRow(e);

             var insertado = parseFloat($("#tblLotes tr:nth-child(" + row + ") td:nth-child(3) input").val());  
             var existencia = parseFloat($("#tblLotes tr:nth-child(" + row + ") td:nth-child(5) input").val()); 

             if (parseFloat(insertado) > parseFloat(existencia)) {                 
                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(9) img").show();
             }
             else
                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(9) img").hide();

             //-- calcular los gramos

             //saber cual es el index del ddl seleccionado
             var index = $("#tblLotes tr:nth-child(" + row + ") td:nth-child(2) select option:selected").index();

             //sacar de ese index los gramos X semilla de la variable lotesTmp
             var lotes = $('#<%=LotesTmp.ClientID %>').val();
             var itemArray = lotes.split('@');
             var seXgram = parseFloat(itemArray[index].split('|')[7]);
             var gram = parseFloat(insertado) / seXgram;
             $("#tblLotes tr:nth-child(" + row + ") td:nth-child(4) input").val(gram.toFixed(1));
             
             delete insertado;
             delete existencia;
             delete row;
             delete lote;
             delete itemArray;
             delete seXgram;
             delete gram;
             delete index;

             //-- sumar todo lo que tienen las cantidades   
             var i = 0;
             var suma = 0;
             $.each($('.txtCantidad'), function () {
                 suma += parseFloat($('.txtCantidad')[i].value);
                 i++;
             });
             $('#<%=lblSemAcum.ClientID %>').text(suma);

             var requerida = $('#<%=lblSemReq.ClientID %>').text()
             $('#<%=lblSemFalt.ClientID %>').text(parseFloat(requerida) - suma);

             //avisar si hay sobrepedido
             if (parseFloat(requerida) < suma) {
                 $('#imgSobrePedido').show();                
                 $('.mensajeTexto').fadeIn();
             }
             else {
                 $('#imgSobrePedido').hide();                
                 $('.mensajeTexto').fadeOut();
             }
                
             return true;
         }


     </script>

     <%--cambios en los ddl de los lotes--%>
     <script type="text/javascript">
         
         /*-- 
         Esta se usa cada vez que se cambia un Lote (ddl) para actualizar los campos que tiene en almacen
         --*/
         function existenciasLoteDDL(ddl) {
             var row = saberRow(ddl);

             var index = ddl.selectedIndex;
            

             //-- sacar existencias de esos lotes
             var lotes = $('#<%=LotesTmp.ClientID %>').val();
             if (lotes) {

                 var itemArray = lotes.split('@');
                 
                 //--------------------------------------
                 $.each(itemArray, function (index, value) {
                     var datosArray = value.split('|');
                     
                    
                     if (parseFloat(datosArray[0]) == parseFloat(ddl.value) ) {
                         $("#tblLotes tr:nth-child(" + row + ") td:nth-child(5) input").val(datosArray[3]);
                         $("#tblLotes tr:nth-child(" + row + ") td:nth-child(6) input").val(datosArray[4]);
                        
                     }
                 });
                 
                 
                 
                 
                 
                 
                // -----------------------------------------
                 /*var exisSemi = itemArray[index].split('|')[3];
                 var exisGrms = itemArray[index].split('|')[4];

                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(5) input").val(exisSemi)
                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(6) input").val(exisGrms)

                 //-- calcular los gramos
                 var seXgram = parseFloat(itemArray[index].split('|')[7]);
                 var insertado = $("#tblLotes tr:nth-child(" + row + ") td:nth-child(3) input").val();
                 var gram = parseFloat(insertado) / seXgram;
                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(4) input").val(gram.toFixed(1));

                 delete itemArray;
                 delete seXgram;
                 delete gram;
                 delete itemArray;
                 delete exisSemi;
                 delete insertado;*/

             }
            
             delete row;
             delete index;
             delete lotes;
         }
     </script>
         
     <%--cambios den los ddl de charola y sustrato--%>
     <script type="text/javascript">
         
         //-- cuando se cambia el ddl de charola ----------------------------------------
         function cantidadCharola(ddl) {
             var c = $('#<%=almacenCharolasTmp.ClientID %>').val();
            
             if (c) {
                 var itemArray = c.split('@');
                 var indexArray = itemArray[ddl.selectedIndex];
                 var datosArray = indexArray.split('|');

                 //nombre charola
                 $('#<%=lblNombreCharolaNecesaria.ClientID%>').text(ddl.options[ddl.selectedIndex].text);
                
                 //existencia
                 $('#<%=lblCharolaAlmacen.ClientID%>').text(datosArray[0]);                     

                 //necesarias requeridas
                 $('#<%=lblCantCharolaNecesaria.ClientID%>').text(datosArray[1]);

                 //sustrato requerido
                 $('#<%=lblCantSustratoNecesario.ClientID%>').text(datosArray[2] + ' Kg.');

                 
                 delete itemArray;
                 delete indexArray;
                 delete datosArray;
             }

             else {
                 //nombre charola
                 $('#<%=lblNombreCharolaNecesaria.ClientID%>').text('??');

                 //existencia
                 $('#<%=lblCharolaAlmacen.ClientID%>').text('??');

                 //necesarias requeridas
                 $('#<%=lblCantCharolaNecesaria.ClientID%>').text('??');

                 //sustrato requerido
                 $('#<%=lblCantSustratoNecesario.ClientID%>').text('??');
             }
             delete c;

         }

         //-- cuando se cambia el ddl de sustrato ----------------------------------------
         function cantidadSustrato(ddl) {
             var c = $('#<%=almacenSustratoTmp.ClientID %>').val();

             if (c) {
                 var itemArray = c.split('|');

                 //existencia en almacen
                 $('#<%=lblSustrato.ClientID%>').text(itemArray[ddl.selectedIndex] + ' Kg.');

                 //nombre
                 $('#<%=lblNombreSustratoNecesario.ClientID%>').text(ddl.options[ddl.selectedIndex].text);
                 delete itemArray;
             }
             else {
                 //existencia en almacen
                 $('#<%=lblSustrato.ClientID%>').text('??');

                 //nombre
                 $('#<%=lblNombreSustratoNecesario.ClientID%>').text('??');
             }
             
             delete c;
         }
     </script>
     
     <%-- addLotesCargados()--%>
    <script type="text/javascript">

        function addLotesCargados() {
            var txt0 = $("#<%= txtID.ClientID %>").clone().html();
            var ddl = $("#<%= ddlLote.ClientID %>").clone().html();
            var txt = $("#<%= txtCantidad.ClientID %>").clone().html();
            var txt1 = $("#<%= txtGramReq.ClientID %>").clone().html();
            var txt2 = $("#<%= txtExistenciSemillas.ClientID %>").clone().html();
            var txt3 = $("#<%= txtExistenciaGrms.ClientID %>").clone().html();

            var html = '<tr><td align="center"> <input name="<%= txtID.UniqueID %>"   class="txtID" style="width:60px;" disabled >' + txt0 + '</input></td>';
            html += '<td><select name="<%= ddlLote.UniqueID %>" class="ddlLote" onchange="existenciasLoteDDL( this );">' + ddl + '</select></td>';           
            html += '<td><input name="<%= txtCantidad.UniqueID %>"   class="txtCantidad" style="width:60px;" onkeypress="javascript:return ValidTec( event );" > ' + txt + '</input></td>';
            html += '<td align="center"> <input name="<%= txtGramReq.UniqueID %>"   class="txtGramReq" style="width:60px;"  disabled>' + txt1 + '</input></td>';
            html += '<td align="center"> <input name="<%= txtExistenciSemillas.UniqueID %>"   class="txtExistenciSemillas" style="width:60px;" disabled >' + txt2 + '</input></td>';
            html += '<td align="center"> <input name="<%= txtExistenciaGrms.UniqueID %>"   class="txtExistenciaGrms" style="width:60px;"  disabled>' + txt3 + '</input></td>';
            html += '<td><img alt="Quitar" src="../comun/img/remove-icon.png" class="btnRemoverPlaga"/></td>';
            html += '<td><img alt="Enviar a Siembra" src="../comun/img/siembra.jpg" width="40" class="enviarSiembra" style="display:none" onClick="javascript:enviarSiembra(this);" /></td>';
            html += '<td><img alt="Sobrepasa Cantidad" src="../comun/img/error.png" width="40" style="display:none" /></td>';
            html += '</tr>'

            $('#tblLotes').append(html);
        }
    </script>

     <%--lotesLoad()--%>
     <script type="text/javascript">

         // var i = 0
         function lotesLoad() {
             var lotes = $('#<%=LotesTmp.ClientID %>').val();
            
             if (lotes) {

                 var itemArray = lotes.split('@');
                 var first = true;
                 var vuelta1 = true;

                 $.each(itemArray, function (index, value) {
                     var datosArray = value.split('|');
                     
                     //parseFloat(datosArray[5]) -> idReqSiembraDetalle
                     if (parseFloat(datosArray[5]) != 0) {
                         if (!first) {

                             addLotesCargados();
                             //i++;
                         }
                         first = false;

                         //console.log(datosArray);
                         $('.ddlLote:last').val(datosArray[0]);
                         $('.txtGramReq:last').val(datosArray[1]);
                         $('.txtCantidad:last').val(datosArray[2]);
//                         $('.txtExistenciSemillas:last').val(datosArray[3]);
//                         $('.txtExistenciaGrms:last').val(datosArray[4]);
                         $('.txtID:last').val(datosArray[5]);
                     }
//                     else {
//                         //esto es para poner los valores de almacen en la fila nueva (fila que no esta guardada en la bd)
//                         $('.enviarSiembra').hide();
//                         if (vuelta1) {
//                             $('.txtExistenciSemillas:last').val(datosArray[3]);
//                             $('.txtExistenciaGrms:last').val(datosArray[4]);
//                             vuelta1 = false;
//                         }
//                     }
                 });
                 delete itemArray;
                 delete first;
                 delete datosArray
                 delete lotes;
             }
         }
     </script>

     <%--auxiliares--%>
     <script type="text/javascript">

         /*--
         Esta solo regresa el row en la tabla del elemento que la manda llamra
         La hice para saber en que row se encuentra el ddl que cambio se selected
         --*/
         function saberRow(elemento) {
             var row = elemento.parentElement.parentNode.rowIndex;
             return row + 1;
         }

         /*
         Esta la creé opara actualizar la pantalla cada vez que se manda un elemento a siembra,
         espara ocultar el boton e inabilitar los campos
         */
         function guardoEnvioSiembra(row) {
             if (row != 3)
                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(7) img").hide(); //boton borrar
             $("#tblLotes tr:nth-child(" + row + ") td:nth-child(8)").hide(); //boton siembra             
             $("#tblLotes tr:nth-child(" + row + ") td:nth-child(3) input").attr("disabled", "disabled");
             $("#tblLotes tr:nth-child(" + row + ") td:nth-child(2) select").attr("disabled", "disabled");
         }
     </script>

     <%--existenciasLote() / existenciasLoteBotonaso()--%>
     <script type="text/javascript">

         /*--
         Esta sirve para que cada vez que se agrega otra fila al control, se cargue con los datos de existencia del almacen.
         Aunque para hacer eso, pues recorre todas las filas de la tabla :s
         Se manda llamar con el boton "+"
         --*/

         function existenciasLote() {
             var frm = document.getElementById("tblLotes");
             var row = frm.rows.length;
             var lotes = $('#<%=LotesTmp.ClientID %>').val();

             $("#tblLotes tr:nth-child(3) td:nth-child(2) select option:selected").index()

             if (lotes) {

                 var itemArray = lotes.split('@');

                 for (var i = 1; i <= row; i++) {

                     //var index = $("#tblLotes tr:nth-child(" + i + ") td:nth-child(2) select option:selected").index(); //frm.rows[2].childNodes[3].children[0].selectedIndex;
                     var val =  $("#tblLotes tr:nth-child(" + i + ") td:nth-child(2) select option:selected").val();
                     //if (parseInt(index) >= 0) {
                     if (parseInt(val) >= 0) {


                         //--------------------------------------
                         $.each(itemArray, function (index, value) {
                             var datosArray = value.split('|');

                             if (val == parseFloat(datosArray[0])) {
                                 var exisSemi = datosArray[3];
                                 var exisGrms = datosArray[4];

                                 $("#tblLotes tr:nth-child(" + i + ") td:nth-child(5) input").val(exisSemi);
                                 $("#tblLotes tr:nth-child(" + i + ") td:nth-child(6) input").val(exisGrms);

                                 //buscar los que tinen idReqSiembra y ver si estan enviados a siembra
                                 if (datosArray[6] == 'True' &&
                                            $("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() != "" && 
                                            $("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() == datosArray[5]) {
                                     
                                     $("#tblLotes tr:nth-child(" + i + ") td:nth-child(8) img").hide();

                                     if (i > 3)
                                        $("#tblLotes tr:nth-child(" + i + ") td:nth-child(7) img").hide(); //boton borrar
                                     
                                     $("#tblLotes tr:nth-child(" + i + ") td:nth-child(3) input").attr("disabled", "disabled");
                                     $("#tblLotes tr:nth-child(" + i + ") td:nth-child(2) select").attr("disabled", "disabled");
                                 }

                                 //si no se mandaron a siembra, poner su botoncito de mandar a siembra
                                 else {
                                     if ($("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() != ""
                                            && datosArray[6] == 'False'
                                            && $("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() == datosArray[5])
                                         $("#tblLotes tr:nth-child(" + i + ") td:nth-child(8) img").show();
                                 }

                                 /*$("#tblLotes tr:nth-child(" + row + ") td:nth-child(5) input").val(datosArray[3]);
                                 $("#tblLotes tr:nth-child(" + row + ") td:nth-child(6) input").val(datosArray[4]);*/
                                 delete datosArray;
                                 return false;
                             }
                         });

                         delete itemArray;

                         // -----------------------------------------

                         /*var exisSemi = itemArray[i - 3].split('|')[3];
                         var exisGrms = itemArray[i - 3].split('|')[4];

                         $("#tblLotes tr:nth-child(" + i + ") td:nth-child(5) input").val(exisSemi)
                         $("#tblLotes tr:nth-child(" + i + ") td:nth-child(6) input").val(exisGrms)

                         //saber si ya se envio a siembra
                         if (itemArray[i-3].split('|')[6] == 'True' && $("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() != "") {
                             $("#tblLotes tr:nth-child(" + i + ") td:nth-child(8) img").hide();
                             if (i > 3)
                                 $("#tblLotes tr:nth-child(" + i + ") td:nth-child(7) img").hide(); //boton borrar
                             $("#tblLotes tr:nth-child(" + i + ") td:nth-child(3) input").attr("disabled", "disabled");
                             $("#tblLotes tr:nth-child(" + i + ") td:nth-child(2) select").attr("disabled", "disabled");
                         }
                         else
                             if ( itemArray[i-3].split('|')[6] == 'False' && $("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() != "") {
                                 $("#tblLotes tr:nth-child(" + i + ") td:nth-child(8) img").show();
                             }
                         delete index;
                         delete exisGrms;
                         delete exisSemi;*/
                     }

                 } //for
                 delete itemArray;
                 delete exisSemi;
             } // if

             delete frm;
             delete row;
             delete index;
             delete lotes;
         }

         /*--
         Esta sirve para que cada vez que se agrega otra fila al control, se cargue con los datos de existencia del almacen.
         Aunque para hacer eso, pues recorre todas las filas de la tabla :s
         Se manda llamar con el boton "+"
         --*/
         function existenciasLoteBotonaso() {
             var frm = document.getElementById("tblLotes");
             var row = frm.rows.length;
             var lotes = $('#<%=LotesTmp.ClientID %>').val();

             $("#tblLotes tr:nth-child(3) td:nth-child(2) select option:selected").index()

             if (lotes) {

                 var itemArray = lotes.split('@');

                 //--------------------------------------
                 $.each(itemArray, function (index, value) {
                     var datosArray = value.split('|');

                     if (parseFloat(datosArray[0]) == parseFloat($("#tblLotes tr:nth-child(" + row + ") td:nth-child(2) select option:selected").val())) {
                         $("#tblLotes tr:nth-child(" + row + ") td:nth-child(5) input").val(datosArray[3]);
                         $("#tblLotes tr:nth-child(" + row + ") td:nth-child(6) input").val(datosArray[4]);
                         delete datosArray;
                         return false;
                     }
                 });

                 delete itemArray;
                 
                 // -----------------------------------------
                 /*for (var i = 1; i <= row; i++) {

                     var index = $("#tblLotes tr:nth-child(" + i + ") td:nth-child(2) select option:selected").index(); //frm.rows[2].childNodes[3].children[0].selectedIndex;
                     if (parseInt(index) >= 0) {

                         if ($("#tblLotes tr:nth-child(" + i + ") td:nth-child(1) input").val() == "") {
                             var exisSemi = itemArray[index].split('|')[3];
                             var exisGrms = itemArray[index].split('|')[4];

                             $("#tblLotes tr:nth-child(" + i + ") td:nth-child(5) input").val(exisSemi)
                             $("#tblLotes tr:nth-child(" + i + ") td:nth-child(6) input").val(exisGrms)

                             delete index;
                             delete exisGrms;
                             delete exisSemi;
                         }
                     }

                 } //for
                 
                 delete itemArray;
                 delete exisSemi;*/
             } // if

             delete frm;
             delete row;
             delete index;
             delete lotes;
         }
     </script>
              
    <%-- /INICIO /--%>
     <script type="text/javascript">

        $(function () {
            $('.mensajeTexto').hide();  
            lotesLoad();
            existenciasLote();
        });

    </script>

<asp:Panel ID="pnlCapturaDosisControl" runat="server" CssClass="modalPopup" 
    Style="padding:5px; display:none;" width="650px" 
    meta:resourcekey="pnlCapturaDosisControlResource1" >

    <asp:HiddenField ID="LotesTmp" runat="server" />
    <asp:HiddenField ID="almacenCharolasTmp" runat="server" />
     <asp:HiddenField ID="almacenSustratoTmp" runat="server" />
     <asp:HiddenField ID="almacenClipTmp" runat="server" />
         
       <table id="SemillasLlevadas" class="index3" align="center" style="min-width:600px;">
        <tr>
            <td colspan="6" style="text-align:left; background:#ffa05f;">
                <h5><asp:Label runat="server" ID="lblBienvenida" 
                        Text="Seleccione los lotes para cubrir la siembra" 
                        meta:resourcekey="lblBienvenidaResource1"></asp:Label></h5>
            </td>
        </tr>
        <tr>
            <td>
                 <h4><asp:Label ID="lblSem" runat="server" Text="S. requeridas:" 
                         meta:resourcekey="lblSemResource1"></asp:Label></h4>
            </td>
            <td>
                 <h2><asp:Label ID="lblSemReq" runat="server" meta:resourcekey="lblSemReqResource1"></asp:Label></h2>
            </td>
            <td>
                 <h4><asp:Label ID="Label1" runat="server" Text="S. acumuladas:" 
                         meta:resourcekey="Label1Resource1"></asp:Label></h4>
            </td>
            <td>
                 <h2><asp:Label ID="lblSemAcum" runat="server" 
                         meta:resourcekey="lblSemAcumResource1"></asp:Label></h2>
            </td>
            <td>
                 <h4><asp:Label ID="Label3" runat="server" Text="S. faltantes:" 
                         meta:resourcekey="Label3Resource1"></asp:Label></h4>
            </td>
            <td>
                 <h2><asp:Label ID="lblSemFalt" runat="server" 
                         meta:resourcekey="lblSemFaltResource1"></asp:Label></h2>
            </td>
        </tr>        
    </table>

        <br />

        <table id="tblLotes">
            <tr>                
                <td></td>
                <td></td>
                <td colspan="2" align="center" style="font-size:small; color:Olive; font-weight:bold;" >                                      
                    Acumulado
                </td>
                <td colspan="2" align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    Existencia
                </td>
            </tr>

            <tr>
                
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">                                  
                    ID</td>
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    Lote                   
                </td>
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    Semillas
                </td>
                 <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    Gramos
                </td>
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    Semillas
                </td>
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    Gramos
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtID" Width="60px" runat="server" CssClass="txtID" 
                        Enabled="False" meta:resourcekey="txtIDResource1" ></asp:TextBox>  
                </td>
                <td align="center">
                    <asp:DropDownList ID="ddlLote" runat="server" AppendDataBoundItems="True"  
                        CssClass="ddlLote" Width="80px" 
                        onchange="javascript:existenciasLoteDDL( this );" 
                        meta:resourcekey="ddlLoteResource1">
                    </asp:DropDownList>
                </td>
                
                <td align="center">
                    <asp:TextBox ID="txtCantidad" Width="60px" runat="server" 
                        CssClass="txtCantidad" onkeypress="javascript:return ValidTec( event );" 
                        onkeyup="javascript:return suma( event );" 
                        meta:resourcekey="txtCantidadResource1" ></asp:TextBox>                    
                </td>

                <td align="center">
                    <asp:TextBox ID="txtGramReq" Width="60px" runat="server" CssClass="txtGramReq" 
                        Enabled="False" meta:resourcekey="txtGramReqResource1" ></asp:TextBox>                    
                </td>

                 <td align="center">
                    <asp:TextBox ID="txtExistenciSemillas" Width="60px" runat="server" 
                         CssClass="txtExistenciSemillas" Enabled="False" 
                         meta:resourcekey="txtExistenciSemillasResource1" ></asp:TextBox>                    
                </td>

                <td align="center">
                    <asp:TextBox ID="txtExistenciaGrms" Width="60px" runat="server" 
                        CssClass="txtExistenciaGrms" Enabled="False" 
                        meta:resourcekey="txtExistenciaGrmsResource1" ></asp:TextBox>                    
                </td>
               
                <td>                  
                    <img id="imgAdd" alt="Agregar" src="../comun/img/add-icon.png" onclick="addLote();"  />
                </td>
                <td>                  
                    <img id="imgSiembra" alt="Enviar a Siembra" src="../comun/img/siembra.jpg" class="enviarSiembra"  style="display:none;" width="40px" onclick="javascript:enviarSiembra( this );" />
                </td>
                <td>
                    <img alt="Sobrepasa Cantidad" src="../comun/img/error.png" width="40" style="display:none;" />
                </td>
            </tr>
        </table>
        
        <table id="sobrepedido">
            <tr>
                <td>
                    <img alt="Sobrepedido" src="../comun/img/error.png" width="40" style="display:none" id="imgSobrePedido" />
                </td>                
                <td>
                    <span class='mensajeTexto'>La candidad de semillas acumuladas es mayor a la cantidad requerida</span>
                </td>
            </tr>
        </table>
        <br />        
        <table id="tblElementosUsados" class="index4" align="center" style="min-width:600px;">
         
         <tr>
            <td colspan="2" style="text-align:left; background:#ffa05f;">
                <h5><asp:Label runat="server" ID="Label8" 
                        Text="Seleccione los recursos necesarios para cubrir la siembra" 
                        meta:resourcekey="Label8Resource1"></asp:Label></h5>
            </td>
        </tr>
        <tr >
            <td  >
             <h4><asp:Label ID="Label2" runat="server" 
                     Text="Charola que se utilizará en Siembra:" meta:resourcekey="Label2Resource1"></asp:Label></h4>
            </td>
            <td >
                <asp:DropDownList ID="ddlCharola" runat="server" 
                    onchange="javascript:cantidadCharola( this );" 
                    meta:resourcekey="ddlCharolaResource1">  </asp:DropDownList>
            </td>
              
        </tr>       
         <tr >
            <td  >
             <h4><asp:Label ID="Label6" runat="server" 
                     Text="Sustrato que se utilizará en Siembra:" meta:resourcekey="Label6Resource1"></asp:Label></h4>
            </td>
            <td >
                <asp:DropDownList ID="ddlSustrato" runat="server" 
                    onchange="javascript:cantidadSustrato( this );" 
                    meta:resourcekey="ddlSustratoResource1">  </asp:DropDownList>
            </td>
        </tr>
        
        </table>

         <br />
      
        <table id="tblCosasNecesarias" class="index3" align="center" style="min-width:600px;">
            <tr>
                <td colspan="6" style="text-align:center; background:#ffa05f;">
                    <h5><asp:Label runat="server" ID="Label5" Text="Existencias en Almacén" 
                            meta:resourcekey="Label5Resource1"></asp:Label></h5>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <h4>Nombre</h4>
                </td>
                
                <td align="center">
                    <h4>Existencia</h4>
                </td>

                <td align="center">
                   <h4>Requerido</h4>
                </td>
            </tr>

            <tr>
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    <asp:Label runat="server" ID="lblNombreCharolaNecesaria" Text="??" 
                        meta:resourcekey="lblNombreCharolaNecesariaResource1"></asp:Label>
                </td>
                
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    <asp:Label ID="lblCharolaAlmacen" runat="server" Text="???" 
                        meta:resourcekey="lblCharolaAlmacenResource1"></asp:Label> 
                </td>

                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                    <asp:Label ID="lblCantCharolaNecesaria" runat="server" Text="???" 
                        meta:resourcekey="lblCantCharolaNecesariaResource1"></asp:Label> 
                </td>
            </tr>

            <tr>
                <td align="center"  style="font-size:small; color:Olive; font-weight:bold;">
                   <asp:Label runat="server" ID="lblNombreSustratoNecesario" Text="??" 
                        meta:resourcekey="lblNombreSustratoNecesarioResource1"></asp:Label>
                </td>

                 <td align="center"  style="font-size:small; color:Olive; font-weight:bold;">
                   <asp:Label ID="lblSustrato" runat="server" Text="???" 
                         meta:resourcekey="lblSustratoResource1"></asp:Label> 
                </td>  
                
                <td align="center" style="font-size:small; color:Olive; font-weight:bold;">
                   <asp:Label ID="lblCantSustratoNecesario" runat="server" Text="???" 
                        meta:resourcekey="lblCantSustratoNecesarioResource1"></asp:Label> 
                </td>                
            </tr>
        </table>

       
        <table id="tblBotones">
        <tr>
            <td style="width:600px;">
            </td>
        </tr>
        <tr>
            <td class="floatnone" align="right">                         
                <asp:Button CssClass="button" runat="server" ID="cancelar" Text="Cerrar" 
                    onclick="cancelar_Click" meta:resourcekey="cancelarResource1"/>
                <asp:Button CssClass="button" runat="server" ID="save" Text="Guardar"  
                    OnClientClick="save_Control();return false;" 
                    meta:resourcekey="saveResource1"   />
                <asp:Button CssClass="button" runat="server" ID="btnOKMessageGralControl"  
                    Text="Cancelar" style="display: none;" 
                    meta:resourcekey="btnOKMessageGralControlResource1" />
            </td>
        </tr>
    </table>

    </asp:Panel>
        <asp:LinkButton runat="server" ID="lnkHiddenMdlPopupControl"  
    Enabled="False" meta:resourcekey="lnkHiddenMdlPopupControlResource1"/>
        <asp:ModalPopupExtender ID="mdlPopupMessageGralControl" runat="server" 
            BackgroundCssClass="modalBackground"
            PopupControlID="pnlCapturaDosisControl" 
            TargetControlID="lnkHiddenMdlPopupControl" 
            CancelControlID="btnOKMessageGralControl" DynamicServicePath="" 
    Enabled="True">
        </asp:ModalPopupExtender>

    <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />
    <div id="siembraEnviada">  
    
    <asp:Panel ID="pnlErrorMessageControl" runat="server" CssClass="modalPopup" 
            Style="display: none;" width="500px" 
            meta:resourcekey="pnlErrorMessageControlResource1">
        <table style="vertical-align:middle; text-align:center; height:100%; width:100%;">
            <tr>
                <td style="background:#ccc repeat;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div class="alerta">
                        <img src="../comun/img/error.png" runat="server" id="imgMessageGralControl"/>   
        
                        &nbsp;<asp:Label ID="lblMessageGralControl" runat="server"  
                            Text="Siembra enviada exitosamente" 
                            meta:resourcekey="lblMessageGralControlResource1"/>                    
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>

                </td>

            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button CssClass="button" runat="server" ID="Button1" Text="OK" 
                        meta:resourcekey="Button1Resource1" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton runat="server" ID="LinkButton1"  Enabled="False" 
            meta:resourcekey="LinkButton1Resource1"/>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        BackgroundCssClass="modalBackground"
        PopupControlID="pnlErrorMessageControl" 
        TargetControlID="lnkHiddenMdlPopupControl" 
        CancelControlID="btnOKMessageGralControl" DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    </div>  