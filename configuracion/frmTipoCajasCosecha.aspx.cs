using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;
using System.Globalization;
public partial class configuracion_frmTipoCajasCosecha :  BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmTipoCajasCosecha));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ObtieneTipoCajas();
                

            }
        }
        catch (Exception exception)
        {
            log.Error(exception.ToString());
        }
    }
    private void ObtieneTipoCajas()
    {
        Dictionary<string, object> par = new Dictionary<string, object>();
        par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneTipoCajasCosecha", null);
        gv_ZonaMonitor.DataSource = dt;
        ViewState["dsTipoCajas"] = dt;
        gv_ZonaMonitor.DataBind();
    }
    protected void gv_ZonaMonitor_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id;
        DataTable dt;

        if (null != gv_ZonaMonitor.SelectedPersistedDataKey)
        {
            Int32.TryParse(gv_ZonaMonitor.SelectedPersistedDataKey["idCajaTipo"].ToString(), out id);
        }
        else
        {
            Int32.TryParse(gv_ZonaMonitor.SelectedDataKey["idCajaTipo"].ToString(), out id);
        }
        hdnIdDepartamento.Value = id.ToString();

        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idCajaTipo", id);

        dt = dataaccess.executeStoreProcedureDataTable("procObtieneTipoCosechaIdCosecha", parameters);
        if (dt.Rows.Count > 0)
        {
            txtNombre.Text = dt.Rows[0]["nombre"].ToString().Trim();
            txtPesoProducto.Text = dt.Rows[0]["pesoProducto"].ToString().Trim();
            txtPesoTara.Text = dt.Rows[0]["pesoTara"].ToString().Trim();
            //txtRuta.Text = dt.Rows[0]["ruta"].ToString().Trim();
            if (dt.Rows[0]["Estatus"].ToString().Equals("True"))
                cbxActivo.Checked = true;
            else
                cbxActivo.Checked = false;
        }
       
    }
    protected void gv_ZonaMonitor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsTipoCajas"])
            {
                DataSet ds = ViewState["dsTipoCajas"] as DataSet;

                if (ds != null)
                {
                    gv_ZonaMonitor.DataSource = ds;
                    gv_ZonaMonitor.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception exception)
        {
            log.Error(exception.ToString());
        }
    }
    protected void gv_ZonaMonitor_PreRender(object sender, EventArgs e)
    {
        if (gv_ZonaMonitor.HeaderRow != null)
            gv_ZonaMonitor.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void gv_ZonaMonitor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gv_ZonaMonitor, ("Select$" + e.Row.RowIndex.ToString()));
                break;
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        LimpiaCampos();
    }

    protected void btn_Guardar_Click(object sender, EventArgs e)
    {
        if (txtNombre.Text.Trim().Equals("") || txtPesoProducto.Text.Trim().Equals("") || txtPesoTara.Text.Trim().Equals(""))
        {
            popUpMessageControl1.setAndShowInfoMessage("Favor de ingresar toda la información solictada.", Comun.MESSAGE_TYPE.Error);
        }
        else
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@idCajaTipo", hdnIdDepartamento.Value);
            parameters.Add("@nombre", txtNombre.Text);
            parameters.Add("@pesoProducto", txtPesoProducto.Text);
            parameters.Add("@pesoTara", txtPesoTara.Text);

            parameters.Add("@idUsuario", Session["idUsuario"]);
            if (cbxActivo.Checked)
                parameters.Add("@estatus", 1);
            else
                parameters.Add("@estatus", 0);

            DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaTipoCajaCosecha", parameters);

            if (result.Rows.Count > 0)
            {
                    if (Convert.ToInt32(result.Rows[0]["Resultado"]) > 0)
                    {
                        popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Mensaje"].ToString(), Comun.MESSAGE_TYPE.Success);
                        ObtieneTipoCajas();
                        LimpiaCampos();

                    }
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage("ERROR AL GUARDAR EL REGISTRO", Comun.MESSAGE_TYPE.Error);
            }

            //gv_Merma.DataSource = da.executeStoreProcedureDataTable("spr_MermaObtener", new Dictionary<string, object>());
           
            //VolverAlPanelInicial();
        }
  
    }
    public void LimpiaCampos()
    {
        txtNombre.Text = "";
        txtPesoProducto.Text = "";
        txtPesoTara.Text = "";
        cbxActivo.Checked = false;
        hdnIdDepartamento.Value = "0";
      
    }
}