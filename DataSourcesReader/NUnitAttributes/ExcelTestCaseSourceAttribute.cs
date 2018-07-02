using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace DataSourcesReaders
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExcelTestCaseSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        private readonly ITestCaseReader _testCaseReader;
        private readonly NUnitTestCaseBuilder _builder;
        private readonly Type DataType;

        public ExcelTestCaseSourceAttribute(string filePath, string sheetName) : this(filePath, sheetName, null)
        {
        }

        public ExcelTestCaseSourceAttribute(string filePath, string sheetName, Type dataType)
        {
            DataType = dataType;

            _testCaseReader = new ExcelTestCaseReader(filePath, sheetName);
            _builder = new NUnitTestCaseBuilder();
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (TestCaseParameters testCases in GetTestCasesFor(method))
            {
                yield return _builder.BuildTestMethod(method, suite, testCases);
            }
        }

        private IEnumerable<TestCaseParameters> GetTestCasesFor(IMethodInfo method)
        {
            if (DataType == null)
            {
                return _testCaseReader.GetData().Select(s => new TestCaseParameters(new[] { s }));
            }

            var getDataMethod = _testCaseReader.GetType().GetMethods()
                .First(m => m.Name == nameof(ExcelTestCaseReader.GetData) && m.IsGenericMethod);

            var getData = getDataMethod.MakeGenericMethod(DataType);

            return ((IEnumerable<TestCaseParameters>)getData.Invoke(_testCaseReader, null)).Select(s => new TestCaseParameters(new[] { s }));
        }
    }
}
