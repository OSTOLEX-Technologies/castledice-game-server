using castledice_game_logic;
using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.Math;
using static castledice_game_server_tests.ObjectCreationUtility;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.BoardConfigCreatorsTests.ContentSpawnersCreatorsTests;

public class DuelCastlesSpawnerCreatorTests
{
    [Theory]
    [MemberData(nameof(InvalidPlayersListTestCases))]
    public void GetCastlesSpawner_ShouldThrowArgumentException_IfGivenListHasMoreOrLessThanTwoPlayers(
        List<Player> invalidList)
    {
        var spawnerCreator = new DuelCastlesSpawnerCreator(GetDuelCastlesPositionsCreatorMock().Object,
            GetCastlesFactoryCreatorMock().Object);
        
        Assert.Throws<ArgumentException>(() => spawnerCreator.GetCastlesSpawner(invalidList));
    }

    public static IEnumerable<object[]> InvalidPlayersListTestCases()
    {
        yield return new[]
        {
            new List<Player> { GetPlayer(1), GetPlayer(2), GetPlayer(3) }
        };
        yield return new[]
        {
            new List<Player> { GetPlayer(1) }
        };
    }

    [Fact]
    public void ReturnedSpawner_ShouldHaveOnlyTwoEntries_InCastlesPlacementData()
    {
        var spawnerCreator = new DuelCastlesSpawnerCreator(GetDuelCastlesPositionsCreatorMock().Object,
            GetCastlesFactoryCreatorMock().Object);
        
        var spawner = spawnerCreator.GetCastlesSpawner(new List<Player> { GetPlayer(1), GetPlayer(2) });
        var playersToCastlePositions = spawner.CastlesPlacementsData;
        
        Assert.Equal(2, playersToCastlePositions.Count);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    public void ReturnedSpawner_ShouldHaveFirstPlayerCastlePosition_AssignedToFirstCastlePositionFromCreator(int x, int y)
    {
        var player = GetPlayer(1);
        var expectedPosition = new Vector2Int(x, y);
        var providerMock = GetDuelCastlesPositionsCreatorMock();
        providerMock.Setup(provider => provider.GetFirstCastlePosition()).Returns(expectedPosition);
        var playerList = new List<Player> { player, GetPlayer(2) };
        var spawnerCreator = new DuelCastlesSpawnerCreator(providerMock.Object,
            GetCastlesFactoryCreatorMock().Object);
        
        var spawner = spawnerCreator.GetCastlesSpawner(playerList);
        var playersToCastlePositions = spawner.CastlesPlacementsData;
        var actualPosition = playersToCastlePositions[player];
        
        Assert.Equal(expectedPosition, actualPosition);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    public void ReturnedSpawner_ShouldHaveSecondPlayerCastlePosition_AssignedToSecondCastlePositionFromCreator(int x, int y)
    {
        var player = GetPlayer(2);
        var expectedPosition = new Vector2Int(x, y);
        var providerMock = GetDuelCastlesPositionsCreatorMock();
        providerMock.Setup(provider => provider.GetSecondCastlePosition()).Returns(expectedPosition);
        var playerList = new List<Player> { GetPlayer(1), player };
        var spawnerCreator = new DuelCastlesSpawnerCreator(providerMock.Object,
            GetCastlesFactoryCreatorMock().Object);
        
        var spawner = spawnerCreator.GetCastlesSpawner(playerList);
        var playersToCastlePositions = spawner.CastlesPlacementsData;
        var actualPosition = playersToCastlePositions[player];
        
        Assert.Equal(expectedPosition, actualPosition);
    }
    
    private static Mock<IDuelCastlesPositionsCreator> GetDuelCastlesPositionsCreatorMock()
    {
        var mock = new Mock<IDuelCastlesPositionsCreator>();
        mock.Setup(provider => provider.GetFirstCastlePosition()).Returns((0, 0));
        mock.Setup(provider => provider.GetSecondCastlePosition()).Returns((9, 9));
        return mock;
    } 
    
    private static Mock<ICastlesFactoryCreator> GetCastlesFactoryCreatorMock()
    {
        var mock = new Mock<ICastlesFactoryCreator>();
        mock.Setup(provider => provider.GetCastlesFactory()).Returns(new Mock<ICastlesFactory>().Object);
        return mock;
    }
}