using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using System.Threading;


public partial class frmLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (null != Session["Locale"])
        {
            //ddlLocale.SelectedValue = (string)Session["Locale"];
        }
    }
    protected override void InitializeCulture()
    {
        if (false)//Request.Form["ddlLocale"] != null)
        {
            //prueba de cambio 
            Session["Locale"] = Request.Form["ddlLocale"];
            //for UI elements
            UICulture = Request.Form["ddlLocale"];

            //for region specific formatting
            Culture = Request.Form["ddlLocale"];
        }
        else
        {
            if (null != Session["Locale"])
            {
                UICulture = (string)Session["Locale"];
                Culture = (string)Session["Locale"];
            }
            else
            {
                Session["Locale"] = CultureInfo.CurrentCulture.Name;
                UICulture = (string)Session["Locale"];
                Culture = (string)Session["Locale"];
            }
        }
        base.InitializeCulture();
    }

}