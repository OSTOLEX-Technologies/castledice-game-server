using castledice_game_logic.Time;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

public interface IPlayerTimerProvider
{
    IPlayerTimer GetPlayerTimer();
}