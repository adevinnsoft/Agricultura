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


public partial class RH_frmCostoVariedadExtraordinario : BasePage
{
    private static string currentFarm;
    private static readonly ILog log = LogManager.GetLogger(typeof(RH_frmCostoVariedadExtraordinario));
    private List<TreeNode> AllCheckedNodes = new List<TreeNode>();
    protected void Page_Load(object sender, EventArgs e)
    {
   
        if (!IsPostBack)
        {

            ObtieneRanchos();
            ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
            ObtieneUnidadMedida();

            BindTreeViewControl(ddlInvernaderos.SelectedValue);
            
            ObtieneCostosActividad();


            Session["idRanchoZonificacion"] = ddlRanchos.SelectedValue;

            Session["idInvernaderoZonificacion"] = ddlInvernaderos.SelectedValue;

        }
    }
    public void TreeViewCheck(DataTable dtSurcos)
    {
        for (int i = 0; i < tvInvernadero.Nodes.Count; i++)
        {
          
            for (int j = 0; j < tvInvernadero.Nodes[i].ChildNodes.Count; j++)
             {                 
                 DataRow[] foundRows;
                 string expression;
                 expression = "idSurco = " + tvInvernadero.Nodes[i].ChildNodes[j].Value;
                 // Use the Select method to find all rows matching the filter.
                 foundRows = dtSurcos.Select(expression);
                 if (foundRows.Length > 0)
                 {
                     tvInvernadero.Nodes[i].ChildNodes[j].Checked = true;
                 }
                   
            }
        }
    }

    public void TreeViewCheckLadoNorte(DataTable dtSurcos)
    {
        for (int i = 0; i < trPares.Nodes.Count; i++)
        {

            for (int j = 0; j < trPares.Nodes[i].ChildNodes.Count; j++)
            {
                DataRow[] foundRows;
                string expression;
                expression = "idSurco = " + trPares.Nodes[i].ChildNodes[j].Value;
                // Use the Select method to find all rows matching the filter.
                foundRows = dtSurcos.Select(expression);
                if (foundRows.Length > 0)
                {
                    trPares.Nodes[i].ChildNodes[j].Checked = true;
                }

            }
        }
    }
    public void ObtieneCostosActividad()
    {
        try
        {

            DataTable dt = dataaccess.executeStoreProcedureDataTableFill("procObtieneCostoPorActividadesExtraordinario", null);
            GvPlantas.DataSource = dt;
            GvPlantas.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
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
                TreeNode root = new TreeNode(Rows[i]["Seccion"].ToString(), Rows[i]["idSeccion"].ToString());
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

            if (ds.Tables.Count >2)
            {
                lblNorte.Text = "NORTE";
                LadoNorte(ds.Tables[2]);
                Session["dtLadoNorte"] = ds.Tables[2];
            }
            else
            {
                lblNorte.Text = "";
            }
        }
        catch (Exception Ex) { throw Ex; }
    }

    public void CreateNode(TreeNode node, DataTable Dt)
    {
        DataRow[] Rows = Dt.Select("idSeccion =" + node.Value + " and idSurco is not null");
        if (Rows.Length == 0) { return; }
        for (int i = 0; i < Rows.Length; i++)
        {
            TreeNode Childnode = new TreeNode("Surco " + Rows[i]["NumeroSurco"].ToString(), Rows[i]["idSurco"].ToString());
            Childnode.SelectAction = TreeNodeSelectAction.Expand;

            node.ChildNodes.Add(Childnode);
            //CreateNode(Childnode, Dt);
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
    public void ObtieneRanchos()
    {
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtenerPlantaPorUsuario", new Dictionary<string, object>() { 
                    {"@idUsuario",Session["idUsuario"].ToString()}});
        ddlRanchos.DataSource = dt;
        ddlRanchos.DataTextField = "nombrePlanta";
        ddlRanchos.DataValueField = "idPlanta";
        ddlRanchos.DataBind();

        ObtieneDepartamentosPorPlanta(ddlRanchos.SelectedValue);
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);

    }
    private void ObtieneUnidadMedida()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("procObtieneUnidadMedidaActivo", null);
            ddlUnidad0.DataSource = dt;
            ddlUnidad0.DataTextField = "Nombre";
            ddlUnidad0.DataValueField = "idUnidadMedida";
            ddlUnidad0.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

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
            ddlActividad0.DataSource = dt;
            ddlActividad0.DataTextField = "NombreHabilidad";
            ddlActividad0.DataValueField = "idHabilidad";
            ddlActividad0.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }
    protected void ddlDepartamento0_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtPlantasSeleccionadas = (DataTable)Session["idPlantasSeleccionadas"];
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);
    }

    protected void ddlPlanta_SelectedIndexChanged1(object sender, EventArgs e)
    {

        ObtieneDepartamentosPorPlanta(ddlRanchos.SelectedValue);
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);
    }

    private void LadoNorte(DataTable dtLadoNorte)
    {
        DataRow[] Rows = dtLadoNorte.Select("idSurco IS NULL"); // Get all parents nodes
        for (int i = 0; i < Rows.Length; i++)
        {
            TreeNode root = new TreeNode(Rows[i]["Seccion"].ToString(), Rows[i]["idSeccion"].ToString());
            root.SelectAction = TreeNodeSelectAction.Expand;
            root.Expand();
            CreateNode(root, dtLadoNorte);
            trPares.Nodes.Add(root);
        }
    }
    protected void ddlInvernaderos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["idInvernaderoZonificacion"] = ddlInvernaderos.SelectedValue;
        tvInvernadero.Nodes.Clear();
        trPares.Nodes.Clear();
        tvInvernadero.DataSource = null;
        trPares.DataSource = null;
        BindTreeViewControl(ddlInvernaderos.SelectedValue);
       
    }
    protected void ddlRanchos_SelectedIndexChanged(object sender, EventArgs e)
    {
        tvInvernadero.Nodes.Clear();
        trPares.Nodes.Clear();
        tvInvernadero.DataSource = null;
        trPares.DataSource = null;
        Session["idRanchoZonificacion"] = ddlRanchos.SelectedValue;
        ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
        Session["idInvernaderoZonificacion"] = ddlInvernaderos.SelectedValue;

        BindTreeViewControl(ddlInvernaderos.SelectedValue);
        ObtieneDepartamentosPorPlanta(ddlRanchos.SelectedValue);
        ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);
        
     
    }
    public void ObtieneNodosSeleccionados()
    {
        for (int i = 0; i < tvInvernadero.CheckedNodes.Count; i++)
        {
            if (tvInvernadero.CheckedNodes[i].Text.Substring(0,3) != "Sec")
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
    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        ObtieneNodosSeleccionados();


        DataTable dtZonificacion = new DataTable();

        dtZonificacion = DataTableCostoZonificado();
        if (AllCheckedNodes.Count == 0)
        {
            popUpMessageControl1.setAndShowInfoMessage("Debe seleccionar al menos UN SURCO", Comun.MESSAGE_TYPE.Warning);
            return;
        }

        for (int i = 0; i <= AllCheckedNodes.Count - 1; i++)
        {
            DataRow dr = dtZonificacion.NewRow();
            dr["idInvernadero"] = ddlInvernaderos.SelectedValue;
            dr["idSurco"] = AllCheckedNodes[i].Value;
            dr["idUsuario"] = Session["idUsuario"].ToString();
            dtZonificacion.Rows.Add(dr);
        }
       
        
        
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@idCostoActividadExtraordinario", hdnIdPlanta.Value);
        parameters.Add("@idPlanta", ddlRanchos.SelectedValue);      
        parameters.Add("@idDepartamento", ddlDepartamento0.SelectedValue);
        parameters.Add("@idHabilidad", ddlActividad0.SelectedValue);
        parameters.Add("@cantidad", txtCantidad0.Text);
        parameters.Add("@idUnidadMedida", ddlUnidad0.SelectedValue);
        parameters.Add("@costo", txtCosto.Text);
        parameters.Add("@idUsuario", Session["idUsuario"].ToString());
        parameters.Add("@activo", chkActivo.Checked);
        parameters.Add("@zonificacion", dtZonificacion);
        parameters.Add("@idInvernadero", ddlInvernaderos.SelectedValue);
        parameters.Add("@fechaInicio", txtFechaInicio.Text);
        parameters.Add("@fechaFin", txtFechaFin.Text);


        DataTable result = dataaccess.executeStoreProcedureDataTable("procGuardaCostoPorActividadExtraordinario", parameters);

        if (Convert.ToInt32(result.Rows[0]["RESULTADO"]) > 0)
        {
            popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["MENSAJE"].ToString(), Comun.MESSAGE_TYPE.Success);

        }
        else
        {
            popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["MENSAJE"].ToString(), Comun.MESSAGE_TYPE.Error);
        }


        ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
        ddlInvernaderos.SelectedValue = ddlInvernaderos.SelectedValue;
        ObtieneUnidadMedida();
        BindTreeViewControl(ddlInvernaderos.SelectedValue);
        ObtieneCostosActividad();
       
    }
    private static DataTable DataTableCostoZonificado()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("idInvernadero");
        dt.Columns.Add("idSurco");
        dt.Columns.Add("idUsuario");
        return dt;
    }
    protected void GvPlantas_PreRender(object sender, EventArgs e)
    {
        if (GvPlantas.HeaderRow != null)
            GvPlantas.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void GvPlantas_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(GvPlantas, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void GvPlantas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != GvPlantas.SelectedPersistedDataKey)
            {
                Int32.TryParse(GvPlantas.SelectedPersistedDataKey["idCostoActividadExtraordinario"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(GvPlantas.SelectedDataKey["idCostoActividadExtraordinario"].ToString(), out id);
            }
            hdnIdPlanta.Value = id.ToString();
            parameters.Add("@idCostoActividadExtraordinario", id);

            ds = dataaccess.executeStoreProcedureDataSet("procObtieneCostoPorActividadExtraordinarioPorIdCosto", parameters);

            ddlRanchos.SelectedValue = ds.Tables[0].Rows[0]["idPlanta"].ToString();
            ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
            ddlInvernaderos.SelectedValue = ds.Tables[0].Rows[0]["idInvernadero"].ToString();
            ObtieneDepartamentosPorPlanta(ddlRanchos.SelectedValue);
            ddlDepartamento0.SelectedValue = ds.Tables[0].Rows[0]["idDepartamento"].ToString();
            ObtieneActividadesPorPlantaDepartamento(ddlRanchos.SelectedValue, ddlDepartamento0.SelectedValue);
            BindTreeViewControl(ddlInvernaderos.SelectedValue);
            ddlActividad0.SelectedValue = ds.Tables[0].Rows[0]["idHabilidad"].ToString();
            txtCantidad0.Text = ds.Tables[0].Rows[0]["cantidad"].ToString();
            txtCosto.Text = ds.Tables[0].Rows[0]["Costo"].ToString();
            ddlUnidad0.SelectedValue = ds.Tables[0].Rows[0]["idUnidadMedida"].ToString();
            chkActivo.Checked = ds.Tables[0].Rows[0]["Activo"].ToString() == "True" ? true : false;
            txtFechaInicio.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fechaInicio"]).ToString("yyyy-MM-dd");
            txtFechaFin.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fechaFin"]).ToString("yyyy-MM-dd");
            
            TreeViewCheck(ds.Tables[1]);
            if (trPares.Nodes.Count > 0)
            {
                TreeViewCheckLadoNorte(ds.Tables[1]);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoCargar").ToString(), Comun.MESSAGE_TYPE.Error);
        }
    }


    public static void SelectNodesRecursive(string searchValue, TreeView Tv)
    {
        foreach (TreeNode tn in Tv.Nodes)
        {
            if (tn.Value == searchValue)
            {
                tn.Expand();
                tn.Select();
                break;
            }

            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode cTn in tn.ChildNodes)
                {
                    int a = SelectChildrenRecursive(cTn, searchValue);
                    if (a == 1)
                    {
                        tn.Expand();
                    }
                }
            }
        }
    }


    private static int SelectChildrenRecursive(TreeNode tn, string searchValue)
    {
        if (tn.Value == searchValue)
        {
            //if(tn.Parent.Parent!=null)
            //{
            // tn.Parent.Parent.Expand();
            //}
            //if (tn.Parent != null)
            //{
            //    tn.Parent.Expand();
            //}

            tn.Expand();
            tn.Select();
            return 1;
        }
        else
        {
            // tn.Parent.Collapse();
            tn.Collapse();
        }
        if (tn.ChildNodes.Count > 0)
        {
            foreach (TreeNode tnC in tn.ChildNodes)
            {
                int a = SelectChildrenRecursive(tnC, searchValue);
                if (a == 1)
                {
                    tn.Expand();
                    return 1;
                }

            }

        }
        return 0;
    } 

  
protected void SelectNodeByValue(TreeNodeCollection Nodes, string ValueToSelect)
{
    foreach (TreeNode n in Nodes)
    {
        if (n.Value == ValueToSelect)
        {
            n.Select();
            return;
        }
        else
        {
            if (n.ChildNodes.Count > 0)
                SelectNodeByValue(n.ChildNodes, ValueToSelect);
        }
        
    }
}
    public void SeleccionaSurcos(DataSet dsInformacion)
    {
        //if (dsInformacion.Tables.Count > 0)
        //{
        //    for (int i = 0; i <= dsInformacion.Tables[1].Rows.Count - 1; i++)
        //    {
                //SelectNodeByValue(tvInvernadero.Nodes, "303182");
        //    }

        //}
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        LimpiaCampos();
    }
    public void LimpiaCampos()
    {
        txtFechaFin.Text = "";
        txtFechaInicio.Text = "";
        txtCantidad0.Text = "";
        txtCosto.Text = "";
        ObtieneRanchos();
        ObtieneInvernaderosPorRancho(ddlRanchos.SelectedValue);
        ObtieneUnidadMedida();
        hdnIdPlanta.Value = "0";
        BindTreeViewControl(ddlInvernaderos.SelectedValue);

        ObtieneCostosActividad();
    }
}