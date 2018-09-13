using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Grupo
/// </summary>
public class Grupo
{
    public int idGrupo;
    public string nombreGrupoES;
    public string nombreGrupoEN;
    public float Ponderacion;
    public float PonderacionNP;
    public int AplicaPlantacion;
    public int AplicaNoPlantacion;
    public Parametro[] Parametros;
    public int activo;
    public int nuevo;
    public int ordenGrupo;

	public Grupo()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}