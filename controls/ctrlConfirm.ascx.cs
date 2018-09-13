using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_ctrlConfirm : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void setAndShowInfoMessage(string message)
    {
        lblMessageGralControl.Text = message;
        mdlPopupMessageGralControl.Show();        
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        actualizaPadre();
    }

    //para mandar llamara el metodo del ASPX
    public delegate void LlamarMetodoEnPadre();
    public LlamarMetodoEnPadre metodoPadre { get; set; }
    public void actualizaPadre() { metodoPadre( ); } 
}