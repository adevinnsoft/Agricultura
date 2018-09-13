using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Runtime.InteropServices;

/// <summary>
/// Descripción breve de DGActiveDirectory
/// </summary>
public static class DGActiveDirectory
{
    //Valores default en caso de que no existan las Keys con la informacion al dia 20160216.
    private static string domain = ConfigurationManager.AppSettings["GDLDomain"] == null ? "192.168.167.18" : ConfigurationManager.AppSettings["GDLDomain"];
    private static string user = ConfigurationManager.AppSettings["GDLAppUser"] == null ? "appuser" : ConfigurationManager.AppSettings["GDLAppUser"];
    private static string password = ConfigurationManager.AppSettings["GDLAppUserPass"] == null ? "Dglory210" : ConfigurationManager.AppSettings["GDLAppUserPass"];
    private static string port = ConfigurationManager.AppSettings["GDLport"] == null ? "389" : ConfigurationManager.AppSettings["GDLport"];
    public enum PROPIEDAD
    {
        Correo = 1,
        Nombre = 2,
        Apellido = 3,
        idEmpleado = 4,
        Planta = 5,
        NombreAMostrar = 6,
        JefeDirecto = 7,
        ReportaA = 8,
        Localizacion = 9,
        Gerente = 10,
        Telefono = 11,
        Oficina = 12,
        Proxy = 13,
        ExchangeDN = 14

    }
    
    public static string obtenerInformacionDeLaCuenta(string cuenta, PROPIEDAD propiedad)
    {
        try
        {

            SearchResult result =
                    new DirectorySearcher(
                            new DirectoryEntry(
                                    string.Format("LDAP://{0}", domain)
                                    , domain + @"\" + user
                                    , password)
                                    , string.Format("(SAMAccountName={0})", cuenta)
                            ).FindOne();


            if (null == result)
            {
                return string.Empty;
            }

            if (result.Properties.Contains(obtenerPropiedad(propiedad)))
            {
                return result.Properties[obtenerPropiedad(propiedad)][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        catch (COMException)
        {
            try
            {
                SearchResult result =
                        new DirectorySearcher(
                                new DirectoryEntry(
                                        string.Format("LDAP://{0}:{1}", domain, port)
                                        , domain + @"\" + user
                                        , password)
                                        , string.Format("(SAMAccountName={0})", cuenta)
                                ).FindOne();


                if (null == result)
                {
                    return "Error";// errorSpeech;
                }

                if (result.Properties.Contains(obtenerPropiedad(propiedad)))
                {
                    return result.Properties[obtenerPropiedad(propiedad)][0].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception x)
            {
                return x.Message;
            }
        }
        catch (Exception x)
        {
            return x.Message;
        }
    }
    public static string obtenerCorreoElectronicoDeLaCuenta(string cuenta)
    {
        try
        {
            SearchResult result =
                    new DirectorySearcher(
                            new DirectoryEntry(
                                    string.Format("LDAP://{0}:{1}", domain, port)
                                    , user
                                    , password)
                                    , string.Format("(SAMAccountName={0})", cuenta)
                            ).FindOne();

            if (result == null)
            {
                return string.Empty;
            }
            else
            {
                if (result.Properties.Contains("mail"))
                {
                    return result.Properties["mail"][0].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
    public static bool esUsuarioRegistrado(string usuario, string contrasena)
    {
        string domainAndUsername = domain + @"\" + usuario;
        DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:{1}", domain, port), domainAndUsername, contrasena);

        try
        {
            //Bind to the native AdsObject to force authentication.
            object obj = entry.NativeObject;

            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(SAMAccountName=" + usuario + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (null == result)
            {
                return false;
            }

        }
        catch (DirectoryServicesCOMException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
    private static string obtenerPropiedad(PROPIEDAD id)
    {
        switch (id)
        {
            case PROPIEDAD.Correo: return "mail";
            case PROPIEDAD.Nombre: return "givenName";
            case PROPIEDAD.Apellido: return "sn";
            case PROPIEDAD.idEmpleado: return "employeeID";
            case PROPIEDAD.Planta: return "physicalDeliveryOfficeName";
            case PROPIEDAD.NombreAMostrar: return "displayName";
            case PROPIEDAD.JefeDirecto: return "managedBy";
            case PROPIEDAD.ReportaA: return "directReports";
            case PROPIEDAD.Localizacion: return "location";
            case PROPIEDAD.Gerente: return "manager";
            case PROPIEDAD.Telefono: return "telephoneNumber";
            case PROPIEDAD.Oficina: return "physicalDeliveryOfficeName";
            case PROPIEDAD.Proxy: return "proxyAddresses";
            case PROPIEDAD.ExchangeDN: return "legacyExchangeDN";
            default: return "sAMAccountName";
        }
    }

    #region Retrocompatibilidad
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
    private static string GetProperty(SearchResult searchResult, string PropertyName)
	{
		if(searchResult.Properties.Contains(PropertyName))
		{
			return searchResult.Properties[PropertyName][0].ToString();
		}
		else
		{
			return string.Empty;
		}
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	public static DataTable getGeneralInfoAll(string username, string domain)
	{
		DirectoryEntry entry;
		DirectorySearcher objDSearch;
		SearchResult objRSearch;
		DataTable dtResult;

		try
		{
			string USADomain = ConfigurationManager.AppSettings["USADomain"];
			string GDLDomain = ConfigurationManager.AppSettings["GDLDomain"];

			string AppUser = ConfigurationManager.AppSettings.Get(domain == USADomain ? "USAAppUser" : "GDLAppUser");
			string AppUserPass = ConfigurationManager.AppSettings.Get(domain == USADomain ? "USAAppUserPass" : "GDLAppUserPass");

			string domainAndAppUsername = domain + @"\" + AppUser;

			entry = new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domainAndAppUsername, AppUserPass);
			objDSearch = new DirectorySearcher(entry, "(SAMAccountName=" + username + ")");
			objRSearch = objDSearch.FindOne();
			dtResult = new DataTable();
			dtResult.Columns.Clear();

			foreach(string item in objRSearch.Properties.PropertyNames)
			{
				dtResult.Columns.Add(item);
			}

			SearchResultCollection src = objDSearch.FindAll();

			dtResult.Rows.Clear();
			foreach(SearchResult item in src)
			{

				DataRow dr = dtResult.NewRow();
				foreach(string itemS in item.Properties.PropertyNames)
				{
					if(dtResult.Columns.Contains(itemS))
						dr[itemS] = GetProperty(item, itemS);
				}
				dtResult.Rows.Add(dr);
			}
		}
		catch(Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return dtResult;
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	public static DataTable getGeneralInfoByLastName(string username, string firstname, string lastname, string domain)
	{
		DirectoryEntry entry;
		DirectorySearcher objDSearch;
		SearchResultCollection objRSearch;
		DataTable dtResult;
		string sFilter;
		try
		{
			if(username.Trim().Length == 0)
				sFilter = "(&(givenName=*" + (firstname.Trim().Length > 0 ? firstname.Trim() + "*" : "") + ")(sn=*" + (lastname.Trim().Length > 0 ? lastname.Trim() + "*" : "") + "))";
			else
				sFilter = "(SAMAccountName=" + username + ")";

			string USADomain = ConfigurationManager.AppSettings["USADomain"];
			string GDLDomain = ConfigurationManager.AppSettings["GDLDomain"];

			string AppUser = ConfigurationManager.AppSettings.Get(domain == USADomain ? "USAAppUser" : "GDLAppUser");
			string AppUserPass = ConfigurationManager.AppSettings.Get(domain == USADomain ? "USAAppUserPass" : "GDLAppUserPass");

			string domainAndAppUsername = domain + @"\" + AppUser;

			entry = new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domainAndAppUsername, AppUserPass);
			objDSearch = new DirectorySearcher(entry, sFilter);
			objRSearch = objDSearch.FindAll();

			dtResult = new DataTable();
			dtResult.Columns.Clear();
			dtResult.Columns.Add("Username");
			dtResult.Columns.Add("FirstName");
			dtResult.Columns.Add("LastName");
			dtResult.Columns.Add("Email");
			dtResult.Columns.Add("Department");

			dtResult.Rows.Clear();
			foreach(SearchResult sResult in objRSearch)
			{
				int iRow;
				dtResult.Rows.Add();
				iRow = dtResult.Rows.Count - 1;

				dtResult.Rows[iRow]["Username"] = GetProperty(sResult, "SAMAccountName");
				dtResult.Rows[iRow]["FirstName"] = GetProperty(sResult, "givenName");
				dtResult.Rows[iRow]["LastName"] = GetProperty(sResult, "sn");
				dtResult.Rows[iRow]["Email"] = GetProperty(sResult, "mail");
				dtResult.Rows[iRow]["Department"] = GetProperty(sResult, "department");
			}
		}
		catch(Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return dtResult;
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	public static string getSAMAccountByFullName(string fullName, string domain)
	{
		DirectoryEntry entry;
		DirectorySearcher objDSearch;
		SearchResult objRSearch;
		string SAMAccount = string.Empty;
		try
		{

			string USADomain = ConfigurationManager.AppSettings["USADomain"];
			string GDLDomain = ConfigurationManager.AppSettings["GDLDomain"];

			string AppUser = ConfigurationManager.AppSettings.Get(domain == USADomain ? "USAAppUser" : "GDLAppUser");
			string AppUserPass = ConfigurationManager.AppSettings.Get(domain == USADomain ? "USAAppUserPass" : "GDLAppUserPass");
			string domainAndAppUsername = domain + @"\" + AppUser;

			entry = new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domainAndAppUsername, AppUserPass);
			objDSearch = new DirectorySearcher(entry, "(cn=" + fullName + "*)");
			objDSearch.PropertiesToLoad.Add("samaccountname");
			objRSearch = objDSearch.FindOne();

			if(objRSearch != null)
			{
				SAMAccount = GetProperty(objRSearch, "samaccountname");
			}
		}
		catch(Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return SAMAccount;

	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	public static List<string> getEmailUsersAdminNominee(DataTable UserList)
	{
		DirectoryEntry entry;
		DirectorySearcher objDSearch;
		SearchResult objRSearch;
		List<string> UserEmails = new List<string>();
		try
		{
			string Domain = ConfigurationManager.AppSettings["GDLDomain"];
			string AppUser = ConfigurationManager.AppSettings.Get("GDLAppUser");
			string AppUserPass = ConfigurationManager.AppSettings.Get("GDLAppUserPass");
			string domainAndAppUsername = Domain + @"\" + AppUser;
			entry = new DirectoryEntry(string.Format("LDAP://{0}:389", Domain), domainAndAppUsername, AppUserPass);

			foreach(DataRow itemRow in UserList.Rows)
			{
				string item = itemRow["UserName"].ToString();
				objDSearch = new DirectorySearcher(entry, "(SAMAccountName=" + item + ")");
				objRSearch = objDSearch.FindOne();

				if(objRSearch != null)
				{
					string email = GetProperty(objRSearch, "mail");
					if(email != "")
					{
						UserEmails.Add(email);
					}
				}
			}

			string domainAppUser = ConfigurationManager.AppSettings.Get("emailApp");

			//Sí, es un pequeño ajuste para evitar consultas al Active Directory
			int indexAt = domainAppUser.IndexOf('@');
			domainAppUser = domainAppUser.Substring(indexAt, domainAppUser.Length - indexAt);

			Domain = ConfigurationManager.AppSettings["USADomain"];
			AppUser = ConfigurationManager.AppSettings.Get("USAAppUser");
			AppUserPass = ConfigurationManager.AppSettings.Get("USAAppUserPass");
			domainAndAppUsername = Domain + @"\" + AppUser;
			entry = new DirectoryEntry(string.Format("LDAP://{0}:389", Domain), domainAndAppUsername, AppUserPass);

			foreach(DataRow itemRow in UserList.Rows)
			{
				string item = itemRow["UserName"].ToString();

				//Sí, es un pequeño ajuste para evitar consultas al Active Directory
				if(!UserEmails.Contains(item + domainAppUser))
				{
					objDSearch = new DirectorySearcher(entry, "(SAMAccountName=" + item + ")");
					objRSearch = objDSearch.FindOne();
					string email = GetProperty(objRSearch, "mail");
					if(email != "")
					{
						UserEmails.Add(email);
					}
				}
			}
		}
		catch(Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return UserEmails;
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>	
	public static SearchResult setDomainVariablesAndGetSearchResult(String zones, String SAMAccount, ref String exception)
	{
		String appUser = null;
		String appUserPass = null;
		String domain = null;
		String zone = getNextZone(zones);

		if(!setDomainVariables(ref appUser, ref appUserPass, ref domain, zone, ref exception))
		{
			return null;
		}

		SearchResult objRSearch = getSearchResult(SAMAccount, appUser, appUserPass, domain, ref exception);

		if(objRSearch == null)
		{
			if(zones.Equals(zone))
			{
				return null;
			}
			else
			{
				return setDomainVariablesAndGetSearchResult(zones.Remove(0, zone.Length + 1), SAMAccount, ref exception);
			}
		}

		return objRSearch;
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
    private static String getNextZone(String zones)
	{
		int zoneLength = zones.IndexOf('|');

		if(zoneLength > -1)
		{
			return zones.Substring(0, zoneLength);
		}
		else
		{
			return zones;
		}
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	private static Boolean setDomainVariables(ref String appUser, ref String appUserPass, ref String domain, String zone, ref String exception)
	{
		try
		{
			appUser = ConfigurationManager.AppSettings.Get(zone + "AppUser");
			appUserPass = ConfigurationManager.AppSettings.Get(zone + "AppUserPass");
			domain = ConfigurationManager.AppSettings[zone + "Domain"];
		}
		catch(ConfigurationErrorsException cEE)
		{
			exception = cEE.ToString();

			return false;
		}

		return true;
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	private static SearchResult getSearchResult(String SAMAccount, String appUser, String appUserPass, String domain, ref String exception)
	{
		DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domain + @"\" + appUser, appUserPass);

		try
		{
			return new DirectorySearcher(entry, "(SAMAccountName=" + SAMAccount + ")").FindOne();
		}
		catch(COMException cE)
		{
			exception = cE.ToString();

			return null;
		}
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	public static DataTable getGeneralInfo(SearchResult objRSearch, String SAMAccount)
	{
		if(objRSearch == null)
		{
			return null;
		}

		DataTable dtResult = new DataTable();

		dtResult.Columns.Add("Account");
		dtResult.Columns.Add("FirstName");
		dtResult.Columns.Add("LastName");
		dtResult.Columns.Add("Email");
		dtResult.Columns.Add("Department");
		dtResult.Columns.Add("Phone");
        dtResult.Columns.Add("idEmpleado");
        dtResult.Columns.Add("idPlanta");
		dtResult.Rows.Add();
		dtResult.Rows[0]["Account"] = SAMAccount;
		dtResult.Rows[0]["FirstName"] = GetProperty(objRSearch, "givenName");
		dtResult.Rows[0]["LastName"] = GetProperty(objRSearch, "sn");
		dtResult.Rows[0]["Email"] = GetProperty(objRSearch, "mail");
		dtResult.Rows[0]["Department"] = GetProperty(objRSearch, "department");
		dtResult.Rows[0]["Phone"] = GetProperty(objRSearch, "homephone") == "" ? GetProperty(objRSearch, "homephone") : GetProperty(objRSearch, "mobile");
        dtResult.Rows[0]["idEmpleado"] = GetProperty(objRSearch, "employeeID");
        dtResult.Rows[0]["idPlanta"] = GetProperty(objRSearch, "physicaldeliveryofficename");
		return dtResult;
	}

    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	public static Boolean userExistOnActiveDirectory(string SAMAccount, String zones, ref String exception)
	{
		String zone = getNextZone(zones);

		if(!userExistOnAD(SAMAccount, zone, ref exception))
		{
			if(zones.Equals(zone))
			{
				return false;
			}
			else
			{
				return userExistOnActiveDirectory(SAMAccount, zones.Remove(0, zone.Length + 1), ref exception);
			}
		}

		return true;
	}

    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
	private static Boolean userExistOnAD(string SAMAccount, String zone, ref String exception)
	{
		try
		{
			String domain = ConfigurationManager.AppSettings[zone + "Domain"];
			String AppUser = ConfigurationManager.AppSettings.Get(zone + "AppUser");
			String AppUserPass = ConfigurationManager.AppSettings.Get(zone + "AppUserPass");
			String domainAndAppUsername = domain + @"\" + AppUser;
			DirectorySearcher objDSearch = new DirectorySearcher(new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domainAndAppUsername, AppUserPass),
															"(SAMAccountName=" + SAMAccount + ")");

			objDSearch.PropertiesToLoad.Add("samaccountname");

			return objDSearch.FindOne() != null;
		}
		catch(Exception e)
		{
			exception = e.ToString();

			return false;
		}
	}
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
    public static DataTable getAccountInfo(SearchResult objRSearch)
    {
        if (objRSearch == null)
        {
            return null;
        }

        DataTable dtResult = new DataTable();

        dtResult.Columns.Add("FirstName");
        dtResult.Columns.Add("LastName");
        dtResult.Columns.Add("Email");
        dtResult.Rows.Add();
        dtResult.Rows[0]["FirstName"] = GetProperty(objRSearch, "givenName");
        dtResult.Rows[0]["LastName"] = GetProperty(objRSearch, "sn");
        dtResult.Rows[0]["Email"] = GetProperty(objRSearch, "mail");

        return dtResult;
    }
    /// <summary>
    /// <para>
    ///     ¡¡¡¡¡FUNCION DESACTUALIZADA!!!!!
    /// </para>
    /// </summary>
    public static SearchResult getActiveDirectoryAccountNode(String zones, String SAMAccount, ref String exception)
    {
        String appUser = null;
        String appUserPass = null;
        String domain = null;
        String zone = getNextZone(zones);

        if (!setDomainVariables(ref appUser, ref appUserPass, ref domain, zone, ref exception))
        {
            return null;
        }

        SearchResult objRSearch = getSearchResult(SAMAccount, appUser, appUserPass, domain, ref exception);

        if (objRSearch == null)
        {
            if (zones.Equals(zone))
            {
                return null;
            }
            else
            {
                return getActiveDirectoryAccountNode(zones.Remove(0, zone.Length + 1), SAMAccount, ref exception);
            }
        }

        return objRSearch;
    }
    #endregion
}