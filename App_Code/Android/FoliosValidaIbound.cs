using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CapturaFormaA
/// </summary>
public class FoliosValidaIbound
{
    public FoliosValidaIbound()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public String folio { get; set; }
    public String fechaCaptura { get; set; }


    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("folio");
        dt.Columns.Add("fechaCaptura");

        return dt;
    }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["folio"] = this.folio;
        dr["fechaCaptura"] = this.fechaCaptura;

        return dr;
    }
}