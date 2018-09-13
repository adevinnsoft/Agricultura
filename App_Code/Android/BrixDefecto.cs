using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de BrixDefecto
/// </summary>
public class BrixDefecto
{
    public Int32 idBrixDefecto { get; set; }
    public Int32 idBrixDefectoLocal { get; set; }
    public Int32 idBrixHeader { get; set; }
    public Int32 idBrixHeaderLocal { get; set; }
    public Int32 idDefecto { get; set; }
    public Int32 idSegmento { get; set; }
    public Decimal Value { get; set; }
    public Decimal porcentaje { get; set; }
    public Int32 estatus { get; set; }
    public String UUID { get; set; }
    public BrixDefecto()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
}