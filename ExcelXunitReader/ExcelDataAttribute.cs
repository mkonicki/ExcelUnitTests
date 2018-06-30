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

        public ExcelDataAttribute(string filePath, string sheetName)
        {
            FilePath = filePath;
            SheetName = sheetName;
        }

        public override IEnumerable<dynamic[]> GetData(MethodInfo testMethod)
        {
            var package = new ExcelDataReader(FilePath, SheetName);

            return package.GetData().Select(s => new object[] { s });

        }
    }
}