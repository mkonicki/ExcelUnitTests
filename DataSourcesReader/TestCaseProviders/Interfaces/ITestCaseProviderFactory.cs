using System;
using System.Reflection;

namespace DataSourcesReaders.TestCaseProviders.Interfaces
{
    public interface ITestCaseProviderFactory
    {
        ITestCaseProvider TestCaseProvider { get; }
        MethodInfo GetProviderMethod(Type testMethodParameterType);
    }
}
