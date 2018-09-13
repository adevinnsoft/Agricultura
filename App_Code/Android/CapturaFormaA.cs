using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CapturaFormaA
/// </summary>
public class CapturaFormaA
{
    public CapturaFormaA()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public int cajas { get; set; }
    public int idAsociado { get; set; }
    public int idCapturaFormaA { get; set; }
    public int idCapturaFormaALocal { get; set; }
    public int idFormaA { get; set; }
    public int idFormaALocal { get; set; }
    public int surco { get; set; }
    public int seccion { get; set; }
    public int estatus { get; set; }
    public bool estimacion { get; set; }
    public String UUID { get; set; }


    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idCapturaFormaA");
        dt.Columns.Add("idCapturaFormaATab");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("cajas");
        dt.Columns.Add("seccion");
        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaATab");
        dt.Columns.Add("surco");
        dt.Columns.Add("estatus");
        dt.Columns.Add("estimacion");
        dt.Columns.Add("UUID");

        return dt;
    }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idCapturaFormaA"] = this.idCapturaFormaA;
        dr["idCapturaFormaATab"] = this.idCapturaFormaALocal;
        dr["idAsociado"] = this.idAsociado;
        dr["cajas"] = this.cajas;
        dr["seccion"] = this.seccion;
        dr["idFormaA"] = this.idFormaA;
        dr["idFormaATab"] = this.idFormaALocal;
        dr["surco"] = this.surco;
        dr["estatus"] = this.estatus;
        dr["estimacion"] = this.estimacion;
        dr["UUID"] = this.UUID;

        return dr;
    }
}