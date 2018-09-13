using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditExt_Materiales : System.Web.UI.Page
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarMateriales();
            LoadPlants();
            loadEncuestas();
        }


    }

    public void CargarMateriales()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 2);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idMaterial", 0);
        parameters.Add("@idPlanta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerMateriales", parameters);
        gvCriteries.DataSource = dt;
        gvCriteries.DataBind();
    }

    protected void gvCriteries_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.className = 'gridViewOver';";
            if ((e.Row.RowIndex % 2) == 1)
                e.Row.Attributes["onmouseout"] = "this.className = 'gridViewAlt';";
            else
                e.Row.Attributes["onmouseout"] = "this.className = 'gridView';";
            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvCriteries_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCriteries.PageIndex = e.NewPageIndex;
        CargarMateriales();
    }
    public void LoadPlants()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_get_plants", parameters);
        CommonAudit.FillDropDownList(ref DropDownList1, dt);
        CommonAudit.FillDropDownList(ref ddlPlants, dt);
    }

    public void loadEncuestas()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 3);
        parameters.Add("@module", 8);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerEncuestas", parameters);
        CommonAudit.FillDropDownList(ref ddlEncuenta, dt);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clean();
    }
    //

    public void clean()
    {
        txtMaterialEn.Text = "";
        txtMaterialEs.Text = "";
        checkActive.Checked = true;
        ddlEncuenta.SelectedIndex = -1;
        ddlPlants.SelectedIndex = -1;
        DropDownList1.SelectedIndex = -1;
        btnSaveCritery.Text = "Save";
        lblIDMaterial.Text = "0";
        CargarMateriales();
    }

    protected void btnSaveCritery_Click(object sender, EventArgs e)
    {
        string striEs = txtMaterialEn.Text.Trim();
        string striEn = txtMaterialEs.Text.Trim();

        if (ddlEncuenta.SelectedIndex != 0 && ddlPlants.SelectedIndex != 0 && !String.IsNullOrEmpty(striEs) && !String.IsNullOrEmpty(striEn))
            SaveMaterial();
        else
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Incorrec data');", true);

    }

    protected void gvCriteries_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@accion", 3);
            parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
            parameters.Add("@idMaterial", gvCriteries.SelectedRow.Cells[0].Text);
            parameters.Add("@idPlanta", 0);
            //parameters.Add("@idEncuesta", 0);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerMateriales", parameters);


            lblIDMaterial.Text = gvCriteries.SelectedRow.Cells[0].Text;
            btnSaveCritery.Text = "Update";

            if (dt.Rows.Count > 0)
            {
                ddlPlants.SelectedIndex = ddlPlants.Items.IndexOf(ddlPlants.Items.FindByValue(dt.Rows[0]["PLANTA"].ToString()));
                ddlEncuenta.SelectedIndex = ddlEncuenta.Items.IndexOf(ddlEncuenta.Items.FindByText(dt.Rows[0]["ENCUESTA"].ToString()));

                txtMaterialEs.Text = dt.Rows[0]["MATERIALES"].ToString().Trim();
                txtMaterialEn.Text = dt.Rows[0]["MATERIALEN"].ToString().Trim();
                checkActive.Checked = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["ACTIVO"].ToString()));
            }
            else
            {
                //No se encontró el registro
            }
        }
        catch (Exception ex)
        {
            //ctrlPopUpMessage1.setMessage(ex.Message, MESSAGE_TYPE.Error);
        }
    }

    public void SaveMaterial()
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idMaterial", Convert.ToInt32(lblIDMaterial.Text));
            parameters.Add("@idPlanta", ddlPlants.SelectedValue.ToString());
            parameters.Add("@idRubro", ddlEncuenta.SelectedValue.ToString());
            parameters.Add("@MaterialEs", txtMaterialEs.Text.Trim());
            parameters.Add("@MaterialEn", txtMaterialEn.Text.Trim());
            parameters.Add("@User", Session["Nombre"].ToString());
            parameters.Add("@Estado", checkActive.Checked);
            /* Ricardo Ramos : Si se almacena pero no se obtiene respuesta del procedimiento */
            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Materials", parameters);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('save successfully');", true);

            clean();
            CargarMateriales();
        }
        catch (Exception ex)
        {
            //  ctrlPopUpMessage1.setMessage(ex.Message, MESSAGE_TYPE.Error);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idMaterial", 0);

        if (DropDownList1.SelectedIndex <= 0)
        {
            parameters.Add("@accion", 2);
            parameters.Add("@idPlanta", 0);
        }
        else
        {
            /*Esta acción no esta declarada en el procedimento que se llama */
            parameters.Add("@accion", 4);
            parameters.Add("@idPlanta", DropDownList1.SelectedValue);
        }


        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerMateriales", parameters);
        gvCriteries.DataSource = dt;
        gvCriteries.DataBind();
    }

    protected void ddlPlants_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idMaterial", 0);
        parameters.Add("@accion", 5);
        if (ddlPlants.SelectedIndex <= 0)
        {
            parameters.Add("@idPlanta", 0);
        }
        else
            parameters.Add("@idPlanta", ddlPlants.SelectedValue);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerMateriales", parameters);
        CommonAudit.FillDropDownList(ref ddlEncuenta, dt);
    }
}