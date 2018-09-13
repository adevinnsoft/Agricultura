using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class pages_frmInvernaderosPorLider : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static Object cargaInvernaderosPlanta(int idPlanta)
    {
        var result = "";

        try
        {
            var parameters = new Dictionary<String, Object>();
            parameters.Add("@EsEnEspanol", true);
            parameters.Add("@IdPlanta",  idPlanta);
            parameters.Add("@NumeroDeError",  0);
            parameters.Add("@MensajeDeError"," ");

            var dt = new DataAccess().executeStoreProcedureDataTable("spr_ObtenerInvernaderos", parameters);

            foreach (DataRow row in dt.Rows) 
            {
                result = result + "<div id='" + row["idInvernadero"] + "' data-event='{\"title\":\"" + row["ClaveInvernadero"] + "\",\"stick\":true}'>" + row["ClaveInvernadero"] + "</div>";
            }
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
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

    [WebMethod]
    public static Object ObtenerContenidoInvernadero(string strInvernadero, int idInvernadero, int intSecciones, int intSurcos)
    {
        var result = "";
        var intSurco = 1;

        try
        {
            for (int intCount = 1; intCount <= intSecciones; intCount++)
            {
                result = result + "<h3 id='Seccion" + intCount.ToString() + "' NoSeccion='" + intCount.ToString() + "'>Sección" + intCount.ToString() + "</h3><table border='1'>";
                result = result + "<tr><th>Surco</th><th>Longitud (m)</th><th>Investigación</th><th>Activo</th><th>Eliminar</th><th>Añadir</th></tr>";
                for (int intCount2 = 1; intCount2 <= intSurcos; intCount2++)
                {
                    result = result + "<tr>";
                    result = result + "<td><span id='Surco" + intSurco.ToString() + " NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "'>" + intSurco.ToString() + "</span></td>";
                    result = result + "<td><input type='text' /></td>";
                    result = result + "<td><input type='checkbox' /></td>";
                    result = result + "<td><input type='checkbox' /></td>";
                    result = result + "<td><img src='../comun/img/remove-icon.png' /></td>";
                    result = result + "<td><img src='../comun/img/add-icon.png' /></td>";
                    result = result + "</tr>";
                    intSurco = intSurco + 1;
                }
                result = result + "</table>";
            }
            result = result + "<br />";
            result = result + "<input type='button' value='Guardar' />";
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }
}