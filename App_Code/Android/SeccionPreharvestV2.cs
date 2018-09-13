using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de SeccionPreharvest
/// </summary>
public class SeccionPreharvestV2
{
    public SeccionPreharvestV2()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    public int seccion { get; set; }
    public String UUIDFormaA { get; set; }
    public String UUID { get; set; }
    public int estatus { get; set; }
    public int borrado { get; set; }
    public int plantulas { get; set; }
    public int posicion { get; set; }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("seccion");
        dt.Columns.Add("UUIDFormaA");
        dt.Columns.Add("UUID");
        dt.Columns.Add("estatus");
        dt.Columns.Add("borrado");
        dt.Columns.Add("plantulas");
        dt.Columns.Add("posicion");

        return dt;
    }

    public DataRow toDataRow(DataTable tabla)
    {
        DataRow dr = tabla.NewRow();

        dr["seccion"] = this.seccion;
        dr["UUIDFormaA"] = this.UUIDFormaA;
        dr["UUID"] = this.UUID;
        dr["estatus"] = this.estatus;
        dr["borrado"] = this.borrado;
        dr["plantulas"] = this.plantulas;
        dr["posicion"] = this.posicion;
        return dr;

    }

}