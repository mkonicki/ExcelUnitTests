CREATE TABLE TestCases (
	Age				INT,
	Brand			TINYINT,
	FuelType		TINYINT,
	EngineCapacity	FLOAT,
	InsuranceType	TINYINT,
	Result			FLOAT
)

  BULK 
INSERT TestCases
  FROM   '...\TestSample.csv'
  WITH (
		FIELDTERMINATOR = ',',
		ROWTERMINATOR = '\n',
		FIRSTROW=2
	   )
 