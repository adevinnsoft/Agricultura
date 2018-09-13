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
using System.Reflection;


public partial class Reportes_ReporteProyeccionJornales : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Reportes_ReporteProyeccionJornales));
    public static string idUsuario = string.Empty;
    public static int idPlanta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string generarConfiguracion(int nSemanas, int nHistoricos, int semanaPartida, int anioPartida)
    {

        string code = "";
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                    { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() },
                    { "@semanas", nSemanas } ,
                    { "@historicosAtras", nHistoricos } ,
                    { "@semanaPartida", semanaPartida } ,
                    { "@anioPartida", anioPartida } 
                };

            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtenerConfiguracionPronostico", parameters);
            if (dt != null && dt.Rows.Count > 0) // VALIDACIÓN DATOS DE SPR
            {
                code = GetDataTableToJson(dt);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return code;
    }



    [WebMethod]
    public static string[] guardarReporte(int anioPartida ,int semanaPartida,int semanas,int eficienciaHistorica,ConfiguracionesJornales[] configuracion)
    {
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataTable dtConfiguraciones = new DataTable();
        DataTable dtPronosticoDetalle = new DataTable();
        DataTable dtPronosticoSemana = new DataTable();
        DataRow drConfiguraciones;
        DataRow drPronosticoDetalle;
        DataRow drPronosticoSemana;
        DataTable dtResponse = new DataTable();

        try
        {
            idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();

            dtConfiguraciones.Columns.Add("anio");
            dtConfiguraciones.Columns.Add("semana");
            dtConfiguraciones.Columns.Add("horas");
            dtConfiguraciones.Columns.Add("ausentismo");
            dtConfiguraciones.Columns.Add("capacitacion");
            dtConfiguraciones.Columns.Add("curva");

            dtPronosticoDetalle.Columns.Add("idLider");
            dtPronosticoDetalle.Columns.Add("idUsuario");
            dtPronosticoDetalle.Columns.Add("semana");
            dtPronosticoDetalle.Columns.Add("anio");
            dtPronosticoDetalle.Columns.Add("producto");
            dtPronosticoDetalle.Columns.Add("idFamilia");
            dtPronosticoDetalle.Columns.Add("idCategoria");
            dtPronosticoDetalle.Columns.Add("esCosecha");
            dtPronosticoDetalle.Columns.Add("jornales");

            dtPronosticoSemana.Columns.Add("idUsuario");
            dtPronosticoSemana.Columns.Add("idLider");
            dtPronosticoSemana.Columns.Add("semana");
            dtPronosticoSemana.Columns.Add("anio");
            dtPronosticoSemana.Columns.Add("producto");
            dtPronosticoSemana.Columns.Add("totalGeneral");


            foreach (ConfiguracionesJornales cj in configuracion)
            {
                drConfiguraciones = dtConfiguraciones.NewRow();
                drConfiguraciones["anio"] = Convert.ToInt32(cj.anio);
                drConfiguraciones["semana"] = Convert.ToInt32(cj.semana);
                drConfiguraciones["horas"] = cj.horas;
                drConfiguraciones["ausentismo"] = cj.ausentismo;
                drConfiguraciones["capacitacion"] = cj.capacitacion;
                drConfiguraciones["curva"] = cj.curva;
                dtConfiguraciones.Rows.Add(drConfiguraciones);

                foreach (PronosticoDetalle pd in cj.pronosticoDetalle)
                {
                    drPronosticoDetalle = dtPronosticoDetalle.NewRow();
                    drPronosticoDetalle["idLider"] = pd.idLider;
                    drPronosticoDetalle["idUsuario"] = pd.idUsuario;
                    drPronosticoDetalle["semana"] = Convert.ToInt32(pd.semana);
                    drPronosticoDetalle["anio"] = Convert.ToInt32(pd.anio);
                    drPronosticoDetalle["producto"] = pd.producto;
                    drPronosticoDetalle["idFamilia"] = pd.idFamilia;
                    drPronosticoDetalle["idCategoria"] = pd.idCategoria;
                    drPronosticoDetalle["esCosecha"] = pd.esCosecha;
                    drPronosticoDetalle["jornales"] = pd.jornales;
                    dtPronosticoDetalle.Rows.Add(drPronosticoDetalle);

                
                }

                foreach (PronosticoSemana ps in cj.pronosticoSemana)
                {
                    drPronosticoSemana = dtPronosticoSemana.NewRow();
                    drPronosticoSemana["idUsuario"] = ps.idUsuario;
                    drPronosticoSemana["idLider"] = ps.idLider;
                    drPronosticoSemana["semana"] = Convert.ToInt32(ps.semana);
                    drPronosticoSemana["anio"] = Convert.ToInt32(ps.anio);
                    drPronosticoSemana["producto"] = ps.producto;
                    drPronosticoSemana["totalGeneral"] = ps.totalGeneral;
                    dtPronosticoSemana.Rows.Add(drPronosticoSemana);
                }
            }

            prm.Add("@idUsuario", idUsuario);
            prm.Add("@idPlanta", idPlanta);
            prm.Add("@anioPartida", anioPartida);
            prm.Add("@semanaPartida", semanaPartida);
            prm.Add("@semanas", semanas);
            prm.Add("@eficienciaHistorica", eficienciaHistorica);
            prm.Add("@Configuraciones", dtConfiguraciones);
            prm.Add("@pronosticoDetalle", dtPronosticoDetalle);
            prm.Add("@pronosticoSemana", dtPronosticoSemana);

            dtResponse = da.executeStoreProcedureDataTable("spr_GuardarReporteJornalesOL", prm);

            if (dtResponse.Rows.Count > 0)
            {   
                if (dtResponse.Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "Reporte guardada correctamente.", "ok" };
                }
                else
                {
                    return new string[] { "0", "Error al guardar la información.", "warning" };
                }
            }
            else
            {
                return new string[] { "0", "El proceso no genero ningún resultado.", "warning" };
            }
        }
        catch ( Exception ex)
        {
            log.Error(ex);
            return new string[] { "0", "Error al tratar de procesar los datos.", "error" };
        }


    }

    [WebMethod]
    public static string[] obtenerReporteJornales(int idPlanta, int anioPartida, int semanaPartida, int numeroSemanas)
    {
        try
        {
            int idioma = 1;
            if (idPlanta.Equals(0))
            {
                idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            }
            DataAccess da = new DataAccess();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idPlanta", idPlanta);
            prm.Add("@anioPartida", anioPartida);
            prm.Add("@semanaPartida", semanaPartida);
            prm.Add("@semanas", numeroSemanas);
            prm.Add("@lenguaje", getIdioma());
            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerPronosticoJornalesOL", prm);


            if (ds.Tables.Count > 0)
            {
                StringBuilder sbLideres = new StringBuilder();
                StringBuilder sbDatos = new StringBuilder();
                string[,] familias = new string[0, 0];
                int filas = ds.Tables[3].Rows.Count;
                int index = 0;
                int bandera = 0;
                List<string> Listfamilias = new List<string>();
                List<string> productos = new List<string>();
                List<string> categorias = new List<string>();

                foreach (DataRow row in ds.Tables[2].Rows)//familias
                {
                    string familia = row["nombreFamilia"].ToString();
                    if (!Listfamilias.Contains(familia))
                    {
                        Listfamilias.Add(familia);
                    }
                    else
                    {

                    }

                }


                foreach (DataRow r in ds.Tables[0].Rows)//productos
                {
                    string productoT1 = r["Product"].ToString();//r["Product"].ToString();
                    foreach (DataRow s in ds.Tables[3].Rows)//datosGenerales
                    {
                        string productoT2 = s["producto"].ToString();
                        if (productoT1.Equals(productoT2))
                        {
                            if (!productos.Contains(productoT1))
                            {
                                productos.Add(productoT1);
                            }
                            else
                            {

                            }
                        }

                    }
                }


                foreach (DataRow dr in ds.Tables[2].Rows)//familias
                {
                    string idFamilia = dr["IdFamilia"].ToString(), nombreFamilia = dr["nombreFamilia"].ToString(), totalCategorias = dr["totalCategorias"].ToString();
                    if (familias.Length == 0)
                    {
                        familias = new string[filas, 3];
                        familias[index, 0] = idFamilia;
                        familias[index, 1] = nombreFamilia;
                        familias[index, 2] = totalCategorias;
                    }
                    else
                    {
                        int contador = 0;
                        for (int i = 0; i < familias.Length; i++)
                        {
                            if (familias[i, 1] != null && nombreFamilia == familias[i, 1])
                            {
                                bandera = 1;
                                break;
                            }
                            else if (familias[i, 1] == null)
                            {
                                familias[contador, 0] = idFamilia;
                                familias[contador, 1] = nombreFamilia;
                                familias[contador, 2] = totalCategorias;
                                index++;
                                break;
                            }
                            contador++;
                        }
                    }
                }

                foreach (DataRow l in ds.Tables[2].Rows)//categorias
                {
                    string categoria = l["Categoria"].ToString();
                    if (!categorias.Contains(categoria))
                    {
                        categorias.Add(categoria);
                    }
                    else
                    {

                    }
                }


                int colspanFamilias = Listfamilias.Count * (categorias.Count + 1);
                int colspanCategorias = categorias.Count;
                foreach (string producto in productos)
                {
                    StringBuilder secondRow = new StringBuilder();
                    StringBuilder tercerRow = new StringBuilder();
                    string Semanas = string.Empty;
                    int contador = 0;
                    sbLideres.AppendLine("<table class=\"tblProducto\" cellspacing=\"0\" producto=\"" + producto + "\">");
                    sbLideres.AppendLine("<thead>");
                    sbLideres.AppendLine(string.Format("<tr><th colspan=\"3\">{0}</th></tr>", producto));
                    sbLideres.AppendLine(string.Format("<tr><th>{0}</th><th>{1}</th><th>{2}</th></tr>", "Código", "Nombre","Área"));
                    sbLideres.AppendLine(string.Format("<tr><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th></tr>"));
                    sbLideres.AppendLine("</thead>");

                    sbDatos.AppendLine("<table class=\"tblDatosOL\" cellspacing=\"0\" Producto=\""+producto+"\">");//tabla datos
                    sbDatos.AppendLine("<thead>");

                    int numeroFamilias = Listfamilias.Count;
                    string familiaActual = string.Empty;
                    foreach (DataRow R in ds.Tables[4].Rows)//semanas
                    {
                        contador = 0;
                        string semana = R["semana"].ToString(), anio = R["anio"].ToString();
                        Semanas += string.Format("<th class=\"Semana\" semana=\"" + semana + "\" anio=\"" + anio + "\">{0}</th>", anio+"_"+semana);

                        for (int i = 0; i < familias.Length; i++)
                        {
                            if (familias[i, 1] != null)
                            {
                                string Familia = familias[i, 1];
                                string colspan = familias[i, 2];
                                secondRow.Append(string.Format("<th class=\"Familia\" semana=\"" + semana + "\" anio=\"" + anio + "\" Familia=\"" + Familia + "\" colspan=\"" + colspan + "\">{0}</th>", Familia));
                            }
                            else
                            {
                                secondRow.Append(string.Format("<th>{0}</th>", "Total Activos"));
                                break;
                            }
                        }
                    }

                    int numeroCategorias = 0;
                    secondRow.Append("<tr>");
                    familiaActual = string.Empty;
                    foreach (DataRow se in ds.Tables[4].Rows)//semanas
                    {
                        contador = 0;
                        string semana = se["semana"].ToString(), anio = se["anio"].ToString();
                        foreach (DataRow r in ds.Tables[2].Rows)//familias
                        {   
                            string fam = r["nombreFamilia"].ToString();
                            
                                numeroCategorias = Convert.ToInt32(r["totalCategorias"].ToString());
                                string categoria = r["Categoria"].ToString();

                                secondRow.Append(string.Format("<th class=\"Categoria\" semana=\"" + semana + "\" anio=\"" + anio + "\" categoria=\"" + categoria + "\">{0}</th>", categoria));
                           
                        }
                        secondRow.Append(string.Format("<th>{0}</th>", ""));
                    }

                    secondRow.AppendLine("</tr>");

                    sbDatos.AppendLine(string.Format("<tr>{0}</tr>", Semanas));
                    sbDatos.AppendLine(secondRow.ToString());
                    sbDatos.AppendLine("</thead>");
                    sbLideres.Append("<tbody>");
                    sbDatos.Append("<tbody>");
                    familiaActual = string.Empty;
                    foreach (DataRow R in ds.Tables[1].Rows)//lideres
                    {
                        string idLider = R["idLider"].ToString()
                              , nombreLider = R["nombreLider"].ToString()
                              , idUsuario = R["IdUsuario"].ToString()
                              , area = R["depto"].ToString();
                        StringBuilder sbJornalesOL = new StringBuilder();

                        foreach (DataRow S in ds.Tables[4].Rows)//semanas
                        {
                            string semana = S["semana"].ToString(), anio = S["anio"].ToString();
                            foreach (DataRow r in ds.Tables[2].Rows)//familias
                            {
                                string categoria = r["Categoria"].ToString(), f = r["nombreFamilia"].ToString();

                                sbJornalesOL.Append(string.Format("<td class=\"Horas\" semana=\"" + semana + "\" anio=\"" + anio + "\" categoria=\"" + categoria + "\">{0}</td>",
                                        ds.Tables[3].Select("idLider=" + idLider + " AND semana=" + semana + " AND anio=" + anio + " AND categoria='" + categoria + "' AND producto='" + producto + "' AND familia='" + f + "'").Length > 0 ?
                                        ds.Tables[3].Select("idLider=" + idLider + " AND semana=" + semana + " AND anio=" + anio + " AND categoria='" + categoria + "' AND producto='" + producto + "' AND familia='" + f + "'")[0]["jornales"].ToString() : "0"
                                    ));    
                            }
                            sbJornalesOL.Append(string.Format("<td class=\"TotalActivos\" semana=\"" + semana + "\" anio=\"" + anio + "\">{0}</td>", "0"));

                                    
                        }
                        sbLideres.AppendLine(string.Format("<tr class=\"trLider\" idLider=\"" + idLider + "\" nombreLider=\"" + nombreLider + "\" idUsuario=\"" + idUsuario + "\"><td>{0}</td><td>{1}</td><td>{2}</td></tr>", idLider, nombreLider, area));
                        sbDatos.AppendLine(string.Format("<tr class=\"trRow\">{0}</tr>", sbJornalesOL.ToString()));
                    }

                    sbLideres.AppendLine("</tbody></table>");
                    sbDatos.AppendLine("</tbody></table>");
                }

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0 && ds.Tables[4].Rows.Count > 0)
                {
                    return new string[] { "1", "ok", sbLideres.ToString(), sbDatos.ToString() };
                }
                else
                {
                    return new string[] { "0", "El proceso no generó ningún resultado.", "warning" };
                }
                
            }
            else
            {
                return new string[] { "0", "Error en la respuesta de base de datos.", "warning" };
            }


        }
        catch (Exception ex)
        {

            log.Error(ex);
            return new string[] { "0", "El proceso no generó ningún resultado.", "warning" };
        }
    }

}