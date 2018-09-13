using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de EmbarqueProductoAndroid
/// </summary>
public class EmbarqueProductoAndroid
{

    public int idEmbarqueProductoLocal { get; set; }
    public int idEmbarqueProducto { get; set; }
    public int idEmbarqueDestinoLocal { get; set; }
    public int idEmbarqueDestino { get; set; }
    public int idCatEmbarqueDetail { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }
    public String UUIDDestino { get; set; }


    public EmbarqueProductoAndroid()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idEmbarqueProducto");
        dt.Columns.Add("idEmbarqueProductoTab");
        dt.Columns.Add("idEmbarqueDestino");
        dt.Columns.Add("idEmbarqueDestinoTab");
        
        dt.Columns.Add("idCatEmbarqueDetail");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        dt.Columns.Add("UUIDDestino");

        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idEmbarqueProducto"] = this.idEmbarqueProducto;
        dr["idEmbarqueProductoTab"] = this.idEmbarqueProductoLocal;
        dr["idEmbarqueDestino"] = this.idEmbarqueDestino;
        dr["idEmbarqueDestinoTab"] = this.idEmbarqueDestinoLocal;
        
        dr["idCatEmbarqueDetail"] = this.idCatEmbarqueDetail;
        dr["estatus"] = this.estatus;
        dr["UUID"] = this.UUID;
        dr["UUIDDestino"] = this.UUIDDestino;

        return dr;

    }

}