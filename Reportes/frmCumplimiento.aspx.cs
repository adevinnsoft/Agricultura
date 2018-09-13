using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Web.Script.Serialization;

public partial class Reportes_frmCumplimiento : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtieneRanchosPorUsuario(Session["idUsuario"].ToString());
            txtAnio.Text = DateTime.Now.Year.ToString();
        }
    }
    protected void btnConsulta_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@idPlanta", ddlRancho.SelectedValue);
        parameters.Add("@semanaIni", txtSemanaInicio.Text);
        parameters.Add("@semanaFin", txtSemanaFin.Text);
        parameters.Add("@Anio", txtAnio.Text);

        DataTable result = dataaccess.executeStoreProcedureDataTable("procObtienePlantasCumplimiento", parameters);
        
        grvRanchos.DataSource = result;
        grvRanchos.DataBind();

        gvCustomers.Visible = false;
        gvActividades.Visible = false;

    }
    public void ObtieneActividadesPorRancho(string idRancho)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@idPlanta", idRancho);
        parameters.Add("@semanaIni", txtSemanaInicio.Text);
        parameters.Add("@semanaFin", txtSemanaFin.Text);
        parameters.Add("@Anio", txtAnio.Text);

        DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneActividadesCumplimientoPorPlanta", parameters);
        gvActividades.DataSource = result;
        gvActividades.DataBind();
    }
    public void ObtieneInvernaderosPorRancho(string idRancho)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@idPlanta", idRancho);
        parameters.Add("@semanaIni", txtSemanaInicio.Text);
        parameters.Add("@semanaFin", txtSemanaFin.Text);
        parameters.Add("@Anio", txtAnio.Text);

        DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneInvernaderosCumplimiento", parameters);

        gvCustomers.DataSource = result;
        gvCustomers.DataBind();
    }
    public void ObtieneRanchosPorUsuario(string idUsuario)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idUsuario", idUsuario);

            ddlRancho.DataSource = dataaccess.executeStoreProcedureDataTableFill("spr_ObtenerPlantaPorUsuario", parameters);
            ddlRancho.DataTextField = "NombrePlanta";
            ddlRancho.DataValueField = "idPlanta";
            ddlRancho.DataBind();


            ddlRancho.Items.Insert(0, new ListItem("Todos", "0"));
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string idInvernadero = gvCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@idInvernadero", idInvernadero);
            parameters.Add("@semanaIni", txtSemanaInicio.Text);
            parameters.Add("@semanaFin", txtSemanaFin.Text);
            parameters.Add("@Anio", txtAnio.Text);

            DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneActividadesCumplimiento", parameters);



            gvOrders.DataSource = result;
            gvOrders.DataBind();

          
        }
    }
    protected void grvRanchos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != grvRanchos.SelectedPersistedDataKey)
            {
                Int32.TryParse(grvRanchos.SelectedPersistedDataKey["idPlanta"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(grvRanchos.SelectedDataKey["idPlanta"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            ObtieneActividadesPorRancho(hdnIdPlanta.Value);
            ObtieneInvernaderosPorRancho(hdnIdPlanta.Value);

            gvActividades.Visible = true;
            gvCustomers.Visible = true;

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoCargar").ToString(), Comun.MESSAGE_TYPE.Error);
        }
    }
    protected void OnRowDataBoundRanchos(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(grvRanchos, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void grvRanchos_PreRender(object sender, EventArgs e)
    {
        if (grvRanchos.HeaderRow != null)
            grvRanchos.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

   
}