using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Class1
/// </summary>
public class EmbarqueHeaderAndroidV2
{
    public int idEmbarqueHeaderLocal { get; set; }
    public int idEmbarqueHeader { get; set; }
    public String catEmbarque { get; set; }
    public String temperaturaInicio { get; set; }
    public String timestampCarga { get; set; }
    public String timestampCargaTerminada { get; set; }
    public String timestampSalida { get; set; }
    public String timestampFin { get; set; }
    public int idUsuarioCarga { get; set; }
    public int idPlanta { get; set; }
    public String nombreChofer { get; set; }
    public String placasCamion { get; set; }
    public String numCajaCamion { get; set; }
    public String selloIzquierdo { get; set; }
    public String selloDerecho { get; set; }
    public String rechazado { get; set; }
    public String comentarioRechazo { get; set; }
    public String timestampRechazo { get; set; }
    public int estatus { get; set; }
    public String QRPuerta { get; set; }
    public int offline { get; set; }
    public int idSubPlanta { get; set; }


    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        
        dr["idEmbarqueHeader"] = this.idEmbarqueHeader;
        dr["idEmbarqueHeaderTab"] = this.idEmbarqueHeaderLocal;
        dr["catEmbarque"] = this.catEmbarque;
        dr["temperaturaInicio"] = this.temperaturaInicio;
        dr["timestampCarga"] = this.timestampCarga;
        dr["timestampCargaTerminada"] = this.timestampCargaTerminada;
        dr["timestampSalida"] = this.timestampSalida;
        dr["timestampFin"] = this.timestampFin;
        dr["idUsuarioCarga"] = this.idUsuarioCarga;
        dr["idPlanta"] = this.idPlanta;
        dr["nombreChofer"] = this.nombreChofer;
        dr["placasCamion"] = this.placasCamion;
        dr["numCajaCamion"] = this.numCajaCamion;
        dr["selloIzquierdo"] = this.selloIzquierdo;
        dr["selloDerecho"] = this.selloDerecho;
        dr["rechazado"] = this.rechazado;
        dr["comentarioRechazo"] = this.comentarioRechazo;
        dr["timestampRechazo"] = this.timestampRechazo;
        dr["estatus"] = this.estatus;
        dr["QRPuerta"] = this.QRPuerta;
        dr["offline"] = this.offline;
        dr["idSubPlanta"] = this.idSubPlanta;

        return dr;
    }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        
        dt.Columns.Add("idEmbarqueHeader");
        dt.Columns.Add("idEmbarqueHeaderTab");
        dt.Columns.Add("catEmbarque");
        dt.Columns.Add("temperaturaInicio");
        dt.Columns.Add("timestampCarga");
        dt.Columns.Add("timestampCargaTerminada");
        dt.Columns.Add("timestampSalida");
        dt.Columns.Add("timestampFin");
        dt.Columns.Add("idUsuarioCarga");
        dt.Columns.Add("idPlanta");
        dt.Columns.Add("nombreChofer");
        dt.Columns.Add("placasCamion");
        dt.Columns.Add("numCajaCamion");
        dt.Columns.Add("selloIzquierdo");
        dt.Columns.Add("selloDerecho");
        dt.Columns.Add("rechazado");
        dt.Columns.Add("comentarioRechazo");
        dt.Columns.Add("timestampRechazo");
        dt.Columns.Add("estatus");
        dt.Columns.Add("QRPuerta");
        dt.Columns.Add("offline");
        dt.Columns.Add("idSubPlanta");

        return dt;
    }


    public EmbarqueHeaderAndroidV2()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }


}