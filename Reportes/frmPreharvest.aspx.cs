using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using log4net;
using System.Text;

public partial class Reportes_frmPreHarvest : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Reportes_frmPreHarvest));
    private static int idUsuario = 0;
    private static string sTargetURLForSessionTimeout;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            //lo que se restsaura o se hace tras cada llamada
        }
        else
        {
            //lo que se hace para inicializar la pagina
            //idUsuario = int.Parse(Session["userIDInj"].ToString());
            sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();
        }
    }

    [WebMethod]
    public static string ObtenerPreharvest(string inicio, string fin)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        try
        {
            DataAccess da = new DataAccess();
            var dt = da.executeStoreProcedureDataTable("spr_PreharvestReporte", new Dictionary<string, object>() { 
                {"@inicio",inicio},
                {"@fin",fin}
            });

            StringBuilder sb = new StringBuilder();
            foreach(DataRow dr in dt.Rows)
            {
                

                var line = "<tr class=\"p_"+dr["idPlanta"]+" inv_"+dr["idInvernadero"]+"\">" +
                                "<td >" + dr["nombreplanta"].ToString() + "</td>" +
                                "<td >" + dr["claveinvernadero"].ToString() + "</td>" +
                                "<td >" + dr["zona"].ToString() + "</td>" +
                                "<td >" + dr["cosecha"].ToString() + "</td>" +
                                "<td >" + dr["FullPreharvest"].ToString() + "</td>" +
                                "<td >" + dr["hora"].ToString() + "</td>" +
                                "<td >" + dr["vSecciones"].ToString() + "</td>" +
                                "<td >" + dr["nValue"].ToString() + "</td>" +
                                "<td >" + dr["vQualityType"].ToString() + "</td>" +
                                "<td >" + dr["folio"].ToString() + "</td>" +
                                "<td >" + dr["folionuevo"].ToString() + "</td>" +
                           "</tr>";

                sb.AppendLine(line);

            }

            return sb.ToString();
        }
        catch (Exception x)
        {
            log.Error(x.Message);
            return "<script>popUpAlert('No se pudo cargar las actividades.','error');</script>";
        }
    }
}