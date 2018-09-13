using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;


public partial class frmReporteResumenCosecha : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmReporteResumenCosecha));


    private static string STR_TD = "td";
    private static string STR_TH = "th";
    private static string STR_TR = "tr";
    private static string STR_THEAD = "thead";
    private static string STR_TBODY = "tbody";
    private static string STR_TABLE = "table";

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] obtenerReporte(string diaInicio, string diaFin, string idPlanta) {

        StringBuilder response = new StringBuilder();
        try
        {
            int lang = HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0;
            DataAccess dataaccess = new DataAccess();

            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_GetResumenCosechaInvernaderos"
                , new Dictionary<string, object>() {
                     
                     { "@idPlanta",          (idPlanta) }
                    ,{ "@fechaInicio",  diaInicio}
                    , { "@fechaFin",     diaFin}
                    , { "@lengua",     lang}
                   });

           

            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sbTable = new StringBuilder();
                StringBuilder sbHeader = new StringBuilder();
                StringBuilder sbBody = new StringBuilder();
                //auxiliares
                StringBuilder sbCell = new StringBuilder();
                StringBuilder sbRow = new StringBuilder();
                //generando theader
                foreach(DataColumn col in ds.Tables[0].Columns){
                    sbCell.Append(tagGen(STR_TH, col.ColumnName));
                }
                sbRow.Append(tagGen(STR_TR, sbCell.ToString()));
                sbTable.Append(tagGen(STR_THEAD, sbRow.ToString()));
                
                //generando tbody
                sbRow =new StringBuilder();
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //recorriendo tr's
                    sbCell = new StringBuilder();
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        sbCell.Append(tagGen(STR_TD, item[col].ToString()));
                    }
                    sbRow.Append(tagGen(STR_TR, sbCell.ToString()));
                }
                sbTable.Append(tagGen(STR_TBODY, sbRow.ToString()));
                //response.Append(tagGen(STR_TABLE, sbTable.ToString()));
                response.Append(sbTable.ToString());


                return new string[] { "1", "ok", response.ToString() };
            }
            else
            {
                return new string[] { "0", "warning", "El proceso no generó ningún resultado" };
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
        }
    }


 [WebMethod]
    public static string[] obtenerPlantasDdl()
    {
        StringBuilder response = new StringBuilder();

        try
        {
            string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
       
            DataAccess da = new DataAccess();
            DataTable dt = da.executeStoreProcedureDataTable("spr_ObtienePlantasDdl", new Dictionary<string,Object>{{"@idUsuario", idUsuario}});
            if (dt.Rows.Count > 0)
            {

                response.Append(tagGen("option", "value=\"0\"", "-- Todas --"));
                foreach(DataRow row in dt.Rows){
                    response.Append(tagGen("option","value=\"" + row["idPlanta"].ToString() + "\"",row["NombrePlanta"].ToString()));
                }
                return new string[] { "1", "ok", response.ToString() };
            }
            else
            {
                return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
            }
        }
        catch (Exception)
        {
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };

        }

    }

    private static String tagGen(string tag, string attr, string value)
    {
        return String.Format("<{0} {1}>{2}</{0}>", tag, attr, value);
    }

    private static String tagGen(string tag, string value)
    {
        return tagGen(tag, string.Empty, value);
    }

}