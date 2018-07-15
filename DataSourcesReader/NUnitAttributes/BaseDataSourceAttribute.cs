using DataSourcesReaders.Models;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataSourcesReaders.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class BaseDataSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        protected readonly ITestCaseProvider TestCaseProvider;
        protected readonly NUnitTestCaseBuilder Builder = new NUnitTestCaseBuilder();

        protected BaseDataSourceAttribute(ITestCaseProvider testCaseReader)
        {
            TestCaseProvider = testCaseReader;
        }

        public abstract IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite);

        protected IEnumerable<TestCaseParameters> GetTestCases(IMethodInfo testMethodInfo)
        {
            var parameterType = testMethodInfo.GetParameters().First().ParameterType;

            if (parameterType == typeof(object))
            {
                return TestCaseProvider.Get().Select(s => new TestCaseParameters(new[] { s }));
            }

            var method = InitializeGetDataMethod(parameterType);

            var testCases = method.Invoke(TestCaseProvider, null);

            return ((IEnumerable<object>)testCases).Select(
                s => new TestCaseParameters(new[] { s }));
        }


        private MethodInfo InitializeGetDataMethod(Type parameter)
        {
            if (!(parameter.IsGenericType
                && parameter.Name.Contains(nameof(TestCase<dynamic, dynamic>))))
            {
                var getMethod = TestCaseProvider.GetType().GetMethods()
                    .First(m => m.Name == nameof(ITestCaseProvider.Get) && m.IsGenericMethod);

                return getMethod.MakeGenericMethod(new Type[] { parameter });
            }

            var testCaseMethod = TestCaseProvider.GetType().GetMethods()
                .First(m => m.Name == nameof(ITestCaseProvider.GetTestCase) && m.IsGenericMethod);

            var properties = parameter.GetProperties();

            var caseType = properties
                .First(s => s.Name == nameof(TestCase<dynamic, dynamic>.Case)).PropertyType;

            var resultType = properties
                .First(s => s.Name == nameof(TestCase<dynamic, dynamic>.Result)).PropertyType;

            var methodDeclaration = TestCaseProvider.GetType().GetMethods()
                .First(m => m.Name == nameof(ITestCaseProvider.GetTestCase) && m.IsGenericMethod);

            return methodDeclaration.MakeGenericMethod(new Type[] { @caseType, resultType });
        }
    }
}
