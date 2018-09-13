using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Administration_frmModulo : BasePage
{
	#region Eventos de Pagina
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if(!IsPostBack)
			{
				if(Session["usernameInj"] == null)
					Response.Redirect("~/frmLogin.aspx", false);

				this.obtieneModulo();
			}
		}
		catch(Exception exception)
		{
			Log.Error(exception.ToString());
		}
	}

	protected void Guardar_Actualizar(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
			int order;

            if (txtModulo.Text.Trim().Length == 0 || txtModulo_EN.Text.Trim().Length == 0)
			{
				popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "nameModulo") as String, Comun.MESSAGE_TYPE.Error);
			}
			else if(!int.TryParse(txtOrden.Text, out order))
			{
				popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("errorOrden"), Comun.MESSAGE_TYPE.Error);
			}
			else
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();

                parameters.Add("@modulo", txtModulo.Text.Trim());
                parameters.Add("@modulo_EN", txtModulo_EN.Text.Trim());
                parameters.Add("@ruta", txtRuta.Text.Trim());
				parameters.Add("@activo", chkActivo.Checked ? 1 : 0);
                parameters.Add("@orden", txtOrden.Text.Trim());
                parameters.Add("@Android", 0);
               // parameters.Add("@Android", ckAndroid.Checked ? 1 : 0);
                if (Accion.Value == "Añadir" && Session["IdModuloCookie"] == null || Session["IdModuloCookie"].ToString().Trim().Length == 0)
				{
					//Verificar que el valor "Modulo" a insertar no estan anteriormente agregados
					Dictionary<string, object> find = new Dictionary<string, object>();

                    find.Add("@modulo", txtModulo.Text);
                    find.Add("@modulo_EN", txtModulo_EN.Text);
                    find.Add("@ruta", txtRuta.Text);
                    find.Add("@orden", txtOrden.Text);
                    find.Add("@Android",0);

					if(dataaccess.executeStoreProcedureGetInt("spr_ExisteModulo", find) > 0)
					{
						popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("NotChangesExist"), Comun.MESSAGE_TYPE.Info);
					}
					else
					{
						if(dataaccess.executeStoreProcedureDataTable("spr_InsertModulo", parameters).Rows.Count > 0)
						{
							popUpMessageControl1.setAndShowInfoMessage(String.Format(GetLocalResourceObject("saveIt").ToString(), txtModulo.Text), Comun.MESSAGE_TYPE.Success);
                            Session["IdModuloCookie"] = string.Empty;
						}
					}
				}
				else
				{
					if(Session["IdModuloCookie"] == null || Session["IdModuloCookie"].ToString() == "")
						popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("lostID"), Comun.MESSAGE_TYPE.Error);
					else
					{
						parameters.Add("@IdModulo", Session["IdModuloCookie"].ToString());

						if(dataaccess.executeStoreProcedureString("spr_UpdateModulo", parameters).Equals("Repetido"))
							popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("NotChangesExist"), Comun.MESSAGE_TYPE.Info);
						else
                        {
                            popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "RecordUpdated") as String, Comun.MESSAGE_TYPE.Success);
                            Session["IdModuloCookie"] = string.Empty;
                        }
                    }
				}

				obtieneModulo();
				VolverAlPanelInicial();
			}
		}
		catch(Exception ex)
		{
			popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "DataDidNotSave") as String, Comun.MESSAGE_TYPE.Info);
			Log.Error(ex.ToString());
		}
	}

	protected void Cancelar_Limpiar(object sender, EventArgs e)
	{
		try
		{
			VolverAlPanelInicial();
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected void gvModulo_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		switch(e.Row.RowType)
		{
			case DataControlRowType.DataRow:
				e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvModulo, "Select$" + e.Row.RowIndex);
				break;
		}
	}

	protected void gvModulo_SelectedIndexChanged(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();

			Session["IdModuloCookie"] = gvModulo.DataKeys[gvModulo.SelectedIndex].Value.ToString();
			parameters.Add("@IdModulo", Session["IdModuloCookie"]);

			DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectFromModuloId", parameters);

			if(dt.Rows.Count > 0)
			{
                txtModulo.Text = dt.Rows[0]["vModulo"].ToString().Trim();
                txtModulo_EN.Text = dt.Rows[0]["vModulo_EN"].ToString().Trim();
                txtRuta.Text = dt.Rows[0]["vRuta"].ToString().Trim();
                txtOrden.Text = dt.Rows[0]["iOrden"].ToString().Trim();
                chkActivo.Checked = dt.Rows[0]["bActivo"].ToString().Equals("True");
                //ckAndroid .Checked = dt.Rows[0]["Android"].ToString().Equals("True");
				Accion.Value = "Guardar Cambios";
				btnActualizar.Visible = true;
				btnCancel.Visible = true;
				btnLimpiar.Visible = false;
				btnSave.Visible = false;
			}
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected void gvModulo_PreRender(object sender, EventArgs e)
	{
		if(gvModulo.HeaderRow != null)
			gvModulo.HeaderRow.TableSection = TableRowSection.TableHeader;
	}

	protected void gvModulo_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		try
		{
			if(null != ViewState["dsModulo"])
			{
				DataSet ds = ViewState["dsModulo"] as DataSet;

				if(ds != null)
				{
					gvModulo.DataSource = ds;
					gvModulo.DataBind();
				}
			}

			((GridView)sender).PageIndex = e.NewPageIndex;
			((GridView)sender).DataBind();
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		try
		{
			for(int i = 0; i < gvModulo.Rows.Count; i++)
			{
				Page.ClientScript.RegisterForEventValidation(new System.Web.UI.PostBackOptions(gvModulo, "Select$" + i.ToString()));
			}

			base.Render(writer);
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}
	#endregion

	#region Aux Methods

	private void obtieneModulo()
	{
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        parameters.Add("@Android", 0);
		DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_SelectAllModulo", parameters);

		ViewState["dsModulo"] = ds;
		gvModulo.DataSource = ds;
		gvModulo.DataBind();
	}

	protected void VolverAlPanelInicial()
	{
		Accion.Value = "Añadir";
        txtModulo.Text = "";
        txtModulo_EN.Text = "";
        txtRuta.Text = "";
		txtOrden.Text = "";
		chkActivo.Checked = true;
        //ckAndroid.Checked = false;
		gvModulo.Enabled = true;
		btnActualizar.Visible = false;
		btnCancel.Visible = false;
		btnLimpiar.Visible = true;
		btnSave.Visible = true;
	}
	#endregion
}