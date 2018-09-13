using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class Planta
{
    private int intIdPlanta = 0;
    private string strNombre = "";

    public int IdPlanta 
    {
        get { return this.intIdPlanta; }
        set { this.intIdPlanta = value; }
    }
    public string Nombre
    {
        get { return this.strNombre; }
        set { this.strNombre = value; }
    }

    public DataTable ObtenerLista()
    {
        DataTable functionReturnValue = null;
        SqlConnection cnnMain = new SqlConnection();
        SqlCommand cmdQuery = null;
        SqlParameter prmGeneric = null;
        SqlDataAdapter daAdapter = default(SqlDataAdapter);
        DataTable tblTable = null;

        try
        {
            cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
            cnnMain.Open();

            cmdQuery = new SqlCommand("spr_ObtenerListaDePlantas", cnnMain);
            cmdQuery.CommandType = CommandType.StoredProcedure;
            cmdQuery.Parameters.Clear();

            daAdapter = new SqlDataAdapter(cmdQuery);
            tblTable = new DataTable();
            daAdapter.Fill(tblTable);

            if (tblTable.Rows.Count > 0)
            {
                functionReturnValue = tblTable;
            }
            else
            {
                functionReturnValue = null;
            }

        }
        catch (SqlException exError)
        {
            functionReturnValue = null;
        }
        finally
        {
            if (tblTable != null)
            {
                tblTable.Dispose();
                tblTable = null;
            }
            if (daAdapter != null)
            {
                daAdapter.Dispose();
                daAdapter = null;
            }
            if (prmGeneric != null)
                prmGeneric = null;
            if (daAdapter != null)
            {
                daAdapter.Dispose();
                daAdapter = null;
            }
            if (cmdQuery != null)
            {
                cmdQuery.Dispose();
                cmdQuery = null;
            }
            if (cnnMain.State == ConnectionState.Open)
            {
                cnnMain.Close();
            }
            if (cnnMain != null)
            {
                cnnMain.Dispose();
                cnnMain = null;
            }
        }
        return functionReturnValue;
    }
}