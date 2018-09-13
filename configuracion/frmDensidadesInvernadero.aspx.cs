using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using Incidencias;
using System.Text;


public partial class frmDensidadesInvernadero : BasePage
{
    //private static string currentFarm;
    //private static string sTargetURLForSessionTimeout;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["usernameInj"] == null)
                Response.Redirect("~/frmLogin.aspx", false);

            cargarEstados();
        }
    }

    #region metodos

    private void cargarEstados()
    {
        try
        {

        }
        catch (Exception ex)
        {
            Log.Error(ex);
        }
    }


    [WebMethod(EnableSession = true)]
    public static string obtieneInvernaderosDensidad()
    {
        var response = "";
        StringBuilder razones = new StringBuilder();
        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_InvernaderoSurcosObtener", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"] } });

            if (dt.Rows.Count != 0)
            {

                response += "<table id='tablaInvernadero' class='gridView'><thead><tr>"
                    + "<th style='width:50px !important;'>" + "Invernaderos" + "</th>"
                    + "<th style='width:50px !important;'>" + "Densidad" + "</th>"
                    + "<th style='width:50px !important;'>" + "Surcos" + "</th>"
                    + "<th style='width:50px !important;'>" + "Plantas x Surco" + "</th>"
                    + "</tr></thead><tbody>";

                foreach (DataRow item in dt.Rows)
                {
                    response += "<tr id='" + item["idInvernadero"] + "'>"
                        + "<td idinvernadero='" + item["idInvernadero"] + "'>" + item["Invernadero"] + "</td>"
                        + "<td style='text-align:center;'>"
                        + "<label class='invisible'>" + item["Densidad"] + "</label>"
                        + "<input id='densidad-" + item["idInvernadero"] + "' vprev='" + item["Densidad"] + "' class='required floatValidate densidad' maxlength='6' style='text-align: center; width:80px;' id='densidad-" + item["idInvernadero"] + "' type='text' value='" + item["Densidad"] + "'>"
                        + "</td>"
                        + "<td id='surco-" + item["idInvernadero"] + "'>" + item["Surcos"] + "</td>"
                        + "<td id='planta-" + item["idInvernadero"] + "'>" + (Math.Round((Convert.ToDecimal(item["Densidad"].ToString())) / (Convert.ToDecimal(item["Surcos"].ToString()) ),2))+ "</td>"//" + string.Format("{0:N0}", Convert.ToInt32(Convert.ToDecimal(item["Planta"].ToString()))) + "
                        + "</tr>";
                }
                response += "</tbody></table>";
            }
            else
            {
                response += "<div><table class='index'><tr><td><h1>No existe configuración de surcos para los invenaderos de esta planta.</h1></td></tr></table></div>";
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
    public static string[] guardaDensidad(string[] matriz)
    {
        var response = new string[2];
        try
        {

            if (matriz.Length > 0)
            {
                DataTable dtr = new DataTable();
                dtr.Columns.Add("idInvernadero");

                dtr.Columns.Add("Plantulas");

                foreach (string row in matriz)
                {
                    if (row.Length > 0)
                    {
                        var col = row.Split(',');
                        DataRow dr = dtr.NewRow();
                        dr["idInvernadero"] = col[0].ToString();
                        dr["Plantulas"] = col[1].ToString();
                        dtr.Rows.Add(dr);
                    }
                    else
                    {
                    }
                }

                if (dtr.Rows.Count >= 1)
                {
                    DataAccess dataaccess = new DataAccess();
                    DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_DensidadInvernaderoGuarda", new Dictionary<string, object>() { 
                        { "@densidad", dtr } 
                    });

                    DataTable result = ds.Tables[0];
                    response[0] = result.Rows[0]["estatus"].ToString();
                    response[1] = result.Rows[0]["mensaje"].ToString();
                }

            }
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

    #endregion

}