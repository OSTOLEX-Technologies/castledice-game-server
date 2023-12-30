using castledice_game_data_logic;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlayerDataCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests.CreatorsTests.PlayerDataCreatorsTests;

public class PlayersDataListCreatorTests
{
    [Fact]
    public void GetPlayersDataList_ShouldCreatePlayersData_WithGivenCreator()
    {
        var players = GetRandomPlayersList();
        var expectedPlayersDataList = new List<PlayerData>();
        var playerDataCreatorMock = new Mock<IPlayerDataCreator>();
        foreach (var player in players)
        {
            var playerData = GetPlayerData();
            expectedPlayersDataList.Add(playerData);
            playerDataCreatorMock.Setup(x => x.GetPlayerData(player)).Returns(playerData);
        }
        var creator = new PlayersDataListCreator(playerDataCreatorMock.Object);
        
        var actualPlayersDataList = creator.GetPlayersData(players);
        
        Assert.Equal(expectedPlayersDataList, actualPlayersDataList);
    }
}