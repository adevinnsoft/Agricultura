using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class Default2 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static Object CargaPlantas()
    {
        var result = "";
        int intPlanta = 0;
        int intPlantaAux = 0;
        int intUsuario = 0;

        try
        {
            DataAccess da = new DataAccess();
            var objDataSet = da.executeStoreProcedureDataSet("spr_ObtenerLideresPorInvernadero", new Dictionary<string, object>() { });

            var tblPlantas = objDataSet.Tables[0];
            var tblLideres = objDataSet.Tables[1];

            foreach (DataRow row in tblPlantas.Rows)
            {
                intPlanta = Convert.ToInt32(row["idPlanta"].ToString());
                result = result + "<h3>" + row["NombrePlanta"] + "</h3><div id='" + row["idPlanta"] + "' idPlanta=" + row["idPlanta"] + " class='DivAcordion' >";
                intUsuario = 0;

                DataRow[] temlider = tblLideres.Select("idPlanta=" + intPlanta);
                foreach (DataRow row2 in temlider)
                {

                    if (intPlanta == Convert.ToInt32(row2["idPlanta"].ToString()))
                    {
                        if (intUsuario != Convert.ToInt32(row2["idUsuario"].ToString()))
                        {
                            intPlantaAux = intPlanta;
                            result = result + "<br /><h3><span class='SpanLider' idPlanta='" + row2["idPlanta"].ToString() + "' idUsuario='" + row2["idUsuario"].ToString() + "'>" +
                                              row2["Lider"] + " <img src='../comun/img/add-icon.png' onclick='AgregarInvernadero($(this).parent());' data-type='zoomin' /></span></h3>";
                        }
                        else
                        {
                            if (intPlantaAux != intPlanta || row2["idInvernadero"].ToString() == "0")
                            {
                                intPlantaAux = intPlanta;
                                result = result + "<br /><h3><span class='SpanLider' idPlanta='" + row2["idPlanta"].ToString() + "' idUsuario='" + row2["idUsuario"].ToString() + "'>" +
                                                  row2["Lider"] + " <img src='../comun/img/add-icon.png' onclick='AgregarInvernadero($(this).parent());' data-type='zoomin' /></span></h3>";
                            }
                        }
                        if (row2["Invernadero"].ToString() != "")
                        {
                            result = result + "<span class='Active' idInv='" + row2["idInvernadero"].ToString() + "' idPlanta='" + row2["idPlanta"].ToString() + "' idUsuario='" + row2["idUsuario"].ToString() + "'>"
                                            + "<input type='text' value='" + row2["Invernadero"].ToString() + "'  readonly /> "
                                            + "<img src='../comun/img/remove-icon.png' onclick='EliminarInvernadero($(this));' /></span>";
                        }
                    }
                    intUsuario = Convert.ToInt32(row2["idUsuario"].ToString());
                }
                result = result + "</div>";
            }
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static Object CargarInvernaderos(int idUsuario, int idPlanta)
    {
        var result = "";
        string strInvernadero = "";
        int intExisteInvernadero = 0;
        string strLider = "";

        try
        {
            DataAccess da = new DataAccess();
            var objDataSet = da.executeStoreProcedureDataSet("spr_ObtenerAccesosDeLiderPorInvernadero", new Dictionary<string, object>() { { "@idUsuario", idUsuario }, { "@idPlanta", idPlanta } });

            // Obtengo las tablas para armar el popup
            var tblColumnas = objDataSet.Tables[0];
            var tblFilas = objDataSet.Tables[1];
            var tblAccesos = objDataSet.Tables[2];

            // Defino la tabla:
            result = result + "<table border='0' class=\"index2\"><tr><th> </th>";
            // Defino las columnas:
            foreach (DataRow RColumnas in tblColumnas.Rows)
            {
                result = result + "<th>" + RColumnas[0].ToString() + "</th>";
                strLider = RColumnas[1].ToString();
            }
            // Cierro el primer renglon:
            result = result + "</tr>";
            // Defino las filas y accesos:
            foreach (DataRow RFilas in tblFilas.Rows)
            {
                result = result + "<tr><td> <input type='checkbox' onclick='$(\"input[type=checkbox][zona=zona_" + RFilas[0].ToString() + "]\").attr(\"checked\",$(this).attr(\"checked\")?true:false);'></span>" + RFilas[0].ToString() + "</td>";
                foreach (DataRow RColumnas in tblColumnas.Rows)
                {
                    intExisteInvernadero = 0;
                    strInvernadero = idPlanta.ToString() + RFilas[0].ToString() + RColumnas[0].ToString();
                    foreach (DataRow RAccesos in tblAccesos.Rows)
                    {
                        if (strInvernadero == RAccesos[1].ToString() && RAccesos[2].ToString() == "1")
                        {
                            result = result + "<td><input type='checkbox' zona='zona_" + RFilas[0].ToString() + "' name='" + strInvernadero + "' idPlanta='" + idPlanta.ToString() + "' idUsuario='" + idUsuario.ToString() + "' ClaveInvernadero='" + strInvernadero + "' idInvernadero='" + RAccesos[0].ToString() + "' checked /></td>";
                            intExisteInvernadero = 1;
                        }
                        else
                        {
                            if (strInvernadero == RAccesos[1].ToString())
                            {
                                result = result + "<td><input type='checkbox' zona='zona_" + RFilas[0].ToString() + "' name='" + strInvernadero + "' idPlanta='" + idPlanta.ToString() + "' idUsuario='" + idUsuario.ToString() + "' ClaveInvernadero='" + strInvernadero + "' idInvernadero='" + RAccesos[0].ToString() + "' /></td>";
                                intExisteInvernadero = 1;
                            }
                        }
                    }
                    if (intExisteInvernadero == 0)
                    {
                        result = result + "<td> </td>";
                    }
                }
                result = result + "</tr>";
            }
            // Cierro la tabla:
            result = result + "</table>";
            // Genero la botonera:
            result = result + "<div id='BotoneraPopup'>" +
                                "<input type='button' idPlanta='" + idPlanta.ToString() + "' idUsuario='" + idUsuario.ToString() + "' Lider='" + strLider + "' onclick='GuardarTemporalmente($(this));' value='Guardar' /> " +
                                "<input type='button' idPlanta='" + idPlanta.ToString() + "' idUsuario='" + idUsuario.ToString() + "' Lider='" + strLider + "' onclick='CerrarPopup();' value='Cerrar' /> " +
                              "</div>";
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static Object EliminarAsignacion(int idUsuario, int idInvernadero)
    {
        var result = "";

        try
        {
            DataAccess da = new DataAccess();
            da.executeStoreProcedureNonQuery("spr_EliminarLiderPorInvernadero", new Dictionary<string, object>() { { "@IdInvernadero", idInvernadero }, { "@IdUsuario", idUsuario }, { "@IdUsuarioModifico", Convert.ToInt32(HttpContext.Current.Session["idUsuario"].ToString()) } });
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static Object AlmacenarAsignacion(int idUsuario, int idInvernadero)
    {
        var result = "";

        try
        {
            DataAccess da = new DataAccess();
            da.executeStoreProcedureNonQuery("spr_AlmacenarLiderPorInvernadero", new Dictionary<string, object>() { { "@IdInvernadero", idInvernadero }, { "@IdUsuario", idUsuario }, { "@IdUsuarioModifico", Convert.ToInt32(HttpContext.Current.Session["idUsuario"].ToString()) } });
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }
}