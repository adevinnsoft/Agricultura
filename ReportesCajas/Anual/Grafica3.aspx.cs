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
    protected void Page_Load(object sender, EventArgs e)
    {
         
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@Grafica", 5);
        DataAccess daccess = new DataAccess();
        DataTable dt = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_Cajas", parameters);
        convertido = JsonConvert.SerializeObject(dt);
        convertido = convertido.Replace('"', ' ');
        convertido = convertido.Replace("null", "0");
    }
}