using System.Collections.Generic;

namespace ProductionPlan.Models;

public class ProductionRequestDto
{
    public decimal Load { get; set; }
    public Fuels Fuels { get; set; }
    public List<PowerPlantDto> Powerplants { get; set; }
}