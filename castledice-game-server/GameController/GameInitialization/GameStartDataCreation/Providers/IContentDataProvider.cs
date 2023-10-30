using castledice_game_data_logic.Content;
using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public interface IContentDataProvider
{
    ContentData GetContentData(Content content);
}