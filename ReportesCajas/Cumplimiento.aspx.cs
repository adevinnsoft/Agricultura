using Newtonsoft.Json;
using System.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Default : System.Web.UI.Page 
{
    public string Datax = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion", 6);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_EjecucionCumplimiento", parameters2);

        Datax = JsonConvert.SerializeObject(dt3);
        Datax = Datax.Replace('"', ' '); 
     }
}
