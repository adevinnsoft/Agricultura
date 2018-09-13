using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Merma
/// </summary>
public class Merma
{
	public Merma()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idCosecha { get; set; }
    public int idCosechaLocal { get; set; }
    public int idMerma { get; set; }
    public int idMermaLocal { get; set; }
    public int idRazon { get; set; }
    public int cantidad { get; set; }
    public string observacion { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }
}