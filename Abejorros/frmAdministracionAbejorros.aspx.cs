using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using log4net;
using System.Globalization;
using System.Configuration;

public partial class frmAdministracionAbejorros : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmAdministracionAbejorros));
    private static string sTargetURLForSessionTimeout;
    private static string[] tagMantto = {"Pasado","Acual","Proximo" };
    private static frmAdministracionAbejorros basePage;
    public static string incidencias = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();
        incidencias = comboIncidencias();
    }

    [WebMethod]
    public static string comboIncidencias()
    {
        var result = "";
        try
        {
            var dt = new DataTable();
            dt = new DataAccess().executeStoreProcedureDataTable("spr_IncidenciasAbejorroObtener", new Dictionary<string, object>() { { "@idioma", getIdioma() } });

            result += "<select onchange='editarFolio(this);'  class='incidencias'>" +
                            "<option value='0' editar='False'>--Seleccione--</option>";
            foreach (DataRow row in dt.Rows)
            {
                result += "<option value='" + row["idIncidencia"] + "' editar='" + row["Tipo"] + "'>" + row["Nombre"] + "</option>";
            }
            result += "</select>";
            return result;
        }
        catch (Exception es)
        {
            log.Error(es);
        }
        return result;
    }


    [WebMethod]
    public static string cargaInvernaderosSlider()
     {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);
        var result = "";
        try
        {
            var dt = new DataTable();
            dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtieneInvernaderosDdl", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) } });
            //dt = new DataAccess().executeStoreProcedureDataTable("spr_InvernaderosParaProgramacionSemanal", new Dictionary<string, object>() { 
                //{ "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, 
                //{ "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } 
            //});

            foreach (DataRow inv in dt.Rows)
            {
                result = result + "<div class='divInvernadero' id='" + inv["idInvernadero"] + "' >" + inv["ClaveInvernadero"] + "<span class='avance'></span></div>";
                //result = result + "<div class=\"divInvernadero\" id='" + inv["idInvernadero"] + "' product='" + inv["Product"] + "(" + inv["Variety"] + ")' fechaPlantacion='" + inv["PlantDate"] + "' semana=\"" + (inv["Week"].ToString().Substring(5, 2)) + "\" densidad='" + inv["Density"] + "' terminado='" + inv["complete"] + "' surcos='" + inv["surcos"] + "' >" + inv["ClaveInvernadero"] + "<span class='avance'></span></div>";
            }

        }
        catch (Exception es)
        {
            log.Error(es);
        }
        return result;
    }

    [WebMethod]
    public static string cargaColmenasPorInvernadero(int idInvernadero)
    {
        var dt = new DataTable();
        //var dtNivel = new DataTable();
        string result = "";
        try
        {
            //int lang = HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0;
            //dtNivel = new DataAccess().executeStoreProcedureDataTable("spr_ObtieneNivelPolinizacion", new Dictionary<string, object>() {{"@lengua", lang }});

            dt = new DataAccess().executeStoreProcedureDataTable("spr_ColmenasPorInvernadero", new Dictionary<string, object>() { 
                { "@idInvernadero", idInvernadero.ToString() } 
            });

            result += "<div style='display: table; width: 100%;'><table class='solicitud'><tr><td>Solicitar: *</td>"
                                + "<td><input invernadero='" + idInvernadero + "' id='cantidad-" + idInvernadero + "' width='' maxlength='4' class='required intValidate' type='text'style='width:80px;'/></td>"
                                + "<td> colmenas, a: </td>"
                                + "<td><input invernadero='" + idInvernadero + "' id='cuenta-" + idInvernadero + "' class='required' type='text' onchange='activeDirectory($(this));'/></td>"
                                + "<td> *semana: </td>"
                                + "<td><input invernadero='" + idInvernadero + "' id='semana-" + idInvernadero + "' type='text' maxlength='2' class='required intValidate' style='width:35px;'/></td>"
            + "<td><input invernadero='" + idInvernadero + "' type='button' value='Solicitar' onclick='solicitarColmenas($(this));' /></td></tr>"
            + "<tr><td colspan='3'/><td><span id='mail-" + idInvernadero + "'/></td><td/>"
            + "</table></div>";


            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {

                result += "                <div id='pager-" + idInvernadero + "' class='pager'>" +
                            "                    <img alt='first' src='../comun/img/first.png' class='first' />" +
                            "                    <img alt='prev' src='../comun/img/prev.png' class='prev' />" +
                            "                    <span class='pagedisplay'></span>" +
                            "                    <img alt='next' src='../comun/img/next.png' class='next' />" +
                            "                    <img alt='last' src='../comun/img/last.png' class='last' />" +
                            "                    <select class='pagesize cajaCh' style='width: 50px; min-width: 50px; max-width: 50px;'>" +
                            "                        <option value='10'>10</option>" +
                            "                        <option value='20'>20</option>" +
                            "                        <option value='30'>30</option>" +
                            "                        <option value='40'>40</option>" +
                            "                        <option value='50'>50</option>" +
                            "                    </select>" +
                            "                </div>" +
                            "            </div>";

                result += "<table id='tablaColmenas-" + idInvernadero + "' class='gridView colmenas'><thead><tr>";
                foreach (DataColumn item in dt.Columns)
                {
                    if (item.ColumnName.Contains("|"))
                    {
                        result += "<th class='wrap'>" + (getIdioma() == 1 ? item.ColumnName.Split('|')[0] : item.ColumnName.Split('|')[1]) + "</th>";
                    }
                }

                //result += "<th>" + "Accion" + "</th>";
                //result += "<th>" + "Comentarios" + "</th>";

                result += "</thead></tr><tbody>";
                foreach (DataRow row in dt.Rows)
                {
                    //int mantto = 0;
                    //Int32.TryParse(row["Mantenimiento"].ToString(), out mantto);
                    result += "<tr idInvernadero='" + idInvernadero + "' idColmenas='" + row["idColmenasDetalle"] + "'>"
                    + "    <td class='semana' value='" + row[2] + "'>" + row[2] + "</td>"
                    + "    <td class='fehaE wrap'>" + row[4] + "</td>"
                    + "    <td class='fehcaS wrap'>" + row[5] + "</td>"
                    + "    <td class='fechaV wrap'>" + String.Format("{0:d}", row[6]) + "</td>"
                    //+ "    <td>" + row[7] + "</td>"
                    + "    <td>"
                    + "<input class='requerid alphanumeric folioRead folios folio-" + idInvernadero + "' idColmenasDetalle='" + row["idColmenasDetalle"] + "' vprev='" + row[7] + "' readonly value='" + row[7] + "' style='/*width:35px;*/' maxlength='20'/>"
                    + "<span style='display:none;'>" + row[7] + "</span></td>"
                    + "<td idInvernadero='" + idInvernadero + "'>" + (row[5].ToString() == "" ? incidencias : "<select disabled><option  value='0'>--Seleccione--</option></select>") + "</td>"
                    + "<td><textarea id='comentario-" + idInvernadero + "' style='width:90%;' rows='3' cols='30' vprev='" + row[9].ToString().Replace("&nbsp;", "").Trim() + "' value='" + row[9].ToString().Replace("&nbsp;", "").Trim() + "' class='comentarios' " + (row[5].ToString() != "" ? "disabled" : "") + ">" + row[9].ToString().Replace("&nbsp;", "").Trim() + "</textarea></td>"
                    + "</tr>";
                }
                result += "</tbody></table>";

                result += "<table><tr><td><input type='button' value='Guardar' onclick='saveFolios(" + idInvernadero + ");'/></td></tr></table>";

                return result;
            }
            else
            {
                result += "<div><table class='index'><tr><td>No hay colmenas en este Invernadero</td></tr></table></div>";
            }
        }
        catch (Exception e)
        {
            log.Error(e.Message);
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public static string[] guardaColmenas(string colmenas, string folios, string acciones, string comentarios)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_ColmenaIncidencias", new Dictionary<string, object>() { 
              { "@colmenas", colmenas }
            , { "@folios", folios }
            , { "@acciones", acciones }
            , { "@comentarios", comentarios }
            , { "@idUser", HttpContext.Current.Session["idUsuario"].ToString() }
            , { "@idioma", getIdioma() }
            });

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



    [WebMethod]
    public static string activeDirectory(string cuenta)
    {
        string result = "";
        String exception = String.Empty;
        try
        {
            DataTable dt = DGActiveDirectory.getGeneralInfo(DGActiveDirectory.setDomainVariablesAndGetSearchResult("GDL|USA", cuenta, ref exception), cuenta);

            if (dt !=null)
            {
                result = dt.Rows[0]["Email"].ToString();
            }

            return result;
        }
        catch (Exception x)
        {
            log.Error(x);
            return result;
        }
    }

    [WebMethod]
    public static string[] solicitarColmenas(int idInvernadero, int cantidad, int semana, string mail)
    {
        try
        {
            if (ConfigurationManager.AppSettings["URLAutorizacionColmena"] == null)
                return new string[] { "0", "error", "No se ha configurado la Key URLAutorizacionColmenas." };
            else
            {
                if (mail != "")
                {
                    DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_ColmenasSolicitar",
                            new Dictionary<string, object>() { 
                        { "@idInvernadero", idInvernadero },
                        { "@semana", semana },
                        { "@cantidad", cantidad },
                        { "@mail", mail },
                        { "@idUsuario",  HttpContext.Current.Session["idUsuario"].ToString() },
                        { "@URL", ConfigurationManager.AppSettings["URLAutorizacionColmena"].ToString() }
                    });
                    if (dt.Rows.Count > 0)
                    {
                        return new string[] { "1", "ok", "Solicitud registrada. Se enviará un correo a tu jefe directo para que autorice." };
                    }
                    else
                    {
                        return new string[] { "0", "error", "Error en el registro de la solicitud." };
                    }
                }
                else
                {
                    return new string[] { "0", "error", "no se pudo obtener el correo de esta cuenta." };
                }
            }
        }
        catch (Exception x)
        {
            log.Error(x);
            return new string[] { "0", "error", "Error en el la obtención de datos." };
        }
    }


    [WebMethod(EnableSession = true)]
    public static string exiteFolio(string folio)
    {
        var response = "0";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ColmenasGuardar", new Dictionary<string, object>() { { "@idColmena", folio } });

            response = dt.Rows[0]["existe"].ToString();

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            return "0";
        }

        return response;
    }
}
