using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.TreesSpawning;

public class DefaultTreeConfigProvider : ITreeConfigProvider
{
    private readonly int _removeCost;
    private readonly bool _canBeRemoved;

    public DefaultTreeConfigProvider(int removeCost, bool canBeRemoved)
    {
        _removeCost = removeCost;
        _canBeRemoved = canBeRemoved;
    }

    public TreeConfig GetTreeConfig()
    {
        return new TreeConfig(_removeCost, _canBeRemoved);
    }
}