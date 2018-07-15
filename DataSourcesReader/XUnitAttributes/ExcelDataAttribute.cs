using DataSourcesReaders.XUnitAttributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataSourcesReaders
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExcelDataAttribute : BaseDataAttribute
    {
        public ExcelDataAttribute(string filePath, string sheetName)
            : base(new ExcelTestCaseProvider(filePath, sheetName))
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return GetTestCases(testMethod);
        }
    }
}