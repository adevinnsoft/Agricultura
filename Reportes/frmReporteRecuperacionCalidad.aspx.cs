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


public partial class frmReporteResumenCosecha : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmReporteResumenCosecha));


    private static string STR_TD = "td";
    private static string STR_TH = "th";
    private static string STR_TR = "tr";
    private static string STR_THEAD = "thead";
    private static string STR_TBODY = "tbody";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] obtenerReporte(string inicio, string fin, string variedad)
    {

        StringBuilder response = new StringBuilder();
        try
        {
            int lang = HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0;
            DataAccess dataaccess = new DataAccess();

            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ReporteRecuperacionCalidadV2"
                , new Dictionary<string, object>() {

                     { "@inicio",     inicio   }
                    , {"@fin",fin }
                    ,{ "@idPlanta",  HttpContext.Current.Session["idPlanta"].ToString()}
                    ,{ "@variedad",  (variedad.Equals("0")? null : variedad)}
             });

            String jsonFolios = "";
            String jsonLbs = "";
            String jsonTipos = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                jsonFolios = GetDataTableToJson(ds.Tables[0]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    jsonLbs = GetDataTableToJson(ds.Tables[1]);
                    jsonTipos = GetDataTableToJson(ds.Tables[2]);
                }

                return new string[] { "1", "ok", response.ToString(), jsonFolios, jsonLbs, jsonTipos };
            }
            else
            {
                return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
        }
    }

    [WebMethod]
    public static String getDetail(String Folio)
    {
        string result = "<table style='min-width:120px;' class='gridView'><tbody><tr>" +
                        "<th>Seccion FullPreharvest</th>" +
                        "<th>Calidad</th>" +
                        "<th>Seccion Cosecha</th>" +
                        "<th>Calidad</th>";

        DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_reporteRecuperacionCalidadDetalle", new Dictionary<string, object>() { { "@folio", Folio } });

        if (dt.Rows.Count == 0)
        {
            result = "<tr><td>No hay Información de FullPreharvest para el folio</td></tr>";
        }
        else
        {

            foreach (DataRow d in dt.Rows)
            {
                int calidadp = 0;
                int calidadf = 0;
                try
                {
                    calidadp = int.Parse(d["idcalidadp"].ToString());
                    calidadf = int.Parse(d["idcalidadf"].ToString());
                }
                catch (Exception e)
                {
                    //do noth
                }
                if (calidadf < calidadp)
                {
                    result += " <tr class='siR'><td>" + d["seccionp"] + "</td> <td>" + d["calidadp"] + "</td> <td>" + d["seccionf"] + "</td> <td>" + d["calidadf"] + "</td></tr>";
                }
                else
                {
                    result += " <tr><td>" + d["seccionp"] + "</td> <td>" + d["calidadp"] + "</td> <td>" + d["seccionf"] + "</td> <td>" + d["calidadf"] + "</td></tr>";
                }



            }
            result += "</tbody></table>";
        }
        return result;
    }


    [WebMethod]
    public static string[] cargar(string idPlanta)
    {

        StringBuilder ddlVariedad = new StringBuilder();

        try
        {
            idPlanta = idPlanta.Equals("0") ? HttpContext.Current.Session["idPlanta"].ToString() : idPlanta;

            DataAccess da = new DataAccess();
            DataTable ds = da.executeStoreProcedureDataTable("spr_ConfiguracionesReporteRecuperacionCalidadV2", new Dictionary<string, Object> { { "@idPlanta", idPlanta } });
            if (ds.Rows.Count != 0)
            {


                //options de variedades
                ddlVariedad.Append(tagGen("option", "value=\"0\"", "-- Todos --"));
                foreach (DataRow row in ds.Rows)
                {
                    ddlVariedad.Append(tagGen("option", "value=\"" + row["idVariedad"].ToString() + "\"", row["nombre"].ToString()));
                }

                return new string[] { "1", "ok", ddlVariedad.ToString() };
            }
            else
            {
                return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            }
        }
        catch (Exception)
        {
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };

        }

    }




    private static String tagGen(string tag, string attr, string value)
    {
        return String.Format("<{0} {1}>{2}</{0}>", tag, attr, value);
    }

    private static String tagGen(string tag, string value)
    {
        return tagGen(tag, string.Empty, value);
    }




}