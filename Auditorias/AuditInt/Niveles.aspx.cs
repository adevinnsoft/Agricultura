using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_Niveles : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadLevel();
    }

    protected void gvLevels_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLevels.PageIndex = e.NewPageIndex;
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
    public void insUpdLevel()
    {
        try
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@accion", 1);
            parametros.Add("@id_Level", idLevel.Text);
            parametros.Add("@NameLevel", txtNameLevel.Text);
            parametros.Add("@ValueLevel", txtValueLevel.Text);
            parametros.Add("@ColorLevel", "#" + txtColorLevel.Text);
            parametros.Add("@Active", checkActive.Checked);
            parametros.Add("@CreateBy", Session["Nombre"].ToString());

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpLevel", parametros);
            LoadLevel();
            clean();
        }
        catch (Exception ex) { }
    }

    //clear fields
    public void clean()
    {
        txtNameLevel.Text = string.Empty;
        txtValueLevel.Text = string.Empty;
        txtColorLevel.Text = string.Empty;
        checkActive.Checked = true;
        idLevel.Text = string.Empty;
        btnSaveLevel.Text = "Save";
    }

    protected void btnSaveLevel_Click(object sender, EventArgs e)
    {
        string strValor = txtValueLevel.Text.Trim();
        string strNombre = txtNameLevel.Text.Trim();
        int intValue;

        bool esNumero = int.TryParse(strValor, out intValue);

        if (!String.IsNullOrEmpty(strNombre) && esNumero)
        {
            insUpdLevel();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Incorrec data');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clean();
    }

    public void LoadLevel()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@accion", 2);
        param.Add("@idNivel", 0);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerNiveles", param);
        gvLevels.DataSource = dt;
        gvLevels.DataBind();
    }

    protected void gvLevels_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@accion", 3);
            parametros.Add("@idNivel", gvLevels.SelectedRow.Cells[0].Text);
            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerNiveles", parametros);

            btnSaveLevel.Text = "Update";

            if (dt.Rows.Count > 0)
            {
                idLevel.Text = dt.Rows[0]["ID"].ToString().Trim();
                txtNameLevel.Text = dt.Rows[0]["Description"].ToString().Trim();
                txtValueLevel.Text = dt.Rows[0]["Valor de Nivel"].ToString().Trim();
                txtColorLevel.Text = dt.Rows[0]["Color Hexadecimal"].ToString().Trim().Replace("#", "");
                checkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Activo"].ToString());
            }
        }
        catch (Exception ex) { }
    }

}