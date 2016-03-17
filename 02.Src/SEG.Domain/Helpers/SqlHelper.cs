using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Helpers
{
    public class SqlHelper
    {
        public SqlConnection connection;
        public SqlCommand sqlCommand;
        public SqlDataReader sqlReader;
        public bool hasError;
        public string errorMessage;

        public string connString;

        public SqlHelper(string aConnString)
        {
            connString = aConnString;

            connection = null;
            sqlCommand = null;
            sqlReader = null;

            hasError = false;
        }

        public void Clear()
        {
            Disconnect();

            hasError = false;
            connection = null;
            sqlCommand = null;
            sqlReader = null;
        }

        public bool DoCommandReader(string query)
        {
            bool flag = true;
            try
            {
                if (connection != null)
                {
                    if (sqlCommand != null)
                        CloseCommand();
                    sqlCommand = new SqlCommand(query, connection);
                    sqlReader = sqlCommand.ExecuteReader();
                }
                else
                {
                    flag = false;
                    hasError = true;
                    errorMessage = "Database query error : The connection was closed.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                hasError = true;
                errorMessage = "Database query error : " + ex.Message;
            }
            return flag;
        }

        public bool DoCommandExecute(string query)
        {
            bool flag = true;
            try
            {
                if (connection != null)
                {
                    if (sqlCommand != null)
                        CloseCommand();
                    sqlCommand = new SqlCommand(query, this.connection);
                    sqlCommand.ExecuteNonQuery();
                }
                else
                {
                    flag = false;
                    hasError = true;
                    errorMessage = "Database command error : The connection was closed.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                hasError = true;
                errorMessage = "Database command execution error : " + ex.Message;
            }
            return flag;
        }

        public bool CloseCommand()
        {
            bool flag = true;
            try
            {
                if (connection != null)
                {
                    if (sqlReader != null)
                    {
                        sqlReader.Close();
                        sqlReader.Dispose();
                        sqlReader = (SqlDataReader)null;
                    }
                    if (sqlCommand != null)
                    {
                        sqlCommand.Dispose();
                        sqlCommand = (SqlCommand)null;
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                hasError = true;
                errorMessage = "Database command error : " + ex.Message;
            }
            return flag;
        }

        public bool Connect()
        {
            bool flag = true;
            hasError = false;
            try
            {
                if (connection != null)
                    Disconnect();
                connection = new SqlConnection(connString);
                connection.Open();
            }
            catch (Exception ex)
            {
                flag = false;
                hasError = true;
                errorMessage = "Could not connect to database : " + ex.Message;
            }
            return flag;
        }

        public bool Disconnect()
        {
            bool flag = true;
            try
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = (SqlConnection)null;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                hasError = true;
                errorMessage = "Could not disconnect from database : " + ex.Message;
            }
            return flag;
        }


        public bool TestSEG()
        {
            // This routine tries to open and select RiskAnalysis types, of which there should be 4
            bool isOk = false;

            if(Connect())
            {
                if (this.DoCommandReader("SELECT * FROM RiskAnalysisTypes"))
                {
                    if(sqlReader.HasRows)
                    {
                        // We should be ok
                        isOk = true;
                        CloseCommand();
                    }
                }
                Disconnect();
            }

            return isOk;
        }
    }
}
