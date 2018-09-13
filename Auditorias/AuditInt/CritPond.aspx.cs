using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_CritPond : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetCriPon();
        }
    }

    #region Events
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clean();
    }

    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        InsUpdCriteriaWeighting(lblCriterioPonderacion.Text.Trim() == string.Empty ? "0" : lblCriterioPonderacion.Text.Trim());
    }

    protected void gvCritPon_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Valida si el campo es 1, de lo contrario se coloca un false en el checkbox
            if (gvCritPon.SelectedRow.Cells[9].Text == "YES")
            {
                chkActivo.Checked = true;
            }
            else
            {
                chkActivo.Checked = false;
            }

            ddlEncuesta.SelectedValue = gvCritPon.SelectedRow.Cells[1].Text;
            lblCriterioPonderacion.Text = gvCritPon.SelectedRow.Cells[0].Text;
            btnSaveUser.Text = "Update";

            //if (ddlEncuesta.SelectedIndex > 0)
            //{
            //Load Data from questions
            Dictionary<string, object> parameters3 = new System.Collections.Generic.Dictionary<string, object>();
            parameters3.Add("@action", 4);
            parameters3.Add("@idEncuesta", Convert.ToInt32(ddlEncuesta.SelectedValue));
            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Questions", parameters3);
            FillDropDownListQuestions(ref ddlPregunta, dt, (string)Session["Locale"] == "es-MX" ? false : true);
            ddlPregunta.SelectedValue = gvCritPon.SelectedRow.Cells[2].Text;
            //}

            if (ddlPregunta.SelectedIndex > 0)
            {
                //Load Data from criteria
                Dictionary<string, object> parameters1 = new System.Collections.Generic.Dictionary<string, object>();
                parameters1.Add("@action", 4);
                parameters1.Add("@idPregunta", Convert.ToInt32(ddlPregunta.SelectedValue));
                parameters1.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
                DataTable dt1 = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Criteria", parameters1);
                FillDropDownListCriteria(ref ddlCriterio, dt1, (string)Session["Locale"] == "es-MX" ? false : true);
                ddlCriterio.SelectedValue = gvCritPon.SelectedRow.Cells[3].Text;

                //Load Data from weightings
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@action", 2);
                parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
                DataTable dt2 = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Weightings", parameters);
                FillDropDownListWeighting(ref ddlPonderacion, dt2, (string)Session["Locale"] == "es-MX" ? false : true);
                ddlPonderacion.SelectedValue = gvCritPon.SelectedRow.Cells[4].Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void ddlEncuesta_DataBound(object sender, EventArgs e)
    {

    }

    protected void gvCritPon_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            gvCritPon.HeaderRow.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            gvCritPon.HeaderRow.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            gvCritPon.HeaderRow.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            gvCritPon.HeaderRow.Cells[4].Visible = false;

            if ((string)Session["Locale"] == "en-US")
            {
                e.Row.Cells[5].Visible = false;
                gvCritPon.HeaderRow.Cells[5].Visible = false;
                e.Row.Cells[7].Visible = false;
                gvCritPon.HeaderRow.Cells[7].Visible = false;
            }
            else
            {
                e.Row.Cells[6].Visible = false;
                gvCritPon.HeaderRow.Cells[6].Visible = false;
                e.Row.Cells[8].Visible = false;
                gvCritPon.HeaderRow.Cells[8].Visible = false;
            }

            e.Row.Attributes["onmouseover"] = "this.className = 'gridViewOver';";
            if ((e.Row.RowIndex % 2) == 1)
                e.Row.Attributes["onmouseout"] = "this.className = 'gridViewAlt';";
            else
                e.Row.Attributes["onmouseout"] = "this.className = 'gridView';";
            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvCritPon_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCritPon.PageIndex = e.NewPageIndex;
        GetCriPon();
    }

    protected void ddlEncuesta_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEncuesta.SelectedIndex > 0)
        {
            //Load Data from questions
            Dictionary<string, object> parameters3 = new System.Collections.Generic.Dictionary<string, object>();
            parameters3.Add("@action", 4);
            parameters3.Add("@idEncuesta", Convert.ToInt32(ddlEncuesta.SelectedValue));
            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Questions", parameters3);
            FillDropDownListQuestions(ref ddlPregunta, dt, (string)Session["Locale"] == "es-MX" ? false : true);
        }
        else
        {
            ddlPregunta.DataSource = "";
            ddlPregunta.DataBind();
            ddlCriterio.DataSource = "";
            ddlCriterio.DataBind();
            ddlPonderacion.DataSource = "";
            ddlPonderacion.DataBind();
            //ddlPregunta.SelectedIndex = 0;
        }
    }

    protected void ddlPregunta_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPregunta.SelectedIndex > 0)
        {
            //Load Data from criteria
            Dictionary<string, object> parameters3 = new System.Collections.Generic.Dictionary<string, object>();
            parameters3.Add("@action", 4);
            parameters3.Add("@idPregunta", Convert.ToInt32(ddlPregunta.SelectedValue));
            parameters3.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Criteria", parameters3);
            FillDropDownListCriteria(ref ddlCriterio, dt, (string)Session["Locale"] == "es-MX" ? false : true);
            //Load Data from weightings
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@action", 2);
            parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
            DataTable dt1 = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Weightings", parameters);
            FillDropDownListWeighting(ref ddlPonderacion, dt1, (string)Session["Locale"] == "es-MX" ? false : true);
        }
        else
        {
            ddlCriterio.DataSource = "";
            ddlCriterio.DataBind();
            ddlPonderacion.DataSource = "";
            ddlPonderacion.DataBind();
        }
    }
    #endregion

    #region Methods
    //Load registered Criteria
    public void GetCriPon()
    {
        //Load Data from Weightings
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@action", 1);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_CriteriaWeighting", parameters);
        gvCritPon.DataSource = dt;
        gvCritPon.DataBind();

        //Load Data from surveys
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@action", 4);
        parameters2.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
        dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Surveys", parameters2);
        FillDropDownListSurvey(ref ddlEncuesta, dt, (string)Session["Locale"] == "es-MX" ? false : true);
    }

    //Fill Module's DropDownList
    public static void FillDropDownListSurvey(ref DropDownList ddl, DataTable dt, Boolean english)
    {
        ddl.DataSource = dt;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "Description";
        ddl.DataBind();
        if (ddl.Items.Count > 0)
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- Choose a Survey --");
            }
            else
            {
                ddl.Items.Insert(0, "-- Elija una Encuesta --");
            }

            ddl.SelectedIndex = 0;
        }
        else
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- There are no records --");
            }
            else
            {
                ddl.Items.Insert(0, "-- No existen registros --");
            }

            ddl.SelectedIndex = 0;
        }

        dt.Dispose();
    }

    //Fill Questions DropDownList
    public static void FillDropDownListQuestions(ref DropDownList ddl, DataTable dt, Boolean english)
    {
        ddl.DataSource = dt;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "PREGUNTA";
        ddl.DataBind();
        if (ddl.Items.Count > 0)
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- Choose a Question --");
            }
            else
            {
                ddl.Items.Insert(0, "-- Elija una Pregunta --");
            }

            ddl.SelectedIndex = 0;
        }
        else
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- There are no records --");
            }
            else
            {
                ddl.Items.Insert(0, "-- No existen registros --");
            }

            ddl.SelectedIndex = 0;
        }

        dt.Dispose();
    }

    //Fill Criterias DropDownList
    public static void FillDropDownListCriteria(ref DropDownList ddl, DataTable dt, Boolean english)
    {
        ddl.DataSource = dt;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "CRITERIO";
        ddl.DataBind();
        if (ddl.Items.Count > 0)
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- Choose a Criteria --");
            }
            else
            {
                ddl.Items.Insert(0, "-- Elija un Criterio --");
            }

            ddl.SelectedIndex = 0;
        }
        else
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- There are no records --");
            }
            else
            {
                ddl.Items.Insert(0, "-- No existen registros --");
            }

            ddl.SelectedIndex = 0;
        }

        dt.Dispose();
    }

    //Fill Weightings DropDownList
    public static void FillDropDownListWeighting(ref DropDownList ddl, DataTable dt, Boolean english)
    {
        ddl.DataSource = dt;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "PONDERACION";
        ddl.DataBind();
        if (ddl.Items.Count > 0)
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- Choose a Weighting --");
            }
            else
            {
                ddl.Items.Insert(0, "-- Elija una Ponderación --");
            }

            ddl.SelectedIndex = 0;
        }
        else
        {
            if (english == true)
            {
                ddl.Items.Insert(0, "-- There are no records --");
            }
            else
            {
                ddl.Items.Insert(0, "-- No existen registros --");
            }

            ddl.SelectedIndex = 0;
        }

        dt.Dispose();
    }

    //Insert/Update Question
    public void InsUpdCriteriaWeighting(string idCriterioPonderacion)
    {
        string error = Validation();
        if (string.IsNullOrEmpty(error))
        {
            try
            {
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idCriterioPonderacion", Convert.ToInt32(idCriterioPonderacion));
                parameters.Add("@idCriterio", ddlCriterio.SelectedValue);
                parameters.Add("@idPonderacion", ddlPonderacion.SelectedValue);
                parameters.Add("@active", Convert.ToInt32(chkActivo.Checked));
                parameters.Add("@usuario", Session["Nombre"].ToString());
                DataTable dtUser = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_CriteriaWeighting", parameters);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save Successfully');", true);

                GetCriPon();
                Clean();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
            }
        }
    }

    //Validate Fields
    public string Validation()
    {
        string leng = (string)Session["Locale"];
        System.Text.StringBuilder mensajes = new System.Text.StringBuilder();
        string instruccionMessage = lablesXML.getNameSpanish("Please complete the following information:", leng);
        mensajes.Append(instruccionMessage);
        try
        {
            if (ddlPonderacion.SelectedIndex < 1)
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Select a Weighting >>> " + ddlEncuesta.SelectedIndex.ToString() + ". Value >>> " + ddlEncuesta.SelectedValue.ToString(), leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }

            if (ddlCriterio.SelectedIndex < 1)
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Select a Criteria >>> " + ddlEncuesta.SelectedIndex.ToString() + ". Value >>> " + ddlEncuesta.SelectedValue.ToString(), leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }
        }
        catch (Exception ex)
        {
            //ctrlPopUpMessage1.setMessage(ex.ToString());
        }


        if (mensajes.ToString().Length <= instruccionMessage.Length)
        {
            mensajes.Length = 0;
        }

        return mensajes.ToString();
    }

    //Clear Fields
    public void Clean()
    {
        lblCriterioPonderacion.Text = string.Empty;
        btnSaveUser.Text = "Save";
        GetCriPon();
        ddlPregunta.DataSource = "";
        ddlPregunta.DataBind();
        ddlCriterio.DataSource = "";
        ddlCriterio.DataBind();
        ddlPonderacion.DataSource = "";
        ddlPonderacion.DataBind();
        chkActivo.Checked = false;
    }
    #endregion

}