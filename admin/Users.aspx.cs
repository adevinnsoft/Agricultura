using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Administration_Users : BasePage
{
	#region Eventos de pagina
	
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
            DepartamentosDdl();
			ddlPlanta.Visible = true;
            if (fillgrViewPendings() && fillddlPlanta() && fillddlTipo())
            {
                limpiaCampos();
            }

		}

		try
		{
            if (Session["usernameInj"] == null)
            {
                Response.End();
                return;
                //Response.Redirect("~/frmLogin.aspx", false);
            }
            else { 
                
            }
		}
		catch(HttpException hE)
		{
			Log.Error(hE.ToString());

			return;
		}
		
	}

    protected void DepartamentosDdl()
    {
        try
        {
            var dt = dataaccess.executeStoreProcedureDataTable("spr_DepartamentosObtener", new Dictionary<string,object>(){{"@lengua",getIdioma()}});
            ddlDepartamento.DataSource = dt;
            ddlDepartamento.DataValueField = "idDepartamento";
            ddlDepartamento.DataTextField = "Departamento";
            ddlDepartamento.DataBind();
            ddlDepartamento.Items.Insert(0, new ListItem(GetGlobalResourceObject("Commun", "Select").ToString(), ""));
        }
        catch (Exception ex)
        {

        }
    }

	protected void grViewPendings_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if(e.Row.RowType == DataControlRowType.DataRow)
		{
			try
			{
				e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(grViewPendings, ("Select$" + e.Row.RowIndex.ToString()));
			}
			catch(Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}
	}

	protected void grViewPendings_PreRender(object sender, EventArgs e)
	{
		if(grViewPendings.HeaderRow != null)
			grViewPendings.HeaderRow.TableSection = TableRowSection.TableHeader;
	}

	protected void txtCuenta_TextChanged(object sender, EventArgs e)
	{
        hdn_exist.Value = "1";

        if (!cbxActiveDirectory.Checked)
        {
            txtEmail.Enabled = true;
            return;
        }
        txtEmail.Enabled = false;
        foreach (ListItem item in ddlPlanta.Items)
        {
            item.Enabled = true;
            item.Selected = false;
        }
#if DEBUG
				if (txtCuenta.Text.Trim().CompareTo("recepcion") == 0)
            {
                ltNombreUsuario.Text = "Usuario recepcion";
                return;
            }else if(txtCuenta.Text.Trim().CompareTo("planta") == 0)
            {
                ltNombreUsuario.Text = "Usuario planta";
                return;
            }else if(txtCuenta.Text.Trim().CompareTo("planta2") == 0)
            {
                ltNombreUsuario.Text = "Usuario planta 2";
                return;
            }else if(txtCuenta.Text.Trim().CompareTo("apoyo") == 0)
            {
                ltNombreUsuario.Text = "Usuario apoyo";
                return;
            }else if(txtCuenta.Text.Trim().CompareTo("apoyo2") == 0)
            {
                ltNombreUsuario.Text = "Usuario apoyo 2";
                return;
            }else if(txtCuenta.Text.Trim().CompareTo("apoyo3") == 0)
            {
                ltNombreUsuario.Text = "Usuario apoyo 3";
                return;
            }else if(txtCuenta.Text.Trim().CompareTo("apoyo4") == 0)
            {
                ltNombreUsuario.Text = "Usuario apoyo 4";
                return;
            }             
#endif
        try
        {
            String exception = String.Empty;
            txtCuenta.Text = txtCuenta.Text.ToLower().Trim().Replace("*", "");
            if (string.IsNullOrEmpty(txtCuenta.Text))
            {
                popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("ErrorAttempt"), Comun.MESSAGE_TYPE.Warning);
                return;
            }
            DataTable tabla = DGActiveDirectory.getGeneralInfo(DGActiveDirectory.setDomainVariablesAndGetSearchResult("GDL|USA", txtCuenta.Text, ref exception), txtCuenta.Text);

            if (null == tabla)
            {
                Log.Error(exception);
                popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("ErrorAttempt"), Comun.MESSAGE_TYPE.Warning);
                hdn_exist.Value = "0";
                txtCuenta.Text = "";
                limpiaCampos();
            }
            else
            {
                ViewState["AD_Info"] = tabla;
                txtNumeroEmpleado.Text = tabla.Rows[0]["id_Empleado"].ToString();
                ltNombreUsuario.Text = tabla.Rows[0]["FirstName"] + " " + tabla.Rows[0]["LastName"];
                txtNombre.Text = ltNombreUsuario.Text;
                txtEmail.Text = tabla.Rows[0]["Email"] as string;
                hdn_exist.Value = "1";

                //if (string.IsNullOrEmpty(tabla.Rows[0]["idPlanta"].ToString()))
                //{
                //    popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("idPlanta"), Comun.MESSAGE_TYPE.Warning);
                //    //limpiaCampos();
                //    //hdn_exist.Value = "0";
                //}
                //else
                //{
                //    ddlPlanta.Items.FindByValue(tabla.Rows[0]["idPlanta"].ToString()).Selected = true;
                //    ddlPlanta.Items.FindByValue(tabla.Rows[0]["idPlanta"].ToString()).Enabled = false;
                //}

                if (string.IsNullOrEmpty(tabla.Rows[0]["idEmpleado"].ToString()))
                {
                    popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("idEmpleado"), Comun.MESSAGE_TYPE.Warning);
                    limpiaCampos();
                    hdn_exist.Value = "0";
                }

            }
        }
        catch (NullReferenceException nre)
        {
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("nocoincideplanta").ToString(),Comun.MESSAGE_TYPE.Error);
            
            Log.Error(nre);
        }
        catch (Exception es)
        {
            Log.Error(es);
        }
	}

	protected void btnGuardar_Click(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

       
        //if (string.IsNullOrEmpty(ltNombreUsuario.Text))
        //{
        //    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("activedirectory").ToString(), Comun.MESSAGE_TYPE.Error);
        //    return;
        //}
        if (string.IsNullOrEmpty(txtCuenta.Text))
        {
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("activedirectoryempty").ToString(),Comun.MESSAGE_TYPE.Error);
            return;
        }
        if (string.IsNullOrEmpty(txtNumeroEmpleado.Text))
        {
            popUpMessageControl1.setAndShowInfoMessage("Debes ingresar el número de empleado", Comun.MESSAGE_TYPE.Error);
            return;
        }

        if (!cbxActiveDirectory.Checked & string.IsNullOrEmpty(txtEmail.Text))
        {
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("FaltaNombre").ToString(), Comun.MESSAGE_TYPE.Error);
            return;
        }
        if (!cbxActiveDirectory.Checked & string.IsNullOrEmpty(txtNumeroEmpleado.Text))
        {
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("FaltaNombre").ToString(), Comun.MESSAGE_TYPE.Error);
            return;
        }
		string plantaList = string.Empty;

		if(formFilledCorrectly(ref plantaList) && hdn_exist.Value=="1")
		{
			if(hdIdItem.Value.Equals(String.Empty))
			{
				saveUser(plantaList);
			}
			else
			{
				updateUser(plantaList);
			}
            limpiaCampos();
		}

		fillgrViewPendings();
	}

	protected void btnCancelar_Click(object sender, EventArgs e)
	{
		limpiaCampos();
	}
	
	protected void grViewPendings_SelectedIndexChanged(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		int id;
		DataTable dt;
		Dictionary<string, object> parameters = new Dictionary<string, object>();

		

		if(null != grViewPendings.SelectedPersistedDataKey)
		{
			Int32.TryParse(grViewPendings.SelectedPersistedDataKey["idUsuario"].ToString(), out id);
		}
		else
		{
			Int32.TryParse(grViewPendings.SelectedDataKey["idUsuario"].ToString(), out id);
		}

		parameters.Add("@id", id);
        limpiaCampos();

		try
		{
			parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);

			dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneUsuarios", parameters);

			ListItem listItem;
			string rol = dt.Rows[0]["Tipo_Usr"].ToString().Trim();
            foreach (ListItem item in ddlPlanta.Items)
            {
                item.Enabled = true;
            }

			txtCuenta.Text = dt.Rows[0]["vUsuario"].ToString().Trim();
			txtCuenta.Enabled = true;
            hddRol.Value = dt.Rows[0]["idRol"].ToString().Trim();
            ddlDepartamento.SelectedValue = dt.Rows[0]["idDepartamento"].ToString();
            hdn_exist.Value = "1";
            if (ddlTipo.Items.FindByText(rol) != null)
                ddlTipo.SelectedValue = ddlTipo.Items.FindByText(rol).Value;
            else
            {
                popUpMessageControl1.setAndShowInfoMessage("El Rol indicado para este usuario se encuentra desactivado", Comun.MESSAGE_TYPE.Info);
            }
			foreach(string item in dt.Rows[0]["plantaList"].ToString().Trim().Split(','))
			{
				listItem = ddlPlanta.Items.FindByValue(item);
				if(null != listItem)
				{
                    listItem.Selected = true;
                }
			}

			ltNombreUsuario.Text = dt.Rows[0]["vNombre"].ToString().Trim();

			txtEmail.Text = dt.Rows[0]["email"].ToString().Trim();
            txtNombre.Text = ltNombreUsuario.Text;
            txtNumeroEmpleado.Text = dt.Rows[0]["idEmpleado"].ToString().Trim();
			checkActivo.Checked = (bool)dt.Rows[0]["bActivo"];
			hdIdItem.Value = dt.Rows[0]["idUsuario"].ToString().Trim();
			grViewPendings.Enabled = false;

            btnCancelar.Text = GetGlobalResourceObject("Commun", "Cancelar").ToString();
            btnGuardar.Text = GetGlobalResourceObject("Commun", "Actualizar").ToString();
            
            if (id.ToString() == ConfigurationManager.AppSettings["SA"].ToString())
            {
                ddlTipo.Enabled = false;
                checkActivo.Enabled = false;
            }
            else
            {
                ddlTipo.Enabled = true;
                checkActivo.Enabled = true;
            }
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
		finally
		{
			Page_Load(null, null);
		}
	}
	#endregion

	#region Metodos Auxiliares

	public Boolean fillgrViewPendings()
	{
		DataSet ds;

		try
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
			ds = dataaccess.executeStoreProcedureDataSet("spr_ObtieneUsuarios", parameters);
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());

			return false;
		}

		ViewState["dsUsers"] = ds;
		grViewPendings.DataSource = ds;
		grViewPendings.DataBind();

		return true;
	}

	
	private Boolean fillddlPlanta()
	{
		try
		{
            ddlPlanta.DataSource = dataaccess.executeStoreProcedureDataSet("spr_PlantaObtener", new Dictionary<string, object>() {{ "@activo", true}});
            ddlPlanta.DataTextField = "NombrePlanta";
            ddlPlanta.DataValueField = "idPlanta";
            ddlPlanta.DataBind();

		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());

			return false;
		}
		
		return true;
	}

	
	private Boolean fillddlTipo()
	{
		var parameters = new Dictionary<string, object>();

		parameters.Add("@ACTIVO", true);
		ddlTipo.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "Select") as String, string.Empty));

		try
		{
			parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
			ddlTipo.DataSource = dataaccess.executeStoreProcedureDataSet("spr_ObtieneRoles", parameters);
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());

			return false;
		}

		ddlTipo.DataTextField = "rolName";
		ddlTipo.DataValueField = "idRol";
		ddlTipo.DataBind();

		return true;
	}

	
	public void limpiaCampos()
	{
		txtCuenta.Text = string.Empty;
        txtNumeroEmpleado.Text = string.Empty;
        txtNombre.Text = string.Empty;
		txtCuenta.Enabled = true;
        ddlDepartamento.SelectedIndex = -1;
		ddlTipo.SelectedIndex = -1;
		//ddlsucursal.SelectedIndex = -1;
		ddlPlanta.ClearSelection();
        foreach (ListItem item in ddlPlanta.Items)
        {
            item.Enabled = true;  
        }
		ltNombreUsuario.Text = string.Empty;
        //ddl_Lider.Items.Clear();
		txtEmail.Text = string.Empty;
		checkActivo.Checked = true;
		hdIdItem.Value = null;
		grViewPendings.SelectedIndex = -1;
		grViewPendings.Enabled = true;
        //ddlsucursal.SelectedIndex = -1;
        ddlTipo.Enabled = true;
        checkActivo.Enabled = true;

        btnCancelar.Text = GetGlobalResourceObject("Commun", "Limpiar").ToString();
        btnGuardar.Text = GetGlobalResourceObject("Commun", "Guardar").ToString();

	}


	
	public Boolean isThereTheUser(String cuenta,String email)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();

		parameters.Add("@cuenta", cuenta);
        parameters.Add("@email", email);

		try
		{
			return dataaccess.executeStoreProcedureDataTable("spr_ExisteUsuario", parameters).Rows.Count > 0;
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
			popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("noValid"), Comun.MESSAGE_TYPE.Error);

			return false;
		}
	}

	
	private Boolean formFilledCorrectly(ref string plantaList)
	{
		String mensaje = String.Empty;

		if(hdIdItem.Value.Equals(String.Empty))
		{
			if(txtCuenta.Text.Equals(String.Empty))
			{
				mensaje += (string)GetLocalResourceObject("CuentaRequerido") + Environment.NewLine;
			}
			else
			{
				if(isThereTheUser(txtCuenta.Text,txtEmail.Text))
				{
					mensaje += (string)GetLocalResourceObject("existeUsuario") + Environment.NewLine;
				}
			}

			try
			{
				if(cbxActiveDirectory.Checked)
				{
					String exception = String.Empty;

					if(!DGActiveDirectory.userExistOnActiveDirectory(txtCuenta.Text.Trim(), "GDL|USA", ref exception))
					{
						if(exception.Length == 0)
						{
							mensaje += (string)GetLocalResourceObject("NoexisteUsuario") + Environment.NewLine;
                            return false;
						}
						else
						{
							Log.Error(exception);
                            ltNombreUsuario.Text = string.Empty;
							return false;
						}
					}
				}
			}
			catch(ConfigurationErrorsException cEE)
			{
				Log.Error(cEE.ToString());
			}
		}

		if(ddlTipo.SelectedIndex < 1)
		{
			mensaje += (string)GetLocalResourceObject("TipoRequerido") + Environment.NewLine;
		}

			foreach(ListItem item in ddlPlanta.Items)
			{
				if(item.Selected && !plantaList.Contains(item.Text + "|"))
				{
					plantaList += item.Value + "|";
				}
			}

			if(plantaList.Length == 0)
			{
				mensaje += (string)GetLocalResourceObject("AtLeastOnePlant") + Environment.NewLine;
			}

            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                string[] emails = txtEmail.Text.Split(';');

                foreach (string email in emails)
                {
                    try
                    {
                        new MailAddress(email);
                        if (email.Contains(" "))
                        {
                            mensaje += String.Format((string)GetLocalResourceObject("CorreoMal"), email) + Environment.NewLine;

                        }
                    }
                    catch (Exception)
                    {
                        mensaje += String.Format((string)GetLocalResourceObject("CorreoMal"), email) + Environment.NewLine;

                    }
                }
            }
            else
            {
                mensaje += String.Format((string)GetLocalResourceObject("CorreoMal"), "") + Environment.NewLine;
            }

		if(mensaje.Length > 0)
		{
			popUpMessageControl1.setAndShowInfoMessage(mensaje, Comun.MESSAGE_TYPE.Error);

			return false;
		}

		return true;
	}

	
	protected void saveUser(String plantList)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		DataTable dt;

        DataTable AD_Info = (DataTable)ViewState["AD_Info"]; 
        parameters.Add("@nombre", txtNombre.Text);
		parameters.Add("@cuenta", txtCuenta.Text.Replace("*",""));
		parameters.Add("@rolesList", ddlTipo.SelectedValue);
        parameters.Add("@plantaList", plantList.Equals(string.Empty) ? AD_Info.Rows[0]["idPlanta"].ToString() : plantList);
        parameters.Add("@activo", checkActivo.Checked);
		parameters.Add("@email", txtEmail.Text);
        parameters.Add("@liderId", txtNumeroEmpleado.Text);//ddl_Lider.SelectedValue);
        parameters.Add("@idDepartamento", ddlDepartamento.SelectedValue.ToString());
        
		try
		{
			dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaUsuarioAdmin", parameters);
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
			popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("noValid"), Comun.MESSAGE_TYPE.Error);

			return;
		}

		if(dt.Rows[0]["repetido"].ToString() == "0")
			popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "RecordSaved") as String, Comun.MESSAGE_TYPE.Success);
		else
            if (dt.Rows[0]["repetido"].ToString() == "4")
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("AlreadyLiderUser") as String, Comun.MESSAGE_TYPE.Warning);
            else
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("AlreadySavedUser") as String, Comun.MESSAGE_TYPE.Warning);
	}

	
	protected void updateUser(String plantList)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		int id;

		Int32.TryParse(hdIdItem.Value.ToString(), out id);

        DataTable AD_Info = (DataTable)ViewState["AD_Info"];
        parameters.Add("@id", id);
		parameters.Add("@rolesList", ddlTipo.SelectedValue);
        parameters.Add("@plantaList", plantList);
        parameters.Add("@activo", checkActivo.Checked);
        parameters.Add("@nombre", txtNombre.Text.Trim());
        parameters.Add("@cuenta", txtCuenta.Text.Trim());
        parameters.Add("@idEmpleado", txtNumeroEmpleado.Text);
		parameters.Add("@email", txtEmail.Text);
        parameters.Add("@idDepartamento", ddlDepartamento.SelectedValue.ToString());
        string msg = string.Empty;
        try
        {

			DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ActualizaUsuarioAdmin", parameters);
            var estado = dt.Rows[0]["Estado"].ToString().Equals("0") ? false : true;
            var clave = dt.Rows[0]["Clave"].ToString();
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject(clave) + msg as String, estado ?  Comun.MESSAGE_TYPE.Success : Comun.MESSAGE_TYPE.Error );

		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
			popUpMessageControl1.setAndShowInfoMessage((string)GetLocalResourceObject("noUpdate"), Comun.MESSAGE_TYPE.Error);

			return;
		}

	}
	#endregion

	
	protected void ddlsucursal_SelectedIndexChanged(object sender, EventArgs e)
	{

		if(ddlTipo.SelectedValue.Equals(ConfigurationManager.AppSettings.GetValues("idSucursal")[0].ToString()))
		{
			foreach(ListItem item in ddlPlanta.Items)
			{
				item.Enabled = true;

			}
		}
	}

	protected void TextBox1_TextChanged(object sender, EventArgs e)
	{
		Response.Write("<script language='JavaScript'>bloquear('no');</script>");
	}

    protected void cbxActiveDirectory_CheckedChanged(object sender, EventArgs e)
    {
        if (!cbxActiveDirectory.Checked)
        {
            txtEmail.Enabled = true;
            txtNombre.Visible = true;
            txtNumeroEmpleado.Enabled = true;
        }
        else
        {
            txtEmail.Enabled = false;
            txtNombre.Visible = false;
            txtNumeroEmpleado.Enabled = false;
        }
    }
}