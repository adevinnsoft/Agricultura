using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Auditorias_AuditInt_Encuestas : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadModulos();
            LoadCheck_plants();
            CargarChkPlants();
        }

        LoadEncuestas();
    }

    protected void gvLevels_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEncuestas.PageIndex = e.NewPageIndex;
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

    public void clean()
    {
        txtEncuestaEn.Text = "";
        txtEncuestaEs.Text = "";
        txtDescripcionEn.Text = "";
        txtDescripcionEs.Text = "";
        ddlModulos.SelectedIndex = -1;
        btnSaveEncuesta.Text = "Save";
        idEncuesta.Text = "0";
        chkActivo.Checked = true;
        checkPlants1.SelectedIndex = -1;
    }//clean

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clean();
        CargarChkPlants();
    }

    public void LoadEncuestas()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@accion", 5);
        param.Add("@module", 0);
        param.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        param.Add("@idEncuesta", 0);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerEncuestas", param);
        gvEncuestas.DataSource = dt;
        gvEncuestas.DataBind();
    }//LoadEncuestas

    protected void gvLevels_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@accion", 6);
        param.Add("@module", 0);
        param.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        param.Add("@idEncuesta", gvEncuestas.SelectedRow.Cells[0].Text);

        DataSet ds = dsAuditoria.executeStoreProcedureDataSet("spr_obtenerEncuestas", param);
        DataTable dt = ds.Tables[0];

        idEncuesta.Text = gvEncuestas.SelectedRow.Cells[0].Text;

        btnSaveEncuesta.Text = "Update";

        if (dt.Rows.Count > 0)
        {
            ddlModulos.SelectedIndex = ddlModulos.Items.IndexOf(ddlModulos.Items.FindByValue(dt.Rows[0]["Modulo"].ToString()));
            idEncuesta.Text = dt.Rows[0]["ID"].ToString().Trim();
            txtEncuestaEs.Text = dt.Rows[0]["EncuestaEs"].ToString().Trim();
            txtEncuestaEn.Text = dt.Rows[0]["EncuestaEn"].ToString().Trim();
            txtDescripcionEs.Text = dt.Rows[0]["DescripcionEs"].ToString().Trim();
            txtDescripcionEn.Text = dt.Rows[0]["DescripcionEn"].ToString().Trim();
            chkActivo.Checked = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Activo"].ToString()));
        }

        DataTable dtP = ds.Tables[1];
        checkPlants1.SelectedIndex = -1;

        foreach (DataRow item in dtP.Rows)
        {
            try
            {
                checkPlants1.Items.FindByValue(item["idPlantaFK"].ToString()).Selected = true;
            }
            catch (Exception ex) { }
        }


    }//gvLevels_SelectedIndexChanged

    public void LoadModulos()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@Action", 2);
        parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? 0 : 1);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Module", parameters);
        CommonAudit.FillDropDownList(ref ddlModulos, dt);
    }//LoadModulos

    protected void btnSaveEncuesta_Click(object sender, EventArgs e)
    {
        int x = 0;
        for (int pos = 0; pos < checkPlants1.Items.Count; pos++)
        {
            if (checkPlants1.Items[pos].Selected == false)
                x = x + 1;
        }
        if (!txtEncuestaEn.Text.Trim().Equals("") && !txtEncuestaEs.Text.Trim().Equals("") && ddlModulos.SelectedIndex > 0 && x != checkPlants1.Items.Count)
            SaveEncuesta();
        else
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Incorrect Data');", true);
    }//btnSaveEncuesta_Click

    public void SaveEncuesta()
    {
        try
        {
            DataTable valoresTabla = (DataTable)JsonConvert.DeserializeObject(generarJSON(), (typeof(DataTable)));

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@idEncuesta", idEncuesta.Text.Trim());
            parametros.Add("@nombreEn", txtEncuestaEn.Text.Trim());
            parametros.Add("@nombreEs", txtEncuestaEs.Text.Trim());
            parametros.Add("@descripcionEn", txtDescripcionEn.Text.Trim());
            parametros.Add("@descripcionEs", txtDescripcionEs.Text.Trim());
            parametros.Add("@idModulo", ddlModulos.SelectedValue);
            parametros.Add("@activo", chkActivo.Checked);
            parametros.Add("@usuario", Session["Nombre"].ToString());
            //string plantasSeleccionadasXML = plantasXML();
            parametros.Add("@tblPlantsSelec", valoresTabla);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Encuestas", parametros);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save Correct');", true);
            LoadEncuestas();
            clean();
            CargarChkPlants();

        }
        catch (Exception) { }
    }//SaveEncuesta

    public void LoadCheck_plants()
    {
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        parametros.Add("@idUser", Session["idUsuario"]);
        parametros.Add("@accion", 2);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_get_plants", parametros);
        checkPlants1.DataSource = dt;
        checkPlants1.DataValueField = "ID";
        checkPlants1.DataTextField = "Description";
        checkPlants1.DataBind();
    }//LoadCheck_plants

    public void CargarChkPlants()
    {
        for (int pos = 0; pos < checkPlants1.Items.Count; pos++)
            checkPlants1.Items[pos].Selected = true;
    }

    public string generarJSON()
    {
        /*JSon generamos el json para enviar al sp*/
        string json = "[";
        foreach (ListItem item in checkPlants1.Items)
        {
            json += "{\"idSurvey\":" + idEncuesta.Text + "," +
                    "\"idPlant\":" + item.Value + "," +
                    "\"Active\":" + (item.Selected ? "1" : "0") +
                     "},";
        }

        json = json.Substring(0, (json.Length - 1));
        json += "]";
        return json;
    }

}