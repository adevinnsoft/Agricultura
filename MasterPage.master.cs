using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Services;
using System.Web.UI;


public partial class MasterPage : System.Web.UI.MasterPage,
                                  System.Web.UI.ICallbackEventHandler
{
    public int iWarningTimeoutInMilliseconds;
    public int iSessionTimeoutInMilliseconds;
    public string sTargetURLForSessionTimeout;
    int minutesToWarning;
    DataAccess dataaccess = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.pnlMasterPopupNot.Visible = true;

        if (!IsPostBack)
        {
            //this.Page.Title = System.Configuration.ConfigurationManager.AppSettings.Get("appTitle");
            // Callback timer:
            //ClientScriptManager objCSM = Page.ClientScript;
            //String cbReference = objCSM.GetCallbackEventReference(this, "arg", "ReceiveServerData", "'" + this.lblCountNotificaciones.ClientID + "'");
            //String callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
            //objCSM.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

            // Inicializo variables de session:
            Session["NumeroNotificaciones"] = "0";
            try
            {
                ltSemana.Text = dataaccess.executeStoreProcedureString("spr_ObtieneSemanaNS", new Dictionary<string, object>() { { "@fecha", DateTime.Today.Date } });
            }
            catch (Exception exception)
            {

            }
            try
            {
                if (Session["Locale"] != null && Session["Locale"].ToString() != "")
                {
                    if (Session["idUsuario"].ToString() != "" && Session["idUsuario"] != null)
                    {
                        if ((bool)Session["MultiplesPlantas"] == true)
                        {
                            if (!IsPostBack)
                            {
                                LlenaPlantas(int.Parse(Session["idUsuario"].ToString()));
                                if (Session["idPlanta"] == null)
                                {
                                    Session["idPlanta"] = ddlPlanta.SelectedValue;
                                }
                                else
                                {
                                    ddlPlanta.SelectedValue = Session["idPlanta"].ToString();
                                }
                            }
                            ltPlant.Visible = false;
                        }
                        else
                        {
                            ltPlant.Visible = true;
                            ddlPlanta.Visible = false;
                            Session["idPlanta"] = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasDeUsuario", new Dictionary<string, object>() { { "@iduser", Session["idUsuario"].ToString() } }).Rows[0]["IdPlanta"];
                        }

                        if (Session["Locale"].ToString() == "es-MX")
                        {
                            Session["Locale"] = "es-MX";
                            ltUsername.Text = "Bienvenido, " + Session["usernameInj"].ToString();
                            ltPlant.Text = "RANCHO: " + Session["Planta"].ToString();
                        }
                        else
                        {
                            ddlLocale.SelectedValue = Session["Locale"].ToString();
                            ltUsername.Text = "Welcome, " + Session["usernameInj"].ToString();
                            ltPlant.Text = "RANCH: " + Session["Planta"].ToString();
                        }
                    }
                    else
                    {
                        Response.Redirect("~/frmLogin.aspx");
                    }
                }

            }
            catch (Exception)
            {

            }
        }
        else
        {
            // Callback timer:
            //ClientScriptManager objCSM = Page.ClientScript;
            //String cbReference = objCSM.GetCallbackEventReference(this, "arg", "ReceiveServerData", "'" + this.lblCountNotificaciones.ClientID + "'");
            //String callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
            //objCSM.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

            // Coloco el valor actual de las notificaciones:
            //this.lblCountNotificaciones.Text = Session["NumeroNotificaciones"] != null ? Session["NumeroNotificaciones"].ToString() : "" ;
        }
        sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();

        //Get the sessionState timeout (from web.config).
        //If not set there, the default is 20 minutes.
        int iSessionTimeoutInMinutes = Session.Timeout;
        try
        {
            minutesToWarning = int.Parse(System.Configuration.ConfigurationManager.AppSettings["endSessionWarn"].ToString());
        }
        catch (Exception exc)
        {
            minutesToWarning = 1;
        }

      

        //Compute our timeout values, one for
        //our warning, one for session termination.
        int iWarningTimeoutInMinutes = iSessionTimeoutInMinutes - minutesToWarning;

        iWarningTimeoutInMilliseconds = iWarningTimeoutInMinutes * 60 * 1000;

        iSessionTimeoutInMilliseconds = iSessionTimeoutInMinutes * 60 * 1000;
       

    }

    protected void LlenaPlantas(int idUser)
    {
        ddlPlanta.DataSource = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasDeUsuario", new Dictionary<string, object>() { { "@iduser", idUser } });
        ddlPlanta.DataTextField = "Planta";
        ddlPlanta.DataValueField = "IdPlanta";
        ddlPlanta.DataBind();
    }

    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        try
        {
            Session.RemoveAll();
            Response.Redirect("~/frmLogin.aspx");
        }
        catch (Exception)
        {
            Response.Redirect("~/frmLogin.aspx");
        }
    }

    protected void lnkSpanish_Click(object sender, EventArgs e)
    {
        Session["Locale"] = "es-MX";
        ddlLocale.SelectedValue = Session["Locale"].ToString();

        Response.Redirect(this.Request.Url.AbsolutePath, true);
    }
    protected void lnkEnglish_Click(object sender, EventArgs e)
    {
        Session["Locale"] = "en-US";
        ddlLocale.SelectedValue = Session["Locale"].ToString();
        Response.Redirect(this.Request.Url.AbsolutePath, true);
    }

    protected void cargaMenus()
    {
        if (Session["usernameInj"] == null)
        {
            Response.Redirect("~/frmLogin.aspx", false);
        }

        var parameters = new Dictionary<string, object>();
        DataRow dtUserInfo = (DataRow)Session["dtUserInfoInj"];
        string roleId = dtUserInfo["roleIds"] != DBNull.Value ? (string)dtUserInfo["roleIds"] : string.Empty;
        int idRol;

        if (int.TryParse(roleId, out idRol))
        {
            parameters.Add("@idRol", idRol);

            var dt = dataaccess.executeStoreProcedureDataTable("spr_GET_MenuModulos", parameters);

            parameters.Clear();

            foreach (DataRow mod in dt.Rows)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                HtmlGenericControl ul = new HtmlGenericControl("ul");

                a.InnerText = mod["modulo"].ToString();
                li.Controls.Add(a);
                parameters.Clear();
                parameters.Add("@idRol", 1);
                parameters.Add("@idMod", Int32.Parse(mod["idModulo"].ToString()));

                var dt2 = dataaccess.executeStoreProcedureDataTable("spr_GET_MenuSubModulos", parameters);

                foreach (DataRow sub in dt2.Rows)
                {
                    HtmlGenericControl li2 = new HtmlGenericControl("li");
                    LinkButton lnkB = new LinkButton();


                    lnkB.Text = sub["subName"].ToString();
                    lnkB.PostBackUrl = string.Format(sub["subRuta"].ToString());

                    li2.Controls.Add(lnkB);

                    ul.Controls.Add(li2);
                    li.Controls.Add(ul);
                }
            }
        }
    }
  
    public string PlantaSeleccionada
    {
        get { return ddlPlanta.SelectedValue; }
        set { ddlPlanta.SelectedValue = value; }
    }

    public void RaiseCallbackEvent(String eventArgument)
    {
        Class.Notificaciones objNotificacion = new Class.Notificaciones();
        string strScript = "";
        int intNumeroNotificaciones = 0;

        try
        {
            objNotificacion.IdUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
            intNumeroNotificaciones = objNotificacion.ObtenerNumeroDeNotifcaciones();
            Session["NumeroNotificaciones"] = intNumeroNotificaciones.ToString();
        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            objNotificacion = null;
            if (strScript != "")
            {
                strScript = "<script language='javascript'> alert('" + strScript + "');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
        }
    }

    public string GetCallbackResult()
    {
        if (Session["NumeroNotificaciones"] == null)
        { 
            //
            return string.Empty;
        }
        else
        {
            return Session["NumeroNotificaciones"].ToString();
        }
    }

    protected void btnNotificaciones_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Class.Notificaciones objNotificacion = new Class.Notificaciones();
        string strScript = "";
        int intNumeroNotificaciones = 0;
        this.pnlMasterPopupNot.Visible = false;
        try
             
        {
            this.pnlMasterPopupNot.Visible = false;
            if (this.pnlNotificaciones.Visible == true)
            {
                this.pnlNotificaciones.Visible = false;
            }
            else
            {
                this.pnlNotificaciones.Visible = true;
                Session["NotificacionesPorMostrar"] = System.Configuration.ConfigurationManager.AppSettings["NotificacionesPorMostrar"].ToString();
                this.dstNotificaciones.DataBind();
                this.gvNotificaciones.DataBind();
            }
            objNotificacion.IdUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
            intNumeroNotificaciones = objNotificacion.ObtenerNumeroDeNotifcaciones();
            Session["NumeroNotificaciones"] = intNumeroNotificaciones.ToString();
            this.lblCountNotificaciones.Text = intNumeroNotificaciones.ToString();
        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            objNotificacion = null;
            if (strScript != "")
            {
                strScript = "<script language='javascript'> alert('" + strScript + "');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
        }
    }
    protected void gvNotificaciones_PreRender(object sender, EventArgs e)
    {
        // Genero la opción para que se vean los filtros:
        if (this.gvNotificaciones.Rows.Count > 0)
        {
            this.gvNotificaciones.UseAccessibleHeader = true;
            this.gvNotificaciones.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void gvNotificaciones_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
    }

    protected void gvNotificaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Coloco el color verde a las notificaciones no leidas:
        e.Row.Cells[1].CssClass = "event";
        if (e.Row.Cells[8].Text == "NO")
        {
            e.Row.CssClass = "noLeido";
            //e.Row.BackColor = System.Drawing.Color.LightGreen;
        }
        else
        {
            e.Row.CssClass = "Leido";
            //e.Row.BackColor = System.Drawing.Color.White;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.background = '#CCCCCC';";
            e.Row.Attributes["onmouseout"] = "this.style.background = '#FFFFFF';";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvNotificaciones, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvNotificaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";
        Class.Notificaciones objNotificacion = new Class.Notificaciones();
        int intNumeroNotificaciones = 0;

        try
        {
            // Marco la notificacion como leida:
            objNotificacion.IdNotificacion = Convert.ToInt32(this.gvNotificaciones.SelectedRow.Cells[0].Text);
            objNotificacion.IdUsuario = Convert.ToInt32(Session["userIDInj"].ToString());
            objNotificacion.AltaNotificacionLeida();
            if (objNotificacion.ErrorNumber != 0)
            {
                strScript = "Error Notificaciones!AltaNotificacionLeida:\\n" + objNotificacion.ErrorMessage.Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
                return;
            }

            // Abro el popup de la notificación seleccionada:
            this.pnlMasterPopupNot.Visible = true;
            MPE.Show();
            //this.txtDepartamento.Text = this.gvNotificaciones.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
            //this.txtRol.Text = this.gvNotificaciones.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
            //this.txtUsuario.Text = this.gvNotificaciones.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
            //this.txtEsParaTodos.Text = this.gvNotificaciones.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
            this.txtMensaje.InnerHtml = this.gvNotificaciones.SelectedRow.Cells[6].Text.Replace("&nbsp;", "");

            // Actualizo el dataset:
            this.dstNotificaciones.DataBind();
            this.gvNotificaciones.DataBind();

            // Actualizo el contador de notificaciones:
            objNotificacion.IdUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
            intNumeroNotificaciones = objNotificacion.ObtenerNumeroDeNotifcaciones();
            Session["NumeroNotificaciones"] = intNumeroNotificaciones.ToString();
            this.lblCountNotificaciones.Text = intNumeroNotificaciones.ToString();
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



    protected void btnSalir_Click(object sender, EventArgs e)
    {
        this.pnlMasterPopupNot.Visible = false;
    }
}