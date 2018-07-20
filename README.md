
# DDT(Data-Driven Testing) is alive!  
[![DDTsterNuGet](https://img.shields.io/nuget/v/DDTster.svg?label=DDTster)](https://www.nuget.org/packages/DDTster)<br />
Sample of usage DDT pattern with XUnit and NUnit libraries.

## What is it? 
 In this solution I want to provide sample how to use external files, to provide highest code quality, by preparing thousands of unit tests in one simple [XUnit](https://xunit.github.io/), or [NUnit](http://nunit.org/) test.
 In data attributes I used [EPPlus.Core](https://github.com/VahidN/EPPlus.Core) to read excel files! Thx a lot guys!
That's my tutorial how I prepared my module to go life on production with minimal number of bugs, only with creating one simple, easy and readable unit test. 
That's just my extension to XUnit and NUnit to provide more and better (yeah I know, how it looks like, but I'm not vegan, hipster with lumberjack beard).

## What do I get?  
Nothing special just an idea, how to minimalize stress and hot fixes after relase.

## How is it done?  
I created simple Data Source Providers to read from mssql database, or excel spreadsheet (CSV provider, and another database providers are during implementation). Each attributes has 2 cases of usage, dynamic, where simple test case is provided to method as dynamic object, or strongly typed where we provide type as parameter in attribute.
## Download

The easiest way to download is via NuGet:
```
Install-Package DDTster
```
## Attributes
### XUnit
 - [DbDataAttribute.cs](https://github.com/mkonicki/DDT-Unit-Tests/blob/master/DataSourcesReader/XUnitAttributes/DbDataAttribute.cs)
 
	- dynamic `[DbData("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]`
	
	- strongly typed, or TestCase<Dto, Result>   `[DbData("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases", typeof(CarInsuranceDetailTestCase))]`
	
 - [ExcelDataAttribute.cs](https://github.com/mkonicki/DDT-Unit-Tests/blob/master/DataSourcesReader/XUnitAttributes/ExcelDataAttribute.cs)
 
	- dynamic `[ExcelData("TestSample.xlsx", "CarInsurance")]`
	
	- strongly typed, or TestCase<Dto, Result>   ` [ExcelData("TestSample.xlsx", "CarInsurance", typeof(CarInsuranceDetailTestCase))]`


### NUnit
 - [DbTestCaseSourceAttribute](https://github.com/mkonicki/DDT-Unit-Tests/blob/master/DataSourcesReader/NUnitAttributes/DbTestCaseSourceAttribute.cs)
  
	- dynamic -  `[DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases")]`
	
	- strongly typed, or TestCase<Dto, Result>  - 	`[DbTestCaseSource("data source=.;initial catalog=Test;integrated security=True;", "dbo.TestCases", typeof(CarInsuranceDetailTestCase))]`

 - [ExcelTestCaseSourceAttribute](https://github.com/mkonicki/DDT-Unit-Tests/blob/master/DataSourcesReader/NUnitAttributes/ExcelTestCaseSourceAttribute.cs)
  
	- dynamic -  `[ExcelTestCaseSource("TestSample.xlsx", "CarInsurance")]`
	
	- strongly typed, or TestCase<Dto, Result> - 	`[ExcelTestCaseSource("TestSample.xlsx", "CarInsurance", typeof(CarInsuranceDetailTestCase))]`


## Sample of usage

To present how use attributes, and DDT pattern in practice, I created simple factory to calculate [Car Insurance](https://github.com/mkonicki/ExcelUnitTests/blob/master/InsuranceModule/InsuranceCalculationFactory.cs).

### [XUnit](https://github.com/mkonicki/ExcelUnitTests/blob/master/ExcelTest/ExcelTestSample.cs)
    [Theory]
    [ExcelData("TestSample.xlsx", "CarInsurance")]
    public void InsuranceTestTestCase(TestCase<CarInsuranceDetailDto, decimal> testCase)
    {
	    //ACT
	    var insuranceCost = _calculationFactory.Calculate(testCase.Case);
    
	    //ASSERT
	    Assert.Equal(testCase.Result, insuranceCost);
    }

### [NUnit](https://github.com/mkonicki/ExcelUnitTests/blob/master/ExcelNUnitTest/ExcelTestSample.cs)
    [Test]    
    [ExcelTestCaseSource("TestSample.xlsx", "CarInsurance")]    
    public void InsuranceTestTestCase(TestCase<CarInsuranceDetailDto, decimal> testCase)
    {    
	   //ACT    
	   var insuranceCost = _calculationFactory.Calculate(testCase.Case);
	   
	   //ASSERT
	   Assert.AreEqual(testCase.Result, insuranceCost);
	}

## Blog
Soon I will finish post on my blog, how I used in practice very similar mechanism to create thousands of unit tests for risk calculation in my project for Fenergo.

## Any Question?
If you want to know more, or just you want to use it in your project feel free to contact me, open an issue.
