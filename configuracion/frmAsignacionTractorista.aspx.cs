using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using Incidencias;


public partial class frmAsignacionTractorista : BasePage
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
    public static string tablaInvernadero()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dtInvernaderos = dataaccess.executeStoreProcedureDataTable("spr_InvernaderosObtiene", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });

            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_TractoristasObtiene", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });
            DataTable dtTractoristas = ds.Tables[0];
            DataTable dtTractoristaInv = ds.Tables[1];

            response += "<table id='tablaInvernadero' class='gridView'><thead><tr><th style='width:50px !important;'>" + "Invernaderos/Tractoristas" + "</th>";
            foreach (DataRow item in dtTractoristas.Rows)
            {
                response += "<th><span class='help' title='¿Quieres asignar o quitar a este Tractorista en todos los invernaderos? <br />"
                    + "<input class=\"check-with-label\" type=\"radio\" id=\"all\" value=\"1\" name=\"selected\"  onClick=\"allSelect(" + item["idUsuario"] + ", true);\"/>"
                    + "<label class=\"label-for-check\" for=\"all\"><span></span>Asignar</label>"
                    + "<br />"
                    + "<input class=\"check-with-label\" type=\"radio\" id=\"none\" value=\"0\" name=\"selected\"  onClick=\"allSelect(" + item["idUsuario"] + ",false);\"/>"
                    + "<label class=\"label-for-check\" for=\"none\"><span></span>Quitar</label>"
                    + "'>" + item["vNombre"] + "</th>";
            }
            response += "</tr></thead><tbody>";
            foreach (DataRow item in dtInvernaderos.Rows)
            {
                response += "<tr>"
                    + "<td>" + item["claveInvernadero"] + "</td>";

                foreach (DataRow item2 in dtTractoristas.Rows)
                {
                    bool check = false;
                    //foreach (DataRow item3 in dtTractoristaInv.Rows)
                    //{
                    //    check = item["idInvernadero"].ToString() == item3["idInvernadero"].ToString() && item2["idUsuario"].ToString() == item3["idUsuario"].ToString();
                    //    if (check) { break; }
                    //}

                    DataRow[] foundRows;
                    foundRows = dtTractoristaInv.Select("idInvernadero = " + item["idInvernadero"].ToString() + " and idUsuario = " + item2["idUsuario"].ToString());
                    check = foundRows.Length > 0 ? true : false;

                    response += "<td>"
                    + "<input class='check-with-label tractoristaInv' type='checkbox' id='" + item["idInvernadero"] + "|" + item2["idUsuario"] + "' value='" + item["idInvernadero"] + "|" + item2["idUsuario"] + "' " + (check ? "checked" : "") + " name='invernaderos" + item["idInvernadero"] + "' />"
                    + "<label class='label-for-check' for='" + item["idInvernadero"] + "|" + item2["idUsuario"] + "'><span></span>"
                    + "<img id='img_" + item["idInvernadero"] + "_" + item2["idUsuario"] + "' src='../comun/img/ok.png' style='display:none;' class='help' title='Guardado' width='18px'>"
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
    public static string[] guardaRelacion(int idInvernadero, int idTractorista, bool activo)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_AgregaQuitaTractorista", new Dictionary<string, object>() { 
                { "@idInvernadero", idInvernadero }, 
                { "@idTractorista", idTractorista }, 
                { "@activo", activo }, 
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