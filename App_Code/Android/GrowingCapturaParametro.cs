using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de GrowingCapturaParametro
/// </summary>
public class GrowingCapturaParametroAndroid
{
	public GrowingCapturaParametroAndroid()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idCapturaParametro { get; set; }
    public int idCapturaParametroLocal { get; set; }
    public int idCaptura { get; set; }
    public int idCapturaLocal { get; set; }
    public int idGrowingParametro { get; set; }
    public int aplica { get; set; }
    public String UUID { get; set; }
    public int estatus { get; set; }

    public DataTable getDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idCapturaParametroTab");
        dt.Columns.Add("idCapturaParametro");
        dt.Columns.Add("idCapturaTab");
        dt.Columns.Add("idCaptura");
        dt.Columns.Add("idGrowingParametro");
        dt.Columns.Add("Aplica");
        dt.Columns.Add("UUID");
        dt.Columns.Add("status");

        return dt;

    }

    public DataRow toDataRow()
    {
        DataTable dt = this.getDataTable();

        DataRow dr = dt.NewRow();

        dr["idCapturaParametroTab"] = this.idCapturaParametroLocal;
        dr["idCapturaParametro"] = this.idCapturaParametro;
        dr["idCapturaTab"] = this.idCapturaLocal;
        dr["idCaptura"] = this.idCaptura;
        dr["idGrowingParametro"] = this.idGrowingParametro;
        dr["Aplica"] = this.aplica;
        dr["UUID"] = this.UUID;
        dr["status"] = this.estatus;

        return dr;

    }


}