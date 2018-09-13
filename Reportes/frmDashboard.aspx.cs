using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using log4net;
using System.Text;

public partial class Reportes_frmDashboard : BasePage
{
    private static int idUsuario = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {

        }
        else
        {
            idUsuario = int.Parse(System.Web.HttpContext.Current.Session["userIDInj"].ToString());
        }

    }

    [WebMethod]
    public static string InicioFinSemana(int semana, int anio)
    {
        string result = "";

        try
        {
            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_SemanaNS", new Dictionary<string, object>() { { "@semana", semana }, { "@anio", anio } });

            result = GetDataTableToJson(dt);

            result = result.Replace("\"\\/Date(", "").Replace(")\\/\"", "");

        }
        catch (Exception ex)
        {

            Log.Error(ex.Message);
        }

        return result;
    }

    [WebMethod]
    public static string[] cargarInvernaderos(string nombreLider, string idLider, int idPlanta)
    {
        
        try
        {
            DataAccess da = new DataAccess();
            StringBuilder sb = new StringBuilder();
            Dictionary<String, Object> parametros = new Dictionary<String, Object>();
            parametros.Add("@idPlanta", idPlanta);
            parametros.Add("@idLider", int.Parse(idLider));

            DataTable dt = da.executeStoreProcedureDataTable("spr_ObtieneInvernaderosPorLider", parametros);

            if (dt.Rows.Count <= 0)
            {
                return new string[] { "0", "warning", "No se encontraron invernaderos para el usuario " + nombreLider + "." };
            }
            else
            {
                DataView dw = new DataView(dt);
                var dtInvernadero = dw.ToTable(true, "idInvernadero", "ClaveInvernadero", "idLider");
                int c = 0;

                foreach (DataRow dr in dtInvernadero.Rows)
                {
                    c++;
                    string idInvernadero = dr["idInvernadero"].ToString(),
                           clvInvernadero = dr["ClaveInvernadero"].ToString();

                    sb.AppendLine("<div id=\"divInvernadero" + c + "\" class=\"Invernadero\"  idLider=\"" + idLider + "\" idInvernadero=\"" + idInvernadero + "\" style=\"display:none;\">" + clvInvernadero + "</div>");
                }

                return new string[] { "1", "ok", sb.ToString()};

            }

        }
        catch (Exception x)
        {
            Log.Error(x);
            return new string[] { "0", "error", "Error al obtener los invernaderos" };
        }
    }

    [WebMethod]
    public static string[] cargarCombo(int idPlanta)
    {
        try
        {
            DataAccess da = new DataAccess();
            var dt = da.executeStoreProcedureDataTable("spr_LiderObtenerPorPlanta", new Dictionary<string, object>(){
                {"@idPlanta", idPlanta}
            });

            if (dt.Rows.Count <= 0)
            {
                return new string[] { "0", "warning", "No se encontraron lideres para la planta actual." };
            }
            else
            {
                DataView dw = new DataView(dt);
                var dtLider = dw.ToTable(true, "idLider", "vNombre");
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<option value=\"\" idLider=\"0\">--Seleccione--</option>");

                foreach (DataRow dr in dtLider.Rows)
                {
                    string idLider = dr["idLider"].ToString(), Lider = dr["vNombre"].ToString();
                    sb.AppendLine("<option idLider=\"" + idLider + "\" >" + Lider + "</option>");
                }

                return new string[] { "1", "ok", sb.ToString()};
            }
        }
        catch (Exception x)
        {
            Log.Error(x);
            return new string[] { "0", "error", "Error al obtener los lideres" };
        }
    }



    [WebMethod]
    public static string[] ObtenerPromediosGraficas(int semana, int anio, int idLider, int[] Invernaderos)
    {
        DataAccess da = new DataAccess();
        StringBuilder sb = new StringBuilder();
        Dictionary<String, Object> parametros = new Dictionary<String, Object>();
        DataTable dtInvernaderos = new DataTable();
        string avgsPlan = string.Empty;
        string avgsEjecucion = string.Empty;
        string porcentajeCumplimiento = string.Empty;

        try
        {
            dtInvernaderos.Columns.Add("idInvernadero");
            foreach (int row in Invernaderos)
            {
                DataRow dr = dtInvernaderos.NewRow();
                dr["idInvernadero"] = row;
                dtInvernaderos.Rows.Add(dr);
            }

            parametros.Add("@semana", semana);
            parametros.Add("@anio", anio);
            parametros.Add("@idUsuario", idLider);
            parametros.Add("@invernaderos", dtInvernaderos);

            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerPlanDashboard", parametros);

            foreach (DataRow row in ds.Tables[2].Rows)
            {
                string pPlandomingo = row["PromedioPlanDomingo"].ToString().Trim(),
                 pPlanlunes = row["PromedioPlanLunes"].ToString().Trim(),
                 pPlanmartes = row["PromedioPlanMartes"].ToString().Trim(),
                 pPlanmiercoles = row["PromedioPlanMiercoles"].ToString().Trim(),
                 pPlanjueves = row["PromedioPlanJueves"].ToString().Trim(),
                 pPlanviernes = row["PromedioPlanViernes"].ToString().Trim(),
                 pPlansabado = row["PromedioPlanSabado"].ToString().Trim(),
                 pEjecuciondomingo = row["PromedioEjecucionDomingo"].ToString().Trim(),
                 pEjecucionlunes = row["PromedioEjecucionLunes"].ToString().Trim(),
                 pEjecucionmartes = row["PromedioEjecucionMartes"].ToString().Trim(),
                 pEjecucionmiercoles = row["PromedioEjecucionMiercoles"].ToString().Trim(),
                 pEjecucionjueves = row["PromedioEjecucionJueves"].ToString().Trim(),
                 pEjecucionviernes = row["PromedioEjecucionViernes"].ToString().Trim(),
                 pEjecucionsabado = row["PromedioEjecucionSabado"].ToString().Trim();

                pPlandomingo = pPlandomingo.Equals(string.Empty) ? "0.0" : pPlandomingo.Substring(0, pPlandomingo.Length - 1);
                pPlanlunes = pPlanlunes.Equals(string.Empty) ? "0.0" : pPlanlunes.Substring(0, pPlanlunes.Length - 1);
                pPlanmartes = pPlanmartes.Equals(string.Empty) ? "0.0" : pPlanmartes.Substring(0, pPlanmartes.Length - 1);
                pPlanmiercoles = pPlanmiercoles.Equals(string.Empty) ? "0.0" : pPlanmiercoles.Substring(0, pPlanmiercoles.Length - 1);
                pPlanjueves = pPlanjueves.Equals(string.Empty) ? "0.0" : pPlanjueves.Substring(0, pPlanjueves.Length - 1);
                pPlanviernes = pPlanviernes.Equals(string.Empty) ? "0.0" : pPlanviernes.Substring(0, pPlanviernes.Length - 1);
                pPlansabado = pPlansabado.Equals(string.Empty) ? "0.0" : pPlansabado.Substring(0, pPlansabado.Length - 1);

                pEjecuciondomingo = pEjecuciondomingo.Equals(string.Empty) ? "0.0" : pEjecuciondomingo.Substring(0, pEjecuciondomingo.Length - 1);
                pEjecucionlunes = pEjecucionlunes.Equals(string.Empty) ? "0.0" : pEjecucionlunes.Substring(0, pEjecucionlunes.Length - 1);
                pEjecucionmartes = pEjecucionmartes.Equals(string.Empty) ? "0.0" : pEjecucionmartes.Substring(0, pEjecucionmartes.Length - 1);
                pEjecucionmiercoles = pEjecucionmiercoles.Equals(string.Empty) ? "0.0" : pEjecucionmiercoles.Substring(0, pEjecucionmiercoles.Length - 1);
                pEjecucionjueves = pEjecucionjueves.Equals(string.Empty) ? "0.0" : pEjecucionjueves.Substring(0, pEjecucionjueves.Length - 1);
                pEjecucionviernes = pEjecucionviernes.Equals(string.Empty) ? "0.0" : pEjecucionviernes.Substring(0, pEjecucionviernes.Length - 1);
                pEjecucionsabado = pEjecucionsabado.Equals(string.Empty) ? "0.0" : pEjecucionsabado.Substring(0, pEjecucionsabado.Length - 1);

                avgsPlan = pPlandomingo + " " + pPlanlunes + " " + pPlanmartes + " " + pPlanmiercoles + " " + pPlanjueves + " " + pPlanviernes + " " + pPlansabado;
                avgsEjecucion = pEjecuciondomingo + " " + pEjecucionlunes + " " + pEjecucionmartes + " " + pEjecucionmiercoles + " " + pEjecucionjueves + " " + pEjecucionviernes + " " + pEjecucionsabado;

            }

            //porcentajeCumplimiento = ds.Tables[3].Rows[0]["PorcentajeDeSurcos"].ToString();//Porcentaje de surcos

            if (ds.Tables[2].Rows.Count > 0)
            {
                return new string[] { "1", "", avgsPlan,avgsEjecucion };
            }
            else
            {
                return new string[] { "0", "No hay informacion para este invernadero", "warning" };
            }


        }
        catch (Exception x)
        {
            Log.Error(x);
            return new string[] { "0", "No hay informacion para este invernadero", "warning" };
        }
    }

    [WebMethod]
    public static string[] ObtenerDetalleDashboard(int semana, int anio, int idLider, int[]Invernaderos)
    {
        DataAccess da = new DataAccess();
        StringBuilder sb = new StringBuilder();
        Dictionary<String, Object> parametros = new Dictionary<String, Object>();
        DataTable dtInvernaderos = new DataTable();
        string avgsPlan = string.Empty;
        string avgsEjecucion = string.Empty;
        int porcentajeCumplimiento = 0;
        int actCumplidas = 0;
        try
        {
             dtInvernaderos.Columns.Add("idInvernadero");

            foreach (int row in Invernaderos)
            {
                DataRow dr = dtInvernaderos.NewRow();
                dr["idInvernadero"] = row;
                dtInvernaderos.Rows.Add(dr);
            }

            parametros.Add("@semana", semana);
            parametros.Add("@anio", anio);
            parametros.Add("@idUsuario", idLider);
            parametros.Add("@invernaderos", dtInvernaderos);

            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerPlanDashboard", parametros);
            bool SemanaCorrecta;
            foreach (DataRow row in ds.Tables[1].Rows)
            {

                string idInvernadero = row["idInvernadero"].ToString().Trim(),
                       invernadero = row["Invernadero"].ToString().Trim(),
                       habilidad = row["NombreHabilidad"].ToString().Trim(),
                       nombreCorto = row["NombreCorto"].ToString().Trim(),
                       Plandomingo = row["PlanDomingo"].ToString().Trim(),
                       Planlunes = row["PlanLunes"].ToString().Trim(),
                       Planmartes = row["PlanMartes"].ToString().Trim(),
                       Planmiercoles = row["PlanMiercoles"].ToString().Trim(),
                       Planjueves = row["PlanJueves"].ToString().Trim(),
                       Planviernes = row["PlanViernes"].ToString().Trim(),
                       Plansabado = row["PlanSabado"].ToString().Trim(),
                       Ejecuciondomingo = row["EjecucionDomingo"].ToString().Trim(),
                       Ejecucionlunes = row["EjecucionLunes"].ToString().Trim(),
                       Ejecucionmartes = row["EjecucionMartes"].ToString().Trim(),
                       Ejecucionmiercoles = row["EjecucionMiercoles"].ToString().Trim(),
                       Ejecucionjueves = row["EjecucionJueves"].ToString().Trim(),
                       Ejecucionviernes = row["EjecucionViernes"].ToString().Trim(),
                       Ejecucionsabado = row["EjecucionSabado"].ToString().Trim();
                
                SemanaCorrecta = Plandomingo.Equals(Ejecuciondomingo) && Planlunes.Equals(Ejecucionlunes) && Planmartes.Equals(Ejecucionmartes) && Planmiercoles.Equals(Ejecucionmiercoles) && Planjueves.Equals(Ejecucionjueves) && Planviernes.Equals(Ejecucionviernes) && Plansabado.Equals(Ejecucionsabado) ;
                
                sb.AppendLine("<div invernadero=\"" + invernadero + "\" class=\"invernaderosTabulares\">");
                sb.AppendLine("<table border=\"1\" id=\"tbl" + nombreCorto + "\" class=\"gridView\">");
                sb.AppendLine("<h2 class=\"invernaderoTabularTitulo\"> <img class=\"imgCumplimiento\" "+(SemanaCorrecta ? "src=\"../comun/img/ok.png\"" : "src=\"../comun/img/error.png\"")+" /> " + invernadero + "</h2>");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");
                sb.AppendLine(nombreCorto.Equals(string.Empty) ? string.Empty :"<th><span><label>" + nombreCorto + "</label></span></th>");
                sb.AppendLine("<th><span><label>Domingo</label></span></th>");
                sb.AppendLine("<th><span><label>Lunes</label></span></th>");
                sb.AppendLine("<th><span><label>Martes</label></span></th>");
                sb.AppendLine("<th><span><label>Miercoles</label></span></th>");
                sb.AppendLine("<th><span><label>Jueves</label></span></th>");
                sb.AppendLine("<th><span><label>Viernes</label></span></th>");
                sb.AppendLine("<th><span><label>Sabado</label></span></th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td><span><label>Plan</label></span></td>");
                sb.AppendLine(Plandomingo.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Plandomingo + "</label></span></td>");
                sb.AppendLine(Planlunes.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Planlunes + "</label></span></td>");
                sb.AppendLine(Planmartes.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Planmartes  + "</label></span></td>");
                sb.AppendLine(Planmiercoles.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Planmiercoles  + "</label></span></td>");
                sb.AppendLine(Planjueves.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Planjueves + "</label></span></td>");
                sb.AppendLine(Planviernes.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Planviernes + "</label></span></td>");
                sb.AppendLine(Plansabado.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Plansabado + "</label></span></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td><span><label>Ejecución</label></span></td> ");
                sb.AppendLine(Ejecuciondomingo.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecuciondomingo + "</label></span></td>");
                sb.AppendLine(Ejecucionlunes.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecucionlunes + "</label></span></td>");
                sb.AppendLine(Ejecucionmartes.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecucionmartes + "</label></span></td>");
                sb.AppendLine(Ejecucionmiercoles.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecucionmiercoles + "</label></span></td>");
                sb.AppendLine(Ejecucionjueves.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecucionjueves  + "</label></span></td>");
                sb.AppendLine(Ejecucionviernes.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecucionviernes  + "</label></span></td>");
                sb.AppendLine(Ejecucionsabado.Equals(string.Empty) ? "<td><span><label>-</label></span></td>" : "<td><span><label>" + Ejecucionsabado  + "</label></span></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</tbody>");

                if (Plandomingo.Equals(Ejecuciondomingo) && Planlunes.Equals(Ejecucionlunes) && Planmartes.Equals(Ejecucionmartes) && Planmiercoles.Equals(Ejecucionmiercoles) && Planjueves.Equals(Ejecucionjueves) && Planviernes.Equals(Ejecucionviernes) && Plansabado.Equals(Ejecucionsabado))
                {
                    actCumplidas++;
                }

                porcentajeCumplimiento = (actCumplidas * 100) / ds.Tables[1].Rows.Count;
                
                sb.AppendLine("</div>");

            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                return new string[] { "1", "", sb.ToString(),porcentajeCumplimiento.ToString() };
            }
            else
            {
                return new string[] { "0", "no hay detalles para el invernadero seleccionado", "warning" };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            return new string[] { "0", "Error:" + " " + ex.Message, "warning" };
        }
       
    }


    [WebMethod]
    public static string[] ObtenerPlanDashboard(int semana,int anio,int idLider,int[] Invernaderos)
    {
        string JSON = "[";
        idUsuario = int.Parse(System.Web.HttpContext.Current.Session["userIDInj"].ToString());
        DataAccess da = new DataAccess();
        StringBuilder sb = new StringBuilder();
        Dictionary<String, Object> parametros = new Dictionary<String, Object>();
        DataTable dtInvernaderos = new DataTable();

        try 
        {
            
            dtInvernaderos.Columns.Add("idInvernadero");

            foreach (int row in Invernaderos)
            {
                DataRow dr = dtInvernaderos.NewRow();
                dr["idInvernadero"] = row;  
                dtInvernaderos.Rows.Add(dr);
            }

            parametros.Add("@semana", semana);
            parametros.Add("@anio", anio);
            parametros.Add("@idUsuario", idLider);
            parametros.Add("@invernaderos", dtInvernaderos);

    
            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerPlanDashboard",parametros);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                JSON += "{\"idLider\":\"" + idLider.ToString() + "\"" +
                        ",\"semana\":\"" + semana.ToString() + "\"" +
                        ",\"anio\":\"" + anio.ToString() + "\"" +
                        ",\"idInvernadero\":\"" + row["idInvernadero"].ToString() + "\"" +
                         ",\"Plan\": {\"invernadero\": \"" + row["Invernadero"].ToString() + "\"" +
                                     ",\"nombreHabilidad\":\"" + row["nombreHabilidad"].ToString() + "\"" +
                                     ",\"producto\":\"" + row["Producto"].ToString() + "\"" +
                                     ",\"variedad\":\"" + row["Variedad"].ToString() + "\"" +
                                     ",\"inicio\":\"" + DateTime.Parse(row["Inicio"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\"" +
                                     ",\"fin\":\"" + DateTime.Parse(row["Fin"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\"" +
                                     ",\"etapa\":\"" + row["Etapa"].ToString() + "\"" +
                                     ",\"color\":\"" + row["color"].ToString() + "\"" + 
                                     "}"+
                       
                        
                         "},";

   
                        
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                
                JSON = JSON.Substring(0, JSON.Length - 1) + "]";
                return new string[] { "1", "", JSON.ToString()};
            }
            else
            {
                return new string[] { "0", "No se encontraron detalles para los invernaderos seleccionados", "warning" };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            return new string[] {"0","Error:" + " " +ex.Message,"warning" };
            
        }
    }
    

    [WebMethod]
    public static string[] ObtenerEjecucionDashboard(int semana, int anio, int idLider, int[] Invernaderos)
    {
        string JSON = "[";
        idUsuario = int.Parse(System.Web.HttpContext.Current.Session["userIDInj"].ToString());

        try
        {
            Dictionary<String, Object> parametros = new Dictionary<String, Object>();
            DataTable dtInvernaderos = new DataTable();

            dtInvernaderos.Columns.Add("idInvernadero");

            foreach (int row in Invernaderos)
            {
                DataRow dr = dtInvernaderos.NewRow();
                dr["idInvernadero"] = row;
                dtInvernaderos.Rows.Add(dr);
            }

            parametros.Add("@semana", semana);
            parametros.Add("@anio", anio);
            parametros.Add("@idUsuario", idLider);
            parametros.Add("@invernaderos", dtInvernaderos);

            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtenerEjecucionDashboard", parametros);

            foreach (DataRow row in dt.Rows)
            {
                JSON += "{\"idLider\":\"" + idLider.ToString() + "\"" +
                        ",\"semana\":\"" + semana.ToString() + "\"" +
                        ",\"anio\":\"" + anio.ToString() + "\"" +
                        ",\"idInvernadero\":\"" + row["idInvernadero"].ToString() + "\"" +
                        ",\"Ejecucion\": {\"invernadero\": \"" + row["Invernadero"].ToString() + "\"" +
                                  ",\"nombreHabilidad\":\"" + row["nombreHabilidad"].ToString() + "\"" +
                                  ",\"producto\":\"" + row["Producto"].ToString() + "\"" +
                                  ",\"variedad\":\"" + row["Variedad"].ToString() + "\"" +
                                  ",\"inicio\":\"" + DateTime.Parse(row["Inicio"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\"" +
                                  ",\"fin\":\"" + DateTime.Parse(row["Fin"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\"" +
                                  ",\"etapa\":\"" + row["Etapa"].ToString() + "\"" +
                                  ",\"color\":\"" + row["color"].ToString() + "\"" +
                                  "}" +
                      "},";

            }

            if (dt.Rows.Count > 0)
            {
                JSON = JSON.Substring(0, JSON.Length - 1) + "]";
                return new string[] { "1", "ok", JSON.ToString() };
            }
            else
            {
                return new string[] { "0", "No se encontraron tareas de ejecución", "warning" };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            return new string[] { "0", "Error:" + " " + ex.Message, "warning" };

        }
    }
    
}