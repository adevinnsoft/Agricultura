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
    public static int idUsuario = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try { 
            idUsuario = int.Parse(Session["idUsuario"].ToString()); 
        }
        catch(Exception x){
            Log.Error(x.Message);
        }
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
    public static Object ObtenerContenidoInvernadero(string strInvernadero, int idInvernadero, int intSecciones, int intSurcos, decimal Longitud, bool Investigacion)
    {
        var result = "";
        var intSurco = 1;

        try
        {
            for (int intCount = 1; intCount <= intSecciones; intCount++)
            {
                result = result + "<h3 id='Seccion" + intCount.ToString() + "' NoSeccion='" + intCount.ToString() + "' idInv='" + idInvernadero.ToString() + "' Invernadero='" + strInvernadero + "'>Sección " + intCount.ToString() + "</h3><table class='Seccion index2' id='Seccion" + intCount.ToString() + "'>";
                result = result + "<tr><th>Surco</th><th>Longitud (m)</th><th>Investigación</th><th>Activo</th><th>Eliminar</th><th>Añadir</th></tr>";
                for (int intCount2 = 1; intCount2 <= intSurcos; intCount2++)
                {
                    result = result + "<tr>";
                    result = result + "<td><span  class='numeroDeSurco' id='Surco" + intSurco.ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount2.ToString() + "'>" + intSurco.ToString() + "</span></td>";
                    result = result + "<td><input  class=\"floatValidate\" type=\"text\" id=\"Surco" + intSurco.ToString() + "\" idInv=\"" + idInvernadero.ToString() + "\" NoSurco=\"" + intSurco.ToString() + "\" NoSeccion=\"" + intCount.ToString() + "\" Invernadero=\"" + strInvernadero + "\" NoFila=\"" + intCount2.ToString() + "\" value=\"" + Longitud.ToString() + "\" /></td>";
                    result = result + "<td><input type='checkbox' id='Surco" + intSurco.ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount2.ToString() + "' "+ ( Investigacion ? "checked" : ""  ) +"/></td>";
                    result = result + "<td><input type='checkbox' id='Surco" + intSurco.ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount2.ToString() + "' checked /></td>";
                    result = result + "<td><span id='Surco" + intSurco.ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount2.ToString() + "' ><img src='../comun/img/remove-icon.png' onclick='EliminarSurco($(this));' /></span></td>";
                    result = result + "<td><span id='Surco" + intSurco.ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + intSurco.ToString() + "' NoSeccion='" + intCount.ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount2.ToString() + "' ><img src='../comun/img/add-icon.png' onclick='AgregarSurco($(this));' /></span></td>";
                    result = result + "</tr>";
                    intSurco = intSurco + 1;
                }
                result = result + "</table>";
            }
            result = result + "<br />";
           // result = result + "<input type='button' value='Guardar' idInv='" + idInvernadero.ToString() + "' Invernadero='" + strInvernadero + "' onclick='AlmacenarSecciones($(this));' />";
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static Object ObtenerConfiguracionActual(string strInvernadero, int idInvernadero)
    {
        var result = "";
        int intCount = 1;

        try
        {
            result = "<h3 id='H3" + strInvernadero + "'>Invernadero: " + strInvernadero + "</h3><div class='DivAcordion' id='DIV" + strInvernadero + "' Invernadero='" + strInvernadero + "' idInv='" + idInvernadero.ToString() + "'>"+
                    "<span class=\"generadorDeSurcosYSecciones\"> Seccion:<input class=\"cajaCh\" id='txtSeccion" + strInvernadero + "' type='text' /> Surcos:<input class=\"cajaCh\"  id='txtSurco" + strInvernadero + "' type='text' />Longitud Promedio:<input class=\"cajaCh\"  id='txtLongitud" + strInvernadero + "'   type='text' /> " +
                    "<label>Investigación</label><input id=\"chkSeccion" + strInvernadero + "\" type=\"checkbox\" />" +
                    "<input type='button' IdInv='" + idInvernadero.ToString() + "' Inv='" + strInvernadero + "' value='¡Generar!' onclick='GenerarGridInvernadero($(this));' /> </span>";
            DataAccess da = new DataAccess();
            var objDataSet = da.executeStoreProcedureDataSet("spr_ObtenerConfiguracionSeccionesySurcos", new Dictionary<string, object>() { { "@idInvernadero", idInvernadero } });

            // Obtengo las tablas para armar la configuración del invernadero
            var tblSecciones = objDataSet.Tables[0];
            var tblSurcos = objDataSet.Tables[1];

            if (tblSecciones.Rows.Count > 0 && tblSurcos.Rows.Count > 0)
            {
                foreach (DataRow DRow in tblSecciones.Rows)
                {
                    result = result + "<h3 id='Seccion" + DRow[0].ToString() + "' NoSeccion='" + DRow[0].ToString() + "' idInv='" + idInvernadero.ToString() + "' Invernadero='" + strInvernadero + "'>" + DRow[1].ToString() + "</h3><table class='Seccion index2' id='Seccion" + DRow[0].ToString() + "'>";
                    result = result + "<tr><th>Surco</th><th>Longitud (m)</th><th>Investigación</th><th>Activo</th><th>Eliminar</th><th>Añadir</th></tr>";
                    intCount = 1;
                    foreach (DataRow DRow2 in tblSurcos.Rows)
                    {
                        if (DRow2[0].ToString() == DRow[0].ToString() && DRow2[5].ToString() == DRow[2].ToString())
                        {
                            result = result + "<tr>";
                            result = result + "<td><span  class='numeroDeSurco' id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "'>" + DRow2[1].ToString() + "</span></td>";
                            result = result + "<td><input  class=\"floatValidate\" type='text' id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' value='" + DRow2[2].ToString() + "' /></td>";
                            if (Convert.ToBoolean(DRow2[3]) == true)
                            {
                                result = result + "<td><input type='checkbox' id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' checked /></td>";
                            }
                            else
                            {
                                result = result + "<td><input type='checkbox' id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' /></td>";
                            }
                            if (Convert.ToBoolean(DRow2[4]) == true)
                            {
                                result = result + "<td><input type='checkbox' id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' checked /></td>";
                            }
                            else
                            {
                                result = result + "<td><input type='checkbox' id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' /></td>";
                            }
                            result = result + "<td><span id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' ><img src='../comun/img/remove-icon.png' onclick='EliminarSurco($(this));' /></span></td>";
                            result = result + "<td><span id='Surco" + DRow2[1].ToString() + "' idInv='" + idInvernadero.ToString() + "' NoSurco='" + DRow2[1].ToString() + "' NoSeccion='" + DRow2[0].ToString() + "' Invernadero='" + strInvernadero + "' NoFila='" + intCount.ToString() + "' ><img src='../comun/img/add-icon.png' onclick='AgregarSurco($(this));' /></span></td>";
                            result = result + "</tr>";
                            intCount = intCount + 1;
                        }
                    }
                    result = result + "</table>";
                }
                result = result + "<br />";
                // result = result + "<input type='button' value='Guardar' idInv='" + idInvernadero.ToString() + "' Invernadero='" + strInvernadero + "' onclick='AlmacenarSecciones($(this));' />";
                //result = result + "<br />";
            }
            result = result + "</div>";

        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static Object GenerarRespaldoDeSecciones(string strInvernadero, int idInvernadero)
    { 
        var result = "";

        try
        {
            DataAccess da = new DataAccess();
            da.executeStoreProcedureNonQuery("spr_GenerarRespaldoDeSecciones",
            new Dictionary<string, object>() { { "@idInvernadero", idInvernadero },
                                               { "@idUsuario", Convert.ToInt32(HttpContext.Current.Session["idUsuario"].ToString()) } });
            result = idInvernadero.ToString();
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static Object AlmacenarSecciones(int intIdInvernadero, string strInvernadero, int intNumeroSeccion, string strNombreSeccion, int intNumeroSurco, decimal decLongitud, bool blnEsRyD, bool blnActivo)
    {
        var result = "";

        try
        {
            DataAccess da = new DataAccess();
            da.executeStoreProcedureNonQuery("spr_AlmacenarSeccionySurcos",
            new Dictionary<string, object>() { { "@idInvernadero", intIdInvernadero }, 
                                               { "@NumeroSeccion", intNumeroSeccion }, 
                                               { "@NombreSeccion", strNombreSeccion }, 
                                               { "@NumeroSurco", intNumeroSurco }, 
                                               { "@Longitud", decLongitud }, 
                                               { "@EsRD", blnEsRyD }, 
                                               { "@Activo", blnActivo }, 
                                               { "@idUsuario", Convert.ToInt32(HttpContext.Current.Session["idUsuario"].ToString()) } });
        }
        catch (Exception ex)
        {
            return "<script>popUpAlert('Error');</script>";
        }
        return result;
    }

    [WebMethod]
    public static string[] AlmacenarInvernaderos(ConfiguracionInvernadero[] Invernaderos) {
        try
        {
            DataAccess da = new DataAccess();
            DataTable dtInvernadero = new DataTable();
            dtInvernadero.Columns.Add("idInvernadero");
            dtInvernadero.Columns.Add("clave");
            dtInvernadero.Columns.Add("Indice");
            DataTable dtSeccion = new DataTable();
            dtSeccion.Columns.Add("NumeroSeccion");
            dtSeccion.Columns.Add("NombreSeccion");
            dtSeccion.Columns.Add("Indice");
            dtSeccion.Columns.Add("Padre");
            DataTable dtSurco = new DataTable();
            dtSurco.Columns.Add("ClaveSurco");
            dtSurco.Columns.Add("Longitud");
            dtSurco.Columns.Add("EsRD");
            dtSurco.Columns.Add("Padre");

            int countInvernadero = 0;
            int countSecciones = 0;
            DataRow dr = null;
            foreach (ConfiguracionInvernadero I in Invernaderos)
            {
                dr = dtInvernadero.NewRow();
                dr["idInvernadero"] = I.idInvernadero;
                dr["clave"] = I.clave;
                dr["Indice"] = ++countInvernadero;
                dtInvernadero.Rows.Add(dr);
                foreach (Seccion S in I.secciones)
                {
                    dr = dtSeccion.NewRow();
                    dr["NumeroSeccion"] = S.NumeroSeccion;
                    dr["NombreSeccion"] =  S.NombreSeccion;
                    dr["Indice"] = ++countSecciones;
                    dr["Padre"] = countInvernadero;
                    dtSeccion.Rows.Add(dr); 
                    foreach (Surco C in S.surcos)
                    {
                        dr = dtSurco.NewRow();
                        dr["ClaveSurco"] = C.ClaveSurco;
                        dr["Longitud"] =  C.Longitud;
                        dr["EsRD"] = C.EsRD ;
                        dr["Padre"] = countSecciones;
                        dtSurco.Rows.Add(dr);
                    }
                }
            }

            DataTable dt = da.executeStoreProcedureDataTable("spr_SeccionesYSurcos_Almacenar", new Dictionary<string, object>() { 
                {"@idUsuario", idUsuario},
                {"@invernadero", dtInvernadero},
                {"@seccion", dtSeccion},
                {"@surco", dtSurco}
            });

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return new string[] { "ok", recursoLocal("GuardadoCorrecto")};
                    
                }
                else
                {
                    switch (dt.Rows[0]["ErrorNumber"].ToString())
                    {
                        default: return new string[] { "error", recursoLocal("ErrorNoIdentificado") };
                    }
                }
            }
            else
            {
                return new string[] { "error", recursoLocal("ErrorDeComunicacionConBD") };
            }
        }
        catch (Exception x)
        {
            Log.Error(x);
            return new string[] { "error", "Error en el procesamiento de datos." };
        }    
    }

    public static string recursoLocal(string recurso){
        return HttpContext.GetLocalResourceObject("~/pages/frmConfiguracionDeSeccionesySurcos.aspx", recurso).ToString();
    }

}