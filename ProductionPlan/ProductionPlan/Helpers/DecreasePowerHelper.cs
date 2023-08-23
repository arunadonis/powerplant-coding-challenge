using ProductionPlan.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ProductionPlan.Helpers
{
    internal static class DecreasePowerHelper
    {
        public static void DecreasePowerToRequired(decimal requiredLoad, ProductionResponseDto productionResponseDto,
            PowerPlant powerPlant, ICollection<ProductionResponseDto> response)
        {
            productionResponseDto.P += requiredLoad;

            if (productionResponseDto.P < powerPlant.Pmin)
            {
                productionResponseDto.P = powerPlant.Pmin;
                requiredLoad += powerPlant.Pmax - powerPlant.Pmin;

                DescreaseLastPlantPower(requiredLoad, response);
            }
        }

        private static void DescreaseLastPlantPower(decimal requiredLoad, ICollection<ProductionResponseDto> response)
        {
            var powerPlantToDecrease = response.Last();
            powerPlantToDecrease.P += requiredLoad;
            // !TODO: What if is lower than minimun again? 
        }
    }
}
