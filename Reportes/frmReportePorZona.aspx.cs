using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Globalization;

public partial class Reportes_frmReportePorZona : BasePage // System.Web.UI.Page//
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] ObtieneCombos()
    {
        DataAccess da = new DataAccess();
        DataSet dtCombos = da.executeStoreProcedureDataSet("spr_ObtieneCombosReporteXZonaGrowing", new Dictionary<string, object>() { 
            {"@idPlanta", HttpContext.Current.Session["idPlanta"]}
        });

        StringBuilder sbSemana = new StringBuilder();
        sbSemana.AppendLine("<option value=\"\" semanaCorta=\"0\">-- Seleccione --</option>");
        foreach (DataRow S in dtCombos.Tables[0].Rows)
        {
            string semana = S["semana"].ToString(), semanaCorta = S["semanaCorta"].ToString();
            sbSemana.AppendLine("<option value=\"" + semana + "\" semanaCorta=\"" + semanaCorta + "\">" + semana + "</option>");
        }

        DataView ViewL = new DataView(dtCombos.Tables[1]);
        DataTable dtLider = ViewL.ToTable(true, "idLider", "nombreLider", "semana");
        StringBuilder sbLider = new StringBuilder();
        sbLider.AppendLine("<option value=\"\" semana=\"\">-- Seleccione --</option>");
        foreach (DataRow L in dtLider.Rows)
        {
            string idLider = L["idLider"].ToString(), lider = L["nombreLider"].ToString(), semana = L["semana"].ToString();
            sbLider.AppendLine("<option value=\"" + idLider + "\" semana=\"" + semana + "\">" + lider + "</option>");
        }

        return new string[] { "1", "ok", sbSemana.ToString(), sbLider.ToString() };
    }

    public static string Colorear(string valor, bool resumenGrupo)
    {
        var Color = "";


        if(valor != "")
        {
            if (resumenGrupo == true)
            {
                if (System.Convert.ToDecimal(valor) >= 90)
                {
                    Color = "green";
                }
                else if (System.Convert.ToDecimal(valor) <= 90 && System.Convert.ToDecimal(valor) >= 70)
                {
                    Color = "orange";
                }
                else
                {
                    Color = "red";
                }
            }
            else
            {
                if (System.Convert.ToDecimal(valor) > 90)
                {
                    Color = "green";
                }
                else if (System.Convert.ToDecimal(valor) <= 90 && System.Convert.ToDecimal(valor) >= 70)
                {
                    Color = "orange";
                }
                else
                {
                    Color = "red";
                }
            }
        }

        return Color;
    }

    [WebMethod]
    public static string[] GenerarReportePlantacion(string semana, int idLider)
    {
        try
        {
            //-------------------------------------------- SECCIÓN (GH PLANTACION) ------------------------------------------
            DataAccess da = new DataAccess();
            DataSet dt = da.executeStoreProcedureDataSet("spr_ObtenerReporteXZonaGrowing", new Dictionary<string, object>() { 
                {"@idPlanta", HttpContext.Current.Session["idPlanta"]},
                {"@semana", semana},
                {"@idLider", idLider},
                {"@idioma",getIdioma()}
            });

            string Errores = string.Empty;
            string Alertas = string.Empty;
            decimal CalTotalPlantacion = 0;
            decimal CalTotalNOPlantacion = 0;
            var cuentaInvernaderos = 0;

            StringBuilder sbResumenGrupoPlantacion = new StringBuilder();
            StringBuilder sbCalificacionGrupoPlantacion = new StringBuilder();
            StringBuilder sbProblemasGrupoPlantacion = new StringBuilder();
            StringBuilder sbInvernaderoPlantacion = new StringBuilder();

            if (dt.Tables[7].Rows[0]["Estado"].ToString().Equals("0"))
            {
                Errores += "Error en la obtención de datos de Plantación. <br />";
            } else {
                if (dt.Tables[0].Rows.Count == 0 && dt.Tables[3].Rows.Count == 0)
                {
                    Alertas += "No se encontraron capturas de Plantación con los datos selecccionados. <br/ >";
                }
                else
                {
                    ////***************************** RESUMEN X GRUPO (GH PLANTACION) *****************************
                    //sbResumenGrupoPlantacion
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        var Grupo = dr["Grupo"].ToString();
                        var Cumplimiento = dr["Cumplimiento"].ToString();
                        var Calificacion = dr["Calificacion"].ToString();
                        var Distribucion = dr["Distribucion"].ToString();

                        var Line = "<tr>" +
                                        "<td>" + Grupo + "</td>" +
                                        "<td class=\"info CumpPlantacionGrupo " + Colorear(Cumplimiento, true) + "\">" + (Cumplimiento == "" ? "" : Cumplimiento + "%") + "</td>" +
                                        "<td class=\"info CalPlantacionGrupo " + Colorear(Cumplimiento, true) + "\">" + (Calificacion == "" ? "" : Calificacion + "%") + "</td>" +
                                        "<td class=\"info DistPlantacionGrupo\">" + Distribucion + "% </td>" +
                                   "</tr>";

                        sbResumenGrupoPlantacion.AppendLine(Line);
                    }

                    //sbCalificacionGrupoPlantacion
                    foreach (DataRow dr in dt.Tables[1].Rows)
                    {
                        var TotalCumplimiento = dr["TotalCumplimiento"].ToString();
                        var TotalCalificacion = dr["TotalCalificacion"].ToString();

                        var Line = "<tr class=\"calificacion\">" +
                                       "<td>CALIFICACIÓN</td>" +
                                       "<td class=\"info CumpPlantacionGrupoTotal " + Colorear(TotalCumplimiento, false) + "\">" + TotalCumplimiento + "%</td>" +
                                       "<td class=\"info CalPlantacionGrupoTotal " + Colorear(TotalCalificacion, false) + "\">" + TotalCalificacion + "%</td>" +
                                   "</tr>";
                        sbCalificacionGrupoPlantacion.AppendLine(Line);
                    }

                    var y = 1;
                    var ProblemasG = "";
                    //sbProblemasGrupoPlantacion
                    foreach (DataRow dr in dt.Tables[2].Rows)
                    {
                        var problema = dr["Grupo"].ToString();
                        var jerarquia = dr["JERARQUIA"].ToString();

                        ProblemasG += "<tr>" +
                                        "<td class=\"blue" + jerarquia + " tdProblema\"> PROBLEMA " + jerarquia + "</td>" +
                                        "<td class=\"info\">" + problema + "</td>" +
                                   "</tr>";
                        y++;
                    }
                    while (y <= 3)
                    {
                        ProblemasG += "<tr>" +
                                        "<td class=\"blue" + y + " tdProblema\">Problema" + y + "</td>" +
                                        "<td></td>" +
                                     "</tr>";
                        y++;
                    }
                    sbProblemasGrupoPlantacion.AppendLine(ProblemasG);

                    ////***************************** RESUMEN X INVERNADERO (GH PLANTACION) *****************************
                    //sbInvernaderoPlantacion
                    foreach (DataRow dr in dt.Tables[4].Rows)
                    {
                        var idInvernadero = dr["idInvernadero"].ToString();
                        var invernadero = dr["invernadero"].ToString();
                        var idStatus = dr["idStatus"].ToString();
                        var status = dr["StatusVisita"].ToString();
                        var porcentaje = idStatus != "0" ? "" : dr["Porcentaje"].ToString();
                        var Comentarios = dr["Comentarios"].ToString() == "" ? "Sin Comentarios Registrados" : dr["Comentarios"].ToString();
                        var LineGrafica = "";

                        //Obtenemos los datos para Graficar
                        foreach (DataRow dr2 in dt.Tables[3].Rows)
                        {
                            var Grupo = dr2["Grupo"].ToString();
                            var Calificacion = dr2["Calificacion"].ToString();
                            var Distribucion = dr2["Distribucion"].ToString();

                            if (dr2["idInvernadero"].ToString() == dr["idInvernadero"].ToString())
                            {
                                LineGrafica += "<tr>" +
                                                    "<td class=\"GrupoGH\">" + Grupo + "</td>" +
                                                    "<td class=\"CalificacionGH\">" + Calificacion + "</td>" +
                                                    "<td class=\"DistribucionGH\">" + Distribucion + "</td>" +
                                               "</tr>";
                            }
                        }

                        //Obtenemos los problemas
                        var x = 1;
                        var Problemas = "";
                        foreach (DataRow dr3 in dt.Tables[5].Rows)
                        {
                            var Grupo = dr3["Grupo"].ToString();

                            if (dr3["idInvernadero"].ToString() == dr["idInvernadero"].ToString() && x <= 3)
                            {
                                Problemas += "<tr>" +
                                                "<td class=\"blue" + x + " tdProblema\">Problema" + x + "</td>" +
                                                "<td>" + Grupo + "</td>" +
                                             "</tr>";
                                x++;
                            }
                        }
                        while (x <= 3)
                        {
                            Problemas += "<tr>" +
                                            "<td class=\"blue" + x + " tdProblema\">Problema" + x + "</td>" +
                                            "<td></td>" +
                                         "</tr>";
                            x++;
                        }

                        //Armado del html por invernadero
                        var Line = "<div class=\"ResInvContainer\">" +
                                       "<div>" +
                                           "<table id=\"tblGHPlantacion" + invernadero + "\" status=\"" + idStatus + "\">" +
                                               "<tr>" +
                                                    "<td class=\"GHVisitado\">" + status + "</td>" +
                                               "</tr>" +
                                               "<tr>" +
                                                    "<td class=\"GH\">" + invernadero + "</td>" +
                                               "</tr>" +
                                               "<tr>" +
                                                    "<td class=\"GHPorcentaje " + Colorear(porcentaje, false) + "\" >" + porcentaje + "%</td>" +
                                               "</tr>" +
                                           "</table>" +
                                       "</div>" +
                                       "<div id=\"GHPlantacionGrafica" + invernadero + "\" class=\"divChart\">" +
                                           "<table id=\"tblGrafica" + invernadero + "\" style=\"display:none\">" +
                                                LineGrafica +
                                           "</table>" +
                                       "</div>" +
                                       "<div>" +
                                           "<table id=\"tblGHProblemasPlantacion" + invernadero + "\">" +
                                                "<tr><th class=\"GHProblemas\" colspan=\"2\">Problemas</th></tr>" +
                                                Problemas +
                                           "</table>" +
                                       "</div>" +
                                       "<div>" +
                                           "<table id=\"tblGHComentariosPlantacion" + invernadero + "\">" +
                                                "<tr><th class=\"GHProblemas\">Sugerencias</th></tr>" +
                                                "<td class=\"Comentarios\">" + Comentarios + "</td>" +
                                           "</table>" +
                                       "</div>" +
                                   "</div>";
                        sbInvernaderoPlantacion.AppendLine(Line);
                    }

                    ////***************************** CALIFICACIÓN TOTAL X INVERNADERO (GH PLANTACION) *****************************
                    foreach (DataRow dr in dt.Tables[6].Rows)
                    {
                        var calificacion = dr["CalTotalInvernadero"].ToString();
                        CalTotalPlantacion = CalTotalPlantacion + decimal.Parse(calificacion);
                        cuentaInvernaderos = cuentaInvernaderos + 1;
                    }
                }
            }


            //------------------------------------------------ SECCIÓN (GH NO PLANTACION) ------------------------------------------
            DataSet dtNP = da.executeStoreProcedureDataSet("spr_ObtenerReporteXZonaGrowingNP", new Dictionary<string, object>() { 
                {"@idPlanta", HttpContext.Current.Session["idPlanta"]},
                {"@semana", semana},
                {"@idLider", idLider},
                {"@idioma",getIdioma()}
            });

            StringBuilder sbResumenGrupoNOPlantacion = new StringBuilder();
            StringBuilder sbCalificacionGrupoNOPlantacion = new StringBuilder();
            StringBuilder sbProblemasGrupoNOPlantacion = new StringBuilder();
            StringBuilder sbInvernaderoNOPlantacion = new StringBuilder();

            if (dtNP.Tables[7].Rows[0]["Estado"].ToString().Equals("0"))
            {
                Errores += "Error en la obtención de datos de No Plantación. <br />";
            } else {
                if (dtNP.Tables[0].Rows.Count == 0 && dtNP.Tables[3].Rows.Count == 0)
                {
                    Alertas += "No se encontraron capturas de No Plantcación con los datos selecccionados. <br />";
                }
                else
                {
                    ////***************************** RESUMEN X GRUPO (GH NO PLANTACION) *****************************
                    //sbResumenGrupoNOPlantacion
                    foreach (DataRow dr in dtNP.Tables[0].Rows)
                    {
                        var Grupo = dr["NombreGrupo"].ToString();
                        var Cumplimiento = dr["Cumplimiento"].ToString();
                        var Calificacion = dr["Calificacion"].ToString();
                        var Distribucion = dr["Distribucion"].ToString();

                        var Line = "<tr>" +
                                        "<td>" + Grupo + "</td>" +
                                        "<td class=\"info CumpPlantacionGrupo " + Colorear(Cumplimiento, true) + "\">" + (Cumplimiento == "" ? "" : Cumplimiento + "%") + "</td>" +
                                        "<td class=\"info CalPlantacionGrupo " + Colorear(Cumplimiento, true) + "\">" + (Calificacion == "" ? "" : Calificacion + "%") + "</td>" +
                                        "<td class=\"info DistPlantacionGrupo\">" + Distribucion + "% </td>" +
                                   "</tr>";

                        sbResumenGrupoNOPlantacion.AppendLine(Line);
                    }

                    //sbCalificacionGrupoNOPlantacion
                    foreach (DataRow dr in dtNP.Tables[1].Rows)
                    {
                        var TotalCumplimiento = dr["TotalCumplimiento"].ToString();
                        var TotalCalificacion = dr["TotalCalificacion"].ToString();

                        var Line = "<tr class=\"calificacion\">" +
                                       "<td>CALIFICACIÓN</td>" +
                                       "<td class=\"info CumpPlantacionGrupoTotal " + Colorear(TotalCumplimiento, false) + "\">" + TotalCumplimiento + "%</td>" +
                                       "<td class=\"info CalPlantacionGrupoTotal " + Colorear(TotalCalificacion, false) + "\">" + TotalCalificacion + "%</td>" +
                                   "</tr>";
                        sbCalificacionGrupoNOPlantacion.AppendLine(Line);
                    }

                    var y = 1;
                    var ProblemasG = "";
                    //sbProblemasGrupoNOPlantacion
                    foreach (DataRow dr in dtNP.Tables[2].Rows)
                    {
                        var problema = dr["NombreGrupo"].ToString();
                        var jerarquia = dr["JERARQUIA"].ToString();

                        ProblemasG += "<tr>" +
                                        "<td class=\"blue" + jerarquia + " tdProblema\"> PROBLEMA " + jerarquia + "</td>" +
                                        "<td class=\"info\">" + problema + "</td>" +
                                   "</tr>";
                        y++;
                    }
                    while (y <= 3)
                    {
                        ProblemasG += "<tr>" +
                                        "<td class=\"blue" + y + " tdProblema\">Problema" + y + "</td>" +
                                        "<td></td>" +
                                     "</tr>";
                        y++;
                    }
                    sbProblemasGrupoNOPlantacion.AppendLine(ProblemasG);

                    ////***************************** RESUMEN X INVERNADERO (GH NO PLANTACION) *****************************
                    //sbInvernaderoNOPlantacion
                    foreach (DataRow dr in dtNP.Tables[4].Rows)
                    {
                        var idInvernadero = dr["idInvernadero"].ToString();
                        var invernadero = dr["invernadero"].ToString();
                        var idStatus = dr["idStatus"].ToString();
                        var status = dr["StatusVisita"].ToString();
                        var porcentaje = idStatus != "0" ? "" : dr["Porcentaje"].ToString();
                        var Comentarios = dr["Comentarios"].ToString() == "" ? "Sin Comentarios Registrados" : dr["Comentarios"].ToString();
                        var LineGrafica = "";

                        //Obtenemos los datos para Graficar
                        foreach (DataRow dr2 in dtNP.Tables[3].Rows)
                        {
                            var Grupo = dr2["GRUPO"].ToString();
                            var Calificacion = dr2["CALIFICACION"].ToString();
                            var Distribucion = dr2["DISTRIBUCION"].ToString();

                            if (dr2["IDINVERNADERO"].ToString() == dr["idInvernadero"].ToString())
                            {
                                LineGrafica += "<tr>" +
                                                    "<td class=\"GrupoGH\">" + Grupo + "</td>" +
                                                    "<td class=\"CalificacionGH\">" + Calificacion + "</td>" +
                                                    "<td class=\"DistribucionGH\">" + Distribucion + "</td>" +
                                               "</tr>";
                            }
                        }

                        //Obtenemos los problemas
                        var x = 1;
                        var Problemas = "";
                        foreach (DataRow dr3 in dtNP.Tables[5].Rows)
                        {
                            var Grupo = dr3["Grupo"].ToString();

                            if (dr3["idInvernadero"].ToString() == dr["idInvernadero"].ToString() && x <= 3)
                            {
                                Problemas += "<tr>" +
                                                "<td class=\"blue" + x + " tdProblema\">Problema" + x + "</td>" +
                                                "<td>" + Grupo + "</td>" +
                                             "</tr>";
                                x++;
                            }
                        }
                        while (x <= 3)
                        {
                            Problemas += "<tr>" +
                                            "<td class=\"blue" + x + " tdProblema\">Problema" + x + "</td>" +
                                            "<td></td>" +
                                         "</tr>";
                            x++;
                        }

                        //Armado del html por invernadero
                        var Line = "<div class=\"ResInvContainer\">" +
                                       "<div>" +
                                           "<table id=\"tblGHPlantacion" + invernadero + "\" status=\"" + idStatus + "\">" +
                                               "<tr>" +
                                                    "<td class=\"GHVisitado\">" + status + "</td>" +
                                               "</tr>" +
                                               "<tr>" +
                                                    "<td class=\"GH\">" + invernadero + "</td>" +
                                               "</tr>" +
                                               "<tr>" +
                                                    "<td class=\"GHPorcentaje " + Colorear(porcentaje, false) + "\" >" + porcentaje + "%</td>" +
                                               "</tr>" +
                                           "</table>" +
                                       "</div>" +
                                       "<div id=\"GHNOPlantacionGrafica" + invernadero + "\" class=\"divChart\">" +
                                           "<table id=\"tblGrafica" + invernadero + "\" style=\"display:none\">" +
                                                LineGrafica +
                                           "</table>" +
                                       "</div>" +
                                       "<div>" +
                                           "<table id=\"tblGHProblemasPlantacion" + invernadero + "\">" +
                                                "<tr><th class=\"GHProblemas\" colspan=\"2\">Problemas</th></tr>" +
                                                Problemas +
                                           "</table>" +
                                       "</div>" +
                                       "<div>" +
                                           "<table id=\"tblGHComentariosPlantacion" + invernadero + "\">" +
                                                "<tr><th class=\"GHProblemas\">Sugerencias</th></tr>" +
                                                "<td class=\"Comentarios\">" + Comentarios + "</td>" +
                                           "</table>" +
                                       "</div>" +
                                   "</div>";
                        sbInvernaderoNOPlantacion.AppendLine(Line);
                    }

                    ////***************************** CALIFICACIÓN TOTAL X INVERNADERO (GH NO PLANTACION) *****************************
                    foreach (DataRow dr in dtNP.Tables[6].Rows)
                    {
                        var calificacion = dr["CalTotalInvernadero"].ToString();
                        CalTotalNOPlantacion = CalTotalNOPlantacion + decimal.Parse(calificacion);
                        cuentaInvernaderos = cuentaInvernaderos + 1;
                    }
                }
            }

            ////***************************** OBTENEMOS LA CALIFICACIÓN DE LA ZONA *****************************
            var suma = CalTotalPlantacion + CalTotalNOPlantacion;
            decimal CalificacionTotal = 0;
            if (suma == 0 || cuentaInvernaderos == 0)
            {
                CalificacionTotal = 0;
            }
            else
            {
                CalificacionTotal = suma / cuentaInvernaderos;
            }
            var tdCalificacionZona = string.Empty;
            tdCalificacionZona = "<td class=\"" + Colorear(CalificacionTotal.ToString(), true) + "\">" + Math.Round(CalificacionTotal,0,MidpointRounding.AwayFromZero) + "%</td>";

            if (Errores.Equals(string.Empty))
            {
                return new string[] { "1", "ok", sbResumenGrupoPlantacion.ToString(), sbCalificacionGrupoPlantacion.ToString(), 
                                      sbProblemasGrupoPlantacion.ToString(), sbInvernaderoPlantacion.ToString(),
                                      sbResumenGrupoNOPlantacion.ToString(), sbCalificacionGrupoNOPlantacion.ToString(),
                                      sbProblemasGrupoNOPlantacion.ToString(), sbInvernaderoNOPlantacion.ToString(), Alertas, tdCalificacionZona.ToString()
                };
            }
            else
            {
                return new string[] { "0", "error", Errores };
            }
        }
        catch(Exception x){
            Log.Error(x);
            return new string[] { "0", "Error en la obtención de la información. Porfavor vuelve a intentalo mas tarde.", "error" };
        }
    }
}