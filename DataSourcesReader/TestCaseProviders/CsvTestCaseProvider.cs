using System.Collections.Generic;
using DataSourcesReaders.Models;

namespace DataSourcesReaders.TestCaseProviders
{
    public class CsvTestCaseProvider : ITestCaseProvider
    {
        public IEnumerable<dynamic> GetDynamic()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetGeneric<T>() where T : new()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TestCase<TCase, TResult>> GetTestCases<TCase, TResult>()
            where TCase : new()
            where TResult : new()
        {
            throw new System.NotImplementedException();
        }
    }
}
