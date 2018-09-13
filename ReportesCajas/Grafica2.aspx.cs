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

public partial class Default2 : System.Web.UI.Page
{
    protected string dos = "";
    protected string NombrePlanta = "";
    protected string fechaI = "";
    protected string fechaF = "";
    protected bool kgs = false;
    protected void Page_Load(object sender, EventArgs e)
    {
  
        string id = Convert.ToString(Request.QueryString["idPlanta"]);
        //id = "8";
        NombrePlanta = "\""+Convert.ToString(Request.QueryString["Name"])+"\"";

        fechaI = Convert.ToString(Request.QueryString["fechaI"]);
        fechaF = Convert.ToString(Request.QueryString["fechaFin"]);
        kgs = Convert.ToBoolean(Request.QueryString["kgs"]);
        
        if (fechaI == null)
        {
            DateTime datevalue = (Convert.ToDateTime(DateTime.Now.ToString()));
            string dd = datevalue.Day.ToString();
            string mm = datevalue.Month.ToString();
            string yy = datevalue.Year.ToString();
            fechaI = yy + "/" + mm + "/" + dd;
            fechaF = yy + "/" + mm + "/" + dd;
        }
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@Grafica", 3);
        parameters2.Add("@idFarm", id);
        parameters2.Add("@fechaIni", fechaI);
        parameters2.Add("@fechaFin", fechaF);
        parameters2.Add("@kgs", kgs);
       // DataTable dt2 = DataAccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Cajas", parameters2);
        DataAccess daccess = new DataAccess();
        DataTable dt2 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_CajasNEW", parameters2);
        dos = JsonConvert.SerializeObject(dt2);
        dos = dos.Replace('"', ' ');
    
    }
}