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


public partial class RH_frmEmpleados : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            ObtieneRanchos();
            ObtieneLideres(ddlRanchos.SelectedValue);
            ObtieneEmpleados(ddlRanchos.SelectedValue, ddlLider.SelectedValue, txtNoEmpleado.Text.Trim(), txtNombreEmpleado.Text.Trim());

        }
    }
    public void ObtieneRanchos()
    {
       

        DataTable result = dataaccess.executeStoreProcedureDataTable("procObtienePlantas", null);

        ddlRanchos.DataSource = result;
        ddlRanchos.DataTextField = "nombrePlanta";
        ddlRanchos.DataValueField = "idPlanta";
        ddlRanchos.DataBind();
        ddlRanchos.Items.Insert(0, new ListItem("Todos", "0"));
        
    }
    public void ObtieneLideres(string idPlanta)
    {
        DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneLideresPlanta", new Dictionary<string, object>() { 
                    {"@idPlanta",idPlanta}});
        ddlLider.DataSource = dt;
        ddlLider.DataTextField = "Nombre_Lider";
        ddlLider.DataValueField = "id_Lider";
        ddlLider.DataBind();
        ddlLider.Items.Insert(0,new ListItem("Todos","0"));

    }
    public void ObtieneEmpleados(string idPlanta, string idLider,string idEmpleado,string nombre)
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
        ddlRanchos.SelectedIndex = 0;
        ddlLider.SelectedIndex = 0;
        txtNoEmpleado.Text = "";
        txtNombreEmpleado.Text = "";
    }
    protected void ddlRanchos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneLideres(ddlRanchos.SelectedValue);
    }
}