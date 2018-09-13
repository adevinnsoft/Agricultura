using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Globalization;

public partial class configuracion_frmEtapas : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LlenaHabilidades();
            llenaTabla();
            obtieneFamilias();
        }
    }

    public void LlenaHabilidades()
    {
        try
        {
            ddlHabilidades.DataSource = dataaccess.executeStoreProcedureDataTable("spr_ObtieneHabilidadesDdl", new Dictionary<string, object>() { { "@lengua", Session["Locale"] } });
            ddlHabilidades.DataTextField = "NombreHabilidad";
            ddlHabilidades.DataValueField = "idHabilidad";
            ddlHabilidades.DataBind();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    public void llenaTabla()
    {
        try
        {
            gvEtapas.DataSource = dataaccess.executeStoreProcedureDataTable("spr_ObtieneEtapasGv", null);
            gvEtapas.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }

    }
    protected void gvEtapas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvEtapas.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvEtapas.SelectedPersistedDataKey["IdEtapa"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvEtapas.SelectedDataKey["IdEtapa"].ToString(), out id);
            }

            parameters.Add("@IdEtapa", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneEtapaEditar", parameters);

            hdnIdEtapa.Value = dt.Rows[0]["IdEtapa"].ToString();
            txtTarget.Text = dt.Rows[0]["target"].ToString();
            txtEtapa.Text = dt.Rows[0]["NombreEtapa"].ToString();
            txtEtapa_EN.Text = dt.Rows[0]["NombreEtapa_EN"].ToString();
            //chkPorTiempo.Checked = dt.Rows[0]["PorTiempo"].ToString() == "True" ? true : false;
            chkActivo.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            ddlHabilidades.SelectedValue = dt.Rows[0]["IdHabilidad"].ToString();

            foreach (ListItem item in rblFamilias.Items){
                if (item.Value == dt.Rows[0]["idFamilia"].ToString())
                {
                    item.Selected = true;
                }
                else { item.Selected = false; }
            }
            obtieneNiveles();

            foreach (ListItem item in rblNiveles.Items)
            {
                if (item.Value == dt.Rows[0]["idNivel"].ToString())
                {
                    item.Selected = true;
                }
                else { item.Selected = false; }
            }

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }

    }
    protected void gvEtapas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEtapas, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void gvEtapas_PreRender(object sender, EventArgs e)
    {
        if (gvEtapas.HeaderRow != null)
            gvEtapas.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void  btnSave_Click(object sender, EventArgs e)
    {
        string result="";
        try
        {
            Dictionary<String, Object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@idetapa", hdnIdEtapa.Value); 
            Parameters.Add("@NombreEtapa", txtEtapa.Text);
            Parameters.Add("@NombreEtapa_EN", txtEtapa_EN.Text);
            //Parameters.Add("@PorTiempo", chkPorTiempo.Checked);
            Parameters.Add("@Activo", chkActivo.Checked);
            Parameters.Add("@UsuarioModifico", Session["idUsuario"].ToString());
            Parameters.Add("@idHabilidad", ddlHabilidades.SelectedValue);
            Parameters.Add("@idNivel", rblNiveles.SelectedValue);
            Parameters.Add("@target", txtTarget.Text);

            if (rblNiveles.SelectedValue == "")
            {
                popUpMessageControl1.setAndShowInfoMessage("Falta seleccionar familia y nivel", Comun.MESSAGE_TYPE.Info);
            }
            else
            {

                result = dataaccess.executeStoreProcedureString("spr_GuardaEtapa", Parameters);

                if (result == "OK")
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Guardado").ToString(), Comun.MESSAGE_TYPE.Success);
                    limpiacampos();
                    llenaTabla();
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoGuardado").ToString(), Comun.MESSAGE_TYPE.Success);
                }
            }
            
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    public void limpiacampos()
    {
         hdnIdEtapa.Value = string.Empty;
         txtEtapa.Text = string.Empty;
         txtEtapa_EN.Text = string.Empty;
         chkPorTiempo.Checked = false;
         chkActivo.Checked = true;
         ddlHabilidades.SelectedIndex = 0;

         foreach (ListItem item in rblFamilias.Items)
         {
            item.Selected = false; 
         }
         obtieneNiveles();

         foreach (ListItem item in rblNiveles.Items)
         {
            item.Selected = false;
         }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiacampos(); 
    }

    private void obtieneFamilias()
    {
        DataTable familias = dataaccess.executeStoreProcedureDataTable("spr_FamiliasObtiene", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name } });

        rblFamilias.Items.Clear();
        int i = 0;
        foreach (DataRow r in familias.Rows)
        {
            i++;
            rblFamilias.Items.Add(new ListItem(r["Nombre"].ToString().Trim(), r["IdFamilia"].ToString(), Convert.ToBoolean(r["bActivo"].ToString())));

        }
    }

    private void obtieneNiveles()
    {
        var idFamilia = rblFamilias.SelectedValue;
        DataTable niveles = dataaccess.executeStoreProcedureDataTable("spr_NivelesObtiene", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@idFamilia", idFamilia } });

        rblNiveles.Items.Clear();
        int i = 0;
        foreach (DataRow r in niveles.Rows)
        {
            i++;
            rblNiveles.Items.Add(new ListItem(r["Nivel"].ToString().Trim(), r["idNivel"].ToString(), Convert.ToBoolean(r["bActivo"].ToString())));

        }
    }

    protected void rblFamilias_SelectedIndexChanged(object sender, EventArgs e)
    {
        obtieneNiveles();
    }
}