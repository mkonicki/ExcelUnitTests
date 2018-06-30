using ExcelXunitReader;
using Xunit;

namespace ExcelTest
{
    public class CarInsuranceTests
    {
        private readonly ICarInsuranceCalculationFactory _calculationFactory;

        public CarInsuranceTests() => _calculationFactory = new CarInsuranceCalculationFactory();

        [Theory]
        [ExcelData("TestSample.xlsx", "CarInsurance")]
        public void SampleExcelTest(dynamic testData)
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