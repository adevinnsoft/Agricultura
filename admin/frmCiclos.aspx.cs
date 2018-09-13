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

public partial class frmCiclos : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmCiclos));
    public DataAccess da = new DataAccess();
    public static DataTable dtc;
    public static DataTable dtExtras;
    public static List<Boolean> Estras;
    public static List<string> Formatos;
    public static List<string> Longitudes;
    public static List<string> Requeridos;
    public static List<Boolean> Editar;
    public static int cpropios = 0;
    public static int cextras = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
        }
        else
        {
        }
    }

    [WebMethod(EnableSession = true)]
    public static string obtineCiclos()
    {
        try
        {
            DataAccess da = new DataAccess();
            Formatos = new List<string>();
            Longitudes = new List<string>();
            Requeridos = new List<string>();
            Editar = new List<Boolean>();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            dtc = da.executeStoreProcedureDataTable("spr_CiclosObtener", new Dictionary<string, object>() { { "@idPlanta", HttpContext.Current.Session["idPlanta"] } });
            if (dtc.Rows.Count != 0)
            {
                dtExtras = da.executeStoreProcedureDataTable("spr_CamposExtrasCiclo", parametros);

                cpropios = dtc.Columns.Count;
                cextras = dtExtras.Columns.Count;

                foreach (DataColumn col in dtExtras.Columns)
                {
                    if (col.ColumnName != "idRealCiclo")
                    {
                        switch (col.DataType.ToString().Split('.')[1])
                        {
                            case "Int32":
                                dtc.Columns.Add(col.ColumnName, typeof(int));
                                break;
                            case "Decimal":
                                dtc.Columns.Add(col.ColumnName, typeof(decimal));
                                break;
                            case "Boolean":
                                dtc.Columns.Add(col.ColumnName, typeof(Boolean));
                                break;
                            case "String":
                                dtc.Columns.Add(col.ColumnName, typeof(string));
                                break;
                            default:
                                dtc.Columns.Add(col.ColumnName);
                                break;
                        }
                    }
                }

                String idCiclos = "";
                foreach (DataRow item in dtc.Rows)
                {
                    idCiclos += item["idRealCiclo"] + "|";
                }
                DataTable dtCE = da.executeStoreProcedureDataTable("spr_CamposExtrasCiclo", new Dictionary<string, object>() { { "@idCiclos", idCiclos } });

                for (int i = 0; i < dtCE.Rows.Count; i++)
                {
                    if (dtc.Rows[i][0].ToString() == dtCE.Rows[i][0].ToString())
                    {
                        for (int j = 0; j < dtCE.Columns.Count - 1; j++)
                        {

                            dtc.Rows[i][cpropios + j] = dtCE.Rows[i][j + 1];
                        }
                    }
                }

                if (dtc.Rows.Count != 0)
                {
                    foreach (DataColumn col in dtc.Columns)
                    {
                        Formatos.Add(string.Format("{0}", col.DataType.ToString().Split('.')[1]));
                        try
                        {
                            Longitudes.Add("10"); //dtExtras.Select("Nombre = '" + col.ColumnName + "'")[0][6].ToString());
                            Requeridos.Add("requerid"); //Convert.ToBoolean(dtExtras.Select("Nombre = '" + col.ColumnName + "'")[0][8].ToString()) ? "requerid" : "");
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            Longitudes.Add("10");
                            Requeridos.Add("requerid");
                        }
                    }
                }
                return ConvertDataTableToHTML(dtc, "<table class='grid' cellspacing='0' rules='all' border='1' id='gv_Ciclos' style='border-collapse:collapse;'>", Formatos, Longitudes, Requeridos);
            }
            else
            {
                return "<div><table class='index'><tr><td><h1>No existen ciclos activos para esta planta.</h1></td></tr></table></div>";
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return ex.Message.ToString();
        }
    }

    [WebMethod]
    public static string[] GuardarCiclos(string[] matriz)
    {
        var response = new string[2];
        try
        {
            if (matriz.Length > 0)
            {
                DataTable dtr = new DataTable();
                foreach (DataColumn tr in dtExtras.Columns)
                {
                    dtr.Columns.Add(tr.ColumnName, tr.DataType);
                }

                DataTable dte = new DataTable();
                int tam = 0;
                foreach (DataColumn tr in dtc.Columns)
                {
                    if (tam < cpropios)
                    {
                        dte.Columns.Add(tr.ColumnName, tr.DataType);
                    }
                    tam++;
                }

                foreach (string ciclos in matriz)
                {
                    if (ciclos.Length > 0)
                    {
                        var columnas = ciclos.Split(',');
                        DataRow dr = dtr.NewRow();
                        for (int i = 0; i < cextras; i++)
                        {
                            if (columnas[i] == "")
                            {
                                switch (dtr.Columns[i].DataType.ToString().Split('.')[1])
                                {
                                    case "Int32":
                                        dr[i] = 0;
                                        break;
                                    case "String":
                                        dr[i] = "";
                                        break;
                                    case "Boolean":
                                        dr[i] = false;
                                        break;
                                    case "Decimal":
                                        dr[i] = 0.0;
                                        break;
                                    default:
                                        dr[i] = "";
                                        break;
                                }
                            }
                            else
                            {
                                switch (dtr.Columns[i].DataType.ToString().Split('.')[1])
                                {
                                    case "Boolean":
                                        dr[i] = (columnas[i] == "0" || columnas[i].ToLower() == "false" || columnas[i].ToLower() == "no" ? false : true);
                                        break;
                                    default:
                                        dr[i] = columnas[i];
                                        break;
                                }
                            }
                        }
                        dtr.Rows.Add(dr);
                    }
                }

                if (dtr.Rows.Count <= 0)
                {
                    response[0] = "error";
                    response[1] = "No hay Ciclos para guardar.";
                }
                else
                {
                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    parametros.Add("@idUsuario", HttpContext.Current.Session["idUsuario"].ToString());
                    parametros.Add("@Ciclos", dtr);
                    DataAccess da = new DataAccess();
                    DataTable dt = da.executeStoreProcedureDataTable("spr_CiclosGuardar", parametros);
                    if (dt.Rows[0][0].ToString().Equals("1"))
                    {
                        response[0] = "ok";
                        response[1] = "Se guardaron los campos adicionales correctamente.";
                    }
                    else
                    {
                        response[0] = "error";
                        response[1] = "La información no se pudo almacenar.";
                    }
                }
            }
            else
            {
                response[0] = "error";
                response[1] = "La información no se pudo almacenar.";
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            response[0] = "error";
            response[1] = ex.Message;
        }
        return response;
    }

    [WebMethod(EnableSession = true)]
    private static string ConvertDataTableToHTML(DataTable dt, string headerTable, List<string> Formato, List<string> Longitud, List<string> Requerido)
    {
        //HttpContext.Current.Session["toExcel"] = dt;
        string html = headerTable;
        Regex regex = new Regex("^[0-9]*$");
        html += "<table id='tablaCiclos'>";
        //add header row
        html += "<thead>";
        html += "<tr>";
        for (int i = 1; i < dt.Columns.Count; i++)
            html += string.Format("<th>{0}</th>", dt.Columns[i].ColumnName);
        html += "</tr>";
        html += "</thead>";
        //add rows
        html += "<tbody>";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr idRealCiclo='" + dt.Rows[i][0] + "'>";
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                string value = Formato[j] == "DateTime" ? DateTime.Parse(dt.Rows[i][j].ToString()).ToString("yyyy-dd-MM") : dt.Rows[i][j].ToString();
                html += string.Format("<td class='{1} {3} {4}' vprev='{0}' longitud='{2}'>{0}</td>", value, Formato[j], Longitud[j], Requerido[j], cpropios <= j ? "edit" : "readonly");
            }
            html += "</tr>";
        }
        html += "</tbody>";
        html += "</table>";
        HttpContext.Current.Session["toExcel"] = html;
        return html;
    }

    private DataSet ConvertHTMLTablesToDataSet(string HTML)
    {
        // Declarations    
        DataSet ds = new DataSet();
        DataTable dt = null;
        DataRow dr = null;
        string TableExpression = "<table[^>]*>(.*?)</table>";
        string HeaderExpression = "<th[^>]*>(.*?)</th>";
        string RowExpression = "<tr[^>]*>(.*?)</tr>";
        string ColumnExpression = "<td[^>]*>(.*?)</td>";
        bool HeadersExist = false;
        int iCurrentColumn = 0;
        int iCurrentRow = 0;

        // Get a match for all the tables in the HTML    
        MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        // Loop through each table element    
        foreach (Match Table in Tables)
        {
            // Reset the current row counter and the header flag    
            iCurrentRow = 0;
            HeadersExist = false;

            // Add a new table to the DataSet    
            dt = new DataTable();

            // Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names)    
            if (Table.Value.Contains("<th"))
            {
                // Set the HeadersExist flag    
                HeadersExist = true;

                // Get a match for all the rows in the table    
                MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Loop through each header element    
                foreach (Match Header in Headers)
                {
                    if (!dt.Columns.Contains(Header.Groups[1].ToString()))
                        dt.Columns.Add(Header.Groups[1].ToString().Replace("&nbsp;", ""));
                }
            }
            else
            {
                for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                {
                    dt.Columns.Add("Column " + iColumns);
                }
            }

            // Get a match for all the rows in the table    
            MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Loop through each row element    
            foreach (Match Row in Rows)
            {

                // Only loop through the row if it isn't a header row    
                if (!(iCurrentRow == 0 & HeadersExist == true))
                {

                    // Create a new row and reset the current column counter    
                    dr = dt.NewRow();
                    iCurrentColumn = 0;

                    // Get a match for all the columns in the row    
                    MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    // Loop through each column element    
                    foreach (Match Column in Columns)
                    {
                        if (Columns.Count != iCurrentColumn)
                            // Add the value to the DataRow    
                            dr[iCurrentColumn] = Convert.ToString(Column.Groups[1]).Replace("&nbsp;", "");

                        // Increase the current column     
                        iCurrentColumn += 1;
                    }
                    // Add the DataRow to the DataTable    
                    dt.Rows.Add(dr);
                }
                // Increase the current row counter    
                iCurrentRow += 1;
            }
            // Add the DataTable to the DataSet    
            ds.Tables.Add(dt);
        }
        return ds;
    }

}