using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;


public partial class Reportes_ReporteCumplimientoEjecucion : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Reportes_ReporteCumplimientoEjecucion));
    public static int idPlanta = 0;
    public static int idUsuario = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }


    [WebMethod]
    public static string[] ObtenerCumplimientoEjecucion(string semana, string anio)
    {

        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataSet ds = new DataSet();
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        StringBuilder sb4 = new StringBuilder();
        StringBuilder sbb = new StringBuilder();
        StringBuilder sb5 = new StringBuilder();
        StringBuilder sb6 = new StringBuilder();



        try
        {
            idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            idUsuario = Convert.ToInt32(System.Web.HttpContext.Current.Session["userIDInj"].ToString());
            prm.Add("@idPlanta", idPlanta);
            prm.Add("@semana", semana);
            prm.Add("@anio", anio);
            prm.Add("@idLider", idUsuario);

            ds = da.executeStoreProcedureDataSet("spr_ObtenerCumplimientoEjecucionCultivoCosecha", prm);
            //creando la tabla de porcentajes generales para cumplimiento y ejeucucion
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th colspan=\"2\">CULTIVO</th>");
            sb.AppendLine("<th colspan=\"2\">COSECHA</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            foreach (DataRow d in ds.Tables[0].Rows)
            {
                string CumplimientoCultivo = d["cumplimientoGeneralCultivo"].ToString().Trim()
                        , EjecucionCultivo = d["ejecucionGeneralCultivo"].ToString().Trim();


                foreach (DataRow d1 in ds.Tables[1].Rows)
                {
                    string CumplimientoCosecha = d1["cumplimientoGeneralCosecha"].ToString().Trim()
                          , EjecucionCosecha = d1["ejecucionGeneralCosecha"].ToString().Trim();

                    sb.AppendLine("<tr>");
                    sb.AppendLine(CumplimientoCultivo.Equals(string.Empty) ? "<td class=\"cumplimientoCultivo\">Cumplimiento:" + " " + "-</td>" : "<td class=\"cumplimientoCultivo\">Cumplimiento:" + " " + CumplimientoCultivo + "%</td>");
                    sb.AppendLine(EjecucionCultivo.Equals(string.Empty) ? "<td class=\"ejecucionCultivo\">Ejecución:" + " " + "-</td>" : "<td class=\"ejecucionCultivo\">Ejecución:" + " " + EjecucionCultivo + "%</td>");
                    sb.AppendLine(CumplimientoCosecha.Equals(string.Empty) ? "<td class=\"cumplimientoCosecha\">Cumplimiento:" + " " + "-</td>" : "<td class=\"cumplimientoCosecha\">Cumplimiento:" + " " + CumplimientoCosecha + "%</td>");
                    sb.AppendLine(EjecucionCosecha.Equals(string.Empty) ? "<td class=\"ejecucionCosecha\">Ejecución:" + " " + "-</td>" : "<td class=\"ejecucionCosecha\">Ejecución:" + " " + EjecucionCosecha + "%</td>");
                    sb.AppendLine("</tr>");
                }
            }
            
            sb.AppendLine("</tbody>");


            //crendo las tablas para el desglose de cumplimiento y ejecucion por invernadero para cultivo y cosecha
            //CULTIVO
            sb2.AppendLine("<thead>");
            sb2.AppendLine("<tr>");
            sb2.AppendLine("<th colspan=\"4\">CULTIVO</th>");
            sb2.AppendLine("</tr>");
            sb2.AppendLine("<tr>");
            sb2.AppendLine("<th>INVERNADERO</th>");
            sb2.AppendLine("<th>HABILIDAD</th>");
            sb2.AppendLine("<th>CUMPLIMIENTO</th>");
            sb2.AppendLine("<th>EJECUCION</th>");
            sb2.AppendLine("</tr>");
            sb2.AppendLine("</thead>");
            sb2.AppendLine("<tbody>");
            foreach (DataRow d in ds.Tables[2].Rows)
            {
                string idInvernadero = d["idInvernadero"].ToString().Trim()
                      , Invernadero = d["claveInvernadero"].ToString().Trim()
                      , Habilidad = d["habilidad"].ToString().Trim()
                      , Cumplimiento = d["Cumplimiento"].ToString().Trim()
                      , Ejecucion = d["Ejecucion"].ToString().Trim();

                sb2.AppendLine(idInvernadero.Equals(string.Empty) && Invernadero.Equals(string.Empty) ? "<tr idInvernadero=\"0\" Invernadero=\"0000\">" : "<tr idInvernadero=\"" + idInvernadero + "\" Invernadero=\"" + Invernadero + "\">");
                sb2.AppendLine(Invernadero.Equals(string.Empty) ? "<td class=\"invernaderoCultivo\">-</td>" : "<td class=\"invernaderoCultivo\">" + Invernadero + "</td>");
                sb2.AppendLine(Habilidad.Equals(string.Empty) ? "<td class=\"habilidadCultivo\">-</td>" : "<td class=\"habilidadCultivo\">" + Habilidad + "</td>");
                sb2.AppendLine(Cumplimiento.Equals(string.Empty) ? "<td class=\"cumplimientoCultivo\">-</td>" : "<td class=\"Cumplimiento\">" + Cumplimiento + "%</td>");
                sb2.AppendLine(Ejecucion.Equals(string.Empty) ? "<td class=\"ejecucionCultivo\">-</td>" : "<td class=\"Ejecucion\">" + Ejecucion + "%</td>");
                sb2.AppendLine("</tr>");
            }
            sb2.AppendLine("</tbody>");


            //COSECHA
            sbb.AppendLine("<thead>");
            sbb.AppendLine("<tr>");
            sbb.AppendLine("<th colspan=\"4\">COSECHA</th>");
            sbb.AppendLine("</tr>");
            sbb.AppendLine("<tr>");
            sbb.AppendLine("<th>INVERNADERO</th>");
            sbb.AppendLine("<th>HABILIDAD</th>");
            sbb.AppendLine("<th>CUMPLIMIENTO</th>");
            sbb.AppendLine("<th>EJECUCION</th>");
            sbb.AppendLine("</tr>");
            sbb.AppendLine("</thead>");
            sbb.AppendLine("<tbody>");
            foreach (DataRow d in ds.Tables[3].Rows)
            {
                string idInvernadero = d["idInvernadero"].ToString().Trim()
                      , Invernadero = d["claveInvernadero"].ToString().Trim()
                      , Habilidad = d["habilidad"].ToString().Trim()
                      , Cumplimiento = d["Cumplimiento"].ToString().Trim()
                      , Ejecucion = d["Ejecucion"].ToString().Trim();

                sbb.AppendLine(idInvernadero.Equals(string.Empty) && Invernadero.Equals(string.Empty) ? "<tr idInvernadero=\"0\" Invernadero=\"0000\">" : "<tr idInvernadero=\"" + idInvernadero + "\" Invernadero=\"" + Invernadero + "\">");
                sbb.AppendLine(Invernadero.Equals(string.Empty) ? "<td class=\"invernaderoCultivo\">-</td>" : "<td class=\"invernaderoCultivo\">" + Invernadero + "</td>");
                sbb.AppendLine(Habilidad.Equals(string.Empty) ? "<td class=\"habilidadCultivo\">-</td>" : "<td class=\"habilidadCultivo\">" + Habilidad + "</td>");
                sbb.AppendLine(Cumplimiento.Equals(string.Empty) ? "<td class=\"cumplimientoCultivo\">-</td>" : "<td class=\"Cumplimiento\">" + Cumplimiento + "%</td>");
                sbb.AppendLine(Ejecucion.Equals(string.Empty) ? "<td class=\"ejecucionCultivo\">-</td>" : "<td class=\"Ejecucion\">" + Ejecucion + "%</td>");
                sbb.AppendLine("</tr>");
            }
            sbb.AppendLine("</tbody>");


            //creando tablas para detalle semamanal de cultivo y cosecha
            
            foreach (DataRow d in ds.Tables[4].Rows)
            {
                string idInvernadero = d["idInvernadero"].ToString().Trim(),
                       Invernadero = d["claveInvernadero"].ToString().Trim(),
                       Habilidad = d["habilidad"].ToString().Trim(),
                       domingoP = d["domingoP"].ToString().Trim(),
                       lunesP = d["lunesP"].ToString().Trim(),
                       martesP = d["martesP"].ToString().Trim(),
                       miercolesP = d["miercolesP"].ToString().Trim(),
                       juevesP = d["juevesP"].ToString().Trim(),
                       viernesP = d["viernesP"].ToString().Trim(),
                       sabadoP = d["sabadoP"].ToString().Trim(),
                       domingoE = d["domingoE"].ToString().Trim(),
                       lunesE = d["lunesE"].ToString().Trim(),
                       martesE = d["martesE"].ToString().Trim(),
                       miercolesE = d["miercolesE"].ToString().Trim(),
                       juevesE = d["juevesE"].ToString().Trim(),
                       viernesE = d["viernesE"].ToString().Trim(),
                       sabadoE = d["sabadoE"].ToString().Trim();

                sb3.AppendLine("<table id=\"" + idInvernadero + "\" class=\"gridView\"");
                sb3.AppendLine("<thead>");
                sb3.AppendLine("<tr>");
                sb3.AppendLine(Invernadero.Equals(string.Empty) ? string.Empty : "<th><span><label>" + Invernadero + "</label></span></th>");
                sb3.AppendLine("<th>Habilidad</th><th>Domingo</th><th>Lunes</th><th>Martes</th><th>Miércoles</th><th>Jueves</th><th>Viernes</th><th>Sábado</th>");
                sb3.AppendLine("</tr>");
                sb3.AppendLine("</thead>");
                sb3.AppendLine("<tbody>");
                sb3.AppendLine("<tr>");
                sb3.AppendLine("<td><b>Planeación</b></td>");
                sb3.AppendLine(Habilidad.Equals(string.Empty) ? "<td>-</td>" : "<td>" + Habilidad + "</td>");
                sb3.AppendLine(domingoP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + domingoP + "</td>");
                sb3.AppendLine(lunesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + lunesP + "</td>");
                sb3.AppendLine(martesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + martesP + "</td>");
                sb3.AppendLine(miercolesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + miercolesP + "</td>");
                sb3.AppendLine(juevesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + juevesP + "</td>");
                sb3.AppendLine(viernesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + viernesP + "</td>");
                sb3.AppendLine(sabadoP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + sabadoP + "</td>");
                sb3.AppendLine("</tr>");
                sb3.AppendLine("<tr>");
                sb3.AppendLine("<td><b>Ejecución</b></td>");
                sb3.AppendLine(Habilidad.Equals(string.Empty) ? "<td>-</td>" : "<td>" + Habilidad + "</td>");
                sb3.AppendLine(domingoE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + domingoE + "</td>");
                sb3.AppendLine(lunesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + lunesE + "</td>");
                sb3.AppendLine(martesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + martesE + "</td>");
                sb3.AppendLine(miercolesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + miercolesE + "</td>");
                sb3.AppendLine(juevesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + juevesE + "</td>");
                sb3.AppendLine(viernesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + viernesE + "</td>");
                sb3.AppendLine(sabadoE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + sabadoE + "</td>");
                sb3.AppendLine("</tr>");
                sb3.AppendLine("</tbody>");
            

          
            }




            //creando tablas para detalle semamanal de cultivo y cosecha

            foreach (DataRow d in ds.Tables[5].Rows)
            {
                string idInvernadero = d["idInvernadero"].ToString().Trim(),
                       Invernadero = d["claveInvernadero"].ToString().Trim(),
                       Habilidad = d["habilidad"].ToString().Trim(),
                       domingoP = d["domingoP"].ToString().Trim(),
                       lunesP = d["lunesP"].ToString().Trim(),
                       martesP = d["martesP"].ToString().Trim(),
                       miercolesP = d["miercolesP"].ToString().Trim(),
                       juevesP = d["juevesP"].ToString().Trim(),
                       viernesP = d["viernesP"].ToString().Trim(),
                       sabadoP = d["sabadoP"].ToString().Trim(),
                       domingoE = d["domingoE"].ToString().Trim(),
                       lunesE = d["lunesE"].ToString().Trim(),
                       martesE = d["martesE"].ToString().Trim(),
                       miercolesE = d["miercolesE"].ToString().Trim(),
                       juevesE = d["juevesE"].ToString().Trim(),
                       viernesE = d["viernesE"].ToString().Trim(),
                       sabadoE = d["sabadoE"].ToString().Trim();

                sb4.AppendLine("<table id=\"" + idInvernadero + "\" class=\"gridView\"");
                sb4.AppendLine("<thead>");
                sb4.AppendLine("<tr>");
                sb4.AppendLine(Invernadero.Equals(string.Empty) ? string.Empty : "<th><span><label>" + Invernadero + "</label></span></th>");
                sb4.AppendLine("<th>Habilidad</th><th>Domingo</th><th>Lunes</th><th>Martes</th><th>Miércoles</th><th>Jueves</th><th>Viernes</th><th>Sábado</th>");
                sb4.AppendLine("</tr>");
                sb4.AppendLine("</thead>");
                sb4.AppendLine("<tbody>");
                sb4.AppendLine("<tr>");
                sb4.AppendLine("<td><b>Planeación</b></td>");
                sb4.AppendLine(Habilidad.Equals(string.Empty) ? "<td>-</td>" : "<td>" + Habilidad + "</td>");
                sb4.AppendLine(domingoP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + domingoP + "</td>");
                sb4.AppendLine(lunesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + lunesP + "</td>");
                sb4.AppendLine(martesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + martesP + "</td>");
                sb4.AppendLine(miercolesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + miercolesP + "</td>");
                sb4.AppendLine(juevesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + juevesP + "</td>");
                sb4.AppendLine(viernesP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + viernesP + "</td>");
                sb4.AppendLine(sabadoP.Equals(string.Empty) ? "<td>-</td>" : "<td>" + sabadoP + "</td>");
                sb4.AppendLine("</tr>");
                sb4.AppendLine("<tr>");
                sb4.AppendLine("<td><b>Ejecución</b></td>");
                sb4.AppendLine(Habilidad.Equals(string.Empty) ? "<td>-</td>" : "<td>" + Habilidad + "</td>");
                sb4.AppendLine(domingoE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + domingoE + "</td>");
                sb4.AppendLine(lunesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + lunesE + "</td>");
                sb4.AppendLine(martesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + martesE + "</td>");
                sb4.AppendLine(miercolesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + miercolesE + "</td>");
                sb4.AppendLine(juevesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + juevesE + "</td>");
                sb4.AppendLine(viernesE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + viernesE + "</td>");
                sb4.AppendLine(sabadoE.Equals(string.Empty) ? "<td>-</td>" : "<td>" + sabadoE + "</td>");
                sb4.AppendLine("</tr>");
                sb4.AppendLine("</tbody>");
           
            }


            foreach (DataRow d in ds.Tables[6].Rows)
            {
                string idInvernadero = d["IdInvernadero"].ToString().Trim()
                      , Invernadero = d["Invernadero"].ToString().Trim()
                      , idHabilidad = d["IdHabilidad"].ToString().Trim()
                      , habilidad = d["habilidad"].ToString().Trim()
                      , diaSemana = d["diaSemana"].ToString().Trim()
                      , surcosProgramados = d["surcosProgramados"].ToString().Trim()
                      , horarioProgramado = d["horarioProgramado"].ToString().Trim()
                      , jornalesProgramados = d["jornalesProgramados"].ToString().Trim();


                //sb5.AppendLine(Invernadero.Equals(string.Empty) ? string.Empty : "<span><label>" + Invernadero + "</label></span>");
                sb5.AppendLine("<table id=\"" + idInvernadero + "\"  class=\"gridView\"  newAttribute=\"EjecucionTable\" ");
                sb5.AppendLine("<thead>");
                sb5.AppendLine("<tr>");
                sb5.AppendLine("<th>Invernadero</th><th>Habilidad</th><th>Dia</th><th>Surcos</th><th>Horario</th><th>Jornales</th>");
                sb5.AppendLine("</thead>");
                sb5.AppendLine("<tbody>");
                sb5.AppendLine("</tr>");
                sb5.AppendLine(idInvernadero.Equals(string.Empty) && Invernadero.Equals(string.Empty) ? "<tr idInvernadero=\"0\" invernadero=\"0000\">" : "<tr idInvernadero=\"" + idInvernadero + "\" Invernadero=\"" + Invernadero + "\">");
                sb5.AppendLine(Invernadero.Equals(string.Empty) ? "<td class=\"Invernadero\">-</td>" : "<td class=\"Invernadero\">" + Invernadero + "</td>");
                sb5.AppendLine(habilidad.Equals(string.Empty) ? "<td class=\"habilidad\">-</td>" : "<td class=\"habilidad\">" + habilidad + "</td>");
                sb5.AppendLine(diaSemana.Equals(string.Empty) ? "<td class=\"diaSemana\">-</td>" : "<td class=\"diaSemana\">" + diaSemana + "</td>");
                sb5.AppendLine(surcosProgramados.Equals(string.Empty) ? "<td class=\"surcosProgramados\">-</td>" : "<td class=\"surcosProgramados\">" + surcosProgramados + "</td>");
                sb5.AppendLine(horarioProgramado.Equals(string.Empty) ? "<td class=\"horarioProgramado\">-</td>" : "<td class=\"horarioProgramado\">" + horarioProgramado + "</td>");
                sb5.AppendLine(jornalesProgramados.Equals(string.Empty) ? "<td class=\"jornalesProgramados\">-</td>" : "<td class=\"jornalesProgramados\">" + jornalesProgramados + "</td>");
                sb5.AppendLine("</tr>");
                sb5.AppendLine("</tbody>");
            }



            foreach (DataRow d in ds.Tables[7].Rows)
            {
                string idInvernadero = d["IdInvernadero"].ToString().Trim()
                      , Invernadero = d["Invernadero"].ToString().Trim()
                      , idHabilidad = d["IdHabilidad"].ToString().Trim()
                      , habilidad = d["habilidad"].ToString().Trim()
                      , diaSemana = d["diaSemana"].ToString().Trim()
                      , surcosTrabajados = d["surcosTrabajados"].ToString().Trim()
                      , horarioTrabajado = d["horarioTrabajado"].ToString().Trim()
                      , jornalesTrabajados = d["jornalesTrabajados"].ToString().Trim();


                //sb6.AppendLine(Invernadero.Equals(string.Empty) ? string.Empty : "<span><label>" + Invernadero + "</label></span>");
                sb6.AppendLine("<table id=\"" + idInvernadero + "\"  class=\"gridView\"  newAttribute=\"PlaneadoTable\" ");
                sb6.AppendLine("<thead>");
                sb6.AppendLine("<tr>");
                sb6.AppendLine("<th>Invernadero</th><th>Habilidad</th><th>Dia</th><th>Surcos</th><th>Horario</th><th>Jornales</th>");
                sb6.AppendLine("</thead>");
                sb6.AppendLine("<tbody>");
                sb6.AppendLine("</tr>");
                sb6.AppendLine(idInvernadero.Equals(string.Empty) && Invernadero.Equals(string.Empty) ? "<tr idInvernadero=\"0\" invernadero=\"0000\">" : "<tr idInvernadero=\"" + idInvernadero + "\" Invernadero=\"" + Invernadero + "\">");
                sb6.AppendLine(Invernadero.Equals(string.Empty) ? "<td class=\"Invernadero\">-</td>" : "<td class=\"Invernadero\">" + Invernadero + "</td>");
                sb6.AppendLine(habilidad.Equals(string.Empty) ? "<td class=\"habilidad\">-</td>" : "<td class=\"habilidad\">" + habilidad + "</td>");
                sb6.AppendLine(diaSemana.Equals(string.Empty) ? "<td class=\"diaSemana\">-</td>" : "<td class=\"diaSemana\">" + diaSemana + "</td>");
                sb6.AppendLine(surcosTrabajados.Equals(string.Empty) ? "<td class=\"surcosTrabajados\">-</td>" : "<td class=\"surcosTrabajados\">" + surcosTrabajados + "</td>");
                sb6.AppendLine(horarioTrabajado.Equals(string.Empty) ? "<td class=\"horarioTrabajado\">-</td>" : "<td class=\"horarioTrabajado\">" + horarioTrabajado + "</td>");
                sb6.AppendLine(jornalesTrabajados.Equals(string.Empty) ? "<td class=\"jornalesTrabajados\">-</td>" : "<td class=\"jornalesTrabajados\">" + jornalesTrabajados + "</td>");
                sb6.AppendLine("</tr>");
                sb6.AppendLine("</tbody>");
            }




            //if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0 && ds.Tables[4].Rows.Count > 0 && ds.Tables[5].Rows.Count > 0 && ds.Tables[6].Rows.Count > 0 && ds.Tables[7].Rows.Count > 0)
            //{
                if (ds.Tables[8].Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "1", "ok", sb.ToString(), sb2.ToString(), sbb.ToString(), sb3.ToString(), sb4.ToString() ,sb5.ToString(), sb6.ToString() };
                }
                else
                {
                    return new string[] { "0", ds.Tables[1].Rows[0]["Mensaje"].ToString(), "warning" };
                }
            }
            //else
            //{
            //    return new string[] { "0", "No existe reporte de Cumplimiento-Ejecución para la semana indicada", "warning" };
            //}
        //}
        catch (Exception x)
        {

            log.Error(x);
            return new string[] { "0", "El proceso no generó ningún resultado", "warning" };
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
}