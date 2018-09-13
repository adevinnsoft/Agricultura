using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;
using log4net;
using System.Web.Services;
using System.Configuration;
using System.Web.UI;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
	private static readonly ILog log = LogManager.GetLogger(typeof(BasePage));

    public DataAccess dataaccess = new DataAccess();

	public static ILog Log
	{
		get
		{
			return log;
		}
	}

	private string _pageName;
	private Collection<string> _pagesForRole;
	private enum AccessDeniedEnum
	{
		NoDefined,
		RedirectToLoginPage,
		ThrowHttpAccessDeniedException
	}
	#region Properties
	public string PageName
	{
		get
		{
			return _pageName;
		}
	}
	public Collection<string> PagesForRole
	{
		get
		{
			return _pagesForRole != null ? _pagesForRole : new Collection<string>();
		}
	}
	#endregion

	#region Constructors
	protected BasePage()
	{
		this.Init += new EventHandler(BasePage_Init);
		this.Load += new EventHandler(BasePage_Load);
	}
	#endregion
	#region PageEvents
	private void BasePage_Load(object sender, EventArgs e)
	{
		Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
		Response.Cache.SetCacheability(HttpCacheability.NoCache);
	}



	private void BasePage_Init(object sender, EventArgs e)
	{
		if(!IsAutorizedAccess())
		{
			this.AccessDeniedAction();
		}
		_pageName = GetPageName();
	}
	#endregion
	#region Virtual Methods
	protected virtual bool IsAutorizedAccess()
	{
		_pagesForRole = LoadRolePermissions();
		string currentPageName = GetPageName();
        string currentpagewebapp = currentPageName.Substring(1, currentPageName.Length-1);
		// Si la página está en la lista, el usuario está autorizado
		//if (_pagesForRole.Contains(currentPageName.Substring(4, currentPageName.Length - 4)))
        if (_pagesForRole.Contains(currentPageName) || _pagesForRole.Contains(currentpagewebapp)) 
		{
			return true;
		}
		//if(null != Session["usernameInj"])
			//return true;
		return false;
	}
	#endregion
	#region Private Methods
	
	private void AccessDeniedAction()
	{
		//if (this._accessDeniedActionEnm == AccessDeniedEnum.NoDefined)
		//{
		//    // acción por defecto
		//    this._accessDeniedActionEnm = AccessDeniedEnum.ThrowHttpAccessDeniedException;
		//}
		//switch (_accessDeniedActionEnm)
		//{
		//    case AccessDeniedEnum.RedirectToLoginPage:
		//        Response.Redirect("~/loginPage.aspx", true);
		//        break;
		//    case AccessDeniedEnum.ThrowHttpAccessDeniedException:
		//        throw new HttpException(403, "No tiene los permisos necesarios para acceder a esta página");
		//        break;
		//}
        try
        {
            Response.Redirect("~/frmLogin.aspx", true);
        }
        catch (Exception x)
        {
            //log.Error(x);
        }
	}

	
	private Collection<string> LoadRolePermissions()
	{
		Collection<string> pagesForRole = new Collection<string>();

		if(null != Session["dtUserInfoInj"])
		{
			//Es administrador
			DataRow dtUserInfo = (DataRow)Session["dtUserInfoInj"];
			bool activo = (bool)dtUserInfo["bActivo"];
			if(!activo)
			{
				Session.Clear();
				Response.Redirect("~/error/NoAccess.aspx");
			}
			int roleId = dtUserInfo["roleIds"] != DBNull.Value ? (int)dtUserInfo["roleIds"] : -1;

           
            pagesForRole = buildAllowedMenu(roleId);
            

		}
		else
		{
			//No autenticado
            try
            {
                //Response.Redirect("~/error/NoAccess.aspx");
            }
            catch (Exception x)
            {
                log.Error(x);
            }
		}


		return pagesForRole;
	}

	private string GetPageName()
	{
		string aux = this.Page.Request.AppRelativeCurrentExecutionFilePath.ToLower(CultureInfo.InvariantCulture);
		if(!string.IsNullOrEmpty(aux) && aux.Length > 1)
		{
			aux = aux.Substring(1);
		}
		else
		{
			aux = string.Empty;
		}
		return aux;
	}
	#endregion

    protected T FindControlFromMaster<T>(string name) where T : Control
    {
        MasterPage master = this.Master;
        while (master != null)
        {
            T control = master.FindControl(name) as T;
            if (control != null)
                return control;

            master = master.Master;
        }
        return null;
    }

		private Collection<string> buildAllowedMenu(int idRol)
	{
        //if (Session["userIDInj"].ToString() == ConfigurationManager.AppSettings["SA"].ToString())
        //{
        //    idRol = 0;
        //}
        try
        {

       
		Collection<string> pagesForRole = new Collection<string>();
		var parameters = new Dictionary<string, object>();
        pagesForRole.Add("/pages/bienvenida.aspx".ToLower());
		parameters.Add("activo", true);
        parameters.Add("idRol", idRol);
        parameters.Add("@Android", 0); 
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);

		var ds = dataaccess.executeStoreProcedureDataTable("spr_SelectSubModulos", parameters);
		var distinctModules = (from row in ds.AsEnumerable()
									  where row["idSubModulo"] == DBNull.Value
									  select new
									  { 
										  modulo = row.Field<string>("vModulo"),
										  ruta = row.Field<string>("modulo_ruta"),
										  idModulo = row.Field<int>("idModulo")
										  , allowed = row.Field<Int32>("tienePermiso")
									  }).Distinct();
		string _htmlMenu = string.Empty;
		var distinctModulesArray = distinctModules.ToArray();
		string HTMLModuleString;
		String HTMLChildrenString;

		for(int i = 0; i < distinctModulesArray.Count(); i++)
		{
			var modulo = distinctModulesArray[i];

			HTMLChildrenString = getSubNodes(ds, "idSubModulo IS NOT NULL AND idSubModuloParent IS NULL AND idModulo = " + modulo.idModulo, pagesForRole);

			if(Convert.ToBoolean(modulo.allowed) || HTMLChildrenString.Length > 0)
			{
				//open tag modulo            
				HTMLModuleString = string.Empty;

				if(Convert.ToBoolean(modulo.allowed) && !string.IsNullOrEmpty(modulo.ruta))
				{
					pagesForRole.Add(modulo.ruta.ToLower());
				}

				HTMLModuleString += "\n\t<li><a "
											+ (modulo.modulo.Length > 23 ? "class=\"double\"" : (i == 0 ? "class=\"first\"" : i == distinctModulesArray.Count() - 1 ? "class=\"last\"" : string.Empty))
					//TODO: Quitar el href cuando no esta permitido
											+ " href=\"" + (string.IsNullOrEmpty(modulo.ruta) || string.CompareOrdinal(modulo.ruta, "#") == 0 || modulo.allowed == 0 ? "#" : this.Page.Request.ApplicationPath + modulo.ruta)
											+ "\">" + modulo.modulo + "</a>";
				HTMLModuleString += "\n\t\t<ul>";
				HTMLModuleString += HTMLChildrenString;
				//close tag modulo
				HTMLModuleString += "\n\t\t</ul>";
				HTMLModuleString += "\n\t</li>";
				_htmlMenu += HTMLModuleString;
			}
		}

		Session["menu"] = _htmlMenu;

		return pagesForRole;
        }
        catch (Exception x)
        {
            log.Error(x);
            return PagesForRole;
        }
	}

        [WebMethod(EnableSession = true)]
        public static int getIdioma()
        {
            //return CultureInfo.CurrentCulture.Name.ToString() == "es-MX" ? 1 : 0;
            return HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0;
        }

	protected override void InitializeCulture()
	{
		if(false)//Request.Form["ctl00$ddlLocale"] != null)
		{
			Session["Locale"] = Request.Form["ctl00$ddlLocale"];
			//for UI elements
			UICulture = Request.Form["ctl00$ddlLocale"];

			//for region specific formatting
			Culture = Request.Form["ctl00$ddlLocale"];
		}
		else
		{
			if(null != Session["Locale"])
			{
				UICulture = (string)Session["Locale"];
				Culture = (string)Session["Locale"];
			}
			else
			{
				Session["Locale"] = CultureInfo.CurrentCulture.Name;
				UICulture = (string)Session["Locale"];
				Culture = (string)Session["Locale"];
			}
		}
		base.InitializeCulture();
	}

    [WebMethod]
    public static void seleccionaPlanta(int idPlanta)
    {
        HttpContext.Current.Session["idPlanta"] = idPlanta;
    }

    public static string GetDataTableToJson(DataTable dt)
    {

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

	
	private string getSubNodes(DataTable ds, String filter, Collection<string> pagesForRole)
	{
		string HTMLString = string.Empty;
		var itemArray = ds.Select(filter, "submodulo ASC").ToArray();

		for(int j = 0; j < itemArray.Count(); j++)
		{
			var item = itemArray[j];

			if((int)item["tienePermiso"] == 1)
			{
				if(!string.IsNullOrEmpty((String)item["vRuta"]))
				{
					pagesForRole.Add(((string)item["vRuta"]).ToLower());
				}

				HTMLString += "\n\t\t\t<li><a " + (((string)item["subModulo"]).Length > 22 ? "class=\"double\"" : (j == 0 ? "class=\"first\"" : j == itemArray.Count() - 1 ? "class=\"last\"" : string.Empty)) + " href=\"" + (string.IsNullOrEmpty((string)item["vRuta"]) || string.CompareOrdinal((string)item["vRuta"], "#") == 0 ? "#" : this.Page.Request.ApplicationPath + item["vRuta"]) + "\">" + item["subModulo"] + "</a>";
				HTMLString += "\n\t\t\t\t<ul>";
				HTMLString += getSubNodes(ds, "idSubModuloParent = " + (int)item["idSubModulo"], pagesForRole);
				//close submodulo
				HTMLString += "\n\t\t\t\t</ul>";
				HTMLString += "\n\t\t\t</li>";
			}
		}

		return HTMLString;
	}

	[WebMethod]
	public static int getLargestQuantityInBag(int idBag)
	{
        DataAccess dataaccess = new DataAccess();

		Dictionary<string, object> parameters = new Dictionary<string, object>();

		parameters.Add("@idBag", idBag);

		return dataaccess.executeStoreProcedureGetInt("GetLargestQuantityInBag", parameters);
	}

    [WebMethod(EnableSession = true)]
    public static string ObtenerHoraUsuario()
    {
        try
        {
            return TimeZone.obtenerHoraDeLaCuenta(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local), TimeZone.obtenerZonaID(Convert.ToInt32(HttpContext.Current.Session["idPlanta"] == null ? 0 : HttpContext.Current.Session["idPlanta"]))).ToString("yyyy-MM-dd HH:mm:ss");
        }
        catch (Exception ex)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}