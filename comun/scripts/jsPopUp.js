/*
Modificacion:   luis.espinoza
Fecha:          11/06/2016
Descripcion:    se implemento la adicion de botones por array (ricardo.ramos)
se agrego metodo sobrecargado para personalizar los popup con tamaño y resize
correccion de centrado por ancho menor min-widht
*/

var _width = 500;
var _height = 150;
var _margintop = -75;
var _marginleft = -250;

$(function () {
    $(window).resize(function () {
        if ($('.jsPopUp[resize=true]').length) {
            var w = parseInt($(this).width() / 1.05);
            var h = parseInt($(this).height() / 1.2);

            $(".jsPopUp").css("width", w + "px");
            $(".jsPopUp").css("height", h + "px");
            $(".jsPopUp").css("margin-top", (h / 2 * -1) + "px");
            $(".jsPopUp").css("margin-left", (w / 2 * -1) + "px");

            $('.jsPopUp .block3').css("height", parseInt($(".jsPopUp").css("height")) - 80 + "px");
        }
    });
});

var popUp =
'<div resize="@resize" class="jsPopUp modalPopup" style="position:fixed; z-index:10001; width:@widthpx; height:@heightpx; left:50%; top:50%; margin-top:@margin-toppx; margin-left:@margin-leftpx;">'
+ '       <table style="vertical-align:middle; text-align:center; height:100%; width:100%; padding:5px !important;"><tbody>'
+ '            <tr style="vertical-align: text-top;">'
+ '                <td colspan="2" style="background:#ccc repeat;">'
+ '                    <div>'
+ '                       <div style="color: #000000;display: table;margin-top: 10px;font-weight: bolder;height:auto;max-height: 260px;overflow: auto;width: 100%;">'
+ '                          <table width="100%" style="text-align:left;"><tr style="vertical-align: top;"><td><img class="imgPopUp" src="" ></td>'
+ '                         <td><span class="txtPopUp"></span></td></tr></table>'
+ '                       </div>'
+ '                    </div>'
+ '                </td>'
+ '          </tr>'
+ '          <tr style="height: 0px;">'
+ '              <td colspan="2" id="jsPopoUpButtons">'
+ '                  <input type="button"  value="OK" id="btnClosePopUp" class="button" onclick="closeJsPopUp();">'
+ '              </td>'
+ '          </tr>'
+ '      </tbody></table>'
+ '</div>'

var popUpConfirm =
'<div resize="@resize" class="jsPopUpConfirm modalPopup" style="position:fixed; z-index:10002; width:@widthpx; height:@heightpx; left:50%; top:50%; margin-top:@margin-toppx; margin-left:@margin-leftpx;">'
+ '       <table style="vertical-align:middle; text-align:center; height:100%; width:100%; padding:5px !important;"><tbody>'
+ '            <tr style="vertical-align: text-top;">'
+ '                <td colspan="2" style="background:#ccc repeat;">'
+ '                    <div>'
+ '                       <div style="color: #000000;display: table;margin-top: 10px;font-weight: bolder;height:auto;max-height: 260px;overflow: auto;width: 100%;">'
+ '                          <table width="100%" style="text-align:left;"><tr><td style="vertical-align: top;"><img class="imgPopUp" src="" ></td>'
+ '                         <td><span class="txtPopUp"></span></td></tr></table>'
+ '                       </div>'
+ '                    </div>'
+ '                </td>'
+ '          </tr>'
+ '          <tr style="height: 0px;">'
+ '              <td colspan="2" id="jsPopoUpButtons">'
+ '                  <input type="button"  value="OK" id="btnClosePopUp" class="button" onclick="closeJsPopUp()">'
+ '              </td>'
+ '          </tr>'
+ '      </tbody></table>'
+ '</div>'

var popUpOk =
'<div resize="@resize" class="jsPopUp modalPopup" style="position:fixed; z-index:10001; width:@widthpx; height:@heightpx; left:50%; top:50%; margin-top:@margin-toppx; margin-left:@margin-leftpx;">'
+ '       <table style="vertical-align:middle; text-align:center; height:100%; width:100%; padding:5px !important;"><tbody>'
+ '            <tr style="vertical-align: text-top;">'
+ '                <td style="background:#ccc repeat;">'
+ '                    <div>'
+ '                       <div style="color: #000000;display: table;margin-top: 10px;font-weight: bolder;height: auto;max-height: 260px;overflow: auto;width: 100%;">'
+ '                          <table width="100%" style="text-align:left;"><tr><td><img class="imgPopUp" src="" ></td>'
+ '                         <td><span class="txtPopUp"></span></td></tr></table>'
+ '                       </div>'
+ '                     </div>'
+ '                 </td>'
+ '          </tr>'
+ '          <tr style="height: 0px;">'
+ '              <td colspan="2">'
+ '                  <input type="button"  value="CANCELAR" id="btnClosePopUp" class="button" onclick="closeJsPopUp()">'
+ '                  <input type="button"  value="ACEPTAR" id="btnOkPopUp" class="button" onclick="okJsPopUp()">'
+ '              </td>'
+ '          </tr>'
+ '      </tbody></table>'
+ '</div>'


var popUpCustomDiv =
'<div resize="@resize" class="jsPopUp modalPopup" style="position:fixed; z-index:10001; width:@widthpx; height:@heightpx; left:50%; top:50%; margin-top:@margin-toppx; margin-left:@margin-leftpx;">'
+ ' <div id="jsPopUpContainer"></div>'
+ ' <div id="jsPopUpButtons">'
+ '     <input type="button"  value="OK" id="btnClosePopUp" class="button" onclick="closeJsPopUp();">'
+ ' </div>'
+ '</div>'

function popUpAlert(mensaje, estado) {
    $('.jsPopUp').remove();
    $('.jsPopUpConfirm').remove();
    $('.screenBlocker2').remove();
    $('.screenBlocker').remove();

    var pop = popUp.replace("@width", _width);
    pop = pop.replace("@height", "auto");
    pop = pop.replace("@margin-top", _margintop);
    pop = pop.replace("@margin-left", _marginleft);
    pop = pop.replace("@resize", "false");

    if ($('.jsPopUp').length == 0) {
        $('body').append(pop);
        $('body').append('<div class="screenBlocker"></div>');
    }
    $('.jsPopUp #btnOkPopUp').remove();
    $('.jsPopUp #btnClosePopUp').val('OK');
    $('.jsPopUp .txtPopUp').html(mensaje);
    $('.jsPopUp .imgPopUp').attr('src', '../comun/img/' + (typeof estado !== "undefined" ? estado : "ok") + '.png');
    $('.screenBlocker').css({ 'width': screen.width * 2 + 'px', height: screen.height * 2 + 'px', position: 'fixed', top: 0, left: 0, 'z-index': 10000, 'background': 'rgba(0,0,0,0.7)' }).show();
    $('.jsPopUp').hide();
    $('.jsPopUp').fadeIn(100);
}

function popUpAlertCustom(mensaje, estado, width, height, resize) {
    $('.jsPopUp').remove();
    $('.jsPopUpConfirm').remove();
    $('.screenBlocker2').remove();
    $('.screenBlocker').remove();

    var modalwidth = $('.modalPopup').length ? parseInt($('.modalPopup').css("min-width")) : 0;
    var pop = popUp.replace("@width", (typeof width !== "undefined" ? width : _width));
    pop = pop.replace("@height", (typeof height !== "undefined" ? height : _height));
    pop = pop.replace("@margin-top", (typeof height !== "undefined" ? height / 2 * -1 : _margintop));
    pop = pop.replace("@margin-left", (typeof width !== "undefined" ? (width < modalwidth ? modalwidth : width) / 2 * -1 : _marginleft));
    pop = pop.replace("@resize", (typeof resize !== "undefined" ? resize : "false"));

    if ($('.jsPopUp').length == 0) {
        $('body').append(pop);
        $('body').append('<div class="screenBlocker"></div>');
    }
    $('.jsPopUp #btnOkPopUp').remove();
    $('.jsPopUp #btnClosePopUp').val('OK');
    $('.jsPopUp .txtPopUp').html(mensaje);
    $('.jsPopUp .imgPopUp').attr('src', '../comun/img/' + (typeof estado !== "undefined" ? estado : "ok") + '.png');
    $('.screenBlocker').css({ 'width': screen.width * 2 + 'px', height: screen.height * 2 + 'px', position: 'fixed', top: 0, left: 0, 'z-index': 10000, 'background': 'rgba(0,0,0,0.7)' }).show();
    $('.jsPopUp').hide();
    $('.jsPopUp').fadeIn(100);
}

function popUpAlertCustomMax(mensaje, estado, width, height, resize) {
    $('.jsPopUp').remove();
    $('.jsPopUpConfirm').remove();
    $('.screenBlocker2').remove();
    $('.screenBlocker').remove();
    var modalwidth = $('.modalPopup').length ? parseInt($('.modalPopup').css("min-width")) : 0;
    var pop = popUpCustomDiv.replace("@max-width", (typeof width !== "undefined" ? width : _width));
    pop = pop.replace("@max-height", (typeof height !== "undefined" ? height : _height));
    pop = pop.replace("@margin-top", (typeof height !== "undefined" ? height / 2 * -1 : _margintop));
    pop = pop.replace("@margin-left", (typeof width !== "undefined" ? (width < modalwidth ? modalwidth : width) / 2 * -1 : _marginleft));
    pop = pop.replace("@resize", (typeof resize !== "undefined" ? resize : "false"));
    
    if ($('.jsPopUp').length == 0) {
        $('body').append(pop);
        $('body').append('<div class="screenBlocker"></div>');
    }
    $('.jsPopUp #btnOkPopUp').remove();
    $('.jsPopUp #btnClosePopUp').val('OK');
    $('.jsPopUp #jsPopUpContainer').html(mensaje);
    $('.jsPopUp .imgPopUp').attr('src', '../comun/img/' + (typeof estado !== "undefined" ? estado : "ok") + '.png');
    $('.screenBlocker').css({ 'width': screen.width * 2 + 'px', height: screen.height * 2 + 'px', position: 'fixed', top: 0, left: 0, 'z-index': 10000, 'background': 'rgba(0,0,0,0.7)' }).show();
    $('.jsPopUp').hide();
    $('.jsPopUp').fadeIn(100);
}

function popUpAlertButtons(mensaje, botones, estado, width, height, resize) {
    $('.jsPopUp').remove();
    $('.jsPopUpConfirm').remove();
    $('.screenBlocker2').remove();
    $('.screenBlocker').remove();

    var modalwidth = $('.modalPopup').length ? parseInt($('.modalPopup').css("min-width")) : 0;
    var pop = popUp.replace("@width", (typeof width !== "undefined" ? width : _width));
    pop = pop.replace("@height", (typeof height !== "undefined" ? height : _height));
    pop = pop.replace("@margin-top", (typeof height !== "undefined" ? height / 2 * -1 : _margintop));
    pop = pop.replace("@margin-left", (typeof width !== "undefined" ? (width < modalwidth ? modalwidth : width) / 2 * -1 : _marginleft));
    pop = pop.replace("@resize", (typeof resize !== "undefined" ? resize : "false"));

    if ($('.jsPopUp').length == 0) {
        $('body').append(pop);
        $('body').append('<div class="screenBlocker"></div>');
    }
    $('.jsPopUp #btnOkPopUp').remove();
    $('.jsPopUp #btnClosePopUp').val('OK');
    $('.jsPopUp .txtPopUp').html(mensaje);
    $('.jsPopUp .imgPopUp').attr('src', '../comun/img/' + estado + '.png');
    $('.screenBlocker').css({ 'width': screen.width * 2 + 'px', height: screen.height * 2 + 'px', position: 'fixed', top: 0, left: 0, 'z-index': 10000, 'background': 'rgba(0,0,0,0.7)' }).show();
    if (Array.isArray(botones)) {
        $('#jsPopoUpButtons').html('');
        for (var i = 0; i < botones.length; i++) {
            $('#jsPopoUpButtons').append('<input type="button" value="' + botones[i][0] + '" onclick="' + botones[i][1] + '">');
        }
    } else {
        $('#jsPopoUpButtons').html(botones);
    }
    $('.jsPopUp').hide();
    $('.jsPopUp').fadeIn(100);
}

function popUpAlertConfirm(mensaje, botones, estado, width, height, resize) {
    $('.jsPopUp').remove();
    $('.jsPopUpConfirm').remove();
    $('.screenBlocker2').remove();
    $('.screenBlocker').remove();

    var modalwidth = $('.modalPopup').length ? parseInt($('.modalPopup').css("min-width")) : 0;
    var pop = popUpConfirm.replace("@width", (typeof width !== "undefined" ? width : _width));
    pop = pop.replace("@height", (typeof height !== "undefined" ? height : _height));
    pop = pop.replace("@margin-top", (typeof height !== "undefined" ? height / 2 * -1 : _margintop));
    pop = pop.replace("@margin-left", (typeof width !== "undefined" ? (width < modalwidth ? modalwidth : width) / 2 * -1 : _marginleft));
    pop = pop.replace("@resize", (typeof resize !== "undefined" ? resize : "false"));

    if ($('.jsPopUpConfirm').length == 0) {
        $('body').append(pop);
        $('body').append('<div class="screenBlocker2"></div>');
    }
    //$('.jsPopUp #btnOkPopUp').remove();
    $('.jsPopUpConfirm #btnClosePopUp').val('OK');
    $('.jsPopUpConfirm .txtPopUp').html(mensaje);
    $('.jsPopUpConfirm .imgPopUp').attr('src', '../comun/img/' + estado + '.png');
    $('.screenBlocker2').css({ 'width': screen.width * 2 + 'px', height: screen.height * 2 + 'px', position: 'fixed', top: 0, left: 0, 'z-index': 10000, 'background': 'rgba(0,0,0,0.7)' }).show();
    if (Array.isArray(botones)) {
        $('#jsPopoUpButtons').html('');
        for (var i = 0; i < botones.length; i++) {
            $('#jsPopoUpButtons').append('<input type="button" value="' + botones[i][0] + '" onclick="' + botones[i][1] + '">');
        }
    } else {
        $('#jsPopoUpButtons').html(botones);
    }
    $('.jsPopUpConfirm').hide();
    $('.jsPopUpConfirm').fadeIn(100);
}


function popUpAlertOption(mensaje, estado, okOption) {
    $('.jsPopUp').remove();
    $('.jsPopUpConfirm').remove();
    $('.screenBlocker2').remove();
    $('.screenBlocker').remove();

    var pop;
    if (okOption) {
        pop = popUpOk;
    }
    else {
        pop = popUp;
    }

    pop = pop.replace("@width", _width);
    pop = pop.replace("@height", "auto");
    pop = pop.replace("@margin-top", _margintop);
    pop = pop.replace("@margin-left", _marginleft);
    pop = pop.replace("@resize", "false");

    if ($('.jsPopUp').length == 0) {
        $('body').append(pop);
        $('body').append('<div class="screenBlocker"></div>');
    }

    $('.jsPopUp .txtPopUp').html(mensaje);
    $('.jsPopUp .imgPopUp').attr('src', '../comun/img/' + estado + '.png');
    $('.screenBlocker').css({ 'width': screen.width * 2 + 'px', height: screen.height * 2 + 'px', position: 'fixed', top: 0, left: 0, 'z-index': 10000, 'background': 'rgba(0,0,0,0.7)' }).show();
    $('.jsPopUp').hide();
    $('.jsPopUp').fadeIn(100);
}

function closeJsPopUp() {
    $('.jsPopUp').fadeOut(100);
    $('.screenBlocker').hide();
    return false;
}

function closeJsPopUpAux() {
    $('.jsPopUpConfirm').fadeOut(100);
    $('.screenBlocker2').hide();
    return false;
}

function okJsPopUp() {
    $('.jsPopUp').fadeOut(100);
    $('.screenBlocker').hide();
    return true;
}
