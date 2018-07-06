using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataSourcesReaders.XUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class DbDataAttribute : BaseDataAttribute
    {
        public DbDataAttribute(string connectionString, string tableName) :
           this(connectionString, tableName, null)
        {
        }

        public DbDataAttribute(string connectionString, string tableName, Type dataType) :
            base(new DbDataReader(connectionString, tableName), dataType)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return GetTestCases();
        }
    }
}
