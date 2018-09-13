using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

public partial class configuracion_TargetVariedad : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_TargetVariedad));
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] obtenerTargetVariedad()
    {
        try
        { int idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            //if (idPlanta.Equals(null) || idPlanta.Equals(0))
            //{
               
            //}
            DataAccess da = new DataAccess();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idPlanta",idPlanta);
            DataTable dt = da.executeStoreProcedureDataTable("spr_ObtenerTargetVariedad",prm);

            if (dt.Rows.Count > 0)
            {
                StringBuilder sbTargetVariedad = new StringBuilder();
                sbTargetVariedad.AppendLine("<table class=\"gridView\" id=\"tblTargetVariedad\" cellspacing=\"0\">");
                sbTargetVariedad.AppendLine("<thead>");
                sbTargetVariedad.AppendLine(string.Format("<tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th></tr>", "Producto", "Variedad", "Habilidad", "Etapa", "Departamento", "Target"));
                sbTargetVariedad.AppendLine("</thead>");
                sbTargetVariedad.AppendLine("<tbody>");
                foreach (DataRow R in dt.Rows)
                {
                    string idProducto = R["idProducto"].ToString()
                          , idEtapa = R["idEtapa"].ToString()
                          , idVariedad = R["idVariedad"].ToString()
                          , idTargetVariedad = R["idTargetVariedad"].ToString()
                          , Producto = R["Product"].ToString()
                          , Variedad = R["CodigoVariedad"].ToString()
                          , Habilidad = R["NombreHabilidad"].ToString()
                          , Etapa = R["NombreEtapa"].ToString()
                          , Departamento = R["NombreDepartamento"].ToString()
                          , Target = R["Target"].ToString();

                    sbTargetVariedad.Append(string.Format("<tr class=\"Producto\" idProducto=\"" + idProducto + "\" idEtapa=\"" + idEtapa + "\" idVariedad=\"" + idVariedad + "\" idPlanta=\"" + idPlanta + "\" idTargetVariedad=\"" + idTargetVariedad + "\" cargado=\"1\">"));
                    sbTargetVariedad.Append(Producto.Equals(string.Empty) ? string.Format("<td>-</td>") : string.Format("<td>" + Producto + "</td>"));
                    sbTargetVariedad.Append(Variedad.Equals(string.Empty) ? string.Format("<td>-</td>") : string.Format("<td>" + Variedad + "</td>"));
                    sbTargetVariedad.Append(Habilidad.Equals(string.Empty) ? string.Format("<td>-</td>") : string.Format("<td>" + Habilidad + "</td>"));
                    sbTargetVariedad.Append(Etapa.Equals(string.Empty) ? string.Format("<td>-</td>") : string.Format("<td>" + Etapa + "</td>"));
                    sbTargetVariedad.Append(Departamento.Equals(string.Empty) ? string.Format("<td>-</td>") : string.Format("<td>" + Departamento + "</td>"));
                    sbTargetVariedad.Append(Target.Equals(string.Empty) ? string.Format("<td><input type=\"text\" class=\"Target\" idProducto=\"" + idProducto + "\" idEtapa=\"" + idEtapa + "\" idVariedad=\"" + idVariedad + "\" idPlanta=\"" + idPlanta + "\" idTargetVariedad=\"" + idTargetVariedad + "\" value=\"\"/></td>") :
                    string.Format("<td><input type=\"text\" class=\"Target\" idTargetVariedad=\"" + idTargetVariedad + "\" value=\"" + Target + "\"/></td>"));
                    sbTargetVariedad.Append(string.Format("</tr>"));
                }

                return new string[] { "1", "Datos obtenidos con éxito", sbTargetVariedad.ToString() };
            }
            else
            {
                return new string[] {"0","El proceso no generó ningún resultado","warning" };
            }
        }
        catch (Exception)
        {
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            throw;
        }
    }


    [WebMethod]
    public static string[] guardarConfiguracion(TargetVariedad[] TargetVariedad)
    {
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataTable dtTargetVariedad = new DataTable();
        DataRow dr;
        DataTable dtResponse = new DataTable();

        if (TargetVariedad.Length == 0)
        {
            return new string[] { "0", "No se realizaron cambios.", "warning" };
        }
        else
        {
            try
            {
                dtTargetVariedad.Columns.Add("idEtapa");
                dtTargetVariedad.Columns.Add("idVariedad");
                dtTargetVariedad.Columns.Add("idPlanta");
                dtTargetVariedad.Columns.Add("Target");
                dtTargetVariedad.Columns.Add("idTargetVariedad");

                foreach (TargetVariedad tv in TargetVariedad)
                {
                    dr = dtTargetVariedad.NewRow();
                    dr["idEtapa"] = tv.idEtapa;
                    dr["idVariedad"] = tv.idVariedad;
                    dr["idPlanta"] = tv.idPlanta;
                    dr["Target"] = tv.Target;
                    dr["idTargetVariedad"] = tv.idTargetVariedad;
                    dtTargetVariedad.Rows.Add(dr);
                }

                prm.Add("@targetVariedad", dtTargetVariedad);
                dtResponse = da.executeStoreProcedureDataTable("spr_GuardarTargetPorVariedad", prm);

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