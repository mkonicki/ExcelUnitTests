using DataSourcesReaders.Models;
using System.Collections.Generic;

namespace DataSourcesReaders
{
    public interface ITestCaseProvider
    {
        IEnumerable<dynamic> GetDynamic();
        IEnumerable<T> GetGeneric<T>() where T : new();
        IEnumerable<TestCase<TCase, TResult>> GetTestCases<TCase, TResult>() where TCase : new() where TResult : new();
    }
}