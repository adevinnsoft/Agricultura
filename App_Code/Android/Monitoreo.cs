using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Monitoreo
/// </summary>
public class Monitoreo
{
	public Monitoreo()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public int idPlagaEnfermedadLocal { get; set; }
    public int idPlagaEnfermedad { get; set; }
    public int idInvernadero { get; set; }
    public int idPlaga { get; set; }
    public int surcoDe { get; set; } 
    public int surcoA { get; set; }
    public string fechaCaptura { get; set; }
    public int estado { get; set; }
    public int nivelInfestacion { get; set; }
    public string ubicacion { get; set; }
    public string observaciones { get; set; }
    public int idZona { get; set; }
    public int BasePlaga { get; set; }
    public int idPercance { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }


    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idPlagaEnfermedad");
        dt.Columns.Add("idPlagaEnfermedadTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idPlaga");
        dt.Columns.Add("SurcoDe");
        dt.Columns.Add("SurcoA");
        dt.Columns.Add("ubicacion");
        dt.Columns.Add("observaciones");
        dt.Columns.Add("base");
        dt.Columns.Add("estado");
        dt.Columns.Add("nivelInfestacion");
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("estatus");
        dt.Columns.Add("idPercance");
        dt.Columns.Add("idZona");
        dt.Columns.Add("UUID");

        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idPlagaEnfermedad"] = this.idPlagaEnfermedad;
        dr["idPlagaEnfermedadTab"] = this.idPlagaEnfermedadLocal;
        dr["idInvernadero"] = this.idInvernadero;
        dr["idPlaga"] = this.idPlaga;
        dr["SurcoDe"] = this.surcoDe;
        dr["SurcoA"] = this.surcoA;
        dr["Ubicacion"] = this.ubicacion;
        dr["observaciones"] = this.observaciones;
        dr["base"] = this.BasePlaga;
        dr["estado"] = this.estado;
        dr["nivelInfestacion"] = this.nivelInfestacion;
        dr["fechaCaptura"] = this.fechaCaptura;
        dr["estatus"] = this.estatus;
        dr["idPercance"] = this.idPercance;
        dr["idZona"] = this.idZona;
        dr["UUID"] = this.UUID;

        return dr;
    }
}