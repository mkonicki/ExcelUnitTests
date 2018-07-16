using DataSourcesReaders.NUnitAttributes;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace DataSourcesReaders
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExcelTestCaseSourceAttribute : BaseDataSourceAttribute
    {
        public ExcelTestCaseSourceAttribute(string filePath, string sheetName)
            : base(new ExcelTestCaseProvider(filePath, sheetName))
        {
        }

        public override IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (var testCases in GetTestCases(method))
            {
                yield return Builder.BuildTestMethod(method, suite, testCases);
            }
        }
    }
}
