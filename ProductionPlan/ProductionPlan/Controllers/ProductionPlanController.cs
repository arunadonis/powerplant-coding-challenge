using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductionPlan.Models;
using ProductionPlan.Services;

namespace ProductionPlan.Controllers;

[ApiController]
public class ProductionPlanController : ControllerBase
{
    private readonly IProductionPlanService _productionPlan;

    public ProductionPlanController(IProductionPlanService productionPlan)
    {
        _productionPlan = productionPlan;
    }

    [HttpPost("productionplan")]
    public IReadOnlyCollection<ProductionResponseDto> GetProductionPlan([FromBody] ProductionRequestDto requestDto)
    {
        return _productionPlan.GetProductionPlan(requestDto);
    }
}