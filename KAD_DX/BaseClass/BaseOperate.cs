namespace KAD_DX.BaseClass
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Windows.Forms;

    internal class BaseOperate
    {
        public void getcom(string M_str_sqlstr)
        {
            OleDbConnection connection = this.getcon();
            connection.Open();
            OleDbCommand command = new OleDbCommand(M_str_sqlstr, connection);
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        public OleDbConnection getcon()
        {
            string startupPath = Application.StartupPath;
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + startupPath + @"\SentCard.mdb;Jet OLEDB:Database Password=xmkad-5725111");
        }

        public DataSet getds(string M_str_sqlstr, string M_str_table)
        {
            OleDbConnection selectConnection = this.getcon();
            OleDbDataAdapter adapter = new OleDbDataAdapter(M_str_sqlstr, selectConnection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, M_str_table);
            return dataSet;
        }

        public void getimgcom(string M_str_sqlstr, byte[] img)
        {
            OleDbConnection connection = this.getcon();
            connection.Open();
            OleDbCommand command = new OleDbCommand(M_str_sqlstr, connection);
            OleDbParameter parameter = new OleDbParameter("?", OleDbType.Binary) {
                Value = img
            };
            command.Parameters.Add(parameter);
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        public OleDbDataReader getread(string M_str_sqlstr)
        {
            OleDbConnection connection = this.getcon();
            OleDbCommand command = new OleDbCommand(M_str_sqlstr, connection);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}

