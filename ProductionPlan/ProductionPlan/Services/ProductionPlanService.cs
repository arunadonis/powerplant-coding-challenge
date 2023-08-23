using Microsoft.Extensions.Logging;
using ProductionPlan.Helpers;
using ProductionPlan.LoadServices.Interfaces;
using ProductionPlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlan.Services;

public class ProductionPlanService : IProductionPlanService
{

    private readonly ILogger<ProductionPlanService> _logger;
    private readonly IWindTurbineService _windTurbineService;
    private readonly IGasPlantService _gasPlantService;
    private readonly ITurbojetPlantService _turbojetPlantService;
    private readonly IValidationHelper _validationHelper;

    public ProductionPlanService(ILogger<ProductionPlanService> logger,
        IValidationHelper validationHelper,
        IGasPlantService gasPlantLoadService,
        IWindTurbineService windTurbineLoadService,
        ITurbojetPlantService turbojetPlantLoadService
        )
    {
        _logger = logger;
        _windTurbineService = windTurbineLoadService;
        _gasPlantService = gasPlantLoadService;
        _turbojetPlantService = turbojetPlantLoadService;
        _validationHelper = validationHelper;
    }

    public IReadOnlyCollection<ProductionResponseDto> GetProductionPlan(ProductionRequestDto productionRequest)
    {
        _validationHelper.ValidateProductionRequest(productionRequest);

        var windTurbines = _windTurbineService.GetPowerPlantsNormalized(productionRequest);
        var gasPlants = _gasPlantService.GetPowerPlantsNormalized(productionRequest);
        var turboJetPlants = _turbojetPlantService.GetPowerPlantsNormalized(productionRequest);

        var meritOrder = GetMeritOrder(windTurbines, gasPlants, turboJetPlants);

        _validationHelper.ValidateProductionLimits(meritOrder, productionRequest.Load);

        var response = GetLoad(meritOrder, productionRequest.Load);

        return response;
    }

    private IOrderedEnumerable<PowerPlant> GetMeritOrder(List<PowerPlant> windTurbines, List<PowerPlant> gasPlants, List<PowerPlant> turboJetPlants)
    {
        return windTurbines.Concat(gasPlants)
            .Concat(turboJetPlants)
            .OrderBy(_ => _.PricePerMWh);
    }

    private List<ProductionResponseDto> GetLoad(IOrderedEnumerable<PowerPlant> powerPlants, decimal requiredLoad)
    {
        var response = new List<ProductionResponseDto>();

        foreach (PowerPlant powerPlant in powerPlants)
        {
            requiredLoad = powerPlant.Type switch
            {
                PlantType.WindTurbine => _windTurbineService.GetLoad(powerPlant, requiredLoad, response),
                PlantType.GasFired => _gasPlantService.GetLoad(powerPlant, requiredLoad, response),
                PlantType.TurboJet => _turbojetPlantService.GetLoad(powerPlant, requiredLoad, response),
                _ => HandleDefaultPowerPlant(powerPlant)

            };

            if (requiredLoad == decimal.Zero)
            {
                break;
            }
        }

        return response;
    }

    private static decimal HandleDefaultPowerPlant(PowerPlant powerPlant)
    {
        var errorMessage = string.Format(ErrorMessages.UnknownPowerPlantType, powerPlant.Type);
        throw new ArgumentOutOfRangeException(nameof(powerPlant), errorMessage);
    }
}