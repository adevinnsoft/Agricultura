using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;
using log4net;
using System.Globalization;

public partial class frmReporteTractorista : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmReporteTractorista));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["usernameInj"] == null)
                Response.Redirect("~/frmLogin.aspx", false);
        }
    }

    [WebMethod(EnableSession = true)]
    public static string tablaReporte()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_TractoristaReporte", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });

            if (dt.Rows.Count >= 1)
            {
                response += "<table id='tablaReporte' class='gridView'><thead><tr>" +
                "<th style='width:50px !important;'>" + "Invernadero" + "</th>" +
                "<th style='width:50px !important;'>" + "Folios" + "</th>" +
                "<th style='width:50px !important;'>" + "Semáforo" + "</th>" +
                "<th style='width:50px !important;'>" + "Creado" + "</th>" +
                "<th style='width:50px !important;'>" + "Hora Cierre" + "</th>" +
                "<th style='width:50px !important;'>" + "Status" + "</th>" +
                "<th>" + "En ruta" + "</th>" +
                "</tr></thead><tbody>";
                foreach (DataRow item in dt.Rows)
                {
                    response += "<tr>"
                        + "<td>" + item["Invernadero"] + "</td>"
                        + "<td class='help' title='" + item["Folios"] + "'>" + item["nFolios"] + "</td>"
                        + "<td><div class='semaforo' style='background-color:#" + item["Semaforo"] + ";'/></td>"
                        + "<td>" + item["Creado"] + "</td>"
                        + "<td>" + item["Minutos"] + "</td>"
                        + "<td>" + item["Status"] + "</td>"
                        + "<td>" + item["Asignado"] + "</td>"
                        + "</tr>";
                }
                response += "</tbody></table>";
            }
            else
            {
                response = "<table id='tablaReporte' class='gridView'><tr><td>" + "No existen folios pendientes en esta planta" + "</td></tr></table>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string tablaHistoria()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_TractoristaReporteHistoria", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });

            if (dt.Rows.Count >= 1)
            {
                response += "<table id='tablaHistoria' class='gridView'><thead><tr>" +
                "<th style='width:50px !important;'>" + "Invernadero" + "</th>" +
                "<th style='width:50px !important;'>" + "Folio" + "</th>" +
                "<th style='width:50px !important;'>" + "hora cierre" + "</th>" +
                "<th style='width:50px !important;'>" + "Recogido" + "</th>" +
                "<th style='width:50px !important;'>" + "Entregado" + "</th>" +
                "<th style='width:50px !important;'>" + "Entregó" + "</th>" +
                "</tr></thead><tbody>";
                foreach (DataRow item in dt.Rows)
                {
                   
                    response += "<tr>"
                        + "<td>" + item["claveinvernadero"] + "</td>"
                        + "<td>" + item["folio"] + "</td>"
                        + "<td>" + item["fin"] + "</td>"
                        + "<td>" + item["FechaInicioTractorista"] + "</td>"
                        + "<td>" + item["FechaFinTractorista"] + "</td>"
                        + "<td>" + item["vNombre"] + "</td>"
                        + "</tr>";
                }
                response += "</tbody></table>";
            }
            else
            {
                response = "<table id='tablaReporte' class='gridView'><tr><td>" + "No existen folios pendientes en esta planta" + "</td></tr></table>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

  
}