using System;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;
using System.Configuration;
using System.Data;
using System.IO.Compression;
using SevenZip.Sdk;
using System.Diagnostics;
using log4net;
using Ionic.Zip;



public partial class pages_DefaultDatabase : Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(SQLiteDataAccess));
    SQLiteDataAccess sqliteda;
    DataAccess dataaccess = new DataAccess();
    DataSet ds;
    DirectoryInfo directoryAPk;
    DirectoryInfo directoryTemp;
    DirectoryInfo app;
    FileInfo file;
    FileInfo apk;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_Importar_Click(object sender, EventArgs e)
    {
        string Destino = string.Empty;
        String[] queries;
        DataTable plantas;
        String apkname;

        DirectoryInfo directoryDbs = new DirectoryInfo(ConfigurationManager.AppSettings["CarpetaSQLite"]);

        try
        {

            using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["txtQueries"] + "QLITEDefault.txt"))
            {

                String query = sr.ReadToEnd();
                queries = query.Split('|');

            }

            log.Info("Archivo Querys leido.");

            if (Directory.Exists(ConfigurationManager.AppSettings["CarpetaSQLite"].ToString()))
            {
                foreach (FileInfo file in new DirectoryInfo(ConfigurationManager.AppSettings["CarpetaSQLite"]).GetFiles("*.db"))
                {
                    file.Delete();
                }
            }

            log.Info("Archivos viejos .bd Eliminados.");

            plantas = dataaccess.executeStoreProcedureDataTable("spr_getActivePlants", null);

            log.Info("Plantas Activas: " + plantas.Rows.Count);

            foreach (DataRow planta in plantas.Rows)
            {
                sqliteda = new SQLiteDataAccess(planta["idPlanta"] + "_DefaultDataBase.db", ConfigurationManager.AppSettings["CarpetaSQLite"]);

                sqliteda.saveDb();

                log.Info("Archivo creado " + planta["idPlanta"] + "_DefaultDataBase.db");
                sqliteda.CreaTablas(queries);

                log.Info("Estructura de tablas");

                ds = dataaccess.executeStoreProcedureDataSet("sprAndroid_defaultInfo", new Dictionary<string, object>() { { "@idPlanta", planta["idPlanta"].ToString() } });
                log.Info("Informacion Default Obtenida.");

                ds.Tables[0].Rows[0][2].ToString();

                sqliteda.insertDefaultData(ds);
                log.Info("Vaciado de Informacion default");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        try
        {
            apkname = dataaccess.executeStoreProcedureString("spr_getApkName", null);
            apkname = apkname.Replace('/', ' ');
            apkname = apkname.Trim();

            apk = new FileInfo(ConfigurationManager.AppSettings["APK"] + apkname);

            log.Info("Archivo .apk encontrado");

            log.Info("Inicio Carga archivos a apk");
            foreach (FileInfo db in directoryDbs.GetFiles("*.db"))
            {
                addfiles(apk.FullName, db.FullName);
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            directoryAPk = null;
            directoryTemp = null;
            app = null;
            file = null;
        }

        popUpMessage.setAndShowInfoMessage("si jaló esta madre", Comun.MESSAGE_TYPE.Success);
    }

    private void addfiles(string sourc, string dest)
    {
        ZipFile myzip = null;
        try
        {
            myzip = ZipFile.Read(sourc);
            myzip.UpdateFile(dest,"/assets/");

            myzip.Save();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {

            myzip.Dispose();
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void save_Click(object sender, EventArgs e)
    {

    }
    protected void btn_GuardarTabla_Click(object sender, EventArgs e)
    {

    }
}