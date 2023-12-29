using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;
using Microsoft.VisualBasic;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;


/// <summary>
/// Name of this class is shortened due to git error.
/// Original name: DefaultDuelCastlesPositionsCreatorTests
/// </summary>
public class DefDuelCastlesPosCreatorTests
{
    [Theory]
    [InlineData(1, 2)]
    [InlineData(0, 0)]
    [InlineData(9, 9)]
    public void GetFirstCastlePosition_ShouldReturnVector2Int_GivenInConstructor(int x, int y)
    {
        var expectedPosition = new Vector2Int(x, y);
        var positionsCreator = new DefaultDuelCastlePositionsCreator(expectedPosition, (0, 0));

        var actualPosition = positionsCreator.GetFirstCastlePosition();
        
        Assert.Equal(expectedPosition, actualPosition);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(0, 0)]
    [InlineData(9, 9)]
    public void GetSecondCastlePosition_ShouldReturnVector2Int_GivenInConstructor(int x, int y)
    {
        var expectedPosition = new Vector2Int(x, y);
        var positionsCreator = new DefaultDuelCastlePositionsCreator((0, 0), expectedPosition);

        var actualPosition = positionsCreator.GetSecondCastlePosition();
        
        Assert.Equal(expectedPosition, actualPosition);
    }
}