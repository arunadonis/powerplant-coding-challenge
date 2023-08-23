using Microsoft.Extensions.Logging;
using ProductionPlan.Helpers;
using ProductionPlan.LoadServices.Interfaces;
using ProductionPlan.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlan.LoadServices;

public class TurbojetPlantService : ITurbojetPlantService
{
    private readonly ILogger<GasPlantService> _logger;

    public TurbojetPlantService(ILogger<GasPlantService> logger)
    {
        _logger = logger;
    }

    public List<PowerPlant> GetPowerPlantsNormalized(ProductionRequestDto productionRequest)
    {
        var turbojetPlantsDtos = productionRequest.Powerplants
            .Where(x => x.Type == PlantType.TurboJet);

        var turbojetPlants = turbojetPlantsDtos.
            Select(_ => new PowerPlant
            {
                Name = _.Name,
                Type = PlantType.TurboJet,
                Pmax = _.Pmax,
                Pmin = _.Pmin,
                PricePerMWh = CalculateCostOfOperationPerMWh(_, productionRequest.Fuels.KerosineEuroMWh)
            })
            .ToList();

        return turbojetPlants;
    }

    public decimal GetLoad(PowerPlant powerPlant, decimal requiredLoad, ICollection<ProductionResponseDto> response)
    {
        var productionResponseDto = new ProductionResponseDto
        {
            Name = powerPlant.Name,
            P = powerPlant.Pmax,
        };

        requiredLoad -= powerPlant.Pmax;
        if (requiredLoad < decimal.Zero)
        {
            DecreasePowerHelper.DecreasePowerToRequired(requiredLoad, productionResponseDto, powerPlant, response);
            requiredLoad = 0;
        }

        response.Add(productionResponseDto);

        return requiredLoad;
    }

    private static double CalculateCostOfOperationPerMWh(PowerPlantDto powerPlant, double kerosineEuroMWh)
    {
        return kerosineEuroMWh / powerPlant.Efficiency;
    }
}