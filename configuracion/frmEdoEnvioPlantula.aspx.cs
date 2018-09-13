using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using log4net;

public partial class configuracion_frmEdoEnvioPlantula : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmEdoEnvioPlantula));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                obtieneEdo();
            }
        }
        catch (Exception exception)
        {
            log.Error(exception.ToString());
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        if (Accion.Value == "Añadir")
        {
            txtEdo.Text = "";
            txtEdo_EN.Text = "";
        }
        else
        {
            txtEdo.Text = "";
            txtEdo_EN.Text = "";
            Accion.Value = "Guardar Cambios";
            
            btn_Enviar.Text = GetLocalResourceObject("Guardar").ToString();
            btnCancelar.Text = GetLocalResourceObject("Limpiar").ToString();
        }
    }
    private void obtieneEdo()
    {
        Dictionary<string, object> par = new Dictionary<string, object>();
        par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_EdoEnvíoPlant", par);
        gv_EdoEnvio.DataSource = dt;
        ViewState["dsEdo"] = dt;
        gv_EdoEnvio.DataBind();
    }
    protected void gv_EdoEnvio_SelectedIndexChanged(object sender, EventArgs e)
    { 
    Session["IdModuloCookie"] = gv_EdoEnvio.DataKeys[gv_EdoEnvio.SelectedIndex].Value.ToString();
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idEstado", Session["IdModuloCookie"]);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectFromEdoEnvioId", parameters);
        if (dt.Rows.Count > 0)
        {
            txtEdo.Text = dt.Rows[0]["vEstado"].ToString().Trim();
            txtEdo_EN.Text = dt.Rows[0]["vEstado_EN"].ToString().Trim();
            //txtRuta.Text = dt.Rows[0]["ruta"].ToString().Trim();
            if (dt.Rows[0]["bActivo"].ToString().Equals("True"))
                Activo.Checked = true;
            else
                Activo.Checked = false;

            Accion.Value = "Guardar Cambios";
            //btn_Enviar.Visible = true;
            //btnCancelar.Visible = true;
            btn_Enviar.Text = GetLocalResourceObject("Actualizar").ToString();
            btnCancelar.Text =GetLocalResourceObject("Cancelar").ToString();
        }
        else
        {
            //No se encontró el registro
        }
    }
    protected void gv_EdoEnvio_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsEdo"])
            {
                DataSet ds = ViewState["dsEdo"] as DataSet;

                if (ds != null)
                {
                    gv_EdoEnvio.DataSource = ds;
                    gv_EdoEnvio.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception)
        {
        }
    }
    protected void gv_EdoEnvio_PreRender(object sender, EventArgs e)
    {
        if (gv_EdoEnvio.HeaderRow != null)
            gv_EdoEnvio.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void gv_EdoEnvio_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gv_EdoEnvio, ("Select$" + e.Row.RowIndex.ToString()));
                break;
        }
    }
    protected void btn_Guardar_Click(object sender, EventArgs e)
    {
        if (txtEdo.Text.Trim().Equals("") || txtEdo_EN.Text.Trim().Equals(""))
        {
            popUpMessageControl1.setAndShowInfoMessage("El estado de envío es requerido.", Comun.MESSAGE_TYPE.Error);
        }
        else
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@Estado", txtEdo.Text);
            parameters.Add("@Estado_EN", txtEdo_EN.Text);

            parameters.Add("@UsuarioModifico", Session["idUsuario"]);
            if (Activo.Checked)
                parameters.Add("@Activo", 1);
            else
                parameters.Add("@Activo", 0);


            if (Accion.Value == "Añadir")
            {
                //Verificar que el valor "Razón" a insertar no estan anteriormente agregados
                Dictionary<string, object> find = new System.Collections.Generic.Dictionary<string, object>();
                find.Add("@Estado", txtEdo.Text);
                find.Add("@Estado_EN", txtEdo_EN.Text);
                find.Add("@UsuarioModifico", Session["idUsuario"]);

                if (dataaccess.executeStoreProcedureGetInt("spr_ExisteEdoEnvioPlantula", find) > 0)
                {
                    popUpMessageControl1.setAndShowInfoMessage("No se efectuarán cambios debido a que el estado ya existe.", Comun.MESSAGE_TYPE.Info);
                }
                else
                {
                    String Rs = dataaccess.executeStoreProcedureString("spr_EdoEnvioInsertar", parameters);
                    if (Rs.Equals("Repetido"))
                    {
                        popUpMessageControl1.setAndShowInfoMessage("Ese registro ya había sido capturado.", Comun.MESSAGE_TYPE.Error);
                    }
                    else
                    {
                        popUpMessageControl1.setAndShowInfoMessage("El Estado  \"" + txtEdo.Text + "\" se guardó exitosamente.", Comun.MESSAGE_TYPE.Success);

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
                    parameters.Add("@idEstado", Session["IdModuloCookie"].ToString());
                    String Rs = dataaccess.executeStoreProcedureString("spr_UpdateEdoEnvioPlantula", parameters);
                    if (Rs.Equals("Igual"))
                    {
                        popUpMessageControl1.setAndShowInfoMessage("No existieron cambios en la razón.", Comun.MESSAGE_TYPE.Info);
                    }
                    else
                    {
                        popUpMessageControl1.setAndShowInfoMessage("El  Estado de envío por plántula fue modificada.", Comun.MESSAGE_TYPE.Success);
                    }

                }
            }
            //obtieneModulo();

            //gv_Merma.DataSource = da.executeStoreProcedureDataTable("spr_MermaObtener", new Dictionary<string, object>());
            obtieneEdo();
            //VolverAlPanelInicial();
        }
        Accion.Value = "Añadir";
        txtEdo.Text = "";
        txtEdo_EN.Text = "";
        btnCancelar.Text = GetLocalResourceObject("Limpiar").ToString();
        btn_Enviar.Text = GetLocalResourceObject("Guardar").ToString();
    }
}