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


public partial class Reportes_frmReporteInfestaciones : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Reportes_frmReporteInfestaciones));
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

            dataset = da.executeStoreProcedureDataSet("spr_ObtenerInfestacionesPlantas", null);

            //cargamos los datos del ddlPlanta
            sb1.AppendLine("<option value=\"\" selected=\"selected\">--Seleccione--</option>");
            sb1.AppendLine("<option value=\"0\" idPlanta=\"0\">Todas</option>");

            foreach (DataRow R in dataset.Tables[0].Rows)
            {
                string idPlanta = R["idPlanta"].ToString().Trim(), nombrePlanta = R["NombrePlanta"].ToString().Trim();

                sb1.AppendLine("<option value=\"" + idPlanta + "\" idPlanta=\"" + idPlanta + "\">" + nombrePlanta + "</option>");
            }

            //cargamos los datos del ddlEstado
            sb2.AppendLine("<option value=\"\" selected=\"selected\">--Seleccione--</option>");
            sb2.AppendLine("<option value=\"0\" idEstadoInfestacion=\"0\">Todos</option>");
          
            foreach (DataRow R in dataset.Tables[1].Rows)
            {
                string idEstadoInfestacion = R["idEstadoInfestacion"].ToString().Trim(), Estado = R["Estado"].ToString().Trim();

                sb2.AppendLine("<option value=\"" + idEstadoInfestacion + "\" idEstadoInfestacion=\"" + idEstadoInfestacion + "\">" + Estado + "</option>");
            }


            //cargamos los datos del ddlInfestacion
            sb3.AppendLine("<option value=\"\" selected=\"selected\">--Seleccione--</option>");
            sb3.AppendLine("<option value=\"0\" idInfestacion=\"0\">Todas</option>");

            foreach (DataRow R in dataset.Tables[2].Rows)
            {
                string idInfestacion = R["idPlaga"].ToString().Trim(), nombreInfestacion = R["nombreComun"].ToString().Trim();

                sb3.AppendLine("<option value=\"" + idInfestacion + "\" idInfestacion=\"" + idInfestacion + "\">" + nombreInfestacion + "</option>");
            }

            if (dataset.Tables[0].Rows.Count > 0 && dataset.Tables[1].Rows.Count > 0 && dataset.Tables[2].Rows.Count > 0)
            {
                if (dataset.Tables[3].Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "ok", sb1.ToString(),sb2.ToString(),sb3.ToString() };
                }
                else
                {
                    return new string[] { "0","Error al cargar las listas de selección", "warning" };
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
    public static string[] ObtenerInfestacionesInvernadero(string semanaDesde,string anioDesde,string semanaHasta,string anioHasta)
    {
        string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataSet ds = new DataSet(); 
        StringBuilder sb = new StringBuilder();
        StringBuilder sb_2 = new StringBuilder();

        try
        {
            prm.Add("@semanaDesde", semanaDesde);
            prm.Add("@anioDesde", anioDesde);
            prm.Add("@semanaHasta", semanaHasta);
            prm.Add("@anioHasta", anioHasta);
            ds = da.executeStoreProcedureDataSet("spr_ObtenerInfestacionesInvernadero", prm);

            
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>PLANTA</th>");
            sb.AppendLine("<th>INVERNADERO</th>");
            sb.AppendLine("<th>FECHA DE REPORTE</th>");
            sb.AppendLine("<th>SECCIONES</th>");
            sb.AppendLine("<th>SURCOS</th>");
            sb.AppendLine("<th>BASE</th>");
            sb.AppendLine("<th>INFESTACION</th>");
            sb.AppendLine("<th>CANTIDAD</th>");
            sb.AppendLine("<th>ESTADO</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>"); 
            sb.AppendLine("<tbody>");
            foreach (DataRow R in ds.Tables[0].Rows)
            {
                string idPlanta = R["idPlanta"].ToString().Trim(),
                       nombrePlanta = R["Planta"].ToString().Trim(),
                       Invernadero = R["Invernadero"].ToString().Trim(),
                       fechaReporte = R["fechaReporte"].ToString().Trim(),
                       Secciones = R["Secciones"].ToString().Trim(),
                       delSurco = R["delSurco"].ToString().Trim(),
                       alSurco = R["alSurco"].ToString().Trim(),
                       Base = R["Base"].ToString().Trim(),
                       Infestacion = R["Infestacion"].ToString().Trim(),
                       Cantidad = R["Cantidad"].ToString().Trim(),
                       Estado = R["Estado"].ToString().Trim(),
                       idEstadoInfestacion = R["idEstadoInfestacion"].ToString().Trim(),
                       idInfestacion = R["idPlaga"].ToString();

                
                sb.AppendLine("<tr idInvernadero=\"" + Invernadero + "\" idEstado=\"" + idEstadoInfestacion + "\"  idPlanta=\"" + idPlanta + "\" idInfestacion=\"" + idInfestacion + "\" class=\"Invernadero\">");
                sb.AppendLine(nombrePlanta.Equals(string.Empty) ? "<td class=\"Planta\">-</td>" : "<td class=\"Planta\">" + nombrePlanta + "</td>");
                sb.AppendLine(Invernadero.Equals(string.Empty) ? "<td class=\"Invernadero\">-</td>" : "<td class=\"Invernadero\">" + Invernadero + "</td>");
                sb.AppendLine(fechaReporte.Equals(string.Empty) ? "<td class=\"fechaReporte\">-</td>" : "<td class=\"fechaReporte\">" + fechaReporte + "</td>");
                sb.AppendLine(Secciones.Equals(string.Empty) ? "<td class=\"Secciones\">-</td>" : "<td class=\"Secciones\">" + Secciones+ "</td>");
                sb.AppendLine(delSurco.Equals(string.Empty) && alSurco.Equals(string.Empty) ? "<td class=\"Surcos\">-</td>" : "<td class=\"Surcos\">" + delSurco + " " + "al" + " " + alSurco + "</td>");
                sb.AppendLine(Base.Equals(string.Empty) ? "<td class=\"Base\">-</td>" : "<td class=\"Base\">" + Base + "</td>");
                sb.AppendLine(Infestacion.Equals(string.Empty) ? "<td class=\"Infestacion\">-</td>" : "<td class=\"Infestacion\">" + Infestacion + "</td>");
                sb.AppendLine(Cantidad.Equals(string.Empty) ? "<td class=\"Cantidad\">-</td>" : "<td class=\"Cantidad\">" + Convert.ToInt32(Cantidad) + "</td>");
                sb.AppendLine(Estado.Equals(string.Empty) ? "<td class=\"Estado\">-</td>" : "<td idEstado=\"" + idEstadoInfestacion + "\" class=\"Estado\">" + Estado + "</td>");
                sb.AppendLine("</tr>");
               
            }
            sb.AppendLine("</tbody>");

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "ok",sb.ToString() };
                }
                else
                {
                    return new string[] { "0", ds.Tables[1].Rows[0]["Mensaje"].ToString(),"warning" };
                }
            }
            else
            {
                return new string[] { "0", "No se encontró registro de infestaciones para el rango de semanas especificado.", "warning" };
            }
            
        }
        catch (Exception x)
        {
          
            log.Error(x);
            return new string[] { "0","El proceso no generó ningún resultado", "warning" };
        }

    }
}