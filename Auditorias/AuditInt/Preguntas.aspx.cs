using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_Preguntas : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargarEncuestas();
            loadEncuestas();
            getQuestions();
        }


    }

    //limpia campos
    public void clean()
    {
        lblIDQuestion.Text = "0";
        btnSaveQuestion.Text = "Save";

        cboxSurvey.SelectedIndex = -1;

        txtQuestionES.Text = string.Empty;
        txtQuestionEN.Text = string.Empty;
        txtDescriptionES.Text = string.Empty;
        txtDescriptionEN.Text = string.Empty;
        gvQuestions.SelectedIndex = -1;
        checkActive.Checked = true;
    }

    //carga preguntas registradas
    public void getQuestions()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idPregunta", 0);
        parameters.Add("@accion", 1);
        parameters.Add("@module", 0);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerPreguntas", parameters);
        gvQuestions.DataSource = dt;
        gvQuestions.DataBind();
    }

    //carga encuestas de Auditorias
    public void cargarEncuestas()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 2);
        parameters.Add("@module", 0);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerEncuestas", parameters);
        CommonAudit.FillDropDownList(ref cboxSurvey, dt);
    }

    protected void btnSaveQuestion_Click(object sender, EventArgs e)
    {
        string strPreguntaES = txtQuestionES.Text.Trim();
        string strPreguntaEN = txtQuestionEN.Text.Trim();
        string strDescripcionES = txtDescriptionES.Text.Trim();
        string strDescripcionEN = txtDescriptionEN.Text.Trim();

        if (cboxSurvey.SelectedIndex != 0 && !String.IsNullOrEmpty(strPreguntaES) && !String.IsNullOrEmpty(strPreguntaEN))
        {
            InsUpdQuestion(lblIDQuestion.Text.Trim() == string.Empty ? "0" : lblIDQuestion.Text.Trim());
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

    protected void gvQuestions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvQuestions.PageIndex = e.NewPageIndex;
        getQuestions();
    }

    protected void gvQuestions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPregunta", gvQuestions.SelectedRow.Cells[0].Text);
            parameters.Add("@accion", 3);
            parameters.Add("@module", 0);
            parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
            parameters.Add("@idEncuesta", 0);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerPreguntas", parameters);

            lblIDQuestion.Text = gvQuestions.SelectedRow.Cells[0].Text;
            btnSaveQuestion.Text = "Update";

            if (dt.Rows.Count > 0)
            {
                cboxSurvey.SelectedIndex = cboxSurvey.Items.IndexOf(cboxSurvey.Items.FindByValue(dt.Rows[0]["IDS"].ToString()));

                txtQuestionES.Text = dt.Rows[0]["QUESTIONES"].ToString().Trim();
                txtQuestionEN.Text = dt.Rows[0]["QUESTIONEN"].ToString().Trim();
                txtDescriptionES.Text = dt.Rows[0]["DESCRIPTIONES"].ToString().Trim();
                txtDescriptionEN.Text = dt.Rows[0]["DESCRIPTIONEN"].ToString().Trim();
                checkActive.Checked = Convert.ToBoolean(dt.Rows[0]["ACTIVE"].ToString());
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

    protected void gvQuestions_RowDataBound(object sender, GridViewRowEventArgs e)
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

    //Insert/Update Question
    public void InsUpdQuestion(string idPregunta)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPregunta", Convert.ToInt32(idPregunta));
            parameters.Add("@idEncuesta", cboxSurvey.SelectedValue.ToString());
            parameters.Add("@preguntaES", txtQuestionES.Text.Trim());
            parameters.Add("@preguntaEN", txtQuestionEN.Text.Trim());
            parameters.Add("@descripES", txtDescriptionES.Text.Trim());
            parameters.Add("@descripEN", txtDescriptionEN.Text.Trim());
            parameters.Add("@usuario", Session["Nombre"].ToString());
            parameters.Add("@activo", checkActive.Checked);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Questions", parameters);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('save successfully');", true);

            clean();
            getQuestions();
        }
        catch (Exception ex)
        {
            //  ctrlPopUpMessage1.setMessage(ex.Message, MESSAGE_TYPE.Error);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
        }
    }

    protected void DDLFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPregunta", 0);
            parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
            parameters.Add("@module", 0);

            if (DDLFilter.SelectedIndex == 0)
            {
                parameters.Add("@accion", 1);
                parameters.Add("@idEncuesta", 0);
            }
            else
            {
                parameters.Add("@accion", 4);
                parameters.Add("@idEncuesta", DDLFilter.SelectedValue);
            }

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerPreguntas", parameters);
            gvQuestions.DataSource = dt;
            gvQuestions.DataBind();
        }
        catch (Exception)
        {

        }
    }


    public void loadEncuestas()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 4);
        parameters.Add("@module", 0);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerEncuestas", parameters);
        CommonAudit.FillDropDownList(ref DDLFilter, dt);
    }

}