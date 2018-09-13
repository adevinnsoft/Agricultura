using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.Data;
using System.Globalization;
//<<<<<<< .mine
using log4net;
//=======
using System.Web.Script.Serialization;
//>>>>>>> .r664

public partial class Jornales_Vista_Aerea : BasePage
{

    private static readonly ILog log = LogManager.GetLogger(typeof(Jornales_Vista_Aerea));
    protected void Page_Load(object sender, EventArgs e)
    {

    } 
    [WebMethod]
    public static string[] obtenerInvernaderosPorPlanta(string nombreDePlanta)
    {
        JavaScriptSerializer jsDeserializer = new JavaScriptSerializer();
        StringBuilder sb = new StringBuilder();
        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
        Actividad[] Actividades;
        Infestacion[] Infestaciones;
        try
        {   
            prm.Add("@idUsuario",idUsuario);
            prm.Add("@idPlanta", HttpContext.Current.Session["idPlanta"]);
            DataTable dtResult = da.executeStoreProcedureDataTable("spr_InvernaderoInformacionGeneral", prm);

            foreach (DataRow item in dtResult.Rows){
              
                string idInvernadero = item["idInvernadero"].ToString().Trim(),
                       Invernadero = item["Invernadero"].ToString().Trim(),
                       Producto = item["Producto"].ToString().Trim(),
                       NumeroDeSecciones = item["NumeroDeSecciones"].ToString().Trim(),
                       Densidad = item["Densidad"].ToString().Trim(),
                       PromedioDeDensidad = item["PromedioDeDensidad"].ToString().Trim(),
                       NumeroDeSurcos = item["NumeroDeSurcos"].ToString().Trim(),
                       //NombreInfestacion = item["NombreInfestacion"].ToString().Trim(),
                       //NivelDeInfestacion = item["NivelDeInfestacion"].ToString().Trim(),
                       Infestacioness = item["Infestaciones"].ToString(),
                       EstadoInfestacion = item["Estado"].ToString().Trim(),
                       Lideres = item["Lideres"].ToString(),
                       SemanasCicloDeVida = item["SemanasCicloDeVida"].ToString(),
                       VariableDeTecnologia = item["VariableDeTecnologia"].ToString(),
                       ActividadesProgramadas = item["ActividadesProgramadas"].ToString(),
                       Variedad = item["Variedad"].ToString();

                sb.AppendLine(EstadoInfestacion.Equals("Infestado") ? "<div idInvernadero=\"" + idInvernadero + "\" value=\"" + idInvernadero + "\" class=\"InvernaderoInfestado\" onclick=\"verDetalleInvernadero($(this))\">" + item["Invernadero"] + "</div>" : "<div idInvernadero=\"" + idInvernadero + "\" value=\"" + idInvernadero + "\" class=\"Invernadero\" onclick=\"verDetalleInvernadero($(this))\">" + item["Invernadero"] + "</div>");
                sb.AppendLine("<div idInvernadero=\"" + idInvernadero + "\" class=\"InformacionGeneraldeInvernadero\" name=\"" + Invernadero + "\" style=\"display:none;\">");
                sb.AppendLine("<img src=\"../comun/img/remove-icon.png\" alt=\"X\" style=\"float:right;margin:10px;cursor:pointer;\" onclick=\"ocultarDetalleInvernadero($(this).parent('div.InformacionGeneraldeInvernadero'))\">");
                sb.AppendLine("<h2 class=\"InformacionGeneralTitulo\">" + Invernadero + ":</h2>");
                sb.AppendLine(Lideres.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblLider\">Lider(es):</label><label>"+Lideres+"</label></span>");
                sb.AppendLine(Producto.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblProducto\">Producto:</label><label>" + Producto + "</label>" + (Variedad.Equals(string.Empty) ? string.Empty : "(Variedad:" + " " + Variedad + ")") + "</span>");
                sb.AppendLine(SemanasCicloDeVida.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblSemanasCicloDeVida\">Edad:</label><label>" + SemanasCicloDeVida + " " + "Semanas</label></span>");
                sb.AppendLine(VariableDeTecnologia.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblVariableDeTecnologia\">Tecnologia:</label><label>" + VariableDeTecnologia + "</label></span>");
                sb.AppendLine(NumeroDeSecciones.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblNumeroDeSecciones\">Numero de Secciones:</label><label>" + NumeroDeSecciones + "</label></span>");
                sb.AppendLine(NumeroDeSurcos.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblNumeroDeSurcos\">Numero de Surcos:</label><label>" + NumeroDeSurcos + "</label></span>");
                sb.AppendLine(Densidad.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblNumeroDensidad\">Densidad:</label><label>" + Densidad + " " + "Plantulas" + " " + (PromedioDeDensidad.Equals(string.Empty) ? string.Empty : "(Promedio:" + " " + PromedioDeDensidad + " " + "P/S)") + "</label></span>");


                //sb.AppendLine(PromedioDeDensidad.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblPromedioDensidad\">Promedio de Densidad:</label><label>" + PromedioDeDensidad + "</label></span>");

                //sb.AppendLine(NombreInfestacion.Equals(string.Empty) ? string.Empty : "<span><label id=\"lblNombreInfestacion\">Nombre de Infestacion:</label><label>" + NombreInfestacion + "</label></span>");
                //sb.AppendLine(NivelDeInfestacion.Equals(string.Empty) ?  string.Empty : "<span><label id=\"lblNivelDeInfestacion\">Nivel de Infestacion:</label><label>" + NivelDeInfestacion + "</label></span>");


                //div para mostrar si el invernadero esta infestado 
                sb.AppendLine(EstadoInfestacion.Equals(string.Empty) ? string.Empty : "<div class=\"divEstadoInfestacion\"><span><label id=\"lblEstadoInfestacion\">Estado:</label><label>" + EstadoInfestacion + "</label></span></div>");

                //tabla que muestra las diferentes infestaciones que tiene el invernadero con su respectivo nivel
                Infestaciones = jsDeserializer.Deserialize<Infestacion[]>(Infestacioness);

                if (Infestaciones != null)
                {
                        sb.AppendLine("<div class=\"divtblDatosInfestacion\">");
                        sb.AppendLine("<table id=\"tblDatosInfestacion\" class=\"gridView\">");
                        sb.AppendLine("<thead>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<th>Infestacion</th>");
                        sb.AppendLine("<th>Nivel</th>");
                        sb.AppendLine("</tr>");
                        sb.AppendLine("</thead>");
                        sb.AppendLine("<tbody>");
                        foreach (Infestacion infestacion in Infestaciones)
                        {
                            sb.AppendLine("<tr class=\"Infestacion\"><td class=\"nombreInfestacion\">" + infestacion.nombreInfestacion + "</td><td class=\"nivelInfestacion\">" + infestacion.nivelInfestacion + "</td></tr>");
                        }
                        sb.AppendLine("</tbody>");
                        sb.AppendLine("</table>");
                        sb.AppendLine("</div>");
                }
                
                Actividades = jsDeserializer.Deserialize<Actividad[]>(ActividadesProgramadas);

                if (Actividades != null)
                {
                    sb.AppendLine("<h2 class=\"ActividadesProgramadasTitulo\">Actividades Programadas</h2>");
                    foreach (Actividad propiedad in Actividades)
                    {
                        //HTML actividades programadas del invernadero
                        
                        sb.AppendLine("<div class=\"Actividad\"id=\"" + propiedad.idActividad + "\">");
                        sb.AppendLine("<div class=\"btnActividad\" id=\"" + propiedad.idActividad + "\" style=\"background:#" + propiedad.color + "; border-color:#" + borderColor(propiedad.color.ToString()) + ";\">");
                        sb.AppendLine(propiedad.codigo.Equals(string.Empty) ? string.Empty : "<span class=\"actividadCodigo\">" + propiedad.codigo + "</span>");
                        sb.AppendLine(propiedad.nombreActividad.Equals(string.Empty) ? string.Empty : "<span class=\"actividadNombre\">" + propiedad.nombreActividad + "</span>");
                        sb.AppendLine(propiedad.etapa.Equals(string.Empty) ? string.Empty : "<span class=\"actividadEtapa\">" + propiedad.etapa + "</span>");
                        sb.AppendLine("</div>");
                        sb.AppendLine("</div>");
                    }
                    sb.AppendLine("<input Invernadero=\"" + Invernadero + "\" id=\"btnVerDetalle\" class=\"ActividadesProgramadasSI\" type=\"button\"  value=\"Ver Detalle\" onclick=\"abrirpopUpSurcos($(this),$(this).parent('div.InformacionGeneraldeInvernadero').attr('idInvernadero'),$(this).parent('div.InformacionGeneraldeInvernadero').attr('name'));\"/>");
                }
                else
                {
                    sb.AppendLine("<input Invernadero=\"" + Invernadero + "\" id=\"btnVerDetalle\" type=\"button\" class=\"ActividadesProgramadasNO\" value=\"Ver Detalle\" onclick=\"abrirpopUpSurcos($(this),$(this).parent('div.InformacionGeneraldeInvernadero').attr('idInvernadero'),$(this).parent('div.InformacionGeneraldeInvernadero').attr('name'));\"/>");//onclick=\"abrirpopUpSurcos($(this),$(this).parent('div.InformacionGeneraldeInvernadero').attr('idInvernadero'),$(this).parent('div.InformacionGeneraldeInvernadero').attr('idInvernadero'));\"/>");
                }
          
                sb.AppendLine("</div>");
            }

            if (dtResult.Rows.Count > 0)
            {
                return new string[] { "1", "ok", sb.ToString() };
            }
            else
            {
                return new string[] { "0", "No se encontraron invernaderos para la planta. ", "warning" };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            return new string[] { "0", "Error: Ocurrio un fallo de sistema", "error" };
        }

    }
    //**Código de Omar
    //**Metodo modificado por Israel Loera
    [WebMethod]
    public static string[] cargaMapaPlan(int idInvernadero)
    {
        var dt = new DataSet();
        var dtPlan = new DataTable();
        string[] result = new string[2];
        decimal top = 0;
        decimal height = 0;

//<<<<<<< .mine
        
       
        try
        {
            DataAccess da = new DataAccess();
            string idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            //prm.Add("@idPlanta", idPlanta);
            prm.Add("@idInvernadero", idInvernadero);
            dt = da.executeStoreProcedureDataSet("spr_ObtenerSeccionSurco", prm);
            result[0] += "<div class=\"accordionBody\" style=\"display: table;\"><div class=\"mapa\">";
            int length = dt.Tables[0].Rows.Count;
            if (dt.Tables[0] != null && dt.Tables[0].Rows != null && dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    result[0] += "<table class='map'><tr class='mapHead'><td colspan='" + row["TotalSurcos"] + "'>" + row["numeroDeSeccion"] + "</td></tr><tr class='seccion'>";
                    foreach(DataRow row2 in dt.Tables[1].Rows)
                    {
                        if (row2["nombreSeccion"].ToString() == row["nombreDeSeccion"].ToString())
                        {
                                result[0] += "" +
                                "<td class='surco'><div class='contenedor'>";
                                top = 0;
                                height = (Convert.ToDecimal(row2["LongitudSurco"].ToString()) * 100);
                                result[0] += "<div class='tooltip invisible2'" +
                                        "title='" +
                                        "<span>" + (row2["numeroSurco"].ToString().Equals(string.Empty) ? string.Empty : "Numero de Surco:" + " " + row2["numeroSurco"].ToString()) + "</span>" +
                                        "<span>" + (row2["Estado"].ToString().Equals(string.Empty) ? string.Empty : "Estado:" + " " + row2["Estado"].ToString()) + "</span>" +
                                        "<span>" + (row2["InfestacionYnivel"].ToString().Equals(string.Empty) ? string.Empty : "Nombre y nivel de Infestacion:" + " " + row2["InfestacionYnivel"].ToString()) + "</span>" +
                                    //(row2["NivelInfestacion"].Equals(string.Empty) ? string.Empty : "Nivel de Infestacion:" + " " + row2["NivelInfestacion"].ToString()) +
                                    //(row2["nombreInfestacion"].Equals(string.Empty) ? string.Empty : "Nombre de Infestacion:" + " " + row2["nombreInfestacion"].ToString()) +
                                        "<span>" + (row2["Densidad"].ToString().Equals(string.Empty) ? string.Empty : "Densidad:" + " " + row2["Densidad"].ToString()) + "</span>" +
                                        "<span>" + (row2["Variedad"].ToString().Equals(string.Empty) ? string.Empty : "Variedad:" + " " + row2["Variedad"].ToString()) + "</span>" +
                                        "<span>" + (row2["EsRD"].ToString().Equals(string.Empty) ? string.Empty : "Es RND:" + " " + row2["EsRD"].ToString()) + "</span>" +
                                        "'" +
                                        "style='height: 100%; width: 100%; top:" + top + "%; "
                                        + "background-color:#" + row2["color"] + ";position: absolute;display: table-column;border-top: 1px solid;" + "'>" +
                                "<span class='textVertical'>" + row2["LongitudSurco"] + " " + "m</span></div>";
                                top += height;

                            result[0] += "<div style='height: 2%; width: 100%; top:49%; background-color:#F0F5E5;position: absolute;display: table-column; opacity:0.5;'></div>" +
                            "</div></td>";

                        }
                    }
                    result[0] += "<tr class='mapHead'>";
                    foreach (DataRow row2 in dt.Tables[1].Rows)
                    {
                        if (row2["nombreSeccion"].ToString() == row["nombreDeSeccion"].ToString())
                        {
                            result[0] += "" +
                            "<td>" + row2["numeroSurco"] + "</td>";
                        }
                    }
                    result[0] += "</tr></tr></table>";

                }
               
                return new string[] { "1", "", result[0].ToString() };
            }
            else
            {
                return new string[] { "0", "Error al cargar el detalle del invernadero", "warning" };
            }

        }
        catch (Exception e)
        {
            log.Error(e.Message);
            return new string[] { "0", "Error:" + e.Message + "", "error" };
        }
    }

    [WebMethod(EnableSession = true)]
    public static string setColor(string color)
    {
        var bcolor1 = Convert.ToInt32(color.Substring(0, 2), 16);
        var bcolor2 = Convert.ToInt32(color.Substring(2, 2), 16);
        var bcolor3 = Convert.ToInt32(color.Substring(4, 2), 16);

        bcolor1 = (bcolor1 - 30) > 0 ? (bcolor1 - 30) : 0;
        bcolor2 = (bcolor2 - 30) > 0 ? (bcolor2 - 30) : 0;
        bcolor3 = (bcolor3 - 30) > 0 ? (bcolor3 - 30) : 0;

        return (String.Format("{0:x}", bcolor1).ToString() == "0" ? "00" : String.Format("{0:x}", bcolor1)) + (String.Format("{0:x}", bcolor2).ToString() == "0" ? "00" : String.Format("{0:x}", bcolor2)) + (String.Format("{0:x}", bcolor3).ToString() == "0" ? "00" : String.Format("{0:x}", bcolor3));//getHexValue(bcolor1) + getHexValue(bcolor2) + getHexValue(bcolor3) + ';');
    }

    private static string generaResultadoItem(string idComponente, string clave, string valor)
    {
        return "<div id=\"" + idComponente + clave + "\" class=\"restultadoItem\" idPlan=\"" + clave + "\">" +
                                  "<span >" + valor + "</span>" +
                               "</div>";
    }

    //** Fin de Código de Omar
    
//=======
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

//>>>>>>> .r664
}