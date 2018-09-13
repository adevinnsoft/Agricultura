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

public partial class configuracion_frmCategoriaEtapa : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmCategoriaEtapa));
    public static int idPlanta = 0;
    public static string idUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {

    }



    [WebMethod]
    public static string[] obtenerHabilidadesEtapas()
    {
        int idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
        idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataSet ds = new DataSet();
        StringBuilder sb = new StringBuilder();
        JavaScriptSerializer jsDeserializer = new JavaScriptSerializer();  
        EtapaCategoria[] arrayEtapas;
        int n = 1;
        int i = 1;

        try
        {
            //idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);

            prm.Add("@idPlanta", idPlanta);
            prm.Add("@idUsuario",idUsuario);
            prm.Add("@lenguaje", getIdioma());

            ds = da.executeStoreProcedureDataSet("spr_ObtenerHabilidadesEtapa", prm);


            sb.AppendLine("<table class=\"gridView\" id=\"tblEtapas\" cellspacing='0'>");
            //sb.AppendLine("<thead>"+
            //            " <tr>"+
            //            " <th>Habilidad</th>"+
            //            "  <th>Etapa</th>"+
            //            "  <th>"+
            //            " <label>Categoría</label><br>"+
            //            " <input type='checkbox' class='checkbox' id='TodosPiso' />"+
            //            " <label for='TodosPiso'>Piso</label>" +
            //            " <input type='checkbox' class='checkbox' id='TodosAire' />"+
            //" <label for='TodosAire'>Aire</label></th></tr></thead><tbody>");

            sb.AppendLine("<thead>" +
                      " <tr>" +
                      " <th>Habilidad</th>" +
                      " <th>Etapa</th><th><label>Categoría</label><br>");
         
            foreach(DataRow R in ds.Tables[1].Rows)
	        {
                  string Categoria = R["categoria"].ToString().Trim(), idCat = R["idCategoria"].ToString().Trim();
                  sb.AppendLine("<input type='checkbox' class='checkbox' idCategoria=" + idCat + " id='Todos" + Categoria + "' />");
                  sb.AppendLine("<label for='Todos"+Categoria+"'>"+Categoria+"</label>");
	        }

           sb.AppendLine("</th></tr></thead><tbody>");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string idHabilidad = dr["idHabilidad"].ToString().Trim(),
                       nombreHabilidad = dr["nombreHabilidad"].ToString().Trim(),
                      // etapasCategorias = dr["etapasCategorias"].ToString().Trim(),
                       idEtapa = dr["idEtapa"].ToString().Trim(),
                       nombreEtapa = dr["nombreEtapa"].ToString().Trim(),
                       //nombreDepartamento = dr["NombreDepartamento"].ToString().Trim();
                       idCategoria = dr["idCategoria"].ToString().Trim();

               

                //sb.AppendLine("<div class=\"name\" class=\"habilidad\" idHabilidad=\""+idHabilidad+"\" Habilidad=\"" + nombreHabilidad + "\" Departamento=\""+nombreDepartamento+"\"><label>" + nombreHabilidad + "-"+nombreDepartamento+"</label><img src=\"../comun/img/sort_desc.png\" element=\"img\" class=\"Abajo\" id=\"imgDESC\"><img src=\"../comun/img/sort_asc.png\" element=\"img\" class=\"Arriba\" id=\"imgASC\"></div>");
                //sb.AppendLine("<div class=\"tblEtapas-container\">");
                //sb.AppendLine("<table class=\"table\" id=\"tblEtapas" + nombreHabilidad + "\">");

                sb.AppendLine("<tr class=\"Habilidad\" idHabilidad=\"" + idHabilidad + "\" Habilidad=\"" + nombreHabilidad + "\"  idCategoria=\"" + idCategoria + "\" idEtapa=\"" + idEtapa + "\">");
                sb.AppendLine("<td>" + nombreHabilidad + "</td>");
                sb.AppendLine("<td>" + nombreEtapa + "</td>");
                //sb.AppendLine("<td><input type='radio' class='radioPiso' name='radio" + n + "' /><label class='piso' >Piso</label><input type='radio' class='radioAire' name='radio" + n + "' /><label class='aire'>Aire</label></td>");
                sb.AppendLine("<td>");
                foreach (DataRow dr2 in ds.Tables[1].Rows)
                {
                    string idCategoriaa = dr2["idCategoria"].ToString().Trim(), Categoria = dr2["categoria"].ToString().Trim();
                    sb.AppendLine("<input type='radio' class='radio" + Categoria + "' idCategoria='" + idCategoriaa + "' name='radio" + n + "'/><label class='" + Categoria + "'>" + Categoria + "</label>");
                }
                sb.AppendLine("</td>");
               // arrayEtapas = jsDeserializer.Deserialize<EtapaCategoria[]>(etapasCategorias);
                //foreach (EtapaCategoria etapa in arrayEtapas)
                //{
                //    sb.AppendLine("<tr class=\"trRow\" idCategoria=\"" + etapa.idCategoria + "\" numero=\"" + i + "\">");
                //    sb.AppendLine("<td class=\"etapa\" idEtapa=\"" + etapa.idEtapa + "\">" + etapa.nombreEtapa + "</td>");

                //    i++;
                //    foreach (DataRow dr2 in ds.Tables[1].Rows)
                //    {

                //        string idCategoria2 = dr2["idCategoria"].ToString().Trim(),
                //               categoria = dr2["categoria"].ToString().Trim();

                //        sb.AppendLine("<td class=\"categoria\" idCategoria=\"" + idCategoria2 + "\"><input type=\"radio\" class=\"radio\" idCategoria=\"" + idCategoria2 + "\" ><label for=\"categoria\">" + categoria + "</label></td>");

                //    }
                //    sb.AppendLine("</tr>");
                //}
                sb.AppendLine("</tr>");
                
                //sb.AppendLine("</div>");
                n++;
            }
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<input type='button' class='btnGuardarConfiguracion' id='btn' value='Guardar' onclick='guardarConfiguracion()'/>");

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[2].Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "ok", sb.ToString() };
                }
                else
                {
                    return new string[] { "0", "Error al obtener la información", "warning" };
                }
            }
            else
            {
                return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            }

        }
        catch (Exception x)
        {
            log.Error(x);
            return new string[] { "0", "No se pudo completar la operación.", "warning" };
        }
    }


    [WebMethod]
    public static string[] guardarConfiguracion(CategoriaEtapa[] categoriaEtapa)
    {
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataTable dtCategoriaEtapa = new DataTable();
        DataRow dr;
        DataTable dtResponse = new DataTable();

        if (categoriaEtapa.Length == 0)
        {
            return new string[] { "0", "No se realizaron cambios.", "warning" };
        }
        else
        {

            try
            {
                idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();

                dtCategoriaEtapa.Columns.Add("idEtapa");
                dtCategoriaEtapa.Columns.Add("idCategoria");

                foreach (CategoriaEtapa ce in categoriaEtapa)
                {
                    dr = dtCategoriaEtapa.NewRow();
                    dr["idEtapa"] = ce.idEtapa;
                    dr["idCategoria"] = ce.idCategoria;
                    dtCategoriaEtapa.Rows.Add(dr);
                }

                prm.Add("@idUsuario", idUsuario);
                prm.Add("@configuracion", dtCategoriaEtapa);

                dtResponse = da.executeStoreProcedureDataTable("spr_GuardarConfiguracionCategoriaEtapa", prm);

                if (dtResponse.Rows.Count > 0)
                {
                    if (dtResponse.Rows[0]["Estado"].ToString().Equals("1"))
                    {
                        return new string[] { "1", "Configuración guardada correctamente.", "ok" };
                    }
                    else
                    {
                        return new string[] { "0", "Error al guardar la configuración.", "warning" };
                    }
                }
                else
                {
                    return new string[] { "0", "El proceso no generó ningún resultado.", "warning" };
                }
            }
            catch (Exception x)
            {

                log.Error(x);
                return new string[] { "0", "Error al tratar de procesar los datos.", "warning" };
            }
        }
    }

}