using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de DataTablesSync
/// </summary>
public class DataTablesSync
{
	public DataTablesSync()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    

    public static DataTable dtActividadProgramada()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idEtapa");
        dt.Columns.Add("idCiclo");
        dt.Columns.Add("cantidadDeElementos");
        dt.Columns.Add("semana");
        dt.Columns.Add("jornalesEstimados");
        dt.Columns.Add("minutosEstimados");
        dt.Columns.Add("esDirectriz");
        dt.Columns.Add("esInterplanting");
        dt.Columns.Add("borrado");
        dt.Columns.Add("aprobadaPor");
        dt.Columns.Add("rechazadaPor");
        dt.Columns.Add("usuarioModifica");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcoFin");
        dt.Columns.Add("esColmena");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");

       
        return dt;
    }

    public static DataTable dtActividadPeriodos()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idPeriodoTab");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("surcos");
        dt.Columns.Add("inicio");
        dt.Columns.Add("fin");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;

    }

    public static DataTable dtActividadNoProgramada()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idActividadNoProgramada");
        dt.Columns.Add("idActividadNoProgramadaTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idEtapa");
        dt.Columns.Add("idCiclo");
        dt.Columns.Add("razon");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("cantidadDeElementos");
        dt.Columns.Add("semanaProgramacion");
        dt.Columns.Add("anioProgramacion");
        dt.Columns.Add("esInterplanting");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtActividadJornales()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividadAsociado");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("idPeriodoTab");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("ausente");
        dt.Columns.Add("estatus");

        return dt;
    }

    public static DataTable dtCosecha()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCosecha");
        dt.Columns.Add("idCosechaTab");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaTab");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("cantidadProduccion");
        dt.Columns.Add("estimadoMedioDia");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");

        return dt;
    }

    public static DataTable dtMerma()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idMerma");
        dt.Columns.Add("idMermaTab");
        dt.Columns.Add("idCoseha");
        dt.Columns.Add("idCosechaTab");
        dt.Columns.Add("idRazon");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("observacion");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtTrasladoMerma()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idTrasladoMerma");
        dt.Columns.Add("idTrasladoMermaLocal");
        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaALocal");
        dt.Columns.Add("idRazon");
        dt.Columns.Add("Cajas");
        dt.Columns.Add("Comentarios");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtFormaA()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaATab");
        dt.Columns.Add("idPrograma");
        dt.Columns.Add("idProgramaTab");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("prefijo");
        dt.Columns.Add("dmcCalidad");
        dt.Columns.Add("dmcMercado");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("folio");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");
        dt.Columns.Add("fechaFinTractorista");
        dt.Columns.Add("fechaInicioTractorista");
        dt.Columns.Add("storage");

        return dt;
    }

    public static DataTable dtFormaAv2()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaATab");
        dt.Columns.Add("idPrograma");
        dt.Columns.Add("idProgramaTab");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("prefijo");
        dt.Columns.Add("dmcCalidad");
        dt.Columns.Add("dmcMercado");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("folio");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");
        dt.Columns.Add("fechaFinTractorista");
        dt.Columns.Add("fechaInicioTractorista");
        dt.Columns.Add("storage");
        dt.Columns.Add("UUID");

        return dt;
    }

    public static DataTable dtFormaAToWs()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("folio");
        dt.Columns.Add("XMLData");
        dt.Columns.Add("pX");
        dt.Columns.Add("pY");
        dt.Columns.Add("pZ");

        return dt;
    }


    //public static DataTable dtCapturaFormaAToWs()
    //{
    //    DataTable dt = new DataTable();

    //    dt.Columns.Add("cajas");
    //    dt.Columns.Add("idFormaA");

    //    return dt;
    //}


    public static DataTable dtBrixCaptura()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("estatus");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaTab");
        dt.Columns.Add("idBrixCaptura");
        dt.Columns.Add("idBrixCapturaTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idSeccion");
        dt.Columns.Add("idCalidad");
        dt.Columns.Add("libras");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtBrixDetalle()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixCaptura");
        dt.Columns.Add("estatus");
        dt.Columns.Add("brix");
        dt.Columns.Add("idBrixCapturaTab");
        dt.Columns.Add("idBrixDetalle");
        dt.Columns.Add("idBrixDetalleTab");
        dt.Columns.Add("idColor");
        dt.Columns.Add("UUID");
        return dt;
    }

    /*Cajas Captura: datatables*/
    public static DataTable dtCajasCaptura()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idEstimadocajas");
        dt.Columns.Add("idEstimadocajasLocal");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idLider");
        dt.Columns.Add("idCosecha");
        dt.Columns.Add("idCosechaLocal");
        dt.Columns.Add("surcos");
        dt.Columns.Add("semana");
        dt.Columns.Add("borrado");
        dt.Columns.Add("usuarioCaptura");
        dt.Columns.Add("usuarioModifica");
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("fechaModifica");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtCajasCapturaDetalle()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idEstimadoCajasCaptura");
        dt.Columns.Add("idEstimadoCajasCapturaLocal");
        dt.Columns.Add("idEstimadoCajas");
        dt.Columns.Add("idEstimadoCajasLocal");
        dt.Columns.Add("surco");
        dt.Columns.Add("cajas");
        dt.Columns.Add("estimado");
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("fechaModifica");
        dt.Columns.Add("borrado");
        dt.Columns.Add("estatus");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("asignado");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable getDtBrixHeader()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idBrixCaptura");
        dt.Columns.Add("idBrixCapturaLocal");
        dt.Columns.Add("idProducto");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("CajasTotales");
        dt.Columns.Add("Folio");
        dt.Columns.Add("Comentarios");
        dt.Columns.Add("idUsuarioCaptura");
        dt.Columns.Add("FechaCaptura");
        dt.Columns.Add("idUsuarioModifica");
        dt.Columns.Add("FechaModifica");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable getDtBrixFirmeza()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixFirmeza");
        dt.Columns.Add("idBrixFirmezaLocal");
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idFirmeza");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("Value");
        dt.Columns.Add("Porcentaje");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable getDtBrixColor()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixColor");
        dt.Columns.Add("idBrixColorLocal");
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idColor");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("Value");
        dt.Columns.Add("Porcentaje");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable getDtBrixDefecto()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixDefecto");
        dt.Columns.Add("idBrixDefectoLocal");
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idDefecto");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("Value");
        dt.Columns.Add("Porcentaje");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtFoliosEmbarques()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Folio");
        return dt;
    }
}