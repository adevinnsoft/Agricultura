<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmRelacionArticulo.aspx.cs" Inherits="Almacen_frmRelacionArticulo" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>

    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="../comun/Scripts/jquery.tablesorter.pager.js"></script>
    <script src="../comun/scripts/jquery.tablesorter.widgets.js" type="text/javascript"></script>


     
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
<script type="text/javascript">
    var formularioArticulo;
    $(function () {
        //        $('.accordion').accordion({ header: 'h3', collapsible: true, heightStyle: "content", autoHeight: false });
        //Función utilizada para el acordeón

        $('#AgregarArt').live('click', function () { agregarFormularioArticulo(); })



        //        if (etapasIndicadas > etapasActuales) {
        //            for (var i = 0; i < (etapasIndicadas - etapasActuales); i++) {
        //agregarFormularioArticulo();
        //            }
        //        } else {
        //            for (var i = 0; i < (etapasActuales - etapasIndicadas); i++) {
        //                eliminarEtapa();
        //            }
        //        }
        //*******************************************************************

        //
        PageMethods.ObtenerArticulosAlmacenados(function (response) {
            $('#ArticulosGuardados tbody').html(response); //.append(formularioArticulo);
            //agregarFormularioArticulo();
            // */*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*
            window.setTimeout(function () {
                registerControls();
            }, 0);
            // */*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*
        });

        function registerControls() {
            if ($("#ArticulosGuardados").find("tbody").find("tr").size() >= 1) {
                var pagerOptions = { // Opciones para el  paginador
                    container: $("#pager"),
                    output: '{page} ' + '<%= (String)GetGlobalResourceObject("commun","de")%>' + ' {totalPages}'
                };

                $("#ArticulosGuardados").tablesorter({
                    widthFixed: true,
                    widgets: ['zebra', 'filter']
                    //,headers: { 3: { filter: false }}
                        , widgetOptions: {
                            zebra: ["gridView", "gridViewAlt"]
                            //filter_hideFilters: true // Autohide
                        }
                }).tablesorterPager(pagerOptions);
                $(".tablesorter-filter.disabled").hide(); // hide disabled filters
            }
            else {
                $("#pager").hide();
            }
        }

        $('.headerAcordion').live('click', function () { $(this).next().toggle(); })
        //
        PageMethods.tablaStockPlantas(function (response) {
            formularioArticulo = response;
            agregarFormularioArticulo();
        });
        //registerControls();
    });
    var objArray = new Array();
    function CargarArticuloPorID(idarticulo) {
        PageMethods.CargarArticuloPorId(idarticulo, function (articulo) {
            var Articulo = $.parseJSON(articulo);

            var ban = false;
            for (var uu = 0; uu <= objArray.length; uu++) {

                // if (objArray[uu] == Articulo["ArticuloES"]) {
                if (objArray[uu] == idarticulo) {
                    ban = true;
                }
                else {

                    //else

                }

            }
            //
            var verif = false; ;
            if (ban == true)
            { popUpAlert("El Artículo ya se encuentra en modo captura", "info"); }
            //
            else {
                var formularioArticuloCargado2 = $('.articulo').first();
                if ($(formularioArticuloCargado2).attr('idarticulo') == null) {
                    $(formularioArticuloCargado2).find('input[type=text]').each(function () {///////////////
                        if ($(this).val() != "")
                        { verif = true; }
                    });
                    if (verif==false)
                    { $(formularioArticuloCargado2).remove(); }
                }

                agregarFormularioArticulo();
                var formularioArticuloCargado = $('.articulo').last();

                $(formularioArticuloCargado).find('.txtArtES').val(Articulo["ArticuloES"]);
                $(formularioArticuloCargado).find('.txtArtENs').val(Articulo["ArticuloEN"]);
                $(formularioArticuloCargado).find('.ddlCategoria').val(Articulo["idCategoria"]); //.options(Articulo["idCategoria"]); //.val(Articulo["idCategoria"]);
                $(formularioArticuloCargado).find('.txtDescripcion_ES').val(Articulo["DescripcionES"]);
                $(formularioArticuloCargado).find('.ddlUnidad').val(Articulo["idUnidad"]);
                $(formularioArticuloCargado).find('.txtDescripcion_EN').val(Articulo["DescripcionEN"]);
                $(formularioArticuloCargado).find('.Activo').attr("checked", (Articulo["Activo"] == false ? null : ""));
                $(formularioArticuloCargado).attr('idarticulo', idarticulo);
                $(formularioArticuloCargado).addClass("regMod");
                //            $(formularioArticuloCargado).add

                for (var i = 0; i < Articulo.stocksplantas.length; i++) {
                    $(formularioArticuloCargado).find('.articuloStock [idPlanta="' + Articulo.stocksplantas[i].idPlanta + '"]').parent().find('[idDepartamento="' + Articulo.stocksplantas[i].idDpto + '"]').parent().find('.stockActual').text(Articulo.stocksplantas[i].StockActual).parent().find('.stockActual').attr('IdArticuloPlanta', Articulo.stocksplantas[i].IdArticuloPlanta);
                }
                //objArray.push('' + Articulo["ArticuloES"] + '');
                objArray.push('' + idarticulo + '');
            }
            //
        });
    }
    function agregarFormularioArticulo() {
        $('#divArticulo').append(formularioArticulo);
    }
    function EliminarFormularioArticulo(imgClicked) {
 
 
        $(imgClicked).parent().parent().parent().parent().parent().find('[idarticulo]').attr('idarticulo')
        if ($(imgClicked).parent().parent().parent().parent().parent().attr('idarticulo') == null) { 
       
        }
        else {
            for (var uu = 0; uu <= objArray.length; uu++) {
                if (objArray[uu] == ($(imgClicked).parent().parent().parent().parent().parent().attr('idarticulo'))) {
                    objArray[uu]= "";
                }

            }
        }
        
        
        
//        for (var uu = 0; uu <= objArray.length; uu++) {

//            if (objArray[uu] == Articulo["ArticuloES"]) {
//                ban = true;
//            }
//            else {

//                //else

//            }

//        }
//        //
////        if (ban == true)
////        { popUpAlert("El Artículo ya se encuentra en modo captura", "info"); }
//        //
//        //
        $(imgClicked).parent().parent().parent().parent().parent().remove();
    }
</script>
        <script type="text/javascript" id="fnAlmacenado">
            $(function () {
                $('#btnGuardar').click(function () {
                    //
                    //                    var Articulos = $('#divArticulo .articulo .articuloCabecera').map(function () {
                    //                        return prueba
                    //                    });
                    //
                    var a = new Array();
                    var verif2 = false;
                    //                     $(this).find('input[type=text]').each(function () {
                    $('.articulo').each(function () {///////////////
                        if ($(this).find('.txtArtES').val() == "" || $(this).find('.txtArtENs').val() == "" || $('.txtDescripcion_ES').val() == "" || $('.txtDescripcion_EN').val() == "") {
                            verif2 = true;
                        }

                    });
                    if (verif2 == true) {//|| $('.txtDescripcion_ES').val() == "" || $('.txtDescripcion_EN').val() == "") {
                        popUpAlert("Insertar datos requeridos.", "error");
                    }
                    else {

                        var Articulos = $('#divArticulo .articulo').map(function () {
                            return articulo = $(this).find('.articuloCabecera').map(function () //
                            {
                                return cabeza = {
                                    idarticulo: $(this).parent().attr('idarticulo'),
                                    ArticuloES: $(this).find('.txtArtES').val(),
                                    ArticuloEN: $(this).find('.txtArtENs').val(),
                                    DescripcionES: $(this).find('.txtDescripcion_ES').val(),
                                    DescripcionEN: $(this).find('.txtDescripcion_EN').val(),
                                    idCategoria: $(this).find('.ddlCategoria').val(),
                                    idUnidad: $(this).find('.ddlUnidad').val(),
                                    Activo: $(this).find('.Activo').prop('checked'),

                                    stocksplantas: $(this).parent().find('.articuloStock input[value!=""]').map(function () {
                                        return stock =
                                        {
                                            IdArticuloPlanta: $(this).parent().parent().find('[IdArticuloPlanta]').attr('IdArticuloPlanta'),
                                            idPlanta: $(this).parent().parent().find('td[idPlanta]').attr('idPlanta'),
                                            idDpto: $(this).parent().parent().find('td[idDepartamento]').attr('idDepartamento'),
                                            //idPlanta: $('#tablaStock tbody input:not(:blank)').parent().parent().find('td[idPlanta]').attr('idPlanta'),
                                            //idDpto: $('#tablaStock tbody input:not(:blank)').parent().parent().find('td[idDepartamento]').attr('idDepartamento'),
                                            Asignacion: $(this).val(),
                                            idMotivo: $(this).parent().parent().find('.Motivo option:selected').val()
                                        }
                                    }).get()
                                }//
                            }).get()
                        }).get();
                        //FinArregloArticulos
                        /////////////////////////////////////////////
                        var a = false;
                        $('.articuloStock input[value!=""]').parent().parent().find('.Motivo option:selected').each(function () {///////////////
                            if ($(this).val() == 0)
                            { a = true; }
                        });
                        if (a == true) {
                            popUpAlert("Insertar datos requeridos.", "error");
                        }
                        else {
                            PageMethods.AlmacenarArticulo(Articulos, function (response) {
                                popUpAlert(response[0], response[1]);
                                if (response[1] == "ok") {
                                    $('#divArticulo').html("");
                                    agregarFormularioArticulo();
                                    objArray.length = 0;
                                } else {
                                    popUpAlert(response[0],response[1]);
                                }
                                //location.assign(\'frmRelacionArticulo.aspx\');
                            });
                        }
                        //$('#ArticulosGuardados tbody').remove();
                        PageMethods.ObtenerArticulosAlmacenados(function (response) {
                            $('#ArticulosGuardados tbody').html(response); //.append(formularioArticulo);
                            $("#ArticulosGuardados").trigger('update');
                            //agregarFormularioArticulo();
                        });
                        //$('#divArticulo').html("");
                        //agregarFormularioArticulo();
                    }

                });
            });
            function ValidarNumeroNegativo(objCantidad) {
                $(objCantidad).parent().parent().find('.Motivo').val('0');
                switch (objCantidad.val()) {
                    case "-":
                        objCantidad.val("");
                        //alert ("entro al primer case");
                        break;
                    case "":
                        $(objCantidad).parent().parent().find('.Final').text("");
                        //alert("entro al segundo case");
                        break;  

                    default:
                        //alert("entro al tercero case");
                        calcularStockFinal(objCantidad);
                }
                
                //

//                if (objCantidad.val() == "-") {
//                    objCantidad.val("");
//                }
//                else { calcularStockFinal(objCantidad); }
//                if (objCantidad.val() == "") {
//                
//                }
            }
        function calcularStockFinal(objCantidad) {
                if (objCantidad.val() != "") {
                    var incremento = parseInt($(objCantidad).val());
                    var stockActual = parseInt($(objCantidad).parent().parent().find('.stockActual').text());
                    $(objCantidad).parent().parent().find('.Final').text(stockActual + incremento);
                    if (objCantidad.val() < 0) {
                        $(objCantidad).parent().parent().find('.Motivo [operacion=True]').hide(); //$('.Motivo3 [operacion=True]')
                        $(objCantidad).parent().parent().find('.Motivo [operacion=False]').show(); //$('.Motivo3 [operacion=True]')
                    }
                    else {
                        $(objCantidad).parent().parent().find('.Motivo [operacion=False]').hide(); //$('.Motivo3 [operacion=True]')
                        $(objCantidad).parent().parent().find('.Motivo [operacion=True]').show();
                    }

                }
            }

        </script>
       
    <style type="text/css">
        .style1
        {
            height: 64px;
        }
        .style2
        {
            width: 24px;
            height: 24px;
        }
        
        table.paginaCompleta
        {
            width: 100%;
            background: #F0F5E5;
            border: 1px solid #ADC995;
            padding: 15px;
            -webkit-border-radius: 5px 10px;
            -moz-border-radius: 5px 10px;
        }
        .regMod
        {
            background:rgba(150, 100, 200, 0.20) !important;
        }
        .articulo
        {      
            /*background: rgba(173, 201, 149, 0.15);*/
            display:table;
            width:100%;
            margin:3px;
        }
        .encabezadotabla
        {
            text-align: center !important;
        }
        .headerAcordion
        {
            cursor:pointer;
            width:100%;
            margin:3px 0px;
            text-align:left;
            background:#ADC995;
            padding:3px;
            margin-top: 20px;
            
        }
        #ArticulosGuardados tr:hover
        {
            cursor:pointer;
        }
        .articuloStock {width:100%; background:#fff; border:1px solid #ccc;}
        table.index tr td table.articuloStock tr th{background: #dddddd; text-align:center;}
        table.index tr td table.articuloStock tr td {text-align:center;}
        table.index tr td table.articuloStock tr:nth-of-type(even) td {background:#d6dfd0;}
        table.index tr td table.articuloStock tr td input[type="text"] {float:none;}
        table.index tr td table.articuloStock tr td select {float:none;}
        
        h3.headerAcordion 
        {
            background: #ADC995 !important;
            border:none;
            padding-bottom: 6px !important;
            padding-top: 6px !important;
            color:#fff;
            
            }
       table.index img 
       {
           position:relative;
           top:6px;
           left:30px;
           }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
    <h1>
        <asp:Label ID="lblTitle" runat="server" Text="Artículo"></asp:Label></h1>
    <asp:Panel ID="form" runat="server">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
        <asp:HiddenField ID="hdnId" runat="server" />
        <asp:HiddenField ID="hdnNombreCorto" runat="server" />
        <asp:HiddenField ID="hdnNombreCorto_EN" runat="server" />
        <table class="index" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="right" colspan="2"><h2><label class="lblAgregarTabla">Agregar Artículo</label><img id="AgregarArt" alt="Agregar Artículo" class="style2" src="../comun/img/add-icon.png" /></h2></td>
            </tr>
            <tr>
                <td colspan="2"><div id="divArticulo"></div></td>
            </tr>
            <tr>
                <td> <asp:HiddenField runat="server" Value="Añadir" ID="Accion" /></td>
                <td> 
                    <input  id="btnGuardar" type="button"  value="Guardar"/> 
                    <asp:Button ID="btn_Cancelar" runat="server" Text="Limpiar" OnClick="btn_Cancelar_Click"  ></asp:Button>
                </td>
                <td>&nbsp;</td>
           </tr>
        </table>
        </asp:Panel>

           <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
                <%--<ContentTemplate>--%>
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
                        <div>
                            <table id="ArticulosGuardados" >
                                <thead class="encabezadotabla"><tr><th>Artículo</th><th>Descripción</th><th>Categoría</th><th>Unidad</th><th>Activo</th></tr></thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
               <%-- </ContentTemplate>--%>
            <%--</asp:UpdatePanel>--%>
        <uc1:popUpMessageControl ID="popUpMessageControl1" runat="server" />

</div>
    
</asp:Content>

