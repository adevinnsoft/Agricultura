using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CheckCriterio
/// </summary>
public class CheckCriterio
{
	public CheckCriterio()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idCheckList { get; set; }
    public int idCheckListLocal { get; set; }
    public int idCheckCriterio { get; set; }
    public int idCheckCriterioLocal { get; set; }
    public int idCriterio { get; set; }
    public string compromiso { get; set; }
    public int idUsuario { get; set; }
    public DateTime fechaModifica { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idCheckList");
        dt.Columns.Add("idCheckListLocal");
        dt.Columns.Add("idCheckCriterio");
        dt.Columns.Add("idCheckCriterioLocal");
        dt.Columns.Add("idCriterio");
        dt.Columns.Add("compromiso");
        dt.Columns.Add("usuarioModifica");
        dt.Columns.Add("fechaModifica");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");

        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idCheckList"] = this.idCheckList;
        dr["idCheckListLocal"] = this.idCheckListLocal;
        dr["idCheckCriterio"] = this.idCheckCriterio;
        dr["idCheckCriterioLocal"] = this.idCheckCriterioLocal;
        dr["idCriterio"] = this.idCriterio;
        dr["compromiso"] = this.compromiso;
        dr["usuarioModifica"] = this.idUsuario;
        dr["fechaModifica"] = this.fechaModifica.ToString("yyyy-MM-dd");
        dr["estatus"] = this.estatus;
        dr["UUID"] = this.UUID;

        return dr;
    }
}