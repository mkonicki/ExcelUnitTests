using DataSourcesReaders.Models;
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
        public void InsuranceTestObject(dynamic testCase)
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
            Assert.Equal((decimal)testCase.Result, insuranceCost);
        }

        [Theory]
        [DbData("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
        public void InsuranceTestGeneric(CarInsuranceDetailTestCase testCase)
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
            Assert.Equal(testCase.Result, insuranceCost);
        }

        [Theory]
        [DbData("TestSample.xlsx", "CarInsurance")]
        public void InsuranceTestTestCase(TestCase<CarInsuranceDetailDto, decimal> testCase)
        {
            //ACT
            var insuranceCost = _calculationFactory.Calculate(testCase.Case);

            //ASSERT
            Assert.Equal(testCase.Result, insuranceCost);
        }
    }
}