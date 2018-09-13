using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Parametro
/// </summary>
public class Parametro
{
    public int idParametro;
    public string nombreParametroES;
    public string nombreParametroEN;
    public int activo;
    public int nuevo;
    public Propiedad[] Propiedades;
    public int ordenParametro;
	public Parametro()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}