using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class DbTestCaseSourceAttribute : BaseDataSourceAttribute
    {
        public DbTestCaseSourceAttribute(string connectionString, string tableName) :
            base(new DbTestCaseProvider(connectionString, tableName))
        {
        }

        public override IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (var parms in GetTestCases(method))
            {
                yield return Builder.BuildTestMethod(method, suite, parms);
            }
        }
    }
}
