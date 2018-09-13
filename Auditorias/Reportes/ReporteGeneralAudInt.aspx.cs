using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_Reportes_ReporteGeneralAudInt : BasePage
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
            //obtenerDatosSemana(txtFecha.Text);

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

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_rpt_generalAuditoriasInternas", dic);

        StringBuilder html = new StringBuilder();

        html.Append("<input type='button' id='btnPrint' value='Imprimir Reporte' style='margin: 0px auto;'/>");
        html.Append("<table id='tablaReporte' runat='server' style = 'width: 100%; height: auto;'>");

        if (dt.Rows.Count > 0)
        {
            int pGral = Convert.ToInt32(dt.Rows[0]["promedio"].ToString());

            html.Append("<tr><th class='centro' colspan='7'>" + dt.Rows[0]["nPlanta"].ToString() + "</th></tr>");
            html.Append("<tr><th class='centro' colspan='7'>SEMANA " + semana + "</th></tr>");

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                if (pGral > (arregloSemaforos[i].INICIAL - 1) && pGral <= arregloSemaforos[i].FINAL)
                {
                    html.Append("<tr><th class='centro' colspan='7' style='background-color: " + arregloSemaforos[i].COLOR_HEX + ";'>PROMEDIO GENERAL: " + pGral + "</th></tr>");
                }
            }
        }

        foreach (DataColumn columna in dt.Columns)
        {
            if (columna.ColumnName != "nPlanta" && columna.ColumnName != "promedio")
            {
                html.Append("<th class='centro'>");
                html.Append(columna.ColumnName);
                html.Append("</th>");
            }
        }

        string[] separators = { "," };

        foreach (DataRow row in dt.Rows)
        {
            html.Append("<tr>");

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != "nPlanta" && column.ColumnName != "promedio")
                {
                    if (column.ColumnName == "ANTERIOR" || column.ColumnName == "ACTUAL")
                    {
                        if (row[column.ColumnName].ToString() == "N/A")
                        {
                            html.Append("<td class='centro' style='font-weight: bold;'>" + row[column.ColumnName] + "</td>");
                        }
                        else
                        {
                            double c = Convert.ToDouble(row[column.ColumnName].ToString());

                            for (int i = 0; i < arregloSemaforos.Length; i++)
                            {
                                if (c > (arregloSemaforos[i].INICIAL - 1) && c <= arregloSemaforos[i].FINAL)
                                {
                                    if (column.ColumnName == "ACTUAL")
                                        arregloSemaforos[i].CONTADOR++;
                                    html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + row[column.ColumnName] + "</td>");
                                }
                            }
                        }
                    }
                    else
                    {
                        html.Append("<td>");

                        if (column.ColumnName == "EVIDENCIAS")
                        {
                            html.Append("<div style='width:465px; margin: 0px auto;'>");
                            string[] imgs = row[column.ColumnName].ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string iii in imgs)
                            {
                                html.Append("<img src='http://192.168.167.17:777/rptImgs/auditoriasInternasEvidencias/" + iii + "' style='width: 150px; margin: 2px; float:left;'>");
                            }

                            html.Append("</div>");
                        }
                        else
                        {
                            html.Append(row[column.ColumnName]);
                        }

                        html.Append("</td>");
                    }
                }
            }

            html.Append("</tr>");
        }

        html.Append("</table>");

        html.Append("</br></br>");

        html.Append("<table style='margin: 0px auto; width: 50%;'>");
        html.Append("<tr>");
        for (int i = 0; i < arregloSemaforos.Length; i++)
        {
            html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + arregloSemaforos[i].CONTADOR + "</td>");
        }
        html.Append("</tr>");
        html.Append("</table>");

        PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
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
}