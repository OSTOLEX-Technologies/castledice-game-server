using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;

public interface IPlaceablesConfigProvider
{
    PlaceablesConfig GetPlaceablesConfig();
}