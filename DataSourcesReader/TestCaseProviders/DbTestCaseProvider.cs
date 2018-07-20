using DataSourcesReaders.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace DataSourcesReaders
{
    public class DbTestCaseProvider : ITestCaseProvider
    {
        private readonly string ConnectionString;
        private readonly string TableName;

        public DbTestCaseProvider(string connectionString, string tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        public IEnumerable<dynamic> GetDynamic() =>
            GetGeneric<dynamic>(() => new ExpandoObject(), (tc, k, v) =>
            {
                var testDataAsDictionary = (ICollection<KeyValuePair<string, object>>)tc;
                testDataAsDictionary.Add(new KeyValuePair<string, object>(k, v));
            });

        public IEnumerable<T> GetGeneric<T>()
            where T : new() =>
            GetGeneric(() => Activator.CreateInstance<T>(), (tc, k, v) => tc.SetCastedValue(k, v));

        public IEnumerable<TestCase<TCase, TResult>> GetTestCases<TCase, TResult>()
            where TCase : new()
            where TResult : new() =>
            GetGeneric(() => Activator.CreateInstance<TestCase<TCase, TResult>>(), (tc, k, v) => tc.SetCastedValue(k, v));

        private IEnumerable<T> GetGeneric<T>(Func<T> initializeTestDataObject,
            Action<T, string, object> setupPropertyValue)
            where T : new()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand($"SELECT * FROM {TableName}", connection);

                using (var sqlDataAdapter = new SqlDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {

                        yield return GetTestDataObject(initializeTestDataObject, setupPropertyValue, dataTable, row);
                    }
                }

                connection.Close();
            }
        }

        private T GetTestDataObject<T>(Func<T> initializeTestDataObject,
           Action<T, string, object> setupPropertyValue, DataTable table, DataRow row)
           where T : new()
        {
            var testCase = initializeTestDataObject.Invoke();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var value = row[i];
                var key = table.Columns[i].ColumnName;

                setupPropertyValue.Invoke(testCase, key, value);
            }

            return testCase;
        }
    }
}