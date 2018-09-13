using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DataAccess
{

    private String connectionName;

    public DataAccess()
    {
        connectionName = "dbConn";

        if (string.IsNullOrEmpty(connectionName))
        {
            throw new ConfigurationErrorsException("Missing connection Strings Key configuration");
        }
    }

    public DataAccess(string ConnectionName)
    {
        this.connectionName = ConnectionName;
    }

    public String GetConnectionName() 
    {
        return this.connectionName;
    }

    public DataTable executeStoreProcedureDataTable(String spName, Dictionary<String, Object> parameters)
    {
        return executeStoreProcedureDataSet(spName, parameters).Tables[0].Copy();
    }

    public DataTable executeStoreProcedureDataTableFill(string spName, Dictionary<String, Object> parameters)
    {
        
            SqlConnection dbconn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ToString());
            SqlCommand cmd = new SqlCommand(spName, dbconn);
            DataTable dt = null;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 7200;

            if (parameters != null)
            {
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(item, parameters[item]);
                }
            }
            try
            {

                dbconn.Open();

                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                sqlDA.AcceptChangesDuringFill = true;
                dt = new DataTable();

                sqlDA.Fill(dt);
                sqlDA.Dispose();
                cmd.Dispose();               

            }
            catch (Exception nfe)
            {
                throw;
            }
            finally
            {
                if (dbconn.State != ConnectionState.Closed)
                {
                    dbconn.Dispose();
                    dbconn.Close();
                }
            }

            return dt;
        
    }

    public void executeStoreProcedureNonQuery(String spName, Dictionary<String, Object> parameters)
    {
        SqlConnection dbconn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ToString());
        SqlCommand cmd = new SqlCommand(spName, dbconn);

        try
        {
           
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 7200;

            if (parameters != null)
            {
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(item, parameters[item]);
                }
            }


            dbconn.Open();
            cmd.ExecuteNonQuery();

        }
        catch (Exception nfe)
        {
            throw;
        }
        finally
        {
            if (dbconn.State != ConnectionState.Closed)
            {
                dbconn.Dispose();
                dbconn.Close();
            }
        }

    }

    public String executeStoreProcedureString(String spName, Dictionary<String, Object> parameters)
    {
        return executeStoreProcedureDataTable(spName, parameters).Rows[0][0].ToString();
    }

    public Int32 executeStoreProcedureGetInt(String spName, Dictionary<String, Object> parameters)
    {
        try
        {
            return Int32.Parse(executeStoreProcedureString(spName, parameters));
        }
        catch (Exception nfe)
        {
            throw;
        }

    }

    public Decimal executeStoreProcedureGetDecimal(String spName, Dictionary<String, Object> parameters)
    {
        try
        {
            return Decimal.Parse(executeStoreProcedureString(spName, parameters));
        }
        catch (Exception nfe)
        {
            throw;
        }
    }

    public Single executeStoreProcedureFloat(String spName, Dictionary<String, Object> parameters)
    {
        try
        {
            return Single.Parse(executeStoreProcedureString(spName, parameters));
        }
        catch (Exception nfe)
        {
            throw;
        }
    }

    public DataSet executeStoreProcedureDataSet( String spName, Dictionary<String, Object> parameters)
    {

        SqlConnection dbconn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ToString());
        SqlCommand cmd = new SqlCommand(spName, dbconn);
        DataSet ds = null;

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 7200;

        if (parameters != null)
        {
            foreach (string item in parameters.Keys)
            {
                cmd.Parameters.AddWithValue(item, parameters[item]);
            }
        }
        try
        {

            dbconn.Open();

            SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
            sqlDA.AcceptChangesDuringFill = true;
            ds = new DataSet();
            
            sqlDA.Fill(ds);
            sqlDA.Dispose();
            cmd.Dispose();

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
            if (dbconn.State != ConnectionState.Closed)
            {
                dbconn.Dispose();
                dbconn.Close();
            }
        }

        return ds;
    }
}