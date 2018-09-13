using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.Services;

public partial class configuracion_frmPlagaIntervalos : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            llenaTabla();
        }
    }

    public void llenaTabla()
    {
        try
        {
            Session["minimo"] = 1;
            Session["maximo"] = 10;
            divGridView.InnerHtml = tablaInfestaciones();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
        }

    }

    protected void  btnSave_Click(object sender, EventArgs e)
    {
        DataTable result;
        try
        {
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("@lengua", CultureInfo.CurrentCulture.Name);
            parameters.Add("@UsuarioModifico", Session["idUsuario"].ToString());

            result = dataaccess.executeStoreProcedureDataTable("spr_EstadoInfestacionGuardar", parameters);

            switch (result.Rows[0]["msg"].ToString())
            {
                case "ok":
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Success);
                    break;
                case "info":
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Info);
                    break;
                default:
                    popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "NoGuardado").ToString() + ": " + result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Error);
                    break;
            }
            limpiacampos();
            llenaTabla();
            
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }
    }

    public void limpiacampos()
    {
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }


    [WebMethod(EnableSession = true)]
    public static string tablaInfestaciones()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_InfestacioIntervalosGv", new Dictionary<string, object>() { });

            response += "<table id='tablaInfestaciones' class='gridView'><thead><tr>"
                     + "<th>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","Tipo") + "</th>"
                     + "<th style='width:150px;'>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","NombreComun") + "</th>"
                     + "<th style='width:150px;'>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","NombreCientifico") + "</th>"
                  //   + "<th>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","Modifico") + "</th>"
                     + "<th>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","Activo") + "</th>"
                     + "<th>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","Minimo") + "</th>"
                     + "<th>" +  HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx","Maximo") + "</th>"
                     + "</tr></thead><tbody>";
            foreach (DataRow item in asociados.Rows)
            {
                response += "<tr>"
                    + "<td>"
                    + (Convert.ToBoolean(item["esPlaga"].ToString()) == true ? HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx", "Plaga")/* "Plaga"*/ : HttpContext.GetLocalResourceObject("~/configuracion/frmPlagaIntervalos.aspx", "Enfermedad")/* "Enfermedad"*/) 
                    + "</td>"
                    + "<td>" + item["nombreComun"].ToString() + "</td>"
                    + "<td>" + item["nombreCientifico"].ToString() + "</td>"
                  //  + "<td><label class='tooltip' " + (item["UsuarioModifica"].ToString() == "0" ? "---":("title='" + item["FechaModifico"] + "'>" + item["UsuarioModifica"]) ) + "</label></td>"
                    + "<td>" 
                    + (Convert.ToBoolean(item["Activo"].ToString()) == true ? HttpContext.GetGlobalResourceObject("Commun", "Si") : HttpContext.GetGlobalResourceObject("Commun", "No")) 
                    + "</td>"
                    + "<td>"
                    + "<label style='display:none;'>" + item["minimo"] + "</label>"
                    + "<input class='requerid int32 minimo focus " + (item["minimo"].ToString() == "0" && item["maximo"].ToString() == "0" ? "change" : "") + "' maxlength='5' style='text-align: center; width:40px;' id='min-" + item["idPlaga"] + "' type='text' value='" + (item["minimo"].ToString() == "0" && item["maximo"].ToString() == "0" ? HttpContext.Current.Session["minimo"].ToString() : item["minimo"].ToString()) + "'></td>"
                    + "<td>"
                    + "<label style='display:none;'>" + item["maximo"] + "</label>"
                    + "<input class='requerid int32 maximo focus " + (item["minimo"].ToString() == "0" && item["maximo"].ToString() == "0" ? "change" : "") + "' maxlength='5' style='text-align: center; width:40px;' id='max-" + item["idPlaga"] + "' type='text' value='" + (item["minimo"].ToString() == "0" && item["maximo"].ToString() == "0" ? HttpContext.Current.Session["maximo"].ToString() : item["maximo"].ToString()) + "'></td>"
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
    public static string[] guardaIntervalos(string ids, string minimos, string maximos)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_PlagaIntervalosGuardar", new Dictionary<string, object>() { { "@ids", ids }, { "@minimos", minimos }, { "@maximos", maximos }, { "@idUser", HttpContext.Current.Session["idUsuario"].ToString() }, { "@lengua", CultureInfo.CurrentCulture.Name } });

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

}