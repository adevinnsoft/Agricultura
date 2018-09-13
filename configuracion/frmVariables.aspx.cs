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

public partial class configuracion_frmVariables : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmVariables));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ObtieneVariables();
         
           
        }
    }

    public void limpiacampos()
    {
        txtDescripcion.Text = string.Empty;
        txtCodigoVariable.Text = string.Empty;
        chkActivo.Checked = false;
    }



    public void ObtieneVariables()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneVariables", null);
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
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idVariable"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idVariable"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idVariable", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneVariablesPorIdVariable", parameters);

            txtCodigoVariable.Text = dt.Rows[0]["codigoVariable"].ToString();
            txtDescripcion.Text = dt.Rows[0]["descripcion"].ToString();
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

    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        try
        {
            if (txtDescripcion.Text.Trim().Equals("") || txtCodigoVariable.Text.Trim().Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Favor de capturar todos los datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idVariable", hdnIdPlanta.Value);
                parameters.Add("@codigoVariable", txtCodigoVariable.Text);
                parameters.Add("@activo", chkActivo.Checked.ToString());
                parameters.Add("@descripcion", txtDescripcion.Text);
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());

                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaVariablesABC", parameters);

                if (Convert.ToInt32(result.Rows[0]["ID"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["Resultado"].ToString(), Comun.MESSAGE_TYPE.Success);
                    ObtieneVariables();
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