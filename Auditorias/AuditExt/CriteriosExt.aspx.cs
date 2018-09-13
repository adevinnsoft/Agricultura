using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_AuditExt_CriteriosExt : BasePage
{
    DataAccess dsAuditoria = new DataAccess("dbAuditoria");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargarPreguntas();
            cargarPonderaciones();
        }

        getCriteries();
    }

    //limpia campos
    public void clean()
    {
        btnSaveCritery.Text = "Save";

        cboxQuestion.SelectedIndex = -1;
        checkCriteries.SelectedIndex = -1;
        gvCriteries.SelectedIndex = -1;
    }

    //carga criterios registrados
    public void getCriteries()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 1);
        parameters.Add("@idPregunta", 0);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerCriteriosExt", parameters);
        gvCriteries.DataSource = dt;
        gvCriteries.DataBind();
    }

    //carga preguntas de Auditorias Internas
    public void cargarPreguntas()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idPregunta", 0);
        parameters.Add("@accion", 2);
        parameters.Add("@module", 8);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);
        parameters.Add("@idEncuesta", 0);

        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerPreguntas", parameters);
        CommonAudit.FillDropDownList(ref cboxQuestion, dt);
    }

    public void cargarPonderaciones()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@accion", 1);
        parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? false : true);
        DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerPonderaciones", parameters);

        checkCriteries.DataSource = dt;
        checkCriteries.DataValueField = "VALUE";
        checkCriteries.DataTextField = "PONDERACION";
        checkCriteries.DataBind();
    }

    protected void btnSaveCritery_Click(object sender, EventArgs e)
    {
        string ponderacionesSeleccionadasXML = ponderacionesXML();

        if (cboxQuestion.SelectedIndex != 0 && ponderacionesSeleccionadasXML != "Error")
        {
            insUpdCritery(cboxQuestion.SelectedValue, ponderacionesSeleccionadasXML);
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
            parameters.Add("@accion", 2);
            parameters.Add("@idPregunta", gvCriteries.SelectedRow.Cells[0].Text);
            parameters.Add("@english", (string)Session["Locale"] == "es-MX" ? 0 : 1);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_obtenerCriteriosExt", parameters);

            btnSaveCritery.Text = "Update";

            if (dt.Rows.Count > 0)
            {
                cboxQuestion.SelectedIndex = cboxQuestion.Items.IndexOf(cboxQuestion.Items.FindByValue(dt.Rows[0]["IDQ"].ToString()));

                String[] criterios = dt.Rows[0]["CRITERY"].ToString().Split(',');

                for (int i = 0; i < criterios.Length; i++)
                {
                    try
                    {
                        checkCriteries.Items.FindByValue(criterios[i].ToString()).Selected = true;
                    }
                    catch (Exception ex) { }
                }
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

    private string ponderacionesXML()
    {
        StringBuilder xmlString = new StringBuilder();
        string xmlRootName = "Ponderaciones";
        int count = 0;

        xmlString.AppendFormat("<{0}>", xmlRootName);

        foreach (ListItem item in checkCriteries.Items)
        {
            if (item.Selected)
            {
                xmlString.AppendFormat("<id>{0}", ++count);
                xmlString.AppendFormat("<valueP>{0}</valueP>", item.Value);
                xmlString.Append("</id>");
            }
        }

        xmlString.AppendFormat("</{0}>", xmlRootName);

        if (count > 0)
        {
            return xmlString.ToString();
        }
        else
        {
            return "Error";
        }
    }

    //insert critery
    public void insUpdCritery(string idPregunta, string ponderacionesXML)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPregunta", cboxQuestion.SelectedValue.ToString());
            parameters.Add("@ponderacionesXML", ponderacionesXML);

            DataTable dt = dsAuditoria.executeStoreProcedureDataTable("spr_InsUpd_CriteriesExt", parameters);
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
}