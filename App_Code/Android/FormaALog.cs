using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de FormaALog
/// </summary>
public class FormaALog
{
	public FormaALog()
	{
		
	}

    public String UUIDFolio { get; set; }
    public int idformaA { get; set; }
    public String folio { get; set; }
    public String fechaInicio { get; set; }
    public String fechaFin { get; set; }
    public String cerrado { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }
    public int idUsuarioModifica { get; set; }
    public String fechaModifica { get; set; }
    public String borrado { get; set; }
    public String idInvernadero { get; set; }

    public DataTable toDataTable(){
        DataTable dt = new DataTable();
        dt.Columns.Add("UUIDFolio");
        dt.Columns.Add("idformaA");
        dt.Columns.Add("folio");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("cerrado");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        dt.Columns.Add("idUsuarioModifica");
        dt.Columns.Add("fechaModifica");
        dt.Columns.Add("borrado");
        dt.Columns.Add("idInvernadero");
    
        return dt;

    }

    public DataRow toDataRow(DataTable dt){
        DataRow dr = dt.NewRow();

        dr["UUIDFolio"]=this.UUIDFolio;
        dr["idformaA"]=this.idformaA;
        dr["folio"]=this.folio;
        dr["fechaInicio"]=this.fechaInicio;
        dr["fechaFin"]=this.fechaFin;
        dr["cerrado"]=this.cerrado;
        dr["estatus"]=this.estatus;
        dr["UUID"]=this.UUID;
        dr["idUsuarioModifica"]=this.idUsuarioModifica;
        dr["fechaModifica"]=this.fechaModifica;
        dr["borrado"]=this.borrado;
        dr["idInvernadero"] = this.idInvernadero;

        return dr;
    }

}

