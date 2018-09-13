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
    protected string Datax = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        int SI = Convert.ToInt32(Request.QueryString["SemanaInicio"]);
        int SF = Convert.ToInt32(Request.QueryString["SemanaFin"]);
        int YR = Convert.ToInt32(Request.QueryString["Anio"]);

        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@SemanaInicio",SI);
        parameters2.Add("@SemanaFin", SF);
        parameters2.Add("@Anio", YR);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_GetCumplimientoyEjecucionV2", parameters2);

        Datax = JsonConvert.SerializeObject(dt3);
      //  Datax = Datax.Replace('"', ' '); 
    }
}