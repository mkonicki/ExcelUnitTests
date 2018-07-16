using System;
using System.Linq;

namespace DataSourcesReaders.Models
{
    public class TestCase<TCase, TResult>
        where TCase : new()
        where TResult : new()
    {
        public TCase Case { get; set; }
        public TResult Result { get; set; }

        public TestCase()
        {
            Case = Activator.CreateInstance<TCase>();
            Result = Activator.CreateInstance<TResult>();
        }

        public void SetCastedValue(string key, object value)
        {
            var properties = this.GetFlattenProperties(GetType()).ToArray();

            var property = properties.Single(p => p.Property.Name == key);

            property.Value.SetCastedValue(property.Property.Name, value);
        }
    }
}
