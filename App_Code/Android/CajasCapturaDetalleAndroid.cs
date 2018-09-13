using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CajasCapturaDetalleAndroid
/// </summary>
public class CajasCapturaDetalle
{
    public Int32 idEstimadoCajasCaptura { get; set; }
    public Int32 idEstimadoCajasCapturaLocal { get; set; }
    public Int32 idEstimadoCajas { get; set; }
    public Int32 idEstimadoCajasLocal { get; set; }
    public Int32 surco { get; set; }
    public Decimal cajas { get; set; }
    public Decimal estimado { get; set; }
    public String fechaCaptura { get; set; }
    public String fechaModifica { get; set; }
    public Boolean borrado { get; set; }
    public Int32 estatus { get; set; }
    public Int32 idAsociado { get; set; }
    public Boolean asignado { get; set; }
    public String UUID { get; set; }
}