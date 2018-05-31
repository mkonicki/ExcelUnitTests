using ExcelXunitReader;
using Xunit;

namespace ExcelTest
{
    public class CarInsuranceTests
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory;

        public CarInsuranceTests()
        {
            _calculationFactory = new CarInsuranceCalculationFactory();
        }

        [Theory]
        [ExcelProvider("TestSample.xlsx", "CarInsurance")]
        public void SampleExcelTest(dynamic testData)
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
    }
}