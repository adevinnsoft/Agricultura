using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Web.Services;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Web.Script.Serialization;


public partial class configuracion_frmCargaEmpleados : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmCargaEmpleados));
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_Importar_Click(object sender, EventArgs e)
    {
         var path="";
        if (!Path.GetFileName(fu_Plantilla.PostedFile.FileName).Split('.').Last().ToLower().Equals("xlsx"))
        {
            popUpMessage.setAndShowInfoMessage("No se cargó un archivo o el archivo no tiene el formato correcto debe ser XLSX.", Comun.MESSAGE_TYPE.Error);
            log.Error("No se cargó un archivo o el archivo no tiene el formato correcto.");
        }
        else
        {
            string Destino = string.Empty;
            try
            {
                var fileName = Path.GetFileName(fu_Plantilla.FileName);
                // store the file inside ~/Content/LearnObject-Repository folder
                path= Path.Combine(Server.MapPath("~/EmpleadosData/"), fileName);
                fu_Plantilla.SaveAs(path);
                //Destino = string.Format("{0}{1}_{2}", ConfigurationManager.AppSettings["CarpetaDeTemporales"], Session["idUsuario"], Path.GetFileName(fu_Plantilla.PostedFile.FileName));
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                popUpMessage.setAndShowInfoMessage("No se pudo generar una ruta para almacenar el archivo.", Comun.MESSAGE_TYPE.Error);
            }
            try
            {
                string FileName = Path.GetFileName(fu_Plantilla.PostedFile.FileName);
                string Extension = Path.GetExtension(fu_Plantilla.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                string FilePath = Server.MapPath(FolderPath + FileName);
                fu_Plantilla.SaveAs(FilePath);
     
                Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
    

            }
            
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                popUpMessage.setAndShowInfoMessage(ex.ToString(), Comun.MESSAGE_TYPE.Error);
                //popUpMessage.setAndShowInfoMessage("La información del archivo no pudo ser leida, probablemente se cambió el formato de la plantilla.", Comun.MESSAGE_TYPE.Error);
            }
        }
    }
    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls": //Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]
                         .ConnectionString;
                break;
            case ".xlsx": //Excel 07
                conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]
                          .ConnectionString;
                break;
        }
        conStr = String.Format(conStr, FilePath, isHDR);
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;

        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();

        //Read Data from First Sheet
        connExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        connExcel.Close();

        //Bind Data to GridView
        CargaEmpleadosBaseDatos(dt);
        GridView1.Caption = Path.GetFileName(FilePath);
        GridView1.DataSource = dt;
        
        GridView1.DataBind();
        lblRegistros.Text = dt.Rows.Count.ToString();
    }
    public void CargaEmpleadosBaseDatos(DataTable dtEmpleados)
    {
        try
        {
            string result = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Empleados", dtEmpleados);
            DataSet dsEmpleados = new DataAccess().executeStoreProcedureDataSet("procCargaEmpleados", parameters);

            grvActualizados.DataSource = null;
            grvActualizados.DataBind();
            grvDuplicados.DataSource = null;
            grvDuplicados.DataBind();
            grvEmpleadosInactivos.DataSource = null;
            grvEmpleadosInactivos.DataBind();

            grvEmpleadosNuevos.DataSource = null;
            grvEmpleadosNuevos.DataBind();

            if (dsEmpleados.Tables.Count > 0)
            {
                if (dsEmpleados.Tables[0].Rows.Count > 0)
                {
                    result = dsEmpleados.Tables[0].Rows[0][0].ToString();
                    lblRegistrosSinDuplicado.Text = dsEmpleados.Tables[0].Rows[0][0].ToString();
                }
                if (dsEmpleados.Tables[1].Rows.Count > 0)
                {
                    lblCantDuplicados.Text = dsEmpleados.Tables[1].Rows[0][0].ToString();
                    if (dsEmpleados.Tables[2].Rows.Count > 0)
                    {
                        grvDuplicados.DataSource = dsEmpleados.Tables[2];
                        grvDuplicados.DataBind();
                    }
                }
                if (dsEmpleados.Tables[3].Rows.Count > 0)
                {
                    lblNoActualizados.Text = dsEmpleados.Tables[3].Rows[0][0].ToString();
                    if (dsEmpleados.Tables[4].Rows.Count > 0)
                    {
                        grvActualizados.DataSource = dsEmpleados.Tables[4];
                        grvActualizados.DataBind();
                    }
                }
                if (dsEmpleados.Tables[5].Rows.Count > 0)
                {
                    lblNoNuevos.Text = dsEmpleados.Tables[5].Rows[0][0].ToString();
                    if (Convert.ToInt32(dsEmpleados.Tables[5].Rows[0][0])>0)
                    {
                        if (dsEmpleados.Tables[6].Rows.Count > 0)
                        {
                            grvEmpleadosNuevos.DataSource = dsEmpleados.Tables[6];
                            grvEmpleadosNuevos.DataBind();
                        }
                    }
                   
                }
                if (dsEmpleados.Tables[7].Rows.Count > 0)
                {
                    lblNoInactivos.Text = dsEmpleados.Tables[7].Rows[0][0].ToString();
                    if (dsEmpleados.Tables[8].Rows.Count > 0)
                    {
                        grvEmpleadosInactivos.DataSource = dsEmpleados.Tables[8];
                        grvEmpleadosInactivos.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
            popUpMessage.setAndShowInfoMessage(ex.ToString(), Comun.MESSAGE_TYPE.Error);
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        if (GridView1.HeaderRow != null)
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
 }