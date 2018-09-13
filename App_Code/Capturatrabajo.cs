using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Capturatrabajo
/// </summary>
public class Capturatrabajo
{
    public Capturatrabajo()
    {

    }
    public int idActividad { get; set; }
    public int idPeriodo { get; set; }
    public int idInvernadero { get; set; }
    public string comentarios { get; set; }
    public AsociadoCaptura[] asociados { get; set; }
}
