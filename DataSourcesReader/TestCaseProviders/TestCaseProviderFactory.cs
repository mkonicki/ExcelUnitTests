using DataSourcesReaders.Models;
using DataSourcesReaders.TestCaseProviders.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace DataSourcesReaders.TestCaseProviders
{
    public class TestCaseProviderFactory : ITestCaseProviderFactory
    {
        public ITestCaseProvider TestCaseProvider { get; }

        public TestCaseProviderFactory(ITestCaseProvider testCaseProvider)
        {
            TestCaseProvider = testCaseProvider;
        }

        public MethodInfo GetProviderMethod(Type parameterType)
        {
            if (parameterType == typeof(object))
            {
                return TestCaseProvider.GetType().GetMethod(nameof(ITestCaseProvider.GetDynamic));
            }

            if (!(parameterType.IsGenericType
                && parameterType.Name.Contains(nameof(TestCase<dynamic, dynamic>))))
            {
                var genericMethodDeclaration = TestCaseProvider.GetType().GetMethods()
                    .First(m => m.Name == nameof(ITestCaseProvider.GetGeneric) && m.IsGenericMethod);

                return genericMethodDeclaration.MakeGenericMethod(new Type[] { parameterType });
            }

            var properties = parameterType.GetProperties();

            var caseType = properties
                .Single(s => s.Name == nameof(TestCase<dynamic, dynamic>.Case)).PropertyType;

            var resultType = properties
                .Single(s => s.Name == nameof(TestCase<dynamic, dynamic>.Result)).PropertyType;

            var methodDeclaration = TestCaseProvider.GetType().GetMethods()
                .First(m => m.Name == nameof(ITestCaseProvider.GetTestCases) && m.IsGenericMethod);

            return methodDeclaration.MakeGenericMethod(new Type[] { @caseType, resultType });
        }
    }
}
