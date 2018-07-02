using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class DbTestCaseSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        private readonly NUnitTestCaseBuilder _builder;
        private readonly ITestCaseReader _testCaseReader;

        public DbTestCaseSourceAttribute(string connectionString, string tableName)
        {
            _testCaseReader = new DbDataReader(connectionString, tableName);
            _builder = new NUnitTestCaseBuilder();
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (TestCaseParameters parms in GetTestCasesFor(method))
            {
                yield return _builder.BuildTestMethod(method, suite, parms);
            }
        }

        private IEnumerable<TestCaseParameters> GetTestCasesFor(IMethodInfo method)
        {
            return _testCaseReader.GetData().Select(s => new TestCaseParameters(new object[] { s }));
        }
    }
}
