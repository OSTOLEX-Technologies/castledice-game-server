using castledice_game_data_logic.Content;
using castledice_game_logic.GameObjects;
using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators.ContentDataCreators;
using CastleGO = castledice_game_logic.GameObjects.Castle;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests.CreatorsTests.BoardDataCreatorsTests.ContentDataCreatorsTests;

public class ContentDataCreatorTests
{
    [Theory]
    [MemberData(nameof(GetContentDataTestCases))]
    public void GetContentData_ShouldReturnAppropriateContentData(Content content, Vector2Int position,
        ContentData expectedData)
    {
        var creator = new ContentDataCreator();
        
        var data = creator.GetContentData(content, position);
        
        Assert.Equal(expectedData, data);
    }

    public static IEnumerable<object[]> GetContentDataTestCases()
    {
        yield return new object[]
        {
            new CastleGO(GetPlayer(1), 3, 3, 1, 1),
            new Vector2Int(3, 3),
            new CastleData((3, 3), 1, 1, 3, 3, 1)
        };
        yield return new object[]
        {
            new Tree(1, true),
            new Vector2Int(3, 3),
            new TreeData((3, 3), 1, true)
        };
        yield return new object[]
        {
            new Knight(GetPlayer(3), 1, 2),
            new Vector2Int(2, 4),
            new KnightData((2, 4), 2, 1, 3)
        };
    }
}