using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

public interface IPlayerCreator
{
    Player GetPlayer(int playerId);
}