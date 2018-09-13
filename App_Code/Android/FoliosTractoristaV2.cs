using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Monitoreo
/// </summary>
public class FoliosTractoristaV2
{
	public FoliosTractoristaV2()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public string tiempoRecoleccion { get; set; }
    public string tiempoFinalizado { get; set; }
    public string folio { get; set; }
    public int estatus { get; set; }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("folio");
        dt.Columns.Add("tiempoRecoleccion");
        dt.Columns.Add("tiempoFinalizado");
        dt.Columns.Add("estatus");

        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["folio"] = this.folio;
        dr["tiempoRecoleccion"] = this.tiempoRecoleccion;
        dr["tiempoFinalizado"] = this.tiempoFinalizado;
        dr["estatus"] = this.estatus;

        return dr;
    }
}