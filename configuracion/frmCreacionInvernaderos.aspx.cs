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

public partial class configuracion_frmCreacionInvernaderos : BasePage
{

    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmCreacionInvernaderos));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            ObtieneSucursales();
            ObtienePlantas(Convert.ToInt32(ddlSucursal.SelectedValue));
            ObtieneInvernaderosPorRancho(Convert.ToInt32(ddlRancho.SelectedValue));
            
        }
    }
    public void ObtieneInvernaderosPorRancho(int idPlanta)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPlanta", idPlanta);
           
            DataSet ds = dataaccess.executeStoreProcedureDataSet("procObtieneInvernaderos", parameters);
            GvPlantas.DataSource = ds.Tables[0];
            GvPlantas.DataBind();
            lblConsecutivoInvernadero.Text = ds.Tables[1].Rows[0]["Consecutivo"].ToString();
            txtNoInvernadero.Text = ds.Tables[1].Rows[0]["Consecutivo"].ToString();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    public void limpiacampos()
    {
        txtGrupo.Text = string.Empty;
        txtNoInvernadero.Text = string.Empty;
        txtClave.Text = string.Empty;
        txtHectarea.Text = string.Empty;
        txtSecciones.Text = string.Empty;
        txtSurcos.Text = string.Empty;
        hdnIdPlanta.Value = "0";
        ObtieneInvernaderosPorRancho(Convert.ToInt32(ddlRancho.SelectedValue));
        chkActivo.Checked = false;
        chkZonificado.Checked = false;
        chkInvestigacion.Checked = false;
        chkPasillo.Checked = false;
        lblClaveInvernadero.Text = string.Empty;
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

    public void ObtienePlantas(int idSucursal)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idSucursal", idSucursal);
            ddlRancho.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtienePlantasActivasPorIdSucursal", parameters);
            ddlRancho.DataTextField = "NombrePlanta";
            ddlRancho.DataValueField = "idPlanta";
            ddlRancho.DataBind();
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idInvernadero"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idInvernadero"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idInvernadero", hdnIdPlanta.Value);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneInvernaderoIdInvernadero", parameters);

            ddlSucursal.SelectedValue = dt.Rows[0]["sucursal"].ToString();
            ddlRancho.SelectedValue = dt.Rows[0]["idPlanta"].ToString();
            txtClave.Text = dt.Rows[0]["zona"].ToString();
            lblClaveInvernadero.Text = dt.Rows[0]["invernadero"].ToString();
            txtNoInvernadero.Text = dt.Rows[0]["noInvernadero"].ToString();
            txtGrupo.Text = dt.Rows[0]["grupo"].ToString();
            txtHectarea.Text = dt.Rows[0]["hectarea"].ToString();
            txtSecciones.Text = dt.Rows[0]["secciones"].ToString();
            txtSurcos.Text = dt.Rows[0]["surcos"].ToString();
            txtIdAgroSmart.Text = dt.Rows[0]["idInvernaderoAGROSMART"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "SI" ? true : false;
            chkZonificado.Checked = dt.Rows[0]["zonificacion"].ToString() == "SI" ? true : false;
            chkInvestigacion.Checked = dt.Rows[0]["investigacion"].ToString() == "SI" ? true : false;
            chkPasillo.Checked = dt.Rows[0]["pasilloMedio"].ToString() == "SI" ? true : false;

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

    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        try
        {
            if (txtClave.Text.Trim().Equals("") || txtGrupo.Text.Trim().Equals("") || txtHectarea.Text.Trim().Equals("") || txtSecciones.Text.Trim().Equals("") || txtSurcos.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idInvernadero", hdnIdPlanta.Value);
                parameters.Add("@idPlanta", ddlRancho.SelectedValue);
                parameters.Add("@hectarea", txtHectarea.Text);
                 parameters.Add("@zona", txtClave.Text.Trim());
                 parameters.Add("@grupo", txtGrupo.Text.Trim());
                 parameters.Add("@noInvernadero", txtNoInvernadero.Text);
                 parameters.Add("@secciones", txtSecciones.Text);
                 parameters.Add("@surcos", txtSurcos.Text);
                parameters.Add("@zonificado", chkZonificado.Checked?1:0);
                parameters.Add("@investigacion", chkInvestigacion.Checked?1:0);
                parameters.Add("@pasilloMedio", chkPasillo.Checked?1:0);
                parameters.Add("@activo", chkActivo.Checked? 1:0);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());
                parameters.Add("@idInvAgroSmart", txtIdAgroSmart.Text);



                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaInvernaderosABC", parameters);

                if (Convert.ToInt32(result.Rows[0]["ID"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                    //ObtienePlantas();
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
    protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtienePlantas(Convert.ToInt32(ddlSucursal.SelectedValue));
        if (ddlRancho.Items.Count > 0)
        {
            ObtieneInvernaderosPorRancho(Convert.ToInt32(ddlRancho.SelectedValue));
        }
        else
        {
            lblConsecutivoInvernadero.Text = string.Empty;
        }
    }
    protected void ddlRancho_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneInvernaderosPorRancho(Convert.ToInt32(ddlRancho.SelectedValue));
    }
}