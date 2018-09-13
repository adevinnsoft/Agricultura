using System.Data;

public class EmbarqueIncidenciaAndroid
{
    public int idEmbarqueIncidencia { get; set; }
    public int idEmbarqueIncidenciaLocal { get; set; }
    public string embarque { get; set; }
    public int idMotivo { get; set; }
    public string motivo { get; set; }
    public int tiempoRetraso { get; set; }
    public int cantidad { get; set; }
    public bool devolucion { get; set; }
    public bool cancelado { get; set; }
    public string observaciones { get; set; }
    public string fechaInicio { get; set; }
    public string fechaFin { get; set; }
    public int semana { get; set; }
    public int idOrigen { get; set; }
    public int idDestino { get; set; }
    public string origen { get; set; }
    public string destino { get; set; }
    public int activo { get; set; }
    public int estatus { get; set; }
    public int borrado { get; set; }
    public int idUsuario { get; set; }
    public int idPlanta { get; set; }
    public string UUID { get; set; }

    public EmbarqueIncidenciaAndroid()
    {
        
    }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idEmbarqueIncidencia");
        dt.Columns.Add("idEmbarqueIncidenciaTab");
        dt.Columns.Add("embarque");
        dt.Columns.Add("idMotivo");
        dt.Columns.Add("motivo");
        dt.Columns.Add("tiempoRetraso");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("devolucion");
        dt.Columns.Add("cancelado");
        dt.Columns.Add("observaciones");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("semana");
        dt.Columns.Add("idOrigen");
        dt.Columns.Add("idDestino");
        dt.Columns.Add("origen");
        dt.Columns.Add("destino");
        dt.Columns.Add("activo");
        dt.Columns.Add("estatus");
        dt.Columns.Add("borrado");
        dt.Columns.Add("idUsuario");
        dt.Columns.Add("idPlanta");
        dt.Columns.Add("UUID");
        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dr["idEmbarqueIncidencia"] = this.idEmbarqueIncidencia;
        dr["idEmbarqueIncidenciaTab"] = this.idEmbarqueIncidenciaLocal;
        dr["embarque"] = this.embarque;
        dr["idMotivo"] = this.idMotivo;
        dr["motivo"] = this.motivo;
        dr["tiempoRetraso"] = this.tiempoRetraso;
        dr["cantidad"] = this.cantidad;
        dr["devolucion"] = this.devolucion;
        dr["cancelado"] = this.cancelado;
        dr["observaciones"] = this.observaciones;
        dr["fechaInicio"] = this.fechaInicio;
        dr["fechaFin"] = this.fechaFin;
        dr["semana"] = this.semana;
        dr["idOrigen"] = this.idOrigen;
        dr["idDestino"] = this.idDestino;
        dr["origen"] = this.origen;
        dr["destino"] = this.destino;
        dr["activo"] = this.activo;
        dr["estatus"] = this.estatus;
        dr["borrado"] = this.borrado;
        dr["idUsuario"] = this.idUsuario;
        dr["idPlanta"] = this.idPlanta;
        dr["UUID"] = this.UUID;
        return dr;
    }
}