using DataSourcesReaders.TestCaseProviders;
using DataSourcesReaders.TestCaseProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace DataSourcesReaders.XUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class BaseDataAttribute : DataAttribute
    {
        protected readonly ITestCaseProviderFactory Factory;

        protected BaseDataAttribute(ITestCaseProvider testCaseProvider)
        {
            Factory = new TestCaseProviderFactory(testCaseProvider);
        }

        public IEnumerable<object[]> GetTestCases(MethodInfo testMethodInfo)
        {
            var parameterType = testMethodInfo.GetParameters().First().ParameterType;

            var method = Factory.GetProviderMethod(parameterType);

            var testCases = method.Invoke(Factory.TestCaseProvider, null);

            return ((IEnumerable<object>)testCases).Select(s => new object[] { s });
        }
    }
}
