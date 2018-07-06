using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class BaseDataSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        protected readonly ITestCaseReader TestCaseReader;
        protected readonly NUnitTestCaseBuilder Builder = new NUnitTestCaseBuilder();
        protected readonly Type DataType;

        protected BaseDataSourceAttribute(ITestCaseReader testCaseReader, Type dataType)
        {
            TestCaseReader = testCaseReader;
            DataType = dataType;
        }

        public abstract IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite);

        protected IEnumerable<TestCaseParameters> GetTestCases()
        {
            if (DataType == null)
            {
                return TestCaseReader.GetData().Select(s => new TestCaseParameters(new[] { s }));
            }

            var getDataMethod = TestCaseReader.GetType().GetMethods()
                .First(m => m.Name == nameof(ITestCaseReader.GetData) && m.IsGenericMethod);

            var getData = getDataMethod.MakeGenericMethod(DataType);

            return ((IEnumerable<object>)getData.Invoke(TestCaseReader, null)).Select(
                s => new TestCaseParameters(new[] { s }));
        }
    }
}
