using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using DataSourcesReaders.Models;
using Newtonsoft.Json;

namespace DataSourcesReaders.TestCaseProviders
{
    public class JsonTestCaseProvider : ITestCaseProvider
    {
        private readonly string FilePath;

        public JsonTestCaseProvider(string filePath)
        {
            FilePath = filePath;
        }

        public IEnumerable<dynamic> GetDynamic() =>
            ReadDataFromSource<dynamic>(() => new ExpandoObject(), (tc, k, v) =>
            {
                var testDataAsDictionary = (ICollection<KeyValuePair<string, object>>)tc;
                testDataAsDictionary.Add(new KeyValuePair<string, object>(k, v));
            });

        public IEnumerable<T> GetGeneric<T>() where T : new()
            => ReadDataFromSource(() => Activator.CreateInstance<T>(), (tc, k, v) => tc.SetCastedValue(k, v));

        public IEnumerable<TestCase<TCase, TResult>> GetTestCases<TCase, TResult>()
            where TCase : new()
            where TResult : new()
            => ReadDataFromSource(() => Activator.CreateInstance<TestCase<TCase, TResult>>(), (tc, k, v) => tc.SetCastedValue(k, v));

        private IEnumerable<T> ReadDataFromSource<T>(Func<T> initializeObject, Action<T, string, object> setupValue)
            where T : new()
            => ReadDataFromSource(new TestCaseWrapper<T>(initializeObject, setupValue));

        private IEnumerable<T> ReadDataFromSource<T>(TestCaseWrapper<T> testCaseWrapper)
           where T : new()
        {
            using (var sr = new StreamReader(FilePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var @object = JsonConvert.DeserializeObject<T>(line);

                    yield return GetTestDataObject(testCaseWrapper, line);
                }
            }
        }

        private T GetTestDataObject<T>(TestCaseWrapper<T> testCaseWrapper, string @case)
            where T : new()
        {
            var @object = JsonConvert.DeserializeObject<T>(@case);

            if (typeof(T) != typeof(TestCase<,>))
            {
                return @object;
            }

            var testCase = testCaseWrapper.Initialize.Invoke();
            var properties = @object.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(@object);

                testCaseWrapper.SetupValue.Invoke(testCase, property.Name, value);
            }

            return testCase;
        }
    }
}
