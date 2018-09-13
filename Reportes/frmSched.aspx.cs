using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using log4net;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class report_frmSched : BasePage
{

    private static readonly ILog log = LogManager.GetLogger(typeof(report_frmSched));
    public static string idUsuario = "0";
    private static int maxWeek = 53;
    private static Table reportTable;
    private static DataAccess dataaccess = new DataAccess();
    private decimal[] ttlActivas = new decimal[maxWeek];
    private decimal[] ttlCosecha = new decimal[maxWeek];
    private decimal[] ttlPrecultivo = new decimal[maxWeek];
    private decimal[] ttlPreparación = new decimal[maxWeek];
    private decimal[] ttlPlantación = new decimal[maxWeek];
    private decimal[] ttlOciosas = new decimal[maxWeek];
    private decimal[] ttlTotal = new decimal[maxWeek];
    private decimal[] ttlPorcentaje = new decimal[maxWeek];

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.ImageButton1.Visible = false;
        if (!IsPostBack)
        {
            try
            {
                this.getUnionAEjecucion();
                this.loadFarms();
                this.ObtenerAniosPorPlantaSeleccionada();
                idUsuario = Session["ID"].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
    }


    #region CargaInicial
    public void getUnionAEjecucion()
    {
        Dictionary<string, object> prm = new Dictionary<string, object>();
        DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_getHistorialEjecucion", prm);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            ddlUnion.Items.Add(new ListItem(dr["semana"].ToString().Trim() + " - " + dr["anio"].ToString().Trim(), dr["idSimulacion"].ToString().Trim()));
        }
        ddlUnion.Items.Insert(0, "-- Seleccione --");
    }
    public void loadFarms()
    {
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_SelectAllPlantas", new Dictionary<string, object>() { { "@activo", true }, {"@idUser", idUsuario} });
        ddlPlanta.DataValueField = "Farm";
        ddlPlanta.DataTextField = "Name";
        ddlPlanta.DataSource = dt;
        ddlPlanta.Items.Insert(0, new ListItem("-- Todas --", "0"));
        ddlPlanta.DataBind();
    }
   

    #endregion

    protected void btnGet_Click(object sender, EventArgs e)
    {
        try
        {
            string idSimulacion = "0"; //Valor default ya que en WMP no se leen simulaciones
            DataSet ds = null;
            var idPlanta = ddlPlanta.SelectedValue;
            var semana = ddlUnion.SelectedItem.Text.Split('-')[0].Trim();
            var anio = ddlUnion.SelectedItem.Text.Split('-')[1].Trim();
            var filtro = ddlYear.SelectedValue;
            if (idPlanta.Equals("0"))
            {
                ds = dataaccess.executeStoreProcedureDataSet("[spr_Sched2Ejecucion_TodasLasPlantas]", new Dictionary<string, object>() {
                        {"@semana", semana},{"@anio", anio},{"@filtro", filtro}, { "@idSimulacion", idSimulacion}
                    });
                printSchedAllPlants(ds, filtro);
            }
            else
            {
                ds = dataaccess.executeStoreProcedureDataSet("[spr_F2_getSchedFromEjecucionSimulacion]", new Dictionary<string, object>() {
                        {"@idPlanta", idPlanta},{"@semana", semana},{"@anio", anio},{"@filtro", filtro}, { "@idSimulacion", idSimulacion}
                    });
                if (ds.Tables.Count >= 2)
                {
                    printSched(ds, filtro, idPlanta);
                }
                else {
                    Literal LX = new Literal();
                    LX.Text = "<h2>No hay infomración disponible para la planta elegida.<h2>";
                    report.Controls.Add(LX);
                }
            }

         
            Literal L = new Literal();
            L.Text = "<div id=\"idSimulacion\" class=\"invisible\">" + idSimulacion + "</div>";
            report.Controls.Add(L);
            ClientScript.RegisterStartupScript(GetType(), "MostrarLibras", "MostrarLibras();", true);

        }
        catch (Exception x)
        {
            popUpMessageControl1.setAndShowInfoMessage("Error en el proceso. No se puede continuar.", Comun.MESSAGE_TYPE.Error);
            log.Error(x);
        }
    }
    
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        clearScreen();
    }
   
    protected void clearScreen()
    {
        report.InnerHtml = "";
        ddlUnion.SelectedIndex = 0;
        ddlYear.Items.Clear();
    }
   
    protected void ddlPlanta_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtenerAniosPorPlantaSeleccionada();
    }
   
    protected void ObtenerAniosPorPlantaSeleccionada()
    {
        try
        {
                string semana = ddlUnion.SelectedItem.Text.Split('-')[0].Trim();
                string anio = ddlUnion.SelectedItem.Text.Split('-')[1].Trim();
                string idSimulacion = "0";
                string idPlanta = ddlPlanta.SelectedValue;
                DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_AniosDeSimulacionEjecucion", new Dictionary<string, object>() {
                            {"@semana",semana},{"@anio",anio},{"@idSimulacion",idSimulacion},{"@idPlanta",idPlanta}
                        });
                ddlYear.Items.Clear();
                int anioInicial = 0, anioFinal = 0;
                int.TryParse(ds.Tables[0].Rows[0]["anioMinimo"].ToString(), out anioInicial);
                int.TryParse(ds.Tables[0].Rows[0]["anioMaximo"].ToString(), out anioFinal);
                if (anioInicial != 0 && anioFinal != 0)
                    for (int i = anioInicial; i <= anioFinal; i++)
                    {
                        ddlYear.Items.Add(i.ToString());
                    }
        }
        catch (Exception x)
        {
            popUpMessageControl1.setAndShowInfoMessage("Un error provocó que no se cargaran los años de la simulacion", Comun.MESSAGE_TYPE.Error);
            log.Error(x);
        }
    }

    protected void printSched(DataSet ds, string anio, string idPlanta)
    {
        reportTable = new Table();
        reportTable.Attributes.Add("idPlanta", idPlanta);
        reportTable.CssClass = "planDeProduccion";
        var SemanasDelAnio = ds.Tables[0].Rows.Count;
        reportTable.CssClass = "tablaReporte";
        reportTable.CellPadding = 0;
        reportTable.CellSpacing = 0;
        foreach (DataColumn C in ds.Tables[0].Columns)
        {
            //TableRow tr = new TableRow();
            TableHeaderRow tr = new TableHeaderRow();
            tr.TableSection = TableRowSection.TableHeader;
            TableHeaderCell c = new TableHeaderCell();
            c.Text = C.ColumnName;
            c.ColumnSpan = 3;
            tr.Cells.Add(c);
            switch (C.ColumnName.Trim())
            {
                case "Normal + Interplanting": c.CssClass = "ni"; c.Style.Value = "color: green;background-color: gray;text-align: center;"; break;
                case "Normal + Interplanting_": c.Text = "Normal + Interplanting"; c.Style.Value = "color: blue;background: gray;text-align: center;"; c.CssClass = "ni2"; break;
                default: c.CssClass = "title1"; break;
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                c = new TableHeaderCell();

                tr.Cells.Add(c);
                switch (C.ColumnName.Trim())
                {
                    case "Normal + Interplanting": c.CssClass = "title1"; c.Text = decimal.Parse(r[C.ColumnName].ToString()).ToString("#,##0.00"); break;
                    case "Normal + Interplanting_": c.CssClass = "title1"; c.Text = decimal.Parse(r[C.ColumnName].ToString()).ToString("#,##0.00"); break;
                    default: c.CssClass = "greenCell"; c.Style.Value = "color: white;background-color: rgb(35, 141, 35);border-color: black;border-width: 1px;border-style: solid;white-space: nowrap;text-align: center;"; c.Text = r[C.ColumnName].ToString(); break;
                }
            }
            reportTable.Rows.Add(tr);
        }
        reportTable.Rows.Add(rowVacio(SemanasDelAnio, TableRowSection.TableHeader));
        reportTable.Rows.Add(rowVacio(SemanasDelAnio));


        //DataView view = new DataView(ds.Tables[1]);
        //DataTable invernaderos = view.ToTable(true, "clave", "superficie","idInvernadero");
        DataTable invernaderos = dataaccess.executeStoreProcedureDataTable("spr_Schedule_InvernaderosDeLaPlantaEnElAnio", new Dictionary<string, object>() { 
            {"@idPlanta",idPlanta},
            {"@filtro",anio}
        });

        foreach (DataRow inv in invernaderos.Rows)
        {
            string invClave = inv["clave"].ToString();
            decimal invSuperficie = decimal.Parse(inv["superficie"].ToString().Trim().Equals(string.Empty)? "0" : inv["superficie"].ToString());
            string idInvernadero = inv["idInvernadero"].ToString(); 

            TableRow rwNormal = new TableRow();
            rwNormal.Cells.Add(cell("&nbsp;" + invClave + "<div class=\"idInvernadero invisible\">" + idInvernadero + "</div>", string.Empty, 0));
            rwNormal.Cells.Add(cell(invSuperficie.ToString("#,##0.00"), string.Empty, 0));
            rwNormal.Cells.Add(cell("1", string.Empty, 0));
            TableRow rwInterp = new TableRow();
            rwInterp.Cells.Add(cell("&nbsp;" + invClave + "<div class=\"idInvernadero invisible\">" + idInvernadero + "</div>", string.Empty, 0));
            rwInterp.Cells.Add(cell(invSuperficie.ToString("#,##0.00"), string.Empty, 0));
            rwInterp.Cells.Add(cell("2", string.Empty, 0));
            for (int week = 1; week <= SemanasDelAnio; week++)
            {
                string claseNormal = string.Empty;
                string idCiclo = string.Empty;
                DataRow[] cycNormal = ds.Tables[1].Select("clave='" + invClave + "' AND interplanting=0 AND semana=" + week +" and anio ="+ anio);
                if (cycNormal.Length > 0)
                {
                    rwNormal.Cells.Add(cell(cycNormal[0], false));
                }
                else
                {
                    rwNormal.Cells.Add(cell(string.Empty, "normal", week.ToString(), anio, false, idInvernadero));
                }
                DataRow[] cycInterp = ds.Tables[1].Select("clave='" + invClave + "' AND interplanting=1 AND semana=" + week + " and anio =" + anio);
                if (cycInterp.Length > 0)
                {
                    rwInterp.Cells.Add(cell(cycInterp[0], true));
                }
                else
                {
                    rwInterp.Cells.Add(cell(string.Empty, "normal", week.ToString(), anio, true, idInvernadero));
                }
            }

            reportTable.Rows.Add(rwNormal);
            reportTable.Rows.Add(rwInterp);
        }

        report.Controls.Add(reportTable);
    }
    protected void printSchedAllPlants(DataSet ds, string anio)
    {
        int plantasTotales = ds.Tables.Count;


        for (int i = 0; i < plantasTotales; i = i + 3)
        {
            DataTable tblTitulo = ds.Tables[i];
            DataTable tblCabeza = ds.Tables[i + 1];
            DataTable tblCuerpo = ds.Tables[i + 2];

            Literal L = new Literal();
            L.Text = "<h2>" + tblTitulo.Rows[0]["Planta"].ToString() + "</h2>";
            report.Controls.Add(L);

            reportTable = new Table();
            reportTable.CssClass = "schedule";
            string idPlanta = tblTitulo.Rows[0]["idPlanta"].ToString();
            reportTable.Attributes.Add("idPlanta", idPlanta);
            reportTable.Attributes.Add("idSimulacion", tblTitulo.Rows[0]["idSimulacion"].ToString());
            var SemanasDelAnio = tblCabeza.Rows.Count;
            reportTable.CssClass = "tablaReporte";
            reportTable.CellPadding = 0;
            reportTable.CellSpacing = 0;
            foreach (DataColumn C in tblCabeza.Columns)
            {
                //TableRow tr = new TableRow();
                TableHeaderRow tr = new TableHeaderRow();
                tr.TableSection = TableRowSection.TableHeader;
                TableHeaderCell c = new TableHeaderCell();
                c.Text = C.ColumnName;
                c.ColumnSpan = 3;
                tr.Cells.Add(c);
                switch (C.ColumnName.Trim())
                {
                    case "Normal + Interplanting": c.CssClass = "ni"; c.Style.Value = "color: green;background-color: gray;text-align: center;"; break;
                    case "Normal + Interplanting_": c.Text = "Normal + Interplanting"; c.Style.Value = "color: blue;background: gray;text-align: center;"; c.CssClass = "ni2"; break;
                    default: c.CssClass = "title1"; break;
                }

                foreach (DataRow r in tblCabeza.Rows)
                {
                    c = new TableHeaderCell();

                    tr.Cells.Add(c);
                    switch (C.ColumnName.Trim())
                    {
                        case "Normal + Interplanting": c.CssClass = "title1"; c.Text = decimal.Parse(r[C.ColumnName].ToString()).ToString("#,##0.00"); break;
                        case "Normal + Interplanting_": c.CssClass = "title1"; c.Text = decimal.Parse(r[C.ColumnName].ToString()).ToString("#,##0.00"); break;
                        default: c.CssClass = "greenCell"; c.Style.Value = "color: white;background-color: rgb(35, 141, 35);border-color: black;border-width: 1px;border-style: solid;white-space: nowrap;text-align: center;"; c.Text = r[C.ColumnName].ToString(); break;
                    }
                }
                reportTable.Rows.Add(tr);
            }
            reportTable.Rows.Add(rowVacio(SemanasDelAnio, TableRowSection.TableHeader));
            reportTable.Rows.Add(rowVacio(SemanasDelAnio));


            //DataView view = new DataView(tblCuerpo);
            //DataTable invernaderos = view.ToTable(true, "clave", "superficie", "idInvernadero");
            DataTable invernaderos = dataaccess.executeStoreProcedureDataTable("spr_Schedule_InvernaderosDeLaPlantaEnElAnio", new Dictionary<string, object>() { 
                {"@idPlanta",idPlanta},
                {"@filtro",anio}
            });
            
            foreach (DataRow inv in invernaderos.Rows)
            {
                string invClave = inv["clave"].ToString();
                string idInvernadero = inv["idInvernadero"].ToString();
                decimal invSuperficie = decimal.Parse(inv["superficie"].ToString().Trim().Equals(string.Empty) ? "0" : inv["superficie"].ToString());
                TableRow rwNormal = new TableRow();
                rwNormal.Cells.Add(cell("&nbsp;" + invClave + "<div class=\"idInvernadero invisible\">" + idInvernadero + "</div>", string.Empty, 0));
                rwNormal.Cells.Add(cell(invSuperficie.ToString("#,##0.00"), string.Empty, 0));
                rwNormal.Cells.Add(cell("1", string.Empty, 0));
                TableRow rwInterp = new TableRow();
                rwInterp.Cells.Add(cell("&nbsp;" + invClave + "<div class=\"idInvernadero invisible\">" + idInvernadero + "</div>", string.Empty, 0));
                rwInterp.Cells.Add(cell(invSuperficie.ToString("#,##0.00"), string.Empty, 0));
                rwInterp.Cells.Add(cell("2", string.Empty, 0));
                for (int week = 1; week <= SemanasDelAnio; week++)
                {
                    string semana = week.ToString();
                    
                    string claseNormal = string.Empty;
                    string idCiclo = string.Empty;
                    DataRow[] cycNormal = tblCuerpo.Select("clave='" + invClave + "' AND interplanting=0 AND semana=" + week + " and anio =" + anio);
                    if (cycNormal.Length > 0)
                    {
                        rwNormal.Cells.Add(cell(cycNormal[0], false));
                    }
                    else
                    {
                        rwNormal.Cells.Add(cell(string.Empty, "normal", week.ToString(), anio, false, idInvernadero));
                    }
                    DataRow[] cycInterp = tblCuerpo.Select("clave='" + invClave + "' AND interplanting=1 AND semana=" + week + " and anio =" + anio);
                    if (cycInterp.Length > 0)
                    {
                        rwInterp.Cells.Add(cell(cycInterp[0], true));
                    }
                    else
                    {
                        rwInterp.Cells.Add(cell(string.Empty, "normal", week.ToString(), anio, true, idInvernadero));
                    }
                }
                reportTable.Rows.Add(rwNormal);
                reportTable.Rows.Add(rwInterp);
            }
            report.Controls.Add(reportTable);
        }

    }

    private TableCell cell(DataRow dataRow, bool interplanting)
    {
        string clase = dataRow["clase"].ToString();
        string idPlanta = dataRow["idPlanta"].ToString();
        string _virtual = dataRow["virtual"].ToString();
        string idInvernadero = dataRow["idInvernadero"].ToString();
        string anio = dataRow["anio"].ToString();
        string semana = dataRow["semana"].ToString();
        string idVariedad = dataRow["idVariedad"].ToString();
        string codigoPrefijo = dataRow["codigoPrefijo"].ToString();
        string codigo = dataRow["codigo"].ToString();
        string superficie = dataRow["superficie"].ToString();
        string division = dataRow["division"].ToString();
        string variable = dataRow["variable"].ToString();
        string nombreVariedad = dataRow["nombreVariedad"].ToString();
        string clave = dataRow["clave"].ToString();
        string idProducto = dataRow["idProducto"].ToString();
        string codigoProducto = dataRow["codigoProducto"].ToString();
        string idCiclo = dataRow["idCiclo"].ToString();
        string librasBrutas = dataRow["librasBrutas"].ToString().Trim().Equals(string.Empty) ? "0" : float.Parse(dataRow["librasBrutas"].ToString()).ToString("#,##0.##");

        TableCell c = new TableCell();
        c.CssClass=clase;
        c.Attributes.Add("semana", semana);
        c.Attributes.Add("anio", anio);
        c.Attributes.Add("interplanting", interplanting.ToString());
        c.Attributes.Add("idInvernadero", idInvernadero);
        c.Attributes.Add("idCiclo", idCiclo);
       
        //c.Attributes.Add("onclick", "abrirDialogoDeOpciones($(this));");
        
        switch (clase)
        {
            case "suelo": c.Text = codigo;
                c.Attributes.Add("codigo", codigo);
                break;
            case "plantacion": c.Text = codigoPrefijo + codigo;
                c.Attributes.Add("codigo", codigoPrefijo + codigo);
                break;
            case "precosecha": c.Text = codigoPrefijo + codigo;
                c.Attributes.Add("codigo", codigoPrefijo + codigo);
                break;
            case "corte": c.Text = codigoPrefijo + int.Parse(codigo).ToString("00");
                c.Attributes.Add("librasBrutas", librasBrutas);
                c.Attributes.Add("codigo", c.Text = codigoPrefijo + int.Parse(codigo).ToString("00"));
                break;
            case "falla": c.Text = codigo;
                c.Attributes.Add("codigo", codigo);
                break;
            case "cambio": c.Text = "CAMBIO";
                c.Attributes.Add("codigo", "CAMBIO");
                break;
            default: c.Text = string.Empty; break;
        }

        return c;
    }
    private TableCell cell(string texto, string clase, string semana, string anio, bool interplanting, string idInvernadero) {
        TableCell c = new TableCell();
        c.CssClass = clase;
        c.Attributes.Add("semana", semana);
        c.Attributes.Add("anio", anio);
        c.Attributes.Add("interplanting", interplanting.ToString());
        c.Attributes.Add("idInvernadero", idInvernadero);
        c.Text = texto;
        //c.Attributes.Add("onclick", "abrirDialogoDeOpciones($(this));");
        return c;
    }
    private TableCell cell(string text, string clase, int colspan)
    {
        TableCell c = new TableCell();
        if (!text.Equals(string.Empty))
            c.Text = text;
        /*Se eliminó el inline-style ya que se genero archivo CSS*/
        if (!clase.Equals(string.Empty))
        {
            c.CssClass = clase;
        }
        if (colspan > 0)
            c.ColumnSpan = colspan;
        return c;
    }
    private TableCell cell(string text, string clase, int colspan, Dictionary<string, string> atributos)
    {
        TableCell c = new TableCell();
        if (!text.Equals(string.Empty))
            c.Text = text;
        /*Se eliminó el inline-style ya que se genero archivo CSS*/
        if (!clase.Equals(string.Empty))
        {
            c.CssClass = clase;
        }
        if (colspan > 0)
            c.ColumnSpan = colspan;
        foreach (String key in atributos.Keys)
        {
            var value = string.Empty;
            atributos.TryGetValue(key, out value);
            c.Attributes.Add(key, value);
        }

        return c;
    }
    private TableCell cell(string text, string clase, int colspan, string semana, string anio, bool interplanting, string idPlanta, string idCiclo)
    {
        TableCell c = new TableCell();
        if (!text.Equals(string.Empty))
            c.Text = text;
        /*Se eliminó el inline-style ya que se genero archivo CSS*/
        if (!clase.Equals(string.Empty))
        {
            c.CssClass = clase;
            c.Attributes.Add("idCiclo", idCiclo);     
        }
        if (colspan > 0)
            c.ColumnSpan = colspan;
        c.Attributes.Add("semana", semana);
        c.Attributes.Add("anio", anio);
        c.Attributes.Add("interplanting", interplanting ? "1": "0");
        c.Attributes.Add("idPlanta", idPlanta);
        return c;
    }
    private TableRow rowVacio(int SemanasDelAnio)
    {
        TableRow tr = new TableRow();
        for (int i = 0; i < SemanasDelAnio + 3; i++)
        {
            TableCell tc = new TableCell();
            tc.Text = "&nbsp;";
            tr.Cells.Add(tc);
        }
        return tr;
    }
    private TableRow rowVacio(int SemanasDelAnio, TableRowSection tableRowSection)
    {
        TableRow tr = new TableRow();
        tr.TableSection = tableRowSection;
        for (int i = 0; i < SemanasDelAnio + 3; i++)
        {
            TableCell tc = new TableCell();
            tc.Text = "&nbsp;";
            tr.Cells.Add(tc);
        }
        return tr;
    }
    private TableRow separador(int semanasDelAnio, string titulo)
    {
        TableRow r = new TableRow();
        TableCell title = new TableCell();
        title.Text = titulo;
        title.ColumnSpan = 3;
        title.CssClass = "separador";
        r.Cells.Add(title);
        TableCell colspan = new TableCell();
        colspan.ColumnSpan = semanasDelAnio;
        colspan.CssClass = "separador";
        r.Cells.Add(colspan);
        return r;
    }

    protected void getXLS(object sender, ImageClickEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        Page page = new Page();
        HtmlForm form = new HtmlForm();
        // Deshabilitar la validación de eventos, sólo asp.net 2
        page.EnableEventValidation = false;

        // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
        page.DesignerInitialize();

        page.Controls.Add(form);
        form.Controls.Add(reportTable == null ? new Table() : reportTable);

        page.RenderControl(htw);

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=data.xls");
        Response.Charset = "UTF-8";
        Response.ContentEncoding = Encoding.Default;
        Response.Write(sb.ToString());
        Response.End();
    }
   
   

   
   

}