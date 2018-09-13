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

public partial class configuracion_frmPlantas : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmPlantas));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtienePlantas();
            ObtieneSucursales();
            txtColorP.Attributes.Add("readonly", "true");
        }
    }

    public void limpiacampos()
    {
        txtNombreCorto.Text = string.Empty;
        txtPlanta.Text = string.Empty;
        txtColorP.Text = string.Empty;
        ddlSucursal.SelectedIndex = 0;
        chkActivo.Checked = false;
    }

    public void ObtieneSucursales()
    {
        try
        {
            
            ddlSucursal.DataSource = dataaccess.executeStoreProcedureDataTable("[procObtieneSucursalesActivos]", null);
            ddlSucursal.DataTextField = "Nombre";
            ddlSucursal.DataValueField = "idSucursal";
            ddlSucursal.DataBind();
            
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    public void ObtienePlantas()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtienePlantas", null);
            GvPlantas.DataSource = dt;
            GvPlantas.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    protected void GvPlantas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != GvPlantas.SelectedPersistedDataKey)
            {
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["IdPlanta"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["IdPlanta"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@IdPlanta", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtienePlantaPorIdPlanta", parameters);

            txtPlanta.Text = dt.Rows[0]["NombrePlanta"].ToString();
            txtNombreCorto.Text = dt.Rows[0]["NombreCorto"].ToString();
            ddlSucursal.SelectedValue = dt.Rows[0]["idSucursal"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            this.txtColorP.Text = this.GvPlantas.SelectedRow.Cells[4].Text;
            

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoCargar").ToString(),Comun.MESSAGE_TYPE.Error);
        }
    }

    protected void GvPlantas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string hex = "#" + e.Row.Cells[4].Text;
            Color myColor = System.Drawing.ColorTranslator.FromHtml(hex);


            //Color myColor = ColorTranslator.FromHtml(e.Row.Cells[4].Text);
            e.Row.Cells[4].BackColor = myColor;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(GvPlantas, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void GvPlantas_PreRender(object sender, EventArgs e)
    {
        if (GvPlantas.HeaderRow != null)
            GvPlantas.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
   
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        
        try
        {
            if (txtColorP.Text.Trim().Equals("") || txtNombreCorto.Text.Trim().Equals("") || txtPlanta.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idPlanta", hdnIdPlanta.Value);
                parameters.Add("@nombrePlanta", txtPlanta.Text);
                parameters.Add("@activo", chkActivo.Checked.ToString());
                parameters.Add("@hexColor", txtColorP.Text);
                parameters.Add("@idSucursal", ddlSucursal.SelectedValue);
                parameters.Add("@NombreCorto", txtNombreCorto.Text);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());

                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaPlantaABC", parameters);

                if (Convert.ToInt32(result.Rows[0]["ID"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtienePlantas();
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
}