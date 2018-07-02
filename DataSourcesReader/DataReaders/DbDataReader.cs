using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataSourcesReaders
{
    public class DbDataReader : ITestCaseReader
    {
        private readonly string ConnectionString;
        private readonly string TableName;
        private const string SqlQuery = "SELECT * FROM @Table";

        public DbDataReader(string connectionString, string tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        public IEnumerable<dynamic> GetData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                //REFACTOR to use adapter as String and connection
                var command = new SqlCommand(SqlQuery);
                command.Parameters.Add(new SqlParameter("@Table", TableName));

                using (var sqlDataAdapter = new SqlDataAdapter(command.CommandText, connection))
                {
                    var dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    foreach (var row in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            var dataRow = ((DataRow)row);
                            var value = dataRow[i];
                            var key = dataTable.Columns[i];

                            yield return new { value, key };
                        }
                    }
                }

                connection.Close();
            }
        }

        public IEnumerable<T> GetData<T>() where T : new()
        {
            throw new System.NotImplementedException();
        }
    }
}
