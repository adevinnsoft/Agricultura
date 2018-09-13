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
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] cargarddls()
    {
        DataAccess da = new DataAccess();
        DataSet dataset = new DataSet();
        StringBuilder sb1 = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        Dictionary<string, object> prm = new Dictionary<string, object>();

        try
        {
            dataset = da.executeStoreProcedureDataSet("spr_ObtenerEficienciaPlantas", null);
            //cargamos los datos del ddlPlanta
            sb1.AppendLine("<option value='' selected='selected'>--Seleccione--</option>");
            sb1.AppendLine("<option value='0' idPlanta='0'>Todas</option>");

            foreach (DataRow R in dataset.Tables[0].Rows)
            {
                string idPlanta = R["idPlanta"].ToString().Trim(), nombrePlanta = R["NombrePlanta"].ToString().Trim();
                sb1.AppendLine("<option value='" + idPlanta + "' idPlanta='" + idPlanta + "'>" + nombrePlanta + "</option>");
            }

            //cargamos los datos del ddlEstado
            sb2.AppendLine("<option value='' selected='selected'>--Seleccione--</option>");
            sb2.AppendLine("<option value='0' idEstadoInfestacion='0'>Todos</option>");

            foreach (DataRow R in dataset.Tables[1].Rows)
            {
                string idEstadoInfestacion = R["idEstadoInfestacion"].ToString().Trim(), Estado = R["Estado"].ToString().Trim();
                sb2.AppendLine("<option value='" + idEstadoInfestacion + "' idEstadoInfestacion='" + idEstadoInfestacion + "'>" + Estado + "</option>");
            }

            //cargamos los datos del ddlInfestacion
            sb3.AppendLine("<option value='' selected='selected'>--Seleccione--</option>");
            sb3.AppendLine("<option value='0' idInfestacion='0'>Todas</option>");

            foreach (DataRow R in dataset.Tables[2].Rows)
            {
                string idInfestacion = R["idPlaga"].ToString().Trim(), nombreInfestacion = R["nombreComun"].ToString().Trim();
                sb3.AppendLine("<option value='" + idInfestacion + "' idInfestacion='" + idInfestacion + "'>" + nombreInfestacion + "</option>");
            }

            if (dataset.Tables[0].Rows.Count > 0 && dataset.Tables[1].Rows.Count > 0 && dataset.Tables[2].Rows.Count > 0)
            {
                if (dataset.Tables[3].Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "ok", sb1.ToString(), sb2.ToString(), sb3.ToString() };
                }
                else
                {
                    return new string[] { "0", "Error al cargar las listas de selección", "warning" };
                }
            }
            else
            {
                return new string[] { "0", "El proceso no devolvió ningún resultado.", "warning" };
            }

        }
        catch (Exception ex)
        {

            log.Error(ex);
            return new string[] { "0", "Surgió un error durante el proceso", "warning" };
        }
    }

    [WebMethod]
    public static string[] ObtenerEficiencia(int idLider, string semanaDesde, string anioDesde, string semanaHasta, string anioHasta)
    {
        string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
        DataAccess da = new DataAccess();
        DataSet ds = new DataSet();

        Dictionary<string, object> param = new Dictionary<string, object>();
        StringBuilder sb = new StringBuilder();

        try
        {
            param.Add("@accion", 2);
            param.Add("@idPlanta", HttpContext.Current.Session["idPlanta"].ToString());
            param.Add("@idLider", idLider);
            param.Add("@semanaDesde", semanaDesde);
            param.Add("@anioDesde", anioDesde);
            param.Add("@semanaHasta", semanaHasta);
            param.Add("@anioHasta", anioHasta);
            ds = da.executeStoreProcedureDataSet("spr_EficienciasObtener", param);

            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            if (idLider == 0)
            {
                sb.AppendLine("<th>Lider</th>");
                sb.AppendLine("<th>Eficiencia Lider</th>");
            }
            sb.AppendLine("<th>Fecha</th>");
            sb.AppendLine("<th>Asociado</th>");
            sb.AppendLine("<th>Eficiencia</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            foreach (DataRow R in ds.Tables[0].Rows)
            {
                string idplanta = R["Planta"].ToString().Trim(),
                    idlider = R["idLider"].ToString().Trim(),
                    lider = R["Lider"].ToString().Trim(),
                    fecha = R["Fecha"].ToString().Trim(),
                    idasociado = R["idAsociado"].ToString().Trim(),
                    asociado = R["Asociado"].ToString().Trim(),
                    eficiencia = R["Eficiencia"].ToString().Trim(),
                    eficienciaLider = "";

                DataRow[] dt = ds.Tables[1].Select("idLider = " + idlider);
                eficienciaLider = dt[0][1].ToString();

                sb.AppendLine("<tr idplanta='" + idplanta + "'>");
                if (idLider == 0)
                {
                    sb.AppendLine("<td idlider='" + idlider + "'>" + lider + "</td>");
                    sb.AppendLine("<td>" + eficienciaLider + "</td>");
                }
                sb.AppendLine("<td>" + fecha + "</td>");
                sb.AppendLine("<td idasociado='" + idasociado + "'>" + asociado + "</td>");
                sb.AppendLine("<td>" + eficiencia + "</td>");
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");


            if (idLider != 0)
            {
                DataRow[] dtl = ds.Tables[1].Select("idLider = " + idLider);
                sb.AppendLine("@");
                sb.AppendLine("<table><tr>");
                sb.AppendLine("<td>" + "Eficiencia Lider" + "</td>");
                sb.AppendLine("<td>" + dtl[0][1].ToString() + "</td>");
                sb.AppendLine("</tr></table>");
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return new string[] { "1", "ok", sb.ToString() };
            }
            else
            {
                return new string[] { "0", "info", "No se encontró registro de Eficiencia para el rango de semanas especificado." };
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "0", "info", "La consulta no regreso resultados con los filtros especificados" };
        }
    }

    [WebMethod(EnableSession = true)]
    public static string comboLideres()
    {
        StringBuilder response = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_EficienciasObtener", new Dictionary<string, object>() { { "@accion", 1 }, { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) } });

            response.Append("<select id='ddlLideres'><option value='0' selected>--Seleccione--</option>");
            foreach (DataRow item in dt.Rows)
            {
                response.Append("<option value='" + item[0] + "'>" + item[0] + " - " + item[1] + "</option>");
            }
            response.Append("</select>");
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
            return response.ToString();
        }

        return response.ToString();
    }

}