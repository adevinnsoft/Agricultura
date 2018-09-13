using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;
using System.Globalization;

public partial class configuracion_frmZonasMonitoreo : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmZonasMonitoreo));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                obtieneZona(); 
             //   popUpMessageControl1.setAndShowInfoMessage("Ese registro ya había sido capturado.", Comun.MESSAGE_TYPE.Error);

            }
        }
        catch (Exception exception)
        {
            log.Error(exception.ToString());
        }
    }
    private void obtieneZona()
    {
        Dictionary<string, object> par = new Dictionary<string, object>();
        par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ZonasMonitoreo", par);
        gv_ZonaMonitor.DataSource = dt;
        ViewState["dsZona"] = dt;
        gv_ZonaMonitor.DataBind();
    }
    protected void gv_ZonaMonitor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["IdModuloCookie"] = gv_ZonaMonitor.DataKeys[gv_ZonaMonitor.SelectedIndex].Value.ToString();
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idZona", Session["IdModuloCookie"]);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectFromZonaId", parameters);
        if (dt.Rows.Count > 0)
        {
            txtZona.Text = dt.Rows[0]["vZona"].ToString().Trim();
            txtZona_EN.Text = dt.Rows[0]["vZona_EN"].ToString().Trim();
            //txtRuta.Text = dt.Rows[0]["ruta"].ToString().Trim();
            if (dt.Rows[0]["bActivo"].ToString().Equals("True"))
                Activo.Checked = true;
            else
                Activo.Checked = false;

            Accion.Value = "Guardar Cambios";
           
            btn_Enviar.Text = GetLocalResourceObject("Actualizar").ToString();
            btnCancelar.Text = GetLocalResourceObject("Cancelar").ToString();
        }
        else
        {
            //No se encontró el registro
        }
    }
    protected void gv_ZonaMonitor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsZona"])
            {
                DataSet ds = ViewState["dsZona"] as DataSet;

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
        if (Accion.Value == "Añadir")
        {
            txtZona.Text = "";
            txtZona_EN.Text = "";
        }
        else
        {
            txtZona.Text = "";
            txtZona_EN.Text = "";
            Accion.Value = "Guardar Cambios";

            btn_Enviar.Text = GetLocalResourceObject("Guardar").ToString();
            btnCancelar.Text = GetLocalResourceObject("Limpiar").ToString();
        }
    }
    protected void btn_Guardar_Click(object sender, EventArgs e)
    {
        if (txtZona.Text.Trim().Equals("") || txtZona_EN.Text.Trim().Equals(""))
        {
            popUpMessageControl1.setAndShowInfoMessage("La Zona es requerida.", Comun.MESSAGE_TYPE.Error);
        }
        else
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@Zona", txtZona.Text);
            parameters.Add("@Zona_EN", txtZona_EN.Text);

            parameters.Add("@UsuarioModifico", Session["idUsuario"]);
            if (Activo.Checked)
                parameters.Add("@Activo", 1);
            else
                parameters.Add("@Activo", 0);


            if (Accion.Value == "Añadir")
            {
                //Verificar que el valor "Razón" a insertar no estan anteriormente agregados
                Dictionary<string, object> find = new System.Collections.Generic.Dictionary<string, object>();
                find.Add("@Zona", txtZona.Text);
                find.Add("@Zona_EN", txtZona_EN.Text);
                find.Add("@UsuarioModifico", Session["idUsuario"]);

                if (dataaccess.executeStoreProcedureGetInt("spr_ExisteZona", find) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage("No se efectuarán cambios debido a que el estado ya existe.", Comun.MESSAGE_TYPE.Info);
                }
                else
                {
                    String Rs = dataaccess.executeStoreProcedureString("spr_ZonaInsertar", parameters);
                    if (Rs.Equals("Repetido"))
                    {
                        popUpMessageControl1.setAndShowInfoMessage("Ese registro ya había sido capturado.", Comun.MESSAGE_TYPE.Error);
                    }
                    else
                    {
                        popUpMessageControl1.setAndShowInfoMessage("La Zona  \"" + txtZona.Text + "\" se guardó exitosamente.", Comun.MESSAGE_TYPE.Success);

                    }
                }
            }
            else
            {
                if (Session["IdModuloCookie"] == null || Session["IdModuloCookie"].ToString() == "")
                {
                    popUpMessageControl1.setAndShowInfoMessage("Error #RGRL02: Se perdió la información del ID actual", Comun.MESSAGE_TYPE.Error);
                }
                else
                {
                    parameters.Add("@idZona", Session["IdModuloCookie"].ToString());
                    String Rs = dataaccess.executeStoreProcedureString("spr_UpdateZonaMonitoreo", parameters);
                    if (Rs.Equals("Igual"))
                    {
                        popUpMessageControl1.setAndShowInfoMessage("No existieron cambios en la Zona.", Comun.MESSAGE_TYPE.Info);
                    }
                    else
                    {
                        popUpMessageControl1.setAndShowInfoMessage("La Zona de Monitoreo fue modificada.", Comun.MESSAGE_TYPE.Success);
                    }

                }
            }
            //obtieneModulo();

            //gv_Merma.DataSource = da.executeStoreProcedureDataTable("spr_MermaObtener", new Dictionary<string, object>());
            obtieneZona();
            //VolverAlPanelInicial();
        }
        Accion.Value = "Añadir";
        txtZona.Text = "";
        txtZona_EN.Text = "";
        btnCancelar.Text = GetLocalResourceObject("Limpiar").ToString();
        btn_Enviar.Text = GetLocalResourceObject("Guardar").ToString();
    }
}