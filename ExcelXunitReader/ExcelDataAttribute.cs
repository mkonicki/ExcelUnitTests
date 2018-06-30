using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExcelReader;
using Xunit.Sdk;

namespace ExcelXunitReader
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExcelDataAttribute : DataAttribute
    {
        private readonly string FilePath;
        private readonly string SheetName;
        private readonly Type DataType;

        public ExcelDataAttribute(string filePath, string sheetName) : this(filePath, sheetName, null)
        {
            FilePath = filePath;
            SheetName = sheetName;
        }

        public ExcelDataAttribute(string filePath, string sheetName, Type dataType)
        {
            FilePath = filePath;
            SheetName = sheetName;
            DataType = dataType;
        }

        public override IEnumerable<dynamic[]> GetData(MethodInfo testMethod)
        {
            var package = new ExcelDataReader(FilePath, SheetName);

            if (DataType == null)
            {
                return package.GetData().Select(s => new object[] { s });
            }

            var genericGetDataMethod = package.GetType().GetMethods()
                .First(m => m.Name == nameof(ExcelDataReader.GetData) && m.IsGenericMethod);

            var getDataMethod = genericGetDataMethod.MakeGenericMethod(new Type[] { DataType });

            return ((IEnumerable<object>)getDataMethod.Invoke(package, null)).Select(s => new object[] { s });
        }
    }
}