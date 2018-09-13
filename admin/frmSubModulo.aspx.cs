using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administration_frmSubModulo : BasePage
{
	#region Eventos Pagina

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if(!Page.IsPostBack)
			{
				if(Session["usernameInj"] == null)
					Response.Redirect("~/frmLogin.aspx", false);

				obtieneModulos();
				getSubmodules();
				obtieneSubModulosGv();
			}
		}
		catch(Exception exception)
		{
			Log.Error(exception.ToString());
		}
	}

	
	protected void ddlModulo_DataBound(object sender, EventArgs e)
	{
		try
		{
			ddlModulo.Items.Insert(0, new ListItem(GetGlobalResourceObject("Commun", "Select").ToString(), "0"));
			ddlModulo.SelectedIndex = 0;
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected void gvSubM_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if(e.Row.RowType == DataControlRowType.DataRow)
		{
			e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSubM, ("Select$" + e.Row.RowIndex.ToString()));
		}
	}

	protected void gvSubM_PreRender(object sender, EventArgs e)
	{
		if(gvSubM.HeaderRow != null)
			gvSubM.HeaderRow.TableSection = TableRowSection.TableHeader;
	}

	protected override void Render(HtmlTextWriter writer)
	{
		for(int i = 0; i < gvSubM.Rows.Count; i++)
		{
			Page.ClientScript.RegisterForEventValidation(new System.Web.UI.PostBackOptions(gvSubM, "Select$" + i.ToString()));
		}

		base.Render(writer);
	}

	
	protected void ddlModulo_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			setddlSunMP();
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected void ddlSunMP_DataBound(object sender, EventArgs e)
	{
		try
		{
			ddlSunMP.Items.Insert(0, new ListItem(GetLocalResourceObject("Ninguno").ToString(), string.Empty));
			ddlSunMP.SelectedIndex = 0;
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	
	protected void btnSave_Click(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
            if (String.IsNullOrEmpty(ddlModulo.SelectedItem.Value.ToString()) || ddlModulo.SelectedItem.Value.ToString().Equals("0")|| txtSubM.Text.Trim().Length == 0 || txtSubM_EN.Text.Trim().Length == 0)
			{
				popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("AsteriskFields"), Comun.MESSAGE_TYPE.Error);
			}
			else
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();

				parameters.Add("@idModulo", ddlModulo.SelectedItem.Value);

				if(!string.IsNullOrEmpty(ddlSunMP.SelectedValue))
					parameters.Add("@IdSubModuloParent", ddlSunMP.SelectedItem.Value);

                parameters.Add("@subModulo", txtSubM.Text.Trim());
                parameters.Add("@subModulo_EN", txtSubM_EN.Text.Trim());
                parameters.Add("@ruta", txtRuta.Text.Trim());
				parameters.Add("@activo", chkActivo.Checked ? 1 : 0);
                parameters.Add("@Android", 0);
				if(Accion.Value == "Añadir")
				{
					//Verificar que el valor "Modulo" a insertar no estan anteriormente agregados
					Dictionary<string, object> find = new Dictionary<string, object>();

                    //find.Add("@Android", 0);
					find.Add("@idModulo", ddlModulo.SelectedItem.Value);
					find.Add("@subModulo", txtSubM.Text);
					find.Add("@subModulo_EN", txtSubM_EN.Text);
					find.Add("@ruta", txtRuta.Text);

					if(dataaccess.executeStoreProcedureGetInt("spr_ExisteSubModulo", find) > 0)
					{
						popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("alreadyExist"), Comun.MESSAGE_TYPE.Info);
					}
					else
					{
						if(dataaccess.executeStoreProcedureDataTable("spr_InsertSubModulo", parameters).Rows.Count > 0)
						{
							popUpMessageControl1.setAndShowInfoMessage(string.Format((string)GetLocalResourceObject("exito"), txtSubM.Text), Comun.MESSAGE_TYPE.Success);
						}
					}
				}
				else
				{
					if(Session["IdSubMCookie"] == null || Session["IdSubMCookie"].ToString() == "")
						popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("RGRL02"), Comun.MESSAGE_TYPE.Error);
					else
					{
						parameters.Add("@IdSubModulo", Session["IdSubMCookie"].ToString());

						String Rs = dataaccess.executeStoreProcedureString("spr_UpdateSubModulo", parameters);

						if(Rs.Equals("Repetido"))
							popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("alreadyExist"), Comun.MESSAGE_TYPE.Info);
						else
							if(Rs.Equals("Success"))
								popUpMessageControl1.setAndShowInfoMessage((string)GetGlobalResourceObject("Commun", "RecordUpdated"), Comun.MESSAGE_TYPE.Success);
							else
								popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("RGUPSM01"), Comun.MESSAGE_TYPE.Success);

					}
				}

				getSubmodules();
				obtieneSubModulosGv();
				VolverAlPanelInicial();
			}
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected void btnCancel_Click(object sender, EventArgs e)
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

	protected void gvSubM_SelectedIndexChanged(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
			Session["IdSubMCookie"] = gvSubM.DataKeys[gvSubM.SelectedIndex].Value.ToString();
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("@IdSubModulo", Session["IdSubMCookie"]);
			DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectFromSubModuloId", parameters);
			if(dt.Rows.Count > 0)
			{

				txtSubM.Text = dt.Rows[0]["subModulo"].ToString().Trim();
				txtSubM_EN.Text = dt.Rows[0]["subModulo_EN"].ToString().Trim();
				txtRuta.Text = dt.Rows[0]["vRuta"].ToString().Trim();
				ddlModulo.SelectedValue = dt.Rows[0]["idModulo"].ToString().Trim();
				setddlSunMP();
				ListItem item = ddlSunMP.Items.FindByValue(Session["IdSubMCookie"].ToString());
				if(null != item)
				{
					ddlSunMP.Items.Remove(item);
				}
				try
				{
					ddlSunMP.SelectedValue = dt.Rows[0]["idSubModuloParent"].ToString().Trim();
				}
				catch(Exception ex)
				{
					Log.Error(ex);
				}


				if(dt.Rows[0]["bActivo"].ToString().Equals("True"))
					chkActivo.Checked = true;
				else
					chkActivo.Checked = false;

                //ckAndroid.Checked = dt.Rows[0]["Android"].ToString().Equals("True");
				Accion.Value = "Guardar Cambios";
				btnActualizar.Visible = true;
				btnCancel.Visible = true;
				btnLimpiar.Visible = false;
				btnSave.Visible = false;
			}
			else
			{
				//No se encontró el registro
			}
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}
	#endregion

	private void obtieneModulos()
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@Android", 0);
        parameters.Add("@activo", 1);
		parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);

		ddlModulo.DataSource = dataaccess.executeStoreProcedureDataSet("spr_SelectAllModulo", parameters);
		ddlModulo.DataTextField = "vModulo";
		ddlModulo.DataValueField = "idModulo";
		ddlModulo.DataBind();
	}


	protected void getSubmodules()
	{
		ViewState["submodules"] = dataaccess.executeStoreProcedureDataTable("spr_SelectSubModuloByIdModulo", null);
	}

	
	private void obtieneSubModulosGv()
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        parameters.Add("@Android", 0);

		gvSubM.DataSource = dataaccess.executeStoreProcedureDataSet("spr_SelectAllSubModulos", parameters);
		gvSubM.DataBind();
	}

	protected void setddlSunMP()
	{
		ddlSunMP.DataTextField = CultureInfo.CurrentCulture.Name == "es-MX" ? "subModulo" : "subModulo_EN";

		DataTable submodules = (ViewState["submodules"] as DataTable);
		DataTable dt = submodules.Clone();
        if (ddlModulo.SelectedValue == "")
        {
            ddlSunMP.Items.Clear();
        }
        else
		foreach(DataRow item in submodules.Select("idSubModuloParent IS NULL"
																+ " AND idModulo = " + ddlModulo.SelectedValue))
		{
			dt.ImportRow(item);
			addChilds(item, dt, submodules, 1);
		}

		ddlSunMP.DataSource = dt;
		ddlSunMP.DataBind();
	}


	private void addChilds(DataRow item, DataTable dtOut, DataTable dsIn, int level)
	{
		foreach(DataRow childItem in dsIn.Select("idSubModuloParent = " + (int)item["idSubModulo"]))
		{
			string indent = string.Empty;

			for(int i = 0; i < level; i++)
			{
				indent += Server.HtmlDecode("&nbsp;&#8226;");
			}

			indent += Server.HtmlDecode("&nbsp;");
			childItem["subModulo"] = indent + childItem["subModulo"];
			dtOut.ImportRow(childItem);
			addChilds(childItem, dtOut, dsIn, level + 1);
		}
	}


	protected void VolverAlPanelInicial()
	{
		Accion.Value = "Añadir";
		txtSubM.Text = "";
		txtSubM_EN.Text = "";
		txtRuta.Text = "";
		ddlModulo.SelectedIndex = 0;
		ddlSunMP.SelectedIndex = 0;
		ddlSunMP.Items.Clear();
		chkActivo.Checked = true;
       // ckAndroid.Checked = false;
		gvSubM.Enabled = true;
		btnActualizar.Visible = false;
		btnCancel.Visible = false;
		btnLimpiar.Visible = true;
		btnSave.Visible = true;
	}
}