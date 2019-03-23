using System;
using System.Collections.Generic;
using DataSourcesReaders.TestCaseProviders;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class JsonTestCaseSourceAttribute : BaseDataSourceAttribute
    {
        public JsonTestCaseSourceAttribute(string filePath) 
            : base(new JsonTestCaseProvider(filePath))
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
