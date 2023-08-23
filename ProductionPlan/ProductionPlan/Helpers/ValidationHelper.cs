using ProductionPlan.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ProductionPlan.Helpers
{
    public class ValidationHelper : IValidationHelper
    {
        public void ValidateProductionLimits(IOrderedEnumerable<PowerPlant> powerPlants, decimal load)
        {
            ValidatePmin(powerPlants, load);
            ValidatePmax(powerPlants, load);
        }

        public void ValidateProductionRequest(ProductionRequestDto productionRequest)
        {
            if (productionRequest == null)
            {
                throw new ArgumentNullException(nameof(productionRequest), ErrorMessages.NullProductionPlanError);
            }
        }

        private void ValidatePmin(IEnumerable<PowerPlant> powerPlants, decimal load)
        {
            var energyMin = powerPlants.Min(x => x.Pmin);

            if (load < energyMin)
            {
                var errorMessage = string.Format(ErrorMessages.PminError, load);
                throw new InvalidOperationException(errorMessage);
            }
        }

        private void ValidatePmax(IEnumerable<PowerPlant> powerPlants, decimal load)
        {
            var energyMax = powerPlants.Sum(x => x.Pmax);

            if (load > energyMax)
            {
                var errorMessage = string.Format(ErrorMessages.PmaxError, load);
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
