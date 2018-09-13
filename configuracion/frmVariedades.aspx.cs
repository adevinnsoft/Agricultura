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

public partial class configuracion_frmVariedades : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmVariedades));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtieneVariedades();
            ObtienePaises();
            ObtieneProductos(Convert.ToInt32(ddlPais.SelectedValue));
            txtColorP.Attributes.Add("readonly", "true");
        }
    }

    public void limpiacampos()
    {
        txtDescripcion.Text = string.Empty;
        txtVariedad.Text = string.Empty;
        txtColorP.Text = string.Empty;
        ddlPais.SelectedIndex = 0;
        ObtieneProductos(Convert.ToInt32(ddlPais.SelectedValue));
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

    public void ObtieneVariedades()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneVariedades", null);
            GvPlantas.DataSource = dt;
            GvPlantas.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    public void ObtieneProductos(int idPais)
    {
        try
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@idPais", idPais);
            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneProductosActivos", parameters);
            ddlProducto.DataValueField = "idProduct";
            ddlProducto.DataTextField = "Producto";
            ddlProducto.DataSource = dt;
            ddlProducto.DataBind();
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idVariety"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idVariety"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idVariedad", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneVariedadPorIdVariedad", parameters);

            txtVariedad.Text = dt.Rows[0]["Variety"].ToString();
            txtDescripcion.Text = dt.Rows[0]["Description"].ToString();
            ddlPais.SelectedValue = dt.Rows[0]["Pais"].ToString();
            ObtieneProductos(Convert.ToInt32(ddlPais.SelectedValue));
            ddlProducto.SelectedValue = dt.Rows[0]["idProduct"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "1" ? true : false;
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
            if (txtColorP.Text.Trim().Equals("") || txtVariedad.Text.Trim().Equals("") || txtDescripcion.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idVariety", hdnIdPlanta.Value);
                parameters.Add("@variedad", txtVariedad.Text);
                parameters.Add("@activo", chkActivo.Checked);
                parameters.Add("@hexColor", txtColorP.Text);
                parameters.Add("@idPais", ddlPais.SelectedValue);
                parameters.Add("@descripcion", txtDescripcion.Text);
                parameters.Add("@idProduct", ddlProducto.SelectedValue);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());

                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaVariedadABC", parameters);

                if (Convert.ToInt32(result.Rows[0]["ID"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtieneVariedades();
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
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        limpiacampos();
    }
}