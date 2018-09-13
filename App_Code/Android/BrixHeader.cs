using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de BrixHeader
/// </summary>
public class BrixHeader
{
    public Int32 idBrixHeader { get; set; }
    public Int32 idBrixHeaderLocal { get; set; }
    public Int32 idBrixCaptura { get; set; }
    public Int32 idBrixCapturaLocal { get; set; }
    public Int32 idProducto { get; set; }
    public Int32 idSegmento { get; set; }
    public Int32 CajasTotales { get; set; }
    public String Folio { get; set; }
    public String Comentarios { get; set; }
    public Int32 idUsuarioCaptura { get; set; }
    public String FechaCaptura { get; set; }
    public Int32 idUsuarioModifica { get; set; }
    public String FechaModifica { get; set; }
    public Int32 estatus { get; set; }
    public String UUID { get; set; }
	public BrixHeader()
	{
	
	}
}