using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de BrixColor
/// </summary>
public class BrixColor
{
    public Int32 idBrixColor { get; set; }
    public Int32 idBrixColorLocal { get; set; }
    public Int32 idBrixHeader { get; set; }
    public Int32 idBrixHeaderLocal { get; set; }
    public Int32 idColor { get; set; }
    public Int32 idSegmento { get; set; }
    public Decimal Value { get; set; }
    public Decimal porcentaje { get; set; }
    public Int32 estatus { get; set; }
    public String UUID { get; set; }
	public BrixColor()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}