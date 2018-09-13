using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Globalization;

public partial class frmAbejorrosCriterios : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            llenaTabla();
        }
    }

    public void llenaTabla()
    {
        try
        {
            gvEtapas.DataSource = dataaccess.executeStoreProcedureDataTable("spr_AbejorrosCriteriosGv", new Dictionary<string, object>() { { "@idioma", getIdioma() } });
            gvEtapas.DataBind();
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }

    }
    protected void gvEtapas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvEtapas.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvEtapas.SelectedPersistedDataKey["idCriterio"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvEtapas.SelectedDataKey["idCriterio"].ToString(), out id);
            }

            parameters.Add("@idCriterio", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_AbejorrosCriteriosGv", parameters);

            hddIdCriterio.Value = dt.Rows[0]["idCriterio"].ToString();
            txtCriterio.Text = dt.Rows[0]["Criterio"].ToString();
            txtCriterio_EN.Text = dt.Rows[0]["Criterio_EN"].ToString();
            txtDescripcion_ES.Text = dt.Rows[0]["Descripcion"].ToString();
            txtDescripcion_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }

    }
    protected void gvEtapas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEtapas, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void gvEtapas_PreRender(object sender, EventArgs e)
    {
        if (gvEtapas.HeaderRow != null)
            gvEtapas.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void  btnSave_Click(object sender, EventArgs e)
    {
        DataTable result;
        try
        {
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("@idioma", getIdioma());
            parameters.Add("@idCriterio", hddIdCriterio.Value);
            parameters.Add("@Criterio", txtCriterio.Text);
            parameters.Add("@Criterio_EN", txtCriterio_EN.Text);
            parameters.Add("@Descripcion", txtDescripcion_ES.Text);
            parameters.Add("@Descripcion_EN", txtDescripcion_EN.Text);
            parameters.Add("@Activo", chkActivo.Checked);
            parameters.Add("@UsuarioModifico", Session["idUsuario"].ToString());


            result = dataaccess.executeStoreProcedureDataTable("spr_AbejorroCriterioGuardar", parameters);

            switch (result.Rows[0]["msg"].ToString())
            {
                case "ok":
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Success);
                    break;
                case "info":
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Info);
                    break;
                default:
                    popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "NoGuardado").ToString() + ": " + result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Error);
                    break;
            }
            limpiacampos();
            llenaTabla();

        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }
    }

    public void limpiacampos()
    {
         hddIdCriterio.Value = string.Empty;
         txtCriterio.Text = string.Empty;
         txtDescripcion_EN.Text = string.Empty;
         txtDescripcion_ES.Text = string.Empty;
         txtCriterio_EN.Text = string.Empty;
         chkActivo.Checked = true;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }
}