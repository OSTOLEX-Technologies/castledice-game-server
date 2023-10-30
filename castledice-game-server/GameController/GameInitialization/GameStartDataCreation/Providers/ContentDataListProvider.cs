using castledice_game_data_logic.Content;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public class ContentDataListProvider : IContentDataListProvider
{
    private readonly IContentDataProvider _contentDataProvider;

    public ContentDataListProvider(IContentDataProvider contentDataProvider)
    {
        _contentDataProvider = contentDataProvider;
    }

    public List<ContentData> GetContentDataList(Board board)
    {
        var contentData = new List<ContentData>();
        foreach (var cell in board)
        {
            foreach (var content in cell.GetContent())
            {
                contentData.Add(_contentDataProvider.GetContentData(content));
            }
        }
        return contentData;
    }
}