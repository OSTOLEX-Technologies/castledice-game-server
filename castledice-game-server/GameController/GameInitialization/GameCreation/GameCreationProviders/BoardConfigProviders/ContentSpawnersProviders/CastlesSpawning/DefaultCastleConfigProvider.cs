using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public class DefaultCastleConfigProvider : ICastleConfigProvider
{
    private readonly int _maxDurability;
    private readonly int _maxFreeDurability;
    private readonly int _captureHitCost;

    public DefaultCastleConfigProvider(int maxDurability, int maxFreeDurability, int captureHitCost)
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