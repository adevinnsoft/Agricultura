using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

    public class GrupoDeGrowing
    {

        private int intIdGrupoGrowing = 0;
        private int intIdUsuario = 0;
        private string strNombreES = "";
        private string strNombreEN = "";
        private bool blnAplicaListaDeNA_OK_X = true;
        private bool blnAplicaCatalogoDetalleDeListaDeNA_OK_X = true;
        private bool blnAplicaListaDeS_A_G_N = true;
        private bool blnAplicaCatalogoDetalleDeListaDeS_A_G_N = true;
        private decimal decPuntajeAsignadoParaPlantacion = 0;
        private decimal decPuntajeAsignadoParaNoPlantacion = 0;
        private bool blnValidoParaPlantacion = true;
        private bool blnValidoParaNoPlantacion = true;
        private bool blnActivo = true;
        private bool blnEsEnEspanol = true;
        private int intErrorNumber = 0;
        private string strErrorMessage = "";

        public int IdGrupoGrowing
        {
            get { return this.intIdGrupoGrowing; }
            set { this.intIdGrupoGrowing = value; }
        }
        public int IdUsuario
        {
            get { return this.intIdUsuario; }
            set { this.intIdUsuario = value; }
        }

        public string NombreES
        {
            get { return this.strNombreES; }
            set { this.strNombreES = value; }
        }
        public string NombreEN
        {
            get { return this.strNombreEN; }
            set { this.strNombreEN = value; }
        }
        public bool AplicaListaDeNA_OK_X
        {
            get { return this.blnAplicaListaDeNA_OK_X; }
            set { this.blnAplicaListaDeNA_OK_X = value; }
        }
        public bool AplicaCatalogoDetalleDeListaDeNA_OK_X
        {
            get { return this.blnAplicaCatalogoDetalleDeListaDeNA_OK_X; }
            set { this.blnAplicaCatalogoDetalleDeListaDeNA_OK_X = value; }
        }
        public bool AplicaListaDeS_A_G_N
        {
            get { return this.blnAplicaListaDeS_A_G_N; }
            set { this.blnAplicaListaDeS_A_G_N = value; }
        }
        public bool AplicaCatalogoDetalleDeListaDeS_A_G_N
        {
            get { return this.blnAplicaCatalogoDetalleDeListaDeS_A_G_N; }
            set { this.blnAplicaCatalogoDetalleDeListaDeS_A_G_N = value; }
        }
        public bool ValidoParaPlantacion
        {
            get { return this.blnValidoParaPlantacion; }
            set { this.blnValidoParaPlantacion = value; }
        }
        public bool ValidoParaNoPlantacion
        {
            get { return this.blnValidoParaNoPlantacion; }
            set { this.blnValidoParaNoPlantacion = value; }
        }
        public decimal PuntajeAsignadoParaPlantacion
        {
            get { return this.decPuntajeAsignadoParaPlantacion; }
            set { this.decPuntajeAsignadoParaPlantacion = value; }
        }
        public decimal PuntajeAsignadoParaNoPlantacion
        {
            get { return this.decPuntajeAsignadoParaNoPlantacion; }
            set { this.decPuntajeAsignadoParaNoPlantacion = value; }
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

                cmdQuery = new SqlCommand("spr_ObtenerGrupoGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = IdGrupoGrowing;
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

                cmdQuery = new SqlCommand("spr_MantenimientoGrupoGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.intIdGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NombreEs", System.Data.SqlDbType.VarChar);
                prmGeneric.Value = this.NombreES;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NombreEn", System.Data.SqlDbType.VarChar);
                prmGeneric.Value = this.NombreEN;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Activo", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnActivo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaListaDeNA_OK_X", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnAplicaListaDeNA_OK_X;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaCatalogoDetalleDeListaDeNA_OK_X", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnAplicaCatalogoDetalleDeListaDeNA_OK_X;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaListaDeS_A_G_N", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnAplicaListaDeS_A_G_N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaCatalogoDetalleDeListaDeS_A_G_N", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnAplicaCatalogoDetalleDeListaDeS_A_G_N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@ValidoParaPlantacion", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnValidoParaPlantacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@ValidoParaNoPlantacion", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnValidoParaNoPlantacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@PuntajeAsignadoParaPlantacion", System.Data.SqlDbType.Decimal);
                prmGeneric.Value = this.decPuntajeAsignadoParaPlantacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@PuntajeAsignadoParaNoPlantacion", System.Data.SqlDbType.Decimal);
                prmGeneric.Value = this.decPuntajeAsignadoParaNoPlantacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);
                                
                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.intIdUsuario;
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
