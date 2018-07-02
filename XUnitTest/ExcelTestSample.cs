using DataSourcesReaders;
using InsuranceModule;
using Xunit;

namespace ExcelTest
{
    public class CarInsuranceTests
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory;

        public CarInsuranceTests() => _calculationFactory = new CarInsuranceCalculationFactory();

        [Theory]
        [ExcelData("TestSample.xlsx", "CarInsurance")]
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
        [ExcelData("TestSample.xlsx", "CarInsurance", typeof(CarInsuranceDetailTestCase))]
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