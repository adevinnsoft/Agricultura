using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

public partial class admin_ImportacionDeEjecucion : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(admin_ImportacionDeEjecucion));
    DataAccess da = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int x = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFullWeek, DateTime.Today.DayOfWeek);
            if (Page.IsPostBack)
            { }
            else
            {
                DataSet dt = da.executeStoreProcedureDataSet("spr_ImportacionEjecucion_ObtenerSemanasYPlantas", new Dictionary<string, object>() {});

                //ddl_Anio.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
                int indexAnio = 0, indexSemanas = 0;

                foreach (DataRow R in dt.Tables[1].DefaultView.ToTable(true, "Anio").Rows)
                {
                    string Anio = R["Anio"].ToString();
                    ddl_Anio.Items.Insert(indexAnio, new ListItem(Anio, Anio));
                    indexAnio++;
                }

                //ddl_Semana.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
                foreach (DataRow R in dt.Tables[1].Rows)
                {
                    string Semana = R["Semana"].ToString();
                    ddl_Semana.Items.Insert(indexSemanas, new ListItem(Semana, Semana));
                    indexSemanas++;
                }

                chksPlantas.Items.Clear();
                chksPlantas.Items.Add(new ListItem( string.Format("<span class='invisible todos'>0</span>Todas" )));
                foreach (DataRow item in dt.Tables[0].Rows)
                {
                    chksPlantas.Items.Add(new ListItem(string.Format("<span class='invisible'>{0}</span>{1}", item["idPlanta"], item["NombrePlanta"])));
                    //chksPlantas.Items.Add(new ListItem(item["NombrePlanta"].ToString(), item["idPlanta"].ToString()));
                }

                foreach (ListItem item in chksPlantas.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

    }

    protected void btnImportar_Click(object sender, EventArgs e)
    {
        int anioDePartida = Convert.ToInt32(ddl_Anio.SelectedItem.Text);
        int semanaDePartida = Convert.ToInt32(ddl_Semana.SelectedItem.Text);
        string idsPlanta = idsPlantas.Value;
        string[] IDS = idsPlanta.Split(',');

        string Destino = "";
        try
        {
            if (!Path.GetFileName(File1.PostedFile.FileName).Equals(""))
            {
                string extension = System.IO.Path.GetExtension(File1.PostedFile.FileName);
                if (extension == ".xls")
                {
                    try
                    {
                        Destino = string.Format("{0}{1}_{2}", ConfigurationManager.AppSettings["CarpetaDeTemporales"], Session["idUsuario"], Path.GetFileName(File1.PostedFile.FileName));
                        if (!Directory.Exists(ConfigurationManager.AppSettings["CarpetaDeTemporales"].ToString()))
                        {
                            Directory.CreateDirectory(ConfigurationManager.AppSettings["CarpetaDeTemporales"].ToString());
                        }
                        File1.PostedFile.SaveAs(Destino);
                        try
                        {
                            if (idsPlanta.Length > 0)
                            {
                                readFile(Destino, anioDePartida, semanaDePartida, IDS);
                            }
                            else
                            {
                                popUpMessageControl1.setAndShowInfoMessage("Falta especificar las plantas.", Comun.MESSAGE_TYPE.Info);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            popUpMessageControl1.setAndShowInfoMessage("Error interno.", Comun.MESSAGE_TYPE.Error);
                            log.Error(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        popUpMessageControl1.setAndShowInfoMessage("No se pudo escribir en el servidor.", Comun.MESSAGE_TYPE.Error);
                        log.Error(ex);
                    }
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage("La extensión del archivo no es válida.", Comun.MESSAGE_TYPE.Error);
                }
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage("Seleccione un archivo para importar.", Comun.MESSAGE_TYPE.Info);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }
        finally
        {
            if (File.Exists(Destino))
                File.Delete(Destino);
        }
    }


    public void readFile(string Destino, int anioPartida, int semanaPartida, string[] idsDePlantas)
    {
        int registrosProcesados = 0;
        CustomOleDbConnection cn = new CustomOleDbConnection(Destino);
        DataAccess da = new DataAccess();
        DataTable dtCiclos = null;
        var errorInvernadero = false;
        string CiclosSinCorte1 = "";
        string CiclosError = "";
        bool Sobreescribir = false;
        string idsPlantas = String.Join(",", idsDePlantas);

        try
        {
            List<string> Formatos = new List<string>();
            TimeCalculator timeCalculator = new TimeCalculator();
            if (CheckSobreescribirCiclos.Checked)
            {
                Sobreescribir = true;
                cn.Open();
                // cn.setCommand("SELECT * FROM Ciclos WHERE planta IN (" + idsPlantas + ") AND (semanap >= " + semanaPartida + " AND aniop >= " + anioPartida + ") AND planta IS NOT NULL AND numcorte IS NOT NULL");
                cn.setCommand("SELECT * "
                            + "FROM Ciclos c "
                            + "WHERE c.planta IN (" + idsPlantas + ") "
                            + "AND planta IS NOT NULL "
                            + "AND variedad IS NOT NULL"
                            + "AND numcorte IS NOT NULL"
                    );
                dtCiclos = cn.executeQuery();
                cn.Close();
            }
            else
            {
                cn.Open();
               // cn.setCommand("SELECT * FROM Ciclos WHERE planta IN (" + idsPlantas + ") AND (semanap >= " + semanaPartida + " AND aniop >= " + anioPartida + ") AND planta IS NOT NULL AND numcorte IS NOT NULL");
                cn.setCommand("SELECT * "
                            + "FROM Ciclos c "
                            + "WHERE c.planta IN (" + idsPlantas + ") "
                            + "AND planta IS NOT NULL AND numcorte IS NOT NULL"
                    );
                dtCiclos = cn.executeQuery();
                cn.Close();
            }

            if (dtCiclos != null)
            {
                if (dtCiclos.Rows.Count > 0)
                {
                    //Proceso para la importacion de los ciclos
                    DataTable dtCycles = CycleTable();
                    DataTable dtCyclesDetail = CycleDetailTable();
                    int index = -1;
                    string lastCombination = string.Empty;
                    int ciclosSinConfigurar = 0;

                    foreach (DataRow R in dtCiclos.Rows)
                    {
                        if (!R["numcorte"].ToString().Trim().Equals(""))
                        {
                            string planta = R["planta"].ToString().Trim();
                            string invernadero = R["invernadero"].ToString().Trim();
                            if (invernadero.Length > 4)
                            {
                                errorInvernadero = true;
                            }

                            string variedad = R["variedad"].ToString().Trim();
                            string semanap = R["semanap"].ToString().Trim();
                            string aniop = R["aniop"].ToString().Trim();
                            string semanapc = R["semanapc"].ToString().Trim();
                            string aniopc = R["aniopc"].ToString().Trim();
                            string interplanting = R["interplanting"].ToString().Trim().Equals("2") ? "1" : "0";
                            string codigoprefijo = R["codigoprefijo"].ToString().Trim();
                            string corte = R["numcorte"].ToString().Trim();
                            string semana = R["semana"].ToString().Trim();
                            string anio = R["año"].ToString().Trim();
                            string fallo = R["fallo"].ToString().Trim();
                            string suelo = R["suelo"].ToString().Trim();

                            if (lastCombination != (planta + invernadero + variedad + semanap + aniop + semanapc + aniopc + interplanting + codigoprefijo + suelo))
                            {
                                /*Ciclo nuevo. Revisar corte 1*/
                                if (!corte.Equals("1"))
                                {
                                    if (!CiclosSinCorte1.Contains("<br/>Planta: " + planta + " Invernadero: " + invernadero + " Plantación: " + semanap + "/" + aniop))
                                        CiclosSinCorte1 += "<br/>Planta: " + planta + " Invernadero: " + invernadero + " Plantación: " + semanap + "/" + aniop;
                                }
                                else
                                {
                                    lastCombination = planta + invernadero + variedad + semanap + aniop + semanapc + aniopc + interplanting + codigoprefijo + suelo;
                                    DataRow dr = dtCycles.NewRow();
                                    int p = 0;
                                    int semp = 0;
                                    int anp = 0;
                                    int sempc = 0;
                                    int anpc = 0;
                                    int interp = 0;
                                    int vari = 0;
                                    int su = 0;

                                    index++; 

                                    dr[0] = index;
                                    if (int.TryParse(planta.ToString(), out p))
                                    {
                                        dr[1] = p;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en la planta.";
                                    }

                                    if (p == Int32.Parse(invernadero[0].ToString()))
                                    {
                                        DataTable dtSearch = da.executeStoreProcedureDataTable("spr_getSearch", new Dictionary<string, object>() { { "@buscar", "GreenHouse" }, { "@valor", invernadero } });
                                        if (dtSearch.Rows.Count > 0)
                                        {
                                            dr[2] = invernadero;
                                        }
                                        else
                                        {
                                            CiclosError += "<br/>El codigo del invernadero " + invernadero + " no es válido.";
                                        }
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El invernadero " + invernadero + " no pertenece a la planta " + planta + ".";
                                    }
                                    
                                    if (int.TryParse(variedad.ToString(), out vari))
                                    {
                                        dr[3] = vari;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en la variedad.";
                                    }
                                    if (int.TryParse(semanap.ToString(), out semp))
                                    {
                                        dr[4] = semp;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en la semana de plantación.";
                                    }
                                    if (int.TryParse(aniop.ToString(), out anp))
                                    {
                                        dr[5] = anp;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto el año de plantación.";
                                    }
                                    if (int.TryParse(semanapc.ToString(), out sempc))
                                    {
                                        dr[6] = sempc;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en la semana de primer corte.";
                                    }
                                    if (int.TryParse(aniopc.ToString(), out anpc))
                                    {
                                        dr[7] = anpc;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en el año de primer corte.";
                                    }
                                    if (int.TryParse(interplanting.ToString(), out interp))
                                    {
                                        dr[8] = interp;
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en interplanting.";
                                    }
                                    dr[9] = codigoprefijo;
                                    if (int.TryParse(suelo.ToString(), out su))
                                    {
                                        dr[10] = su;
                                    }
                                    dtCycles.Rows.Add(dr);
                                }
                            }
                            else
                            {
                                //incremento = true;
                            }

                            decimal d = 0;
                            int co = 0;
                            int se = 0;
                            int an = 0;
                            for (int i = 14; i < dtCiclos.Columns.Count; i++)
                            {
                                DataRow drD = dtCyclesDetail.NewRow();
                                drD[0] = index;
                                if (int.TryParse(corte.ToString(), out co))
                                {
                                    drD[1] = co;
                                }
                                else
                                {
                                    CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en el corte.";
                                }
                                if (int.TryParse(semana.ToString(), out se))
                                {
                                    drD[2] = se;
                                }
                                else
                                {
                                    CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en la semana.";
                                }
                                if (int.TryParse(anio.ToString(), out an))
                                {
                                    drD[3] = an;
                                }
                                else
                                {
                                    CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en el año.";
                                }
                                drD[4] = fallo;
                                drD[5] = dtCiclos.Columns[i].ColumnName.Trim();
                                if (R[dtCiclos.Columns[i].ColumnName].ToString().Trim().Equals(""))//se valida si el ciclo tiene libras
                                {
                                    if (!CiclosError.Contains("<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " no se le configuraron libras."))
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " no se le configuraron libras.";
                                }
                                else
                                {
                                    if (decimal.TryParse(R[dtCiclos.Columns[i].ColumnName].ToString().Trim(), System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out d))
                                    {
                                        drD[6] = d.ToString("0.000000000000");//libras
                                    }
                                    else
                                    {
                                        CiclosError += "<br/>El ciclo de la planta " + planta + " invernadero " + invernadero + " de la semana " + semanap + " en el año " + aniop + " tiene el formato incorrecto en las libras.";
                                    }
                                }

                                dtCyclesDetail.Rows.Add(drD);
                                registrosProcesados++;
                            }

                        }
                        else
                        {
                            ciclosSinConfigurar++;
                        }
                    }

                    Dictionary<string, object> prm = new Dictionary<string, object>();
                    prm.Add("@Ciclos", dtCycles);
                    prm.Add("@Cortes", dtCyclesDetail);
                    prm.Add("@semanaUnion", timeCalculator.getActualWeek());
                    prm.Add("@anioUnion", timeCalculator.getYearFromDate(DateTime.Now));
                    prm.Add("@Sobreescribir", Sobreescribir ? 1 : 0);
                    prm.Add("@semanaPartida", semanaPartida);
                    prm.Add("@anioPartida", anioPartida);
                    prm.Add("@idUser", int.Parse(System.Web.HttpContext.Current.Session["userIDInj"].ToString()));

                    if (errorInvernadero)
                    {
                        popUpMessageControl1.setAndShowInfoMessage("Los invernaderos deben anotarse con máximo 4 caracteres", Comun.MESSAGE_TYPE.Success);
                    }
                    else
                    {
                        if (CiclosSinCorte1.Equals(""))
                        {
                            if (CiclosError.Equals(""))
                            {
                                foreach (DataRow row in dtCiclos.Rows)
                                {
                                    if (dtCiclos.Rows.Count != 0)
                                    {
                                        foreach (DataColumn tr in dtCiclos.Columns)
                                        {
                                            Formatos.Add(string.Format("{0}", tr.DataType));
                                        }
                                    }
                                }

                                //divGrid.InnerHtml = ConvertDataTableToHTML(dtCiclos, "<table class=\"grid\" cellspacing=\"0\" rules=\"all\" border=\"1\" id=\"gv_Ciclos\" style=\"border-collapse:collapse;\">", Formatos);

                                DataSet dt = da.executeStoreProcedureDataSet("spr_EjecucionImportar", prm);
                                if (dt.Tables[0].Rows[0][0].ToString().Equals("ok"))
                                {
                                    if (ciclosSinConfigurar == 0)
                                        popUpMessageControl1.setAndShowInfoMessage("La ejecución se cargó exitosamente.<br />Se guardaron " + (index + 1) + " ciclos.", Comun.MESSAGE_TYPE.Success);
                                    else
                                        popUpMessageControl1.setAndShowInfoMessage("La ejecución se cargó exitosamente, sin embargo " + ciclosSinConfigurar + " ciclos fueron omitidos ya que no estaban configurados en la plantilla.", Comun.MESSAGE_TYPE.Success);//Este cas ya no sale porque se omiten en el primer select
                                }
                                else
                                {
                                    switch (dt.Tables[0].Rows[0]["ErrorNumber"].ToString())
                                    {
                                        case "801":
                                            popUpMessageControl1.setAndShowInfoMessage("Existen productos o variedades configuradas en la plantilla que no se encuentran en el sistema.<br/>" + DTtoHTMLstring(dt.Tables[1], ""), Comun.MESSAGE_TYPE.Error); 
                                            break;
                                        case "802":
                                            popUpMessageControl1.setAndShowInfoMessage("Existen invernaderos configurados en la plantilla que no se encuentran en el sistema.<br/>" + DTtoHTMLstring(dt.Tables[1], ""), Comun.MESSAGE_TYPE.Error); 
                                            break;
                                        case "245":
                                            popUpMessageControl1.setAndShowInfoMessage("Error en la conversión de tipo de dato.<br/> " + DTtoHTMLstring(dt.Tables[1], ""), Comun.MESSAGE_TYPE.Error); 
                                            break;
                                        default: 
                                            popUpMessageControl1.setAndShowInfoMessage("No se logró completar la importación: " + dt.Tables[0].Rows[0]["ErrorMessage"].ToString(), Comun.MESSAGE_TYPE.Error); 
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                popUpMessageControl1.setAndShowInfoMessage("La importación no puede continuar: <br /><br />" + CiclosError, Comun.MESSAGE_TYPE.Error);
                            }
                        }
                        else
                        {
                            popUpMessageControl1.setAndShowInfoMessage("Imposible importar la ejecución porque los siguientes ciclos no tienen Corte 1:<br/><br/>" + CiclosSinCorte1, Comun.MESSAGE_TYPE.Error);
                        }
                    }
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage("No existen ciclos con semana y año mayores a los especificados.", Comun.MESSAGE_TYPE.Error);
                }
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage("Revise que las tablas de ciclos y marcas no hayan sido alteradas.", Comun.MESSAGE_TYPE.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            //popUpMessageControl1.setAndShowInfoMessage("No se pudo cargar la ejecución. El último registro procesado es el " + registrosProcesados.ToString(), Comun.MESSAGE_TYPE.Error);
            popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
        }
        finally
        {
            cn.Close();
        }
    }

    protected DataTable CycleTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("CIndex", typeof(int));
        table.Columns.Add("planta", typeof(int));
        table.Columns.Add("invernadero", typeof(string));
        table.Columns.Add("variedad", typeof(int));
        table.Columns.Add("semanap", typeof(int));
        table.Columns.Add("aniop", typeof(int));
        table.Columns.Add("semanapc", typeof(int));
        table.Columns.Add("aniopc", typeof(int));
        table.Columns.Add("interplanting", typeof(int));
        table.Columns.Add("codigoPrefijo", typeof(string));
        table.Columns.Add("suelo", typeof(int));
        return table;
    }

    protected DataTable CycleDetailTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("CIndex", typeof(int));
        table.Columns.Add("corte", typeof(int));
        table.Columns.Add("semana", typeof(int));
        table.Columns.Add("anio", typeof(int));
        table.Columns.Add("fallo", typeof(string));
        table.Columns.Add("tipo", typeof(string));
        table.Columns.Add("monto", typeof(decimal));
        return table;
    }

    public static string DTtoHTMLstring(DataTable dt, string format)
    {
        string tipo = "";
        string idProducto = "";
        String Html = "<table class=\"gridView\" cellspacing=\"0\" border=\"1\" style=\"width:100%;border-collapse:collapse;\" rules=\"all\" />";

        if (dt.Columns.Contains("tipo"))
        {
            tipo = dt.Rows[0]["tipo"].ToString();
            idProducto = dt.Rows[0]["idProducto"].ToString();
            Html = "<h3>" + tipo + (idProducto.Equals("") ? "" : " - " + idProducto) + "</h3>" + Html;
        }

        Html += "<table class=\"gridView2\"><tr>";
        foreach (DataColumn C in dt.Columns)
        {
            if (!(C.ColumnName.ToLower().Equals("idproducto") || C.ColumnName.ToLower().Equals("grupo") || C.ColumnName.ToLower().Equals("tipo")))
                Html += "<th>" + C.ColumnName + "</th>";
        }
        Html += "</tr>";
        foreach (DataRow R in dt.Rows)
        {
            Html += "<tr>";
            foreach (DataColumn C in dt.Columns)
            {
                if (format.Equals("SAC"))
                {
                    if (R[C.ColumnName].ToString().ToLower().Equals("idsemana"))
                        Html += "<td>Semana</td>";
                    else
                        Html += "<td>" + R[C.ColumnName].ToString() + "</td>";
                }
                else
                {
                    if (tipo.ToLower().Equals("ps"))
                    {
                        switch (C.ColumnName.ToLower())
                        {
                            case "tipo": break;
                            case "idproducto": break;
                            case "semana": Html += "<td>" + R[C.ColumnName].ToString().Split('.')[0] + "</td>"; break;
                            case "grupo": break;
                            case "idsemana": Html += "<td>" + R[C.ColumnName].ToString().Split('.')[0] + "</td>"; break;
                            default:
                                Html += "<td>" + (decimal.Parse(R[C.ColumnName].ToString())).ToString().Split('.')[0] + "</td>"; break;

                        }
                    }
                    else
                    {
                        switch (C.ColumnName.ToLower())
                        {
                            case "tipo": break;
                            case "idproducto": break;
                            case "semana": Html += "<td>" + R[C.ColumnName].ToString().Split('.')[0] + "</td>"; break;
                            case "grupo": break;
                            case "idsemana": Html += "<td>" + R[C.ColumnName].ToString().Split('.')[0] + "</td>"; break;
                            default:
                                //if (C.ColumnName.ToString().ToLower().Contains("corte"))
                                //{
                                Html += "<td>" + R[C.ColumnName].ToString() + " </td>"; break;
                            //}
                            //else
                            //  Html += "<td>" + R[C.ColumnName].ToString().Split('.')[0] + format + "</td>"; break;
                        }
                    }

                }
            }
            Html += "</tr>";
        }
        Html += "</table>";
        return Html.ToString();
    }

    [WebMethod(EnableSession = true)]
    private static string ConvertDataTableToHTML(DataTable dt, string headerTable, List<string> Formato)
    {
        //HttpContext.Current.Session["toExcel"] = dt;
        string html = headerTable;
        Regex regex = new Regex("^[0-9]*$");
        //add header row
        html += "<thead>";
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
                html += string.Format("<td class='{1}'>{0}</td>", (Formato[j].ToString().Split('.')[1] == "DateTime" ? DateTime.Parse(dt.Rows[i][j].ToString()).ToString("yyyy-dd-MM") : dt.Rows[i][j].ToString()), (Formato[j].ToString().Split('.')[1] == "Int32" ? "Int32" : "String"));
            html += "</tr>";
        }
        html += "</tbody>";
        html += "</table>";
        HttpContext.Current.Session["toExcel"] = html;
        return html;
    }

}