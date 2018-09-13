using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Web.Script.Serialization;

public partial class configuracion_frmCostoActividad : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            llenaPlantas();
           
            ObtieneUnidadMedida();
            ObtieneCostosActividad();
            ObtieneProductosPorPlanta(ddlPlanta.SelectedValue);
            ObtieneDepartamentosPorPlanta(ddlPlanta.SelectedValue);
            ObtieneActividadesPorPlantaDepartamento(ddlPlanta.SelectedValue, ddlDepartamento0.SelectedValue, ddlProducto.SelectedValue);
            ValidaActividadTipoCosecha(ddlActividad0.SelectedValue);
        }
    }
    private void llenaPlantas()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasDeUsuario", new Dictionary<string, object>() { 
                { "@iduser", Session["idUsuario"] } 
            });
            ddlPlanta.DataSource = dt;
            ddlPlanta.DataTextField = "Planta";
            ddlPlanta.DataValueField = "idPlanta";
            ddlPlanta.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
  
        
        
    }

    private void ObtieneProductosPorPlanta(string idPlanta)
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneProductosPorRancho", new Dictionary<string, object>() { 
                { "@idPlanta", idPlanta } 
            });
            ddlProducto.DataSource = dt;
            ddlProducto.DataTextField = "Product";
            ddlProducto.DataValueField = "idProduct";
            ddlProducto.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }

    private void ObtieneDepartamentosPorPlanta(string idPlanta)
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneDepartamentoHabilidadPorPlanta", new Dictionary<string, object>() { 
                { "@idPlanta", idPlanta } 
            });
            ddlDepartamento0.DataSource = dt;
            ddlDepartamento0.DataTextField = "NombreDepartamento";
            ddlDepartamento0.DataValueField = "idDepartamento";
            ddlDepartamento0.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }
    private void ObtieneUnidadMedida()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneUnidadMedidaActivo", null);
            ddlUnidad0.DataSource = dt;
            ddlUnidad0.DataTextField = "Nombre";
            ddlUnidad0.DataValueField = "idUnidadMedida";
            ddlUnidad0.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }
       private void ObtieneActividadesPorPlantaDepartamento(string idPlanta, string idDepartamento, string idProducto)
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneHabilidadPorPlantaPorProducto", new Dictionary<string, object>() { 
                { "@idPlanta", idPlanta },  { "@idDepartamento", idDepartamento }, { "@idProducto", idProducto } 
            });
            ddlActividad0.DataSource = dt;
            ddlActividad0.DataTextField = "NombreHabilidad";
            ddlActividad0.DataValueField = "idHabilidad";
            ddlActividad0.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }

   
    protected void ddlDepartamento0_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtPlantasSeleccionadas = (DataTable)Session["idPlantasSeleccionadas"];
        ObtieneActividadesPorPlantaDepartamento(ddlPlanta.SelectedValue, ddlDepartamento0.SelectedValue,ddlProducto.SelectedValue);
        ValidaActividadTipoCosecha(ddlActividad0.SelectedValue);
    }
  
    protected void ddlPlanta_SelectedIndexChanged1(object sender, EventArgs e)
    {

        ObtieneProductosPorPlanta(ddlPlanta.SelectedValue);
        ObtieneDepartamentosPorPlanta(ddlPlanta.SelectedValue);
        ObtieneActividadesPorPlantaDepartamento(ddlPlanta.SelectedValue, ddlDepartamento0.SelectedValue,ddlProducto.SelectedValue);
        ValidaActividadTipoCosecha(ddlActividad0.SelectedValue);
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCantidad0.Text.Trim().Equals("") || txtCosto.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idCostoActividad", hdnIdPlanta.Value);
                parameters.Add("@idPlanta", ddlPlanta.SelectedValue);
                parameters.Add("@idDepartamento", ddlDepartamento0.SelectedValue);
                parameters.Add("@idHabilidad", ddlActividad0.SelectedValue);
                parameters.Add("@cantidad", txtCantidad0.Text);
                parameters.Add("@idUnidadMedida", ddlUnidad0.SelectedValue);
                parameters.Add("@costo", txtCosto.Text);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());
                parameters.Add("@activo", chkActivo.Checked);
                parameters.Add("@idProducto", ddlProducto.SelectedValue);

                if (ddlTipoCosecha.Items.Count > 0)
                {
                    parameters.Add("@idTipoCosecha", ddlTipoCosecha.SelectedValue);
                    parameters.Add("@idCajaTipo", ddlTipoCaja.SelectedValue);
                }
                else
                {
                    parameters.Add("@idTipoCosecha", 0);
                    parameters.Add("@idCajaTipo", 0);
                }
              

                


                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaCostoPorActividad", parameters);

                if (Convert.ToInt32(result.Rows[0]["Resultado"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Mensaje"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtieneCostosActividad();
                    LimpiaCampos();

                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Mensaje"].ToString(), Comun.MESSAGE_TYPE.Error);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
    public void LimpiaCampos()
    {
        txtCantidad0.Text = "";
        txtCosto.Text = "";
        hdnIdPlanta.Value = "0";
    }

    public void ObtieneCostosActividad()
    {
       try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneCostoPorActividades", null);
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idCostoActividad"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idCostoActividad"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idCostoActividad", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneCostoPorActividadPorIdCosto", parameters);

            ddlPlanta.SelectedValue = dt.Rows[0]["idPlanta"].ToString();
            ObtieneDepartamentosPorPlanta(ddlPlanta.SelectedValue);
            ddlDepartamento0.SelectedValue = dt.Rows[0]["idDepartamento"].ToString();
            ObtieneProductosPorPlanta(ddlPlanta.SelectedValue);
            ddlProducto.SelectedValue = dt.Rows[0]["idProducto"].ToString();
            ObtieneActividadesPorPlantaDepartamento(ddlPlanta.SelectedValue, ddlDepartamento0.SelectedValue, ddlProducto.SelectedValue);
           
            ddlActividad0.SelectedValue = dt.Rows[0]["idHabilidad"].ToString();
            ValidaActividadTipoCosecha(ddlActividad0.SelectedValue);
            if (ddlTipoCosecha.Items.Count > 0)
            {
                ddlTipoCosecha.SelectedValue = dt.Rows[0]["idTipoCosecha"].ToString();
                ddlTipoCaja.SelectedValue = dt.Rows[0]["idCajaTipo"].ToString();
            }

            
            txtCantidad0.Text = dt.Rows[0]["cantidad"].ToString();
            txtCosto.Text = dt.Rows[0]["Costo"].ToString();
            ddlUnidad0.SelectedValue = dt.Rows[0]["idUnidadMedida"].ToString();
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
 

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
    protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneActividadesPorPlantaDepartamento(ddlPlanta.SelectedValue, ddlDepartamento0.SelectedValue, ddlProducto.SelectedValue);
        ValidaActividadTipoCosecha(ddlActividad0.SelectedValue);
    }
    protected void ddlActividad0_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidaActividadTipoCosecha(ddlActividad0.SelectedValue);
    }
    public void ValidaActividadTipoCosecha(string idActividad)
    {
        DataTable dt;
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idActividad", idActividad);

        dt = dataaccess.executeStoreProcedureDataTable("procValidaActividadCosecha", parameters);

        if (dt.Rows.Count > 0)
        {
            if (Convert.ToInt32(dt.Rows[0]["Resultado"]) > 0)
            {
                trTipoCosecha.Visible = true;
                ObtieneTipoCosechas();
            }
            else
            {
                trTipoCosecha.Visible = false;
                ddlTipoCosecha.Items.Clear();
                ddlTipoCaja.Items.Clear();

              
                

            }
        }
        else
        {
            trTipoCosecha.Visible = false;
        }
    }
    public void ObtieneTipoCosechas()
    {

        DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneTipoCosechas", null);
        ddlTipoCosecha.DataSource = dt;
        ddlTipoCosecha.DataTextField = "Nombre";
        ddlTipoCosecha.DataValueField = "idTipoCosecha";
        ddlTipoCosecha.DataBind();


        dt = dataaccess.executeStoreProcedureDataTable("[procObtieneTipoCajasCosecha]", null);
        ddlTipoCaja.DataSource = dt;
        ddlTipoCaja.DataTextField = "Nombre";
        ddlTipoCaja.DataValueField = "idCajaTipo";
        ddlTipoCaja.DataBind();
    }
}