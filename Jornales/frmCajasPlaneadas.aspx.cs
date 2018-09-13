using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;
using System.IO;
using System.Web.Services;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;


public partial class Jornales_frmCajasPlaneadas : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Jornales_frmCajasPlaneadas));
    private static int idUsuario = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            //lo que se restsaura o se hace tras cada llamada
        }
        else
        {
            //lo que se hace para inicializar la pagina
            idUsuario = int.Parse(Session["userIDInj"].ToString());
        }
    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        if (!Path.GetFileName((fu_Plantilla.PostedFile != null ? fu_Plantilla.PostedFile.FileName : "")).Split('.').Last().ToLower().Equals("xls"))
        {
            popUpMessageControl1.setAndShowInfoMessage("No se cargó un archivo o el archivo no tiene el formato correcto.", Comun.MESSAGE_TYPE.Error);
        }else{
            //Verifica si podemos crear una ruta para el archivo de acuerdo a la key "CarpetaTemporalLibras" configurada en el web config
            string Destino = string.Empty;
            try
            {
                Destino = string.Format("{0}{1}_{2}", ConfigurationManager.AppSettings["CarpetaTemporalLibras"], Session["idUsuario"], Path.GetFileName(fu_Plantilla.PostedFile.FileName));
            }
            catch(Exception x)
            {
                log.Error(x);
                popUpMessageControl1.setAndShowInfoMessage("No se pudo generar una ruta para almacenar el archivo.", Comun.MESSAGE_TYPE.Error);
            }

            //Guarda boletin en la ruta destino
            try
            {
                if (Directory.Exists(ConfigurationManager.AppSettings["CarpetaTemporalLibras"].ToString()))
                {
                    if (File.Exists(Destino))
                    {
                        File.Delete(Destino);
                    }

                    fu_Plantilla.PostedFile.SaveAs(Destino);
                }
                else
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["CarpetatemporalLibras"].ToString());
                    fu_Plantilla.PostedFile.SaveAs(Destino);
                }

                //Lee todo el archivo para obtener los datos
                LecturaDeArchivoYGuardadoDeDatos(Destino);
            }
            catch (Exception x)
            {
                log.Error(x);
                popUpMessageControl1.setAndShowInfoMessage("La información del archivo no pudo ser leida, probablemente se cambió el formato de la plantilla.", Comun.MESSAGE_TYPE.Error);
            }
        }
    }

    private void LecturaDeArchivoYGuardadoDeDatos(string Destino)
    {
        try
        {
            CustomOleDbConnection cn = new CustomOleDbConnection(Destino);
            cn.Open();
            cn.setCommand("SELECT * FROM tbl_LibrasPlaneadas WHERE [Clave Invernadero] <> ''");
            //AND [Año Plantación] <> '' AND [Semana Plantación] <> '' AND [Interplanting] <> '' AND [Número De Corte] <> '' AND [Semana De Corte] <> '' AND [Año De Corte] <> '' AND [Libras] <> ''
            DataTable dtL = cn.executeQuery();
            
            foreach(DataColumn C in dtL.Columns){
                string columnName = C.ColumnName;
                columnName = columnName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "ni");
                columnName = Regex.Replace(columnName, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
                C.ColumnName = columnName;
            }

            if (dtL.Rows.Count > 0)
            {
                DataAccess da = new DataAccess();
                DataTable dt = da.executeStoreProcedureDataTable("spr_InsertarCajasPlaneadas", new Dictionary<string, object>() { 
                    {"@LibrasPlaneadas", dtL},
                    {"@idUsuario", 0}
                });

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Estado"].ToString().Equals("1"))
                    {
                        fu_Plantilla.Dispose();
                        popUpMessageControl1.setAndShowInfoMessage("Se ha importado correctamente la plantilla.", Comun.MESSAGE_TYPE.Success);
                    }
                    else
                    {
                        fu_Plantilla.Dispose();
                        popUpMessageControl1.setAndShowInfoMessage("No fué posible importar la plantilla.", Comun.MESSAGE_TYPE.Error);
                    }
                }
                else
                {
                    fu_Plantilla.Dispose();
                    popUpMessageControl1.setAndShowInfoMessage("No fué posible importar la plantilla.", Comun.MESSAGE_TYPE.Error);
                }
            }
            else
            {
                fu_Plantilla.Dispose();
                log.Error("El archivo no tiene información para leer.");
                popUpMessageControl1.setAndShowInfoMessage("El archivo no tiene información para leer.", Comun.MESSAGE_TYPE.Info);
            }
            
            cn.Close();
        }catch(Exception x)
        {
            fu_Plantilla.Dispose();
            log.Error(x);
            popUpMessageControl1.setAndShowInfoMessage("Error en la importación, descargue la plantilla llene el formato e intente subirlo de nuevo.", Comun.MESSAGE_TYPE.Error);
        }
    }

    protected void btnPlantilla_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            List<string> L = new List<string>(); 
            L.Add("Clave Invernadero");
            L.Add("Año Plantación");
            L.Add("Semana Plantación");
            L.Add("Interplanting");
            L.Add("Número De Corte");
            L.Add("Semana De Corte");
            L.Add("Año De Corte");
            L.Add("Libras");	

            Plantilla P = new Plantilla(Response, "Libras Planeadas");
            P.CrearTabla(L, 40000, 1, 1, "Libras Planeadas", "tbl_LibrasPlaneadas", "Libras Planeadas");
            P.ProtegerArchivo();
            P.GuardarPlantilla();
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }
    }
}