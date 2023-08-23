using ProductionPlan.Models;
using System.Linq;

namespace ProductionPlan.Helpers
{
    public interface IValidationHelper
    {
        void ValidateProductionLimits(IOrderedEnumerable<PowerPlant> powerPlants, decimal load);
        void ValidateProductionRequest(ProductionRequestDto productionRequest);
    }
}