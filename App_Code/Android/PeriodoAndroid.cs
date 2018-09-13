using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Periodo
/// </summary>
public class PeriodoAndroid
{
	public PeriodoAndroid()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idActividadPeriodo { get; set; }
    public int idActividadPeriodoLocal { get; set; }
    public int idActividadPrograma { get; set; }
    public int idActividadProgramaLocal { get; set; }
    public DateTime inicio { get; set; }
    public DateTime fin { get; set; }
    public int surcos { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }

}