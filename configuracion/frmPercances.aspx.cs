using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Globalization;

public partial class configuracion_frmPercances : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            limpiacampos();
            LlenaCatPercances();
            llenaTabla();
        }
    }

    public void LlenaCatPercances()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("[spr_ObtieneCategoriaPercancesCorrecto]",null);

            ddlCatPercance.Items.Insert(0, new ListItem("--Seleccione--", string.Empty));
            foreach (DataRow r in dt.Rows)
            {
                ddlCatPercance.Items.Add(new ListItem(r["Nombre"].ToString(), r["idCategoriaPercance"].ToString()));

            }
            ddlCatPercance.SelectedIndex = 0;
        }
        catch (Exception exs)
        {
            Log.Error(exs.Message);
        }
    }

    public void llenaTabla()
    {
        try
        {
            gvEtapas.DataSource = dataaccess.executeStoreProcedureDataTable("spr_PercancesGv", new Dictionary<string, object>() { { "@idioma", getIdioma() } });
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
                Int32.TryParse(gvEtapas.SelectedPersistedDataKey["idPercance"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvEtapas.SelectedDataKey["idPercance"].ToString(), out id);
            }

            parameters.Add("@idPercance", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_PercancesGv", parameters);

            hddIdPercance.Value = dt.Rows[0]["idPercance"].ToString();
            txtMin.Text = dt.Rows[0]["Minimo"].ToString();
            txtMax.Text = dt.Rows[0]["Maximo"].ToString();
            txtPercance.Text = dt.Rows[0]["Percance"].ToString();
            txtPercance_EN.Text = dt.Rows[0]["Percance_EN"].ToString();
            txtDescripcion_ES.Text = dt.Rows[0]["Descripcion"].ToString();
            txtDescripcion_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            btnClear.Text = GetGlobalResourceObject("Commun", "Cancelar").ToString();
            btnSave.Text = GetGlobalResourceObject("Commun", "Actualizar").ToString();

            if (ddlCatPercance.Items.FindByValue(dt.Rows[0]["idCategoriaPercance"].ToString()) != null)
            {
                ddlCatPercance.SelectedValue = dt.Rows[0]["idCategoriaPercance"].ToString();
            }
            else
            {
                ddlCatPercance.SelectedIndex = 0;
            }
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
            parameters.Add("@idPercance", hddIdPercance.Value);
            parameters.Add("@Percance", txtPercance.Text);
            parameters.Add("@Percance_EN", txtPercance_EN.Text);
            parameters.Add("@Descripcion", txtDescripcion_ES.Text);
            parameters.Add("@Descripcion_EN", txtDescripcion_EN.Text);
            parameters.Add("@Activo", chkActivo.Checked);
            parameters.Add("@UsuarioModifico", Session["idUsuario"].ToString());
            parameters.Add("@idCategoriaPercance", ddlCatPercance.SelectedValue);
            parameters.Add("@minimo", txtMin.Text);
            parameters.Add("@maximo", txtMax.Text);

            if (txtMin.CssClass.Contains("Error")) 
            {
                popUpMessageControl1.setAndShowInfoMessage("Verique los intervalos.", Comun.MESSAGE_TYPE.Warning);
            }
            else
            {

                result = dataaccess.executeStoreProcedureDataTable("spr_PercancesGuardar", parameters);

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
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }
    }

    public void limpiacampos()
    {
         hddIdPercance.Value = string.Empty;
         txtPercance.Text = string.Empty;
         txtDescripcion_EN.Text = string.Empty;
         txtDescripcion_ES.Text = string.Empty;
         txtPercance_EN.Text = string.Empty;
         txtMin.Text = string.Empty;
         txtMax.Text = string.Empty;
         chkActivo.Checked = true;
         ddlCatPercance.SelectedIndex = 0;

         btnClear.Text = GetGlobalResourceObject("Commun", "Limpiar").ToString();
         btnSave.Text = GetGlobalResourceObject("Commun", "Guardar").ToString();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }
    protected void txtMin_TextChanged(object sender, EventArgs e)
    {
        if (txtMin.Text != "" && txtMax.Text != "")
        {
            if (Convert.ToInt32(txtMin.Text) > Convert.ToInt32(txtMax.Text))
            {
                txtMin.CssClass = "Error help";
                txtMin.ToolTip = "este intervalo debe ser menor";
                txtMax.CssClass = "Error help";
                txtMax.ToolTip = "este intervalo debe ser mayor";
            }
            else 
            {
                txtMin.CssClass = "";
                txtMin.ToolTip = "";
                txtMax.CssClass = "";
                txtMax.ToolTip = "";
            }
        }
    }
    protected void txtMax_TextChanged(object sender, EventArgs e)
    {
        if (txtMin.Text != "" && txtMax.Text != "")
        {
            if (Convert.ToInt32(txtMin.Text) > Convert.ToInt32(txtMax.Text))
            {
                txtMin.CssClass = "Error help";
                txtMin.ToolTip = "este intervalo debe ser menor";
                txtMax.CssClass = "Error help";
                txtMax.ToolTip = "este intervalo debe ser mayor";
            }
            else
            {
                txtMin.CssClass = "";
                txtMin.ToolTip = "";
                txtMax.CssClass = "";
                txtMax.ToolTip = "";
            }
        }
    }
}