using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public class CatalogoListaS_A_G_N
    {
        private int intIdCatalogoListaS_A_G_N = 0;
        private int intIdParametroPorGrupoGrowing = 0;
        private string strDescripcionES = "";
        private string strDescripcionEN = "";
        private bool blnActivo = true;
        private bool blnEsEnEspanol = true;
        private int intErrorNumber = 0;
        private string strErrorMessage = "";

        public int IdCatalogoListaS_A_G_N
        {
            get { return this.intIdCatalogoListaS_A_G_N; }
            set { this.intIdCatalogoListaS_A_G_N = value; }
        }
        public int IdParametroPorGrupoGrowing
        {
            get { return this.intIdParametroPorGrupoGrowing; }
            set { this.intIdParametroPorGrupoGrowing = value; }
        }
        public string DescripcionES
        {
            get { return this.strDescripcionES; }
            set { this.strDescripcionES = value; }
        }
        public string DescripcionEN
        {
            get { return this.strDescripcionEN; }
            set { this.strDescripcionEN = value; }
        }
        public bool Activo
        {
            get { return this.blnActivo; }
            set { this.blnActivo = value; }
        }
        public bool EsEnEspanol
        {
            get { return this.blnEsEnEspanol; }
            set { this.blnEsEnEspanol = value; }
        }
        public int ErrorNumber
        {
            get { return this.intErrorNumber; }
        }

        public string ErrorMessage
        {
            get { return this.strErrorMessage; }
        }


        public DataTable ObtenerCatalogo()
        {
            DataTable functionReturnValue = null;
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerCatalogoListaS_A_G_N_PorParametro", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdParametroPorGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = IdParametroPorGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new System.Data.SqlClient.SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                this.strErrorMessage = daAdapter.SelectCommand.Parameters["@MensajeDeError"].Value.ToString();
                this.intErrorNumber = Convert.ToInt32(daAdapter.SelectCommand.Parameters["@NumeroDeError"].Value.ToString());

                if (intErrorNumber == 0)
                {
                    functionReturnValue = tblTable;
                }
                else
                {
                    functionReturnValue = null;
                }

            }
            catch (SqlException exError)
            {
                functionReturnValue = null;
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.Number;
            }
            finally
            {
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (prmGeneric != null)
                    prmGeneric = null;
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }
                if (cnnMain != null)
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return functionReturnValue;
        }


        public bool MantenimientoCatalogo(int intMovimiento)
        {
            //intMovement Description:
            //1: Insert
            //2: Update
            //3: Delete
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            bool blnResult = false;
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_MantenimientoCatalogoListaS_A_G_N_PorParametro", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdCatalogoListaS_A_G_N_PorParametro", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.intIdCatalogoListaS_A_G_N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdParametroPorGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.intIdParametroPorGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@DescripcionES", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.strDescripcionES;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@DescripcionEN", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.strDescripcionEN;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Activo", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnActivo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new System.Data.SqlClient.SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                this.strErrorMessage = daAdapter.SelectCommand.Parameters["@MensajeDeError"].Value.ToString();
                this.intErrorNumber = Convert.ToInt32(daAdapter.SelectCommand.Parameters["@NumeroDeError"].Value.ToString());

                if (intErrorNumber == 0)
                {
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }

            }
            catch (SqlException exError)
            {
                blnResult = false;
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.Number;
            }
            finally
            {
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (prmGeneric != null)
                    prmGeneric = null;
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }
                if ((cnnMain != null))
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return blnResult;
        }
    }
