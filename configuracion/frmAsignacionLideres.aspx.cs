using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using Incidencias;


public partial class frmAsignacionLideres : BasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["usernameInj"] == null)
                Response.Redirect("~/frmLogin.aspx", false);
        }
    }

    #region metodos


    [WebMethod(EnableSession = true)]
    public static string tablaLider()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dtLider = dataaccess.executeStoreProcedureDataTable("spr_ObtieneLideres", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });
            DataTable dtGerente = dataaccess.executeStoreProcedureDataTable("spr_ObtieneGerentes", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"] } });

            response += "<table id='tablaLider' class='gridView'><thead><tr><th>" + "Lideres/Gerentes" + "</th>";
            foreach (DataRow item in dtGerente.Rows)
            {
                response += "<th>" + item["vNombre"] + "</th>";
            }
            response += "</tr></thead><tbody>";
            foreach (DataRow item in dtLider.Rows)
            {
                response += "<tr>"
                    + "<td class='left'>"
                    + "<label><span></span>" + item["vNombre"] + "</label>"
                    + "</td>";

                foreach (DataRow item2 in dtGerente.Rows)
                {
                    response += "<td>"
                    + "<input class='check-with-label gerenteLider' type='radio' id='" + item["idUsuario"] + "|" + item2["idUsuario"] + "' value='" + item["idUsuario"] + "|" + item2["idUsuario"] + "' " + (item["idGerente"].ToString() == item2["idUsuario"].ToString() ? "checked" : "") + " name='gerentes" + item["idUsuario"] + "' />"
                    + "<label class='label-for-check' for='" + item["idUsuario"] + "|" + item2["idUsuario"] + "'><span></span>"
                    + "<img id='img_" + item["idUsuario"] + "_" + item2["idUsuario"] + "' src='../comun/img/ok.png' style='display:none;' class='help' title='Guardado' width='18px'>"
                    + "</label>"
                   + "</td>";
                }

                response += "</tr>";
            }
            response += "</tbody></table>";

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
    public static string[] guardaRelacion(int idLider, int idGerente)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_AgregaQuitaLiderAGerente", new Dictionary<string, object>() { 
                { "@idLider", idLider }, 
                { "@idGerente", idGerente }, 
                { "@idusuario", HttpContext.Current.Session["idUsuario"]}
            });

            response[0] = result.Rows[0]["result"].ToString();

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[0] = ex.Message;
            return response;
        }

        return response;
    }

    #endregion

}