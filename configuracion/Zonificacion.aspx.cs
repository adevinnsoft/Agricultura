using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using log4net;
using System.Text;
using System.Collections;

public partial class configuracion_Zonificacion : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_Zonificacion));
    public static int idLider = 0;
    public static string idUsuario = string.Empty;
    public static int idPlanta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    [WebMethod]
    public static string[] generarMatriz()
    {
        DataAccess da = new DataAccess();
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        DataSet dt = new DataSet();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        idLider = Convert.ToInt32(HttpContext.Current.Session["idEmpleado"]);
        idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
        prm.Add("@idLider", idLider);
        prm.Add("@idPlanta",idPlanta);

        try
        {

            dt = da.executeStoreProcedureDataSet("spr_ObtenerConfiguracionZonificacion", prm);

            sb.AppendLine("<thead>");//encabezados de invernaderos
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Asociados/Invernaderos</th>");
            foreach (DataRow dr1 in dt.Tables[1].Rows)
            {
                string idInvernadero = dr1["idInvernadero"].ToString().Trim(), Invernadero = dr1["Invernadero"].ToString().Trim(),numeroSurcos = dr1["numeroSurcos"].ToString().Trim();
                sb.AppendLine("<th colspan=\"1\" class='Encabezado'>" + Invernadero + "<span class=\"numeroSurcos\" NoSurcos=\"" + numeroSurcos + "\">(" + numeroSurcos + " " + "surcos" + ")</span></th>");
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");

            sb.AppendLine("<tbody>");
            foreach (DataRow item in dt.Tables[0].Rows)
            {
                string idAsociado = item["idAsociado"].ToString().Trim(), nombreAsociado = item["nombreAsociado"].ToString().Trim();
                sb.AppendLine("<tr class='Asociado' nombreAsociado='" + nombreAsociado + "' idAsociado=\"" + idAsociado + "\">");
                sb.AppendLine("<td class='nombreAsociado'>"+nombreAsociado+"</td>");
                foreach(DataRow Drow in dt.Tables[1].Rows){
                    string idInvernadero = Drow["idInvernadero"].ToString().Trim(), Invernadero = Drow["Invernadero"].ToString().Trim(), numSurcos = Drow["numeroSurcos"].ToString().Trim();
                    sb.AppendLine("<td class=\"invisible\"><input type=\"text\" id=\"txtHidden\" idAsociado=\"" + idAsociado + "\" nombreAsociado=\"" + nombreAsociado + "\" idinvernadero=\"" + idInvernadero + "\" invernadero=\"" + Invernadero + "\" numeroSurcos=\"" + numSurcos + "\" nuevo=\"1\" accesible=\"0\" activo=\"0\" value=\"\"></td>");
                    sb.AppendLine("<td class=\"surco\" ><input type=\"text\" id=\"txtSurcos\" nuevo=\"1\" vacioSurco=\"1\"></td>");
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            //sb.AppendLine("<tbody>");
            //foreach (DataRow dr  in dt.Tables[0].Rows)
            //{
            //    string idAsociado = dr["idAsociado"].ToString().Trim(), nombreAsociado = dr["nombreAsociado"].ToString().Trim();
            //    sb.AppendLine("<tr>");
            //    foreach (DataRow d in dt.Tables[1].Rows)
            //    {
            //        string idInvernadero = d["idInvernadero"].ToString().Trim(), Invernadero = d["Invernadero"].ToString().Trim();
            //        sb.AppendLine("<td class=\"invisible\"><input type=\"text\" id=\"txtHidden\" idAsociado=\"" + idAsociado + "\" nombreAsociado=\"" + nombreAsociado + "\" idinvernadero=\"" + idInvernadero + "\" invernadero=\"" + Invernadero + "\" nuevo=\"1\" accesible=\"0\" activo=\"0\" value=\"\"></td>");
            //        //sb.AppendLine("<td class=\"seccion\"><input type=\"text\" id=\"txtSeccion\" vacioSeccion=\"1\"></td>");
            //        sb.AppendLine("<td class=\"surco\" ><input type=\"text\" id=\"txtSurcos\" nuevo=\"1\" vacioSurco=\"1\"></td>");
            //    }
            //    sb.AppendLine("</tr>");
            //}
            //sb.AppendLine("</tbody>");//cuerpo de la tabla de invernaderos



            //sb2.AppendLine("<thead>");
            //sb2.AppendLine("<tr>");
            //sb2.AppendLine("<th>INVERNADERO<span class=\"spanInvisible\">&nbsp;</span></th>");
            //sb2.AppendLine("</tr>");;
            //sb2.AppendLine("</thead>");
            //sb2.AppendLine("<tbody>");
            ////sb2.AppendLine("</tr><td>&nbsp;</td></tr>");
            //foreach (DataRow dr1 in dt.Tables[0].Rows )
            //{
            //    string idAsociado = dr1["idAsociado"].ToString().Trim(), nombreAsociado = dr1["nombreAsociado"].ToString().Trim();
            //    sb2.AppendLine("<tr>");
            //    sb2.AppendLine(idAsociado.Equals(string.Empty) && nombreAsociado.Equals(string.Empty) ? "<td class=\"Asociado\">-</td>" : "<td class=\"Asociado\" id=\"" + idAsociado + "\"><b>" + nombreAsociado + "</b><input type=\"text\" class=\"invisible2\" value=\"\"/></td>");
            //    sb2.AppendLine("</tr>");
            //}
            //sb2.AppendLine("</tbody>");



            if (dt.Tables[0].Rows.Count > 0 && dt.Tables[1].Rows.Count > 0)
            {
                return new string[] { "1", "ok", sb.ToString(),sb2.ToString() };
            }
            else
            {
                return new string[] { "0", "No existen asociados para el líder seleccionado", "warning" };
            }
        }
        catch (Exception x)
        {
            Log.Error(x);
            return new string[] { "0", "Error al obtener los asociados del líder seleccionado", "warning" };
        }

    }


    [WebMethod]
    public static string[] guardarConfiguracion(Zonificacion[] zonificacion)
    {
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataTable dtZonificacion = new DataTable();
        DataRow dr;
        DataTable dtResponse = new DataTable();
        ArrayList arregloSurcos = new ArrayList();
        ArrayList subArreglo = new ArrayList();
        int primero = 0,segundo=0;

        if (zonificacion.Length == 0)
        {
            return new string[] { "0", "No se realizaron cambios.", "warning" };
        }
        else
        {
            try
            {
               idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
               idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();

               dtZonificacion.Columns.Add("idAsociado");
               dtZonificacion.Columns.Add("nombreAsociado");
               dtZonificacion.Columns.Add("idInvernadero");
               dtZonificacion.Columns.Add("invernadero");
               dtZonificacion.Columns.Add("surcos");


               foreach(Zonificacion z in zonificacion)//convertimos en array los surcos capturados
               {
                  // z.surcos = Convert.ToInt32(z.surcos);
                   if (z.surcos != null)
                   {
                       arregloSurcos.Clear();
                       subArreglo.Clear();
                       foreach (string s in z.surcos.Split(','))
                       {
                           if (s.Contains('-'))
                           {
                               primero = Convert.ToInt32(s.Split('-')[0]);
                               segundo = Convert.ToInt32(s.Split('-')[1]);
                               for (int x = primero; x <= segundo; x++)
                               {
                                   subArreglo.Add(Convert.ToInt32(x));
                               }
                           }
                           else
                           {
                               arregloSurcos.Add(Convert.ToInt32(s));
                           }
                       }

                       if (subArreglo.Count > 0)
                       {
                           arregloSurcos.AddRange(subArreglo);
                       }
                       else
                       {

                       }

                       foreach (int surco in arregloSurcos)
                       {
                           dr = dtZonificacion.NewRow();
                           dr["idAsociado"] = z.idAsociado;
                           dr["nombreAsociado"] = z.nombreAsociado;
                           dr["idInvernadero"] = z.idInvernadero;
                           dr["invernadero"] = z.invernadero;
                           dr["surcos"] = surco;
                           dtZonificacion.Rows.Add(dr);
                       }
                   }
                   else
                   {
                       dr = dtZonificacion.NewRow();
                       dr["idAsociado"] = z.idAsociado;
                       dr["nombreAsociado"] = z.nombreAsociado;
                       dr["idInvernadero"] = z.idInvernadero;
                       dr["invernadero"] = z.invernadero;
                       dr["surcos"] = 0;
                       dtZonificacion.Rows.Add(dr);
                   }

               }


               prm.Add("@idUsuario", idUsuario);
               prm.Add("@zonificacion", dtZonificacion);

               dtResponse = da.executeStoreProcedureDataTable("spr_GuardarConfiguracionZonificacion", prm);

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


    [WebMethod]
    public static string[] obtenerMatrizConfiguracion()
    {
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataSet dt = new DataSet();
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        idLider = Convert.ToInt32(HttpContext.Current.Session["idEmpleado"]);
        idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
        
        string cadena = string.Empty;
        prm.Add("@idLider", idLider);
        prm.Add("@idPlanta", idPlanta);

        try
        {
            dt = da.executeStoreProcedureDataSet("spr_ObtenerConfiguracionZonificacion", prm);

            foreach(DataRow dar in dt.Tables[2].Rows)
            {
                string idAsociado=dar["idAsociado"].ToString().Trim()
                    , nombreAsociado = dar["nombreAsociado"].ToString().Trim()
                    ,idInvernadero=dar["idInvernadero"].ToString().Trim()
                    ,invernadero=dar["invernadero"].ToString().Trim()
                    ,surcos=dar["surcos"].ToString().Trim();

                sb.AppendLine(idAsociado.Equals(string.Empty) &&
                              nombreAsociado.Equals(string.Empty) &&
                              idInvernadero.Equals(string.Empty) &&
                              invernadero.Equals(string.Empty) &&
                              surcos.Equals(string.Empty)
                ? "<input type=\"text\" value=\"\">"
                : "<input type=\"text\" idAsociado=\"" + idAsociado + "\" nombreAsociado=\"" + nombreAsociado + "\" idInvernadero=\"" + idInvernadero + "\" invernadero=\"" + invernadero + "\" value=\"" + surcos + "\">");
               
            }


            if (dt.Tables[2].Rows.Count > 0)
            {
                return new string[] { "1", "ok", sb.ToString()};
            }
            else
            {
                return new string[] { "0", "No hay configuración existente", "warning" };
            }
     
        }
        catch (Exception x)
        {
            Log.Error(x);
            return new string[] { "0", "Error al obtener la configuración", "warning" };
        }
    }

} 