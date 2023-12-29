using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersListsCreatorsTests;

public class PlayersListCreatorTests
{
    [Fact]
    public void GetPlayersList_ShouldReturnPlayers_CreatedWithGivenCreator()
    {
        var playersIds = GetIdsListWithRandomLength();
        var expectedPlayers = GetPlayersList(playersIds);
        var playerCreatorMock = new Mock<IPlayerCreator>();
        foreach (var player in expectedPlayers)
        {
            playerCreatorMock.Setup(x => x.GetPlayer(player.Id)).Returns(player);
        }
        var playersListCreator = new PlayersListCreator(playerCreatorMock.Object);
        
        var actualPlayers = playersListCreator.GetPlayersList(playersIds);
        
        Assert.Equal(expectedPlayers, actualPlayers);
    }
    
    private static List<Player> GetPlayersList(IEnumerable<int> ids)
    {
        return ids.Select(GetPlayer).ToList();
    } 
}