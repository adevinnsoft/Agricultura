using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

public partial class Jornales_frmDirectrizPreparacionDeSuelo : BasePage//System.Web.UI.Page
{
    DataAccess da = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder response = new StringBuilder();
        
    }
    [WebMethod(EnableSession = true)]
    public static string HabilidadesAcomodo(int planta)
    {
        StringBuilder response = new StringBuilder();
        int cantidadfilas = 0;
        DataTable pr = (DataTable)HttpContext.Current.Session["AllHabilidades"];
        response.Append("<tr>");
        foreach (DataRow item in pr.Select("IdPlanta=" + planta))
        {
            if (cantidadfilas <= 2)
            {                                                                                                                                                                            
                response.Append("<td><label idHabilidad='" + item["idHabilidad"] + "' idEtapa='" + item["idEtapa"] + "' idNivel='" + item["idNivel"] + "' for='male'> " + item["NombreHabilidad"] + "-" + item["NombreEtapa"] + "-Nivel " + item["idNivel"] + "</label></td><td><input class='floatValidate' type='text' name='lname'></td>");
                
                cantidadfilas = cantidadfilas + 1;
            }
            else
            {
                cantidadfilas = 0;
                response.Append("<td><label idHabilidad='" + item["idHabilidad"] + "' idEtapa='" + item["idEtapa"] + "' idNivel='" + item["idNivel"] + "' for='male'> " + item["NombreHabilidad"] + "-" + item["NombreEtapa"] + "-Nivel " + item["idNivel"] + "</label></td><td><input class='floatValidate' type='text' name='lname'></td>");
                response.Append("</tr><tr>");
            }
        }
        return response.ToString();
    }
    [WebMethod(EnableSession = true)]
    public static string WMHabiidades(int planta)
    {
        DataAccess da = new DataAccess();
        StringBuilder response = new StringBuilder();          
        int cantidadfilas = 0;
        try
        {
            response.Append("<tr>");
            DataTable dt = da.executeStoreProcedureDataTable("spr_HabilidadesPreparacionSuelo",  new Dictionary<string, object>());
            HttpContext.Current.Session["AllHabilidades"] = dt;
            foreach (DataRow item in dt.Select("IdPlanta=" + planta))
            {
                if (cantidadfilas <= 2)
                {
                    response.Append("<td><label idHabilidad='" + item["idHabilidad"] + "' idEtapa='" + item["idEtapa"] + "' idNivel='" + item["idNivel"] + "' for='male'> " + item["NombreHabilidad"] + "-" + item["NombreEtapa"] + "-Nivel " + item["idNivel"] + "</label></td><td><input class='floatValidate' type='text' name='lname'></td>");
                    
                    cantidadfilas = cantidadfilas + 1;
                }
                else
                {
                    cantidadfilas = 0;
                    response.Append("<td><label idHabilidad='" + item["idHabilidad"] + "' idEtapa='" + item["idEtapa"] + "' idNivel='" + item["idNivel"] + "' for='male'> " + item["NombreHabilidad"] + "-" + item["NombreEtapa"] + "-Nivel " + item["idNivel"] + "</label></td><td><input class='floatValidate' type='text' name='lname'></td>");
                    response.Append("</tr><tr>");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
        }
        return response.ToString();
    }
       [WebMethod]
    public static string WMVariedad()
    {
        DataAccess da = new DataAccess();
        StringBuilder response = new StringBuilder();           //<br>
        int cantidadfilas = 0;
        try
        {
            response.Append("<tr>");
            foreach (DataRow item in da.executeStoreProcedureDataTable("spr_Variedad", new Dictionary<string, object>()).Rows)
            {
                
                
                if (cantidadfilas <= 3)
                {
                    response.Append("<td><input type='checkbox' name='" + item["idVariedad"] + "' idVariedad='" + item["idVariedad"] + "' value='Bike'> " + item["CodigoVariedad"] + "</td>");
                    cantidadfilas = cantidadfilas + 1;
                }
                else { cantidadfilas = 0;
                response.Append("<td><input type='checkbox' name='" + item["idVariedad"] + "' idVariedad='" + item["idVariedad"] + "' value='Bike'> " + item["CodigoVariedad"] + "</td>");
                response.Append("</tr><tr>");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");

        }

        return response.ToString();
    }
       [WebMethod]
    public static string WMVariable()
       {
           DataAccess da = new DataAccess();
           StringBuilder response = new StringBuilder();           //<br>
           int cantidadfilas = 0;
           try
           {
               response.Append("<tr>");
               foreach (DataRow item in da.executeStoreProcedureDataTable("spr_Variable", new Dictionary<string, object>()).Rows)
               {

                   
                   if (cantidadfilas <= 6)
                   {
                       response.Append("<td><input type='checkbox' name='" + item["idVariable"] + "' idVariable='" + item["idVariable"] + "' value='Bike'> " + item["CodigoVariable"] + "</td>");
                       cantidadfilas = cantidadfilas + 1;
                   }
                   else
                   {
                       cantidadfilas = 0;
                       response.Append("<td><input type='checkbox' name='" + item["idVariable"] + "' idVariable='" + item["idVariable"] + "' value='Bike'> " + item["CodigoVariable"] + "</td>");
                       response.Append("</tr><tr>");
                   }
               }
           }
           catch (Exception ex)
           {
               Log.Error(ex);
               response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
               //return response.ToString();
           }

           return response.ToString();
       }
     [WebMethod]
     public static string WMTemporal()
     {
         DataAccess da = new DataAccess();
         StringBuilder response = new StringBuilder();           //<br>
         int cantidadfilas = 0;
         try
         {
             response.Append("<tr>");
             foreach (DataRow item in da.executeStoreProcedureDataTable("spr_Temporal", new Dictionary<string, object>()).Rows)
             {

                 //for (int numfilas = 1; numfilas <= cantidadfilas; numfilas++)
                 if (cantidadfilas <= 1)
                 {
                     response.Append("<td><input type='checkbox' name='" + item["idTemporal"] + "' idTemporal='" + item["idTemporal"] + "' value='Bike'> " + item["Temporal"] + "</td>");
                     cantidadfilas = cantidadfilas + 1;
                 }
                 else
                 {
                     cantidadfilas = 0;
                     response.Append("<td><input type='checkbox' name='" + item["idTemporal"] + "' idTemporal='" + item["idTemporal"] + "' value='Bike'> " + item["Temporal"] + "</td>");
                     response.Append("</tr><tr>");
                 }
             }
         }
         catch (Exception ex)
         {
             Log.Error(ex);
             response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
             //return response.ToString();
         }

         return response.ToString();
     }
    [WebMethod]
     public static string ConsultarDirectriz(string idVariedad, string idVariable, string idTemporal, int idplanta)
    {
        Dictionary<string, object> parametros = new Dictionary<string, object>();
       
        parametros.Add("@idVariedad", idVariedad);
        parametros.Add("@idVariable", idVariable);
        parametros.Add("@idTemporal", idTemporal);
        parametros.Add("@idplanta", idplanta);
        DataAccess da = new DataAccess();
        StringBuilder response = new StringBuilder();          
        int cantidadfilas = 0;
        try
        {
            DataSet ds = da.executeStoreProcedureDataSet("spr_ConsultarDirectriz_PreparacionSuelo", parametros);
            DataTable dtrespuesta = ds.Tables[0];
            if (dtrespuesta.Rows[0][0].ToString().Equals("OK"))
            {
                DataTable dt = ds.Tables[1];
                StringBuilder sb = new StringBuilder();
                response.Append("<tr>");
                foreach (DataRow R in dt.Rows)
                {
                    if (cantidadfilas <= 5)
                    {
                        response.Append("<td><input type='checkbox' name='" + R["Nombre_Directriz"] + "' idDirectriz='" + R["idDirectriz"] + "' value='" + R["idDirectriz"] + "'> <label> " + R["Nombre_Directriz"] + "</label><img onclick=\"popUpMostrar($(this))\" alt=\"Consultar Directriz\" class=\"style2 consultarDirectriz\" src=\"../comun/img/lupa.png\" /></td>");
                        cantidadfilas = cantidadfilas + 1;
                    }
                    else
                    {
                        cantidadfilas = 0;
                        //response.Append("<td><input type='checkbox' name='" + R["Nombre_Directriz"] + "' idDirectriz='" + R["idDirectriz"] + "' value='" + R["idDirectriz"] + "'> " + R["Nombre_Directriz"] + "</td>");
                        response.Append("<td><input type='checkbox' name='" + R["Nombre_Directriz"] + "' idDirectriz='" + R["idDirectriz"] + "' value='" + R["idDirectriz"] + "'> <label> " + R["Nombre_Directriz"] + "</label><img onclick=\"popUpMostrar($(this))\" alt=\"Consultar Directriz\" class=\"style2 consultarDirectriz\" src=\"../comun/img/lupa.png\" /></td>");
                        response.Append("</tr><tr>");
                    }
          
                }
            }
            else
            {

                //popUpMessageControl1.setAndShowInfoMessage(dtrespuesta.Rows[0][1].ToString(), Comun.MESSAGE_TYPE.Info);
            }
        
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
            //return response.ToString();
        }

     


        return response.ToString();
        
        
    }

    [WebMethod]
    public static string ConsultarDirectrizUni(int idDirectriz)
    {
        DataAccess da = new DataAccess();
        DataTable dt = da.executeStoreProcedureDataTable("[spr_DirectrizPreparacionSueloObtenerTabla]", new Dictionary<string, object>() { 
            {"@idDirectriz", idDirectriz}
          , {"@idDepartamento", 4}//HttpContext.Current.Session["idDepartamento"]
        });
        DataTable dt2 = da.executeStoreProcedureDataTable("spr_DirectrizObtenerConfiguracionPorId", new Dictionary<string, object>() { 
            {"@idDirectriz",idDirectriz}
        });
        string nombreDirectriz = da.executeStoreProcedureString("spr_DirectrizObtenerNombre", new Dictionary<string, object>() { { "@idDirectriz", idDirectriz } });
        var idPlanta = dt2.Rows[0]["idPlanta"].ToString();

        Directriz directriz = new Directriz();
        directriz.nombre = nombreDirectriz;

        directriz.tabla = ConvertDataTableToHTML(
          dt
          , string.Format(
              "<script> $('.txtPlantaImportada').val('{0}');</script><table class=\"grid\" cellspacing=\"0\" rules=\"all\" border=\"1\" id=\"gv_Directriz\" style=\"border-collapse:collapse;\">"
            , idPlanta));

        DataTable dtVariedades = da.executeStoreProcedureDataTable("spr_DirectrizObtenerVariedades", new Dictionary<string, object>() { { "@idDirectriz", idDirectriz } });
        DataTable dtVariables = da.executeStoreProcedureDataTable("spr_DirectrizObtenerVariable", new Dictionary<string, object>() { { "@idDirectriz", idDirectriz } });
        DataTable dtTemporales = da.executeStoreProcedureDataTable("spr_DirectrizObtenerTemporales", new Dictionary<string, object>() { { "@idDirectriz", idDirectriz } });

        directriz.variedades = dataTableValuesToArray(dtVariedades);
        directriz.variables = dataTableValuesToArray(dtVariables);
        directriz.temporales = dataTableValuesToArray(dtTemporales);

        return new JavaScriptSerializer().Serialize(directriz);
    }

    [WebMethod]
    public static string DatosHabilidadDirectriz(int idDirectriz)
    {
        //DataAccess da = new DataAccess();
        //StringBuilder response = new StringBuilder();
        //int cantidadfilas = 0;
        //try
        //{
        //    response.Append("<tr>");
        //    DataTable dt = da.executeStoreProcedureDataTable("spr_ObtenerHabilidadesDirectrizPreparacionSuelo", new Dictionary<string, object>());

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        if (cantidadfilas <= 2)
        //        {
        //            response.Append("<td><label idHabilidad='" + item["idHabilidad"] + "' idEtapa='" + item["idEtapa"] + "' idNivel='" + item["idNivel"] + "' for='male'> " + item["NombreHabilidad"] + "-" + item["NombreEtapa"] + "-Nivel " + item["idNivel"] + "</label></td><td><input class='intValidate' type='text' name='lname'></td>");

        //            cantidadfilas = cantidadfilas + 1;
        //        }
        //        else
        //        {
        //            cantidadfilas = 0;
        //            response.Append("<td><label idHabilidad='" + item["idHabilidad"] + "' idEtapa='" + item["idEtapa"] + "' idNivel='" + item["idNivel"] + "' for='male'> " + item["NombreHabilidad"] + "-" + item["NombreEtapa"] + "-Nivel " + item["idNivel"] + "</label></td><td><input class='intValidate' type='text' name='lname'></td>");
        //            response.Append("</tr><tr>");
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(ex);
        //    response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
        //}
        //return response.ToString();
        return "";
    }
    
    [WebMethod(EnableSession = true)]
    private static string ConvertDataTableToHTML(DataTable dt, string headerTable)
    {
        //HttpContext.Current.Session["toExcel"] = dt;
        string html = headerTable;
        //Regex regex = new Regex("^[0-9]*$");
        Regex regex = new Regex("^[0-9]([.,][0-9]{1,3})?$");
        //add header row
        html += "<thead>";
        html += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
            if (dt.Columns[i].ColumnName ==Convert.ToString(0))
            //if (i== 1)
            {
                html += string.Format("<th>{0}</th>", "Preparación Suelo");
            }
            else
            {
                html += string.Format("<th>{0}</th>", dt.Columns[i].ColumnName);
            }
        html += "</tr>";
        html += "</thead>";
        //add rows
        html += "<tbody>";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)// agregando validación números no negativos no letras
                html += string.Format("<td>{0}</td>", (dt.Rows[i][j].ToString() == "0" || (j > 0 && !regex.IsMatch(dt.Rows[i][j].ToString())) ? "" : dt.Rows[i][j].ToString()));
            html += "</tr>";
        }
        html += "</tbody>";
        html += "</table>";
        HttpContext.Current.Session["toExcel"] = html;
        return html;
    }
    private static int[] dataTableValuesToArray(DataTable dt)
    {
        int[] intArray = new int[(dt != null && dt.Rows != null && dt.Rows.Count > 0) ? dt.Rows.Count : 1];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            intArray[i] = Int32.Parse(dt.Rows[i][0].ToString());
        }
        return intArray;
    }
    [WebMethod]
    public static string[] Guardar(HabilidadesPrepSuelo[] Habilidades, string directrices)
    {
        string[] mensaje = new string[2];


        if (Habilidades.Length == 0)
        {
            mensaje[0] = "No se realizaron cambios.";
            mensaje[1] = "warning";
            return mensaje;
        }
        else
        {
            try
            {
                Dictionary<string, object> prm = new Dictionary<string, object>();

                DataTable habilidad = new DataTable();
                habilidad.Columns.Add("idHabilidad");
                habilidad.Columns.Add("idEtapa");
                habilidad.Columns.Add("idNivel");
                habilidad.Columns.Add("Repeticiones");

                
                int ContadorDeArticulos = 0;
                int ContadorPorPlanta = 0;
                foreach (var art in Habilidades)
                {
                    DataRow dr = habilidad.NewRow();
                    if (art.idHabilidad == null)
                    { //Inserción de un registro nuevo
                    }
                    else
                    {
                        dr["idHabilidad"] = art.idHabilidad;
                    }
                    dr["idEtapa"] = art.idEtapa;
                    dr["idNivel"] = art.idNivel;
                    dr["Repeticiones"] =(decimal) art.Repeticiones;
                    habilidad.Rows.Add(dr);
                }

                prm.Add("@idUsuario", HttpContext.Current.Session["idUsuario"]);
                prm.Add("@directrices", directrices);
                prm.Add("@habilidad", habilidad);
                //prm.Add("@stockPlantas", StocksPlantas);

                DataAccess da = new DataAccess();
                DataTable dt = da.executeStoreProcedureDataTable("spr_GuardarHabilidadesDirectrizSuelo", prm);
                switch (int.Parse(dt.Rows[0]["Estado"].ToString()))
                {
                    case 0:
                        mensaje[0] = "No se efectuarán cambios en los artículos, debido a un error interno";
                        mensaje[1] = "error";
                        //return mensaje;
                        break;
                    case 1:
                        mensaje[0] = "Los datos se almacenaron correctamente";
                        mensaje[1] = "ok";
                        //return mensaje;
                        break;
                    
                    default:
                        break;
                }
                return mensaje;
               
            }
            catch (Exception)
            {

                mensaje[0] = "No se Almacenaron los datos registrados";
                mensaje[1] = "error";
                return mensaje;
            }
        }
    }
    public string VariedadC;
    public string VariableC;
    public string TemporalC;
    protected void UpdateButton_Click(object sender, EventArgs e)
    { DataTable dts; }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
         VariedadC = "";
         VariableC = "";
         TemporalC = "";
         VariedadC = Variedad.Value;
         VariableC = Variable.Value;
         TemporalC = Temporal.Value;
        //if (Accion.Value == "Añadir" && Session["IdModuloCookie"] == null || Session["IdModuloCookie"].ToString().Trim().Length == 0){}
        //Accion.Value = "Guardar Cambios";
        //chk_Temporales.
        

        Dictionary<string, object> parametros = new Dictionary<string, object>();



        //parametros.Add("@idDepartamento", HttpContext.Current.Session["idDepartamento"]);
        parametros.Add("@idVariedad", VariedadC);
        parametros.Add("@idVariable", VariableC);
        parametros.Add("@idTemporal", TemporalC);
        
        string Cadena;
        DataAccess da = new DataAccess();
       
        var response = new string[4];
        DataSet ds = da.executeStoreProcedureDataSet("spr_ConsultarDirectriz_PreparacionSuelo", parametros);
        DataTable dtrespuesta = ds.Tables[0];
        if (dtrespuesta.Rows[0][0].ToString().Equals("OK"))
        {
            DataTable dt = ds.Tables[1];
            StringBuilder sb = new StringBuilder();
            foreach (DataRow R in dt.Rows)
            {
                int idDirectriz = Int32.Parse(R["idDirectriz"].ToString().Trim()); //R["idDirectriz"].ToString();
                var NombreDirectriz = R["Nombre_Directriz"].ToString();
              
            }
        }
        else {

            popUpMessageControl1.setAndShowInfoMessage(dtrespuesta.Rows[0][1].ToString(), Comun.MESSAGE_TYPE.Info);
        }
        

    }
}