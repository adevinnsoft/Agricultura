using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;


public partial class pages_ColoresPalet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strScript = "";
            bool blnDebugSession = false;

            try
            {
                
                if (!Page.IsPostBack)
                {
                    this.txtMinutoInicio.Attributes.Add("onKeyPress", "Javascript:return OnlyNumbers(event);");
                    this.txtMinutoFin.Attributes.Add("onKeyPress", "Javascript:return OnlyNumbers(event);");
                    txtColorP.Attributes.Add("readonly", "true");

                    // Seteo por default el valor para la variable de session de las imagenes y variables de session:
                    this.Session["UserTemp"] = "";
                    Session["ImagesUpload"] = null;
                    blnDebugSession = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SessionDebug"]);
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
                    cargaVariedades();
                    chkVariedad.Visible = true;

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

        private void cargaVariedades()
        {
            try
            {
                chkVariedad.DataSource = dataaccess.executeStoreProcedureDataTable("spr_getVariedades", null);
                chkVariedad.DataTextField = "CodigoVariedad";
                chkVariedad.DataValueField = "idVariedad";

                chkVariedad.DataBind();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
       
        private void CargarColores(DropDownList ddl)
        {
            int index = -1;
            if (ddl.SelectedIndex > 0)
                index = ddl.SelectedIndex;
            //Inicializamos los colores y mostramos el mensaje de seleccionar color.
            ddl.Items.Clear();
            ddl.Items.Add("Color...");
            foreach (int pos in Enum.GetValues(typeof(System.Drawing.KnownColor)))
            {
                string strnColor = Enum.GetName(typeof(System.Drawing.KnownColor), pos);
                System.Drawing.Color color = System.Drawing.Color.FromName(strnColor);
                if (!color.IsSystemColor)
                {
                    ListItem item = new ListItem(color.Name, pos.ToString());
                    item.Attributes.CssStyle.Add(HtmlTextWriterStyle.BackgroundColor, color.Name);
                    ddl.Items.Add(item);
                 }
              }

            //Si tenia indice lo seleccionamos.
            if (index > 0)
                ddl.SelectedIndex = index;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Vuelvo a generar visible el header para el tema del sorting:
            if (this.gvConfiguracion.Rows.Count > 0)
            {
                this.gvConfiguracion.UseAccessibleHeader = true;
                this.gvConfiguracion.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            ColoresDePalet objColoresDePalet = new ColoresDePalet();

            string strScript = "";
            bool blnResultado = false;
            bool blnExito = true;
            string idVariedades = "";

            if (ValidarValores())
            {
                try
                {
                    if (UICulture.ToString().ToUpper().IndexOf("Ñ") > 0)
                    {
                        this.hidEsEnEspanol.Value = "true";
                    }
                    else
                    {
                        this.hidEsEnEspanol.Value = "false";
                    }

                    objColoresDePalet.MinutoInicio= Convert.ToInt32(this.txtMinutoInicio.Text.Trim());
                    objColoresDePalet.MinutoFin= Convert.ToInt32(this.txtMinutoFin.Text.Trim());
                    objColoresDePalet.Color = this.txtColorP.Text;
                    objColoresDePalet.Activo = this.chkActivo.Checked;
                    objColoresDePalet.IdUsuario = Convert.ToInt32(Session["userIDInj"]);

                    for (var item = 0; item < chkVariedad.Items.Count; item++ )
                    {
                        if (chkVariedad.Items[item].Selected)
                        {
                            idVariedades += chkVariedad.Items[item].Value + "|";
                        }
                        
                    }
                    idVariedades = idVariedades.Substring(0, idVariedades.Length - 1);
                    objColoresDePalet.idVariedad = idVariedades;

                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        objColoresDePalet.EsEnEspanol = true;
                        if (this.gvConfiguracion.SelectedIndex >= 0)
                        {
                            objColoresDePalet.IdColoresDePalet = Convert.ToInt32(this.gvConfiguracion.SelectedRow.Cells[0].Text);
                            blnResultado = objColoresDePalet.MantenimientoCatalogo(2);
                        }
                        else
                        {
                            blnResultado = objColoresDePalet.MantenimientoCatalogo(1);
                        }
                        if (blnResultado == true)
                        {
                            strScript = "Registro guardado correctamente.\\n";
                            blnExito = true;
                        }
                        else
                        {
                            strScript = objColoresDePalet.ErrorMessage;
                            blnExito = false;
                        }
                        }

                    else
                    {
                        objColoresDePalet.EsEnEspanol = false;
                        if (this.gvConfiguracion.SelectedIndex >= 0)
                        {
                            objColoresDePalet.IdColoresDePalet = Convert.ToInt32(this.gvConfiguracion.SelectedRow.Cells[0].Text);
                            blnResultado = objColoresDePalet.MantenimientoCatalogo(2);
                        }
                        else
                        {
                            blnResultado = objColoresDePalet.MantenimientoCatalogo(1);
                        }
                        if (blnResultado == true)
                        {
                            strScript = "Record successfully saved.\\n";
                            blnExito = true;
                        }
                        else
                        {
                            strScript = objColoresDePalet.ErrorMessage;
                            blnExito = false;
                        }
                   
                    }
                    if (blnResultado == true)
                    {
                        this.dstColoresPalet.DataBind();
                        this.gvConfiguracion.DataBind();
                        this.chkActivo.Checked = true;
                        this.txtMinutoInicio.Text = "";
                        this.txtMinutoFin.Text = "";
                        this.txtColorP.Text = "";
                        this.gvConfiguracion.SelectedIndex = -1;
                        this.btnActualizar.Visible = false;
                        this.btnCancelar.Visible = false;
                        this.btnGuardar.Visible = true;
                        this.btnLimpiar.Visible = true;
                        for (var ck = 0; ck < chkVariedad.Items.Count; ck++)
                        {
                            chkVariedad.Items[ck].Enabled = true;
                            chkVariedad.Items[ck].Selected = false;
                        }
                    }

                }
                catch (Exception exError)
                {
                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        strScript = "Error al guardar el registro en la clase ColoresDePalet:MantenimientoCatalogo().\\n" + exError.Message;
                        blnExito = false;
                    }
                    else
                    {
                        strScript = "Failed to save the record to the class ColoresDePalet:MantenimientoCatalogo().\\n" + exError.Message;
                        blnExito = false;
                    }

                }
                finally
                {
                    objColoresDePalet = null;
                }
            }

            if (!string.IsNullOrEmpty(strScript))
            {
                if (blnExito==true)
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

        protected void gvConfiguracion_SelectedIndexChanged(object sender, EventArgs e)
        {   
            string strScript = "";
            
            try
            {
                // Vuelvo a generar visible el header para el tema del sorting:
                if (this.gvConfiguracion.Rows.Count > 0)
                {
                    this.gvConfiguracion.UseAccessibleHeader = true;
                    this.gvConfiguracion.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                this.txtColorP.Text = "";
                if (this.gvConfiguracion.SelectedIndex != -1)
                {
                    this.txtMinutoInicio.Text = this.gvConfiguracion.SelectedRow.Cells[2].Text;
                    this.txtMinutoFin.Text = this.gvConfiguracion.SelectedRow.Cells[3].Text;

                    this.txtColorP.Text = this.gvConfiguracion.SelectedRow.Cells[4].Text;


                    //this.ddlColores.Items.FindByText(this.gvConfiguracion.SelectedRow.Cells[4].Text).Selected = true;

                    //CheckBox chkActivo2 = (CheckBox)this.gvConfiguracion.SelectedRow.Cells[5].Controls[0];
                    //this.chkActivo.Checked = chkActivo2.Checked;

                    if (this.gvConfiguracion.SelectedRow.Cells[6].Text.ToUpper() == "NO")
                    {
                        this.chkActivo.Checked = false;
                    }
                    else
                    {
                        this.chkActivo.Checked = true;
                    }
                    this.btnActualizar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.btnGuardar.Visible = false;
                    this.btnLimpiar.Visible = false;

                    this.chkVariedad.Items.FindByText(this.gvConfiguracion.SelectedRow.Cells[5].Text).Selected = true;

                    for (var ck = 0; ck < chkVariedad.Items.Count; ck++)
                    {
                        chkVariedad.Items[ck].Enabled = false;
                    }
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Vuelvo a generar visible el header para el tema del sorting:
            if (this.gvConfiguracion.Rows.Count > 0)
            {
                this.gvConfiguracion.UseAccessibleHeader = true;
                this.gvConfiguracion.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            for (var ck = 0; ck < chkVariedad.Items.Count; ck++)
            {
                chkVariedad.Items[ck].Enabled = true;
                chkVariedad.Items[ck].Selected = false;
            }

            this.chkActivo.Checked = true;
            this.txtMinutoInicio.Text = "";
            this.txtMinutoFin.Text = "";
            this.txtColorP.Text = "";
            this.gvConfiguracion.SelectedIndex = -1;
            this.btnActualizar.Visible = false;
            this.btnCancelar.Visible = false;
            this.btnGuardar.Visible = true;
            this.btnLimpiar.Visible = true;

            for (int ch = 0; ch < chkVariedad.Items.Count; ch++)
            {
                chkVariedad.Items[ch].Selected = false;
            }
        }

        public bool ValidarValores()
        {
            bool blnResult = true;
            string strMensaje = "Favor de especificar los siguientes campos:\\n";
            string strMensajeEN = "Please specify the following fields:\\n";

            if (this.txtMinutoInicio.Text == "")
            {
                strMensaje = strMensaje + "* Minuto de inicio\\n";
                strMensajeEN = strMensajeEN + "* First Minute\\n";
                blnResult = false;
            }

            if (null == this.chkVariedad.SelectedItem)
            {
                strMensaje += "* Variedad\\n";
                strMensajeEN += "*  Variety\\n";
            }

            if (this.txtMinutoFin.Text == "")
            {
                strMensaje = strMensaje + "* Minuto final\\n";
                strMensajeEN = strMensajeEN + "* Last minute\\n";
                blnResult = false;
            }
            if (blnResult == true)
            {
                if (Convert.ToInt32(this.txtMinutoInicio.Text) > Convert.ToInt32(this.txtMinutoFin.Text))
                {
                    strMensaje = strMensaje + "* Minuto de inicio mayor que minuto final\\n";
                    strMensajeEN = strMensajeEN + "* The minute of start is greater than the final minute\\n";
                    blnResult = false;
                }
            }

            if (this.txtColorP.Text == "")
            {
                strMensaje = strMensaje + "* Color\\n";
                strMensajeEN = strMensajeEN + "* Color\\n";
                blnResult = false;
            }
                       
            if (blnResult == false)
            {
                strMensaje = "<script language='javascript'> popUpAlert('" + strMensaje + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
            }

            return blnResult;
        }

        protected void gvConfiguracion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (this.gvConfiguracion.Rows.Count > 0)
            {
                this.gvConfiguracion.UseAccessibleHeader = true;
                this.gvConfiguracion.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string hex = "#" + e.Row.Cells[4].Text;
                Color myColor = System.Drawing.ColorTranslator.FromHtml(hex);


                //Color myColor = ColorTranslator.FromHtml(e.Row.Cells[4].Text);
                e.Row.Cells[4].BackColor = myColor;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
                e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvConfiguracion, "Select$" + e.Row.RowIndex);
            }
        }

        protected void gvConfiguracion_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
    }
