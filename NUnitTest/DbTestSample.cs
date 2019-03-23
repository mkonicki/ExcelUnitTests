//using DataSourcesReaders.Models;
//using DataSourcesReaders.NUnitAttributes;
//using InsuranceModule;
//using NUnit.Framework;

//namespace NUnitTest
//{
//    public class DbTestSample
//    {
//        private readonly ICarInsuranceCalculationFactory _calculationFactory =
//            new CarInsuranceCalculationFactory();

//        [Test]
//        [DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
//        public void InsuranceTestDynamic(dynamic testCase)
//        {
//            //ARRENGE
//            var @case = new CarInsuranceDetailDto
//            {
//                Age = (int)testCase.Age,
//                Brand = (CarBrand)testCase.Brand,
//                EngineCapacity = (decimal)testCase.EngineCapacity,
//                FuelType = (FuelType)testCase.FuelType,
//                InsuranceType = (InsuranceType)testCase.InsuranceType
//            };

//            //ACT
//            var insuranceCost = _calculationFactory.Calculate(@case);

//            //ASSERT
//            Assert.AreEqual((decimal)testCase.Result, insuranceCost);
//        }

//        [Test]
//        [DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
//        public void InsuranceTestObject(CarInsuranceDetailTestCase testCase)
//        {
//            //ARRENGE
//            var @case = new CarInsuranceDetailDto
//            {
//                Age = testCase.Age,
//                Brand = testCase.Brand,
//                EngineCapacity = testCase.EngineCapacity,
//                FuelType = testCase.FuelType,
//                InsuranceType = testCase.InsuranceType
//            };

//            //ACT
//            var insuranceCost = _calculationFactory.Calculate(@case);

//            //ASSERT
//            Assert.AreEqual(testCase.Result, insuranceCost);
//        }

//        [Test]
//        [DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]
//        public void InsuranceTestTestCase(TestCase<CarInsuranceDetailDto, decimal> testCase)
//        {
//            //ACT
//            var insuranceCost = _calculationFactory.Calculate(testCase.Case);

//            //ASSERT
//            Assert.AreEqual(testCase.Result, insuranceCost);
//        }
//    }
//}
