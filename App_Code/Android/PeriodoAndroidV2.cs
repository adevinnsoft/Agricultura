using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Periodo
/// </summary>
public class PeriodoAndroidV2
{
	public PeriodoAndroidV2()
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
    public int borrado { get; set; }

    public  DataTable toDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idPeriodoTab");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("surcos");
        dt.Columns.Add("inicio");
        dt.Columns.Add("fin");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        dt.Columns.Add("borrado");
        return dt;

    }

    public DataRow toDataRow()
    {
        DataRow dr = this.toDataTable().NewRow();
        dr["idPeriodo"] = this.idActividadPeriodo;
        dr["idperiodoTab"] = this.idActividadPeriodoLocal;
        dr["idActividad"] = this.idActividadPrograma;
        dr["idActividadTab"] = this.idActividadProgramaLocal;
        dr["surcos"] = this.surcos;
        dr["inicio"] = this.inicio;
        dr["fin"] = this.fin;
        dr["estatus"] = this.estatus;
        dr["UUID"] = this.UUID;
        dr["borrado"] = this.borrado;

        return dr;
    }

}