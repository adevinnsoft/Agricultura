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
using log4net;

public partial class Jornales_Default : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Jornales_Default));
    public static int idPlanta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Page.IsPostBack) { }
        //else
        //{
        //    obtenerReporte();
        //}
    }

    [WebMethod]
    public static string[] obtenerReporte(int idPlanta, int semana, int anio, int pronostico) {
        try
        {
            int idioma = 1;
            if (idPlanta.Equals(0))
            {
                idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            }
            DataAccess da = new DataAccess();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idPlanta", idPlanta);
            prm.Add("@semana", semana);
            prm.Add("@anio", anio);
            prm.Add("@semanasDeProyeccion", pronostico);
            DataSet ds = da.executeStoreProcedureDataSet("spr_Jornales_PorHectarea", prm);
           
            if (ds.Tables.Count > 0)
            {
                StringBuilder sbA = new StringBuilder();
                StringBuilder sbB = new StringBuilder(); 
                StringBuilder secondRow = new StringBuilder();
                List<string> familias = new List<string>();

              
                string[] Hectareas = new string[] {"Activas","Cosecha", "Cultivo", "Preparacion","Plantadas"};    

          

                sbA.AppendLine("<table class=\"tblJornalesAsociados\" cellspacing=\"0\">");
                sbA.AppendLine("<thead>");
                sbA.AppendLine(string.Format("<tr><th>{0}</th><th>{1}</th></tr>", "Código", "Nombre"));
                sbA.AppendLine(string.Format("<tr><th>&nbsp;</th><th>&nbsp;</th></tr>"));
                sbA.AppendLine("</thead>");
                
                string Familias  = string.Empty;

                DataView view = new DataView(ds.Tables[4]);
                DataTable dtFamilias = view.ToTable(true, "Familia", "Familia_EN");

                string columna = idioma == 1 ? "Familia" : "Familia_EN";
                secondRow.Append("<tr><th></th><th></th><th></th><th></th>");
                
                foreach (DataRow R in dtFamilias.Rows)
	            {
                    Familias += "<th rowspan=\"2\">" + R[columna] + "</th>";
                    secondRow.Append("<th></th>");
                }
                secondRow.Append("<th></th><th></th>");
                string Semanas = string.Empty;
                foreach (DataRow R in ds.Tables[6].Rows)
                {
                    Semanas += string.Format("<th colspan=\"5\">{0}</th><th>{1}</th>", R["Semana"], "Jornales por HA Activa");
                    secondRow.Append(string.Format("<th class=\"hectareas\">{0}</th><th class=\"hectareas\" semana=\"" + R["Semana"] + "\">{1}</th><th class=\"hectareas\" semana=\"" + R["Semana"] + "\">{2}</th><th class=\"hectareas\" semana=\"" + R["Semana"] + "\">{3}</th><th class=\"hectareas\" semana=\"" + R["Semana"] + "\">{4}</th><th></th>", Hectareas[0], Hectareas[1], Hectareas[2], Hectareas[3], Hectareas[4]));
                }
                sbB.AppendLine("<table class=\"tblDatosJornales\" cellspacing=\"0\">");
                sbB.AppendLine("<thead>");
                sbB.AppendLine(string.Format("<tr><th rowspan=\"2\">{0}</th><th rowspan=\"2\">{1}</th><th rowspan=\"2\">{2}</th><th rowspan=\"2\">{3}</th>{4}<th rowspan=\"2\">{5}</th><th rowspan=\"2\">{6}</th>{7}</tr>"
                    ,"Asociados Asignado"	
                    ,"Asociados Uso"
                    ,"Incapacitados"
                    ,"Fijos"
                    , Familias
                    ,"Área"
                    ,"Invernaderos"
                    , Semanas
                    ));
                sbB.AppendLine("</thead>");
                sbB.AppendLine(secondRow.ToString());
                foreach (DataRow R in ds.Tables[0].Rows)
                {
                   
                }

                sbA.Append("<tbody>");
                sbB.Append("<tbody>");
                foreach (DataRow l in ds.Tables[0].Rows)
                {
                    string idLider = l["idLider"].ToString();
                    string nombreLider = l["NombreLider"].ToString();
                    string AsociadosAsignados = l["AsociadosAsignados"].ToString();
                    string AsociadosEnUso =ds.Tables[1].Select("idLider="+idLider).Length > 0 
                            ? ds.Tables[1].Select("idLider="+idLider)[0]["AsociadosEnUso"].ToString()
                            : "0" ;
                    string AsociadosIncapacitados =ds.Tables[2].Select("idLider="+idLider).Length > 0 
                            ? ds.Tables[2].Select("idLider="+idLider)[0]["AsociadosIncapacitados"].ToString()
                            : "0" ;
                    string Fijos =ds.Tables[3].Select("idLider="+idLider).Length > 0 
                            ? ds.Tables[3].Select("idLider="+idLider)[0]["Fijos"].ToString()
                            : "0" ;
                    StringBuilder sbAsociadosEnFamilia = new StringBuilder();
                    StringBuilder sinFamilia = new StringBuilder();

                    foreach (DataRow f in dtFamilias.Rows)
                    {
                        string familia = f["Familia"].ToString();

                            sbAsociadosEnFamilia.Append(string.Format("<td class=\"Familia\" >{0}</td>",
                                ds.Tables[4].Select("idLider=" + idLider + " AND Familia='" + familia+ "'").Length > 0 ?
                                ds.Tables[4].Select("idLider=" + idLider + " AND Familia='" + familia + "'")[0]["AsociadosPorFamilia"].ToString()
                                : "0"
                             ));
                    }

                    string NombreDepartamento = ds.Tables[5].Select("idLider=" + idLider).Length > 0
                           ? ds.Tables[5].Select("idLider=" + idLider)[0]["NombreDepartamento"].ToString()
                           : "0";
                    string Invernaderos = ds.Tables[5].Select("idLider=" + idLider).Length > 0
                            ? ds.Tables[5].Select("idLider=" + idLider)[0]["Invernaderos"].ToString()
                            : "0";
                    
                    StringBuilder sbHectareas = new StringBuilder();
                    foreach (DataRow S in ds.Tables[6].Rows)
                    {   
                        string[] W = S["Semana"].ToString().Split('-');
                        foreach (string H in Hectareas)
	                    {
                            if (H.Equals("Activas"))
                            {
                                sbHectareas.Append(string.Format("<td class=\"Activas\" Semana=\"{1}\">{0}</td>",
                                    //ds.Tables[7].Select("idLider=" + idLider + " AND semana=" + W[1] + " AND anio=" + W[0] + " AND tipo='" + H + "'").Length > 0 ?
                                    //ds.Tables[7].Select("idLider=" + idLider + " AND semana=" + W[1] + " AND anio=" + W[0] + " AND tipo='" + H + "'")[0]["hectareas"].ToString()
                                    //: 
                                    "0"
                                    , S["Semana"].ToString() 
                                ));
                            }
                            else
                            {
                                sbHectareas.Append(string.Format("<td  class=\"Hectareas\" Semana=\"{1}\" >{0}</td>",
                                    ds.Tables[7].Select("idLider=" + idLider + " AND semana=" + W[1] + " AND anio=" + W[0] + " AND tipo='" + H + "'").Length > 0 ?
                                    ds.Tables[7].Select("idLider=" + idLider + " AND semana=" + W[1] + " AND anio=" + W[0] + " AND tipo='" + H + "'")[0]["hectareas"].ToString()
                                    : "0"
                                    , S["Semana"].ToString() 
                                ));
                            }
                        }
                        sbHectareas.Append("<td class=\"JHA\" Semana=\""+S["Semana"].ToString()+"\"> &nbsp; </td>");
                    }

                    sbA.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", idLider, nombreLider));
                    sbB.AppendLine(string.Format("<tr class=\"trRow\"><td>{0}</td><td>{1}</td><td>{2}</td><td class=\"Fijos\">{3}</td>{4}{5}<td>{6}</td><td>{7}</td>{8}</tr>"
                            , AsociadosAsignados
                            , AsociadosEnUso
                            , AsociadosIncapacitados
                            , Fijos
                            , sinFamilia.ToString()
                            , sbAsociadosEnFamilia.ToString()
                            , NombreDepartamento
                            , Invernaderos
                            , sbHectareas.ToString()
                            ));

                }

                sbA.AppendLine("</tbody></table>");
                sbB.AppendLine("</tbody></table>");
                //PanelA.InnerHtml=sbA.ToString();
                //PanelB.InnerHtml=sbB.ToString();

                return new string[] { "ok", sbA.ToString(), sbB.ToString() };
                //return new string[] { "ok", ""};
            }
            else
            {
                return new string[] { "error", "Error en la respuesta de base de datos.", "warning" };
                 //PanelB.InnerHtml = "<h3>Error en la respuesta de base de datos.</h3><span class=\"invisible\"> DataSet vacio.</span>";
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            return new string[] { "error", "Error al procesar los datos de entrada.", e.Message };
            //PanelB.InnerHtml = "<h3>Error al procesar los datos de entrada.</h3><span class=\"invisible\"> "+e.Message+"</span>";
        }
    }


}