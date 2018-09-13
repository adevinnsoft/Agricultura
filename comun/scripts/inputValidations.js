function validKeys(key)
{//Flechas de dirección, suprimir y retroceso
	switch(key)
	{
		//Firefox
		//case 8: //Retroceso
		//case 37: //Left
		//case 38: //Up
		//case 39: //Rigth
		//case 40: //Down
		//case 46: //Suprimir 

		//Chrome
		case 8:
		case 37: //'%'
		case 38: //'&'
		case 39: //'''
		case 40: //'('
		//case 46: //'.'
			return true;
		default:
			return false;
	}
}

$(function () {

    var positiveInteger = /^\d+$/;
    var _decimal = /^\d*\.\d+$/;


    var nonZeroInt32 = /^(-?[123456789]\d{0,9})?$/; //int32, rango de -2^31 (-2.147.483.648) a 2^31-1 (2.147.483.647)

    var int32 = /^(-?\d{1,10})?$/; //int32, rango de -2^31 (-2.147.483.648) a 2^31-1 (2.147.483.647)


    $(".nonZeroInt32").live('focusout', function () {
        var sender = $(this);

        if (!nonZeroInt32.test(sender.val()) || Number(sender.val()) < -2147483648 || Number(sender.val()) > 2147483647) {
            sender.val('');
            return false;

        } else if (Number(sender.val()) < 0) {

            return true;

        }
    });

    $(".individualTotalInBag").live('focusout', function () {
        var sender = $(this);

        PageMethods.getLargestQuantityInBag($('#ctl00_ContentPlaceHolder1_ddlMorral').val(), function (result) {
            if (!nonZeroInt32.test(sender.val()) || Number(sender.val() * result) < -2147483648 || Number(sender.val() * result) > 2147483647) {
                sender.val('');
                popUpAlert('Este campo requiere un número entero diferente de cero.<br \> Para este morral el rango permitido es de '
								+ parseInt(-2147483648 / result) + ' a ' + parseInt(2147483647 / result), 'error');
            }
        });
    });

    $(".intValidate").live('focusout', function () {
        var este = $(this);

        if (este.val().length >= 8)
            este.val(este.val().substring(0, 8));

        if (!(positiveInteger.test($(this).val()) || $(this).val() == '')) {
            popUpAlert('Se requiere un número entero.', 'error');
            este.val('').focus();
        }
    });


    $(".intValidate").live('keypress', function (e) {
        if (validKeys(e.keyCode))
            return true
        else {
            var este = $(this);

            if (este.val().length >= 8)
                este.val(este.val().substring(0, 8));

            if (e.charCode < 48 || e.charCode > 57) {
                return false;
            }

            return true;
        }
    });


    $('.btnSave').live('click', function () {
        var send = true;

        $('.required').each(function () {
            var actual = $(this);

            if (actual.children().length > 0) {
                if ($('#' + actual.attr('id') + ' :selected').text() == "--Seleccione--") {
                    send = false;
                    $(actual).addClass("Error");
                }
            }
            else {
                if (actual.val() == '') {
                    send = false;
                    $(actual).addClass("Error");
                }
            }
        });

        if (!send)
            popUpAlert('Por favor revise que haya ingresado todas las opciones requeridas.', 'error');

        return send;
    });


  

    $('.required').on('change', function () {
        var actual = $(this);

        if (actual.children().length > 0) {
            if ($('#' + actual.attr('id') + ' :selected').text() != "--Seleccione--" || $('#' + actual.attr('id') + ' :selected').val() != 0) {
                $(actual).removeClass("Error");
            }
        }
        else {
            if (actual.val() != '') {
                $(actual).removeClass("Error");
            }
        }
    });


    $(".stringWithoutSpecialSymbols").live('focusout', function () {
        var sender = $(this);
        var specialSymbols = '<>';
        var regExpSpecialSymbols = new RegExp('^[^' + specialSymbols + ']+$');

        if (sender.val() != "") {
            if (!regExpSpecialSymbols.test(sender.val())) {
                popUpAlert('Este campo es obligatorio y en el no está permitido el uso de los simbolos "' + specialSymbols + '"'
							    + '<br />'
							    + 'Dichos simbolos han sido eliminados', 'error');

                for (var index = 0; index < specialSymbols.length; index++) {
                    sender.val(replaceAll(specialSymbols.charAt(index), '', sender.val()));
                }
            }
        }
    });

    $(".stringValidate").live('focusout', function () {
        var este = $(this);

        if (este.val().length >= 30)
            este.val(este.val().substring(0, 30));

        if (este.val().indexOf('<') != -1 || este.val().indexOf('>') != -1) {
            este.val(replaceAll('<', '', este.val()));
            este.val(replaceAll('>', '', este.val()));
        }
    });

    $(".stringValidate").live('keypress', function (e) {
        if (validKeys(e.keyCode))
            return true
        else {
            var este = $(this);

            if (este.val().length >= 30)
                este.val(este.val().substring(0, 29));

            if (e.charCode == 34//"
				|| e.charCode == 36//$
				|| e.charCode == 37//%
				|| e.charCode == 39//'
				|| e.charCode == 60//<
				|| e.charCode == 62//>
				|| e.charCode == 64//@
				|| e.charCode == 91//[
				|| e.charCode == 93//]
				|| e.charCode == 132//
				|| e.charCode == 139 || e.charCode == 155)
                return false;

            return true;
        }
    });

    $(".longStringValidate").live('focusout', function () {
        var este = $(this);

        if (este.val().length >= 250)
            este.val(este.val().substring(0, 250));

        if (este.val().indexOf('<') != -1 || este.val().indexOf('>') != -1) {
            este.val(replaceAll('<', '', este.val()));
            este.val(replaceAll('>', '', este.val()));
        }
    });

    $(".longStringValidate").live('keypress', function (e) {
        if (validKeys(e.keyCode))
            return true
        else {
            var este = $(this);

            if (este.val().length >= 250)
                este.val(este.val().substring(0, 250));

            if (e.charCode == 60 || e.charCode == 62)
                return false;
        }
    });


    
    $(".cajaChValida").live('keypress', function (e) {
        if (validKeys(e.keyCode))
            return true
        else {
            var este = $(this);
            if (este.val().length >= 30)
                este.val(este.val().substring(0, 29));
            if (e.charCode == 46 || (e.charCode > 47 && e.charCode < 58)) {

                var Cadena = este.val();
                var Search = "."
                var i = 0;
                var counter = 0;
                while (i != -1) {
                    var i = Cadena.indexOf(Search, i);
                    if (i != -1) {
                        i++;
                        counter++;
                    }
                }
                if (e.charCode == 46 && (este.val().split('.').length - 1) > 0 || counter > 1)
                    return false;
            }
            else {
                return false;
            }
        }
    });

    $(".floatValidate").live('keypress', function (e) {
        if (validKeys(e.keyCode))
            return true
        else {
            var este = $(this);
            if (este.val().length >= 30)
                este.val(este.val().substring(0, 29));
            if (e.charCode == 46 || (e.charCode > 47 && e.charCode < 58)) {

                var Cadena = este.val();
                var Search = "."
                var i = 0;
                var counter = 0;
                while (i != -1) {
                    var i = Cadena.indexOf(Search, i);
                    if (i != -1) {
                        i++;
                        counter++;
                    }
                }
                if (e.charCode == 46 && (este.val().split('.').length - 1) > 0 || counter > 1)
                    return false;
            }
            else {
                return false;
            }
        }
    });

    $(".floatValidate").live('focusout', function (e) {
        var este = $(this);
        if (este.val().length >= 30)
            este.val(este.val().substring(0, 29));
        if (!(positiveInteger.test($(this).val()) || _decimal.test($(this).val()) || $(this).val() == '')) {
            popUpAlert('Se requiere un número entero o decimal.', 'error');
            este.val('').focus();
        }
    });



    $(".int312").live('keypress', function (e) {
        var result = true;
        if (validKeys(e.keyCode))
            result = true
        else {
            var este = $(this);
            if (este.val().length >= 8) {
                este.val(este.val().substring(0, 8));
            }

            if (!(e.keyCode != 45 && e.keyCode < 48 || e.keyCode > 57)) {
                var Cadena = este.val();
                var Search = "-"
                var ban1 = false, ban2 = false
                var i = 0;
                var counter = 0;
                while (i != -1) {
                    var i = Cadena.indexOf(Search, i);
                    if (i != -1) {
                        i++;
                        if (!(Cadena[0] == "-")) {
                            ban1 = true;
                            //i++;
                            counter++;
                        }
                    }
                    if (Cadena != "") {
                        ban2 = true;
                        if (e.keyCode == 45) {
                            counter++;
                        }
                    }
                }
                if (counter > 1 && ban1 == true)
                { result = false; }
                if (counter > 0 && ban2 == true)
                { result = false; }
            }
            else {
                result = false;
            }
            return result;
        }
    });










    $(".int32").live('focusout', function () {
        var sender = $(this);

        if (!int32.test(sender.val()) || Number(sender.val()) < -2147483648 || Number(sender.val()) > 2147483647) {
            sender.val('');
            popUpAlert('Este campo requiere un número entero en el rango de -2^31 (-2.147.483.648) a 2^31-1 (2.147.483.647)', 'error');
        }
    });
});

function replaceAll(find, replace, str)
{
	return str.replace(new RegExp(find, 'g'), replace);
}

function checkTime(field) 
{ 
    var errorMsg = ""; 
    // regular expression to match required time format 
    re = /^(\d{1,2}):(\d{2})(:00)?([ap]m)?$/; 
    if(field.val() != '') 
    { 
        if(regs = field.val().match(re)) 
        { 
            if(regs[4]) 
            { 
                // 12-hour time format with am/pm 
                if(regs[1] < 1 || regs[1] > 12) 
                { 
                    errorMsg = "Valor invalido para Horas: " + regs[1]; 
                } 
            } 
            else 
            {
                // 24-hour time format 
                if (regs[1] > 23) {
                    errorMsg = "Valor invalido para Horas: " + regs[1];
                }
            } if (!errorMsg && regs[2] > 59) {
                errorMsg = "Valor invalido Para Minutos: " + regs[2];
            }
        }
        else 
        {
            errorMsg = "Formato para Hora incorrecto: " + field.val();
        }
    }
    return errorMsg;
}

$(".timeValidate").live('keypress', function (e) {
    if (validKeys(e.keyCode))
        return true
    else {
        var este = $(this);
        if (este.val().length > 5)
            este.val(este.val().substring(0, 5));
        if (e.charCode > 47 && e.charCode <= 58) {
            var Cadena = este.val();
            var Search = ":"
            var i = 0;
            var counter = 0;
            while (i != -1) {
                var i = Cadena.indexOf(Search, i);
                if (i != -1) {
                    i++;
                    counter++;
                }
            }

            if (Cadena.length == 2 && e.charCode != 58) {
                return false;
            }
            if (e.charCode == 58 && (este.val().split(':').length - 1) > 0 || counter > 1)
                return false;
        }
        else {
            return false;
        }
    }
});

$(".noValue").live('keypress', function (e) {

    if (validKeys(e.keyCode))
        return true;
    else
        return false;
});

$(".alphanumeric").live('keypress', function (e) {
 //   window.console && console.log(e.charCode);

    if (validKeys(e.keyCode) || (e.keyCode >= 48 && e.keyCode <= 122))
        return true;
    else
        return false;
});

/*
 *     Formato de hora: hh:mm
 */
function sonHorasValidas(horaInicio, horaFin) {
    if (Date.parse("Wed, 09 Aug 1995 " + horaFin + ":00") == Date.parse("Wed, 09 Aug 1995 00:00:00") || Date.parse("Wed, 09 Aug 1995 " + horaInicio + ":00") <= Date.parse("Wed, 09 Aug 1995 " + horaFin + ":00"))
        return "";
    else
          return "La Hora Inicial debe ser Menor a la Hora Final.";
}



function validarTodosLosCamposContenidosEn(idElemento) {
    $('#' + idElemento + ' .intValidate, #' + idElemento + ' .int32, #' + idElemento + ' .int').each(function () {
        if (($(this).attr('class').indexOf('requerido') >= 0 || $(this).attr('class').indexOf('required') >= 0) && $(this).val() == '') {
            $(this).addClass('error');
        }
        else {
            if ($(this).val() == '') { $(this).removeClass('error'); }
            else {
                try {
                    var valor = parseInt($(this).val());
                    if (isNaN(valor) || valor == null || valor == undefined)
                        $(this).addClass('error');
                    else
                        $(this).removeClass('error');
                } catch (e) {
                    $(this).addClass('error');
                }
            }
        }
    });
    $('#' + idElemento + ' .floatValidate, #' + idElemento + ' .decimal, #' + idElemento + ' .float').each(function () {
        if (($(this).attr('class').indexOf('requerido') >= 0 || $(this).attr('class').indexOf('required') >= 0) && $(this).val() == '') {
            $(this).addClass('error');
        }
        else {
            if ($(this).val() == '') { $(this).removeClass('error'); }
            else {
                try {
                    var valor = parseFloat($(this).val());
                    if (isNaN(valor) || valor == null || valor == undefined)
                        $(this).addClass('error');
                    else
                        $(this).removeClass('error');
                } catch (e) {
                    $(this).addClass('error');
                }
            }
        }
    });
    $('#' + idElemento + ' .string, #' + idElemento + ' .stringValidate ').each(function () {
        $(this).val(replaceAll('<', '', $(this).val()));
        $(this).val(replaceAll('>', '', $(this).val()));
        if ($(this).hasClass('requerido') || $(this).hasClass('required')) {
            if ($(this).val().trim() == '') {
                $(this).addClass('error');
            } else {
                $(this).removeClass('error');
            }
        }
        else {
            $(this).removeClass('error');
        }
    });

    $('#' + idElemento + ' select').each(function () {
        var valor = $(this).val();
        var id = $(this).attr('ID');
        if (($(this).hasClass('requerido') || $(this).hasClass('required')) && ($(this).val() == '' || $(this).val() == '0')) {
            $(this).addClass('error');
            if ($(this).next().hasClass('chosen-container')) {
                $(this).next().addClass('error');
            } else {
                //No tiene chosen
            }
        } else {
            $(this).removeClass('error');
            if ($(this).next().hasClass('chosen-container')) {
                $(this).next().removeClass('error');
            } else {
                //No tiene chosen
            }
        }
    });


    if ($('.error').length > 0)
        return false;
    else
        return true;
}