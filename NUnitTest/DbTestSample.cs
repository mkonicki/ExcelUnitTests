using DataSourcesReaders.NUnitAttributes;
using InsuranceModule;
using NUnit.Framework;

namespace NUnitTest
{
    public class DbTestSample
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory;

        public DbTestSample() => _calculationFactory = new CarInsuranceCalculationFactory();

        [Test]
        [DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
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
        [DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases", typeof(CarInsuranceDetailTestCase))]
        public void InsuranceTestStronglyTyped(CarInsuranceDetailTestCase testData)
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
