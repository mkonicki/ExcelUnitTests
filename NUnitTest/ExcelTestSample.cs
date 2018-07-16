using DataSourcesReaders;
using DataSourcesReaders.Models;
using InsuranceModule;
using NUnit.Framework;

namespace NUnitTest
{
    [TestFixture]
    public class ExcelTestSample
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory =
            new CarInsuranceCalculationFactory();

        [Test]
        [ExcelTestCaseSource("TestSample.xlsx", "CarInsurance")]
        public void InsuranceTestDynamic(dynamic testCase)
        {
            //ARRENGE
            var @case = new CarInsuranceDetailDto
            {
                Age = (int)testCase.Age,
                Brand = (CarBrand)testCase.Brand,
                EngineCapacity = (decimal)testCase.EngineCapacity,
                FuelType = (FuelType)testCase.FuelType,
                InsuranceType = (InsuranceType)testCase.InsuranceType
            };

            //ACT
            var insuranceCost = _calculationFactory.Calculate(@case);

            //ASSERT
            Assert.AreEqual((decimal)testCase.Result, insuranceCost);
        }

        [Test]
        [ExcelTestCaseSource("TestSample.xlsx", "CarInsurance")]
        public void InsuranceTestObject(CarInsuranceDetailTestCase testCase)
        {
            //ARRENGE
            var @case = new CarInsuranceDetailDto
            {
                Age = testCase.Age,
                Brand = testCase.Brand,
                EngineCapacity = testCase.EngineCapacity,
                FuelType = testCase.FuelType,
                InsuranceType = testCase.InsuranceType
            };

            //ACT
            var insuranceCost = _calculationFactory.Calculate(@case);

            //ASSERT
            Assert.AreEqual(testCase.Result, insuranceCost);
        }

        [Test]
        [ExcelTestCaseSource("TestSample.xlsx", "CarInsurance")]
        public void InsuranceTestTestCase(TestCase<CarInsuranceDetailDto, decimal> testCase)
        {
            //ACT
            var insuranceCost = _calculationFactory.Calculate(testCase.Case);

            //ASSERT
            Assert.AreEqual(testCase.Result, insuranceCost);
        }
    }
}