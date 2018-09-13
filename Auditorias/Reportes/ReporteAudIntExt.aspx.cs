using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;



public partial class Auditorias_Reportes_ReporteAudIntExt : BasePage
{

    public Semaforo[] arregloSemaforos;
    public StringBuilder html;
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

    public void generarTabla(string idPta, string nPta, string semana, string fecha)
    {
        String[] fechaSplit = fecha.Split('-');

        Dictionary<string, Object> dic = new Dictionary<string, Object>();
        dic.Add("@idPlanta", idPta);
        dic.Add("@anio", fechaSplit[0]);
        dic.Add("@noSemana", semana);

        DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_rpt_gralAudInExt", dic);

        if (ds.Tables.Count > 0)
        {
            btnPrint.Visible = true;

            DataTable dtReporte = ds.Tables[0];

            html = new StringBuilder();

            if (dtReporte.Rows.Count > 0)
            {
                int totalFilas = dtReporte.Rows.Count;
                int totalColumnas = dtReporte.Columns.Count - 1;
                int contador = 0;
                int calif = 0;

                html.Append("<table id='tablaReporte' runat='server' style = 'width: 90%; height: auto; margin: 0px auto;'>");
                html.Append("<tr><th class='centro' colspan='" + totalColumnas + "'>SEMANA: " + semana + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th class='centro' style='background-color: #CCCCCC; color: #FFFFFF'><b>PLANTA</b></th>");
                html.Append("<th class='centro' style='background-color: #CCCCCC; color: #FFFFFF'><b>INVERNADERO</b></th>");
                html.Append("<th class='centro' style='background-color: #CCCCCC; color: #FFFFFF'><b>FINAL</b></th>");
                html.Append("<th class='centro' style='background-color: #CCCCCC; color: #FFFFFF'><b>OBSERVACIONES Y COMENTARIOS</b></th>");
                html.Append("</tr>");

                foreach (DataRow row in dtReporte.Rows)
                {
                    html.Append("<tr>");

                    if (contador == 0)
                    {
                        html.Append("<td class='centro' rowspan='" + totalFilas + "' style='background-color: #CCCCCC; color: #FFFFFF'><b>" + nPta.ToUpper() + "</b></td>");
                        contador++;
                    }

                    calif = Convert.ToInt32(row["Promedio"].ToString());

                    html.Append("<td class='centro'><b>" + row["Invernadero"].ToString() + "</b></td>");

                    for (int i = 0; i < arregloSemaforos.Length; i++)
                    {
                        if (calif > (arregloSemaforos[i].INICIAL - 1) && calif <= arregloSemaforos[i].FINAL)
                        {
                            html.Append("<td class='centro' style='background-color: " + arregloSemaforos[i].COLOR_HEX + "; font-weight: bold;'>" + calif + "</td>");
                        }
                    }

                    if (row["Observaciones"].ToString() == "")
                        html.Append("<td>" + row["Comentarios"].ToString() + "</td>");
                    else
                        html.Append("<td>" + row["Observaciones"].ToString() + "; " + row["Comentarios"].ToString() + "</td>");

                    html.Append("</tr>");
                }

                html.Append("</table>");
                html.Append("</br>");
            }

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
        string hora = DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString("D2");
        string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "//xls//";

        string filename = "reportAuditIntExt_" + hora + ".xls";
        File.WriteAllText(path + filename, lblHtm.Text.ToString());

        //creamos nuestra lista de archivos a enviar
        List<string> lstArchivos = new List<string>();
        lstArchivos.Add(path + filename);

        //creamos nuestro objeto de la clase que hicimos
        Mail oMail = new Mail("desarrollo.baltazar@gmail.com", "christopher.baltazar@improntadsi.com.mx", "Hola", "RPT Auditorias", lstArchivos);

        //y enviamos
        if (oMail.enviaMail())
        {
            Console.Write("se envio el mail");
        }
        else
        {
            Console.Write("no se envio el mail: " + oMail.error);
        }

        /*Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = reportAuditIntExt.xls");
        Response.ContentEncoding = Encoding.UTF8;
        //StringWriter tw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(tw);
       // tbl.RenderControl(hw);
        Response.Write(lblHtm.Text.ToString());
        Response.End();*/
    }
}
