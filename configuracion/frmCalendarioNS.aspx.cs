using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class configuracion_frmCalendarioNS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        { }
        else
        {
            int aniosMaximos = int.Parse(ConfigurationManager.AppSettings["AniosMaximosParaCalendario"].ToString());
            //Cargar Calendario
            for (int i = 0; i < aniosMaximos; i++)
            {
                ddlAnio.Items.Add(new ListItem((DateTime.Now.Year + i).ToString()));
            }
            DataAccess da = new DataAccess();
            var dt = da.executeStoreProcedureDataTable("spr_CalendarioObtenerAnio", new Dictionary<string, object>() {
                {"@anio",DateTime.Now.Year}
            });

            

        }
    }
}