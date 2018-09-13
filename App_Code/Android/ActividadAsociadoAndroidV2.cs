using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de ActividadAsociadoAndroid
/// </summary>
public class ActividadAsociadoAndroidV2
{
	public ActividadAsociadoAndroidV2()
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
    public int borrado { get; set; }
    public String UUID { get; set; }


    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividadAsociado");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("idPeriodoTab");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("ausente");
        dt.Columns.Add("estatus");
        dt.Columns.Add("borrado");
        dt.Columns.Add("UUID");

        return dt;
    }

    public DataRow toDataRow()
    {
        DataRow dt = this.toDataTable().NewRow();

        dt["idActividadAsociado"] = this.idAsociadoActividad;
        dt["idActividad"] = this.idActividadPrograma;
        dt["idActividadTab"] = this.idActividadProgramaLocal;
        dt["idPeriodoTab"] = this.idActividadPeriodoLocal;
        dt["idPeriodo"] = this.idActividadPeriodo;
        dt["idAsociado"] = this.idAsociado;
        dt["ausente"] = this.ausente;
        dt["estatus"] = this.estatus;
        dt["borrado"] = this.borrado;
        dt["UUID"] = this.UUID;

        return dt;

    }
}