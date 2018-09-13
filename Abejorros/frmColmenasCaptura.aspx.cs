using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Globalization;
using System.Text;

public partial class frmColmenasCaptura : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            limpiacampos();
            txtDisponible.Text = hddDisponible.Value = stockColmenas();
            divGridView.InnerHtml = gvColmenas();
            //divComboInvernaderos.InnerHtml = comboInvernaderos();
        }
    }

    public void limpiacampos()
    {
        hddIdCriterio.Value = string.Empty;
        txtMtto.Text = string.Empty;
        txtSemanas.Text = string.Empty;
        txtNumero.Text = "1";
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }

    [WebMethod(EnableSession = true)]
    public static string[] comboInvernaderos()
    {
        var response = new String[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneInvernaderosAll", new Dictionary<string, object>() { /*{ "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) }*/ });

            response[0]  = "<select id='ddlInvernaderos' class='ddlInvernaderos'><option value='0' selected>--Seleccione--</option>";
            foreach (DataRow item in dt.Rows)
            {
                response[0] += "<option planta='" + item["idplanta"] + "' class='" + (HttpContext.Current.Session["idPlanta"].ToString() == item["idplanta"].ToString() ? "" : "invisible") + "' value='" + item["idInvernadero"] + "'>" + item["ClaveInvernadero"] + "</option>";
            }
            response[0] += "</select>";
            response[1] = HttpContext.Current.Session["idPlanta"].ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[0] = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string stockColmenas()
    {
        string val = "";
        try
        {
            DataAccess dataaccess = new DataAccess();
            val= dataaccess.executeStoreProcedureString("spr_ColmenasStockObtener", new Dictionary<string, object>() { { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) } });
            return val;
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            return "0";
        }

    }

    [WebMethod(EnableSession = true)]
    public static string[] guardaColmenas(string folios, string invernaderos, string acciones, string semanasentre, string mantenimientos)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_ColmenasGuardar", new Dictionary<string, object>() { 
              { "@folios", folios }
            , { "@invernaderos", invernaderos }
            , { "@semanas", semanasentre }
            , { "@mantenimientos", mantenimientos }
            , { "@acciones", acciones }
            , { "@idUser", HttpContext.Current.Session["idUsuario"].ToString() }
            , { "@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0 }
            , { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) }
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

    [WebMethod(EnableSession = true)]
    public static string gvColmenas()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ColmenasGv", new Dictionary<string, object>() { });

            if (dt.Rows.Count == 0)
            {
                response += "<table id='gvAsociados' class='gridView'><thead><tr><td>No hay colmenas registradas.</td></tr></thead>";
                //response += "<tbody><tr><td>No hay asociados relacionados para un nivel.</td></tr><tbody>";
            }
            else
            {
                
                response += "<table id='gvAsociados' class='gridView'><thead><tr><th>" + "Planta" + "</th><th>" + "Invernadero" + "</th><th>" + "Semana" + "</th><th>" + "Cantidad" + "</th></tr></thead><tbody>";
                foreach (DataRow item in dt.Rows)
                {
                    response += "<tr bloqueo='" + item["bloqueo"] + "' mantenimientos='" + item["Mantenimientos"] + "' semanas='" + item["SemanasEntreMantenimiento"] + "' onClick='showColmena(this, " + item["idColmenas"] + /*", " + item["idInvernadero"] + ", " + item["SemanaRecepcion"] +*/ ")'>"
                        + "<td>" + item["NombrePlanta"] + "</td>"
                        + "<td>" + item["ClaveInvernadero"] + "</td>"
                        + "<td>" + item["SemanaRecepcion"] + "</td>"                        
                        + "<td>" + item["Colmenas"] + "</td>"
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
    public static string[] cargaColmenas(int idColmenas)
    {
        var response = new String[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ColmenasGv", new Dictionary<string, object>() { { "@idColmenas", idColmenas } });

            if (dt.Rows.Count == 0)
            {
                response[0] += "<table id='gvAsociados' class='gridView'><thead><tr><td>No hay colmenas registradas.</td></tr></thead>";
                //response += "<tbody><tr><td>No hay asociados relacionados para un nivel.</td></tr><tbody>";
            }
            else
            {
                //response += "<table id='gvAsociados' class='gridView'><thead><tr><th>" + "Planta" + "</th><th>" + "Invernadero" + "</th><th>" + "Semana" + "</th><th>" + "Cantidad" + "</th></tr></thead><tbody>";
                int i = 0;
                foreach (DataRow item in dt.Rows)
                {
                    HttpContext.Current.Session["idPlanta"] = item["idPlanta"].ToString();
                    response[1] = item["idPlanta"].ToString();
                    i++;
                    response[0] += "<tr>"
                        + "<td class='alineacion'><label>folio " + i + ":</label></td>"
                        + "<td><input type='text' id='folio-" + i + "' maxlength='20' value='" + item["Folio"] + "' class='alphanumeric folios' /></td>"
                        + "<td class='alineacion'><label>Invernadero:</label></td>"
                        + "<td class='combo'><div id='invernadero-" + i + "' class='divComboInvernaderos' invernadero='" + item["idInvernadero"] + "'></div></td>"
                        + "</tr>";
                }
                response[0] += "</tbody></table>";
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