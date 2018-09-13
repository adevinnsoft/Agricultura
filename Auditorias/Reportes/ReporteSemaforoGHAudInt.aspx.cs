using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Auditorias_Reportes_ReporteSemaforoGHAudInt : BasePage
{
    public Semaforo[] arregloSemaforos;
    DataAccess dataaccess = new DataAccess("dbAuditoria");
    DataAccess dempaque = new DataAccess("EmpaqueConn");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargarPlantas();
        }
        //else
        //    obtenerDatosSemana(txtFecha.Text);

        obtenerSemaforos();
    }

    private void cargarPlantas()
    {
        Dictionary<string, object> parametersP = new Dictionary<string, object>();
        parametersP.Add("@ACTION", 2);
        DataTable dtPlantas = dempaque.executeStoreProcedureDataTable("ev2_spr_get_PackFarms", parametersP);
        CommonReportAudit.FillDropDownList(ref ddlPlant, dtPlantas);
    }

    public void obtenerDatosSemana(string fecha)
    {
        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@fecha", fecha);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("srp_obtenerSemana", dic);
        string[] resultados = new string[dt.Rows.Count];
        string semana = "";

        if (dt.Rows.Count > 0)
        {
            int i = 0;

            foreach (DataRow row in dt.Rows)
            {
                resultados[i] = row["fecha"].ToString();
                semana = row["semana"].ToString();
                i++;
            }
        }

        txtSemana.Text = semana;
        txtFechaInicio.Text = resultados[0];
        txtFechaFin.Text = resultados[resultados.Length - 1];
    }

    public void obtenerSemaforos()
    {
        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@accion", 2);
        dic.Add("@idModulo", 4);
        dic.Add("@idSemaforo", 0);
        dic.Add("@english", 0);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_get_Semaforos", dic);

        arregloSemaforos = new Semaforo[dt.Rows.Count];
        int cont = 0;

        foreach (DataRow row in dt.Rows)
        {
            arregloSemaforos[cont] = new Semaforo();

            arregloSemaforos[cont].ID = Convert.ToInt32(row["idSemaforo"]);
            arregloSemaforos[cont].MODULO = Convert.ToInt32(row["idModulo"]);
            arregloSemaforos[cont].INICIAL = Convert.ToInt32(row["iInicial"]);
            arregloSemaforos[cont].FINAL = Convert.ToInt32(row["iFinal"]);
            arregloSemaforos[cont].COLOR_HEX = row["vColorHex"].ToString();
            arregloSemaforos[cont].CONTADOR = 0;

            cont++;
        }
    }

    public void generarTabla(string planta, string semana)
    {
        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@idPlanta", planta);
        dic.Add("@noSemana", semana);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_rpt_semaforoGHAuditoriasInternas", dic);

        StringBuilder html = new StringBuilder();

        if (dt.Rows.Count > 0)
        {
            //btnPrint.Visible = true;
            html.Append("<input type='button' id='btnPrints' value='Imprimir Reporte' style='margin: 0px auto;'/>");

            html.Append("<table id='tablaReporte' runat='server' style = 'width: 100%; height: auto;'>");
            html.Append("<tr><th class='centro' colspan='8'>" + dt.Rows[0]["nPlanta"].ToString() + "</th></tr>");
            html.Append("<tr><th class='centro' colspan='8'>SEMANA " + semana + "</th></tr>");

            int promGral = Convert.ToInt32(dt.Rows[0]["promedio"].ToString());

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                if (promGral > (arregloSemaforos[i].INICIAL - 1) && promGral <= arregloSemaforos[i].FINAL)
                {
                    html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>PROMEDIO: " + promGral + "</td>");
                }
            }

            html.Append("</table>");
            html.Append("</br>");

            foreach (DataRow row in dt.Rows)
            {
                int calif = Convert.ToInt32(row["CALIF"].ToString());

                for (int i = 0; i < arregloSemaforos.Length; i++)
                {
                    if (calif > (arregloSemaforos[i].INICIAL - 1) && calif <= arregloSemaforos[i].FINAL)
                    {
                        arregloSemaforos[i].CONTADOR++;
                    }
                }
            }

            GHSemaforo[][] arregloGH = new GHSemaforo[arregloSemaforos.Length][];
            int[] contadorGH = new int[arregloSemaforos.Length];

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                arregloGH[i] = new GHSemaforo[arregloSemaforos[i].CONTADOR];
                contadorGH[i] = 0;
            }

            GHSemaforo GH;

            foreach (DataRow row in dt.Rows)
            {
                int calif = Convert.ToInt32(row["CALIF"].ToString());

                for (int i = 0; i < arregloSemaforos.Length; i++)
                {
                    if (calif > (arregloSemaforos[i].INICIAL - 1) && calif <= arregloSemaforos[i].FINAL)
                    {
                        GH = new GHSemaforo();
                        GH.GH = row["GH"].ToString();
                        GH.LIDER = row["LIDER"].ToString();
                        GH.CALIFICACION = calif;

                        arregloGH[i][contadorGH[i]] = GH;
                        contadorGH[i]++;
                    }
                }
            }

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                html.Append("<table style='width: " + (100 / arregloSemaforos.Length) + "%; float: left;'>");
                html.Append("<tr>");
                html.Append("<th class='centro'>GH</th>");
                html.Append("<th class='centro'>LIDER</th>");
                html.Append("<th class='centro'>CALIF</th>");
                html.Append("</tr>");

                for (int j = 0; j < arregloGH[i].Length; j++)
                {
                    html.Append("<tr>");
                    html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + arregloGH[i][j].GH + "</td>");
                    html.Append("<td style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + arregloGH[i][j].LIDER + "</td>");
                    html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + arregloGH[i][j].CALIFICACION + "</td>");
                    html.Append("</tr>");
                }

                html.Append("<tr>");
                html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;' colspan='3'>TOTAL: " + arregloSemaforos[i].CONTADOR + "</td>");
                html.Append("</tr>");

                html.Append("</table>");
            }

        }

        PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

        lblHtm.Text = html.ToString();
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        if (ddlPlant.SelectedIndex != 0 && txtSemana.Text != "")
        {
            generarTabla(ddlPlant.SelectedValue, txtSemana.Text);
            obtenerDatosSemana(txtFecha.Text);
        }
    }

    protected void txtFecha_TextChanged(object sender, EventArgs e)
    {
        obtenerDatosSemana(txtFecha.Text);
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = reportSemaforoGHAudit.xls");
        Response.ContentEncoding = Encoding.UTF8;
        //StringWriter tw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(tw);
        // tbl.RenderControl(hw);
        Response.Write(lblHtm.Text.ToString());
        Response.End();
    }
}