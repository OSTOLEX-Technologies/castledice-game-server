using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

public interface IPlayersListCreator
{
    List<Player> GetPlayersList(List<int> playersIds);
}