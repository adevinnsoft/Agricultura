using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.Services;
using System.Text;

public partial class frmChecklistInfestaciones : BasePage
{
    public static string estados = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            estadosInfestaciones();
        }
    }

    [WebMethod(EnableSession = true)]
    public static string estadosInfestaciones()
    {
        DataAccess dataaccess = new DataAccess();
        estados = "";
        var index = "";
        var valores = "";
        var labels = "";
        var res = "";
        try
        {

            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_EstadosInfestacionesObtener", new Dictionary<string, object>() { { "@idioma", getIdioma() } });
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {

                int i = 1;
                foreach (DataRow item in dt.Rows)
                {
                    index += i + ",";
                    valores += item["idEstadoInfestacion"] + ",";
                    labels += "\"" + item["Estado"] + "\",";
                    res += "\"" + item["Resuelto"] + "\",";
                    i++;
                }

                estados += "<div style='width:" + (dt.Rows.Count * 80) + "px;'>" 
                + "<input class='estados' type='text'"
                + "data-provide='slider'"
                + "data-slider-ticks='[" + index.Substring(0, index.Length - 1) + "]'"
                + "data-slider-ticks-vals='[" + valores.Substring(0, valores.Length - 1) + "]'"
                + "data-slider-ticks-labels='[" + labels.Substring(0, labels.Length - 1) + "]'"
                + "data-slider-ticks-res='[" + res.Substring(0, res.Length - 1) + "]'"
                + "data-slider-min='" + 1 + "'"
                + "data-slider-max='" + dt.Rows.Count + "'"
                + "data-slider-step='" + 1 + "'"
                + "data-slider-value='@val'"
                + "data-slider-tooltip='hide' />"
                + "</div>";
            }
            else
            {
                estados += "";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            estados = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return estados;
        }

        return estados;
    }


    [WebMethod(EnableSession = true)]
    public static string tablaChecklist()
    {
        DataAccess dataaccess = new DataAccess();
        var response = "";

        try
        {

            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ChecklistInfestacionesGv", new Dictionary<string, object>() { { "@idioma", getIdioma() }, { "@idPlanta", HttpContext.Current.Session["idPlanta"] } });
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {

                response += "<table id='tablaCheckList' class='gridView'><thead><tr>"
                         + "<th>" + "Invernadero" + "</th>"
                         + "<th>" + "Fecha" + "</th>"
                         + "<th>" + "Percance/Infestación" + "</th>"
                         + "<th>" + "Surco" + "</th>"
                         + "<th>" + "Ubicación" + "</th>"
                         + "<th>" + "Zona" + "</th>"
                         + "<th>" + "Base" + "</th>"
                         + "<th>" + "Observaciones" + "</th>"
                         + "<th style='width:150px;'>" + "Atendido" + "</th>"
                         + "</tr></thead><tbody>";
                foreach (DataRow item in dt.Rows)
                {
                    response += "<tr idInfestacion='" + item["idInfestacionInvernadero"] + "'>"
                        + "<td idInvernadero='" + item["idInvernadero"] + "'>" + item["Invernadero"] + "</td>"
                        + "<td>" + item["fechaCaptura"] + "</td>"
                        + "<td idPoi='" + (item["idInfestacion"] == "0" ? item["idPercance"] : item["idInfestacion"]) + "' poi='" + (item["idInfestacion"] == "0" ? "percance" : "infestacion") + "'>" + (item["idInfestacion"] == "0" ? item["Percance"] : item["Infestacion"]) + "</td>"
                        + "<td>" + item["surcoDe"] + "-" + item["surcoA"] + "</td>"
                        + "<td>" + item["ubicacion"] + "</td>"
                        + "<td>" + item["Zona"] + "</td>"
                        + "<td>" + item["base"] + "</td>"
                        + "<td>" + item["observaciones"] + "</td>"

                        + "<td>"
                        + estados.Replace("@val", item["idEstadoInfestacion"].ToString())
                        /*+ "<div class='onoffswitch'>"
                        + "<input type='checkbox' name='onoffswitch' class='onoffswitch-checkbox' id='chk-" + item["idEstadoInfestacion"] + "' " + (item["idEstadoInfestacion"].ToString() != "1" ? "checked" : "") + " >"
                        + "<label class='onoffswitch-label' for='chk-" + item["idEstadoInfestacion"] + "'>"
                        + "<span class='onoffswitch-inner'></span>"
                        + "<span class='onoffswitch-switch'></span>"
                        + "</label>"
                        + "</div>"*/
                        + "</td>"
                        + "</tr>";
                }
                response += "</tbody></table>";
            }
            else
            {
                response += "<table><tbody><tr><td><h3>No hay Invernaderos Infestados en esta Planta</h3></td></tr></tbody></table>";
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
    public static string[] guardaCheckList(string ids, string valores)
    {
        var response = new string[3];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet result = dataaccess.executeStoreProcedureDataSet("spr_InfestacionesCheckListGuardar", new Dictionary<string, object>() { 
              { "@ids", ids }
            , { "@valores", valores }
            , { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }
            , { "@idioma", getIdioma() }
            });

            response[0] = result.Tables[0].Rows[0]["msg"].ToString();
            response[1] = result.Tables[0].Rows[0]["detalle"].ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[0] = "error";
            response[1] = ex.Message;
            return response;
        }

        return response;
    }

}