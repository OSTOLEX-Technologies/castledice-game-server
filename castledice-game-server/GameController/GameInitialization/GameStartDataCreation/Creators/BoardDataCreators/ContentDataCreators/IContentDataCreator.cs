using castledice_game_data_logic.Content;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;

public interface IContentDataCreator
{
    ContentData GetContentData(Content content, Vector2Int position);
}