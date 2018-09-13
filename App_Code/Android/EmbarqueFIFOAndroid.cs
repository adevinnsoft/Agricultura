using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de EmbarqueFIFO
/// </summary>
public class EmbarqueFIFOAndroid
{

    public int idEmbarqueHeaderLocal { get; set; }
    public int idEmbarqueHeader { get; set; }
    public int idFolioEmpaque { get; set; }
    public int estatus { get; set; }

    public EmbarqueFIFOAndroid()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idEmbarqueHeader");
        dt.Columns.Add("idEmbarqueHeaderTab");
        
        dt.Columns.Add("idFolioEmpaque");
        dt.Columns.Add("estatus");

        return dt;
    }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idEmbarqueHeader"] = this.idEmbarqueHeader;
        dr["idEmbarqueHeaderTab"] = this.idEmbarqueHeaderLocal;
        
        dr["idFolioEmpaque"] = this.idFolioEmpaque;
        dr["estatus"] = this.estatus;
        return dr;
    }


}