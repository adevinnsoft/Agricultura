using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Quartz;
using log4net;
using System.IO;
using System.Web;
using System.Globalization;
using System.Net.Mail;
using System.Configuration;
using Ionic.Zip;
namespace Jobs
{
    public class tarea : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(tarea));

        public void Execute(Quartz.IJobExecutionContext context)
        {
            //GenerarAPK();

        }

        SQLiteDataAccess sqliteda;
        DataAccess dataaccess = new DataAccess();
        DataSet ds;
        DirectoryInfo directoryAPk;
        DirectoryInfo directoryTemp;
        DirectoryInfo app;
        FileInfo file;
        FileInfo apk;

        protected void GenerarAPK()
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
                    foreach(FileInfo file in new DirectoryInfo(ConfigurationManager.AppSettings["CarpetaSQLite"]).GetFiles("*.db")){
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

                    log.Info("Archivo creado "+ planta["idPlanta"]+"_DefaultDataBase.db" );
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

        }

        private void addfiles(string sourc, string dest)
        {
            ZipFile myzip = null;
            try
            {
                myzip = ZipFile.Read(sourc);
                log.Info("Archivo apk leido");

                myzip.UpdateFile(dest, "/assets/");
                log.Info("Archivo " + dest + " cargado.");

                myzip.Save();
                log.Info("Apk guardado.");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {

                myzip.Dispose();
                log.Info(".apk liberado");
            }
        }



        private void sendMail(string para, string dias)
        {
            try
            {
                var pv = new Dictionary<string, string>();
                var files = new Dictionary<string, Stream>();
                Comun.SendMailByDictionary(pv, files, para, "notification", dias);
                log.Info("Correo Enviado");
            }
            catch (SmtpException smt)
            {
                log.Error(smt);
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }
        }
    }
}