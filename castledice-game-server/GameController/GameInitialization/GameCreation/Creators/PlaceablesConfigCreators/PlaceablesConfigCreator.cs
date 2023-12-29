using castledice_game_logic.GameConfiguration;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;

public class PlaceablesConfigCreator : IPlaceablesConfigCreator
{
    private readonly IKnightConfigCreator _knightConfigCreator;

    public PlaceablesConfigCreator(IKnightConfigCreator knightConfigCreator)
    {
        _knightConfigCreator = knightConfigCreator;
    }

    public PlaceablesConfig GetPlaceablesConfig()
    {
        var knightConfig = _knightConfigCreator.GetKnightConfig();
        return new PlaceablesConfig(knightConfig);
    }
}