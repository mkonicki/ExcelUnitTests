using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataSourcesReaders
{
    public static class PropertyInfoHelper
    {
        public static void SetCastedValue(this object target, string propertyName, object value)
        {
            var property = target.GetType().GetProperties().First(s => s.Name == propertyName);

            SetCastedValue(target, property, value);
        }

        public static void SetCastedValue(this object target, PropertyInfo property, object value)
        {
            var castedValue = CastValueToPropertyType(property, value);

            property.SetValue(target, castedValue);
        }

        public static object CastValueToPropertyType(this PropertyInfo property, object value)
        {
            if (property.PropertyType.IsEnum)
            {
                var enumType = property.PropertyType;

                return Enum.Parse(enumType, value.ToString());
            }

            return Convert.ChangeType(value, property.PropertyType);
        }

        public static IEnumerable<FlattenPropertyInfo> GetFlattenProperty(this object target, Type type)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsValueType)
                {
                    yield return new FlattenPropertyInfo
                    {
                        Value = target is PropertyInfo ? property.GetValue(target) : target,
                        Property = property
                    };
                }

                else
                {
                    foreach (var flattenedProperties in GetFlattenProperty(property.GetValue(target), property.PropertyType))
                    {
                        yield return flattenedProperties;
                    }
                }
            }
        }

        public class FlattenPropertyInfo
        {
            public object Value { get; set; }
            public PropertyInfo Property { get; set; }
        }
    }
}