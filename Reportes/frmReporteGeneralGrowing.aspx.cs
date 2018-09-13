using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.Data;

public partial class Reportes_frmReporteGeneralGrowing : BasePage//System.Web.UI.Page//BasePage //System.Web.UI.Page
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
        //sbsemanas.Append("<option value='Todas'><label for='male'> Todas </label></option>");
        foreach (DataRow R in DTSemanas.Rows)
        {
            sbsemanas.Append("<option value=" + R["vWeek"] + "><label for='male'> " + R["vWeek"] + "</label></option>");
        }
        return sbsemanas.ToString(); 
    }


    [WebMethod]
    public static string ObtenerGrower(int idplanta, int idgerente, String semana)
    {
        StringBuilder sbGrower = new StringBuilder();
        DataAccess da = new DataAccess();
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        parametros.Add("@idplanta", (idplanta == 0 ? Convert.ToInt32(HttpContext.Current.Session["idPlanta"].ToString()) : idplanta));
        parametros.Add("@idgerente", idgerente);
        //if (semana == null)
        //{ parametros.Add("@Semanaanio", null); }
        //else
        //{
        //    parametros.Add("@Semanaanio", semana);
        //}
        DataTable DTGrower = da.executeStoreProcedureDataTable("spr_obtenerGrowerGrowing", parametros);
        sbGrower.Append("<option value=0><label for='male'> Todos </label></option>");
        foreach (DataRow R in DTGrower.Rows)
        {
            sbGrower.Append("<option value=" + R["IDUSUARIO"] + "><label for='male'> " + R["VGROWER"] + "</label></option>");
        }
        return sbGrower.ToString(); //return "";
    }
    [WebMethod]
    public static string ObtenerLideresPlanta(int idplanta, int idgerente, String semana)
    {
        StringBuilder sblider = new StringBuilder();
        DataAccess da = new DataAccess();
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        parametros.Add("@idplanta", (idplanta == 0 ? Convert.ToInt32(HttpContext.Current.Session["idPlanta"].ToString()) : idplanta));
        parametros.Add("@idgerente", idgerente);
        //if (semana == null)
        //{ parametros.Add("@Semanaanio", null); }
        //else
        //{
        //    parametros.Add("@Semanaanio", semana);
        //}
        DataTable DTSemanas = da.executeStoreProcedureDataTable("spr_obtenerLideresXplanta", parametros);
        sblider.Append("<option value=0><label for='male'> Todos </label></option>");
        foreach (DataRow R in DTSemanas.Rows)
        {
            sblider.Append("<option value=" + R["idlider"] + "><label for='male'> " + R["vNombre"] + "</label></option>");
        }
        return sblider.ToString(); //return "";
    }
    [WebMethod]
    public static string ObtenerGerentePlanta(int idplanta, String semana)
    {
        StringBuilder sbGerente = new StringBuilder();
        DataAccess da = new DataAccess();
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        parametros.Add("@idplanta", (idplanta == 0 ? Convert.ToInt32(HttpContext.Current.Session["idPlanta"].ToString()) : idplanta));
        //if (semana == null)
        //{ parametros.Add("@Semanaanio", null); }
        //else
        //{
        //    parametros.Add("@Semanaanio", semana);
        //}
        DataTable DTSemanas = da.executeStoreProcedureDataTable("spr_obtenerGerenteDeLiderXplanta", parametros);
        sbGerente.Append("<option value='0'><label for='male'> Todos </label></option>");
        foreach (DataRow R in DTSemanas.Rows)
        {
            sbGerente.Append("<option value=" + R["idGerente"] + "><label for='male'> " + R["vNombre"] + "</label></option>");
        }
        return sbGerente.ToString(); //return "";
    }
    [WebMethod]
    public static string[] ObtenerDatosIniciales(int idplanta, String semana, int idGrower,int idLider, int idGerente)
    {
        Dictionary<string, object> parametros = new Dictionary<string, object>();
        StringBuilder sb = new StringBuilder();
        StringBuilder sb1grafica = new StringBuilder();
        StringBuilder sb1Cumplimiento = new StringBuilder();
        StringBuilder sb1Calificacion = new StringBuilder();
        StringBuilder sb1Distribucion = new StringBuilder();
        StringBuilder sb2grafica = new StringBuilder();
        StringBuilder sb3grafica = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();

        StringBuilder sb22 = new StringBuilder();
        StringBuilder sb1grafica2 = new StringBuilder();
        StringBuilder sb1Cumplimiento2 = new StringBuilder();
        StringBuilder sb1Calificacion2 = new StringBuilder();
        StringBuilder sb1Distribucion2 = new StringBuilder();
        StringBuilder sb2grafica2 = new StringBuilder();
        StringBuilder sb3grafica2 = new StringBuilder();
        StringBuilder sb2_2 = new StringBuilder();
        StringBuilder sb3_2 = new StringBuilder();



        StringBuilder sbXActividadPlantacion = new StringBuilder();
        StringBuilder sbXActividadPlantacionColumnasNombre = new StringBuilder();
        StringBuilder sbXActividadPlantacionColumnasPorcentaje = new StringBuilder();

        StringBuilder sbXActividadNoPlantacion = new StringBuilder();
        StringBuilder sbXActividadNoPlantacionColumnasNombre = new StringBuilder();
        StringBuilder sbXActividadNoPlantacionColumnasPorcentaje = new StringBuilder();

        StringBuilder sbProblemasPlantacion = new StringBuilder();
        StringBuilder sbProblemasNoPlantacion = new StringBuilder();
        StringBuilder sbplantasinconfiguracion = new StringBuilder();


        StringBuilder SBTOTALPLANT_NOPL = new StringBuilder();
       
        //parametros.Add("@idplanta", idplanta);
        parametros.Add("@idplanta", (idplanta == 0 ? Convert.ToInt32(HttpContext.Current.Session["idPlanta"].ToString()) : idplanta));
        
        if (semana == null)
        { parametros.Add("@Semanaanio", null); }
        else
        {
            parametros.Add("@Semanaanio", semana);
        }
        if (idGrower == 0)
        { parametros.Add("@idGrower", null); }
        else
        {
            parametros.Add("@idGrower", idGrower);
        }
        if (idLider == 0)
        { parametros.Add("@LiderInvernadero", null); }
        else
        {
            parametros.Add("@LiderInvernadero", idLider);
        }
        if (idGerente == 0)
        { parametros.Add("@IDGERENTEZ", null); }
        else
        {
            parametros.Add("@IDGERENTEZ", idGerente);
        }




        DataAccess da = new DataAccess();

        var response = new string[4];
        int ranking = 0;

        try
        {
            DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerDatosReporteGrowing_GeneraPLANTACION", parametros);
            //DataSet ds = da.executeStoreProcedureDataSet("spr_ObtenerDatosReporteGrowing_GeneraPLANTACION0405", parametros);
            String SinConfigpl = "false";
            //DataTable dtrespuesta = ds.Tables[0];
            if (ds.Tables[0].Rows.Count == 0)
            {
                SinConfigpl="true";//sbplantasinconfiguracion.Append("No existe configuracion para esta planta");
            }
                DataTable DTResumenGHPlantacion = ds.Tables[0];
                DataTable CALIFTOTALPLANTACION = ds.Tables[1];
                DataTable DTPORCENTAJESPORACTIVIDADPLANTACION = ds.Tables[2];
                DataTable DTPROBLEMASPLANTACION = ds.Tables[3];
                DataTable DTTOTALPLANTACION = ds.Tables[4];
                HttpContext.Current.Session["PuntosMalosAllPlantas1"] = DTResumenGHPlantacion;
                Boolean bcolor = true;
                foreach (DataRow R in DTResumenGHPlantacion.Rows)
                {                    
                    //        sb.Append("<tr><td><label for='male'> " + (R["GrupoES"].ToString() == "" ? "--" : R["GrupoES"]) + "</label></td><td class='green'><label for='male'> " + (R["CUMPLIMIENTO"].ToString() == "" ? "0" : (R["CUMPLIMIENTO"]) + "%") + "</label></td><td class='green'><label  for='male'>" + (R["CALIFICACION"].ToString() == "" ? "0" : (R["CALIFICACION"]) + "%") + "</label></td><td class='green'><label  for='male'> " + (R["DISTRIBUCION"].ToString() == "" ? "0" : (R["DISTRIBUCION"]) + "%") + "</label></td></tr>");
                    sb.Append("<tr><td><label for='male'> " + (R["GrupoES"].ToString() == "" ? "--" : R["GrupoES"]) + "</label></td><td><label for='male'> " + (R["CUMPLIMIENTO"].ToString() == "" ? "--" : (R["CUMPLIMIENTO"]) + "%") + "</label></td><td><label  for='male'>" + (R["CALIFICACION"].ToString() == "" ? "--" : (R["CALIFICACION"]) + "%") + "</label></td><td><label  for='male'> " + (R["DISTRIBUCION"].ToString() == "" ? "--" : (Convert.ToDecimal(R["DISTRIBUCION"].ToString()) == Convert.ToDecimal(0.0) ?"":(R["DISTRIBUCION"]) + "%")) + "</label></td></tr>"); 

                    sb1grafica.Append(""+R["GrupoES"]+",");

                     sb1Cumplimiento.Append("" + R["CUMPLIMIENTO"].ToString() + ",");
                     sb1Calificacion.Append("" + R["CALIFICACION"].ToString() + ",");
                     sb1Distribucion.Append("" + R["DISTRIBUCION"].ToString() + ",");

                }
                sb2grafica.Append(sb1Cumplimiento + "|" + sb1Calificacion + "|" + sb1Distribucion);
                int auxiliaridlider = 0;
                foreach (DataRow R in CALIFTOTALPLANTACION.Rows)
                {
                    sb.Append("<tr><td colspan='4'>&nbsp;</td></tr><tr class='calificacion'><td>Calificación GH Plantación</td><td><label id='lblSumaTotalCumplimientoPlantacion' for='male'> " + (R["PORCENT_CUMPLIMIENTO"].ToString() == "" ? "0%" : (R["PORCENT_CUMPLIMIENTO"]) + "%") + "</label></td><td><label id='lblSumaTotalCalificacionPlantacion' for='male'>" + (R["PORCENT_CALIFICACION"].ToString() == "" ? "0%" : (R["PORCENT_CALIFICACION"]) + "%") + "</label></td></tr>");
                }
                foreach (DataRow R in DTPORCENTAJESPORACTIVIDADPLANTACION.Rows)
                {
                    //if ((sbXActividadPlantacion.ToString().Equals("''") || R["GrupoES"].ToString().Equals("OK")))
                    if (auxiliaridlider != Convert.ToInt32(R["idGrupo"].ToString()))
                    {
                        sbXActividadPlantacion.Append("|" + R["GrupoEs"].ToString()+ "&" + R["ParametroEs"].ToString() + ",");
                        sbXActividadPlantacionColumnasNombre.Append("|" + R["ParametroEs"].ToString() + ",");
                        sbXActividadPlantacionColumnasPorcentaje.Append("|" + R["PROMEDIO"].ToString() + ",");
                    }
                    else
                    {
                        sbXActividadPlantacion.Append("" + R["ParametroEs"].ToString() + ",");
                        sbXActividadPlantacionColumnasNombre.Append("" + R["ParametroEs"].ToString() + ",");
                        sbXActividadPlantacionColumnasPorcentaje.Append("" + R["PROMEDIO"].ToString() + ",");
                    }
                    auxiliaridlider = Convert.ToInt32(R["idGrupo"].ToString());
                }
                int contadorProblemasPlantacion = 0;
                foreach (DataRow R in DTPROBLEMASPLANTACION.Rows)
                {
                    sbProblemasPlantacion.Append("<tr><td class='blue1'><label class='ProblemasPlantacion' for='male'> Problema " + (contadorProblemasPlantacion = contadorProblemasPlantacion + 1) + "</label></td><td ><label class='ProblemasNombrePlantacion' for='male'>" + R["GrupoEs"].ToString() + "</label></td></tr>");

                }
                if (contadorProblemasPlantacion != 3)
                {
                    for (int i = contadorProblemasPlantacion + 1; i <= 3; i++)
                    {
                        sbProblemasPlantacion.Append("<tr><td  class='blue1'><label class='ProblemasPlantacion' for='male'> Problema " + i + "</label></td><td ><label class='ProblemasNombrePlantacion' for='male'>--</label></td></tr>");
                    }
                }
                //bool AUX_PLANT=false;
                String AUX_PLANT = "false";
                foreach (DataRow R in DTTOTALPLANTACION.Rows)
                {
                   if((Convert.ToInt32(R["NUM_REGISTROS"].ToString()))>0)
                    {
                        AUX_PLANT="true";
                    }
                    else{
                        AUX_PLANT="false";
                   }
                    //SBTOTALPLANT_NOPL
                }
                /////////////////////////////////No plantacion/////////////////////////////////////////////
                DataSet ds2 = da.executeStoreProcedureDataSet("spr_ObtenerDatosReporteGrowing_GeneraNOPLANTACION", parametros);
                //DataSet ds2 = da.executeStoreProcedureDataSet("spr_ObtenerDatosReporteGrowing_GeneraNOPLANTACION0405", parametros);

                DataTable dtrespuesta2 = ds2.Tables[0];
                String SinConfignopl = "false";
                if (ds.Tables[0].Rows.Count == 0)
                {
                    SinConfignopl = "true";//sbplantasinconfiguracion.Append("No existe configuracion para esta planta");
                }
                DataTable DTResumenGHNOPlantacion = ds2.Tables[0];
                DataTable CALIFTOTALNOPLANTACION = ds2.Tables[1];
                DataTable DTPORCENTAJESPORACTIVIDADNOPLANTACION = ds2.Tables[2];
                DataTable DTPROBLEMASNOPLANTACION = ds2.Tables[3];
                DataTable DTTOTALNOPLANTACION = ds2.Tables[4];
                HttpContext.Current.Session["PuntosMalosAllPlantas1"] = DTResumenGHNOPlantacion;
                Boolean bcolor2 = true;
                foreach (DataRow R in DTResumenGHNOPlantacion.Rows)
                {
                    //if (bcolor2 == true)
                    //{
                    //    sb2_2.Append("<tr><td><label for='male'> " + (R["GrupoES"].ToString() == "" ? "--" : R["GrupoES"]) + "</label></td><td class='green'><label for='male'> " + (R["CUMPLIMIENTO"].ToString() == "" ? "0" : (R["CUMPLIMIENTO"])+"%") + "</label></td><td class='green'><label  for='male'> " + (R["CALIFICACION"].ToString() == "" ? "0" : (R["CALIFICACION"])+"%") + "</label></td><td class='green'><label  for='male'> " + (R["DISTRIBUCION"].ToString() == "" ? "0" : (R["DISTRIBUCION"])+"%") + "</label></td></tr>");
                    //    bcolor2 = false;
                    //}
                    //else
                    //{
                    //    sb2_2.Append("<tr><td><label for='male'> " + (R["GrupoES"].ToString() == "" ? "--" : R["GrupoES"]) + "</label></td><td class='orange'><label for='male'> " + (R["CUMPLIMIENTO"].ToString() == "" ? "0" : (R["CUMPLIMIENTO"])+"%") + "</label></td><td class='orange'><label  for='male'> " + (R["CALIFICACION"].ToString() == "" ? "0" : (R["CALIFICACION"])+"%") + "</label></td><td class='orange'><label  for='male'> " + (R["DISTRIBUCION"].ToString() == "" ? "0" : (R["DISTRIBUCION"])+"%") + "</label></td></tr>");
                    //    bcolor2 = true;
                    //}
                    sb2_2.Append("<tr><td><label for='male'> " + (R["GrupoES"].ToString() == "" ? "--" : R["GrupoES"]) + "</label></td><td><label for='male'> " + (R["CUMPLIMIENTO"].ToString() == "" ? "--" : (R["CUMPLIMIENTO"]) + "%") + "</label></td><td><label  for='male'> " + (R["CALIFICACION"].ToString() == "" ? "--" : (R["CALIFICACION"]) + "%") + "</label></td><td><label  for='male'> " + (R["DISTRIBUCION"].ToString() == "" ? "--" : (Convert.ToDecimal(R["DISTRIBUCION"].ToString()) == Convert.ToDecimal(0.0) ? "" : (R["DISTRIBUCION"]) + "%")) + "</label></td></tr>");
                    sb1grafica2.Append("" + R["GrupoES"] + ",");                  
                    sb1Cumplimiento2.Append("" + R["CUMPLIMIENTO"].ToString() + ",");
                    sb1Calificacion2.Append("" + R["CALIFICACION"].ToString() + ",");
                    sb1Distribucion2.Append("" + R["DISTRIBUCION"].ToString() + ",");
                    
                }
                sb2grafica2.Append(sb1Cumplimiento2 + "|" + sb1Calificacion2 + "|" + sb1Distribucion2);
                foreach (DataRow R in CALIFTOTALNOPLANTACION.Rows)
                {
                    sb2_2.Append("<tr><td colspan='4'>&nbsp;</td></tr><tr class='calificacion'><td>Calificación GH No Plantación</td><td><label id='lblSumaTotalCumplimientoNoPlantacion' for='male'> " + (R["PORCENT_CUMPLIMIENTO"].ToString() == "" ? "0%" : (R["PORCENT_CUMPLIMIENTO"]) + "%") + "</label></td><td><label id='lblSumaTotalCalificacionNoPlantacion' for='male'>" + (R["PORCENT_CALIFICACION"].ToString() == "" ? "0%" : (R["PORCENT_CALIFICACION"]) + "%") + "</label></td></tr>");
                }
                int auxiliaridliderNoP = 0;
                foreach (DataRow R in DTPORCENTAJESPORACTIVIDADNOPLANTACION.Rows)
                {
                    //if ((sbXActividadPlantacion.ToString().Equals("''") || R["GrupoES"].ToString().Equals("OK")))
                    if (auxiliaridliderNoP != Convert.ToInt32(R["idGrupo"].ToString()))
                    {
                        sbXActividadNoPlantacion.Append("|NP-" + R["GrupoEs"].ToString() + "&" + R["ParametroEs"].ToString() + ",");
                        sbXActividadNoPlantacionColumnasNombre.Append("|" + R["ParametroEs"].ToString() + ",");
                        sbXActividadNoPlantacionColumnasPorcentaje.Append("|" + R["PORCENTAJE"].ToString() + ",");
                    }
                    else
                    {
                        sbXActividadNoPlantacion.Append("" + R["ParametroEs"].ToString() + ",");
                        sbXActividadNoPlantacionColumnasNombre.Append("" + R["ParametroEs"].ToString() + ",");
                        sbXActividadNoPlantacionColumnasPorcentaje.Append("" + R["PORCENTAJE"].ToString() + ",");
                    }
                    auxiliaridliderNoP = Convert.ToInt32(R["idGrupo"].ToString());
                }
                int contadorProblemasNoPlantacion = 0;
                foreach (DataRow R in DTPROBLEMASNOPLANTACION.Rows)
                {
                    sbProblemasNoPlantacion.Append("<tr><td  class='blue1'><label class='ProblemasNoPlantacion' for='male'> Problema " + (contadorProblemasNoPlantacion = contadorProblemasNoPlantacion + 1) + "</label></td><td><label class='ProblemasNombreNoPlantacion' for='male'>" + R["GrupoEs"].ToString() + "</label></td></tr>");
                }
                if (contadorProblemasNoPlantacion != 3)
                {
                    for (int i = contadorProblemasNoPlantacion + 1; i <= 3; i++)
                    {
                        sbProblemasNoPlantacion.Append("<tr><td  class='blue1'><label class='ProblemasNoPlantacion' for='male'> Problema " + i + "</label></td><td ><label class='ProblemasNombreNoPlantacion' for='male'>--</label></td></tr>");
                    }
                }
                //bool AUX_NOPL=false;
                 String AUX_NOPL = "false";
                 foreach (DataRow R in DTTOTALNOPLANTACION.Rows)
                 {
                     if ((Convert.ToInt32(R["NUM_REGISTROS"].ToString())) > 0)
                     {
                         AUX_NOPL = "true";
                     }
                     else
                     {
                         AUX_NOPL = "false";
                     }

                 }


                 StringBuilder sbCalificacionFinal = new StringBuilder();
                 String CalificacionFinal="";
                 DataTable dscalificaciongeneral = da.executeStoreProcedureDataTable("spr_ObtenerDatosReporteGrowing_CALIGENERAL", parametros);
                 foreach (DataRow R in dscalificaciongeneral.Rows)
                 {
                     //sbCalificacionFinal.Append("<tr><td  class='blue1'><label class='ProblemasNoPlantacion' for='male'> Problema " + (contadorProblemasNoPlantacion = contadorProblemasNoPlantacion + 1) + "");
                     CalificacionFinal = R["CALIFICACION"].ToString();
                 }

                //SBTOTALPLANT_NOPL.Append(""+AUX_PLANT+"|"+AUX_NOPL+"");

                return new string[] 
                {       
                    "ok",                                                       //0
                    sb.ToString(),                                              //1
                    sb2.ToString(),                                             //2
                    sb3.ToString(),                                             //3
                    sb1grafica.ToString(),                                      //4
                    sb2grafica.ToString(),                                      //5
                    sb2_2.ToString(),                                           //6
                    sb2.ToString(),                                             //7
                    sb3_2.ToString(),                                           //8
                    sb1grafica2.ToString(),                                     //9
                    sb2grafica2.ToString(),                                     //10
                    sbXActividadPlantacion.ToString(),                          //11
                    sbXActividadPlantacionColumnasNombre.ToString(),            //12
                    sbXActividadPlantacionColumnasPorcentaje.ToString(),        //13
                    sbProblemasPlantacion.ToString(),                           //14
                    sbProblemasNoPlantacion.ToString(),                          //15
                    sbXActividadNoPlantacion.ToString(),                          //16
                    sbXActividadNoPlantacionColumnasNombre.ToString(),            //17
                    sbXActividadNoPlantacionColumnasPorcentaje.ToString(),         //18
                    SinConfigpl,                                                   //19
                    SinConfignopl,                                                 //20 
                    AUX_PLANT,                                                     //21
                    AUX_NOPL,                                                       //22
                    CalificacionFinal                                               //23
                    //SBTOTALPLANT_NOPL.ToString()  
                };
         
            
        }
        catch (Exception ex)
        {
            //Log.Error(ex);
            //response.Append("<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>");
            //return response.ToString();
        }
        return new string[] { };
    }
}