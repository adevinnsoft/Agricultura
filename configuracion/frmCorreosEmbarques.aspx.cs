using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
public partial class frmCorreosEmbarques : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmCorreosEmbarques));
    public static int idCapturaDefault = 10;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] buscarEnActiveDirectory(string SAMaccount)
    {
        StringBuilder sb = new StringBuilder();
        string correoElectronico = DGActiveDirectory.obtenerCorreoElectronicoDeLaCuenta(SAMaccount);
        string nombreCuenta = DGActiveDirectory.obtenerInformacionDeLaCuenta(SAMaccount, DGActiveDirectory.PROPIEDAD.NombreAMostrar);

        try
        {
            if (correoElectronico == string.Empty && nombreCuenta == string.Empty)
            {
                return new string[] { "0", "Este usuario no tiene correo en Active Directory", "warning" };
            }
            else
            {
                sb.AppendLine("<span><label class='bold'>Nombre:</label><label>" + nombreCuenta + "</label></span>");
                sb.AppendLine("<span><label class='bold'>Cuenta:</label><labe class='lblCuentaMostrado'>" + SAMaccount + "</label></span>");
                sb.AppendLine("<img src='../comun/img/add-icon.png' id='btnAgregarEmbarques' class='botonAgregar' onclick='agregarCorreos();'>");

                return new string[] { "1", "ok", sb.ToString() };
            }
        }
        catch (Exception ex)
        {
            log.Error("buscarEnActiveDirectory: " + ex.Message);
            return new string[] { "0", "Error de sistema", "error" };
        }
    }

    [WebMethod]
    public static string[] agregarCorreos(string SAMaccount)
    {
        StringBuilder sb = new StringBuilder();
        string correoElectronico = DGActiveDirectory.obtenerCorreoElectronicoDeLaCuenta(SAMaccount);

        try
        {
            if (SAMaccount == string.Empty)
            {
                return new string[] { "0", "Ingrese un correo porfavor", "warning" };
            }
            else
            {
                //se procede a verificar la existencia del correo electronico
            }

            if (correoElectronico == string.Empty)
            {
                return new string[] { "0", "No se pudo procesar la cuenta", "warning" };
            }
            else
            {
                sb.AppendLine("<tr class='Nuevo' miAtributo='LosTRsNuevos'>");
                sb.AppendLine("<td class='invisible' idCaptura='" + idCapturaDefault + "'>" + idCapturaDefault + "</td>");
                sb.AppendLine("<td class='invisible' otherClass='SAMaccount'>" + SAMaccount + "</td>");
                sb.AppendLine("<td class='Correo' otherClass='Correo'>" + correoElectronico + "</td>");
                sb.AppendLine("<td><img src='../comun/img/remove-icon.png' class='btnEliminar' id='" + correoElectronico + "' onclick='EliminarCorreo(this);' /></td>");
                sb.AppendLine("</tr>");

                return new string[] { "1", "ok", sb.ToString() };
            }
        }
        catch (Exception ex)
        {
            log.Error("agregarCorreos: " + ex.Message);
            return new string[] { "0", "Error de sistema", "error" };
        }
    }


    [WebMethod]
    public static string[] GuardarListaDeDistribucion(ListaDeDistribucion[] listaDeDistribucion)
    {
        if (listaDeDistribucion[0].correos.Length == 0)
        {
            return new string[] { "0", "No se detectaron cambios.", "info" };
        }
        else
        {
            int indice = 0;
            int indice2 = 0;
            try
            {
                DataAccess da = new DataAccess();
                Dictionary<string, object> prm = new Dictionary<string, object>();
                DataTable dtlistaDeDistribucion = new DataTable();
                DataTable dtlistaDeDistribucionCorreo = new DataTable();

                dtlistaDeDistribucion.Columns.Add("etiqueta");
                dtlistaDeDistribucion.Columns.Add("indice");
                dtlistaDeDistribucion.Columns.Add("padre");

                dtlistaDeDistribucionCorreo.Columns.Add("Correo");
                dtlistaDeDistribucionCorreo.Columns.Add("Cuenta");
                dtlistaDeDistribucionCorreo.Columns.Add("Estado");
                dtlistaDeDistribucionCorreo.Columns.Add("indice");
                dtlistaDeDistribucionCorreo.Columns.Add("padre");

                foreach (ListaDeDistribucion LD in listaDeDistribucion)
                {
                    DataRow dr = dtlistaDeDistribucion.NewRow();
                    dr["etiqueta"] = LD.etiqueta;
                    dr["indice"] = ++indice;
                    dr["padre"] = null;
                    dtlistaDeDistribucion.Rows.Add(dr);

                    foreach (ListaDeDistribucionCorreo LDC in LD.correos)
                    {
                        DataRow dr2 = dtlistaDeDistribucionCorreo.NewRow();
                        dr2["correo"] = LDC.correo;
                        dr2["cuenta"] = LDC.cuenta;
                        dr2["estado"] = LDC.estado;
                        dr2["indice"] = ++indice2;
                        dr2["padre"] = indice;
                        dtlistaDeDistribucionCorreo.Rows.Add(dr2);
                    }
                }

                string IdUsuarioCaptura = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
                prm.Add("@IdUsuarioCaptura", IdUsuarioCaptura);
                prm.Add("@idCaptura", listaDeDistribucion[0].idCaptura);
                prm.Add("@correo", "");
                prm.Add("@listaDeDistribucion", dtlistaDeDistribucion);
                prm.Add("@listaDeDistribucionCorreo", dtlistaDeDistribucionCorreo);
                DataTable dt = da.executeStoreProcedureDataTable("spr_AlmacenarListaDeDistribucion", prm);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Estado"].ToString().Equals("1"))
                    {
                        return new string[] { "1", "Se guardó la información correctamente", "ok" };
                    }
                    else
                    {
                        return new string[] { "0", "No se guardo la informacion correctamente", "error" };
                    }
                }
                else
                {
                    return new string[] { "0", "", "error" };
                }
            }
            catch (Exception ex)
            {
                log.Error("GuardarListaDeDistribucion: " + ex.Message);
                return new string[] { "0", "No se pudieron guardar los datos", "error" };
            }
        }
    }

    [WebMethod]
    public static string[] obtenerListaDeDistribucion()
    {
        try
        {
            DataAccess da = new DataAccess();
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idCaptura", idCapturaDefault);
            DataTable dt = da.executeStoreProcedureDataTable("spr_ObtenerListaDeDistribucion", prm);

            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th rowspan='1' class='invisible'>IdLista</th>");
            sb.AppendLine("<th rowspan='1' class='invisible'>SAMaccount</th>");
            sb.AppendLine("<th colspan='1'>Correo</th>");
            sb.AppendLine("<th colspan='1'>Acción</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            foreach (DataRow dr in dt.Rows)
            {
                string idLista = dr["idCapturaDefault"].ToString(),
                       SAMaccount = dr["SAMaccount"].ToString(),
                       Correo = dr["Correo"].ToString();

                sb.AppendLine("<tr class='Cargado'  miAtributo='LosTRsCargados'>");
                sb.AppendLine("<td class='invisible' otherClass='idLista'>" + idLista + "</td>");
                sb.AppendLine("<td class='invisible' otherClass='SAMaccount'>" + SAMaccount + "</td>");
                sb.AppendLine("<td class='Correo' otherClass='Correo'>" + Correo + "</td>");
                sb.AppendLine("<td><img src='../comun/img/remove-icon.png' class='btnEliminar' id='" + Correo + "' onclick='EliminarCorreo(this);'></td>");
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            return new string[] { "1", "ok", sb.ToString() };
        }
        catch (Exception ex)
        {
            log.Error("obtenerListaDeDistribucion: " + ex.Message);
            return new string[] { "0", "No existen correos", "warning" };
        }
    }
}