using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AutorizarColmenas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //se necesita asignar una masterpage que no requiera session //borrar estas variables
            //Session["Locale"] = "es-MX";
            //Session["usernameInj"] = "Fenologia";

            if (Request.QueryString["I"] == null || Request.QueryString["A"] == null)
            {
                lblMensaje.Text = "La URL no es válida.";
            }
            else
            {
                int idSolicitud = int.Parse(Request.QueryString["I"].ToString());
                bool autorizado = Request.QueryString["A"].ToString().Equals("1");
                DataTable dt = new DataAccess().executeStoreProcedureDataTable("spr_ColmenasAutorizacion", new Dictionary<string, object>() { 
                    {"@idSolicitud", idSolicitud },
                    {"@autorizado", autorizado}
                    //TODO: Insertar quien autorizó
                });
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Estado"].ToString().Equals("1"))
                        lblMensaje.Text = "Gracias, su respuesta fue almacenada.";
                    else
                        lblMensaje.Text = "Error al guardar la autorización.";
                }
                else
                {
                    lblMensaje.Text = "No se obtuvo respuesta del servidor de Base de Datos, intente nuevamente más tarde.";
                }
            }
        }
        catch (Exception)
        {
            lblMensaje.Text = "La ruta ingresada no esta ligada a un requerimiento.";
        }
        
    }
}