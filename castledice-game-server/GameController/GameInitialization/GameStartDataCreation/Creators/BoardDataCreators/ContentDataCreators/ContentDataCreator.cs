using castledice_game_data_logic.Content;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;

public class ContentDataCreator : IContentDataCreator, IContentVisitor<ContentData>
{
    private Vector2Int _currentContentPosition;
    
    public ContentData GetContentData(Content content, Vector2Int position)
    {
        _currentContentPosition = position;
        return content.Accept(this);
    }

    public ContentData VisitTree(Tree tree)
    {
        return new TreeData(_currentContentPosition, tree.GetRemoveCost(), tree.CanBeRemoved());
    }

    public ContentData VisitCastle(Castle castle)
    {
        return new CastleData(_currentContentPosition, 
            castle.GetCaptureHitCost(), 
            castle.GetMaxFreeDurability(), 
            castle.GetMaxDurability(), 
            castle.GetDurability(), 
            castle.GetOwner().Id);
    }

    public ContentData VisitKnight(Knight knight)
    {
        return new KnightData(_currentContentPosition, knight.GetReplaceCost(), knight.GetPlacementCost(),
            knight.GetOwner().Id);
    }
}