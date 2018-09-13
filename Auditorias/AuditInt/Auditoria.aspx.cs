using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_Auditoria : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            llenarGridViewAuditoria();
            loadPlantas();
            stDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            endDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

    }

    public void llenarGridViewAuditoria()
    {
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();

        parameters2.Add("@funcion", 1);
        parameters2.Add("@fechaIni", "");
        parameters2.Add("@fechaFin", "");
        parameters2.Add("@idPlanta", 0);
        parameters2.Add("@idUsuario", Convert.ToInt32(Session["idUsuario"].ToString()));

        DataTable dt5 = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerAuditoriasInt", parameters2);

        gvAuditInt.DataSource = dt5;
        gvAuditInt.DataBind();
    }
    public void loadPlantas()
    {
        //PLANTAS
        Dictionary<string, object> parametersP = new System.Collections.Generic.Dictionary<string, object>();
        //parametersP.Add("@ACTION", 2);
        DataTable dtPlantas = dsAuditoria.executeStoreProcedureDataTable("spr_get_plants", parametersP);
        CommonAudit.FillDropDownList(ref ddlPlanta, dtPlantas);

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        cleanFields();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (stDate.Text.Equals(""))
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Selecciona Fecha de Inicio');", true);
        else if (endDate.Text.Equals(""))
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Selecciona Fecha Final');", true);
        else if (ddlPlanta.SelectedIndex <= 0)
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Selecciona la Planta');", true);
        else
            filtroFechas();
    }



    public void filtroFechas()
    {
        try
        {
            Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();

            parameters2.Add("@funcion", 2);
            parameters2.Add("@fechaIni", stDate.Text);
            parameters2.Add("@fechaFin", endDate.Text);
            parameters2.Add("@idPlanta", ddlPlanta.SelectedValue);
            parameters2.Add("@idUsuario", Convert.ToInt32(Session["idUsuario"].ToString()));

            DataTable dt5 = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerAuditoriasInt", parameters2);

            gvAuditInt.DataSource = dt5;
            gvAuditInt.DataBind();


        }//try
        catch (Exception e) { }
    }

    public void cleanFields()
    {
        llenarGridViewAuditoria();
        stDate.Text = "";
        endDate.Text = "";
        ddlPlanta.SelectedIndex = 0;
        stDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        endDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

    protected void gvAuditInt_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAuditInt.PageIndex = e.NewPageIndex;
        llenarGridViewAuditoria();
    }

    protected void gvAuditInt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < (e.Row.Cells.Count - 1); i++)
        {
            e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("\\n", "<br/>");
        }
    }
}
