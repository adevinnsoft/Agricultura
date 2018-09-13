using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Cosecha
/// </summary>
public class CosechaAndroidV2
{
	public CosechaAndroidV2()
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
    public String UUID { get; set; }
    public int proyeccion { get; set; }


    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCosecha");
        dt.Columns.Add("idCosechaTab");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaTab");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("cantidadProduccion");
        dt.Columns.Add("estimadoMedioDia");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        dt.Columns.Add("proyeccion");

        return dt;
    }

    public DataRow toDataRow()
    {
        DataRow dr = this.toDataTable().NewRow();

        dr["idCosecha"] = this.idCosecha;
        dr["idCosechaTab"] = this.idCosechaLocal;
        dr["idActividadPrograma"] = this.idActividadPrograma;
        dr["idActividadProgramaTab"] = this.idActividadProgramaLocal;
        dr["fechaInicio"] = this.fechaInicio;
        dr["fechaFin"] = this.fechaFin;
        dr["cantidadProduccion"] = this.cantidadProduccion;
        dr["estimadoMedioDia"] = this.estimadoMedioDia;
        dr["cerrada"] = this.cerrada;
        dr["estatus"] = this.estatus;
        dr["UUID"] = this.UUID;
        dr["proyeccion"] = this.proyeccion;

        return dr;

    }
}