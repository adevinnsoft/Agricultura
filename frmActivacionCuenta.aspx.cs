using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class frmActivacionCuenta : System.Web.UI.Page
{
    DataAccess data = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
          
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();

            DataTable dt = null;
            dt = validaCodigoActivacion(activationCode);


            if (dt.Rows.Count == 0)
            {
                dt = null;
                ltMessage.Text = "Código de Activación Invalido.";
            }
            else
            {
                ltMessage.Text = "Cuenta Activada.";
            }
                            
                       
                            
               
        }
    }
    private DataTable validaCodigoActivacion(string activationCode)
    {

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@activiationCode", activationCode);

        try
        {
            return data.executeStoreProcedureDataTable("procActivaCuentaUsuario", parameters);
        }
        catch (Exception x)
        {         

            return null;
        }
    }
}