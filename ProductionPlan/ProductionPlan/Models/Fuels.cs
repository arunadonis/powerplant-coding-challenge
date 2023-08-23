using System.Text.Json.Serialization;

namespace ProductionPlan.Models;

public class Fuels
{
    [JsonPropertyName("gas(euro/MWh)")]
    public double GasEuroMWh { get; set; }

    [JsonPropertyName("kerosine(euro/MWh)")]
    public double KerosineEuroMWh { get; set; }

    [JsonPropertyName("co2(euro/ton)")]
    public double Co2EuroTon { get; set; }

    [JsonPropertyName("wind(%)")]
    public int Wind { get; set; }
}
