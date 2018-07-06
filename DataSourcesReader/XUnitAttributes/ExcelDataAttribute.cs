using System;
using System.Collections.Generic;
using System.Reflection;
using DataSourcesReaders.XUnitAttributes;

namespace DataSourcesReaders
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExcelDataAttribute : BaseDataAttribute
    {
        public ExcelDataAttribute(string filePath, string sheetName) : this(filePath, sheetName, null)
        {
        }

        public ExcelDataAttribute(string filePath, string sheetName, Type dataType)
            : base(new ExcelTestCaseReader(filePath, sheetName), dataType)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return GetTestCases();
        }
    }
}