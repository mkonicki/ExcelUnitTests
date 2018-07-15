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
        private readonly ITestCaseProvider TestCaseReader;

        protected BaseDataAttribute(ITestCaseProvider testCaseReader)
        {
            TestCaseReader = testCaseReader;
        }

        public IEnumerable<object[]> GetTestCases(MethodInfo testMethod)
        {
            var dataType = testMethod.GetParameters().First().ParameterType;

            if (dataType == typeof(Object))
            {
                return TestCaseReader.Get().Select(s => new object[] { s });
            }

            var genericGetDataMethod = TestCaseReader.GetType().GetMethods()
                .First(m => m.Name == nameof(ExcelTestCaseProvider.Get) && m.IsGenericMethod);

            var getDataMethod = genericGetDataMethod.MakeGenericMethod(new Type[] { dataType });

            return ((IEnumerable<object>)getDataMethod.Invoke(TestCaseReader, null)).Select(s => new object[] { s });
        }
    }
}
