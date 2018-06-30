using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OfficeOpenXml;
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
            FilePath = $"{Directory.GetCurrentDirectory()}\\{filePath}";
            SheetName = sheetName;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public override IEnumerable<dynamic[]> GetData(MethodInfo testMethod)
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

                    yield return new object[] { (dynamic)eo };
                }
            }
        }
    }
}