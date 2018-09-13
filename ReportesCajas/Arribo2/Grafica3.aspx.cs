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

public partial class Grafica3 : System.Web.UI.Page
{
    protected string convertido = "";
    protected string fechaI = "";
    protected string fechaF = "";
    protected void Page_Load(object sender, EventArgs e)
    {
          fechaI = "" + Convert.ToString(Request.QueryString["fechaI"]);
          fechaF = "" + Convert.ToString(Request.QueryString["fechaF"]);

          if (fechaI == "")
          {
              String sDate = DateTime.Now.ToString();
              DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
              fechaI = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
              fechaF = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
          }
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@Grafica", 6);
        parameters.Add("@fechaIni", fechaI);
        parameters.Add("@fechaFin", fechaF);
        //DataTable dt = DataAccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Cajas", parameters);
        DataAccess daccess = new DataAccess();
        DataTable dt = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Cajas", parameters);

        //fechaI = "'" + fechaI + "'";
        //fechaF = "'" + fechaF + "'";
        convertido = JsonConvert.SerializeObject(dt);
        //convertido = convertido.Replace('"', ' ');
        //convertido = convertido.Replace("null", "0");
    }
}