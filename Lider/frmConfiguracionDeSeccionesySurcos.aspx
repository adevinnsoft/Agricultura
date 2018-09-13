<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmConfiguracionDeSeccionesySurcos.aspx.cs"
    Inherits="pages_frmInvernaderosPorLider" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../comun/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/<%=GetGlobalResourceObject("Commun","inputValidation")%>.js"></script>
    <script src="../comun/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../comun/scripts/chosen<%=Session["Locale"].ToString() == "es-MX" ? "" : "_EN"%>.jquery.js"
        type="text/javascript"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <link href="../comun/scripts/slider/slick.css" rel="stylesheet" type="text/css" />
    <link href="../comun/scripts/slider/slick-theme.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/slider/slick.min.js" type="text/javascript"></script>
    <script src="../comun/scripts/moment.min.js" type="text/javascript"></script>
    <link href="../comun/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../comun/scripts/jquery-ui-1.7.2.min.js" type="text/javascript"></script>
   
    <script type="text/javascript">
        function GenerarGridInvernadero(obj) {
            var Secciones = document.getElementById("txtSeccion" + obj.attr('Inv')).value;
            var Surcos = document.getElementById("txtSurco" + obj.attr('Inv')).value;
            PageMethods.ObtenerContenidoInvernadero(obj.attr('Inv'), obj.attr('IdInv'), Secciones, Surcos, function (response) {
                $("#DIV" + obj.attr('Inv')).empty();
                $("#DIV" + obj.attr('Inv')).append("Seccion:<input id='txtSeccion" + obj.attr('Inv') + "' type='text' /> Surcos:<input id='txtSurco" + obj.attr('Inv') + "' type='text' /> <input type='button' IdInv='" + obj.attr('IdInv') + "' Inv='" + obj.attr('Inv') + "' value='¡Generar!' onclick='GenerarGridInvernadero($(this));' />");
                $("#DIV" + obj.attr('Inv')).append(response);
                $('#accordion').accordion('destroy').accordion();
            });
        }
        $(function () {
            $("#accordion").accordion();
            registerControls();
            $('#ctl00_ddlPlanta').change(function () {
                $('.slick-slide').html('');
                $('.slick-slide').css({ 'background-image': 'url("../comun/scripts/slider/ajax-loader.gif")',
                    'background-repeat': 'no-repeat',
                    'background-position': 'center'
                });
                PageMethods.cargaInvernaderosPlanta($('#ctl00_ddlPlanta').val(), function (response) {
                    $('#rollslider2').removeClass();
                    $('.habilidades #rollslider2').html(response);
                    setHabilidades();
                });
            });
            PageMethods.cargaInvernaderosPlanta($('#ctl00_ddlPlanta').val(), function (response) {
                $('#rollslider2').removeClass();
                $('.habilidades #rollslider2').html(response);
                setHabilidades();
            });
              setHabilidades();
        });

        function setHabilidades() {//inicializa los controles Slider en los que se muestran las habilidades además de las funciones DragDrop.
            $('#rollslider2').slick({
                slidesToShow: $('#rollslider2 div').length < 12 ? $(this).length : 12,
                slidesToScroll: $('#rollslider2 div').length > 12 ? 5 : 2,
                infinite: $('#rollslider2 div').length < 12 ? false : true,
                variableWidth: true
            });

            $('#rollslider2 .slick-slide').draggable({
                helper: function () {
                    return $(this).clone().appendTo('body').css({
                        'zIndex': 9
                    });
                },
                cursor: 'move',
                containment: 'document'
            });

            $('#rollslider2 .slick-slide').mouseup(function () {
                var selected = $(this);
                if (selected.attr('selected')) {
                    selected.removeClass('selected');
                    selected.attr('selected', false);
                    $('#DIV' + $(this).text()).empty();
                    $('#H3' + $(this).text()).empty();
                } else {
                    selected.attr('selected', true);
                    selected.addClass('selected');
                    var newDiv = "<h3 id='H3" + $(this).text() + "'>Invernadero: " + $(this).text() + "</h3><div id='DIV" + $(this).text() + "' idInv='" + $(this).attr('id') + "'>Seccion:<input id='txtSeccion" + $(this).text() + "' type='text' /> Surcos:<input id='txtSurco" + $(this).text() + "' type='text' /> <input type='button' IdInv='" + $(this).attr('id') + "' Inv='" + $(this).text() + "' value='¡Generar!' onclick='GenerarGridInvernadero($(this));' /></div>";
                    $('#accordion').append(newDiv)
                    $('#accordion').accordion('destroy').accordion();
                }
            });
        }

    </script>
    <style type="text/css">
        
        
        #calendar
        {
            width: 900px;
        }
        .slick-slide
        {
            width: 60px !important;
            cursor: pointer;
            display: none;
        }
        .stick-prev
        {
            background: inherit;
        }
        .slick-active
        {
            display: block !important;
        }
        #rollslider div
        {
            width: 800px;
        }
        #rollslider2 div
        {
            width: 800px;
        }
        .invernaderos
        {
            padding: 0 25px;
        }
        
        .habilidades
        {
            padding: 0 25px;
        }
        .clicked
        {
            background: #3f3f3f; /* Old browsers */
            background: -moz-linear-gradient(top,  #3f3f3f 0%, #cccccc 77%, #f2f2f2 99%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#3f3f3f), color-stop(77%,#cccccc), color-stop(99%,#f2f2f2)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* IE10+ */
            background: linear-gradient(to bottom,  #3f3f3f 0%,#cccccc 77%,#f2f2f2 99%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#3f3f3f', endColorstr='#f2f2f2',GradientType=0 ); /* IE6-9 */
            -webkit-box-shadow: inset 0px 3px 3px 3px rgba(0,0,0,0.5);
            box-shadow: inset 0px 3px 3px 3px rgba(0,0,0,0.5);
        }
        .selected
        {
            border: 1px solid #adc995;
            -webkit-box-shadow: 0px 0px 3px 3px #adc995;
            box-shadow: 0px 0px 3px 3px #adc995;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>
            <asp:Label ID="lblTitle" runat="server" 
                text="Secciones y Surcos "></asp:Label></h1>
        <table class="index">
            <tr>
                <td>
                    <h2>Invernaderos</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="roller" class="habilidades">
                        <div id="rollslider2"> 
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    <br />
    <div id="accordion" class="accordion">
    </div>
        <br />
    <label id="lblMensajes"></label>
    <asp:HiddenField runat="server" ID="hidEsEnEspanol"></asp:HiddenField>
    </div>
</asp:Content>
