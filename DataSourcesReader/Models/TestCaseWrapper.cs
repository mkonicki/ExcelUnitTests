using System;

namespace DataSourcesReaders.Models
{
    public class TestCaseWrapper<T> where T : new()
    {
        public Func<T> Initialize { get; }
        public Action<T, string, object> SetupValue { get; }

        public TestCaseWrapper(Func<T> initialize, Action<T, string, object> setupValue)
        {
            Initialize = initialize;
            SetupValue = setupValue;
        }
    }
}
