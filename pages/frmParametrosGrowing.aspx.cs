using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using log4net;
using System.Text;

public partial class pages_frmParametrosGrowing : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(pages_frmParametrosGrowing));
    private static bool spanish;
    protected void Page_Load(object sender, EventArgs e)
    {
        spanish = Session["Locale"].ToString() == "es-MX" ? true : false;
    }

    [WebMethod]
    public static string[] ObtenerGrupos(){
        try
        {
            DataAccess da = new DataAccess();
            DataTable dt = da.executeStoreProcedureDataTable("spr_GrupoGrowingOtener", new Dictionary<string, object>());
            if (dt.Rows.Count > 0) {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow R in dt.Rows)
                {
                    string id = R["idGrupoGrowing"].ToString();
                    string grupo = spanish ? R["NombreEs"].ToString() : R["NombreEn"].ToString();

                    sb.AppendLine("<span class=\"grupoGrowing\" idGrupo=\"" + id + "\" onclick=\"obtenerParametrosDelGrupo(this);\">  ");
                    sb.AppendLine("     <label>"+grupo+"</label>                                                                ");
                    sb.AppendLine("</span>                                                                                       ");
                }
                return new string[] { "ok", sb.ToString() };
            }
            else {
                return new string[] { "error", "Error al obener información de la base de datos." };
            }

        }
        catch (Exception x)
        {
            log.Error(x);
            return new string[] { "error", "Error en el proceso de obtención de datos." };
        }
    }
    [WebMethod]
    public static Object ObtenerParametrosDeGrupo(int idGrupo, string nombreGrupo)
    {
        var result = "";
        try
        {
            DataAccess da = new DataAccess();
            var objDataSet = da.executeStoreProcedureDataSet("spr_ObtenerParametrosPorGrupoGrowing", new Dictionary<string, object>() { { "@idGrupoGrowing", idGrupo }, { "@EsEnEspanol", spanish }, { "@NumeroDeError", 0 }, { "@MensajeDeError", "" } });
            var tblParametros = objDataSet.Tables[0];


            result = "<span class='grupo' idGrupo='" + idGrupo + "'><h3>" + nombreGrupo + "</h3>";

            if (tblParametros.Rows.Count == 0)
            {
                result = result + "<table class='index' style='background:#F0F5E5;'>";

                result = result + "<tr idParametro = '0'><td>Nombre</td><td><input value=''/></td> <td>Nombre (Inglés)</td><td><input value=''/></td> <td>Puntaje Plantación</td><td><input value='' class='cajaCh' /></td> <td>Puntaje No Plantación</td><td><input value='' class='cajaCh' /></td> <td>Activo</td><td><input type='checkbox' checked /></td> <td><img src='../comun/img/remove-icon.png' onclick='EliminaParametro($(this));'/>&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametro($(this));'/></td></tr> ";
                result = result + "<tr> <td> NA, OK y X</td> <td id='NAOKX' colspan='5'> ";
                result = result + "<table class='index2' style='background:#F0F5E5;'>";
                result = result + "<thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>";
                result = result + "<tr idParametroNAOKX = '0'><td><input value=''/></td><td><input value=''/></td><td><input type='checkbox' checked/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroNAOKX($(this));' />&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroNAOKX($(this));'  /></td></tr>";
                result = result + " </table></td>";
                result = result + "<td>S, A, G y NA</td><td colspan='5'> ";
                result = result + "<table class='index2' style='background:#F0F5E5;'>";
                result = result + "<thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>";
                result = result + "<tr idParametroSAGNA = '0'><td><input value=''/></td><td><input value=''/></td><td><input type='checkbox' checked/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroSAGN($(this));' />&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroSAGN($(this));'  /></td></tr>";
                result = result + " </table></td> </tr></table> ";
            }
            else
            {
                foreach (DataRow DRow in tblParametros.Rows)
                {
                    result = result + "<table class='index' style='background:#F0F5E5;'>";
                    if (Convert.ToBoolean(DRow["Activo"]) == true)
                    {
                        result = result + "<tr idParametro = '" + DRow[0].ToString() + "' ><td>Nombre</td><td><input value='" + DRow[8].ToString() + "'/></td> <td>Nombre (Inglés)</td><td><input value='" + DRow[9].ToString() + "'/></td> <td>Puntaje Plantación</td><td><input value='" + DRow["PuntajeAsignado"].ToString() + "' class='cajaCh' /></td> <td>Puntaje No Plantación</td><td><input value='" + DRow["PuntajeAsignadoNP"].ToString() + "' class='cajaCh' /></td> <td>Activo</td><td><input type='checkbox' checked /></td> <td><img src='../comun/img/remove-icon.png' onclick='EliminaParametro($(this));'/>&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametro($(this));'></td></tr> ";
                    }
                    else
                    {
                        result = result + "<tr idParametro = '" + DRow[0].ToString() + "' ><td>Nombre</td><td><input value='" + DRow[8].ToString() + "'/></td> <td>Nombre (Inglés)</td><td><input value='" + DRow[9].ToString() + "'/></td> <td>Puntaje Plantación</td><td><input value='" + DRow["PuntajeAsignado"].ToString() + "' class='cajaCh' /></td> <td>Puntaje No Plantación</td><td><input value='" + DRow["PuntajeAsignadoNP"].ToString() + "' class='cajaCh' /></td> <td>Activo</td><td><input type='checkbox' /></td> <td><img src='../comun/img/remove-icon.png' onclick='EliminaParametro($(this));'/>&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametro($(this));'/></td></tr> ";
                    }

                    result = result + "<tr> <td> NA, OK y X</td> <td id='NAOKX' colspan='5'> ";
                    result = result + "<table class='index2' style='background:#F0F5E5;'>";
                    result = result + "<thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>";
                    DataAccess da2 = new DataAccess();
                    var objDataSet2 = da.executeStoreProcedureDataSet("spr_ObtenerCatalogoListaNA_OK_X_PorParametro", new Dictionary<string, object>() { { "@idParametroPorGrupoGrowing", DRow[0].ToString() }, { "@EsEnEspanol", true }, { "@NumeroDeError", 0 }, { "@MensajeDeError", "" } });
                    var tblListaNAOKX = objDataSet2.Tables[0];

                    if (tblListaNAOKX.Rows.Count == 0)
                    {
                        result = result + "<tr idParametroNAOKX = '0'><td><input value=''/></td><td><input value=''/></td><td><input type='checkbox' checked/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroNAOKX($(this));' />&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroNAOKX($(this));' /></td></tr>";
                    }
                    else
                    {
                        foreach (DataRow DRow2 in tblListaNAOKX.Rows)
                        {
                            if (Convert.ToBoolean(DRow2["Activo"]) == true)
                            {
                                result = result + "<tr idParametroNAOKX = '" + DRow2[0].ToString() + "'><td><input value='" + DRow2[2].ToString() + "'/></td><td><input value='" + DRow2[3].ToString() + "'/></td><td><input type='checkbox' checked/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroNAOKX($(this));'/>&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroNAOKX($(this));' /></td></tr>";
                            }
                            else
                            {
                                result = result + "<tr idParametroNAOKX = '" + DRow2[0].ToString() + "'><td><input value='" + DRow2[2].ToString() + "'/></td><td><input value='" + DRow2[3].ToString() + "'/></td><td><input type='checkbox'/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroNAOKX($(this));'/>&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroNAOKX($(this));' /></td></tr>";
                            }

                        }
                    }
                    result = result + " </table></td>";
                    result = result + "<td>S, A, G y NA</td><td colspan='5'> ";
                    result = result + "<table class='index2' style='background:#F0F5E5;'>";
                    result = result + "<thead><tr><th>Nombre</th><th>Nombre (Inglés)</th><th>Activo</th><th>&nbsp;</th></tr></thead>";

                    DataAccess da3 = new DataAccess();
                    var objDataSet3 = da.executeStoreProcedureDataSet("spr_ObtenerCatalogoListaS_A_G_N_PorParametro", new Dictionary<string, object>() { { "@idParametroPorGrupoGrowing", DRow[0].ToString() }, { "@EsEnEspanol", true }, { "@NumeroDeError", 0 }, { "@MensajeDeError", "" } });
                    var tblListaSAGN = objDataSet3.Tables[0];
                    if (tblListaSAGN.Rows.Count == 0)
                    {
                        result = result + "<tr idParametroSAGNA = '0'><td><input value=''/></td><td><input value=''/></td><td><input type='checkbox' checked/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroSAGN($(this));' />&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroSAGN($(this));'  /></td></tr>";
                    }
                    else
                    {
                        foreach (DataRow DRow3 in tblListaSAGN.Rows)
                        {
                            if (Convert.ToBoolean(DRow3["Activo"]) == true)
                            {
                                result = result + "<tr idParametroSAGNA='" + DRow3[0].ToString() + "'><td><input value='" + DRow3[2].ToString() + "'/></td><td><input value='" + DRow3[3].ToString() + "'/></td><td><input type='checkbox' checked/></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroSAGN($(this));' />&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroSAGN($(this));' /></td></tr>";
                            }
                            else
                            {
                                result = result + "<tr idParametroSAGNA='" + DRow3[0].ToString() + "'><td><input value='" + DRow3[2].ToString() + "'/></td><td><input value='" + DRow3[3].ToString() + "'/></td><td><input type='checkbox' /></td><td><img src='../comun/img/remove-icon.png' onclick='EliminaParametroSAGN($(this));' />&nbsp;&nbsp;<img src='../comun/img/add-icon.png' onclick='AgregaParametroSAGN($(this));' /></td></tr>";
                            }

                        }
                    }
                    result = result + " </table></td> </tr></table> ";

                }
            }



            result = result + "</span>  ";
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

}