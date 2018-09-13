using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CajasCapturaAndroid
/// </summary>
public class CajasCaptura
{
    public Int32 idEstimadocajas { get; set; }
    public Int32 idEstimadocajasLocal { get; set; }
    public Int32 idInvernadero { get; set; }
    public Int32 idLider { get; set; }
    public Int32 idCosecha { get; set; }
    public Int32 idCosechaLocal { get; set; }
    public Int32 surcos { get; set; }
    public Int32 semana { get; set; }
    public Boolean borrado { get; set; }
    public Int32 usuarioCaptura { get; set; }
    public Int32 usuarioModifica { get; set; }
    public String fechaCaptura { get; set; }
    public String fechaModifica { get; set; }
    public Int32 estatus { get; set; }
    public String UUID { get; set; }
}