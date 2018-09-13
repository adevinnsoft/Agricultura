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
using log4net;

public partial class frmReporteEmbarques : Page
{
    //private static string currentFarm;
    //private static string sTargetURLForSessionTimeout;
    private static string plantas;
    private static string productos;
    private static readonly ILog Log = LogManager.GetLogger(typeof(BasePage));

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

            //plantas = getComboDestinos();
            //productos = getComboProductos();
		}
	}

	#region metodos

    //[WebMethod(EnableSession = true)]
    //public static string getComboDestinos()
    //{
    //    StringBuilder response = new StringBuilder();

    //    try
    //    {
    //        DataAccess dataaccess = new DataAccess();
    //        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_PlantasDestinoObtener", new Dictionary<string, object>() {});

    //        response.Append("<select class='chosen destino'>");
    //        //response.Append("<option value='0' selected>--Seleccione--</option>");
    //        foreach (DataRow item in dt.Rows)
    //        {
    //            response.Append("<option value='" + item["Codigo"].ToString().Trim() + "' planta='" + item["idPlanta"] + "'>" + item["Codigo"].ToString().Trim() + (item["NombrePlanta"].ToString() != "" ? " - " + item["NombrePlanta"] : "") + "</option>");
    //        }
    //        response.Append("</select>");
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex);
    //        response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
    //        return response.ToString();
    //    }

    //    return response.ToString();
    //}

    //[WebMethod(EnableSession = true)]
    //public static string getComboProductos()
    //{
    //    StringBuilder response = new StringBuilder();

    //    try
    //    {
    //        DataAccess dataaccess = new DataAccess();
    //        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ProductosObetener", new Dictionary<string, object>() {});

    //        response.Append("<select class='chosen producto'>");
    //        //response.Append("<option value='0' selected>--Seleccione--</option>");
    //        foreach (DataRow item in dt.Rows)
    //        {
    //            response.Append("<option value='" + item["Item"].ToString().Trim() + "'>" + item["Item"].ToString().Trim() + " - " + item["Producto"] + "</option>");
    //        }
    //        response.Append("</select>");
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex);
    //        response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
    //        return response.ToString();
    //    }

    //    return response.ToString();
    //}


    [WebMethod(EnableSession = true)]
    public static string obtineEmbarques()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_getReporteEmbarques", new Dictionary<string, object>() {  });//{ "@idLider", HttpContext.Current.Session["idUsuario"] }

            if (dt.Rows.Count == 0)
            {
                response += "<table id='tablaEmbarques' class='gridView'><thead><tr><td>No hay ninguna orden de Embarque</td></tr></thead>";
            }
            else
            {

                response += "<table id='tablaEmbarques' class='gridView'><thead><tr>"
                    + "<th>" + "Orden Embarque" + "</th>"
                     + "<th>" + "No Partida" + "</th>"
                   + "<th>" + "Temp Inicio" + "</th>"
                    + "<th>" + "F Carga" + "</th>"
                    + "<th>" + "F Carga Terminada" + "</th>"
                    + "<th>" + "F Salida" + "</th>"
                    + "<th>" + "F Final" + "</th>"
                    + "<th>" + "Destino" + "</th>"
                    + "<th>" + "Llegada" + "</th>"
                    + "<th>" + "Inicio Descarga" + "</th>"
                    + "<th>" + "Temp Inicio" + "</th>"
                    + "<th>" + "Fin Descargar" + "</th>"
                    + "<th>" + "Temp Fin" + "</th>"
                    + "<th>" + "Cajas Cargadas" + "</th>"
                    + "<th>" + "Cajas Descargadas" + "</th>"
                    + "<th>" + "Cajas no Descargadas" + "</th>"
                    + "</tr></thead><tbody>";

                foreach (DataRow item in dt.Rows)
                {
                    response += "<tr idEmbarque='" + item["orden"] + "' >"//onClick='gvPartidas(this);'
                        + "<td>" + item["orden"] + "</td>"
                        + "<td>" + item["Partida"] + "</td>"
                        + "<td>" + item["temperaturaInicio"] + "</td>"
                        + "<td>" + item["timestampCarga"] + "</td>"
                        + "<td>" + item["timestampCargaTerminada"] + "</td>"
                        + "<td>" + item["timestampSalida"] + "</td>"
                        + "<td>" + item["timestampFin"] + "</td>"
                        + "<td>" + item["destino"] + "</td>"
                        + "<td>" + item["timestampllegada"] + "</td>"
                        + "<td>" + item["timestampiniciodescarga"] + "</td>"
                        + "<td>" + item["temperaturainicio"] + "</td>"
                        + "<td>" + item["timestampfindescarga"] + "</td>"
                        + "<td>" + item["temperaturafin"] + "</td>"
                        + "<td>" + item["cargados"] + "</td>"
                        + "<td>" + item["descargados"] + "</td>"
                        + "<td>" + item["noDescargados"] + "</td>"
                        //+ "<td>" + DateTime.Parse(item["FechaOrigen"].ToString()).ToString("yyy-MM-dd HH:mm") + "</td>"

                        + "</tr>";
                }
                response += "</tbody></table>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='6'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    //[WebMethod(EnableSession = true)]
    //public static string obtienePartidas(string idEmbarque, string comentario)
    //{
    //    var response = "";

    //    try
    //    {
    //        DataAccess dataaccess = new DataAccess();
    //        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueDetalle", new Dictionary<string, object>() { { "@idLider",  HttpContext.Current.Session["idUsuario"]}, { "@idEmbarque", idEmbarque } });

    //        if (dt.Rows.Count == 0)
    //        {
    //            response += "<table id='tablaPartidas' class='gridView'><thead><tr><td>No hay partidas en este Embarque</td></tr></thead>";
    //        }
    //        else
    //        {
    //            int minLavel = Convert.ToInt32(dt.Compute("MAX(Orden)", string.Empty));

    //            StringBuilder orden = new StringBuilder();
    //            orden.Append("<select class='chosen orden' previo='@previo'>");
    //            for (int i = 1; i <= minLavel; i++ )
    //            {
    //                //if (!orden.ToString().Contains("value='" + i + "'"))
    //                //{
    //                    orden.Append("<option value='" + i + "'>" + i + "</option>");
    //                //}
    //            }
    //            orden.Append("</select>");

    //            response += "<table id='tablaPartidas' class='gridView'><thead><tr>"
    //                + "<th>" + "Partida" + "</th>"
    //                + "<th>" + "Fecha Destino" + "</th>"
    //                + "<th>" + "Destino" + "</th>"
    //                + "<th>" + "Producto" + "</th>"
    //                + "<th>" + "Cajas" + "</th>"
    //                + "<th>" + "Orden" + "</th>"
    //                + "<th>" + "Comentarios" + "</th>"
    //                + "</tr></thead><tbody>";

    //            foreach (DataRow item in dt.Rows)
    //            {
    //                response += "<tr idEmbarque='" + item["idEmbarque"] + "' idPartida='" + item["idPartida"] + "' idDestino='" + item["idDestino"].ToString().Trim() + "' idProducto='" + item["idProductoWeb"] + "' orden='" + item["Orden"] + "' fecha='" + DateTime.Parse(item["FechaDestino"].ToString()).ToString("yyy-MM-dd") + "' cajas='" + item["Cajas"] + "' onClick='/*cargaInformacion(this);*/'>"
    //                + "<td>" + item["idPartida"] + "</td>"
    //                + "<td><span style='display:none;'>" + DateTime.Parse(item["FechaDestino"].ToString()).ToString("yyy-MM-dd") + "</span><input class='datePicker' type='text' value='" + DateTime.Parse(item["FechaDestino"].ToString()).ToString("yyy-MM-dd") + "'/></td>"
    //                + "<td><span style='display:none;'>" + item["Destino"] + "</span>" + plantas + "</td>"
    //                + "<td><span style='display:none;'>" + item["Producto"] + "</span>" + productos + "</td>"
    //                + "<td><span style='display:none;'>" + item["Cajas"] + "</span><input class='cajas intValidate rerquired' type='text' value='" + item["Cajas"] + "'/></td>"
    //                + "<td><span style='display:none;'>" + item["Orden"] + "</span>" + orden.ToString().Replace("@previo", item["Orden"].ToString()) + "</td>"
    //                + "<td><textarea style='width:150px;' rows='3' cols='30' placeholder='" + item["Comentarios"].ToString().Replace("&nbsp;", "").Trim() + "' value='" + item["Comentarios"].ToString().Replace("&nbsp;", "").Trim() + "' class='comentarios'>" + "" + "</textarea></td>"
    //                + "</tr>";
    //            }
    //            response += "</tbody></table>";

    //            response += "<table><tbody>"
    //            + "<tr><td>" + "Comentarios Generales" + "</textarea></td></tr>"
    //            + "<tr><td><textarea style='width:100%;' id='comentario' rows='3' cols='30' placeholder='" + comentario + "' class='comentarios'>" + "" + "</textarea></td></tr>"
    //            + "</tbody></table>";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex);
    //        response = "<tr><td></td><td colspan='5'>" + ex.Message + "</td></tr>";
    //        return response;
    //    }

    //    return response;
    //}


    //[WebMethod(EnableSession = true)]
    //public static string[] guardaPartidasCambios(string idEmbarque, string comentario, string ids, string fechas, string destinos, string productos, string cajas, string ordenes, string comentarios)
    //{
    //    var response = new string[2];

    //    try
    //    {
    //        DataAccess dataaccess = new DataAccess();
    //        DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_PartidasCambiosGuardar", new Dictionary<string, object>() { 
    //        { "@idioma", getIdioma() }, 
    //        { "@idEmbarque", idEmbarque }, 
    //        { "@comentario", comentario }, 
    //        { "@ids", ids }, 
    //        { "@fechas", fechas}, 
    //        { "@destinos", destinos}, 
    //        { "@productos", productos}, 
    //        { "@cajas", cajas}, 
    //        { "@ordenes", ordenes}, 
    //        { "@comentarios", comentarios}, 
    //        { "@idLider", HttpContext.Current.Session["idLider"].ToString() },
    //        { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() } 
    //        });

    //        DataTable result = ds.Tables[0];

    //        response[0] = result.Rows[0]["msg"].ToString();
    //        response[1] = result.Rows[0]["detalle"].ToString();

    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex);
    //        response[0] = "error";
    //        response[1] = ex.Message;
    //        return response;
    //    }

    //    return response;
    //}

	#endregion

}