using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CapturaFormaAV2
/// </summary>
public class FoliosValidaIboundV2
{
    public FoliosValidaIboundV2()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public String folio { get; set; }
    public String fechaCaptura { get; set; }
    public String UUID { get; set; }


    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("folio");
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("UUID");

        return dt;
    }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["folio"] = this.folio;
        dr["fechaCaptura"] = this.fechaCaptura;
        dr["UUID"] = this.UUID;

        return dr;
    }
}