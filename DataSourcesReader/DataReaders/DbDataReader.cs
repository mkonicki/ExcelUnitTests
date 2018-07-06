using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace DataSourcesReaders
{
    public class DbDataReader : ITestCaseReader
    {
        private readonly string ConnectionString;
        private readonly string TableName;

        public DbDataReader(string connectionString, string tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        public IEnumerable<dynamic> GetData() =>
            GetData<dynamic>(() => new ExpandoObject(), (tc, k, v) =>
            {
                var testDataAsDictionary = (ICollection<KeyValuePair<string, object>>)tc;
                testDataAsDictionary.Add(new KeyValuePair<string, object>(k, v));
            });

        public IEnumerable<T> GetData<T>() where T : new() =>
            GetData(() => Activator.CreateInstance<T>(), (tc, k, v) => tc.SetCastedValue(k, v));


        private IEnumerable<T> GetData<T>(Func<T> initializeTestDataObject,
            Action<T, string, object> setupPropertyValue) where T : new()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand($"SELECT * FROM {TableName}", connection);

                using (var sqlDataAdapter = new SqlDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    foreach (var row in dataTable.Rows)
                    {
                        var testCase = initializeTestDataObject.Invoke();

                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            var dataRow = ((DataRow)row);
                            var value = dataRow[i];
                            var key = dataTable.Columns[i].ColumnName;

                            setupPropertyValue.Invoke(testCase, key, value);
                        }

                        yield return testCase;
                    }
                }

                connection.Close();
            }
        }
    }
}