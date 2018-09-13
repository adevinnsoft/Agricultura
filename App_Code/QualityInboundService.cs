using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Reflection;

/// <summary>
/// Descripción breve de QualityInboundService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
// [System.Web.Script.Services.ScriptService]
public class QualityInboundService : System.Web.Services.WebService
{

    private static readonly ILog log = LogManager.GetLogger(typeof(QualityInboundService));
    DataAccess dataaccess = new DataAccess("EmpaqueConn");
    public QualityInboundService()
    {

        //Eliminar la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String JsonTemplate()
    {
        String result = "";

        DataTable dtHeader = new DataTable();
        DataTable dtQualityDetails = new DataTable();

        dtHeader = dataaccess.executeStoreProcedureDataTable("spr_TestQualityWebService", null);



        dtHeader.Columns.Add("DTGeneralDetails");

        dtQualityDetails = dataaccess.executeStoreProcedureDataTable("spr_TestQualityDetailsWebService", null);

        foreach (DataRow d in dtHeader.Rows)
        {
            d["DTGeneralDetails"] = dtQualityDetails.Rows[0][0].ToString();
        }


        result = GetDataTableToJson(dtHeader);

        result = result.Replace("\\r", "").Replace("\\n", "").Replace("\"[", "[").Replace("]\"", "]").Replace("\\", "").Replace("null", "\"\"");

        return result;

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
                if (col.DataType.Name.Equals("DateTime"))
                {
                    row.Add(col.ColumnName, DateTime.Parse(dr[col].ToString()).ToString("yyyy-MM-dd HH:mm"));
                }
                else if (col.DataType.Name.Equals("Int16") || col.DataType.Name.Equals("Int32"))
                {
                    row.Add(col.ColumnName, String.IsNullOrEmpty(dr[col].ToString()) ? "0" : dr[col].ToString());
                }
                else if (col.DataType.Name.Equals("Decimal"))
                {
                    row.Add(col.ColumnName, String.IsNullOrEmpty(dr[col].ToString()) ? "0.0" : dr[col].ToString());
                }
                else
                {
                    row.Add(col.ColumnName, dr[col].ToString().Trim());
                }

            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public String QualityIboundSend(String QualityHdr)
    {
        bool xmlok = false, jsonok = false;
        XmlSerializer xml = new XmlSerializer(typeof(QualityHeader[]));
        JavaScriptSerializer js = new JavaScriptSerializer();
        QualityHeader[] array = null;
        string result = "";

        if (String.IsNullOrEmpty(QualityHdr) || QualityHdr.Equals("[]"))
        {
            return "No se recibió Dato alguno";
        }

        try
        {
            var reader = new StringReader(QualityHdr);
            array = (QualityHeader[])xml.Deserialize(reader);
            xmlok = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);

        }

        if (!xmlok)
        {
            try
            {
                array = js.Deserialize<QualityHeader[]>(QualityHdr);
                jsonok = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        if (xmlok || jsonok)
        {
            Dictionary<String, object> param = new Dictionary<string, object>();

            foreach (QualityHeader item in array)
            {
                try
                {
                    param = item.toParam();
                    result += dataaccess.executeStoreProcedureString("spr_QualityInboundProvider", param);

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result += "[" + item.txtVFolio + "]  " + ex.Message;
                }
            }
        }
        else
        {
            result += "[" + QualityHdr + "] - La cadena proporcionada no es un objeto con estructura de un XML o de un JSON válido, revise el formato e intente de nuevo a enviar la información.\n Para validar JSON visite: http://json.parser.online.fr/ \n Para validar XML visite: http://www.xmlvalidation.com/. Y compare los parámetros con la plantilla.";
        }


        return result;
    }

}
