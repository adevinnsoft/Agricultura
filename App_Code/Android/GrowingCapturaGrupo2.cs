using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de GrowingCapturaGrupo
/// </summary>
public class GrowingCapturaGrupoAndroid2
{
    public GrowingCapturaGrupoAndroid2()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    public int idCapturaGrupo { get; set; }
    public int idCapturaGrupoLocal { get; set; }
    public int idCaptura { get; set; }
    public int idCapturaLocal { get; set; }
    public int idGrowingGrupo { get; set; }
    public Decimal Calificacion { get; set; }
    public Decimal CalificacionCalculada { get; set; }
    public String UUID { get; set; }
    public int estatus { get; set; }

    public DataTable getDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idCapturaGrupo");
        dt.Columns.Add("idCapturaGrupoTab");
        dt.Columns.Add("idCaptura");
        dt.Columns.Add("idCapturaTab");
        dt.Columns.Add("idGrowingGrupo");
        dt.Columns.Add("Calificacion");
        dt.Columns.Add("CalificacionCalculada");
        dt.Columns.Add("UUID");
        dt.Columns.Add("status");

        return dt;

    }

    public DataRow toDataRow()
    {
        DataTable dt = this.getDataTable();

        DataRow dr = dt.NewRow();

        dr["idCaptura"] = this.idCaptura;
        dr["idCapturaTab"] = this.idCapturaLocal;
        dr["idCapturaGrupo"] = this.idCapturaGrupo;
        dr["idCapturaGrupoTab"] = this.idCapturaGrupoLocal;
        dr["idGrowingGrupo"] = this.idGrowingGrupo;
        dr["Calificacion"] = this.Calificacion;
        dr["CalificacionCalculada"] = this.CalificacionCalculada;
        dr["UUID"] = this.UUID;
        dr["status"] = this.estatus;

        return dr;

    }

}