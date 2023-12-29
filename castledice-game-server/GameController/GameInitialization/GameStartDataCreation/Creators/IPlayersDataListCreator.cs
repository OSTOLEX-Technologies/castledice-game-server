using castledice_game_data_logic;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;

public interface IPlayersDataListCreator
{
    List<PlayerData> GetPlayersData(List<Player> players);
}