using castledice_game_data_logic.ConfigsData;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;

public class PlaceablesConfigDataCreator : IPlaceablesConfigDataCreator
{
    public PlaceablesConfigData GetPlaceablesConfigData(PlaceablesConfig config)
    {
        return new PlaceablesConfigData(GetKnightConfigData(config.KnightConfig));
    }

    private KnightConfigData GetKnightConfigData(KnightConfig config)
    {
        return new KnightConfigData(config.PlacementCost, config.Health);
    }
}