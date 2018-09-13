using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Services;

public partial class NoAccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.RemoveAll();
    }

    [WebMethod(EnableSession = true)]
    public static string ObtenerHoraUsuario()
    {
        try
        {
            return TimeZone.obtenerHoraDeLaCuenta(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local), TimeZone.obtenerZonaID(Convert.ToInt32(HttpContext.Current.Session["idPlanta"] == null ? 0 : HttpContext.Current.Session["idPlanta"]))).ToString("yyyy-MM-dd HH:mm:ss");
        }
        catch (Exception ex)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
