namespace ProductionPlan.Models;

public class PowerPlant
{
    public string Name { get; set; }
    public PlantType Type { get; set; }
    public double PricePerMWh { get; set; }
    public decimal Pmin { get; set; }
    public decimal Pmax { get; set; }
}
