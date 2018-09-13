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

public partial class configuracion_frmCreacionCiclos : BasePage 
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmCreacionCiclos));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ObtieneCiclos(Session["idUsuario"].ToString());
            ObtieneRanchosPorUsuario(Session["idUsuario"].ToString());
            ObtieneInvernaderosPorIdRancho(ddlRancho.SelectedValue);
            ObtieneProductos();
            ObtieneVariedadesPorProducto(ddlProducto.SelectedValue);
            ObtieneVariables();

        }
    }
    public void ObtieneCiclos(string idUsuario)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idUsuario", idUsuario);

            DataSet ds = dataaccess.executeStoreProcedureDataSet("procObtieneCiclos", parameters);
            GvPlantas.DataSource = ds.Tables[0];
            GvPlantas.DataBind();

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    

    public void ObtieneVariables()
    {

        ddlVariables.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtieneVariablesActivas", null);
        ddlVariables.DataTextField = "Descripcion";
        ddlVariables.DataValueField = "idVariable";
        ddlVariables.DataBind();
    }
    public void ObtieneProductos()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idPais", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        ddlProducto.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtieneProductosActivos", parameters);
        ddlProducto.DataTextField = "Producto";
        ddlProducto.DataValueField = "idProduct";
        ddlProducto.DataBind();
    }
      public void ObtieneVariedadesPorProducto(string idProducto)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idProduct", idProducto);
        parameters.Add("@Pais", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        ddlVariedad.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtieneVariedadesPorProducto", parameters);
        ddlVariedad.DataTextField = "Variety";
        ddlVariedad.DataValueField = "idVariety";
        ddlVariedad.DataBind();
    }
   

    public void ObtieneRanchosPorUsuario(string idUsuario)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idUsuario", idUsuario);

            ddlRancho.DataSource = dataaccess.executeStoreProcedureDataTableFill("spr_ObtenerPlantaPorUsuario", parameters);
            ddlRancho.DataTextField = "NombrePlanta";
            ddlRancho.DataValueField = "idPlanta";
            ddlRancho.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    public void ObtieneInvernaderosPorIdRancho(string idRancho)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPlanta", idRancho);

            ddlInvernadero.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtieneInvernaderosActivosPorRancho", parameters);
            ddlInvernadero.DataTextField = "Invernadero";
            ddlInvernadero.DataValueField = "idInvernadero";
            ddlInvernadero.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    protected void ddlRancho_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneInvernaderosPorIdRancho(ddlRancho.SelectedValue);
    }
    protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneVariedadesPorProducto(ddlProducto.SelectedValue);
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idCycle"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idCycle"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idCycle", hdnIdPlanta.Value);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneCicloIdCiclo", parameters);

            lblClaveCiclo.Text = dt.Rows[0]["cycle"].ToString();
            ddlRancho.SelectedValue = dt.Rows[0]["farm"].ToString();
            ObtieneInvernaderosPorIdRancho(ddlRancho.SelectedValue);
            ddlInvernadero.SelectedValue = dt.Rows[0]["idInvernadero"].ToString();
            ddlProducto.SelectedValue = dt.Rows[0]["idProduct"].ToString();
            ObtieneVariedadesPorProducto(ddlProducto.SelectedValue);
            ddlVariedad.SelectedValue = dt.Rows[0]["idVariety"].ToString();
            ddlVariables.SelectedValue = dt.Rows[0]["Variable"].ToString();
            txtPlantDate.Text = Convert.ToDateTime(dt.Rows[0]["plantDate"]).ToString("yyyy-MM-dd");
            txtHarvestDate.Text = Convert.ToDateTime(dt.Rows[0]["firstHarvest"]).ToString("yyyy-MM-dd");
            txtLastHarvestDate.Text = Convert.ToDateTime(dt.Rows[0]["lastHarvest"]).ToString("yyyy-MM-dd");
            txtNumeroCabezas.Text = dt.Rows[0]["headNumber"].ToString();
            chkAbojorros.Checked = (bool)dt.Rows[0]["Abejorro"];
            chkComplete0.Checked = (bool)dt.Rows[0]["complete"];
            chkInjertado.Checked = (bool)dt.Rows[0]["complete"];
            //ddlSucursal.SelectedValue = dt.Rows[0]["sucursal"].ToString();
            //ddlRancho.SelectedValue = dt.Rows[0]["idPlanta"].ToString();
            //txtClave.Text = dt.Rows[0]["zona"].ToString();
            //lblClaveInvernadero.Text = dt.Rows[0]["invernadero"].ToString();
            //txtNoInvernadero.Text = dt.Rows[0]["noInvernadero"].ToString();
            //txtGrupo.Text = dt.Rows[0]["grupo"].ToString();
            //txtHectarea.Text = dt.Rows[0]["hectarea"].ToString();
            //txtSecciones.Text = dt.Rows[0]["secciones"].ToString();
            //txtSurcos.Text = dt.Rows[0]["surcos"].ToString();
            //chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "SI" ? true : false;
            //chkZonificado.Checked = dt.Rows[0]["zonificacion"].ToString() == "SI" ? true : false;
            //chkInvestigacion.Checked = dt.Rows[0]["investigacion"].ToString() == "SI" ? true : false;
            //chkPasillo.Checked = dt.Rows[0]["pasilloMedio"].ToString() == "SI" ? true : false;

            //this.txtColorP.Text = this.GvPlantas.SelectedRow.Cells[4].Text;


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
    public void LimpiaCampos()
    {
        txtNumeroCabezas.Text = string.Empty;
        txtPlantDate.Text = string.Empty;
        txtHarvestDate.Text = string.Empty;
        txtLastHarvestDate.Text = string.Empty;
        chkAbojorros.Checked = false;
        chkComplete0.Checked = false;
        chkInjertado.Checked = false;
        ddlRancho.SelectedIndex = 0;
        ObtieneInvernaderosPorIdRancho(ddlRancho.SelectedValue);
        ddlProducto.SelectedIndex = 0;
        ObtieneVariedadesPorProducto(ddlProducto.SelectedValue);
        ddlVariables.SelectedIndex = 0;
        lblClaveCiclo.Text = string.Empty;
        hdnIdPlanta.Value = "0";
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        LimpiaCampos();
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtNumeroCabezas.Text.Trim().Equals("") || txtHarvestDate.Text.Trim().Equals("") || txtLastHarvestDate.Text.Trim().Equals("") || txtPlantDate.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idCycle", hdnIdPlanta.Value);
                parameters.Add("@cycle", ddlRancho.SelectedValue);
                parameters.Add("@greenhouse", ddlInvernadero.SelectedItem.Text);
                parameters.Add("@idInvernadero", ddlInvernadero.SelectedValue);
                parameters.Add("@farm", ddlRancho.SelectedValue);
                parameters.Add("@product", ddlProducto.SelectedItem.Text);
                parameters.Add("@plantDate", txtPlantDate.Text);
                parameters.Add("@grafted", chkInjertado.Checked);
                parameters.Add("@firstHarvest", txtHarvestDate.Text);
                parameters.Add("@lartHarvest", txtLastHarvestDate.Text);
                parameters.Add("@variety", ddlVariedad.SelectedItem.Text);
                parameters.Add("@headNumber",txtNumeroCabezas.Text);
                parameters.Add("@complete", chkComplete0.Checked);
                parameters.Add("@abejorro", chkAbojorros.Checked);
                parameters.Add("@variable", ddlVariables.SelectedValue);



                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaCiclos", parameters);

                if (Convert.ToInt32(result.Rows[0]["RESULTADO"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["MENSAJE"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtieneCiclos(Session["idUsuario"].ToString());
                    LimpiaCampos();

                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["MENSAJE"].ToString(), Comun.MESSAGE_TYPE.Info);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
}