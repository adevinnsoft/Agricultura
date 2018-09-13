using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using Incidencias;


/*using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.IO;*/



public partial class frmPrestamoAsociados : BasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			if(Session["usernameInj"] == null)
				Response.Redirect("~/frmLogin.aspx", false);
		}
	}

	#region metodos


    [WebMethod(EnableSession = true)]
    public static string gvRelacionAsociados()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_RelacionAsociadosGv", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@idLider", HttpContext.Current.Session["idLider"] } });

            if (asociados.Rows.Count == 0)
            {
                response += "<table id='gvAsociados' class='gridView'><thead><tr><td>No hay asociados relacionados para un nivel.</td></tr></thead>";
                //response += "<tbody><tr><td>No hay asociados relacionados para un nivel.</td></tr><tbody>";
            }
            else
            {
                response += "<table id='gvAsociados' class='gridView'><thead><tr><th>" + "Familia" + "</th><th>" + "Nivel" + "</th><th>" + "Cantidad" + "</th></tr></thead><tbody>";
                foreach (DataRow item in asociados.Rows)
                {
                    response += "<tr onClick='showGrupo(" + item["idFamilia"] + "," + item["idNivel"] + ")'>"
                        + "<td>" + item["Familia"] + "</td>"
                        + "<td>" + item["Nivel"] + "</td>"
                        + "<td>" + item["cantidad"] + "</td>"
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
    public static string tablaAsociadosLibres()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_getAsociadosLiberados", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"] }});

            response += "<table id='tablaAsociadosLibres' class='gridView'><thead><tr><th>" + "Nombre" + "</th><th>" + "Familia/Nivel" + "</th><th>" + "Lider" + "</th></tr></thead><tbody>";
            foreach (DataRow item in asociados.Rows)
            {
                response += "<tr>"
                    + "<td class='left'>"
                    + "<input class='check-with-label asociadosTomar' type='checkbox' id='" + item["idAsociado"] + "' value='" + item["idAsociado"] + "|" + item["Nombre"] + "' name='asociados' />" //" + (item["Grupo"].ToString() != "---" ? "disabled='disabled'": "") + "
                    + "<label class='label-for-check' for='" + item["idAsociado"] + "'><span></span>" + item["idAsociado"] + " - " + item["Nombre"] + "</label>"
                    + "</td>"
                    + "<td id='label" + item["idAsociado"] + "'>" + item["Grupo"] + "</td>"
                    + "<td id='label" + item["id_lider"] + "'>" + item["nombre_lider"] + "</td>"
                    + "</tr>";
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
    public static string tablaAsociadosLider()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_AsociadosObtener", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() }, { "@idLider",  HttpContext.Current.Session["idLider"].ToString() } });

            response += "<table id='tablaAsociadosLider' class='gridView'><thead><tr><th>" + "Nombre" + "</th><th>" + "Familia/Nivel" + "</th></tr></thead><tbody>";//<th>" + "Prestado" + "</th>
            foreach (DataRow item in asociados.Rows)
            {
                //if (item["Grupo"].ToString() != "---")
                //{
                    response += "<tr estado='" + (item["idLider"].ToString() != HttpContext.Current.Session["idLider"].ToString() ? 2 : 1) + "'>"
                        + "<td class='left'>"
                        + "<input class='check-with-label asociadosPrestar' type='checkbox' id='" + item["IDEmployee"] + "' value='" + item["IDEmployee"] + "|" + item["FullName"] + "' name='asociados' />" //" + (item["Grupo"].ToString() != "---" ? "disabled='disabled'": "") + "
                        + "<label class='label-for-check' for='" + item["IDEmployee"] + "'><span></span>" + item["IDEmployee"] + " - " + item["FullName"] + (item["idLider"].ToString() != HttpContext.Current.Session["idLider"].ToString() ? "<img src='../comun/img/strar.png' style='left:10px;' class='help' title='" + item["Lider"] + "' alt='prestado' width='15px' style='display:inline;'>" : "") + "</label>"
                        + "</td>"
                        + "<td id='label" + item["IDEmployee"] + "'>" + item["Grupo"] + "</td>"
                        //+ "<td>" + (item["idLider"].ToString() != HttpContext.Current.Session["idLider"].ToString() ? "Si" : "No") + "</td>"
                        + "</tr>";
                //}
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
    public static string[] guardaRelacion(string idasociados, bool prestar, string idasociadosgrupo, string nivel)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_AsociadosPrestarTomar", new Dictionary<string, object>() { 
                { "@idAsociados", idasociados }, 
                { "@idasociadosgrupo", idasociadosgrupo }, 
                { "@idusuario", HttpContext.Current.Session["idUsuario"]}, 
                { "@idLider", HttpContext.Current.Session["idLider"]}, 
                { "@nivel", nivel}, 
                { "@prestar", prestar } ,
                { "@idioma", getIdioma() } 
            });

            response[0] = result.Rows[0]["estado"].ToString();
            response[1] = result.Rows[0]["msg"].ToString();
            
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


    [WebMethod(EnableSession = true)]
    public static string dibujaRadiosFamilias()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable familias = dataaccess.executeStoreProcedureDataTable("spr_FamiliasObtiene", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name } });

            response += "<table><tr>";
            foreach (DataRow item in familias.Rows)
            {
                response += "<td style='display:" + (item["bActivo"].ToString() == "True" ? "inline-block" : "none") + "; height:25px;'><input class='check-with-label selFamilia' type='radio' id='F" + item["IdFamilia"] + "' value='" + item["IdFamilia"] + "|" + item["Nombre"] + "' " + (item["bActivo"].ToString() == "True" ? "" : "disabled") + " name='familias'/>"
                    + "<label class='label-for-check' for='F" + item["IdFamilia"] + "'><span></span>" + item["Nombre"] + "</label></td>";
            }
            response += "</tr></table>";

            //Incidencias.IncidenciasWS ws = new Incidencias.IncidenciasWS();
            //Asociados[] query = ws.swAsociados("idPlanta", 0, 0, 0, 0, 0, 0, true);

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
    public static string dibujaRadiosNiveles(int idFamilia)
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable familias = dataaccess.executeStoreProcedureDataTable("spr_NivelesObtiene", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@idFamilia", idFamilia } });

            foreach (DataRow item in familias.Rows)
            {
                response += "<td style='display:" + (item["bActivo"].ToString() == "True" ? "inline-block" : "none") + "; height:25px;'><input class='check-with-label selNivel' type='radio' id='N" + item["Nivel"] + "' value='" + item["idNivel"] + "' " + (item["bActivo"].ToString() == "True" ? "" : "disabled") + " name='niveles'/>"
                    + "<label class='label-for-check' for='N" + item["Nivel"] + "'><span></span>" + item["Nivel"] + "</label></td>";
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
    public static string[] addRows(int idFamilia, int idNivel)
    {
        var response = new string[2];
        response[0] = "" + idFamilia + "" + idNivel;

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_RelacionAsociadosObtener", new Dictionary<string, object>() { { "@idFamilia", idFamilia }, { "@idNivel", idNivel }, { "@idLider", HttpContext.Current.Session["idLider"]} });

            foreach (DataRow item in asociados.Rows)
            {
                response[1] += "<tr><td><div class='imgGuardado' id='img" + item["IDEmployee"] + "' title='Este asociado ya esta guardado.'/></td><td>" + item["IDEmployee"] + " - " + item["FullName"] + "</td><td  style='text-align:center;'><img src='../comun/img/remove-icon.png' alt='eliminar' title='Eliminar' width='20' height='20' onClick='regresarAsociado(\"" + item["IDEmployee"] + "\", this);' /></td></tr>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[1] = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string[] borrarAsociado(string idAsociado)
    {
        var response = new string[3];
        response[2] = idAsociado;
        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_RelacionAsociadosEliminar", new Dictionary<string, object>() { { "@idAsociado", idAsociado } });

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