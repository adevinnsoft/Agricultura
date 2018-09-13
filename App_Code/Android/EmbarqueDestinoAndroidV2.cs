using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de EmbarqueDestinoAndroid
/// </summary>
public class EmbarqueDestinoAndroidV2
{

    public int idEmbarqueDestinoLocal { get; set; }
    public int idEmbarqueDestino { get; set; }
    public int idEmbarqueHeaderLocal { get; set; }
    public int idEmbarqueHeader { get; set; }
    public String timestampLlegada { get; set; }
    public String timestampInicioDescarga { get; set; }
    public String timestampFinDescarga { get; set; }
    public String timestampSalida { get; set; }
    public String destino { get; set; }
    public Decimal temperaturaInicio { get; set; }
    public Decimal temperaturaFin { get; set; }
    public int idUsuarioDescarga { get; set; }
    public int idPlanta { get; set; }
    public Boolean rechazado { get; set; }
    public String comentarioRechazo { get; set; }
    public String timestampRechazo { get; set; }
    public String QRPuerta { get; set; }
    public int estatus { get; set; }
    public String UUID { get; set; }
    public String catEmbarque { get; set; }
    public int idSubPlanta { get; set; }

    public EmbarqueDestinoAndroidV2()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idEmbarqueDestino");
        dt.Columns.Add("idEmbarqueDestinoTab");
        dt.Columns.Add("idEmbarqueHeader");
        dt.Columns.Add("idEmbarqueHeaderTab");
        dt.Columns.Add("timestampLlegada");
        dt.Columns.Add("timestampInicioDescarga");
        dt.Columns.Add("timestampFinDescarga");
        dt.Columns.Add("timestampSalida");
        dt.Columns.Add("destino");
        dt.Columns.Add("temperaturaInicio");
        dt.Columns.Add("temperaturaFin");
        dt.Columns.Add("idUsuarioDescarga");
        dt.Columns.Add("idPlanta");
        dt.Columns.Add("rechazado");
        dt.Columns.Add("comentarioRechazo");
        dt.Columns.Add("timestampRechazo");
        dt.Columns.Add("estatus");
        dt.Columns.Add("QRPuerta");

        dt.Columns.Add("UUID");
        dt.Columns.Add("catEmbarque");
        dt.Columns.Add("idSubPlanta");

        return dt;
    }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idEmbarqueDestino"] = this.idEmbarqueDestino;
        dr["idEmbarqueDestinoTab"] = this.idEmbarqueDestinoLocal;
        dr["idEmbarqueHeader"] = this.idEmbarqueHeader;
        dr["idEmbarqueHeaderTab"] = this.idEmbarqueHeaderLocal;
        dr["timestampLlegada"] = this.timestampLlegada;
        dr["timestampInicioDescarga"] = this.timestampInicioDescarga;
        dr["timestampFinDescarga"] = this.timestampFinDescarga;
        dr["timestampSalida"] = this.timestampSalida;
        dr["destino"] = this.destino;
        dr["temperaturaInicio"] = this.temperaturaInicio;
        dr["temperaturaFin"] = this.temperaturaFin;
        dr["idUsuarioDescarga"] = this.idUsuarioDescarga;
        dr["idPlanta"] = this.idPlanta;
        dr["rechazado"] = this.rechazado;
        dr["comentarioRechazo"] = this.comentarioRechazo;
        dr["timestampRechazo"] = this.timestampRechazo;
        dr["estatus"] = this.estatus;
        dr["QRPuerta"] = this.QRPuerta;
        dr["UUID"] = this.UUID;
        dr["catEmbarque"] = this.catEmbarque;
        dr["idSubPlanta"] = this.idSubPlanta;

        return dr;
    }
}