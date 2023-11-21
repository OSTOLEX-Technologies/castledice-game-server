using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;

public interface IKnightConfigProvider
{
    KnightConfig GetKnightConfig();
}