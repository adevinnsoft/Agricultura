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


public partial class configuracion_frmCandadoCapturaTrabajo : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmCandadoCapturaTrabajo));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DiasSemanaCombos();
            ObtieneCandados();
        }
    }

    public void limpiacampos()
    {
        ddlDiaInicio.SelectedIndex = 0;
        ddlDiaFin0.SelectedIndex = 0;
        chkActivo.Checked = false;
        hdnIdPlanta.Value = "0";
    }

    public void DiasSemanaCombos()
    {
        try
        {
            
         
            ddlDiaInicio.DataSource = dataaccess.executeStoreProcedureDataTable("procObtieneDiasSemana", null);
            ddlDiaInicio.DataTextField = "Dia";
            ddlDiaInicio.DataValueField = "noDia";
            ddlDiaInicio.DataBind();

            ddlDiaFin0.DataSource = ddlDiaInicio.DataSource;
            ddlDiaFin0.DataTextField = "Dia";
            ddlDiaFin0.DataValueField = "noDia";
            ddlDiaFin0.DataBind();
            

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    public void ObtieneCandados()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneCandadoCapturaTrabajo", null);
            GvSucursales.DataSource = dt;
            GvSucursales.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
    protected void GvSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != GvSucursales.SelectedPersistedDataKey)
            {
                Int32.TryParse(GvSucursales.SelectedPersistedDataKey["idCapturaFecha"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvSucursales.SelectedDataKey["idCapturaFecha"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idCapturaFecha", id);

            dt = dataaccess.executeStoreProcedureDataTable("procObtieneCandadoCapturaTrabajoPorId", parameters);

            ddlDiaInicio.SelectedValue = dt.Rows[0]["diaInicio"].ToString();
            ddlDiaFin0.SelectedValue = dt.Rows[0]["diaFin"].ToString();
            chkActivo.Checked = dt.Rows[0]["Estatus"].ToString() == "True" ? true : false;


        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoCargar").ToString(), Comun.MESSAGE_TYPE.Error);
        }
    }

    protected void GvSucursales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(GvSucursales, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void GvSucursales_PreRender(object sender, EventArgs e)
    {
        if (GvSucursales.HeaderRow != null)
            GvSucursales.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        
            
         try{   
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idCapturaFecha", hdnIdPlanta.Value);
                parameters.Add("@diaInicio", ddlDiaInicio.SelectedValue);
                parameters.Add("@diaFin", ddlDiaFin0.SelectedValue);

                if (chkActivo.Checked)
                {
                    parameters.Add("@estatus", 1);
                }
                else
                {
                    parameters.Add("@estatus", 0);
                }
                
       
                parameters.Add("@idUsuario", Session["idUsuario"].ToString());

                DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaCandadoCapturaTrabajo", parameters);

                if (Convert.ToInt32(result.Rows[0]["resultado"]) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage("REGISTRO GUARDADO CORRECTAMENTE", Comun.MESSAGE_TYPE.Success);
                    ObtieneCandados();
                    limpiacampos();

                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage("NO PUEDEN EXISTIR DOS CANDADOS ACTIVOS AL MISMO TIEMPO", Comun.MESSAGE_TYPE.Error);
                }
           
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos();
    }
}