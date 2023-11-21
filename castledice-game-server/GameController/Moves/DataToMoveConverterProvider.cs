using castledice_game_data_logic.MoveConverters;
using castledice_game_logic;

namespace castledice_game_server.GameController.Moves;

public class DataToMoveConverterProvider : IDataToMoveConverterProvider
{
    public IDataToMoveConverter GetDataToMoveConverter(Game game)
    {
        return new DataToMoveConverter(game.PlaceablesFactory);
    }
}