using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de EmbarqueFormasAAndroid
/// </summary>
public class EmbarqueFormasAAndroid
{

    public int idEmbarqueFormaALocal { get; set; }
    public int idEmbarqueFormaA { get; set; }
    public int idEmbarqueProductoLocal { get; set; }
    public int idEmbarqueProducto { get; set; }
    public String folio { get; set; }
    public String dividido { get; set; }
    public int idFormaAEmpaque { get; set; }
    public Decimal temperaturaInicio { get; set; }
    public Decimal temperaturaFin { get; set; }
    public String horaCarga { get; set; }
    public String horaDescarga { get; set; }
    public int idPlantaDescargo { get; set; }
    public int estatus { get; set; }
    public string locacion { get; set; }
    public String UUIDProducto { get; set; }
    public int idEmbarqueDestino { get; set; }
    public int idEmbarqueDestinoLocal { get; set; }

    public EmbarqueFormasAAndroid()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idEmbarqueFormaA");
        dt.Columns.Add("idEmbarqueFormaATab");
        dt.Columns.Add("idEmbarqueProducto");
        dt.Columns.Add("idEmbarqueProductoTab");
        dt.Columns.Add("folio");
        dt.Columns.Add("dividido");
        dt.Columns.Add("idFormaAEmpaque");
        dt.Columns.Add("temperaturaInicio");
        dt.Columns.Add("temperaturaFin");
        dt.Columns.Add("horaCarga");
        dt.Columns.Add("horaDescarga");
        dt.Columns.Add("idPlantaDescargo");
        dt.Columns.Add("estatus");
        dt.Columns.Add("locacion");
        dt.Columns.Add("UUIDProducto");
        dt.Columns.Add("idEmbarqueDestino");
        dt.Columns.Add("idEmbarqueDestinoTab");
        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {

        DataRow dr = dt.NewRow();

        dr["idEmbarqueFormaA"] = this.idEmbarqueFormaA;
        dr["idEmbarqueFormaATab"] = this.idEmbarqueFormaALocal;
        dr["idEmbarqueProducto"] = this.idEmbarqueProducto;
        dr["idEmbarqueProductoTab"] = this.idEmbarqueProductoLocal;
        dr["folio"] = this.folio;
        dr["dividido"] = this.dividido;
        dr["idFormaAEmpaque"] = this.idFormaAEmpaque;
        dr["temperaturaInicio"] = this.temperaturaInicio;
        dr["temperaturaFin"] = this.temperaturaFin;
        dr["horaCarga"] = this.horaCarga;
        dr["horaDescarga"] = this.horaDescarga;
        dr["idPlantaDescargo"] = this.idPlantaDescargo;
        dr["estatus"] = this.estatus;
        dr["locacion"] = this.locacion;
        dr["UUIDProducto"] = this.UUIDProducto;
        dr["idEmbarqueDestino"] = this.idEmbarqueDestino;
        dr["idEmbarqueDestinoTab"] = this.idEmbarqueDestinoLocal;
        return dr;

    }

}