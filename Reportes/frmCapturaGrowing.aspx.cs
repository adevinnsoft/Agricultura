using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Text;
using log4net;
using System.IO;
using System.Web.Script.Serialization;

public partial class Growing_frmCapturaGrowing : BasePage
{
    /*
     *   Procedimientos Almacenados
     *      spr_ObtenerListaDeInvernaderosPorPlanta
     *      spr_GrowingObjetoActivoEnXML
     *      spr_GrowingCapturaGuardar
     *      spr_GrowingCapturaEdicion
     *      spr_GrowingCapturasAlmacenadas
     *      spr_GrowingCapturasObtenerPorID
     */

    private static readonly ILog log = LogManager.GetLogger(typeof(Growing_frmCapturaGrowing));
    public static bool spanish;
    public static string idUsuario;
    public static string idPlantaSession;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["idUsuario"] != null)
        {
            spanish = Session["Locale"].ToString().ToString().Equals("es-MX");
            idUsuario = Session["idUsuario"].ToString();
            idPlantaSession = Session["idPlanta"].ToString();
        }
        else {
             //Response.Redirect("~/frmLogin.aspx", false);
        }
        if (!Page.IsPostBack)
        {
            
        }
    }
    [WebMethod]
    public static string obtieneGerentes(String idPlanta)
    {
        String gerentes = "";
        try
        {
            idPlanta = idPlanta.Equals("0") ? HttpContext.Current.Session["idPlanta"].ToString() : idPlanta;

            DataTable ds = new DataTable();

            ds = new DataAccess().executeStoreProcedureDataTable("spr_GrowingGerentes", new Dictionary<string, object>() { { "@idPlanta", idPlanta } });

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow d in ds.Rows)
                {
                    gerentes += "<option value='" + d["idUsuario"] + "'>" + d["vNombre"] + "</option>";
                }
            }
            return gerentes;

        }
        catch (Exception e)
        {
            log.Error(e);
            return "Error";
        }
    }

    [WebMethod]
    public static string[] gerenteChange(String idUsuario)
    {
        String[] result = null;
        String lideres ="", invernaderos="", growers="";
        DataSet ds;
        try
        {
            ds = new DataAccess().executeStoreProcedureDataSet("spr_GrowingLideresInvs", new Dictionary<string, object>() { { "@idGerente", idUsuario } });

            if (ds.Tables.Count > 0)
            {
                
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    lideres += "<option value='" + r["idUsuario"] + "'>" + r["vNombre"] + "</option>";
                }
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    invernaderos += "<option value='" + r["idinvernadero"] + "'>" + r["claveinvernadero"] + "</option>";
                }
                foreach (DataRow r in ds.Tables[2].Rows)
                {
                    growers += "<option value='" + r["idUsuario"] + "'>" + r["vNombre"] + "</option>";
                }
                

            }
            return new String[] {lideres, invernaderos, growers};
        }
        catch (Exception e)
        {
            log.Error(e);
            return new String[] { "error" };
        }

    }

    [WebMethod]
    public static string[] obtenerInvernaderos(string idLider) {
        try 
	    {	        
		    DataAccess da = new DataAccess();
            DataTable dt = null;
            
                dt = da.executeStoreProcedureDataTable("spr_ObtieneInvernaderosPorLider", new Dictionary<string, object>() { 
                    {"@idLider", idLider },
                    {"@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() }
                });
            
            if (dt.Rows.Count > 0) {
                StringBuilder sb = new StringBuilder(); 
                foreach (DataRow R in dt.Rows)
                {
                    string IdInvernadero    = R["IdInvernadero"].ToString();
                    string ClaveInvernadero = R["ClaveInvernadero"].ToString();
                    sb.AppendLine("<option idInvernadero=\"" + IdInvernadero + "\">" + ClaveInvernadero + "</option>");
                }
                return new string[] { "ok", sb.ToString() };
            }
            else {
                return new string[] { "error", spanish ? "No tiene invernaderos asignados en la planta seleccionada." : "No assigned greenhouses in the selected plant." };
            }
	    }
	    catch (Exception x)
	    {
            log.Error(x);
            return new string[] { "error", spanish ? "Error en el envío de datos." : "Error in sending data." };
	    }
    }

    [WebMethod]
    public static string[] obtenerFormulario()
    {
        try
        {
            DataAccess da = new DataAccess();
            DataTable dt = da.executeStoreProcedureDataTable("spr_GrowingObjetoActivoEnXML", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"].ToString() } });
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow R in dt.Rows)
                {
                    sb.Append(R[0].ToString());
                }
                return new string[] { "ok", sb.ToString() };
            }
            else
            {
                return new string[] { "error", spanish ? "Error en la lectura de base de datos." : "Database reading failure." };
            }
        }
        catch (Exception x)
        {
            log.Error(x);
            return new string[] { "error", spanish ? "Error en el envío de datos." : "Error in sending data." };
        }
    }


    [WebMethod]
    public static string[] guardarCaptura(GrowingCaptura captura)
    {
        //    try
        //    {
        //        DataTable dtGrupo     = Utileria.obtenerDataTableDeLaClase(typeof(GrowingGrupo));
        //        DataTable dtParametro = Utileria.obtenerDataTableDeLaClase(typeof(GrowingParametro));
        //        DataTable dtPropiedad = Utileria.obtenerDataTableDeLaClase(typeof(GrowingPropiedad));

        //        dtGrupo.Columns.Add("UUID");    
        //        dtParametro.Columns.Add("UUID");
        //        dtPropiedad.Columns.Add("UUID");

        //        foreach (GrowingGrupo G in captura.grupo)
        //        {
        //            DataRow R = dtGrupo.NewRow();
        //            R["idGrupo"] = G.idGrupo;
        //            R["Calificacion"] = G.Calificacion;
        //            R["CalificacionCalculada"] = G.CalificacionCalculada;
        //            R["Cumplimiento"] = 0;
        //            R["UUID"] = Guid.NewGuid();
        //            dtGrupo.Rows.Add(R);
        //        }

        //        foreach (GrowingParametro P in captura.parametro)
        //        {
        //            DataRow R = dtParametro.NewRow();
        //            R["idParametro"] = P.idParametro;
        //            R["Calificacion"] = P.Calificacion;
        //            R["Cumplimiento"] = P.Cumplimiento;
        //            R["UUID"] = Guid.NewGuid();
        //            dtParametro.Rows.Add(R);
        //        }

        //        foreach (GrowingPropiedad P in captura.propiedad)
        //        {
        //            DataRow R = dtPropiedad.NewRow();
        //            R["idPropiedad"] = P.idPropiedad;
        //            R["Calificacion"] = P.Calificacion;
        //            R["Cumplimiento"] = P.Cumplimiento;
        //            R["OpcionSeleccionada"] = P.OpcionSeleccionada;
        //            R["UUID"] = Guid.NewGuid();
        //            dtPropiedad.Rows.Add(R);
        //        }

        //        DataAccess da = new DataAccess();
        //        Dictionary<string, object> parametros = new Dictionary<string, object>() { 
        //            {"@etiqueta", captura.Etiqueta},
        //            {"@comentarios", captura.Comentarios},
        //            {"@fechaCaptura", captura.FechaCaptura},
        //            {"@idInvernadero", captura.idInvernadero},
        //            {"@CalificacionCalculada", captura.CalificacionCalculada},
        //            {"@Calificacion", captura.Calificacion},
        //            {"@plantacion", captura.Plantacion},
        //            {"@grupos", dtGrupo},
        //            {"@parametros", dtParametro},
        //            {"@propiedades", dtPropiedad},
        //            {"@idUsuario", idUsuario}

        //        };
        //        DataTable dt = null;
        //        if (captura.idCaptura == 0)
        //        {
        //            parametros.Add("@UUID",  Guid.NewGuid());
        //            dt = da.executeStoreProcedureDataTable("spr_GrowingCapturaGuardar", parametros);
        //        }
        //        else
        //        {
        //            parametros.Add("@idCaptura", captura.idCaptura);
        //            dt = da.executeStoreProcedureDataTable("spr_GrowingCapturaEdicion", parametros);
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0]["Estado"].ToString().Equals("1"))
        //            {
        //                return new string[] { "ok", spanish ? "La información se almacenó correctamente." : "The information was stored properly." };
        //            }
        //            else {
        //                return new string[] { "error", spanish ? "Error en el procedimiento de base de datos." : "Procedural error database." };
        //            }
        //        }
        //        else
        //        {
        //            return new string[] { "error", spanish ? "Error en la llamada a base de datos." : "Database error.", dt.Rows[0]["Mensaje"].ToString() };
        //        }

        //    }
        //    catch (Exception x)
        //    {
        //        log.Error(x);
        //        return new string[] { "error", spanish ? "Error en el envío de datos." : "Error in data sending.", x.Message };
        //    }
        return null;
    }

    [WebMethod]
    public static string[] obtenerCapturas(string idPlanta, string idlider, string idGerente, string idGrower, string idInvernadero )
    {
        try
        {
            idPlanta = String.IsNullOrEmpty(idPlanta)? "0" : idPlanta;
            idlider = String.IsNullOrEmpty(idlider) ? "0" : idlider;
            idGrower = String.IsNullOrEmpty(idGrower) ? "0" : idGrower;
            idInvernadero = String.IsNullOrEmpty(idInvernadero) ? "0" : idInvernadero;
            idGerente = String.IsNullOrEmpty(idGerente) ? "0" : idGerente;
            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_GrowingCapturasAlmacenadas", new Dictionary<string, object>() { 
                  {"@idGrower", idGrower},
                  {"@idPlanta", idPlanta.Equals("0") ? HttpContext.Current.Session["idPlanta"] : idPlanta  },
                  {"@idGerente",idGerente},
                  {"@idLider",idlider},
                  {"@idInvernadero",idInvernadero}
            });
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string idCaptura = row["idCaptura"].ToString();
                    string Etiqueta	 = row["Etiqueta"].ToString();
                    string FechaCaptura = row["FechaCaptura"].ToString().Split(' ')[0];	
                    string FechaCreacion = row["FechaCreacion"].ToString();

                    sb.AppendLine("<li idCaptura=\""+idCaptura+"\" onclick=\"cargarCaptura("+idCaptura+");\">"+FechaCaptura+" - "+ Etiqueta +"</li>");
                }
                return new string[] { "ok", sb.ToString() };
            }
            else
            {
                return new string[] { "info", "No se encontraron capturas registradas.", "No records were obtained." };
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "error", "Obtener Capturas: Error al obtener los datos.", "Failed to get the data." };
        }
    }

    [WebMethod]
    public static string[] cargarDatosDeCaptura(int idCaptura){
     try
        {
            DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_GrowingCapturasObtenerPorID", new Dictionary<string, object>() { 
                {"@idCaptura",idCaptura}
            });
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                var stringBuilder = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    stringBuilder.Append(row[0]);
                }
                var xml = stringBuilder.ToString();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GrowingCaptura));
                var reader = new StringReader(xml);
                var objeto = (GrowingCaptura)serializer.Deserialize(reader);
                return new string[] { "ok", new JavaScriptSerializer().Serialize(objeto) };
            }
            else
            {
                return new string[] { "info", "No se obtuvieron registros.", "No records were obtained." };
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] { "error", "Cargar Captura: Error al obtener los datos.", "Failed to get the data." };
        }
    }
}
