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

/// <summary>
/// Summary description for Fumigaciones
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Fumigaciones : System.Web.Services.WebService
{
    DataAccess dataaccess = new DataAccess();
    public Fumigaciones()
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
    public string setFumigacion(DataSet ds, string reentrada, string cosecha)
    {
        string result = "";

        try
        {
            DataTable fumigacion = ds.Tables[0];
            DataTable detalles = ds.Tables[1];
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("@Fumigacion", fumigacion);
            param.Add("@reentrada", reentrada);
            param.Add("@cosecha", cosecha);
            param.Add("@FumigacionDetalle", detalles);

            DataSet sd = dataaccess.executeStoreProcedureDataSet("spr_FumigacionInsertar", param);
            result = "ok";
        }

        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string setCancelarFumigacion(DataSet ds)
    {
        string result = "";

        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@FumigacionCancel", ds.Tables[0]);

            DataSet sd = dataaccess.executeStoreProcedureDataSet("spr_FumigacionCancelar", param);
            result = "ok";
        }

        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }

}
