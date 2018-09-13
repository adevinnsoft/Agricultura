using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CheckList
/// </summary>
public class CheckList
{
	public CheckList()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public int idCheckList { get; set; }
    public int idCheckListLocal { get; set; }
    public int idInvernadero { get; set; }
    public int idLiderCultivo { get; set; }
    public int idLiderCosecha { get; set; }
    public int idTipo { get; set; }
    public DateTime fechaElaboracion { get; set; }
    public string observaciones { get; set; }
    public int estatusAutorizado { get; set; }
    public int usuarioModifica { get; set; }
    public DateTime fechaModifica { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idCheckList");
        dt.Columns.Add("idCheckListLocal");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idLiderCultivo");
        dt.Columns.Add("idLiderCosecha");
        dt.Columns.Add("idTipo");
        dt.Columns.Add("fechaElaboracion");
        dt.Columns.Add("observaciones");
        dt.Columns.Add("estatusAutorizado");
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
        dr["idInvernadero"] = this.idInvernadero;
        dr["idLiderCultivo"] = this.idLiderCultivo;
        dr["idLiderCosecha"] = this.idLiderCosecha;
        dr["idTipo"] = this.idTipo;
        dr["fechaElaboracion"] = this.fechaElaboracion.ToString("yyyy-MM-dd HH:mm"); //item.fechaElaboracion.ToString("yyyy-MM-dd HH':'mm':'ss.FFF");
        dr["observaciones"] = this.observaciones;
        dr["estatusAutorizado"] = this.estatusAutorizado;
        dr["usuarioModifica"] = this.usuarioModifica;
        dr["fechaModifica"] = this.fechaModifica.ToString("yyyy-MM-dd HH:mm");
        dr["estatus"] = this.estatus;
        dr["UUID"] = this.UUID;

        return dr;
    }
}