using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;

/// <summary>
/// Summary description for Utileria
/// </summary>
public class Utileria
{
	public Utileria()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static DataTable obtenerDataTableDeLaClase(Type tipo)
    { 
        DataTable dt = new DataTable();
        foreach (FieldInfo item in tipo.GetFields())
        {
            if (!item.FieldType.Name.ToString().Contains("[]"))
                dt.Columns.Add(item.Name.ToString());
        }
        return dt;    
    }
    public static DataTable obtenerDataTableDeLaClase(Type tipo, string[] columnasAdicionales)
    {
        DataTable dt = new DataTable();
        foreach (FieldInfo item in tipo.GetFields())
        {
            if (!item.FieldType.Name.ToString().Contains("[]"))
                dt.Columns.Add(item.Name.ToString());
        }
        foreach (string columna in columnasAdicionales)
        {
            dt.Columns.Add(columna);
        }
        return dt;
    }
}