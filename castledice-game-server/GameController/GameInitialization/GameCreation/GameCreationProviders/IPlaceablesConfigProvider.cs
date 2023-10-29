using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders;

public interface IPlaceablesConfigProvider
{
    PlaceablesConfig GetPlaceablesConfig();
}