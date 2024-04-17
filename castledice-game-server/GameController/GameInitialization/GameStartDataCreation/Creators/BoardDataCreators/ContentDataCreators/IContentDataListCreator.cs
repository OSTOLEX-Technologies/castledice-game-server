using castledice_game_data_logic.Content;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;

public interface IContentDataListCreator
{
    List<ContentData> GetContentDataList(Board board);
}