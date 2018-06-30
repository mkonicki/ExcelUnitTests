using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace ExcelReader
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ExcelTestCaseSourceAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
    {
        private readonly NUnitTestCaseBuilder _builder;
        private readonly string FilePath;
        private readonly string SheetName;
        private readonly Type DataType;


        public ExcelTestCaseSourceAttribute(string filePath, string sheetName) : this(filePath, sheetName, null)
        {

        }

        public ExcelTestCaseSourceAttribute(string filePath, string sheetName, Type dataType)
        {
            FilePath = filePath;
            SheetName = sheetName;
            DataType = dataType;
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
            var package = new ExcelDataReader(FilePath, SheetName);

            if (DataType == null)
            {
                return package.GetData().Select(s => new TestCaseParameters(new object[] { s }));
            }

            var getDataMethod = package.GetType().GetMethods().First(m => m.Name == "GetData" && m.IsGenericMethod);

            var getData = getDataMethod.MakeGenericMethod(new Type[] { DataType });

            return ((IEnumerable<object>)getData.Invoke(package, null)).Select(s => new TestCaseParameters(new object[] { s }));
        }
    }
}
