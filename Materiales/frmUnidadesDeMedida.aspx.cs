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

    protected void gvUnidadMedida_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvUnidadMedida.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvUnidadMedida.SelectedPersistedDataKey["IdUnidadMedida"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvUnidadMedida.SelectedDataKey["IdUnidadMedida"].ToString(), out id);
            }

            parameters.Add("@id", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneUnidadMedidaEdit", parameters);

            txtUnidadMedida.Text = dt.Rows[0]["Nombre"].ToString();
            txtUnidadMedida_EN.Text = dt.Rows[0]["Nombre_EN"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            txtDescripcion.Text = dt.Rows[0]["Descripcion"].ToString();
            txtDescription_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
            hdnIdUnidadMedida.Value = dt.Rows[0]["IdUnidadMedida"].ToString();

            btnClear.Text = GetLocalResourceObject("Cancelar").ToString();
            btnSave.Text = GetLocalResourceObject("Actualizar").ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
    protected void gvUnidadMedida_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUnidadMedida, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void gvUnidadMedida_PreRender(object sender, EventArgs e)
    {
        if (gvUnidadMedida.HeaderRow != null)
            gvUnidadMedida.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    public void llenaTabla()
    {
        try
        {
            gvUnidadMedida.DataSource = dataaccess.executeStoreProcedureDataTable("spr_GetUnidadesMedidaGv", null);
            gvUnidadMedida.DataBind();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    public void limpiaCampos()
    {
        txtDescripcion.Text = string.Empty;
        txtDescription_EN.Text = string.Empty;
        txtUnidadMedida.Text = string.Empty;
        txtUnidadMedida_EN.Text = string.Empty;
        hdnIdUnidadMedida.Value = string.Empty;
        chkActivo.Checked = true;
        btnClear.Text = GetLocalResourceObject("Limpiar").ToString();
        btnSave.Text = GetLocalResourceObject("Guardar").ToString();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string result = "";
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            Parameters.Add("@id", hdnIdUnidadMedida.Value);
            Parameters.Add("@Nombre", txtUnidadMedida.Text);
            Parameters.Add("@Nombre_EN", txtUnidadMedida_EN.Text);
            Parameters.Add("@Descripcion", txtDescripcion.Text);
            Parameters.Add("@Descripcion_EN", txtDescription_EN.Text);
            Parameters.Add("@Activo", chkActivo.Checked.ToString());
            Parameters.Add("@Usuario", Session["IdUsuario"].ToString());

            result = dataaccess.executeStoreProcedureString("spr_GuardaUnidadMedida",Parameters);

            if (result != null && result.Trim().Length > 0)
            {
                if (result.Equals("Error"))
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Error);
                else if (result.Equals("Existe"))
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Error);
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Info);
                    limpiaCampos();
                    llenaTabla();
                }

            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(result).ToString(), Comun.MESSAGE_TYPE.Info);
            }


            
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiaCampos();
      
    }
}