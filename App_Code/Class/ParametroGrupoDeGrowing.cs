using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

    public class ParametroGrupoDeGrowing
    {

        private int intIdGrupoGrowing = 0;
        private int intIdParametroPorGrupoGrowing = 0;
        private int intIdUsuario = 0;
        private string strNombreES = "";
        private string strNombreEN = "";
        private bool blnAplicaListaDeNA_OK_X = true;
        private bool blnAplicaCatalogoDetalleDeListaDeNA_OK_X = true;
        private bool blnNValoresSeleccionableParaDetalleDeListaDeNA_OK_X = true;
        private bool blnAplicaListaDeS_A_G_N = true;
        private bool blnAplicaCatalogoDetalleDeListaDeS_A_G_N = true;
        private bool blnNValoresSeleccionableParaDetalleDeListaDeS_A_G_N = true;
        private decimal decPuntajeAsignado = 0;
        private bool blnActivo = true;
        private bool blnEsEnEspanol = true;
        private int intErrorNumber = 0;
        private string strErrorMessage = "";

        public int IdGrupoGrowing
        {
            get { return this.intIdGrupoGrowing; }
            set { this.intIdGrupoGrowing = value; }
        }
        public int IdParametroPorGrupoGrowing
        {
            get { return this.intIdParametroPorGrupoGrowing; }
            set { this.intIdParametroPorGrupoGrowing = value; }
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
        public bool NValoresSeleccionableParaDetalleDeListaDeNA_OK_X
        {
            get { return this.blnNValoresSeleccionableParaDetalleDeListaDeNA_OK_X; }
            set { this.blnNValoresSeleccionableParaDetalleDeListaDeNA_OK_X = value; }
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
        public bool NValoresSeleccionableParaDetalleDeListaDeS_A_G_N
        {
            get { return this.blnNValoresSeleccionableParaDetalleDeListaDeS_A_G_N; }
            set { this.blnNValoresSeleccionableParaDetalleDeListaDeS_A_G_N = value; }
        }
        public decimal PuntajeAsignado
        {
            get { return this.decPuntajeAsignado; }
            set { this.decPuntajeAsignado = value; }
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

                cmdQuery = new SqlCommand("spr_ObtenerParametroPorGrupoGrowing", cnnMain);
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

                cmdQuery = new SqlCommand("spr_MantenimientoParametroPorGrupoGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdParametroPorGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdParametroPorGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaListaDeNA_OK_X", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.AplicaListaDeNA_OK_X;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaCatalogoDetalleDeListaDeNA_OK_X", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.AplicaCatalogoDetalleDeListaDeNA_OK_X;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NValoresSeleccionableParaDetalleDeListaDeNA_OK_X", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.NValoresSeleccionableParaDetalleDeListaDeNA_OK_X;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaListaDeS_A_G_N", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.AplicaListaDeS_A_G_N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@AplicaCatalogoDetalleDeListaDeS_A_G_N", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.AplicaCatalogoDetalleDeListaDeS_A_G_N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NValoresSeleccionableParaDetalleDeListaDeS_A_G_N", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.NValoresSeleccionableParaDetalleDeListaDeS_A_G_N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@PuntajeAsignado", System.Data.SqlDbType.Float);
                prmGeneric.Value = this.PuntajeAsignado;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NombreES", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.NombreES;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NombreEN", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.NombreEN;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);
                                                            
                prmGeneric = new System.Data.SqlClient.SqlParameter("@Activo", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.Activo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdUsuarioCaptura", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuario;
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
