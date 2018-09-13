using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using log4net;
using System.Globalization;

public partial class Abejorros_frmAbejorrosEstadoDeColmena : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Abejorros_frmAbejorrosEstadoDeColmena));
    private static string sTargetURLForSessionTimeout;
    private static string[] tagMantto = {"Pasado","Acual","Proximo" };
    private static Abejorros_frmAbejorrosEstadoDeColmena basePage;

    protected void Page_Load(object sender, EventArgs e)
    {
        sTargetURLForSessionTimeout = System.Configuration.ConfigurationManager.AppSettings["SessionOutPage"].ToString();
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
            dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtieneInvernaderosDdl", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, { "@idPlanta", (HttpContext.Current.Session["idPlanta"] == null ? "0" : HttpContext.Current.Session["idPlanta"].ToString()) } });
            //dt = new DataAccess().executeStoreProcedureDataTable("spr_InvernaderosParaProgramacionSemanal", new Dictionary<string, object>() { 
                //{ "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() }, 
                //{ "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } 
            //});

            foreach (DataRow inv in dt.Rows)
            {
                result = result + "<div class=\"divInvernadero\" id='" + inv["idInvernadero"] + "' >" + inv["ClaveInvernadero"] + "<span class='avance'></span></div>";
                //result = result + "<div class=\"divInvernadero\" id='" + inv["idInvernadero"] + "' product='" + inv["Product"] + "(" + inv["Variety"] + ")' fechaPlantacion='" + inv["PlantDate"] + "' semana=\"" + (inv["Week"].ToString().Substring(5, 2)) + "\" densidad='" + inv["Density"] + "' terminado='" + inv["complete"] + "' surcos='" + inv["surcos"] + "' >" + inv["ClaveInvernadero"] + "<span class='avance'></span></div>";
            }

        }
        catch (Exception es)
        {
            log.Error(es);
        }
        return result;
    }

    [WebMethod]
    public static string cargaColmenasPorInvernadero(int idInvernadero)
    {
        var dt = new DataTable();
        var dtNivel = new DataTable();
        string result = "";
        try
        {
            int lang = HttpContext.Current.Session["Locale"].ToString() == "es-MX" ? 1 : 0;
            dtNivel = new DataAccess().executeStoreProcedureDataTable("spr_ObtieneNivelPolinizacion", new Dictionary<string, object>() {{"@lengua", lang }});

            dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtieneEstadoColmenaPorInvernadero", new Dictionary<string, object>() { 
                { "@idInvernadero", idInvernadero.ToString() } 
            });
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int mantto = 0;
                    Int32.TryParse(row["Mantenimiento"].ToString(), out mantto);
                    result += "<tr idInvernadero=\"" + idInvernadero.ToString() + "\" idColmenas=\"" + row["idColmenas"] + "\">                                       " +
                  "    <td class=\"Semana\" value=\""+ row["semanaNS"] + " \">" + row["semanaNS"] + "</td>                                           " +
      //          "    <td class=\"Ingreso\">" + DateTime.Parse(row["Ingreso"].ToString()).ToString("yyyy-MM-dd HH:mm") + "</td>                                           " +
                  "    <td class=\"Mantenimiento\" idMantto=\"" + mantto + "\">" + (mantto > 0? tagMantto[mantto - 1]: "N/A") + "</td>         " +
                  "    <td class=\"ColmenasPlaneadas\">" + (row["ColmenasPlaneadas"]) + "</td>                     " +
                  generaComponentePolinizacion(Int32.Parse(row["Polinizacion"].ToString()), mantto) +
                  generaComponenteNivelPolinizacion(dtNivel, Int32.Parse(row["NivelPolinizacion"].ToString()), mantto) +
                  "<td><textarea class=\"Observaciones longStringValidate\"  rows=\"5\" cols=\"50\" " + (mantto != 2 ? "disabled" : "") + ">" + row["Observaciones"].ToString() + "</textarea>" +
                  "</tr>";
                }
                return result;
            }
        }
        catch (Exception e)
        {
            log.Error(e.Message);
        }
        return null;
    }

    private static string generaComponentePolinizacion(int valor, int mantto)
    {
        return "<td><input type=\"number\" class=\"Polinizacion intValidate\" min=\"0\" max=\"100\" step=\"1\" value=\"" + (valor >= 0 ? valor : 0) + "\" " + (mantto != 2 ? "disabled" : " ") + "></td>";

    }

    private static string generaComponenteNivelPolinizacion(DataTable dt, int valor, int mantto)
    {
        string result = "<td><select class=\"NivelPolinizacion\" " + (mantto != 2 ? "disabled" : " ") + ">" +
                "    <option value=\"0\"" + (valor == 0 ? " selected" : "") + "> - Seleccione - </option>       ";
        foreach(DataRow row in dt.Rows){
            result += "    <option value=\"" + row["idNivelPolinizacion"].ToString() + "\"" + (valor == Int32.Parse(row["idNivelPolinizacion"].ToString()) ? " selected" : "") + ">" +
                row["idNivelPolinizacion"].ToString() + " - " + row["Descripcion"].ToString() + 
                "</option>       ";
        }
        result += "</select></td>";
        return result;
    }




    [WebMethod]
    public static string almacenarMantenimientos(MantenimientoColmena[] manttos)
    {
        try
        {
            DataTable dtMantto = buildDtMantto();
            foreach (MantenimientoColmena A in manttos)
            {
                // validar polinizacion lvl y observaciones
                if (A.Mantenimiento == 2)
                {
                    if (A.Observaciones.Trim().Length > 0)
                    {
                        DataRow dr = dtMantto.NewRow();
                        dr["idInvernadero"] = A.idInvernadero;
                        dr["idColmenas"] = A.idColmenas;
                        dr["Semana"] = A.semanaNS;
                        dr["Mantenimiento"] = A.Mantenimiento;
                        dr["Polinizacion"] = A.Polinizacion;
                        dr["NivelPolinizacion"] = A.NivelPolinizacion;
                        dr["Observaciones"] = A.Observaciones;

                        dtMantto.Rows.Add(dr);
                    }
                    else if (A.Polinizacion > 0 || A.NivelPolinizacion > 0)
                    {
                        return HttpContext.GetLocalResourceObject("~/Abejorros/frmAbejorrosEstadoDeColmena.aspx", "mensajeErrorObservaciones").ToString();
                    }
                }
            }

            if (dtMantto.Rows.Count == 0)
            {
                return HttpContext.GetLocalResourceObject("~/Abejorros/frmAbejorrosEstadoDeColmena.aspx", "mensajeSinCambios").ToString();
            }
            else
            {
                try
                {
                    DataTable dt = null;
                    // guardar datos
                    foreach (DataRow row in dtMantto.Rows)
                    {

                        dt = new DataAccess().executeStoreProcedureDataTable("spr_guardaMantenimientoColmenas", new Dictionary<string, object>() { 
                            {"@idColmenas",row["idColmenas"]},
                            {"@idInvernadero",row["idInvernadero"]},
                            {"@UsuarioModifico", HttpContext.Current.Session["idUsuario"].ToString() }, 
                            {"@SemanaMantenimiento",row["Semana"]},
                            {"@PorcentajePolinizacion",row["Polinizacion"]},
                            {"@NivelPolinizacion",row["NivelPolinizacion"]},
                            {"@Observaciones",row["Observaciones"]}
                        });
                        if (dt != null && dt.Rows != null && dt.Rows.Count > 0 && Int32.Parse(dt.Rows[0][0].ToString()) > 0)
                        {
                            return HttpContext.GetLocalResourceObject("~/Abejorros/frmAbejorrosEstadoDeColmena.aspx", "mensajeCambiosOK").ToString();
                        }
                        else
                        {
                            return HttpContext.GetLocalResourceObject("~/Abejorros/frmAbejorrosEstadoDeColmena.aspx", "mensajeErrorAlGuardar").ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return HttpContext.GetLocalResourceObject("~/Abejorros/frmAbejorrosEstadoDeColmena.aspx", "mensajeErrorAlGuardar").ToString();
                }
            }
            return string.Empty;
        }
        catch (Exception x)
        {
            log.Error(x);
            return HttpContext.GetLocalResourceObject("~/Abejorros/frmAbejorrosEstadoDeColmena.aspx", "mensajeErrorAlGuardar").ToString();
        }
    }


    private static DataTable buildDtMantto()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idColmenas");
        dt.Columns.Add("Semana");
        dt.Columns.Add("Mantenimiento");
        dt.Columns.Add("Polinizacion");
        dt.Columns.Add("NivelPolinizacion");
        dt.Columns.Add("Observaciones");
        return dt;
    }

}
