using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class controls_ctrlLoteXSiembra : System.Web.UI.UserControl
{

    DataAccess dataaccess = new DataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            Session["LotesTmp"] = "";
        }

        LotesTmp.Value = Session["LotesTmp"].ToString();

        Limpiar();        
    }

    private void Limpiar()
    {
        txtCantidad.Text = String.Empty;
    }

    private void cargaDdls()
    {
        ddlCharola.Items.Clear();
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        DataSet ds = null;
        try
        {
            ds = dataaccess.executeStoreProcedureDataSet("spr_GET_dllsToSiembra", parameters);            
        }
        catch(Exception e){
            popUpMessageControl1.setAndShowInfoMessage("" + e.Message, Comun.MESSAGE_TYPE.Warning);   
        }

        if(ds.Tables[0].Rows.Count > 0 )
        {
            ddlCharola.DataSource = ds.Tables[0];
            ddlCharola.DataTextField = "nombre";
            ddlCharola.DataValueField = "id";            
            ddlCharola.DataBind();            
            ddlCharola.SelectedIndex = 0;
        }
        if(ds.Tables[1].Rows.Count > 0 )
        {
            ddlSustrato.DataSource = ds.Tables[1];
            ddlSustrato.DataTextField = "nombre";
            ddlSustrato.DataValueField = "id";            
            ddlSustrato.DataBind();           
            ddlSustrato.SelectedIndex = 0;
        }
        //if(ds.Tables[2].Rows.Count > 0 )
        //{
        //    ddlClips.DataSource = ds.Tables[2];
        //    ddlClips.DataTextField = "nombre";
        //    ddlClips.DataValueField = "id";
        //    ddlClips.DataBind();
        //    ddlClips.SelectedIndex = 0;
        //}
    }

    public void showPopup(int idSiembra, int iAnio, int iSemana, int cantidad, string variedad)
    {
        Session["idSiembra"] = idSiembra;
        Session["iAnio"] = iAnio;
        Session["iSemana"] = iSemana;
        Session["cantidad"] = cantidad;
        Session["variedad"] = variedad;
        lblSemReq.Text = "" + cantidad;
        ddlLote.Items.Clear();
        mdlPopupMessageGralControl.Show();
        cargaDdls();
        cargaDatos();
    }

    private void cargaDatos()
    {       

        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idSiembra", Session["idSiembra"].ToString());
        parameters.Add("@cantidad", Session["cantidad"].ToString());
        parameters.Add("@idUser", Session["userIDInj"] != null ? Session["userIDInj"].ToString() : "0");
        parameters.Add("@variedad", Session["variedad"].ToString() );
        DataSet dtlotes = null; ;
        try
        {
            dtlotes = dataaccess.executeStoreProcedureDataSet("spr_GET_LoteXSiembra", parameters);
            
        }
        catch (Exception e)
        {
        }

        LotesTmp.Value = string.Empty;
        lblSemAcum.Text = String.Empty;
        lblSemFalt.Text = String.Empty;

        //cargar lotes desde la base de datos
        if (dtlotes.Tables[0].Rows.Count > 0)
        {


            int acum = 0;
            int validador = 0;
            ddlLote.Items.Clear();
            ddlLote.DataSource = dtlotes.Tables[5];
            ddlLote.DataTextField = "lote";
            ddlLote.DataValueField = "idLote";
            ddlLote.DataBind();

            //if (String.IsNullOrEmpty(LotesTmp.Value.ToString()))
            //{

            foreach (DataRow item in dtlotes.Tables[0].Rows)
            {
                if (dtlotes.Tables[0].Rows.IndexOf(item) == dtlotes.Tables[0].Rows.Count - 1)
                {
                    LotesTmp.Value += item["idLote"].ToString() + "|";
                    LotesTmp.Value += item["dGramos"].ToString() + "|";
                    LotesTmp.Value += item["iCantidad"].ToString() + "|";
                    LotesTmp.Value += item["existenSemillas"].ToString() + "|";
                    LotesTmp.Value += item["existenGramos"].ToString() + "|";
                    LotesTmp.Value += item["idReqSiembraDetalle"].ToString() + "|";
                    LotesTmp.Value += item["enviado"].ToString() + "|";
                    LotesTmp.Value += item["semXgram"].ToString();

                    if (Int32.TryParse(item["iCantidad"].ToString(), out validador))
                        acum += Int32.Parse(item["iCantidad"].ToString());
                }
                else
                {
                    LotesTmp.Value += item["idLote"].ToString() + "|";
                    LotesTmp.Value += item["dGramos"].ToString() + "|";
                    LotesTmp.Value += item["iCantidad"].ToString() + "|";
                    LotesTmp.Value += item["existenSemillas"].ToString() + "|";
                    LotesTmp.Value += item["existenGramos"].ToString() + "|";
                    LotesTmp.Value += item["idReqSiembraDetalle"].ToString() + "|";
                    LotesTmp.Value += item["enviado"].ToString() + "|";
                    LotesTmp.Value += item["semXgram"].ToString() + "@";

                    if (Int32.TryParse(item["iCantidad"].ToString(), out validador))
                        acum += Int32.Parse(item["iCantidad"].ToString());
                }

            }
            Session["LotesTmp"] = LotesTmp.Value;

            lblSemAcum.Text = "" + acum;
            if (Int32.TryParse(lblSemReq.Text, out validador))
                lblSemFalt.Text = "" + (Int32.Parse(lblSemReq.Text) - acum);
            //}
        }
       //no hay lotes en almacen
        else 
        {
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("pop1").ToString(), Comun.MESSAGE_TYPE.Warning);
        }
        //cargar lo que ya se usa
        if (dtlotes.Tables[1].Rows.Count > 0)
        {
            //charola
            if (dtlotes.Tables[1].Rows[0]["idCharola"].ToString() == "0")
            {                    
            }
            else
            {
                if (ddlCharola.Items.FindByValue(dtlotes.Tables[1].Rows[0]["idCharola"].ToString()) != null)
                {
                    int index = ddlCharola.Items.IndexOf((ListItem)ddlCharola.Items.FindByValue(dtlotes.Tables[1].Rows[0]["idCharola"].ToString()));
                    ddlCharola.SelectedValue = dtlotes.Tables[1].Rows[0]["idCharola"].ToString();                        
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("pop2").ToString(), Comun.MESSAGE_TYPE.Info);
                    ddlCharola.SelectedIndex = 0;
                }
            }
            //sustrato
            if (dtlotes.Tables[1].Rows[0]["idSustrato"].ToString() == "0")
            {
            }
            else
            {
                if (ddlSustrato.Items.FindByValue(dtlotes.Tables[1].Rows[0]["idSustrato"].ToString()) != null)
                {
                    int index = ddlSustrato.Items.IndexOf((ListItem)ddlSustrato.Items.FindByValue(dtlotes.Tables[1].Rows[0]["idSustrato"].ToString()));
                    ddlSustrato.SelectedValue = dtlotes.Tables[1].Rows[0]["idSustrato"].ToString();
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("pop3").ToString(), Comun.MESSAGE_TYPE.Info);
                    ddlSustrato.SelectedIndex = 0;
                }
            }

            ////clip
            //if (dtlotes.Tables[1].Rows[0]["idClip"].ToString() == "0")
            //{
            //}
            //else
            //{
            //    if (ddlClips.Items.FindByValue(dtlotes.Tables[1].Rows[0]["idClip"].ToString()) != null)
            //    {
            //        int index = ddlClips.Items.IndexOf((ListItem)ddlClips.Items.FindByValue(dtlotes.Tables[1].Rows[0]["idClip"].ToString()));
            //        ddlClips.SelectedValue = dtlotes.Tables[1].Rows[0]["idClip"].ToString();
            //    }
            //    else
            //    {
            //        popUpMessageControl1.setAndShowInfoMessage("El Clip del registro no se encuentra activo en el sistema", Comun.MESSAGE_TYPE.Info);
            //        ddlClips.SelectedIndex = 0;
            //    }
            //}

        }            
        //almacen charolas
        if (dtlotes.Tables[2].Rows.Count > 0)
        {
            //ddlCharolaAlmacen.DataSource = dtlotes.Tables[2];
            //ddlCharolaAlmacen.DataTextField = "nombre";
            //ddlCharolaAlmacen.DataValueField = "id";
            //ddlCharolaAlmacen.DataBind();
            //ddlCharolaAlmacen.SelectedIndex = 0;

            foreach (DataRow item in dtlotes.Tables[2].Rows)
            {
                almacenCharolasTmp.Value += item["existen"].ToString() + "|";
                almacenCharolasTmp.Value += item["necesarias"].ToString() + "|";
                almacenCharolasTmp.Value += item["sustrato"].ToString() + "@";
                
            }

            lblCharolaAlmacen.Text = dtlotes.Tables[2].Rows[0]["existen"].ToString();
            lblNombreCharolaNecesaria.Text = ddlCharola.SelectedItem.Text;
            lblCantCharolaNecesaria.Text = dtlotes.Tables[2].Rows[0]["necesarias"].ToString();
            lblCantSustratoNecesario.Text = dtlotes.Tables[2].Rows[0]["sustrato"].ToString() + " Kg.";
        }

        //almacen sustrato
        if (dtlotes.Tables[3].Rows.Count > 0)
        {
            //ddlSustratoAlmacen.DataSource = dtlotes.Tables[3];
            //ddlSustratoAlmacen.DataTextField = "nombre";
            //ddlSustratoAlmacen.DataValueField = "id";
            //ddlSustratoAlmacen.DataBind();
            //ddlSustratoAlmacen.SelectedIndex = 0;

            foreach (DataRow item in dtlotes.Tables[3].Rows)
                almacenSustratoTmp.Value += item["existen"].ToString() + "|";

            lblSustrato.Text = dtlotes.Tables[3].Rows[0]["existen"].ToString() + " Kg.";
            lblNombreSustratoNecesario.Text = ddlSustrato.SelectedItem.Text;
        }

        ////almacen clip
        //if (dtlotes.Tables[4].Rows.Count > 0)
        //{
        //    ddlClipsAlmacen.DataSource = dtlotes.Tables[4];
        //    ddlClipsAlmacen.DataTextField = "nombre";
        //    ddlClipsAlmacen.DataValueField = "id";
        //    ddlClipsAlmacen.DataBind();
        //    ddlClipsAlmacen.SelectedIndex = 0;

        //    foreach (DataRow item in dtlotes.Tables[4].Rows)
        //    {
        //        almacenClipTmp.Value += item["existen"].ToString() + "|";
        //        almacenClipTmp.Value += item["necesarias"].ToString() + "@";
        //    }

        //    lblClipsAlmacen.Text = dtlotes.Tables[4].Rows[0]["existen"].ToString();
        //    lblNombreClipNecesario.Text = ddlClipsAlmacen.SelectedItem.Text;
        //    lblCantClipNecesario.Text = dtlotes.Tables[4].Rows[0]["necesarias"].ToString();
        //}   
        
    }
            
    protected void cancelar_Click(object sender, EventArgs e)
    {
        actualizaPadre();
    }
      

    //para mandar llamara el metodo del ASPX
    public delegate void LlamarMetodoEnPadre();
    public LlamarMetodoEnPadre metodoPadre { get; set; } 
    public void actualizaPadre() { metodoPadre();  } 

  
}