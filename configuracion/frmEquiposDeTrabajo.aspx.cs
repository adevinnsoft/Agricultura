using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;


public partial class frmReporteEficiencias : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmReporteEficiencias));


    private static string STR_TD = "td";
    private static string STR_TH = "th";
    private static string STR_TR = "tr";
    private static string STR_THEAD = "thead";
    private static string STR_TBODY = "tbody";
    private static string STR_TABLE = "table";
    private static string STR_DIV = "div";
    private static string STR_OPTION = "option";
    private static string STR_ATTR_VALUE = " value=\"{0}\"";
    private static string STR_ATTR_ID_EMPLEADO = " idEmpleado=\"{0}\"";
    private static string STR_ITEM_LIST = "<div class=\"divItemAsociado\" idEmpleado=\"{0}\"><span>{1}</span><img src=\"../comun/img/add-icon.png\" alt=\"[+]\" class=\"addAsociado\"/> </div>";
    private static string COL_idEquipoTrabajo = "idEquipoTrabajo";
    private static string COL_NombreEquipo = "NombreEquipo";
    private static string COL_idEmpleadoAsociado = "idEmpleadoAsociado";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region WebMethods
    [WebMethod]
    public static string[] precargaDatos()
    {
        StringBuilder response = new StringBuilder();
        StringBuilder ddlOptions = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();

            if (HttpContext.Current.Session["idRole"].ToString() == "2" || HttpContext.Current.Session["idRole"].ToString() == "5" || HttpContext.Current.Session["idRole"].ToString() == "4")
            {
                DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_LiderObtenerPorPlanta"
                    , new Dictionary<string, object>() {
                    { "@idPlanta",     (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) }
                   });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ddlOptions.Append(
                            tagGen(
                               STR_OPTION
                             , string.Format(STR_ATTR_VALUE, row["idLider"].ToString()) + string.Format(STR_ATTR_ID_EMPLEADO, row["idEmpleado"].ToString())
                             , row["vNombre"].ToString()
                            ));
                    }
                    response.Append(ddlOptions);
                }
                else
                {
                    return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
                }
                return new string[] { "1", "ok", response.ToString() };
            }
            else
            {
                 ddlOptions.Append(tagGen(
                               STR_OPTION
                             , string.Format(STR_ATTR_VALUE, HttpContext.Current.Session["idUsuario"].ToString()) + string.Format(STR_ATTR_ID_EMPLEADO, HttpContext.Current.Session["idEmpleado"].ToString())
                             , HttpContext.Current.Session["Nombre"].ToString()
                            ));
                 response.Append(ddlOptions);
                 return new string[] { "1", "ok", response.ToString() };
            }
        }
        catch (Exception e)
        {
            log.Error(e.Message);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
        }
    }



    [WebMethod]
    public static string[] obtenerAsociado(string claveLider)
    {
        StringBuilder listaAsociados = new StringBuilder();
        StringBuilder ddlOptions = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_obtenerEquipoTrabajo"
                , new Dictionary<string, object>() {
                    { "@idLider", claveLider}
                    ,{ "@idPlanta",     (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) }
                   });
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    ddlOptions.Append(
                        string.Format(STR_ITEM_LIST
                         , row["IDEmployee"].ToString()
                         , row["FullName"].ToString()
                        ));
                }
                listaAsociados.Append(ddlOptions);
                
            }
            else
            {
                return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            }
            return new string[] { "1", "ok", listaAsociados.ToString() };
        }
        catch (Exception e)
        {
            log.Error(e.Message);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
        }
    }



    [WebMethod]
    public static string[] obtenerDatosDeLider(string claveLider)
    {
      //  StringBuilder listaAsociados = new StringBuilder();
        StringBuilder JSONAsociados = new StringBuilder();
        StringBuilder JSONEquipo = new StringBuilder();
        int contador = 1;

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_obtenerEquipoTrabajo"
                , new Dictionary<string, object>() {
                    { "@idLider", claveLider}
                    ,{ "@idPlanta",     (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) }
                   });
            if (ds.Tables[0].Rows.Count > 0)
            {
                JSONAsociados.Append("[");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    
                   
                    JSONAsociados
                        .Append(JSONAsociados.Length > 1 ? "," : string.Empty)
                        .Append("{")
                        .Append(string.Format(" \"idEmpleadoAsociado\":{0}, \"nombre\":\"{1}\" , \"consecutivo\":\"{2}\""
                                    , row["IDEmployee"].ToString()
                                    , row["FullName"].ToString()
                                    , contador.ToString()
                                    ))
                         .Append("}");
                    contador = contador + 1;

                }
                JSONAsociados.Append("]");
                if (ds.Tables[1].Rows.Count > 0)
                {
                    JSONEquipo.Append("[");
                    foreach (DataRow rowEquipo in ds.Tables[1].Rows)
                    {
                        StringBuilder listaEquipoAsociados = new StringBuilder();
                        foreach (DataRow rowAsociado in ds.Tables[2].Rows)
                        {
                            if (Int32.Parse(rowEquipo[COL_idEquipoTrabajo].ToString()) == Int32.Parse(rowAsociado[COL_idEquipoTrabajo].ToString()))
                            {
                                listaEquipoAsociados
                                    .Append(listaEquipoAsociados.Length > 0 ? "," : string.Empty)
                                    .Append(rowAsociado[COL_idEmpleadoAsociado]);
                            }
                        }
                        JSONEquipo
                            .Append(JSONEquipo.Length > 1 ? "," : string.Empty)
                            .Append("{")
                            .Append(string.Format("\"idEquipo\":{0}, \"equipo\":\"{1}\", \"asociados\":[{2}]"
                                , rowEquipo[COL_idEquipoTrabajo]
                                , rowEquipo[COL_NombreEquipo]
                                , listaEquipoAsociados.ToString()))
                            .Append("}");
                    }
                    JSONEquipo.Append("]");
                }
                else
                {
                    // no hay grupos

                }
            }
            else
            {
                return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            }
            return new string[] { "1", "ok", JSONAsociados.ToString(), JSONEquipo.ToString() };
        }
        catch (Exception e)
        {
            log.Error(e.Message);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
        }
    }

    [WebMethod]
    public static string[] guardar(EquipoTrabajo[] equipos, EquipoTrabajoAsociado[] asociados)
    {
        try
        {
            DataAccess dataAccess = new DataAccess();
            Dictionary<String, Object> dictionary = new Dictionary<string, object>() {
                     {"@idUsuario", HttpContext.Current.Session["idUsuario"]}
                    ,{"@idPlanta" , HttpContext.Current.Session["idPlanta"]}
                    ,{"@ttEquipos", getDataTableEquipos(equipos)}
                    ,{"@ttAsociados", getDataTableEquiposAsociados(asociados)}
            };
            DataTable dt = dataAccess.executeStoreProcedureDataTable("spr_GuardarEquiposTrabajo", dictionary);
            if( dt != null && dt.Rows != null && dt.Rows.Count > 0){
            return new String[] {"1","OK","Cambios Guardados Con Exito"};
            }else {
                return new String[] { "0", "El proceso no generó ningún resultado", "warning" };
            }
        }
        catch (Exception e)
        {
            log.Error(e.Message);
            return new String[] { "0", "El proceso no generó ningún resultado", "warning" };
        }

    }
    
    
    
    
    #endregion
    #region TOOLS
    private static String tagGen(string tag, string attr, string value)
    {
        return String.Format("<{0} {1}>{2}</{0}>", tag, attr, value);
    }
    private static String tagGen(string tag, string value)
    {
        return tagGen(tag, string.Empty, value);
    }

    public static DataTable getDataTableEquipos(EquipoTrabajo[] equipos)
    {

        String[] cols = {
                   "idEquipoTrabajo"
                 , "idEquipoTrabajoTemp"
                 , "idEmpleadoLider"
                 , "idLider"
                 , "NombreEquipo"
                 , "Modificado"
                 , "Eliminado" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (EquipoTrabajo item in equipos)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = item.idEquipo;
            dr[cols[index++]] = item.idEquipoTemp;
            dr[cols[index++]] = item.idEmpleadoLider;
            dr[cols[index++]] = item.idLider;
            dr[cols[index++]] = item.equipo;
            dr[cols[index++]] = item.modificado;
            dr[cols[index]] = item.eliminado;
            dtConfiguraciones.Rows.Add(dr);

        }
        return dtConfiguraciones;
    }


    public static DataTable getDataTableEquiposAsociados(EquipoTrabajoAsociado[] asociados)
    {

        String[] cols = {
                   "idEquipoTrabajoTemp"
                 , "idEmpleadoAsociado" };
        DataTable dtConfiguraciones = new DataTable();
        foreach (String col in cols)
        {
            dtConfiguraciones.Columns.Add(col);
        }

        DataRow dr = null;
        foreach (EquipoTrabajoAsociado item in asociados)
        {
            int index = 0;
            dr = dtConfiguraciones.NewRow();
            dr[cols[index++]] = item.idEquipoTemp;
            dr[cols[index]] = item.idEmpleado;
            dtConfiguraciones.Rows.Add(dr);

        }
        return dtConfiguraciones;
    }

    #endregion
}