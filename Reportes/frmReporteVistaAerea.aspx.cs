using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Text;
using log4net;

public partial class Reportes_frmReporteVistaArea : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Reportes_frmReporteVistaArea));
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static string llenatablaInvernadero()
    {
        //try
        //{
        //    DataAccess da = new DataAccess();
        //    DataTable dt = da.executeStoreProcedureDataTable("spr_ParametrosDeEvaluacionObtenerPorIdCaptura", new Dictionary<string, object>() { 
        //     {"@idCaptura", 1 } //VDA
        //    //,{"@idioma", idioma } 
        //});

            StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("<tr class=\"parametrosDeEvaluacion\"><td colspan=\"6\"><h3>Parámetros de Evaluación</h3></td></tr>");
        //    sb.AppendLine("<tr class=\"parametrosDeEvaluacion\">");
        //    if (dt.Rows.Count > 0)
        //    {
        //        int counter = 0;
        //        foreach (DataRow R in dt.Rows)
        //        {
        //            bool Requerido = R["EsRequerido"].ToString().ToLower().Equals("true") ? true : false;
        //            sb.AppendLine("<td><label>" + (Requerido ? "*" : string.Empty) + R["Parametro"].ToString().Trim() + ": </label></td><td>");
        //            string EsRequerido = Requerido ? "required" : string.Empty;
        //            string idParametro = R["IdParametroEvaluacion"].ToString().Trim();
        //            switch (int.Parse(R["IdTipo"].ToString()))
        //            {               /*  Tipos  1: Cadena       2: Bool         3: Int      4: Decimal   */
        //                case 1: sb.AppendLine("<input idParametro=\"" + idParametro + "\" type=\"text\" class=\"stringValidate " + EsRequerido + "\">"); break;
        //                case 2: sb.AppendLine("<input idParametro=\"" + idParametro + "\" type=\"checkbox\" class=\"stringValidate " + EsRequerido + "\">"); break;
        //                case 3: sb.AppendLine("<input idParametro=\"" + idParametro + "\" type=\"text\" class=\"intValidate " + EsRequerido + "\">"); break;
        //                case 4: sb.AppendLine("<input idParametro=\"" + idParametro + "\" type=\"text\" class=\"floatlValidate " + EsRequerido + "\">"); break;
        //                default:
        //                    break;
        //            }
        //            sb.Append("</td>");
        //            counter++;
        //            if (counter % 3 == 0)
        //            {
        //                sb.Append("</tr>");
        //                sb.AppendLine("<tr class=\"fijo\">");
        //            }
        //        }
        //        sb.Append("</tr>");
        //        return new string[] { "1", "ok", sb.ToString() };
        //    }
        //    else
        //    {
        //        return new string[] { "0", "error", string.Empty };//No se encontraron parametros de evaluación.
        //    }
        //}
        //catch (Exception x)
        //{
        //    log.Error(x);
        //    return new string[] { "0", "error", string.Empty };
            //}005722
//#F4D101
            sb.AppendLine("<table><tr><td><div id=\"aerea\" onclick=\"popUpMostrar($(this));\" style=\"background-color:#F4D101 ;\" border-style: solid double;><p>2A01</p></div></td><td>&nbsp;</td><td><div id=\"aerea\" style=\"background-color:#F4D101 ;\" border-style: solid double;><p>2B12</p></div></td><td>&nbsp;</td><td><div id=\"aerea\" style=\"background-color:#F4D101 ;\" border-style: solid double;><p>2C01</p></div></td></tr></table>");
        //    sb.AppendLine(" <div id=\"wrapper\"><div id=\"tabs\"> <ul><li><a href=\"#tabs-1\" title=\"\">Tab 1</a></li><li><a href=\"#tabs-2\" title=\"\">Tab 2</a></li><li><a href=\"#tabs-3\" title=\"\">Tab 3</a></li></ul>");
        //     sb.AppendLine("<div id='tabs_container'>  <div id='tabs-1'><p>Proinsus.</p><p>Aene s vel pede . Nuns.</p></div>");
        //       sb.AppendLine("<div id='tabs-2'><p>Morbi malesuad</p>");
        //       sb.AppendLine("</div><div id='tabs-3'><p>Mtrem.</p><p> Vestibules.</p>");
        //sb.AppendLine("</div></div><!--End tabs container--> </div><!--End tabs--><div id='tabs2'><ul>");
        //sb.AppendLine("<li><a href='#tabs-1' title=''>Tab 1</a></li><li><a href='#tabs-2' title=''>Tab 2</a></li><li><a href='#tabs-3' title=''>Tab 3</a></li></ul>");
        //sb.AppendLine("<div id='tabs_container'><div id='tabs-1'><p>Procrisus.</p><p>Aenean lectus.</p>");
        //sb.AppendLine("</div><div id='tabs-2'><p>Morbi tina masa  ut dolor.</p>");
        //sb.AppendLine("</div><div id='tabs-3'><p>Mauris elem eget lorem.</p><p> Vos. Fusce sodales.</p>");
        //sb.AppendLine("</div> </div><!--End tabs container--></div><!--End tabs--><div id='tabs3'>");
        //sb.AppendLine("<ul><li><a href='#tabs-1' title=''>Tab 1</a></li><li><a href='#tabs-2' title=''>Tab 2</a></li><li><a href='#tabs-3' title=''>Tab 3</a></li></ul>");
        //sb.AppendLine("<div id='tabs_container'><div id='tabs-1'><p>Proin us lacus auctor risus.</p><p>Aenean te</p></div>");
        //sb.AppendLine("<div id='tabs-2'><p>Morbuada,a ut dolor.</p></div>");
        //sb.AppendLine("<div id='tabs-3'><p>Maurit lorem.</p><p> Ver conubia nostra, perdales.</p></div>");
        //sb.AppendLine("</div><!--End tabs container--></div><!--End tabs--><div id='tabs4'><ul><li><a href='#tabs-1' title=''>Tab 1</a></li>");
        //sb.AppendLine("<li><a href='#tabs-2' title=''>Tab 2</a></li><li><a href='#tabs-3' title=''>Tab 3</a></li></ul>");
        //sb.AppendLine("<div id='tabs_container'><div id='tabs-1'><p>Proicitudin m. Mauris dapibus lacus auctor risus.</p><p>Aenea ipsum. Nunc tristiqueectus.</p></div>");
        //sb.AppendLine("<div id='tabs-2'><p>Morbi tin</p></div>");
        //sb.AppendLine("<div id='tabs-3'><p>Maureget lorem.</p><p> Vestisodales.</p></div>");
        //sb.AppendLine("</div><!--End tabs container--></div><!--End tabs--></div>");
       
      
        //div id=\"aerea\"> <p>Lorem</p></div>
        return sb.ToString(); ;
    }


    [WebMethod]
    public static Object ObtenerContenidoInvernadero()//string strInvernadero, int idInvernadero, int intSecciones, int intSurcos)
    {
        var result = "";
        var intSurco = 1;

        //try
        //{
        //    for (int intCount = 1; intCount <= intSecciones; intCount++)
        //    {
        //        result = result + "<h3 id='Seccion" + intCount.ToString() + "' NoSeccion='" + intCount.ToString() + "'>Sección" + intCount.ToString() + "</h3><table border='1'>";
        //        result = result + "<tr><th>Surco</th><th>Longitud (m)</th><th>Investigación</th><th>Activo</th><th>Eliminar</th><th>Añadir</th></tr>";
        //        for (int intCount2 = 1; intCount2 <= intSurcos; intCount2++)
        //        {
        //            result = result + "<tr>";
        //            result = result + "<td><span id='Surco" + intSurco.ToString() + " NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "'>" + intSurco.ToString() + "</span></td>";
        //            result = result + "<td><input type='text' /></td>";
        //            result = result + "<td><input type='checkbox' /></td>";
        //            result = result + "<td><input type='checkbox' /></td>";
        //            result = result + "<td><img src='../comun/img/remove-icon.png' /></td>";
        //            result = result + "<td><img src='../comun/img/add-icon.png' /></td>";
        //            result = result + "</tr>";
        //            intSurco = intSurco + 1;
        //        }
        //        result = result + "</table>";
        //    }
        //    result = result + "<br />";
        //    result = result + "<input type='button' value='Guardar' />";
        //}
        //catch (Exception ex)
        //{
        //    return "<script>popUpAlert('Error');</script>";
        //}
        result = result = result + "<h3 id='Seccion1' NoSeccion='1'>Información de Surcos</h3><table class='dd' border='1'>";
        //result = result + "<tr><th>Surco</th><th>Longitud (m)</th><th>Investigación</th><th>Activo</th><th>Eliminar</th><th>Añadir</th></tr>";
        result = result + "<tr><th>Surco</th><th>Longitud (m)</th>";
        for (int intCount2 = 1; intCount2 <= 4; intCount2++)//intSurcos; intCount2++)
        {
            result = result + "<tr>";
            result = result + "<td><span id='Surco" + intSurco.ToString() + " NoSurco='" + intSurco.ToString() + "' NoSeccion='1'>1</span></td>";
            result = result + "<td><input type='text' /></td>";
            //result = result + "<td><input type='checkbox' /></td>";
            //result = result + "<td><input type='checkbox' /></td>";
            //result = result + "<td><img src='../comun/img/remove-icon.png' /></td>";
            //result = result + "<td><img src='../comun/img/add-icon.png' /></td>";
            result = result + "</tr>";
            intSurco = intSurco + 1;
        }
        result = result + "</table>";
        return result;
    }
}