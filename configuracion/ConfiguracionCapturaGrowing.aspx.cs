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

public partial class configuracion_ConfiguracionCapturaGrowing : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_ConfiguracionCapturaGrowing));
    private static string idUsuario = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string[] guardarConfiguracionGrowing(Grupo[] Grupos)
    {
        int indice1 = 0;
        int indice2 = 0;
        int indice3 = 0;
        int indice4 = 0;

        DataAccess da = new DataAccess();
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataTable dtGrupo = new DataTable();
        DataTable dtParametro = new DataTable();
        DataTable dtPropiedad = new DataTable();
        DataTable dtOpcion = new DataTable();
        DataRow drGrupo;
        DataRow drParametro;
        DataRow drPropiedad;
        DataRow drOpcion;
        DataTable dtResultado;


        if (Grupos.Length == 0)
        {
            return new string[] {"0","No se realizaron cambios.","warning" };
        }
        else
        {
            try
            {
                idUsuario = System.Web.HttpContext.Current.Session["userIDInj"].ToString();

                dtGrupo.Columns.Add("idGrupo");
                dtGrupo.Columns.Add("nombreES");
                dtGrupo.Columns.Add("nombreEN");
                dtGrupo.Columns.Add("Ponderacion");
                dtGrupo.Columns.Add("PonderacionNP");
                dtGrupo.Columns.Add("AplicaPlantacion");
                dtGrupo.Columns.Add("AplicaNoPlantacion");
                dtGrupo.Columns.Add("activo");
                dtGrupo.Columns.Add("ordenGrupo");
                dtGrupo.Columns.Add("indice");
                dtGrupo.Columns.Add("padre");

                dtParametro.Columns.Add("idGrupo");
                dtParametro.Columns.Add("idParametro");
                dtParametro.Columns.Add("nombreES");
                dtParametro.Columns.Add("nombreEN");
                dtParametro.Columns.Add("activo");
                dtParametro.Columns.Add("ordenParametro");
                dtParametro.Columns.Add("indice");
                dtParametro.Columns.Add("padre");

                dtPropiedad.Columns.Add("idParametro");
                dtPropiedad.Columns.Add("idPropiedad");
                dtPropiedad.Columns.Add("nombreES");
                dtPropiedad.Columns.Add("nombreEN");
                dtPropiedad.Columns.Add("activo");
                dtPropiedad.Columns.Add("tieneCapturaNumero");
                dtPropiedad.Columns.Add("tieneCapturaTexto");
                dtPropiedad.Columns.Add("tieneCapturaCumplimiento");
                dtPropiedad.Columns.Add("ordenPropiedad");
                dtPropiedad.Columns.Add("indice");
                dtPropiedad.Columns.Add("padre");

                dtOpcion.Columns.Add("idPropiedad");
                dtOpcion.Columns.Add("idOpcion");
                dtOpcion.Columns.Add("nombreES");
                dtOpcion.Columns.Add("nombreEN");
                dtOpcion.Columns.Add("activo");
                dtOpcion.Columns.Add("calificacionOpcion");
                dtOpcion.Columns.Add("tieneCapturaNumero");
                dtOpcion.Columns.Add("tieneCapturaTexto");
                dtOpcion.Columns.Add("tieneCapturaCumplimiento");
                dtOpcion.Columns.Add("ordenOpcion");
                dtOpcion.Columns.Add("sag");
                dtOpcion.Columns.Add("indice");
                dtOpcion.Columns.Add("padre");

                foreach (Grupo G in Grupos)
                {
                    drGrupo = dtGrupo.NewRow();
                    drGrupo["idGrupo"] = G.idGrupo;
                    drGrupo["nombreES"] = G.nombreGrupoES;
                    drGrupo["nombreEN"] = G.nombreGrupoEN;
                    drGrupo["Ponderacion"] = G.Ponderacion;
                    drGrupo["PonderacionNP"] = G.PonderacionNP;
                    drGrupo["AplicaPlantacion"] = G.AplicaPlantacion;
                    drGrupo["AplicaNoPlantacion"] = G.AplicaNoPlantacion;
                    drGrupo["activo"] = G.activo;
                    drGrupo["ordenGrupo"] = G.ordenGrupo;
                    drGrupo["indice"] = ++indice1;
                    drGrupo["padre"] = null;
                    dtGrupo.Rows.Add(drGrupo);

                    foreach (Parametro P in G.Parametros)
                    {
                        drParametro = dtParametro.NewRow();
                        drParametro["idGrupo"] = G.idGrupo;
                        drParametro["idParametro"] = P.idParametro;
                        drParametro["nombreES"] = P.nombreParametroES;
                        drParametro["nombreEN"] = P.nombreParametroEN;
                        drParametro["activo"] = P.activo;
                        drParametro["ordenParametro"] = P.ordenParametro;
                        drParametro["indice"] = ++indice2;
                        drParametro["padre"] = indice1;
                        dtParametro.Rows.Add(drParametro);

                        foreach (Propiedad PRO in P.Propiedades)
                        {
                            drPropiedad = dtPropiedad.NewRow();
                            drPropiedad["idParametro"] = P.idParametro;
                            drPropiedad["idPropiedad"] = PRO.idPropiedad;
                            drPropiedad["nombreES"] = PRO.nombrePropiedadES;
                            drPropiedad["nombreEN"] = PRO.nombrePropiedadEN;
                            drPropiedad["activo"] = PRO.activo;
                            drPropiedad["tieneCapturaNumero"] = PRO.tieneCapturaNumero;
                            drPropiedad["tieneCapturaTexto"] = PRO.tieneCapturaTexto;
                            drPropiedad["tieneCapturaCumplimiento"] = PRO.tieneCapturaCumplimiento;
                            drPropiedad["ordenPropiedad"] = PRO.ordenPropiedad;
                            drPropiedad["indice"] = ++indice3;
                            drPropiedad["padre"] = indice2;
                            dtPropiedad.Rows.Add(drPropiedad);

                            foreach (Opcion O in PRO.Opciones)
                            {
                                drOpcion = dtOpcion.NewRow();
                                drOpcion["idPropiedad"] = PRO.idPropiedad;
                                drOpcion["idOpcion"] = O.idOpcion;
                                drOpcion["nombreES"] = O.nombreOpcionES;
                                drOpcion["nombreEN"] = O.nombreOpcionEN;
                                drOpcion["activo"] = O.activo;
                                drOpcion["calificacionOpcion"] = O.calificacionOpcion;
                                drOpcion["tieneCapturaNumero"] = O.tieneCapturaNumero;
                                drOpcion["tieneCapturaTexto"] = O.tieneCapturaTexto;
                                drOpcion["tieneCapturaCumplimiento"] = O.tieneCapturaCumplimiento;
                                drOpcion["ordenOpcion"] = O.ordenOpcion;
                                drOpcion["sag"] = O.sag;
                                drOpcion["indice"] = ++indice4;
                                drOpcion["padre"] = indice3;
                                dtOpcion.Rows.Add(drOpcion);
                            }
                        }
                    }
                }

                prm.Add("@idUsuario", idUsuario);
                prm.Add("@Grupos", dtGrupo);
                prm.Add("@Parametros", dtParametro);
                prm.Add("@Propiedades", dtPropiedad);
                prm.Add("@Opciones", dtOpcion);
                prm.Add("@idPlanta", HttpContext.Current.Session["idPlanta"].ToString());

                dtResultado = da.executeStoreProcedureDataTable("spr_GuardarConfiguracionCapturaGrowing", prm);

                if (dtResultado.Rows.Count > 0)
                {
                    if (dtResultado.Rows[0]["Estado"].ToString().Equals("1"))
                    {
                        return new string[] { "1", "Configuración guardada correctamente.", "ok" };
                    }
                    else
                    {
                        return new string[] { "0", "Error al guardar la configuración.", "warning" };
                    }
                }
                else
                {
                    return new string[] { "0", "El proceso no generó ningún resultado.", "warning" };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new string[] { "0", "Error al tratar de procesar los datos.", "error" };
            }
        }
      
    }


    [WebMethod]
    public static string[] obtenerConfiguracionCapturaGrowing()
    {
        DataAccess da = new DataAccess();
        StringBuilder sb = new StringBuilder();
        DataTable dt = new DataTable();

        try
        {
            dt = da.executeStoreProcedureDataTable("spr_ObtenerConfiguracionCapturaGrowing", new Dictionary<string, object>() {
                {"@idPlanta", HttpContext.Current.Session["idPlanta"].ToString()}
            });
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {   
                    sb.Append(row[0].ToString());
                }

                return new string[] {"1", "ok", sb.ToString() };
            }
            else
            {
                return new string[] {"0","No hay configuración para esta planta.", "warning" };
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return new string[] {"0","Error al obtener la configuración", "warning" };
        }
    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
       

    }

}