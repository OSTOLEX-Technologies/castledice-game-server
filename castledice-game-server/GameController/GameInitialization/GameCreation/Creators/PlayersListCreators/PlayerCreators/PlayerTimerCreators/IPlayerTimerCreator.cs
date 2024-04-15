using castledice_game_logic.Time;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayerTimerCreators;

public interface IPlayerTimerCreator
{
    IPlayerTimer GetPlayerTimer();
}