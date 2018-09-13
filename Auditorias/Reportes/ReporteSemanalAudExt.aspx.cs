using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Auditorias_Reportes_ReporteSemanalAudExt : BasePage
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
        Dictionary<string, object> parametersP = new System.Collections.Generic.Dictionary<string, object>();
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
        dic.Add("@idModulo", 8);
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

        DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_reporteAuditoriasExternas", dic);
        DataTable dtUUID = ds.Tables[0];
        DataTable dt = ds.Tables[1];
        DataTable dtDetails = ds.Tables[2];

        StringBuilder html = new StringBuilder();

        html.Append("<input type='button' id='btnPrint' value='Imprimir Reporte' style='margin: 0px auto;'/>");
        html.Append("<table id='tablaReporte' runat='server' style = 'width: 100%; height: auto;'>");

        if (dtUUID.Rows.Count > 0)
        {
            int pGral = Convert.ToInt32(dtUUID.Rows[0]["PromGral"].ToString());

            html.Append("<tr><th class='centro' colspan='5'>" + dt.Rows[0]["Planta"].ToString() + "</th></tr>");
            html.Append("<tr><th class='centro' colspan='5'>SEMANA " + semana + "</th></tr>");

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                if (pGral > (arregloSemaforos[i].INICIAL - 1) && pGral <= arregloSemaforos[i].FINAL)
                {
                    html.Append("<tr><th class='centro' colspan='5' style='background-color: " + arregloSemaforos[i].COLOR_HEX + ";'>PROMEDIO GENERAL: " + pGral + "</th></tr>");
                }
            }
        }

        html.Append("<tr>");

        foreach (DataColumn columna in dt.Columns)
        {
            if (columna.ColumnName != "Planta")
            {
                html.Append("<th class='centro'>");
                html.Append(columna.ColumnName);
                html.Append("</th>");
            }
        }

        html.Append("</tr>");

        string[] separators = { "," };

        foreach (DataRow row in dt.Rows)
        {
            html.Append("<tr>");

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != "Planta")
                {
                    if (column.ColumnName == "Calif")
                    {
                        double c = Convert.ToDouble(row[column.ColumnName].ToString());

                        for (int i = 0; i < arregloSemaforos.Length; i++)
                        {
                            if (c > (arregloSemaforos[i].INICIAL - 1) && c <= arregloSemaforos[i].FINAL)
                            {
                                arregloSemaforos[i].CONTADOR++;
                                html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + row[column.ColumnName] + "</td>");
                            }
                        }
                    }
                    else
                    {
                        html.Append("<td>");

                        if (column.ColumnName == "Fotos")
                        {
                            html.Append("<div style='width:465px; margin: 0px auto;'>");
                            string[] imgs = row[column.ColumnName].ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string iii in imgs)
                            {
                                html.Append("<img src='http://192.168.167.17:777/rptImgs/auditoriasExternasEvidencias/" + iii + "' style='width: 150px; margin: 2px; float:left;'>");
                                //html.Append("<img src=@'192.168.167.17\\e$\\Auditorias\\auditoriasExternasEvidencias\\" + iii + "' style='width: 150px; margin: 2px; float:left;'>");
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

        html.Append("<table style='margin: 0px auto; width: 50%;'>");
        html.Append("<tr>");
        for (int i = 0; i < arregloSemaforos.Length; i++)
        {
            html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + arregloSemaforos[i].CONTADOR + "</td>");
        }
        html.Append("</tr>");
        html.Append("</table>");

        html.Append("</br></br>");
        html.Append("<div style='width:100%'> <input type='button' id='btnDetails' value='Ver detalles!'/> </div>");


        /*****************************************************/
        //DETALES
        /*****************************************************/

        html.Append("</br></br><div id='details'>");
        html.Append("</br><hr style='width:100%; height:5px; background:#f4d101;'/></br>");

        html.Append("<h2 class='centro'>Detalles de Auditorias Externas</h2>");

        html.Append("</br>");

        DataRow[] det = null;
        int cont = 1;
        foreach (DataRow item in dtUUID.Rows)
        {
            det = dtDetails.Select("vUUID='" + item["vUUID"].ToString() + "'");
            DataTable dataTable = det.CopyToDataTable();

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                arregloSemaforos[i].CONTADOR = 0;
            }

            html.Append("</br></br>");
            html.Append("<h3 class='centro'>Auditoria " + cont + " </h3>");
            html.Append("</br><hr style='width:100%; height:5px; background:#f4d101;'/></br>");

            cont++;

            html.Append("<table id='tablaReporte' runat='server' style = 'width: 100%; height: auto;'>");

            int pAud = Convert.ToInt32(item["PromAud"].ToString());

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                if (pAud > (arregloSemaforos[i].INICIAL - 1) && pAud <= arregloSemaforos[i].FINAL)
                {
                    html.Append("<tr><th class='centro' colspan='5' style='background-color: " + arregloSemaforos[i].COLOR_HEX + ";'>PROMEDIO AUDITORIA: " + pAud + "</th></tr>");
                }
            }

            html.Append("<tr>");

            foreach (DataColumn columna in dataTable.Columns)
            {
                if (columna.ColumnName != "vUUID" && columna.ColumnName != "Planta")
                {
                    html.Append("<th class='centro'>");
                    html.Append(columna.ColumnName);
                    html.Append("</th>");
                }
            }

            html.Append("</tr>");

            //  string[] separators = { "," };

            foreach (DataRow row in dataTable.Rows)
            {
                html.Append("<tr>");

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName != "vUUID" && column.ColumnName != "Planta")
                    {
                        if (column.ColumnName == "Calif")
                        {
                            double c = Convert.ToDouble(row[column.ColumnName].ToString());

                            for (int i = 0; i < arregloSemaforos.Length; i++)
                            {
                                if (c > (arregloSemaforos[i].INICIAL - 1) && c <= arregloSemaforos[i].FINAL)
                                {
                                    arregloSemaforos[i].CONTADOR++;
                                    html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + row[column.ColumnName] + "</td>");
                                }
                            }
                        }
                        else
                        {
                            html.Append("<td>");

                            if (column.ColumnName == "Fotos")
                            {
                                string[] imgs = row[column.ColumnName].ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                html.Append("<div style='width:465px; margin: 0px auto;'>");

                                foreach (string iii in imgs)
                                {
                                    html.Append("<img src='http://192.168.167.17:777/rptImgs/auditoriasExternasEvidencias/" + iii + "' style='width: 150px; margin: 2px; float:left;'>");
                                    //html.Append("<img src='http://192.168.167.130:777/auditoriasExternasEvidencias/" + iii + "' style='width: 150px; margin: 2px; float:left;'>");
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

            html.Append("<tr>");
            html.Append("<td colspan='5' class='centro'>" + item["Observaciones"] + "</td>");
            html.Append("</tr>");

            html.Append("</table>");

            html.Append("<table style='margin: 0px auto; width: 50%;'>");
            html.Append("<tr>");

            for (int i = 0; i < arregloSemaforos.Length; i++)
            {
                html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + arregloSemaforos[i].CONTADOR + "</td>");
            }

            html.Append("</tr>");
            html.Append("</table>");
        }

        html.Append("</div>");
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