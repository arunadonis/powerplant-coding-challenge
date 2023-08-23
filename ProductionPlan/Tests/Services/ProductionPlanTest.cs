using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductionPlan.Models;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Services;

public class ProductionPlanTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProductionPlanTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [JsonFileData("JsonDataPayload/payload1.json")]
    public async Task ProductionPlanServiceTest_Payload1_WorkAsExpectedAsync(ProductionRequestDto productionRequestDto)
    {
        // Arrange
        var client = _factory.CreateClient();

        var expected = new List<ProductionResponseDto>
        {
            new ProductionResponseDto
            {
                Name = "windpark1",
                P = 90
            },
            new ProductionResponseDto
            {
                Name = "windpark2",
                P = 21.6M
            },
            new ProductionResponseDto
            {
                Name = "gasfiredbig1",
                P = 368.4M
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/productionplan", productionRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var productionResponseDto = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ProductionResponseDto>>();

        productionResponseDto.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [JsonFileData("JsonDataPayload/payload2.json")]
    public async Task ProductionPlanServiceTest_Payload2_WorkAsExpectedAsync(ProductionRequestDto productionRequestDto)
    {
        // Arrange
        var client = _factory.CreateClient();

        var expected = new List<ProductionResponseDto>
        {
            new ProductionResponseDto
            {
                Name = "windpark1",
                P = 0
            },
            new ProductionResponseDto
            {
                Name = "windpark2",
                P = 0
            },
            new ProductionResponseDto
            {
                Name = "gasfiredbig1",
                P = 380M
            },
            new ProductionResponseDto
            {
                Name = "gasfiredbig2",
                P = 100M
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/productionplan", productionRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var productionResponseDto = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ProductionResponseDto>>();

        productionResponseDto.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [JsonFileData("JsonDataPayload/payload3.json")]
    public async Task ProductionPlanServiceTest_Payload3_WorkAsExpectedAsync(ProductionRequestDto productionRequestDto)
    {
        // Arrange
        var client = _factory.CreateClient();

        var expected = new List<ProductionResponseDto>
        {
            new ProductionResponseDto
            {
                Name = "windpark1",
                P = 90M
            },
            new ProductionResponseDto
            {
                Name = "windpark2",
                P = 21.6M
            },
            new ProductionResponseDto
            {
                Name = "gasfiredbig1",
                P = 460M
            },
            new ProductionResponseDto
            {
                Name = "gasfiredbig2",
                P = 338.4M
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/productionplan", productionRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var productionResponseDto = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ProductionResponseDto>>();

        productionResponseDto.Should().BeEquivalentTo(expected);
    }
}