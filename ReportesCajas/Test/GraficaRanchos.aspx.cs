using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportesCajas_Test_GraficaRanchos : System.Web.UI.Page
{
    protected string ranchos = "";
    protected string fechaI = "";
    protected string fechaF = "";
    protected bool kgs = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        fechaI = "" + Convert.ToString(Request.QueryString["fechaI"]);
        fechaF = "" + Convert.ToString(Request.QueryString["fechaF"]);
        kgs = Convert.ToBoolean(Request.QueryString["kgs"]);
        string id = Convert.ToString(Request.QueryString["idPlanta"]);
        if (fechaI == "")
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            fechaI = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
            fechaF = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
        }
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion", 1);
        parameters2.Add("@fechaIni", fechaI);
        parameters2.Add("@fechaFin", fechaF);
        parameters2.Add("@kgs", kgs);
        DataAccess dacces = new DataAccess();
        DataTable dt3 = dacces.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Arribo2", parameters2);
        ranchos = JsonConvert.SerializeObject(dt3);
        ranchos = ranchos.Replace('"', ' ');

    }
}