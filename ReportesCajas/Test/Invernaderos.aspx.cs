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
    protected string tipo = "";
    protected string FarmName = "";
    protected string colors = "";
    protected bool kgs = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        fechaI = "" + Convert.ToString(Request.QueryString["fechaI"]);
        fechaF = "" + Convert.ToString(Request.QueryString["fechaF"]);
        tipo = "" + Convert.ToString(Request.QueryString["tipo"]);
        kgs =  Convert.ToBoolean(Request.QueryString["kgs"]);
        string id = Convert.ToString(Request.QueryString["idPlanta"]);
        if (fechaI == "")
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            fechaI = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
            fechaF = datevalue.Year.ToString() + "/" + datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
        }
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion", 2);
        parameters2.Add("@fechaIni", fechaI);
        parameters2.Add("@fechaFin", fechaF);
        parameters2.Add("@tipo", tipo);
        parameters2.Add("@idFarm", id);
        parameters2.Add("@kgs", kgs);
        DataAccess dacces = new DataAccess();
        DataTable dt3 = dacces.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Arribo2", parameters2);
        topTenData = JsonConvert.SerializeObject(dt3);
        if (tipo == "Estimado" || tipo == "Cosecha" || tipo == "Arribo" || tipo == "Merma")
        {
            tipo = "{ valueField: 'Estimado', name: 'Estimado', color:'#01DFD7'}," +
                   "{ valueField: 'Cosecha', name: 'Cosecha', color:'#045FB4'}," +
                   "{ valueField: 'CosechaVerde', name: 'Cosecha Verde', color:'#01DF01'}," +
                   "{ valueField: 'CosechaMerma', name: 'Merma',color:'#FF0000'}";
        } 
        

        
   
     
        Dictionary<string, object> parametersx = new System.Collections.Generic.Dictionary<string, object>();
        parametersx.Add("@funcion", 3);
        parametersx.Add("@idFarm", id);
        parametersx.Add("@kgs", kgs);
        DataTable dt34 = dacces.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Arribo2", parametersx);
        FarmName= dt34.Rows[0]["Name"].ToString();
    }
}