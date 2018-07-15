using DataSourcesReaders.Models;
using System.Collections.Generic;

namespace DataSourcesReaders
{
    public interface ITestCaseProvider
    {
        IEnumerable<dynamic> Get();
        IEnumerable<T> Get<T>();
        IEnumerable<TestCase<TCase, TResult>> GetTestCase<TCase, TResult>() where TCase : new() where TResult : new();
    }
}