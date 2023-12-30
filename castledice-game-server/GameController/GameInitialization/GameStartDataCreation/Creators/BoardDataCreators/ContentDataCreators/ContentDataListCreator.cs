using castledice_game_data_logic.Content;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;

public class ContentDataListCreator : IContentDataListCreator
{
    private readonly IContentDataCreator _contentDataCreator;

    public ContentDataListCreator(IContentDataCreator contentDataCreator)
    {
        _contentDataCreator = contentDataCreator;
    }

    public List<ContentData> GetContentDataList(Board board)
    {
        var contentData = new List<ContentData>();
        foreach (var cell in board)
        {
            foreach (var content in cell.GetContent())
            {
                contentData.Add(_contentDataCreator.GetContentData(content, cell.Position));
            }
        }
        return contentData;
    }
}