using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

public partial class configuracion_frmDensidadDePlantula : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmDensidadDePlantula));
    private static int idUsuario = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    [WebMethod]
    public static string[] obtenerSurcosPorInvernadero()
    {
        int cont = 1;
        try
        {
            string idLider = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
            DataAccess da = new DataAccess();
           
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idLider", idLider);
            prm.Add("@idplanta", HttpContext.Current.Session["idPlanta"].ToString());
            DataTable dt = da.executeStoreProcedureDataTable("spr_ObtenerSurcosPorInvernadero", prm);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow R in dt.Rows)
            {
                string idInvernadero = R["IdInvernadero"].ToString(),
                         Invernadero = R["Invernadero"].ToString(),
                         Surco = R["Surco"].ToString(),
                         Variedad = R["Variedad"].ToString(),
                         Densidad = R["Densidad"].ToString();

                sb.AppendLine("<tr idInvernadero=\"" + idInvernadero + "\" class=\"densidadPlantula\" style  miAtributo=\"LosTRs\">");
   
                sb.AppendLine(Invernadero.Equals(string.Empty) ? "<td class=\"Invernadero\">-</td>" : "<td class=\"Invernadero\">" + Invernadero + "</td>");
                sb.AppendLine(Surco.Equals(string.Empty) ? "<td class=\"Surco\">-</td>" : "<td class=\"Surco\">" + Surco + "</td>");
                sb.AppendLine(Variedad.Equals(string.Empty) ? "<td class=\"Variedad\">0</td>" : "<td class=\"Variedad\">" + Variedad + "</td>");
                sb.AppendLine(Densidad.Equals(string.Empty) ? "<td><input type=\"text\"  id=\"densidadCapturada" + cont + "\" value=\"\"  class=\"Densidad\"/></td>" : "<td><input type=\"text\"  id=\"densidadCapturada" + cont + "\" value=\"" + Densidad + "\"  class=\"Densidad\"/></td>");
                sb.AppendLine("<input type=\"hidden\" id=\"idInvernadero" + cont + "\" value=" + idInvernadero + " class=\"idInvernaderos\">");
               
                sb.AppendLine(Surco.Equals(string.Empty) ? "<input type=\"hidden\" id=\"Surco" + cont + "\"  value=\" \" class=\"NumeroDeSurcos\">" :"<input type=\"hidden\" id=\"Surco" + cont + "\" value=" + Surco + " value=\" \" class=\"NumeroDeSurcos\">");
                sb.AppendLine("</tr>");
                cont++;
            }

            return new string[] { "1", "ok", sb.ToString() };
        }
        catch (Exception x)
        {
            return new string[] { "0", "Error: No se encontraron invernaderos", string.Empty };
            throw;
        }
    }


    [WebMethod]
    public static string[] AlmacenarDensidadDePlantulaPorSurco(DensidadDePlantulaPorSurco[] DensidadFinal)
    {
        int indice = 0;
        int indice2 = 0;

        try
        {
            DataAccess da = new DataAccess();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            DataTable dtDensidadDePlantulaPorSurco = new DataTable();
            DataTable dtDensidadDePlantulaPorSurcoDetalle = new DataTable();

            dtDensidadDePlantulaPorSurco.Columns.Add("IdInvernadero");
            dtDensidadDePlantulaPorSurco.Columns.Add("indice");
            dtDensidadDePlantulaPorSurco.Columns.Add("padre");
            dtDensidadDePlantulaPorSurcoDetalle.Columns.Add("NumeroDeSurco");
            dtDensidadDePlantulaPorSurcoDetalle.Columns.Add("Densidad");
            dtDensidadDePlantulaPorSurcoDetalle.Columns.Add("indice");
            dtDensidadDePlantulaPorSurcoDetalle.Columns.Add("padre");

            foreach (DensidadDePlantulaPorSurco DPS in DensidadFinal)
            {
                if(DPS.IdInvernadero != 0){
                    DataRow dr1 = dtDensidadDePlantulaPorSurco.NewRow();
                    dr1["IdInvernadero"] = DPS.IdInvernadero;
                    dr1["indice"] = ++indice;
                    dr1["padre"] = null;
                    dtDensidadDePlantulaPorSurco.Rows.Add(dr1);

                    foreach (DensidadDePlantulaPorSurcoDetalle DPSD in DPS.densidadSurcoDetalle)
                    {
                        DataRow dr2 = dtDensidadDePlantulaPorSurcoDetalle.NewRow();
                        dr2["NumeroDeSurco"] = DPSD.NumeroDeSurco;
                        dr2["Densidad"] = DPSD.Densidad;
                        dr2["indice"] = ++indice2;
                        dr2["padre"] = indice;
                        dtDensidadDePlantulaPorSurcoDetalle.Rows.Add(dr2);
                    }
                }
            }

            if (dtDensidadDePlantulaPorSurco.Rows.Count == 0)
            {
                return new string[] {"1","No se detectaron cambios.","info" };
            }

            string IdUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();
            prm.Add("IdUsuarioCaptura", IdUsuario);
            prm.Add("@densidadPlantulaPorSurco", dtDensidadDePlantulaPorSurco);
            prm.Add("@densidadPlantulaPorSurcoDetalle", dtDensidadDePlantulaPorSurcoDetalle);
            DataTable dtResult = da.executeStoreProcedureDataTable("spr_AlmacenarDensidadDePlantulas", prm);

            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["Estado"].ToString().Equals("1"))
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

            log.Error(ex);
            return new string[] { "0", "No se pudieron guardar los datos", "error" };
        }
       
    }

}