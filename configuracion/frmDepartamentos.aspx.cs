using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;
using System.Globalization;

public partial class configuracion_frmDepartamentos : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmDepartamentos));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ObtieneDepartamentos();
                

            }
        }
        catch (Exception exception)
        {
            log.Error(exception.ToString());
        }
    }
    private void ObtieneDepartamentos()
    {
        Dictionary<string, object> par = new Dictionary<string, object>();
        par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneDepartamentos", null);
        gv_ZonaMonitor.DataSource = dt;
        ViewState["dsDepartamento"] = dt;
        gv_ZonaMonitor.DataBind();
    }
    protected void gv_ZonaMonitor_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id;
        DataTable dt;

        if (null != gv_ZonaMonitor.SelectedPersistedDataKey)
        {
            Int32.TryParse(gv_ZonaMonitor.SelectedPersistedDataKey["idDepartamento"].ToString(), out id);
        }
        else
        {
            Int32.TryParse(gv_ZonaMonitor.SelectedDataKey["idDepartamento"].ToString(), out id);
        }
        hdnIdDepartamento.Value = id.ToString();

        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idDepartamento", id);

        dt = dataaccess.executeStoreProcedureDataTable("procObtieneDepartamentoPorIdDepartamento", parameters);
        if (dt.Rows.Count > 0)
        {
            txtDepartametno.Text = dt.Rows[0]["NombreDepartamento"].ToString().Trim();
            txtDepartmento_EN.Text = dt.Rows[0]["NombreDepartamento_EN"].ToString().Trim();
            //txtRuta.Text = dt.Rows[0]["ruta"].ToString().Trim();
            if (dt.Rows[0]["Activo"].ToString().Equals("True"))
                cbxActivo.Checked = true;
            else
                cbxActivo.Checked = false;
        }
       
    }
    protected void gv_ZonaMonitor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsDepartamento"])
            {
                DataSet ds = ViewState["dsDepartamento"] as DataSet;

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
        if (txtDepartametno.Text.Trim().Equals("") || txtDepartmento_EN.Text.Trim().Equals(""))
        {
            popUpMessageControl1.setAndShowInfoMessage("El Departamento es requerido.", Comun.MESSAGE_TYPE.Error);
        }
        else
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@idDepartamento", hdnIdDepartamento.Value);
            parameters.Add("@NombreDepartamento", txtDepartametno.Text);
            parameters.Add("@NombreDepartamento_EN", txtDepartmento_EN.Text);

            parameters.Add("@idUsuario", Session["idUsuario"]);
            if (cbxActivo.Checked)
                parameters.Add("@Activo", 1);
            else
                parameters.Add("@Activo", 0);

            DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaDepartamentoABC", parameters);


            if (Convert.ToInt32(result.Rows[0]["Resultado"]) > 0)
            {
                popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Mensaje"].ToString(), Comun.MESSAGE_TYPE.Success);
                ObtieneDepartamentos();
                LimpiaCampos();

            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Mensaje"].ToString(), Comun.MESSAGE_TYPE.Success);
            }

            //gv_Merma.DataSource = da.executeStoreProcedureDataTable("spr_MermaObtener", new Dictionary<string, object>());
           
            //VolverAlPanelInicial();
        }
  
    }
    public void LimpiaCampos()
    {
        txtDepartmento_EN.Text = "";
        txtDepartametno.Text = "";
        cbxActivo.Checked = false;
        hdnIdDepartamento.Value = "0";
      
    }
}