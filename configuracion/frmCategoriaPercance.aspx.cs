using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

public partial class configuracion_frmCategoriaPercance : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                llenaGridView();
                limpiarCampos();
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.ToString());
        }
    }
    protected void gvCategoriaPercances_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsCategoria"])
            {
                DataSet ds = ViewState["dsCategoria"] as DataSet;

                if (ds != null)
                {
                    gvCategoriaPercances.DataSource = ds;
                    gvCategoriaPercances.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception)
        {
        }
    }
    protected void gvCategoriaPercances_SelectedIndexChanged(object sender, EventArgs e)
    {
        limpiarCampos();
        Session["IdModuloCookie"] = gvCategoriaPercances.DataKeys[gvCategoriaPercances.SelectedIndex].Value;
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idCategoriaPercance", Session["IdModuloCookie"]);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneCategoriaPercance", parameters);
         if (dt != null && dt.Rows.Count > 0)
         {
             hiddenIdCategoriaPercance.Value = dt.Rows[0]["IdCategoriaPercance"].ToString();
             txtNombre.Text = dt.Rows[0]["Nombre"].ToString();
             txtNombre_EN.Text = dt.Rows[0]["Nombre_EN"].ToString();
             txtDescripcion.Text = dt.Rows[0]["Descripcion"].ToString();
             txtDescripcion_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
             ckActivo.Checked = Boolean.Parse(dt.Rows[0]["Activo"].ToString());

             btnClear.Text = GetGlobalResourceObject("commun", "Cancelar").ToString();
             btnSave.Text = GetGlobalResourceObject("commun", "Actualizar").ToString();
         }
    }
    protected void gvCategoriaPercances_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCategoriaPercances, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void gvCategoriaPercances_PreRender(object sender, EventArgs e)
    {
        if (gvCategoriaPercances.HeaderRow != null)
            gvCategoriaPercances.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiarCampos();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDescripcion.Text.Trim().Length > 0 && txtDescripcion_EN.Text.Trim().Length > 0 && txtNombre.Text.Trim().Length > 0 && txtNombre_EN.Text.Trim().Length > 0)
            {
                int id = 0;
                Int32.TryParse(hiddenIdCategoriaPercance.Value, out id);
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idCategoriaPercance",id);
	            parameters.Add("@Nombre",txtNombre.Text);
	            parameters.Add("@Nombre_EN", txtNombre_EN.Text);
	            parameters.Add("@Descripcion",txtDescripcion.Text);
	            parameters.Add("@Descripcion_EN",txtDescripcion_EN.Text);
	            parameters.Add("@Activo",ckActivo.Checked);
                parameters.Add("@UsuarioModifico", Session["idUsuario"]);

                DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaCategoriaPercance", parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    id = 0;
                    Int32.TryParse(dt.Rows[0][0].ToString(), out id);

                    if (id > 0)
                    {
                        limpiarCampos();
                        llenaGridView();
                        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Guardado").ToString(), Comun.MESSAGE_TYPE.Success);
                    }
                    else
                    {
                        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoGuardado").ToString(), Comun.MESSAGE_TYPE.Error);
                    }
                }
            } 
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("CamposPorLlenar").ToString(), Comun.MESSAGE_TYPE.Warning);
            }

        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ErrorGuardado").ToString(), Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }
    }

    private void limpiarCampos()
    {
        txtDescripcion.Text = "";
        txtDescripcion_EN.Text = "";
        txtNombre.Text = "";
        txtNombre_EN.Text = "";
        ckActivo.Checked = true;
        hiddenIdCategoriaPercance.Value = "";
        btnClear.Text = GetGlobalResourceObject("commun", "Limpiar").ToString();
        btnSave.Text = GetGlobalResourceObject("commun", "Guardar").ToString();
    }

    private void llenaGridView()
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            parameters.Add("@SoloActivos", 0);
            DataTable dt = dataaccess.executeStoreProcedureDataTable("[spr_ObtieneCategoriaPercancesCorrecto]", null);
            ViewState["dsCategoria"] = dt;
            gvCategoriaPercances.DataSource = dt;
            gvCategoriaPercances.DataBind();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

}