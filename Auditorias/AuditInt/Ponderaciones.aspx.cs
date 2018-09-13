using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditInt_Ponderaciones : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetWeightings();
        }
    }

    #region Methods
    //Load registered Weighting
    public void GetWeightings()
    {
        //Load Data from Weightings
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@action", 1);
        parameters.Add("@English", (string)Session["Locale"] == "es-MX" ? false : true);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_Get_Weightings", parameters);
        gvWeightings.DataSource = dt;
        gvWeightings.DataBind();
    }

    //Clear Textboxes
    public void Clean()
    {
        txtName.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtWeightings.Text = "0";
        btnSaveUser.Text = "Save";
        lblIdPonderacion.Text = string.Empty;
        txtNameEN.Text = string.Empty;
        txtDescriptionEN.Text = string.Empty;
    }

    //Insert/Update Weighting
    public void InsUpdWeighting(string idPonderacion)
    {
        string error = Validation();
        if (string.IsNullOrEmpty(error))
        {
            try
            {
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idPonderacion", Convert.ToInt32(idPonderacion));
                parameters.Add("@vName", txtName.Text.Trim());
                parameters.Add("@vDescription", txtDescription.Text.Trim());
                parameters.Add("@vNameEN", txtNameEN.Text.Trim());
                parameters.Add("@vDescriptionEN", txtDescriptionEN.Text.Trim());
                parameters.Add("@iPonderacion", Convert.ToInt32(txtWeightings.Text.Trim()));
                parameters.Add("@active", chkActive.Checked);
                parameters.Add("@usuario", Session["Nombre"].ToString());
                DataTable dtUser = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_Weighting", parameters);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save Successfully');", true);
                GetWeightings();
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
            if (string.IsNullOrEmpty(txtNameEN.Text.Trim()))
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Type the Name of the weighting.", leng));
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + mensajes + "');", true);
            }

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                mensajes.Append(" " + lablesXML.getNameSpanish("Escriba el nombre de la ponderación.", leng));
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
    protected void btnMenos_Click(object sender, EventArgs e)
    {
        int num = Convert.ToInt32(txtWeightings.Text);
        num--;
        txtWeightings.Text = Convert.ToString(num);
    }

    protected void btnMas_Click(object sender, EventArgs e)
    {
        int num = Convert.ToInt32(txtWeightings.Text);
        num++;
        txtWeightings.Text = Convert.ToString(num);
    }

    protected void gvWeightings_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((string)Session["Locale"] == "en-US")
            {
                e.Row.Cells[1].Visible = false;
                gvWeightings.HeaderRow.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                gvWeightings.HeaderRow.Cells[2].Visible = false;
            }
            else
            {
                e.Row.Cells[3].Visible = false;
                gvWeightings.HeaderRow.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                gvWeightings.HeaderRow.Cells[4].Visible = false;
            }

            e.Row.Attributes["onmouseover"] = "this.className = 'gridViewOver';";
            if ((e.Row.RowIndex % 2) == 1)
                e.Row.Attributes["onmouseout"] = "this.className = 'gridViewAlt';";
            else
                e.Row.Attributes["onmouseout"] = "this.className = 'gridView';";
            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvWeightings_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ////Valida si el campo viene vació, de lo contrario se coloca su valor
            //if (gvWeightings.SelectedRow.Cells[2].Text != "&nbsp;")
            //{
            //    txtDescription.Text = gvWeightings.SelectedRow.Cells[2].Text;
            //}
            //else
            //{
            //    txtDescription.Text = "";
            //}
            txtDescription.Text = System.Net.WebUtility.HtmlDecode(gvWeightings.SelectedRow.Cells[2].Text);
            txtDescriptionEN.Text = System.Net.WebUtility.HtmlDecode(gvWeightings.SelectedRow.Cells[4].Text);
            //Valida si el campo es 1, de lo contrario se coloca un false en el checkbox
            if (gvWeightings.SelectedRow.Cells[6].Text == "YES")
            {
                chkActive.Checked = true;
            }
            else
            {
                chkActive.Checked = false;
            }
            txtName.Text = System.Net.WebUtility.HtmlDecode(gvWeightings.SelectedRow.Cells[1].Text);
            txtNameEN.Text = System.Net.WebUtility.HtmlDecode(gvWeightings.SelectedRow.Cells[3].Text);
            txtWeightings.Text = gvWeightings.SelectedRow.Cells[5].Text;
            lblIdPonderacion.Text = gvWeightings.SelectedRow.Cells[0].Text;
            btnSaveUser.Text = "Update";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void gvWeightings_PageIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvWeightings_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvWeightings.PageIndex = e.NewPageIndex;
        GetWeightings();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clean();
    }

    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        InsUpdWeighting(lblIdPonderacion.Text.Trim() == string.Empty ? "0" : lblIdPonderacion.Text.Trim());
    }
    #endregion

}