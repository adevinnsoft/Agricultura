using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

public partial class Jornales_frmPronosticoDeJornales : BasePage
{
    
    [WebMethod]
    public static string generarConfiguracion(int nSemanas, int nHistoricos, int semanaPartida, int anioPartida, int idplanta)
    {

        string code = "";
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                    { "@idPlanta", idplanta == 0 ? HttpContext.Current.Session["idPlanta"].ToString():idplanta.ToString() },
                    { "@semanas", nSemanas } ,
                 //   { "@historicosAtras", nHistoricos } ,
                    { "@semanaPartida", semanaPartida } ,
                    { "@anioPartida", anioPartida } 
                };

            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ObtenerConfiguracionPronostico", parameters);

            code = "{"; 
            foreach (DataTable dt in ds.Tables)
            {
                if (dt != null && dt.Rows.Count > 0) // VALIDACIÓN DATOS DE SPR
                {
                    string json = "";

                      if (dt.Columns.Contains("anioNS"))
                        {
                            json = GetDataTableToJson(dt);
                            if (json.Length > 4)
                            {
                                code += (code.Length > 1 ? "," : "") + "\"C\":" + json + "";
                            }
                        }
                        else if (dt.Columns.Contains("idInvernaderoNoConf"))
                        {
                            json = GetDataTableToJson(dt);
                            if (json.Length > 4)
                            {
                                code += (code.Length > 1 ? "," : "") + "\"N\":" + json + "";
                            }
                        }

                }
            }
            code += "}";
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return code;
    }

    [WebMethod]
    public static string calcularPorNiveles( int nSemanas, int semanasAtras, int semanaPartida, int anioPartida)
    {

        string code = "";
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                    { "@lenguaje", HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0 } ,
                    { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() },
                    { "@semanas", nSemanas } ,
                    { "@semanasAtras", semanasAtras } ,
                    { "@semanaPartida", semanaPartida } ,
                    { "@anioPartida", anioPartida },
                    { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }
                };

            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ObtenerPronosticoJornalesPorCategorias", parameters);
            if (ds != null && ds.Tables.Count > 0) // VALIDACIÓN DATOS DE SPR
            {
                code = "{";
                string json = "";
                for (int index = 0; index < ds.Tables.Count; index++)
                {
                    DataTable dt = ds.Tables[index];
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        switch (index)
                        {
                            case 0:
                                if (dt.Columns.Contains("idLider"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"L\":" + json + "";
                                    }
                                }
                                break;
                            case 1:
                                if (dt.Columns.Contains("claveCat"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"C\":" + json + "";
                                    }
                                }
                                break;
                            case 2:
                                if (dt.Columns.Contains("semana"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"S\":" + json + "";
                                    }
                                }
                                break;
                            case 3:
                                if (dt.Columns.Contains("fijos"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"J\":" + json + "";
                                    }
                                }
                                break;
                            case 4:
                                if (dt.Columns.Contains("nombreFamilia"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"F\":" + json + "";
                                    }
                                }
                                break;
                            case 5:
                                if (dt.Columns.Contains("AsociadosEnUso"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"A\":" + json + "";
                                    }
                                }
                                break;
                            case 6:
                                if (dt.Columns.Contains("semanaPronostico"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"D\":" + json + "";
                                    }
                                }
                                break;
                            case 7:
                                if (dt.Columns.Contains("variedad"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"V\":" + json + "";
                                    }
                                }
                                break;
                            case 8:
                                if (dt.Columns.Contains("totalEtapas"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"E\":" + json + "";
                                    }
                                }
                                break;
                            case 9: // json etapas
                                if (dt.Columns.Contains("nombreCorto"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"H\":" + json + "";
                                    }
                                }
                                break;
                            case 10: // json desglose actividades
                                if (dt.Columns.Contains("cajas"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"HJ\":" + json + "";
                                    }
                                }
                                break;
                            case 11: // json ciclos plan ejecucion desactualizados
                                if (dt.Columns.Contains("ciclos"))
                                {
                                    json = GetDataTableToJson(dt);
                                    if (json.Length > 4)
                                    {
                                        code += (code.Length > 1 ? "," : "") + "\"PE\":" + json + "";
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                    }

                }
                code += "}";

            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            code = "";
        }

        return code;
    }

    [WebMethod]                   //guarda(configuraciones, totalSemana, detalle, fijos, semanaPartida, anioPartida
    public static string guarda(ConfiguracionPronostico[] configuraciones, PronosticoTotalSemana[] totalSemana
                                , PronosticoDetalle[] detalles, PronosticoFijos[] fijos, PronosticoInactivos[] inactivos, String semanaPartida
                                , String anioPartida, String nSemanas, String eficienciaHistorica
                                , PronosticoJornalesDesglose[] desglose, PronosticoJornalesEficiencia[] eficiencias)
    {

        string code = "";
        try
        {
            if (configuraciones == null || configuraciones.Length == 0
                || totalSemana == null || totalSemana.Length == 0
                || detalles == null || detalles.Length == 0
                || fijos == null || fijos.Length == 0
                || semanaPartida == null || semanaPartida.Equals("")
                || anioPartida == null || anioPartida.Equals("")
                || desglose == null || desglose.Equals("")
                || eficiencias == null || eficiencias.Equals(""))
            {
                return "ERR";
            }

            DataTable dtConfiguraciones = getDataTableConfiguracion(configuraciones);
            DataTable dtPronostico = getDataTableTotales(totalSemana);
            DataTable dtDetalle = getDataTableDetalle(detalles);
            DataTable dtFijos = getDataTableFijos(fijos);
            DataTable dtInactivos = getDataTableInactivos(inactivos);
            DataTable dtDesglose = getDataTableDesglose(desglose);
            DataTable dtEficiencias = getDataTableEficiencias(eficiencias);
            


            Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                    { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() } ,
                    { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() },
                    { "@anioPartida", anioPartida },
                    { "@semanaPartida", semanaPartida },
                    { "@nSemanas", nSemanas },
                    { "@eficienciaHistorica", eficienciaHistorica },
                    { "@ttConfiguraciones", dtConfiguraciones},
                    { "@ttTotales", dtPronostico},
                    { "@ttDetalle", dtDetalle},
                    { "@ttFijos", dtFijos},
                    { "@ttInactivos", dtInactivos},
                    { "@ttDesglose", dtDesglose},
                    { "@ttEficiencias", dtEficiencias}
                };

            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_GuardarPronosticoJornales", parameters);
            if (dt != null && dt.Rows.Count > 0) // VALIDACIÓN DATOS DE SPR
            {
                
               // RESPUESTA DE SPR
                code = dt.Rows[0][1].ToString();
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            code = "err " + ex.Message;
        }
        return code;
    }

    [WebMethod]
    public static string getAnios()
    {
        string code = "";
        try
        {

            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtenerListaUltimaSemanaAnio", null);
            if (dt != null && dt.Rows.Count > 0) // VALIDACIÓN DATOS DE SPR
            {
                // RESPUESTA DE SPR
                    code = GetDataTableToJson(dt);
            }
        }
        catch(Exception ex){
            Log.Error(ex.Message);
        }
        return code;
    }

    public static string getValue(string jsonValue)
    {
        return (jsonValue == null || jsonValue.Equals("null") || jsonValue.Equals("") ? null : jsonValue);
    }

    public static DataTable getDataTableConfiguracion(ConfiguracionPronostico[] configuraciones)
    {

        String[] cols = { "anio", "semana", "horas", "ausentismo", "capacitacion", "curva" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (ConfiguracionPronostico conf in configuraciones)
        {
            dr = dtConfiguraciones.NewRow();
            dr[cols[0]] = conf.anio;
            dr[cols[1]] = conf.semana;
            dr[cols[2]] = conf.horas;
            dr[cols[3]] = conf.ausentismo;
            dr[cols[4]] = conf.capacitacion;
            dr[cols[5]] = conf.curva;
            dtConfiguraciones.Rows.Add(dr);
        }


        return dtConfiguraciones;

    }

    public static DataTable getDataTableTotales(PronosticoTotalSemana[] totalesSemana)
    {

        String[] cols = { "idUsuario", "idLider", "nSemana", "Anio", "Semana", "TotalJornales", "TotalGerente", "totalFinal" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (PronosticoTotalSemana total in totalesSemana)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = total.idUsuario;
            dr[cols[index++]] = total.idLider;
            dr[cols[index++]] = total.nSemana;
            dr[cols[index++]] = total.anio;
            dr[cols[index++]] = total.semana;
            dr[cols[index++]] = total.totalJornales;
            dr[cols[index++]] = total.totalGerente;
            dr[cols[index]] = total.totalFinal;

            dtConfiguraciones.Rows.Add(dr);
        }


        return dtConfiguraciones;

    }

    public static DataTable getDataTableDetalle(PronosticoDetalle[] detalles)
    {

        String[] cols = { "idLider", "idUsuario", "nSemana", "semana", "anio", "idFamilia", "idCategoria", "esCosecha", "esPreparacionSuelo", "jornales" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (PronosticoDetalle detalle in detalles)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = detalle.idLider;
            dr[cols[index++]] = detalle.idUsuario;
            dr[cols[index++]] = detalle.nSemana;
            dr[cols[index++]] = detalle.semana;
            dr[cols[index++]] = detalle.anio;
            dr[cols[index++]] = detalle.idFamilia;
            if (detalle.esCosecha == 0 && detalle.idCategoria != 0)
            {
                dr[cols[index]] = detalle.idCategoria;
            }
            index++;
            dr[cols[index++]] = detalle.esCosecha;
            dr[cols[index++]] = detalle.esPreparacionSuelo;
            dr[cols[index]] = detalle.jornales;
            if (dr["jornales"] == null)
            {
                dr["jornales"] = 0;
            }
            if (dr["idFamilia"] == null)
            {
                dr["idFamilia"] = 0;
            }
            dtConfiguraciones.Rows.Add(dr);
        }


        return dtConfiguraciones;

    }

    public static DataTable getDataTableFijos(PronosticoFijos[] fijos)
    {

        String[] cols = { "idUsuario", "idLider", "Anio", "Semana", "JornalesFijos" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (PronosticoFijos item in fijos)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = item.idUsuario;
            dr[cols[index++]] = item.idLider;
            dr[cols[index++]] = item.anio;
            dr[cols[index++]] = item.semana;
            dr[cols[index]] = item.jornalesFijos;
            dtConfiguraciones.Rows.Add(dr);
        }


        return dtConfiguraciones;

    }
    public static DataTable getDataTableInactivos(PronosticoInactivos[] inactivos)
    {

        String[] cols = { "idUsuario", "idLider", "Anio", "Semana", "JornalesInactivos" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (PronosticoInactivos item in inactivos)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = item.idUsuario;
            dr[cols[index++]] = item.idLider;
            dr[cols[index++]] = item.anio;
            dr[cols[index++]] = item.semana;
            dr[cols[index]] = item.jornalesInactivos;
            dtConfiguraciones.Rows.Add(dr);
        }


        return dtConfiguraciones;

    }

    public static DataTable getDataTableDesglose(PronosticoJornalesDesglose[] desglose)
    {

        String[] cols = { 
                    "idUsuario"
	                ,"idProducto"
	                ,"idVariedad"
                    ,"Invernadero"
                    ,"Edad"
                    ,"nSemana"
                    ,"NSWeek"
                    ,"Dencidad"
                    ,"Cajas"
                    ,"idEtapa"
                    ,"Repeticion"
                    ,"HorasTotal"
                    ,"Jornales"
                        };
        DataTable dtDesglose = new DataTable();
        foreach (String col in cols)
        {
            dtDesglose.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (PronosticoJornalesDesglose item in desglose)
        {
            int index = 0;
            dr = dtDesglose.NewRow();
            dr[cols[index++]] = item.idUsuario;
            dr[cols[index++]] = item.idProducto;
            dr[cols[index++]] = item.idVariedad;
            dr[cols[index++]] = item.invernadero;
            dr[cols[index++]] = item.edad;
            dr[cols[index++]] = item.nSemana;
            dr[cols[index++]] = item.nsWeek;
            dr[cols[index++]] = item.densidad;
            dr[cols[index++]] = item.cajas;
            dr[cols[index++]] = item.idEtapa;
            dr[cols[index++]] = item.repeticiones;
            dr[cols[index++]] = item.horas;
            dr[cols[index]] = item.jornales;

            dtDesglose.Rows.Add(dr);
        }


        return dtDesglose;

    }

    public static DataTable getDataTableEficiencias(PronosticoJornalesEficiencia[] eficiencias)
    {

        String[] cols = { "idUsuario"
                 , "idEtapa"
                 , "idProducto"
                 , "idVariedad"
                 , "eficiencia" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (PronosticoJornalesEficiencia item in eficiencias)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = item.idUsuario;
            dr[cols[index++]] = item.idEtapa;
            dr[cols[index++]] = item.idProducto;
            dr[cols[index++]] = item.idVariedad;
            dr[cols[index]] = item.eficiencia;
            dtConfiguraciones.Rows.Add(dr);
        }

        return dtConfiguraciones;

    }


}