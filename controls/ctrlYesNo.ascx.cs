using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;



public partial class controls_ctrlYesNo : System.Web.UI.UserControl
{
   

    private static readonly ILog Log = LogManager.GetLogger(typeof(BasePage));

    protected void Page_Load(object sender, EventArgs e)
    {

    }



    public void showPopup(int idPrograma)
    {     
                //ddlRazones.DataSource = ds;
                //ddlRazones.DataBind();
                mdlPopupMessageGralControl.Show();
            
    }

    protected void save_OnClick(object sender, EventArgs e)
    {

        //string si_no = "si";
        //("~/frmAlmacenCharola.aspx", si_no);






        //var parameters = new Dictionary<string, object>();
        //parameters.Add("@idPrograma", ViewState["_idPrograma"].ToString());
        //parameters.Add("@user", Session["usernameCalidad"].ToString());

        //try
        //{
        //    int guardo = dataaccess.executeStoreProcedureGetInt("spr_CANCEL_ProgramaHeader", parameters, this.Session["connection"].ToString());
        //    if (guardo == 1)
        //    {
        //        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ProgramaCancelado").ToString(), Comun.MESSAGE_TYPE.Success);

        //    }
        //    else if (guardo == 2)
        //        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ProgramaEnUso").ToString(), Comun.MESSAGE_TYPE.Warning);
        //    else if (guardo == 3)
        //        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ProgramaGuadrado").ToString(), Comun.MESSAGE_TYPE.Warning);
        //}
        //catch (Exception ex)
        //{
        //    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("error").ToString(), Comun.MESSAGE_TYPE.Warning);
        //    Log.Error(ex);
        //}
    }

    protected void cancelar2_OnClick(object sender, EventArgs e)
    {
        ViewState["_idPrograma"] = 0;
    }
}