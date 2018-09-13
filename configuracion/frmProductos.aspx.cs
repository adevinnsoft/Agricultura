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

public partial class configuracion_frmProductos : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmProductos));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtieneProductos();
            ObtienePaises();
            txtColorP.Attributes.Add("readonly", "true");
        }
    }

    public void limpiacampos()
    {
        txtCodigo.Text = string.Empty;
        txtProducto.Text = string.Empty;
        txtColorP.Text = string.Empty;
        ddlPais.SelectedIndex = 0;
        chkActivo.Checked = false;
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

    public void ObtieneProductos()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneProductos", null);
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idProduct"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idProduct"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idProducto", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneProductoPorIdProducto", parameters);

            txtProducto.Text = dt.Rows[0]["Product"].ToString();
            txtCodigo.Text = dt.Rows[0]["Codigo"].ToString();
            txtDescripcion.Text = dt.Rows[0]["Description"].ToString();
            ddlPais.SelectedValue = dt.Rows[0]["Pais"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            this.txtColorP.Text = dt.Rows[0]["HexColor"].ToString();


        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoCargar").ToString(), Comun.MESSAGE_TYPE.Error);
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
            if (txtColorP.Text.Trim().Equals("") || txtCodigo.Text.Trim().Equals("") || txtProducto.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idProducto", hdnIdPlanta.Value);
                parameters.Add("@producto", txtProducto.Text);
                parameters.Add("@descripcion", txtDescripcion.Text);
                parameters.Add("@activo", chkActivo.Checked.ToString());
                parameters.Add("@hexColor", txtColorP.Text);
                parameters.Add("@pais", ddlPais.SelectedValue);
                parameters.Add("@codigo", txtCodigo.Text);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());

                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaProductoABC", parameters);

                if (Convert.ToInt32(result.Rows[0]["ID"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtieneProductos();
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