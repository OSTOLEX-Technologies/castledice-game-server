using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.TreesSpawning;

public class DefaultTreeConfigCreator : ITreeConfigCreator
{
    private readonly int _removeCost;
    private readonly bool _canBeRemoved;

    public DefaultTreeConfigCreator(int removeCost, bool canBeRemoved)
    {
        _removeCost = removeCost;
        _canBeRemoved = canBeRemoved;
    }

    public TreeConfig GetTreeConfig()
    {
        return new TreeConfig(_removeCost, _canBeRemoved);
    }
}