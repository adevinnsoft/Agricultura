using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


public class PrestamoAsociadosAndroid
{
    public PrestamoAsociadosAndroid() { }

    public Int32 idPrestamoAsociado { get; set; }
    public Int32 idPrestamoAsociadoLocal { get; set; }
    public Int32 idUsuarioPresta { get; set; }
    public Int32 idUsuarioDestino { get; set; }
    public Int32 idAsociado { get; set; }
    public String fechaInicioPrestamo { get; set; }
    public String fechaDevolucion { get; set; }
    public Int32 estatus { get; set; }

    public DataTable toDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idPrestamoAsociado");
        dt.Columns.Add("idPrestamoAsociadoLocal");
        dt.Columns.Add("idUsuarioPresta");
        dt.Columns.Add("idUsuarioDestino");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("fechaInicioPrestamo");
        dt.Columns.Add("fechaDevolucion");
        dt.Columns.Add("estatus");

        return dt;
    }

    public DataRow toDataRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();

        dr["idPrestamoAsociado"] = this.idPrestamoAsociado;
        dr["idPrestamoAsociadoLocal"] = this.idPrestamoAsociadoLocal;
        dr["idUsuarioPresta"] = this.idUsuarioPresta;
        dr["idUsuarioDestino"] = this.idUsuarioDestino;
        dr["idAsociado"] = this.idAsociado;
        dr["fechaInicioPrestamo"] = this.fechaInicioPrestamo;
        dr["fechaDevolucion"] = this.fechaDevolucion;
        dr["estatus"] = this.estatus;

        return dr;
    }
}