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

public partial class Grafica5 : System.Web.UI.Page
{
    protected string topTenData = "";
    protected string fechaI = "";
    protected string fechaF = "";
    protected bool kgs = false;
    protected void Page_Load(object sender, EventArgs e)
    {

        fechaI = "" + Convert.ToString(Request.QueryString["fechaI"]);
        fechaF = "" + Convert.ToString(Request.QueryString["fechaF"]);
        string id = Convert.ToString(Request.QueryString["idPlanta"]);
        kgs = Convert.ToBoolean(Request.QueryString["kgs"]);
        if (fechaI == "")
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            fechaI = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
            fechaF = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
        }
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@Grafica", 4);
        parameters2.Add("@idFarm", id);
        parameters2.Add("@fechaIni", fechaI);
        parameters2.Add("@fechaFin", fechaF);
        parameters2.Add("@kgs", kgs);
       // DataTable dt3 = DataAccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Cajas", parameters2);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_CajasNEW", parameters2);
        topTenData = JsonConvert.SerializeObject(dt3);
        topTenData = topTenData.Replace('"', ' ');
       
    }
}