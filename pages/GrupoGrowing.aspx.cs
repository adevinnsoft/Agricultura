using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class pages_GrupoGrowing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strScript = "";
        bool blnDebugSession = false;
        this.txtPuntajeAsignadoPlantacion.Attributes.Add("onKeyPress", "Javascript:return OnlyNumbers(event);");
        this.txtPuntajeAsignadoNoPlantacion.Attributes.Add("onKeyPress", "Javascript:return OnlyNumbers(event);");
        
        try
        {
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
            strScript = "Error al inicializar la pagina:<br/>" + exError.ToString();
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);

        }
    }

  
    public bool ValidarValores()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:<br/>";
        string strMensajeEN = "Please specify the following fields:<br/>";

        if (this.txtNombreEspanol.Text == "")
        {
            strMensaje = strMensaje + "&nbsp;* Nombre Español<br/>";
            strMensajeEN = strMensajeEN + "&nbsp;* Spanish Name<br/>";
            blnResult = false;
        }

        if (this.txtNombreIngles.Text == "")
        {
            strMensaje = strMensaje + "&nbsp;* Nombre Ingles<br/>";
            strMensajeEN = strMensajeEN + "&nbsp;* English name<br/>";
            blnResult = false;
        }
             
        if (blnResult == false)
        {
            strMensaje = "<script language='javascript'>popUpAlert('" + strMensaje + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
        }

        return blnResult;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvGruposGrowing.Rows.Count > 0)
        {
            this.gvGruposGrowing.UseAccessibleHeader = true;
            this.gvGruposGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        
        GrupoDeGrowing objGrupoDeGrowing = new GrupoDeGrowing();

        string strScript = "";
        bool blnResultado = false;
        bool blnExito = true;

        if (ValidarValores())
        {
            try
            {
                var puntajePlantacion = this.txtPuntajeAsignadoPlantacion.Text.Equals("0") || this.txtPuntajeAsignadoPlantacion.Text.Equals(string.Empty) ? 0 : Convert.ToDecimal(this.txtPuntajeAsignadoPlantacion.Text);
                var puntajeNoPlantacion = this.txtPuntajeAsignadoNoPlantacion.Text.Equals("0") || this.txtPuntajeAsignadoNoPlantacion.Text.Equals(string.Empty) ? 0 : Convert.ToDecimal(this.txtPuntajeAsignadoNoPlantacion.Text);
                var validoPlantacion = puntajePlantacion > 0 ? true : false;
                var validoNoPlantacion = puntajeNoPlantacion > 0 ? true : false;
                objGrupoDeGrowing.NombreES = this.txtNombreEspanol.Text.Trim();
                objGrupoDeGrowing.NombreEN = this.txtNombreIngles.Text.Trim();
                objGrupoDeGrowing.IdGrupoGrowing = Convert.ToInt32(this.gvGruposGrowing.SelectedRow == null ? "0" : this.gvGruposGrowing.SelectedRow.Cells[0].Text);
                objGrupoDeGrowing.AplicaListaDeNA_OK_X = this.chkAplicaListaDeNA_OK_X.Checked;
                objGrupoDeGrowing.AplicaCatalogoDetalleDeListaDeNA_OK_X = this.chkAplicaCatalogoDetalleListaDeNA_OK_X.Checked;
                objGrupoDeGrowing.AplicaListaDeS_A_G_N = this.chkAplicaListaDeS_A_G_N.Checked;
                objGrupoDeGrowing.AplicaCatalogoDetalleDeListaDeS_A_G_N = this.chkAplicaCatalogoDetalleListaDeS_A_G_N.Checked;
                objGrupoDeGrowing.ValidoParaPlantacion = validoPlantacion;
                objGrupoDeGrowing.ValidoParaNoPlantacion = validoNoPlantacion;
                objGrupoDeGrowing.PuntajeAsignadoParaPlantacion = puntajePlantacion;
                objGrupoDeGrowing.PuntajeAsignadoParaNoPlantacion = puntajeNoPlantacion;
                objGrupoDeGrowing.Activo = this.chkActivo.Checked;
                objGrupoDeGrowing.IdUsuario = Convert.ToInt32(Session["userIDInj"]);

                objGrupoDeGrowing.EsEnEspanol = this.hidEsEnEspanol.Value.Equals("true");
                if (objGrupoDeGrowing.IdGrupoGrowing == 0)
                {
                     blnResultado = objGrupoDeGrowing.MantenimientoCatalogo(1);
                }
                else
                {
                    blnResultado = objGrupoDeGrowing.MantenimientoCatalogo(2);
                }
               
               
                if (blnResultado == true)
                    {
                        strScript = objGrupoDeGrowing.EsEnEspanol ? "Registro guardado correctamente.<br/>" :"Record successfully saved.<br/>";
                        blnExito = true;
                        this.dstGrupoGrowing.DataBind();
                        this.gvGruposGrowing.DataBind();
                        this.chkActivo.Checked = true;
                        this.txtNombreEspanol.Text = "";
                        this.txtNombreIngles.Text = "";
                        this.chkActivo.Checked = true;
                        this.chkAplicaCatalogoDetalleListaDeNA_OK_X.Checked = false;
                        this.chkAplicaCatalogoDetalleListaDeS_A_G_N.Checked = false;
                        this.chkAplicaListaDeNA_OK_X.Checked = false;
                        this.chkAplicaListaDeS_A_G_N.Checked = false;
                        this.chkValidoParaNoPlantacion.Checked = false;
                        this.chkValidoParaPlantacion.Checked = false;
                        this.txtPuntajeAsignadoPlantacion.Text = "0";
                        this.txtPuntajeAsignadoNoPlantacion.Text = "0";
                        this.gvGruposGrowing.SelectedIndex = -1;
                        this.btnCancelar.Visible = false;
                        this.btnSave.Visible = false;
                        this.btnGuardar.Visible = true;
                        this.btnLimpiar.Visible = true;
                    }
                    else
                    {
                        strScript =  objGrupoDeGrowing.EsEnEspanol ?"Error al guardar el registro. " + objGrupoDeGrowing.ErrorMessage : "Failed to save the record."   + objGrupoDeGrowing.ErrorMessage;;
                        blnExito = false; 
                    }
            }
            catch (Exception exError)
            {
                if (this.hidEsEnEspanol.Value == "true")
                {
                    strScript = "Error al guardar el registro en la clase clsGrupoDeGrowing:MantenimientoCatalogo().<br/>" + exError.Message;
                }
                else
                {
                    strScript = "Failed to save the record to the class clsGrupoDeGrowing:MantenimientoCatalogo().<br/>" + exError.Message;
                }
                blnExito = false;
            }
            finally
            {
                objGrupoDeGrowing = null;
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

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvGruposGrowing.Rows.Count > 0)
        {
            this.gvGruposGrowing.UseAccessibleHeader = true;
            this.gvGruposGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        this.dstGrupoGrowing.DataBind();
        this.gvGruposGrowing.DataBind();
        this.chkActivo.Checked = true;
        this.txtNombreEspanol.Text = "";
        this.txtNombreIngles.Text = "";
        this.chkActivo.Checked = true;
        this.chkAplicaCatalogoDetalleListaDeNA_OK_X.Checked = false;
        this.chkAplicaCatalogoDetalleListaDeS_A_G_N.Checked = false;
        this.chkAplicaListaDeNA_OK_X.Checked = false;
        this.chkAplicaListaDeS_A_G_N.Checked = false;
        this.chkValidoParaNoPlantacion.Checked = false;
        this.chkValidoParaPlantacion.Checked = false;
        this.txtPuntajeAsignadoPlantacion.Text = "0";
        this.txtPuntajeAsignadoNoPlantacion.Text = "0";
        this.gvGruposGrowing.SelectedIndex = -1;

        this.btnCancelar.Visible = false;
        this.btnSave.Visible = false;
        this.btnGuardar.Visible = true;
        this.btnLimpiar.Visible = true;

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
                this.txtNombreEspanol.Text = this.gvGruposGrowing.SelectedRow.Cells[2].Text;
                this.txtNombreIngles.Text = this.gvGruposGrowing.SelectedRow.Cells[3].Text;
                bool chkApListaDeNA_OK_X = this.gvGruposGrowing.SelectedRow.Cells[4].Text.Equals("False") ? false : true;
                bool chkApCatalogoDetalleListaDeNA_OK_X = this.gvGruposGrowing.SelectedRow.Cells[5].Text.Equals("False") ? false : true;
                bool chkApListaDeS_A_G_N = this.gvGruposGrowing.SelectedRow.Cells[6].Text.Equals("False") ? false : true;
                bool chkApCatalogoDetalleListaDeS_A_G_N = this.gvGruposGrowing.SelectedRow.Cells[7].Text.Equals("False") ? false : true;
                this.txtPuntajeAsignadoPlantacion.Text = this.gvGruposGrowing.SelectedRow.Cells[8].Text;
                bool chkValidoPP = this.gvGruposGrowing.SelectedRow.Cells[9].Text.Equals("False") ? false : true;
                this.txtPuntajeAsignadoNoPlantacion.Text = this.gvGruposGrowing.SelectedRow.Cells[10].Text;
                bool chkValidoPNO = this.gvGruposGrowing.SelectedRow.Cells[11].Text.Equals("False") ? false : true;
                bool chkActivo2 = this.gvGruposGrowing.SelectedRow.Cells[12].Text.Equals("False") ? false : true;
                
                this.chkActivo.Checked = chkActivo2;
                this.chkAplicaListaDeNA_OK_X.Checked = chkApListaDeNA_OK_X;
                this.chkAplicaCatalogoDetalleListaDeNA_OK_X.Checked = chkApCatalogoDetalleListaDeNA_OK_X;
                this.chkAplicaListaDeS_A_G_N.Checked = chkApListaDeS_A_G_N;
                this.chkAplicaCatalogoDetalleListaDeS_A_G_N.Checked = chkApCatalogoDetalleListaDeS_A_G_N;
                this.chkValidoParaPlantacion.Checked = chkValidoPP;
                this.chkValidoParaNoPlantacion.Checked = chkValidoPNO;

                this.btnCancelar.Visible = true;
                this.btnSave.Visible = true;
                this.btnGuardar.Visible = false;
                this.btnLimpiar.Visible = false;

            }
            else
            {
                strScript = "Debe seleccionar el registro que desea Actualizar.";
            }
        }

        catch (Exception ex)
        {
            strScript = "Error al inicializar objeto:<br/>" + ex.Message;
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
        }
    }
    protected void gvGruposGrowing_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
        e.Row.Cells[10].Visible = false;
        e.Row.Cells[11].Visible = false;
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
}