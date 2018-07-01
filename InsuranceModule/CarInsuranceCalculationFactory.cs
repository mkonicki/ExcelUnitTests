using System;

namespace InsuranceModule
{
    public class CarInsuranceCalculationFactory : ICarInsuranceCalculationFactory
    {
        public decimal Calculate(CarInsuranceDetailDto insuranceDetailDto)
        {
            switch (insuranceDetailDto.InsuranceType)
            {
                case InsuranceType.OC:
                    return Math.Round(CalculateOc(insuranceDetailDto), 2);
                case InsuranceType.AC:
                    return Math.Round(CalculateAc(insuranceDetailDto), 2);
                case InsuranceType.OCAC:
                    return Math.Round(CalculateOcAc(insuranceDetailDto), 2);
            }

            return 0;
        }

        private decimal CalculateOcAc(CarInsuranceDetailDto insuranceDetailDto)
        {
            return (CalculateAc(insuranceDetailDto) + CalculateOc(insuranceDetailDto)) * 0.91m;
        }

        private decimal CalculateAc(CarInsuranceDetailDto insuranceDetailDto)
        {
            switch (insuranceDetailDto.Brand)
            {
                case CarBrand.Volvo:
                    return ((int)insuranceDetailDto.FuelType) * 13 + insuranceDetailDto.Age * 12 + 32.5m;
                case CarBrand.BMW:
                    return ((int)insuranceDetailDto.FuelType) * 21 + insuranceDetailDto.Age * 25 + 23.7m;
                case CarBrand.VW:
                    return ((int)insuranceDetailDto.FuelType) * 16 + insuranceDetailDto.Age * 21 + 21.94m;
                case CarBrand.Ford:
                    return ((int)insuranceDetailDto.FuelType) * 18 + insuranceDetailDto.Age * 19 + 18.87m;
            }

            return 0;
        }

        private decimal CalculateOc(CarInsuranceDetailDto insuranceDetailDto)
        {
            switch (insuranceDetailDto.Brand)
            {
                case CarBrand.Volvo:
                    return ((int)insuranceDetailDto.FuelType) * 23 + insuranceDetailDto.Age * 25 + 73 + insuranceDetailDto.EngineCapacity * 5;
                case CarBrand.BMW:
                    return ((int)insuranceDetailDto.FuelType) * 19 + insuranceDetailDto.Age * 14 + 61 + insuranceDetailDto.EngineCapacity * 3;
                case CarBrand.VW:
                    return ((int)insuranceDetailDto.FuelType) * 31 + insuranceDetailDto.Age * 19 + 53 + insuranceDetailDto.EngineCapacity * 2.5m;
                case CarBrand.Ford:
                    return ((int)insuranceDetailDto.FuelType) * 11 + insuranceDetailDto.Age * 31 + 84 + insuranceDetailDto.EngineCapacity * 1.32m;
            }

            return 0;
        }
    }

    public interface ICarInsuranceCalculationFactory
    {
        decimal Calculate(CarInsuranceDetailDto insuranceDetailDto);
    }
}
