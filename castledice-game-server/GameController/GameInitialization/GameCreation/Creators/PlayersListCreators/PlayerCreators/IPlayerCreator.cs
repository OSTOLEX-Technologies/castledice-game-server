using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators;

public interface IPlayerCreator
{
    Player GetPlayer(int playerId);
}