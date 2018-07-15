using DataSourcesReaders.XUnitAttributes;
using InsuranceModule;
using Xunit;

namespace ExcelTest
{
    public class DbTestSample
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory =
            new CarInsuranceCalculationFactory();

        [Theory]
        [DbData("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
        public void SampleExcelTestDynamic(dynamic testData)
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
            Assert.Equal((decimal)testData.Result, insuranceCost);
        }

        [Theory]
        [DbData("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
        public void SampleExcelTestStonglyTyped(CarInsuranceDetailTestCase testData)
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
            Assert.Equal(testData.Result, insuranceCost);
        }
    }
}