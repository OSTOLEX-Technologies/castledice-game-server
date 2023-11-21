using castledice_game_data_logic.MoveConverters;
using castledice_game_logic;

namespace castledice_game_server.GameController.Moves;

public interface IDataToMoveConverterProvider
{
    IDataToMoveConverter GetDataToMoveConverter(Game game);
}