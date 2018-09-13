using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de TrasladoMermaAndroid
/// </summary>
public class TrasladoMerma
{
    public Int32 idTrasladoMerma { get; set; }
    public Int32 idTrasladoMermaLocal { get; set; }
    public Int32 idFormaA { get; set; }
    public Int32 idFormaALocal { get; set; }
    public Int32 idRazon { get; set; }
    public Int32 Cajas { get; set; }
    public String Comentarios { get; set; }
    public Int32 Estatus { get; set; }
    public String UUID { get; set; }
}