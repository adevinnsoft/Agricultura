 String.prototype.format = function()
 {
 	var literal = this;

 	for(var i = 0; i < arguments.length; i++)
 	{
 		var regex = new RegExp('\{[' + i + ']\}', 'g');
 		literal = literal.replace(regex, arguments[i]);
 	}

 	return literal;
};

var bloqueoDePantalla = {
    transaccionTerminada: false,
    bloquearPantalla: function () {
        transaccionTerminada = false;
        $.blockUI();
    },
    indicarTerminoDeTransaccion: function () {
        transaccionTerminada = true;
    },
    desbloquearPantalla: function () {
        var intervalo = window.setInterval(function () {
            if (transaccionTerminada) {
                $.unblockUI();
                window.clearInterval(intervalo);
                transaccionTerminada = false;
            }
        }, 10);
    }
}

function ZeroIfNullOrEmpty(value) {
    if (value == '' || value == null || isNaN(value))
        return 0;
    else
        return value;
}