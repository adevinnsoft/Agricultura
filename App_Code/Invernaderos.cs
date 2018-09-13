using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Descripción breve de Invernaderos
/// </summary>
public class Invernaderos
{
    public int idInvernadero { get; set; }

    public string ClaveInvernadero;
    public decimal Hectarea;
    public string Zona;
    public int Grupo;
    public int Secciones;
    public bool Zonificacion;
    public bool Investigacion;
    public bool PasilloMedio;
    public bool Activo;
    public int idRancho;
	public Invernaderos()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}