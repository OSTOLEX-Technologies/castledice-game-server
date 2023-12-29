using castledice_game_logic.Time;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersListsCreatorsTests;

public class PlayersListCreatorTests
{
    [Fact]
    public void GetPlayersList_ShouldReturnListOfPlayers_WithAppropriateIds()
    {
        var idsList = GetIdsListWithRandomLength();
        var creator = new PlayersListCreator(new Mock<IPlayerTimerCreator>().Object);
        
        var players = creator.GetPlayersList(idsList);
        
        Assert.Equal(idsList.Count, players.Count);
        foreach (var id in idsList)
        {
            Assert.Contains(players, p => p.Id == id);
        }
    }
    
    [Fact]
    public void GetPlayersList_ShouldReturnPlayers_WithTimerFromCreator()
    {
        var expectedTimer = new Mock<IPlayerTimer>();
        var timerCreatorMock = new Mock<IPlayerTimerCreator>();
        timerCreatorMock.Setup(x => x.GetPlayerTimer()).Returns(expectedTimer.Object);
        var creator = new PlayersListCreator(timerCreatorMock.Object);
        
        var players = creator.GetPlayersList(GetIdsListWithRandomLength());
        
        foreach (var player in players)
        {
            Assert.Same(expectedTimer.Object, player.Timer);
        }
    }
    
    private static List<int> GetIdsListWithRandomLength()
    {
        var random = new Random();
        var count = random.Next(1, 10);
        return GetIdsList(count);
    }
    
    private static List<int> GetIdsList(int count)
    {
        var ids = new List<int>();
        for (var i = 0; i < count; i++)
        {
            ids.Add(i);
        }

        return ids;
    }
}