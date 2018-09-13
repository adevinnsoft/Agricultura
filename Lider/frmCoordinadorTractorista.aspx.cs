using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;
using log4net;
using System.Globalization;

public partial class Lider_frmCoordinadorTractorista : BasePage
{
    bool ban = false;
    int cont = 0;
    public string sb5;
    private static readonly ILog log = LogManager.GetLogger(typeof(Lider_frmCoordinadorTractorista));
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    //public static string tablaStockPlantas()
    public static string llenatablaFormaA()
    {

        try
        {
            DateTime d;
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
            DataAccess dataaccess = new DataAccess();
            //DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneStatusFormasACoordinador", new Dictionary<string, object>() { });
            DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_ObtieneStatusFormasACoordinador", new Dictionary<string, object>() { });
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            string[] sb3 = new string[1];
            string sb4;
            int min, max;

            foreach (DataRow P in dt.Rows)
            {
                //sb.AppendLine("<tr solohora=\"" + P["hora"] + "\"><td>" + P["ClaveInvernadero"] + "</td><td>" + P["vNombre"] + "</td><td>" + P["folio"] + "</td><td>");

                sb4 = Convert.ToString(P["fechaInicio"]);

                //string a = "2015/01/21 09:37 pm";
                //convertT
                d = Convert.ToDateTime(sb4);


                //DateTime oldDate = new DateTime(2016, 01, 21, 16, 5, 7, 123);
                DateTime newDate = DateTime.Now;
                TimeSpan ts2 = newDate - d;
                int differenceInHours = ts2.Hours;
                int convhrs = differenceInHours * 60;
                int differenceInMins = ts2.Minutes;
                differenceInMins = differenceInMins + convhrs;     //
                sb.AppendLine("<tr solohora=\"" + P["hora"] + "\" DiferenciaMins=\"" + differenceInMins + "\"><td>" + P["ClaveInvernadero"] + "</td><td>" + P["vNombre"] + "</td><td>" + P["folio"] + "</td>");
                //sb.AppendLine("" + differenceInMins + "</td><td>" + d + "</td><td>" + newDate + "</td></tr>");//P["Cerrado"] + "</td><td></tr>");
                ////bool ban = false;
                foreach (DataRow P2 in dt2.Rows)
                {
                    min = Convert.ToInt32(P2["MinutoInicio"]);
                    max = Convert.ToInt32(P2["MinutoFin"]);
                    if ((differenceInMins >= min) && (differenceInMins <= max) && P["idVariedad"].ToString() == P2["idVariedad"].ToString())
                    {
                        sb.AppendLine("<td>" + P["fechaInicio"] + "</td><td DiferenciaMins='" + differenceInMins + "'><input type='text' readonly style='background-color:#" + P2["Color"] + ";' name='nombredelacaja'/><label>" + differenceInMins + " mins</label></td><td>" + ((Convert.ToBoolean(P["Cerrado"]) == false) ? "Abierto" : "Cerrado") + "</td><td>" + P["Variety"] + "</td>");
                        ////ban = true;
                    }

                    // + differenceInMins + "</td><td>" + P["Cerrado"] + "</td><td></tr>");
                    //<input type="text" name="nombredelacaja">
                }

                ////if (!ban)
                ////{
                ////    sb.AppendLine("<td>" + P["fechaInicio"] + "</td><td DiferenciaMins='" + differenceInMins + "'><input type='text' readonly style='background-color:red;' name='nombredelacaja'/><label>" + differenceInMins + " mins</label></td><td>" + ((Convert.ToBoolean(P["Cerrado"]) == false) ? "Abierto" : "Cerrado") + "</td><td>" + P["Variety"] + "</td>");
                ////}
            }

            sb.AppendLine("</tr>");

            return sb.ToString();
        }
        catch (Exception e)
        {
            log.Error(e.Message);

        }
        return "";
    }
}