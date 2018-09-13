using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Propiedad
/// </summary>
public class Propiedad
{
    public int idPropiedad;
    public string nombrePropiedadES;
    public string nombrePropiedadEN;
    public int activo;
    public int nuevo;
    public int tieneCapturaNumero;
    public int tieneCapturaTexto;
    public int tieneCapturaCumplimiento;
    public Opcion[] Opciones;
    public int ordenPropiedad;
	public Propiedad()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}