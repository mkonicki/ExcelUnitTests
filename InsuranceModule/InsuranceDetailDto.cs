namespace ExcelTest
{
    public class CarInsuranceDetailDto
    {
        public int Age { get; set; }
        public CarBrand Brand { get; set; }
        public FuelType FuelType { get; set; }
        public decimal EngineCapacity { get; set; }
        public InsuranceType InsuranceType { get; set; }

        public override string ToString()
        {
            return $"{Age}, {Brand}, {FuelType}, {EngineCapacity}, {InsuranceType}";
        }
    }

    public class CarInsuranceDetailTestCase : CarInsuranceDetailDto
    {
        public decimal Result { get; set; }
    }

    public enum CarBrand
    {
        Volvo = 1,
        BMW = 2,
        VW = 3,
        Ford = 4
    }

    public enum FuelType
    {
        Diesel = 1,
        Petrol = 2
    }

    public enum InsuranceType
    {
        OC = 1,
        AC = 2,
        OCAC = 3
    }
}
