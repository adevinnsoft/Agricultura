using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de GrowingCaptura
/// </summary>
public class GrowingCapturaAndroid2
{
    public GrowingCapturaAndroid2()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public int idCaptura { get; set; }
    public int idCapturaLocal { get; set; }
    public String Etiqueta { get; set; }
    public String Comentarios { get; set; }
    public String FechaCaptura { get; set; }
    public int idInvernadero { get; set; }
    public int idUsuarioCreacion { get; set; }
    public int idUsuarioModifica { get; set; }
    public int idUsuarioElimina { get; set; }
    public String FechaCreacion { get; set; }
    public String FechaModifica { get; set; }
    public String FechaElimina { get; set; }
    public Boolean Plantacion { get; set; }
    public Decimal Calificacion { get; set; }
    public Decimal CalificacionCalculada { get; set; }
    public String UUID { get; set; }
    public int estatus { get; set; }
    public String NombreGrower { get; set; }
    public String Gerente { get; set; }
    public String Lider { get; set; }
    public int idStatus { get; set; }
    public int Activo { get; set; }


    public DataTable getDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCaptura");
        dt.Columns.Add("idCapturaTab");
        dt.Columns.Add("Etiqueta");
        dt.Columns.Add("Comentarios");
        dt.Columns.Add("FechaCaptura");
        dt.Columns.Add("NombreGrower");
        dt.Columns.Add("Gerente");
        dt.Columns.Add("Lider");
        dt.Columns.Add("IdStatus");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idUsuarioCreacion");
        dt.Columns.Add("idUsuarioModifica");
        dt.Columns.Add("idUsuarioElimina");
        dt.Columns.Add("FechaCreacion");
        dt.Columns.Add("FechaModifica");
        dt.Columns.Add("FechaElimina");
        dt.Columns.Add("Plantacion");
        dt.Columns.Add("Calificacion");
        dt.Columns.Add("CalificacionCalculada");
        dt.Columns.Add("UUID");
        dt.Columns.Add("status");
        dt.Columns.Add("Activo");

        return dt;

    }

    public DataRow toDataRow()
    {
        DataTable dt = this.getDataTable();

        DataRow dr = dt.NewRow();

        dr["idCaptura"] = this.idCaptura;
        dr["idCapturaTab"] = this.idCapturaLocal;
        dr["Etiqueta"] = this.Etiqueta;
        dr["Comentarios"] = this.Comentarios;
        dr["FechaCaptura"] = this.FechaCaptura;
        dr["NombreGrower"] = this.NombreGrower;
        dr["Gerente"] = this.Gerente;
        dr["Lider"] = this.Lider;
        dr["IdStatus"] = this.idStatus;
        dr["idInvernadero"] = this.idInvernadero;
        dr["idUsuarioCreacion"] = this.idUsuarioCreacion;
        dr["idUsuarioModifica"] = this.idUsuarioModifica;
        dr["idUsuarioElimina"] = this.idUsuarioElimina;
        dr["FechaCreacion"] = this.FechaCreacion;
        dr["FechaModifica"] = this.FechaModifica;
        dr["FechaElimina"] = this.FechaElimina;
        dr["Plantacion"] = this.Plantacion ? 1 : 0;
        dr["Calificacion"] = this.Calificacion;
        dr["CalificacionCalculada"] = this.CalificacionCalculada;
        dr["UUID"] = this.UUID;
        dr["status"] = this.estatus;
        dr["Activo"] = this.Activo;

        return dr;

    }

}