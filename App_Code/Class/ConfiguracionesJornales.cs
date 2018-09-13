using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ConfiguracionesJornales
/// </summary>
public class ConfiguracionesJornales
{
    public int anio;
    public int semana;
    public int horas;
    public float ausentismo;
    public float capacitacion;
    public float curva;
    public PronosticoDetalle[] pronosticoDetalle;
    public PronosticoSemana[] pronosticoSemana;
	public ConfiguracionesJornales()
	{
	
	}
}