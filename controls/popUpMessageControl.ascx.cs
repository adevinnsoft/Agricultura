using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_popUpControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        mdlPopupMessageGralControl.Hide();
    }

    public void setAndShowInfoMessage(string message, Comun.MESSAGE_TYPE type)
    {
        string imageScr = "error";
        switch (type)
        {
            case Comun.MESSAGE_TYPE.Error:
                imageScr = "error";
                break;
            case Comun.MESSAGE_TYPE.Info:
                imageScr = "info";
                break;
            case Comun.MESSAGE_TYPE.Warning:
                imageScr = "warning";
                break;
            case Comun.MESSAGE_TYPE.Success:
                imageScr = "ok";
                break;
            case Comun.MESSAGE_TYPE.YesNo:
                imageScr = "warning";
                break;
            default:
                imageScr = "error";
                break;
        }

        imgMessageGralControl.Src = string.Format("../comun/img/{0}.png",imageScr);
        lblMessageGralControl.Text = message;
        mdlPopupMessageGralControl.Show();
    }
    protected void btnOKMessageGralControl_Click(object sender, EventArgs e)
    {

    }
}
