using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_CriteriosInt : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargarPreguntas();
            cargarNiveles();
            loadEncuestas();
        }

        getCriteries();
    }

    //limpia campos
    public void clean()
    {
        lblIDCritery.Text = "0";
        btnSaveCritery.Text = "Save";

        cboxQuestion.SelectedIndex = -1;
        cboxLevel.SelectedIndex = -1;

        txtCriteryES.Text = string.Empty;
        txtCriteryEN.Text = string.Empty;
        txtValueCritery.Text = "0";
        gvCriteries.SelectedIndex = -1;
        checkActive.Checked = true;
    }

    //carga criterios registrados
    public void getCriteries()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 2);
        parameters.Add("@idCriterio", 0);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerCriterios", parameters);
        gvCriteries.DataSource = dt;
        gvCriteries.DataBind();
    }

    //carga preguntas de Auditorias Internas
    public void cargarPreguntas()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idPregunta", 0);
        parameters.Add("@accion", 2);
        parameters.Add("@module", 4);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerPreguntas", parameters);
        CommonAudit.FillDropDownList(ref cboxQuestion, dt);
    }

    //carga niveles
    public void cargarNiveles()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 2);
        parameters.Add("@idNivel", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerNiveles", parameters);
        CommonAudit.FillDropDownList(ref cboxLevel, dt);
    }

    protected void btnSaveCritery_Click(object sender, EventArgs e)
    {
        string strValorCriterio = txtValueCritery.Text.Trim();
        string strNombreCriterioES = txtCriteryES.Text.Trim();
        string strNombreCriterioEN = txtCriteryEN.Text.Trim();

        int intValorCriterio;

        //bool esNumero = int.TryParse(strValorCriterio, out intValorCriterio);

        if (cboxQuestion.SelectedIndex != 0 && cboxLevel.SelectedIndex != 0 && !String.IsNullOrEmpty(strNombreCriterioES) && !String.IsNullOrEmpty(strNombreCriterioEN)/* && esNumero*/)
        {
            insUpdCritery(lblIDCritery.Text.Trim() == string.Empty ? "0" : lblIDCritery.Text.Trim());
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

    protected void gvCriteries_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCriteries.PageIndex = e.NewPageIndex;
        getCriteries();
    }

    protected void gvCriteries_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@accion", 3);
            parameters.Add("@idCriterio", gvCriteries.SelectedRow.Cells[0].Text);
            parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
            parameters.Add("@idEncuesta", 0);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerCriterios", parameters);


            lblIDCritery.Text = gvCriteries.SelectedRow.Cells[0].Text;
            btnSaveCritery.Text = "Update";

            if (dt.Rows.Count > 0)
            {
                cboxQuestion.SelectedIndex = cboxQuestion.Items.IndexOf(cboxQuestion.Items.FindByValue(dt.Rows[0]["IDQ"].ToString()));
                cboxLevel.SelectedIndex = cboxLevel.Items.IndexOf(cboxLevel.Items.FindByText(dt.Rows[0]["NIVEL"].ToString()));

                txtCriteryES.Text = dt.Rows[0]["CRITERYES"].ToString().Trim();
                txtCriteryEN.Text = dt.Rows[0]["CRITERYEN"].ToString().Trim();
                txtValueCritery.Text = dt.Rows[0]["VALUE"].ToString().Trim();
                checkActive.Checked = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["ACTIVE"].ToString()));
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

    //insert critery
    public void insUpdCritery(string idCritery)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idCriterio", Convert.ToInt32(idCritery));
            parameters.Add("@idPregunta", cboxQuestion.SelectedValue.ToString());
            parameters.Add("@idNivel", cboxLevel.SelectedValue.ToString());
            parameters.Add("@criterioES", txtCriteryES.Text.Trim());
            parameters.Add("@criterioEN", txtCriteryEN.Text.Trim());
            parameters.Add("@valorCriterio", txtValueCritery.Text.Trim());
            parameters.Add("@usuario", Session["Nombre"].ToString());
            parameters.Add("@activo", checkActive.Checked);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Criteries", parameters);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('save successfully');", true);

            clean();
            getCriteries();
        }
        catch (Exception ex)
        {
            //  ctrlPopUpMessage1.setMessage(ex.Message, MESSAGE_TYPE.Error);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
        }
    }


    public void loadEncuestas()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 3);
        parameters.Add("@module", 4);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerEncuestas", parameters);
        CommonAudit.FillDropDownList(ref DDLFilter, dt);
    }

    protected void DDLFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idCriterio", 0);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);

        if (DDLFilter.SelectedIndex == 0)
        {
            parameters.Add("@accion", 2);
            parameters.Add("@idEncuesta", 0);
        }
        else
        {
            parameters.Add("@accion", 4);
            parameters.Add("@idEncuesta", DDLFilter.SelectedValue);
        }

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerCriterios", parameters);
        gvCriteries.DataSource = dt;
        gvCriteries.DataBind();

    }

}