using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_Criterios : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");

    public string error = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetCriteria();
        }
    }

    #region Methods
    //Load Registered Criteria
    public void GetCriteria()
    {
        //Load Data from Weightings
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@action", 1);
        parameters.Add("@idPregunta", 1);
        parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Criteria", parameters);
        gvCriteria.DataSource = dt;
        gvCriteria.DataBind();
        //Load Data from surveys
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@action", 4);
        parameters2.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
        dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Surveys", parameters2);
        FillDropDownList(ref ddlEncuesta, dt, (string)Session["Locale"] == "es-MX" ? false : true);
        ////Load Data from questions
        //Dictionary<string, object> parameters3 = new System.Collections.Generic.Dictionary<string, object>();
        //parameters3.Add("@action", 4);
        //parameters3.Add("@idEncuesta", Convert.ToInt32(lblIdEncuesta.Text));
        //dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Questions", parameters3);
        //FillDropDownListQuestions(ref ddlPregunta, dt, (string)Session["Locale"] == "es-MX" ? false : true);
    }

    //Fill Module's DropDownList
    public static void FillDropDownList(ref DropDownList ddl, DataTable dt, Boolean english)
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

    //Fill Module's DropDownList
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

    //Insert/Update Question
    public void InsUpdCriteria(string idCriterio)
    {
        string error = Validation();
        if (string.IsNullOrEmpty(error))
        {
            try
            {
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idCriterio", Convert.ToInt32(idCriterio));
                parameters.Add("@vCriterio", txtCriterio.Text.Trim());
                parameters.Add("@vCriterioEng", txtCriterioEng.Text.Trim());
                parameters.Add("@active", Convert.ToInt32(chkActivo.Checked));
                parameters.Add("@usuario", Session["Nombre"].ToString());
                parameters.Add("@idPregunta", ddlPregunta.SelectedValue);
                DataTable dtUser = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Criteria", parameters);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save Successfully');", true);

                GetCriteria();
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
            if (string.IsNullOrEmpty(txtCriterio.Text.Trim()))
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Type the criteria.", leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }

            if (ddlEncuesta.SelectedIndex < 1)
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Select a Survey >>> " + ddlEncuesta.SelectedIndex.ToString() + ". Value >>> " + ddlEncuesta.SelectedValue.ToString(), leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }

            if (ddlPregunta.SelectedIndex < 1)
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Select a Question >>> " + ddlEncuesta.SelectedIndex.ToString() + ". Value >>> " + ddlEncuesta.SelectedValue.ToString(), leng));
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
        txtCriterio.Text = string.Empty;
        txtCriterioEng.Text = string.Empty;
        lblIdCriterio.Text = string.Empty;
        btnSaveUser.Text = "Save";
        //ddlPregunta.SelectedIndex = -1;
        ddlPregunta.DataSource = "";
        ddlPregunta.DataBind();
        //ddlPregunta.DataTextField = "";
        //ddlPregunta.DataValueField = "";
        GetCriteria();
    }

    #endregion

    #region Events
    protected void gvCriteria_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Cells[1].Visible = false;
            gvCriteria.HeaderRow.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            gvCriteria.HeaderRow.Cells[2].Visible = false;

            if ((string)Session["Locale"] == "en-US")
            {
                e.Row.Cells[4].Visible = false;
                gvCriteria.HeaderRow.Cells[4].Visible = false;
            }
            else
            {
                e.Row.Cells[3].Visible = false;
                gvCriteria.HeaderRow.Cells[3].Visible = false;
            }

            e.Row.Attributes["onmouseover"] = "this.className = 'gridViewOver';";
            if ((e.Row.RowIndex % 2) == 1)
                e.Row.Attributes["onmouseout"] = "this.className = 'gridViewAlt';";
            else
                e.Row.Attributes["onmouseout"] = "this.className = 'gridView';";
            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvCriteria_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCriteria.PageIndex = e.NewPageIndex;
        GetCriteria();
    }

    protected void gvCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtCriterio.Text = System.Net.WebUtility.HtmlDecode(gvCriteria.SelectedRow.Cells[4].Text);
            txtCriterioEng.Text = System.Net.WebUtility.HtmlDecode(gvCriteria.SelectedRow.Cells[3].Text);
            //Valida si el campo es 1, de lo contrario se coloca un false en el checkbox
            if (gvCriteria.SelectedRow.Cells[5].Text == "YES")
            {
                chkActivo.Checked = true;
            }
            else
            {
                chkActivo.Checked = false;
            }

            //if (ddlEncuesta.SelectedIndex > 0)
            //{
            error = "survey";
            ddlEncuesta.SelectedValue = gvCriteria.SelectedRow.Cells[2].Text;
            //}
            lblIdCriterio.Text = gvCriteria.SelectedRow.Cells[0].Text;
            lblIdEncuesta.Text = gvCriteria.SelectedRow.Cells[2].Text;
            btnSaveUser.Text = "Update";

            //Load Data from questions
            Dictionary<string, object> parameters3 = new System.Collections.Generic.Dictionary<string, object>();
            parameters3.Add("@action", 4);
            parameters3.Add("@idEncuesta", Convert.ToInt32(lblIdEncuesta.Text));
            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Questions", parameters3);
            FillDropDownListQuestions(ref ddlPregunta, dt, (string)Session["Locale"] == "es-MX" ? false : true);
            error = "question";
            ddlPregunta.SelectedValue = gvCriteria.SelectedRow.Cells[1].Text;
        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('The " + error + " is disabled, please active the question before continue.');", true);
            Clean();
        }
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
        }
    }

    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        InsUpdCriteria(lblIdCriterio.Text.Trim() == string.Empty ? "0" : lblIdCriterio.Text.Trim());
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clean();
    }
    #endregion
}