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

public partial class Jornales_frmResumenJornales : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Jornales_frmResumenJornales));
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
    public static string[] obtenerReporte(int idPlanta)
    {

        try
        {
            int idioma = 1;
            if (idPlanta.Equals(0))
            {
                idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            }
            //idPlanta = Convert.ToInt32(HttpContext.Current.Session["idPlanta"]);
            DataAccess da = new DataAccess();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idPlanta", idPlanta);
            DataSet ds = da.executeStoreProcedureDataSet("spr_ResumenJornales", prm);

            if (ds.Tables.Count > 0)
            {
                StringBuilder sbLideres = new StringBuilder();
                StringBuilder sbDatos = new StringBuilder();
                StringBuilder secondRow = new StringBuilder();

                string[] columnas = new string[] { "Jornales", "Hectareas Activas" };

                sbLideres.AppendLine("<table class=\"tblJornalesLideres\" cellspacing=\"0\">");
                sbLideres.AppendLine("<thead>");
                sbLideres.AppendLine(string.Format("<tr><th>{0}</th><th>{1}</th><th>{2}</th></tr>", "Código", "Nombre", "Area"));
                sbLideres.AppendLine(string.Format("<tr><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th></tr>"));
                sbLideres.AppendLine("</thead>");

                string Semanas = string.Empty;
                foreach (DataRow R in ds.Tables[1].Rows)
                {
                    string[] semana_Anio = R["Semana"].ToString().Split('-');
                    Semanas += string.Format("<th class=\"thsemana\" semana=\"" + semana_Anio[1] + "\" anio=\"" + semana_Anio[0] + "\" colspan=\"2\">{0}</th>", R["Semana"]);
                    secondRow.Append(string.Format("<th>{0}</th><th>{1}</th>", columnas[0], columnas[1]));
                }
                Semanas += string.Format("<th>{0}</th><th>{1}</th>","Promedio Requerido","Sugerencia de Contratacion");
                secondRow.Append("<th></th><th></th>");
                sbDatos.AppendLine("<table class=\"tblDatosResumen\" cellspacing=\"0\">");
                sbDatos.AppendLine("<thead>");
                sbDatos.AppendLine(string.Format("<tr>{0}</tr>", Semanas));
                sbDatos.AppendLine(secondRow.ToString());

                sbLideres.Append("<tbody>");
                sbDatos.Append("<tbody>");

                string[] semanaAnio = new string[2];
                foreach (DataRow R in ds.Tables[0].Rows)
                {
                    string idLider = R["idLider"].ToString().Trim()
                           , idLiderAD = R["idLiderAD"].ToString().Trim()
                           , nombreLider = R["NombreLider"].ToString().Trim()
                           ,nombreDepartamento = R["NombreDepartamento"].ToString().Trim();

                    StringBuilder sbJornalesHectareas = new StringBuilder();
                    foreach (DataRow S in ds.Tables[1].Rows)
                    {
                        semanaAnio = S["Semana"].ToString().Split('-');
                        foreach (string col in columnas)
                        {
                            if (col.Equals("Jornales"))
                            {
                                sbJornalesHectareas.Append(string.Format("<td class=\"Jornales\" semana=\"" + semanaAnio[1] + "\" anio=\"" + semanaAnio[0] + "\">{0}</td>",
                                        ds.Tables[0].Select("idLider=" + idLider + " AND idLiderAD="+idLiderAD+"").Length > 0 ?
                                        ds.Tables[0].Select("idLider=" + idLider + " AND idLiderAD=" + idLiderAD + "")[0]["AsociadosAsignados"].ToString() : "0"
                                    ));
                            }
                            else 
                            {
                                sbJornalesHectareas.Append(string.Format("<td class=\"Hectareas\" semana=\"" + S["Semana"].ToString() + "\">{0}</td>",
                                        ds.Tables[2].Select("idLider=" + idLider + " AND semana=" + semanaAnio[1] + " AND anio=" + semanaAnio[0] + "").Length > 0 ?
                                        ds.Tables[2].Select("idLider=" + idLider + " AND semana=" + semanaAnio[1] + " AND anio=" + semanaAnio[0] + "")[0]["hectareas"].ToString() : "0"
                                    ));
                            }
                        }

                        //sbJornalesHectareas.Append("<td class=\"PromedioRequerido\" semana=\"" + S["Semana"].ToString() + "\"> &nbsp; </td>");
                        //sbJornalesHectareas.Append("<td class=\"SugerenciaContratacion\" semana=\"" + S["Semana"].ToString() + "\"> &nbsp; </td>");
                    }

                    sbLideres.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",idLider,nombreLider,nombreDepartamento));
                    sbDatos.AppendLine(string.Format("<tr class=\"trRow\">{0}<td class=\"PromedioRequerido\"></td><td class=\"SugerenciaContratacion\"></td></tr>", sbJornalesHectareas.ToString(), "&nbsp;", "&nbsp;"));
                           
                }

                sbLideres.AppendLine("</tbody></table>");
                sbDatos.AppendLine("</tbody></table>");
                //PanelA.InnerHtml = sbLideres.ToString();
                //PanelB.InnerHtml = sbDatos.ToString();

                return new string[] { "1", "ok", sbLideres.ToString(),sbDatos.ToString() };
            }
            else
            {
                return new string[] { "0", "Error en la respuesta de base de datos.", "warning" };
                //PanelB.InnerHtml = "<h3>Error en la respuesta de base de datos.</h3><span class=\"invisible\"> DataSet vacio.</span>";
            }

        }
        catch (Exception e)
        {
            Log.Error(e);
            return new string[] { "0", "Error al procesar los datos de entrada.", "warning" };

            //PanelB.InnerHtml = "<h3>Error al procesar los datos de entrada.</h3><span class=\"invisible\"> " + e.Message + "</span>";
        }

    }


}