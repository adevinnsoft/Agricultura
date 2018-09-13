using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using log4net;
using System.Text;
using System.Xml;
using System.Web.Script.Serialization;
using System.Windows.Forms;

public partial class RH_frmADMEmpleados : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ObtieneRanchosPorUsuario(Session["idUsuario"].ToString());
            ObtieneLideres(ddlRanchos.SelectedValue);
            ObtieneEmpleados(ddlRanchos.SelectedValue, ddlLider.SelectedValue, txtNoEmpleado.Text.Trim(), txtNombreEmpleado.Text.Trim());

        }
    }
    public void ObtieneRanchosPorUsuario(string idUsuario)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idUsuario", idUsuario);

            ddlRanchos.DataSource = dataaccess.executeStoreProcedureDataTableFill("spr_ObtenerPlantaPorUsuario", parameters);
            ddlRanchos.DataTextField = "NombrePlanta";
            ddlRanchos.DataValueField = "idPlanta";
            ddlRanchos.DataBind();


            //ddlRanchos.Items.Insert(0, new ListItem("Todos", "0"));
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    public void ObtieneRanchos()
    {

        DataTable result = dataaccess.executeStoreProcedureDataTable("procObtienePlantas", null);

        ddlRanchos.DataSource = result;
        ddlRanchos.DataTextField = "nombrePlanta";
        ddlRanchos.DataValueField = "idPlanta";
        ddlRanchos.DataBind();
        //ddlRanchos.Items.Insert(0, new ListItem("Todos", "0"));

    }
    public void ObtieneLideres(string idPlanta)
    {
        DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneLideresPlanta", new Dictionary<string, object>() { 
                    {"@idPlanta",idPlanta}});
        ddlLider.DataSource = dt;
        ddlLider.DataTextField = "Nombre_Lider";
        ddlLider.DataValueField = "id_Lider";
        ddlLider.DataBind();
        ddlLider.Items.Insert(0, new ListItem("Todos", "0"));

    }
    public void ObtieneEmpleados(string idPlanta, string idLider, string idEmpleado, string nombre)
    {
        try
        {
            if (idEmpleado.Length == 0)
            {
                idEmpleado = "0";
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idPlanta", idPlanta);
            parameters.Add("@idLider", idLider);
            parameters.Add("@id_Empleado", idEmpleado);
            parameters.Add("@NombreEmpleado", nombre);

            DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneReporteEmpleados", parameters);
            GvPlantas.DataSource = result;
            GvPlantas.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        ObtieneEmpleados(ddlRanchos.SelectedValue, ddlLider.SelectedValue, txtNoEmpleado.Text.Trim(), txtNombreEmpleado.Text.Trim());
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Limpiar();
    }
    public void Limpiar()
    {
        ddlRanchos.SelectedIndex = 0;
        ddlLider.SelectedIndex = 0;
        txtNoEmpleado.Text = "";
        txtNombreEmpleado.Text = "";
        hdinIdAsociado.Value = "0";
        txtFechaAlta.Text = "";
        txtFechaBaja0.Text = "";
    }
    protected void ddlRanchos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneLideres(ddlRanchos.SelectedValue);
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["ID_EMPLEADO"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["ID_EMPLEADO"].ToString(), out id);
            }
            
            parameters.Add("@noEmpleado", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneEmpleado", parameters);
            hdinIdAsociado.Value = dt.Rows[0]["idAsociado"].ToString();
            ddlRanchos.SelectedValue = dt.Rows[0]["idPlanta"].ToString();
            ddlLider.SelectedValue = dt.Rows[0]["id_Lider"].ToString();
            txtNoEmpleado.Text=dt.Rows[0]["idEmpleado"].ToString();
            txtNombreEmpleado.Text = dt.Rows[0]["nombreAsociado"].ToString();
            txtFechaAlta.Text = dt.Rows[0]["fechaAlta"].ToString();
            txtFechaBaja0.Text = dt.Rows[0]["fechaBaja"].ToString();
            cbxActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            


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
           
            Dictionary<string, object> parameters = new Dictionary<string, object>();

             parameters.Add("@noEmpleado", txtNoEmpleado.Text);
             parameters.Add("@nombreEmpleado", txtNombreEmpleado.Text.Trim().ToUpper());
             parameters.Add("@idPlanta", ddlRanchos.SelectedValue);
             parameters.Add("@idSupervisor", ddlLider.SelectedValue);
             parameters.Add("@fechaAlta", txtFechaAlta.Text);
           
            parameters.Add("@fechaBaja", txtFechaBaja0.Text);
             
            
            parameters.Add("@estatus", cbxActivo.Checked);
            parameters.Add("@idAsociado", hdinIdAsociado.Value);
            parameters.Add("@idUsuario", Session["idUsuario"].ToString());

            DataTable result = dataaccess.executeStoreProcedureDataTable("procAltaBajaEmpleado", parameters);


            if (Convert.ToInt32(result.Rows[0]["resultado"]) < 0)
            {
                popUpMessageControl1.setAndShowInfoMessage("NO. EMPLEADO YA EXISTE EN LA BASE DE DATOS FAVOR DE REVISARLO", Comun.MESSAGE_TYPE.Error);

            }
            else
            {
                if (Convert.ToInt32(result.Rows[0]["resultado"]) == 1)
                {
                    popUpMessageControl1.setAndShowInfoMessage("EMPLEADO SE ACTUALIZÓ CORRECTAMENTE", Comun.MESSAGE_TYPE.Success);
                    ObtieneEmpleados(ddlRanchos.SelectedValue, ddlLider.SelectedValue, txtNoEmpleado.Text.Trim(), txtNombreEmpleado.Text.Trim());
                    Limpiar();
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage("EMPLEADO SE DIO DE ALTA CORRECTAMENTE", Comun.MESSAGE_TYPE.Success);
                    ObtieneEmpleados(ddlRanchos.SelectedValue, ddlLider.SelectedValue, txtNoEmpleado.Text.Trim(), txtNombreEmpleado.Text.Trim());
                    Limpiar();
                }
            }
     
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
}