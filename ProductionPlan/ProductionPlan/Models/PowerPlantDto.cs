namespace ProductionPlan.Models;

public class PowerPlantDto
{
    public string Name { get; set; }
    public PlantType Type { get; set; }
    public double Efficiency { get; set; }
    public decimal Pmin { get; set; }
    public decimal Pmax { get; set; }
}
