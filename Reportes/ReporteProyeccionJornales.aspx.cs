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
    public static string[] obtenerReporteJornales(int idPlanta,int anioPartida, int semanaPartida,int numeroSemanas)
    {
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataSet ds = new DataSet();
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        JavaScriptSerializer jsDeserializer = new JavaScriptSerializer();
        Dictionary<string, string> semanas = new Dictionary<string, string>();

        try
        {
            //idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            prm.Add("@idPlanta",idPlanta);
            prm.Add("@anioPartida", anioPartida);
            prm.Add("@semanaPartida", semanaPartida);
            prm.Add("@semanas", numeroSemanas);
            prm.Add("@lenguaje", getIdioma());

            ds = da.executeStoreProcedureDataSet("spr_ObtenerPronosticoJornalesResumen", prm);

             int filas = ds.Tables[3].Rows.Count;
             string[,] familias = new string[0,0];
             int index = 0;
             int x = 0;   
             int bandera = 0;
             int flag=0;
             int cont = 0;
             int totalCategoria = 0;
             int totalActivos = 0;
             List<string> nombresFamilias = new List<string>();
             foreach (DataRow dr in ds.Tables[3].Rows)
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

             int cnt = 0;
             for (int indice = 0; indice < familias.Length; indice++)
             {
                 
                 if (familias[indice, 1] == null)
                 {
                     break;
                 }
                 else
                 {

                     nombresFamilias.Add(familias[cnt, 1]);
                 }
                 cnt++;
                 
             }


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.AppendLine("<table class=\"gridView\" id=\"tblSemanas\">");
                string productot1 = dr["Descripcion"].ToString().Trim(),idProducto=dr["idProducto"].ToString().Trim();
                foreach (DataRow dr2 in ds.Tables[1].Rows)
                {
                    string productot2 = dr2["Producto"].ToString().Trim();

                    if (productot1 == productot2 || productot2 == null)
                    {
                        flag++;
                        if (flag == 1)
                        {
                            sb.AppendLine("<thead>");
                            sb.AppendLine("<tr class=\"trSemana\">");
                            foreach (DataColumn col in ds.Tables[1].Columns)
                            {
                                if (col.ColumnName.Contains('_'))
                                {
                                    x++;
                                    string semana = col.ColumnName.ToString().Trim();
                                    sb.AppendLine("<th colspan=\"4 \" semana=\""+col.ColumnName+"\" class=\"semana\">Semana:" + x + " " + "(" + semana + ")" + "</th>");
                                }
                            }
                            sb.AppendLine("</tr>");

                            sb.AppendLine("<tr>");//segundo encabezado
                           
                            foreach(DataColumn col in ds.Tables[1].Columns)//////CONFIGURAR LA TABLA A 10 COLSPAN
                            {
                                if (col.ColumnName.Contains('_'))
                                {
                                     for (int i = 0; i < familias.Length; i++)
			                         {
                                         if (familias[i, 1] != null)
                                         {
                                             string Familia = familias[i, 1];
                                             string rowSpan = familias[i, 2];
                                             sb.AppendLine("<th  class=\"thfamilia\" id=\"" + Familia + "\" colspan=\"" + rowSpan + "\" semana=\""+col.ColumnName+"\">Familia" + " " + Familia + "</th>");
                                         }
                                         else
                                         {
                                             break;
                                         }
			                         }
                                }
                                
                            }
                            
                            sb.AppendLine("</tr>");

                            sb.AppendLine("<tr class=\"encabezadoCategorias\">");//tercer encabezado

                            
                            foreach(DataColumn col in ds.Tables[1].Columns)
                            {
                                if (col.ColumnName.Contains('_'))
                                {
                                    foreach (DataRow d in ds.Tables[3].Rows)
                                    {
                                        string family = d["nombreFamilia"].ToString().Trim(), categoria = d["Categoria"].ToString().Trim();
                                        sb.AppendLine("<th class=\"th\" color=\"green\" familiacategoria=\""+family+categoria+"\" idfamilia=\"" + family + "\" familia=\"" + family + "\" semana=\""+col.ColumnName+"\" categoria=\""+categoria+"\">" + categoria + "</th>");
                                       
                                    }
                                    
                                }
                            
                               
                            }


                            
                            sb.AppendLine("</tr>");
                            sb.AppendLine("</thead>");


                            sb.AppendLine("<tbody>");
                            foreach (DataRow dr1 in ds.Tables[2].Rows)//lideres
                            {
                                semanas.Clear();
                                string idLider1 = dr1["idLider"].ToString().Trim(), nombreLider1 = dr1["nombreLider"].ToString().Trim(), idUsuario1 = dr1["idUsuario"].ToString().Trim();
                                sb.AppendLine("<tr class=\"trLider\" idLider=\"" + idLider1 + "\" nombreLider=\""+nombreLider1+"\" idUsuario=\""+idUsuario1+"\">");
                                string familiaActual = string.Empty;
                                int numeroCategorias=0;
                                string semanaValor = string.Empty;
                                cont = 0;
                                string ultimaFamilia = string.Empty;
                                ultimaFamilia= nombresFamilias.Last();

                               
                        
                                    foreach (DataRow a in ds.Tables[3].Rows)//categorias
                                    {
                                        //OBTENER CATEGORIA
                                        int ban = 0;
                                        string categoria = a["Categoria"].ToString().Trim();
                                        string familia = a["nombreFamilia"].ToString().Trim();
                                        int totalCategorias = Convert.ToInt32(a["totalCategorias"].ToString().Trim());
                                        string familiaPivote = string.Empty;
                                        foreach (DataRow dro in ds.Tables[1].Rows)//pivote
                                        {

                                            string idLider2 = dro["idLider"].ToString().Trim(), nombreLider2 = dro["nombreLider"].ToString().Trim(), idUsuario2 = dro["idUsuario"].ToString().Trim(), productoo = dro["Producto"].ToString().Trim(),
                                              categoria1 = dro["categoria"].ToString().Trim(), idFamilia = dro["idFamilia"].ToString().Trim(), idCategoria = dro["idCategoria"].ToString().Trim();


                                            familiaPivote = dro["Familia"].ToString().Trim();

                                            if (idLider1.Equals(idLider2) && idUsuario1.Equals(idUsuario2) && categoria.Equals(categoria1) && familia.Equals(familiaPivote) /*&& productot1.Equals(productoo)*/)
                                            {
                                                ban = 1;

                                                cont++;
                                                if (familiaPivote != "")//obtenemos el numero de categorias
                                                {
                                                    if (familiaActual != familiaPivote)//si la familia actual es igual a la familiaPivote
                                                    {
                                                        familiaActual = familiaPivote;//actualizamos bandera

                                                        for (int y = 0; y < familias.Length; y++)//iteramos nuestra matriz de familias
                                                        {

                                                            string familiaMatriz = familias[y, 1];

                                                            if (familiaMatriz != null)
                                                            {

                                                                if (familiaMatriz == familiaActual)//si la familia de la matriz es igual a la familia actual
                                                                {
                                                                    //cont = 0;
                                                                    numeroCategorias = Convert.ToInt32(familias[y, 2]);//obtenemos el total de categorias de esa familia

                                                                }
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }

                                                    }

                                                }



                                              
                                                //ultimaFamilia = nombresFamilias.Last();
                                                foreach (DataColumn col in ds.Tables[1].Columns)//iteracion semanas
                                                {

                                                    if (col.ColumnName.Contains('_'))
                                                    {

                                                        semanaValor = string.Empty;

                                                        string hora = dro["" + col.ColumnName + ""].ToString().Trim();
                                                        if (!hora.Equals(string.Empty))
                                                        {

                                                            if (semanas.ContainsKey(col.ColumnName))
                                                            {
                                                                semanaValor = semanas[col.ColumnName];
                                                            }

                                                            semanaValor += "<td class=\"tdhoras\" color=\"green\" tieneJornales=\"1\" semana=\"" + col.ColumnName + "\"   idFamilia=\"" + idFamilia + "\" familia=\"" + familiaActual + "\"  idCategoria=\"" + idCategoria + "\" categoria=\"" + categoria + "\" idUsuario=\"" + idUsuario1 + "\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\">" + hora + "</td>";

                                                            if (cont == numeroCategorias)
                                                            {

                                                                semanaValor += "<td class=\"tdactivos\" color=\"black\" tieneActivos=\"1\" semana=\"" + col.ColumnName + "\"  idFamilia=\"" + idFamilia + "\"  familia=\"" + familiaActual + "\" idUsuario=\"" + idUsuario1 + "\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\"></td>";
                                                                if (familiaActual == ultimaFamilia)
                                                                {
                                                                    semanaValor += "<td class=\"tdactivosgenerales\"  color=\"red\" tieneActivosGenerales=\"1\" semana=\"" + col.ColumnName + "\"  idUsuario=\"" + idUsuario1 + "\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\">1</td>";
                                                                }
                                                            }



                                                            if (semanas.ContainsKey(col.ColumnName))
                                                            {
                                                                semanas[col.ColumnName] = semanaValor;
                                                            }
                                                            else
                                                            {
                                                                semanas.Add(col.ColumnName, semanaValor);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            if (semanas.ContainsKey(col.ColumnName))
                                                            {
                                                                semanaValor = semanas[col.ColumnName];
                                                            }

                                                            semanaValor += "<td>0</td>";

                                                            if (cont == numeroCategorias)
                                                            {

                                                                semanaValor += "<td>0</td>";
                                                                if (familiaActual == ultimaFamilia)
                                                                {
                                                                    semanaValor += "<td>0</td>";
                                                                }
                                                            }
                                                            if (semanas.ContainsKey(col.ColumnName))
                                                            {
                                                                semanas[col.ColumnName] = semanaValor;
                                                            }
                                                            else
                                                            {
                                                                semanas.Add(col.ColumnName, semanaValor);
                                                            }
                                                        }
                   

                                                    }//if validacion _   
                                                }//iteramos columnas semanas

                                                if (ban == 1)
                                                {
                                                    break;
                                                }

                                            }
                                            else
                                            {
                                                ban = 0;
                        
                                            }
                                        }//iteracion de pivote

                                        
                                        if (ban == 0)//validamos bandera
                                        {
                                            cont++;
                                            foreach (DataRow drsemana in ds.Tables[4].Rows)
                                            {
                                                string week = drsemana["semana"].ToString().Trim();
                                                if (semanas.ContainsKey(week))
                                                {
                                                    semanaValor = semanas[week];
                                                }
                                                else
                                                {
                                                    semanaValor = string.Empty;

                                                }
                                                semanaValor += "<td color=\"green\" class=\"tdactivos\" tieneActivos=\"1\" semana=\"" + week + "\"  familia=\"" + familiaActual + "\" idUsuario=\"" + idUsuario1 + "\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\">0</td>";

                                                if (cont == totalCategorias)
                                                {

                                                    semanaValor += "<td class=\"tdactivos\" color=\"black\" tieneActivos=\"1\" semana=\"" + week + "\"  familia=\"" + familiaActual + "\" idUsuario=\"" + idUsuario1 + "\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\">0</td>";

                                                    if (familia == ultimaFamilia)
                                                    {
                                                        semanaValor += "<td class=\"tdactivosgenerales\" color=\"red\" tieneActivosGenerales=\"1\" semana=\"" + week + "\"  idUsuario=\"" + idUsuario1 + "\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\">0</td>";

                                                    }

                                                }

                                                if (semanas.ContainsKey(week))
                                                {
                                                    semanas[week] = semanaValor;
                                                }
                                                else
                                                {
                                                    semanas.Add(week, semanaValor);
                                                }

                                                
                                            }
                                           
                                        }

                                        if (cont == totalCategorias)
                                        {
                                            cont = 0;
                                        }

                                    }
                               
      
                                foreach (string key in semanas.Keys)
                                {
                                    sb.AppendLine(semanas[key]);
                                   
                                }
                               
                                sb.AppendLine("</tr>");
                            }
                            sb.AppendLine("</tbody");
                        }
                    }
                }
                sb.AppendLine("</table>");
                
            }

            int mibandera = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb2.AppendLine("<table class=\"gridView\" id=\"tblAsociados\">");
                string productot1 = dr["Descripcion"].ToString().Trim(), idProducto = dr["idProducto"].ToString().Trim();
                foreach (DataRow dr2 in ds.Tables[1].Rows)
                {
                    string productot2 = dr2["Producto"].ToString().Trim();

                    if (productot1 == productot2 || productot2 == null)
                    {
                        mibandera++;
                        if (mibandera == 1)
                        {
                            sb2.AppendLine("<thead>");
                            sb2.AppendLine("<tr class=\"trProducto\" idProducto=\"" + idProducto + "\" producto=\"" + productot1 + "\">");//primer encabezado
                            string producto = dr["Descripcion"].ToString().Trim();
                            sb2.AppendLine(productot2.Equals(string.Empty) ? "<th colspan=\"2\">-</th>" : "<th colspan=\"2\">" + producto + "</th>");

                            sb2.AppendLine("</tr>");

                            sb2.AppendLine("<tr>");//segundo encabezado
                            sb2.AppendLine("<th>Código</th>");
                            sb2.AppendLine("<th>Nombre</th>");
                            sb2.AppendLine("</tr>");
                            sb2.AppendLine("<tr>");
                            sb2.AppendLine("<th>&nbsp;</th>");
                            sb2.AppendLine("<th>&nbsp;</th>");
                            sb2.AppendLine("</tr>");
                            sb2.AppendLine("</thead>");

                            sb2.AppendLine("<tbody>");
                            foreach (DataRow dr1 in ds.Tables[2].Rows)//lideres
                            {
                                string idLider1 = dr1["idLider"].ToString().Trim(), nombreLider1 = dr1["nombreLider"].ToString().Trim(), idUsuario1 = dr1["idUsuario"].ToString().Trim();
                                sb2.AppendLine("<tr class=\"trLider\" idLider=\"" + idLider1 + "\" nombreLider=\"" + nombreLider1 + "\" idUsuario=\"" + idUsuario1 + "\">");
                                sb2.AppendLine("<td class=\"tdidLider\">" + idLider1 + "</td>");
                                sb2.AppendLine("<td class=\"tdLider\" idUsuario=\"" + idUsuario1 + "\">" + nombreLider1 + "</td>");
                                sb2.AppendLine("</tr>");
                            }
                            
                            sb2.AppendLine("</tbody>");

                        }
                    }
                }
                sb2.AppendLine("</table>");
            }

            //sb.AppendLine("<input type=\"button\" class=\"button\" id=\"btnGuardarReporte\" value=\"Guardar Reporte\"/>");

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0 && ds.Tables[4].Rows.Count > 0)
            {
                if (ds.Tables[5].Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "ok", sb.ToString(),sb2.ToString()};
                }
                else
                {
                    return new string[] { "0", ds.Tables[3].Rows[0]["Mensaje"].ToString(), "warning" };
                }
            }
            else
            {
                return new string[] { "0", "No existe información para la semana especificada", "warning" };
            }
       
        }
        catch (Exception ex)
        {

            log.Error(ex);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
        }
    }

}