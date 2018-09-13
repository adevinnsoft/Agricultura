using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_Semaforos : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadExInt();
        }

        LoadSemaforos();
    }

    protected void gvLevels_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSemaforos.PageIndex = e.NewPageIndex;
    }

    protected void gvLevels_RowDataBound(object sender, GridViewRowEventArgs e)
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

    //Insert user
    public void insUpdSemaforo()
    {
        try
        {

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@idSemaforo", idSemaforo.Text);
            parametros.Add("@idModulo", ddlModulo.SelectedValue.ToString());
            parametros.Add("@inicial", txtInitial.Text.Trim());
            parametros.Add("@final", txtFinal.Text.Trim());
            parametros.Add("@color", "#" + txtColorSemaforo.Text.Trim());
            parametros.Add("@usuario", Session["Nombre"].ToString());
            parametros.Add("@estado", checkActive.Checked);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Semaforos", parametros);
            LoadSemaforos();
            clean();

        }
        catch (Exception ex) { }
    }

    //clear fields
    public void clean()
    {
        txtFinal.Text = string.Empty;
        txtInitial.Text = string.Empty;
        txtColorSemaforo.Text = string.Empty;
        idSemaforo.Text = string.Empty;
        btnSaveSemaforo.Text = "Save";
        ddlModulo.SelectedIndex = -1;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clean();
    }

    public void LoadSemaforos()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@accion", 3);
        param.Add("@idModulo", 0);
        param.Add("@idSemaforo", 0);
        param.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_get_Semaforos", param);
        gvSemaforos.DataSource = dt;
        gvSemaforos.DataBind();
    }

    protected void gvLevels_SelectedIndexChanged(object sender, EventArgs e)
    {

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@accion", 4);
        param.Add("@idModulo", 0);
        param.Add("@idSemaforo", gvSemaforos.SelectedRow.Cells[0].Text);
        param.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);

        idSemaforo.Text = gvSemaforos.SelectedRow.Cells[0].Text;

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_get_Semaforos", param);

        btnSaveSemaforo.Text = "Update";

        if (dt.Rows.Count > 0)
        {
            ddlModulo.SelectedIndex = ddlModulo.Items.IndexOf(ddlModulo.Items.FindByValue(dt.Rows[0]["Module"].ToString()));
            idSemaforo.Text = dt.Rows[0]["ID"].ToString().Trim();
            txtInitial.Text = dt.Rows[0]["Inicial"].ToString().Trim();
            txtFinal.Text = dt.Rows[0]["Final"].ToString().Trim();
            txtColorSemaforo.Text = dt.Rows[0]["Color"].ToString().Trim().Replace("#", "");
            checkActive.Checked = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Activo"].ToString()));
        }
    }


    public void LoadExInt()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@Action", 2);
        parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? 0 : 1);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Module", parameters);
        CommonAudit.FillDropDownList(ref ddlModulo, dt);
    }



    protected void btnSaveSemaforo_Click(object sender, EventArgs e)
    {
        string strInitial = txtInitial.Text.Trim();
        string strFinal = txtFinal.Text.Trim();
        int intValue;

        bool Inicial = int.TryParse(strInitial, out intValue);
        bool Final = int.TryParse(strFinal, out intValue);

        if (Inicial && Final && !string.IsNullOrWhiteSpace(txtColorSemaforo.Text) && ddlModulo.SelectedIndex > 0)
        {
            insUpdSemaforo();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save Correct');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Incorrec data');", true);
        }
    }

}