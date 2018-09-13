using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.Services;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Web.Script.Serialization;
using System.Data.SQLite;

public partial class pages_frmCargaDRP :BasePage
{
    SQLiteDataAccess ldataaccess;
    private static readonly ILog log = LogManager.GetLogger(typeof(SQLiteDataAccess));
    DataTable result = new DataTable();
    DataTable embarq = new DataTable();
    DataTable grow = new DataTable();
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        divGrid.Visible = false;
    }
    protected void btn_Importar_Click(object sender, EventArgs e)
    {
        result.Columns.Add("tabla");
        result.Columns.Add("realizado");
        result.Columns.Add("registros");

        embarq = result.Clone();
        grow = result.Clone();

        DataSet Sync;
        DataSet Embarques;
        DataSet Growing;
        Dictionary<String, Object> dataCosecha = new Dictionary<string, object>();
        Dictionary<String, Object> dataEmbarques = new Dictionary<string, object>();
        Dictionary<String, Object> dataGrowing = new Dictionary<string, object>();
        String[] Queries = null;
        String queriespath;
        String idUsuario="";

        //----------ActividadPrograma
        DataTable dtActividadesProgramadas = DataTablesSync.dtActividadProgramada();
        DataTable dtPeriodos = DataTablesSync.dtActividadPeriodos();
        DataTable dtJornales = DataTablesSync.dtActividadJornales();
        DataTable dtNoProgramadas = DataTablesSync.dtActividadNoProgramada();

        //-----------Cosecha
        DataTable dtCosechas = DataTablesSync.dtCosecha();
        DataTable dtFormasA = DataTablesSync.dtFormaAv2();
        DataTable dtCapturasFormaA = new CapturaFormaAv2().toDataTable();
        DataTable dtMermas = DataTablesSync.dtMerma();
        DataTable dtTrasladoMermas = DataTablesSync.dtTrasladoMerma();

        //-----------Checklist

        DataTable dtMonitoreos = new Monitoreo().toDataTable();
        DataTable dtChecklists = new CheckList().toDataTable();
        DataTable dtCheckcriterios = new CheckCriterio().toDataTable();

        //----------PreHarvest

        DataTable dtBrixCapturas = DataTablesSync.dtBrixCaptura();
        DataTable dtBrixDetalles = DataTablesSync.dtBrixDetalle();
        DataTable dtBrixHeader = DataTablesSync.getDtBrixHeader();
        DataTable dtBrixFirmeza = DataTablesSync.getDtBrixFirmeza();
        DataTable dtBrixColor = DataTablesSync.getDtBrixColor();
        DataTable dtBrixDefecto = DataTablesSync.getDtBrixDefecto();

        //-----------Projection

        DataTable dtCajasCapturas = DataTablesSync.dtCajasCaptura();
        DataTable dtCajasCapturaDetalles = DataTablesSync.dtCajasCapturaDetalle();

        if (!Path.GetFileName(fu_Plantilla.PostedFile.FileName).Split('.').Last().ToLower().Equals("db"))
        {
            popUpMessage.setAndShowInfoMessage("No se cargó un archivo o el archivo no tiene el formato correcto.", Comun.MESSAGE_TYPE.Error);
            log.Error("No se cargó un archivo o el archivo no tiene el formato correcto.");
        }
        else
        {
            string Destino = string.Empty;
            try
            {
                Destino = string.Format("{0}{1}_{2}", ConfigurationManager.AppSettings["CarpetaSQLite"], Session["idUsuario"], Path.GetFileName(fu_Plantilla.PostedFile.FileName));
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                popUpMessage.setAndShowInfoMessage("No se pudo generar una ruta para almacenar el archivo.", Comun.MESSAGE_TYPE.Error);
            }
            try
            {
                queriespath = ConfigurationManager.AppSettings["txtQueries"].ToString() + ConfigurationManager.AppSettings["DRPQueries"].ToString();
                log.Info(queriespath);
                if (File.Exists(queriespath))
                {
                    using (StreamReader sr = new StreamReader(queriespath))
                    {

                        String query = sr.ReadToEnd();
                        Queries = query.Split('|');

                    }
                }
                log.Info("leer archivo.");
                if (Directory.Exists(ConfigurationManager.AppSettings["CarpetaSQLite"].ToString()))
                {
                    fu_Plantilla.PostedFile.SaveAs(Destino);
                }
                else
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["CarpetaSQLite"].ToString());
                    fu_Plantilla.PostedFile.SaveAs(Destino);
                }
                log.Info("guarda bd.");
            }catch(Exception ex){
                log.Error("Al guardar bd-"+ex);
            }
            
            try{

                ldataaccess = new SQLiteDataAccess(Destino, ConfigurationManager.AppSettings["CarpetaSQLite"]);
                log.Info("lee bd.");
                log.Info(Queries);
            }
            catch(Exception ex){
                log.Error(ex);

            }
            
                if (null != Queries || Queries.Length > 0)
                {
                    try
                    {
                        log.Info("encontró arcihvo queries.");
                        idUsuario = ldataaccess.executeStoreProcedureDataSet(Queries[0]).Tables[0].Rows[0]["idUsuario"].ToString();

                        Sync = ldataaccess.executeStoreProcedureDataSet(Queries[1]);

                        log.Info("Obtiene info cosecha.");

                        int count = 0;
                        foreach (DataTable tabla in Sync.Tables)
                        {
                            tabla.TableName = tablasCosecha(count);
                            dataCosecha.Add(tabla.TableName, fixColumns(tabla, count++));


                        }
                        dataCosecha.Add("@idUsuario", idUsuario);
                        dataCosecha.Add("@checksum", -1);
                        Sync = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncAllV3", dataCosecha);

                        foreach (DataTable item in Sync.Tables)
                        {
                            DataRow row = result.NewRow();

                            row["tabla"] = item.TableName;
                            row["registros"] = item.Rows.Count;
                            if (item.Rows.Count > 0)
                                row["realizado"] = !item.Rows[0][0].ToString().Contains("Error") ? "True" : "False";
                            else
                                row["realizado"] = "True";


                            result.Rows.Add(row);
                        }

                        log.Info("sincroniza info cosecha.");
                    }
                    catch (IndexOutOfRangeException or)
                    {
                        log.Error("DRP-Cosecha bdVacia" + or);
                        popUpMessage.setAndShowInfoMessage("La Base de datos está vacía.",Comun.MESSAGE_TYPE.Warning);
                    }
                    catch (Exception ex)
                    {
                        log.Error("DRP Cosecha-" + ex);
                        popUpMessage.setAndShowInfoMessage("Error en Cosecha. El archivo cargado no es la ultima versión de base de datos.", Comun.MESSAGE_TYPE.Error);
                        return;
                    }

                    try
                    {

                        Embarques = ldataaccess.executeStoreProcedureDataSet(Queries[2]);
                        log.Info("obtiene info embarques.");

                        var count = 0;

                        foreach (DataTable tabla in Embarques.Tables)
                        {
                            tabla.TableName = tablasEmbarques(count++);
                            dataEmbarques.Add(tabla.TableName, tabla);
                            if (tabla.Columns.Count == 21)
                            {
                                var a = 8;
                            }
                        }
                        dataEmbarques.Add("@idUsuario", idUsuario);
                        dataEmbarques.Add("@checksum", -1);
                        Embarques = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncEmbarquesV5", dataEmbarques);

                        foreach (DataTable item in Embarques.Tables)
                        {
                            DataRow row = embarq.NewRow();

                            row["tabla"] = item.TableName;
                            row["registros"] = item.Rows.Count;
                            if (item.Rows.Count > 0)
                                row["realizado"] = !item.Rows[0][0].ToString().Contains("Error") ? "True" : "False";
                            else
                                row["realizado"] = "True";

                            embarq.Rows.Add(row);
                        }

                        log.Info("sincroniza info embarques.");

                    }
                    catch (Exception ex)
                    {
                        log.Error("Embarques-"+ex);
                        popUpMessage.setAndShowInfoMessage("Error en Embarques" + ex, Comun.MESSAGE_TYPE.Error);
                    }

                    Growing = ldataaccess.executeStoreProcedureDataSet(Queries[3]);
                    log.Info("obtiene info growing.");

                    try
                    {
                        var count = 0;

                        foreach (DataTable tabla in Growing.Tables)
                        {
                            tabla.TableName = tablasGrowing(count++);

                            dataGrowing.Add(tabla.TableName, tabla);
                        }
                        dataGrowing.Add("@idUsuario", idUsuario);
                        dataGrowing.Add("@checksum", -1);
                        Growing = dataaccess.executeStoreProcedureDataSet("sprAndroid_SyncGrowing", dataGrowing);

                        foreach (DataTable item in Growing.Tables)
                        {
                            DataRow row = grow.NewRow();

                            row["tabla"] = item.TableName;
                            row["registros"] = item.Rows.Count;
                            if (item.Rows.Count > 0)
                                row["realizado"] = !item.Rows[0][0].ToString().Contains("Error")?"True":"False";
                            else
                                row["realizado"] = "True";

                            grow.Rows.Add(row);
                        }

                        log.Info("sincroniza growing.");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Growing-" + ex);
                        popUpMessage.setAndShowInfoMessage("Error en Growing" + ex, Comun.MESSAGE_TYPE.Error);
                    }

                    gv_cosecha.DataSource = result;
                    gv_cosecha.DataBind();

                    gv_embarques.DataSource = embarq;
                    gv_embarques.DataBind();

                    gv_growing.DataSource = grow;
                    gv_growing.DataBind();

                    divGrid.Visible = true;

                    popUpMessage.setAndShowInfoMessage("Archivo procesado correctamente", Comun.MESSAGE_TYPE.Success);
                }
                else
                {
                    popUpMessage.setAndShowInfoMessage("Archivo de consultas no encontrado.", Comun.MESSAGE_TYPE.Error);
                }
            
        }
    }
    private DataTable fixColumns(DataTable tabla, int index)
    {
        DataTable dt = tabla.Clone();
        switch (index)
        {
            case 0:
                dt.Columns["semana"].DataType = System.Type.GetType("System.Int32");

                break;

            case 4:
                dt.Columns["cantidadProduccion"].DataType = System.Type.GetType("System.Int32");
                break;
            case 7:
                dt.Columns["surco"].DataType = System.Type.GetType("System.Int32");
                break;
            case 8:
                dt.Columns["surcoDe"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["surcoA"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["base"].DataType = System.Type.GetType("System.Int32");
                break;
            case 9:
            case 10:
                
                dt.Columns["usuarioModifica"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["fechaModifica"].DataType = System.Type.GetType("System.String");
                break;

            case 11:
                dt.Columns["idEstimadoCajas"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["UsuarioCaptura"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["usuarioModifica"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["fechaModifica"].DataType = System.Type.GetType("System.String");
                break;

            case 12:
                dt.Columns["idEstimadoCajasCaptura"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["idEstimadoCajas"].DataType = System.Type.GetType("System.Int32");
                dt.Columns["fechaModifica"].DataType = System.Type.GetType("System.String");
                break;

            case 13:
                dt.Columns["UUID"].DataType = System.Type.GetType("System.String");
                break;

            default:
                return tabla;
        }

        foreach (DataRow dr in tabla.Rows)
        {
            dt.ImportRow(dr);
        }

        return dt;
    }
    private String tablasEmbarques(int num)
    {
        switch (num)
        {
            case 0:
                return "@dtEmbarqueHeader";

            case 1:
                return "@dtEmbarqueDestino";
            case 2:
                return "@dtEmbarqueProducto";
            case 3:
                return "@dtEmbarqueFormaA";
            
            case 4:
                return "@dtEmbarqueFIFO";
            default:
                return "";
        }
    }

    private String tablasGrowing(int num)
    {
        switch (num)
        {
            case 0:
                return "@GrowingCapturaAndroid";

            case 1:
                return "@GrowingCapturaGrupoAndroid";
            case 2:
                return "@GrowingCapturaParametroAndroid";
            case 3:
                return "@GrowingCapturaParametroPropiedadAndroid";

            default:
                return "";
        }
    }

    private String tablasCosecha(int num)
    {
        switch (num)
        {
        case 0:
                return "@Actividades";
                

            case 1:
                return "@Periodos";
            case 2:
                return "@NoProgramadas";
            case 3:
                return "@Asociados";
            case 4:
                return "@Cosecha";
            case 5:
                return "@Merma";
            case 6:
                return "@FormaA";
            case 7:
                return "@CapturaFormaA";
            case 8:
                return "@Monitoreo";
            case 9:
                return "@CheckList";
            case 10:
                return "@CheckCriterio";
            case 11:
                return "@CajasCaptura";
            case 12:
                return "@CajasCapturaDetalle";
            case 13:
                return "@Traslado";
            case 14:
                return "@SeccionesPreharvest";
            default:
                return "";
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void save_Click(object sender, EventArgs e)
    {
    }
}