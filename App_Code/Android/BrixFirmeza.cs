using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Firmeza
/// </summary>
public class BrixFirmeza
{
    public Int32 idBrixFirmeza { get; set; }
    public Int32 idBrixFirmezaLocal { get; set; }
    public Int32 idBrixHeader { get; set; }
    public Int32 idBrixHeaderLocal { get; set; }
    public Int32 idFirmeza { get; set; }
    public Int32 idSegmento { get; set; }
    public Decimal Value { get; set; }
    public Decimal porcentaje { get; set; }
    public Int32 estatus { get; set; }
    public String UUID { get; set; }
	public BrixFirmeza()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}