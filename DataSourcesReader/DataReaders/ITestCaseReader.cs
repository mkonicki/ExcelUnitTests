using System.Collections.Generic;

namespace DataSourcesReaders
{
    public interface ITestCaseReader
    {
        IEnumerable<dynamic> GetData();
        IEnumerable<T> GetData<T>() where T : new();
    }
}