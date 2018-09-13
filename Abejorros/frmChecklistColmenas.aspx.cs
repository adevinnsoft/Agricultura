using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.Services;
using System.Text;

public partial class frmChecklistColmenas : BasePage
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
            int semanaNS = dataaccess.executeStoreProcedureGetInt("spr_ObtieneSemanaNS", new Dictionary<string, object>() { { "@fecha", DateTime.Now.ToString("yyyy-MM-dd") } });
            hddSemanaNS.Value = semanaNS.ToString();
            for (int a = 1; a <= semanaNS; a++)
            {
                ddlSemana.Items.Insert(a - 1, new ListItem(a.ToString(), "" + a));
            }
            ddlSemana.DataBind();
            ddlSemana.SelectedIndex = semanaNS - 1;
            //var array = new string[2];
            //array = tablaChecklist(semanaNS, 0);
            //divGridView.InnerHtml = array[0];
            //hddIdCheck.Value = array[1];

            divComboInvernaderos.InnerHtml = comboInvernaderos();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
        }

    }

    public void llenaCombo()
    {
        try
        {
            int semanaNS = dataaccess.executeStoreProcedureGetInt("spr_ObtieneSemanaNS", new Dictionary<string, object>() { { "@fecha", DateTime.Now } });
            //DataTable años = lTiempo.Tables[0];
            //DataTable linea = lTiempo.Tables[1];


            /*ddlAño.DataSource = años;
            ddlAño.DataTextField = "year";
            ddlAño.DataValueField = "year";

            ddlAño.DataBind();
            //ddlAño.Items.Insert(0, new ListItem(GetGlobalResourceObject("Commun", "Select").ToString(), ""));*/
            //sacar año actual

            for (int a = 1; a <= semanaNS; a++)
            {
                ddlSemana.Items.Insert(a - 1, new ListItem(a.ToString(), "" + a));
            }
            /*ddlAño.Items.Insert(0, new ListItem(anio.Year.ToString(), "" + anio.Year));
            ddlAño.Items.Insert(1, new ListItem((anio.Year + 1).ToString(), "" + (anio.Year + 1)));
            ddlAño.Items.Insert(2, new ListItem((anio.Year + 2).ToString(), "" + (anio.Year +2)));*/
            ddlSemana.DataBind();
            ddlSemana.SelectedIndex = semanaNS - 1;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            //popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
        }

    }

    [WebMethod(EnableSession = true)]
    public static string comboInvernaderos()
    {
        StringBuilder response = new StringBuilder();

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneInvernaderosDdl", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) } });

            response.Append("<select id='ddlInvernaderos' class='ddlInv'><option value='0' selected>--Seleccione--</option>");
            foreach (DataRow item in dt.Rows)
            {
                response.Append("<option value='" + item[0] + "'>" + item[1] + "</option>");
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
    public static string[] tablaChecklist(int semanaNS, int idinvernadero)
    {
        DataAccess dataaccess = new DataAccess();
        var response = new string [4];

        //int semanaNS = dataaccess.executeStoreProcedureGetInt("spr_ObtieneSemanaNS", new Dictionary<string, object>() { { "@fecha", DateTime.Now } });

        try
        {

            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ChecklistColmenasGv", new Dictionary<string, object>() { { "@semana", semanaNS }, { "@idinvernadero", idinvernadero }, { "@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0 } });
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            if (dt2.Rows.Count > 0)
            {
                response[1] = dt2.Rows[0]["idCheckList"].ToString();
                response[2] = dt2.Rows[0]["Observacion"].ToString();
                response[3] = dt2.Rows[0]["Observacion_EN"].ToString();
            }
            else
            {
                response[1] = "0";
            }

            response[0] += "<table id='tablaCheckList' class='gridView'><thead><tr>"
                     + "<th>" + "Criterio" + "</th>"
                     + "<th style='width:150px;'>" + "Cumple" + "</th>"
                 //    + "<th>" + "Modifico" + "</th>"
                     + "</tr></thead><tbody>";
            foreach (DataRow item in dt.Rows)
            {
                response[0] += "<tr>"
                    + "<td style='text-align:left;'><label class='tooltip' " + (item["nDescripcion"].ToString() != "" ? "title='" + item["nDescripcion"].ToString() + "'" : "" ) + ">" + item["nCriterio"].ToString() + "</label></td>"
                    + "<td>" 
                    
                    + "<div class='onoffswitch'>" +
                    "<input type='checkbox' name='onoffswitch' class='onoffswitch-checkbox' id='chk-" + item["idCriterio"] + "' " + (Convert.ToBoolean(item["Cumple"].ToString()) ? "checked" : "") + " >" +
                    "<label class='onoffswitch-label' for='chk-" + item["idCriterio"] + "'>" +
                    "<span class='onoffswitch-inner'></span>" +
                    "<span class='onoffswitch-switch'></span>" +
                    "</label>" +
                    "</div>"
                    
              //      + "</td>"
              //      + "<td><label class='tooltip' " + (item["UsuarioModifica"].ToString() == "0" ? "---":("title='" + item["FechaModifico"] + "'>" + item["UsuarioModifica"]) ) + "</label></td>"
                    + "</tr>";
            }
            response[0] += "</tbody></table>";

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
    public static string[] guardaCheckList(string ids, string valores, int semana, int idInvernadero, int idCheck, string observacion, string observacion_EN)
    {
        var response = new string[3];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataSet result = dataaccess.executeStoreProcedureDataSet("spr_ColmenasCheckListGuardar", new Dictionary<string, object>() { 
              { "@ids", ids }
            , { "@valores", valores }
            , { "@idCheck", idCheck }
            , { "@semana", semana }
            , { "@idinvernadero", idInvernadero }
            , { "@observacion", observacion }
            , { "@observacion_EN", observacion_EN }
            , { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }
            , { "@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0 }
            });

            response[0] = result.Tables[0].Rows[0]["msg"].ToString();
            response[1] = result.Tables[0].Rows[0]["detalle"].ToString();
            response[2] = result.Tables[1].Rows[0]["idCheck"].ToString();

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