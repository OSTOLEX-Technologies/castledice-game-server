using castledice_game_logic.Math;

namespace castledice_game_server.GameController.ActionPoints;

public class GeneratorsCollection : INumberGeneratorsCollection
{
    private readonly Dictionary<int, IRandomNumberGenerator> _generators = new();
    private readonly INumberGeneratorsFactory _factory;

    public GeneratorsCollection(INumberGeneratorsFactory factory)
    {
        _factory = factory;
    }

    public void AddGeneratorForPlayer(int playerId)
    {
        if (_generators.ContainsKey(playerId))
        {
            throw new ArgumentException($"Generator for player with id {playerId} already exists");
        }
        _generators.Add(playerId, _factory.GetGenerator());
    }

    public IRandomNumberGenerator GetGeneratorForPlayer(int playerId)
    {
        if (!_generators.ContainsKey(playerId))
        {
            throw new ArgumentException($"Generator for player with id {playerId} does not exist");
        }
        return _generators[playerId];
    }

    public bool RemoveGeneratorForPlayer(int playerId)
    {
        return _generators.Remove(playerId);
    }
}