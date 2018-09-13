using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_AuditoriaConfig : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetSurveys();
        }
    }

    #region Methods
    //Load registered Surveys
    public void GetSurveys()
    {
        //Load Data from Surveys
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@action", 1);
        parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Surveys", parameters);
        gvSurveys.DataSource = dt;
        gvSurveys.DataBind();
        //Load Data from Modules
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@action", 4);
        parameters2.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
        dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Module", parameters2);
        FillDropDownList(ref ddlModulos, dt, (string)Session["Locale"] == "es-MX" ? false : true);
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
                ddl.Items.Insert(0, "-- Choose One --");
            }
            else
            {
                ddl.Items.Insert(0, "-- Elige uno --");
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

    //Insert/Update Survey
    public void InsUpdSurvey(string idEncuesta)
    {
        string error = Validation();
        if (string.IsNullOrEmpty(error))
        {
            try
            {
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idEncuesta", Convert.ToInt32(idEncuesta));
                parameters.Add("@nombreEn", txtNombreEn.Text.Trim());
                parameters.Add("@nombreEs", txtNombreEs.Text.Trim());
                parameters.Add("@descripcionEn", txtDescEn.Text.Trim());
                parameters.Add("@descripcionEs", txtDescEs.Text.Trim());
                parameters.Add("@active", Convert.ToInt32(chkActivo.Checked));
                parameters.Add("@usuario", Session["Nombre"].ToString());
                parameters.Add("@idModulo", ddlModulos.SelectedValue);
                DataTable dtUser = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Survey", parameters);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save Successfully');", true);

                GetSurveys();
                Clean();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
            }
        }
    }

    //Clear Fields in Form
    public void Clean()
    {
        txtDescEn.Text = string.Empty;
        txtDescEs.Text = string.Empty;
        txtNombreEn.Text = string.Empty;
        txtNombreEs.Text = string.Empty;
        ddlModulos.SelectedIndex = -1;
        lblIdEncuesta.Text = string.Empty;
        btnSaveUser.Text = "Save";
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
            if (string.IsNullOrEmpty(txtNombreEn.Text.Trim()))
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Type the English Name of the survey.", leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }

            if (string.IsNullOrEmpty(txtNombreEs.Text.Trim()))
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Type the Spanish Name of the survey.", leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }

            if (ddlModulos.SelectedIndex < 1)
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Select a Module >>> " + ddlModulos.SelectedIndex.ToString() + ". Value >>> " + ddlModulos.SelectedValue.ToString(), leng));
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
    #endregion

    #region Events
    protected void gvSurveys_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[6].Visible = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clean();
    }

    protected void gvSurveys_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSurveys.PageIndex = e.NewPageIndex;
        GetSurveys();
    }

    protected void gvSurveys_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Validate if the field is empty, if isn't empty, sets the value
            if (gvSurveys.SelectedRow.Cells[2].Text != "&nbsp;")
            {
                txtDescEn.Text = gvSurveys.SelectedRow.Cells[2].Text;
            }
            else
            {
                txtDescEn.Text = "";
            }
            //Validate if the field is empty, if isn't empty, sets the value
            if (gvSurveys.SelectedRow.Cells[4].Text != "&nbsp;")
            {
                txtDescEs.Text = gvSurveys.SelectedRow.Cells[4].Text;
            }
            else
            {
                txtDescEs.Text = "";
            }
            txtNombreEn.Text = gvSurveys.SelectedRow.Cells[1].Text;
            txtNombreEs.Text = gvSurveys.SelectedRow.Cells[3].Text;
            //Validate if the field is 1, if isn't sets a false on the checkbox
            if (gvSurveys.SelectedRow.Cells[5].Text == "YES")
            {
                chkActivo.Checked = true;
            }
            else
            {
                chkActivo.Checked = false;
            }
            ddlModulos.SelectedValue = gvSurveys.SelectedRow.Cells[6].Text;
            lblIdEncuesta.Text = gvSurveys.SelectedRow.Cells[0].Text;
            btnSaveUser.Text = "Update";
        }
        catch (Exception ex)
        {
            //ctrlPopUpMessage1.setMessage(ex.Message, MESSAGE_TYPE.Error);
        }
    }

    protected void gvSurveys_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[6].Visible = false;
            gvSurveys.HeaderRow.Cells[6].Visible = false;//6

            //string leng = (string)Session["Locale"];
            if ((string)Session["Locale"] == "en-US")
            {
                e.Row.Cells[3].Visible = false;
                gvSurveys.HeaderRow.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                gvSurveys.HeaderRow.Cells[4].Visible = false;
            }
            else
            {
                e.Row.Cells[1].Visible = false;
                gvSurveys.HeaderRow.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                gvSurveys.HeaderRow.Cells[2].Visible = false;
            }

            e.Row.Attributes["onmouseover"] = "this.className = 'gridViewOver';";
            if ((e.Row.RowIndex % 2) == 1)
                e.Row.Attributes["onmouseout"] = "this.className = 'gridViewAlt';";
            else
                e.Row.Attributes["onmouseout"] = "this.className = 'gridView';";
            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, "Select$" + e.Row.RowIndex);
        }
    }

    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        InsUpdSurvey(lblIdEncuesta.Text.Trim() == string.Empty ? "0" : lblIdEncuesta.Text.Trim());
    }
    #endregion
}