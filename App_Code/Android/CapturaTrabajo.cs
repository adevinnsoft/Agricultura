using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CapturaTrabajo
/// </summary>
public class CapturaTrabajo
{

    public CapturaTrabajo()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public String UUID { get; set; }
    public decimal calidad { get; set; }
    public int cantidadSurcos { get; set; }
    public String comentarios { get; set; }
    public int estatus { get; set; }
    public String fechaCaptura { get; set; }
    public String fechaFin { get; set; }
    public String fechaInicio { get; set; }
    public String fechaModifico { get; set; }
    public int idActividadPrograma { get; set; }
    public int idActividadProgramaLocal { get; set; }
    public int idAsociado { get; set; }
    public int idCapturaHeaderHistoria { get; set; }
    public int idCapturaHeaderHistoriaLocal { get; set; }
    public int idPeriodo { get; set; }
    public int idPeriodoLocal { get; set; }
    public int idUsuario { get; set; }        
    public int surcoFin { get; set; }
    public int surcoInicio { get; set; }



    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCapturaHeaderHistoria");
        dt.Columns.Add("idCapturaHeaderHistoriaLocal");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcoFin");
        dt.Columns.Add("horaInicio");
        dt.Columns.Add("horaFin");
        dt.Columns.Add("calidad");
        dt.Columns.Add("comentario");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaLocal");
        dt.Columns.Add("fechaModificacion");
        dt.Columns.Add("usuarioModifico");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idPeriodoLocal");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        dt.Columns.Add("fechaCapturaTableta");

        return dt;
    }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idCapturaHeaderHistoria"] = this.idCapturaHeaderHistoria;
        dr["idCapturaHeaderHistoriaLocal"] = this.idCapturaHeaderHistoriaLocal;
        dr["idAsociado"] = this.idAsociado;
        dr["surcoInicio"] = this.surcoInicio;
        dr["surcoFin"] = this.surcoFin;
        dr["horaInicio"] = this.fechaInicio;
        dr["horaFin"] = this.fechaFin;
        dr["calidad"] = this.calidad;
        dr["comentario"] = this.comentarios;
        dr["idActividadPrograma"] = this.idActividadPrograma;
        dr["idActividadProgramaLocal"] = this.idActividadProgramaLocal;
        dr["fechaModificacion"] = this.fechaModifico;
        dr["usuarioModifico"] = this.idUsuario;
        dr["idPeriodo"] = this.idPeriodo;
        dr["idPeriodoLocal"] = this.idPeriodoLocal;
        dr["cantidad"] = this.cantidadSurcos;
        dr["estatus"] = this.estatus;
        dr["UUID"] = this.UUID;
        dr["fechaCapturaTableta"] = this.fechaCaptura;

        return dr;
    }
}