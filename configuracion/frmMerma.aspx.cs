using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;
using System.Globalization;

public partial class configuracion_Merma : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_Merma));
    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
            if (!IsPostBack)
            {
                obtieneMerma();
                obtieneCategorias();
            }
        }
         catch (Exception exception)
         {
             log.Error(exception.ToString());
         }
    }
    protected void obtieneCategorias()
    {
        try
        {
            //ddlCategoria.Items.Insert(0, new ListItem(GetGlobalResourceObject("Commun", "Select").ToString(), "0"));
            //ddlCategoria.SelectedIndex = 0;

            //
            ddlCategoria.Items.Clear();
            var parameters = new Dictionary<string, object>();

            //parameters.Add("@ACTIVO", true);
            //parameters.Add("@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            

            try
            {
                parameters.Add("@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
                //parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
                ddlCategoria.DataSource = dataaccess.executeStoreProcedureDataSet("spr_CategoriaMermaObtener", parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                //return false;     //Esta variable se puede utilizar como un retorno de si existió un error y efectuar una acción
            }

            ddlCategoria.DataTextField = "NombreCategoria";
            ddlCategoria.DataValueField = "IdCategoria";
            ddlCategoria.DataBind();
            //ddlCategoria.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "Select") as String, string.Empty));
            ddlCategoria.Items.Insert(0, new ListItem(GetGlobalResourceObject("Commun", "Select").ToString(), "")); 
            ddlCategoria.SelectedIndex = 0;
            //throw new NotImplementedException();
            //
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
    protected void ddlCategoria_DataBound(object sender, EventArgs e)
    {
        
    }
    protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            setddlSunMP();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
    protected void setddlSunMP()
    {
        //ddlSunMP.DataTextField = CultureInfo.CurrentCulture.Name == "es-MX" ? "subModulo" : "subModulo_EN";

        //DataTable submodules = (ViewState["submodules"] as DataTable);
        //DataTable dt = submodules.Clone();
        //if (ddlModulo.SelectedValue == "")
        //{
        //    ddlSunMP.Items.Clear();
        //}
        //else
        //    foreach (DataRow item in submodules.Select("idSubModuloParent IS NULL"
        //                                                            + " AND idModulo = " + ddlModulo.SelectedValue))
        //    {
        //        dt.ImportRow(item);
        //        addChilds(item, dt, submodules, 1);
        //    }

        //ddlSunMP.DataSource = dt;
        //ddlSunMP.DataBind();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        
            txtMerma.Text = "";
            txtMerma_EN.Text = "";
            ddlCategoria.SelectedIndex = 0;
            Activo.Checked = true;
        if (Accion.Value == "Añadir")
        {
            txtMerma.Text = "";
            txtMerma_EN.Text = "";

        }
        else 
        {
            Accion.Value = "Guardar Cambios";
            btn_Enviar.Visible = true;
            btnCancelar.Visible = true;
            btn_Enviar.Text = GetLocalResourceObject("Guardar").ToString();
            btnCancelar.Text = GetLocalResourceObject("Limpiar").ToString();
        }
        Activo.Checked = true;
        obtieneCategorias();
    }
    protected void btn_Enviar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMerma.Text.Trim().Equals("") || txtMerma_EN.Text.Trim().Equals("") || ddlCategoria.SelectedValue.Equals(""))
            {
                popUpMessageControl1.setAndShowInfoMessage("Capturar datos requeridos.", Comun.MESSAGE_TYPE.Error);
            }
            else
            {
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@Razon", txtMerma.Text);
                parameters.Add("@Razon_EN", txtMerma_EN.Text);

                parameters.Add("@UsuarioModifico", Session["idUsuario"]);
                parameters.Add("@idCategoriaMerma", ddlCategoria.SelectedValue);
                if (Activo.Checked)
                    parameters.Add("@activo", 1);
                else
                    parameters.Add("@activo", 0);


                if (Accion.Value == "Añadir")
                {
                    //Verificar que el valor "Razón" a insertar no estan anteriormente agregados
                    Dictionary<string, object> find = new System.Collections.Generic.Dictionary<string, object>();
                    find.Add("@Razon", txtMerma.Text);
                    find.Add("@Razon_EN", txtMerma_EN.Text);
                    find.Add("@UsuarioModifico", Session["idUsuario"]);
                    find.Add("@idCategoria", ddlCategoria.SelectedValue);

                    if (dataaccess.executeStoreProcedureGetInt("spr_ExisteRazonMerma", find) > 0)
                    {
                        popUpMessageControl1.setAndShowInfoMessage("No se efectuarán cambios debido a que la razón ya existe.", Comun.MESSAGE_TYPE.Info);
                    }
                    else
                    {
                        String Rs = dataaccess.executeStoreProcedureString("spr_MermaInsertar", parameters);
                        if (Rs.Equals("Repetido"))
                        {
                            popUpMessageControl1.setAndShowInfoMessage("Ese registro ya había sido capturado.", Comun.MESSAGE_TYPE.Error);
                        }
                        else
                        {
                            popUpMessageControl1.setAndShowInfoMessage("La Razón  \"" + txtMerma.Text + "\" se guardó exitosamente.", Comun.MESSAGE_TYPE.Success);

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
                        parameters.Add("@IdMerma", Session["IdModuloCookie"].ToString());

                        String Rs = dataaccess.executeStoreProcedureString("spr_UpdateMerma", parameters);
                        if (Rs.Equals("Igual"))
                        {
                            popUpMessageControl1.setAndShowInfoMessage("No existieron cambios en la razón.", Comun.MESSAGE_TYPE.Info);
                        }
                        else
                        {
                            popUpMessageControl1.setAndShowInfoMessage("La Razón fue modificada.", Comun.MESSAGE_TYPE.Success);
                        }

                    }
                }
                //obtieneModulo();

                //gv_Merma.DataSource = da.executeStoreProcedureDataTable("spr_MermaObtener", new Dictionary<string, object>());
                obtieneMerma();
                //VolverAlPanelInicial();
                Accion.Value = "Añadir";
                txtMerma.Text = "";
                txtMerma_EN.Text = "";
                ddlCategoria.SelectedIndex = 0;
                Activo.Checked = true;
                btnCancelar.Text = GetLocalResourceObject("Limpiar").ToString();
                btn_Enviar.Text = GetLocalResourceObject("Guardar").ToString();
            }
        } catch(Exception ex){
            Log.Error(ex);
        }
    }

    private void obtieneMerma()
    {
        Dictionary<string, object> par = new Dictionary<string, object>();
        par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_MermaObtener", par);
        gv_Merma.DataSource = dt;
        ViewState["dsMerma"] = dt;
        gv_Merma.DataBind();
    }
    //Métodos del grid
    
    protected void gv_Merma_SelectedIndexChanged(object sender, EventArgs e)
    {

        Session["IdModuloCookie"] = gv_Merma.DataKeys[gv_Merma.SelectedIndex].Value.ToString();
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        parameters.Add("@Id_Merma", Session["IdModuloCookie"]);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectFromMermaId", parameters);
        if (dt.Rows.Count > 0)
        {
            txtMerma.Text = dt.Rows[0]["Razon"].ToString().Trim();
            txtMerma_EN.Text = dt.Rows[0]["Razon_EN"].ToString().Trim();
            //ddlCategoria.SelectedValue.Equals(dt.Rows[0]["NombreCategoria"].ToString().Trim());
            //ddlCategoria.SelectedItem.Value = dt.Rows[0]["NombreCategoria"].ToString().Trim();
            if (ddlCategoria.Items.FindByValue(dt.Rows[0]["idCategoria"].ToString()) != null)
            {
                ddlCategoria.SelectedValue = dt.Rows[0]["idCategoria"].ToString();
            }
            else
            {
                ddlCategoria.SelectedIndex = 0;
            }
            //txtRuta.Text = dt.Rows[0]["ruta"].ToString().Trim();
            if (dt.Rows[0]["Activo"].ToString().Equals("True"))
                Activo.Checked = true; 
            else
                Activo.Checked = false;

            Accion.Value = "Guardar Cambios";
            btn_Enviar.Visible = true;
            btnCancelar.Visible = true;
            btn_Enviar.Text = GetLocalResourceObject("Editar").ToString();
            btnCancelar.Text = GetLocalResourceObject("Cancelar").ToString();
        }
        else
        {
            //No se encontró el registro
        }
    }
   
    protected void gvMerma_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsMerma"])
            {
                DataSet ds = ViewState["dsMerma"] as DataSet;

                if (ds != null)
                {
                    gv_Merma.DataSource = ds;
                    gv_Merma.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception)
        {
        }

    }
    protected void gvMerma_PreRender(object sender, EventArgs e)
    {
        if (gv_Merma.HeaderRow != null)
            gv_Merma.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void gvMerma_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gv_Merma, ("Select$" + e.Row.RowIndex.ToString()));
                break;
        }
    }
}