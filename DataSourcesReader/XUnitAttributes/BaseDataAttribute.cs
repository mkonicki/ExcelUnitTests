using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace DataSourcesReaders.XUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class BaseDataAttribute : DataAttribute
    {
        private readonly Type DataType;
        private readonly ITestCaseReader TestCaseReader;

        protected BaseDataAttribute(ITestCaseReader testCaseReader, Type dataType)
        {
            DataType = dataType;
            TestCaseReader = testCaseReader;
        }

        public IEnumerable<object[]> GetTestCases()
        {
            if (DataType == null)
            {
                return TestCaseReader.GetData().Select(s => new object[] { s });
            }

            var genericGetDataMethod = TestCaseReader.GetType().GetMethods()
                .First(m => m.Name == nameof(ExcelTestCaseReader.GetData) && m.IsGenericMethod);

            var getDataMethod = genericGetDataMethod.MakeGenericMethod(new Type[] { DataType });

            return ((IEnumerable<object>)getDataMethod.Invoke(TestCaseReader, null)).Select(s => new object[] { s });
        }
    }
}
