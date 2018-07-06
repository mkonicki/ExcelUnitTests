using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class DbTestCaseSourceAttribute : BaseDataSourceAttribute
    {
        public DbTestCaseSourceAttribute(string connectionString, string tableName) :
            this(connectionString, tableName, null)
        {
        }

        public DbTestCaseSourceAttribute(string connectionString, string tableName, Type dataType) :
            base(new DbDataReader(connectionString, tableName), dataType)
        {
        }

        public override IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (var parms in GetTestCases())
            {
                yield return Builder.BuildTestMethod(method, suite, parms);
            }
        }
    }
}
