using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using log4net;

public partial class Lider_frmCapturaTrabajo : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Lider_frmCapturaTrabajo));
    private static string currentFarm;
    private static string sTargetURLForSessionTimeout;
    public string AsociadosLider;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AsociadosLider = Session["Asociados"].ToString();
            AsociadosLider = AsociadosLider.Replace("\\t", "");
            if (Session["MultiplesPlantas"].ToString() == "true")
            {
                currentFarm = this.Master.PlantaSeleccionada;
            }
            else
            {
                currentFarm = Session["Planta"].ToString();
            }
            sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();
        }

    }

    private static string borderColor(string color)
    {
        var bcolor1 = Convert.ToInt32(color.Substring(0, 2), 16);
        var bcolor2 = Convert.ToInt32(color.Substring(2, 2), 16);
        var bcolor3 = Convert.ToInt32(color.Substring(4, 2), 16);

        bcolor1 = (bcolor1 - 30) > 0 ? (bcolor1 - 30) : 0;
        bcolor2 = (bcolor2 - 30) > 0 ? (bcolor2 - 30) : 0;
        bcolor3 = (bcolor3 - 30) > 0 ? (bcolor3 - 30) : 0;

        color = (bcolor1.ToString("X").Length < 2 ? ("0" + bcolor1.ToString("X")) : bcolor1.ToString("X")) + (bcolor2.ToString("X").Length < 2 ? ("0" + bcolor2.ToString("X")) : bcolor2.ToString("X")) + (bcolor3.ToString("X").Length < 2 ? ("0" + bcolor3.ToString("X")) : bcolor3.ToString("X"));
        return color;
    }

    [WebMethod]
    public static string GuardaCaptura(Capturatrabajo[] captura)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        DataTable actividades = Actividades();
        DataTable asociados = Asociados();
        string result = "";
        try
        {
            if (captura.Length > 0)
            {
                foreach (var item in captura)
                {
                    DataRow dr = actividades.NewRow();
                    dr["idActividad"] = item.idActividad;
                    dr["idPeriodo"] = item.idPeriodo;
                    dr["idInvernadero"] = item.idInvernadero;
                    dr["comentarios"] = item.comentarios;

                    foreach (var aso in item.asociados)
                    {
                        DataRow asociado = asociados.NewRow();
                        asociado["idActividad"] = item.idActividad;
                        asociado["idPeriodo"] = item.idPeriodo;
                        asociado["idAsociado"] = aso.idAsociado;
                        asociado["surcoInicio"] = aso.surcoInicio;
                        asociado["surcoFin"] = aso.surcoFin;
                        asociado["horaInicio"] = DateTime.ParseExact(aso.horaInicio, "MM-dd HH:mm", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd HH:mm");
                        asociado["horaFin"] = DateTime.ParseExact(aso.horaFin, "MM-dd HH:mm", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd HH:mm");
                        asociado["realizado"] = aso.realizado;
                        asociado["calidad"] = aso.calidad;
                        asociado["cantidadSurcos"] = aso.cantidadSurcos;

                        asociados.Rows.Add(asociado);
                    }
                    actividades.Rows.Add(dr);
                }
                parameters.Add("@idlider", HttpContext.Current.Session["idUsuario"].ToString());
                parameters.Add("@actividades", actividades);
                parameters.Add("@asociados", asociados);
                DataSet sdsdfdsf = new DataAccess().executeStoreProcedureDataSet("spr_GuardaCapturaTrabajo", parameters);
                result = sdsdfdsf.Tables[0].Rows[0][0].ToString();

            }
            else
            {
                result = "No hay captura de trabajo para guardar";
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
        return result;
    }

    [WebMethod]
    public static string cargaActividadesProgramadas(int idInvernadero, string fecha, int idCiclo)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        var result = string.Empty;
        bool porTiempo;
                DateTime date = string.IsNullOrEmpty(fecha) ? DateTime.Now : DateTime.Parse(fecha);

        try
        {
            var parameters = new Dictionary<String, Object>();


            parameters.Add("@idInvernadero", idInvernadero);
            parameters.Add("@fecha", date);
            //parameters.Add("@fecha", date.ToString("MM/dd/yyyy"));
            parameters.Add("@idCiclo", idCiclo);
            parameters.Add("@idUsuario", HttpContext.Current.Session["idUsuario"].ToString());

            var dt = new DataAccess().executeStoreProcedureDataSet("spr_ActividadesProgramadasCaptura", parameters);

            if (dt.Tables[0].Rows.Count == 0)
            {
                return "0";
            }

            foreach (DataRow hab in dt.Tables[0].Rows)//Agregar propiedades de bd a la actividad en html.
            {
                porTiempo = (hab["PorTiempo"].ToString() == "False");
                hab["capturado"] = string.IsNullOrEmpty(hab["capturado"].ToString()) ? "0" : hab["capturado"].ToString();

                result += "<div idProducto='" + hab["idProducto"] + "' idPeriodo='" + hab["idActividadPeriodo"] + "' actividad='" + hab["idActividad"] + "' class='Actividad " + (hab["asociados"].ToString().Length > 0 ? "enabled" : "disabled") + "'>" +
                   "         <table class='programadas header' id='capt" + hab["idActividad"] + "'>                        " +
                   "             <thead>                                                                                   " +
                   "                 <tr>                                                                                  " +
                   "                     <td rowspan='3'>                                                                  " +
                                            "<div class=\"divHabilidadProgramable\">                                       " +
                   "     <div class=\"btnHabilidad\" style=\"background:#" + hab["Color"] + ";  border-color:#" + borderColor(hab["Color"].ToString()) + ";\"  >" +
                   "        <span class=\"habilidad_icono\">" + hab["NombreCorto"] + "</span>                              " +
                   "        <span class=\"habilidad_descripcion\">(" + hab["NombreHabilidad"] + ")</span>                  " +
                   "        <span class=\"habilidad_etapa \">" + hab["NombreEtapa"] + "</span>                             " +
                   "     </div>                                                                                            " +
                   "</div>                                                                                                 " +
                   "                     </td>                                                                             " +
                   "                     <td colspan='3'>                                                                  " +
                   "                         " + hab["Etapa"] + "                                                          " +
                   "                     </td>                                                                             " +
                   (porTiempo ? "<td><span>Avance:  " + Math.Round(float.Parse(hab["capturado"].ToString()),2) + "%</span></td>" : "") +
                   "<td colspan='" + (porTiempo ? "4" : "5") + "'></td>" +
                   "                 </tr>                                                                                 " +
                   "                 <tr>                                                                                  " +
                   "                     <td align='right'>                                                                " +
                   "                         Del:                                                                          " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         " + DateTime.Parse(hab["inicio"].ToString()).ToString("yyyy-MM-dd hh:mm") + " " +
                   "                     </td>                                                                             " +
                   "                     <td align='right'>                                                                " +
                   (porTiempo ? "Surcos Planeados:" : "&nbsp;") +
                   "                     </td>                                                                             " +
                   "                     <td class='surcos'>                                                                              " +
                   "                         " + hab["surcos"] + "                                                         " +
                   "                     </td>                                                                             " +
                   "                     <td align='right'>                                                                " +
                   "                         Hora inicio:                                                                  " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         <input type='text' class='cajaChica horaInicio required' value='" + DateTime.Parse(hab["inicio"].ToString()).ToString("MM-dd HH:mm") + "' /> " +
                   "                         a:                                                                            " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         <input type='text' class='cajaChica horaFin required' value='" + DateTime.Parse(hab["fin"].ToString()).ToString("MM-dd HH:mm") + "'/>" +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         <input type='button' class='asignarInicioFin' value='Asignar' />                " +
                   "                     </td>                                                                             " +
                   "                 </tr>                                                                                 " +
                   "                 <tr>                                                                                  " +
                   "                     <td align='right'>                                                                " +
                   "                         Al:                                                                           " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         " + DateTime.Parse(hab["fin"].ToString()).ToString("yyyy-MM-dd HH:mm") + "    " +
                   "                     </td>                                                                             " +
                   "                     <td align='right'>                                                                " +
                   (porTiempo ? "Surcos Trabajados:" : "&nbsp;") +
                   "                     </td>                                                                             " +
                   "                     <td class='surcosAcumulados'>                                                     " +
                    "                         " + hab["cantidadTrabajo"] + "                                                         " +
                   "                     </td>                                                                             " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                           &nbsp;                                                                      " +
                   "                     </td>                                                                             " +
                   "                     <td align='right'>                                                                " +
                   "                         Calidad:                                                                      " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         <input type='text' class='cajaChica Calificacion required intValidate' value='9' />       " +
                   "                     </td>                                                                             " +
                   "                     <td>                                                                              " +
                   "                         <input type='button' class='asignarCalidad' value='Asignar' />                " +
                   "                     </td>                                                                             " +
                   "                 </tr>                                                                                 " +
                   "             </thead>                                                                                  " +
                   "         </table>                                                                                      " +
                   "         <table class='captura'>                                                                       " +
                   "             <tr>                                                                                      " +
                   "                 <th colspan='3'>                                                                      " +
                   "                     Asociados                                                                         " +
                   "                 </th>                                                                                 " +
                   "                 <th colspan='2'>                                                                      " +
                   (porTiempo ? "Surco Inicio/Surco Fin" : "Realizado") +
                   "                 </th>                                                                                 " +
                   "                 <th>                                                                                  " +
                   "                     inició                                                                            " +
                   "                 </th>                                                                                 " +
                   "                 <th>                                                                                  " +
                   "                     Terminó                                                                           " +
                   "                 </th>                                                                                 " +
                   "                 <th>                                                                                  " +
                   "                     Calidad                                                                           " +
                   "                 </th>                                                                                 " +
                   "                 <th>                                                                                  " +
                   "                     Cant Surcos                                                                           " +
                   "                 </th>                                                                                 " +
                   "             </tr>                                                                                     ";

                var cantidadAsociados = hab["asociados"].ToString().Substring(0, hab["asociados"].ToString().Length > 0 ? hab["asociados"].ToString().Length - 2 : 0).Split('|').Count();

                var surcosPorAsociado = (int)double.Parse(hab["surcos"].ToString()) / cantidadAsociados;
                var SurcoInicio = 1;
                var SurcoFin = surcosPorAsociado;

                var comentarios = "";


                if (dt.Tables[1].Rows.Count > 0 && hab["realizado"].ToString() == "1")
                {

                    foreach (DataRow user in dt.Tables[1].Rows)
                    {
                        if (hab["idActividadPeriodo"].ToString() == user["idActividadPeriodo"].ToString())
                        {
                            DateTime inicio, fin;
                            DateTime.TryParse(user["horaInicio"].ToString(), out inicio);
                            DateTime.TryParse(user["horaFin"].ToString(), out fin);
                            if (hab["PorTiempo"].ToString() != "False")
                            {
                                comentarios = user["comentarios"].ToString();
                            }

                            result += "" +

                               "<tr idAsociado=" + user["idAsociado"] + " class='enabled' " + " densidad =" + hab["densidad"] + "  totalSurcos=" + hab["totalSurcos"] + ">                                            " +
                               "<td><img src='../comun/img/remove-icon.png' class='removeUser'/></td>" +
                               "            <td idEtapa='" + hab["idEtapa"] + "'>                                             " +
                               "                <span class='idAsociado'>" + user["idAsociado"] + "</span>   " +
                               "            </td>                                            " +
                               "            <td class='nombreAsociado'>                                             " +
                               "                " + user["Nombre"] + "                               " +
                               "            </td>                                            " +
                               "      <td " + (porTiempo ? "" : "colspan='2'") + ">" +
                               (porTiempo ?
                               "                <input type='text' class='cajaChica surco surcoInicio required enabled' value='" + user["surcoInicio"] + "' />    " +
                               "            </td>                                            " +
                               "            <td>                                             " +
                               "                <input type='text' class='cajaChica surco surcoFin required enabled' value='" + user["surcoFin"] + "'/>       "
                               : "<input type='checkbox' class='realizado' checked />Realizado") +
                               "            </td>                                            " +
                               "            <td>                                             " +
                               "                <input type='text' class='cajaChica horaInicio required enabled' value='" + inicio.ToString("MM-dd HH:mm") + "'/>     " +
                               "            </td>                                            " +
                               "            <td>                                             " +
                               "                <input type='text' class='cajaChica horaFin required enabled' value='" + fin.ToString("MM-dd HH:mm") + "'/>        " +
                               "            </td>                                            " +
                               "            <td>                                             " +
                               "                <input type='text' class='cajaChica Calidad  enabled intValidate' value='" + user["calidad"] + "'/>        " +
                               "            </td>                                            " +
                                 "            <td>                                             " +
                                "                <input type='text' class='cajaChica CantidadSurcos required enabled intValidate' value='" + user["cantidadSurcos"] + "'" + (porTiempo ? "" : "disabled") + "/>        " +
                                "            </td>                                            " +

                               "            <td>                                             " +
                               "               <img src='../comun/img/add-icon.png' class='addRow' />                " +
                               "            </td>                                            " +
                               "        </tr> ";
                        }
                    }
                }
                else
                {
                    if (hab["asociados"].ToString().Length > 0)
                    {
                        foreach (DataRow user in dt.Tables[1].Rows)
                        {
                            if (hab["idActividadPeriodo"].ToString() == user["idActividadPeriodo"].ToString())
                            {
                                var id = user["idAsociado"].ToString();
                                var nombre = user["Nombre"].ToString();
                                DateTime inicio, fin;
                                DateTime.TryParse(hab["inicio"].ToString(), out inicio);
                                DateTime.TryParse(hab["fin"].ToString(), out fin);

                                result += "" +

                                "<tr idAsociado=" + id + " class='enabled' " + " densidad =" + hab["densidad"] + "  totalSurcos=" + hab["totalSurcos"] + ">                                           " +
                                "<td><img src='../comun/img/remove-icon.png' class='removeUser'/></td>" +
                                "            <td idEtapa='" + hab["idEtapa"] + "'>                                             " +
                                "                <span class='idAsociado'>" + id + "</span>                            " +
                                "            </td>                                            " +
                                 "            <td class='nombreAsociado'>                                             " +
                                "                " + nombre + "                               " +
                                "            </td>                                            " +
                                "      <td " + (porTiempo ? "" : "colspan='2'") + ">" +
                                (porTiempo ?
                                "                <input type='text' class='cajaChica surco surcoInicio required enabled intValidate' value='" + (user["surcoInicio"].ToString().Equals("") ? SurcoInicio.ToString() : user["surcoInicio"].ToString()) + "' />    " +
                                "            </td>                                            " +
                                "            <td>                                             " +
                                "                <input type='text' class='cajaChica surco surcoFin required enabled intValidate' value='" + (user["surcoFin"].ToString().Equals("") ? SurcoFin.ToString() : user["surcoFin"].ToString()) + "'/>       "
                                : "<input type='checkbox' class='realizado' checked />Realizado") +
                                "            </td>                                            " +
                                "            <td>                                             " +
                                "                <input type='text' class='cajaChica horaInicio required enabled' value='" + inicio.ToString("MM-dd HH:mm") + "'/>     " +
                                "            </td>                                            " +
                                "            <td>                                             " +
                                "                <input type='text' class='cajaChica horaFin required enabled' value='" + fin.ToString("MM-dd HH:mm") + "'/>        " +
                                "            </td>                                            " +
                                "            <td>                                             " +
                                "                <input type='text' class='cajaChica Calidad  enabled intValidate' />        " +
                                "            </td>                                            " +
                                "            <td>                                             " +
                                "                <input type='text' class='cajaChica CantidadSurcos required enabled intValidate'  value='" + user["cantidadSurcos"] + "'" + "'" + (porTiempo ? "" : "0") + "'" + (porTiempo ? "" : "disabled") + "  />        " +
                                "            </td>                                            " +

                                "            <td>                                             " +
                                "               <img src='../comun/img/add-icon.png' class='addRow' />                " +
                                "            </td>                                            " +
                                "        </tr> ";
                                SurcoInicio = SurcoInicio + surcosPorAsociado;
                                SurcoFin = SurcoFin + surcosPorAsociado;
                            }
                        }
                    }
                    else
                    {
                        result += "<tr><td colspan='8'>No se Registraron Asociados para esta actividad</td></tr>";
                    }
                }

                result += (porTiempo ? "<tr  style='height:45px;'><td colspan='8'></td>"
                : "        <tr style='height:60px;'>" +
                    "           <td> Comentarios</td>" +
                    "           <td colspan='7'><textarea rows='4' type='text' class='comentarios'>" + comentarios + "</textarea></td>") +
                    ((hab["PorTiempo"].ToString() == "False") ? "<td class='" + (float.Parse(hab["capturado"].ToString()) >= 100 ? "capturado" : "") + "'><span  >  " + float.Parse(hab["capturado"].ToString()) + "%</span></td>" : "<td  class='" + (hab["realizado"].ToString() == "1" ? "realizado" : "") + "'><span></span></td>") +

                    "        </tr>" +
                    "<tr><th colspan='9' class='separator'></th></tr>" +
                    "</table></div>";

            }
            return result;
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('No se pudo cargar las actividades programadas','error');</script>";
        }
    }

    [WebMethod]
    public static string getDensidadSurcos(int invernadero)
    {
        string result = "";
        DataTable dt = new DataTable();

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@idInvernadero", invernadero);

        dt = new DataAccess().executeStoreProcedureDataTable("spr_DensidadSurcosInvernadero", parameters);

        result = GetDataTableToJson(dt);


        return result;
    }

    [WebMethod]
    public static string ContadorProgresoActividades(DateTime date)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);
        string result = "[";
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@fecha", date);
        parameters.Add("@idLider", HttpContext.Current.Session["idUsuario"].ToString());
        DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_ActividadesRealizadasProgreso", parameters);
        try
        {
            for (var row = 0; row < dt.Rows.Count; row++)
            {
                result += "{\"progreso\":\"" + dt.Rows[row]["progreso"] + "\",\"idInvernadero\":" + dt.Rows[row]["idInvernadero"] + ",\"idCiclo\":" + dt.Rows[row]["idCiclo"] + " }";
                if (row < dt.Rows.Count - 1)
                {
                    result += ",";
                }
            }
        }
        catch (Exception ex)
        {

            Log.Error(ex.Message);
        }
        return result + "]";
    }

    [WebMethod]
    public static string cargaInvernaderosSlider()
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);
        var result = "";
        try
        {
            var dt = new DataTable();

            dt = new DataAccess().executeStoreProcedureDataTable("spr_InvernaderosParaProgramacionSemanal", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });

            foreach (DataRow inv in dt.Rows)
            {
                result = result + "<div class=\"divInvernadero\" idInvernadero='" + inv["idInvernadero"] + "' product='" + inv["Product"] + "(" + inv["Variety"] + ")' fechaPlantacion='" + inv["PlantDate"] + "' semana=\"" + (inv["Week"].ToString().Substring(5, 2)) + "\" densidad='" + inv["Density"] + "' terminado='" + inv["complete"] + "' surcos='" + inv["surcos"] + "' idCiclo='" + inv["idCiclo"] + "' >" + inv["ClaveInvernadero"] + "<span class='avance'></span></div>";
            }

        }
        catch (Exception es)
        {
            log.Error(es);
        }
        return result;
    }

    public static DataTable Actividades()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("comentarios");

        return dt;
    }

    public static DataTable Asociados()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcoFin");
        dt.Columns.Add("horaInicio");
        dt.Columns.Add("horaFin");
        dt.Columns.Add("realizado");
        dt.Columns.Add("calidad");
        dt.Columns.Add("cantidadSurcos");

        return dt;
    }
}