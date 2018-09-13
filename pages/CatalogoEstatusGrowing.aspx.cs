using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class pages_CatalogoEstatusGrowing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strScript = "";
        bool blnDebugSession = false;

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
            strScript = "Error al inicializar la pagina:\\n" + exError.ToString();
        }

        if (strScript != "")
        {
            strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);

        }
    }

    protected void gvEstatusGrowing_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";

        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvEstatusGrowing.Rows.Count > 0)
        {
            this.gvEstatusGrowing.UseAccessibleHeader = true;
            this.gvEstatusGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        try
        {

            if (this.gvEstatusGrowing.SelectedIndex != -1)
            {
                this.txtNombreEspanol.Text = this.gvEstatusGrowing.SelectedRow.Cells[2].Text;
                this.txtNombreIngles.Text = this.gvEstatusGrowing.SelectedRow.Cells[3].Text;
                //CheckBox chkActivo2 = (CheckBox)this.gvEstatusGrowing.SelectedRow.Cells[6].Controls[0];
                //this.chkActivo.Checked = chkActivo2.Checked;
                if (this.gvEstatusGrowing.SelectedRow.Cells[4].Text.ToUpper() == "NO")
                {
                    this.chkActivo.Checked = false;
                }
                else
                {
                    this.chkActivo.Checked = true;
                }

                this.btnActualizar.Visible = true;
                this.btnCancelar.Visible = true;
                this.btnSave.Visible = false;
                this.btnLimpiar.Visible = false;
            }
            else
            {
                strScript = "Debe seleccionar el registro que desea Actualizar.";
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

    public bool ValidarValores()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:\\n";
        string strMensajeEN = "Please specify the following fields:\\n";

        if (this.txtNombreEspanol.Text == "")
        {
            strMensaje = strMensaje + "* Nombre Español\\n";
            strMensajeEN = strMensajeEN + "* Spanish Name\\n";
            blnResult = false;
        }

        if (this.txtNombreIngles.Text == "")
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvEstatusGrowing.Rows.Count > 0)
        {
            this.gvEstatusGrowing.UseAccessibleHeader = true;
            this.gvEstatusGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        EstatusGrowing objEstatusGrowing = new EstatusGrowing();

        string strScript = "";
        bool blnResultado = false;

        if (ValidarValores())
        {
            try
            {
                objEstatusGrowing.NombreEspanol = this.txtNombreEspanol.Text.Trim();
                objEstatusGrowing.NombreIngles = this.txtNombreIngles.Text.Trim();
                objEstatusGrowing.Activo = this.chkActivo.Checked;
                objEstatusGrowing.IdUsuario = Convert.ToInt32(Session["userIDInj"]);

                if (this.hidEsEnEspanol.Value == "true")
                {
                    objEstatusGrowing.EsEnEspanol = true;
                    if (this.gvEstatusGrowing.SelectedIndex >= 0)
                    {
                        objEstatusGrowing.IdEstatusGrowing = Convert.ToInt32(this.gvEstatusGrowing.SelectedRow.Cells[0].Text);
                        blnResultado = objEstatusGrowing.MantenimientoCatalogo(2);
                    }
                    else
                    {
                        blnResultado = objEstatusGrowing.MantenimientoCatalogo(1);
                    }

                    if (blnResultado == true)
                    {
                        strScript = "Registro guardado correctamente.\\n";
                    }
                    else
                    {
                        strScript = objEstatusGrowing.ErrorMessage;
                    }
                }

                else
                {
                    objEstatusGrowing.EsEnEspanol = false;
                    if (this.gvEstatusGrowing.SelectedIndex >= 0)
                    {
                        objEstatusGrowing.IdEstatusGrowing = Convert.ToInt32(this.gvEstatusGrowing.SelectedRow.Cells[0].Text);
                        blnResultado = objEstatusGrowing.MantenimientoCatalogo(2);
                    }
                    else
                    {
                        blnResultado = objEstatusGrowing.MantenimientoCatalogo(1);
                    }

                    if (blnResultado == true)
                    {
                        strScript = "Record successfully saved.\\n";
                    }
                    else
                    {
                        strScript = objEstatusGrowing.ErrorMessage;
                    }

                }
                if (blnResultado == true)
                {
                    this.dstEstatusGrowing.DataBind();
                    this.gvEstatusGrowing.DataBind();
                    this.chkActivo.Checked = true;
                    this.txtNombreEspanol.Text = "";
                    this.txtNombreIngles.Text = "";
                    this.gvEstatusGrowing.SelectedIndex = -1;

                    this.btnActualizar.Visible = false;
                    this.btnCancelar.Visible = false;
                    this.btnSave.Visible = true;
                    this.btnLimpiar.Visible = true;
                }

            }
            catch (Exception exError)
            {
                if (this.hidEsEnEspanol.Value == "true")
                {
                    strScript = "Error al guardar el registro en la clase EstatusGrowing:MantenimientoCatalogo().\\n" + exError.Message;
                }
                else
                {
                    strScript = "Failed to save the record to the class EstatusGrowing:MantenimientoCatalogo().\\n" + exError.Message;
                }

            }
            finally
            {
                objEstatusGrowing = null;
            }
        }

        if (!string.IsNullOrEmpty(strScript))
        {
            if (blnResultado == true)
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
        if (this.gvEstatusGrowing.Rows.Count > 0)
        {
            this.gvEstatusGrowing.UseAccessibleHeader = true;
            this.gvEstatusGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        this.chkActivo.Checked = true;
        this.txtNombreEspanol.Text = "";
        this.txtNombreIngles.Text = "";
        this.gvEstatusGrowing.SelectedIndex = -1;
        
        this.btnActualizar.Visible = false;
        this.btnCancelar.Visible = false;
        this.btnSave.Visible = true;
        this.btnLimpiar.Visible = true;

    }

    protected void gvEstatusGrowing_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
    }

    protected void gvEstatusGrowing_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.gvEstatusGrowing.Rows.Count > 0)
        {
            this.gvEstatusGrowing.UseAccessibleHeader = true;
            this.gvEstatusGrowing.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvEstatusGrowing, "Select$" + e.Row.RowIndex);
        }
    }
}