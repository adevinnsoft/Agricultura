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
using System.Reflection;

public partial class Lider_ProgramacionSemanalAutorizacion : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Lider_ProgramacionSemanalAutorizacion));
    private static int idUsuario = 0;
    private static string sTargetURLForSessionTimeout;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            //lo que se restsaura o se hace tras cada llamada
        }
        else
        {
            //lo que se hace para inicializar la pagina
            idUsuario = int.Parse(Session["userIDInj"].ToString());
            sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();
        }
    }

    [WebMethod]
    public static string[] ObtenerLiderEInvernadero()
    {
        try { 
            DataAccess da = new DataAccess();
            var dt = da.executeStoreProcedureDataTable("spr_ObtenerLiderEInvernadero", new Dictionary<string, object>(){
                {"@idPlanta", HttpContext.Current.Session["idPlanta"]}
            });

            if (dt.Rows.Count <= 0)
            {
                return new string[] { "0", "error", "No se encontraron lideres con invernaderos." };
            }
            else
            {
                DataView dw = new DataView(dt);
                var dtLider = dw.ToTable(true, "idLider", "vNombre", "idPlanta", "idGerente", "NombreGerente");
                StringBuilder sb = new StringBuilder();
                StringBuilder lideres = new StringBuilder();
                lideres.AppendLine("<tr><th>Lider</th><th>Gerente</th></tr>");
                sb.AppendLine("<option value=\"\" idLider=\"0\">--Seleccione--</option>");

                foreach(DataRow dr in dtLider.Rows)
                {
                    string idLider = dr["idLider"].ToString(), Lider = dr["vNombre"].ToString(), idPlanta = dr["idPlanta"].ToString(), idGerente = dr["idGerente"].ToString(),NombreGerente = dr["NombreGerente"].ToString();
                    if((HttpContext.Current.Session["idUsuario"].ToString()==idGerente))
                    sb.AppendLine("<option idLider=\"" + idLider + "\" idPlanta=\"" + idPlanta + "\" >" + Lider + "</option>");

                    lideres.AppendLine("<tr> <td>" + Lider + "</td>  <td>"+NombreGerente+"</td></tr>");
                }



                DataView dw2 = new DataView(dt);
                var dtInvernadero = dw2.ToTable(true, "idInvernadero", "ClaveInvernadero", "idLider");
                StringBuilder sb2 = new StringBuilder();
                int c = 0;

                foreach (DataRow dr in dtInvernadero.Rows)
                {
                    c++;
                    string idInvernadero = dr["idInvernadero"].ToString(),
                           clvInvernadero = dr["ClaveInvernadero"].ToString(),
                           idLider = dr["idLider"].ToString();
                    sb2.AppendLine("<td class=\"tdInvernadero\" style=\"display:none;\"><input type=\"checkbox\" id=\"filtroInvernadero" + c + "\" class=\"filtroInvernadero\" idLider=\"" + idLider + "\" idInvernadero=\"" + idInvernadero + "\" onchange=\"filterbySite();\" /><label for=\"filtroInvernadero" + c + "\">" + clvInvernadero + "</label></td>");
                }

                return new string[] { "1", "ok", sb.ToString(), sb2.ToString(), lideres.ToString() };
            }
        }
        catch (Exception x) {
            Log.Error(x);
            return new string[] { "0", "error", "Error al obtener los datos" };
        }
    }

    [WebMethod]
    public static string QuitaAgregaLider(int idGerente, int idLider, int act)
    {
        string result = "";
        Dictionary<string, object> param = new Dictionary<string,object>();
        try
        {
            if (HttpContext.Current.Session["idUsuario"] == null)
            {
                result = "Tu sesión se cerró, esto puede suceder debido a desconexiones al servidor por problemas de red o el tiempo se sesión se agotó, vuelve a cargar la pagina o inicia sesión de nuevo. Disculpe las molestias.";
            }
            else
            {
                param.Add("@idGerente", HttpContext.Current.Session["idusuario"].ToString());
                param.Add("@idLider", idLider);
                param.Add("@act", act);
                param.Add("@idUsuario", HttpContext.Current.Session["idusuario"].ToString());
                result = new DataAccess().executeStoreProcedureString("spr_AgregaQuitaLiderAGerente", param);
            }
        }
        catch (Exception ex)
        {
            result = act == 1 ? "Ocurrió un error al intentar Agregar al lider, intenta de nuevo." : "Ocurrió un error al intentar Remover al lider, intenta de nuevo.";
            Log.Error(ex);
        }

        return result;
    }

    [WebMethod]
    public static string ObtenerProgramacionSemanal(int semana, int anio, int idPlanta, int idLider)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        var result = string.Empty;
        var json = "[";

        try
        {
            var parameters = new Dictionary<String, Object>();
            parameters.Add("@idioma", HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0);
            parameters.Add("@semana", semana);
            parameters.Add("@anio", anio);
            parameters.Add("@idUsuario", idLider);
            parameters.Add("@idPlanta", idPlanta);

            var dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtenerProgramacionSemanalParaAutorizacion", parameters);

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
                        ",\"semana\":" + semana +
                        ",\"invernadero\":\"" + hab["GreenHouse"] + "\"" +
                        ",\"idHabilidad\":" + hab["idHabilidad"] +
                        ",\"idActividad\":" + hab["idActividadPrograma"] +
                        ",\"idPeriodo\":" + hab["idActividadPeriodo"] +
                        ",\"idEtapa\":" + hab["idEtapa"] +
                        ",\"porTiempo\":" + hab["SoloEjecutable"].ToString().ToLower() +
                        ",\"target\":" + hab["target"] +
                        ",\"plantasPorSurco\":" + hab["porSurco"] +
                        ",\"idCiclo\":" + hab["idCiclo"] +
                        ",\"editable\":\"false\"" +
                        ",\"idInvernadero\":" + hab["idInvernadero"] +
                        ",\"idTr\":\"directriz_" + hab["idActividadPrograma"] +
                        "\",\"directriz\":\"" + hab["Directriz"].ToString().ToLower() + "\"" +
                        ",\"title\":\"" + hab["GreenHouse"] + ":" + hab["nombreHabilidad"] + " - SURCOS: " +  hab["surcosp"] +" - "+ hab["NombreEtapa"] + hab["actividadPeriodoAprobadoPor"] + (hab["actividadPeriodoAprobadoPor"].ToString() != "" ? " (Aprobado)" : (hab["actividadPeriodoRechazadoPor"].ToString() != "" ? " (Rechazado)" : "")) + "\"" +
                        ",\"backgroundColor\":\"#" + hab["color"] +
                        "\",\"densidad\":" + hab["Densidad"] +
                        ",\"nombreHabilidad\":\"" + hab["NombreHabilidad"] +
                        
                        "\",\"nombreEtapa\":\"" + hab["NombreEtapa"] +
                        "\",\"comentario\":\"" + hab["comentario"] +
                        "\",\"elemento\":\"" + hab["Elemento"] +
                        "\",\"editable\":false"+
                        ",\"numeroElementos\":\"" + hab["cantidadDeElementos"] +
                        "\",\"cantidadElemento\":\"" + hab["cantidadDeElementos"] +
                        "\",\"start\":\"" + DateTime.Parse(hab["inicio"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                        "\",\"end\":\"" + DateTime.Parse(hab["fin"].ToString()).ToString("yyyy-MM-dd HH:mm") +
                        "\",\"surcosT\":" + hab["surcos"] +
                        ",\"surcos\":\"" + hab["surcosp"] +

                        "\",\"actividadPeriodoAprobadoPor\":\"" + hab["actividadPeriodoAprobadoPor"] +
                        "\",\"actividadPeriodoRechazadoPor\":\"" + hab["actividadPeriodoRechazadoPor"] +
                        
                        "\",\"Asociados\":[" + (hab["Asociados"].ToString().Length == 0 ? "" : hab["Asociados"].ToString().Substring(0, hab["Asociados"].ToString().Length - 1)) + "]},";


            }
            if (dt.Rows.Count > 0)
                result = json.Substring(0, json.Length - 1) + "]";
            else
                result = string.Empty;
            return result;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return "<script>popUpAlert('No se pudo cargar las actividades.','error');</script>";
        }
    }

    [WebMethod]
    public static string GuardarAutorizacion(ProgramacionAutoriza Captura)
    {
        if (HttpContext.Current.Session["idUsuario"] == null)
            return string.Format("<script>popUpAlert('Su sesión finalizó.','error'); window.location.assign(\"{0}\") </script>", sTargetURLForSessionTimeout);

        DataTable dtA = getDataTableByTypeClass(typeof(ProgramacionAutoriza));
        DataRow dr = dtA.NewRow();

        dr["aprobado"] = Captura.aprobado;
        dr["comentarios"] = Captura.comentarios;
        dr["idLider"] = Captura.idLider;
        dr["padre"] = null;
        dr["indice"] = 1;
        dtA.Rows.Add(dr);

        DataTable dtP = getDataTableByTypeClass(typeof(AutorizacionPlan));
        int c = 0;
        foreach (AutorizacionPlan a in Captura.AutorizarPlan)
        {
            dr = dtP.NewRow();
            dr["anio"] = a.anio;
            dr["idActividad"] = a.idActividad;
            dr["idInvernadero"] = a.idInvernadero;
            dr["idPeriodo"] = a.idPeriodo;
            dr["semana"] = a.semana;
            dr["padre"] = 1;
            dr["indice"] = ++c;
            dtP.Rows.Add(dr);
        }

        try
        {
            DataAccess da = new DataAccess();
            var dt = da.executeStoreProcedureDataSet("spr_ProgramacionSemanalAutoriza", new Dictionary<string, object>() { 
                {"@idUsuario",HttpContext.Current.Session["idUsuario"]},
                {"@actividadPrograma",dtA},
                {"@actividadPeriodo", dtP},
            });

            var dd = dt.Tables[0].Rows.Count;
            var estado = "";
            var envio = "";
            var msj = "";

            //Validamos si la autorizacion fue exitosa
            if (dt.Tables[0].Rows[0]["Estado"].ToString().Equals("1"))
            {
                estado = "OK";
                //return dt.Tables[0].Rows[0]["Mensaje"].ToString() + "|ok";
            }
            else
            {
                estado = "error";
                log.Error(dt.Tables[0].Rows[0]["MensajeEspecifico"].ToString());
                //return "La programación no se pudo autorizar.|error";
            }

            //Validamos si el envío de correo fue exitoso
            switch (dt.Tables[1].Rows[0]["Envio"].ToString())
            {
                case "1":
                    envio = "OK";
                    break;
                case "2":
                    envio = "error";
                    msj = dt.Tables[1].Rows[0]["Mensaje"].ToString();
                    break;
                default:
                    envio = "error";
                    break;
            }

            //Definimos los mensajes a mostrar al usuario de acuerdo a las 2 validaciones anteriores
            if (estado == "OK" && envio == "OK")
            {
                return dt.Tables[0].Rows[0]["Mensaje"].ToString() + "|OK";
            }
            else if (estado == "OK" && string.IsNullOrEmpty(envio))
            {
                return dt.Tables[0].Rows[0]["Mensaje"].ToString() + "|OK";
            }
            else if (estado == "OK" && envio == "error")
            {
                return "Se almaceno correctamente pero no se envió el correo.|OK";
            }
            else if (estado == "error" && envio == "error")
            {
                return "La programación no se pudo almacenar y el correo no pudo ser enviado.|error";
            }
            else
            {
                return "La programación no se pudo almacenar y se envio el correo.|error";
            }

        }
        catch (Exception x)
        {
            log.Error(x.Message);
            return "La programación no se pudo autorizar.|error";
        }
    }

    public static DataTable getDataTableByTypeClass(Type myType)
    {
        DataTable dt = new DataTable();
        foreach (FieldInfo item in myType.GetFields())
        {
            if (!item.FieldType.Name.ToString().Contains("[]"))
                dt.Columns.Add(item.Name.ToString());
        }
        dt.Columns.Add("padre");
        dt.Columns.Add("indice");
        return dt;
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

    //[WebMethod]
    //public static string InicioFinSemana(int semana, int anio)
    //{
    //    string result = "";

    //    try
    //    {
    //        DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_SemanaNS", new Dictionary<string, object>() { { "@semana", semana }, { "@anio", anio } });

    //        result = GetDataTableToJson(dt);

    //        result = result.Replace("\"\\/Date(", "").Replace(")\\/\"", "");

    //    }
    //    catch (Exception ex)
    //    {

    //        Log.Error(ex.Message);
    //    }

    //    return result;
    //}
}