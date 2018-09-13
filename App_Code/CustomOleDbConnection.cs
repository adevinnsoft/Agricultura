using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using log4net;

public class CustomOleDbConnection
{
    OleDbConnection oConn = new OleDbConnection();
    OleDbCommand oCmd = new OleDbCommand();
    OleDbDataAdapter oDa = new OleDbDataAdapter();

    private static readonly ILog log = LogManager.GetLogger(typeof(CustomOleDbConnection));

    public CustomOleDbConnection(String fuente)
    {
        oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fuente + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; 
         //oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fuente + "Extended Properties=Excel 12.0 Xml;HDR=YES
    }
    public CustomOleDbConnection(string fuente, bool IMEX)
    {
        if (IMEX)
        {
            oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fuente + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        }
        else
        {
            oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fuente + ";Extended Properties=\"Excel 8.0;\"";
        }
    }
    public bool Open()
    {
        try
        {
            oConn.Open();
            oCmd.Connection = oConn;
            return true;
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
            return false;
        }


    }
    public void setCommand(string sql_query)
    {
        oCmd.CommandText = sql_query;
    }
    public DataTable executeQuery()
    {
        DataTable oDs = new DataTable();
        oDa.SelectCommand = oCmd;
        try
        {
            oDa.Fill(oDs);
        }
        catch (OleDbException ex)
        {
            log.Error(ex.ToString());
            oDs = null;
        }
        catch (Exception e) 
        {         
        }
        return oDs;
    }
 
    public bool isConnected()
    {
        if(oConn.State.ToString().Equals("Open"))
            return true;
        return false;

    }
    public bool Close()
    {
        try
        {
            oConn.Close();
            oConn.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString()); 
            return false;
        }
    }
}
