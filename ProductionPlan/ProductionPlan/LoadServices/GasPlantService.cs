using Microsoft.Extensions.Logging;
using ProductionPlan.Helpers;
using ProductionPlan.LoadServices.Interfaces;
using ProductionPlan.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlan.LoadServices;

public class GasPlantService : IGasPlantService
{
    private const double Co2GeneratedPerMWh = 0.3;

    private readonly ILogger<GasPlantService> _logger;

    public GasPlantService(ILogger<GasPlantService> logger)
    {
        _logger = logger;
    }

    public List<PowerPlant> GetPowerPlantsNormalized(ProductionRequestDto productionRequest)
    {
        var gasPlantsDtos = productionRequest.Powerplants
            .Where(x => x.Type == PlantType.GasFired);

        var gasPlants = gasPlantsDtos.
            Select(_ => new PowerPlant
            {
                Name = _.Name,
                Type = PlantType.GasFired,
                Pmax = _.Pmax,
                Pmin = _.Pmin,
                PricePerMWh = CalculateCostOfOperationPerMWh(_, productionRequest.Fuels)
            })
            .ToList();

        return gasPlants;
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

    private static double CalculateCostOfOperationPerMWh(PowerPlantDto powerPlant, Fuels fuels)
    {
        var pricePerMWh = fuels.GasEuroMWh + Co2GeneratedPerMWh * fuels.Co2EuroTon;

        return pricePerMWh / powerPlant.Efficiency;
    }
}
