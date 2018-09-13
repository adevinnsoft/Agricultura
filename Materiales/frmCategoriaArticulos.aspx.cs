using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Materiales_frmUnidadesDeMedida : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            llenaTabla();
        }
    }

    public void limpiaCampos()
    {
        txtCategoria.Text = string.Empty;
        txtCategoria_EN.Text = string.Empty;
        chkActivo.Checked = true;
        txtDescripcion.Text = string.Empty;
        txtDescription_EN.Text = string.Empty;
        hdnIdCategoria.Value = null;
    }

    public void llenaTabla()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneCategoriasArticulosGv",null);
            gvCategorias.DataSource = dt;
            gvCategorias.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    protected void gvCategorias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvCategorias.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvCategorias.SelectedPersistedDataKey["IdCategoriaArticulo"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvCategorias.SelectedDataKey["IdCategoriaArticulo"].ToString(), out id);
            }

            parameters.Add("@id", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneCategoriaArticuloEdit", parameters);

            txtCategoria.Text = dt.Rows[0]["NombreCategoria"].ToString();
            txtCategoria_EN.Text = dt.Rows[0]["NombreCategoria_EN"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            txtDescripcion.Text = dt.Rows[0]["Descripcion"].ToString();
            txtDescription_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
            hdnIdCategoria.Value = dt.Rows[0]["IdCategoriaArticulo"].ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

    }

    protected void gvCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCategorias, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }

    protected void gvCategorias_PreRender(object sender, EventArgs e)
    {
        if (gvCategorias.HeaderRow != null)
            gvCategorias.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        var result = "";
        try
        {
            parameters.Add("@IdCategoria", hdnIdCategoria.Value);
            parameters.Add("@Nombre", txtCategoria.Text);
            parameters.Add("@Nombre_EN", txtCategoria_EN.Text);
            parameters.Add("@Descripcion", txtDescripcion.Text);
            parameters.Add("@Descripcion_EN", txtDescription_EN.Text);
            parameters.Add("@Activo", chkActivo.Checked.ToString());
            parameters.Add("@Usuario", Session["idUsuario"].ToString());

            result = dataaccess.executeStoreProcedureString("spr_GuardaCategoriaArticulo", parameters);
            if (result != null && result.Trim().Length > 0)
            {
                if (result.Equals("Error"))
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Error);
                }
                if (result.Equals("Existe"))
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Error);
                }
                else
                {
                    limpiaCampos();
                    llenaTabla();
                    
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Success);
                    
                }
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Error").ToString(), Comun.MESSAGE_TYPE.Success);
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Error").ToString(), Comun.MESSAGE_TYPE.Success);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiaCampos();
    }
}