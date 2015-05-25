namespace DAL
{
    using System;
    using System.Data;
    using System.Data.OleDb;

    public class DBHelper
    {
        private static OleDbConnection connection;

        public static int ExecuteCommand(string safeSql)
        {
            OleDbCommand command = new OleDbCommand(safeSql, Connection);
            return command.ExecuteNonQuery();
        }

        public static int ExecuteCommand(string sql, params OleDbParameter[] values)
        {
            OleDbCommand command = new OleDbCommand(sql, Connection);
            command.Parameters.AddRange(values);
            return command.ExecuteNonQuery();
        }

        public static DataTable GetDataSet(string safeSql)
        {
            DataSet dataSet = new DataSet();
            OleDbCommand selectCommand = new OleDbCommand(safeSql, Connection);
            new OleDbDataAdapter(selectCommand).Fill(dataSet);
            return dataSet.Tables[0];
        }

        public static DataTable GetDataSet(string sql, params OleDbParameter[] values)
        {
            DataSet dataSet = new DataSet();
            OleDbCommand selectCommand = new OleDbCommand(sql, Connection);
            selectCommand.Parameters.AddRange(values);
            new OleDbDataAdapter(selectCommand).Fill(dataSet);
            return dataSet.Tables[0];
        }

        public static OleDbDataReader GetReader(string safeSql)
        {
            OleDbCommand command = new OleDbCommand(safeSql, Connection);
            return command.ExecuteReader();
        }

        public static OleDbDataReader GetReader(string sql, params OleDbParameter[] values)
        {
            OleDbCommand command = new OleDbCommand(sql, Connection);
            command.Parameters.AddRange(values);
            return command.ExecuteReader();
        }

        public static int GetScalar(string safeSql)
        {
            OleDbCommand command = new OleDbCommand(safeSql, Connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public static int GetScalar(string sql, params OleDbParameter[] values)
        {
            OleDbCommand command = new OleDbCommand(sql, Connection);
            command.Parameters.AddRange(values);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public static string ReturnStringScalar(string safeSql)
        {
            OleDbCommand command = new OleDbCommand(safeSql, Connection);
            try
            {
                return command.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public static OleDbConnection Connection
        {
            get
            {
                string connectionString = @"provider = Microsoft.Jet.OLEDB.4.0; Data Source=.\SentCard.mdb;Jet OLEDB:Database Password=xmkad-5725111";
                if (connection == null)
                {
                    connection = new OleDbConnection(connectionString);
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
    }
}

