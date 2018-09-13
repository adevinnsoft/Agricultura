using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

public partial class Administration_frmPermisosPorRol : BasePage
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

				ObtieneRoles();
				ObtieneModulos();
			}
		}
		catch(Exception exception)
		{
			Log.Error(exception.ToString());
		}
	}


	
	protected void btnGuardar_Click(object sender, EventArgs e)
	{
        try
        {
            if (Session["usernameInj"] == null)
                Response.Redirect("~/frmLogin.aspx", false);


            if (ddlModulo.SelectedValue == "" || ddlRol.SelectedValue == "")
            {

                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("camposRequeridos") as String, Comun.MESSAGE_TYPE.Warning);
                return;
            }
            else
            {

                int idRol, idModulo;
                String pathList = String.Empty;
                ListItem item = listaSecciones.Items[0];

                if (item.Selected)
                {
                    pathList += "|";
                }

                for (int i = 1; i < listaSecciones.Items.Count; i++)
                {
                    item = listaSecciones.Items[i];

                    if (item.Selected)
                    {
                        pathList += item.Value + '|';
                    }
                }

                int.TryParse(ddlRol.SelectedValue, out idRol);
                int.TryParse(ddlModulo.SelectedValue, out idModulo);
                guardaAsignacionPermisos(idRol, idModulo, pathList);
            }
        }
        catch (Exception es)
        {
            Log.Error(es);
        }
	}

	protected void getSections(object sender, EventArgs e)
	{
		try
		{
			ObtieneSecciones();
		}
		catch(Exception exception)
		{
			Log.Error(exception.ToString());
		}
	}

	protected void listaSecciones_DataBound(object sender, EventArgs e)
	{
		foreach(ListItem item in listaSecciones.Items)
		{
			item.Text = HttpUtility.HtmlEncode(item.Text);
		}
	}
	#endregion

	#region Auxiliares
	private void ObtieneRoles()
	{
		var parameters = new Dictionary<string, object>();
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);

		parameters.Add("activo", true);
		ddlRol.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "Select") as String, string.Empty));
		ddlRol.DataSource = dataaccess.executeStoreProcedureDataSet("spr_SelectAllRol", parameters);
		ddlRol.DataTextField = "rolName";
		ddlRol.DataValueField = "idRol";
		ddlRol.DataBind();
	}

	private void ObtieneModulos()
	{
        try
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            //parameters.Add("@Android", 0);
            parameters.Add("activo", true);
            ddlModulo.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "Select") as String, string.Empty));
            foreach (DataRow D in dataaccess.executeStoreProcedureDataTable("spr_SelectAllModulo", parameters).Rows)
            {
                bool android =Boolean.Parse(D["Android"].ToString());
                ListItem l = new ListItem();
                l.Value = D["idModulo"].ToString();
                l.Text = D["vModulo"].ToString() + (android ? "(ANDROID)" : "(WEB)");
             //   l.Attributes.CssStyle.Value = ;
                
                ddlModulo.Items.Add(l);
            }
        }
        catch (Exception e)
        {
            return;
        }
		
        //ddlModulo.DataSource = dataaccess.executeStoreProcedureDataSet("spr_SelectAllModulo", parameters);
        //ddlModulo.DataTextField = "vModulo";
        //ddlModulo.DataValueField = "idModulo";
        //ddlModulo.DataBind();
	}

	private void ObtieneSecciones()
	{
		listaSecciones.Items.Clear();

		if(ddlRol.SelectedValue.Length == 0 || ddlModulo.SelectedValue.Length == 0)
		{
			return;
		}

		var parameters = new Dictionary<string, object>();

		parameters.Add("activo", true);
        parameters.Add("idRol", ddlRol.SelectedValue);
        //parameters.Add("@Android", 0);
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);

		var ds = dataaccess.executeStoreProcedureDataTable("spr_SelectSubModulos", parameters);

		listaSecciones.Items.Add(new ListItem(ddlModulo.SelectedItem.Text, ddlModulo.SelectedValue));
		listaSecciones.Items[0].Selected = Convert.ToBoolean(ds.Select("idSubModulo IS NULL AND idModulo = " + ddlModulo.SelectedValue)[0]["tienePermiso"]);
		addSubmodules(ds, "idSubModulo IS NOT NULL AND idSubModuloParent IS NULL AND idModulo = " + ddlModulo.SelectedValue, 1);
		listaSecciones.DataTextField = "subModulo";
		listaSecciones.DataValueField = "idSubModulo";
		listaSecciones.DataBind();
	}

	private void addSubmodules(DataTable table, String filter, int level)
	{
		ListItem listItem;

		foreach(DataRow childItem in table.Select(filter))
		{
			listItem = new ListItem();
			listItem.Attributes.CssStyle.Add("padding-left", 20 * level + "px");
			listItem.Text = childItem["subModulo"].ToString();
			listItem.Value = childItem["idSubModulo"].ToString();
			listItem.Selected = Convert.ToBoolean(childItem["tienePermiso"]);
			listaSecciones.Items.Add(listItem);
			addSubmodules(table, "idSubModuloParent = " + (int)childItem["idSubModulo"], level + 1);
		}
	}

	private void guardaAsignacionPermisos(int idRol, int idModulo, string idSubModuloList)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		string msj = "";
        if (!string.IsNullOrEmpty(idSubModuloList))
        {
            idSubModuloList = "0" + idSubModuloList;
        }

		try
		{
			if(idRol <= 0)
			{
				msj += GetLocalResourceObject("SelectOneRole") as String;
			}

			if(idModulo <= 0)
			{
				msj += GetGlobalResourceObject("Commun", "SelectOneModule") as String;
			}

			if(msj.Trim().Length == 0)
			{
				//Error free, hace el insert...
				var parameters = new Dictionary<string, object>();

				parameters.Add("@idRol", idRol);
				parameters.Add("@idSubModuloList", idSubModuloList);
				parameters.Add("@idModulo", idModulo);
				dataaccess.executeStoreProcedureDataTable("spr_GUARDAR_AsignacionPermisos", parameters);
				popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "RecordSaved") as String, Comun.MESSAGE_TYPE.Success);
			}
			else
			{
				popUpMessageControl1.setAndShowInfoMessage(msj, Comun.MESSAGE_TYPE.Error);
			}
		}
		catch(Exception ex)
		{
			popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "DataDidNotSave") as String, Comun.MESSAGE_TYPE.Error);
			Log.Error(ex.ToString());
		}
	}
	#endregion
}