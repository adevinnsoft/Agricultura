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
using Newtonsoft.Json;


public partial class frmReporteCumplimientoInvCosechados : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmReporteCumplimientoInvCosechados));
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod]
    public static string[] ObtenerReporte(string inicio, string fin, int idPlanta)
    {
        string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
        DataAccess da = new DataAccess("EmpaqueConn");
        DataSet ds = new DataSet();

        Dictionary<string, object> param = new Dictionary<string, object>();
        

        try
        {
            param.Add("@idPlanta",  HttpContext.Current.Session["idPlanta"].ToString());
            param.Add("@inicio",inicio);
            param.Add("@fin", fin);
            ds = da.executeStoreProcedureDataSet("spr_ReporteOnHold", param);


            DataTable dt = ds.Tables[0];

         
            if (ds.Tables[0].Rows.Count > 0)
            {
                return new string[] { "ok", JsonConvert.SerializeObject(ds.Tables[0]), JsonConvert.SerializeObject(ds.Tables[1]), JsonConvert.SerializeObject(ds.Tables[2]) };
            }
            else
            {
                return new string[] { "info", "No existen folios en onhold en el rango de fecha especificado." };
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "error", "Hubo un error al obtener el reporte: " + ex.Message };
        }
    }

   

}