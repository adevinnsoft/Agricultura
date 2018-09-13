using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Globalization;

public partial class Reportes_frmGrowingLiderDeLaSemana : BasePage//System.Web.UI.Page    //BasePage  //
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
   
 
   
    [WebMethod]
    public static string ObtenerSemanasNS()
    {
        StringBuilder sbsemanas = new StringBuilder();
        DataAccess da = new DataAccess();
        var parameters = new Dictionary<string, object>();
        DataTable DTSemanas = da.executeStoreProcedureDataTable("spr_ObtieneSemanasGrowing", parameters);
        sbsemanas.Append("<option value='Todas'><label for='male'> Todas </label></option>");
        foreach (DataRow R in DTSemanas.Rows)
        {
            //<option value="Lorem">Lorem</option>

            sbsemanas.Append("<option value=" + R["vWeek"] + "><label for='male'> " + R["vWeek"] + "</label></option>");
        }
        return sbsemanas.ToString();
    }
    public static string[] DatosAcomodo(int planta)
    {
        int cantidadfilas = 0;
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        string Cadena;
        DataAccess da = new DataAccess();

        var response = new string[4];
        int ranking = 0;
        int rankingNoPlantacion = 1;
        int auxiliaridlider = 0;
        int contadorauxcolumn = 3;

        int auxiliaridlider2 = 0;
        int contadorauxcolumn2 = 3;
        int ran = 0;
        //int planta = 6;
        
        //DataTable pr = (DataTable)HttpContext.Current.Session["PuntosMalosAllPlantas"];
        DataTable DTLIDER = (DataTable)HttpContext.Current.Session["PuntosMalosAllPlantas1"];
        DataTable DTPLANTACION = (DataTable)HttpContext.Current.Session["PuntosMalosAllPlantas1"];
        DataTable DTNOPLANTACION = (DataTable)HttpContext.Current.Session["PuntosMalosAllPlantas1"]; ;
        foreach (DataRow R in DTLIDER.Select("IdPlanta=" + planta))
        //foreach (DataRow R in DTLIDER.Rows)
        {
            sb.Append("<tr><td class='marino'><label for='male'> " + (ranking = ranking + 1) + "</label></td><td class='cyan'><label idUsuario='" + R["idLider"] + "' for='male'> " + R["vNombre"] + "</label></td><td class='red'><label  for='male'> " + R["Calificacion"] + "%</label></td>");

        }
        foreach (DataRow R in DTPLANTACION.Select("IdPlanta=" + planta))
        //foreach (DataRow R in DTPLANTACION.Rows)
        {
            if (auxiliaridlider != Convert.ToInt32(R["idlider"].ToString()))
            {
                if (contadorauxcolumn != 0)
                {
                    if (sb2.ToString() != "")
                    {
                        for (int i = contadorauxcolumn; i > 0; i--)
                        {
                            sb2.Append("<td>--</td>");
                        }
                        sb2.Append("</tr>");
                        contadorauxcolumn = 3;
                    }
                }
                sb2.Append("<tr>");
                auxiliaridlider = Convert.ToInt32(R["idlider"].ToString());
            }
            else
            { }
            int Ridlider = Convert.ToInt32(R["ranking"].ToString());
            if (Ridlider == 1)
            { sb2.Append("<td class='marino'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
            if (Ridlider == 2)
            { sb2.Append("<td class='cyan'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
            if (Ridlider == 3)
            { sb2.Append("<td class='red'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
            contadorauxcolumn--;
        }
        foreach (DataRow R in DTNOPLANTACION.Select("IdPlanta=" + planta))
        //foreach (DataRow R in DTNOPLANTACION.Rows)
        {
            if (auxiliaridlider2 != Convert.ToInt32(R["idlider"].ToString()))
            {
                if (contadorauxcolumn2 != 0)
                {
                    if (sb3.ToString() != "")
                    {
                        for (int i = contadorauxcolumn2; i > 0; i--)
                        {
                            sb3.Append("<td>--</td>");
                        }
                        sb3.Append("</tr>");
                        contadorauxcolumn2 = 3;
                    }
                }
                sb3.Append("<tr>");
                auxiliaridlider2 = Convert.ToInt32(R["idlider"].ToString());
            }
            else
            { }
            int Ridlider2 = Convert.ToInt32(R["ranking"].ToString());
            if (Ridlider2 == 1)
            { sb3.Append("<td class='marino'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
            if (Ridlider2 == 2)
            { sb3.Append("<td class='cyan'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
            if (Ridlider2 == 3)
            { sb3.Append("<td class='red'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
            contadorauxcolumn2--;
        }
        return new string[] { "ok", sb.ToString(), sb2.ToString(), sb3.ToString() };
    }
    [WebMethod]
    public static string[] ObtenerDatosIniciales(int idplanta,String semana)
    {
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        //parametros.Add("@idDepartamento", HttpContext.Current.Session["idDepartamento"]);
        //idplanta

        parametros.Add("@idplanta", idplanta);
        if (semana=="0" )
        {parametros.Add("@Semanaanio", "Todas"); }
        else
        { 
            parametros.Add("@Semanaanio", semana); 
        }
        
        

        string Cadena;
        DataAccess da = new DataAccess();

        var response = new string[4];
        int ranking = 0;
        int rankingNoPlantacion = 1;
        int auxiliaridlider=0;
        int contadorauxcolumn = 3;

        int auxiliaridlider2 = 0;
        int contadorauxcolumn2 = 3;
        int ran = 0;
        //int planta = 6;
        try
        {
            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerDatosReporteGrowingGeneral", parametros);
            
            DataTable dtrespuesta = ds.Tables[0];
        
        if (dtrespuesta.Rows[0][0].ToString().Equals("OK"))
        //if (true)
        {
            
            DataTable DTLIDER = ds.Tables[1];
            DataTable DTPLANTACION = ds.Tables[2];
            DataTable DTNOPLANTACION = ds.Tables[3];
            HttpContext.Current.Session["PuntosMalosAllPlantas1"]=DTLIDER;
           
            foreach (DataRow R in DTLIDER.Rows)
            {
                sb.Append("<tr><td class='marino'><label for='male'> " + (ranking = ranking + 1) + "</label></td><td class='cyan'><label idUsuario='" + R["idLider"] + "' for='male'> " + R["vNombre"] + "</label></td><td class='red'><label  for='male'> " + R["Calificacion"] + "%</label></td>");

            }
            //foreach (DataRow R in DTPLANTACION.Select("IdPlanta=" + idplanta))
            foreach (DataRow R in DTPLANTACION.Rows)
            {
                if(auxiliaridlider!=Convert.ToInt32(R["idlider"].ToString()))	
	            {
		            if(contadorauxcolumn!=0)
		            {
                        if (sb2.ToString() != "")
                        {
                            for (int i = contadorauxcolumn; i > 0; i--)
                            {
                                //sb2.Append("<td>--</td>");
                            }
                            sb2.Append("</tr>");
                            contadorauxcolumn = 3;
                        }
		            }
		            sb2.Append("<tr>");
                    auxiliaridlider = Convert.ToInt32(R["idlider"].ToString());
	            }
	            else
	            {}
                int Ridlider = Convert.ToInt32(R["ranking"].ToString());
                if( Ridlider==1)
                {sb2.Append("<td class='marino'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>");}
                if( Ridlider==2)
                {sb2.Append("<td class='cyan'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>");}
                if (Ridlider==3)
                {sb2.Append("<td class='red'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>");}
                contadorauxcolumn--;
            }
            //foreach (DataRow R in DTNOPLANTACION.Select("IdPlanta=" + idplanta))
            foreach (DataRow R in DTNOPLANTACION.Rows)
            {                
                if (auxiliaridlider2 != Convert.ToInt32(R["idlider"].ToString()))
                {
                    if (contadorauxcolumn2 != 0)
                    {
                        if (sb3.ToString() != "")
                        {
                            for (int i = contadorauxcolumn2; i > 0; i--)
                            {
                                sb3.Append("<td>--</td>");
                            }
                            sb3.Append("</tr>");
                            contadorauxcolumn2 = 3;
                        }
                    }
                    sb3.Append("<tr>");
                    auxiliaridlider2 = Convert.ToInt32(R["idlider"].ToString());
                }
                else
                {}
                int Ridlider2 = Convert.ToInt32(R["ranking"].ToString());
                if (Ridlider2 == 1)
                { sb3.Append("<td class='marino'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
                if (Ridlider2 == 2)
                { sb3.Append("<td class='cyan'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
                if (Ridlider2 == 3)
                { sb3.Append("<td class='red'><label idgrupo='" + R["idGrupo"] + "' for='male'> " + R["grupoes"] + "</label></td>"); }
                contadorauxcolumn2--;
            }
            return new string[] { "ok", sb.ToString(),sb2.ToString(),sb3.ToString() };
        }
        else
        {
            return new string[] { "info", "No se encontraron capturas registradas.", "No records were obtained." };
        }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            sb.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
            return new string[] { "error",sb.ToString()};
        }
        //return new string[] {};
    }
    [WebMethod]
    public static string[] ObtenerDatosIniciales2(int idplanta, String semana)
    {
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbgrafica = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        

        //parametros.Add("@idplanta", idplanta);
        parametros.Add("@idplanta", (idplanta == 0 ? Convert.ToInt32(HttpContext.Current.Session["idPlanta"].ToString()) : idplanta));
        if (semana == "0")
        { parametros.Add("@Semanaanio", "Todas"); }
        else
        {
            parametros.Add("@Semanaanio", semana);
        }

       

        
        DataAccess da = new DataAccess();

        var response = new string[4];
        int ranking = 0;
        
        try
        {
            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerDatosReporteGrowingGeneral2", parametros);

            DataTable dtrespuesta = ds.Tables[0];

            if (dtrespuesta.Rows[0][0].ToString().Equals("OK"))
            //if (true)
            {

                DataTable DTLIDER = ds.Tables[1];
                DataTable DTPLANTACION = ds.Tables[2];
                DataTable DTNOPLANTACION = ds.Tables[3];
                HttpContext.Current.Session["PuntosMalosAllPlantas1"] = DTLIDER;

                foreach (DataRow R in DTLIDER.Rows)
                {
                    sb.Append("<tr><td class='marino'><label for='male'> " + (ranking = ranking + 1) + "</label></td><td class='cyan'><label idUsuario='" + R["idLider"] + "' for='male'> " + R["vNombre"] + "</label></td><td class='red'><label  for='male'> " + R["Calificacion"] + "%</label></td>");



                    //De la columna GrupoEs formar el string
                    //string Grupos = "'Plantacion','Control Clima','Fertirriego','Polinización'";
                    //De la columna Cumplimiento formar el string
                    //string Cumplimiento = ".3852,.75,.6667,1";
                    //ETC

                    //return new string[] { Grupos, Cumplimiento };




                }
                foreach (DataRow R in DTPLANTACION.Rows)
                {
                    sb2.Append("<tr><td class='marino'><label idgrupo='" + R["idlider"] + "' for='male'> " + (R["1"].ToString() == "" ? "--" : R["1"]) + "</label></td><td class='cyan'><label idgrupo='" + R["idlider"] + "' for='male'> " + (R["2"].ToString() == "" ? "--" : R["2"]) + "</label></td><td class='red'><label idgrupo='" + R["idlider"] + "' for='male'> " + (R["3"].ToString() == "" ? "--" : R["3"]) + "</label></td></tr>");
                }
                foreach (DataRow R in DTNOPLANTACION.Rows)
                {
                    sb3.Append("<tr><td class='marino'><label idgrupo='" + R["idlider"] + "' for='male'> " + (R["1"].ToString() == "" ? "--" : R["1"]) + "</label></td><td class='cyan'><label idgrupo='" + R["idlider"] + "' for='male'> " + (R["2"].ToString() == "" ? "--" : R["2"]) + "</label></td><td class='red'><label idgrupo='" + R["idlider"] + "' for='male'> " + (R["3"].ToString() == "" ? "--" : R["3"]) + "</label></td></tr>");
                }
                return new string[] { "ok", sb.ToString(), sb2.ToString(), sb3.ToString() };
            }
            else
            {
                return new string[] { "info", "No se encontraron capturas registradas.", "No records were obtained." };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            //response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
            //return response.ToString();
        }
        return new string[] { };
    }
}