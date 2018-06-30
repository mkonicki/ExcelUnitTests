
# DDT(Data-Driven Testing) is alive!  
Sample of usage DDT pattern with XUnit and NUnit libraries.

## What is it? 
 In this solution I want to provide sample how to use external files, to provide highest code quality, by preparing thousands of unit tests in one simple [XUnit](https://xunit.github.io/), or [NUnit](http://nunit.org/) test.
 In data attributes I used [EPPlus.Core](https://github.com/VahidN/EPPlus.Core) to read excel files! Thx a lot guys!
That's my tutorial how I prepared my module to go life on production with minimal number of bugs, only with creating one simple, easy and readable unit test. 
That's just my extension to XUnit and NUnit to provide more and better (yeah I know, how it looks like, but I'm not vegan, hipster with lumberjack beard).

## What do I get?  
Nothing special just an idea, how to minimalize stress and hot fixes after relase.

## How is it done?  
I created simple Data Source Providers to read from excel spreadsheet (CSV and DB providers are during implementation).
### XUnit
 - [ExcelDataAttribute.cs](https://github.com/mkonicki/ExcelUnitTests/blob/master/ExcelXunitReader/ExcelDataAttribute.cs)

### NUnit
 - [ExcelTestCaseSourceAttribute](https://github.com/mkonicki/ExcelUnitTests/blob/master/ExcelXunitReader/ExcelTestCaseSourceAttribute.cs)

## Sample of usage

To present how use attributes, and DDT pattern in practice, I created simple factory to calculate [Car Insurance](https://github.com/mkonicki/ExcelUnitTests/blob/master/InsuranceModule/InsuranceCalculationFactory.cs).

### XUnit

    [Theory]
       [ExcelData("TestSample.xlsx", "CarInsurance")]
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


### NUnit


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

## Blog
Soon I will finish post on my blog, how I used in practice very similar mechanism to create thousands of unit tests for risk calculation in my project for Fenergo.

## Any Question?
If you want to know more, or just you want to use it in your project feel free to contact me, open an issue.
