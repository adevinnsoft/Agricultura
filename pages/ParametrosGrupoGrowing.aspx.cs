using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class pages_ParametrosGrupoGrowing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strScript = "";
        bool blnDebugSession = false;
        this.txtParametroPuntajeAsignado.Attributes.Add("onKeyPress", "Javascript:return OnlyNumbers(event);");
        
        try
        {
            if (this.hidIdParametroGrupoGrowing.Value == "")
            {
                this.txtCatalogoNombreEN_NA_OK_X.Enabled = false;
                this.txtCatalogoNombreES_NA_OK_X.Enabled = false;
                this.chkCatalogoActivo_NA_OK_X.Enabled = false;
                this.btnCatalogoSave_NA_OK_X.Enabled = false;
                this.txtCatalogoNombreEN_S_A_G_N.Enabled = false;
                this.txtCatalogoNombreES_S_A_G_N.Enabled = false;
                this.chkCatalogoActivo_S_A_G_N.Enabled = false;
                this.btnCatalogoSave_S_A_G_N.Enabled = false;
            }
            //Verifico el lenguaje

            // Seteo por default el valor para la variable de session de las imagenes y variables de session:
            this.Session["UserTemp"] = "";
            Session["ImagesUpload"] = null;
            blnDebugSession = Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]);
            if (blnDebugSession)
            {
                Session["userIDInj"] = "0";
                Session["usernameInj"] = "Admin";
            }

            if (UICulture.ToString().ToUpper().IndexOf("Ñ") > 0)
            {
                this.hidEsEnEspanol.Value = "true";
            }
            else
            {
                this.hidEsEnEspanol.Value = "false";
            }

        }
        catch (Exception exError)
        {
            strScript = "Error al inicializar la pagina:\\n" + exError.ToString();
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);

        }
    }

   protected void gvGruposGrowing_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";

        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvGruposGrowing.Rows.Count > 0)
        {
            this.gvGruposGrowing.UseAccessibleHeader = true;
            this.gvGruposGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        try
        {

            if (this.gvGruposGrowing.SelectedIndex != -1)
            {
                this.hidIdGrupoGrowing.Value = this.gvGruposGrowing.SelectedRow.Cells[0].Text;
                CheckBox chkApListaDeNA_OK_X = (CheckBox)this.gvGruposGrowing.SelectedRow.Cells[4].Controls[0];
                CheckBox chkApCatalogoDetalleListaDeNA_OK_X = (CheckBox)this.gvGruposGrowing.SelectedRow.Cells[5].Controls[0];
                CheckBox chkApListaDeS_A_G_N = (CheckBox)this.gvGruposGrowing.SelectedRow.Cells[6].Controls[0];
                CheckBox chkApCatalogoDetalleListaDeS_A_G_N = (CheckBox)this.gvGruposGrowing.SelectedRow.Cells[7].Controls[0];
                CheckBox chkActivo2 = (CheckBox)this.gvGruposGrowing.SelectedRow.Cells[12].Controls[0];

                this.chkParametroAplicaListaDeNA_OK_X.Checked = chkApCatalogoDetalleListaDeNA_OK_X.Checked;
                this.chkParametroAplicaListaDeS_A_G_N.Checked = chkApCatalogoDetalleListaDeS_A_G_N.Checked;
                this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Checked = chkApListaDeNA_OK_X.Checked;
                this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Checked = chkApListaDeS_A_G_N.Checked;

                this.hidApCatalogoDetalleListaDeNA_OK_X.Value = Convert.ToString(chkApCatalogoDetalleListaDeNA_OK_X.Checked);
                this.hidApCatalogoDetalleListaDeS_A_G_N.Value = Convert.ToString(chkApCatalogoDetalleListaDeS_A_G_N.Checked);
                this.hidApListaDeNA_OK_X.Value = Convert.ToString(chkApListaDeNA_OK_X.Checked);
                this.hidApListaDeS_A_G_N.Value = Convert.ToString(chkApListaDeS_A_G_N.Checked);

                if (this.hidApListaDeNA_OK_X.Value != "False")
                { this.chkParametroAplicaListaDeNA_OK_X.Enabled = true; }
                else
                { this.chkParametroAplicaListaDeNA_OK_X.Enabled = false; }

                if (this.hidApListaDeS_A_G_N.Value != "False")
                { this.chkParametroAplicaListaDeS_A_G_N.Enabled = true; }
                else
                { this.chkParametroAplicaListaDeS_A_G_N.Enabled = false; }

                if (this.hidApCatalogoDetalleListaDeNA_OK_X.Value != "False")
                { this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Enabled = true;
                  this.chkParametroNValoresListaNA_OK_X.Enabled = true;
                }
                else
                { this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Enabled = false;
                  this.chkParametroNValoresListaNA_OK_X.Enabled = true;
                }

                if (this.hidApCatalogoDetalleListaDeS_A_G_N.Value != "False")
                { this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Enabled = true;
                  this.chkParametroNValoresListaDeS_A_G_N.Enabled = true;
                }
                else
                { this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Enabled = false;
                  this.chkParametroNValoresListaDeS_A_G_N.Enabled = false;
                }

                //this.chkCatalogoActivo_S_A_G_N.Checked = false;
                //this.txtCatalogoNombreES_S_A_G_N.Text = "";
                //this.txtCatalogoNombreEN_S_A_G_N.Text = "";
                //this.chkCatalogoActivo_NA_OK_X.Checked = false;
                //this.txtCatalogoNombreES_NA_OK_X.Text = "";
                //this.txtCatalogoNombreEN_NA_OK_X.Text = "";
                //this.chkParametroActivo.Checked = true;
                //this.txtParametroNombreES.Text = "";
                //this.txtParametroNombreEN.Text = "";
                //this.txtParametroPuntajeAsignado.Text = "";
                //this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Checked = false;
                //this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Checked = false;
                //this.chkParametroAplicaListaDeNA_OK_X.Checked = false;
                //this.chkParametroAplicaListaDeS_A_G_N.Checked = false;
                //this.chkParametroNValoresListaNA_OK_X.Checked = false;
                //this.chkParametroNValoresListaDeS_A_G_N.Checked = false;
            }
            else
            {
                strScript = "Debe seleccionar el registro.";
            }

            if (this.gvGruposGrowing.Rows.Count > 0)
            {
                this.gvGruposGrowing.UseAccessibleHeader = true;
                this.gvGruposGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        catch (Exception ex)
        {
            strScript = "Error al inicializar objeto:\\n" + ex.Message;
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }
    protected void gvGruposGrowing_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        //e.Row.Cells[1].Visible = false;
        e.Row.Cells[4].Visible = false; 
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
        e.Row.Cells[10].Visible = false;
        e.Row.Cells[11].Visible = false;
        e.Row.Cells[12].Visible = false;


        if (Session["Locale"].ToString() == "es-MX")
        {
            e.Row.Cells[2].Visible = true;
            e.Row.Cells[3].Visible = false;
        }
        else
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = true;
        }
    }
    protected void gvGruposGrowing_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.gvGruposGrowing.Rows.Count > 0)
        {
            this.gvGruposGrowing.UseAccessibleHeader = true;
            this.gvGruposGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvGruposGrowing, "Select$" + e.Row.RowIndex);
        }
    }           
    protected void gvGruposGrowing_PreRender(object sender, EventArgs e)
    {
        if (this.gvGruposGrowing.Rows.Count > 0)
        {
            this.gvGruposGrowing.UseAccessibleHeader = true;
            this.gvGruposGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    public bool ValidarValoresParametro()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:\\n";
        string strMensajeEN = "Please specify the following fields:\\n";

        if (this.txtParametroNombreES.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Español\\n";
            strMensajeEN = strMensajeEN + "* Spanish Name\\n";
            blnResult = false;
        }

        if (this.txtParametroNombreEN.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Ingles\\n";
            strMensajeEN = strMensajeEN + "* English name\\n";
            blnResult = false;
        }

        if (this.txtParametroPuntajeAsignado.Text == "")
        {
            strMensaje = strMensaje + "* Puntaje Asignado\\n";
            strMensajeEN = strMensajeEN + "* Assigned points\\n";
            blnResult = false;
        }
        else
        {
            if (this.txtParametroPuntajeAsignado.Text == "0")
            {
                strMensaje = strMensaje + "* El Puntaje Asignado debe se mayor a 0\\n";
                strMensajeEN = strMensajeEN + "* The assigned points must be greater than 0\\n";
                blnResult = false;
            }
        }


        if (blnResult == false)
        {
            strMensaje = "<script language='javascript'> popUpAlert('" + strMensaje + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
        }

        return blnResult;
    }
    protected void btnParametroSave_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvParametrosPorGrupo.Rows.Count > 0)
        {
            this.gvParametrosPorGrupo.UseAccessibleHeader = true;
            this.gvParametrosPorGrupo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        ParametroGrupoDeGrowing objParametroGrupo = new ParametroGrupoDeGrowing();

        string strScript = "";
        bool blnResultado = false;
        bool blnExito = true;

        if (ValidarValoresParametro())
        {
            try
            {
                objParametroGrupo.IdGrupoGrowing = Convert.ToInt32(this.hidIdGrupoGrowing.Value);
                if (this.hidIdParametroGrupoGrowing.Value == "")
                {
                    this.hidIdParametroGrupoGrowing.Value = "0";
                }
                objParametroGrupo.IdParametroPorGrupoGrowing = Convert.ToInt32(this.hidIdParametroGrupoGrowing.Value);

                objParametroGrupo.NombreES = this.txtParametroNombreES.Text;
                objParametroGrupo.NombreEN = this.txtParametroNombreEN.Text;
                objParametroGrupo.AplicaListaDeNA_OK_X = this.chkParametroAplicaListaDeNA_OK_X.Checked;
                objParametroGrupo.AplicaCatalogoDetalleDeListaDeNA_OK_X = this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Checked;
                objParametroGrupo.NValoresSeleccionableParaDetalleDeListaDeNA_OK_X = this.chkParametroNValoresListaNA_OK_X.Checked;
                objParametroGrupo.AplicaListaDeS_A_G_N = this.chkParametroAplicaListaDeS_A_G_N.Checked;
                objParametroGrupo.AplicaCatalogoDetalleDeListaDeS_A_G_N = this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Checked;
                objParametroGrupo.NValoresSeleccionableParaDetalleDeListaDeS_A_G_N = this.chkParametroNValoresListaDeS_A_G_N.Checked;
                objParametroGrupo.PuntajeAsignado = Convert.ToInt16(this.txtParametroPuntajeAsignado.Text);
                objParametroGrupo.Activo = this.chkParametroActivo.Checked;
                objParametroGrupo.IdUsuario = Convert.ToInt32(Session["userIDInj"]);

                if (this.hidEsEnEspanol.Value == "true")
                {
                    objParametroGrupo.EsEnEspanol = true;
                    //if (this.gvParametrosPorGrupo.SelectedIndex >= 0)
                    //{
                        if (this.hidIdParametroGrupoGrowing.Value != "0")
                        { blnResultado = objParametroGrupo.MantenimientoCatalogo(2); }
                        else
                        { blnResultado = objParametroGrupo.MantenimientoCatalogo(1); }

                    //}

                    if (blnResultado == true)
                    {
                        strScript = "Registro guardado correctamente.\\n";
                        blnExito = true;
                    }
                    else
                    {
                        strScript = "Error al guardar el registro. " + objParametroGrupo.ErrorMessage;
                        blnExito = false;
                    }
                }

                else
                {
                    objParametroGrupo.EsEnEspanol = false;
                    if (this.gvParametrosPorGrupo.SelectedIndex >= 0)
                    {
                        if (this.hidIdParametroGrupoGrowing.Value != "")
                        { blnResultado = objParametroGrupo.MantenimientoCatalogo(2); }
                        else
                        { blnResultado = objParametroGrupo.MantenimientoCatalogo(1); }
                    }

                    if (blnResultado == true)
                    {
                        strScript = "Record successfully saved.\\n";
                        blnExito = true;
                    }
                    else
                    {
                        strScript = "Failed to save the record." + objParametroGrupo.ErrorMessage;
                        blnExito = false;
                    }
                }
                if (blnResultado == true)
                {

                    this.chkParametroActivo.Checked = true;
                    this.txtParametroNombreES.Text = "";
                    this.txtParametroNombreEN.Text = "";
                    this.txtParametroPuntajeAsignado.Text = "";
                    this.txtParametroPuntajeAsignadoNoPlantacion.Text = "";
                    //this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Checked = false;
                    //this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Checked = false;
                    //this.chkParametroAplicaListaDeNA_OK_X.Checked = false;
                    //this.chkParametroAplicaListaDeS_A_G_N.Checked = false;
                    //this.chkParametroNValoresListaNA_OK_X.Checked = false;
                    //this.chkParametroNValoresListaDeS_A_G_N.Checked = false;

                    this.gvParametrosPorGrupo.SelectedIndex = -1;

                    this.hidIdParametroGrupoGrowing.Value = "";

                    this.dstParametros.DataBind();
                    this.gvParametrosPorGrupo.DataBind();
                }

            }
            catch (Exception exError)
            {
                if (this.hidEsEnEspanol.Value == "true")
                {
                    strScript = "Error al guardar el registro en la clase ParametroGrupoDeGrowing:MantenimientoCatalogo().\\n" + exError.Message;
                }
                else
                {
                    strScript = "Failed to save the record to the class ParametroGrupoDeGrowing:MantenimientoCatalogo().\\n" + exError.Message;
                }
                blnExito = false;

            }
            finally
            {
                objParametroGrupo = null;
            }
        }

        if (!string.IsNullOrEmpty(strScript))
        {
            if (blnExito == true)
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','ok');</script>";
            }
            else
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }
    protected void btnParametroCancelar_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvParametrosPorGrupo.Rows.Count > 0)
        {
            this.gvParametrosPorGrupo.UseAccessibleHeader = true;
            this.gvParametrosPorGrupo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        this.chkParametroActivo.Checked = false;
        this.txtParametroNombreES.Text = "";
        this.txtParametroNombreEN.Text = "";
        this.txtParametroPuntajeAsignado.Text = "";
        this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Checked = false;
        this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Checked = false;
        this.chkParametroAplicaListaDeNA_OK_X.Checked = false;
        this.chkParametroAplicaListaDeS_A_G_N.Checked = false;
        this.chkParametroNValoresListaNA_OK_X.Checked = false;
        this.chkParametroNValoresListaDeS_A_G_N.Checked = false;

        this.gvGruposGrowing.SelectedIndex = -1;
        this.gvParametrosPorGrupo.SelectedIndex = -1;
        
        this.hidApCatalogoDetalleListaDeNA_OK_X.Value = "";
        this.hidApCatalogoDetalleListaDeS_A_G_N.Value = "";
        this.hidApListaDeNA_OK_X.Value = "";
        this.hidApListaDeS_A_G_N.Value = "";
        this.hidIdGrupoGrowing.Value = "";
        this.hidIdParametroGrupoGrowing.Value = "";

        this.dstParametros.DataBind();
        this.gvParametrosPorGrupo.DataBind();
    }
    protected void gvParametrosPorGrupo_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
    }
    protected void gvParametrosPorGrupo_PreRender(object sender, EventArgs e)
    {
        if (this.gvParametrosPorGrupo.Rows.Count > 0)
        {
            this.gvParametrosPorGrupo.UseAccessibleHeader = true;
            this.gvParametrosPorGrupo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void gvParametrosPorGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (this.gvParametrosPorGrupo.Rows.Count > 0)
        //{
        //    this.gvParametrosPorGrupo.UseAccessibleHeader = true;
        //    this.gvParametrosPorGrupo.HeaderRow.TableSection = TableRowSection.TableHeader;
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvParametrosPorGrupo, "Select$" + e.Row.RowIndex);
        }

    }
    protected void gvParametrosPorGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";

        // Vuelvo a generar visible el header para el tema del sorting:
        //if (this.gvParametrosPorGrupo.Rows.Count > 0)
        //{
        //    this.gvParametrosPorGrupo.UseAccessibleHeader = true;
        //    this.gvParametrosPorGrupo.HeaderRow.TableSection = TableRowSection.TableHeader;
        //}

        try
        {

            if (this.gvParametrosPorGrupo.SelectedIndex != -1)
            {
                this.hidIdParametroGrupoGrowing.Value = this.gvParametrosPorGrupo.SelectedRow.Cells[0].Text;
                this.txtParametroNombreEN.Text = this.gvParametrosPorGrupo.SelectedRow.Cells[3].Text;
                this.txtParametroNombreES.Text = this.gvParametrosPorGrupo.SelectedRow.Cells[2].Text;
                this.txtParametroPuntajeAsignado.Text = this.gvParametrosPorGrupo.SelectedRow.Cells[10].Text;

                CheckBox chkApListaDeNA_OK_X = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[4].Controls[0];
                CheckBox chkApCatalogoDetalleListaDeNA_OK_X = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[5].Controls[0];
                CheckBox chkNValoresNA_OK_X = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[6].Controls[0];
                CheckBox chkApListaDeS_A_G_N = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[7].Controls[0];
                CheckBox chkApCatalogoDetalleListaDeS_A_G_N = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[8].Controls[0];
                CheckBox chkNValoresS_A_G_N = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[9].Controls[0];
                CheckBox chkActivo2 = (CheckBox)this.gvParametrosPorGrupo.SelectedRow.Cells[11].Controls[0];

                if (chkApCatalogoDetalleListaDeNA_OK_X.Checked == true)
                { 
                    this.txtCatalogoNombreEN_NA_OK_X.Enabled = true;
                    this.txtCatalogoNombreES_NA_OK_X.Enabled = true;
                    this.chkCatalogoActivo_NA_OK_X.Enabled = true;
                    this.btnCatalogoSave_NA_OK_X.Enabled = true;
                }
                else
                {
                    this.txtCatalogoNombreEN_NA_OK_X.Enabled = false;
                    this.txtCatalogoNombreES_NA_OK_X.Enabled = false;
                    this.chkCatalogoActivo_NA_OK_X.Enabled = false;
                    this.btnCatalogoSave_NA_OK_X.Enabled = false;
                }

                if (chkApCatalogoDetalleListaDeS_A_G_N.Checked == true)
                {
                    this.txtCatalogoNombreEN_S_A_G_N.Enabled = true;
                    this.txtCatalogoNombreES_S_A_G_N.Enabled = true;
                    this.chkCatalogoActivo_S_A_G_N.Enabled = true;
                    this.btnCatalogoSave_S_A_G_N.Enabled = true;
                }
                else
                {
                    this.txtCatalogoNombreEN_S_A_G_N.Enabled = false;
                    this.txtCatalogoNombreES_S_A_G_N.Enabled = false;
                    this.chkCatalogoActivo_S_A_G_N.Enabled = false;
                    this.btnCatalogoSave_S_A_G_N.Enabled = false;
                }

                this.chkParametroActivo.Checked = chkActivo2.Checked;
                this.chkParametroAplicaListaDeNA_OK_X.Checked = chkApListaDeNA_OK_X.Checked;
                this.chkParametroAplicaCatalogoDetalleListaDeNA_OK_X.Checked = chkApCatalogoDetalleListaDeNA_OK_X.Checked;
                this.chkParametroNValoresListaNA_OK_X.Checked = chkNValoresNA_OK_X.Checked;
                this.chkParametroAplicaListaDeS_A_G_N.Checked = chkApListaDeS_A_G_N.Checked;
                this.chkParametroAplicaCatalogoDetalleListaDeS_A_G_N.Checked = chkApCatalogoDetalleListaDeS_A_G_N.Checked;
                this.chkParametroNValoresListaDeS_A_G_N.Checked = chkNValoresS_A_G_N.Checked;

                if (this.hidEsEnEspanol.Value == "true")
                {
                    this.lblDatoParametro.Text = this.txtParametroNombreES.Text;
                    this.lblDatoParametro0.Text = this.txtParametroNombreES.Text;
                }
                else
                {
                    this.lblDatoParametro.Text = this.txtParametroNombreEN.Text;
                    this.lblDatoParametro0.Text = this.txtParametroNombreEN.Text;
                }
                

            }
            else
            {
                strScript = "Debe seleccionar el registro.";
            }
        }

        catch (Exception ex)
        {
            strScript = "Error al inicializar objeto:\\n" + ex.Message;
        }
        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
        
    }
    public bool ValidarValoresNA_OK_X()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:\\n";
        string strMensajeEN = "Please specify the following fields:\\n";

        if (this.txtCatalogoNombreES_NA_OK_X.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Español\\n";
            strMensajeEN = strMensajeEN + "* Spanish Name\\n";
            blnResult = false;
        }

        if (this.txtCatalogoNombreEN_NA_OK_X.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Ingles\\n";
            strMensajeEN = strMensajeEN + "* English name\\n";
            blnResult = false;
        }

        if (blnResult == false)
        {
            strMensaje = "<script language='javascript'> popUpAlert('" + strMensaje + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
        }

        return blnResult;
    }
    public bool ValidarValoresS_A_G_N()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:\\n";
        string strMensajeEN = "Please specify the following fields:\\n";

        if (this.txtCatalogoNombreES_S_A_G_N.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Español\\n";
            strMensajeEN = strMensajeEN + "* Spanish Name\\n";
            blnResult = false;
        }

        if (this.txtCatalogoNombreEN_S_A_G_N.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Ingles\\n";
            strMensajeEN = strMensajeEN + "* English name\\n";
            blnResult = false;
        }

        if (blnResult == false)
        {
            strMensaje = "<script language='javascript'> popUpAlert('" + strMensaje + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
        }

        return blnResult;
    }
    protected void gvCatalogoListaN_OK_X_PreRender(object sender, EventArgs e)
    {
        if (this.gvCatalogoListaN_OK_X.Rows.Count > 0)
        {
            this.gvCatalogoListaN_OK_X.UseAccessibleHeader = true;
            this.gvCatalogoListaN_OK_X.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void gvCatalogoListaN_OK_X_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
    }
    protected void gvCatalogoListaN_OK_X_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.gvCatalogoListaN_OK_X.Rows.Count > 0)
        {
            this.gvCatalogoListaN_OK_X.UseAccessibleHeader = true;
            this.gvCatalogoListaN_OK_X.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvCatalogoListaN_OK_X, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvCatalogoListaN_OK_X_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";

        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvCatalogoListaN_OK_X.Rows.Count > 0)
        {
            this.gvCatalogoListaN_OK_X.UseAccessibleHeader = true;
            this.gvCatalogoListaN_OK_X.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        try
        {

            if (this.gvCatalogoListaN_OK_X.SelectedIndex != -1)
            {
                this.hidPKNA_OK_X.Value = this.gvCatalogoListaN_OK_X.SelectedRow.Cells[0].Text;
                this.txtCatalogoNombreES_NA_OK_X.Text = this.gvCatalogoListaN_OK_X.SelectedRow.Cells[2].Text;
                this.txtCatalogoNombreEN_NA_OK_X.Text = this.gvCatalogoListaN_OK_X.SelectedRow.Cells[3].Text;
                CheckBox chkActivo2 = (CheckBox)this.gvCatalogoListaN_OK_X.SelectedRow.Cells[4].Controls[0];
                this.chkCatalogoActivo_NA_OK_X.Checked = chkActivo2.Checked;
            }
            else
            {
                strScript = "Debe seleccionar el registro.";
            }
        }

        catch (Exception ex)
        {
            strScript = "Error al inicializar objeto:\\n" + ex.Message;
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }
    protected void gvCatalogoListaS_A_G_N_PreRender(object sender, EventArgs e)
    {
        if (this.gvCatalogoListaS_A_G_N.Rows.Count > 0)
        {
            this.gvCatalogoListaS_A_G_N.UseAccessibleHeader = true;
            this.gvCatalogoListaS_A_G_N.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void gvCatalogoListaS_A_G_N_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
    }
    protected void gvCatalogoListaS_A_G_N_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.gvCatalogoListaS_A_G_N.Rows.Count > 0)
        {
            this.gvCatalogoListaS_A_G_N.UseAccessibleHeader = true;
            this.gvCatalogoListaS_A_G_N.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvCatalogoListaS_A_G_N, "Select$" + e.Row.RowIndex);
        }

    }
    protected void gvCatalogoListaS_A_G_N_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";

        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvCatalogoListaS_A_G_N.Rows.Count > 0)
        {
            this.gvCatalogoListaS_A_G_N.UseAccessibleHeader = true;
            this.gvCatalogoListaS_A_G_N.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        try
        {

            if (this.gvCatalogoListaS_A_G_N.SelectedIndex != -1)
            {
                this.hidPKS_A_G_N.Value = this.gvCatalogoListaS_A_G_N.SelectedRow.Cells[0].Text;
                this.txtCatalogoNombreES_S_A_G_N.Text = this.gvCatalogoListaS_A_G_N.SelectedRow.Cells[2].Text;
                this.txtCatalogoNombreEN_S_A_G_N.Text = this.gvCatalogoListaS_A_G_N.SelectedRow.Cells[3].Text;
                CheckBox chkActivo2 = (CheckBox)this.gvCatalogoListaS_A_G_N.SelectedRow.Cells[4].Controls[0];
                this.chkCatalogoActivo_S_A_G_N.Checked = chkActivo2.Checked;
            }
            else
            {
                strScript = "Debe seleccionar el registro.";
            }
        }

        catch (Exception ex)
        {
            strScript = "Error al inicializar objeto:\\n" + ex.Message;
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }

    protected void btnCatalogoCancelar_NA_OK_X_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvCatalogoListaN_OK_X.Rows.Count > 0)
        {
            this.gvCatalogoListaN_OK_X.UseAccessibleHeader = true;
            this.gvCatalogoListaN_OK_X.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        this.chkCatalogoActivo_NA_OK_X.Checked = false;
        this.txtCatalogoNombreES_NA_OK_X.Text = "";
        this.txtCatalogoNombreEN_NA_OK_X.Text = "";
        this.gvCatalogoListaN_OK_X.SelectedIndex = -1;
        this.dstCatalogoListaN_OK_X.DataBind();
        this.gvCatalogoListaN_OK_X.DataBind();
    }
    protected void btnCatalogoCancelar_S_A_G_N_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvCatalogoListaS_A_G_N.Rows.Count > 0)
        {
            this.gvCatalogoListaS_A_G_N.UseAccessibleHeader = true;
            this.gvCatalogoListaS_A_G_N.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        this.chkCatalogoActivo_S_A_G_N.Checked = false;
        this.txtCatalogoNombreES_S_A_G_N.Text = "";
        this.txtCatalogoNombreEN_S_A_G_N.Text = "";
        this.gvCatalogoListaS_A_G_N.SelectedIndex = -1;
        this.dstCatalogoListaS_A_G_N.DataBind();
        this.gvCatalogoListaS_A_G_N.DataBind();
    }
    protected void btnCatalogoSave_S_A_G_N_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvCatalogoListaS_A_G_N.Rows.Count > 0)
        {
            this.gvCatalogoListaS_A_G_N.UseAccessibleHeader = true;
            this.gvCatalogoListaS_A_G_N.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        CatalogoListaS_A_G_N objCatalogoListaS_A_G_N = new CatalogoListaS_A_G_N();

        string strScript = "";
        bool blnResultado = false;
        int intCount = 0;
        bool blnExito = true;

        if (ValidarValoresS_A_G_N())
        {
            try
            {
                if (this.hidPKS_A_G_N.Value == "")
                {
                    this.hidPKS_A_G_N.Value = "0";
                }

                objCatalogoListaS_A_G_N.IdCatalogoListaS_A_G_N = Convert.ToInt32(this.hidPKS_A_G_N.Value);
                objCatalogoListaS_A_G_N.DescripcionES = this.txtCatalogoNombreES_S_A_G_N.Text;
                objCatalogoListaS_A_G_N.DescripcionEN = this.txtCatalogoNombreEN_S_A_G_N.Text;
                objCatalogoListaS_A_G_N.Activo = this.chkCatalogoActivo_S_A_G_N.Checked;

                while (intCount <= this.gvParametrosPorGrupo.Rows.Count - 1)
                {
                    CheckBox chKSeleccion = this.gvParametrosPorGrupo.Rows[intCount].Cells[12].FindControl("chkSeleccion") as CheckBox;
                    if (chKSeleccion.Checked)
                    {
                        objCatalogoListaS_A_G_N.IdParametroPorGrupoGrowing = Convert.ToInt32(this.gvParametrosPorGrupo.Rows[intCount].Cells[0].Text);
                        if (this.hidEsEnEspanol.Value == "true")
                        {
                            objCatalogoListaS_A_G_N.EsEnEspanol = true;
                            if (this.gvCatalogoListaS_A_G_N.SelectedIndex >= 0)
                            {
                                // if (this.hidPKNA_OK_X.Value != "")
                                blnResultado = objCatalogoListaS_A_G_N.MantenimientoCatalogo(2);
                            }
                            else
                            {
                                blnResultado = objCatalogoListaS_A_G_N.MantenimientoCatalogo(1);
                            }

                            if (blnResultado == true)
                            {
                                strScript = "Registro guardado correctamente.\\n";
                                blnExito = true;
                            }
                            else
                            {
                                strScript = "Error al guardar el registro. " + objCatalogoListaS_A_G_N.ErrorMessage;
                                blnExito = false;
                            }
                        }

                        else
                        {
                            objCatalogoListaS_A_G_N.EsEnEspanol = false;
                            if (this.gvCatalogoListaN_OK_X.SelectedIndex >= 0)
                            {
                                if (this.hidPKS_A_G_N.Value != "")
                                { blnResultado = objCatalogoListaS_A_G_N.MantenimientoCatalogo(2); }
                                else
                                { blnResultado = objCatalogoListaS_A_G_N.MantenimientoCatalogo(1); }
                            }

                            if (blnResultado == true)
                            {
                                strScript = "Record successfully saved.\\n";
                                blnExito = true;
                            }
                            else
                            {
                                strScript = "Failed to save the record." + objCatalogoListaS_A_G_N.ErrorMessage;
                                blnExito = false;
                            }
                        }
                    }
                    intCount = intCount + 1;
                }

                if (blnResultado == true)
                {
                    this.chkCatalogoActivo_S_A_G_N.Checked = true;
                    this.txtCatalogoNombreES_S_A_G_N.Text = "";
                    this.txtCatalogoNombreEN_S_A_G_N.Text = "";

                    this.dstCatalogoListaS_A_G_N.DataBind();
                    this.gvCatalogoListaS_A_G_N.DataBind();
                }

            }
            catch (Exception exError)
            {
                if (this.hidEsEnEspanol.Value == "true")
                {
                    strScript = "Error al guardar el registro en la clase CatalogoListaS_A_G_N:MantenimientoCatalogo().\\n" + exError.Message;
                }
                else
                {
                    strScript = "Failed to save the record to the class CatalogoListaS_A_G_N:MantenimientoCatalogo().\\n" + exError.Message;
                }
                blnExito = false;
            }
            finally
            {
                objCatalogoListaS_A_G_N = null;
            }
        }

        if (!string.IsNullOrEmpty(strScript))
        {
            if (blnExito == true)
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','ok');</script>";
            }
            else
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            }
            
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }
    protected void btnCatalogoSave_NA_OK_X_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvCatalogoListaN_OK_X.Rows.Count > 0)
        {
            this.gvCatalogoListaN_OK_X.UseAccessibleHeader = true;
            this.gvCatalogoListaN_OK_X.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        CatalogoListaNA_OK_X objCatalogoListaNA_OK_X = new CatalogoListaNA_OK_X();

        string strScript = "";
        bool blnResultado = false;
        int intCount = 0;
        bool blnExito = true;
        

        if (ValidarValoresNA_OK_X())
        {
            try
            {
 
                
                //objCatalogoListaNA_OK_X.IdParametroPorGrupoGrowing = Convert.ToInt32(this.hidIdParametroGrupoGrowing.Value);
                if (this.hidPKNA_OK_X.Value == "")
                {
                    this.hidPKNA_OK_X.Value = "0";
                }
                objCatalogoListaNA_OK_X.IdCatalogoListaNA_OK_X = Convert.ToInt32(this.hidPKNA_OK_X.Value);
                objCatalogoListaNA_OK_X.DescripcionES = this.txtCatalogoNombreES_NA_OK_X.Text;
                objCatalogoListaNA_OK_X.DescripcionEN = this.txtCatalogoNombreEN_NA_OK_X.Text;
                objCatalogoListaNA_OK_X.Activo = this.chkCatalogoActivo_NA_OK_X.Checked;
                
                while (intCount <= this.gvParametrosPorGrupo.Rows.Count - 1)
                {
                    CheckBox chKSeleccion = this.gvParametrosPorGrupo.Rows[intCount].Cells[12].FindControl("chkSeleccion") as CheckBox;
                    if (chKSeleccion.Checked)
                    {
                        objCatalogoListaNA_OK_X.IdParametroPorGrupoGrowing = Convert.ToInt32(this.gvParametrosPorGrupo.Rows[intCount].Cells[0].Text);
                        if (this.hidEsEnEspanol.Value == "true")
                        {
                            objCatalogoListaNA_OK_X.EsEnEspanol = true;
                            if (this.gvCatalogoListaN_OK_X.SelectedIndex >= 0)
                            {
                               // if (this.hidPKNA_OK_X.Value != "")
                                 blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(2); 
                            }
                            else
                            { 
                                blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(1); 
                            }

                            if (blnResultado == true)
                            {
                                strScript = "Registro guardado correctamente.\\n";
                                blnExito = true;
                            }
                            else
                            {
                                strScript = "Error al guardar el registro. " + objCatalogoListaNA_OK_X.ErrorMessage;
                                blnExito = false;
                            }
                        }

                        else
                        {
                            objCatalogoListaNA_OK_X.EsEnEspanol = false;
                            if (this.gvCatalogoListaN_OK_X.SelectedIndex >= 0)
                            {
                                if (this.hidPKNA_OK_X.Value != "")
                                { blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(2); }
                                else
                                { blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(1); }
                            }

                            if (blnResultado == true)
                            {
                                strScript = "Record successfully saved.\\n";
                                blnExito = true;
                            }
                            else
                            {
                                strScript = "Failed to save the record." + objCatalogoListaNA_OK_X.ErrorMessage;
                                blnExito = true;
                            }
                        }
                    }
                    intCount = intCount + 1;
                }
                                
                //if (this.hidEsEnEspanol.Value == "true")
                //{
                //    objCatalogoListaNA_OK_X.EsEnEspanol = true;
                //    if (this.gvCatalogoListaN_OK_X.SelectedIndex >= 0)
                //    {
                //        if (this.hidPKNA_OK_X.Value != "")
                //        {blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(2);}
                //        else
                //        {blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(1);}
                        
                //    }

                //    if (blnResultado == true)
                //    {
                //        strScript = "Registro guardado correctamente.\\n";
                //    }
                //    else
                //    {
                //        strScript = "Error al guardar el registro. " + objCatalogoListaNA_OK_X.ErrorMessage;
                //    }
                //}

                //else
                //{
                //    objCatalogoListaNA_OK_X.EsEnEspanol = false;
                //    if (this.gvCatalogoListaN_OK_X.SelectedIndex >= 0)
                //    {
                //        if (this.hidPKNA_OK_X.Value != "")
                //        { blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(2); }
                //        else
                //        { blnResultado = objCatalogoListaNA_OK_X.MantenimientoCatalogo(1); }
                //    }

                //    if (blnResultado == true)
                //    {
                //        strScript = "Record successfully saved.\\n";
                //    }
                //    else
                //    {
                //        strScript = "Failed to save the record." + objCatalogoListaNA_OK_X.ErrorMessage;
                //    }
                //}
                if (blnResultado == true)
                {

                    this.chkCatalogoActivo_NA_OK_X.Checked = false;
                    this.txtCatalogoNombreES_NA_OK_X.Text = "";
                    this.txtCatalogoNombreEN_NA_OK_X.Text  = "";

                    this.gvCatalogoListaN_OK_X.SelectedIndex = -1;

                    this.dstCatalogoListaN_OK_X.DataBind();
                    this.gvCatalogoListaN_OK_X.DataBind();
                }

            }
            catch (Exception exError)
            {
                if (this.hidEsEnEspanol.Value == "true")
                {
                    strScript = "Error al guardar el registro en la clase CatalogoListaNA_OK_X:MantenimientoCatalogo().\\n" + exError.Message;
                }
                else
                {
                    strScript = "Failed to save the record to the class CatalogoListaNA_OK_X:MantenimientoCatalogo().\\n" + exError.Message;
                }
                blnExito = false;
            }
            finally
            {
                objCatalogoListaNA_OK_X = null;
            }
        }

        if (!string.IsNullOrEmpty(strScript))
        {
            if (blnExito == true)
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','ok');</script>";
            }
            else
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            }
            
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }
}