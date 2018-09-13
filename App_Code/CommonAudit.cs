using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;


    public class CommonAudit
    {

        //public enum MESSAGE_TYPE
        //{
        //    Error,
        //    Info,
        //    Warning,
        //    Success
        //}

        #region Methods

        public static string FormatEmails(DataTable dt)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow item in dt.Rows)
            {
                sb.AppendFormat("{0};", item[0].ToString());
            }
            return sb.ToString();
        }


        public static void FillDropDownList(ref DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataValueField = "ID";
            ddl.DataTextField = "Description";
            ddl.DataBind();
            if (ddl.Items.Count > 0)
            {
              
                    ddl.Items.Insert(0, "-- Selecciona uno --");
                    ddl.SelectedIndex = 0;
            }
            dt.Dispose();
        }

        public static void FillDropDownList(ref DropDownList ddl, DataTable dt, Boolean english)
        {
            ddl.DataSource = dt;
            ddl.DataValueField = "ID";
            ddl.DataTextField = "Description";
            ddl.DataBind();
            if (ddl.Items.Count > 0)
            {
                if (english == true)
                {
                    ddl.Items.Insert(0, "-- Choose One --");
                }
                else
                {
                    ddl.Items.Insert(0, "-- Selecciona uno --");
                }

                ddl.SelectedIndex = 0;
            }
            else
            {
                if (english == true)
                {
                    ddl.Items.Insert(0, "-- No existen registros --");
                }
                else
                {
                    ddl.Items.Insert(0, "-- There are no records --");
                }
            
            }
            dt.Dispose();
        }

        public static void FillDropDownListClean(ref DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataValueField = "ID";
            ddl.DataTextField = "Description";
            ddl.DataBind();
            dt.Dispose();
        }

        public static void FillCheckBoxList(ref CheckBoxList cbl, DataTable dt)
        {
            cbl.DataSource = dt;
            cbl.DataValueField = "ID";
            cbl.DataTextField = "Description";
            cbl.DataBind();
            dt.Dispose();
        }
        /// <summary>
        /// Fill by foreach in dt, including columns: ID, Description, Active
        /// </summary>
        /// <param name="cbl">By ref pass de CheckBoxList wanted</param>
        /// <param name="dtItems">columns: ID, Description, Active</param>
        public static void FillCheckBoxListCycle(ref CheckBoxList cbl, DataTable dtItems)
        {
            int itemCount = 0;
            if (dtItems != null && dtItems.Rows.Count > 0)
            {
                foreach (DataRow item in dtItems.Rows)
                {
                    cbl.Items.Add(new ListItem(item["Description"].ToString(), item["ID"].ToString()));
                    cbl.Items[itemCount++].Selected = Convert.ToBoolean(item["Active"]);
                }
            }
            dtItems.Dispose();
        }

        public static void sendEmail(CommonAudit.EMAIL_KEYS emailFormat, float idReport, string planta)
        {
            DataSet ds;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@IdReport", idReport);
            parameters.Add("@Planta", planta);
            parameters.Add("@EmailKey", emailFormat.ToString());
            try
            {
                DataAccess dsAuditoria = new DataAccess();
                ds = dsAuditoria.executeStoreProcedureDataSet("dbo.spr_GET_EmailInformation", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string To = FormatEmails(ds.Tables[1]);
            string fromApp = System.Configuration.ConfigurationManager.AppSettings.Get("emailApp");
            clsEmail objEmail = new clsEmail(emailFormat.ToString(), fromApp, To);
        //20170322 Ricardo Ramos : comentado debido a que en la importación a WMP se indicó que no existe una sobrecarga para este metodo con el parametro elegido.    
        //objEmail.setMessage(ds.Tables[0]);
        try
        {
            //20170322 Ricardo Ramos : comentado debido a que en la importación a WMP se indicó que no existe una sobrecarga para este metodo con el parametro elegido.    
            //objEmail.Send();
        }
        catch (Exception ex)
        { throw ex; }
    }



    public enum EMAIL_KEYS
        {
            NuevoReporte,
            ReporteAprobadoHC,
            ReporteAprobadoNominas
        }
        #endregion
    }
