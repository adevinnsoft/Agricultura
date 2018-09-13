using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using log4net;
using System.Web.Script.Serialization;

public partial class controls_ctrlLoginLdap : System.Web.UI.UserControl
{
    DataAccess data = new DataAccess();
    private static readonly ILog log = LogManager.GetLogger(typeof(controls_ctrlLoginLdap));
    public string UserName
    {
        set
        {
            txtUsername.Text = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            txtUsername.Focus();
            txtPassword.Attributes.Add("OnKeyDown", "if(event.wich || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {" +
                                                     " document.getElementById('" + lnkLogin.ClientID + "').click();return false;}} else {return true}; ");
        }
    }


    protected void lnkLogin_Click(object sender, EventArgs e)
    {
        /*You can use any method to athentificat the user
         *DATA BASE: Check stored procedures, names and create them
         *ACTIVE DIRECTORY: On web.config check the access to AD if it is correcto or if that you need.*/
        try
        {


            bool authenticated = false;
            DataSet ds = null;
            DataTable dt = null;
            var usuario = txtUsername.Text;
            var contrasena = txtPassword.Text;

            var superUsr = ConfigurationManager.AppSettings["SuperUsuario_USR"].ToString();
            var superPsw = ConfigurationManager.AppSettings["SuperUsuario_PSW"].ToString();


            if (ConfigurationManager.AppSettings["bTesting"] == "True")
            {
                dt = userExistsOnDataBase(usuario, contrasena);

                if (dt.Rows.Count == 0)
                {
                    dt = null;
                    lblError.Text = GetLocalResourceObject("nfound").ToString();
                }
            }
            else
            {
                if (superUsr.Equals(usuario) && superPsw.Equals(usuario))
                {
                    dt = userExistsOnDataBase(usuario, contrasena);
                    authenticated = true;
                }
                else
                {
                    //---------------validar ActiveDirectory-----------------//

                    dt = userExistsOnDataBase(txtUsername.Text, contrasena);
                            if (dt.Rows.Count == 0)
                            {
                                dt = null;
                                lblError.Text = GetLocalResourceObject("badUser").ToString();
                                return;
                            }                        
                            
               }
            }

            if (dt != null)
            {
                authenticated = true;
                ds = new DataSet();
                ds.Tables.Add(dt);
                if (dt.Rows.Count > 1)
                {
                    Session["MultiplesPlantas"] = true;
                }
                else
                {
                    Session["MultiplesPlantas"] = false;
                }
            }

            this.ViewState["tbls"] = ds;

            if (authenticated && dt != null)
            {
                SetValuesInSession(dt.Rows.Count > 0 ? dt.Rows[0] : null);

                //CheckEmployeeID();
                Response.Redirect("~/pages/Bienvenida.aspx", false);
            }
            else
            {
                lblError.Text = GetLocalResourceObject("badUser").ToString();
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
    }

    private string CheckEmployeeID()
    {

        string result = "Error";
        string idusrAD = "";
        try
        {
            idusrAD = DGActiveDirectory.getGeneralInfo(DGActiveDirectory.setDomainVariablesAndGetSearchResult("GDL|USA", Session["usernameInj"].ToString(), ref result), Session["usernameInj"].ToString()).Rows[0]["idEmpleado"].ToString();
            if (!Session["idEmpleado"].ToString().Equals(idusrAD) && !string.IsNullOrEmpty(Session["idEmpleado"].ToString()))
            {
                result = data.executeStoreProcedureString("UpdateEmployeeID", new Dictionary<string, object>()
                {
                    {"@username",Session["usernameInj"].ToString()},
                    {"@idEmpleado",idusrAD}
                });
            }
        }
        catch (Exception e)
        {
            log.Error(e);
        }
        return result;
    }

    private DataTable userExistsOnDataBase(string userName, string contrasena)
    {
        DataTable dt = null;
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@user", userName);
        parameters.Add("@password", contrasena);

        try
        {
             dt=data.executeStoreProcedureDataTable("dbo.spr_AccesoUsuario", parameters);
             return dt;
        }
        catch (Exception x)
        {
            lblError.Text = x.Message;       
            log.Error(x);
            return null;
        }
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


    public void SetValuesInSession(DataRow row)
    {
        Session["dtUserInfoInj"] = row;
        Session["userIDInj"] = row != null ? row["idUsuario"].ToString() : null;
        Session["usernameInj"] = txtUsername.Text;
        Session["Nombre"] = row["vNombre"].ToString();
        Session["connection"] = "dbConn";
        Session["Locale"] = row["Lenguaje"].ToString();
        Session["country"] = row["Lenguaje"].ToString();
        Session["idRole"] = row["roleIds"].ToString();
        Session["idEmpleado"] = row["idEmpleado"].ToString();
        Session["idLider"] = row["idEmpleado"].ToString();
        Session["idUsuario"] = row["idUsuario"].ToString();
        Session["Planta"] = row["Planta"].ToString();
        Session["idDepartamento"] = row["idDepartamento"].ToString();
        try
        {
            Session["Asociados"] = GetDataTableToJson(data.executeStoreProcedureDataTable("spr_AsociadosPorNivelLider", new Dictionary<string, object>() { { "@idUsuario", row["idUsuario"].ToString() } }));
            Session["Familias"] = GetDataTableToJson(data.executeStoreProcedureDataTable("spr_FamiliasObtenerJson", null));
        }
        catch (Exception es)
        {

            log.Error(es.Message);
        }
        
    }

    private bool ActiveDirectoryAuthentification(string userName, string password)
    {
        bool bIsOnActiveDirectory = false;

        try
        {
            bIsOnActiveDirectory = isOnActiveDirectory(txtUsername.Text, txtPassword.Text);
        }
        catch(Exception x)
        {
            lblError.Text = System.Configuration.ConfigurationManager.AppSettings.Get("LoginError");
            log.Error(x);
            return false;
        }

        //Check if the user was correct authentificated on AD
        if (bIsOnActiveDirectory)
        {
            return true;
        }
        else
        {
            lblError.Text = System.Configuration.ConfigurationManager.AppSettings.Get("LoginError");

            return false;
        }
    }


    private bool isOnActiveDirectory(string userName, string password)
    {
        string GDLDomain = ConfigurationManager.AppSettings["GDLDomain"];

        //DataTable dt = getUserLogInformation(userName, Security.Encrypt(password),"");
        //IF EXITS ON USER LOG
        try
        {
            return IsAuthenticated("LDAP://" + GDLDomain, GDLDomain, userName, password).Length == 0;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    private string IsAuthenticated(string _path, string domain, string username, string pwd)
    {
        string domainAndUsername = domain + @"\" + username;
        DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domainAndUsername, pwd);

        try
        {
            //Bind to the native AdsObject to force authentication.
            //object obj = entry.NativeObject;
            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");

            if (null == search.FindOne())
            {
                return "Error";
            }

            return "";
        }
        catch (Exception e)
        {
            log.Error(e);
            return "Error";
        }
    }

    protected void lbSalir_Click(object sender, EventArgs e)
    {
        tblLogin.Visible = true;
    }
}