using castledice_game_logic.Math;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;
using Microsoft.VisualBasic;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.BoardConfigProvidersTests.ContentSpawnersProivdersTests;


/// <summary>
/// Name of this class is shortened due to git error.
/// Original name: DefaultDuelCastlesPositionsProviderTests
/// </summary>
public class DefDuelCastlesPosProviderTests
{
    [Theory]
    [InlineData(1, 2)]
    [InlineData(0, 0)]
    [InlineData(9, 9)]
    public void GetFirstCastlePosition_ShouldReturnVector2Int_GivenInConstructor(int x, int y)
    {
        var expectedPosition = new Vector2Int(x, y);
        var positionsProvider = new DefaultDuelCastlePositionsProvider(expectedPosition, (0, 0));

        var actualPosition = positionsProvider.GetFirstCastlePosition();
        
        Assert.Equal(expectedPosition, actualPosition);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(0, 0)]
    [InlineData(9, 9)]
    public void GetSecondCastlePosition_ShouldReturnVector2Int_GivenInConstructor(int x, int y)
    {
        var expectedPosition = new Vector2Int(x, y);
        var positionsProvider = new DefaultDuelCastlePositionsProvider((0, 0), expectedPosition);

        var actualPosition = positionsProvider.GetSecondCastlePosition();
        
        Assert.Equal(expectedPosition, actualPosition);
    }
}