using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AsociadoCaptura
{
    public AsociadoCaptura()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idAsociado { get; set; }
    public int surcoInicio { get; set; }
    public int surcoFin { get; set; }
    public string horaInicio { get; set; }
    public string horaFin { get; set; }
    public decimal calidad { get; set; }
    public int cantidadSurcos { get; set; }
    public bool realizado { get; set; }
}
