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

public partial class configuracion_frmTabletaUsuario : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtieneListaUsuarios();
            ObtenerUsuariosAsignados();
        }
    }

    protected void ObtieneListaUsuarios()
    {
        try
        {
            var dt = dataaccess.executeStoreProcedureDataTable("procObtieneListaUsuarios", null);
            ddlUsuario.DataSource = dt;
            ddlUsuario.DataValueField = "idUsuario";
            ddlUsuario.DataTextField = "vNombre";
            ddlUsuario.DataBind();
        }
        catch (Exception ex)
        {

        }
    }
    public void ObtenerUsuariosAsignados()
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            DataSet ds = dataaccess.executeStoreProcedureDataSet("procObtieenTabletaUsuarios", null);
            GvPlantas.DataSource = ds.Tables[0];
            GvPlantas.DataBind();

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }

    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idUsuario", ddlUsuario.SelectedValue);
        parameters.Add("@estatus", chkEstatus.Checked);


        DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaTabletaUsuario", parameters);

        if (Convert.ToInt32(result.Rows[0]["RESULTADO"]) == 1)
        {
            popUpMessageControl1.setAndShowInfoMessage("Se asigno correctamente el ID TABLETA", Comun.MESSAGE_TYPE.Success);
            

        }
        else
        {
            popUpMessageControl1.setAndShowInfoMessage("Se actualizo correctamente", Comun.MESSAGE_TYPE.Success);
        }
        ObtenerUsuariosAsignados();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        chkEstatus.Checked = false;
        ObtieneListaUsuarios();
    }
}