using ExcelReader;
using InsuranceModule;
using NUnit.Framework;

namespace ExcelMSTest
{
    [TestFixture]
    public class ExcelTestSample
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory;

        public ExcelTestSample() => _calculationFactory = new CarInsuranceCalculationFactory();

        [Test]
        [ExcelTestCaseSource("TestSample.xlsx", "CarInsurance")]
        public void InsuranceTest(dynamic testData)
        {
            //ARRENGE
            var testCase = new CarInsuranceDetailDto
            {
                Age = (int)testData.Age,
                Brand = (CarBrand)testData.Brand,
                EngineCapacity = (decimal)testData.EngineCapacity,
                FuelType = (FuelType)testData.FuelType,
                InsuranceType = (InsuranceType)testData.InsuranceType
            };

            //ACT
            var insuranceCost = _calculationFactory.Calculate(testCase);

            //ASSERT
            Assert.AreEqual((decimal)testData.Result, insuranceCost);
        }

        [Test]
        [ExcelTestCaseSource("TestSample.xlsx", "CarInsurance", typeof(CarInsuranceDetailTestCase))]
        public void InsuranceTest(CarInsuranceDetailTestCase testData)
        {
            //ARRENGE
            var testCase = new CarInsuranceDetailDto
            {
                Age = testData.Age,
                Brand = testData.Brand,
                EngineCapacity = testData.EngineCapacity,
                FuelType = testData.FuelType,
                InsuranceType = testData.InsuranceType
            };

            //ACT
            var insuranceCost = _calculationFactory.Calculate(testCase);

            //ASSERT
            Assert.AreEqual(testData.Result, insuranceCost);
        }
    }
}