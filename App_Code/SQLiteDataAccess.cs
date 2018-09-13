using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using System.IO;
using log4net;

/// <summary>
/// Descripción breve de SQLiteDataAccess
/// </summary>
public class SQLiteDataAccess
{
    private String Database;
    private static readonly ILog log = LogManager.GetLogger(typeof(SQLiteDataAccess));
    private String connectionString;
    private String savePath;

    public SQLiteDataAccess()
    {
        if (string.IsNullOrEmpty(Database))
        {
            throw new SQLiteException("No Database Attached.");
        }
    }

    public SQLiteDataAccess(String Database,String Path)
    {
        this.Database = Database;
        this.savePath = Path;
        this.connectionString = "Data Source=" +Path+ Database+";Version=3; Count Changes=off;Journal Mode=off; Pooling=true;Cache Size=10000;Page Size=4096;Synchronous=off; ";

    }

    public DataSet executeStoreProcedureDataSet(String Query)
    {

        SQLiteConnection db = new SQLiteConnection("Data Source=" + Database + ";Version=3;");
        SQLiteCommand cmd = new SQLiteCommand(Query, db);
        DataSet ds = null;

        cmd.CommandType = CommandType.Text;
        cmd.CommandTimeout = 7200;


        try
        {

            db.Open();

            SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(cmd);

            ds = new DataSet();
            sqlDA.Fill(ds);
            sqlDA.Dispose();


            for (int i = 0; i < ds.Tables.Count; ++i)
            {
                ds.Tables[i].TableName = string.Format("table{0}", i + 1);
            }

        }
        catch (Exception nfe)
        {
            throw;
        }
        finally
        {
            if (db.State != ConnectionState.Closed)
            {
                cmd.Dispose();
                db.Close();
            }
        }

        return ds;
    }

    public void CreaTablas(String[] queries)
    {
        SQLiteCommand command;
        int count = 0;
        SQLiteConnection db = new SQLiteConnection(connectionString);

        try
        {
            db.Open();

            foreach (String query in queries)
            {
                count++;
                command = new SQLiteCommand(query, db);
                command.ExecuteNonQuery();
            }


            
        }
        catch (Exception ex)
        {
            log.Error(ex + "num: " + count + queries[count - 1]);
            throw new SQLiteException("ocurrió un error en la sentencia: " + count);
        }
        finally
        {
            db.Close();
        }
    }

    public void saveDb()
    {
        try
        {
            if (Directory.Exists(savePath))
            {

                SQLiteConnection.CreateFile(System.IO.Path.Combine(savePath + Database));

            }
            else
            {
                Directory.CreateDirectory(savePath);
                SQLiteConnection.CreateFile(System.IO.Path.Combine(savePath + Database));

            }

        }
        catch (Exception e)
        {
            log.Error(e);
            throw;
        }
    }
    public void insertDefaultData(DataSet data)
    {
        int count = 0;
        SQLiteConnection db = new SQLiteConnection(connectionString);

        try
        {
            foreach (DataTable tabla in data.Tables)
            {
                count++;
                tabla.TableName = getname(count);

                db.Open();

                SQLiteCommand command = db.CreateCommand();
                command.CommandText = (string.Format("DELETE FROM  {0}", tabla.TableName));
                command.ExecuteNonQuery();


                var cmd = db.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM {0}", tabla.TableName);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.AcceptChangesDuringFill = true;
                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);

                int asdf = adapter.Update(tabla);
                db.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw;
        }
        finally
        {
            db.Dispose();
        }
    }

    private String getname(int i)
    {
        switch (i)
        {
            case 1:
                return "DataCode";
            case 2:
                return "InvernaderoWppDos";
            case 3:
                return "EtapaWppDos";
            case 4:
                return "HabilidadWppDos";
            case 5:
                return "SeccionWppDos";
            case 6:
                return "SurcoWppDos";
            case 7:
                return "PlantasWppDos";
            case 8:
                return "tblLiderInvernadero";
            case 9:
                return "tblConfiguraciones";
            case 10:
                return "tblProductos";
            case 11:
                return "tblVariedad";
            case 12:
                return "Usuario";
            case 13:
                return "CiclosWppDos";
            case 14:
                return "TipoMermaCosechaWppDos";
            
            default:
                return "not";

        }
    }

}