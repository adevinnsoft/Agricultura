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
using System.Xml;
using System.Web.Script.Serialization;
using System.Collections;

public partial class Lider_frmProgramacionSemanal : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Lider_frmProgramacionSemanal));
    private static string currentFarm;
    private static string sTargetURLForSessionTimeout;
    public static string horaInicio, horaFin, minutoInicio, minutoFin, diaInicioDeSemana;
    public string idiomaCalendario;
    public static float Absentism = 0.0F;
    public string AsociadosLider, Familias;
    public static string fumigacion, cosecha, preparacionSuelos, tecnologias, podayvuelta, limpieza, deshoje;
    public static string razonesDeEliminacion;
    private static int IdUsuario = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        currentFarm = this.Master.PlantaSeleccionada;
        sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();



        if (Page.IsPostBack)
        {
            
        }
        else
        {
            try
            {

                HttpContext.Current.Session["idUsuarioTemp"] = HttpContext.Current.Session["idUsuario"];
                HttpContext.Current.Session["idEmpleadoTemp"] = HttpContext.Current.Session["idEmpleado"];

                if (HttpContext.Current.Session["idUsuario"] == null)
                    Response.Redirect(sTargetURLForSessionTimeout);

                AsociadosLider = HttpContext.Current.Session["Asociados"].ToString();
                Familias = HttpContext.Current.Session["Familias"].ToString();

                Absentism = 2.0F;
                //foreach (DataRow dr in dataaccess.executeStoreProcedureDataTable("spr_AbsentismoAnioObtiene", new Dictionary<string, object>() { { "@anio", DateTime.Now.Year } }).Rows)
                //{
                //    float count;
                //    float.TryParse(dr["Ausentismo"].ToString(), out count);
                //    Absentism += count;
                //}

                DataTable actividades = dataaccess.executeStoreProcedureDataTable("spr_ActividadesIds", new Dictionary<string, object>() { { "@idplanta", Session["idPlanta"] }, { "@idUsuario", Session["idUsuarioTemp"] } });

                fumigacion = actividades.Rows[0]["fumigacion"].ToString().Replace("</f><f>", "','").Replace("<f>", "['").Replace("</f>", "']");
                cosecha = actividades.Rows[0]["Cosecha"].ToString().Replace("</c><c>", "','").Replace("<c>", "['").Replace("</c>", "']");
                preparacionSuelos = actividades.Rows[0]["PS"].ToString().Replace("</p><p>", "','").Replace("<p>", "['").Replace("</p>", "']");
                limpieza = actividades.Rows[0]["LM"].ToString().Replace("</l><l>", "','").Replace("<l>", "['").Replace("</l>", "']");
                podayvuelta = actividades.Rows[0]["PV"].ToString().Replace("</pv><pv>", "','").Replace("<pv>", "['").Replace("</pv>", "']");
                deshoje = actividades.Rows[0]["DH"].ToString().Replace("</d><d>", "','").Replace("<d>", "['").Replace("</d>", "']");
                tecnologias = tecnologiasObtener();

                razonesDeEliminacion = obtenerRazonesDeEliminacion();

                Absentism = (Absentism * 100);
                idiomaCalendario = Session["Locale"].ToString() == "es-MX" ? "es" : "en";
                DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_HorarioDePlanta", new Dictionary<string, object>() { 
                    {"@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]}
                });
                if (dt.Rows.Count > 0)
                {
                    horaInicio = dt.Rows[0]["horaInicio"].ToString();
                    horaFin = dt.Rows[0]["horaFin"].ToString();
                    minutoInicio = dt.Rows[0]["minutoInicio"].ToString();
                    minutoFin = dt.Rows[0]["minutoFin"].ToString();
                    diaInicioDeSemana = dt.Rows[0]["diaInicioSemana"].ToString();

                    horaInicio = horaInicio.Length < 2 ? "0" + horaInicio : horaInicio;
                    horaFin = horaFin.Length < 2 ? "0" + horaFin : horaFin;
                    minutoInicio = minutoInicio.Length < 2 ? "0" + minutoInicio : minutoInicio;
                    minutoFin = minutoFin.Length < 2 ? "0" + minutoFin : minutoFin;
                }
                else
                {
                    horaInicio = string.Empty;
                    horaFin = string.Empty;
                    minutoInicio = string.Empty;
                    minutoFin = string.Empty;
                    diaInicioDeSemana = string.Empty;
                    //TODO: Mostrar Error
                }


            }
            catch (Exception x)
            {
                log.Error(x.Message);
            }
        }
    }
   
   
    public static string tecnologiasObtener()
    {
        string result = "";
        DataTable dt = new DataTable();
        dt = new DataAccess().executeStoreProcedureDataTable("spr_tecnologiasObtener", null);

        result = GetDataTableToJson(dt);

        return result;
    }

    [System.Web.Services.WebMethod]
    public static string ObtieneInvernaderosProgramadosWeb(int semana, int anio)
    {
       string invernaderosProgrmados= obtieneInvernaderosProgramados(semana, anio);
       return invernaderosProgrmados;
   

    }
    [System.Web.Services.WebMethod]
    public static string ObtieneSemanaAnioProgramadosWeb(int semana, int anio)
    {
        string semanaAnio = obtieneSemanaAnio(semana, anio);
        return semanaAnio;


    }
    [System.Web.Services.WebMethod]
    public static string ObtieneInvernaderosProgramadosPorIdInvernadero(int semana, int anio, string idInvernadero)
    {
        string invernaderosNOProgrmados = obtieneInvernaderosSINProgramacionPorInvernadero(semana, anio, Convert.ToInt32( idInvernadero));
        return invernaderosNOProgrmados;


    }
    [System.Web.Services.WebMethod]
    public static string ObtieneInvernaderosSINProgramadosWeb(int semana, int anio)
    {
        string invernaderosNOProgrmados = obtieneInvernaderosSINProgramacion(semana, anio);
       return invernaderosNOProgrmados;
   
    }

    

    [WebMethod]
    public static string cargaInvernaderosSlider()
    {
        if (HttpContext.Current.Session["idUsuarioTemp"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        var result = "";
        try
        {
            var dt = new DataTable();

                dt = new DataAccess().executeStoreProcedureDataTable("spr_InvernaderosParaProgramacionSemanal", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuarioTemp"] }, { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });

            foreach (DataRow inv in dt.Rows)
            {
                result += string.Format("<div class=\"divInvernadero\" id='{0}' product='{1}({2})' cortes='{12}' fechaPlantacion='{3}' esInterplanting=\"{11}\" invernadero=\"{9}"+(inv["esInterplanting"].ToString()=="1"?"-1":"")+"\" semana=\"{4}\" densidad='{5}' terminado='{6}' surcos='{7}' idCiclo='{8}' >{9}<br/> {10} {11}</div>"
                            , inv["idInvernadero"], inv["Product"], inv["Variety"], inv["PlantDate"], (inv["Week"].ToString().Substring(5, 2)), inv["Density"], inv["complete"], inv["surcos"], inv["idCiclo"], inv["ClaveInvernadero"], inv["Experiment"], inv["esInterplanting"], inv["cortes"]);
            }

        }
        catch (Exception es)
        {
            log.Error(es.Message);
        }
        return result;

    }

    [WebMethod]
    public static string cargaEquiposTrabajo( String idCiclo, String idEtapa)
    {
        string result = "";
        DataSet ds = new DataSet();
        DataTable Equipos, Asociados, grupo;
        Dictionary<string, object> para = new Dictionary<string, object>();
        para.Add("@idEmpleado", HttpContext.Current.Session["idEmpleadoTemp"].ToString());
        para.Add("@idPlanta",HttpContext.Current.Session["idPlanta"].ToString());
        para.Add("@idEtapa", idEtapa);
        para.Add("@idCiclo", idCiclo);
        try
        {
            ds = new DataAccess().executeStoreProcedureDataSet("spr_EquipoTrabajoProgramacionSemanal", para);

            para = new Dictionary<string, object>();
            var i = 0;

            if (null != ds)
            {

                Equipos = ds.Tables[0];
                Asociados = ds.Tables[1];
                grupo = new DataTable();

                foreach (DataColumn c in Asociados.Columns)
                {
                    grupo.Columns.Add(c.ColumnName);
                }
                
                Equipos.Columns.Add("Asociados");
                foreach (DataRow dr in Equipos.Rows)
                {
                    foreach (DataRow da in Asociados.Rows)
                    {
                        if (dr["idEquipoTrabajo"].ToString().Equals(da["idEquipo"].ToString()))
                        {
                            grupo.Rows.Add(da.ItemArray);
                        }
                    }
                    dr["Asociados"] = GetDataTableToJson(grupo);
                    grupo.Clear();
                }

                Equipos.TableName = "Equipos";

                result = GetDataTableToJson(Equipos).Replace("\\", String.Empty).Replace("]\"","]").Replace("\"[","[");
            }

                        
        }
        catch (Exception ex)
        {
            log.Error(ex);
            result = "Error al obtener los equipos, intente de nuevo.";
        }
        return result;
    }

    private static string tablasEquiposTrabajo(int i)
    {
        switch (i)
        {
            case 0:
                return "Equipos";

            case 1:
                return "Asociados";
                
            default:
                return "";
        }
    }

    [WebMethod]
    public static string ObtieneJornalesAutorizados(int semana, int anio)
    {
        string result = "";
        DataTable dt = new DataTable(); 
        Dictionary<string, object> para = new Dictionary<string, object>();
        string lider;
        if (HttpContext.Current.Session["idRole"].ToString() == "2" || HttpContext.Current.Session["idRole"].ToString() == "0" || HttpContext.Current.Session["idRole"].ToString() == "4")
        {
            lider = obtieneListaLideres();
        }
        else
        lider = HttpContext.Current.Session["Nombre"].ToString();

        para.Add("@idEmpleado", HttpContext.Current.Session["idEmpleadoTemp"].ToString());
        para.Add("@semana", semana);
        para.Add("@anio", anio);
        try
        {
            dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtieneJornalesAutorizados", para);

            if(dt.Rows.Count>0)
            result = "Tienes "+dt.Rows[0]["TotalFinal"].ToString() + " jornales Aprobados.|" + lider; 
            else
                result= "No hay registro de Jornales Aprobados.|" + lider; 
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            result = "[{\"NoData\":\"NoData\"}]";
            throw;
        }

        return result;
    }

    [WebMethod]
    public static void changeUser(int idUser, int idLider)
    {

        HttpContext.Current.Session["idEmpleadoTemp"] = idLider;
        HttpContext.Current.Session["idUsuarioTemp"] = idUser;
    }

    [WebMethod]
    public static string FechasOficiales(int idciclo, int invernadero, int departamento)
    {
        var result = "[ ";
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@invernadero", invernadero);
        parameters.Add("@lider", HttpContext.Current.Session["idEmpleadoTemp"].ToString());
        parameters.Add("@departamento", departamento);

        try
        {
            foreach (DataRow data in new DataAccess().executeStoreProcedureDataTable("spr_FechasOficialesProgramacionSemanal", parameters).Rows)
            {
                switch (data["TipoRepeticion"].ToString())
                {
                    case "1":

                    case "2":
                        result += "{\"title\":\"" + data["Evento"] + "\",\"id\":\"Cal\",\"idCiclo\":"+idciclo+",\"Calendar\":true,\"start\":\"" + data["horaInicio"] + "\",\"end\":\"" + data["horaFin"] + "\", \"dow\":" + data["dias"] + ",\"editable\":false,\"stick\":true,\"idInvernadero\":\"" + invernadero + "\"},";
                        break;

                    case "3":
                    case "4":
                        result += "{\"title\":\"" + data["Evento"] + "\",\"id\":\"Cal\",\"idCiclo\":" + idciclo + ",\"Calendar\":true,\"start\":\"" + DateTime.Parse(data["fecha"].ToString()).ToString("yyyy-MM-dd") + " " + data["horaInicio"] + "\",\"end\":\"" + DateTime.Parse(data["fecha"].ToString()).ToString("yyyy-MM-dd") + " " + data["horaFin"] + "\", \"editable\":false,\"stick\":true,\"idInvernadero\":\"" + invernadero + "\"},";
                        break;

                    default:
                        break;

                }
            }
        }
        catch (Exception x)
        {
            log.Error(x.Message);
        }
        result = result.Substring(0, result.Length - 1);
        result += "]";
        return result;
    }

    [WebMethod]
    public static string actividadDetalle(int idInvernadero, int idHabilidad, DateTime inicio, int surcos, string directriz, string id)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);
        ActividadObject Actividad = new ActividadObject(HttpContext.Current.Session["idPlanta"].ToString(), idInvernadero, HttpContext.Current.Session["idUsuarioTemp"].ToString(), idHabilidad, inicio, inicio.AddHours(2), surcos, (directriz == "0" ? false : true), int.Parse(id));
        return Actividad.html;
    }

    [WebMethod]
    public static int ObtieneSemanaNS(DateTime date)
    {
        int result = 0;
        try
        {
            result = new DataAccess().executeStoreProcedureGetInt("spr_ObtieneSemanaNS", new Dictionary<string, object>() { { "@fecha", date } });
        }
        catch (Exception x)
        {
            log.Error(x.Message);
        }

        return result;
    }

    [WebMethod]
    public static string InicioFinSemana(int semana, int anio)
    {
        string result = "";

        try
        {
            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_SemanaNS", new Dictionary<string, object>() { { "@semana", semana }, { "@anio", anio } });
            dt.Rows[0][0] = DateTime.Parse(dt.Rows[0][0].ToString()).AddHours(6);


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
    public static string AusenciasAsociados(int semana, int anio)
    {
        string result = "";
        DataTable dt;
        try
        {
            dt = new DataAccess().executeStoreProcedureDataTable("spr_AusenciasAsociados", new Dictionary<string, object>() { { "@semana", semana }, { "@anio", anio }, { "@idLider", HttpContext.Current.Session["idUsuarioTemp"].ToString() } });

            result = GetDataTableToJson(dt);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        return result;
    }

    [WebMethod]
    public static string cargaHabilidadesDirectriz(int idInvernadero, string fechaPlantacion, int semana, int anio, int ciclo)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        var result = string.Empty;
        var json = "[";

        try
        {
            var parameters = new Dictionary<String, Object>();
            parameters.Add("@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]);
            parameters.Add("@idDepartamento", HttpContext.Current.Session["idDepartamento"].ToString());
            parameters.Add("@idioma", HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0);
            parameters.Add("@idInvernadero", idInvernadero);
            parameters.Add("@fechaPlantacion", DateTime.Parse(fechaPlantacion));
            parameters.Add("@semana", semana);
            parameters.Add("@anio", anio);
            parameters.Add("@idciclo", ciclo);
            parameters.Add("@idlider", HttpContext.Current.Session["idUsuarioTemp"].ToString());

            var dt = new DataAccess().executeStoreProcedureDataTable("spr_HabilidadesDirectrizProgramacionSemanalSaved", parameters);

            if (dt.Columns.Contains("Repeticiones"))
            {
                int time = int.Parse(horaInicio);
                int cont = 0;
                DateTime inic;
                foreach (DataRow hab in dt.Rows)//Agregar propiedades de bd a la actividad en html.
                {

                    for (int i = 0; i < decimal.Parse(hab["Repeticiones"].ToString()); i++)
                    {
                        time = time + i;
                        inic = DateTime.Parse(hab["semanaInicio"].ToString());
                        inic = inic.AddHours(time);

                        //result += "<tr act='N' anio=\"" + anio + "\" semanaInicio=\"" + DateTime.Parse(hab["semanaInicio"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\" semanaFin=\"" + DateTime.Parse(hab["semanaFin"].ToString()).AddMinutes(1439).ToString("yyyy-MM-dd HH:mm") + "\" editable=true semana=\"" + semana + "\" idHabilidad=\"" + hab["idHabilidad"] + "\" portiempo=\"" + hab["SoloEjecutable"] + "\" target=\"" + hab["target"] + "\" plantasPorSurco=\"" + hab["porSurco"] + "\" idCiclo=\"" + hab["idCiclo"] + "\"  idInvernadero=\"" + idInvernadero + "\" contador=\"directriz_" + time + "\" color=\"" + hab["Color"] + "\" class=\"habilidadDeDirectriz saved\" densidad=\"" + hab["Densidad"] + "\">" +
                        //          "<td><span class=\"invisible idEtapa\">" + hab["IdEtapa"] + "</span> <span class=\"invisible idHabilidad\">" + hab["idHabilidad"] + "</span><img class=\"starImage\" src=\"../comun/img/star.png\" />" + hab["NombreHabilidad"].ToString() + " - " + hab["NombreEtapa"].ToString() + (!string.IsNullOrEmpty(hab["Elemento"].ToString()) ? ("</br><select class='elementos cajaChica'><option value='1'>1</option><option value='2'>2</option><option value='3'>3</option></select> " + hab["Elemento"].ToString()) : "") + "</td>" +
                        //          "<td class=\"switchA\"><input type=\"text\" class=\"fechaInicio\" value=\"" + inic.AddHours(cont).ToString("yyyy-MM-dd HH:mm") + "\"/></td>" +
                        //          "<td class=\"switchA\"><input type=\"text\" class=\"fechaFin\" value=\"" + inic.AddHours(++cont).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm") + "\" /></td>" +
                        //          "<td class=\"switchA\"><input type=\"text\" class=\"surcos intValidate\" value=\"" + hab["surcos"] + "\"/><span> /" + hab["surcos"] + "</span></td>" +
                        //          //"<td class=\"switchA target\">" + hab["Target"] + "</td>" +
                        //          "<td class=\"switchA tiempoEstimado\">Seleccione Asociados</td>" +
                        //          "<td class=\"switchA jornalesEstimados\">0</td>" +
                        //          "<td class=\"switchA jornales\"><h3>0</h3><ul>" +
                        //          "</ul></td>" +
                        //          "<td class=\"switchB razon\">" + razonesDeEliminacion + "</td>" +
                        //          "<td class=\"switchB comentario\"><textarea></textarea></td>" +
                        //          "<td class=\"switchA\"><img class='hint' src=\"../comun/img/remove.ico\" title ='No progaramar esta Actividad'  onclick=\"eliminarHabilidadProgramada($(this));\" /></td>" +
                        //          "<td class=\"switchB\"><img class='hint' src=\"../comun/img/goback.png\" title ='Programar esta actividad' onclick=\"RegresaTareaDirectriz($(this));\" /></td>" +
                        //          "</tr>";

                        json += "{\"anio\":" + anio +
                            ",\"semanaInicio\":\"" + DateTime.Parse(hab["semanaInicio"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"semanaFin\":\"" + DateTime.Parse(hab["semanaFin"].ToString()).AddMinutes(1439).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"semana\":" + semana +
                            ",\"invernadero\":\"" + hab["GreenHouse"] + "\"" +
                            ",\"idHabilidad\":" + hab["idHabilidad"] +
                            ",\"idEtapa\":" + hab["idEtapa"] +
                            ",\"porTiempo\":" + hab["SoloEjecutable"].ToString().ToLower() +
                            ",\"target\":" + hab["target"] +
                            ",\"plantasPorSurco\":" + hab["porSurco"] +
                            ",\"idCiclo\":" + hab["idCiclo"] +
                            ",\"editable\":true" +
                            ",\"idInvernadero\":" + idInvernadero +
                            ",\"idTr\":\"directriz_" + idInvernadero + "" + cont +
                            "\",\"directriz\":\"true\"" +
                            ",\"title\":\"" + hab["GreenHouse"] + ":" + hab["nombreHabilidad"] + " - " + hab["NombreEtapa"] + " " + hab["Product"] + "(" + hab["CodigoVariedad"] + ") n."+hab["numeroactividad"] + "\""+
                            ",\"backgroundColor\":\"#" + hab["color"] +
                            "\",\"densidad\":" + hab["Densidad"] +
                            ",\"nombreHabilidad\":\"" + hab["NombreHabilidad"] +
                            "\",\"nombreEtapa\":\"" + hab["NombreEtapa"] +
                            "\",\"act\":\"N" +
                            "\",\"comentario\":\"" + hab["comentarioPeriodo"].ToString().Trim() +
                            "\",\"elemento\":\"" + hab["Elemento"] +
                            "\",\"numeroElementos\":\"" + hab["numeroElementos"] +
                            "\",\"cantidadElemento\":\"1"+
                            "\",\"start\":\"" + inic.AddHours(cont).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"end\":\"" + inic.AddHours(++cont).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"surcosT\":" + hab["surcos"] +
                            ",\"surcos\":" + hab["surcos"] +
                            ",\"edad\":" + hab["edad"] +
                            ",\"esColmena\":\"" + hab["esColmena"] +
                            "\",\"aceptaColmena\":\"" + hab["aceptacolmena"] +
                            "\",\"surcoInicio\":" + hab["surcoInicio"] +
                            ",\"surcoFin\":" + hab["surcofin"] +
                            ",\"numeroactividad\":\"" + hab["numeroactividad"] +
                            "\",\"Asociados\":[" + (hab["Asociados"].ToString().Length == 0 ? "" : hab["Asociados"].ToString().Substring(0, hab["Asociados"].ToString().Length - 1)) + "]" +
                            ",\"razonesDirectriz\":\"" + razonesDeEliminacion.Replace('\"', '\'') + "\""+
                            ",\"UUID\":\""+hab["UUID_Act"]+"\"},";

                    }
                    
                }
                if (dt.Rows.Count > 0)
                    result = json.Substring(0, json.Length - 1) + "]";
                else
                    result = string.Empty;
                return result;
            }
            else
            {
                foreach (DataRow hab in dt.Rows)//Agregar propiedades de bd a la actividad en html.
                {

                    //result += "<tr act='U' anio=\"" + anio + "\" semana=\"" + semana + "\" semanaInicio=\"" + DateTime.Parse(hab["semanaInicio"].ToString()).ToString("yyyy-MM-dd HH:mm") + 
                    //    "\" semanaFin=\"" + DateTime.Parse(hab["semanaFin"].ToString()).AddMinutes(1439).ToString("yyyy-MM-dd HH:mm") + "\" idHabilidad=\"" + hab["idHabilidad"] + "\" editable=\"" + 
                    //    hab["Editable"] + "\" portiempo=\"" + hab["SoloEjecutable"] + "\" target=\"" + hab["target"] + "\" plantasPorSurco=\"" + hab["porSurco"] + "\" idActividad=\"" + hab["idActividad"] + 
                    //    "\" idCiclo=\"" + hab["idCiclo"] + "\"  idInvernadero=\"" + idInvernadero + "\" contador=\"directriz_" + hab["idActividad"] + "\" color=\"" + hab["Color"] + "\" " + 
                    //    (hab["Directriz"].ToString() == "1" ? "class=\"habilidadDeDirectriz\"" : "class=\"normal\"") + " programado=\"" + hab["programada"] + "\" densidad=\"" + hab["Densidad"] + "\">" +
                    //    "<td><span class=\"invisible idEtapa\">" + hab["IdEtapa"] + "</span><span class=\"invisible idHabilidad\">" + hab["idHabilidad"] + "</span>" + 
                    //    (hab["Directriz"].ToString() == "1" ? " <img class=\"starImage\" src=\"../comun/img/star.png\" />" : "") + hab["NombreHabilidad"].ToString() + " - " + 
                    //    hab["NombreEtapa"].ToString() + (!string.IsNullOrEmpty(hab["Elemento"].ToString()) ? ("</br><select class='elementos cajaChica' ><option " + (hab["cantidadDeElementos"].ToString() == "1" ? "selected" : "") + 
                    //    " value='1'>1</option><option " + (hab["cantidadDeElementos"].ToString() == "2" ? "selected" : "") + " value='2'>2</option><option " + (hab["cantidadDeElementos"].ToString() == "3" ? "selected" : "") + 
                    //    " value='3'>3</option></select>" + hab["Elemento"].ToString()) : "") + "</td>" +
                    //            "<td class=\"switchA\"><input type=\"text\" class=\"fechaInicio\" value=\"" + DateTime.Parse(hab["inicioProgramado"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\"/></td>" +
                    //            "<td class=\"switchA\"><input type=\"text\" class=\"fechaFin\" value=\"" + DateTime.Parse(hab["finProgramado"].ToString()).ToString("yyyy-MM-dd HH:mm") + "\" /></td>" +
                    //            "<td class=\"switchA\"><input type=\"text\" class=\"surcos intValidate\" value=\"" + hab["surcosp"] + "\"/><span> /" + hab["surcos"] + "</span></td>" +
                    //    //"<td class=\"switchA target\">" + hab["Target"] + "</td>" +
                    //            "<td class=\"switchA " + (hab["idHabilidad"].ToString() == fumigacion ? "tiempoReentrada" : "tiempoEstimado") + "\">" + hab["minutosEstimados"] + " minutos Reentrada. "+(int.Parse(hab["minutosEstimados"].ToString()).ToString("0.00")) +" Horas</td>" +
                    //            "<td class=\"switchA jornalesEstimados\">0</td>" +
                    //            "<td class=\"switchA jornales\" asociados='[" +(!string.IsNullOrEmpty(hab["Asociados"].ToString())? hab["Asociados"].ToString().Substring(0, hab["Asociados"].ToString().Length - 1):"") + "]'><h3>0</h3><ul>" +
                    //            "</ul></td>" +
                    //            "<td class=\"switchB razon\">" + razonesDeEliminacion + "</td>" +
                    //            "<td class=\"switchB comentario\" selected =" + hab["razon"] + "><textarea>" + hab["comentario"] + "</textarea></td>" +
                    //            "<td class=\"switchA\"><img class='hint' src=\"../comun/img/remove.ico\" title ='No progaramar esta Actividad'  onclick=\"eliminarHabilidadProgramada($(this));\" /></td>" +
                    //            "<td class=\"switchB\"><img class='hint' src=\"../comun/img/goback.png\" title ='Programar esta actividad' onclick=\"RegresaTareaDirectriz($(this));\" /></td>" +
                    //            "</tr>";

                    json += "{\"anio\":" + anio +
                            ",\"semanaInicio\":\"" + DateTime.Parse(hab["semanaInicio"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"semanaFin\":\"" + DateTime.Parse(hab["semanaFin"].ToString()).AddMinutes(1439).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"semana\":" + semana +
                            ",\"programada\":" + hab["programada"] +
                            ",\"invernadero\":\"" + hab["GreenHouse"] + "\"" +
                            ",\"idHabilidad\":" + hab["idHabilidad"] +
                            ",\"idActividad\":" + hab["idActividadPrograma"] +
                            ",\"idPeriodo\":" + hab["idActividadPeriodo"] +
                            ",\"idEtapa\":" + hab["idEtapa"] +
                            ",\"porTiempo\":" + hab["SoloEjecutable"].ToString().ToLower() +
                            ",\"target\":" + hab["target"] +
                            ",\"plantasPorSurco\":" + hab["porSurco"] +
                            ",\"enviado\":" + hab["enviado"]+
                            ",\"idCiclo\":" + hab["idCiclo"] +
                            ",\"editable\":" + hab["Editable"].ToString().ToLower() +                           
                            //",\"editable\":true" +
                            ",\"idInvernadero\":" + idInvernadero +
                            ",\"idTr\":\"directriz_" + hab["idActividadPrograma"] +
                            "\",\"directriz\":\"" + hab["Directriz"].ToString().ToLower() + "\"" +
                            ",\"title\":\"" + hab["GreenHouse"] + ":" + hab["nombreHabilidad"] + " - " + hab["NombreEtapa"] + " " + hab["Product"] + "(" + hab["CodigoVariedad"] + ") n." + hab["numeroactividad"] +"- SURCOS:"+ hab["surcosp"] +"\""+
                            ",\"backgroundColor\":\"#" + hab["color"] +
                            "\",\"densidad\":" + hab["Densidad"] +
                            ",\"nombreHabilidad\":\"" + hab["NombreHabilidad"] +
                            "\",\"act\":\"U" +
                            "\",\"nombreEtapa\":\"" + hab["NombreEtapa"] +
                            "\",\"comentario\":\"" + hab["comentarioPeriodo"].ToString().Trim() +
                            "\",\"elemento\":\"" + hab["Elemento"] +
                            "\",\"razon\":\"" + hab["razon"] +
                            "\",\"numeroElementos\":\"" + hab["numeroElementos"] +
                            "\",\"cantidadElemento\":\"" + hab["cantidadDeElementos"] +
                            "\",\"start\":\"" + DateTime.Parse(hab["inicio"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"end\":\"" + DateTime.Parse(hab["fin"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"surcosT\":" + hab["surcos"] +
                            ",\"surcos\":" + hab["surcosp"] +
                            ",\"esColmena\":\"" + hab["esColmena"] +
                            "\",\"aceptaColmena\":\"" + hab["aceptacolmena"] +
                            "\",\"surcoInicio\":" + hab["surcoInicio"] +
                            ",\"surcoFin\":" + hab["surcofin"] +
                            ",\"numeroactividad\":\"" + hab["numeroactividad"] +
                            "\",\"razonesDirectriz\":\"" + obtenerRazonesDeEliminacion().Replace('\"', '\'') + "\"" +
                            ",\"Asociados\":[" + (hab["Asociados"].ToString().Length == 0 ? "" : hab["Asociados"].ToString().Substring(0, hab["Asociados"].ToString().Length - 1)) + "]},";


                }
                if (dt.Rows.Count > 0)
                    result = json.Substring(0, json.Length - 1) + "]";
                    //result = result.Replace('   ','');
                else
                    result = string.Empty;
                return result;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return "<script>popUpAlert('No se pudo cargar las actividades.','error');</script>";
        }
    }

    [WebMethod]
    public static string cargaFumigaciones(int semana, int idInvernadero, int anio)
    {
        string result = "[";
        int cont = 0;
        try
        {
            var dt = new DataAccess().executeStoreProcedureDataTable("spr_getFumigaciones", new Dictionary<string, object>(){
                {"@semana",semana},
                {"@idInvernadero",idInvernadero},
                {"@anio",anio}
            });

            DateTime inic;

            foreach (DataRow hab in dt.Rows)
            {
                inic = DateTime.Parse(DateTime.Parse(hab["dFechaSugerida"].ToString()).ToString("yyyy-MM-dd ") + horaInicio + ":" + minutoInicio);

                result += "{\"anio\":" + anio +
                            ",\"semanaInicio\":\"" + DateTime.Parse(hab["semanaInicio"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"semanaFin\":\"" + DateTime.Parse(hab["semanaFin"].ToString()).AddMinutes(1439).ToString("yyyy-MM-dd HH:mm") +
                            "\",\"semana\":" + semana +
                            ",\"invernadero\":\"" + hab["GreenHouse"] + "\"" +
                            ",\"idHabilidad\":" + hab["idHabilidad"] +
                            ",\"idActividad\":" + hab["idFumigacion"] +
                            ",\"idEtapa\":" + hab["idEtapa"] +
                            ",\"porTiempo\":" + hab["SoloEjecutable"].ToString().ToLower() +
                            ",\"target\":" + hab["target"] +
                            ",\"plantasPorSurco\":" + hab["porSurco"] +
                            ",\"idCiclo\":" + hab["idCiclo"] +
                            ",\"editable\":\"false\"" +
                            ",\"idInvernadero\":" + idInvernadero +
                            ",\"idTr\":\"fumigacion_" + idInvernadero + "" + cont +
                            "\",\"directriz\":\"true\"" +
                            ",\"title\":\"" + hab["GreenHouse"] + ":" + hab["nombreHabilidad"] + " - " + hab["NombreEtapa"] + " " + hab["Product"] + "(" + hab["CodigoVariedad"] + ")\"" +
                            ",\"backgroundColor\":\"#" + hab["color"] +
                            "\",\"densidad\":" + hab["Densidad"] +
                            ",\"nombreHabilidad\":\"" + hab["NombreHabilidad"] +
                            "\",\"nombreEtapa\":\"" + hab["NombreEtapa"] +
                            "\",\"act\":\"F" +
                            "\",\"elemento\":\"" +
                            "\",\"numeroElementos\":\"" +
                            "\",\"start\":\"" + inic.ToString("yyyy-MM-dd HH:mm") +
                            "\",\"end\":\"" + inic.AddMinutes(int.Parse(hab["target"].ToString())).ToString("yyyy-MM-dd HH:mm") + 
                            "\",\"surcosT\":" + hab["surcos"] +
                            ",\"surcos\":" + hab["surcos"] +
                            ",\"Asociados\":[]" +
                            ",\"razonesDirectriz\":\"" + razonesDeEliminacion.Replace('\"', '\'') + "\"},";
            }
            if (dt.Rows.Count > 0)
                result = result.Substring(0, result.Length - 1) + "]";
            else
                result = string.Empty;
        }
        catch (Exception x)
        {
            log.Error(x.Message);
        }

        return result;
    }

    [WebMethod]
    public static string cargaInfestacionNivelInvernadero()
    {   
        string result;
        try
        {

            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_InfestacionNivelInvernadero", new Dictionary<string, object>() { { "@idLider", HttpContext.Current.Session["idUsuarioTemp"].ToString() } });

            result = GetDataTableToJson(dt);
        }
        catch (Exception ex)
        {
            result = "error";
            log.Error(ex);

        }

        return result;
    }

    [WebMethod]
    public static string cargaAsociadosFamiliasEficiencia(int idCiclo, int idEtapa)
    {
        string result="";

        Dictionary<string,object> param = new Dictionary<string,object>();
        try
        {
            param.Add("@idCiclo", idCiclo);
            param.Add("@idEtapa", idEtapa);
            param.Add("@idLider", HttpContext.Current.Session["idUsuarioTemp"].ToString());

            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_AsociadosFamiliasEficiencia", param);

            result = GetDataTableToJson(dt);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            result = "<script>alert('No se Pudieron cargar las eficiencias, intente de nuevo')</script>";
        }
        return result;
    }

    [WebMethod]
    public static string CargarHabilidadesDelDepartamento()
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return "<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"" + sTargetURLForSessionTimeout + "\") </script>";
        var result = string.Empty;
        try
        {

            var parameters = new Dictionary<String, Object>();
            parameters.Add("@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]);
            parameters.Add("@idDepartamento", HttpContext.Current.Session["idDepartamento"].ToString());
            parameters.Add("@idioma", HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0);

            var dt = new DataAccess().executeStoreProcedureDataTable("spr_HabilidadesPorPlantaDepartamento", parameters);

            foreach (DataRow hab in dt.Rows)//Agregar propiedades de bd a la actividad en html.
            {
                result += "<div class=\"divHabilidadProgramable\">" +
                          "     <div class=\"btnHabilidad\" id=\"" + hab["idHabilidad"] + "\" style=\"background:#" + hab["Color"] + ";  border-color:#" + borderColor(hab["Color"].ToString()) + ";\"  >" +
                          "        <span class=\"habilidad_icono\">" + hab["NombreCorto"] + "</span>" +
                          "        <span class=\"habilidad_descripcion\">" + hab["NombreHabilidad"] + "</span>" +
                          "        <span class=\"habilidad_etapa \">" + hab["NombreEtapa"] + "</span>" +
                          "        <span class=\"invisible idEtapa\">" + hab["idEtapa"] + "</span>" +
                          "        <span class=\"invisible idHabilidad\">" + hab["idHabilidad"] + "</span>" +
                          "        <span class=\"invisible target\">" + hab["Target"] + "</span>" +
                          "        <span class=\"invisible portiempo\">" + hab["SoloEjecutable"] + "</span>" +
                          "        <span class=\"invisible elemento\">" + hab["Elemento"] + "</span>" +
                    //"        <span class=\"invisible cantidadElementos\">" + hab["cantidadElementos"] + "</span>" +
                          "     </div>" +
                          "</div>";
            }
            return result;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return "<script>popUpAlert('Error al cargar las habilidades del departamento','error');</script>";
        }
    }

   
    private static string obtieneListaLideres()
    {
        string result = "<select id='ddlLider' class='ddlLiderAdmin'><option value='1'>--</option>";
        Dictionary<string,object> param = new Dictionary<string,object>();
        DataTable dt = new DataTable();

        try
        {
            param.Add("@idPlanta", HttpContext.Current.Session["idPlanta"]);
            dt = new DataAccess().executeStoreProcedureDataTable("spr_LiderObtenerPorPlanta", param);
            foreach (DataRow row in dt.Rows)
            {
                result += "<option value='" + row["idLider"] + "' idEmpleado='" + row["idEmpleado"] + "' "+(HttpContext.Current.Session["idUsuarioTemp"].ToString() == row["idLider"].ToString() ? "selected" : "")+"> " + row["vNombre"] + "</option>";
            }
            result += "</select>";
        }
        catch (Exception ex)
        {
            result = "Error al obtener lista de lideres.";
            log.Error(ex);   
        }

        return result;
    }

    private static string obtieneSemanaAnio(int semana, int anio)
    {

        string result = "<table><tr><td>SEMANA: </td><td> " + semana.ToString() + "&nbsp; " + " </td><td> AÑO: </td><td> " + anio.ToString() + "</td></tr><table>";
   

        return result;
    }

    [WebMethod]
    private static string obtieneInvernaderosProgramados(int semana,int anio)
    {

        string result = "<select id='ddlInvernaderosProgramados' class='ddlInvernaderosPro'><option value='0'>SELECCIONA INVERNADERO</option>";
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = new DataTable();

        try
        {
            param.Add("@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]);
            param.Add("@semana", semana);
            param.Add("@anio", anio);

            dt = new DataAccess().executeStoreProcedureDataTable("procObtieneInvernaderosProgramados", param);
            foreach (DataRow row in dt.Rows)
            {
                result += "<option value='" + row["idInvernadero"] + "'> " + row["Greenhouse"] + "</option>";
            }
            result += "</select>";
        }
        catch (Exception ex)
        {
            result = "Error al obtener lista de invernaderos programados.";
            log.Error(ex);
        }

        return result;
    }

    [WebMethod]
     private static string obtieneInvernaderosSINProgramacion(int semana,int anio)
    {
        string result = "<table>";
        Dictionary<string, object> param = new Dictionary<string, object>();
        DataTable dt = new DataTable();
         int contador=0;

        try
        {
            param.Add("@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]);
            param.Add("@semana", semana);
            param.Add("@anio", anio);

            dt = new DataAccess().executeStoreProcedureDataTable("procObtieneInvernaderosNOProgramados", param);

          
            foreach (DataRow row in dt.Rows)
            {
                if (contador == 0)
                {
                    result += "<tr><td>";
                }
                result += "<input type='checkbox' id='id_" + row["idInvernadero"] + "' value='" + row["idInvernadero"] + "'  checked/>  <label for='id_" + row["Greenhouse"] + "' >" + row["Greenhouse"] + "</label> ";                                    
                contador += 1;
                if (contador == 5)
                {
                    result += "</td></tr>";
                    contador = 0;
                }
            }
            if (contador == 0 || contador < 5)
            {
                result += "</td></tr></table>";
            }
            else
            {
                result += "</table>";
            }
        }
        catch (Exception ex)
        {
            result = "Error al obtener lista de invernaderos a programar.";
            log.Error(ex);
        }

        return result;
    }


     private static string obtieneInvernaderosSINProgramacionPorInvernadero(int semana, int anio, int idInvernadero)
     {
         string result = "<table>";
         Dictionary<string, object> param = new Dictionary<string, object>();
         DataTable dt = new DataTable();
         int contador = 0;

         try
         {
             param.Add("@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]);
             param.Add("@semana", semana);
             param.Add("@anio", anio);
             param.Add("@idInvernadero", idInvernadero);

             dt = new DataAccess().executeStoreProcedureDataTable("procObtieneInvernaderosNOProgramadosPorInvernadero", param);


             foreach (DataRow row in dt.Rows)
             {
                 if (contador == 0)
                 {
                     result += "<tr><td>";
                 }
                 result += "<input type='checkbox' id='id_" + row["idInvernadero"] + "' value='" + row["idInvernadero"] + "'  checked/>  <label for='id_" + row["Greenhouse"] + "' >" + row["Greenhouse"] + "</label> ";
                 contador += 1;
                 if (contador == 5)
                 {
                     result += "</td></tr>";
                     contador = 0;
                 }
             }
             if (contador == 0 || contador < 5)
             {
                 result += "</td></tr></table>";
             }
             else
             {
                 result += "</table>";
             }
         }
         catch (Exception ex)
         {
             result = "Error al obtener lista de invernaderos a programar.";
             log.Error(ex);
         }

         return result;
     }



    [WebMethod]
    public static Object cargaHabilidadesPlanta(int edad, int idinvernadero)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);
        var result = "";

        try
        {
            var parameters = new Dictionary<String, Object>();
            parameters.Add("@idPlanta", HttpContext.Current.Session["idPlanta"] == null ? currentFarm : HttpContext.Current.Session["idPlanta"]);
            parameters.Add("@idDepartamento", HttpContext.Current.Session["idDepartamento"].ToString());
            parameters.Add("@idioma", HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0);
            parameters.Add("@idInvernadero", idinvernadero);
            parameters.Add("@edad", edad);

            var dt = new DataAccess().executeStoreProcedureDataTable("spr_HabilidadesProgramacionSemanal", parameters);

            foreach (DataRow hab in dt.Rows)//Agregar propiedades de bd a la actividad en html.
            {
                result = string.Format("{0}<div Directriz=\"{1}\" id=\"{2}\" style=\"background:#{3};  border-color:#{4};\"   data-event='{{\"title\":\"{5}\",\"color\":\"#{3}\",\"stick\":true}}'>{6}<span>{5}</span></div>",
                                        result, hab["Directriz"], hab["idHabilidad"], hab["Color"], borderColor(hab["Color"].ToString()), hab["NombreHabilidad"], hab["NombreCorto"]);
            }

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return "<script>popUpAlert('Error al cargar habilidades de planta','error');</script>";
        }
        return result;
    }


    //[WebMethod]
    //public static string almacenarConfiguracionDeInvernadero(actividadProgramada[] programadas, actividadCancelada[] canceladas, actividadEliminada[] eliminada)
    //{
    //    try
    //    {
    //        DataTable dtActividades = dtActividadProgramada();
    //        DataTable dtJornales = dtActividadJornales();
    //        DataTable dtPeriodos = dtActividadPeriodos();
    //        int indice = 0;
    //        foreach (actividadProgramada A in programadas)
    //        {
    //            DataRow dr = dtActividades.NewRow();
    //            dr["idActividad"] = indice++;
    //            dr["idInvernnadero"] = A.idInvernadero;
    //            dr["idEtapa"] = A.idEtapa;
    //            dr["idCiclo"] = A.idCiclo;
    //            dr["cantidadDeElementos"] = A.cantidadDeElementos;
    //            dr["surcos"] = A.surcos;
    //            dr["jornalesEstimados"] = A.jornalesEstimados;
    //            dr["minutosEstimados"] = A.tiempoEstimado;
    //            dr["esDirectriz"] = A.esDirectriz;
    //            dr["esInterplanting"] = A.esInterplanting;
    //            dr["borrado"] = A.borrado;
    //            foreach (int idJornal in A.jornales)
    //            {
    //                DataRow drj = dtJornales.NewRow();
    //                drj["idActividad"] = indice;
    //                drj["idEtapa"] = idJornal;
    //                dtJornales.Rows.Add(drj);
    //            }
    //            dtActividades.Rows.Add(dr);
    //            foreach (Periodo P in A.periodos)
    //            {
    //                DataRow drj = dtPeriodos.NewRow();
    //                drj["idActividad"] = indice;
    //                drj["inicio"] = P.inicio;
    //                drj["fin"] = P.fin;
    //            }
    //        }
    //        return string.Empty;
    //    }
    //    catch (Exception x)
    //    {
    //        log.Error(x.Message);
    //        return string.Empty;
    //    }
    //}

    [WebMethod]
    public static string copiarProgramacionCompleta(InvernaderosProgramaCopia[] invernaderos, int idInvernaderoOrigen, int semana, int anio)
    {
        

        DataTable dtInvernaderosACopiar = dtInvernaderosCopia();
        foreach (InvernaderosProgramaCopia P in invernaderos)
        {
            if (P.invernaderosCopia != null)
                foreach (InvernaderosCopia A in P.invernaderosCopia)
                {
                    DataRow dr = dtInvernaderosACopiar.NewRow();
    
                    dr["idInvernadero"] = A.idInvernadero;

                    dtInvernaderosACopiar.Rows.Add(dr);
                }
        }
        DataAccess da = new DataAccess();
        DataSet dt = new DataSet();
        dt = da.executeStoreProcedureDataSet("procCopiaProgramacionSemanalInvernadero", new Dictionary<string, object>() { 
                {"@idUsuario",HttpContext.Current.Session["idUsuarioTemp"]},
                {"@invernaderos",dtInvernaderosACopiar},
                {"@idInvernadero",idInvernaderoOrigen },
                {"@semana",semana },
                {"@anio",anio}
            });

        return "OK|OK";
    }

    [WebMethod]
    public static string almacenarProgramacionCompleta(Programacion[] programacionTotal, bool enviar)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        DataTable dtActividadesProgramadas = dtActividadProgramada();
        DataTable dtActividadesCanceladas = dtActividadCancelada();
        DataTable dtActividadesEliminadas = dtActividadEliminada();
        DataTable dtJornales = dtActividadJornales();
        DataTable dtPeriodos = dtActividadPeriodos();

        int Actividad = 0, periodo = 0;
        foreach (Programacion P in programacionTotal)
        {
            if (P.programadas != null)
                foreach (actividadProgramada A in P.programadas)
                {
                    DataRow dr = dtActividadesProgramadas.NewRow();
                    dr["idActividad"] = A.idActividad;
                    dr["idActividadNoP"] = A.idActividadNoP;
                    dr["idInvernadero"] = A.idInvernadero;
                    dr["idEtapa"] = A.idEtapa;
                    dr["idCiclo"] = A.idCiclo;
                    dr["cantidadDeElementos"] = A.cantidadDeElementos;
                    //dr["surcos"] = A.surcos;
                    dr["semana"] = A.semana;
                    //dr["jornalesEstimados"] = (A.jornalesEstimados.All(char.IsLetter) ? "0" : A.jornalesEstimados);
                    //dr["minutosEstimados"] = A.tiempoEstimado.Split(' ')[0];
                    dr["esDirectriz"] = A.esDirectriz;
                    dr["esInterplanting"] = A.esInterplanting;
                    dr["anio"] = A.anio;
                    dr["surcoInicio"] = A.surcoInicio;
                    dr["surcofin"]=A.surcoFin;
                    dr["esColmena"] = A.esColmena;
                    dr["act"] = A.act;
                    dr["indice"] = ++Actividad;                  

                    dtActividadesProgramadas.Rows.Add(dr);

                    DataAccess daccess = new DataAccess();
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@idInvernadero", A.idInvernadero);
                    parameters.Add("@idEtapa", A.idEtapa);
                    DataSet dsZonificacion = daccess.executeStoreProcedureDataSet("procObtieneZonificacionPorInvernadero", parameters);

                    foreach (Periodo Per in A.periodos)
                    {
                        DataRow drP = dtPeriodos.NewRow();
                        drP["idPeriodo"] = Per.idPeriodo;
                        drP["idActividad"] = A.idActividad == 0 ? Actividad : A.idActividad;
                        drP["inicio"] = Per.inicio;
                        drP["fin"] = Per.fin;
                        drP["surcos"] = Per.surcos;
                        drP["indice"] = ++periodo;
                        drP["comentario"] =Per.comentario;
                       
                        if (Per.Asociados.Length > 0)
                        {
                            foreach (Asociados idJornal in Per.Asociados)
                            {
                                DataRow drj = dtJornales.NewRow();
                                drj["idPeriodo"] = Per.idPeriodo == 0 ? periodo : Per.idPeriodo;
                                drj["idEmpleado"] = idJornal.idAsociado;
                                dtJornales.Rows.Add(drj);
                            }
                        }
                        else
                        {
                            if (dsZonificacion.Tables.Count > 0)
                            {
                                if (dsZonificacion.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i <= dsZonificacion.Tables[0].Rows.Count - 1; i++)
                                    {
                                        DataRow drj = dtJornales.NewRow();
                                        drj["idPeriodo"] = Per.idPeriodo == 0 ? periodo : Per.idPeriodo;
                                        drj["idEmpleado"] = dsZonificacion.Tables[0].Rows[i]["idAsociado"].ToString();
                                        dtJornales.Rows.Add(drj);
                                    }
                                }
                            }

                        }
                        
                        dtPeriodos.Rows.Add(drP);

                    }
                }
            if (P.canceladas != null)
                foreach (actividadCancelada A in P.canceladas)
                {
                    DataRow drC = dtActividadesCanceladas.NewRow();
                    drC["idActividad"] = A.idActividad;
                    drC["idActividadNoP"] = A.idActividadNoP;
                    drC["anioProgramacion"] = A.anioProgramacion;
                    drC["semanaProgramacion"] = A.semanaProgramacion;
                    drC["idInvernadero"] = A.idInvernadero;
                    drC["idEtapa"] = A.idEtapa;
                    drC["razon"] = A.razon;
                    drC["comentario"] = A.comentario;
                    drC["idCiclo"] = A.idCiclo;
                    drC["cantidadDeElementos"] = A.cantidadDeElementos;
                    drC["esInterplanting"] = A.esInterplanting;
                    drC["act"] = A.act;
                    dtActividadesCanceladas.Rows.Add(drC);

                }
            if (P.eliminadas != null)

                foreach (var id in P.eliminadas)
                {
                    DataRow drE = dtActividadesEliminadas.NewRow();
                    drE["idActividad"] = id;
                    dtActividadesEliminadas.Rows.Add(drE);
                }

        }

        try
        {
            DataAccess da = new DataAccess();
            DataSet dt = new DataSet();
            dt = da.executeStoreProcedureDataSet("spr_ProgramacionTotalAlmacena", new Dictionary<string, object>() { 
                {"@idUsuario",HttpContext.Current.Session["idUsuarioTemp"]},
                {"@ActividadesProgramadas",dtActividadesProgramadas},
                {"@ActividadesCanceladas",dtActividadesCanceladas },
                {"@ActividadesEliminadas",dtActividadesEliminadas },
                {"@Jornales",dtJornales},
                {"@Periodos",dtPeriodos},
                {"@enviar",enviar}
            });
            //DataTable dt = new DataTable();
            var dd = dt.Tables[0].Rows.Count;

            var Estado = "";
            var Envio = "";
            var msj = "";

            if (dt.Tables[0].Rows[0]["Estado"].ToString().Equals("1"))
            {
                //return dt.Rows[0]["Mensaje"].ToString() + "|OK";
                Estado = "OK";
            }
            else
            {
                Log.Error(dt.Tables[0].Rows[0]["MensajeEspecifico"].ToString());
                //return "La programación no se pudo almacenar.|error";
                Estado = "error";
            }

            if (dt.Tables.Count > 1 && enviar )
            {
                switch (dt.Tables[1].Rows[0]["Envio"].ToString())
                {
                    case "1":
                        Envio = "OK";
                        break;
                    case "2":
                        Envio = "error";
                        msj = dt.Tables[1].Rows[0]["Mensaje"].ToString();
                        break;
                    default:
                        Envio = "error";
                        break;
                }
            }

            if (Estado == "OK" && Envio == "OK")
            {
                return "Se ha enviado la programación semanal a autorización." + "|OK";
            }
            else if(Estado == "OK" && string.IsNullOrEmpty (Envio)){
                return "La Programagión Semanal se ha guardado correctamente." + "|OK";
            }
            else if (Estado == "OK" && Envio == "error")
            {
                return "Se almacenó correctamente pero no se pudo enviar el correo: " + msj + ".|OK";
            }
            else if (Estado == "error" && Envio == "error")
            {
                return "La programación no se pudo almacenar y el correo no pudo ser enviado.|error";
            }
            else if (Estado == "error" && string.IsNullOrEmpty(Envio))
            {
                return "La programación no se pudo almacenar.|error";
            }
            else
            {
                return "La programación no se pudo almacenar. |error";
            }

        }
        catch (Exception x)
        {
            log.Error(x.Message);
            if (x.Message.Contains("network-related") || x.Message.Contains("transport-level error"))
            {
                return "Error de Conexión. No se pudo conectar con el servidor de base de datos.|error";
            }
            return "La programación no se pudo almacenar.|error";
        }
    }

    [WebMethod]
    public static string[] buscarInvernaderosMalConfigurados(String semana, String anio)
    {

        if (HttpContext.Current.Session["idUsuarioTemp"] == null)
            return new string[] { "0", string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout),"warning" };

        var JSONResult = "";
        

        try
        {
            var dt = new DataTable();

            dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtenerDetalleInvernaderosNoConfigurados", new Dictionary<string, object>() {
                        { "@idUsuario", HttpContext.Current.Session["idUsuarioTemp"] }
                      , { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } 
                      , { "@semanaActual", semana}
                      , { "@anioActual", anio }
            });


            String xml = "";

            foreach (DataRow row in dt.Rows)
            {
                xml += row[0];
            }

          //  string xml = dt.Rows[0][0].ToString();
            JSONResult = Json.XmlToJson(xml);
   
            return new string[] { "1", "ok", JSONResult };
        }
        catch (Exception es)
        {
            log.Error(es.Message);
            return new string[] { "1", "El proceso no generó ningún resultado", "warning"};
        }
        

    }

    private static DataTable dtActividadEliminada()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        return dt;
    }
    private static DataTable dtActividadCancelada()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadNoP");
        dt.Columns.Add("anioProgramacion");
        dt.Columns.Add("semanaProgramacion");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idEtapa");
        dt.Columns.Add("razon");
        dt.Columns.Add("comentario");
        dt.Columns.Add("idCiclo");
        dt.Columns.Add("cantidadDeElementos");
        dt.Columns.Add("esInterplanting");
        dt.Columns.Add("act");
        return dt;
    }
    private static DataTable dtInvernaderosCopia()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idInvernadero");
        return dt;
    }
    private static DataTable dtActividadProgramada()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");
        dt.Columns.Add("idActividadNoP");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idEtapa");
        dt.Columns.Add("idCiclo");
        dt.Columns.Add("cantidadDeElementos");
        //dt.Columns.Add("surcos");
        dt.Columns.Add("semana");
        //dt.Columns.Add("jornalesEstimados");
        //dt.Columns.Add("minutosEstimados");
        dt.Columns.Add("esDirectriz");
        dt.Columns.Add("esInterplanting");
        //dt.Columns.Add("borrado");
        dt.Columns.Add("anio");
        dt.Columns.Add("surcoInicio");
        dt.Columns.Add("surcofin");
        dt.Columns.Add("esColmena");

        dt.Columns.Add("act");
        dt.Columns.Add("indice");

        return dt;
    }
    private static DataTable dtActividadJornales()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idEmpleado");

        return dt;
    }
    private static DataTable dtActividadPeriodos()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idPeriodo");
        dt.Columns.Add("idActividad");
        dt.Columns.Add("inicio");
        dt.Columns.Add("fin");
        dt.Columns.Add("surcos");
        dt.Columns.Add("indice");
        dt.Columns.Add("comentario");
        return dt;

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
    private static string obtenerRazonesDeEliminacion()
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return "<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"" + sTargetURLForSessionTimeout + "\") </script>";
        try
        {
            DataAccess da = new DataAccess();
            DataTable dt = da.executeStoreProcedureDataTable("spr_MotivoActividadNoProgramada", new Dictionary<string, object>() {
            {"@idioma", HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0}
        });
            StringBuilder sb = new StringBuilder();
            sb.Append("<select class=\"ddlRazon\"><option>--</option>");
            foreach (DataRow item in dt.Rows)
            {
                sb.Append(string.Format("<option value=\"{0}\">{1}</option>", item["idMotivoActividadNoProgramada"], item["MotivoActividadNoProgramada"]));
            }
            sb.Append("</select>");
            return sb.ToString();
        }
        catch (Exception x)
        {
            log.Error(x.Message);
            return string.Empty;
        }
    }

}
