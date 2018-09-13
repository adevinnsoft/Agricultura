using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administration_frmRol : BasePage
{
	#region Eventos Pagina
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if(!IsPostBack)
			{
				if(Session["usernameInj"] == null)
					Response.Redirect("~/frmLogin.aspx", false);

				this.obtieneRoles();
			}
		}
		catch(Exception exception)
		{
			Log.Error(exception.ToString());
		}
	}

	protected void btnSaveRol_Click(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
			if(txtRol.Text.Trim().Equals("") || txtRol_EN.Text.Trim().Equals(""))
			{
				popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("campoRequerido"), Comun.MESSAGE_TYPE.Error);
			}
			else
			{
				Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
				DataSet ds = null;

				parameters.Add("@rolName", txtRol.Text.Trim());
				parameters.Add("@rolName_EN", txtRol_EN.Text.Trim());
				if(chkRolActivo.Checked)
					parameters.Add("@activo", 1);
				else
					parameters.Add("@activo", 0);


				if(Accion.Value == "Añadir")
				{
					//Verificar que el valor "Rol" a insertar no estan anteriormente agregados
					Dictionary<string, object> find = new System.Collections.Generic.Dictionary<string, object>();
					find.Add("@rolName", txtRol.Text);
					find.Add("@rolName_EN", txtRol_EN.Text);

					if(dataaccess.executeStoreProcedureGetInt("spr_ExisteRol", find) > 0)
					{
						popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("noChangesMade"), Comun.MESSAGE_TYPE.Info);
					}
					else
					{
						ds = dataaccess.executeStoreProcedureDataSet("spr_InsertRol", parameters);
						if(ds == null || ds.Tables[0].Rows.Count == 0)
							popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("RGRL03"), Comun.MESSAGE_TYPE.Error);
						else
						{
							popUpMessageControl1.setAndShowInfoMessage(string.Format((string)GetLocalResourceObject("rolSaved"), txtRol.Text), Comun.MESSAGE_TYPE.Success);

						}
					}
				}
				else
				{
					if(Session["IdRolCookie"] == null || Session["IdRolCookie"].ToString() == "")
						popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("RGRL02"), Comun.MESSAGE_TYPE.Error);
					else
					{
						parameters.Add("@IdRol", Session["IdRolCookie"].ToString());
						ds = dataaccess.executeStoreProcedureDataSet("spr_UpdateRol", parameters);
						if(ds == null || ds.Tables[0].Rows.Count == 0)
							popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("RGRL01"), Comun.MESSAGE_TYPE.Error);
						else
						{
							String Resultado = ds.Tables[0].Rows[0]["Resultado"].ToString();
							if(Resultado.Equals("Existe"))
								popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("noChangesMade"), Comun.MESSAGE_TYPE.Info);
							else
								if(Resultado.Equals("Update"))
									popUpMessageControl1.setAndShowInfoMessage((string)GetGlobalResourceObject("Commun", "RecordUpdated"), Comun.MESSAGE_TYPE.Success);
							else if(Resultado.Equals("No Editable"))
								popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("noeditable").ToString(), Comun.MESSAGE_TYPE.Warning);
						}
					}
				}
				obtieneRoles();
				VolverAlPanelInicial();

			}
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	protected void btnCancelRol_Click(object sender, EventArgs e)
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

	protected void gvRol_PreRender(object sender, EventArgs e)
	{
		if(gvRol.HeaderRow != null)
			gvRol.HeaderRow.TableSection = TableRowSection.TableHeader;
	}

	protected void gvRol_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		switch(e.Row.RowType)
		{
			case DataControlRowType.DataRow:
				e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvRol, ("Select$" + e.Row.RowIndex.ToString()));
				break;
		}
	}

	protected void gvRol_SelectedIndexChanged(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
			Session["IdRolCookie"] = gvRol.DataKeys[gvRol.SelectedIndex].Value.ToString();

			Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();

			parameters.Add("@IdRol", Session["IdRolCookie"]);

			DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectFromRolId", parameters);

			if(dt.Rows.Count > 0)
			{
				txtRol.Text = dt.Rows[0]["rolName"].ToString().Trim();
				txtRol_EN.Text = dt.Rows[0]["rolName_EN"].ToString().Trim();
				hddRol.Value = dt.Rows[0]["idRol"].ToString().Trim();

				if(dt.Rows[0]["bActivo"].ToString().Equals("True"))
					chkRolActivo.Checked = true;
				else
					chkRolActivo.Checked = false;

				Accion.Value = "Guardar Cambios";
				btnActualizar.Visible = true;
				btnCancelRol.Visible = true;
				btnLimpiar.Visible = false;
				btnSaveRol.Visible = false;
			}
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
			for(int i = 0; i < gvRol.Rows.Count; i++)
			{
				Page.ClientScript.RegisterForEventValidation(new System.Web.UI.PostBackOptions(gvRol, "Select$" + i.ToString()));
			}
			base.Render(writer);
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}
	#endregion

	private void obtieneRoles()
	{
		Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
		parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
		DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ObtieneRoles", parameters);
		ViewState["dsRoles"] = ds;
		gvRol.DataSource = ds;
		gvRol.DataBind();
	}

	protected void VolverAlPanelInicial()
	{
		Accion.Value = "Añadir";
		txtRol.Text = "";
		txtRol_EN.Text = "";
		chkRolActivo.Checked = true;
		gvRol.Enabled = true;
		btnActualizar.Visible = false;
		btnCancelRol.Visible = false;
		btnLimpiar.Visible = true;
		btnSaveRol.Visible = true;
	}
}