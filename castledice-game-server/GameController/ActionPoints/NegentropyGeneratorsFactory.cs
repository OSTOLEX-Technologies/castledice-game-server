using castledice_game_logic.GameConfiguration;
using castledice_game_logic.Math;

namespace castledice_game_server.GameController.ActionPoints;

public class NegentropyGeneratorsFactory : INumberGeneratorsFactory
{
    private readonly RandomConfig _config;

    public NegentropyGeneratorsFactory(RandomConfig config)
    {
        _config = config;
    }

    public IRandomNumberGenerator GetGenerator()
    {
        return new NegentropyRandomNumberGenerator(_config.MinInclusive, _config.MaxExclusive, _config.Precision);
    }
}