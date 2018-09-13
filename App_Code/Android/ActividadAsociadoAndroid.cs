using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ActividadAsociadoAndroid
/// </summary>
public class ActividadAsociadoAndroid
{
	public ActividadAsociadoAndroid()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idAsociadoActividad { get; set; }
    public int idActividadPrograma { get; set; }
    public int idActividadProgramaLocal  { get; set; }
    public int idActividadPeriodo { get; set; }
    public int idActividadPeriodoLocal { get; set; }
    public int idAsociado { get; set; }
    public int ausente { get; set; }
    public int estatus { get; set; }
}