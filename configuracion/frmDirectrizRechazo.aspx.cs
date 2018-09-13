using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class configuracion_frmDirectrizRechazo : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            limpiacampos();
            llenaTabla();
        }
    }

    public void llenaTabla()
    {
        try
        {
            gvRechazos.DataSource = dataaccess.executeStoreProcedureDataTable("spr_DirectrizRechazosGv", new Dictionary<string, object>() { { "@idioma",  getIdioma() }});
            gvRechazos.DataBind();
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }

    }
    protected void gvRechazos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvRechazos.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvRechazos.SelectedPersistedDataKey["idDirectrizRechazo"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvRechazos.SelectedDataKey["idDirectrizRechazo"].ToString(), out id);
            }

            parameters.Add("@idRechazo", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_DirectrizRechazosGv", parameters);

            hddIdRechazo.Value = dt.Rows[0]["idDirectrizRechazo"].ToString();
            txtRechazo.Text = dt.Rows[0]["Rechazo"].ToString();
            txtRechazo_EN.Text = dt.Rows[0]["Rechazo_EN"].ToString();
            txtDescripcion_ES.Text = dt.Rows[0]["Descripcion"].ToString();
            txtDescripcion_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;

            btnClear.Text = GetGlobalResourceObject("Commun", "Cancelar").ToString();
            btnSave.Text = GetGlobalResourceObject("Commun", "Actualizar").ToString();
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.Message);
        }

    }
    protected void gvRechazos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvRechazos, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void gvRechazos_PreRender(object sender, EventArgs e)
    {
        if (gvRechazos.HeaderRow != null)
            gvRechazos.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void  btnSave_Click(object sender, EventArgs e)
    {
        DataTable result;
        try
        {
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("@idioma", getIdioma());
            parameters.Add("@idRechazo", hddIdRechazo.Value);
            parameters.Add("@Rechazo", txtRechazo.Text);
            parameters.Add("@Rechazo_EN", txtRechazo_EN.Text);
            parameters.Add("@Descripcion", txtDescripcion_ES.Text);
            parameters.Add("@Descripcion_EN", txtDescripcion_EN.Text);
            parameters.Add("@Activo", chkActivo.Checked);
            parameters.Add("@UsuarioModifico", Session["idUsuario"].ToString());

            result = dataaccess.executeStoreProcedureDataTable("spr_DirectrizRechazoGuardar", parameters);

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
         hddIdRechazo.Value = string.Empty;
         txtRechazo.Text = string.Empty;
         txtDescripcion_EN.Text = string.Empty;
         txtDescripcion_ES.Text = string.Empty;
         txtRechazo_EN.Text = string.Empty;
         chkActivo.Checked = true;
         btnClear.Text = GetGlobalResourceObject("Commun", "Limpiar").ToString();
         btnSave.Text = GetGlobalResourceObject("Commun", "Guardar").ToString();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }
}