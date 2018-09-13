using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de GeneralDetailsQuality
/// </summary>
public class GeneralDetailsQuality
{
    public GeneralDetailsQuality()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public String typeOf { get; set; }
    public String idGeneral { get; set; }
    public String idColor { get; set; }
    public String idSize { get; set; }
    public String idFirmness { get; set; }
    public String Value { get; set; }
    public String ValueAcidity { get; set; }
    public String ValueRelation { get; set; }
    public String ValuePercent { get; set; }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("typeOf");
        dt.Columns.Add("idGeneral");
        dt.Columns.Add("idColor");
        dt.Columns.Add("idSize");
        dt.Columns.Add("idFirmness");
        dt.Columns.Add("Value");
        dt.Columns.Add("ValueAcidity");
        dt.Columns.Add("ValueRelation");
        dt.Columns.Add("ValuePercent");

        return dt;
    }

    public DataRow toDataRow()
    {
        DataRow dr = this.toDataTable().NewRow();

        dr["typeOf"] = String.IsNullOrEmpty(typeOf) ? "0" : this.typeOf;
        dr["idGeneral"] = String.IsNullOrEmpty(idGeneral) ? "0" : this.idGeneral;
        dr["idColor"] = String.IsNullOrEmpty(idColor) ? "0" : this.idColor;
        dr["idSize"] = String.IsNullOrEmpty(idSize) ? "0" : this.idSize;
        dr["idFirmness"] = String.IsNullOrEmpty(idFirmness) ? "0" : this.idFirmness;
        dr["Value"] = String.IsNullOrEmpty(Value) ? "0" : this.Value;
        dr["ValueAcidity"] = String.IsNullOrEmpty(ValueAcidity) ? "0" : this.ValueAcidity;
        dr["ValueRelation"] = String.IsNullOrEmpty(ValueRelation) ? "0" : this.ValueRelation;
        dr["ValuePercent"] = String.IsNullOrEmpty(ValuePercent) ? "0" : this.ValuePercent;

        return dr;
    }
}