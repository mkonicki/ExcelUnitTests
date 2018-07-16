using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataSourcesReaders.XUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class DbDataAttribute : BaseDataAttribute
    {

        public DbDataAttribute(string connectionString, string tableName) :
            base(new DbTestCaseProvider(connectionString, tableName))
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return GetTestCases(testMethod);
        }
    }
}
