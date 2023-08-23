using Microsoft.Extensions.Logging;
using ProductionPlan.LoadServices.Interfaces;
using ProductionPlan.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlan.LoadServices
{
    public class WindTurbineService : IWindTurbineService
    {
        private readonly ILogger<GasPlantService> _logger;

        public WindTurbineService(ILogger<GasPlantService> logger)
        {
            _logger = logger;
        }

        public List<PowerPlant> GetPowerPlantsNormalized(ProductionRequestDto productionRequest)
        {
            var windTurbines = productionRequest.Powerplants
                .Where(x => x.Type == PlantType.WindTurbine)
                .ToList();

            var windTurbinesNormalized = windTurbines.Select(_ =>
                new PowerPlant
                {
                    Name = _.Name,
                    Type = PlantType.WindTurbine,
                    Pmax = decimal.Round(_.Pmax * productionRequest.Fuels.Wind / 100, 1),
                    Pmin = _.Pmin,
                    PricePerMWh = 0
                })
                .ToList();

            return windTurbinesNormalized;
        }

        public decimal GetLoad(PowerPlant powerPlant,
            decimal requiredLoad, ICollection<ProductionResponseDto> response)
        {
            var productionResponseDto = new ProductionResponseDto
            {
                Name = powerPlant.Name,
                P = powerPlant.Pmax,
            };

            requiredLoad -= powerPlant.Pmax;
            if (requiredLoad < decimal.Zero)
            {
                productionResponseDto.P += requiredLoad;
                requiredLoad = 0;
            }

            response.Add(productionResponseDto);

            return requiredLoad;
        }
    }
}