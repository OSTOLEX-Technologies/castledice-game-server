using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;

public class PlaceablesConfigProvider : IPlaceablesConfigProvider
{
    private readonly IKnightConfigProvider _knightConfigProvider;

    public PlaceablesConfigProvider(IKnightConfigProvider knightConfigProvider)
    {
        _knightConfigProvider = knightConfigProvider;
    }

    public PlaceablesConfig GetPlaceablesConfig()
    {
        var knightConfig = _knightConfigProvider.GetKnightConfig();
        return new PlaceablesConfig(knightConfig);
    }
}