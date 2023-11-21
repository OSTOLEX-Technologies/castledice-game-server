using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

public interface IPlayersListProvider
{
    List<Player> GetPlayersList(List<int> playersIds);
}