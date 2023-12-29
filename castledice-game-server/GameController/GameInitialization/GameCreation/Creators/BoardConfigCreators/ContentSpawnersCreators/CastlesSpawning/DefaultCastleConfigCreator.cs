using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

public class DefaultCastleConfigCreator : ICastleConfigCreator
{
    private readonly int _maxDurability;
    private readonly int _maxFreeDurability;
    private readonly int _captureHitCost;

    public DefaultCastleConfigCreator(int maxDurability, int maxFreeDurability, int captureHitCost)
    {
        _maxDurability = maxDurability;
        _maxFreeDurability = maxFreeDurability;
        _captureHitCost = captureHitCost;
    }

    public CastleConfig GetCastleConfig()
    {
        return new CastleConfig(_maxDurability, _maxFreeDurability, _captureHitCost);
    }
}