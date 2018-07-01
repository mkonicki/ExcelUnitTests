using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace ExcelReader
{
    public class ExcelDataReader
    {
        private readonly string FilePath;
        private readonly string SheetName;
        private const int FirstDataRow = 2;
        private const int LabelDataRow = 1;

        public ExcelDataReader(string filePath, string sheetName)
        {
            FilePath = $"{Directory.GetCurrentDirectory()}\\{filePath}";
            SheetName = sheetName;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IEnumerable<dynamic> GetData() =>
            GetData<dynamic>(() => new ExpandoObject(), (tc, k, v) =>
            {
                var testDataAsDictionary = (ICollection<KeyValuePair<string, object>>)tc;
                testDataAsDictionary.Add(new KeyValuePair<string, object>(k, v));
            });

        public IEnumerable<T> GetData<T>() =>
            GetData(() => Activator.CreateInstance<T>(), (tc, k, v) => tc.SetCastedValue(k, v));
        
        private IEnumerable<T> GetData<T>(Func<T> initializeTestDataObject,
            Action<T, string, object> setupPropertyValue)
        {
            using (var excelPackage = new ExcelPackage(new FileInfo(FilePath)))
            {

                var worksheets = excelPackage.Workbook.Worksheets;
                var sheet = worksheets.ToList().First(s => s.Name == SheetName);

                for (int i = FirstDataRow; i <= sheet.Dimension.End.Row; i++)
                {
                    var excelrow = sheet.Row(i);

                    var testCase = initializeTestDataObject.Invoke();

                    for (int j = LabelDataRow; j <= sheet.Dimension.End.Column; j++)
                    {
                        var labelCell = sheet.Cells[LabelDataRow, j];
                        var key = labelCell.Value.ToString();
                        var value = sheet.Cells[i, j].Value;

                        setupPropertyValue.Invoke(testCase, key, value);
                    }

                    yield return testCase;
                }
            }
        }
    }
}
