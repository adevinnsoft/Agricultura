using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Cosecha
/// </summary>
public class CosechaAndroid
{
	public CosechaAndroid()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idCosecha { get; set; }
    public int idCosechaLocal { get; set; }
    public int idActividadPrograma { get; set; }
    public int idActividadProgramaLocal { get; set; }
    public string fechaInicio { get; set; }
    public string fechaFin { get; set; }
    public int cantidadProduccion { get; set; }
    public string estimadoMedioDia { get; set; }
    public int cerrada { get; set; }
    public int estatus { get; set; }
    
}