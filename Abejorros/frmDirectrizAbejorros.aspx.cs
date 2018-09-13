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

public partial class frmDirectrizAbejorros : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(frmDirectrizAbejorros));
    DataAccess da = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.IsPostBack)
        { }
        else
        {
            ddl_Planta.DataSource = da.executeStoreProcedureDataTable("spr_PlantaObtenerPorIdUsuario", new Dictionary<string, object>(){ 
                {"@idUsuario", Session["idUsuario"].ToString()}
            });
            ddl_Planta.DataTextField = "NombrePlanta";
            ddl_Planta.DataValueField = "idPlanta";
            ddl_Planta.DataBind();
            ddl_Planta.Items.Insert(0, new ListItem("-- Seleccione --", "0"));


            //chkl_Variable.Items.Clear();
            //foreach (DataRow item in  da.executeStoreProcedureDataTable("spr_Variable", new Dictionary<string, object>()).Rows)
            //{
            //    chkl_Variable.Items.Add(new ListItem(string.Format("<span class=\"invisible\">{0}</span>{1}", item["idVariable"], item["CodigoVariable"])));
            //}

            //chkl_Variedad.Items.Clear();
            //foreach (DataRow item in da.executeStoreProcedureDataTable("spr_Variedad", new Dictionary<string, object>()).Rows)
            //{
            //    chkl_Variedad.Items.Add(new ListItem(string.Format("<span class=\"invisible\">{0}</span>{1}", item["idVariedad"], item["CodigoVariedad"])));
            //}

            chk_Temporales.Items.Clear();
            foreach (DataRow item in da.executeStoreProcedureDataTable("spr_Temporal", new Dictionary<string, object>()).Rows)
            {
                chk_Temporales.Items.Add(new ListItem(string.Format("<span class=\"invisible\">{0}</span>{1}", item["idTemporal"], item["Temporal"])));
            }


            
        }
    }

    protected void btn_Importar_Click(object sender, EventArgs e)
    {
        if (!Path.GetFileName(fu_Plantilla.PostedFile.FileName).Split('.').Last().ToLower().Equals("xls"))
        {
            popUpMessage.setAndShowInfoMessage("No se cargó un archivo o el archivo no tiene el formato correcto.", Comun.MESSAGE_TYPE.Error);
            log.Error("No se cargó un archivo o el archivo no tiene el formato correcto.");
        }
        else
        {
            string Destino = string.Empty;
            try
            {
                Destino = string.Format("{0}{1}_{2}", ConfigurationManager.AppSettings["CarpetaDeTemporales"], Session["idUsuario"], Path.GetFileName(fu_Plantilla.PostedFile.FileName));
            }
            catch (Exception ex)    
            {
                log.Error(ex.ToString());
                popUpMessage.setAndShowInfoMessage("No se pudo generar una ruta para almacenar el archivo.", Comun.MESSAGE_TYPE.Error);
            }
            try
            {
                if (Directory.Exists(ConfigurationManager.AppSettings["CarpetaDeTemporales"].ToString()))
                { 
                    fu_Plantilla.PostedFile.SaveAs(Destino);
                }
                else
                { 
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["CarpetaDeTemporales"].ToString());
                    fu_Plantilla.PostedFile.SaveAs(Destino);
                }
                
                if (LecturaDeArchivoYCreacionDeTablas(Destino))
                {
                    //tbl_formulario.Style = "";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                popUpMessage.setAndShowInfoMessage("La información del archivo no pudo ser leida, probablemente se cambió el formato de la plantilla.", Comun.MESSAGE_TYPE.Error);
            }
        }
        
    }

    private bool LecturaDeArchivoYCreacionDeTablas(string Destino)
    {
        try
        {
            CustomOleDbConnection cn = new CustomOleDbConnection(Destino);
            cn.Open();
            cn.setCommand("SELECT * FROM tbl_DirectrizAbejorro");
            DataTable dt = cn.executeQuery();
            cn.setCommand("SELECT * FROM tbl_Generales");
            var planta = cn.executeQuery().Rows[0][0].ToString();
            var plantaValida = ddl_Planta.Items.FindByText(planta);
            if (plantaValida != null)
            {
                int Semanas = dt.Columns.Count - 1;
                divGrid.InnerHtml = ConvertDataTableToHTML(dt, "<table class=\"grid\" cellspacing=\"0\" rules=\"all\" border=\"1\" id=\"gv_Directriz\" style=\"border-collapse:collapse;\">");
                lblPlantaImportada.Text = plantaValida.Value;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception X)
        {
            log.Error(X);
            return false;
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        string Errores = string.Empty;
        Errores += ddl_Planta.SelectedValue.Equals("0") ? "Es necesario elegir una planta.<br/>" : string.Empty;
        Errores += Regex.IsMatch(txt_Semanas.Text, @"^\d+$") ? ((Decimal.Parse(txt_Semanas.Text) > 0 && Decimal.Parse(txt_Semanas.Text) <= 255) ? string.Empty : "La plantilla no se puede generar debido a que se excede el limite de columnas disponibles.") : "Es necesario un numero entero positivo para especificar las semanas.<br/>";

        if (Errores.Equals(string.Empty))
        {
            Plantilla P = new Plantilla(this.Response, "Abejorros_");
            try
            {
                Decimal semanas = Decimal.Parse(txt_Semanas.Text);
                List<string> Semanas = new List<string>();
                Semanas.Add("Semanas");
                for (int i = 1; i < semanas + 1; i++)
                {
                    Semanas.Add(i.ToString());
                }
                /*var dt = da.executeStoreProcedureDataTable("spr_EtapasObtenerPorPlantaActividad", new Dictionary<string, object>() { 
                        {"@idPlanta", ddl_Planta.SelectedValue},
                        {"@idioma", getIdioma()}
                    });*/
                List<string> Etapas = new List<string>();
                DataTable dtq = new DataTable();
                /*foreach (DataRow tr in dt.Rows)
                {
                    Etapas.Add(string.Format("{0}-{1}", tr["NombreCorto"], tr["NombreEtapa"]));
                }*/
                Etapas.Add("Colmenas");

                Errores += Etapas.Count > 0 ? "No se detectaron etapas para la planta elegida" : string.Empty;
                P.CrearTabla(Semanas, Etapas, 1, 5, "Directriz Abejorros", "tbl_DirectrizAbejorro", "Directriz Abejorros");
                DataTable dtPlanta = new DataTable();
                dtPlanta.Columns.Add("Planta");
                var dr = dtPlanta.NewRow();
                dr["Planta"] = ddl_Planta.SelectedItem.Text;
                dtPlanta.Rows.Add(dr);
                P.CrearTabla(dtPlanta, 1, 0, "Generales", "tbl_Generales", "Directriz Abejorros");
                P.ProtegerArchivo(ConfigurationManager.AppSettings["ContrasenaDeArchivos"].ToString());
                P.GuardarPlantilla();
            }
            catch (Exception x)
            {
                P.evitarDescargaDeArchivo();
                popUpMessage.setAndShowInfoMessage(x.Message.ToString(), Comun.MESSAGE_TYPE.Error);
                log.Error(x);
            }
        }
        else
        {
            popUpMessage.setAndShowInfoMessage(Errores, Comun.MESSAGE_TYPE.Error);
            return;
        }
    }

    [WebMethod]
    public static string GuardarDirectriz(string[] matriz, string nombreDirectriz, string idTemporal, int idPlanta)
    {
        string Errores = string.Empty;
        Errores += nombreDirectriz.Trim().Length == 0 ? "Se requiere un identificador para la directriz." : string.Empty;
        //Errores += idVariedad.Trim().Length == 0 ? "Se requiere que se configure para al menos una variedad." : string.Empty;
        //Errores += idVariable.Trim().Length == 0 ? "Se requiere que se configure para al menos una variable" : string.Empty;
        Errores += idTemporal.Trim().Length == 0 ? "Se requiere que se configure para al menos un temporal" : string.Empty;
        

        try
        {
            if (matriz.Length >= 1)
            {
                Errores = string.Empty;
                DataTable dtr = new DataTable();
                dtr.Columns.Add("Semana");
                //dtr.Columns.Add("Actividad");
                //dtr.Columns.Add("Etapa");
                dtr.Columns.Add("Cantidad");

                foreach (string etapa in matriz)
                {
                    if (etapa.Length > 0)
                    {
                        var etapaPorSemana = etapa.Split(',');
                        var Actividad = etapaPorSemana[0].ToString().Split('-')[0].Trim();
                        var Etapa = etapaPorSemana[0];//.ToString().Split('-')[1].Trim();

                        for (int i = 1; i < etapaPorSemana.Length; i++)
                        {
                            if (!etapaPorSemana[i].ToString().Equals(string.Empty))
                            {
                                DataRow dr = dtr.NewRow();
                                dr["Semana"] = i;
                                //dr["Actividad"] = Actividad;
                                //dr["Etapa"] = Etapa;
                                Errores += Regex.IsMatch(etapaPorSemana[i].ToString(), @"^\d+$") ? string.Empty : (etapaPorSemana[i].ToString().Equals(string.Empty) ? string.Empty : etapaPorSemana[0].ToString() + " Semana " + i + ": Se espera un numero entero. <br/>");
                                dr["Cantidad"] = /*etapaPorSemana[i].ToString().Equals(string.Empty) ? "0" : */etapaPorSemana[i].ToString();
                                dtr.Rows.Add(dr);
                            }
                            else
                            {
                                // Semana sin repeticiones
                            }
                        }
                    }
                    else
                    {
                        // Etapa sin datos
                    }
                }

                if (dtr.Rows.Count == 0)
                {
                    return "error" + ";" + "La directriz no tiene repeticiones de actividad.";
                }
                else
                {
                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    parametros.Add("@nombreDirectriz", nombreDirectriz);
                    parametros.Add("@nombreDirectriz_EN", nombreDirectriz);
                    //parametros.Add("@idVariedad", idVariedad);
                    //parametros.Add("@idVariable", idVariable);
                    parametros.Add("@idTemporal", idTemporal);
                    parametros.Add("@idPlanta", idPlanta);
                    //parametros.Add("@normal", normal);
                    //parametros.Add("@interplanting", interplanting);
                    parametros.Add("@idUsuario", HttpContext.Current.Session["idUsuario"].ToString());
                    parametros.Add("@idioma", 1);
                    parametros.Add("@directriz", dtr);
                    DataAccess da = new DataAccess();
                    var dt = da.executeStoreProcedureDataTable("spr_DirectrizAbejorroAlmacena", parametros);
                    if (dt.Rows[0][0].ToString().Equals("1"))
                        return "ok; Se guardó la configuración proporcionada.";
                    else
                        return "error" + ";" + "La información no se pudo almacenar.";
                    //Validar que se haya guardado correctamente.
                }
            }
            else
            {
                return "error" + ";" + "Aún no se ha cargado una directriz.";
            }
        }
        catch (Exception x)
        {
            log.Error(x);
            return "error;" + x.Message;
        }
    }

    [WebMethod]
    public static string GuardadasRecientemente()
    {
        DataAccess da = new DataAccess();
        StringBuilder sb_DirectricesAlmacenadas = new StringBuilder();
        int cantidad = Convert.ToInt32(ReadSetting("showNumDirectriz") == "" ? "5" : ReadSetting("showNumDirectriz"));
        int show = 1;
        foreach (DataRow R in da.executeStoreProcedureDataTable("spr_DirectrizAbejorros", new Dictionary<string, object>()).Rows)
        {
          sb_DirectricesAlmacenadas.Append("    <div class=\"group\" style='" + (show <= cantidad ? "" : "display:none;")+ "'>                     ");
          sb_DirectricesAlmacenadas.AppendFormat("        <h3 class=\"titulo\" id=\"{0}\">{1} - {2}</h3> ", R["idDirectrizabejorro"], R["FechaModifico"].ToString().Split(' ')[0], R["Nombre"]);
          sb_DirectricesAlmacenadas.Append("        <div class=\"detalle\"></div>                ");
          sb_DirectricesAlmacenadas.AppendFormat("        <input type=\"button\" class=\"cargarTablaDirectriz\" value=\"Cargar Directriz\" id=\"{0}\" />", R["idDirectrizAbejorro"]);
          sb_DirectricesAlmacenadas.Append("    </div>                                  ");

          show++;
        }

        return sb_DirectricesAlmacenadas.ToString();
    }


    public static string ReadSetting(string key)
    {
        try
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key];
            return result;
        }
        catch (ConfigurationErrorsException)
        {
           return "";
        }
    }
    [WebMethod]
    public static string ObtenerDetallesDeDirectriz(string idDirectriz)
    { 
        DataAccess da = new DataAccess();
        StringBuilder sb = new StringBuilder();
        DataTable dt = da.executeStoreProcedureDataTable("spr_DirectrizAbejorroObtenerConfiguracionPorId", new Dictionary<string, object>() { 
            {"@idDirectriz",idDirectriz}
        });

        DataView view = new DataView(dt);
        DataTable plantas = view.ToTable(true, "NombrePlanta");
        //DataTable variedades = view.ToTable(true, "CodigoVariedad");
        //DataTable variables = view.ToTable(true, "CodigoVariable");
        DataTable temporales = view.ToTable(true, "Temporal");

        sb.AppendLine("<h4>Plantas</h4>");
        foreach (DataRow R in plantas.Rows)
	    {
            sb.AppendLine(string.Format("<h5>{0}</h5>", R["NombrePlanta"]));
	    }
         /*sb.AppendLine("<h4>Variedades</h4>");
         foreach (DataRow R in variedades.Rows)
	    {
            sb.AppendLine(string.Format("<h5>{0}</h5>", R["CodigoVariedad"]));
	    }
         sb.AppendLine("<h4>Variables</h4>");
         foreach (DataRow R in variables.Rows)
	    {
            sb.AppendLine(string.Format("<h5>{0}</h5>", R["CodigoVariable"]));
	    }*/
         sb.AppendLine("<h4>Temporales</h4>");
         foreach (DataRow R in temporales.Rows)
	    {
            sb.AppendLine(string.Format("<h5>{0}</h5>", R["Temporal"]));
	    }
        return sb.ToString();
    }


    [WebMethod]
    public static string DirectrizObtenerTabla(string idDirectriz)
    {
        DataAccess da = new DataAccess(); 
        DataTable dt = da.executeStoreProcedureDataTable("spr_DirectrizAbejorroObtenerTabla", new Dictionary<string, object>() { 
            {"@idDirectriz", idDirectriz}
        });
        DataTable dt2 = da.executeStoreProcedureDataTable("spr_DirectrizAbejorroObtenerConfiguracionPorId", new Dictionary<string, object>() { 
            {"@idDirectriz",idDirectriz}
        });
        var idPlanta = dt2.Rows[0]["idPlanta"].ToString();
        return ConvertDataTableToHTML(dt, string.Format("<script> $('.txtPlantaImportada').val('{0}'); </script><table class=\"grid\" cellspacing=\"0\" rules=\"all\" border=\"1\" id=\"gv_Directriz\" style=\"border-collapse:collapse;\">", idPlanta));
    }

    [WebMethod (EnableSession=true)]
    private static string ConvertDataTableToHTML(DataTable dt, string headerTable)
    {
        //HttpContext.Current.Session["toExcel"] = dt;
        string html = headerTable;
        //add header row
        html +="<thead>";
        html += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
            html += string.Format("<th>{0}</th>", dt.Columns[i].ColumnName);
        html += "</tr>";
        html += "</thead>";
        //add rows
        html += "<tbody>";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
                html += string.Format("<td>{0}</td>", (dt.Rows[i][j].ToString() == "0" ? "" : dt.Rows[i][j]));
            html += "</tr>";
        }
        html += "</tbody>";
        html += "</table>";
        HttpContext.Current.Session["toExcel"] = html;
        return html;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        DataSet ds = ConvertHTMLTablesToDataSet(hddTabla.Value.Replace("☺", "<") /*Session["toExcel"].ToString()*/);
        DataTable dt = ds.Tables[0];

        int col = Convert.ToInt32(addColum.Text); //dt.Columns.Count;
        for (int j = 0; j < col; j++)
        { dt.Columns.Add("" + (dt.Columns.Count) , typeof(string));}

        Plantilla P = new Plantilla(this.Response, "Abejorros_");

        P.CrearTablaExport( dt, 1, 5, "Directriz Abejorro", "tbl_DirectrizAbejorro", "Directriz Abejorro");
        DataTable dtPlanta = new DataTable();
        dtPlanta.Columns.Add("Planta");
        var dr = dtPlanta.NewRow();
        dr["Planta"] = "";

        foreach (ListItem item in ddl_Planta.Items)
        {
            Console.WriteLine(item.Value +" " + item.Text);
            if (txtPlantaImportada.Text.Equals(item.Value))
            {
                dr["Planta"] = item.Text;
                break;
            }
        }

        dtPlanta.Rows.Add(dr);
        P.CrearTabla(dtPlanta, 1, 0, "Generales", "tbl_Generales", "Directriz Abejorro");
        P.ProtegerArchivo(ConfigurationManager.AppSettings["ContrasenaDeArchivos"].ToString());
        P.GuardarPlantilla();
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
                        if (Columns.Count  != iCurrentColumn)
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