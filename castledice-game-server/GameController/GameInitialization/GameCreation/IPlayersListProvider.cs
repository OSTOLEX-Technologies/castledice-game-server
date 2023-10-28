using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public interface IPlayersListProvider
{
    List<Player> GetPlayersList(List<int> playersIds);
}