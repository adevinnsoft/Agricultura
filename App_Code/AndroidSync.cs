using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.DirectoryServices;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using log4net;
//using Empaque;

/// <summary>
/// Summary description for AndroidSync
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class AndroidSync : System.Web.Services.WebService
{

    private static readonly ILog log = LogManager.GetLogger(typeof(AndroidSync));
    DataAccess dataaccess = new DataAccess();
    public AndroidSync()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
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

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String SincAutoCosecha(String cosecha, String formaA, String capturaFormaA, String merma,String idUsuario, int checksum)
    {
        String result ="[";
        CosechaAndroid[] Cosechas;
        FormaA[] FormasA;
        CapturaFormaA[] CapturasFormaA;
        Merma[] mermas;

        DataTable dtCosechas = dtCosecha();
        DataTable dtFormasA = dtFormaA();
        DataTable dtCapturasFormaA = null;
        DataTable dtMermas = dtMerma();

        Dictionary<String, Object> param = new Dictionary<string, object>();

        JavaScriptSerializer js = new JavaScriptSerializer();

        try
        {
            Cosechas = js.Deserialize<CosechaAndroid[]>(cosecha);
            FormasA = js.Deserialize<FormaA[]>(formaA);
            CapturasFormaA = js.Deserialize<CapturaFormaA[]>(capturaFormaA);
            mermas = js.Deserialize<Merma[]>(merma);

            dtCapturasFormaA = new CapturaFormaA().toDataTable();

            foreach (CosechaAndroid ca in Cosechas)
            {
                DataRow dr = dtCosechas.NewRow();

                dr["idCosecha"] = ca.idCosecha;
                dr["idCosechaTab"] = ca.idCosechaLocal;
                dr["idActividadPrograma"] = ca.idActividadPrograma;
                dr["idActividadProgramaTab"] = ca.idActividadProgramaLocal;
                dr["fechaInicio"] = ca.fechaInicio;
                dr["fechaFin"] = ca.fechaFin;
                dr["cantidadProduccion"] = ca.cantidadProduccion;
                dr["estimadoMedioDia"] = ca.estimadoMedioDia;
                dr["cerrada"] = ca.cerrada;
                dr["estatus"] = ca.estatus;

                dtCosechas.Rows.Add(dr);

            }

            foreach (FormaA fa in FormasA)
            {
                DataRow dr = dtFormasA.NewRow();

                dr["idFormaA"] = fa.idFormaA;
                dr["idFormaATab"] = fa.idFormaALocal;
                dr["idPrograma"] = fa.idPrograma;
                dr["idProgramaTab"] = fa.idProgramaLocal;
                dr["fechaFin"] = fa.fechaFin;
                dr["fechaInicio"] = fa.fechaInicio;
                dr["prefijo"] = fa.prefijo;
                dr["dmcCalidad"] = fa.dmcCalidad;
                dr["dmcMercado"] = fa.dmcMercado;
                dr["comentarios"] = fa.comentarios;
                dr["folio"] = fa.folio;
                dr["cerrada"] = fa.cerrada;
                dr["estatus"] = fa.estatus;
                dr["fechaFinTractorista"] = fa.fechaFinTractorista;
                dr["fechaInicioTractorista"] = fa.fechaInicioTractorista;
                dr["storage"] = fa.storage;

                dtFormasA.Rows.Add(dr);
            }

            foreach (CapturaFormaA fa in CapturasFormaA)
            {
                dtCapturasFormaA.Rows.Add(fa.toDataRow(dtCapturasFormaA));
            }

            foreach (Merma merm in mermas)
            {
                DataRow dr = dtMermas.NewRow();

                dr["idMerma"] = merm.idMerma;
                dr["idMermaTab"] = merm.idMermaLocal;
                dr["idCoseha"] = merm.idCosecha;
                dr["idCosechaTab"] = merm.idCosechaLocal;
                dr["idRazon"] = merm.idRazon;
                dr["cantidad"] = merm.cantidad;
                dr["observacion"] = merm.observacion;
                dr["estatus"] = merm.estatus;

                dtMermas.Rows.Add(dr);
            }


        }
        catch (Exception es)
        {
            log.Error(es);

            dtCapturasFormaA = null;
        }
        finally
        {
            param.Add("@cosecha", dtCosechas);
            param.Add("@formaA", dtFormasA);
            param.Add("@capturaFormaA", dtCapturasFormaA);
            param.Add("@merma", dtMermas);
            param.Add("@idUsuario", idUsuario);
            param.Add("@checksum",checksum );
        }

        DataSet ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncCosecha", param);
        int i = 1;
        foreach (DataTable item in ds.Tables)
        {
            result += tablasCosecha(i++) + GetDataTableToJson(item);
        }

        return result;
    }



    private string tablasCosecha(int i)
    {
        switch (i)
        {
            case 1:
                return "'Cosecha':";

            case 2:
                return ",'FormaA':";

            case 3:
                return ",'CapturaFormaA':";

            case 4:
                return ",'Merma':";

            case 5:
                return ",'Checksum':";
            default:
                return "";

        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getAutentication(string user, string pass)
    {
        /*You can use any method to athentificat the user
      *DATA BASE: Check stored procedures, names and create them
      *ACTIVE DIRECTORY: On web.config check the access to AD if it is correcto or if that you need.
      */
        bool authenticated = false;
        DataSet dt = null;
        if (ConfigurationManager.AppSettings["bTesting"] == "True")
        {
            dt = userExistsOnDataBase(user);
        }

        else
        {
            //---------------validar ActiveDirectory-----------------//
            if (ConfigurationManager.AppSettings["validoActiveDirectory"] == "True")
            {
                authenticated = ActiveDirectoryAuthentification(user.Trim(), user.Trim());

                if (authenticated)
                    dt = userExistsOnDataBase(user);
            }
        }


        if (dt != null)
            if (dt.Tables.Count >= 0)
            {
                authenticated = true;
            }


        if (authenticated)
        {

        }
        else
        {

        }
        return GetDataTableToJson(dt.Tables[0]);

    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getAutenticationV2(string user, string pass, String DeviceID)
    {
        /*You can use any method to athentificat the user
      *DATA BASE: Check stored procedures, names and create them
      *ACTIVE DIRECTORY: On web.config check the access to AD if it is correcto or if that you need.
      */
        bool authenticated = false;
        DataSet dt = null;
        if (ConfigurationManager.AppSettings["bTesting"] == "True")
        {
            dt = userExistsOnDataBase(user,DeviceID, pass);
        }

        else
        {
            //---------------validar ActiveDirectory-----------------//
            if (ConfigurationManager.AppSettings["validoActiveDirectory"] == "True")
            {
                authenticated = ActiveDirectoryAuthentification(user.Trim(), user.Trim());

                if (authenticated)
                    dt = userExistsOnDataBase(user);
            }
        }


        if (dt != null)
            if (dt.Tables.Count >= 0)
            {
                authenticated = true;
            }


        if (authenticated)
        {

        }
        else
        {

        }
        return GetDataTableToJson(dt.Tables[0]);

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPermisosUsuario(String id)
    {
        return dataaccess.executeStoreProcedureString("sprAndroid_getPermisosUsuario", new Dictionary<string, object>() { { "@idUsuario", id } });
    }


    private bool ActiveDirectoryAuthentification(string userName, string password)
    {
        bool bIsOnActiveDirectory = false;
        try
        {
            bIsOnActiveDirectory = isOnActiveDirectory(userName.Trim(), password.Trim());
        }
        catch (Exception x)
        {
            log.Error(x);
        }
        //Check if the user was correct authentificated on AD
        if (bIsOnActiveDirectory)
        {
            return true;
        }
        else
        {

            return false;
        }
    }

    private bool isOnActiveDirectory(string userName, string password)
    {
        string GDLDomain = ConfigurationManager.AppSettings["GDLDomain"];

        string errorSpeech = string.Empty;
        //DataTable dt = getUserLogInformation(userName, Security.Encrypt(password),"");
        //IF EXITS ON USER LOG
        try
        {
            errorSpeech = this.IsAuthenticated("LDAP://" + GDLDomain, GDLDomain, userName, password);
            if (errorSpeech == "")
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw new Exception(ex.Message);
        }
    }

    private string IsAuthenticated(string _path, string domain, string username, string pwd)
    {
        string domainAndUsername = domain + @"\" + username;
        //string errorSpeech = lablesXML.getNameSpanish("errorSpeech");
        DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:389", domain), domainAndUsername, pwd);

        try
        {
            //Bind to the native AdsObject to force authentication.
            //object obj = entry.NativeObject;

            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (null == result)
            {
                return "Error";// errorSpeech;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "Error";
        }

        return "";
    }


    #region GetInfo tables
    private DataSet userExistsOnDataBase(string userName)
    {
        System.Collections.Generic.Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@user", userName);
        //parameters.Add("@pass", pass);
        DataSet dt = new DataSet();

        try
        {
            DataTable dtBD = dataaccess.executeStoreProcedureDataTable("dbo.sprAndroid_AccesoUsuario", parameters);
            dt.Tables.Add(dtBD);

            return dt;
        }

        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    private DataSet userExistsOnDataBase(string userName, String DeviceID, string password)
    {
        System.Collections.Generic.Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@user", userName);
        parameters.Add("@DeviceID", DeviceID);
        parameters.Add("@password", password);
        DataSet dt = new DataSet();

        try
        {
            DataTable dtBD = dataaccess.executeStoreProcedureDataTable("dbo.sprAndroid_AccesoUsuarioV2", parameters);
            dt.Tables.Add(dtBD);

            return dt;
        }

        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    [WebMethod]
    public string infoReceive(string requerimientosSiembra, string sd)
    {
        return "Hello World";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string GetDataCode(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getDataCode", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string InsertaEmpleado(int noEmpleado, string nombreEmpleado,int idPlanta,int idSupervisor,string fechaAlta,string fechaBaja, int estatus, int idAsociado, int idUsuario)
    {
      
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@noEmpleado", noEmpleado);
            param.Add("@nombreEmpleado", nombreEmpleado);
            param.Add("@idPlanta", idPlanta);
            param.Add("@idSupervisor", idSupervisor);
            param.Add("@fechaAlta", fechaAlta);
            param.Add("@fechaBaja", fechaBaja);
            param.Add("@estatus", estatus);
            param.Add("@idAsociado", idAsociado);
            param.Add("@idUsuario", idUsuario);

            DataTable data = dataaccess.executeStoreProcedureDataTable("procAltaBajaEmpleado", param);
            return data.Rows[0][0].ToString();
        
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getHabilidades(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getHabilidades", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEtapas(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEtapas", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEquiposDeTrabajo(string idUsuario, string idPlanta)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idLider", idUsuario);
            param.Add("@idPlanta", idPlanta);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEquiposDeTrabajo", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCostoActividad(string idPlanta)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idPlanta", idPlanta);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCostoPorActividad", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }
  

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getInvernaderos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getInvernaderos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPlantas(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getPlantas", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getSecciones(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getSecciones", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getSurcos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getSurcos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCriterios(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCriterios", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getVersionAPK(string id)
    {
        try
        {
            
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getVersionAPK", null);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getParametros(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getParametros", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getActividadPeriodo(string id, string checksum, string invernaderos)
    {
        
        try
        {
            

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getActividadPeriodo", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    private DataTable toParam(String Invernaderos)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        Invernaderos[] invs;
        DataTable dt = new DataTable();
        try
        {
            invs = js.Deserialize<Invernaderos[]>(Invernaderos);

            
            dt.Columns.Add("id");
            foreach (var inv in invs)
            {
                DataRow dr = dt.NewRow();

                dr["id"] = inv.idInvernadero;

                dt.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            //log.Error("Es normal que truene aquí, no afecta."+ex);
            return null;
        }

        return dt;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getActividadPrograma(string id, string checksum, String invernaderos)
    {
        
        try
        {
            
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getActividadPrograma", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getNiveles(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getNiveles", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEtapaArticulos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEtapaArticulos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEtapaElementos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEtapaElementos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEtapaTarget(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEtapaTarget", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFamilias(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getFamilias", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getAsociadosNivel(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getAsociadosNivel", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFumigaciones(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getFumigaciones", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getLiderInvernaderos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getLiderInvernaderos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getInvernaderoZonificacionPorIdLider(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getLiderInvernaderos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEficienciaAsociados(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEficienciaAsociados", null);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getProductos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getProductos", null);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getZonificaciones(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getZonificaciones", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getVariedades(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getVariedades", null);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFamiliaNivel(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getFamiliaNivel", null);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getBrixCaptura(string id, String checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getBrixCaptura", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getBrixColor(string idLider, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", idLider);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getBrixColor", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getBrixDefecto(string idLider, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", idLider);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getBrixDefecto", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getBrixDetalle(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getBrixDetalle", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getBrixFirmeza(string idLider, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", idLider);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getBrixFirmeza", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getBrixHeader(string idLider, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", idLider);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getBrixHeader", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPlagas(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getPlagas", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getDirectriz(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getDirectriz", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCiclos(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCiclos", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getTemporales(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getTemporales", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getRazonDirectriz(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getRazonDirectriz", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getActividadesAsociados(string id, string checksum, String invernaderos)
    {
        
        try
        {
            

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getActividadesAsociados", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

  
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCheckCriterio(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCheckCriterio", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCheckList(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCheckList", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getActividadNoProgramada(string id, string checksum, string invernaderos)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getActividadNoProgramada", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getMermaCosecha(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getMermaCosecha", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getMermaTomate(string id, string checksum, string invernaderos)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getMermaTomate", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getRadiografiaInvernadero(string Invernadero)
    {
        String result = "";
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idInvernadero", Invernadero);
            DataSet data = dataaccess.executeStoreProcedureDataSet("sprAndroid_getRadiografiaInvernadero", param);
            
                
            result += GetDataTableToJson(data.Tables[0]) ;
            result = result.Replace("\"datos\":\"datos\"", "'chartdata':"+GetDataTableToJson(data.Tables[1]));
            result = result.Replace("\"lideres\":\"usuarios\"", "'lideres':"+GetDataTableToJson(data.Tables[2]));
             
            
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCosechas(string id, string checksum, string invernaderos)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCosechas", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getTrasladoMerma()
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getTrasladoMerma", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFormaA(string id, string checksum, string invernaderos)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getFormaA", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCapturaFormaA(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCapturaFormaA", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getSeccionPreharvest(string id, string checksum, string invernaderos)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", id);
            param.Add("@checksum",checksum);
            param.Add("@invernaderos", toParam(invernaderos));

            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getSeccionPreharvest", param);
            return GetDataTableToJson(data);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCapturaFormaAV2(string id, string checksum, string invernaderos)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            param.Add("@invernaderos", toParam(invernaderos));
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCapturaFormaAV2", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFechasOficiales(int id)
    {
        DataTable dt = new DataTable();

        try
        {
            dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_FechasOficialesProgramacionSemanal", new Dictionary<string, object>() { { "@idLider", id } });
            return GetDataTableToJson(dt);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPercances()
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_GetPercances", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getZonaMonitoreo()
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_GetZonaMonitoreo", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getTipoCajaCosecha()
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getTipoCajas", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getTipoCosecha()
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getTipoCosecha", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    /*Captura Cajas: envio tablet */
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCajasCaptura(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCajasCaptura", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCajasCapturaDetalle(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getCajasCapturaDetalle", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    /*Ordenes de Embarque: envio tablet */
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCatEmbarqueHeader(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueHeader", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getCatEmbarqueDetalle(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueDetalle", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getDetalleDeEmbarque(string id, string codigoEmbarque)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@codigoEmbarque", codigoEmbarque);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueDetalle", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }
        

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getSKUProduct()
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getSkuProducto", null);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getInfoEmbarqueFromQR(string QR)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@QR", QR);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueInfoFromQR", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueFIFO(string id)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@IdLider", id);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueFIFO", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueIncidencia(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idPlanta", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getEmbarqueIncidencia", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }


    /*Folios de Empaque*/
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFoliosEmpaque(string idPlanta)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idPlanta", idPlanta);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getFoliosEmpaque", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string infoSend(string id)
    {
        return "[{\"Value\":1}]";
    }

    [WebMethod]
    public string Configuraciones()
    {
        string result;
        DataTable dt = new DataTable();

        dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_getConfiguraciones", null);

        result = GetDataTableToJson(dt);

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getMonitoreo(int id, string checksum)
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@idLider", id);
        param.Add("@checksum", checksum);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_GetMonitoreo", param);
        return GetDataTableToJson(dt);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getMonitoreoV2(int id)
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("@idLider", id);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_GetMonitoreoV2", param);
        return GetDataTableToJson(dt);
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getChecksum()
    {
        return "[{'checksum':" + dataaccess.executeStoreProcedureGetInt("spr_GetChecksum", null) + "}]";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmpaqueColoresBrix()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_EmpaqueColoresBrix", param);
        return GetDataTableToJson(dt);
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmpaqueDefectos()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_EmpaqueDefecto", param);
        return GetDataTableToJson(dt);
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmpaqueFirmeza()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_EmpaqueFirmeza", param);
        return GetDataTableToJson(dt);
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmpaqueSegmento()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_EmpaqueSegmento", param);
        return GetDataTableToJson(dt);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueIncidenciaMotivo()
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("sprAndroid_EmbarqueIncidenciaMotivo", param);
        return GetDataTableToJson(dt);
    }
    #endregion

    #region Sincronizacion


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String validaFoliosInbound(string Folios)
    {
        FoliosValidaIbound[] FoliosValidar;
        JavaScriptSerializer js = new JavaScriptSerializer();
        Dictionary<string, object> param = new Dictionary<string, object>();

        DataTable dtFolios = null;
        string result = "";
        
        try
        {
            FoliosValidar = js.Deserialize<FoliosValidaIbound[]>(Folios);
            dtFolios = new FoliosValidaIbound().toDataTable();

            foreach (FoliosValidaIbound row in FoliosValidar)
            {
                dtFolios.Rows.Add(row.toDataRow(dtFolios));
            }


            result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_ValidaFoliosInbound", new Dictionary<string, object>() { { "@folios", dtFolios } }));
            
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return result;
    }



    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String validaFoliosInboundV2(string Folios)
    {
        FoliosValidaIboundV2[] FoliosValidar;
        JavaScriptSerializer js = new JavaScriptSerializer();
        Dictionary<string, object> param = new Dictionary<string, object>();

        DataTable dtFolios = null;
        string result = "";

        try
        {
            FoliosValidar = js.Deserialize<FoliosValidaIboundV2[]>(Folios);
            dtFolios = new FoliosValidaIboundV2().toDataTable();

            foreach (FoliosValidaIboundV2 row in FoliosValidar)
            {
                dtFolios.Rows.Add(row.toDataRow(dtFolios));
            }


            result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_ValidaFoliosInboundV2", new Dictionary<string, object>() { { "@folios", dtFolios } }));

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncAll(string actividades, string periodos, string asociados, string noProgramadas, string cosecha, string formaA, string capturaFormaA, string merma, string Traslado, string monitoreo, int idUsuario, int checksum, string checkList, string checkCriterio, string BrixCaptura, string BrixDetalle, string BrixHeader, string BrixFirmeza, string BrixColor, string BrixDefecto, string CajasCaptura, string CajasCapturaDetalle)
    {
        string result = "{";
        int estatusCheckList = 0;
        int estatusCheckCriterio = 0;
        //log.Info(capturaFormaA);

        JavaScriptSerializer js = new JavaScriptSerializer();
        int cont = 0;

        ActividadProgramaAndroid[] Actividades;
        PeriodoAndroid[] Periodos;
        ActividadAsociadoAndroid[] Asociados;
        ActividadNoProgramadaAndroid[] NoProgramadas;
        CosechaAndroid[] Cosechas;
        FormaAv2[] FormasA;
        CapturaFormaAv2[] CapturasFormaA;
        Merma[] mermas;
        TrasladoMerma[] Traslados;
        Monitoreo[] Monitoreos;
        CheckList[] CheckLists;
        CheckCriterio[] checkCriterios;
        BrixCaptura[] BrixCapturas;
        BrixDetalle[] BrixDetalles;
        CajasCaptura[] CajasCapturas;
        CajasCapturaDetalle[] CajasCapturaDetalles;

        //Parámetros para stored procedure
        Dictionary<string, object> param = new Dictionary<string, object>();

        //tablas para stored procedure
        DataTable dtActividadesProgramadas = dtActividadProgramada();
        DataTable dtPeriodos = dtActividadPeriodos();
        DataTable dtJornales = dtActividadJornales();
        DataTable dtNoProgramadas = dtActividadNoProgramada();

        DataTable dtCosechas = dtCosecha();
        DataTable dtFormasA = dtFormaAv2();
        DataTable dtCapturasFormaA = null;
        DataTable dtMermas = dtMerma();
        DataTable dtTrasladoMermas = dtTrasladoMerma();

        DataTable dtMonitoreos = null;
        DataTable dtChecklists = null;
        DataTable dtCheckcriterios = null;

        DataTable dtBrixCapturas = dtBrixCaptura();
        DataTable dtBrixDetalles = dtBrixDetalle();
        DataTable dtBrixHeader = getDtBrixHeader();
        DataTable dtBrixFirmeza = getDtBrixFirmeza();
        DataTable dtBrixColor = getDtBrixColor();
        DataTable dtBrixDefecto = getDtBrixDefecto();

        DataTable dtCajasCapturas = dtCajasCaptura();
        DataTable dtCajasCapturaDetalles = dtCajasCapturaDetalle();

        try
        {
            Actividades = js.Deserialize<ActividadProgramaAndroid[]>(actividades);
            Periodos = js.Deserialize<PeriodoAndroid[]>(periodos);
            Asociados = js.Deserialize<ActividadAsociadoAndroid[]>(asociados);
            NoProgramadas = js.Deserialize<ActividadNoProgramadaAndroid[]>(noProgramadas);

            //Id**Tab es el id***Local del objeto, ya que en la tablet está como local, pero en Servidor se guarda como Tab, 

            if (Actividades != null)
                foreach (ActividadProgramaAndroid A in Actividades)
                {
                    DataRow dt = dtActividadesProgramadas.NewRow();

                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idInvernadero"] = A.idInvernadero;
                    dt["idEtapa"] = A.idEtapa;
                    dt["idCiclo"] = A.idCiclo;
                    dt["cantidadDeElementos"] = A.cantidadDeElementos;
                    dt["semana"] = A.semana;
                    dt["jornalesEstimados"] = A.jornalesEstimados;
                    dt["minutosEstimados"] = A.minutosEstimados;
                    dt["esDirectriz"] = A.esDirectriz;
                    dt["esInterplanting"] = A.esInterplanting;
                    dt["borrado"] = A.borrado;
                    dt["aprobadaPor"] = A.aprobadaPor;
                    dt["rechazadaPor"] = A.rechazadaPor;
                    dt["usuarioModifica"] = A.usuarioModifica;
                    dt["surcoInicio"] = A.surcoInicio;
                    dt["surcoFin"] = A.surcoFin;
                    dt["esColmena"] = A.esColmena;
                    dt["estatus"] = A.estatus;
                    dt["UUID"] = A.UUID;
                    dtActividadesProgramadas.Rows.Add(dt);
                }

            if (Asociados != null)
                foreach (ActividadAsociadoAndroid A in Asociados)
                {
                    DataRow dt = dtJornales.NewRow();

                    dt["idActividadAsociado"] = A.idAsociadoActividad;
                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idPeriodoTab"] = A.idActividadPeriodoLocal;
                    dt["idPeriodo"] = A.idActividadPeriodo;
                    dt["idAsociado"] = A.idAsociado;
                    dt["ausente"] = A.ausente;
                    dt["estatus"] = A.estatus;

                    dtJornales.Rows.Add(dt);
                }

            if (Periodos != null)
                foreach (PeriodoAndroid p in Periodos)
                {
                    DataRow dt = dtPeriodos.NewRow();
                    dt["idPeriodo"] = p.idActividadPeriodo;
                    dt["idPeriodoTab"] = p.idActividadPeriodoLocal;
                    dt["idActividad"] = p.idActividadPrograma;
                    dt["idActividadTab"] = p.idActividadProgramaLocal;
                    dt["surcos"] = p.surcos;
                    dt["inicio"] = p.inicio.ToString("yyyy-MM-dd HH:mm");
                    dt["fin"] = p.fin.ToString("yyyy-MM-dd HH:mm");
                    dt["estatus"] = p.estatus;
                    dt["UUID"] = p.UUID;
                    dtPeriodos.Rows.Add(dt);
                }

            if (NoProgramadas != null)
                foreach (ActividadNoProgramadaAndroid act in NoProgramadas)
                {
                    DataRow dt = dtNoProgramadas.NewRow();

                    dt["idActividadNoProgramada"] = act.idActividadNoProgramada;
                    dt["idActividadNoProgramadaTab"] = act.idActividadNoProgramadaLocal;
                    dt["idInvernadero"] = act.idInvernadero;
                    dt["idEtapa"] = act.idEtapa;
                    dt["idCiclo"] = act.idCiclo;
                    dt["razon"] = act.razon;
                    dt["comentarios"] = act.comentario;
                    dt["cantidadDeElementos"] = act.cantidadDeElementos;
                    dt["semanaProgramacion"] = act.semanaProgramacion;
                    dt["anioProgramacion"] = act.anioProgramacion;
                    dt["esInterplanting"] = act.esInterplanting;
                    dt["estatus"] = act.estatus;
                    dt["UUID"] = act.UUID;
                    dtNoProgramadas.Rows.Add(dt);
                }
        }
        catch (Exception ex)
        {

            dtActividadesProgramadas.Clear();
            dtPeriodos.Clear();
            dtJornales.Clear();
            dtNoProgramadas.Clear();
            log.Error(ex);
        }
        finally
        {
            param.Add("@Actividades", dtActividadesProgramadas);
            param.Add("@Periodos", dtPeriodos);
            param.Add("@Asociados", dtJornales);
            param.Add("@NoProgramadas", dtNoProgramadas);
        }

        try
        {
            Cosechas = js.Deserialize<CosechaAndroid[]>(cosecha);
            FormasA = js.Deserialize<FormaAv2[]>(formaA);
            CapturasFormaA = js.Deserialize<CapturaFormaAv2[]>(capturaFormaA);
            mermas = js.Deserialize<Merma[]>(merma);
            Traslados = js.Deserialize<TrasladoMerma[]>(Traslado);

            dtCapturasFormaA = new CapturaFormaAv2().toDataTable();

            foreach (CosechaAndroid ca in Cosechas)
            {
                DataRow dr = dtCosechas.NewRow();

                dr["idCosecha"] = ca.idCosecha;
                dr["idCosechaTab"] = ca.idCosechaLocal;
                dr["idActividadPrograma"] = ca.idActividadPrograma;
                dr["idActividadProgramaTab"] = ca.idActividadProgramaLocal;
                dr["fechaInicio"] = ca.fechaInicio;
                dr["fechaFin"] = ca.fechaFin;
                dr["cantidadProduccion"] = ca.cantidadProduccion;
                dr["estimadoMedioDia"] = ca.estimadoMedioDia;
                dr["cerrada"] = ca.cerrada;
                dr["estatus"] = ca.estatus;

                dtCosechas.Rows.Add(dr);

            }

            foreach (FormaAv2 fa in FormasA)
            {
                DataRow dr = dtFormasA.NewRow();

                dr["idFormaA"] = fa.idFormaA;
                dr["idFormaATab"] = fa.idFormaALocal;
                dr["idPrograma"] = fa.idPrograma;
                dr["idProgramaTab"] = fa.idProgramaLocal;
                dr["fechaFin"] = fa.fechaFin;
                dr["fechaInicio"] = fa.fechaInicio;
                dr["prefijo"] = fa.prefijo;
                dr["dmcCalidad"] = fa.dmcCalidad;
                dr["dmcMercado"] = fa.dmcMercado;
                dr["comentarios"] = fa.comentarios;
                dr["folio"] = fa.folio;
                dr["cerrada"] = fa.cerrada;
                dr["estatus"] = fa.estatus;
                dr["fechaFinTractorista"] = fa.fechaFinTractorista;
                dr["fechaInicioTractorista"] = fa.fechaInicioTractorista;
                dr["storage"] = fa.storage;
                dr["UUID"] = fa.UUID;

                dtFormasA.Rows.Add(dr);
            }

            foreach (CapturaFormaAv2 fa in CapturasFormaA)
            {
                dtCapturasFormaA.Rows.Add(fa.toDataRow(dtCapturasFormaA));
            }

            foreach (Merma merm in mermas)
            {
                DataRow dr = dtMermas.NewRow();

                dr["idMerma"] = merm.idMerma;
                dr["idMermaTab"] = merm.idMermaLocal;
                dr["idCoseha"] = merm.idCosecha;
                dr["idCosechaTab"] = merm.idCosechaLocal;
                dr["idRazon"] = merm.idRazon;
                dr["cantidad"] = merm.cantidad;
                dr["observacion"] = merm.observacion;
                dr["estatus"] = merm.estatus;
                dr["UUID"] = merm.UUID;
                dtMermas.Rows.Add(dr);
            }

            foreach (TrasladoMerma t in Traslados)
            {
                DataRow dr = dtTrasladoMermas.NewRow();

                dr["idTrasladoMerma"] = t.idTrasladoMerma;
                dr["idTrasladoMermaLocal"] = t.idTrasladoMermaLocal;
                dr["idFormaA"] = t.idFormaA;
                dr["idFormaALocal"] = t.idFormaALocal;
                dr["idRazon"] = t.idRazon;
                dr["Cajas"] = t.Cajas;
                dr["Comentarios"] = t.Comentarios;
                dr["Estatus"] = t.Estatus;
                dr["UUID"] = t.UUID;
                dtTrasladoMermas.Rows.Add(dr);
            }

        }
        catch (Exception es)
        {
            log.Error(es);

            dtCapturasFormaA = null;
        }
        finally
        {
            param.Add("@cosecha", dtCosechas);
            param.Add("@formaA", dtFormasA);
            param.Add("@capturaFormaA", dtCapturasFormaA);
            param.Add("@merma", dtMermas);
            param.Add("@Traslado", dtTrasladoMermas);
        }

        try
        {

            Monitoreos = js.Deserialize<Monitoreo[]>(monitoreo);
            CheckLists = js.Deserialize<CheckList[]>(checkList);
            checkCriterios = js.Deserialize<CheckCriterio[]>(checkCriterio);


            dtMonitoreos = new Monitoreo().toDataTable();
            dtChecklists = new CheckList().toDataTable();
            dtCheckcriterios = new CheckCriterio().toDataTable();

            foreach (Monitoreo item in Monitoreos)
            {
                dtMonitoreos.Rows.Add(item.toDataRow(dtMonitoreos));
            }

            foreach (CheckList item in CheckLists)
            {
                dtChecklists.Rows.Add(item.toDataRow(dtChecklists));
            }

            foreach (CheckCriterio item in checkCriterios)
            {
                dtCheckcriterios.Rows.Add(item.toDataRow(dtCheckcriterios));
            }
        }
        catch (Exception exsd)
        {
            log.Error(exsd);
            throw;
        }
        finally
        {
            param.Add("@Monitoreo", dtMonitoreos);
            param.Add("@CheckList", dtChecklists);
            param.Add("@CheckCriterio", dtCheckcriterios);
        }


        try
        {
            BrixCapturas = js.Deserialize<BrixCaptura[]>(BrixCaptura);
            BrixDetalles = js.Deserialize<BrixDetalle[]>(BrixDetalle);

            foreach (var captura in BrixCapturas)
            {
                DataRow dr = dtBrixCapturas.NewRow();
                dr["fechaCaptura"] = captura.fechaCaptura;
                dr["estatus"] = captura.estatus;
                dr["idActividadPrograma"] = captura.idActividadPrograma;
                dr["idActividadProgramaTab"] = captura.idActividadProgramaLocal;
                dr["idBrixCaptura"] = captura.idBrixCaptura;
                dr["idBrixCapturaTab"] = captura.idBrixCapturaLocal;
                dr["idInvernadero"] = captura.idInvernadero;
                dr["idSeccion"] = captura.idSeccion;
                dr["idCalidad"] = captura.idCalidad;
                dr["libras"] = captura.libras;
                dr["UUID"] = captura.UUID;
                dtBrixCapturas.Rows.Add(dr);

            }

            foreach (var detalle in BrixDetalles)
            {
                DataRow dr = dtBrixDetalles.NewRow();

                dr["idBrixCaptura"] = detalle.idBrixCaptura;
                dr["estatus"] = detalle.estatus;
                dr["brix"] = detalle.brix;
                dr["idBrixCapturaTab"] = detalle.idBrixCapturaLocal;
                dr["idBrixDetalle"] = detalle.idBrixDetalle;
                dr["idBrixDetalleTab"] = detalle.idBrixDetalleLocal;
                dr["idColor"] = detalle.idColor;
                dr["UUID"] = detalle.UUID;
                dtBrixDetalles.Rows.Add(dr);
            }


            BrixHeader[] brixHeaders = js.Deserialize<BrixHeader[]>(BrixHeader);
            BrixFirmeza[] brixFirmezas = js.Deserialize<BrixFirmeza[]>(BrixFirmeza);
            BrixColor[] brixColors = js.Deserialize<BrixColor[]>(BrixColor);
            BrixDefecto[] brixDefectos = js.Deserialize<BrixDefecto[]>(BrixDefecto);

            DataRow R = null;
            foreach (BrixHeader header in brixHeaders)
            {
                R = dtBrixHeader.NewRow();
                R["idBrixHeader"] = header.idBrixHeader;
                R["idBrixHeaderLocal"] = header.idBrixHeaderLocal;
                R["idBrixCaptura"] = header.idBrixCaptura;
                R["idBrixCapturaLocal"] = header.idBrixCapturaLocal;
                R["idProducto"] = header.idProducto;
                R["idSegmento"] = header.idSegmento;
                R["CajasTotales"] = header.CajasTotales; // DEPRECATED, VALOR EN 0
                R["Folio"] = header.Folio; // DEPRECATED, VALOR EN 0
                R["Comentarios"] = header.Comentarios;
                R["idUsuarioCaptura"] = header.idUsuarioCaptura;
                R["FechaCaptura"] = header.FechaCaptura;
                R["idUsuarioModifica"] = header.idUsuarioModifica;
                R["FechaModifica"] = header.FechaModifica;
                R["Estatus"] = header.estatus;
                R["UUID"] = header.UUID;
                dtBrixHeader.Rows.Add(R);
            }
            foreach (BrixFirmeza firmeza in brixFirmezas)
            {
                R = dtBrixFirmeza.NewRow();
                R["idBrixFirmeza"] = firmeza.idBrixFirmeza;
                R["idBrixFirmezaLocal"] = firmeza.idBrixFirmezaLocal;
                R["idBrixHeader"] = firmeza.idBrixHeader;
                R["idBrixHeaderLocal"] = firmeza.idBrixHeaderLocal;
                R["idFirmeza"] = firmeza.idFirmeza;
                R["idSegmento"] = firmeza.idSegmento;
                R["Value"] = firmeza.Value;
                R["Porcentaje"] = firmeza.porcentaje;
                R["Estatus"] = firmeza.estatus;
                R["UUID"] = firmeza.UUID;
                dtBrixFirmeza.Rows.Add(R);
            }
            foreach (var color in brixColors)
            {
                R = dtBrixColor.NewRow();
                R["idBrixColor"] = color.idBrixColor;
                R["idBrixColorLocal"] = color.idBrixColorLocal;
                R["idBrixHeader"] = color.idBrixHeader;
                R["idBrixHeaderLocal"] = color.idBrixHeaderLocal;
                R["idColor"] = color.idColor;
                R["idSegmento"] = color.idSegmento;
                R["Value"] = color.Value;
                R["Porcentaje"] = color.porcentaje;
                R["Estatus"] = color.estatus;
                R["UUID"] = color.UUID;
                dtBrixColor.Rows.Add(R);
            }
            foreach (var defecto in brixDefectos)
            {
                R = dtBrixDefecto.NewRow();
                R["idBrixDefecto"] = defecto.idBrixDefecto;
                R["idBrixDefectoLocal"] = defecto.idBrixDefectoLocal;
                R["idBrixHeader"] = defecto.idBrixHeader;
                R["idBrixHeaderLocal"] = defecto.idBrixHeaderLocal;
                R["idDefecto"] = defecto.idDefecto;
                R["idSegmento"] = defecto.idSegmento;
                R["Value"] = defecto.Value;
                R["Porcentaje"] = defecto.porcentaje;
                R["Estatus"] = defecto.estatus;
                R["UUID"] = defecto.UUID;
                dtBrixDefecto.Rows.Add(R);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@BrixCaptura", dtBrixCapturas);
            param.Add("@BrixDetalle", dtBrixDetalles);
            param.Add("@BrixHeader", dtBrixHeader);
            param.Add("@BrixColor", dtBrixColor);
            param.Add("@BrixFirmeza", dtBrixFirmeza);
            param.Add("@BrixDefecto", dtBrixDefecto);
        }

        try
        {
            CajasCapturas = js.Deserialize<CajasCaptura[]>(CajasCaptura);
            CajasCapturaDetalles = js.Deserialize<CajasCapturaDetalle[]>(CajasCapturaDetalle);

            foreach (var captura in CajasCapturas)
            {
                DataRow dr = dtCajasCapturas.NewRow();
                dr["idEstimadocajas"] = captura.idEstimadocajas;
                dr["idEstimadocajasLocal"] = captura.idEstimadocajasLocal;
                dr["idInvernadero"] = captura.idInvernadero;
                dr["idLider"] = captura.idLider;
                dr["idCosecha"] = captura.idCosecha;
                dr["idCosechaLocal"] = captura.idCosechaLocal;
                dr["surcos"] = captura.surcos;
                dr["semana"] = captura.semana;
                dr["borrado"] = captura.borrado;
                dr["usuarioCaptura"] = captura.usuarioCaptura;
                dr["usuarioModifica"] = captura.usuarioModifica;
                dr["fechaCaptura"] = captura.fechaCaptura;
                dr["fechaModifica"] = captura.fechaModifica;
                dr["estatus"] = captura.estatus;
                dr["UUID"] = captura.UUID;
                dtCajasCapturas.Rows.Add(dr);
            }

            foreach (var detalle in CajasCapturaDetalles)
            {
                DataRow dr = dtCajasCapturaDetalles.NewRow();

                dr["idEstimadoCajasCaptura"] = detalle.idEstimadoCajasCaptura;
                dr["idEstimadoCajasCapturaLocal"] = detalle.idEstimadoCajasCapturaLocal;
                dr["idEstimadoCajas"] = detalle.idEstimadoCajas;
                dr["idEstimadoCajasLocal"] = detalle.idEstimadoCajasLocal;
                dr["surco"] = detalle.surco;
                dr["cajas"] = detalle.cajas;
                dr["estimado"] = detalle.estimado;
                dr["fechaCaptura"] = detalle.fechaCaptura;
                dr["fechaModifica"] = detalle.fechaModifica;
                dr["borrado"] = detalle.borrado;
                dr["estatus"] = detalle.estatus;
                dr["idAsociado"] = detalle.idAsociado;
                dr["asignado"] = detalle.asignado;
                dr["UUID"] = detalle.UUID;
                dtCajasCapturaDetalles.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@CajasCaptura", dtCajasCapturas);
            param.Add("@CajasCapturaDetalle", dtCajasCapturaDetalles);
        }


        param.Add("@idUsuario", idUsuario);
        param.Add("@checksum", checksum);
        try
        {
            DataSet sd = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncAll", param);
            foreach (DataTable table in sd.Tables)
            {
                result += intToString(++cont) + ":" + GetDataTableToJson(table);
            }

        }
        catch (Exception esx)
        {
            log.Error(esx);
            log.Error(idUsuario);
            log.Error(checksum);
        }

        result += "}";
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncAllV2(string actividades, string periodos, string asociados, string noProgramadas, string cosecha, string formaA, string capturaFormaA, string merma, string Traslado, string monitoreo, int idUsuario, int checksum, string checkList, string checkCriterio, string CajasCaptura, string CajasCapturaDetalle, string SeccionPreharvest)
    {
        string result = "{";
        int estatusCheckList = 0;
        int estatusCheckCriterio = 0;

        JavaScriptSerializer js = new JavaScriptSerializer();
        int cont = 0;

        ActividadProgramaAndroid[] Actividades;
        PeriodoAndroid[] Periodos;
        ActividadAsociadoAndroid[] Asociados;
        ActividadNoProgramadaAndroid[] NoProgramadas;
        CosechaAndroid[] Cosechas;
        FormaAv3[] FormasA;
        CapturaFormaAv2[] CapturasFormaA;
        Merma[] mermas;
        TrasladoMerma[] Traslados;
        Monitoreo[] Monitoreos;
        CheckList[] CheckLists;
        CheckCriterio[] checkCriterios;
        BrixCaptura[] BrixCapturas;
        BrixDetalle[] BrixDetalles;
        CajasCaptura[] CajasCapturas;
        CajasCapturaDetalle[] CajasCapturaDetalles;
        SeccionPreharvestV1[] SeccionesPreharvest;

        //Parámetros para stored procedure
        Dictionary<string, object> param = new Dictionary<string, object>();

        //tablas para stored procedure
        DataTable dtActividadesProgramadas = dtActividadProgramada();
        DataTable dtPeriodos = dtActividadPeriodos();
        DataTable dtJornales = dtActividadJornales();
        DataTable dtNoProgramadas = dtActividadNoProgramada();

        DataTable dtCosechas = dtCosecha();
        DataTable dtFormasA = dtFormaAv3();
        DataTable dtCapturasFormaA = null;
        DataTable dtMermas = dtMerma();
        DataTable dtTrasladoMermas = dtTrasladoMerma();

        DataTable dtSeccionesPreharvest=null;

        DataTable dtMonitoreos = null;
        DataTable dtChecklists = null;
        DataTable dtCheckcriterios = null;

        //DataTable dtBrixCapturas = dtBrixCaptura();
        //DataTable dtBrixDetalles = dtBrixDetalle();
        //DataTable dtBrixHeader = getDtBrixHeader();
        //DataTable dtBrixFirmeza = getDtBrixFirmeza();
        //DataTable dtBrixColor = getDtBrixColor();
        //DataTable dtBrixDefecto = getDtBrixDefecto();

        DataTable dtCajasCapturas = dtCajasCaptura();
        DataTable dtCajasCapturaDetalles = dtCajasCapturaDetalle();

        try
        {
            Actividades = js.Deserialize<ActividadProgramaAndroid[]>(actividades);
            Periodos = js.Deserialize<PeriodoAndroid[]>(periodos);
            Asociados = js.Deserialize<ActividadAsociadoAndroid[]>(asociados);
            NoProgramadas = js.Deserialize<ActividadNoProgramadaAndroid[]>(noProgramadas);

            //Id**Tab es el id***Local del objeto, ya que en la tablet está como local, pero en Servidor se guarda como Tab, 

            if (Actividades != null)
                foreach (ActividadProgramaAndroid A in Actividades)
                {
                    DataRow dt = dtActividadesProgramadas.NewRow();

                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idInvernadero"] = A.idInvernadero;
                    dt["idEtapa"] = A.idEtapa;
                    dt["idCiclo"] = A.idCiclo;
                    dt["cantidadDeElementos"] = A.cantidadDeElementos;
                    dt["semana"] = A.semana;
                    dt["jornalesEstimados"] = A.jornalesEstimados;
                    dt["minutosEstimados"] = A.minutosEstimados;
                    dt["esDirectriz"] = A.esDirectriz;
                    dt["esInterplanting"] = A.esInterplanting;
                    dt["borrado"] = A.borrado;
                    dt["aprobadaPor"] = A.aprobadaPor;
                    dt["rechazadaPor"] = A.rechazadaPor;
                    dt["usuarioModifica"] = A.usuarioModifica;
                    dt["surcoInicio"] = A.surcoInicio;
                    dt["surcoFin"] = A.surcoFin;
                    dt["esColmena"] = A.esColmena;
                    dt["estatus"] = A.estatus;
                    dt["UUID"] = A.UUID;
                    dtActividadesProgramadas.Rows.Add(dt);
                }

            if (Asociados != null)
                foreach (ActividadAsociadoAndroid A in Asociados)
                {
                    DataRow dt = dtJornales.NewRow();

                    dt["idActividadAsociado"] = A.idAsociadoActividad;
                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idPeriodoTab"] = A.idActividadPeriodoLocal;
                    dt["idPeriodo"] = A.idActividadPeriodo;
                    dt["idAsociado"] = A.idAsociado;
                    dt["ausente"] = A.ausente;
                    dt["estatus"] = A.estatus;

                    dtJornales.Rows.Add(dt);
                }

            if (Periodos != null)
                foreach (PeriodoAndroid p in Periodos)
                {
                    DataRow dt = dtPeriodos.NewRow();
                    dt["idPeriodo"] = p.idActividadPeriodo;
                    dt["idPeriodoTab"] = p.idActividadPeriodoLocal;
                    dt["idActividad"] = p.idActividadPrograma;
                    dt["idActividadTab"] = p.idActividadProgramaLocal;
                    dt["surcos"] = p.surcos;
                    dt["inicio"] = p.inicio.ToString("yyyy-MM-dd HH:mm");
                    dt["fin"] = p.fin.ToString("yyyy-MM-dd HH:mm");
                    dt["estatus"] = p.estatus;
                    dt["UUID"] = p.UUID;
                    dtPeriodos.Rows.Add(dt);
                }

            if (NoProgramadas != null)
                foreach (ActividadNoProgramadaAndroid act in NoProgramadas)
                {
                    DataRow dt = dtNoProgramadas.NewRow();

                    dt["idActividadNoProgramada"] = act.idActividadNoProgramada;
                    dt["idActividadNoProgramadaTab"] = act.idActividadNoProgramadaLocal;
                    dt["idInvernadero"] = act.idInvernadero;
                    dt["idEtapa"] = act.idEtapa;
                    dt["idCiclo"] = act.idCiclo;
                    dt["razon"] = act.razon;
                    dt["comentarios"] = act.comentario;
                    dt["cantidadDeElementos"] = act.cantidadDeElementos;
                    dt["semanaProgramacion"] = act.semanaProgramacion;
                    dt["anioProgramacion"] = act.anioProgramacion;
                    dt["esInterplanting"] = act.esInterplanting;
                    dt["estatus"] = act.estatus;
                    dt["UUID"] = act.UUID;
                    dtNoProgramadas.Rows.Add(dt);
                }
        }
        catch (Exception ex)
        {

            dtActividadesProgramadas.Clear();
            dtPeriodos.Clear();
            dtJornales.Clear();
            dtNoProgramadas.Clear();
            log.Error(ex);
        }
        finally
        {
            param.Add("@Actividades", dtActividadesProgramadas);
            param.Add("@Periodos", dtPeriodos);
            param.Add("@Asociados", dtJornales);
            param.Add("@NoProgramadas", dtNoProgramadas);
        }

        try
        {
            Cosechas = js.Deserialize<CosechaAndroid[]>(cosecha);
            FormasA = js.Deserialize<FormaAv3[]>(formaA);
            CapturasFormaA = js.Deserialize<CapturaFormaAv2[]>(capturaFormaA);
            mermas = js.Deserialize<Merma[]>(merma);
            Traslados = js.Deserialize<TrasladoMerma[]>(Traslado);

            dtCapturasFormaA = new CapturaFormaAv2().toDataTable();

            foreach (CosechaAndroid ca in Cosechas)
            {
                DataRow dr = dtCosechas.NewRow();

                dr["idCosecha"] = ca.idCosecha;
                dr["idCosechaTab"] = ca.idCosechaLocal;
                dr["idActividadPrograma"] = ca.idActividadPrograma;
                dr["idActividadProgramaTab"] = ca.idActividadProgramaLocal;
                dr["fechaInicio"] = ca.fechaInicio;
                dr["fechaFin"] = ca.fechaFin;
                dr["cantidadProduccion"] = ca.cantidadProduccion;
                dr["estimadoMedioDia"] = ca.estimadoMedioDia;
                dr["cerrada"] = ca.cerrada;
                dr["estatus"] = ca.estatus;

                dtCosechas.Rows.Add(dr);

            }

            foreach (FormaAv3 fa in FormasA)
            {
                DataRow dr = dtFormasA.NewRow();

                dr["idFormaA"] = fa.idFormaA;
                dr["idFormaATab"] = fa.idFormaALocal;
                dr["idPrograma"] = fa.idPrograma;
                dr["idProgramaTab"] = fa.idProgramaLocal;
                dr["fechaFin"] = fa.fechaFin;
                dr["fechaInicio"] = fa.fechaInicio;
                dr["prefijo"] = fa.prefijo;
                dr["dmcCalidad"] = fa.dmcCalidad;
                dr["dmcMercado"] = fa.dmcMercado;
                dr["comentarios"] = fa.comentarios;
                dr["folio"] = fa.folio;
                dr["cerrada"] = fa.cerrada;
                dr["estatus"] = fa.estatus;
                dr["fechaFinTractorista"] = fa.fechaFinTractorista;
                dr["fechaInicioTractorista"] = fa.fechaInicioTractorista;
                dr["storage"] = fa.storage;
                dr["UUID"] = fa.UUID;
                dr["Preharvest"] = fa.Preharvest;

                dtFormasA.Rows.Add(dr);
            }

            foreach (CapturaFormaAv2 fa in CapturasFormaA)
            {
                dtCapturasFormaA.Rows.Add(fa.toDataRow(dtCapturasFormaA));
            }

            foreach (Merma merm in mermas)
            {
                DataRow dr = dtMermas.NewRow();

                dr["idMerma"] = merm.idMerma;
                dr["idMermaTab"] = merm.idMermaLocal;
                dr["idCoseha"] = merm.idCosecha;
                dr["idCosechaTab"] = merm.idCosechaLocal;
                dr["idRazon"] = merm.idRazon;
                dr["cantidad"] = merm.cantidad;
                dr["observacion"] = merm.observacion;
                dr["estatus"] = merm.estatus;
                dr["UUID"] = merm.UUID;
                dtMermas.Rows.Add(dr);
            }

            foreach (TrasladoMerma t in Traslados)
            {
                DataRow dr = dtTrasladoMermas.NewRow();

                dr["idTrasladoMerma"] = t.idTrasladoMerma;
                dr["idTrasladoMermaLocal"] = t.idTrasladoMermaLocal;
                dr["idFormaA"] = t.idFormaA;
                dr["idFormaALocal"] = t.idFormaALocal;
                dr["idRazon"] = t.idRazon;
                dr["Cajas"] = t.Cajas;
                dr["Comentarios"] = t.Comentarios;
                dr["Estatus"] = t.Estatus;
                dr["UUID"] = t.UUID;
                dtTrasladoMermas.Rows.Add(dr);
            }

        }
        catch (Exception es)
        {
            log.Error(es);

            dtCapturasFormaA = null;
        }
        finally
        {
            param.Add("@cosecha", dtCosechas);
            param.Add("@formaA", dtFormasA);
            param.Add("@capturaFormaA", dtCapturasFormaA);
            param.Add("@merma", dtMermas);
            param.Add("@Traslado", dtTrasladoMermas);
        }

        try
        {

            Monitoreos = js.Deserialize<Monitoreo[]>(monitoreo);
            CheckLists = js.Deserialize<CheckList[]>(checkList);
            checkCriterios = js.Deserialize<CheckCriterio[]>(checkCriterio);


            dtMonitoreos = new Monitoreo().toDataTable();
            dtChecklists = new CheckList().toDataTable();
            dtCheckcriterios = new CheckCriterio().toDataTable();

            foreach (Monitoreo item in Monitoreos)
            {
                dtMonitoreos.Rows.Add(item.toDataRow(dtMonitoreos));
            }

            foreach (CheckList item in CheckLists)
            {
                dtChecklists.Rows.Add(item.toDataRow(dtChecklists));
            }

            foreach (CheckCriterio item in checkCriterios)
            {
                dtCheckcriterios.Rows.Add(item.toDataRow(dtCheckcriterios));
            }
        }
        catch (Exception exsd)
        {
            log.Error(exsd);
            throw;
        }
        finally
        {
            param.Add("@Monitoreo", dtMonitoreos);
            param.Add("@CheckList", dtChecklists);
            param.Add("@CheckCriterio", dtCheckcriterios);
        }


        try
        {
            CajasCapturas = js.Deserialize<CajasCaptura[]>(CajasCaptura);
            CajasCapturaDetalles = js.Deserialize<CajasCapturaDetalle[]>(CajasCapturaDetalle);

            foreach (var captura in CajasCapturas)
            {
                DataRow dr = dtCajasCapturas.NewRow();
                dr["idEstimadocajas"] = captura.idEstimadocajas;
                dr["idEstimadocajasLocal"] = captura.idEstimadocajasLocal;
                dr["idInvernadero"] = captura.idInvernadero;
                dr["idLider"] = captura.idLider;
                dr["idCosecha"] = captura.idCosecha;
                dr["idCosechaLocal"] = captura.idCosechaLocal;
                dr["surcos"] = captura.surcos;
                dr["semana"] = captura.semana;
                dr["borrado"] = captura.borrado;
                dr["usuarioCaptura"] = captura.usuarioCaptura;
                dr["usuarioModifica"] = captura.usuarioModifica;
                dr["fechaCaptura"] = captura.fechaCaptura;
                dr["fechaModifica"] = captura.fechaModifica;
                dr["estatus"] = captura.estatus;
                dr["UUID"] = captura.UUID;
                dtCajasCapturas.Rows.Add(dr);
            }

            foreach (var detalle in CajasCapturaDetalles)
            {
                DataRow dr = dtCajasCapturaDetalles.NewRow();

                dr["idEstimadoCajasCaptura"] = detalle.idEstimadoCajasCaptura;
                dr["idEstimadoCajasCapturaLocal"] = detalle.idEstimadoCajasCapturaLocal;
                dr["idEstimadoCajas"] = detalle.idEstimadoCajas;
                dr["idEstimadoCajasLocal"] = detalle.idEstimadoCajasLocal;
                dr["surco"] = detalle.surco;
                dr["cajas"] = detalle.cajas;
                dr["estimado"] = detalle.estimado;
                dr["fechaCaptura"] = detalle.fechaCaptura;
                dr["fechaModifica"] = detalle.fechaModifica;
                dr["borrado"] = detalle.borrado;
                dr["estatus"] = detalle.estatus;
                dr["idAsociado"] = detalle.idAsociado;
                dr["asignado"] = detalle.asignado;
                dr["UUID"] = detalle.UUID;
                dtCajasCapturaDetalles.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@CajasCaptura", dtCajasCapturas);
            param.Add("@CajasCapturaDetalle", dtCajasCapturaDetalles);
        }

        dtSeccionesPreharvest = new SeccionPreharvestV1().toDataTable();
        try
        {
            SeccionesPreharvest = js.Deserialize<SeccionPreharvestV1[]>(SeccionPreharvest);
            foreach (SeccionPreharvestV1 item in SeccionesPreharvest)
            {
                dtSeccionesPreharvest.Rows.Add(item.toDataRow(dtSeccionesPreharvest));
            }
        }
        catch (Exception ex)
        {

            log.Error(ex);
            log.Error(idUsuario + "-" + SeccionPreharvest);
            dtSeccionesPreharvest = null;
            throw;
        }
        finally
        {
            param.Add("@SeccionesPreharvest", dtSeccionesPreharvest);
        }


        param.Add("@idUsuario", idUsuario);
        param.Add("@checksum", checksum);
        try
        {
            DataSet sd = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncAllV2", param);
            foreach (DataTable table in sd.Tables)
            {
                result += intToStringV2(++cont) + ":" + GetDataTableToJson(table);
            }

        }
        catch (Exception esx)
        {
            log.Error(esx);
            log.Error(idUsuario);
            log.Error(checksum);
        }

        result += "}";
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String getAutosync(String idUsuario, String DeviceID)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", idUsuario);
            param.Add("@DeviceID", DeviceID);
            String data = dataaccess.executeStoreProcedureString("sprAndroid_getAutoSync", param);
            return data;
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncAllV3(string actividades, string periodos, string asociados, string noProgramadas, string cosecha, string formaA, string capturaFormaA, string merma, string Traslado, string monitoreo, int idUsuario, int checksum, string checkList, string checkCriterio, string BrixCaptura, string BrixDetalle, string BrixHeader, string BrixFirmeza, string BrixColor, string BrixDefecto, string CajasCaptura, string CajasCapturaDetalle, string SeccionPreharvest)
    {
        string result = "{";
        int estatusCheckList = 0;
        int estatusCheckCriterio = 0;

        JavaScriptSerializer js = new JavaScriptSerializer();
        int cont = 0;

        ActividadProgramaAndroid[] Actividades;
        PeriodoAndroid[] Periodos;
        ActividadAsociadoAndroid[] Asociados;
        ActividadNoProgramadaAndroid[] NoProgramadas;
        CosechaAndroid[] Cosechas;
        FormaAv3[] FormasA;
        CapturaFormaAv2[] CapturasFormaA;
        Merma[] mermas;
        TrasladoMerma[] Traslados;
        Monitoreo[] Monitoreos;
        CheckList[] CheckLists;
        CheckCriterio[] checkCriterios;
        BrixCaptura[] BrixCapturas;
        BrixDetalle[] BrixDetalles;
        CajasCaptura[] CajasCapturas;
        CajasCapturaDetalle[] CajasCapturaDetalles;
        SeccionPreharvestV2[] SeccionesPreharvest;

        //Parámetros para stored procedure
        Dictionary<string, object> param = new Dictionary<string, object>();

        //tablas para stored procedure
        DataTable dtActividadesProgramadas = dtActividadProgramada();
        DataTable dtPeriodos = dtActividadPeriodos();
        DataTable dtJornales = dtActividadJornales();
        DataTable dtNoProgramadas = dtActividadNoProgramada();

        DataTable dtCosechas = dtCosecha();
        DataTable dtFormasA = dtFormaAv3();
        DataTable dtCapturasFormaA = null;
        DataTable dtMermas = dtMerma();
        DataTable dtTrasladoMermas = dtTrasladoMerma();

        DataTable dtSeccionesPreharvest = null;

        DataTable dtMonitoreos = null;
        DataTable dtChecklists = null;
        DataTable dtCheckcriterios = null;

        //DataTable dtBrixCapturas = dtBrixCaptura();
        //DataTable dtBrixDetalles = dtBrixDetalle();
        //DataTable dtBrixHeader = getDtBrixHeader();
        //DataTable dtBrixFirmeza = getDtBrixFirmeza();
        //DataTable dtBrixColor = getDtBrixColor();
        //DataTable dtBrixDefecto = getDtBrixDefecto();

        DataTable dtCajasCapturas = dtCajasCaptura();
        DataTable dtCajasCapturaDetalles = dtCajasCapturaDetalle();

        try
        {
            Actividades = js.Deserialize<ActividadProgramaAndroid[]>(actividades);
            Periodos = js.Deserialize<PeriodoAndroid[]>(periodos);
            Asociados = js.Deserialize<ActividadAsociadoAndroid[]>(asociados);
            NoProgramadas = js.Deserialize<ActividadNoProgramadaAndroid[]>(noProgramadas);

            //Id**Tab es el id***Local del objeto, ya que en la tablet está como local, pero en Servidor se guarda como Tab, 

            if (Actividades != null)
                foreach (ActividadProgramaAndroid A in Actividades)
                {
                    DataRow dt = dtActividadesProgramadas.NewRow();

                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idInvernadero"] = A.idInvernadero;
                    dt["idEtapa"] = A.idEtapa;
                    dt["idCiclo"] = A.idCiclo;
                    dt["cantidadDeElementos"] = A.cantidadDeElementos;
                    dt["semana"] = A.semana;
                    dt["jornalesEstimados"] = A.jornalesEstimados;
                    dt["minutosEstimados"] = A.minutosEstimados;
                    dt["esDirectriz"] = A.esDirectriz;
                    dt["esInterplanting"] = A.esInterplanting;
                    dt["borrado"] = A.borrado;
                    dt["aprobadaPor"] = A.aprobadaPor;
                    dt["rechazadaPor"] = A.rechazadaPor;
                    dt["usuarioModifica"] = A.usuarioModifica;
                    dt["surcoInicio"] = A.surcoInicio;
                    dt["surcoFin"] = A.surcoFin;
                    dt["esColmena"] = A.esColmena;
                    dt["estatus"] = A.estatus;
                    dt["UUID"] = A.UUID;
                    dtActividadesProgramadas.Rows.Add(dt);
                }

            if (Asociados != null)
                foreach (ActividadAsociadoAndroid A in Asociados)
                {
                    DataRow dt = dtJornales.NewRow();

                    dt["idActividadAsociado"] = A.idAsociadoActividad;
                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idPeriodoTab"] = A.idActividadPeriodoLocal;
                    dt["idPeriodo"] = A.idActividadPeriodo;
                    dt["idAsociado"] = A.idAsociado;
                    dt["ausente"] = A.ausente;
                    dt["estatus"] = A.estatus;

                    dtJornales.Rows.Add(dt);
                }

            if (Periodos != null)
                foreach (PeriodoAndroid p in Periodos)
                {
                    DataRow dt = dtPeriodos.NewRow();
                    dt["idPeriodo"] = p.idActividadPeriodo;
                    dt["idPeriodoTab"] = p.idActividadPeriodoLocal;
                    dt["idActividad"] = p.idActividadPrograma;
                    dt["idActividadTab"] = p.idActividadProgramaLocal;
                    dt["surcos"] = p.surcos;
                    dt["inicio"] = p.inicio.ToString("yyyy-MM-dd HH:mm");
                    dt["fin"] = p.fin.ToString("yyyy-MM-dd HH:mm");
                    dt["estatus"] = p.estatus;
                    dt["UUID"] = p.UUID;
                    dtPeriodos.Rows.Add(dt);
                }

            if (NoProgramadas != null)
                foreach (ActividadNoProgramadaAndroid act in NoProgramadas)
                {
                    DataRow dt = dtNoProgramadas.NewRow();

                    dt["idActividadNoProgramada"] = act.idActividadNoProgramada;
                    dt["idActividadNoProgramadaTab"] = act.idActividadNoProgramadaLocal;
                    dt["idInvernadero"] = act.idInvernadero;
                    dt["idEtapa"] = act.idEtapa;
                    dt["idCiclo"] = act.idCiclo;
                    dt["razon"] = act.razon;
                    dt["comentarios"] = act.comentario;
                    dt["cantidadDeElementos"] = act.cantidadDeElementos;
                    dt["semanaProgramacion"] = act.semanaProgramacion;
                    dt["anioProgramacion"] = act.anioProgramacion;
                    dt["esInterplanting"] = act.esInterplanting;
                    dt["estatus"] = act.estatus;
                    dt["UUID"] = act.UUID;
                    dtNoProgramadas.Rows.Add(dt);
                }
        }
        catch (Exception ex)
        {

            dtActividadesProgramadas.Clear();
            dtPeriodos.Clear();
            dtJornales.Clear();
            dtNoProgramadas.Clear();
            log.Error(ex);
        }
        finally
        {
            param.Add("@Actividades", dtActividadesProgramadas);
            param.Add("@Periodos", dtPeriodos);
            param.Add("@Asociados", dtJornales);
            param.Add("@NoProgramadas", dtNoProgramadas);
        }

        try
        {
            Cosechas = js.Deserialize<CosechaAndroid[]>(cosecha);
            FormasA = js.Deserialize<FormaAv3[]>(formaA);
            CapturasFormaA = js.Deserialize<CapturaFormaAv2[]>(capturaFormaA);
            mermas = js.Deserialize<Merma[]>(merma);
            Traslados = js.Deserialize<TrasladoMerma[]>(Traslado);

            dtCapturasFormaA = new CapturaFormaAv2().toDataTable();

            foreach (CosechaAndroid ca in Cosechas)
            {
                DataRow dr = dtCosechas.NewRow();

                dr["idCosecha"] = ca.idCosecha;
                dr["idCosechaTab"] = ca.idCosechaLocal;
                dr["idActividadPrograma"] = ca.idActividadPrograma;
                dr["idActividadProgramaTab"] = ca.idActividadProgramaLocal;
                dr["fechaInicio"] = ca.fechaInicio;
                dr["fechaFin"] = ca.fechaFin;
                dr["cantidadProduccion"] = ca.cantidadProduccion;
                dr["estimadoMedioDia"] = ca.estimadoMedioDia;
                dr["cerrada"] = ca.cerrada;
                dr["estatus"] = ca.estatus;

                dtCosechas.Rows.Add(dr);

            }

            foreach (FormaAv3 fa in FormasA)
            {
                DataRow dr = dtFormasA.NewRow();

                dr["idFormaA"] = fa.idFormaA;
                dr["idFormaATab"] = fa.idFormaALocal;
                dr["idPrograma"] = fa.idPrograma;
                dr["idProgramaTab"] = fa.idProgramaLocal;
                dr["fechaFin"] = fa.fechaFin;
                dr["fechaInicio"] = fa.fechaInicio;
                dr["prefijo"] = fa.prefijo;
                dr["dmcCalidad"] = fa.dmcCalidad;
                dr["dmcMercado"] = fa.dmcMercado;
                dr["comentarios"] = fa.comentarios;
                dr["folio"] = fa.folio;
                dr["cerrada"] = fa.cerrada;
                dr["estatus"] = fa.estatus;
                dr["fechaFinTractorista"] = fa.fechaFinTractorista;
                dr["fechaInicioTractorista"] = fa.fechaInicioTractorista;
                dr["storage"] = fa.storage;
                dr["UUID"] = fa.UUID;
                dr["Preharvest"] = fa.Preharvest;

                dtFormasA.Rows.Add(dr);
            }

            foreach (CapturaFormaAv2 fa in CapturasFormaA)
            {
                dtCapturasFormaA.Rows.Add(fa.toDataRow(dtCapturasFormaA));
            }

            foreach (Merma merm in mermas)
            {
                DataRow dr = dtMermas.NewRow();

                dr["idMerma"] = merm.idMerma;
                dr["idMermaTab"] = merm.idMermaLocal;
                dr["idCoseha"] = merm.idCosecha;
                dr["idCosechaTab"] = merm.idCosechaLocal;
                dr["idRazon"] = merm.idRazon;
                dr["cantidad"] = merm.cantidad;
                dr["observacion"] = merm.observacion;
                dr["estatus"] = merm.estatus;
                dr["UUID"] = merm.UUID;
                dtMermas.Rows.Add(dr);
            }

            foreach (TrasladoMerma t in Traslados)
            {
                DataRow dr = dtTrasladoMermas.NewRow();

                dr["idTrasladoMerma"] = t.idTrasladoMerma;
                dr["idTrasladoMermaLocal"] = t.idTrasladoMermaLocal;
                dr["idFormaA"] = t.idFormaA;
                dr["idFormaALocal"] = t.idFormaALocal;
                dr["idRazon"] = t.idRazon;
                dr["Cajas"] = t.Cajas;
                dr["Comentarios"] = t.Comentarios;
                dr["Estatus"] = t.Estatus;
                dr["UUID"] = t.UUID;
                dtTrasladoMermas.Rows.Add(dr);
            }

        }
        catch (Exception es)
        {
            log.Error(es);

            dtCapturasFormaA = null;
        }
        finally
        {
            param.Add("@cosecha", dtCosechas);
            param.Add("@formaA", dtFormasA);
            param.Add("@capturaFormaA", dtCapturasFormaA);
            param.Add("@merma", dtMermas);
            param.Add("@Traslado", dtTrasladoMermas);
        }

        try
        {

            Monitoreos = js.Deserialize<Monitoreo[]>(monitoreo);
            CheckLists = js.Deserialize<CheckList[]>(checkList);
            checkCriterios = js.Deserialize<CheckCriterio[]>(checkCriterio);


            dtMonitoreos = new Monitoreo().toDataTable();
            dtChecklists = new CheckList().toDataTable();
            dtCheckcriterios = new CheckCriterio().toDataTable();

            foreach (Monitoreo item in Monitoreos)
            {
                dtMonitoreos.Rows.Add(item.toDataRow(dtMonitoreos));
            }

            foreach (CheckList item in CheckLists)
            {
                dtChecklists.Rows.Add(item.toDataRow(dtChecklists));
            }

            foreach (CheckCriterio item in checkCriterios)
            {
                dtCheckcriterios.Rows.Add(item.toDataRow(dtCheckcriterios));
            }
        }
        catch (Exception exsd)
        {
            log.Error(exsd);
            throw;
        }
        finally
        {
            param.Add("@Monitoreo", dtMonitoreos);
            param.Add("@CheckList", dtChecklists);
            param.Add("@CheckCriterio", dtCheckcriterios);
        }


        try
        {
            CajasCapturas = js.Deserialize<CajasCaptura[]>(CajasCaptura);
            CajasCapturaDetalles = js.Deserialize<CajasCapturaDetalle[]>(CajasCapturaDetalle);

            foreach (var captura in CajasCapturas)
            {
                DataRow dr = dtCajasCapturas.NewRow();
                dr["idEstimadocajas"] = captura.idEstimadocajas;
                dr["idEstimadocajasLocal"] = captura.idEstimadocajasLocal;
                dr["idInvernadero"] = captura.idInvernadero;
                dr["idLider"] = captura.idLider;
                dr["idCosecha"] = captura.idCosecha;
                dr["idCosechaLocal"] = captura.idCosechaLocal;
                dr["surcos"] = captura.surcos;
                dr["semana"] = captura.semana;
                dr["borrado"] = captura.borrado;
                dr["usuarioCaptura"] = captura.usuarioCaptura;
                dr["usuarioModifica"] = captura.usuarioModifica;
                dr["fechaCaptura"] = captura.fechaCaptura;
                dr["fechaModifica"] = captura.fechaModifica;
                dr["estatus"] = captura.estatus;
                dr["UUID"] = captura.UUID;
                dtCajasCapturas.Rows.Add(dr);
            }

            foreach (var detalle in CajasCapturaDetalles)
            {
                DataRow dr = dtCajasCapturaDetalles.NewRow();

                dr["idEstimadoCajasCaptura"] = detalle.idEstimadoCajasCaptura;
                dr["idEstimadoCajasCapturaLocal"] = detalle.idEstimadoCajasCapturaLocal;
                dr["idEstimadoCajas"] = detalle.idEstimadoCajas;
                dr["idEstimadoCajasLocal"] = detalle.idEstimadoCajasLocal;
                dr["surco"] = detalle.surco;
                dr["cajas"] = detalle.cajas;
                dr["estimado"] = detalle.estimado;
                dr["fechaCaptura"] = detalle.fechaCaptura;
                dr["fechaModifica"] = detalle.fechaModifica;
                dr["borrado"] = detalle.borrado;
                dr["estatus"] = detalle.estatus;
                dr["idAsociado"] = detalle.idAsociado;
                dr["asignado"] = detalle.asignado;
                dr["UUID"] = detalle.UUID;
                dtCajasCapturaDetalles.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@CajasCaptura", dtCajasCapturas);
            param.Add("@CajasCapturaDetalle", dtCajasCapturaDetalles);
        }

        dtSeccionesPreharvest = new SeccionPreharvestV2().toDataTable();
        try
        {
            SeccionesPreharvest = js.Deserialize<SeccionPreharvestV2[]>(SeccionPreharvest);
            foreach (SeccionPreharvestV2 item in SeccionesPreharvest)
            {
                dtSeccionesPreharvest.Rows.Add(item.toDataRow(dtSeccionesPreharvest));
            }
        }
        catch (Exception ex)
        {

            log.Error(ex);
            log.Error(idUsuario + "-" + SeccionPreharvest);
            dtSeccionesPreharvest = null;
            throw;
        }
        finally
        {
            param.Add("@SeccionesPreharvest", dtSeccionesPreharvest);
        }


        param.Add("@idUsuario", idUsuario);
        param.Add("@checksum", checksum);
        try
        {
            DataSet sd = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncAllV3", param);
            foreach (DataTable table in sd.Tables)
            {
                result += intToStringV2(++cont) + ":" + GetDataTableToJson(table);
            }

        }
        catch (Exception esx)
        {
            log.Error(esx);
            log.Error(idUsuario);
            log.Error(checksum);
        }

        result += "}";
        return result;
    }

    



    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncAllV4(string actividades, string periodos, string asociados, string noProgramadas, string cosecha, string formaA, string capturaFormaA, string merma, string Traslado, string monitoreo, int idUsuario, int checksum, string checkList, string checkCriterio, string BrixCaptura, string BrixDetalle, string BrixHeader, string BrixFirmeza, string BrixColor, string BrixDefecto, string CajasCaptura, string CajasCapturaDetalle, string SeccionPreharvest)
    {
        string result = "{";
        int estatusCheckList = 0;
        int estatusCheckCriterio = 0;

        JavaScriptSerializer js = new JavaScriptSerializer();
        int cont = 0;

        ActividadProgramaAndroid[] Actividades;
        PeriodoAndroid[] Periodos;
        ActividadAsociadoAndroid[] Asociados;
        ActividadNoProgramadaAndroid[] NoProgramadas;
        CosechaAndroid[] Cosechas;
        FormaAv3[] FormasA;
        CapturaFormaAV4[] CapturasFormaA;
        Merma[] mermas;
        TrasladoMerma[] Traslados;
        Monitoreo[] Monitoreos;
        CheckList[] CheckLists;
        CheckCriterio[] checkCriterios;
        BrixCaptura[] BrixCapturas;
        BrixDetalle[] BrixDetalles;
        CajasCaptura[] CajasCapturas;
        CajasCapturaDetalle[] CajasCapturaDetalles;
        SeccionPreharvestV2[] SeccionesPreharvest;

        //Parámetros para stored procedure
        Dictionary<string, object> param = new Dictionary<string, object>();

        //tablas para stored procedure
        DataTable dtActividadesProgramadas = dtActividadProgramada();
        DataTable dtPeriodos = dtActividadPeriodos();
        DataTable dtJornales = dtActividadJornales();
        DataTable dtNoProgramadas = dtActividadNoProgramada();

        DataTable dtCosechas = dtCosecha();
        DataTable dtFormasA = dtFormaAv3();
        DataTable dtCapturasFormaA = null;
        DataTable dtMermas = dtMerma();
        DataTable dtTrasladoMermas = dtTrasladoMerma();

        DataTable dtSeccionesPreharvest = null;

        DataTable dtMonitoreos = null;
        DataTable dtChecklists = null;
        DataTable dtCheckcriterios = null;

        //DataTable dtBrixCapturas = dtBrixCaptura();
        //DataTable dtBrixDetalles = dtBrixDetalle();
        //DataTable dtBrixHeader = getDtBrixHeader();
        //DataTable dtBrixFirmeza = getDtBrixFirmeza();
        //DataTable dtBrixColor = getDtBrixColor();
        //DataTable dtBrixDefecto = getDtBrixDefecto();

        DataTable dtCajasCapturas = dtCajasCaptura();
        DataTable dtCajasCapturaDetalles = dtCajasCapturaDetalle();

        try
        {
            Actividades = js.Deserialize<ActividadProgramaAndroid[]>(actividades);
            Periodos = js.Deserialize<PeriodoAndroid[]>(periodos);
            Asociados = js.Deserialize<ActividadAsociadoAndroid[]>(asociados);
            NoProgramadas = js.Deserialize<ActividadNoProgramadaAndroid[]>(noProgramadas);

            //Id**Tab es el id***Local del objeto, ya que en la tablet está como local, pero en Servidor se guarda como Tab, 

            if (Actividades != null)
                foreach (ActividadProgramaAndroid A in Actividades)
                {
                    DataRow dt = dtActividadesProgramadas.NewRow();

                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idInvernadero"] = A.idInvernadero;
                    dt["idEtapa"] = A.idEtapa;
                    dt["idCiclo"] = A.idCiclo;
                    dt["cantidadDeElementos"] = A.cantidadDeElementos;
                    dt["semana"] = A.semana;
                    dt["jornalesEstimados"] = A.jornalesEstimados;
                    dt["minutosEstimados"] = A.minutosEstimados;
                    dt["esDirectriz"] = A.esDirectriz;
                    dt["esInterplanting"] = A.esInterplanting;
                    dt["borrado"] = A.borrado;
                    dt["aprobadaPor"] = A.aprobadaPor;
                    dt["rechazadaPor"] = A.rechazadaPor;
                    dt["usuarioModifica"] = A.usuarioModifica;
                    dt["surcoInicio"] = A.surcoInicio;
                    dt["surcoFin"] = A.surcoFin;
                    dt["esColmena"] = A.esColmena;
                    dt["estatus"] = A.estatus;
                    dt["UUID"] = A.UUID;
                    dtActividadesProgramadas.Rows.Add(dt);
                }

            if (Asociados != null)
                foreach (ActividadAsociadoAndroid A in Asociados)
                {
                    DataRow dt = dtJornales.NewRow();

                    dt["idActividadAsociado"] = A.idAsociadoActividad;
                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idPeriodoTab"] = A.idActividadPeriodoLocal;
                    dt["idPeriodo"] = A.idActividadPeriodo;
                    dt["idAsociado"] = A.idAsociado;
                    dt["ausente"] = A.ausente;
                    dt["estatus"] = A.estatus;

                    dtJornales.Rows.Add(dt);
                }

            if (Periodos != null)
                foreach (PeriodoAndroid p in Periodos)
                {
                    DataRow dt = dtPeriodos.NewRow();
                    dt["idPeriodo"] = p.idActividadPeriodo;
                    dt["idPeriodoTab"] = p.idActividadPeriodoLocal;
                    dt["idActividad"] = p.idActividadPrograma;
                    dt["idActividadTab"] = p.idActividadProgramaLocal;
                    dt["surcos"] = p.surcos;
                    dt["inicio"] = p.inicio.ToString("yyyy-MM-dd HH:mm");
                    dt["fin"] = p.fin.ToString("yyyy-MM-dd HH:mm");
                    dt["estatus"] = p.estatus;
                    dt["UUID"] = p.UUID;
                    dtPeriodos.Rows.Add(dt);
                }

            if (NoProgramadas != null)
                foreach (ActividadNoProgramadaAndroid act in NoProgramadas)
                {
                    DataRow dt = dtNoProgramadas.NewRow();

                    dt["idActividadNoProgramada"] = act.idActividadNoProgramada;
                    dt["idActividadNoProgramadaTab"] = act.idActividadNoProgramadaLocal;
                    dt["idInvernadero"] = act.idInvernadero;
                    dt["idEtapa"] = act.idEtapa;
                    dt["idCiclo"] = act.idCiclo;
                    dt["razon"] = act.razon;
                    dt["comentarios"] = act.comentario;
                    dt["cantidadDeElementos"] = act.cantidadDeElementos;
                    dt["semanaProgramacion"] = act.semanaProgramacion;
                    dt["anioProgramacion"] = act.anioProgramacion;
                    dt["esInterplanting"] = act.esInterplanting;
                    dt["estatus"] = act.estatus;
                    dt["UUID"] = act.UUID;
                    dtNoProgramadas.Rows.Add(dt);
                }
        }
        catch (Exception ex)
        {

            dtActividadesProgramadas.Clear();
            dtPeriodos.Clear();
            dtJornales.Clear();
            dtNoProgramadas.Clear();
            log.Error(ex);
        }
        finally
        {
            param.Add("@Actividades", dtActividadesProgramadas);
            param.Add("@Periodos", dtPeriodos);
            param.Add("@Asociados", dtJornales);
            param.Add("@NoProgramadas", dtNoProgramadas);
        }

        try
        {
            Cosechas = js.Deserialize<CosechaAndroid[]>(cosecha);
            FormasA = js.Deserialize<FormaAv3[]>(formaA);
            CapturasFormaA = js.Deserialize<CapturaFormaAV4[]>(capturaFormaA);
            mermas = js.Deserialize<Merma[]>(merma);
            Traslados = js.Deserialize<TrasladoMerma[]>(Traslado);

            dtCapturasFormaA = new CapturaFormaAV4().toDataTable();

            foreach (CosechaAndroid ca in Cosechas)
            {
                DataRow dr = dtCosechas.NewRow();

                dr["idCosecha"] = ca.idCosecha;
                dr["idCosechaTab"] = ca.idCosechaLocal;
                dr["idActividadPrograma"] = ca.idActividadPrograma;
                dr["idActividadProgramaTab"] = ca.idActividadProgramaLocal;
                dr["fechaInicio"] = ca.fechaInicio;
                dr["fechaFin"] = ca.fechaFin;
                dr["cantidadProduccion"] = ca.cantidadProduccion;
                dr["estimadoMedioDia"] = ca.estimadoMedioDia;
                dr["cerrada"] = ca.cerrada;
                dr["estatus"] = ca.estatus;

                dtCosechas.Rows.Add(dr);

            }

            foreach (FormaAv3 fa in FormasA)
            {
                DataRow dr = dtFormasA.NewRow();

                dr["idFormaA"] = fa.idFormaA;
                dr["idFormaATab"] = fa.idFormaALocal;
                dr["idPrograma"] = fa.idPrograma;
                dr["idProgramaTab"] = fa.idProgramaLocal;
                dr["fechaFin"] = fa.fechaFin;
                dr["fechaInicio"] = fa.fechaInicio;
                dr["prefijo"] = fa.prefijo;
                dr["dmcCalidad"] = fa.dmcCalidad;
                dr["dmcMercado"] = fa.dmcMercado;
                dr["comentarios"] = fa.comentarios;
                dr["folio"] = fa.folio;
                dr["cerrada"] = fa.cerrada;
                dr["estatus"] = fa.estatus;
                dr["fechaFinTractorista"] = fa.fechaFinTractorista;
                dr["fechaInicioTractorista"] = fa.fechaInicioTractorista;
                dr["storage"] = fa.storage;
                dr["UUID"] = fa.UUID;
                dr["Preharvest"] = fa.Preharvest;

                dtFormasA.Rows.Add(dr);
            }

            foreach (CapturaFormaAV4 fa in CapturasFormaA)
            {
                dtCapturasFormaA.Rows.Add(fa.toDataRow(dtCapturasFormaA));
            }

            foreach (Merma merm in mermas)
            {
                DataRow dr = dtMermas.NewRow();

                dr["idMerma"] = merm.idMerma;
                dr["idMermaTab"] = merm.idMermaLocal;
                dr["idCoseha"] = merm.idCosecha;
                dr["idCosechaTab"] = merm.idCosechaLocal;
                dr["idRazon"] = merm.idRazon;
                dr["cantidad"] = merm.cantidad;
                dr["observacion"] = merm.observacion;
                dr["estatus"] = merm.estatus;
                dr["UUID"] = merm.UUID;
                dtMermas.Rows.Add(dr);
            }

            foreach (TrasladoMerma t in Traslados)
            {
                DataRow dr = dtTrasladoMermas.NewRow();

                dr["idTrasladoMerma"] = t.idTrasladoMerma;
                dr["idTrasladoMermaLocal"] = t.idTrasladoMermaLocal;
                dr["idFormaA"] = t.idFormaA;
                dr["idFormaALocal"] = t.idFormaALocal;
                dr["idRazon"] = t.idRazon;
                dr["Cajas"] = t.Cajas;
                dr["Comentarios"] = t.Comentarios;
                dr["Estatus"] = t.Estatus;
                dr["UUID"] = t.UUID;
                dtTrasladoMermas.Rows.Add(dr);
            }

        }
        catch (Exception es)
        {
            log.Error(es);

            dtCapturasFormaA = null;
        }
        finally
        {
            param.Add("@cosecha", dtCosechas);
            param.Add("@formaA", dtFormasA);
            param.Add("@capturaFormaA", dtCapturasFormaA);
            param.Add("@merma", dtMermas);
            param.Add("@Traslado", dtTrasladoMermas);
        }

        try
        {

            Monitoreos = js.Deserialize<Monitoreo[]>(monitoreo);
            CheckLists = js.Deserialize<CheckList[]>(checkList);
            checkCriterios = js.Deserialize<CheckCriterio[]>(checkCriterio);


            dtMonitoreos = new Monitoreo().toDataTable();
            dtChecklists = new CheckList().toDataTable();
            dtCheckcriterios = new CheckCriterio().toDataTable();

            foreach (Monitoreo item in Monitoreos)
            {
                dtMonitoreos.Rows.Add(item.toDataRow(dtMonitoreos));
            }

            foreach (CheckList item in CheckLists)
            {
                dtChecklists.Rows.Add(item.toDataRow(dtChecklists));
            }

            foreach (CheckCriterio item in checkCriterios)
            {
                dtCheckcriterios.Rows.Add(item.toDataRow(dtCheckcriterios));
            }
        }
        catch (Exception exsd)
        {
            log.Error(exsd);
            throw;
        }
        finally
        {
            param.Add("@Monitoreo", dtMonitoreos);
            param.Add("@CheckList", dtChecklists);
            param.Add("@CheckCriterio", dtCheckcriterios);
        }


        try
        {
            CajasCapturas = js.Deserialize<CajasCaptura[]>(CajasCaptura);
            CajasCapturaDetalles = js.Deserialize<CajasCapturaDetalle[]>(CajasCapturaDetalle);

            foreach (var captura in CajasCapturas)
            {
                DataRow dr = dtCajasCapturas.NewRow();
                dr["idEstimadocajas"] = captura.idEstimadocajas;
                dr["idEstimadocajasLocal"] = captura.idEstimadocajasLocal;
                dr["idInvernadero"] = captura.idInvernadero;
                dr["idLider"] = captura.idLider;
                dr["idCosecha"] = captura.idCosecha;
                dr["idCosechaLocal"] = captura.idCosechaLocal;
                dr["surcos"] = captura.surcos;
                dr["semana"] = captura.semana;
                dr["borrado"] = captura.borrado;
                dr["usuarioCaptura"] = captura.usuarioCaptura;
                dr["usuarioModifica"] = captura.usuarioModifica;
                dr["fechaCaptura"] = captura.fechaCaptura;
                dr["fechaModifica"] = captura.fechaModifica;
                dr["estatus"] = captura.estatus;
                dr["UUID"] = captura.UUID;
                dtCajasCapturas.Rows.Add(dr);
            }

            foreach (var detalle in CajasCapturaDetalles)
            {
                DataRow dr = dtCajasCapturaDetalles.NewRow();

                dr["idEstimadoCajasCaptura"] = detalle.idEstimadoCajasCaptura;
                dr["idEstimadoCajasCapturaLocal"] = detalle.idEstimadoCajasCapturaLocal;
                dr["idEstimadoCajas"] = detalle.idEstimadoCajas;
                dr["idEstimadoCajasLocal"] = detalle.idEstimadoCajasLocal;
                dr["surco"] = detalle.surco;
                dr["cajas"] = detalle.cajas;
                dr["estimado"] = detalle.estimado;
                dr["fechaCaptura"] = detalle.fechaCaptura;
                dr["fechaModifica"] = detalle.fechaModifica;
                dr["borrado"] = detalle.borrado;
                dr["estatus"] = detalle.estatus;
                dr["idAsociado"] = detalle.idAsociado;
                dr["asignado"] = detalle.asignado;
                dr["UUID"] = detalle.UUID;
                dtCajasCapturaDetalles.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@CajasCaptura", dtCajasCapturas);
            param.Add("@CajasCapturaDetalle", dtCajasCapturaDetalles);
        }

        dtSeccionesPreharvest = new SeccionPreharvestV2().toDataTable();
        try
        {
            SeccionesPreharvest = js.Deserialize<SeccionPreharvestV2[]>(SeccionPreharvest);
            foreach (SeccionPreharvestV2 item in SeccionesPreharvest)
            {
                dtSeccionesPreharvest.Rows.Add(item.toDataRow(dtSeccionesPreharvest));
            }
        }
        catch (Exception ex)
        {

            log.Error(ex);
            log.Error(idUsuario + "-" + SeccionPreharvest);
            dtSeccionesPreharvest = null;
            throw;
        }
        finally
        {
            param.Add("@SeccionesPreharvest", dtSeccionesPreharvest);
        }


        param.Add("@idUsuario", idUsuario);
        param.Add("@checksum", checksum);
        try
        {
            DataSet sd = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncAllV4", param);
            foreach (DataTable table in sd.Tables)
            {
                result += intToStringV2(++cont) + ":" + GetDataTableToJson(table);
            }

        }
        catch (Exception esx)
        {
            log.Error(esx);
            log.Error(idUsuario);
            log.Error(checksum);
        }

        result += "}";
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String syncFormaALog(string FormaALog)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        String result = "";
        FormaALog[] LogFolios;
        DataTable dtFormaALog = new FormaALog().toDataTable();

        try
        {
            LogFolios = js.Deserialize<FormaALog[]>(FormaALog);
            foreach (FormaALog item in LogFolios)
            {
                dtFormaALog.Rows.Add(item.toDataRow(dtFormaALog));
            }

            return dataaccess.executeStoreProcedureString("sprAndroid_SyncFormaAlog", new Dictionary<string, object>() { { "@FormaALog", dtFormaALog } });
        }
        catch (Exception s)
        {
            log.Error(s);
            return "false";
        }
    }





    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncAllV5(string actividades, string periodos, string asociados, string noProgramadas, string cosecha, string formaA, string capturaFormaA, string merma, string Traslado, string monitoreo, int idUsuario, int checksum, string checkList, string checkCriterio, string BrixCaptura, string BrixDetalle, string BrixHeader, string BrixFirmeza, string BrixColor, string BrixDefecto, string CajasCaptura, string CajasCapturaDetalle, string SeccionPreharvest, string CapturaTrabajo, string CapturaTrabajoHeader)
    {
        string result = "{";
        int estatusCheckList = 0;
        int estatusCheckCriterio = 0;

        JavaScriptSerializer js = new JavaScriptSerializer();
        int cont = 0;

        ActividadProgramaAndroid[] Actividades;
        PeriodoAndroid[] Periodos;
        ActividadAsociadoAndroid[] Asociados;
        ActividadNoProgramadaAndroid[] NoProgramadas;
        CosechaAndroidV2[] Cosechas;
        FormaAv3[] FormasA;
        CapturaFormaAV4[] CapturasFormaA;
        Merma[] mermas;
        TrasladoMerma[] Traslados;
        Monitoreo[] Monitoreos;
        CheckList[] CheckLists;
        CheckCriterio[] checkCriterios;
        BrixCaptura[] BrixCapturas;
        BrixDetalle[] BrixDetalles;
        CajasCaptura[] CajasCapturas;
        CajasCapturaDetalle[] CajasCapturaDetalles;
        SeccionPreharvestV2[] SeccionesPreharvest;
        CapturaTrabajo[] CapturaTrabajos;
        CapturaTrabajoHeader[] CapturaTrabajosHeader;

        //Parámetros para stored procedure
        Dictionary<string, object> param = new Dictionary<string, object>();

        //tablas para stored procedure
        DataTable dtActividadesProgramadas = dtActividadProgramada();
        DataTable dtPeriodos = dtActividadPeriodos();
        DataTable dtJornales = dtActividadJornales();
        DataTable dtNoProgramadas = dtActividadNoProgramada();

        DataTable dtCosechas = new CosechaAndroidV2().toDataTable();
        DataTable dtFormasA = dtFormaAv3();
        DataTable dtCapturasFormaA = null;
        DataTable dtMermas = dtMerma();
        DataTable dtTrasladoMermas = dtTrasladoMerma();

        DataTable dtSeccionesPreharvest = null;

        DataTable dtMonitoreos = null;
        DataTable dtChecklists = null;
        DataTable dtCheckcriterios = null;

        //DataTable dtBrixCapturas = dtBrixCaptura();
        //DataTable dtBrixDetalles = dtBrixDetalle();
        //DataTable dtBrixHeader = getDtBrixHeader();
        //DataTable dtBrixFirmeza = getDtBrixFirmeza();
        //DataTable dtBrixColor = getDtBrixColor();
        //DataTable dtBrixDefecto = getDtBrixDefecto();

        DataTable dtCajasCapturas = dtCajasCaptura();
        DataTable dtCajasCapturaDetalles = dtCajasCapturaDetalle();
        DataTable dtCapturaTrabajoHeaderHistoria = null;
        DataTable dtCapturaTrabajoHeaders = null;

        try
        {
            Actividades = js.Deserialize<ActividadProgramaAndroid[]>(actividades);
            
            Periodos = js.Deserialize<PeriodoAndroid[]>(periodos);
            Asociados = js.Deserialize<ActividadAsociadoAndroid[]>(asociados);
            NoProgramadas = js.Deserialize<ActividadNoProgramadaAndroid[]>(noProgramadas);

            //Id**Tab es el id***Local del objeto, ya que en la tablet está como local, pero en Servidor se guarda como Tab, 

            if (Actividades != null)
                foreach (ActividadProgramaAndroid A in Actividades)
                {
                    DataRow dt = dtActividadesProgramadas.NewRow();

                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idInvernadero"] = A.idInvernadero;
                    dt["idEtapa"] = A.idEtapa;
                    dt["idCiclo"] = A.idCiclo;
                    dt["cantidadDeElementos"] = A.cantidadDeElementos;
                    dt["semana"] = A.semana;
                    dt["jornalesEstimados"] = A.jornalesEstimados;
                    dt["minutosEstimados"] = A.minutosEstimados;
                    dt["esDirectriz"] = A.esDirectriz;
                    dt["esInterplanting"] = A.esInterplanting;
                    dt["borrado"] = A.borrado;
                    dt["aprobadaPor"] = A.aprobadaPor;
                    dt["rechazadaPor"] = A.rechazadaPor;
                    dt["usuarioModifica"] = A.usuarioModifica;
                    dt["surcoInicio"] = A.surcoInicio;
                    dt["surcoFin"] = A.surcoFin;
                    dt["esColmena"] = A.esColmena;
                    dt["estatus"] = A.estatus;
                    dt["UUID"] = A.UUID;
                    dtActividadesProgramadas.Rows.Add(dt);
                }

            if (Asociados != null)
                foreach (ActividadAsociadoAndroid A in Asociados)
                {
                    DataRow dt = dtJornales.NewRow();

                    dt["idActividadAsociado"] = A.idAsociadoActividad;
                    dt["idActividad"] = A.idActividadPrograma;
                    dt["idActividadTab"] = A.idActividadProgramaLocal;
                    dt["idPeriodoTab"] = A.idActividadPeriodoLocal;
                    dt["idPeriodo"] = A.idActividadPeriodo;
                    dt["idAsociado"] = A.idAsociado;
                    dt["ausente"] = A.ausente;
                    dt["estatus"] = A.estatus;

                    dtJornales.Rows.Add(dt);
                }

            if (Periodos != null)
                foreach (PeriodoAndroid p in Periodos)
                {
                    DataRow dt = dtPeriodos.NewRow();
                    dt["idPeriodo"] = p.idActividadPeriodo;
                    dt["idPeriodoTab"] = p.idActividadPeriodoLocal;
                    dt["idActividad"] = p.idActividadPrograma;
                    dt["idActividadTab"] = p.idActividadProgramaLocal;
                    dt["surcos"] = p.surcos;
                    dt["inicio"] = p.inicio.ToString("yyyy-MM-dd HH:mm");                                            
                    dt["fin"] = p.fin.ToString("yyyy-MM-dd HH:mm");
                    //dt["inicio"] = p.inicio.AddHours(-2).ToString("yyyy-MM-dd HH:mm");
                    //dt["fin"] = p.fin.AddHours(-2).ToString("yyyy-MM-dd HH:mm");
                    dt["estatus"] = p.estatus;
                    dt["UUID"] = p.UUID;
                    dtPeriodos.Rows.Add(dt);
                    
                }

            if (NoProgramadas != null)
                foreach (ActividadNoProgramadaAndroid act in NoProgramadas)
                {
                    DataRow dt = dtNoProgramadas.NewRow();

                    dt["idActividadNoProgramada"] = act.idActividadNoProgramada;
                    dt["idActividadNoProgramadaTab"] = act.idActividadNoProgramadaLocal;
                    dt["idInvernadero"] = act.idInvernadero;
                    dt["idEtapa"] = act.idEtapa;
                    dt["idCiclo"] = act.idCiclo;
                    dt["razon"] = act.razon;
                    dt["comentarios"] = act.comentario;
                    dt["cantidadDeElementos"] = act.cantidadDeElementos;
                    dt["semanaProgramacion"] = act.semanaProgramacion;
                    dt["anioProgramacion"] = act.anioProgramacion;
                    dt["esInterplanting"] = act.esInterplanting;
                    dt["estatus"] = act.estatus;
                    dt["UUID"] = act.UUID;
                    dtNoProgramadas.Rows.Add(dt);
                }
        }
        catch (Exception ex)
        {

            dtActividadesProgramadas.Clear();
            dtPeriodos.Clear();
            dtJornales.Clear();
            dtNoProgramadas.Clear();
            log.Error(ex);
        }
        finally
        {
            param.Add("@Actividades", dtActividadesProgramadas);
            param.Add("@Periodos", dtPeriodos);
            param.Add("@Asociados", dtJornales);
            param.Add("@NoProgramadas", dtNoProgramadas);
        }

        try
        {
            Cosechas = js.Deserialize<CosechaAndroidV2[]>(cosecha);
            FormasA = js.Deserialize<FormaAv3[]>(formaA);
            CapturasFormaA = js.Deserialize<CapturaFormaAV4[]>(capturaFormaA);
            
            mermas = js.Deserialize<Merma[]>(merma);
            Traslados = js.Deserialize<TrasladoMerma[]>(Traslado);

            dtCapturasFormaA = new CapturaFormaAV4().toDataTable();

            foreach (CosechaAndroidV2 ca in Cosechas)
            {
                DataRow dr = dtCosechas.NewRow();

                dr = ca.toDataRow();

                dtCosechas.Rows.Add(dr.ItemArray);

            }

            foreach (FormaAv3 fa in FormasA)
            {
                DataRow dr = dtFormasA.NewRow();

                dr["idFormaA"] = fa.idFormaA;
                dr["idFormaATab"] = fa.idFormaALocal;
                dr["idPrograma"] = fa.idPrograma;
                dr["idProgramaTab"] = fa.idProgramaLocal;
                dr["fechaFin"] = fa.fechaFin;
                dr["fechaInicio"] = fa.fechaInicio;
                dr["prefijo"] = fa.prefijo;
                dr["dmcCalidad"] = fa.dmcCalidad;
                dr["dmcMercado"] = fa.dmcMercado;
                dr["comentarios"] = fa.comentarios;
                dr["folio"] = fa.folio;
                dr["cerrada"] = fa.cerrada;
                dr["estatus"] = fa.estatus;
                dr["fechaFinTractorista"] = fa.fechaFinTractorista;
                dr["fechaInicioTractorista"] = fa.fechaInicioTractorista;
                dr["storage"] = fa.storage;
                dr["UUID"] = fa.UUID;
                dr["Preharvest"] = fa.Preharvest;
                dr["idTipoCaja"] = fa.idTipoCaja;
                dr["idTipoCosecha"] = fa.idTipoCosecha;

                dtFormasA.Rows.Add(dr);
            }

            foreach (CapturaFormaAV4 fa in CapturasFormaA)
            {
                dtCapturasFormaA.Rows.Add(fa.toDataRow(dtCapturasFormaA));
            }

            foreach (Merma merm in mermas)
            {
                DataRow dr = dtMermas.NewRow();

                dr["idMerma"] = merm.idMerma;
                dr["idMermaTab"] = merm.idMermaLocal;
                dr["idCoseha"] = merm.idCosecha;
                dr["idCosechaTab"] = merm.idCosechaLocal;
                dr["idRazon"] = merm.idRazon;
                dr["cantidad"] = merm.cantidad;
                dr["observacion"] = merm.observacion;
                dr["estatus"] = merm.estatus;
                dr["UUID"] = merm.UUID;
                dtMermas.Rows.Add(dr);
            }

            foreach (TrasladoMerma t in Traslados)
            {
                DataRow dr = dtTrasladoMermas.NewRow();

                dr["idTrasladoMerma"] = t.idTrasladoMerma;
                dr["idTrasladoMermaLocal"] = t.idTrasladoMermaLocal;
                dr["idFormaA"] = t.idFormaA;
                dr["idFormaALocal"] = t.idFormaALocal;
                dr["idRazon"] = t.idRazon;
                dr["Cajas"] = t.Cajas;
                dr["Comentarios"] = t.Comentarios;
                dr["Estatus"] = t.Estatus;
                dr["UUID"] = t.UUID;
                dtTrasladoMermas.Rows.Add(dr);
            }

        }
        catch (Exception es)
        {
            log.Error(es);

            dtCapturasFormaA = null;
        }
        finally
        {
            param.Add("@cosecha", dtCosechas);
            param.Add("@formaA", dtFormasA);
            param.Add("@capturaFormaA", dtCapturasFormaA);
            param.Add("@merma", dtMermas);
            param.Add("@Traslado", dtTrasladoMermas);
        }

        try
        {

            Monitoreos = js.Deserialize<Monitoreo[]>(monitoreo);
            CheckLists = js.Deserialize<CheckList[]>(checkList);
            checkCriterios = js.Deserialize<CheckCriterio[]>(checkCriterio);


            dtMonitoreos = new Monitoreo().toDataTable();
            dtChecklists = new CheckList().toDataTable();
            dtCheckcriterios = new CheckCriterio().toDataTable();

            foreach (Monitoreo item in Monitoreos)
            {
                dtMonitoreos.Rows.Add(item.toDataRow(dtMonitoreos));
            }

            foreach (CheckList item in CheckLists)
            {
                dtChecklists.Rows.Add(item.toDataRow(dtChecklists));
            }

            foreach (CheckCriterio item in checkCriterios)
            {
                dtCheckcriterios.Rows.Add(item.toDataRow(dtCheckcriterios));
            }
        }
        catch (Exception exsd)
        {
            log.Error(exsd);
            throw;
        }
        finally
        {
            param.Add("@Monitoreo", dtMonitoreos);
            param.Add("@CheckList", dtChecklists);
            param.Add("@CheckCriterio", dtCheckcriterios);
        }


        try
        {
            CajasCapturas = js.Deserialize<CajasCaptura[]>(CajasCaptura);
            CajasCapturaDetalles = js.Deserialize<CajasCapturaDetalle[]>(CajasCapturaDetalle);

            foreach (var captura in CajasCapturas)
            {
                DataRow dr = dtCajasCapturas.NewRow();
                dr["idEstimadocajas"] = captura.idEstimadocajas;
                dr["idEstimadocajasLocal"] = captura.idEstimadocajasLocal;
                dr["idInvernadero"] = captura.idInvernadero;
                dr["idLider"] = captura.idLider;
                dr["idCosecha"] = captura.idCosecha;
                dr["idCosechaLocal"] = captura.idCosechaLocal;
                dr["surcos"] = captura.surcos;
                dr["semana"] = captura.semana;
                dr["borrado"] = captura.borrado;
                dr["usuarioCaptura"] = captura.usuarioCaptura;
                dr["usuarioModifica"] = captura.usuarioModifica;
                dr["fechaCaptura"] = captura.fechaCaptura;
                dr["fechaModifica"] = captura.fechaModifica;
                dr["estatus"] = captura.estatus;
                dr["UUID"] = captura.UUID;
                dtCajasCapturas.Rows.Add(dr);
            }

            foreach (var detalle in CajasCapturaDetalles)
            {
                DataRow dr = dtCajasCapturaDetalles.NewRow();

                dr["idEstimadoCajasCaptura"] = detalle.idEstimadoCajasCaptura;
                dr["idEstimadoCajasCapturaLocal"] = detalle.idEstimadoCajasCapturaLocal;
                dr["idEstimadoCajas"] = detalle.idEstimadoCajas;
                dr["idEstimadoCajasLocal"] = detalle.idEstimadoCajasLocal;
                dr["surco"] = detalle.surco;
                dr["cajas"] = detalle.cajas;
                dr["estimado"] = detalle.estimado;
                dr["fechaCaptura"] = detalle.fechaCaptura;
                dr["fechaModifica"] = detalle.fechaModifica;
                dr["borrado"] = detalle.borrado;
                dr["estatus"] = detalle.estatus;
                dr["idAsociado"] = detalle.idAsociado;
                dr["asignado"] = detalle.asignado;
                dr["UUID"] = detalle.UUID;
                dtCajasCapturaDetalles.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@CajasCaptura", dtCajasCapturas);
            param.Add("@CajasCapturaDetalle", dtCajasCapturaDetalles);
        }

       

        dtSeccionesPreharvest = new SeccionPreharvestV2().toDataTable();
        try
        {
            SeccionesPreharvest = js.Deserialize<SeccionPreharvestV2[]>(SeccionPreharvest);
            foreach (SeccionPreharvestV2 item in SeccionesPreharvest)
            {
                dtSeccionesPreharvest.Rows.Add(item.toDataRow(dtSeccionesPreharvest));
            }
        }
        catch (Exception ex)
        {

            log.Error(ex);
            log.Error(idUsuario + "-" + SeccionPreharvest);
            dtSeccionesPreharvest = null;
            throw;
        }
        finally
        {
            param.Add("@SeccionesPreharvest", dtSeccionesPreharvest);
        }

        //CapturaTrabajos = js.Deserialize<CapturaTrabajo[]>(CapturaTrabajoHeaderHistoria);
        //List<CapturaTrabajo> obj = js.Deserialize<List<CapturaTrabajo>>(CapturaTrabajoHeaderHistoria);
        //var dict = js.Deserialize<dynamic>(CapturaTrabajoHeaderHistoria);
        //CapturaTrabajos = js.Deserialize<CapturaTrabajo[]>(CapturaTrabajoHeaderHistoria);
        try
        {

            CapturaTrabajos = js.Deserialize<CapturaTrabajo[]>(CapturaTrabajo);
            CapturaTrabajosHeader = js.Deserialize<CapturaTrabajoHeader[]>(CapturaTrabajoHeader);

            //var jsonString = js.Deserialize<string>(CapturaTrabajoHeaderHistoria);
            //return js.Deserialize<CapturaTrabajo[]>(jsonString);

            dtCapturaTrabajoHeaderHistoria = new CapturaTrabajo().toDataTable();
            foreach (CapturaTrabajo fa in CapturaTrabajos)
            {
                dtCapturaTrabajoHeaderHistoria.Rows.Add(fa.toDataRow(dtCapturaTrabajoHeaderHistoria));
            }

            dtCapturaTrabajoHeaders = new CapturaTrabajoHeader().toDataTable();
            foreach (CapturaTrabajoHeader fa in CapturaTrabajosHeader)
            {
                dtCapturaTrabajoHeaders.Rows.Add(fa.toDataRow(dtCapturaTrabajoHeaders));
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@CapturaTrabajoHeaderHistoria", dtCapturaTrabajoHeaderHistoria);
            param.Add("@CapturaTrabajoHeader", dtCapturaTrabajoHeaders);
        }


        param.Add("@idUsuario", idUsuario);
        param.Add("@checksum", checksum);
        try
        {
            DataSet sd = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncAllV5", param);
            foreach (DataTable table in sd.Tables)
            {
                result += intToStringV2(++cont) + ":" + GetDataTableToJson(table);
            }

        }
        catch (Exception esx)
        {
            log.Error(esx);
            log.Error(idUsuario);
            log.Error(checksum);
        }

        result += "}";
        return result;
    }

   

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncEmbarque(String embarqueHeader, String embarqueDestino, String embarqueProducto, String embarqueFormaA, String embarqueFIFO, int Checksum, int idusuario)
    {
        string result = "{";

        EmbarqueHeaderAndroid[] EmbarqueHeaders;
        EmbarqueDestinoAndroidV2[] EmbarqueDestinos;
        EmbarqueProductoAndroid[] EmbarqueProductos;
        EmbarqueFormasAAndroidV2[] EmbarqueFormasA;
        EmbarqueFIFOAndroid[] EmbarqueFIFO;

        JavaScriptSerializer js = new JavaScriptSerializer();

        DataTable dtEmbarqueHeader;
        DataTable dtEmbarqueDestino;
        DataTable dtEmbarqueProducto;
        DataTable dtEmbarqueFormaA;
        DataTable dtEmbarqueFIFO;

        Dictionary<String, object> param;

        DataSet ds;
        int cont = 0;

        try
        {
            EmbarqueHeaders = js.Deserialize<EmbarqueHeaderAndroid[]>(embarqueHeader);
            EmbarqueDestinos = js.Deserialize<EmbarqueDestinoAndroidV2[]>(embarqueDestino);
            EmbarqueProductos = js.Deserialize<EmbarqueProductoAndroid[]>(embarqueProducto);
            EmbarqueFormasA = js.Deserialize<EmbarqueFormasAAndroidV2[]>(embarqueFormaA);
            EmbarqueFIFO = js.Deserialize<EmbarqueFIFOAndroid[]>(embarqueFIFO);

            dtEmbarqueHeader = new EmbarqueHeaderAndroidV2().toDataTable();
            dtEmbarqueDestino = new EmbarqueDestinoAndroidV2().toDataTable();
            dtEmbarqueProducto = new EmbarqueProductoAndroid().toDataTable();
            dtEmbarqueFormaA = new EmbarqueFormasAAndroidV2().toDataTable();
            dtEmbarqueFIFO = new EmbarqueFIFOAndroid().toDataTable();

            foreach (EmbarqueHeaderAndroid eh in EmbarqueHeaders)
            {
                dtEmbarqueHeader.Rows.Add(eh.toDataRow(dtEmbarqueHeader));
            }

            foreach (EmbarqueDestinoAndroidV2 d in EmbarqueDestinos)
            {
                dtEmbarqueDestino.Rows.Add(d.toDataRow(dtEmbarqueDestino));
            }

            foreach (EmbarqueProductoAndroid p in EmbarqueProductos)
            {
                dtEmbarqueProducto.Rows.Add(p.toDataRow(dtEmbarqueProducto));
            }

            foreach (EmbarqueFormasAAndroidV2 f in EmbarqueFormasA)
            {
                dtEmbarqueFormaA.Rows.Add(f.toDataRow(dtEmbarqueFormaA));
            }

            foreach (EmbarqueFIFOAndroid ef in EmbarqueFIFO)
            {
                dtEmbarqueFIFO.Rows.Add(ef.toDataRow(dtEmbarqueFIFO));
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);

            dtEmbarqueHeader = null;
            dtEmbarqueDestino = null;
            dtEmbarqueProducto = null;
            dtEmbarqueFormaA = null;
            dtEmbarqueFIFO = null;
        }

        try
        {
            param = new Dictionary<string, object>();

            param.Add("@dtEmbarqueHeader", dtEmbarqueHeader);
            param.Add("@dtEmbarqueDestino", dtEmbarqueDestino);
            param.Add("@dtEmbarqueProducto", dtEmbarqueProducto);
            param.Add("@dtEmbarqueFormaA", dtEmbarqueFormaA);
            param.Add("@dtEmbarqueFIFO", dtEmbarqueFIFO);
            param.Add("@checksum", Checksum);
            param.Add("@idUsuario", idusuario);

            ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncEmbarques", param);

            foreach (DataTable dt in ds.Tables)
            {
                result += EmbarqueTablas(++cont) + ":" + GetDataTableToJson(dt);
            }

        }
        catch (Exception sync)
        {
            log.Error(sync);
            log.Error("ErrorUser: " + idusuario);
            result = "{'Error':'" + sync + "'";
        }



        return result + "}";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String SyncEmbarqueIncidencias(String embarqueIncidencia, int Checksum, int idusuario, int idPlanta)
    {
        string result = "{";
        EmbarqueIncidenciaAndroid[] EmbarqueIncidencia;
        JavaScriptSerializer js = new JavaScriptSerializer();
        DataTable dtEmbarqueIncidencia;
        try
        {
            EmbarqueIncidencia = js.Deserialize<EmbarqueIncidenciaAndroid[]>(embarqueIncidencia);
            dtEmbarqueIncidencia = new EmbarqueIncidenciaAndroid().toDataTable();
            foreach (EmbarqueIncidenciaAndroid ei in EmbarqueIncidencia)
            {
                dtEmbarqueIncidencia.Rows.Add(ei.toDataRow(dtEmbarqueIncidencia));
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);

            dtEmbarqueIncidencia = null;
        }

        try
        {
            DataTable dtIncidencias = dataaccess.executeStoreProcedureDataTable("sprAndroid_SyncEmbarqueIncidencias", new Dictionary<string, object>() {
                {"@dtEmbarqueIncidencia", dtEmbarqueIncidencia },
                { "@checksum", Checksum },
                { "@idUsuario", idusuario },
                { "@idPlanta", idPlanta}
            });
            int cont = 0;
            
            result += "EmbarqueIncidencia" + ":" + GetDataTableToJson(dtIncidencias);
            
        }
        catch(Exception ex)
        {
            log.Error(ex);
            log.Error("ErrorUser: " + idusuario);
            result = "{'Error':'" + ex + "'";
        }
        return result + "}";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncEmbarqueV2(String embarqueHeader, String embarqueDestino, String embarqueProducto, String embarqueFormaA, String embarqueFIFO, int Checksum, int idusuario)
    {
        string result = "{";

        EmbarqueHeaderAndroid[] EmbarqueHeaders;
        EmbarqueDestinoAndroidV2[] EmbarqueDestinos;
        EmbarqueProductoAndroid[] EmbarqueProductos;
        EmbarqueFormasAAndroidV2[] EmbarqueFormasA;
        EmbarqueFIFOAndroid[] EmbarqueFIFO;

        JavaScriptSerializer js = new JavaScriptSerializer();

        DataTable dtEmbarqueHeader;
        DataTable dtEmbarqueDestino;
        DataTable dtEmbarqueProducto;
        DataTable dtEmbarqueFormaA;
        DataTable dtEmbarqueFIFO;

        Dictionary<String, object> param;

        DataSet ds;
        int cont = 0;

        try
        {
            EmbarqueHeaders = js.Deserialize<EmbarqueHeaderAndroid[]>(embarqueHeader);
            EmbarqueDestinos = js.Deserialize<EmbarqueDestinoAndroidV2[]>(embarqueDestino);
            EmbarqueProductos = js.Deserialize<EmbarqueProductoAndroid[]>(embarqueProducto);
            EmbarqueFormasA = js.Deserialize<EmbarqueFormasAAndroidV2[]>(embarqueFormaA);
            EmbarqueFIFO = js.Deserialize<EmbarqueFIFOAndroid[]>(embarqueFIFO);

            dtEmbarqueHeader = new EmbarqueHeaderAndroid().toDataTable();
            dtEmbarqueDestino = new EmbarqueDestinoAndroidV2().toDataTable();
            dtEmbarqueProducto = new EmbarqueProductoAndroid().toDataTable();
            dtEmbarqueFormaA = new EmbarqueFormasAAndroidV2().toDataTable();
            dtEmbarqueFIFO = new EmbarqueFIFOAndroid().toDataTable();

            foreach (EmbarqueHeaderAndroid eh in EmbarqueHeaders)
            {
                dtEmbarqueHeader.Rows.Add(eh.toDataRow(dtEmbarqueHeader));
            }

            foreach (EmbarqueDestinoAndroidV2 d in EmbarqueDestinos)
            {
                dtEmbarqueDestino.Rows.Add(d.toDataRow(dtEmbarqueDestino));
            }

            foreach (EmbarqueProductoAndroid p in EmbarqueProductos)
            {
                dtEmbarqueProducto.Rows.Add(p.toDataRow(dtEmbarqueProducto));
            }

            foreach (EmbarqueFormasAAndroidV2 f in EmbarqueFormasA)
            {
                dtEmbarqueFormaA.Rows.Add(f.toDataRow(dtEmbarqueFormaA));
            }

            foreach (EmbarqueFIFOAndroid ef in EmbarqueFIFO)
            {
                dtEmbarqueFIFO.Rows.Add(ef.toDataRow(dtEmbarqueFIFO));
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);

            dtEmbarqueHeader = null;
            dtEmbarqueDestino = null;
            dtEmbarqueProducto = null;
            dtEmbarqueFormaA = null;
            dtEmbarqueFIFO = null;
        }

        try
        {
            param = new Dictionary<string, object>();

            param.Add("@dtEmbarqueHeader", dtEmbarqueHeader);
            param.Add("@dtEmbarqueDestino", dtEmbarqueDestino);
            param.Add("@dtEmbarqueProducto", dtEmbarqueProducto);
            param.Add("@dtEmbarqueFormaA", dtEmbarqueFormaA);
            param.Add("@dtEmbarqueFIFO", dtEmbarqueFIFO);
            param.Add("@checksum", Checksum);
            param.Add("@idUsuario", idusuario);

            ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncEmbarquesV7", param);

            foreach (DataTable dt in ds.Tables)
            {
                result += EmbarqueTablas(++cont) + ":" + GetDataTableToJson(dt);
            }

        }
        catch (Exception sync)
        {
            log.Error(sync);
            log.Error("ErrorUser: " + idusuario);
            result = "{'Error':'" + sync + "'";
        }



        return result + "}";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncEmbarqueV4(String embarqueHeader, String embarqueDestino, String embarqueProducto, String embarqueFormaA, String embarqueFIFO, int Checksum, int idusuario)
    {
        string result = "{";

        EmbarqueHeaderAndroidV3[] EmbarqueHeaders;
        EmbarqueDestinoAndroidV2[] EmbarqueDestinos;
        EmbarqueProductoAndroid[] EmbarqueProductos;
        EmbarqueFormasAAndroidV2[] EmbarqueFormasA;
        EmbarqueFIFOAndroid[] EmbarqueFIFO;

        JavaScriptSerializer js = new JavaScriptSerializer();

        DataTable dtEmbarqueHeader;
        DataTable dtEmbarqueDestino;
        DataTable dtEmbarqueProducto;
        DataTable dtEmbarqueFormaA;
        DataTable dtEmbarqueFIFO;

        Dictionary<String, object> param;

        DataSet ds;
        int cont = 0;

        try
        {
            EmbarqueHeaders = js.Deserialize<EmbarqueHeaderAndroidV3[]>(embarqueHeader);
            EmbarqueDestinos = js.Deserialize<EmbarqueDestinoAndroidV2[]>(embarqueDestino);
            EmbarqueProductos = js.Deserialize<EmbarqueProductoAndroid[]>(embarqueProducto);
            EmbarqueFormasA = js.Deserialize<EmbarqueFormasAAndroidV2[]>(embarqueFormaA);
            EmbarqueFIFO = js.Deserialize<EmbarqueFIFOAndroid[]>(embarqueFIFO);

            dtEmbarqueHeader = new EmbarqueHeaderAndroidV3().toDataTable();
            dtEmbarqueDestino = new EmbarqueDestinoAndroidV2().toDataTable();
            dtEmbarqueProducto = new EmbarqueProductoAndroid().toDataTable();
            dtEmbarqueFormaA = new EmbarqueFormasAAndroidV2().toDataTable();
            dtEmbarqueFIFO = new EmbarqueFIFOAndroid().toDataTable();

            foreach (EmbarqueHeaderAndroidV3 eh in EmbarqueHeaders)
            {
                dtEmbarqueHeader.Rows.Add(eh.toDataRow(dtEmbarqueHeader));
            }

            foreach (EmbarqueDestinoAndroidV2 d in EmbarqueDestinos)
            {
                dtEmbarqueDestino.Rows.Add(d.toDataRow(dtEmbarqueDestino));
            }

            foreach (EmbarqueProductoAndroid p in EmbarqueProductos)
            {
                dtEmbarqueProducto.Rows.Add(p.toDataRow(dtEmbarqueProducto));
            }

            foreach (EmbarqueFormasAAndroidV2 f in EmbarqueFormasA)
            {
                dtEmbarqueFormaA.Rows.Add(f.toDataRow(dtEmbarqueFormaA));
            }

            foreach (EmbarqueFIFOAndroid ef in EmbarqueFIFO)
            {
                dtEmbarqueFIFO.Rows.Add(ef.toDataRow(dtEmbarqueFIFO));
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);

            dtEmbarqueHeader = null;
            dtEmbarqueDestino = null;
            dtEmbarqueProducto = null;
            dtEmbarqueFormaA = null;
            dtEmbarqueFIFO = null;
        }

        try
        {
            param = new Dictionary<string, object>();

            param.Add("@dtEmbarqueHeader", dtEmbarqueHeader);
            param.Add("@dtEmbarqueDestino", dtEmbarqueDestino);
            param.Add("@dtEmbarqueProducto", dtEmbarqueProducto);
            param.Add("@dtEmbarqueFormaA", dtEmbarqueFormaA);
            param.Add("@dtEmbarqueFIFO", dtEmbarqueFIFO);
            param.Add("@checksum", Checksum);
            param.Add("@idUsuario", idusuario);

            ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncEmbarquesV9", param);

            foreach (DataTable dt in ds.Tables)
            {
                result += EmbarqueTablas(++cont) + ":" + GetDataTableToJson(dt);
            }

        }
        catch (Exception sync)
        {
            log.Error(sync);
            log.Error("ErrorUser: " + idusuario);
            result = "{'Error':'" + sync + "'";
        }



        return result + "}";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncEmbarqueV3(String embarqueHeader, String embarqueDestino, String embarqueProducto, String embarqueFormaA, String embarqueFIFO, int Checksum, int idusuario)
    {
        string result = "{";

        EmbarqueHeaderAndroidV2[] EmbarqueHeaders;
        EmbarqueDestinoAndroidV2[] EmbarqueDestinos;
        EmbarqueProductoAndroid[] EmbarqueProductos;
        EmbarqueFormasAAndroidV2[] EmbarqueFormasA;
        EmbarqueFIFOAndroid[] EmbarqueFIFO;

        JavaScriptSerializer js = new JavaScriptSerializer();

        DataTable dtEmbarqueHeader;
        DataTable dtEmbarqueDestino;
        DataTable dtEmbarqueProducto;
        DataTable dtEmbarqueFormaA;
        DataTable dtEmbarqueFIFO;

        Dictionary<String, object> param;

        DataSet ds;
        int cont = 0;

        try
        {
            EmbarqueHeaders = js.Deserialize<EmbarqueHeaderAndroidV2[]>(embarqueHeader);
            EmbarqueDestinos = js.Deserialize<EmbarqueDestinoAndroidV2[]>(embarqueDestino);
            EmbarqueProductos = js.Deserialize<EmbarqueProductoAndroid[]>(embarqueProducto);
            EmbarqueFormasA = js.Deserialize<EmbarqueFormasAAndroidV2[]>(embarqueFormaA);
            EmbarqueFIFO = js.Deserialize<EmbarqueFIFOAndroid[]>(embarqueFIFO);

            dtEmbarqueHeader = new EmbarqueHeaderAndroidV2().toDataTable();
            dtEmbarqueDestino = new EmbarqueDestinoAndroidV2().toDataTable();
            dtEmbarqueProducto = new EmbarqueProductoAndroid().toDataTable();
            dtEmbarqueFormaA = new EmbarqueFormasAAndroidV2().toDataTable();
            dtEmbarqueFIFO = new EmbarqueFIFOAndroid().toDataTable();

            foreach (EmbarqueHeaderAndroidV2 eh in EmbarqueHeaders)
            {
                dtEmbarqueHeader.Rows.Add(eh.toDataRow(dtEmbarqueHeader));
            }

            foreach (EmbarqueDestinoAndroidV2 d in EmbarqueDestinos)
            {
                dtEmbarqueDestino.Rows.Add(d.toDataRow(dtEmbarqueDestino));
            }

            foreach (EmbarqueProductoAndroid p in EmbarqueProductos)
            {
                dtEmbarqueProducto.Rows.Add(p.toDataRow(dtEmbarqueProducto));
            }

            foreach (EmbarqueFormasAAndroidV2 f in EmbarqueFormasA)
            {
                dtEmbarqueFormaA.Rows.Add(f.toDataRow(dtEmbarqueFormaA));
            }

            foreach (EmbarqueFIFOAndroid ef in EmbarqueFIFO)
            {
                dtEmbarqueFIFO.Rows.Add(ef.toDataRow(dtEmbarqueFIFO));
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);

            dtEmbarqueHeader = null;
            dtEmbarqueDestino = null;
            dtEmbarqueProducto = null;
            dtEmbarqueFormaA = null;
            dtEmbarqueFIFO = null;
        }

        try
        {
            param = new Dictionary<string, object>();

            param.Add("@dtEmbarqueHeader", dtEmbarqueHeader);
            param.Add("@dtEmbarqueDestino", dtEmbarqueDestino);
            param.Add("@dtEmbarqueProducto", dtEmbarqueProducto);
            param.Add("@dtEmbarqueFormaA", dtEmbarqueFormaA);
            param.Add("@dtEmbarqueFIFO", dtEmbarqueFIFO);
            param.Add("@checksum", Checksum);
            param.Add("@idUsuario", idusuario);

            ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncEmbarquesV5", param);

            foreach (DataTable dt in ds.Tables)
            {
                result += EmbarqueTablas(++cont) + ":" + GetDataTableToJson(dt);
            }

        }
        catch (Exception sync)
        {
            log.Error(sync);
            log.Error("ErrorUser: " + idusuario);
            result = "{'Error':'" + sync + "'";
        }



        return result + "}";
    }

    [WebMethod]
    [ScriptMethod]
    public string SyncGrowing(String growingcaptura, String growingcapturagrupo, String growingcapturaparametro, String growingcapturaparametropropiedad, int Checksum, int idUsuario)
    {
        string result = "{";

        GrowingCapturaAndroid[] growingCapturaAndroid;
        GrowingCapturaGrupoAndroid[] growingCapturaGrupo;
        GrowingCapturaParametroAndroid[] growingCapturaParametro;
        CapturaGrowingParametroPropiedadAndroid[] growingCapturaPropiedad;

        DataTable dtGrowingCaptura, dtGrowingCapturaGrupo, dtGrowingCapturaParametro, dtGrowingCapturaPropiedad;


        Dictionary<string, object> param = new Dictionary<string,object>();
        JavaScriptSerializer js = new JavaScriptSerializer();
 
        try
        {
            growingCapturaAndroid = js.Deserialize<GrowingCapturaAndroid[]>(growingcaptura);
            growingCapturaGrupo = js.Deserialize<GrowingCapturaGrupoAndroid[]>(growingcapturagrupo);
            growingCapturaParametro = js.Deserialize<GrowingCapturaParametroAndroid[]>(growingcapturaparametro);
            growingCapturaPropiedad = js.Deserialize<CapturaGrowingParametroPropiedadAndroid[]>(growingcapturaparametropropiedad);

            dtGrowingCaptura = new GrowingCapturaAndroid().getDataTable();
            dtGrowingCapturaGrupo = new GrowingCapturaGrupoAndroid().getDataTable();
            dtGrowingCapturaParametro = new GrowingCapturaParametroAndroid().getDataTable();
            dtGrowingCapturaPropiedad = new CapturaGrowingParametroPropiedadAndroid().getDataTable();

            foreach (GrowingCapturaAndroid item in growingCapturaAndroid)
            {
                dtGrowingCaptura.Rows.Add(item.toDataRow().ItemArray);
            }

            foreach (GrowingCapturaGrupoAndroid item in growingCapturaGrupo)
            {
                dtGrowingCapturaGrupo.Rows.Add(item.toDataRow().ItemArray);
            }

            foreach (GrowingCapturaParametroAndroid item in growingCapturaParametro)
            {
                dtGrowingCapturaParametro.Rows.Add(item.toDataRow().ItemArray);   
            }

            foreach (CapturaGrowingParametroPropiedadAndroid item in growingCapturaPropiedad)
            {
                dtGrowingCapturaPropiedad.Rows.Add(item.toDataRow().ItemArray);
            }

            param.Add("@GrowingCapturaAndroid", dtGrowingCaptura);
            param.Add("@GrowingCapturaGrupoAndroid", dtGrowingCapturaGrupo);
            param.Add("@GrowingCapturaParametroAndroid", dtGrowingCapturaParametro);
            param.Add("@GrowingCapturaParametroPropiedadAndroid", dtGrowingCapturaPropiedad);
            param.Add("@Checksum", Checksum);
            param.Add("@idUsuario", idUsuario);

            DataSet ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncGrowing", param);
            int index = 0;
            foreach (DataTable dt in ds.Tables)
            {
                result += tablasGrowing(index)+GetDataTableToJson(dt);
                
                index++;
            }

        }
        catch (Exception ex)
        {

            log.Error(ex);
        }

        return result + "}";
    }
  
    [WebMethod]
    [ScriptMethod]
    public string SyncGrowing2(String growingcaptura, String growingcapturagrupo, String growingcapturaparametro, String growingcapturaparametropropiedad, int Checksum, int idUsuario)
    {
        string result = "{";

        GrowingCapturaAndroid2[] growingCapturaAndroid;
        GrowingCapturaGrupoAndroid2[] growingCapturaGrupo;
        GrowingCapturaParametroAndroid2[] growingCapturaParametro;
        CapturaGrowingParametroPropiedadAndroid2[] growingCapturaPropiedad;

        DataTable dtGrowingCaptura, dtGrowingCapturaGrupo, dtGrowingCapturaParametro, dtGrowingCapturaPropiedad;


        Dictionary<string, object> param = new Dictionary<string, object>();
        JavaScriptSerializer js = new JavaScriptSerializer();

        try
        {
            growingCapturaAndroid = js.Deserialize<GrowingCapturaAndroid2[]>(growingcaptura);
            growingCapturaGrupo = js.Deserialize<GrowingCapturaGrupoAndroid2[]>(growingcapturagrupo);
            growingCapturaParametro = js.Deserialize<GrowingCapturaParametroAndroid2[]>(growingcapturaparametro);
            growingCapturaPropiedad = js.Deserialize<CapturaGrowingParametroPropiedadAndroid2[]>(growingcapturaparametropropiedad);

            dtGrowingCaptura = new GrowingCapturaAndroid2().getDataTable();
            dtGrowingCapturaGrupo = new GrowingCapturaGrupoAndroid2().getDataTable();
            dtGrowingCapturaParametro = new GrowingCapturaParametroAndroid2().getDataTable();
            dtGrowingCapturaPropiedad = new CapturaGrowingParametroPropiedadAndroid2().getDataTable();

            foreach (GrowingCapturaAndroid2 item in growingCapturaAndroid)
            {
                dtGrowingCaptura.Rows.Add(item.toDataRow().ItemArray);
            }

            foreach (GrowingCapturaGrupoAndroid2 item in growingCapturaGrupo)
            {
                dtGrowingCapturaGrupo.Rows.Add(item.toDataRow().ItemArray);
            }

            foreach (GrowingCapturaParametroAndroid2 item in growingCapturaParametro)
            {
                dtGrowingCapturaParametro.Rows.Add(item.toDataRow().ItemArray);
            }

            foreach (CapturaGrowingParametroPropiedadAndroid2 item in growingCapturaPropiedad)
            {
                dtGrowingCapturaPropiedad.Rows.Add(item.toDataRow().ItemArray);
            }

            param.Add("@GrowingCapturaAndroid", dtGrowingCaptura);
            param.Add("@GrowingCapturaGrupoAndroid", dtGrowingCapturaGrupo);
            param.Add("@GrowingCapturaParametroAndroid", dtGrowingCapturaParametro);
            param.Add("@GrowingCapturaParametroPropiedadAndroid", dtGrowingCapturaPropiedad);
            param.Add("@Checksum", Checksum);
            param.Add("@idUsuario", idUsuario);

            DataSet ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncGrowingV2", param);
            int index = 0;
            foreach (DataTable dt in ds.Tables)
            {
                result += tablasGrowing(index) + GetDataTableToJson(dt);

                index++;
            }

        }
        catch (Exception ex)
        {

            log.Error(ex);
        }

        return result + "}";
    }
    
    private string tablasGrowing(int index)
    {
        String[] tablas = { "'GrowingCaptura':", ",'GrowingCapturaGrupo':", ",'GrowingCapturaParametro':", ",'GrowingCapturaPropiedad':", ",'Checksum':" };

            return tablas[index];
    }



    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingCaptura(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingCaptura", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingGrupoV2(string idplanta)
    {
        try
        {
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingGrupo", new Dictionary<string, object>() { { "@idPlanta", idplanta } });
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingParametroV2(string idplanta)
    {
        try
        {
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingParametro", new Dictionary<string, object>() { { "@idPlanta", idplanta } });
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingParametroPropiedadV2(string idplanta)
    {
        try
        {
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingParametroPropiedad", new Dictionary<string, object>() { { "@idPlanta", idplanta } });
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingParametroPropiedadOpcionV2(string idplanta)
    {
        try
        {
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingParametroPropiedadOpcion", new Dictionary<string, object>() { { "@idPlanta", idplanta } });
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingGrupo()
    {
        try
        {

            return "[]";
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingParametro()
    {
        try
        {
           
            return "[]";
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingParametroPropiedad()
    {
        try
        {

            return "[]";
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingParametroPropiedadOpcion()
    {
        try
        {

            return "[]";
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingCapturaGrupo(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingCapturaGrupo", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingCapturaParametro(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingCapturaParametro", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getGrowingCapturaParametroPropiedad(string id, string checksum)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", id);
            param.Add("@checksum", checksum);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingCapturaParametroPropiedad", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string SyncPrestamoAsociados(String prestamosAsociados, int Checksum, int idUsuario)
    {
        string result = "{";

        PrestamoAsociadosAndroid[] PrestamoAsociados;

        JavaScriptSerializer js = new JavaScriptSerializer();

        DataTable dtPrestamoAsociados;


        Dictionary<String, object> param;

        DataSet ds;
        int cont = 0;

        try
        {
            PrestamoAsociados = js.Deserialize<PrestamoAsociadosAndroid[]>(prestamosAsociados);
            dtPrestamoAsociados = new PrestamoAsociadosAndroid().toDataTable();


            foreach (PrestamoAsociadosAndroid eh in PrestamoAsociados)
            {
                dtPrestamoAsociados.Rows.Add(eh.toDataRow(dtPrestamoAsociados));
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
            dtPrestamoAsociados = null;
        }

        try
        {
            param = new Dictionary<string, object>();

            param.Add("@dtPrestamoAsociados", dtPrestamoAsociados);
            param.Add("@checksum", Checksum);
            param.Add("@idUsuario", idUsuario);

            ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncPrestamoAsociados", param);

            foreach (DataTable dt in ds.Tables)
            {
                result += PrestamoTablas(++cont) + ":" + GetDataTableToJson(dt);
            }

        }
        catch (Exception sync)
        {
            log.Error(sync);
            result = "{'Error':'" + sync + "'";
        }



        return result + "}";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPreharvestDetalle(int idUsuario, int idPlanta)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getPreharvestDetalle", new Dictionary<string, object>() { { "@idUsuario", idUsuario }, { "@idPlanta", idPlanta } }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPrestamoAsociados(int idUsuario)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getPrestamoAsociados", new Dictionary<string, object>() { { "@idUsuario", idUsuario } }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getLideres()
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getLideres", null));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getRelLiderInvernadero(int idPlanta)
    {
        try
        {
            return GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getRelLiderInvernadero",
                                        new Dictionary<string, object> { { "@idPlanta", idPlanta } }));
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmpaqueBrix()
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmpaqueBrix", null));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueHeader(string idUsuario, string checksum)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueHeaderWMP2", new Dictionary<string, object>() { { "@idUsuario", idUsuario }, { "@checksum", checksum } }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueDestino(string idUsuario)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueDestinoWMP2", new Dictionary<string, object>() { { "@idUsuario", idUsuario } }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueProducto(string idUsuario)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueProductoWMP2", new Dictionary<string, object>() { { "@idUsuario", idUsuario } }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueFormaA(string idUsuario)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueFormaAWMP2", new Dictionary<string, object>() { { "@idUsuario", idUsuario } }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueDestinoV2(string idUsuario, string checksum)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueDestinoWMP2V2", new Dictionary<string, object>() { { "@idUsuario", idUsuario } ,{"@checksum",checksum} }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueProductoV2(string idUsuario, string checksum)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueProductoWMP2V2", new Dictionary<string, object>() { { "@idUsuario", idUsuario } ,{"@checksum",checksum} }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getEmbarqueFormaAV2(string idUsuario, string checksum)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetEmbarqueFormaAWMP2V2", new Dictionary<string, object>() { { "@idUsuario", idUsuario } ,{"@checksum",checksum} }));
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String getFoliosTractorista(string folio)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetFoliosTractorista", new Dictionary<string, object>() { { "@folio", folio } }));

        return result;
        
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getPuertasEmpaque(int idPlanta)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_GetPuertasEmpaque", new Dictionary<string, object>() { { "@idPlanta", idPlanta } }));

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string getFosliosAsignados(int idPlanta, int idUsuario)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idPlanta", idPlanta);
            param.Add("@idUsuario", idUsuario);
            DataTable data = dataaccess.executeStoreProcedureDataTable("sprAndroid_getFosliosAsignados", param);
            return GetDataTableToJson(data);
        }
        catch (Exception x)
        {
            log.Error(x);
            throw;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string recogeFoliosInvernadero(int idUsuario, int idInvernadero)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idUsuario", idUsuario);
            param.Add("@idInvernadero", idInvernadero);
            String data = dataaccess.executeStoreProcedureString("sprAndroid_recogeFoliosInvernadero", param);
            return data;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "false";
            throw;
        }
    }

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = true)]
    //public String getGrowingGrupo()
    //{
    //    string result = "";

    //    result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingGrupo", new Dictionary<string, object>() { }));

    //    return result;

    //}

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = true)]
    //public String getGrowingParametro()
    //{
    //    string result = "";

    //    result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingParametro", new Dictionary<string, object>() { }));

    //    return result;

    //}

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = true)]
    //public String getGrowingParametroPropiedad()
    //{
    //    string result = "";

    //    result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingParametroPropiedad", new Dictionary<string, object>() { }));

    //    return result;

    //}

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = true)]
    //public String getGrowingParametroPropiedadOpcion()
    //{
    //    string result = "";

    //    result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_getGrowingParametroPropiedadOpcion", new Dictionary<string, object>() { }));

    //    return result;

    //}

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String borraFoliosTractorista(string folio)
    {
        string result = "";
        try
        {
            result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_BorraFoliosTractorista", new Dictionary<string, object>() { { "@folio", folio }}));
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return result;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String entregaFoliosTractorista(string folios, string merma, string tunel, int idPlanta)
    {
        FolioTractorista[] Folios;
        MermaTractorista[] Merma;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string result = "";
        int cont =0;
        try
        {
            Folios = serializer.Deserialize<FolioTractorista[]>(folios);
            Merma = serializer.Deserialize<MermaTractorista[]>(merma);

            DataTable dt = new DataTable();
            dt.Columns.Add("folio");
            dt.Columns.Add("merma");
            foreach (FolioTractorista item in Folios)
            {
                DataRow dr = dt.NewRow();
                dr["folio"] = item.folio;
                dr["merma"] = Merma[cont++].merma;
                dt.Rows.Add(dr);

            }

            
            result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_EntregaFoliosTractorista", new Dictionary<string, object>() { { "@folios", dt }, { "@tunel", tunel },{"@idPlanta", idPlanta} }));
            result = result.Replace("[", "").Replace("]", "");
            
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
            return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String entregaFoliosTractoristaV2(string folios, int idUsuario)
    {
        Dictionary<string, object> param = new Dictionary<string, object>();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        FoliosTractoristaV2[] Folios;

        DataTable dtFolios = null;

        string result = "";
        try
        {
            Folios = serializer.Deserialize<FoliosTractoristaV2[]>(folios);

            dtFolios = new FoliosTractoristaV2().toDataTable();

            foreach (FoliosTractoristaV2 item in Folios)
            {
                dtFolios.Rows.Add(item.toDataRow(dtFolios));
            }

            param.Add("@folios", dtFolios);
            param.Add("@idUsuario", idUsuario);
            result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_EntregaFoliosTractoristaV2", param));

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String regresaFolioTractorista(string folio)
    {
        string result = "";

        result = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("sprAndroid_regresaFolioTractorista", new Dictionary<string, object>() { { "@folios", folio } }));

        return result;

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string validateGate(string codigo, int idPlanta)
    {
        DataTable result;

        result = dataaccess.executeStoreProcedureDataTable("sprAndroid_validaPuertaEmbarque", new Dictionary<string, object>() { { "@codigo", codigo }, {"@idPlanta", idPlanta} });



        return GetDataTableToJson(result);

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string moverFolio(string QR, string folio, bool dividido, int id /*, int idFormaA, int idPlanta*/)
    {
        string idQR = "";
        string pX = "";
        string pY = "";
        string pZ = "";

        string result = "{";
        Dictionary<string, object> param = new Dictionary<string, object>();

        try
        {
            if (QR.Contains(".") && QR.Contains("|"))
            {
                idQR = QR.Split('|')[0];
                pX = QR.Split('|')[1].Split('.')[0];
                pY = QR.Split('|')[1].Split('.')[1];
                pZ = QR.Split('|')[1].Split('.')[2];
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            param.Add("@idQR", idQR);
            param.Add("@pX", pX);
            param.Add("@pY", pY);
            param.Add("@pZ", pZ); 
            param.Add("@folio", folio);
        }

        try
        {
            string[] Tablas = { "\"FolioInsertado\":" };
            DataSet ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_moverFoliosShipping", param);
            if (ds.Tables[0].Rows.Count != 0 && ds.Tables[3].Rows.Count == 0) //valida que exista una posicion valida y que este libre
            {
                param.Add("@id", id);
                param.Add("@dividido", dividido);
                param.Add("@valida", false);
                //param.Add("@idFormaA", idFormaA);
                //param.Add("@idPlanta", idPlanta);
                ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_moverFoliosShipping", param);
                for (var indx = 0; indx < ds.Tables.Count; indx++)
                {
                    result += Tablas[indx] + "\"true\"";
                }
            }
            else
            {
                result += Tablas[0] + "\"false\"";
            }
            result += "}";

        }
        catch (Exception esx)
        {
            result += "}";
            log.Error(esx);
        }

        return result;
    }


    //[WebMethod]
    //[ScriptMethod(UseHttpGet = true)]
    //public string insertFolios(string idLocation, DataTable json)
    //{

    //    string result = "{";
    //    try
    //    {
    //        Dictionary<string, object> param = new Dictionary<string, object>();

    //        param.Add("@idQR", idLocation);
    //        param.Add("@tblFoliosConfirmXML", json);

    //        string[] Tablas = { "'tabla1':", ",'tabla2':" };
    //        DataSet ds = dataaccess.executeStoreProcedureDataSet("ev2_spr_GET_Dataqrs", param);
    //        for (var indx = 0; indx < ds.Tables.Count; indx++)
    //        {
    //            result += Tablas[indx] + GetDataTableToJson(ds.Tables[indx]);
    //        }
    //        result += "}";
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        result += "}";
    //    }
    //    return result;
    //}

    //[WebMethod] //regresado a syncAll
    //[ScriptMethod(UseHttpGet = true)]
    //public string EtimadocajasAll(int idUsuario, int checksum, string CajasCaptura, string CajasCapturaDetalle)
    //{
    //    string result = "{";
    //    Dictionary<string, object> param = new Dictionary<string, object>();
    //    JavaScriptSerializer js = new JavaScriptSerializer();

    //    CajasCaptura[] CajasCapturas;
    //    CajasCapturaDetalle[] CajasCapturaDetalles;

    //    DataTable dtCajasCapturas = dtCajasCaptura();
    //    DataTable dtCajasCapturaDetalles = dtCajasCapturaDetalle();

    //    try
    //    {
    //        CajasCapturas = js.Deserialize<CajasCaptura[]>(CajasCaptura);
    //        CajasCapturaDetalles = js.Deserialize<CajasCapturaDetalle[]>(CajasCapturaDetalle);

    //        foreach (var captura in CajasCapturas)
    //        {
    //            DataRow dr = dtCajasCapturas.NewRow();
    //            dr["idEstimadocajas"] = captura.idEstimadocajas;
    //            dr["idEstimadocajasLocal"] = captura.idEstimadocajasLocal;
    //            dr["idInvernadero"] = captura.idInvernadero;
    //            dr["idLider"] = captura.idLider;
    //            dr["idCosecha"] = captura.idCosecha;
    //            dr["idCosechaLocal"] = captura.idCosechaLocal;
    //            dr["surcos"] = captura.surcos;
    //            dr["semana"] = captura.semana;
    //            dr["borrado"] = captura.borrado;
    //            dr["usuarioCaptura"] = captura.usuarioCaptura;
    //            dr["usuarioModifica"] = captura.usuarioModifica;
    //            dr["fechaCaptura"] = captura.fechaCaptura;
    //            dr["fechaModifica"] = captura.fechaModifica;
    //            dr["estatus"] = captura.estatus;

    //            dtCajasCapturas.Rows.Add(dr);
    //        }

    //        foreach (var detalle in CajasCapturaDetalles)
    //        {
    //            DataRow dr = dtCajasCapturaDetalles.NewRow();

    //            dr["idEstimadoCajasCaptura"] = detalle.idEstimadoCajasCaptura;
    //            dr["idEstimadoCajasCapturaLocal"] = detalle.idEstimadoCajasCapturaLocal;
    //            dr["idEstimadoCajas"] = detalle.idEstimadoCajas;
    //            dr["idEstimadoCajasLocal"] = detalle.idEstimadoCajasLocal;
    //            dr["surco"] = detalle.surco;
    //            dr["cajas"] = detalle.cajas;
    //            dr["estimado"] = detalle.estimado;
    //            dr["fechaCaptura"] = detalle.fechaCaptura;
    //            dr["fechaModifica"] = detalle.fechaModifica;
    //            dr["borrado"] = detalle.borrado;
    //            dr["estatus"] = detalle.estatus;

    //            dtCajasCapturaDetalles.Rows.Add(dr);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        throw;
    //    }
    //    finally
    //    {
    //        param.Add("@CajasCaptura", dtCajasCapturas);
    //        param.Add("@CajasCapturaDetalle", dtCajasCapturaDetalles);
    //    }

    //    param.Add("@idUsuario", idUsuario);
    //    param.Add("@checksum", checksum);
    //    try
    //    {
    //        string[] Tablas = { "'CajasCaptura':", ",'CajasCapturaDetalle':", ",'Checksum':" };
    //        DataSet ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_EstimadoCajasAll", param);
    //        for (var indx = 0; indx < ds.Tables.Count; indx++)
    //        {
    //            result += Tablas[indx] + GetDataTableToJson(ds.Tables[indx]);
    //        }
    //        result += "}";

    //    }
    //    catch (Exception esx)
    //    {
    //        result += "}";
    //        log.Error(esx);
    //    }

    //    return result;
    //}

    public string PrestamoTablas(int a)
    {
        switch (a)
        {
            case 1:
                return "'PrestamoAsociados'";
            case 2:
                return ",'checksum'";
            default:
                return "";

        }
    }

    public string EmbarqueTablas(int a)
    {
        switch (a)
        {
            case 1:
                return "'EmbarqueHeader'";
            case 2:
                return ",'EmbarqueDestino'";
            case 3:
                return ",'EmbarqueProducto'";
            case 4:
                return ",'EmbarqueFormaA'";
            case 5:
                return ",'checksum'";
            default:
                return "";

        }
    }


    public string intToString(int a)
    {
        switch (a)
        {
            case 1:
                return "'Actividad Programa'";

            case 2:
                return ",'No Programadas'";

            case 3:
                return ",'Asociados'";

            case 4:
                return ",'Periodos'";

            case 5:
                return ",'Cosecha'";

            case 6:
                return ",'FormaA'";

            case 7:
                return ",'CapturaFormaA'";

            case 8:
                return ",'Merma'";

            case 9:
                return ",'TrasladoMerma'";

            case 10:
                return ",'Monitoreo'";

            case 11:
                return ",'CheckList'";

            case 12:
                return ",'CheckCriterio'";

            case 13:
                return ",'BrixCaptura'";

            case 14:
                return ",'BrixDetalle'";

            case 15:
                return ",'BrixHeader'";

            case 16:
                return ",'BrixFirmeza'";

            case 17:
                return ",'BrixColor'";

            case 18:
                return ",'BrixDefecto'";

            case 19:
                return ",'CajasCaptura'";

            case 20:
                return ",'CajasCapturaDetalle'";

            case 21:
                return ",'Checksum'";

            default:
                return "";

        }
    }


    public string intToStringV2(int a)
    {
        switch (a)
        {
            case 1:
                return "'Actividad Programa'";

            case 2:
                return ",'No Programadas'";

            case 3:
                return ",'Asociados'";

            case 4:
                return ",'Periodos'";

            case 5:
                return ",'Cosecha'";

            case 6:
                return ",'FormaA'";

            case 7:
                return ",'CapturaFormaA'";

            case 8:
                return ",'Merma'";

            case 9:
                return ",'TrasladoMerma'";

            case 10:
                return ",'Monitoreo'";

            case 11:
                return ",'CheckList'";

            case 12:
                return ",'CheckCriterio'";

            case 13:
                return ",'CajasCaptura'";

            case 14:
                return ",'CajasCapturaDetalle'";

            //case 15:
            //    return ",'CapturaTrabajos'";

            //case 16:
            //    return ",'CapturaTrabajosHeader'";

            case 15:
                return ",'SeccionPreharvest'";

            case 16:
                return ",'Checksum'";

            default:
                return "";

        }
    }

    #endregion

    #region Datatbles

    private static DataTable dtActividadProgramada()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idEtapa");
        dt.Columns.Add("idCiclo");
        dt.Columns.Add("cantidadDeElementos");
        dt.Columns.Add("semana");
        dt.Columns.Add("jornalesEstimados");
        dt.Columns.Add("minutosEstimados");
        dt.Columns.Add("esDirectriz");
        dt.Columns.Add("esInterplanting");
        dt.Columns.Add("borrado");
        dt.Columns.Add("aprobadaPor");
        dt.Columns.Add("rechazadaPor");
        dt.Columns.Add("usuarioModifica");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcoFin");
        dt.Columns.Add("esColmena");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    private static DataTable dtActividadPeriodos()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idPeriodoTab");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("surcos");
        dt.Columns.Add("inicio");
        dt.Columns.Add("fin");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;

    }

    private static DataTable dtActividadNoProgramada()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idActividadNoProgramada");
        dt.Columns.Add("idActividadNoProgramadaTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idEtapa");
        dt.Columns.Add("idCiclo");
        dt.Columns.Add("razon");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("cantidadDeElementos");
        dt.Columns.Add("semanaProgramacion");
        dt.Columns.Add("anioProgramacion");
        dt.Columns.Add("esInterplanting");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    private static DataTable dtActividadJornales()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividadAsociado");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadTab");
        dt.Columns.Add("idPeriodoTab");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("ausente");
        dt.Columns.Add("estatus");

        return dt;
    }

    private static DataTable dtCosecha()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCosecha");
        dt.Columns.Add("idCosechaTab");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaTab");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("cantidadProduccion");
        dt.Columns.Add("estimadoMedioDia");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");

        return dt;
    }

    private static DataTable dtMerma()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idMerma");
        dt.Columns.Add("idMermaTab");
        dt.Columns.Add("idCoseha");
        dt.Columns.Add("idCosechaTab");
        dt.Columns.Add("idRazon");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("observacion");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    private static DataTable dtTrasladoMerma()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idTrasladoMerma");
        dt.Columns.Add("idTrasladoMermaLocal");
        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaALocal");
        dt.Columns.Add("idRazon");
        dt.Columns.Add("Cajas");
        dt.Columns.Add("Comentarios");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    private static DataTable dtFormaA()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaATab");
        dt.Columns.Add("idPrograma");
        dt.Columns.Add("idProgramaTab");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("prefijo");
        dt.Columns.Add("dmcCalidad");
        dt.Columns.Add("dmcMercado");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("folio");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");
        dt.Columns.Add("fechaFinTractorista");
        dt.Columns.Add("fechaInicioTractorista");
        dt.Columns.Add("storage");

        return dt;
    }

    private static DataTable dtFormaAv2()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaATab");
        dt.Columns.Add("idPrograma");
        dt.Columns.Add("idProgramaTab");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("prefijo");
        dt.Columns.Add("dmcCalidad");
        dt.Columns.Add("dmcMercado");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("folio");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");
        dt.Columns.Add("fechaFinTractorista");
        dt.Columns.Add("fechaInicioTractorista");
        dt.Columns.Add("storage");
        dt.Columns.Add("UUID");

        return dt;
    }


    private static DataTable dtFormaAv3()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("idFormaA");
        dt.Columns.Add("idFormaATab");
        dt.Columns.Add("idPrograma");
        dt.Columns.Add("idProgramaTab");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("fechaInicio");
        dt.Columns.Add("prefijo");
        dt.Columns.Add("dmcCalidad");
        dt.Columns.Add("dmcMercado");
        dt.Columns.Add("comentarios");
        dt.Columns.Add("folio");
        dt.Columns.Add("cerrada");
        dt.Columns.Add("estatus");
        dt.Columns.Add("fechaFinTractorista");
        dt.Columns.Add("fechaInicioTractorista");
        dt.Columns.Add("storage");
        dt.Columns.Add("UUID");
        dt.Columns.Add("Preharvest");
        dt.Columns.Add("idTipoCaja");
        dt.Columns.Add("idTipoCosecha");
        return dt;
    }

    private static DataTable dtFormaAToWs()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("folio");
        dt.Columns.Add("XMLData");
        dt.Columns.Add("pX");
        dt.Columns.Add("pY");
        dt.Columns.Add("pZ");

        return dt;
    }


    //private static DataTable dtCapturaFormaAToWs()
    //{
    //    DataTable dt = new DataTable();

    //    dt.Columns.Add("cajas");
    //    dt.Columns.Add("idFormaA");

    //    return dt;
    //}


    public static DataTable dtBrixCaptura()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("estatus");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaTab");
        dt.Columns.Add("idBrixCaptura");
        dt.Columns.Add("idBrixCapturaTab");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idSeccion");
        dt.Columns.Add("idCalidad");
        dt.Columns.Add("libras");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtBrixDetalle()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixCaptura");
        dt.Columns.Add("estatus");
        dt.Columns.Add("brix");
        dt.Columns.Add("idBrixCapturaTab");
        dt.Columns.Add("idBrixDetalle");
        dt.Columns.Add("idBrixDetalleTab");
        dt.Columns.Add("idColor");
        dt.Columns.Add("UUID");
        return dt;
    }

    /*Cajas Captura: datatables*/
    public static DataTable dtCajasCaptura()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idEstimadocajas");
        dt.Columns.Add("idEstimadocajasLocal");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idLider");
        dt.Columns.Add("idCosecha");
        dt.Columns.Add("idCosechaLocal");
        dt.Columns.Add("surcos");
        dt.Columns.Add("semana");
        dt.Columns.Add("borrado");
        dt.Columns.Add("usuarioCaptura");
        dt.Columns.Add("usuarioModifica");
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("fechaModifica");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    public static DataTable dtCajasCapturaDetalle()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idEstimadoCajasCaptura");
        dt.Columns.Add("idEstimadoCajasCapturaLocal");
        dt.Columns.Add("idEstimadoCajas");
        dt.Columns.Add("idEstimadoCajasLocal");
        dt.Columns.Add("surco");
        dt.Columns.Add("cajas");
        dt.Columns.Add("estimado");
        dt.Columns.Add("fechaCaptura");
        dt.Columns.Add("fechaModifica");
        dt.Columns.Add("borrado");
        dt.Columns.Add("estatus");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("asignado");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable dtCapturaTrabajo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCapturaHeaderHistoria");
        dt.Columns.Add("idCapturaHeaderHistoriaLocal");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcoFin");
        dt.Columns.Add("horaInicio");
        dt.Columns.Add("horaFin");
        dt.Columns.Add("calidad");
        dt.Columns.Add("comentario");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaLocal");
        dt.Columns.Add("fechaModificacion");
        dt.Columns.Add("usuarioModifico");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idPeriodoLocal");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("estatus");  
        dt.Columns.Add("UUID");
        dt.Columns.Add("fechaCapturaTableta");
        return dt;


    }
    public static DataTable dtCapturaTrabajoHeader()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idCapturaHeader");
        dt.Columns.Add("idCapturaHeaderLocal");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcoFin");
        dt.Columns.Add("horaInicio");
        dt.Columns.Add("horaFin");
        dt.Columns.Add("calidad");
        dt.Columns.Add("comentario");
        dt.Columns.Add("idActividadPrograma");
        dt.Columns.Add("idActividadProgramaLocal");
        dt.Columns.Add("fechaModificacion");
        dt.Columns.Add("usuarioModifico");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idPeriodoLocal");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("estatus");
        dt.Columns.Add("UUID");
        dt.Columns.Add("fechaCapturaTableta");
        return dt;


    }
    public static DataTable getDtBrixHeader()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idBrixCaptura");
        dt.Columns.Add("idBrixCapturaLocal");
        dt.Columns.Add("idProducto");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("CajasTotales");
        dt.Columns.Add("Folio");
        dt.Columns.Add("Comentarios");
        dt.Columns.Add("idUsuarioCaptura");
        dt.Columns.Add("FechaCaptura");
        dt.Columns.Add("idUsuarioModifica");
        dt.Columns.Add("FechaModifica");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable getDtBrixFirmeza()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixFirmeza");
        dt.Columns.Add("idBrixFirmezaLocal");
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idFirmeza");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("Value");
        dt.Columns.Add("Porcentaje");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable getDtBrixColor()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixColor");
        dt.Columns.Add("idBrixColorLocal");
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idColor");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("Value");
        dt.Columns.Add("Porcentaje");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }
    public static DataTable getDtBrixDefecto()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idBrixDefecto");
        dt.Columns.Add("idBrixDefectoLocal");
        dt.Columns.Add("idBrixHeader");
        dt.Columns.Add("idBrixHeaderLocal");
        dt.Columns.Add("idDefecto");
        dt.Columns.Add("idSegmento");
        dt.Columns.Add("Value");
        dt.Columns.Add("Porcentaje");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("UUID");
        return dt;
    }

    private static DataTable dtFoliosEmbarques()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Folio");
        return dt;
    }
}

    #endregion
