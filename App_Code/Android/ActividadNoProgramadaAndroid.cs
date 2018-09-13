using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ActividadNoProgramadaAndrodi
/// </summary>
public class ActividadNoProgramadaAndroid
{
	public ActividadNoProgramadaAndroid()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idActividadNoProgramada { get; set; }
    public int idActividadNoProgramadaLocal { get; set; }
    public int semanaProgramacion { get; set; }
    public int anioProgramacion { get; set; }
    public int idInvernadero { get; set; }
    public int idEtapa { get; set; }
    public int razon { get; set; }
    public string comentario { get; set; }
    public int idCiclo { get; set; }
    public int cantidadDeElementos { get; set; }
    public int esInterplanting { get; set; }
    public int surcoInicio { get; set; }
    public int surcoFin { get; set; }
    public int esColmena { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }
}