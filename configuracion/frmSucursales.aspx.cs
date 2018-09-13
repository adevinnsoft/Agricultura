using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Drawing;
using log4net;

public partial class configuracion_frmSucursales : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmSucursales));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtieneSucursales();
            ObtienePaises();
  
        }
    }

    public void limpiacampos()
    {
        txtNombre.Text = string.Empty;
        ddlPais.SelectedIndex = 0;
        chkActivo.Checked = false;
        hdnIdPlanta.Value = "0";
    }

    public void ObtienePaises()
    {
        try
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            ddlPais.DataSource = dataaccess.executeStoreProcedureDataTable("[spr_ObtienePaises]", parameters);
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataValueField = "idPais";
            ddlPais.DataBind();

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    public void ObtieneSucursales()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneSucursales", null);
            GvSucursales.DataSource = dt;
            GvSucursales.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    protected void GvSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != GvSucursales.SelectedPersistedDataKey)
            {
                Int32.TryParse(GvSucursales.SelectedPersistedDataKey["idSucursal"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvSucursales.SelectedDataKey["idSucursal"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idSucursal", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneSucursalIdSucursal", parameters);

            txtNombre.Text = dt.Rows[0]["nombre"].ToString();
            ddlPais.SelectedValue = dt.Rows[0]["idPais"].ToString();
            chkActivo.Checked = dt.Rows[0]["Estatus"].ToString() == "True" ? true : false;


        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoCargar").ToString(), Comun.MESSAGE_TYPE.Error);
        }
    }

    protected void GvSucursales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(GvSucursales, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void GvSucursales_PreRender(object sender, EventArgs e)
    {
        if (GvSucursales.HeaderRow != null)
            GvSucursales.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        try
        {
            if (txtNombre.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idSucursal", hdnIdPlanta.Value);
                parameters.Add("@nombre", txtNombre.Text);
                parameters.Add("@estatus", chkActivo.Checked.ToString());
                parameters.Add("@idPais", ddlPais.SelectedValue);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());

                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaSucursalABC", parameters);

                if (Convert.ToInt32(result.Rows[0]["ID"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtieneSucursales();
                    limpiacampos();

                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos();
    }
}