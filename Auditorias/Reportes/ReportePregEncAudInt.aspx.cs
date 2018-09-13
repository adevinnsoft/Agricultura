using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auditorias_Reportes_ReportePregEncAudInt : System.Web.UI.Page
{

    public Nivel[] arregloNiveles;
    StringBuilder html;
    DataAccess dataaccess = new DataAccess("dbAuditoria");
    DataAccess dempaque = new DataAccess("EmpaqueConn");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargarPlantas();
        }
        else
        {
            //obtenerDatosSemana(txtFecha.Text);
        }

        obtenerNiveles();
    }

    private void cargarPlantas()
    {
        Dictionary<string, object> parametersP = new Dictionary<string, object>();
        parametersP.Add("@ACTION", 2);
        DataTable dtPlantas =dempaque.executeStoreProcedureDataTable("ev2_spr_get_PackFarms", parametersP);
        CommonReportAudit.FillDropDownList(ref ddlPlant, dtPlantas);
    }

    public void obtenerDatosSemana(string fecha)
    {
        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@fecha", fecha);

        DataTable dt =dataaccess.executeStoreProcedureDataTable("srp_obtenerSemana", dic);
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

    public void obtenerNiveles()
    {
        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@accion", 1);
        dic.Add("@idNivel", 0);

        DataTable dt =dataaccess.executeStoreProcedureDataTable("spr_obtenerNiveles", dic);

        arregloNiveles = new Nivel[dt.Rows.Count];
        int cont = 0;

        foreach (DataRow row in dt.Rows)
        {
            arregloNiveles[cont] = new Nivel();

            arregloNiveles[cont].ID = Convert.ToInt32(row["idLevel"]);
            arregloNiveles[cont].NOMBRE = row["vLevelName"].ToString();
            arregloNiveles[cont].VALOR = Convert.ToInt32(row["iLevelValue"]);
            arregloNiveles[cont].COLOR_HEX = row["vHexColor"].ToString();

            cont++;
        }
    }

    public void generarTabla(string idPta, string nPta, string semana, string fecha)
    {
        String[] fechaSplit = fecha.Split('-');

        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@idPlanta", idPta);
        dic.Add("@anio", fechaSplit[0]);
        dic.Add("@noSemana", semana);

        DataSet ds =dataaccess.executeStoreProcedureDataSet("spr_rpt_pregEncAudIntGH", dic);

        if (ds.Tables.Count > 0)
        {

            btnPrint.Visible = true;

            DataTable dtPregGH = ds.Tables[0];
            DataTable dtAvgEnc = ds.Tables[1];
            DataTable dtComEnc = ds.Tables[2];

            html = new StringBuilder();

            int columnaEnc = 2;
            int columnaEncBD = 3;
            int totalNiveles = arregloNiveles.Length;
            int columnasPivote = (dtPregGH.Columns.Count - columnaEncBD);
            int columnasNiveles = (columnasPivote * totalNiveles);
            int totalColumnas = (columnasNiveles + columnaEnc);

            string[] nombreColumnas = new string[dtPregGH.Columns.Count];
            int contColum = 0;

            foreach (DataColumn columna in dtPregGH.Columns)
            {
                nombreColumnas[contColum] = columna.ColumnName;
                contColum++;
            }

            if (dtPregGH.Rows.Count > 0)
            {
                html.Append("<table id='tablaReporte' runat='server' style = 'width: 100%; height: auto;'>");
                html.Append("<tr><th class='centro' colspan='" + totalColumnas + "'>PLANTA: " + nPta.ToUpper() + "</th></tr>");
                html.Append("<tr><th class='centro' colspan='" + totalColumnas + "'>SEMANA: " + semana + "</th></tr>");

                int encuesta = 0;
                int contador = 0;
                int contPreg = 1;

                foreach (DataRow row in dtPregGH.Rows)
                {
                    if (Convert.ToInt32(row["idEncuesta"].ToString()) == encuesta)
                    {
                        html.Append("<tr>");
                        html.Append("<td class='centro'>" + row["idEncuesta"].ToString() + "." + contPreg + "</td>");
                        html.Append("<td style='min-width: 350px'>" + row["nPregunta"].ToString() + "</td>");

                        int indexColumna = 0;

                        foreach (DataColumn columna in dtPregGH.Columns)
                        {
                            if (indexColumna >= columnaEncBD)
                            {
                                string calNiv = row[columna.ColumnName].ToString();

                                Char delimiter = '*';
                                String[] arregloCalNiv = calNiv.Split(delimiter);

                                if (arregloCalNiv.Length > 1)
                                {
                                    for (int i = 0; i < totalNiveles; i++)
                                    {
                                        if (Convert.ToInt32(arregloCalNiv[1]) == arregloNiveles[i].ID)
                                        {
                                            html.Append("<td class='centro' style='background-color: " + arregloNiveles[i].COLOR_HEX + "'><b>" + arregloCalNiv[0] + "</b></td>");
                                        }
                                        else
                                        {
                                            html.Append("<td></td>");
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < totalNiveles; i++)
                                    {
                                        html.Append("<td></td>");
                                    }
                                }
                            }

                            indexColumna++;
                        }

                        html.Append("</tr>");

                        contPreg++;
                    }
                    else
                    {
                        encuesta = Convert.ToInt32(row["idEncuesta"].ToString());
                        contPreg = 1;

                        if (contador == 0)
                        {
                            html.Append("<tr>");
                            html.Append("<td colspan='" + columnaEnc + "'> </td>");

                            for (int i = 0; i < columnasPivote; i++)
                            {
                                html.Append("<td colspan='" + totalNiveles + "' class='centro'><b>" + nombreColumnas[i + columnaEncBD] + "</b></td>");
                            }

                            html.Append("</tr>");
                            html.Append("<tr>");
                            html.Append("<td colspan='" + columnaEnc + "'> </td>");

                            int n = 1;

                            for (int i = 0; i < columnasNiveles; i++)
                            {
                                html.Append("<td class='centro'><b>N" + n + "</b></td>");

                                if (n == totalNiveles)
                                    n = 1;
                                else
                                    n++;
                            }

                            html.Append("</tr>");
                        }

                        html.Append("<tr>");
                        html.Append("<td class='centro' style='background-color: #A4A4A4; color: #FFFFFF;'><b>" + row["idEncuesta"].ToString() + "</b></td>");
                        html.Append("<td class='centro' style='min-width: 350px; background-color: #A4A4A4; color: #FFFFFF;'><b>" + row["nEncuesta"].ToString() + "</b></td>");

                        for (int i = 0; i < columnasPivote; i++)
                        {
                            html.Append("<td colspan='" + totalNiveles + "' class='centro' style='background-color: #A4A4A4; color: #FFFFFF;'><b>" + dtAvgEnc.Rows[contador][nombreColumnas[i + columnaEncBD]].ToString() + "%</b></td>");
                        }

                        html.Append("</tr>");
                        html.Append("<tr>");
                        html.Append("<td class='centro'>" + row["idEncuesta"].ToString() + "." + contPreg + "</td>");
                        html.Append("<td style='min-width: 350px'>" + row["nPregunta"].ToString() + "</td>");

                        int indexColumna = 0;

                        foreach (DataColumn columna in dtPregGH.Columns)
                        {
                            if (indexColumna >= columnaEncBD)
                            {
                                string calNiv = row[columna.ColumnName].ToString();

                                Char delimiter = '*';
                                String[] arregloCalNiv = calNiv.Split(delimiter);

                                if (arregloCalNiv.Length > 1)
                                {
                                    for (int i = 0; i < totalNiveles; i++)
                                    {
                                        if (Convert.ToInt32(arregloCalNiv[1]) == arregloNiveles[i].ID)
                                        {
                                            html.Append("<td class='centro' style='background-color: " + arregloNiveles[i].COLOR_HEX + "'><b>" + arregloCalNiv[0] + "</b></td>");
                                        }
                                        else
                                        {
                                            html.Append("<td></td>");
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < totalNiveles; i++)
                                    {
                                        html.Append("<td></td>");
                                    }
                                }
                            }

                            indexColumna++;
                        }

                        html.Append("</tr>");

                        contPreg++;
                        contador++;
                    }
                }

                foreach (DataRow row in dtComEnc.Rows)
                {
                    html.Append("<tr>");
                    html.Append("<td colspan='2' class='centro' style='background-color: #A4A4A4; color: #FFFFFF; min-width: 350px;'><b>Comentarios</b></td>");

                    string strCom;

                    for (int i = 0; i < columnasPivote; i++)
                    {
                        strCom = row[nombreColumnas[i + columnaEncBD]].ToString();
                        html.Append("<td colspan='" + totalNiveles + "'>" + strCom + "</td>");
                    }

                    html.Append("</tr>");
                }

                html.Append("</table>");
                html.Append("</br>");
            }

            /* html.Append("</hr>");
             html.Append("<h3 class='centro'> Observaciones </h3 >");

             if (dtPregGH.Rows.Count > 0) {
                 html.Append("<table id='tablaReporte' runat='server' style = 'width: 100%; height: auto;'>");
                 html.Append("<tr><th class='centro' colspan='" + (columnasPivote + columnaEnc) + "'>PLANTA: " + nPta.ToUpper() + "</th></tr>");
                 html.Append("<tr><th class='centro' colspan='" + (columnasPivote + columnaEnc) + "'>SEMANA: " + semana + "</th></tr>");

                 int contador = 0;

                 foreach (DataRow row in dtComEnc.Rows) {
                     if (contador == 0) {
                         html.Append("<tr>");
                         html.Append("<td colspan='" + columnaEnc + "'> </td>");

                         for (int i = 0; i < columnasPivote; i++) {
                             html.Append("<td class='centro'><b>" + nombreColumnas[i + columnaEncBD] + "</b></td>");
                         }

                         html.Append("</tr>");
                     }

                     html.Append("<tr>");
                     html.Append("<td class='centro'><b>" + row["idEncuesta"].ToString() + "</b></td>");
                     html.Append("<td style='min-width: 350px'><b>" + row["nEncuesta"].ToString() + "</b></td>");

                     string strCom;

                     for (int i = 0; i < columnasPivote; i++) {
                         strCom = row[nombreColumnas[i + columnaEncBD]].ToString();

                         //if (strCom.Length == 0)
                             html.Append("<td>" + strCom + "</td>");
                         //else
                             //html.Append("<td style='min-width: 350px'>" + row[nombreColumnas[i + columnaEncBD]].ToString() + "</td>");
                     }

                     html.Append("</tr>");

                     contador++;
                 }
                    
                 html.Append("</table>");
                 html.Append("</br>");
             }*/

            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

            lblHtm.Text = html.ToString().Replace((idPta + "E"), ("'" + idPta + "E"));
        }
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        if (ddlPlant.SelectedIndex != 0 && txtSemana.Text != "")
        {
            generarTabla(ddlPlant.SelectedValue, ddlPlant.SelectedItem.ToString(), txtSemana.Text, txtFecha.Text);
            obtenerDatosSemana(txtFecha.Text);
        }
    }

    protected void txtFecha_TextChanged(object sender, EventArgs e)
    {
        obtenerDatosSemana(txtFecha.Text);
    }

    protected void btnPint_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = reportWeekAudit.xls");
        Response.ContentEncoding = Encoding.UTF8;
        //StringWriter tw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(tw);
        // tbl.RenderControl(hw);
        Response.Write(lblHtm.Text.ToString());
        Response.End();
    }
}