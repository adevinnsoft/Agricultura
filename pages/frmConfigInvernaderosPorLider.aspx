<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmConfigInvernaderosPorLider.aspx.cs" Inherits="Default2" %>

<asp:content id="Content1" contentplaceholderid="head" runat="Server">
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
    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../comun/scripts/Utilities.js"></script>
    <style type="text/css">
        .Deleted
        {
            display: none;
        }
        
        span.Active input
        {
            border: none;
            width: 40px;
            text-align: center;
            padding: 0px;
            background-color: rgba(0,0,0,0);
            box-shadow: 0px 0px 0px rgba(0, 0, 0, 0);
            -webkit-box-shadow: 0px 0px 0px rgba(0, 0, 0, 0);
            -moz-box-shadow: 0px 0px 0px rgba(0, 0, 0, 0);
        }
        
        .Active
        {
            display: inline-block;
            padding: 3px;
            margin: 2px;
            background: #D5E3C9;
        }
        
        .Active img
        {
            width: 18px;
        }
        
        .SpanLider
        {
            display: block;
        }
        
        .overlay-container
        {
            display: none;
            content: " ";
            height: 100%;
            width: 100%;
            position: fixed;
            left: 0;
            top: 43px;
            background: -moz-radial-gradient(center, ellipse cover,  rgba(127,127,127,0) 0%, rgba(127,127,127,0.9) 100%);
            background: -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(0%,rgba(127,127,127,0)), color-stop(100%,rgba(127,127,127,0.9)));
            background: -webkit-radial-gradient(center, ellipse cover,  rgba(127,127,127,0) 0%,rgba(127,127,127,0.9) 100%);
            background: -o-radial-gradient(center, ellipse cover,  rgba(127,127,127,0) 0%,rgba(127,127,127,0.9) 100%);
            background: -ms-radial-gradient(center, ellipse cover,  rgba(127,127,127,0) 0%,rgba(127,127,127,0.9) 100%);
            background: radial-gradient(center, ellipse cover,  rgba(127,127,127,0) 0%,rgba(127,127,127,0.9) 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#007f7f7f', endColorstr='#e67f7f7f',GradientType=1 );
        }
        
        .window-container
        {
            display: block;
            background: #fcfcfc;
            margin: 8em auto;
            width: 500px;
            max-width: 500px;
            padding: 10px 20px 20px;
            text-align: left;
            z-index: 3;
            border-radius: 3px;
            box-shadow: 0px 0px 30px rgba(0,0,0,0.2);
            -webkit-transition: 0.4s ease-out;
            -moz-transition: 0.4s ease-out;
            -ms-transition: 0.4s ease-out;
            -o-transition: 0.4s ease-out;
            transition: 0.4s ease-out;
            opacity: 0;
        }
        
        .zoomin
        {
            -webkit-transform: scale(1.2);
            -moz-transform: scale(1.2);
            -ms-transform: scale(1.2);
            transform: scale(1.2);
        }
        
        .window-container-visible
        {
            -webkit-transform: scale(1);
            -moz-transform: scale(1);
            -ms-transform: scale(1);
            transform: scale(1);
            opacity: 1;
        }
        
        .window-container h3
        {
            margin: 1em 0 0.5em;
            font-family: "Oleo Script";
            font-weight: normal;
            font-size: 25px;
            text-align: center;
        }
        
        .DivAcordion
        {
            position: relative;
            overflow: auto;
            max-height: 500px;
            min-height: 150px;
            border: 1px solid #ccc;
            width: 100%;
            padding-left: 15px;
            padding-right: 15px;
            padding-bottom: 15px;
            box-sizing: border-box;
        }
        
        h3.ui-accordion-header
        {
            display: block;
            text-align: left;
            padding-bottom: 10px;
            padding-left: 15px;
            padding-top: 10px;
            background: #ADC995;
            background-image: url(../comun/img/sort_desc.png);
            background-repeat: no-repeat;
            background-position: 770px 7px;
            color: #fff;
            font-size: 17px;
            border: 3px solid #f0f5e5;
            width: 785px;
            cursor: pointer;
            margin-bottom: 0;
        }
        #accordion
        {
            max-width: 800px;
        }
        
        img
        {
            width: 15px;
        }
        
        span.Active img
        {
            width: 15px;
        }
        
        span.SpanLider
        {
            margin-bottom: 8px;
            margin-top: 8px;
            
        }
        
        span.Active
        {
            /*margin-right: 5px;
            margin-left: 10px;*/
        }
        
        span.Active input[type="text"]
        {
            font-size: 11px;
        }
    </style>
    <script type="text/javascript">
        var currentActive = [];

        $(function () {
            registerControls();
            PageMethods.CargaPlantas(function (response) {
                $('#accordion').append(response);
                $('#accordion').accordion('destroy').accordion({ collapsible: true });
            });
            $('#accordion').accordion('destroy').accordion({ collapsible: true });
        });

        // Elimina los invernaderos a traves de clase de CSS:
        function EliminarInvernadero(obj) {
            $(obj).parent().removeClass('Active');
            $(obj).parent().addClass('Deleted');
        }

        function AgregarInvernadero(obj) {
            var jsonObj = $('#' + obj.attr('idPlanta')).data(obj.attr('idUsuario'));
            PageMethods.CargarInvernaderos(obj.attr('idUsuario'), obj.attr('idPlanta'), function (response) {
                type = obj.children().attr('data-type');
                $('.overlay-container').fadeIn(function () {
                    window.setTimeout(function () {
                        $('.window-container.' + type).addClass('window-container-visible');
                        checkDeleted(obj.attr('idUsuario'), obj.attr('idPlanta'));
                    }, 100);
                });
                $('#invernaderos').empty();
                $('#invernaderos').append(response);
                if (typeof (jsonObj) != "undefined") {
                    jQuery.each(jsonObj, function (i, val) {
                        $("#invernaderos input:checkbox[idInvernadero=" + val.Inv + "]").attr('checked', true);
                    });
                }
            });
        }

        function CerrarPopup() {
            $('.overlay-container').fadeOut().end().find('.window-container').removeClass('window-container-visible');
        }

        function GuardarTemporalmente(obj) {
            //$('#' + obj.attr('idPlanta')).find('.Active').empty();
            var jsonObj = [];
            var Invernadero = '';

            $("span[idUsuario=" + obj.attr('idUsuario') + "][idPlanta=" + obj.attr('idPlanta') + "]").remove();
            $("#" + obj.attr('idPlanta')).append("<br /><h3><span class='SpanLider' idPlanta='" + obj.attr('idPlanta') + "' idUsuario='" + obj.attr('idUsuario') + "'>" + obj.attr('Lider') + " <img src='../comun/img/add-icon.png' onclick='AgregarInvernadero($(this).parent());' data-type='zoomin' /></span></h3>");
            $("input:checkbox[zona]:checked").each(function () {
                $('#' + obj.attr('idPlanta')).append("<span class='Active' idInv=" + $(this).attr('idInvernadero') + " idPlanta=" + obj.attr('idPlanta') + " idUsuario=" + obj.attr('idUsuario') + "> <input type='text' value=" + $(this).attr('ClaveInvernadero') + " style='background-color: silver; color: black; width: 50px;' readonly /> <img src='../comun/img/remove-icon.png' onclick='EliminarInvernadero($(this));' /></span>");
                jsonObj.push({ "Inv": $(this).attr('idInvernadero') });
            });

            $("input:checkbox[zona]:not(:checked)").each(function () {
                if (currentActive.indexOf($(this).attr('idInvernadero')) >= 0) {
                    $('#' + obj.attr('idPlanta')).append("<span class='Deleted' idInv=" + $(this).attr('idInvernadero') + " idPlanta=" + obj.attr('idPlanta') + " idUsuario=" + obj.attr('idUsuario') + "> <input type='text' value=" + $(this).attr('ClaveInvernadero') + " style='background-color: silver; color: black; width: 50px;' readonly /> <img src='../comun/img/remove-icon.png' onclick='EliminarInvernadero($(this));' /></span>");
                }
            });

            $('#' + obj.attr('idPlanta')).data(obj.attr('idUsuario'), jsonObj)
            $('.overlay-container').fadeOut().end().find('.window-container').removeClass('window-container-visible');
        }

        function Guardar() {
            $.blockUI();
            var totalDeleted = $('.Deleted').length
            var totalActive = $('.Active').length;
            var countDeleted = 0;
            var countActive = 0;

            $('.Deleted').each(function () {
                PageMethods.EliminarAsignacion($(this).attr('idUsuario'), $(this).attr('idInv'), function (response) {
                    $('#Resultado').empty();
                    $('#Resultado').append(response);
                    countDeleted++;
                    if (totalActive == countActive && totalDeleted == countDeleted) {
                        $.unblockUI();
                        popUpAlert('Se almaceno correctamente la asignación.');
                    }
                }, function () {
                    countDeleted++;
                    if (totalActive == countActive && totalDeleted == countDeleted) {
                        $.unblockUI();
                        popUpAlert('Se almaceno correctamente la asignación.');
                    }
                });
            });

            $('.Active').each(function () {
                PageMethods.AlmacenarAsignacion($(this).attr('idUsuario'), $(this).attr('idInv'), function (response) {
                    $('#Resultado').empty();
                    $('#Resultado').append(response);
                    countActive++;
                    if (totalActive == countActive && totalDeleted == countDeleted) {
                        $.unblockUI();
                        popUpAlert('Se almaceno correctamente la asignación.');
                    }
                }, function () {
                    countActive++;
                    if (totalActive == countActive && totalDeleted == countDeleted) {
                        $.unblockUI();
                        popUpAlert('Se almaceno correctamente la asignación.');
                    }
                });
            });
        }

        function checkDeleted(idusuario, idplanta) {
            var borrado = [];
            currentActive.length = 0;

            $("span[idplanta=" + idplanta + "][idusuario=" + idusuario + "].Deleted").each(function () {
                borrado.push($(this).attr("idinv"));
            });

            $("input:checkbox:checked").each(function () {
                if (borrado.indexOf($(this).attr('idInvernadero')) >= 0) {
                    $(this).prop("checked", false);
                    currentActive.push($(this).attr('idInvernadero'));
                }
            });

            $("input:checkbox").change(function () {
                if ($(this).is(":checked")) {
                    var index = currentActive.indexOf($(this).attr('idInvernadero'));
                    if (index > -1) {
                        currentActive.splice(index, 1);
                    }
                } else {
                    currentActive.push($(this).attr('idInvernadero'));
                }
            });
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h1>
            <asp:label id="lblTitle" runat="server" text="Invernaderos por lider"></asp:label></h1>
        <br />
        <div id="accordion" class="accordion">
        </div>
        <div>
            <input type="button" onclick="Guardar();" value="Guardar" />
        </div>
        <div class="overlay-container">
            <div class="window-container zoomin">
                <h3>
                    Asignar invernaderos</h3>
                <div id="invernaderos">
                </div>
                <br />
            </div>
        </div>
        <div id="Resultado">
        </div>
    </div>
</asp:content>
