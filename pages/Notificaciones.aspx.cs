using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;


 public partial class Notificaciones : BasePage
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            string strScript = "";
            bool blnDebugSession = false;
            Departamento objDepartamento = new Departamento();
            Rol objRol = new Rol();
            Usuario objUsuario = new Usuario();

            try
            {
                if (!this.IsPostBack)
                {
                    this.ddlDepartamento.DataSource = objDepartamento.ObtenerCatalogo();
                    this.ddlDepartamento.DataTextField = "Descripcion";
                    this.ddlDepartamento.DataValueField = "idDepartamento";
                    this.ddlDepartamento.DataBind();
                    this.ddlDepartamento.Items.Insert(0, "");

                    this.ddlRol.DataSource = objRol.ObtenerCatalogo();
                    this.ddlRol.DataTextField = "Descripcion";
                    this.ddlRol.DataValueField = "idRol";
                    this.ddlRol.DataBind();
                    this.ddlRol.Items.Insert(0, "");

                    this.ddlUser.DataSource = objUsuario.ObtenerCatalogo();
                    this.ddlUser.DataTextField = "vNombre";
                    this.ddlUser.DataValueField = "idUsuario";
                    this.ddlUser.DataBind();
                    this.ddlUser.Items.Insert(0, "");
                }
                //Verifico el lenguaje
                if (UICulture.ToString().ToUpper().IndexOf(ConfigurationManager.AppSettings["CaracterUICultura"].ToString()) > 0)
                {
                    this.hidEsEnEspanol.Value = "true";
                }
                else
                {
                    this.hidEsEnEspanol.Value = "false";
                }
                // Veo variables de session:
                blnDebugSession = Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]);
                if (blnDebugSession)
                {
                    Session["userIDInj"] = "1";
                    Session["usernameInj"] = "Admin";
                }
            }
            catch (Exception ex)
            {
                strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
            }
            finally
            {
                if (strScript != "")
                {
                    strScript = "<script language='javascript'> alert('" + strScript + "');</script>";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
                }
            }
        }

        protected void gvNotificaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Coloco el color verde a las notificaciones no leidas:
            if (e.Row.Cells[8].Text == "NO")
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.White;
            }
        }

        protected void gvNotificaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
        }

        protected void gvNotificaciones_PreRender(object sender, EventArgs e)
        {
            // Genero la opción para que se vean los filtros:
            if (this.gvNotificacionesClient.Rows.Count > 0)
            {
                this.gvNotificacionesClient.UseAccessibleHeader = true;
                this.gvNotificacionesClient.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btnEnviarNotificacion_Click(object sender, EventArgs e)
        {
            string strScript = "";
            Class.Notificaciones objNotificacion = new Class.Notificaciones();

            try
            {
                if (this.txtMensajeEdit.Text.Trim() == "")
                {
                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        strScript = "No hay ningun mensaje para enviar";
                    }
                    else
                    {
                        strScript = "There is no message to send";
                    }
                    return;
                }

                if (this.ddlDepartamento.SelectedItem.Text == "" && this.ddlRol.SelectedItem.Text == "" && this.ddlUser.SelectedItem.Text == "" && this.ddlEsParaTodos.SelectedItem.Text.ToUpper() == "NO")
                {
                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        strScript = "No hay ningun destinatario seleccionado";
                    }
                    else
                    {
                        strScript = "No selected recipients";
                    }
                    return;
                }



                if (this.ddlDepartamento.SelectedItem.Text != "")
                {
                    objNotificacion.IdDepartamento = Convert.ToInt32(this.ddlDepartamento.SelectedItem.Value);
                }
                if (this.ddlRol.SelectedItem.Text != "")
                {
                    objNotificacion.IdRol = Convert.ToInt32(this.ddlRol.SelectedItem.Value);
                }
                if (this.ddlUser.SelectedItem.Text != "")
                {
                    objNotificacion.IdUsuario = Convert.ToInt32(this.ddlUser.SelectedItem.Value);
                }
                if (this.ddlEsParaTodos.SelectedItem.Text.ToUpper() == "NO")
                {
                    objNotificacion.EsParaTodos =false;
                }
                else
                {
                     objNotificacion.EsParaTodos =true;
                }
                                
                objNotificacion.Mensaje = this.txtMensajeEdit.Text;
                objNotificacion.Mantenimiento(1);
                if (objNotificacion.ErrorNumber != 0)
                {
                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        strScript = "Error Notificaciones!AltaNotificacionLeida:\\n" + objNotificacion.ErrorMessage.Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
                    }
                    else
                    {
                        strScript = "Error Notificaciones!AltaNotificacionLeida:\\n" + objNotificacion.ErrorMessage.Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
                    }
                    return;
                }
                else
                {
                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        strScript = "Notificacion enviada correctamente";
                    }
                    else
                    {
                        strScript = "Notification sent correctly";
                    }
                }
               

                // Actualizo el dataset:
                this.dstNotificaciones.DataBind();
                this.gvNotificacionesClient.DataBind();
                // Limpio valores:
                this.ddlDepartamento.Text = "";
                this.ddlEsParaTodos.Text = "";
                this.ddlRol.Text = "";
                this.ddlUser.Text = "";
                this.txtMensajeEdit.Text = "";
                this.ddlDepartamento.Focus();
            }
            catch (Exception ex)
            {
                strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
            }
            finally
            {
                if (strScript != "")
                {
                    strScript = "<script language='javascript'> alert('" + strScript + "');</script>";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
                }
            }
        } 
    }
