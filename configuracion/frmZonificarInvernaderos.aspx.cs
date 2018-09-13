using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using log4net;
using System.Text;
using System.Xml;
using System.Web.Script.Serialization;
using System.Windows.Forms;



public partial class configuracion_frmZonificarInvernaderos : BasePage
{
     private static string currentFarm;
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmZonificarInvernaderos));
    private List<System.Web.UI.WebControls.TreeNode> AllCheckedNodes = new List<System.Web.UI.WebControls.TreeNode>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            llenaPlantas();
            ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
            ObtieneEmpleados(ddlRanchos.SelectedValue);
            llevaZonificacionesEmpleados(ddlEmpleados.SelectedValue);
            BindTreeViewControl(ddlInvernaderos.SelectedValue);

            Session["idRanchoZonificacion"] = ddlRanchos.SelectedValue;
            
            Session["idInvernaderoZonificacion"] = ddlInvernaderos.SelectedValue;
            
            Session["idEmpleadoZonificacion"] = ddlEmpleados.SelectedValue;
        }
    }
    public void llevaZonificacionesEmpleados(string idAsociado)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idInvernadero", ddlInvernaderos.SelectedValue);
        parameters.Add("@idAsociado", idAsociado);
        DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneZonificacionesEncabezadoEmpleado", parameters);
        ddlZonificacionesEmpleado.DataSource = result;
        ddlZonificacionesEmpleado.DataTextField = "Nombre";
        ddlZonificacionesEmpleado.DataValueField = "idZonificacionEncabezado";
        ddlZonificacionesEmpleado.DataBind();
        ddlZonificacionesEmpleado.Items.Insert(0,new ListItem("CREAR NUEVA ZONIFICACION","0"));
    }
    private void llenaPlantas()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasDeUsuario", new Dictionary<string, object>() { 
                { "@iduser", Session["idUsuario"] } 
            });
            ddlRanchos.DataSource = dt;
            ddlRanchos.DataTextField = "Planta";
            ddlRanchos.DataValueField = "idPlanta";
            ddlRanchos.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }

        ObtieneDepartamentosPorPlanta(ddlRanchos.SelectedValue);
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);
    }

    private void BindTreeViewControl(string idInvernadero)
    {
        try
        {
            tvInvernadero.Nodes.Clear();
            trPares.Nodes.Clear();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idInvernadero", idInvernadero);
            DataSet ds = dataaccess.executeStoreProcedureDataSet("procObtieneInvernaderoCostoxActividad", parameters);
            DataRow[] Rows = ds.Tables[1].Select("idSurco IS NULL"); // Get all parents nodes
            for (int i = 0; i < Rows.Length; i++)
            {
                System.Web.UI.WebControls.TreeNode root = new System.Web.UI.WebControls.TreeNode(Rows[i]["Seccion"].ToString(), Rows[i]["idSeccion"].ToString());
                root.SelectAction = TreeNodeSelectAction.Expand;
                root.Expand();
                CreateNode(root, ds.Tables[1]);
                tvInvernadero.Nodes.Add(root);
            }
            if (ds.Tables.Count > 0)
            {
                lblInvernadero.Text = ds.Tables[0].Rows[0]["Invernadero"].ToString();
                if (((bool)ds.Tables[0].Rows[0]["pasilloMedio"]))
                {
                    lblPasilloMedio.Text = "SI";
                }
                else
                {
                    lblPasilloMedio.Text = "NO";
                }
            }

            if (ds.Tables.Count > 2)
            {
                lblNorte.Text = "NORTE";
                LadoNorteNew(ds.Tables[2]);
                Session["dtLadoNorte"] = ds.Tables[2];
            }
            else
            {
                lblNorte.Text = "";
            }
        }
        catch (Exception Ex) { throw Ex; }
    }
    private void LadoNorteNew(DataTable dtLadoNorte)
    {
        DataRow[] Rows = dtLadoNorte.Select("idSurco IS NULL"); // Get all parents nodes
        for (int i = 0; i < Rows.Length; i++)
        {
            System.Web.UI.WebControls.TreeNode root = new System.Web.UI.WebControls.TreeNode(Rows[i]["Seccion"].ToString(), Rows[i]["idSeccion"].ToString());
            root.SelectAction = TreeNodeSelectAction.Expand;
            root.Expand();
            CreateNode(root, dtLadoNorte);
            trPares.Nodes.Add(root);
        }
    }
    public void CreateNode(System.Web.UI.WebControls.TreeNode node, DataTable Dt)
    {
        DataRow[] Rows = Dt.Select("idSeccion =" + node.Value + " and idSurco is not null");
        if (Rows.Length == 0) { return; }
        for (int i = 0; i < Rows.Length; i++)
        {
            System.Web.UI.WebControls.TreeNode Childnode = new System.Web.UI.WebControls.TreeNode("Surco " + Rows[i]["NumeroSurco"].ToString() + " " + Rows[i]["Nombre"].ToString() , Rows[i]["idSurco"].ToString());
            Childnode.SelectAction = TreeNodeSelectAction.Expand;

            node.ChildNodes.Add(Childnode);
            //CreateNode(Childnode, Dt);
        }
    }

    private void ObtieneDepartamentosPorPlanta(string idPlanta)
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneDepartamentoHabilidadPorPlanta", new Dictionary<string, object>() { 
                { "@idPlanta", idPlanta } 
            });
            ddlDepartamento0.DataSource = dt;
            ddlDepartamento0.DataTextField = "NombreDepartamento";
            ddlDepartamento0.DataValueField = "idDepartamento";
            ddlDepartamento0.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }
    private void ObtieneActividadesPorPlantaDepartamento(string idPlanta, string idDepartamento)
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneHabilidadPorPlanta", new Dictionary<string, object>() { 
                { "@idPlanta", idPlanta },  { "@idDepartamento", idDepartamento } 
            });
            cbxListActividades.DataSource = dt;
            cbxListActividades.DataTextField = "NombreHabilidad";
            cbxListActividades.DataValueField = "idHabilidad";
            cbxListActividades.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }
  
    public void ObtieneInvernaderosPorRancho(string idPlanta)
    {
        DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneInvernaderosActivosPorRancho", new Dictionary<string, object>() { 
                    {"@idPlanta",idPlanta}});
        ddlInvernaderos.DataSource = dt;
        ddlInvernaderos.DataTextField = "invernadero";
        ddlInvernaderos.DataValueField = "idInvernadero";
        ddlInvernaderos.DataBind();


    }
   
    public void ObtieneEmpleados(string idPlanta)
    {
        DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneEmpleadoPorRancho", new Dictionary<string, object>() { 
                    {"@idPlanta",idPlanta}});
        ddlEmpleados.DataSource = dt;
        ddlEmpleados.DataTextField = "nombre";
        ddlEmpleados.DataValueField = "id_Empleado";
        ddlEmpleados.DataBind();

    }
 
    protected void ddlInvernaderos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["idInvernaderoZonificacion"] = ddlInvernaderos.SelectedValue;
        Session["idEmpleadoZonificacion"] = ddlEmpleados.SelectedValue;
        BindTreeViewControl(ddlInvernaderos.SelectedValue);
        limpiaCheckboxList();
    }
    protected void ddlRanchos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneDepartamentosPorPlanta(ddlRanchos.SelectedValue);
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);

        Session["idRanchoZonificacion"] = ddlRanchos.SelectedValue;
        ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
        Session["idInvernaderoZonificacion"] = ddlInvernaderos.SelectedValue;
        
        ObtieneEmpleados(ddlRanchos.SelectedValue);
        llevaZonificacionesEmpleados(ddlEmpleados.SelectedValue);
        Session["idEmpleadoZonificacion"] = ddlEmpleados.SelectedValue;
        BindTreeViewControl(ddlInvernaderos.SelectedValue);
        limpiaCheckboxList();
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
       
      

        ObtieneNodosSeleccionados();

        DataTable dtZonificacionSurcos = new DataTable();

        dtZonificacionSurcos = DataTableCostoZonificado();

        DataTable dtZonificacionActividades = new DataTable();

        dtZonificacionActividades = DataTableActividadesZonificado();
        

        if (AllCheckedNodes.Count == 0)
        {
            popUpMessageControl1.setAndShowInfoMessage("Debe seleccionar al menos UN SURCO", Comun.MESSAGE_TYPE.Warning);
            return;
        }

        for (int i = 0; i <= AllCheckedNodes.Count - 1; i++)
        {
            DataRow dr = dtZonificacionSurcos.NewRow();
            dr["idInvernadero"] = ddlInvernaderos.SelectedValue;
            dr["idSurco"] = AllCheckedNodes[i].Value;
            dr["idUsuario"] = Session["idUsuario"].ToString();
            dr["idAsociado"] = ddlEmpleados.SelectedValue;
            dr["nombreEmpleado"] = ddlEmpleados.SelectedItem.Text;
            dtZonificacionSurcos.Rows.Add(dr);
        }
        for (int j = 0; j <= cbxListActividades.Items.Count - 1; j++)
        {
            if (cbxListActividades.Items[j].Selected)
            {
                DataRow dr = dtZonificacionActividades.NewRow();
                dr["idActividad"] = cbxListActividades.Items[j].Value;
                dtZonificacionActividades.Rows.Add(dr);
            }
        }

        if (dtZonificacionActividades.Rows.Count==0 )
        {
            popUpMessageControl1.setAndShowInfoMessage("Debe seleccionar al menos UNA ACTIVIDAD", Comun.MESSAGE_TYPE.Warning);
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@zonificacion", dtZonificacionSurcos);
        parameters.Add("@idInvernadero", ddlInvernaderos.SelectedValue);
        parameters.Add("@idAsociado", ddlEmpleados.SelectedValue);
        parameters.Add("@actividades", dtZonificacionActividades);
        parameters.Add("@idZonificacionEncabezado", ddlZonificacionesEmpleado.SelectedValue);
        


        DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaZonficacionInvernadero", parameters);

        if (Convert.ToInt32(result.Rows[0]["RESULTADO"]) > 0)
        {
            popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["MENSAJE"].ToString(), Comun.MESSAGE_TYPE.Success);

        }
        else
        {
            popUpMessageControl1.setAndShowInfoMessage("ERROR AL REGISTRAR LA ZONIFICACIÓN", Comun.MESSAGE_TYPE.Error);
            return;
        }

     
        ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
        ddlInvernaderos.SelectedValue = ddlInvernaderos.SelectedValue;
        ObtieneEmpleados(ddlRanchos.SelectedValue);
        ddlEmpleados.SelectedValue = ddlEmpleados.SelectedValue;
        llevaZonificacionesEmpleados(ddlEmpleados.SelectedValue);
        BindTreeViewControl(ddlInvernaderos.SelectedValue);
    }
    private static DataTable DataTableCostoZonificado()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idSurco");
        dt.Columns.Add("idUsuario");
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("nombreEmpleado");
        return dt;
    }
    private static DataTable DataTableActividadesZonificado()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idActividad");;
        return dt;
    }

    public void ObtieneNodosSeleccionados()
    {
        for (int i = 0; i < tvInvernadero.CheckedNodes.Count; i++)
        {
            if (tvInvernadero.CheckedNodes[i].Text.Substring(0, 3) != "Sec")
            {
                AllCheckedNodes.Add(tvInvernadero.CheckedNodes[i]);
            }

        }
        for (int i = 0; i < trPares.CheckedNodes.Count; i++)
        {
            if (trPares.CheckedNodes[i].Text.Substring(0, 3) != "Sec")
            {
                AllCheckedNodes.Add(trPares.CheckedNodes[i]);
            }

        }

    }
    private static DataTable DataTableEmpleadosZonificado()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idAsociado");
        dt.Columns.Add("nombreAsociado");
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idSeccion");
        dt.Columns.Add("idSurco");
        dt.Columns.Add("idUsuario");

        return dt;
    }
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        //popUpMessageControl1.setAndShowInfoMessage("Esta seguro de ELIMINAR la zonificación de todo el INVERNADERO", Comun.MESSAGE_TYPE.YesNo);
        DialogResult result = MessageBox.Show("Esta seguro de ELIMINAR la zonificación de todo el INVERNADERO","Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (result == DialogResult.Yes)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idInvernadero", ddlInvernaderos.SelectedValue);

            DataTable dtEliminado = dataaccess.executeStoreProcedureDataTable("procBorraZonificacionInvernaderoPorInvernadero", parameters);
            popUpMessageControl1.setAndShowInfoMessage("Se eliminó correctamente la zonificación del invernadero " + ddlInvernaderos.SelectedItem.Text , Comun.MESSAGE_TYPE.Success);

            ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
            ddlInvernaderos.SelectedValue = ddlInvernaderos.SelectedValue;
            ObtieneEmpleados(ddlRanchos.SelectedValue);
            ddlEmpleados.SelectedValue = ddlEmpleados.SelectedValue;
            BindTreeViewControl(ddlInvernaderos.SelectedValue);
        }
    
    }
    protected void btnEliminarZonEmpleado_Click(object sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Esta seguro de ELIMINAR la zonificación del EMPLEADO seleccionado", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (result == DialogResult.Yes)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idInvernadero", ddlInvernaderos.SelectedValue);
            parameters.Add("@idAsociado", ddlEmpleados.SelectedValue);
            parameters.Add("@idZonificacionEncabezado", ddlZonificacionesEmpleado.SelectedValue);


            DataTable dtEliminado = dataaccess.executeStoreProcedureDataTable("procBorraZonificacionInvernaderoPorTrabajador", parameters);
            popUpMessageControl1.setAndShowInfoMessage("Se eliminó correctamente la zonificación del empleado " + ddlEmpleados.SelectedItem.Text, Comun.MESSAGE_TYPE.Success);

            ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
            ddlInvernaderos.SelectedValue = ddlInvernaderos.SelectedValue;
            ObtieneEmpleados(ddlRanchos.SelectedValue);
            ddlEmpleados.SelectedValue = ddlEmpleados.SelectedValue;
            BindTreeViewControl(ddlInvernaderos.SelectedValue);
         
        }
    }
    protected void ddlDepartamento0_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);
    }
    protected void ddlEmpleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        llevaZonificacionesEmpleados(ddlEmpleados.SelectedValue);
        limpiaCheckboxList();
    }
    public void limpiaCheckboxList()
    {
        for (int index = 0; index < cbxListActividades.Items.Count; ++index)
        {
            cbxListActividades.Items[index].Selected = false;
        }
    }
    protected void ddlZonificacionesEmpleado_SelectedIndexChanged(object sender, EventArgs e)
    {
        limpiaCheckboxList();
        if (ddlZonificacionesEmpleado.SelectedValue != "0")
        {
            BindTreeViewControlPorZonificacionEncabezadoID(ddlInvernaderos.SelectedValue, ddlZonificacionesEmpleado.SelectedValue);
        }
    }
    private void BindTreeViewControlPorZonificacionEncabezadoID(string idInvernadero, string idZonificacionEncabezado)
    {
        try
        {
            tvInvernadero.Nodes.Clear();
            trPares.Nodes.Clear();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idInvernadero", idInvernadero);
            parameters.Add("@idZonificacionEncabezado", idZonificacionEncabezado);
            DataSet ds = dataaccess.executeStoreProcedureDataSet("procObtieneInvernaderosZonificadoEmpleado", parameters);
            DataRow[] Rows = ds.Tables[1].Select("idSurco IS NULL"); // Get all parents nodes
            for (int i = 0; i < Rows.Length; i++)
            {
                System.Web.UI.WebControls.TreeNode root = new System.Web.UI.WebControls.TreeNode(Rows[i]["Seccion"].ToString(), Rows[i]["idSeccion"].ToString());
                root.SelectAction = TreeNodeSelectAction.Expand;
                root.Expand();
                CreateNode(root, ds.Tables[1]);
                tvInvernadero.Nodes.Add(root);
            }
            if (ds.Tables.Count > 0)
            {
                lblInvernadero.Text = ds.Tables[0].Rows[0]["Invernadero"].ToString();
                if (((bool)ds.Tables[0].Rows[0]["pasilloMedio"]))
                {
                    lblPasilloMedio.Text = "SI";
                }
                else
                {
                    lblPasilloMedio.Text = "NO";
                }
            }

            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows[0][0].ToString() == "0")
                {
                   
                    for(int j=0; j <= ds.Tables[2].Rows.Count-1;j++){
                        for (int i = 0; i <= cbxListActividades.Items.Count - 1; i++)
                        {
                            if (ds.Tables[2].Rows[j]["idHabilidad"].ToString() == cbxListActividades.Items[i].Value)
                            {
                                cbxListActividades.Items[i].Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    lblNorte.Text = "NORTE";
                    LadoNorteNew(ds.Tables[2]);
                    Session["dtLadoNorte"] = ds.Tables[2];

                    for (int j = 0; j <= ds.Tables[3].Rows.Count - 1; j++)
                    {
                        for (int i = 0; i <= cbxListActividades.Items.Count - 1; i++)
                        {
                            if (ds.Tables[3].Rows[j]["idHabilidad"].ToString() == cbxListActividades.Items[i].Value)
                            {
                                cbxListActividades.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
            else
            {
                lblNorte.Text = "";
            }
        }
        catch (Exception Ex) { throw Ex; }
    }
}