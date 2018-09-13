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


public partial class frmBrixCaptura : BasePage
{
    private static string currentFarm;
    private static string sTargetURLForSessionTimeout;

	protected void Page_Load(object sender, EventArgs e)
	{
        //if (Session["MultiplesPlantas"].ToString() == "true")
        //{
        //    currentFarm = this.Master.PlantaSeleccionada;
        //}
        //else
        //{
        //    currentFarm = Session["Planta"].ToString();
        //}
        //sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();

		if(!IsPostBack)
		{
			if(Session["usernameInj"] == null)
				Response.Redirect("~/frmLogin.aspx", false);

			cargarDatos();
			//cargar_ddlTipoInventario();
		}
	}

	#region metodos
	protected void cargarDatos()
	{
		try
		{
            divGridView.InnerHtml += gvCapturaBrix();
            divComboInvernaderos.InnerHtml = comboInvernaderos();
            divComboCosechas.InnerHtml = comboCosechas(0);
        }
		catch(Exception e)
		{
			Log.Error(e.ToString());
		}

	}

    [WebMethod(EnableSession = true)]
    public static string[] obtenerCalidad(double red70, double red80, double mix, double promedio)
    {
        var response = new String[4];
        response[2] = promedio.ToString("#.##");

        try
        {
            /*DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_CalidadObtiene", new Dictionary<string, object>() { { "@brix", promedio } });

            response[0] = dt.Rows[0]["Calidad"].ToString();
            response[1] = "#" + dt.Rows[0]["Color"].ToString();
            response[3] = dt.Rows[0]["idCalidad"].ToString();*/

            if (red80 >= 7.3 && (mix >= 7.5 || promedio >= 7.5)){
                response[0] = "NATURESWEET";
                response[1] = "#F7FF79";
                response[3] = "0";
                }
            else{
                if (red80 >= 7.2 && (mix >= 7.2 || promedio >= 7.2)){
                    response[0] = "CLUB";
                    response[1] = "#BCFF79";
                    response[3] = "1";
                }
                else{
                    response[0] = "COMMODITY";
                    response[1] = "#FFFFFF";
                    response[3] = "2";
                }
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[0] = ex.Message;
            response[1] = "#FFFFFF";
            response[2] = "-1";
            return response;
        }

        return response;
    }


    [WebMethod(EnableSession = true)]
    public static string comboInvernaderos()
    {
        StringBuilder response = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneInvernaderosDdl", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) } });

            response.Append("<select id='ddlInvernaderos'><option value='0' selected>--Seleccione--</option>");
            foreach (DataRow item in dt.Rows)
            {
                response.Append("<option value='" + item[0] + "' invernadero='" + item[1] + "'>" + item[1] + "</option>");
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

    [WebMethod(EnableSession = true)]
    public static string comboCosechas(int idInvernadero)
    {
        StringBuilder response = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneCosechas", new Dictionary<string, object>() { { "@idLider", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idInvernadero", idInvernadero } });

            response.Append("<select id='ddlCosechas'><option value='0' selected>--Seleccione--</option>");
            DateTime hoy = DateTime.Now.Date;
            foreach (DataRow item in dt.Rows)
            {
                if(Convert.ToDateTime(item["finProgramado"]).Date >= hoy ){
                    response.Append("<option value='" + item["idActividadPrograma"] + "'>" + item["inicioProgramado"] + "</option>");
                }
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


    [WebMethod(EnableSession = true)]
    public static string gvCapturaBrix()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DateTime time = TimeZone.obtenerHoraDeLaCuenta(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), TimeZone.obtenerZonaID(3));
            time = TimeZone.obtenerHoraDeLaCuenta(DateTime.SpecifyKind(time, DateTimeKind.Unspecified), TimeZone.obtenerZonaID(3));
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_CapturaBrixGv", new Dictionary<string, object>() { { "@idLider", HttpContext.Current.Session["idUsuario"] } });

            if (dt.Rows.Count == 0)
            {
                response += "<table id='gvBrix' class='gridView'><thead><tr><td>No hay ninguna captura de Brix</td></tr></thead>";
                //response += "<tbody><tr><td>No hay asociados relacionados para un nivel.</td></tr><tbody>";
            }
            else
            {
                response += "<table id='gvBrix' class='gridView'><thead><tr>"
                    + "<th>" + "Invernadero" + "</th>"
                    + "<th>" + "Libras" + "</th>"
                    + "<th>" + "Fecha Cosecha" + "</th>"
                    + "<th>" + "Fecha Captura" + "</th>"
                    + "<th>" + "Modificó" + "</th>"
                    + "</tr></thead><tbody>";
                foreach (DataRow item in dt.Rows)
                {
                    response += "<tr invernadero='" + item["ClaveInvernadero"] + "' fecha='" + item["FechaCaptura"] + "' onClick='showTabla(this, " + item["IdInvernadero"] + ", " + item["idActividadPrograma"] + ")'>"
                        + "<td>" + item["ClaveInvernadero"] + "</td>"
                        + "<td>" + item["Libras"] + "</td>"
                        + "<td>" + item["Fecha Cosecha"] + "</td>"
                        + "<td>" + item["FechaCaptura"] + "</td>"
                        + "<td><label title='" + item["FechaModifica"] + "'>" + item["UsuarioModifico"] + "</td>"
                        + "</tr>";
                }
                response += "</tbody></table>";
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
    public static string tablaSecciones(int idInvernadero, int idActividad)
    {
        var response = "";
        var libras = "0.00";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_SeccionesObtener", new Dictionary<string, object>() { { "@idInvernadero", idInvernadero }, { "@idActividad", idActividad } });
            DataTable secciones = ds.Tables[0];
            DataTable colores = ds.Tables[1];

            response += "<table id='tablaSecciones' class='gridView'><thead><tr><th>" + "Seccion" + "</th>";
            foreach (DataRow item2 in colores.Rows)
            {
                response += "<th>" + item2["NombreColor" + (getIdioma() == 1 ? "" : "_EN")] + "</th>";
            }
            response += "<th>" + "Promedio" + "</th>"
            + "<th>" + "Calidad" + "</th>"
            + "<th>" + "Color" + "</th>"
            + "<th>" + "..." + "</th></tr></thead><tbody>";

            foreach (DataRow item in secciones.Rows)
            {
                response += "<tr id='" + item["IdSeccion"] + "'>"
                + "<td>" + item["NombreSeccion"].ToString() + "</td>";


                foreach (DataRow item2 in colores.Rows)
                {
                    response += "<td style='text-align:center;'>"
                    + "<label style='display:none;'>" + item["Brix_" + item2["idColor"]] + "</label>"
                    + "<input class='requerid floatValidate brix focus " + (item["Brix_" + item2["idColor"]].ToString() == "0" ? "change" : "") + " " + item["IdSeccion"] + "' maxlength='5' style='text-align: center; width:40px;' id='brix-" + item["IdSeccion"] + "-" + item2["idColor"] + "' type='text' value='" + (item["Brix_" + item2["idColor"]].ToString() == "0" ? "0" : item["Brix_" + item2["idColor"]].ToString()) + "'>"
                    + "</td>";
                }
                response += "<td><label id='promedio-" + item["IdSeccion"] + "'>" + Convert.ToDecimal(item["Promedio"]).ToString("#.##") +"</label></td>"
                + "<td><label id='calidad-" + item["IdSeccion"] + "'>" + (item["Calidad"].ToString() == "" ? "---" : item["Calidad"]) + "</label></td>"
                + "<td><center>"
                + "<div id='color-" + item["IdSeccion"] + "' style='width: 16px; height: 16px; border:1px black solid; background-color: #" + (item["Color"].ToString() == "" ? "ffffff" : item["Color"]) + "'></div>"
                + "</center></td>"
                + "<td style='text-align:center;'>"
                + "<label id='coment-" + item["IdSeccion"] + "'>" + "" + "</label>"
                //+ "<input class='requerid floatValidate libras focus " + (item["Libras"].ToString() == "0" ? "change" : "") + " " + item["IdSeccion"] + "' maxlength='5' style='text-align: center; width:40px;' id='brix-" + item["IdSeccion"] + "' type='text' value='" + (item["Libras"].ToString() == "0" ? "0" : item["Libras"].ToString()) + "'>"
                + "</td>"
                + "</tr>";

                
                if (item["Libras"].ToString() != "0.00")
                {
                    libras = item["Libras"].ToString();
                }
            }
            response += "</tbody></table>"
                + "<table><tr>"
                + "<td style='text-align: right;'><label>" + "Libras: " + "</label></td>"
                + "<td style='width: 100px;'><input class='requerid floatValidate libras " + (libras == "0" ? "change" : "") + "' maxlength='5' style='text-align: center; width:40px;' id='libras' type='text' value='" + (libras == "0" ? "0" : libras) + "'>"
                + "</td></tr></table>";

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
    public static string[] guardaBrix(string ids, string red70, string red80, string mix, string calidad, decimal libras, int idInvernadero, int idactividad, int idBrix)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_BrixGuardar", new Dictionary<string, object>() { 
            { "@idioma", getIdioma() }, 
            { "@idInvernadero", idInvernadero }, 
            { "@ids", ids }, 
            { "@red70", red70}, 
            { "@red80", red80}, 
            { "@mix", mix}, 
            { "@calidad", calidad}, 
            { "@libras", libras}, 
            { "@idActividad", idactividad}, 
            { "@idBrix", idBrix /*HttpContext.Current.Session["idBrix"] == null ? "0": HttpContext.Current.Session["idBrix"].ToString()*/},
            { "@idLider", HttpContext.Current.Session["idLider"].ToString() },
            { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() } 
            });

            DataTable result = ds.Tables[0];
            DataTable result2 = ds.Tables[1];

            HttpContext.Current.Session["idBrix"] = result2.Rows[0]["idBrix"].ToString();
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