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
            ReadDataFromSource<dynamic>(
                () => new ExpandoObject(),
                (tc, k, v) =>
                {
                    var testDataAsDictionary = (ICollection<KeyValuePair<string, object>>)tc;
                    testDataAsDictionary.Add(new KeyValuePair<string, object>(k, v));
                }
            );

        public IEnumerable<T> GetGeneric<T>()
            where T : new() =>
            ReadDataFromSource(
                () => Activator.CreateInstance<T>(),
                (tc, k, v) => tc.SetCastedValue(k, v)
            );

        public IEnumerable<TestCase<TCase, TResult>> GetTestCases<TCase, TResult>()
            where TCase : new()
            where TResult : new() =>
            ReadDataFromSource(
                () => Activator.CreateInstance<TestCase<TCase, TResult>>(),
                (tc, k, v) => tc.SetCastedValue(k, v));

        private IEnumerable<T> ReadDataFromSource<T>(Func<T> initializeObject, Action<T, string, object> setupValue)
            where T : new()
            => ReadDataFromSource(new TestCaseWrapper<T>(initializeObject, setupValue));

        private IEnumerable<T> ReadDataFromSource<T>(TestCaseWrapper<T> testCaseWrapper)
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
                        yield return GetTestDataObject(testCaseWrapper, dataTable, row);
                    }
                }

                connection.Close();
            }
        }

        private T GetTestDataObject<T>(TestCaseWrapper<T> testCaseWrapper, DataTable table, DataRow row)
           where T : new()
        {
            var testCase = testCaseWrapper.Initialize.Invoke();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var value = row[i];
                var key = table.Columns[i].ColumnName;

                testCaseWrapper.SetupValue.Invoke(testCase, key, value);
            }

            return testCase;
        }
    }
}