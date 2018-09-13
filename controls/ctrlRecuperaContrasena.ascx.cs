using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;



public partial class controls_ctrlRecuperaContrasena : System.Web.UI.UserControl
{
    DataAccess data = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            txtUsername.Focus();
            txtPassword.Attributes.Add("OnKeyDown", "if(event.wich || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {" +
                                                     " document.getElementById('" + lnkRecuperar.ClientID + "').click();return false;}} else {return true}; ");
        }
    }
    protected void lnkRecuperar_Click(object sender, EventArgs e)
    {
        DataTable dt = null;
        var usuario = txtUsername.Text;
        var contrasena = txtPassword.Text;
        var confirmContrasena = txtConfirmPassword.Text;
        dt = userExistsOnDataBase(usuario);

        if (dt.Rows.Count == 0)
        {
            dt = null;
            lblError.Text = "No se encontró el usuario en la base de dato contacta a SISTEMAS";
        }
        else
        {
          
           SendActivationEmail(dt.Rows[0]["idUsuario"].ToString(),dt.Rows[0]["email"].ToString());
        }
    }
    private void SendActivationEmail(string userId,string email)
    {
        string activationCode = Guid.NewGuid().ToString();

        InsertaActualizaActivacionUsuario(userId, activationCode, txtPassword.Text.Trim());
        
        using (MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["NCusername"], email))
        {
           
            mm.Subject = "Activación de Cuenta";
            string body = "Hola " + txtUsername.Text.Trim() + ",";
            body += "<br /><br />Por favor da click en el siguiente link para activar tu cuenta de usuario, si no activas tu cuenta no podras ingresar al sistema de control de invernaderos";
            body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("frmRecuperaContrasena.aspx", "frmActivacionCuenta.aspx?ActivationCode=" + activationCode) + "'>Da click aqui para activar tu cuenta.</a>";
            body += "<br /><br />Gracias";
            mm.Body = body;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["SMTPClient"];
            smtp.EnableSsl = false;
            NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["NCusername"], ConfigurationManager.AppSettings["NCpassword"]);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
            smtp.Send(mm);
            lblError.Text = "Se envió un correo a la cuenta de correo electrónico que tiene registrada en el sistema de control de invernaderos para activar su cuenta";
        }
    }
    private DataTable userExistsOnDataBase(string userName)
    {

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@user", userName);

        try
        {
            return data.executeStoreProcedureDataTable("dbo.procObtieneUsuarioPorUser", parameters);
        }
        catch (Exception x)
        {
            lblError.Text = x.Message;
           
            return null;
        }
    }
    public void InsertaActualizaActivacionUsuario(string idUsuario, string activationCode,string passwordNew)
    {
        DataTable dt = null;
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("@idUsuario", idUsuario);
        parameters.Add("@activiationCode", activationCode);
        parameters.Add("@passwordNew", passwordNew);

        try
        {
           dt= data.executeStoreProcedureDataTable("procInsertaUsuarioActivarCuenta", parameters);
        }
        catch (Exception x)
        {
            lblError.Text = x.Message;

        }
    }


}