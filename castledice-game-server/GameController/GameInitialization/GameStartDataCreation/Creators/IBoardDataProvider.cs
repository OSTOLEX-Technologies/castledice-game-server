using castledice_game_data_logic.ConfigsData;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public interface IBoardDataProvider
{
    BoardData GetBoardData(Board board);
}