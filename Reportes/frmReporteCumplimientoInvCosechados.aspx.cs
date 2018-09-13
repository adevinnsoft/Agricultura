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
using Newtonsoft.Json;


public partial class frmReporteCumplimientoInvCosechados : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmReporteCumplimientoInvCosechados));
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] ObtenerReporte(string Dia, string Variety, int idPlanta)
    {
        string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
        DataAccess da = new DataAccess();
        DataSet ds = new DataSet();

        Dictionary<string, object> param = new Dictionary<string, object>();
        //StringBuilder sb = new StringBuilder();
        //StringBuilder sb1 = new StringBuilder();
        //StringBuilder sb2 = new StringBuilder();

        try
        {
            param.Add("@Dia", Dia);
            param.Add("@idPlanta", HttpContext.Current.Session["idPlanta"].ToString());
            param.Add("@Variedad", Variety);
            ds = da.executeStoreProcedureDataSet("spr_rptCumplimientoInvCosechados", param);


            DataTable dt = ds.Tables[0];
            DataColumn toolTipCosecha = dt.Columns.Add("toolTipCosecha", typeof(String));
            DataColumn toolTipPreharvest = dt.Columns.Add("toolTipPreharvest", typeof(String));

            foreach (DataRow R in ds.Tables[0].Rows)
            {
                R["toolTipCosecha"] = R["Folios"].ToString() != "0" ? generarTabla(R["FoliosCosecha"].ToString(), R["SeccionesCosecha"].ToString(), R["CajasCosecha"].ToString(), "") : "";
                R["toolTipPreharvest"] = R["FullPreHarvest"].ToString() != "0" ? generarTabla(R["FoliosPreHarvest"].ToString(), R["SeccionesPreHarvest"].ToString(), R["CajasPreHarvest"].ToString(), R["CalidadPreHarvest"].ToString()) : "";
            }

         
            if (ds.Tables[0].Rows.Count > 0)
            {
                return new string[] { "ok", JsonConvert.SerializeObject(ds.Tables[0]), JsonConvert.SerializeObject(ds.Tables[1]), JsonConvert.SerializeObject(ds.Tables[2]), JsonConvert.SerializeObject(ds.Tables[3]) };
            }
            else
            {
                return new string[] { "info", "No se encontró invernaderos con cosechas planeadas con filtros especificados" };
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "error", "Hubo un error al obtener el reporte: " + ex.Message };
        }
    }

    [WebMethod(EnableSession = true)]
    public static string generarTabla(string Folios, string Secciones, string Cajas, string Calidades)
    {
        StringBuilder response = new StringBuilder();

        string[] folios = Folios.Split(',');
        string[] secciones = Secciones.Split('|');
        string[] cajas = Cajas.Split(',');
        string[] calidades = Calidades.Split(',');

        try
        {
            response.AppendLine("<table class=\"gridView\" style=\"min-width:auto !important;\">");
            response.AppendLine("<thead>");
            response.AppendLine("<tr>");
            response.AppendLine("<th>Folios</th>");
            response.AppendLine("<th>Secciones</th>");
            response.AppendLine("<th>Cajas</th>");
            if (Calidades != "") { response.AppendLine("<th>Calidades</th>"); }
            response.AppendLine("</tr>");
            response.AppendLine("</thead>");
            response.AppendLine("<tbody>");

            for (int i = 0; i <= folios.Length - 1; i++)
            {
                response.AppendLine("<tr>");
                response.AppendLine("<td>" + folios[i] + "</td>");
                response.AppendLine("<td>" + (i < secciones.Length ? secciones[i] : "0") + "</td>");
                response.AppendLine("<td>" + (i < cajas.Length ? cajas[i] : "0" ) + "</td>");
                if (Calidades != "") { response.AppendLine("<td>" + calidades[i] + "</td>"); }
                response.AppendLine("</tr>");
            }
            response.AppendLine("</table>");

            return response.ToString();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return ex.Message;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string comboVariedades()
    {
        StringBuilder response = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_getVariedades", new Dictionary<string, object>() { });

            response.Append("<select id='ddlVariedades'><option value='0' selected>--Todas--</option>");
            foreach (DataRow item in dt.Rows)
            {
                response.Append("<option value='" + item["CodigoVariedad"] + "'>" + item["CodigoVariedad"] + " - " + item["Segmento"] + "</option>");
            }
            response.Append("</select>");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
            return response.ToString();
        }

        return response.ToString();
    }

}