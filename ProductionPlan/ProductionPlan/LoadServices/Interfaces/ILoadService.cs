using ProductionPlan.Models;
using System.Collections.Generic;

namespace ProductionPlan.LoadServices.Interfaces
{
    public interface ILoadService
    {
        List<PowerPlant> GetPowerPlantsNormalized(ProductionRequestDto productionRequest);

        decimal GetLoad(PowerPlant powerPlant, decimal requiredLoad, ICollection<ProductionResponseDto> response);
    }
}