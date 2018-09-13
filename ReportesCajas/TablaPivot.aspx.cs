using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TablaPivot : System.Web.UI.Page
{
    protected string pivotData = "";
    protected bool kgs = false;
    protected void Page_Load(object sender, EventArgs e)
    {

        string fechaI = Convert.ToString(Request.QueryString["fechaI"]);
        string fechaF = Convert.ToString(Request.QueryString["fechaF"]);
        
        kgs = Convert.ToBoolean(Request.QueryString["kgs"]);
        if (fechaI == null)
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            string dd = datevalue.Day.ToString();
            string mm = datevalue.Month.ToString();
            string yy = datevalue.Year.ToString();
            fechaI = yy + "/" + mm + "/" + dd;
            fechaF = yy + "/" + mm + "/" + dd;
        }
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@Grafica", 2);
        parameters2.Add("@FechaIni", fechaI);
        parameters2.Add("@FechaFin", fechaF);
        parameters2.Add("@kgs", kgs);
      //  DataTable dt3 = DataAccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Cajas", parameters2);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_CajasNEW", parameters2);
        pivotData = JsonConvert.SerializeObject(dt3);
        pivotData = pivotData.Replace('"', ' '); 



    }
}