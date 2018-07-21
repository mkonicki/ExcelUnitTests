using DataSourcesReaders.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataSourcesReaders
{
    public class ExcelTestCaseProvider : ITestCaseProvider
    {
        private readonly string FilePath;
        private readonly string SheetName;
        private const int FirstDataRow = 2;
        private const int LabelDataRow = 1;

        public ExcelTestCaseProvider(string filePath, string sheetName)
        {
            FilePath = $"{Directory.GetCurrentDirectory()}\\{filePath}";
            SheetName = sheetName;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IEnumerable<dynamic> GetDynamic() =>
            ReadDataFromSource<dynamic>(() => new ExpandoObject(), (tc, k, v) =>
            {
                var testDataAsDictionary = (ICollection<KeyValuePair<string, object>>)tc;
                testDataAsDictionary.Add(new KeyValuePair<string, object>(k, v));
            });

        public IEnumerable<T> GetGeneric<T>() where T : new()
            => ReadDataFromSource(() => Activator.CreateInstance<T>(), (tc, k, v) => tc.SetCastedValue(k, v));

        public IEnumerable<TestCase<TCase, TResult>> GetTestCases<TCase, TResult>()
            where TCase : new()
            where TResult : new()
            => ReadDataFromSource(() => Activator.CreateInstance<TestCase<TCase, TResult>>(), (tc, k, v) => tc.SetCastedValue(k, v));

        private IEnumerable<T> ReadDataFromSource<T>(Func<T> initializeObject, Action<T, string, object> setupValue)
            where T : new()
            => ReadDataFromSource(new TestCaseWrapper<T>(initializeObject, setupValue));

        private IEnumerable<T> ReadDataFromSource<T>(TestCaseWrapper<T> testCaseWrapper)
            where T : new()
        {
            using (var excelPackage = new ExcelPackage(new FileInfo(FilePath)))
            {
                var worksheets = excelPackage.Workbook.Worksheets;
                var sheet = worksheets.ToList().First(s => s.Name == SheetName);

                for (int rowIndex = FirstDataRow; rowIndex <= sheet.Dimension.End.Row; rowIndex++)
                {
                    yield return GetTestDataObject(testCaseWrapper,
                        new TestSourceWrapper<ExcelWorksheet, int>(sheet, rowIndex));
                }
            }
        }

        private T GetTestDataObject<T>(TestCaseWrapper<T> testCaseWrapper, TestSourceWrapper<ExcelWorksheet, int> testSource)
            where T : new()
        {
            var testCase = testCaseWrapper.Initialize.Invoke();
            var endColumnIndex = testSource.Table.Dimension.End.Column;

            for (int columnIndex = LabelDataRow; columnIndex <= endColumnIndex; columnIndex++)
            {
                var labelCell = testSource.Table.Cells[LabelDataRow, columnIndex];
                var key = labelCell.Value.ToString();
                var value = testSource.Table.Cells[testSource.Row, columnIndex].Value;

                testCaseWrapper.SetupValue.Invoke(testCase, key, value);
            }

            return testCase;
        }
    }
}
