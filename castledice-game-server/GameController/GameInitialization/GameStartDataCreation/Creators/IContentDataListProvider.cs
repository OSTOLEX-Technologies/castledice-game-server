using castledice_game_data_logic.Content;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public interface IContentDataListProvider
{
    List<ContentData> GetContentDataList(Board board);
}