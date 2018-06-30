using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OfficeOpenXml;

namespace ExcelReader
{
    public class ExcelDataReader
    {
        private readonly string FilePath;
        private readonly string SheetName;
        public ExcelDataReader(string filePath, string sheetName)
        {
            FilePath = $"{Directory.GetCurrentDirectory()}\\{filePath}";
            SheetName = sheetName;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IEnumerable<dynamic> GetData()
        {
            using (var excelPackage = new ExcelPackage(new FileInfo(FilePath)))
            {
                var worksheets = excelPackage.Workbook.Worksheets;
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

                    yield return (dynamic)eo;
                }
            }
        }

        public IEnumerable<T> GetData<T>()
        {
            using (var excelPackage = new ExcelPackage(new FileInfo(FilePath)))
            {
                var dataType = typeof(T);
                var worksheets = excelPackage.Workbook.Worksheets;
                var sheet = worksheets.ToList().First(s => s.Name == SheetName);

                for (int i = 2; i <= sheet.Dimension.End.Row; i++)
                {
                    var excelrow = sheet.Row(i);

                    var testData = (T)Activator.CreateInstance(dataType);

                    for (int j = 1; j <= sheet.Dimension.End.Column; j++)
                    {
                        var labelCell = sheet.Cells[1, j];
                        var key = labelCell.Value.ToString();
                        var value = sheet.Cells[i, j].Value;

                        var property = dataType.GetProperties().First(s => s.Name == key);
                        var propertyValue = CastPropertyValue(property, value);

                        property.SetValue(testData, propertyValue);
                    }

                    yield return testData;
                }
            }
        }

        private object CastPropertyValue(PropertyInfo property, object value)
        {
            if (property.PropertyType.IsEnum)
            {
                var enumType = property.PropertyType;

                return Enum.Parse(enumType, value.ToString());
            }

            return Convert.ChangeType(value, property.PropertyType);
        }
    }
}
