using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class configuracion_frmEstadosInfestaciones : BasePage
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
            gvEstados.DataSource = dataaccess.executeStoreProcedureDataTable("spr_EstadosInfestacioGv", new Dictionary<string, object>() { { "@idioma", getIdioma() } });
            gvEstados.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }

    }
    protected void gvEstados_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvEstados.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvEstados.SelectedPersistedDataKey["idEstadoInfestacion"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvEstados.SelectedDataKey["idEstadoInfestacion"].ToString(), out id);
            }

            parameters.Add("@idEstado", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_EstadosInfestacioGv", parameters);

            hddIdEstado.Value = dt.Rows[0]["idEstadoInfestacion"].ToString();
            txtEstado.Text = dt.Rows[0]["Estado"].ToString();
            txtEstado_EN.Text = dt.Rows[0]["Estado_EN"].ToString();
            txtDescripcion_ES.Text = dt.Rows[0]["Descripcion"].ToString();
            txtDescripcion_EN.Text = dt.Rows[0]["Descripcion_EN"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;

            btnClear.Text = GetGlobalResourceObject("commun","Cancelar").ToString();
            btnSave.Text = GetGlobalResourceObject("commun", "Actualizar").ToString();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }

    }
    protected void gvEstados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEstados, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void gvEstados_PreRender(object sender, EventArgs e)
    {
        if (gvEstados.HeaderRow != null)
            gvEstados.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void  btnSave_Click(object sender, EventArgs e)
    {
        DataTable result;
        try
        {
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("@lengua", CultureInfo.CurrentCulture.Name);
            parameters.Add("@idEstado", hddIdEstado.Value);
            parameters.Add("@Estado", txtEstado.Text);
            parameters.Add("@Estado_EN", txtEstado_EN.Text);
            parameters.Add("@Descripcion", txtDescripcion_ES.Text);
            parameters.Add("@Descripcion_EN", txtDescripcion_EN.Text);
            parameters.Add("@Activo", chkActivo.Checked);
            parameters.Add("@UsuarioModifico", Session["idUsuario"].ToString());

            result = dataaccess.executeStoreProcedureDataTable("spr_EstadoInfestacionGuardar", parameters);

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
         //hdnIdEtapa.Value = string.Empty;
         txtEstado.Text = string.Empty;
         txtEstado_EN.Text = string.Empty;
         txtDescripcion_ES.Text = string.Empty;
         txtDescripcion_EN.Text = string.Empty;
         chkActivo.Checked = true;

         btnClear.Text = GetGlobalResourceObject("commun", "Limpiar").ToString();
         btnSave.Text = GetGlobalResourceObject("commun", "Guardar").ToString();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }
}