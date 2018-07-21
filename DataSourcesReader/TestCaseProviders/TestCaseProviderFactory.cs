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
        public Type TestCaseProviderType => TestCaseProvider.GetType();

        public TestCaseProviderFactory(ITestCaseProvider testCaseProvider)
        {
            TestCaseProvider = testCaseProvider;
        }

        public MethodInfo GetProviderMethod(Type parameterType)
        {
            if (parameterType == typeof(object))
            {
                return GetDynamicMethodInfo(parameterType);
            }

            if (!parameterType.IsGenericType
                || !parameterType.Name.Contains(nameof(TestCase<dynamic, dynamic>)))
            {
                return GetGenericMethodInfo(parameterType);
            }

            return GetTestCaseMethodInfo(parameterType);
        }

        private MethodInfo GetDynamicMethodInfo(Type parameterType)
        {
            return TestCaseProviderType.GetMethod(nameof(ITestCaseProvider.GetDynamic));
        }

        private MethodInfo GetGenericMethodInfo(Type parameterType)
        {
            var genericMethodDeclaration = TestCaseProviderType.GetMethod(nameof(ITestCaseProvider.GetGeneric));

            return genericMethodDeclaration.MakeGenericMethod(new Type[] { parameterType });
        }

        private MethodInfo GetTestCaseMethodInfo(Type parameterType)
        {
            var properties = parameterType.GetProperties();

            var caseType = properties
                .Single(s => s.Name == nameof(TestCase<dynamic, dynamic>.Case)).PropertyType;

            var resultType = properties
                .Single(s => s.Name == nameof(TestCase<dynamic, dynamic>.Result)).PropertyType;

            var methodDeclaration = TestCaseProviderType.GetMethod(nameof(ITestCaseProvider.GetTestCases));

            return methodDeclaration.MakeGenericMethod(new Type[] { caseType, resultType });
        }
    }
}
