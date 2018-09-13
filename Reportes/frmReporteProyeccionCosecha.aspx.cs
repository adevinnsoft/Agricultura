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
    public static string[] ObtenerReporte(string Dia, string idPlanta, decimal pesocaja, decimal equivalente)
    {
        var idioma = HttpContext.Current.Session["Locale"].ToString();
        //string idUsuario = HttpContext.Current.Session["userIDInj"].ToString();
        DataAccess da = new DataAccess();
        DataSet ds = new DataSet();

        idPlanta = idPlanta == "0" ? HttpContext.Current.Session["idPlanta"].ToString() : idPlanta.ToString();

        Dictionary<string, object> param = new Dictionary<string, object>();
        //StringBuilder sb = new StringBuilder();
        //StringBuilder sb1 = new StringBuilder();
        //StringBuilder sb2 = new StringBuilder();

        try
        {
            param.Add("@fecha", Dia);
            param.Add("@idPlanta",idPlanta);
            param.Add("@lbsCaja", pesocaja);
            param.Add("@equivalente", equivalente);
            ds = da.executeStoreProcedureDataSet("spr_rpt_proyeccionV2", param);


            DataTable dt = ds.Tables[0];
          
            if (ds.Tables[0].Rows.Count > 0)
            {
                return new string[] { "ok", JsonConvert.SerializeObject(ds.Tables[0]) };
            }
            else
            {
                return new string[] { "info", (string)HttpContext.GetLocalResourceObject("~/Reportes/frmReporteProyeccionCosecha.aspx", "NoCosechas", System.Globalization.CultureInfo.GetCultureInfo(idioma)) };
               
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "info", (string)HttpContext.GetLocalResourceObject("~/Reportes/frmReporteProyeccionCosecha.aspx", "NoCosechas", System.Globalization.CultureInfo.GetCultureInfo(idioma)) };

            return new string[] { "error", (string)HttpContext.GetLocalResourceObject("~/Reportes/frmReporteProyeccionCosecha.aspx", "ErrorAdmin", System.Globalization.CultureInfo.GetCultureInfo(idioma)) };
        }
    }
    
}