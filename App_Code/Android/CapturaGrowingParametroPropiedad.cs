using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CapturaGrowingParametroPropiedad
/// </summary>
public class CapturaGrowingParametroPropiedadAndroid
{
	public CapturaGrowingParametroPropiedadAndroid()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idcapturaPropiedad { get; set; }
    public int idCapturaPropiedadLocal { get; set; }
    public int idCaptura { get; set; }
    public int idCapturaLocal { get; set; }
    public int idGrowingPropiedad { get; set; }
    public float Calificacion { get; set; }
    public Boolean Cumplimiento { get; set; }
    public int idOpcionSeleccionada { get; set; }
    public String Texto { get; set; }
    public Double Numero { get; set; }
    public String UUID { get; set; }
    public int estatus { get; set; }

    public DataTable getDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idcapturaPropiedad");
        dt.Columns.Add("idCapturaPropiedadTab");
        dt.Columns.Add("idCaptura");
        dt.Columns.Add("idCapturaTab");
        dt.Columns.Add("idGrowingPropiedad");
        dt.Columns.Add("Calificacion");
        dt.Columns.Add("Cumplimiento");
        dt.Columns.Add("idOpcionSeleccionada");
        dt.Columns.Add("Texto");
        dt.Columns.Add("Numero");
        dt.Columns.Add("UUID");
        dt.Columns.Add("status");

        return dt;

    }

    public DataRow toDataRow()
    {
        DataTable dt = this.getDataTable();

        DataRow dr = dt.NewRow();

        dr["idcapturaPropiedad"] = this.idCaptura;
        dr["idCapturaPropiedadTab"] = this.idCapturaPropiedadLocal;
        dr["idCaptura"] = this.idCaptura;
        dr["idCapturaTab"] = this.idCapturaLocal;
        dr["idGrowingPropiedad"] = this.idGrowingPropiedad;
        dr["Calificacion"] = this.Calificacion;
        dr["Cumplimiento"] = this.Cumplimiento?1:0;
        dr["idOpcionSeleccionada"] = this.idOpcionSeleccionada;
        dr["Texto"] = this.Texto;
        dr["Numero"] = this.Numero;
        dr["UUID"] = this.UUID;
        dr["status"] = this.estatus;

        return dr;

    }

}