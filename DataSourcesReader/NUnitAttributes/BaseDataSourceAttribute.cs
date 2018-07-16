using DataSourcesReaders.TestCaseProviders;
using DataSourcesReaders.TestCaseProviders.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class BaseDataSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        protected readonly ITestCaseProviderFactory Factory;
        protected readonly NUnitTestCaseBuilder Builder = new NUnitTestCaseBuilder();

        protected BaseDataSourceAttribute(ITestCaseProvider testCaseProvider)
        {
            Factory = new TestCaseProviderFactory(testCaseProvider);
        }

        public abstract IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite);

        protected IEnumerable<TestCaseParameters> GetTestCases(IMethodInfo testMethodInfo)
        {
            var parameterType = testMethodInfo.GetParameters().First().ParameterType;

            var method = Factory.GetProviderMethod(parameterType);

            var testCases = method.Invoke(Factory.TestCaseProvider, null);

            return ((IEnumerable<object>)testCases).Select(
                s => new TestCaseParameters(new[] { s }));
        }
    }
}
