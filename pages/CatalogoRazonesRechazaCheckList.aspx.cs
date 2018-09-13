using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class pages_CatalogoRazonesRechazaCheckList : BasePage
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

    protected void gvDefectos_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";

        // Vuelvo a generar visible el header para el tema del sorting:
        if (this.gvRazonesDeRechazo.Rows.Count > 0)
        {
            this.gvRazonesDeRechazo.UseAccessibleHeader = true;
            this.gvRazonesDeRechazo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        try
        {

            if (this.gvRazonesDeRechazo.SelectedIndex != -1)
            {
                this.txtDescripcionEspanol.Text = HttpUtility.HtmlDecode( this.gvRazonesDeRechazo.SelectedRow.Cells[5].Text);
                this.txtDescripcionIngles.Text = HttpUtility.HtmlDecode(this.gvRazonesDeRechazo.SelectedRow.Cells[6].Text);
                this.txtNombreEspanol.Text = HttpUtility.HtmlDecode(this.gvRazonesDeRechazo.SelectedRow.Cells[3].Text);
                this.txtNombreIngles.Text = HttpUtility.HtmlDecode(this.gvRazonesDeRechazo.SelectedRow.Cells[4].Text);
            //    CheckBox chkActivo2 = (CheckBox)this.gvRazonesDeRechazo.SelectedRow.Cells[2].Controls[0];
                if (this.gvRazonesDeRechazo.SelectedRow.Cells[2].Text.ToUpper() == "YES" || this.gvRazonesDeRechazo.SelectedRow.Cells[2].Text.ToUpper() == "SI")
                {
                    this.chkActivo.Checked = true;
                }
                else
                {
                    this.chkActivo.Checked = false;
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

        if (this.txtDescripcionEspanol.Text == "")
        {
            strMensaje = strMensaje + "* Descripcion Español\\n";
            strMensajeEN = strMensajeEN + "* Spanish description Up\\n";
            blnResult = false;
        }

        if (this.txtDescripcionIngles.Text == "")
        {
            strMensaje = strMensaje + "* Descripcion Ingles\\n";
            strMensajeEN = strMensajeEN + "* Engish description Up\\n";
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
        if (this.gvRazonesDeRechazo.Rows.Count > 0)
        {
            this.gvRazonesDeRechazo.UseAccessibleHeader = true;
            this.gvRazonesDeRechazo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        RazonDeRechazo objRazonDeRechazo = new RazonDeRechazo();

        string strScript = "";
        bool blnResultado = false;

        if (ValidarValores())
        {
            try
            {
                objRazonDeRechazo.DescripcionEspanol = this.txtDescripcionEspanol.Text.Trim();
                objRazonDeRechazo.DescripcionIngles = this.txtDescripcionIngles.Text.Trim();
                objRazonDeRechazo.NombreEspanol = this.txtNombreEspanol.Text.Trim();
                objRazonDeRechazo.NombreIngles = this.txtNombreIngles.Text.Trim();
                objRazonDeRechazo.Activo = this.chkActivo.Checked;
                objRazonDeRechazo.IdUsuario = Convert.ToInt32(Session["userIDInj"]);

                if (this.hidEsEnEspanol.Value == "true")
                {
                    objRazonDeRechazo.EsEnEspanol = true;
                    if (this.gvRazonesDeRechazo.SelectedIndex >= 0)
                    {
                        objRazonDeRechazo.IdRazonDeRechazo = Convert.ToInt32(this.gvRazonesDeRechazo.SelectedRow.Cells[0].Text);
                        blnResultado = objRazonDeRechazo.MantenimientoCatalogo(2);
                    }
                    else
                    {
                        blnResultado = objRazonDeRechazo.MantenimientoCatalogo(1);
                    }

                    if (blnResultado == true)
                    {
                        strScript = "Registro guardado correctamente.\\n";
                    }
                    else
                    {
                        strScript = objRazonDeRechazo.ErrorMessage;
                    }
                }

                else
                {
                    objRazonDeRechazo.EsEnEspanol = false;
                    if (this.gvRazonesDeRechazo.SelectedIndex >= 0)
                    {
                        objRazonDeRechazo.IdRazonDeRechazo = Convert.ToInt32(this.gvRazonesDeRechazo.SelectedRow.Cells[0].Text);
                        blnResultado = objRazonDeRechazo.MantenimientoCatalogo(2);
                    }
                    else
                    {
                        blnResultado = objRazonDeRechazo.MantenimientoCatalogo(1);
                    }

                    if (blnResultado == true)
                    {
                        strScript = "Record successfully saved.\\n";
                    }
                    else { strScript = objRazonDeRechazo.ErrorMessage; }
                }
                if (blnResultado == true)
                {
                    this.dstRazonesDeRechazo.DataBind();
                    this.gvRazonesDeRechazo.DataBind();
                    this.chkActivo.Checked = true;
                    this.txtDescripcionEspanol.Text = "";
                    this.txtDescripcionIngles.Text = "";
                    this.txtNombreEspanol.Text = "";
                    this.txtNombreIngles.Text = "";
                    this.gvRazonesDeRechazo.SelectedIndex = -1;
                    this.btnActualizar.Visible = false;
                    this.btnCancelar.Visible = false;
                    this.btnSave.Visible = true;
                    this.btnLimpiar.Visible = true;
                }

            }
            catch (Exception exError)
            {
                blnResultado = false;
                if (this.hidEsEnEspanol.Value == "true")
                {
                    strScript = "Error al guardar el registro en la clase RazonDeRechazo:MantenimientoCatalogo().\\n" + exError.Message;
                }
                else
                {
                    strScript = "Failed to save the record to the class RazonDeRechazo:MantenimientoCatalogo().\\n" + exError.Message;
                }

            }
            finally
            {
                objRazonDeRechazo = null;
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
        if (this.gvRazonesDeRechazo.Rows.Count > 0)
        {
            this.gvRazonesDeRechazo.UseAccessibleHeader = true;
            this.gvRazonesDeRechazo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        this.chkActivo.Checked = true;
        this.txtDescripcionEspanol.Text = "";
        this.txtDescripcionIngles.Text = "";
        this.txtNombreEspanol.Text = "";
        this.txtNombreIngles.Text = "";
        this.gvRazonesDeRechazo.SelectedIndex = -1;
        this.btnActualizar.Visible = false;
        this.btnCancelar.Visible = false;
        this.btnSave.Visible = true;
        this.btnLimpiar.Visible = true;

    }

    protected void gvRazonesDeRechazo_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
    }

    protected void gvRazonesDeRechazo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.gvRazonesDeRechazo.Rows.Count > 0)
        {
            this.gvRazonesDeRechazo.UseAccessibleHeader = true;
            this.gvRazonesDeRechazo.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvRazonesDeRechazo, "Select$" + e.Row.RowIndex);
        }


    }
}