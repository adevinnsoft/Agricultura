using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Xml;


public partial class Auditorias_Reportes_Ejecucion : BasePage
{
    DataAccess dataaccess = new DataAccess("dbAuditoria");

    protected void Page_Load(object sender, EventArgs e)
    {
        string fInicio = Request.QueryString["fInicio"];
        string fFinal = Request.QueryString["fFinal"];

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@fInicio", fInicio);
        parameters.Add("@fFin", fFinal);
        DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_rpt_resumenAnualAudInt", parameters);

        DataTable dtDatos = ds.Tables[0];
        DataTable dtSemanas = ds.Tables[1];
        DataTable dtSemaforos = ds.Tables[2];

        lblDatax.Text = JsonConvert.SerializeObject(dtDatos);
        lblSemanas.Text = dtSemanas.Rows[0]["Semanas"].ToString();
        lblSemaforos.Text = JsonConvert.SerializeObject(dtSemaforos);
    }
}