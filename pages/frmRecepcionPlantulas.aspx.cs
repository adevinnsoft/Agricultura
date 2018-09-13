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


public partial class frmRecepcionPlantulas : BasePage
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
            //DataAccess da = new DataAccess();
            //DataTable dt = da.executeStoreProcedureDataTable("spr_EstadosCapturasObtener", new Dictionary<string, object>() { { "@idioma", idioma } });
            ListItem L = null;
            /*foreach (DataRow R in dt.Rows)
            {
                L = new ListItem(R["Estado"].ToString(), R["idEstado"].ToString());
                L.Attributes.Add("idEstado", R["idEstado"].ToString());
                chkEstado.Items.Add(L);
            }*/
            L = new ListItem("Sin Recibir", "0");
            L.Attributes.Add("idEstado", "0");
            chkEstado.Items.Add(L);

            L = new ListItem("Completo", "1");
            L.Attributes.Add("idEstado", "1");
            chkEstado.Items.Add(L);

            L = new ListItem("Incompleto", "2");
            L.Attributes.Add("idEstado", "2");
            chkEstado.Items.Add(L);
            chkEstado.Attributes.CssStyle.Value = "width:100%;";
            chkEstado.Items[0].Selected = true;
        }
        catch (Exception ex)
        {
            Log.Error(ex);
        }
    }


    [WebMethod(EnableSession = true)]
    public static string obtieneRequerimeintos()
    {
        var response = "";
        StringBuilder razones = new StringBuilder();
        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("[spr_RequerimientosEnviadosObtener]", new Dictionary<string, object>() { 
                { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }
                ,{ "@idPlanta", HttpContext.Current.Session["idPlanta"] }
            });
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow item in dt2.Rows)
                {
                    razones.Append("<option value='" + item["idMerma"] + "'>" + item[getIdioma() == 1 ? "Razon" : "Razon_EN"] + "</option>");
                }

                response += "<table id='tablaRequerimientos' class='gridView'><thead><tr>";
                foreach (DataColumn item in dt.Columns)
                {
                    if (item.ColumnName.Contains("|"))
                    {
                        response += "<th>" + (getIdioma() == 1 ? item.ColumnName.Split('|')[0] : item.ColumnName.Split('|')[1]) + "</th>";
                    }
                }
                response += "</thead></tr><tbody>";
                foreach (DataRow item in dt.Rows)
                {

                    response += "<tr estado='" + item["estado"] + "'  id='" + item["idEnvio"] + "'>"
                    + "<td>" + item["Patrón/Variedad|Pattern/Variety"].ToString() + "</td>"
                    + "<td>" + item["Compañía|Company"].ToString() + "</td>"
                    + "<td>" + item["Chofer|Driver"].ToString() + "</td>"
                    + "<td>" + item["Planta|Farm"].ToString() + "</td>"
                    + "<td>" + item["Fecha Envío|Shipping Date"].ToString() + "</td>"
                        //+ "<td>" + item["Estado|Status"].ToString() + "</td>"
                    + "<td>" + item["Quien Envía|Who Send"].ToString() + "</td>"
                    + "<td id='cantidad-" + item["idEnvio"] + "'>" + string.Format("{0:N0}", Convert.ToInt32(item["Charolas|Trays"].ToString())) + "</td>"
                    + "<td>" + item["Invernadero|Greenhouse"].ToString() + "</td>"
                        + "<td style='text-align:center;'>"
                        + " <label style='display:none;'>" + item["Recibido|Received"] + "</label>"
                        + " <input vprev='" + Convert.ToInt32(Convert.ToDecimal(item["Recibido|Received"].ToString())) + "' class='required intValidate enviados' maxlength='5' style='text-align: center; width:40px;' id='envio-" + item["idEnvio"] + "' type='text' value='" + Convert.ToInt32(Convert.ToDecimal(item["Recibido|Received"].ToString())) + "'>"
                        + "</td>"

                    + "<td id='merma-" + item["idEnvio"] + "'>" + string.Format("{0:N0}", Convert.ToInt32(Convert.ToDecimal(item["Merma|Decrease"].ToString()))) + "</td>"
                    + "<td><select id='razon-" + item["idEnvio"] + "' vprev='" + item["Razones|Reasons"] + "' class='required razones'>" + razones + "</select></td>"
                    + "<td><textarea id='comentario-" + item["idEnvio"] + "' style='width:90%;' rows='3' cols='30' vprev='" + item["Observaciones|Observations"].ToString().Replace("&nbsp;", "").Trim() + "' value='" + item["Observaciones|Observations"].ToString().Replace("&nbsp;", "").Trim() + "' class='comentarios'>" + item["Observaciones|Observations"].ToString().Replace("&nbsp;", "").Trim() + "</textarea></td>"
                    + "</tr>";
                }
                response += "</tbody></table>";
            }
            else
            {
                response += "<div><table class='index'><tr><td><h1>No existen envios de requerimientos en ningunas de las plantas.</h1></td></tr></table></div>";
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
    public static string[] guardaEnvio(string ids, string plantulas, string mermas, string razones, string observaciones)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_EnviosPlantulasGuardar", new Dictionary<string, object>() { 
            { "@idioma", (HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0) }, 
            { "@ids", ids }, 
            { "@plantulas", plantulas}, 
            { "@mermas", mermas}, 
            { "@razones", razones}, 
            { "@observaciones", observaciones}, 
            { "@idLider", HttpContext.Current.Session["idLider"].ToString() },
            { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() } 
            });

            DataTable result = ds.Tables[0];
            //DataTable result2 = ds.Tables[1];

            //HttpContext.Current.Session["idEnvio"] = result2.Rows[0]["idEnvio"].ToString();
            response[0] = result.Rows[0]["msg"].ToString();
            response[1] = result.Rows[0]["detalle"].ToString();

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