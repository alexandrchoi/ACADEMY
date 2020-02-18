using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Data.OracleClient;
//
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.BIZ.FX.BIZBASE
{
    /// <summary>
    /// 1. 작업개요 : DAC Class
    /// 
    /// 2. 주요기능 : 공통 DB 처리
    ///				  
    /// 3. Class 명 : DAC
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          DAC.cs
    ///        * Comment 
    ///          public int Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제
    ///          => DBAlias로 DB 개체생성 후 처리에 Transaction 처리 불가로 삭제처리. 해당 사용 모듈 전체 변경완료함.
    /// </summary>

    public class DAC
    {
        public DAC()
        { }

        public OracleConnection CreateConnection(string DBAlias)
        {
            Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
            OracleConnection cnn = database.CreateConnection() as OracleConnection;
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            return cnn;
        }

        public object CheckNull(object paramValue)
        {

            if (paramValue == null || paramValue.ToString() == "")
            {
                paramValue = System.DBNull.Value;
            }
            return paramValue;
        }

        public void MergeTable(ref DataSet dataset, DataTable datatable, string tableName)
        {
            try
            {
                datatable.TableName = tableName;
                dataset.Tables.Add(datatable.Copy());
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ■ Parameter Control
        public void AddParamToCommand(OracleCommand command, string paramName, OracleType oracleType, object paramValue)
        {
            try
            {
                OracleParameter oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(oOracleParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void AddParamToCommand(OracleCommand command, string paramName, OracleType oracleType, object paramValue, ParameterDirection parameterDirection)
        {
            try
            {
                OracleParameter oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = parameterDirection;
                command.Parameters.Add(oOracleParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void AddParamToCommand(OracleCommand command, string paramName, OracleType oracleType, int size, object paramValue)
        {
            try
            {
                OracleParameter oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Size = size;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(oOracleParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void AddParamToCommand(OracleCommand command, string paramName, OracleType oracleType, int size, object paramValue, ParameterDirection parameterDirection)
        {
            try
            {
                OracleParameter oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Size = size;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = parameterDirection;
                command.Parameters.Add(oOracleParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public OracleParameter AddParamCursor(string paramName)
        {
            OracleParameter oOracleParameter = null;
            try
            {
                oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = OracleType.Cursor;
                oOracleParameter.Direction = ParameterDirection.ReturnValue;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleParameter;
        }

        public OracleParameter AddParam(string paramName, OracleType oracleType, object paramValue)
        {
            OracleParameter oOracleParameter = null;
            try
            {
                oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = ParameterDirection.Input;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleParameter;
        }
        public OracleParameter AddParam(string paramName, OracleType oracleType, object paramValue, ParameterDirection parameterDirection)
        {
            OracleParameter oOracleParameter = null;
            try
            {
                oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = parameterDirection;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleParameter;
        }

        public OracleParameter AddParam(string paramName, OracleType oracleType, int size, object paramValue)
        {
            OracleParameter oOracleParameter = null;
            try
            {
                oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Size = size;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = ParameterDirection.Input;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleParameter;
        }
        public OracleParameter AddParam(string paramName, OracleType oracleType, int size, object paramValue, ParameterDirection parameterDirection)
        {
            OracleParameter oOracleParameter = null;
            try
            {
                oOracleParameter = new OracleParameter();
                oOracleParameter.ParameterName = paramName;
                oOracleParameter.OracleType = oracleType;
                oOracleParameter.Size = size;
                oOracleParameter.Value = CheckNull(paramValue);
                oOracleParameter.Direction = parameterDirection;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleParameter;
        }
        #endregion

        #region ■ Connection,Command Control
        public Database GetDataBase(string DBAlias)
        {
            Database database = null;
            try
            {
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return database;
        }

        public OracleCommand GetSqlCommand(Database database, string SqlQueryText)
        {
            OracleCommand oOracleCommand = null;
            try
            {
                oOracleCommand = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleCommand;
        }

        public OracleCommand GetSqlCommand(Database database)
        {
            OracleCommand oOracleCommand = null;
            try
            {
                oOracleCommand = database.GetSqlStringCommand(" ") as OracleCommand;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleCommand;
        }

        public OracleCommand GetSqlCommand(string DBAlias)
        {
            Database database = null;
            OracleCommand oOracleCommand = null;
            try
            {
                database = this.GetDataBase(DBAlias) as OracleDatabase;
                oOracleCommand = database.GetSqlStringCommand(" ") as OracleCommand;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oOracleCommand;
        }

        #endregion

        #region ■ ExecuteDataTable By Scope of Rows
        public DataTable ExecuteDataTable(string DBAlias, string SqlQueryText, int startRecord, int maxRecords)
        {
            DataTable dtRtn = null;
            Database database = null;
            OracleDataAdapter oDA = null;
            OracleCommand command = null;

            try
            {
                dtRtn = new DataTable();
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                command = GetSqlCommand(database, SqlQueryText);

                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }

                oDA.SelectCommand = command;

                oDA.Fill(startRecord, maxRecords, dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (command != null) command.Dispose();
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(string DBAlias, OracleCommand command, int startRecord, int maxRecords)
        {
            DataTable dtRtn = null;
            Database database = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oDA = database.GetDataAdapter() as OracleDataAdapter;

                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }

                oDA.SelectCommand = command;

                oDA.Fill(startRecord, maxRecords, dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(string DBAlias, OracleCommand command, OracleParameter[] oracleParameters, int startRecord, int maxRecords)
        {
            DataTable dtRtn = null;
            Database database = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }

                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }

                oDA.SelectCommand = command;

                oDA.Fill(startRecord, maxRecords, dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }

                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }

        public DataTable ExecuteDataTable(Database database, string SqlQueryText, int startRecord, int maxRecords)
        {
            DataTable dtRtn = null;
            OracleDataAdapter oDA = null;
            OracleCommand command = null;
            try
            {
                command = GetSqlCommand(database, SqlQueryText);
                dtRtn = new DataTable();
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }
                oDA.SelectCommand = command;
                oDA.Fill(startRecord, maxRecords, dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (command != null) command.Dispose();
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(Database database, OracleCommand command, int startRecord, int maxRecords)
        {
            DataTable dtRtn = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }
                oDA.SelectCommand = command;
                oDA.Fill(startRecord, maxRecords, dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(Database database, OracleCommand command, OracleParameter[] oracleParameters, int startRecord, int maxRecords)
        {
            DataTable dtRtn = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }
                oDA.SelectCommand = command;

                oDA.Fill(startRecord, maxRecords, dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        #endregion

        #region ■ ExecuteDataTable By Scope of Rows
        public DataTable ExecuteDataTable(string DBAlias, string SqlQueryText)
        {
            DataTable dtRtn = null;
            Database database = null;
            OracleDataAdapter oDA = null;
            OracleCommand command = null;

            try
            {
                dtRtn = new DataTable();
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                command = GetSqlCommand(database, SqlQueryText);

                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }

                oDA.SelectCommand = command;

                oDA.Fill(dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (command != null) command.Dispose();
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(string DBAlias, OracleCommand command)
        {
            DataTable dtRtn = null;
            Database database = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oDA = database.GetDataAdapter() as OracleDataAdapter;

                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }

                oDA.SelectCommand = command;

                oDA.Fill(dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(string DBAlias, OracleCommand command, OracleParameter[] oracleParameters)
        {
            DataTable dtRtn = null;
            Database database = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }

                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }

                oDA.SelectCommand = command;

                oDA.Fill(dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }

                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }

        public DataTable ExecuteDataTable(Database database, string SqlQueryText)
        {
            DataTable dtRtn = null;
            OracleDataAdapter oDA = null;
            OracleCommand command = null;
            try
            {
                command = GetSqlCommand(database, SqlQueryText);
                dtRtn = new DataTable();
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }
                oDA.SelectCommand = command;
                oDA.Fill(dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (command != null) command.Dispose();
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(Database database, OracleCommand command)
        {
            DataTable dtRtn = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }
                oDA.SelectCommand = command;
                oDA.Fill(dtRtn);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        public DataTable ExecuteDataTable(Database database, OracleCommand command, OracleParameter[] oracleParameters)
        {
            DataTable dtRtn = null;
            OracleDataAdapter oDA = null;

            try
            {
                dtRtn = new DataTable();
                oDA = database.GetDataAdapter() as OracleDataAdapter;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                if (command.Connection == null)
                {
                    command.Connection = database.CreateConnection() as OracleConnection;
                }
                oDA.SelectCommand = command;

                oDA.Fill(dtRtn);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                command.Parameters.Clear();
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    command.Connection.Dispose();
                }
                if (oDA != null) oDA.Dispose();
            }
            return dtRtn;
        }
        protected DataTable ExecuteTable(string DBAlias, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            return ExecuteDataSet(DBAlias, SqlQueryText, oracleParameters).Tables[0];
        }
        #endregion

        #region ■ ExecuteDataSet

        public DataSet ExecuteDataSet(Database database, OracleCommand command)
        {
            DataSet dsReturn = null;
            try
            {
                dsReturn = database.ExecuteDataSet(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }

        public DataSet ExecuteDataSet(Database database, OracleCommand command, OracleTransaction transaction)
        {
            DataSet dsReturn = null;
            try
            {
                dsReturn = database.ExecuteDataSet(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }


        public DataSet ExecuteDataSet(Database database, OracleCommand command, OracleParameter[] oracleParameters)
        {
            DataSet dsReturn = null;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                dsReturn = database.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }



        public DataSet ExecuteDataSet(Database database, OracleCommand command, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            DataSet dsReturn = null;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }

                dsReturn = database.ExecuteDataSet(command, transaction);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }


        public DataSet ExecuteDataSet(string DBAlias, string SqlQueryText)
        {
            DataSet dsReturn = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                dsReturn = database.ExecuteDataSet(CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }


        public DataSet ExecuteDataSet(string DBAlias, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            DataSet dsReturn = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                OracleCommand command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;

                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                dsReturn = database.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }


        // SP 호출용 : 리턴받기 위해서는 반드시 리턴 파라미터가 필요하다.
        public DataSet ExecuteDataSet(string DBAlias, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            DataSet dsReturn = null;
            OracleCommand command = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;

                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }

                }
                dsReturn = database.ExecuteDataSet(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }
        public DataSet ExecuteDataSet(Database database, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            DataSet dsReturn = null;
            OracleCommand command = null;
            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }

                }
                dsReturn = database.ExecuteDataSet(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }
        public DataSet ExecuteDataSet(Database database, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            DataSet dsReturn = null;
            OracleCommand command = null;
            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }

                }
                dsReturn = database.ExecuteDataSet(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return dsReturn;
        }
        // END SP 호출용



        #endregion

        #region ■ ExecuteStringTable // 파라미터 들어가는거도 만들어야 되는데...
        public string ExecuteStringTable(string DBAlias, string SqlQueryText)
        {
            Database database = null;
            string sReturn = string.Empty;
            try
            {
                database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                sReturn = ReadTableToEnd(database.ExecuteReader(CommandType.Text, SqlQueryText));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                database = null;
            }
            return sReturn;
        }


        public string ExecuteStringTable(string DBAlias, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            string sReturn = string.Empty;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                OracleCommand command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;

                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                sReturn = ReadTableToEnd(database.ExecuteReader(command));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }


        public string ExecuteStringTable(Database database, string SqlQueryText)
        {
            string sReturn = string.Empty;
            try
            {
                sReturn = ReadTableToEnd(database.ExecuteReader(CommandType.Text, SqlQueryText));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }


        public string ExecuteStringTable(Database database, OracleCommand command)
        {
            string sReturn = string.Empty;
            try
            {
                sReturn = ReadTableToEnd(database.ExecuteReader(command));
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }


        public string ExecuteStringTable(Database database, OracleCommand command, OracleParameter[] oracleParameters)
        {
            string sReturn = string.Empty;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                sReturn = ReadTableToEnd(database.ExecuteReader(command));
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }


        public string ExecuteStringTable(Database database, OracleCommand command, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            string sReturn = string.Empty;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                sReturn = ReadTableToEnd(database.ExecuteReader(command, transaction));
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }

        public string ExecuteStringTable(Database database, OracleCommand command, OracleTransaction transaction)
        {
            string sReturn = string.Empty;
            try
            {
                sReturn = ReadTableToEnd(database.ExecuteReader(command, transaction));
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }
        /********************************************************
        * Function name  : ExecuteStringTable
        * Purpose		 : ExecuteStringTable (SP 호출용) 
        * Input		     : Database database, CommandType commandType,string SqlQueryText, OracleParameter[] oracleParameters, OracleTransaction transaction
        * Output		 : String DataTable
        *********************************************************/
        public string ExecuteStringTable(Database database, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            OracleCommand command = null;
            string sReturn = string.Empty;
            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                sReturn = ReadTableToEnd(database.ExecuteReader(command, transaction));
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return sReturn;
        }
        #endregion

        #region ■ ExecuteScala

        public object ExecuteScalar(string DBAlias, string SqlQueryText)
        {
            object oReturn = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                oReturn = database.ExecuteScalar(CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }

        public object ExecuteScalar(string DBAlias, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            object oReturn = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                OracleCommand command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                oReturn = database.ExecuteScalar(command);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }


        public object ExecuteScalar(Database database, string SqlQueryText)
        {
            object oReturn = null;
            try
            {
                oReturn = database.ExecuteScalar(CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }

        public object ExecuteScalar(Database database, string SqlQueryText, OracleTransaction transaction)
        {
            object oReturn = null;
            try
            {
                oReturn = database.ExecuteScalar(transaction, CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }

        public object ExecuteScalar(Database database, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            object oReturn = null;
            try
            {
                OracleCommand cmd = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    cmd.Parameters.Add(oracleParameters[i]);
                }
                oReturn = database.ExecuteScalar(CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }

        public object ExecuteScalar(Database database, string SqlQueryText, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            object oReturn = null;
            try
            {
                OracleCommand command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                oReturn = database.ExecuteScalar(command, transaction);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }

        public object ExecuteScalar(Database database, OracleCommand command)
        {
            object oReturn = null;
            try
            {
                oReturn = database.ExecuteScalar(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }
        public object ExecuteScalar(Database database, OracleCommand command, OracleParameter[] oracleParameters)
        {
            object oReturn = null;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }

                oReturn = database.ExecuteScalar(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }
        public object ExecuteScalar(Database database, OracleCommand command, OracleTransaction transaction)
        {
            object oReturn = null;
            try
            {
                oReturn = database.ExecuteScalar(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }
        public object ExecuteScalar(Database database, OracleCommand command, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            object oReturn = null;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                oReturn = database.ExecuteScalar(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return oReturn;
        }
        #endregion

        #region ■ Execute

        public int Execute(string DBAlias, string SqlQueryText)
        {
            int iAffectedRow = 0;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                iAffectedRow = database.ExecuteNonQuery(CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(string DBAlias, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            int iAffectedRow = 0;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                OracleCommand command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                iAffectedRow = database.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, string SqlQueryText)
        {
            int iAffectedRow = 0;
            try
            {
                iAffectedRow = database.ExecuteNonQuery(CommandType.Text, SqlQueryText);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            int iAffectedRow = 0;
            try
            {
                OracleCommand cmd = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    cmd.Parameters.Add(oracleParameters[i]);
                }
                iAffectedRow = database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, string SqlQueryText, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            int iAffectedRow = 0;
            try
            {
                OracleCommand cmd = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    cmd.Parameters.Add(oracleParameters[i]);
                }
                iAffectedRow = database.ExecuteNonQuery(cmd, transaction);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, OracleCommand command)
        {
            int iAffectedRow = 0;
            try
            {
                iAffectedRow = database.ExecuteNonQuery(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, OracleCommand command, OracleParameter[] oracleParameters)
        {
            int iAffectedRow = 0;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                iAffectedRow = database.ExecuteNonQuery(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, OracleCommand command, OracleTransaction transaction)
        {
            int iAffectedRow = 0;
            try
            {
                iAffectedRow = database.ExecuteNonQuery(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(Database database, OracleCommand command, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            int iAffectedRow = 0;
            try
            {
                for (int i = 0; i < oracleParameters.Length; i++)
                {
                    command.Parameters.Add(oracleParameters[i]);
                }
                iAffectedRow = database.ExecuteNonQuery(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        // SP 호출용 : 파라미터가 있을수도 있고 없을수도 있다.
        public int Execute(string DBAlias, CommandType commandType, string SqlQueryText)
        {
            int iAffectedRow = 0;

            OracleCommand command = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                }
                iAffectedRow = database.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }

        public int Execute(string DBAlias, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            int iAffectedRow = 0;

            OracleCommand command = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                iAffectedRow = database.ExecuteNonQuery(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }
        public int Execute(Database database, CommandType commandType, string SqlQueryText)
        {
            int iAffectedRow = 0;

            OracleCommand command = null;

            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                }
                iAffectedRow = database.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }
        public int Execute(Database database, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters)
        {
            int iAffectedRow = 0;

            OracleCommand command = null;

            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                iAffectedRow = database.ExecuteNonQuery(command);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }
        public int Execute(Database database, CommandType commandType, string SqlQueryText, OracleTransaction transaction)
        {
            int iAffectedRow = 0;

            OracleCommand command = null;

            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                }
                iAffectedRow = database.ExecuteNonQuery(command, transaction);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }
        public int Execute(Database database, CommandType commandType, string SqlQueryText, OracleParameter[] oracleParameters, OracleTransaction transaction)
        {
            int iAffectedRow = 0;

            OracleCommand command = null;
            try
            {
                if (commandType == CommandType.StoredProcedure)
                {
                    command = database.GetStoredProcCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                else
                {
                    command = database.GetSqlStringCommand(SqlQueryText) as OracleCommand;
                    for (int i = 0; i < oracleParameters.Length; i++)
                    {
                        command.Parameters.Add(oracleParameters[i]);
                    }
                }
                iAffectedRow = database.ExecuteNonQuery(command, transaction);
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return iAffectedRow;
        }
        // END SP 호출용



        #endregion

        #region ■ BATCH Execution Control
        // multi query 수행을 위한 메소드 ( BATCH QUERY이므로 AUTO TRANSACTION 적용함.)
        // 일괄 batch transaction 아니면.. 메뉴얼 트랜젝션에서 배치를 사용할 수 있다.
        // 메뉴얼 트랜잭션에 대한 배치 메소드도 있어야 할거 같다..ㅠㅠ
        public void ExecuteBatch(string DBAlias, string[] SqlQueries)
        {
            Database database = DatabaseFactory.CreateDatabase(DBAlias) as OracleDatabase;
            using (IDbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = (DbTransaction)connection.BeginTransaction();
                try
                {
                    for (int i = 0; i < SqlQueries.Length; i++)
                    {
                        OracleCommand cmd = database.GetSqlStringCommand(SqlQueries[i]) as OracleCommand;
                        database.ExecuteNonQuery(cmd, transaction);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
        }

        public void ExecuteBatch(Database database, string[] SqlQueries, OracleTransaction transaction)
        {
            OracleCommand cmd = null;

            try
            {
                for (int i = 0; i < SqlQueries.Length; i++)
                {
                    cmd = database.GetSqlStringCommand(SqlQueries[i]) as OracleCommand;
                    database.ExecuteNonQuery(cmd, transaction);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region  ■ Data Packing Utility
        public byte[] STR2BLOB(string blobString)
        {
            Byte[] result = null;
            if (blobString.Trim() == "") return null;
            if (blobString != null)
            {
                string[] strings = blobString.Split(new char[] { '♠' });
                result = new byte[strings.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = byte.Parse(strings[i]);
                }
            }

            return result;
        }
        public string BLOB2STR(Byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString());
                sb.Append("♠");
            }

            sb.Append(bytes[bytes.Length - 1].ToString());

            return sb.ToString();
        }

        private string ReadTableToEnd(IDataReader _dbReader)
        {
            StringBuilder s = new StringBuilder();
            StringBuilder s2 = new StringBuilder();

            DataTable SchemaTable = null;
            try
            {
                #region DataTable Schema 정보 저장 0 - Col Name 1 - ColType 2 ~ N Data

                SchemaTable = _dbReader.GetSchemaTable();

                int columnCount = SchemaTable.Rows.Count;

                foreach (DataRow row in SchemaTable.Rows)
                {
                    s.Append(row["ColumnName"].ToString());
                    s.Append('│');

                    s2.Append(row["DataType"].ToString());
                    s2.Append('│');
                }

                s.Remove(s.Length - 1, 1);
                s.Append('┼');

                s2.Remove(s2.Length - 1, 1);
                s2.Append('┼');

                s.Append(s2.ToString());

                #endregion

                while (_dbReader.Read())
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        object obj = _dbReader[i];

                        if (obj == DBNull.Value)
                        {
                            s.Append("┴"); //DBNull지정 문자
                        }
                        else
                        {
                            //if (SchemaTable.Rows[i]["DataType"].ToString().ToLower() == "System.Byte[]".ToLower())
                            //{
                            //    s.Append(BLOB2STR(obj as byte[]));
                            //}
                            //else
                            //{
                            //    s.Append(obj.ToString());
                            //}


                            if (SchemaTable.Rows[i]["DataType"].ToString().ToLower() == "System.Byte[]".ToLower())
                            {
                                s.Append(BLOB2STR(obj as byte[]));
                            }
                            else if (SchemaTable.Rows[i]["DataType"].ToString().ToLower() == "System.DateTime".ToLower())
                            {
                                s.Append(DateTime.Parse(obj.ToString()).ToString("MM-dd-yyyy hh:mm:ss"));
                            }
                            else
                            {
                                s.Append(obj.ToString());
                            }


                        }

                        s.Append('│');
                    }

                    s.Remove(s.Length - 1, 1);
                    s.Append('┼');
                }
                /// s.Append('┼'); 제거
                s.Remove(s.Length - 1, 1);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (!_dbReader.IsClosed)
                {
                    _dbReader.Close();
                }
                _dbReader.Dispose();
                _dbReader = null;
            }


            return s.ToString();
        }



        #region string --> DataTable, DataTable --> string
        /// <summary>
        /// 수정번호: 1
        /// 
        /// DataTable을 String으로 변환합니다.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DT2Str(DataTable dt)
        {
            StringBuilder s = new StringBuilder();
            #region DataTable Schema 정보 저장 0 - Col Name 1 - ColType 2 ~ N Data
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                s.Append(dt.Columns[i].ColumnName);
                s.Append('│');
            }
            s.Remove(s.Length - 1, 1);
            s.Append('┼');

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                s.Append(dt.Columns[i].DataType.ToString());
                s.Append('│');
            }
            s.Remove(s.Length - 1, 1);
            s.Append('┼');
            #endregion

            #region DataTable Data 저장



            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    object obj = dt.Rows[r][dt.Columns[c].ColumnName];

                    if (obj == DBNull.Value)
                    {
                        s.Append("┴"); //DBNull지정 문자
                    }
                    else
                    {
                        ///
                        ///수정번호: 4
                        if (dt.Columns[c].DataType.ToString() == "System.Byte[]")
                        {
                            s.Append(BLOB2STR(obj as byte[]));
                        }
                        else
                        {
                            s.Append(obj.ToString());
                        }
                    }

                    s.Append('│');
                }
                s.Remove(s.Length - 1, 1);
                s.Append('┼');
            }
            s.Remove(s.Length - 1, 1);
            #endregion

            return s.ToString();
        }

        /// <summary>
        /// 수정번호: 1
        /// 
        /// string을 DataTable로 변환합니다.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public DataTable Str2DT(string s)
        {
            DataTable dt = new DataTable();
            string[] data = s.Split(new char[] { '┼' });

            string[] colNames = data[0].Split(new char[] { '│' });
            string[] colTypes = data[1].Split(new char[] { '│' });
            int colCount = colNames.Length;

            Type colType = null;
            DataColumn col = null;

            for (int i = 0; i < colCount; i++)
            {
                colType = Type.GetType(colTypes[i]);
                col = new DataColumn(colNames[i], colType);
                dt.Columns.Add(col);
            }

            string[] colValues = null;
            DataRow row = null;

            for (int r = 2; r < data.Length; r++)
            {
                colValues = data[r].Split(new char[] { '│' });
                row = dt.NewRow();
                for (int c = 0; c < colCount; c++)
                {
                    //전달된 문자열을 이용해 Table을 생성할 때 DateTime객체인 경우 ""이면 캐스팅에러 발생
                    if (colValues[c] == "┴")  //DBNull지정 문자
                    {
                        row[c] = System.DBNull.Value;
                    }
                    else
                    {
                        if (dt.Columns[c].DataType.ToString() == "System.Byte[]")
                        {
                            row[c] = STR2BLOB(colValues[c]);
                        }
                        else
                        {
                            row[c] = colValues[c];
                        }
                    }
                }

                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion


        public string MD2Array2STR(string[,] r2DMArray)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < r2DMArray.GetLength(0); i++)
            {
                for (int j = 0; j < r2DMArray.GetLength(1); j++)
                {
                    sb.Append(r2DMArray[i, j]);
                    sb.Append('│');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append('┼');
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public string[,] STR2MD2Array(string r2DMArrayStr)
        {
            if (r2DMArrayStr == null || r2DMArrayStr == "") return null;
            string[,] xArrReturn = null;
            string[] xArrCols = null;
            string[] xArrRows = r2DMArrayStr.Split(new char[] { '┼' });
            for (int i = 0; i < xArrRows.Length; i++)
            {
                xArrCols = xArrRows[i].Split(new char[] { '│' });
                if (i == 0)
                {
                    xArrReturn = new string[xArrRows.Length, xArrCols.Length];
                }
                for (int j = 0; j < xArrCols.Length; j++)
                {
                    xArrReturn[i, j] = xArrCols[j];
                }
            }
            return xArrReturn;
        }


        public string MD2ArrayList2STR(ArrayList rArrayList)
        {
            if (rArrayList == null) return null;
            StringBuilder sb = new StringBuilder();
            string[] xArrCols = null;
            for (int i = 0; i < rArrayList.Count; i++)
            {
                xArrCols = (string[])rArrayList[i];
                for (int j = 0; j < xArrCols.Length; j++)
                {
                    sb.Append(xArrCols[j]);
                    sb.Append('│');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append('┼');
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        public ArrayList STR2MD2ArrayList(string r2DMArrayListStr)
        {
            if (r2DMArrayListStr == null) return null;
            ArrayList xArrReturn = new ArrayList();
            if (r2DMArrayListStr.Length == 0) return xArrReturn;
            string[] xArrCols = null;
            string[] xArrRows = r2DMArrayListStr.Split(new char[] { '┼' });
            for (int i = 0; i < xArrRows.Length; i++)
            {
                xArrCols = xArrRows[i].Split(new char[] { '│' });
                xArrReturn.Add(xArrCols);
            }
            return xArrReturn;
        }

        #endregion
    }
}
