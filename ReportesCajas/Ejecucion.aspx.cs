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
    protected void Page_Load(object sender, EventArgs e)
    {

        //string fechaI = Convert.ToString(Request.QueryString["fechaI"]);
        //string fechaF = Convert.ToString(Request.QueryString["fechaF"]);
        //if (fechaI == null)
        //{
        //    String sDate = DateTime.Now.ToString();
        //    DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
        //    string dd = datevalue.Day.ToString();
        //    string mm = datevalue.Month.ToString();
        //    string yy = datevalue.Year.ToString();
        //    fechaI = yy + "/" + mm + "/" + dd;
        //    fechaF = yy + "/" + mm + "/" + dd;
        //}
        int idPlanta = Convert.ToInt32(Request.QueryString["idPlanta"]);
        int idLider = Convert.ToInt32(Request.QueryString["idLider"]);
        int Semana = Convert.ToInt32(Request.QueryString["Semana"]);
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion",4);
        parameters2.Add("@idLider", idLider);
        parameters2.Add("@Semana", Semana);
        parameters2.Add("@idFarm", idPlanta);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme_EjecucionCumplimiento", parameters2);

        pivotData = JsonConvert.SerializeObject(dt3);
        pivotData = pivotData.Replace('"', ' '); 
    }
}