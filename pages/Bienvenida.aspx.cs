using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

public partial class pages_Bienvenida : BasePage
{
    string exception;
    public String idEmpleado = "";
    public String sIdEmpleado = "";
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            sIdEmpleado = Session["idEmpleado"].ToString();
        }
        catch (Exception x)
        {

            Log.Error(x);
        }
        
    }
}