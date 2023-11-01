using castledice_game_logic.Math;

namespace castledice_game_server.GameController.ActionPoints;

public class GeneratorsCollection : INumberGeneratorsCollection
{
    private Dictionary<int, IRandomNumberGenerator> _generators = new();
    private readonly INumberGeneratorsFactory _factory;

    public GeneratorsCollection(INumberGeneratorsFactory factory)
    {
        _factory = factory;
    }

    public void AddGeneratorForPlayer(int playerId)
    {
        throw new NotImplementedException();
    }

    public IRandomNumberGenerator GetGeneratorForPlayer(int playerId)
    {
        throw new NotImplementedException();
    }

    public bool RemoveGeneratorForPlayer(int playerId)
    {
        throw new NotImplementedException();
    }
}