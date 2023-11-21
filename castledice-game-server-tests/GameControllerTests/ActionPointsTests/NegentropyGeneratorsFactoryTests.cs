using castledice_game_logic.GameConfiguration;
using castledice_game_logic.Math;
using castledice_game_server.GameController.ActionPoints;

namespace castledice_game_server_tests.GameControllerTests.ActionPointsTests;

public class NegentropyGeneratorsFactoryTests
{
    [Theory]
    [InlineData(0, 10, 1)]
    [InlineData(1, 6, 100)]
    [InlineData(2, 18, 1000)]
    public void GetGenerator_ShouldReturnNegentropyNumberGenerator_CreatedAccordingToConfig(int minInclusive,
        int maxExclusive, int precision)
    {
        var config = new RandomConfig(minInclusive, maxExclusive, precision);
        var factory = new NegentropyGeneratorsFactory(config);
        
        var generator = factory.GetGenerator();
        var negentropyGenerator = generator as NegentropyRandomNumberGenerator;

        Assert.NotNull(negentropyGenerator);
        Assert.Equal(minInclusive, negentropyGenerator.MinInclusive);
        Assert.Equal(maxExclusive, negentropyGenerator.MaxExclusive);
        Assert.Equal(precision, negentropyGenerator.Precision);
    }
}