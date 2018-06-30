using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using OfficeOpenXml;

namespace ExcelReader
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ExcelTestCaseSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        private readonly NUnitTestCaseBuilder _builder;
        private readonly string FilePath;
        private readonly string SheetName;

        public ExcelTestCaseSourceAttribute(string filePath, string sheetName)
        {
            FilePath = $"{Directory.GetCurrentDirectory()}\\{filePath}";
            SheetName = sheetName;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _builder = new NUnitTestCaseBuilder();
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (TestCaseParameters parms in GetTestCasesFor(method))
            {
                yield return _builder.BuildTestMethod(method, suite, parms);
            }
        }

        private IEnumerable<TestCaseParameters> GetTestCasesFor(IMethodInfo method)
        {
            using (var package = new ExcelPackage(new FileInfo(FilePath)))
            {
                var worksheets = package.Workbook.Worksheets;
                var sheet = worksheets.ToList().First(s => s.Name == SheetName);


                for (int i = 2; i <= sheet.Dimension.End.Row; i++)
                {
                    var eo = new ExpandoObject();
                    var expandoDic = (ICollection<KeyValuePair<string, object>>)eo;
                    var excelrow = sheet.Row(i);

                    for (int j = 1; j <= sheet.Dimension.End.Column; j++)
                    {
                        var labelCell = sheet.Cells[1, j];
                        var key = labelCell.Value.ToString();
                        expandoDic.Add(new KeyValuePair<string, object>(key, sheet.Cells[i, j].Value));
                    }

                    yield return new TestCaseParameters(new object[] { (dynamic)eo });
                }
            }
        }
    }
}
