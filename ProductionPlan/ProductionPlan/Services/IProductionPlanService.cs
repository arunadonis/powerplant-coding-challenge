using System.Collections.Generic;
using ProductionPlan.Models;

namespace ProductionPlan.Services;

public interface IProductionPlanService
{
    IReadOnlyCollection<ProductionResponseDto> GetProductionPlan(ProductionRequestDto productionRequest);
}
